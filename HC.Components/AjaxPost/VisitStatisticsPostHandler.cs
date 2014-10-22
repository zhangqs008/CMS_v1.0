//--------------------------------------------------------------------------------
// 文件描述：访问记录Post请求响应类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxPost
{
    /// <summary>
    ///     访问记录Post请求响应类
    /// </summary>
    public class VisitStatisticsPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     添加访问记录
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Add(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            try
            {
                string path = GetNodeInnerText(xmldoc, "path");
                IpDetail ipInfo = IPHelper.GetIpDetail(IPHelper.GetClientIP());
                VisitStatistics statistics = ModelFactory<VisitStatistics>.Insten();
                statistics.IP = IPHelper.GetClientIP();
                statistics.Broswer = HttpContext.Current.Request.Browser.Browser + "(" +
                                     HttpContext.Current.Request.Browser.Version + ")";
                statistics.CreateDate = DateTime.Now;
                statistics.Url = path;
                statistics.City = ipInfo == null ? "" : ipInfo.city;
                statistics.CreateUser = HCContext.Current.Admin.Identity.IsAuthenticated
                                            ? HCContext.Current.Admin.Identity.Name
                                            : "游客";
                if (statistics.IP != "127.0.0.1")
                {
                    VisitStatisticsService.Instance.Insert(statistics);
                }
                result.Add("body", "ok");
            }
            catch (Exception ex)
            {
                result.Add("body", ex.Message);
            }
            return result;
        }


        /// <summary>
        ///     删除访问记录
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
                    var ids = GetNodeInnerText(xmldoc, "ids").ToStr();
                    bool flag =
                        VisitStatisticsService.Instance.Execute("DELETE FROM HC_VisitStatistics WHERE Id IN(" + ids.TrimEnd(',') + ")") >= 0;
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
        ///     清除访问记录
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
                    VisitStatisticsService.Instance.Execute("DELETE FROM HC_VisitStatistics WHERE CreateDate <getdate()-7");
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