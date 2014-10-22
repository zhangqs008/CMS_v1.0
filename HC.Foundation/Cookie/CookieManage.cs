//--------------------------------------------------------------------------------
// 文件描述：统一管理网站的Cookie的写入和移除
// 文件作者： 
// 创建日期：2008-09-01
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Security;

namespace HC.Foundation
{
    /// <summary>
    /// 统一管理网站的Cookie的写入和移除
    /// </summary>
    public class CookieManage
    {
        /// <summary>
        /// 获取用于存储前台用户 Forms 身份验证票证的 Cookie 名称。
        /// </summary>
        public static string UserCookieName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    string applicationPath = string.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath) ? string.Empty : HttpContext.Current.Request.ApplicationPath.Replace("/", string.Empty);
                    if (!string.IsNullOrEmpty(SiteConfig.SiteInfo.MainDomain))
                    {
                        return applicationPath + FormsAuthentication.FormsCookieName;
                    }
                    return HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.Url.Port + applicationPath + FormsAuthentication.FormsCookieName;
                }

                return FormsAuthentication.FormsCookieName;
            }
        }

        /// <summary>
        /// 获取用于存储后台管理员 Forms 身份验证票证的 Cookie 名称。
        /// </summary>
        public static string AdminCookieName
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Request.ApplicationPath != null)
                {
                    string applicationPath = string.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath) ? string.Empty : HttpContext.Current.Request.ApplicationPath.Replace("/", string.Empty);
                    if (!string.IsNullOrEmpty(SiteConfig.SiteInfo.MainDomain))
                    {
                        return applicationPath + FormsAuthentication.FormsCookieName + "AdminCookie";
                    }
                    return HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.Url.Port + applicationPath + FormsAuthentication.FormsCookieName + "AdminCookie";
                }

                return FormsAuthentication.FormsCookieName + "AdminCookie";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authTicket"></param>
        /// <param name="isPersistent"></param>
        /// <param name="expirationTime"></param>
        public static void CreateUserCookie(FormsAuthenticationTicket authTicket, bool isPersistent, DateTime expirationTime)
        {
            CreateCookie(UserCookieName, authTicket, isPersistent, expirationTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authTicket"></param>
        /// <param name="isPersistent"></param>
        /// <param name="expirationTime"></param>
        public static void CreateAdminCookie(FormsAuthenticationTicket authTicket, bool isPersistent, DateTime expirationTime)
        {
            CreateCookie(AdminCookieName, authTicket, isPersistent, expirationTime);
        }

        /// <summary>
        /// 创建Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="authTicket">要写入cookie的内容</param>
        /// <param name="isPersistent">是否指定过期时间</param>
        /// <param name="expirationTime">过期时间</param>
        public static void CreateCookie(string cookieName, FormsAuthenticationTicket authTicket, bool isPersistent, DateTime expirationTime)
        {
            string cookieValue = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(cookieName, cookieValue);
            if (isPersistent)
            {
                authCookie.Expires = expirationTime;
            }

            authCookie.HttpOnly = true;
            authCookie.Path = FormsAuthentication.FormsCookiePath;
            authCookie.Secure = FormsAuthentication.RequireSSL;
            if (!string.IsNullOrEmpty(SiteConfig.SiteInfo.MainDomain))
            {
                if (HttpContext.Current != null)
                {
                    string host = "." + HttpContext.Current.Request.Url.Host;
                    if (host.EndsWith(SiteConfig.SiteInfo.MainDomain, StringComparison.OrdinalIgnoreCase))
                    {
                        authCookie.Domain = SiteConfig.SiteInfo.MainDomain;
                    }
                }
                else
                {
                    authCookie.Domain = SiteConfig.SiteInfo.MainDomain;
                }
            }

            if (HttpContext.Current != null) HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// 移除管理员的Cookie
        /// </summary>
        public static void RemoveAdminCookie()
        {
            string cookieName = AdminCookieName;
            RemoveCookie(cookieName);
            if (SiteConfig.SiteInfo.AdminIsLogoutUser)
            {
                cookieName = UserCookieName;
                RemoveCookie(cookieName);
            }
        }

        /// <summary>
        /// 移除前台用户的cookie
        /// </summary>
        public static void RemoveUserCookie()
        {
            string cookieName = UserCookieName;
            RemoveCookie(cookieName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieName"></param>
        private static void RemoveCookie(string cookieName)
        {
            string cookieValue = string.Empty;
            if (HttpContext.Current.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
            {
                cookieValue = "NoCookie";
            }

            var cookie = new HttpCookie(cookieName, cookieValue)
                {
                    HttpOnly = true,
                    Path = FormsAuthentication.FormsCookiePath,
                    Expires = new DateTime(1999, 10, 12),
                    Secure = FormsAuthentication.RequireSSL
                };
            if (!string.IsNullOrEmpty(SiteConfig.SiteInfo.MainDomain))
            {
                if (HttpContext.Current != null)
                {
                    string host = "." + HttpContext.Current.Request.Url.Host;
                    if (host.EndsWith(SiteConfig.SiteInfo.MainDomain, StringComparison.OrdinalIgnoreCase))
                    {
                        cookie.Domain = SiteConfig.SiteInfo.MainDomain;
                    }
                }
                else
                {
                    cookie.Domain = SiteConfig.SiteInfo.MainDomain;
                }
            }

            HttpContext.Current.Response.Cookies.Remove(cookieName);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
