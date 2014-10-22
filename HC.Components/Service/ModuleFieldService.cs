﻿//--------------------------------------------------------------------------------
// 文件描述：模型字段服务类
// 文件作者：张清山
// 创建日期：2014-08-30 15:15:15
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
    ///模型字段服务类
    /// </summary>
    public class ModuleFieldService : ServiceBase<ModuleField>
    {
        #region 单例实例

        private static ModuleFieldService _instance;
        private static readonly object SynObject = new object();

        private ModuleFieldService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static ModuleFieldService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new ModuleFieldService());
                }
            }
        }

        #endregion

        #region 添加

        /// <summary>
        /// 添加模型字段
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(ModuleField instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 添加模型字段记录", instance.CreateUser),
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
        public Page<ModuleField> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
        {
            string sql = " WHERE IsDel=0 ";
            if (conditions.Count > 0)
            {
                foreach (var condition in conditions)
                {
                    switch (condition.Key)
                    {
                        case "name":
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND (Name like '%{0}%' OR  Note like '%{0}%' )".FormatWith(condition.Value);
                            }
                            break;
                        case "moduleid":
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND {0}={1} ".FormatWith(condition.Key, condition.Value);
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
            return DbHelper.CurrentDb.Page<ModuleField>(pageIndex, pageSize, sql);
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改模型字段
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(ModuleField instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 修改模型字段记录", instance.CreateUser),
                                        string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        #endregion

        #region 删除

        /// <summary>
        ///     删除模型字段记录
        /// </summary>
        /// <param name="id">根据ID删除模型字段记录</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            try
            {
                const string sql = "DELETE FROM HC_ModuleField WHERE Id=@0";
                var item = DbHelper.CurrentDb.SingleOrDefault<ModuleField>(id);
                LogService.Instance.Log(string.Format("{0} 删除模型字段记录", HCContext.Current.Admin.LoginName),
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
        ///     批量删除模型字段实体
        /// </summary>
        /// <param name="ids">根据ID批量删除模型字段实体</param>
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
        /// <summary>
        ///  排序
        /// </summary> 
        /// <param name="sorts"></param>
        /// <returns></returns>
        public bool Sort(string sorts)
        {
            string[] ids = sorts.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            int count = 0;
            foreach (string id in ids)
            {
                string[] split = id.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2)
                {
                    const string sql = "UPDATE HC_ModuleField SET SORT=@0 WHERE Id=@1";
                    Execute(sql, split[1].ToInt(), split[0].ToInt());
                    count++;
                }
            }
            return ids.Length == count;
        }
    }
}