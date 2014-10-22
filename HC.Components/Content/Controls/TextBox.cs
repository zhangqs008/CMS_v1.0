using HC.Components.Model;
using HC.Framework.Extension;

namespace HC.Components.Content.Controls
{
    public class TextBox : Control
    {
        public TextBox(ColumnField field, ColumnFieldOption option)
            : base(field, option)
        {
        }

        public override string Render(string val)
        {
            if (Field != null)
            {
                val = val.IsEmpty() ? Field.DefaultValue : val;
                var html = string.Format("<input type=\"text\" class=\"easyui-textbox\" id=\"txt{0}\" name=\"{0}\" value='{1}'", Field.Name, val);

                html += " data-options=\"";
                if (!FieldOption.TextBoxAllowNull)
                {
                    html += string.Format("required:true,missingMessage:'{0}不能为空',", Field.Note);
                }

                if (FieldOption.TextBoxRegex.IsNotEmpty())
                {
                    html += string.Format("validType:'{0}',", FieldOption.TextBoxRegex);
                }

                if (FieldOption.TextBoxLength > 0)
                {
                    html += string.Format("width:{0},", FieldOption.TextBoxLength);
                }
                html = html.TrimEnd(',');
                html += "\">";
                html += "</input>";
                html += string.Format("<span class='fieldTips'>{0}</span>", Field.Tips);
                return html;
            }
            return string.Empty;
        }
    }
}