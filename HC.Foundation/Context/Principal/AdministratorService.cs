//--------------------------------------------------------------------------------
// 文件描述：系统管理员服务类
// 文件作者：张清山
// 创建日期：2014-08-11 17:47:10
// 修改记录： 
//--------------------------------------------------------------------------------

using System.Collections.Generic;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Foundation.Context.Principal
{
    /// <summary>
    ///     系统管理员服务类
    /// </summary>
    public class AdministratorService : ServiceBase<Administrator>
    {
        private static AdministratorService _instance;
        private static readonly object SynObject = new object();

        private AdministratorService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static AdministratorService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new AdministratorService());
                }
            }
        }
        /// <summary>
        /// 设置单个管理员菜单权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="menuPurview"></param>
        /// <returns></returns>
        public bool SetAdminMenuPurview(int adminId, string menuPurview)
        {
            const string sql = "UPDATE HC_Administrator SET MenuPurview=@0 WHERE Id=@1";
            return DbHelper.CurrentDb.Execute(sql, menuPurview, adminId) > 0;
        }
        /// <summary>
        /// 设置单个管理员主题
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        public bool SetAdminTheme(int adminId, string theme)
        {
            const string sql = "UPDATE HC_Administrator SET Theme=@0 WHERE Id=@1";
            return DbHelper.CurrentDb.Execute(sql, theme, adminId) > 0;
        }


        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Administrator GetAdminInfoByName(string name)
        {
            const string sql = "SELECT * FROM HC_Administrator WHERE LoginName=@0";
            var admin = DbHelper.CurrentDb.SingleOrDefault<Administrator>(sql, name);
            return admin;
        }

        /// <summary>
        ///     添加系统管理员
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(Administrator instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 添加系统管理员实体", instance.CreateUser),
                                        string.Format("{0} 添加系统管理员实体:{1}", instance.CreateUser, instance.ToXml()));
                return true;
            }
            return false;
        }

        /// <summary>
        ///     获取分页数据
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="conditions">搜索条件</param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public Page<Administrator> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions,
                                         string orderby)
        {
            string sql = " WHERE IsDel=0 ";
            if (conditions.Count > 0)
            {
                foreach (var condition in conditions)
                {
                    switch (condition.Key.ToLower())
                    {
                        case "ids":
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND Id IN({1}) ".FormatWith(condition.Key, condition.Value.Trim().TrimEnd(','));
                            }
                            break;
                        case "loginname":
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND (LoginName like '%{0}%' OR  Email like '%{0}%' OR  Phone like '%{0}%' OR  Theme like '%{0}%' )".FormatWith(condition.Value);
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
            return DbHelper.CurrentDb.Page<Administrator>(pageIndex, pageSize, sql);
        }


        /// <summary>
        ///     修改系统管理员
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(Administrator instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 修改系统管理员实体", instance.CreateUser),
                                        string.Format("{0} 修改系统管理员实体:{1}", instance.CreateUser, instance.ToXml()));
                return true;
            }
            return false;
        }

        /// <summary>
        ///     删除系统管理员实体
        /// </summary>
        /// <param name="userName">操作者</param>
        /// <param name="id">根据ID删除系统管理员实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(string userName, int id)
        {
            const string sql = "DELETE FROM HC_Administrator WHERE Id=@0";
            LogService.Instance.Log(string.Format("{0} 删除系统管理员", userName),
                                    string.Format("{0} 删除系统管理员:{1}", userName, id.ToStr()));
            return DbHelper.CurrentDb.Execute(sql, id) > 0;
        }

        /// <summary>
        ///     批量删除系统管理员实体
        /// </summary>
        /// <param name="userName">操作者</param>
        /// <param name="ids">根据ID批量删除系统管理员实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(string userName, string ids)
        {
            const string sql = "DELETE FROM HC_Administrator WHERE Id IN (@0)";
            LogService.Instance.Log(string.Format("{0} 删除系统管理员", userName),
                                    string.Format("{0} 删除系统管理员:{1}", userName, ids));
            return DbHelper.CurrentDb.Execute(sql, ids) > 0;
        }
        /// <summary>
        /// 判断管理员是否具有相应操作权限，权限码不分大小写
        /// </summary>
        /// <param name="permissionCode"></param>
        /// <returns></returns>
        public bool HasPermissions(string permissionCode)
        {
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                const string sql = "SELECT Id FROM HC_Purview  WHERE isdel=0 AND UPPER(OperateCode)=@0";
                var id = DbHelper.CurrentDb.ExecuteScalar<object>(sql, permissionCode.ToUpper()).ToInt(0);
                if (id > 0)
                {
                    var operatePurview = HCContext.Current.Admin.AdministratorInfo.OperatePurview;
                    return ("," + operatePurview + ",").Contains("," + id + ",");
                }
            }
            return false;
        }
    }
}