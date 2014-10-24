//--------------------------------------------------------------------------------
// 文件描述：系统栏目添加、编辑页面
// 文件作者：张清山
// 创建日期：2014-08-31 23:04:38
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Column
{
    public partial class ColumnEdit : AdminPage
    {
        protected Components.Model.Column Instance = new Components.Model.Column();
        protected string ParentColumnHtml = "";
        protected string ModuleIdHtml = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestString("action");
            if (!IsPostBack)
            {
                //上级栏目
                IEnumerable<Components.Model.Column> columns = ColumnService.Instance.GetListForDropDownList(0, "");
                foreach (Components.Model.Column column in columns)
                {
                    string selected = "";
                    if (RequestInt32("parentId") > 0)
                    {
                        if (column.Id == RequestInt32("parentId"))
                        {
                            selected = " selected='selected' ";
                        }
                    }
                    ParentColumnHtml += string.Format("<option value='{0}' {1}>{2}</option>", column.Id, selected, column.Name);
                }

                //绑定模型
                var modules = ModuleService.Instance.GetList();
                foreach (Components.Model.Module module in modules)
                {
                    ModuleIdHtml += string.Format("<option value='{0}' >{1}</option>", module.Id, module.Name);

                }

                if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    int id = RequestInt32("Id", 0);
                    Instance = ColumnService.Instance.SingleOrDefault<Components.Model.Column>(id);
                }
            }
        }
    }
}