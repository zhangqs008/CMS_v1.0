using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxGet
{
    public class ContentGetHandler : AjaxGetHandler
    {
        public string GetPageData(string columnId, string title, string page, string pageSize)
        {
            try
            {
                var dic = new Dictionary<string, string>();
                if (title != "*")
                {
                    dic.Add("title", title);
                }
                var column = ColumnService.Instance.SingleOrDefault<Column>(columnId.ToInt());
                var module = ModuleService.Instance.SingleOrDefault<Module>(column.ModuleId);
                var tableName = module.TableName;

                var sql = string.Format("SELECT * FROM {0} WHERE ColumnId={1} AND IsDel=0 ORDER BY Sort DESC,CreateDate Desc", tableName, columnId);
                var dt = DbHelper.CurrentDb.Query(sql).Tables[0];
                var data = dt.ToJson();
                data = ProcressJson(data);
                  
                var countSql = string.Format("SELECT Count(*) FROM {0} WHERE ColumnId={1} AND IsDel=0", tableName, columnId);
                var total = DbHelper.CurrentDb.ExecuteScalar<object>(countSql).ToInt();

                string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
                return json;
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return ex.Message;
            }
        }

    }
}
