using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using HC.Framework.Extension;

namespace HC.Foundation.Page
{
    public class AdminPage : BasePage
    {
        /// <summary>
        /// 网站管理根目录路径，末尾已包含“/”
        /// </summary>
        public string AdminPath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath) + "Admin/"; }
        }

        /// <summary>
        ///     引发 Init 事件以对页进行初始化
        /// </summary>
        /// <param name="e">事件数据</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.Header.Title = string.IsNullOrEmpty(Page.Header.Title)
                                    ? SiteConfig.SiteInfo.SiteName
                                    : Page.Header.Title;

            if (!HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                Response.Redirect(AdminPath + "Account/login.aspx");
            }
        }

        #region 提示信息

        public void Alert(string message)
        {
            Alert("提示信息", message, string.Empty);
        }

        #region 错误提示信息
        public void AlertErrorMsg(string message)
        {
            Alert("错误提示信息", message, string.Empty);
        }

        protected void AlertErrorMsg(string msg, bool refush)
        {
            if (refush)
            {
                string url = Request.Url.OriginalString;
                Alert("错误提示信息", msg, url);
            }
            else
            {
                Alert("错误提示信息", msg, string.Empty);
            }

        }

        protected void AlertErrorMsg(string msg, string url)
        {
            Alert("错误提示信息", msg, url);
        }

        #endregion

        #region 成功提示信息
        protected void AlertSucessMsg(string msg)
        {
            Alert("成功提示信息", msg, string.Empty);
        }
        protected void AlertSucessMsg(string msg, bool refush)
        {
            string url = Request.Url.OriginalString;
            Alert("成功提示信息", msg, url);
        }

        protected void AlertSucessMsg(string msg, string url)
        {
            Alert("成功提示信息", msg, url);
        }

        public void Alert(string title, string msg, string url)
        {
            title = title.Replace("\"", "\\\"");
            msg = msg.Replace("\"", "\\\"");
            string script = "$.messager.alert(\"{0}\", \"{1}\");".FormatWith(title, msg);
            if (!string.IsNullOrEmpty(url))
            {
                if (url.StartsWith("~/"))
                {
                    url = BasePath + url.Substring(2);
                }
                script += "window.location.href='" + url + "'";
            }
            ClientScript.RegisterStartupScript(GetType(), DateTime.Now.Ticks.ToStr(), script, true);
        }

        #endregion

        #endregion


    }
}