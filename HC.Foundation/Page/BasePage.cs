using System;
using System.Web;
using HC.Framework.Extension;
using HC.Framework.Helper;

namespace HC.Foundation.Page
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 当前登录用户 
        /// </summary>
        public static string CurrentUserName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        return HttpContext.Current.User.Identity.Name;
                    }
                }
                return "游客";
            }
        }

        /// <summary>
        ///     消息提示模板
        /// </summary>
        public string MsgTemplate
        {
            get { return Server.MapPath("~/Style/template/msg.html"); }
        }

        /// <summary>
        ///     网站根目录路径，末尾已包含“/”
        /// </summary>
        public static string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }

        #region Url查询参数

        /// <summary>
        ///     获取指定查询字符串变量的 Int32 值，如果获取失败则返回默认值。
        /// </summary>
        /// <param name="queryItem"></param>
        /// <returns></returns>
        public static int RequestInt32(string queryItem)
        {
            return RequestInt32(queryItem, 0);
        }

        /// <summary>
        ///     获取指定查询字符串变量的 Int32 值，如果获取失败则返回默认值。
        /// </summary>
        /// <param name="queryItem">查询字符串变量</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>查询参数值</returns>
        public static int RequestInt32(string queryItem, int defaultValue)
        {
            return HttpContext.Current.Request.QueryString[queryItem].ToInt(defaultValue);
        }

        /// <summary>
        ///     获取指定查询字符串变量的 String 值，如果获取失败则返回默认字符串。
        /// </summary>
        /// <param name="queryItem"></param>
        /// <returns></returns>
        public static string RequestString(string queryItem)
        {
            return RequestString(queryItem, string.Empty);
        }

        /// <summary>
        ///     获取指定查询字符串变量的 String 值，如果获取失败则返回默认字符串。
        /// </summary>
        /// <param name="queryItem">查询字符串变量</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>查询参数值</returns>
        public static string RequestString(string queryItem, string defaultValue)
        {
            string requestString = HttpContext.Current.Request.QueryString[queryItem];
            if (requestString == null)
            {
                return defaultValue;
            }
            return requestString.Trim();
        }

        #endregion

        #region 错误提示信息

        /// <summary>
        /// 错误提示信息
        /// </summary>
        /// <param name="title"></param>
        public void WriteErrMsg(string title)
        {
            Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "错误提示信息");
            html = html.Replace("{$content}", "<span style='color:red'>" + title + "</span>");
            html = html.Replace("{$url}", "");
            Response.Write(html);
            Response.End();
        }

        /// <summary>
        /// 错误提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        public void WriteErrMsg(string title, string url)
        {
            Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "错误提示信息");
            html = html.Replace("{$content}", "<span style='color:red'>" + title + "</span>");
            html = html.Replace("{$url}", "<div style='text-align: center'><a href='" + url + "'>返回上一页</a></div>");
            Response.Write(html);
            Response.End();
        }

        #endregion

        #region 提示信息

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param> 
        public void WriteMsg(string title)
        {
            Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "提示信息");
            html = html.Replace("{$content}", "<span style='color:black'>" + title + "</span>");
            html = html.Replace("{$url}", "");
            Response.Write(html);
            Response.End();
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param> 
        public void WriteMsg(string title, string url)
        {
            Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "提示信息");
            html = html.Replace("{$content}", "<span style='color:black'>" + title + "</span>");
            html = html.Replace("{$url}", "<div style='text-align: center'><a href='" + url + "'>返回上一页</a></div>");
            Response.Write(html);
            Response.End();
        }

        #endregion


        #region 成功提示信息


        /// <summary>
        /// 成功提示信息
        /// </summary>
        /// <param name="title"></param> 
        public void WriteSuccessMsg(string title)
        {
            Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "成功提示信息");
            html = html.Replace("{$content}", "<span style='color:green'>" + title + "</span>");
            html = html.Replace("{$url}", "");
            Response.Write(html);
            Response.End();
        }

        /// <summary>
        /// 成功提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param> 
        public void WriteSuccessMsg(string title, string url)
        {
            Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "成功提示信息");
            html = html.Replace("{$content}", "<span style='color:green'>" + title + "</span>");
            html = html.Replace("{$url}", "<div style='text-align: center'><a href='" + url + "'>返回上一页</a></div>");
            Response.Write(html);
            Response.End();
        }

        #endregion


        /// <summary>
        ///     引发 Init 事件以对页进行初始化
        /// </summary>
        /// <param name="e">事件数据</param>
        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);

            #region 设置站点标题

            Page.Header.Title = string.IsNullOrEmpty(Page.Header.Title)
                                    ? SiteConfig.SiteInfo.SiteName
                                    : Page.Header.Title;

            #endregion

        }

        public void SetTitle(string title)
        {
            Title = title + "-" + SiteConfig.SiteInfo.SiteName;
        }
    }
}