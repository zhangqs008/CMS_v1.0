//--------------------------------------------------------------------------------
// 文件描述：栏目字段Post处理类
// 文件作者：张清山
// 创建日期：2014-09-06 13:56:18
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxPost
{
    public class ColumnFieldPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     添加 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Add(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    try
                    {
                        string ids = GetNodeInnerText(xmldoc, "ids");
                        int columnId = GetNodeInnerText(xmldoc, "columnId").ToInt();
                        ids = ids.TrimEnd(',');
                        string[] idArr = ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string s in idArr)
                        {
                            int id = s.ToInt();
                            var moduleField = ModuleFieldService.Instance.SingleOrDefault<ModuleField>(id);
                            ModuleFieldOption moduleFieldOption =
                                ModuleFieldOptionService.Instance.GetByFieldId(moduleField.Id);
                            int exsit = DbHelper.CurrentDb.ExecuteScalar<object>(
                                "SELECT COUNT(*) FROM HC_ColumnField hcf WHERE hcf.ColumnId=@0 AND FieldId=@1",
                                columnId, id).ToInt();
                            if (exsit <= 0)
                            {
                                #region 字段基本信息

                                ColumnField field = ModelFactory<ColumnField>.Insten();
                                field.ColumnId = columnId;
                                field.FieldId = id;
                                field.ModuleId = moduleField.ModuleId;
                                field.Name = moduleField.Name;
                                field.Note = moduleField.Note;
                                field.TableName = moduleField.TableName;
                                field.Type = moduleField.Type;
                                field.Tips = moduleField.Tips;
                                field.DefaultValue = moduleField.DefaultValue;
                                ColumnFieldService.Instance.Add(field);

                                #endregion

                                #region 字段配置信息

                                ColumnFieldOption option = ModelFactory<ColumnFieldOption>.Insten();
                                option.ColumnId = columnId; //字段配置栏目Id
                                option.FieldId = field.Id; //栏目字段Id

                                option.ModuleId = moduleFieldOption.ModuleId;
                                option.TextBoxLength = moduleFieldOption.TextBoxLength;
                                option.TextBoxAllowNull = moduleFieldOption.TextBoxAllowNull;
                                option.TextBoxRegex = moduleFieldOption.TextBoxRegex;
                                option.TextAreaWidth = moduleFieldOption.TextAreaWidth;
                                option.TextAreaHeight = moduleFieldOption.TextAreaHeight;
                                option.TextAreaAllowNull = moduleFieldOption.TextAreaAllowNull;
                                option.RadioText = moduleFieldOption.RadioText;
                                option.RadioValue = moduleFieldOption.RadioValue;
                                option.CheckBoxText = moduleFieldOption.CheckBoxText;
                                option.CheckBoxValue = moduleFieldOption.CheckBoxValue;
                                option.EditerHeight = moduleFieldOption.EditerHeight;
                                option.EditerWidth = moduleFieldOption.EditerWidth;
                                option.EditerStyle = moduleFieldOption.EditerStyle;

                                ColumnFieldOptionService.Instance.Add(option);

                                #endregion
                            }
                        } 
                        result.Add("body", "ok");
                        result.Add("status", "true");
                    }
                    catch (Exception ex)
                    {
                        result.Add("status", "false");
                        result.Add("body", ex.Message);
                        LogService.Instance.LogException(ex);
                    }
                }
                else
                {
                    result.Add("status", "false");
                    result.Add("body", "用户尚未登录，操作被拒绝");
                }
            }
            catch (Exception ex)
            {
                result.Add("body", ex.Message);
                LogService.Instance.LogException(ex);
            }
            return result;
        }

        /// <summary>
        ///     删除 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Delete(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    try
                    {
                        string ids = GetNodeInnerText(xmldoc, "ids");
                        ids = ids.TrimEnd(',');
                        var idArr = ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(id => id.ToInt()).ToList();

                        DbHelper.CurrentDb.Execute("DELETE FROM HC_ColumnField WHERE Id IN(@0)", idArr.ToArray());
                        DbHelper.CurrentDb.Execute("DELETE FROM HC_ColumnFieldOption WHERE FieldId IN(@0)", idArr.ToArray());

                        result.Add("body", "ok");
                        result.Add("status", "true");
                    }
                    catch (Exception ex)
                    {
                        result.Add("status", "false");
                        result.Add("body", ex.Message);
                        LogService.Instance.LogException(ex);
                    }
                }
                else
                {
                    result.Add("status", "false");
                    result.Add("body", "用户尚未登录，操作被拒绝");
                }
            }
            catch (Exception ex)
            {
                result.Add("body", ex.Message);
                LogService.Instance.LogException(ex);
            }
            return result;
        }

        /// <summary>
        ///    排序 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Sort(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    string sorts = GetNodeInnerText(xmldoc, "sorts");
                    bool flag = ColumnFieldService.Instance.Sort(sorts);
                    result.Add("body", flag ? "ok" : "err");
                    result.Add("status", flag ? "true" : "false");
                }
                else
                {
                    result.Add("status", "false");
                    result.Add("body", "用户尚未登录，操作被拒绝");
                }
            }
            catch (Exception ex)
            {
                result.Add("body", ex.Message);
                LogService.Instance.LogException(ex);
            }
            return result;
        }
    }
}