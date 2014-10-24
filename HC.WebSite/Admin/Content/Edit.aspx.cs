using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HC.Components.Content;
using HC.Components.Content.Controls;
using HC.Components.Model;
using HC.Components.Service;
using HC.Enumerations.Content;
using HC.Foundation.Page;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.WebSite.Admin.Content
{
    public partial class Edit : AdminPage
    {
        public Components.Model.Column Column = new Components.Model.Column();
        public StringBuilder Html = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RequestInt32("columnId") > 0)
                {
                    Column = ColumnService.Instance.SingleOrDefault<Components.Model.Column>(RequestInt32("columnId"));
                    Page<ColumnField> fields = ColumnFieldService.Instance.Page(1, 10000,
                                                                                new Dictionary<string, string> { { "columnId", Column.Id.ToString(CultureInfo.InvariantCulture) } },
                                                                                " Order by Sort ASC ");
                    if (RequestString("action", "add") == "add")
                    {
                        #region 添加

                        foreach (ColumnField field in fields.Items)
                        {
                            RenderSingleControl(field, "");
                        }
                        #endregion
                    }
                    else
                    {
                        #region 修改

                        var module = ModuleService.Instance.SingleOrDefault<Components.Model.Module>(Column.ModuleId);
                        var sql = string.Format("SELECT * FROM {0} WHERE Id={1}", module.TableName, RequestInt32("Id"));
                        var dt = DbHelper.CurrentDb.Query(sql).Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            foreach (ColumnField field in fields.Items)
                            {
                                var fieldValue = dt.Rows[0][field.Name].ToStr();
                                RenderSingleControl(field, fieldValue);
                            }
                        }

                        #endregion
                    }
                }
            }
        }

        private void RenderSingleControl(ColumnField field, string fieldValue)
        {
            ColumnFieldOption option = ColumnFieldOptionService.Instance.GetByFieldId(field.Id);
            var control = new ControlContext(new TextBox(field, option));
            switch (field.Type)
            {
                case (int)ModuleFieldType.TextBox:
                    control = new ControlContext(new TextBox(field, option));
                    break;
                case (int)ModuleFieldType.TextArea:
                    control = new ControlContext(new TextArea(field, option));
                    break;
                case (int)ModuleFieldType.Editer:
                    control = new ControlContext(new Editer(field, option));
                    break;
            }
            string controlHtml = control.Render(fieldValue);

            Html.Append("<tr>" + Environment.NewLine);
            Html.Append("   <td class=\"panel-header td_left\">" + Environment.NewLine);
            Html.Append("   " + field.Note + "：" + Environment.NewLine);
            Html.Append("   </td>");
            Html.Append("   <td class=\"panel-header td_right\">" + Environment.NewLine);
            Html.Append("   " + controlHtml + Environment.NewLine);
            Html.Append("   </td>" + Environment.NewLine);
            Html.Append("</tr>" + Environment.NewLine);
        }
    }
}