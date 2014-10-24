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

namespace HC.WebSite.Admin.Module
{
    public partial class ModuleFieldEdit : AdminPage
    {
        protected ModuleField Field = new ModuleField();
        protected ModuleFieldOption FieldOption = new ModuleFieldOption();
        protected Components.Model.Module Module = new Components.Model.Module();
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
                    Field = ModuleFieldService.Instance.SingleOrDefault<ModuleField>(id) ?? new ModuleField();
                }

                #endregion

                #region 字段配置信息

                if (Field.Id > 0)
                {
                    FieldOption = ModuleFieldOptionService.Instance.GetByFieldId(Field.Id) ?? new ModuleFieldOption();
                }

                #endregion

                #region 字段类型

                ModuleFieldTypes = "";
                foreach (int code in Enum.GetValues(typeof (ModuleFieldType)))
                {
                    string name = ((ModuleFieldType) code).GetResource(); //获取名称           
                    string value = code.ToString(CultureInfo.InvariantCulture); //获取值 
                    ModuleFieldTypes += string.Format("<option value='{0}'>{1}</option>", value, name) +
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