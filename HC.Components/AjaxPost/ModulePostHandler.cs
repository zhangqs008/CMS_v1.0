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
using HC.Components.Service;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxPost
{
    public class ModulePostHandler : AjaxPostHandler
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
                    string id = GetNodeInnerText(xmldoc, "id");
                    bool flag = ModuleService.Delete(id.ToInt());
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