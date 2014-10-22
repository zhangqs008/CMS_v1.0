using System;
using System.Text.RegularExpressions;
using System.Web;
using HC.Foundation.Page;
using HC.Framework.Helper;

namespace HC.Foundation.HttpModules
{
    /// <summary>
    /// Web请求安全检查：防止跨站点脚本,Sql注入等攻击,来自:http://bbs.webscan.360.cn/forum.php?mod=viewthread&tid=711&page=1&extra=#pid1927
    /// 检查数据包括:
    /// 1.Cookie
    /// 2.当前页面地址
    /// 3.ReferrerUrl
    /// 4.Post数据
    /// 5.Get数据
    /// </summary>
    public class Safe360 : BaseHttpModule
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public Safe360()
        {
            LoadEventList = EventOptions.BeginRequest;
        }

        /// <summary>
        /// 请求开始事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        internal override void Application_BeginRequest(object source, EventArgs e)
        {
            Procress();
        }
        /// <summary>
        ///     消息提示模板
        /// </summary>
        public static string MsgTemplate
        {
            get { return HttpContext.Current.Server.MapPath("~/Admin/Style/template/msg.html"); }
        }
        /// <summary>
        /// 错误提示信息
        /// </summary>
        /// <param name="title"></param>
        public static void WriteErrMsg(string title)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "错误提示信息");
            html = html.Replace("{$content}", "<span style='color:red'>" + title + "</span>");
            html = html.Replace("{$url}", "<div style='text-align: center'><a href='" + BasePath + "'>返回网站首页</a></div>");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }
        #region 执行安全检查

        /// <summary>
        /// 执行安全检查
        /// </summary>
        public static void Procress()
        {
            if (RawUrl())
            {
                WriteErrMsg("对不起，您的请求中含有恶意字符，请求已停止响应");
            }

            if (CookieData())
            {
                WriteErrMsg("对不起，您的请求中含有恶意字符，请求已停止响应");
            }

            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                if (Referer())
                {
                    WriteErrMsg("对不起，您的请求中含有恶意字符，请求已停止响应");
                }
            }

            if (HttpContext.Current.Request.RequestType.ToUpper() == "POST")
            {
                if (PostData())
                {
                    WriteErrMsg("对不起，您的请求中含有恶意字符，请求已停止响应");
                }
            }
            if (HttpContext.Current.Request.RequestType.ToUpper() == "GET")
            {
                if (GetData())
                {
                    WriteErrMsg("对不起，您的请求中含有恶意字符，请求已停止响应");
                }
            }
        }

        #endregion

        #region 安全检查正则

        /// <summary>
        /// 安全检查正则
        /// </summary>
        private const string StrRegex =
            @"(?i)<[^>]+?style=[\w]+?:expression\(|\b(alert|confirm|prompt)\b|^\+/v(8|9)|<[^>]*?=[^>]*?&#[^>]*?>|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";

        #endregion

        #region 检查Post数据

        /// <summary>
        /// 检查Post数据
        /// </summary>
        /// <returns></returns>
        private static bool PostData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Form[i]);
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        #region 检查Get数据

        /// <summary>
        /// 检查Get数据
        /// </summary>
        /// <returns></returns>
        private static bool GetData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.QueryString[i]);
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        #region 检查Cookie数据

        /// <summary>
        /// 检查Cookie数据
        /// </summary>
        /// <returns></returns>
        private static bool CookieData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Cookies[i].Value.ToLower());
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        #region 检查Referer

        /// <summary>
        /// 检查Referer
        /// </summary>
        /// <returns></returns>
        private static bool Referer()
        {
            return CheckData(HttpContext.Current.Request.UrlReferrer.ToString());
        }

        #endregion

        #region 检查当前请求路径

        /// <summary>
        /// 检查当前请求路径
        /// </summary>
        /// <returns></returns>
        private static bool RawUrl()
        {
            return CheckData(HttpContext.Current.Request.RawUrl);
        }

        #endregion

        #region 正则匹配

        /// <summary>
        /// 正则匹配
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private static bool CheckData(string inputData)
        {
            return Regex.IsMatch(inputData, StrRegex);
        }

        #endregion
    }
}