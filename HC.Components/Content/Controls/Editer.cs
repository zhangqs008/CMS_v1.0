using System.Text;
using HC.Components.Model;
using HC.Framework.Extension;

namespace HC.Components.Content.Controls
{
    public class Editer : Control
    {
        public Editer(ColumnField field, ColumnFieldOption option)
            : base(field, option)
        {
        }

        public override string Render(string val)
        {
            var html = new StringBuilder();

            html.Append(string.Format("<textarea name='editor_{2}' id='editor_{2}' style='width: {0}px; height:{1}px; visibility: hidden;'></textarea>", FieldOption.EditerWidth, FieldOption.EditerHeight, Field.Name)).Append(System.Environment.NewLine);
            html.Append(string.Format("<span class='fieldTips'>{0}</span>", Field.Tips)).Append(System.Environment.NewLine);

            //添加个隐藏的Textbox，用来保存编辑器的值
            html.Append(
                string.Format(
                    "<div style='display:none'> <input type='text' class='easyui-textbox' id='{0}' name='{0}' value=''></input></div>",
                    Field.Name)).Append(System.Environment.NewLine);


            html.Append("<script type='text/javascript'>").Append(System.Environment.NewLine);
            html.Append("    var editor_" + Field.Name + ";").Append(System.Environment.NewLine);
            html.Append("        //初始化编辑器").Append(System.Environment.NewLine);
            html.Append("        KindEditor.ready(function (k) {").Append(System.Environment.NewLine);
            html.AppendFormat("            editor_{0}= k.create('#editor_{0}', {{ ", Field.Name).Append(System.Environment.NewLine);
            html.Append("                uploadJson: '{0}FileManagerHandler.aspx',".FormatWith(BasePath)).Append(System.Environment.NewLine);
            html.Append("                fileManagerJson: '{0}UploadFilesHandler.aspx',".FormatWith(BasePath)).Append(System.Environment.NewLine);
            html.Append("                allowFileManager: true,").Append(System.Environment.NewLine);
            html.Append("                fileUploadLimit: 30,").Append(System.Environment.NewLine);
            if (FieldOption.EditerStyle.ToLower() == "simple")
            {
                html.Append("                items : ['fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline','removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist','insertunorderedlist', '|', 'emoticons', 'image', 'link'],");
            }
            html.Append("                afterChange: function () {").Append(System.Environment.NewLine);
            //编辑器的值需要进行编码
            html.AppendFormat("                   $(\"[name='{0}']\").val(encodeURIComponent(this.html()));", Field.Name).Append(System.Environment.NewLine);
            html.Append("                }").Append(System.Environment.NewLine);
            html.Append("            });").Append(System.Environment.NewLine);
            if (val.IsEmpty())
            {
                val = Field.DefaultValue;
            }
            val = val.Replace("\"", "&quot;");
            val = val.Replace("'", "&apos;");
            html.AppendFormat("                editor_{0}.html('{1}');", Field.Name, val).Append(System.Environment.NewLine);
            //编辑器的值需要进行编码
            html.AppendFormat("                $(\"[name='{0}']\").val(encodeURIComponent({1}));", Field.Name, val).Append(System.Environment.NewLine);
            html.Append("        });").Append(System.Environment.NewLine);
            html.Append(" </script>").Append(System.Environment.NewLine);
            return html.ToString();
        }
    }
}