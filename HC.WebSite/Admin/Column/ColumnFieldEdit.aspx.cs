//--------------------------------------------------------------------------------
// 文件描述：模型字段添加、编辑页面
// 文件作者：张清山
// 创建日期：2014-08-30 15:27:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Globalization;
using HC.Components.Model;
using HC.Components.Service;
using HC.Enumerations;
using HC.Enumerations.Content;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Column
{
    public partial class ColumnFieldEdit : AdminPage
    {
        protected ColumnField Field = new ColumnField();
        protected ColumnFieldOption FieldOption = new ColumnFieldOption();
        protected Components.Model.Module Module = new Components.Model.Module();
        protected Components.Model.Column Column = new Components.Model.Column();
        protected string ModuleFieldTypes = string.Empty; 

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestString("action");
            int moduleId = RequestInt32("moduleId");
            if (!IsPostBack)
            {
                #region 字段基本信息

                Module = ModuleService.Instance.SingleOrDefault<Components.Model.Module>(moduleId);
                if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    int id = RequestInt32("Id", 0);
                    Field = ColumnFieldService.Instance.SingleOrDefault<ColumnField>(id) ?? new ColumnField();
                    Column = ColumnService.Instance.SingleOrDefault<Components.Model.Column>(Field.ColumnId);
                }

                #endregion

                #region 字段配置信息

                if (Field.Id > 0)
                {
                    FieldOption = ColumnFieldOptionService.Instance.GetByFieldId(Field.Id) ?? new ColumnFieldOption(); 
                }

                #endregion

                #region 字段类型

                ModuleFieldTypes = "";
                foreach (int code in Enum.GetValues(typeof(ModuleFieldType)))
                {
                    string name = ((ModuleFieldType)code).GetResource(); //获取名称           
                    string value = code.ToString(CultureInfo.InvariantCulture); //获取值 

                    var check = (Field.Type == code) ? " checked='checked' " : "";
                    ModuleFieldTypes += string.Format("<option value='{0}' {2}>{1}</option>", value, name, check) +
                                        Environment.NewLine;
                }

                #endregion
            }
        }

        protected string GetFieldStyle(int type)
        {
            string action = RequestString("action");
            if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                return Field.Type == type ? "" : "display:none";
            }
            return "display:none";
        }
    }
}