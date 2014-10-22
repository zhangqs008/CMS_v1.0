using System;
using System.Web;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxForm
{
    public class RoleFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["id"].ToInt();
                    string name = HttpContext.Current.Request.Form["Name"].ToStr();
                    string parentId = HttpContext.Current.Request.Form["ctl00$ContentPlaceHolder1$dropParentId"].ToStr();

                    var instance = RoleService.Instance.SingleOrDefault<Role>(id);
                    if (instance == null)
                    {
                        instance = ModelFactory<Role>.Insten();
                    }
                    instance.ParentId = parentId.ToInt();
                    if (instance.ParentId > 0)
                    {
                        int orderId = MenuService.Instance.GetListByParentId(instance.ParentId).Count + 1;
                        instance.Sort = orderId;
                    }
                    instance.ParentId = parentId.ToInt();
                    instance.Name = name.Trim();
                    if (id.ToInt() > 0)
                    {
                        RoleService.Instance.Modify(instance);
                    }
                    else
                    {
                        RoleService.Instance.Add(instance);
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