//--------------------------------------------------------------------------------
// 文件描述：模型字段Post处理类
// 文件作者：张清山
// 创建日期：2014-08-30 15:18:42
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
    public class ModuleFieldPostHandler : AjaxPostHandler
    {
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
                    int id = GetNodeInnerText(xmldoc, "id").ToInt();
                    var field = ModuleFieldService.Instance.SingleOrDefault<ModuleField>(id);

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
                    checkSql = checkSql.FormatWith(field.TableName, field.Name);
                    int count = DbHelper.CurrentDb.ExecuteScalar<object>(checkSql).ToInt();
                    if (count > 0)
                    {
                        var sql = "ALTER TABLE {0} DROP COLUMN {1}".FormatWith(field.TableName, field.Name);
                        DbHelper.CurrentDb.Execute(sql);
                    }

                    bool flag = ModuleFieldService.Delete(id.ToInt());
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
                    bool flag = ModuleFieldService.Instance.Sort(sorts);
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