using System;
using System.Web;
using System.Web.Security;
using HC.Foundation;
using HC.Foundation.Context.Principal;
using HC.Foundation.Log;
using HC.Foundation.Page;
using HC.Framework;
using HC.Framework.Extension;
using HC.Framework.Helper;

namespace HC.WebSite.Admin.Account
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = SiteConfig.SiteInfo.SiteName;
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length > 0 && txtPwd.Text.Trim().Length > 0)
            {
                if (txtVerifiyCode.Text.Trim().ToLower() ==
                    HttpContext.Current.Session["ValidateCodeSession"].ToStr().ToLower())
                {
                    Administrator admin = AdministratorService.Instance.GetAdminInfoByName(txtName.Text.Trim());
                    if (admin != null)
                    {
                        if (admin.Password == txtPwd.Text.Trim().ToMD5())
                        {
                            var adminPrincipal = new AdminPrincipal
                                {
                                    LoginName = admin.LoginName
                                };
                            string userData = adminPrincipal.SerializeToString();

                            int ticketTime = SiteConfig.SiteInfo.TicketTime == 0 ? 20 : SiteConfig.SiteInfo.TicketTime;

                            var authTicket = new FormsAuthenticationTicket(
                                1, admin.LoginName, DateTime.Now,
                                DateTime.Now.AddMinutes(ticketTime), false, userData);

                            CookieManage.CreateAdminCookie(authTicket, false, DateTime.Now);
                            admin.LoginCount = admin.LoginCount + 1;
                            AdministratorService.Instance.Update(admin);

                            LogService.Instance.Log(string.Format("管理员 {0} 登录成功", admin.LoginName),
                                                    "管理员 " + admin.LoginName + " 登录成功，IP地址：" + IPHelper.GetClientIP(),
                                                    LogCategory.Member);
                            Response.Redirect(BasePath + "Admin/Default.aspx");
                        }
                        else
                        {
                            LogService.Instance.Log(string.Format("管理员 {0} 登录失败", admin.LoginName),
                                                    "管理员 " + admin.LoginName + " 登录失败，密码输入错误", LogCategory.Member);
                            WriteErrMsg("对不起，密码输入错误！", BasePath + "Admin/Account/login.aspx");
                        }
                    }
                    else
                    {
                        LogService.Instance.Log(string.Format("管理员 {0} 登录失败", txtName.Text.Trim()),
                                                "管理员 " + txtName.Text.Trim() + " 登录失败，该管理员不存在", LogCategory.Member);
                        WriteErrMsg("对不起，该管理员不存在！", BasePath + "Admin/Account/login.aspx");
                    }
                }
                else
                {
                    WriteErrMsg("对不起，验证码输入错误！", BasePath + "Admin/Account/login.aspx");
                }
            }
            else
            {
                WriteErrMsg("管理员名和密码不能为空！", BasePath + "Admin/Account/login.aspx");
            }
        }

        protected void BtnCancleClick(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtPwd.Text = "";
        }

        protected void BtnRegisteClick(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}