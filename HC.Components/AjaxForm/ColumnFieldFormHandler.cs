//--------------------------------------------------------------------------------
// 文件描述：栏目字段表单提交处理类
// 文件作者：张清山
// 创建日期：2014-09-06 13:55:30
// 修改记录： 
//--------------------------------------------------------------------------------
using System;
using System.Web;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxForm
{
    public class ColumnFieldFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["Id"].ToInt(); //Id
                    int columnId = HttpContext.Current.Request.Form["ColumnId"].ToInt(); //栏目Id
                    int moduleId = HttpContext.Current.Request.Form["ModuleId"].ToInt(); //内容模型Id

                    string tableName = HttpContext.Current.Request.Form["TableName"].ToStr(); //模型表名
                    string name = HttpContext.Current.Request.Form["Name"].ToStr(); //字段名称
                    string note = HttpContext.Current.Request.Form["Note"].ToStr(); //字段注释
                    string tips = HttpContext.Current.Request.Form["Tips"].ToStr(); //字段提示信息
                    string defaultValue = HttpContext.Current.Request.Form["DefaultValue"].ToStr(); //默认值
                    int type = HttpContext.Current.Request.Form["Type"].ToInt(); //字段类型（枚举）


                    var instance = ColumnFieldService.Instance.SingleOrDefault<ColumnField>(id) ??
                                   ModelFactory<ColumnField>.Insten();

                    instance.Id = id;
                    instance.ColumnId = columnId;
                    instance.ModuleId = moduleId;
                    instance.TableName = tableName;
                    instance.Name = name;
                    instance.Note = note;
                    instance.Type = type;
                    instance.DefaultValue = defaultValue;
                    instance.Tips = tips;
                    instance.ModuleId = moduleId;


                    #region 字段配置

                    int textBoxLength = HttpContext.Current.Request.Form["TextBoxLength"].ToInt(); //单行文本-长度
                    var textBoxAllowNull = HttpContext.Current.Request.Form["TextBoxAllowNull"].ToBoolean(); //单行文本-是否允许为空
                    string textBoxRegex = HttpContext.Current.Request.Form["TextBoxRegex"].ToStr(); //单行文本-验证规则
                    int textAreaWidth = HttpContext.Current.Request.Form["TextAreaWidth"].ToInt(); //多行文本-宽度
                    int textAreaHeight = HttpContext.Current.Request.Form["TextAreaHeight"].ToInt(); //多行文本-高度
                    var textAreaAllowNull = HttpContext.Current.Request.Form["TextAreaAllowNull"].ToBoolean(); //单行文本-是否允许为空
                    string radioText = HttpContext.Current.Request.Form["RadioText"].ToStr(); //单选按钮-文本
                    string radioValue = HttpContext.Current.Request.Form["RadioValue"].ToStr(); //单选按钮-值
                    string checkBoxText = HttpContext.Current.Request.Form["CheckBoxText"].ToStr(); //复选框-值
                    string checkBoxValue = HttpContext.Current.Request.Form["CheckBoxValue"].ToStr(); //复选框-值 

                    int editerWidth = HttpContext.Current.Request.Form["EditerWidth"].ToInt(); //编辑器-宽度
                    int editerHeight = HttpContext.Current.Request.Form["EditerHeight"].ToInt(); //编辑器-高度
                    string editerStyle = HttpContext.Current.Request.Form["EditerStyle"].ToStr(); //编辑器-模式


                    ColumnFieldOption fieldOption = ColumnFieldOptionService.Instance.GetByFieldId(instance.Id) ??
                                                    ModelFactory<ColumnFieldOption>.Insten();

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
                        ColumnFieldOptionService.Instance.Modify(fieldOption);
                    }
                    else
                    {
                        ColumnFieldOptionService.Instance.Add(fieldOption);
                    }

                    #endregion

                    if (id.ToInt() > 0)
                    {
                        ColumnFieldService.Instance.Modify(instance);
                    }
                    else
                    {
                        ColumnFieldService.Instance.Add(instance);
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
    }
}