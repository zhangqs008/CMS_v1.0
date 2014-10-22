using System;
using System.Web;
using HC.Ajax;
using HC.Foundation;
using HC.Foundation.Context.Principal;
using HC.Framework.Extension;
using HC.Framework.Helper;
using HC.Repository;

namespace HC.Components.AjaxForm
{
    public class AdminFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["id"].ToInt();
                    string loginName = HttpContext.Current.Request.Form["LoginName"].ToStr();
                    string trueName = HttpContext.Current.Request.Form["TrueName"].ToStr();
                    string password = HttpContext.Current.Request.Form["Password"].ToStr().ToMD5();
                    string email = HttpContext.Current.Request.Form["Email"].ToStr();
                    string phone = HttpContext.Current.Request.Form["Phone"].ToStr();
                    string sex = HttpContext.Current.Request.Form["Sex"].ToStr();
                    string birthday = HttpContext.Current.Request.Form["Birthday"].ToStr();
                    string status = HttpContext.Current.Request.Form["Status"].ToStr();

                    if (loginName.IsEmpty())
                    {
                        return "用户名不能为空";
                    }
                    if (id == 0 & password.IsEmpty())
                    {
                        return "密码不能为空";
                    }
                    if (id == 0)
                    {
                        if (AdministratorService.Instance.GetAdminInfoByName(loginName) != null)
                        {
                            return "对不起，该管理员已经存在";
                        }
                    }
                    var instance = AdministratorService.Instance.SingleOrDefault<Administrator>(id) ??
                                   ModelFactory<Administrator>.Insten();

                    instance.LoginName = loginName;
                    instance.TrueName = trueName;
                    instance.Password = password;
                    instance.Email = email;
                    instance.Phone = phone;
                    instance.Sex = sex.ToBoolean();
                    instance.Birthday = birthday.ToDateTime();
                    instance.Status = status.ToInt();

                    if (id.ToInt() > 0)
                    {
                        AdministratorService.Instance.Modify(instance);
                    }
                    else
                    {
                        AdministratorService.Instance.Add(instance);
                    }
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