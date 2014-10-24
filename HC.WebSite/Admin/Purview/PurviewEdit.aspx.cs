//--------------------------------------------------------------------------------
// 文件描述：系统菜单表添加、编辑页面
// 文件作者：张清山
// 创建日期：2014-08-12 14:41:38
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Purview
{
    public partial class PurviewEdit : AdminPage
    {
        protected Components.Model.Purview Instance = new Components.Model.Purview();

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestString("action");
            if (!IsPostBack)
            {
                dropParentId.DataSource = PurviewService.Instance.GetListForDropDownList(0, "");
                dropParentId.DataTextField = "Name";
                dropParentId.DataValueField = "Id";
                dropParentId.DataBind();
                if (RequestInt32("parentId") > 0)
                {
                    dropParentId.SelectedValue = RequestString("parentId");
                }

                if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    int id = RequestInt32("Id", 0);
                    Instance = PurviewService.Instance.SingleOrDefault<Components.Model.Purview>(id);
                }
            }
        }
    }
}