//--------------------------------------------------------------------------------
// 文件描述：栏目字段表单提交处理类
// 文件作者：张清山
// 创建日期：2014-09-06 13:55:54
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Enumerations;
using HC.Enumerations.Content;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxGet
{
    public class ColumnFieldGetHandler : AjaxGetHandler
    {
        /// <summary>
        /// 取得分页列表
        /// </summary>
        /// <param name="columnId"> </param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetSearchPageData(string columnId,string name, string page, string pageSize)
        {
            try
            {
                var dic = new Dictionary<string, string>();
                if (name != "*")
                {
                    dic.Add("Name", name);
                }
                if(columnId.ToInt()>0)
                {
                    dic.Add("ColumnId", columnId);
                }
                Page<Model.ColumnField> pageData = ColumnFieldService.Instance.Page(page.ToInt(1), pageSize.ToInt32(10),
                                                                            dic,
                                                                            " Order by Sort ASC ");
                foreach (ColumnField field in pageData.Items)
                {
                    field.TypeName = ((ModuleFieldType)field.Type).GetResource();
                }
                long total = pageData.TotalItems;
                string data = pageData.Items.ToJson();
                data = ProcressJson(data);
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