using System;
using System.Collections.Generic;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Components.AjaxGet
{
    public class MenuGetHandler : AjaxGetHandler
    {
        public static string InitTree()
        {
            string tree;
            try
            {
                return MenuService.Instance.GetJqueryEasyUiTree();
            }
            catch (Exception ex)
            {
                tree = ex.Message;
            }
            return tree;
        }

        /// <summary>
        /// 初始化单个管理员权限树
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static string InitAdminMenuTree(string adminId)
        {
            try
            {
                return MenuService.Instance.InitAdminMenuTree(adminId.ToInt());
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 初始化单个角色权限树
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public static string InitRoleMenuTree(string roleid)
        {
            try
            {
                return MenuService.Instance.InitRoleMenuTree(roleid.ToInt());
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 后台管理首页，生成导航栏目树数据源Json
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string InitTreeByParentId(string pid)
        {
            string newLine =   Environment.NewLine;
            const string span = " ";
            string tree;
            try
            {
                string html = "{" + newLine;
                List<Menu> firsts = MenuService.Instance.GetListByParentId(1);
                foreach (Menu menu in firsts)
                {
                    html = html.TrimEnd(',');
                    menu.Ico = menu.Ico ?? "default.png";
                    menu.Ico = menu.Ico.Contains(".") ? menu.Ico.Substring(0, menu.Ico.LastIndexOf('.')) : menu.Ico;
                    html += menu.Name + ":[";

                    string temp = "";
                    List<Menu> seconds = MenuService.Instance.GetListByParentId(menu.Id);
                    foreach (Menu item in seconds)
                    {
                        item.Ico = item.Ico ?? "default.png";
                        item.Ico = item.Ico.Contains(".") ? item.Ico.Substring(0, item.Ico.LastIndexOf('.')) : item.Ico;
                        temp += "{" + newLine;
                        temp += string.Format("{0}\"menuid\":\"{1}\",{2}", span, item.Id, newLine);
                        temp += string.Format("{0}\"icon\":\"{1}\",{2}", span, item.Ico, newLine);
                        temp += string.Format("{0}\"menuname\":\"{1}\",{2}", span, item.Name, newLine);

                        List<Menu> thirds = MenuService.Instance.GetListByParentId(item.Id);
                        foreach (Menu info in thirds)
                        {
                            info.Ico = info.Ico ?? "default.png";
                            info.Ico = info.Ico.Contains(".")
                                           ? info.Ico.Substring(0, info.Ico.LastIndexOf('.'))
                                           : info.Ico;
                        }
                        temp += string.Format("{0}\"menus\":{1}{2}", span, ProcressJson(thirds.ToJson()), newLine);
                        temp += "}," + newLine;
                    }
                    html += (temp.Contains(",") ? temp.Substring(0, temp.LastIndexOf(',')) : temp) + "]," + newLine;
                }
                html = html.TrimEnd(',') + "}" + newLine;
                tree = html;
            }
            catch (Exception ex)
            {
                tree = ex.Message;
            }
            return tree;
        }
    }
}