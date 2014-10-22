//--------------------------------------------------------------------------------
// 文件描述：系统权限表表单提交处理类
// 文件作者：张清山
// 创建日期：2014-08-21 09:27:08
// 修改记录： 
//--------------------------------------------------------------------------------

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
    public class PurviewFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["Id"].ToInt(); //Id
                    string name = HttpContext.Current.Request.Form["Name"].ToStr(); //权限名称
                    int parentId = HttpContext.Current.Request.Form["ctl00$ContentPlaceHolder1$dropParentId"].ToInt(); //上级权限
                    int level = HttpContext.Current.Request.Form["Level"].ToInt(); //权限层级
                    string operateCode = HttpContext.Current.Request.Form["OperateCode"].ToStr(); //权限操作码
                    string description = HttpContext.Current.Request.Form["Description"].ToStr(); //权限描述

                    var exsitRoot = DbHelper.CurrentDb.ExecuteScalar<object>("SELECT COUNT(*) FROM HC_Purview").ToInt(0) > 0;
                    var root = ModelFactory<Purview>.Insten();
                    if (!exsitRoot)
                    {
                        root.ParentId = 0;
                        root.Name = "系统权限";
                        root.Level = 0;
                        root.OperateCode = "System";
                        root.Description = "系统权限根节点";
                        PurviewService.Instance.Add(root);
                    }

                    if (id == 0)
                    {
                        if (DbHelper.CurrentDb.ExecuteScalar<object>("SELECT COUNT(*) FROM HC_Purview WHERE OperateCode=@0", operateCode).ToInt(0) >
                            0)
                        {
                            return "对不起，该权限码已存在";
                        }
                    }

                    var instance = PurviewService.Instance.SingleOrDefault<Purview>(id) ??
                                   ModelFactory<Purview>.Insten();
                    if (instance.ParentId > 0)
                    {
                        var parentCategory = PurviewService.Instance.SingleOrDefault<Purview>(instance.ParentId);
                        level = parentCategory.Level + 1;
                        int orderId = PurviewService.Instance.GetListByParentId(instance.ParentId).Count + 1;
                        instance.Level = level.ToInt(); //菜单层级
                        instance.Sort = orderId;
                    }

                    instance.ParentId = exsitRoot ? parentId.ToInt() : root.Id;
                    instance.Id = id;
                    instance.Name = name; 
                    instance.Level = level;
                    instance.OperateCode = operateCode;
                    instance.Description = description;

                    if (id.ToInt() > 0)
                    {
                        PurviewService.Instance.Modify(instance);
                    }
                    else
                    {
                        PurviewService.Instance.Add(instance);
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