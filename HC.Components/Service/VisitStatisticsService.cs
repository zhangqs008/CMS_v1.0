//--------------------------------------------------------------------------------
// 文件描述：访问统计业务逻辑类
// 文件作者：张清山 
// 创建日期：2014-06-14 20:36:13
// 修改记录： 
//--------------------------------------------------------------------------------

using System.Data;
using HC.Components.Model;
using System.Collections.Generic;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.Service
{
    /// <summary>
    /// 访问统计管理业务类
    /// </summary>
    public class VisitStatisticsService : ServiceBase<VisitStatistics>
    {
        private static VisitStatisticsService _instance;
        private static readonly object SynObject = new object();

        private VisitStatisticsService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static VisitStatisticsService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new VisitStatisticsService());
                }
            }
        }


        /// <summary>
        ///     获取分页数据
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="conditions">搜索条件</param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public Page<VisitStatistics> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
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
                                sql += " AND (Url like '%{0}%' OR  IP like '%{0}%' OR  CreateUser like '%{0}%' OR  Broswer like '%{0}%'  OR  City like '%{0}%' )".FormatWith(condition.Value);
                            }
                            break;
                        default:
                            if (!condition.Value.IsEmpty())
                            {
                                sql += string.Format(" AND {0} like '%{1}%' ", condition.Key, condition.Value);
                            }
                            break;
                    }
                }
            }
            sql += orderby;
            return DbHelper.CurrentDb.Page<VisitStatistics>(pageIndex, pageSize, sql);
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
            string sql = "SELECT * FROM HC_VisitStatistics WHERE IsDel=0 ";
            var parms = new List<object>();
            sql = GetFilterSql(ref parms, conditions, sql);
            sql += orderby;
            PageDataSet ds = Query(pageIndex, pageSize, sql, parms.ToArray());
            count = ds.TotalItems.ToInt();
            DataTable dt = ds.Items.Tables[0];

            return dt;
        }
        private static string GetFilterSql(ref List<object> parms, Dictionary<string, object> conditions, string sql)
        {
            if (conditions.Count > 0)
            {
                int index = 0;
                foreach (var condition in conditions)
                {
                    if (condition.Value.ToStr().IsNotEmpty())
                    {
                        switch (condition.Key)
                        {
                            default:
                                sql += string.Format(" AND {0} LIKE '%'+@" + index + "+'%' ", condition.Key);
                                parms.Add(condition.Value);
                                break;
                        }
                        index++;
                    }
                }
            }
            return sql;
        }
    }
}

