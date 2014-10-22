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
    public class MenuPostHandler : AjaxPostHandler
    {
        /// <summary>
        ///     菜单排序
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
                    bool json = MenuService.Instance.Sort(pId, sorts);
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
        ///     删除菜单
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Delete(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                string id = GetNodeInnerText(xmldoc, "id");
                List<Menu> instance = MenuService.Instance.GetChildrenCategoriesRrecusive(id.ToInt());
                int count = 0;
                foreach (Menu category in instance)
                {
                    MenuService.Instance.Delete(category);
                    LogService.Instance.Log(string.Format("用户 {0} 删除菜单：{1}", CurrentUser, category.Name),
                                            string.Format("用户 {0} 删除菜单：{1}", CurrentUser, category.Name),
                                            LogCategory.Menu);
                    count++;
                }
                SiteCache.Remove(SiteCacheKey.MenuTree);
                SiteCache.Remove(SiteCacheKey.CategorySingle + "_" + id);
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

        /// <summary>
        ///     取得后台左侧导航菜单
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLeftGuideMenu(XmlDocument xmldoc)
        {
            var result = new Dictionary<string, string>();
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                int pId = GetNodeInnerText(xmldoc, "menuId").ToInt();
                string json = MenuService.Instance.GetLeftGuideMenu(pId);
                SiteCache.Remove(SiteCacheKey.MenuTree);
                result.Add("body", json);
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