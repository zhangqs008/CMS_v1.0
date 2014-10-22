using System.Configuration;
using System.Web;
using HC.Framework.Helper;

namespace HC.Presentation
{
    /// <summary>
    /// 展现层基类
    /// </summary>
    public class PresentBase
    {
        protected string TemplatePath { get { return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TemplatePath"]); } }

        /// <summary>
        /// 网站根目录路径，末尾已包含“/”
        /// </summary>
        public string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }
        /// <summary>
        ///     消息提示模板
        /// </summary>
        public string MsgTemplate
        {
            get { return HttpContext.Current.Server.MapPath("~/Admin/Style/template/msg.html"); }
        }
        #region 错误提示信息

        /// <summary>
        /// 错误提示信息
        /// </summary>
        /// <param name="title"></param>
        public void WriteErrMsg(string title)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "错误提示信息");
            html = html.Replace("{$content}", "<span style='color:red'>" + title + "</span>");
            html = html.Replace("{$url}", "");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 错误提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        public void WriteErrMsg(string title, string url)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "错误提示信息");
            html = html.Replace("{$content}", "<span style='color:red'>" + title + "</span>");
            html = html.Replace("{$url}", "<div style='text-align: center'><a href='" + url + "'>返回上一页</a></div>");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        #endregion

        #region 提示信息

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param> 
        public void WriteMsg(string title)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "提示信息");
            html = html.Replace("{$content}", "<span style='color:black'>" + title + "</span>");
            html = html.Replace("{$url}", "");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param> 
        public void WriteMsg(string title, string url)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "提示信息");
            html = html.Replace("{$content}", "<span style='color:black'>" + title + "</span>");
            html = html.Replace("{$url}", "<div style='text-align: center'><a href='" + url + "'>返回上一页</a></div>");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        #endregion

        #region 成功提示信息


        /// <summary>
        /// 成功提示信息
        /// </summary>
        /// <param name="title"></param> 
        public void WriteSuccessMsg(string title)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "成功提示信息");
            html = html.Replace("{$content}", "<span style='color:green'>" + title + "</span>");
            html = html.Replace("{$url}", "");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 成功提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param> 
        public void WriteSuccessMsg(string title, string url)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "成功提示信息");
            html = html.Replace("{$content}", "<span style='color:green'>" + title + "</span>");
            html = html.Replace("{$url}", "<div style='text-align: center'><a href='" + url + "'>返回上一页</a></div>");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        #endregion

    }
}
