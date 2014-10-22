//--------------------------------------------------------------------------------
// 文件描述：管理员（用户）Get请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HC.Ajax;
using HC.Foundation.Context.Principal;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxGet
{
    /// <summary>
    /// 管理员（用户）Get请求响应类
    /// </summary>
    public class AdminGetHandler : AjaxGetHandler
    {
        /// <summary>
        /// 取得管理员分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetPageData(string page, string pageSize)
        {
            try
            {
                Page<Administrator> pageData = AdministratorService.Instance.Page(page.ToInt(1), pageSize.ToInt32(10),
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
        /// 取得管理员分页列表
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
                    dic.Add("LoginName", name);
                }
                Page<Administrator> pageData = AdministratorService.Instance.Page(page.ToInt(1), pageSize.ToInt32(10),
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

        /// <summary>
        /// 取得单个部门下的管理员列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetRoleAdminPageData(string roleId, string name, string page, string pageSize)
        {
            try
            {
                var dic = new Dictionary<string, string>();
                var ids = new StringBuilder();
                DataTable dt = DbHelper.CurrentDb.Query("SELECT DISTINCT AdminId FROM HC_RoleMember WHERE RoleId IN(@0) ",
                    roleId.ToInt()).Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    ids.Append(row["AdminId"] + ",");
                }
                if (name != "*")
                {
                    dic.Add("LoginName", name);
                }
                if (ids.ToString().IsNotEmpty())
                {
                    dic.Add("ids", ids.ToString());
                }
                else
                {
                    dic.Add("ids", "-1");
                }
                Page<Administrator> pageData = AdministratorService.Instance.Page(page.ToInt(1), pageSize.ToInt32(10),
                                                                                  dic, " Order by Sort ASC ");
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