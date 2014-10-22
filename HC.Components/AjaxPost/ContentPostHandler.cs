using System.Collections.Generic;
using System.Xml;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxPost
{
    public class ContentPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     删除 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Delete(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                int id = GetNodeInnerText(xmldoc, "id").ToInt();
                int columnId = GetNodeInnerText(xmldoc, "columnId").ToInt();

                var column = ColumnService.Instance.SingleOrDefault<Column>(columnId);
                var module = ModuleService.Instance.SingleOrDefault<Module>(column.ModuleId);
                string tableName = module.TableName;
                string sql = string.Format("DELETE FROM {0} WHERE Id={1}", tableName, id);

                bool flag = DbHelper.CurrentDb.Execute(sql) >= 0;
                result.Add("body", flag ? "ok" : "err");
                result.Add("status", flag ? "true" : "false");
            }
            else
            {
                result.Add("status", "false");
                result.Add("body", "用户尚未登录，操作被拒绝");
            }
            return result;
        }
    }
}