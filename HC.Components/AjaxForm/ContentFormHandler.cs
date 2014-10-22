using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Enumerations.Content;
using HC.Foundation;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxForm
{
    public class ContentFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["Id"].ToInt(0); //Id
                    int columnId = HttpContext.Current.Request.Form["columnId"].ToInt();
                    var column = ColumnService.Instance.SingleOrDefault<Column>(columnId);
                    var module = ModuleService.Instance.SingleOrDefault<Module>(column.ModuleId);

                    string tableName = module.TableName;
                    Page<ColumnField> fields = ColumnFieldService.Instance.Page(1, 10000,
                                                                                   new Dictionary<string, string>
                                                                                        {
                                                                                            {
                                                                                                "columnId",
                                                                                                columnId.ToString(
                                                                                                    CultureInfo.
                                                                                                        InvariantCulture)
                                                                                            }
                                                                                        },
                                                                                   " Order by Sort ASC ");

                    if (id == 0)
                    {
                        #region 添加

                        string sql = string.Format("INSERT INTO {0} (", tableName); 
                        var dic = new Dictionary<string, string>
                                      {
                                          {"ColumnId", columnId.ToString(CultureInfo.InvariantCulture)},
                                          {"IsDel", "0"},
                                          {"Sort", "0"},
                                          {"Status", "0"},
                                          {"CreateDate", "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"},
                                          {"CreateUser", "'" + HCContext.Current.Admin.LoginName + "'"},
                                          {"UpdateDate", "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"},
                                          {"UpdateUser", "'" + HCContext.Current.Admin.LoginName + "'"}
                                      };
                        foreach (ColumnField field in fields.Items)
                        {
                            string val = HttpContext.Current.Request.Form[field.Name].ToStr();
                            switch (field.Type)
                            {
                                case (int)ModuleFieldType.TextArea:
                                case (int)ModuleFieldType.TextBox:
                                case (int)ModuleFieldType.Radio:
                                case (int)ModuleFieldType.Select:
                                case (int)ModuleFieldType.File:
                                case (int)ModuleFieldType.CheckBox:
                                case (int)ModuleFieldType.DataPicker:
                                    val = string.Format("'{0}'", val);
                                    break;
                                case (int)ModuleFieldType.Editer:
                                    val = string.Format("'{0}'", HttpUtility.UrlDecode(val));
                                    break;
                            }
                            if (!dic.ContainsKey(field.Name))
                            {
                                dic.Add(field.Name, val);
                            }
                        }
                        sql = dic.Aggregate(sql, (current, pair) => current + (pair.Key + ","));
                        sql = sql.TrimEnd(',') + ")VALUES(";
                        sql = dic.Aggregate(sql, (current, pair) => current + (pair.Value + ","));
                        sql = sql.TrimEnd(',') + ")";
                        DbHelper.CurrentDb.Execute(sql);
                        return "true";

                        #endregion

                    }
                    else
                    {
                        #region 修改

                        var dic = new Dictionary<string, string>();
                        foreach (ColumnField field in fields.Items)
                        {
                            string val = HttpContext.Current.Request.Form[field.Name].ToStr();
                            switch (field.Type)
                            {
                                case (int)ModuleFieldType.TextArea:
                                case (int)ModuleFieldType.TextBox:
                                case (int)ModuleFieldType.Radio:
                                case (int)ModuleFieldType.Select:
                                case (int)ModuleFieldType.File:
                                case (int)ModuleFieldType.CheckBox:
                                case (int)ModuleFieldType.DataPicker:
                                    val = string.Format("'{0}'", val);
                                    break;
                                case (int)ModuleFieldType.Editer:
                                    val = string.Format("'{0}'", HttpUtility.UrlDecode(val));
                                    break;
                            }
                            if (!dic.ContainsKey(field.Name))
                            {
                                dic.Add(field.Name, val);
                            }
                        }
                        var temp = "";
                        foreach (KeyValuePair<string, string> pair in dic)
                        {
                            temp += pair.Key + "=" + pair.Value + ",";
                        }
                        temp = temp.TrimEnd(',');

                        var sql = string.Format("UPDATE {0} SET {1} WHERE Id={2}", tableName, temp, id);
                        DbHelper.CurrentDb.Execute(sql);
                        return "true";

                        #endregion
                    }
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