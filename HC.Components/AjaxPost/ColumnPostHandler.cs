using System.Collections.Generic;
using System.Xml;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxPost
{
    public class ColumnPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///   排序
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Sort(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                int pId = GetNodeInnerText(xmldoc, "pid").ToInt();
                string sorts = GetNodeInnerText(xmldoc, "sorts");
                if (!sorts.IsEmpty())
                {
                    bool json = ColumnService.Instance.Sort(pId, sorts);
                    SiteCache.Remove(SiteCacheKey.MenuTree);
                    result.Add("body", json ? "ok" : "err");
                }
            }
            else
            {
                result.Add("status", "false");
                result.Add("body", "用户尚未登录，操作被拒绝");
            }
            return result;
        }

        /// <summary>
        ///     删除 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Delete(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                string id = GetNodeInnerText(xmldoc, "id");
                List<Column> instance = ColumnService.Instance.GetChildrenCategoriesRrecusive(id.ToInt());
                int count = 0;
                foreach (Column category in instance)
                {
                    ColumnService.Instance.Delete(category);
                    LogService.Instance.Log(string.Format("用户 {0} 删除栏目：{1}", CurrentUser, category.Name),
                                            string.Format("用户 {0} 删除栏目：{1}", CurrentUser, category.Name),
                                            LogCategory.Menu);
                    count++;
                }
                result.Add("body", count == instance.Count ? "ok" : "err");
                result.Add("status", count == instance.Count ? "true" : "false");
            }
            else
            {
                result.Add("status", "false");
                result.Add("body", "用户尚未登录，操作被拒绝");
            }
            return result;
        }

    }
}