using System;
using System.Collections.Generic;
using System.Globalization;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation.Page;
using HC.Repository;

namespace HC.WebSite.Admin.Content
{
    public partial class List : AdminPage
    {
        public Components.Model.Column Column = new Components.Model.Column();
        public string TableColumn = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestInt32("columnId") > 0)
            {
                Column = ColumnService.Instance.SingleOrDefault<Components.Model.Column>(RequestInt32("columnId"));

                Page<ColumnField> fields = ColumnFieldService.Instance.Page(1, 10000,
                                                                             new Dictionary<string, string>
                                                                                        {
                                                                                            {
                                                                                                "columnId",
                                                                                                Column.Id.ToString(CultureInfo.InvariantCulture)
                                                                                            }
                                                                                        },
                                                                             " Order by Sort ASC ");
                foreach (ColumnField field in fields.Items)
                {
                    TableColumn += string.Format("{{field:'{1}',title:'{0}',width:100,align:'left'}},", field.Note.Replace("'", ""), field.Name.Replace("'", ""));
                }
            }
        }
    }
}