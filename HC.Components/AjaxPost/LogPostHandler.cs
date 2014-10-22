//--------------------------------------------------------------------------------
// 文件描述：日志Post请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using HC.Ajax;
using HC.Foundation;
using HC.Foundation.Context.Principal;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxPost
{
    /// <summary>
    /// 日志Post请求响应类
    /// </summary>
    public class LogPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     删除日志
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
                    string ids = GetNodeInnerText(xmldoc, "ids").ToStr();
                    bool flag = LogService.Instance.Execute("DELETE FROM HC_Log WHERE Id IN(" + ids.TrimEnd(',') + ")") >= 0;
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
        ///     清除日志
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Clear(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    LogService.Instance.Execute("DELETE FROM HC_Log WHERE CreateDate <getdate()-7");
                    result.Add("body", "ok");
                    result.Add("status", "true");
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