//--------------------------------------------------------------------------------
// 文件描述：模型字段管理页面
// 文件作者：张清山
// 创建日期：2014-08-30 15:22:33
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Module
{
    public partial class ModuleFieldManage : AdminPage
    {
        protected Components.Model.Module Module = new Components.Model.Module();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int moduleId = RequestInt32("moduleId");
                Module = ModuleService.Instance.SingleOrDefault<Components.Model.Module>(moduleId);
            }
        }
    }
}