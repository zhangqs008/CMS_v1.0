//--------------------------------------------------------------------------------
// 文件描述：安全模块
// 文件作者：张清山
// 创建日期：2014-8-11 
//--------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using HC.Foundation.Context.Principal;
using HC.Repository;

namespace HC.Foundation.HttpModules.AuthModule
{
    /// <summary>
    ///     安全模块
    /// </summary>
    public class AdminAuthModule : BaseHttpModule
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public AdminAuthModule()
        {
            LoadEventList = EventOptions.AuthenticateRequest | EventOptions.PostAuthenticateRequest;
        }


        internal override void Application_AuthenticateRequest(object source, EventArgs e)
        {
            HttpContext context = ((HttpApplication)source).Context;

            string adminCookieName = CookieManage.AdminCookieName;
            FormsAuthenticationTicket adminAuthTicket = ExtractTicketFromCookie(context, adminCookieName);
            if (adminAuthTicket == null)
            {
                return;
            }

            #region 设置HttpContext上下文

            SlidingExpiration(context, adminAuthTicket, adminCookieName);
            AdminPrincipal principal = AdminPrincipal.CreatePrincipal(adminAuthTicket);

            if (principal.Identity.IsAuthenticated)
            {
                principal.AdministratorInfo = GetAdminInfoByName(principal.LoginName);
                HCContext.Current.Admin = principal;
            }

            #endregion
        }

        public Administrator GetAdminInfoByName(string name)
        {
            const string sql = "SELECT * FROM HC_Administrator WHERE LoginName=@0";
            var admin = DbHelper.CurrentDb.SingleOrDefault<Administrator>(sql, name);
            return admin ?? new Administrator();
        }

        /// <summary>
        ///     设置Cookie的可调过期
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="ticket">FormsAuthenticationTicket</param>
        /// <param name="cookieName">Cookie名称</param>
        private static void SlidingExpiration(HttpContext context, FormsAuthenticationTicket ticket, string cookieName)
        {
            FormsAuthenticationTicket newAuthTicket = FormsAuthentication.SlidingExpiration ? FormsAuthentication.RenewTicketIfOld(ticket) : ticket;

            if (newAuthTicket != null)
            {
                string cookieValue = FormsAuthentication.Encrypt(newAuthTicket);

                HttpCookie authCookie = context.Request.Cookies[cookieName] ??
                                        new HttpCookie(cookieName, cookieValue) { Path = newAuthTicket.CookiePath };

                if (ticket.IsPersistent)
                {
                    authCookie.Expires = newAuthTicket.Expiration;
                }

                authCookie.Value = cookieValue;
                authCookie.Secure = FormsAuthentication.RequireSSL;
                authCookie.HttpOnly = true;
                if (!string.IsNullOrEmpty(SiteConfig.SiteInfo.MainDomain))
                {
                    authCookie.Domain = SiteConfig.SiteInfo.MainDomain;
                }
                context.Response.Cookies.Remove(authCookie.Name);
                context.Response.Cookies.Add(authCookie);
            }
        }

        /// <summary>
        ///     从Cookie获取FormsAuthenticationTicket
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="name">Cookie名称</param>
        /// <returns>FormsAuthenticationTicket</returns>
        private static FormsAuthenticationTicket ExtractTicketFromCookie(HttpContext context, string name)
        {
            FormsAuthenticationTicket ticket = null;
            string encryptedTicket = null;

            HttpCookie cookie = context.Request.Cookies[name];
            if (cookie != null)
            {
                encryptedTicket = cookie.Value;
            }

            if ((encryptedTicket != null) && (encryptedTicket.Length > 1))
            {
                try
                {
                    ticket = FormsAuthentication.Decrypt(encryptedTicket);
                }
                catch (ArgumentException)
                {
                    return null;
                }
                catch (CryptographicException)
                {
                    context.Request.Cookies.Remove(name);
                }

                if (ticket != null)
                {
                    if (SiteConfig.SiteInfo.TicketTime == 0)
                    {
                        return ticket;
                    }

                    if (!ticket.Expired)
                    {
                        return ticket;
                    }
                }

                return null;
            }

            return null;
        }
    }
}