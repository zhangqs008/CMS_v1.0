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
    public class MenuFormHandler : AjaxFormHandler
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
                    string ico = HttpContext.Current.Request.Form["ctl00$ContentPlaceHolder1$txtIco"].ToStr();
                    string url = HttpContext.Current.Request.Form["Url"].ToStr();
                    string description = HttpContext.Current.Request.Form["Description"].ToStr();

                    var instance = MenuService.Instance.SingleOrDefault<Menu>(id);
                    if (instance == null)
                    {
                        instance = ModelFactory<Menu>.Insten();
                    }
                    instance.ParentId = parentId.ToInt();
                    if (instance.ParentId > 0)
                    {
                        var parentCategory = MenuService.Instance.SingleOrDefault<Menu>(instance.ParentId);
                        int level = parentCategory.Level + 1;
                        int orderId = MenuService.Instance.GetListByParentId(instance.ParentId).Count + 1;
                        instance.Level = level.ToInt(); //菜单层级
                        instance.Sort = orderId;
                    }
                    instance.ParentId = parentId.ToInt();
                    instance.Name = name.Trim(); //菜单名称 
                    instance.Ico = ico.Trim(); //图标
                    instance.Url = url.Trim(); //链接地址
                    instance.Description = description.Trim(); //菜单描述
                    if (id.ToInt() > 0)
                    {
                        MenuService.Instance.Modify(instance);
                    }
                    else
                    {
                        MenuService.Instance.Add(instance);
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