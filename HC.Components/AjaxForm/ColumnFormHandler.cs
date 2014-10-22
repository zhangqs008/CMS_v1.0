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
    public class ColumnFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["Id"].ToInt(); //Id
                    string name = HttpContext.Current.Request.Form["Name"].ToStr(); //栏目名称
                    int parentId = HttpContext.Current.Request.Form["ParentId"].ToInt(); //上级栏目 
                    bool isShowFront = HttpContext.Current.Request.Form["IsShowFront"].ToBoolean(); //是否前台显示
                    int moduleId = HttpContext.Current.Request.Form["ModuleId"].ToInt(); //栏目绑定模型Id 


                    var instance = ColumnService.Instance.SingleOrDefault<Column>(id) ??
                                   ModelFactory<Column>.Insten();

                    instance.ParentId = parentId.ToInt();
                    if (instance.ParentId > 0)
                    {
                        var parentCategory = ColumnService.Instance.SingleOrDefault<Column>(instance.ParentId);
                        int level = parentCategory.Level + 1;
                        instance.Level = level.ToInt(); //层级
                        if (instance.Id == 0)
                        {
                            int orderId = ColumnService.Instance.GetListByParentId(instance.ParentId).Count + 1;
                            instance.Sort = orderId;
                        }
                    }
                    instance.Name = name;
                    instance.ParentId = parentId;
                    instance.IsShowFront = isShowFront;
                    instance.ModuleId = moduleId;
                    instance.CreateUser = HCContext.Current.Admin.LoginName;
                    if (id.ToInt() > 0)
                    {
                        ColumnService.Instance.Modify(instance);
                    }
                    else
                    {
                        ColumnService.Instance.Add(instance);
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