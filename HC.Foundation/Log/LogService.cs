//--------------------------------------------------------------------------------
// 文件描述：日志服务类
// 文件作者：张清山
// 创建日期：2013-12-10 10:55:05
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Foundation.Log
{
    /// <summary>
    /// 系统日志服务类
    /// </summary>
    public class LogService : ServiceBase<Log>
    {
        private static LogService _instance;

        private LogService()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static LogService Instance
        {
            get { return _instance ?? (_instance = new LogService()); }
        }
        /// <summary>
        /// 记录普通日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param> 
        public void Log(string title, string content)
        {
            Log(title, content, LogCategory.None, LogPriority.Info);
        }
        /// <summary>
        /// 记录普通日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="category"></param>
        public void Log(string title, string content, LogCategory category)
        {
            Log(title, content, category, LogPriority.Info);
        }
        /// <summary>
        /// 记录异常日志
        /// </summary> 
        public void LogException(Exception ex)
        {
            LogException("系统异常", ex.Message, LogCategory.System);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="category"></param>
        public void LogException(string title, string content, LogCategory category)
        {
            Log(title, content, category, LogPriority.Exception);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="category"></param>
        /// <param name="priority"> </param> 
        public void Log(string title, string content, LogCategory category, LogPriority priority)
        {
            Log logInfo = ModelFactory<Log>.Insten();
            logInfo.Title = title;
            logInfo.Message = content;
            logInfo.Priority = priority;
            logInfo.Category = category;
            logInfo.Source = HttpContext.Current.Request.Url.ToString();
            logInfo.UserIP = GetClientIp();
            if (HttpContext.Current != null)
            {
                if (HCContext.Current != null)
                {
                    logInfo.CreateUser = HCContext.Current.Admin != null
                        ? HCContext.Current.Admin.LoginName
                        : HCContext.Current.Context.User.Identity.Name;
                }
            }
            else
            {
                logInfo.CreateUser = "system";
            }

            Insert(logInfo);
        }
        /// <summary>
        ///     取得客户端IP
        /// </summary>
        /// <returns></returns>
        private static string GetClientIp()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                string strHostName = Dns.GetHostName();
                ip = Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            return ip;
        }
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="conditions"></param>
        /// <param name="orderBy"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetPageLogTable(int pageIndex, int pageSize, Dictionary<string, object> conditions,
                                    string orderBy, ref int count)
        {
            var sql = @"SELECT * FROM HC_Log " + Environment.NewLine;
            sql += " Where 1=1 " + Environment.NewLine;
            var parms = new List<object>();
            sql = GetFilterSql(ref parms, conditions, sql) + Environment.NewLine;
            if (!orderBy.IsEmpty())
            {
                sql += " ORDER BY " + orderBy + " ";
            }

            var ds = Query(pageIndex, pageSize, sql, parms.ToArray());
            count = ds.TotalItems.ToInt();
            return ds.Items.Tables[0];
        }
        private static string GetFilterSql(ref List<object> parms, Dictionary<string, object> conditions, string sql)
        {
            if (conditions.Count > 0)
            {
                int index = 0;
                foreach (var condition in conditions)
                {
                    switch (condition.Key)
                    {
                        case "Category":
                        case "Priority":
                            if (condition.Value.ToInt(0) > 0)
                            {
                                sql += string.Format(" AND {0}=@" + index, condition.Key);
                                parms.Add(condition.Value);
                                index++;
                            }
                            break;
                        default:
                            sql += string.Format(" AND {0} LIKE '%'+@" + index + "+'%' ", condition.Key);
                            parms.Add(condition.Value);
                            index++;
                            break;
                    }
                }
            }
            return sql;
        }

        /// <summary>
        ///     获取分页数据
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="conditions">搜索条件</param>
        /// <param name="orderby"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetPage(int pageIndex, int pageSize, Dictionary<string, object> conditions, string orderby, ref int count)
        {
            string sql = "SELECT * FROM HC_Log WHERE IsDel=0 ";
            if (conditions.Count > 0)
            {
                foreach (var condition in conditions)
                {
                    switch (condition.Key)
                    {
                        case "Priority":
                        case "Category":
                            if (condition.Value.ToInt(0) > 0)
                            {
                                sql += string.Format(" AND {0}={1} ", condition.Key, condition.Value);
                            }
                            break;
                        default:
                            if (!condition.Value.ToStr().IsEmpty())
                            {
                                sql += string.Format(" AND {0} like '%{1}%' ", condition.Key, condition.Value);
                            }
                            break;
                    }
                }
            }
            sql += orderby;

            PageDataSet ds = DbHelper.CurrentDb.Query(pageIndex, pageSize, sql);
            count = ds.TotalItems.ToInt();
            DataTable dt = ds.Items.Tables[0];
            dt.Columns.Add("CategoryStr");
            dt.Columns.Add("PriorityStr");

            foreach (DataRow row in dt.Rows)
            {
                row["CategoryStr"] = ((LogCategory)row["Category"].ToInt()).GetResource();
                row["PriorityStr"] = ((LogPriority)row["Priority"].ToInt()).GetResource();
            }

            return dt;
        }


        public Page<Log> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions,
                                      string orderby)
        {
            string sql = " WHERE IsDel=0 ";
            if (conditions.Count > 0)
            {
                foreach (var condition in conditions)
                {
                    switch (condition.Key.ToLower())
                    {
                        case "title":
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND (Title like '%{0}%' OR  Message like '%{0}%' OR  CreateUser like '%{0}%' OR  UserIP like '%{0}%' )".FormatWith(condition.Value);
                            }
                            break;
                        default:
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND {0} like '%{1}%' ".FormatWith(condition.Key, condition.Value);
                            }
                            break;
                    }
                }
            }
            sql += orderby;
            return DbHelper.CurrentDb.Page<Log>(pageIndex, pageSize, sql);
        }
    }
}