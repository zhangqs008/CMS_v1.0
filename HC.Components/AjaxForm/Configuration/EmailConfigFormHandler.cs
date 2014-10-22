using System;
using System.Web;
using HC.Ajax;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxForm.Configuration
{
    internal class EmailConfigFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    string host = HttpContext.Current.Request.Form["Host"].ToStr();
                    string mailFrom = HttpContext.Current.Request.Form["MailFrom"].ToStr();
                    string notifyEmail = HttpContext.Current.Request.Form["NotifyEmail"].ToStr();
                    string password = HttpContext.Current.Request.Form["Password"].ToStr();
                    string port = HttpContext.Current.Request.Form["Port"].ToStr();
                    string userName = HttpContext.Current.Request.Form["UserName"].ToStr();

                    if (host.IsEmpty())
                    {
                        return "服务器名称不能为空";
                    }
                    EmailConfig config = SiteConfig.EmailConfig;
                    config.Host = host;
                    config.MailFrom = mailFrom;
                    config.NotifyEmail = notifyEmail;
                    config.Password = password;
                    config.Port = port;
                    config.UserName = userName;
                    BaseSiteConfig.UpdateConfig(config);
                    LogService.Instance.Log("管理员 " + HCContext.Current.Admin.Identity.Name + " 修改邮件配置", config.ToXml());
                    return "true";
                }
                return "用户尚未登录，操作被拒绝";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}