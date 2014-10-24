using System;
using System.Web;
using HC.Foundation;
using HC.Foundation.Context.Principal;
using HC.Foundation.Log;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Account
{
    public partial class Logout : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                //更新最后退出时间
                Administrator admin =
                    AdministratorService.Instance.GetAdminInfoByName(
                        HCContext.Current.Admin.AdministratorInfo.LoginName);
                admin.LastLogOffTime = DateTime.Now;
                AdministratorService.Instance.Update(admin);

                CookieManage.RemoveAdminCookie();

                //记录日志
                LogService.Instance.Log(string.Format("管理员 {0} 退出登录", admin.LoginName),
                                        "管理员 " + HttpContext.Current.User.Identity.Name + " 退出成功", LogCategory.Member);
            }

            Response.Redirect("Login.aspx");
        }
    }
}