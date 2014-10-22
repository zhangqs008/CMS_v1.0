//--------------------------------------------------------------------------------
// 文件描述：内容模型表单提交处理类
// 文件作者：张清山
// 创建日期：2014-08-30 12:13:23
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using HC.Ajax;
using HC.Components.Service;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxGet
{
    public class ModuleGetHandler : AjaxGetHandler
    {
        /// <summary>
        /// 取得分页列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetSearchPageData(string name, string page, string pageSize)
        {
            try
            {
                var dic = new Dictionary<string, string>();
                if (name != "*")
                {
                    dic.Add("Name", name);
                }
                Page<Model.Module> pageData = ModuleService.Instance.Page(page.ToInt(1), pageSize.ToInt32(10),
                                                                            dic,
                                                                            " Order by CREATEDATE DESC ");
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