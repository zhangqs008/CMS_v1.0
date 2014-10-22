//--------------------------------------------------------------------------------
// 文件描述：模型字段表单提交处理类
// 文件作者：张清山
// 创建日期：2014-08-30 15:16:00
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Web;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Enumerations.Content;
using HC.Foundation;
using HC.Framework.Extension;
using HC.Framework.Helper;
using HC.Repository;

namespace HC.Components.AjaxForm
{
    public class ModuleFieldFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["Id"].ToInt(); //Id
                    int moduleId = HttpContext.Current.Request.Form["ModuleId"].ToInt(); //ModuleId
                    string name = HttpContext.Current.Request.Form["Name"].ToStr(); //字段名称
                    string note = HttpContext.Current.Request.Form["Note"].ToStr(); //字段注释
                    string tips = HttpContext.Current.Request.Form["Tips"].ToStr(); //字段提示信息
                    string defaultValue = HttpContext.Current.Request.Form["DefaultValue"].ToStr(); //默认值
                     
                    int type = HttpContext.Current.Request.Form["Type"].ToInt(); //字段类型（枚举）

                    name = CamelCase(name.Replace(" ", ""));
                    if (ChineseHelper.GetChineseString(name).Count > 0)
                    {
                        return "对不起，字段名不能含中文字符";
                    }

                    ModuleField instance = ModuleFieldService.Instance.SingleOrDefault<ModuleField>(id) ??
                                           ModelFactory<ModuleField>.Insten();

                    instance.Name = name;
                    instance.Note = note;
                    instance.Type = type;
                    instance.Tips = tips;
                    instance.DefaultValue = defaultValue;
                    instance.ModuleId = moduleId;

                    #region  创建字段

                    var module = ModuleService.Instance.SingleOrDefault<Module>(instance.ModuleId);
                    string checkSql = @"SELECT COUNT(*)
                                            FROM   sys.columns C
                                                   INNER JOIN sys.objects O
                                                        ON  C.[object_id] = O.[object_id]
                                                        AND O.type = 'U'
                                                        AND O.is_ms_shipped = 0
                                                   INNER JOIN sys.tables AS ta
                                                        ON  c.object_id = ta.object_id
                                            WHERE  ta.[name] = '{0}'
                                                   AND c.[name] = '{1}'";
                    instance.TableName = module.TableName;

                    checkSql = checkSql.FormatWith(module.TableName, name);
                    int count = DbHelper.CurrentDb.ExecuteScalar<object>(checkSql).ToInt();
                    if (instance.Id == 0)
                    {
                        if (count > 0)
                        {
                            return string.Format("对不起，表：{0}已存在字段：{1}", module.TableName, name);
                        }
                        ProcessField(name, instance, module);
                    }
                    else
                    {
                        if (count == 0)
                        {
                            ProcessField(name, instance, module);
                        }
                    }

                    #endregion

                    #region 字段配置

                    int textBoxLength = HttpContext.Current.Request.Form["TextBoxLength"].ToInt(); //单行文本-长度
                    var textBoxAllowNull = HttpContext.Current.Request.Form["TextBoxAllowNull"].ToBoolean(); //单行文本-是否允许为空
                    string textBoxRegex = HttpContext.Current.Request.Form["TextBoxRegex"].ToStr(); //单行文本-验证规则

                    int textAreaWidth = HttpContext.Current.Request.Form["TextAreaWidth"].ToInt(); //多行文本-宽度
                    int textAreaHeight = HttpContext.Current.Request.Form["TextAreaHeight"].ToInt(); //多行文本-高度
                    var textAreaAllowNull = HttpContext.Current.Request.Form["TextAreaAllowNull"].ToBoolean(); //多行文本-是否允许为空

                    string radioText = HttpContext.Current.Request.Form["RadioText"].ToStr(); //单选按钮-文本
                    string radioValue = HttpContext.Current.Request.Form["RadioValue"].ToStr(); //单选按钮-值

                    string checkBoxText = HttpContext.Current.Request.Form["CheckBoxText"].ToStr(); //复选框-值
                    string checkBoxValue = HttpContext.Current.Request.Form["CheckBoxValue"].ToStr(); //复选框-值


                    int editerWidth = HttpContext.Current.Request.Form["EditerWidth"].ToInt(); //编辑器-宽度
                    int editerHeight = HttpContext.Current.Request.Form["EditerHeight"].ToInt(); //编辑器-高度
                    string editerStyle = HttpContext.Current.Request.Form["EditerStyle"].ToStr(); //编辑器-模式
                     
                    ModuleFieldOption fieldOption = ModuleFieldOptionService.Instance.GetByFieldId(instance.Id) ??
                                                    ModelFactory<ModuleFieldOption>.Insten();

                    fieldOption.FieldId = instance.Id;
                    fieldOption.ModuleId = instance.ModuleId;

                    #region 单行文本

                    fieldOption.TextBoxLength = textBoxLength;
                    fieldOption.TextBoxAllowNull = textBoxAllowNull;
                    fieldOption.TextBoxRegex = textBoxRegex;

                    #endregion

                    #region 多行文本

                    fieldOption.TextAreaWidth = textAreaWidth;
                    fieldOption.TextAreaHeight = textAreaHeight;
                    fieldOption.TextAreaAllowNull = textAreaAllowNull;

                    #endregion

                    #region 单选按钮

                    fieldOption.RadioText = radioText;
                    fieldOption.RadioValue = radioValue;

                    #endregion

                    #region 多选按钮

                    fieldOption.CheckBoxText = checkBoxText;
                    fieldOption.CheckBoxValue = checkBoxValue;

                    #endregion

                    #region 编辑器

                    fieldOption.EditerWidth = editerWidth;
                    fieldOption.EditerHeight = editerHeight;
                    fieldOption.EditerStyle = editerStyle;

                    #endregion

                    if (fieldOption.Id > 0)
                    {
                        ModuleFieldOptionService.Instance.Modify(fieldOption);
                    }
                    else
                    {
                        ModuleFieldOptionService.Instance.Add(fieldOption);
                    }

                    #endregion

                    if (id.ToInt() > 0)
                    {
                        ModuleFieldService.Instance.Modify(instance);
                    }
                    else
                    {
                        ModuleFieldService.Instance.Add(instance);
                    }
                    return "true";
                }
                return "用户尚未登录，操作被拒绝";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static void ProcessField(string name, ModuleField instance, Module module)
        {
            string sql = "";
            switch (instance.Type)
            {
                case (int)ModuleFieldType.TextBox:
                    sql = string.Format("ALTER TABLE {0} ADD [{1}]  [nvarchar] (4000) NULL", module.TableName, name);
                    break;
                case (int)ModuleFieldType.TextArea:
                case (int)ModuleFieldType.Editer:
                    sql = string.Format("ALTER TABLE {0} ADD [{1}]  [TEXT] NULL", module.TableName, name);
                    break;
                case (int)ModuleFieldType.Radio:
                case (int)ModuleFieldType.Select:
                case (int)ModuleFieldType.CheckBox:
                    sql = string.Format("ALTER TABLE {0} ADD [{1}]  [nvarchar] (4000) NULL", module.TableName, name);
                    break;
                case (int)ModuleFieldType.DataPicker:
                    sql = string.Format("ALTER TABLE {0} ADD [{1}]  [datetime]  NULL", module.TableName, name);
                    break;
            }
            if (sql.IsNotEmpty())
            {
                DbHelper.CurrentDb.Execute(sql);
            }
        }

        /// <summary>
        ///     [辅助方法]驼峰命名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string CamelCase(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = input.ToCharArray()[0].ToString(CultureInfo.InvariantCulture).ToUpper() + input.Substring(1);
                return input;
            }
            return string.Empty;
        }
    }
}