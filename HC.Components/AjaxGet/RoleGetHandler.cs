//--------------------------------------------------------------------------------
// 文件描述：角色Get请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Ajax;
using HC.Components.Service;
using HC.Foundation.Log;

namespace HC.Components.AjaxGet
{
    /// <summary>
    /// 角色Get请求响应类
    /// </summary>
    public class RoleGetHandler : AjaxGetHandler
    {
        public static string InitTree()
        {
            try
            {
                return RoleService.Instance.GetJqueryEasyUiTree();
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return ex.Message;
            }
        }
    }
}