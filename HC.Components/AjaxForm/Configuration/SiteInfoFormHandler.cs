using System;
using System.Web;
using HC.Ajax;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxForm.Configuration
{
    internal class SiteInfoFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    string siteName = HttpContext.Current.Request.Form["SiteName"].ToStr();
                    string ticketTime = HttpContext.Current.Request.Form["TicketTime"].ToStr();
                    string copyright = HttpContext.Current.Request.Form["Copyright"].ToStr();
                    string mainDomain = HttpContext.Current.Request.Form["MainDomain"].ToStr();
                    string logo = HttpContext.Current.Request.Form["Logo"].ToStr();
                    string icon = HttpContext.Current.Request.Form["Icon"].ToStr();

                    if (siteName.IsEmpty())
                    {
                        return "站点名称不能为空";
                    }
                    SiteInfo config = SiteConfig.SiteInfo;
                    config.SiteName = siteName;
                    config.TicketTime = ticketTime.ToInt(20);
                    config.Copyright = copyright;
                    config.MainDomain = mainDomain;
                    config.Logo = logo;
                    config.Icon = icon;
                    BaseSiteConfig.UpdateConfig(config);
                    LogService.Instance.Log("管理员 " + HCContext.Current.Admin.Identity.Name + " 修改站点基本配置", config.ToXml());
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