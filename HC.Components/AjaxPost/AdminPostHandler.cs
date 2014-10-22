//--------------------------------------------------------------------------------
// 文件描述：管理员（用户）Post请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using HC.Ajax;
using HC.Foundation;
using HC.Foundation.Context.Principal;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Framework.Helper;

namespace HC.Components.AjaxPost
{
    /// <summary>
    /// 管理员（用户）Post请求响应类
    /// </summary>
    public class AdminPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     删除管理员
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
                    int id = GetNodeInnerText(xmldoc, "id").ToInt();
                    bool flag = AdministratorService.Delete(HCContext.Current.Admin.Identity.Name, id);
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

        /// <summary>
        ///     设置单个管理员菜单权限
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SetAdminMenuPurview(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = GetNodeInnerText(xmldoc, "adminId").ToInt();
                    string purview = GetNodeInnerText(xmldoc, "purview");
                    bool flag = AdministratorService.Instance.SetAdminMenuPurview(id, purview);
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


        /// <summary>
        ///     设置单个管理员菜单权限
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SetAdminTheme(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HCContext.Current.Admin.AdministratorInfo.Id;
                    string theme = GetNodeInnerText(xmldoc, "theme");
                    bool flag = AdministratorService.Instance.SetAdminTheme(id, theme);
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


        /// <summary>
        ///     设置单个管理员菜单权限
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SendEmail(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    string email = GetNodeInnerText(xmldoc, "email");
                    string title = GetNodeInnerText(xmldoc, "title");
                    string content = GetNodeInnerText(xmldoc, "content");
                    bool flag = EmailService.Send(email, title, content);
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