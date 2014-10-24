//--------------------------------------------------------------------------------
// 文件描述：内容模型添加、编辑页面
// 文件作者：张清山
// 创建日期：2014-08-30 12:07:56
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Components.Service;
using HC.Foundation.Page;
using ModuleInfo = HC.Components.Model.Module;
namespace HC.WebSite.Admin.Module
{
    public partial class ModuleEdit : AdminPage
    {
        protected ModuleInfo Instance = new ModuleInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestString("action");
            if (!IsPostBack)
            {
                if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    int id = RequestInt32("Id", 0);
                    Instance = ModuleService.Instance.SingleOrDefault<ModuleInfo>(id);
                }
            }
        }
    }
}