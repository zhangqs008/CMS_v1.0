using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Column
{
    public partial class ColumnFieldManage : AdminPage
    {
        protected Components.Model.Column Column = new Components.Model.Column();
        protected Components.Model.Module Module = new Components.Model.Module();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int columnId = RequestInt32("ColumnId");
                Column = ColumnService.Instance.SingleOrDefault<Components.Model.Column>(columnId);
                Module = ModuleService.Instance.SingleOrDefault<Components.Model.Module>(Column.ModuleId);
            }
        }
    }
}