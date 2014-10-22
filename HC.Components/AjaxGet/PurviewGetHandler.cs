//--------------------------------------------------------------------------------
// 文件描述：系统权限Get请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Ajax;
using HC.Components.Service;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxGet
{
    /// <summary>
    /// 系统权限Get请求响应类
    /// </summary>
    public class PurviewGetHandler : AjaxGetHandler
    {
        /// <summary>
        /// 初始化权限树
        /// </summary>
        /// <returns></returns>
        public static string InitTree()
        {
            try
            {
                return PurviewService.Instance.GetJqueryEasyUiTree();
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 初始化单个管理员权限树
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static string InitAdminPurviewTree(string adminId)
        {
            try
            {
                return PurviewService.Instance.InitAdminPurviewTree(adminId.ToInt());
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 初始化单个部门权限树
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public static string InitDepartmentPurviewTree(string departmentId)
        {
            try
            {
                return PurviewService.Instance.InitDepartmentPurviewTree(departmentId.ToInt());
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return ex.Message;
            }
        }
    }
}