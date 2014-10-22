//--------------------------------------------------------------------------------
// 文件描述： 角色成员Post请求响应类
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
using HC.Repository;

namespace HC.Components.AjaxPost
{
    /// <summary>
    /// 角色成员Post请求响应类
    /// </summary>
    public class RoleMemberPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     添加成员到角色
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Add(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                string roleId = GetNodeInnerText(xmldoc, "roleId");
                string adminIds = GetNodeInnerText(xmldoc, "adminIds");
                if (roleId.IsNotEmpty() & adminIds.IsNotEmpty())
                {
                    try
                    {
                        DbHelper.CurrentDb.BeginTransaction();
                        string[] admins = adminIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string admin in admins)
                        {
                            bool exsit = DbHelper.CurrentDb.ExecuteScalar<object>(
                                    "SELECT COUNT(*)FROM HC_RoleMember WHERE AdminId=@0 AND RoleId=@1",
                                    admin.ToInt(), roleId.ToInt()).ToInt() > 0;
                            if (!exsit)
                            {
                                RoleMember instance = ModelFactory<RoleMember>.Insten();
                                instance.AdminId = admin.ToInt();
                                instance.RoleId = roleId.ToInt();
                                RoleMemberService.Instance.Add(instance);
                            }
                        }
                        DbHelper.CurrentDb.CompleteTransaction();
                        result.Add("status", "true");
                        result.Add("body", "ok");
                    }
                    catch (Exception ex)
                    {
                        result.Add("body", ex.Message);
                        LogService.Instance.LogException(ex);
                        DbHelper.CurrentDb.AbortTransaction();
                    }
                }
            }
            else
            {
                result.Add("status", "false");
                result.Add("body", "用户尚未登录，操作被拒绝");
            }
            return result;
        }

        /// <summary>
        ///     从角色移除成员
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Delete(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                string roleId = GetNodeInnerText(xmldoc, "roleId");
                string adminIds = GetNodeInnerText(xmldoc, "adminIds");
                if (roleId.IsNotEmpty() & adminIds.IsNotEmpty())
                {
                    try
                    {
                        DbHelper.CurrentDb.BeginTransaction();
                        string[] admins = adminIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string admin in admins)
                        {
                            DbHelper.CurrentDb.Execute("Delete FROM HC_RoleMember WHERE AdminId=@0 AND RoleId=@1", admin.ToInt(), roleId.ToInt());
                        }
                        DbHelper.CurrentDb.CompleteTransaction();
                        result.Add("status", "true");
                        result.Add("body", "ok");
                    }
                    catch (Exception ex)
                    {
                        result.Add("body", ex.Message);
                        LogService.Instance.LogException(ex);
                        DbHelper.CurrentDb.AbortTransaction();
                    }
                }
            }
            else
            {
                result.Add("status", "false");
                result.Add("body", "用户尚未登录，操作被拒绝");
            }
            return result;
        }
    }
}