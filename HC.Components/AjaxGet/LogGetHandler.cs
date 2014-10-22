//--------------------------------------------------------------------------------
// 文件描述：日志Get请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using HC.Ajax;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxGet
{
    /// <summary>
    ///     日志Get请求响应类
    /// </summary>
    public class LogGetHandler : AjaxGetHandler
    {
        /// <summary>
        ///     取得分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetPageData(string page, string pageSize)
        {
            try
            {
                Page<Log> pageData = LogService.Instance.Page(page.ToInt(1), pageSize.ToInt32(10),
                                                              new Dictionary<string, string>(),
                                                              " Order by Sort ASC ");
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

        /// <summary>
        ///     取得分页列表
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
                    dic.Add("Title", name);
                }
                Page<Log> pageData = LogService.Instance.Page(page.ToInt(1), pageSize.ToInt32(10),
                                                              dic,
                                                              " Order by CreateDate DESC ");
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