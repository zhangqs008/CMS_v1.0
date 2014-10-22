//--------------------------------------------------------------------------------
// 文件描述：内容模型服务类
// 文件作者：张清山
// 创建日期：2014-08-30 12:03:22
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using HC.Components.Model;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.Service
{
    /// <summary>
    ///内容模型服务类
    /// </summary>
    public class ModuleService : ServiceBase<Module>
    {
        #region 单例实例

        private static ModuleService _instance;
        private static readonly object SynObject = new object();

        private ModuleService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static ModuleService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new ModuleService());
                }
            }
        }

        #endregion

        #region 添加

        /// <summary>
        /// 添加内容模型
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(Module instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 添加内容模型记录", instance.CreateUser),
                                        string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        #endregion

        #region 查询分页

        /// <summary>
        ///     获取分页数据
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="conditions">搜索条件</param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public Page<Module> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
        {
            string sql = " WHERE IsDel=0 ";
            if (conditions.Count > 0)
            {
                foreach (var condition in conditions)
                {
                    switch (condition.Key.ToLower())
                    {
                        case "name":
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND (Name like '%{0}%' OR  Description like '%{0}%' OR  TableName like '%{0}%'  )".FormatWith(condition.Value);
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
            return DbHelper.CurrentDb.Page<Module>(pageIndex, pageSize, sql);
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改内容模型
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(Module instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 修改内容模型记录", instance.CreateUser),
                                        string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        #endregion

        #region 删除

        /// <summary>
        ///     删除内容模型记录
        /// </summary>
        /// <param name="id">根据ID删除内容模型记录</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            try
            {
                const string sql = "DELETE FROM HC_Module WHERE Id=@0";
                var item = DbHelper.CurrentDb.SingleOrDefault<Module>(id);
                try
                {
                    var dropTableSql = "DROP Table " + item.TableName;
                    DbHelper.CurrentDb.Execute(dropTableSql);
                }
                catch (Exception ex)
                {
                    LogService.Instance.Log(string.Format("{0} 删除内容模型时，删除表异常", HCContext.Current.Admin.LoginName),
                                           ex.Message);

                }
                LogService.Instance.Log(string.Format("{0} 删除内容模型记录", HCContext.Current.Admin.LoginName),
                                        item.ToXml());
                DbHelper.CurrentDb.Execute(sql, id);
                return true;
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return false;
            }
        }

        /// <summary>
        ///     批量删除内容模型实体
        /// </summary>
        /// <param name="ids">根据ID批量删除内容模型实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(string ids)
        {
            try
            {
                string[] idArr = ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in idArr)
                {
                    Delete(id.ToInt());
                }
                return true;
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return false;
            }
        }

        #endregion
    }
}