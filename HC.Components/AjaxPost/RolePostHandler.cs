//--------------------------------------------------------------------------------
// 文件描述： 角色Post请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxPost
{
    /// <summary>
    ///  角色Post请求响应类
    /// </summary>
    public class RolePostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     角色排序
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Sort(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int pId = GetNodeInnerText(xmldoc, "pid").ToInt();
                    string sorts = GetNodeInnerText(xmldoc, "sorts");
                    if (!sorts.IsEmpty())
                    {
                        bool json = RoleService.Instance.Sort(pId, sorts);
                        result.Add("body", json ? "ok" : "err");
                    }
                }
                else
                {
                    result.Add("status", "false");
                    result.Add("body", "用户尚未登录，操作被拒绝");
                }
            }
            catch (Exception ex)
            {
                result.Add("body", ex.Message);
                LogService.Instance.LogException(ex);
            }
            return result;
        }

        /// <summary>
        ///     删除角色
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Delete(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    string id = GetNodeInnerText(xmldoc, "id");
                    List<Role> instance = RoleService.Instance.GetChildrensRrecusive(id.ToInt());
                    int count = 0;
                    foreach (Role role in instance)
                    {
                        RoleService.Instance.Delete(role);
                        LogService.Instance.Log(string.Format("用户 {0} 删除角色：{1}", CurrentUser, role.Name),
                                                string.Format("用户 {0} 删除角色：{1}", CurrentUser, role.Name),
                                                LogCategory.Member);
                        count++;
                    }
                    result.Add("body", count == instance.Count ? "ok" : "err");
                    result.Add("status", count == instance.Count ? "true" : "false");
                }
                else
                {
                    result.Add("status", "false");
                    result.Add("body", "用户尚未登录，操作被拒绝");
                }
            }
            catch (Exception ex)
            {
                result.Add("body", ex.Message);
                LogService.Instance.LogException(ex);
            }
            return result;
        }
        /// <summary>
        ///     设置单个角色菜单权限
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SetRoleMenuPurview(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    var id = GetNodeInnerText(xmldoc, "roleId").ToInt();
                    var purview = GetNodeInnerText(xmldoc, "purview");
                    var flag = RoleService.Instance.SetRoleMenuPurview(id, purview);
                    result.Add("body", flag ? "ok" : "err");
                    result.Add("status", flag ? "true" : "false");
                }
                else
                {
                    result.Add("status", "false");
                    result.Add("body", "用户尚未登录，操作被拒绝");
                }
            }
            catch (Exception ex)
            {
                result.Add("body", ex.Message);
                LogService.Instance.LogException(ex);
            }
            return result;
        }
    }
}