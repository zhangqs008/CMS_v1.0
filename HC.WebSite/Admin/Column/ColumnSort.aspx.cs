using System;
using System.Collections.Generic;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Column
{
    public partial class ColumnSort : AdminPage
    {
        protected string Html = "";
        protected int ParentId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ParentId = RequestInt32("id");
            if (!IsPostBack)
            {
                Html = "";
                List<Components.Model.Column> columns = ColumnService.Instance.GetListByParentId(ParentId);
                foreach (Components.Model.Column column in columns)
                {
                    Html += string.Format("<li class=\"sortItem\" cid=\"{0}\">{1}</li>{2}", column.Id, column.Name,
                                          Environment.NewLine);
                }
            }
        }
    }
}