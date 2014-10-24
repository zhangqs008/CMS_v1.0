using System;
using System.Web;
using System.Web.Security;
using HC.Foundation;
using HC.Foundation.Context.Principal;
using HC.Foundation.Page;
using HC.Framework.Extension;
using HC.Framework.Helper;
using HC.Repository;

namespace HC.WebSite.Admin.Account
{
    public partial class Register : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "用户注册";
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim().Length > 0 &&
                txtPassword.Text.Trim().Length > 0 &&
                txtEmail.Text.Trim().Length > 0
                )
            {
                if (txtVerifiyCode.Text.Trim().ToLower() ==
                    HttpContext.Current.Session["ValidateCodeSession"].ToStr().ToLower())
                {
                    Administrator admin = AdministratorService.Instance.GetAdminInfoByName(txtUserName.Text.Trim());
                    if (admin == null)
                    {
                        admin = ModelFactory<Administrator>.Insten();
                        admin.LoginName = txtUserName.Text.Trim();
                        admin.Password = txtPassword.Text.Trim().ToMD5();
                        admin.Email = txtEmail.Text.Trim();
                        admin.TrueName = txtUserName.Text.Trim();
                        if (AdministratorService.Instance.Insert(admin).ToInt() > 0)
                        {
                            //登录
                            var adminPrincipal = new AdminPrincipal();
                            adminPrincipal.LoginName = admin.LoginName;
                            string userData = adminPrincipal.SerializeToString();

                            var authTicket = new FormsAuthenticationTicket(
                                1, admin.LoginName, DateTime.Now,
                                DateTime.Now.AddMinutes(SiteConfig.SiteInfo.TicketTime), false, userData);

                            CookieManage.CreateAdminCookie(authTicket, false, DateTime.Now);
                            admin.LoginCount = admin.LoginCount + 1;
                            AdministratorService.Instance.Update(admin);

                            Response.Redirect(BasePath + "Default.aspx");
                        }
                    }
                    else
                    {
                        WriteErrMsg("对不起，该用户名已存在！", BasePath + "Admin/Account/Register.aspx");
                    }
                }
                else
                {
                    WriteErrMsg("对不起，验证码输入错误！", BasePath + "Admin/Account/login.aspx");
                }
            }
            else
            {
                WriteErrMsg("对不起，用户名，密码，邮箱均不能为空！", BasePath + "Admin/Account/Register.aspx");
            }
        }

        protected void BtnCancleClick(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}