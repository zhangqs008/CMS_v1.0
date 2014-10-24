//--------------------------------------------------------------------------------
// 文件描述：角色组织架构添加、编辑页面
// 文件作者：张清山
// 创建日期：2014-08-13 10:14:46
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Globalization;
using HC.Components.Service;
using HC.Foundation.Page;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.WebSite.Admin.Role
{
    public partial class RoleEdit : AdminPage
    {
        protected HC.Components.Model.Role Instance = new Components.Model.Role();
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestString("action");
            if (!IsPostBack)
            {
                dropParentId.DataSource = RoleService.Instance.GetListForDropDownList(0, "");
                dropParentId.DataTextField = "Name";
                dropParentId.DataValueField = "Id";
                dropParentId.DataBind();

                if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    int id = RequestInt32("Id", 0);
                    Instance = RoleService.Instance.SingleOrDefault<Components.Model.Role>(id);

                }
            }
        }

    }
}