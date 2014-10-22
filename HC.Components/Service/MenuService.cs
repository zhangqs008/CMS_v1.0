//--------------------------------------------------------------------------------
// 文件描述：系统菜单表服务类
// 文件作者：张清山
// 创建日期：2014-08-11 17:47:10
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HC.Components.Model;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.Service
{
    /// <summary>
    ///     系统菜单表服务类
    /// </summary>
    public class MenuService : ServiceBase<Menu>
    {
        private static MenuService _instance;
        private static readonly object SynObject = new object();

        private MenuService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static MenuService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new MenuService());
                }
            }
        }

        /// <summary>
        ///     添加系统菜单表
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(Menu instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 添加系统菜单-" + instance.Name, instance.CreateUser),
                                        string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        /// <summary>
        ///     获取分页数据
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="conditions">搜索条件</param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public Page<Menu> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
        {
            string sql = " WHERE IsDel=0 ";
            if (conditions.Count > 0)
            {
                foreach (var condition in conditions)
                {
                    switch (condition.Key)
                    {
                        default:
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND {0} like '%{1}%' ".FormatWith(condition.Key, condition.Value);
                            }
                            break;
                    }
                }
            }
            sql += orderby;
            return DbHelper.CurrentDb.Page<Menu>(pageIndex, pageSize, sql);
        }


        /// <summary>
        ///     修改系统菜单表
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(Menu instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 修改系统菜单-" + instance.Name, instance.CreateUser),
                                        string.Format("{0} ", instance.ToXml()));
                return true;
            }
            return false;
        }

        /// <summary>
        ///     删除系统菜单表实体
        /// </summary> 
        /// <param name="id">根据ID删除系统菜单表实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            const string sql = "DELETE FROM HC_Menu WHERE Id=@0";
            var menu = DbHelper.CurrentDb.SingleOrDefault<Menu>(id);

            LogService.Instance.Log(string.Format("{0} 删除系统菜单-" + menu.Name, HCContext.Current.Admin.LoginName),
                                    string.Format("{0}", menu.ToXml()));
            return DbHelper.CurrentDb.Execute(sql, id) > 0;
        }


        /// <summary>
        ///     批量删除系统菜单表实体
        /// </summary>
        /// <param name="ids">根据ID批量删除系统菜单表实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(string ids)
        {
            try
            {
                var idArr = ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in idArr)
                {
                    Delete(id.ToInt());
                }
                return true;
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return false;
            }
        }

        /// <summary>
        ///     获取栏目列表的下级栏目, 供下拉列表使用, 递归
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<Menu> GetListForDropDownList(int parentId, string str)
        {
            var list = new List<Menu>();
            List<Menu> childrens = GetListByParentId(parentId);

            for (int i = 0; i < childrens.Count; i++)
            {
                if (i == (childrens.Count - 1)) //当前层次最后一个栏目时
                {
                    str = str.Replace("├─", "└─");
                }
                Menu menu = childrens[i];

                menu.Name = str + menu.Name;
                list.Add(menu);

                string newStr = str;
                if (str.IsEmpty())
                    newStr = "　├─";
                else
                    newStr = (newStr.Replace("├─", "│").Replace("└─", "　") + "　├─");
                List<Menu> childList = GetListForDropDownList(menu.Id, newStr).ToList();
                list.AddRange(childList);
            }
            return list;
        }

        /// <summary>
        ///     生成一级导航菜单
        /// </summary>
        /// <returns></returns>
        public string GetFirstLevelMenu()
        {
            DataTable table =
                DbHelper.CurrentDb.Query(
                    "SELECT id,NAME,URL,hm.Description,ico,hm.ParentId FROM HC_Menu hm WHERE hm.ParentId=1 ORDER BY sort ASC")
                        .Tables[0];
            var html = new StringBuilder();
            const string temp =
                "<a href='javascript:void(0)' id='menu{0}' menuId='{0}' class='easyui-linkbutton firstMenu' data-options=\"plain:true,iconCls:'icon-custom-{1}'\">{2} </a>";

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                string ico = row["ico"].ToStr();
                ico = ico.IsEmpty() ? "default.png" : ico;
                ico = ico.Substring(0, ico.LastIndexOf('.'));
                html.Append(temp.FormatWith(row["id"], ico, row["Name"]) + Environment.NewLine);
            }
            return html.ToString();
        }


        /// <summary>
        ///     生成导航菜单
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string GetLeftGuideMenu(int pid)
        {
            var html = new StringBuilder();

            //二级导航
            List<Menu> menus = GetListByParentId(pid);
            foreach (Menu menu in menus)
            {
                html.AppendFormat(
                    "<div class='secondMenu' pid='{2}' id='secondMenu{3}' title=\"{0}\" data-options=\"iconCls:'icon-custom-{1}'\" style=\"overflow: auto; padding: 10px;\">",
                    menu.Name, menu.Ico.Replace(".png", ""), pid, menu.Id)
                    .Append(Environment.NewLine);
                //三级导航
                List<Menu> subMenus = GetListByParentId(menu.Id);
                foreach (Menu subMenu in subMenus)
                {
                    html.AppendFormat(
                        "<a href=\"javascript:addTab('{0}','{1}')\" class=\"easyui-linkbutton\" data-options=\"plain:true,iconCls:'icon-custom-{2}'\" style=\"width: 180px; text-align: left\">{0}</a>",
                        subMenu.Name, (BasePath + subMenu.Url).Replace("//", "/"), subMenu.Ico.Replace(".png", ""))
                        .Append(Environment.NewLine);
                }
                html.Append("</div> ").Append(Environment.NewLine);
            }
            return html.ToString();
        }

        /// <summary>
        ///     取得节点下所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Menu> GetListByParentId(int parentId)
        {
            return Query<Menu>(" WHERE ParentId=@0 ORDER BY Sort ASC", parentId).ToList();
        }
        /// <summary>
        ///     取得节点下所有子节点（递归，含当前节点）
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public List<Menu> GetChildrenCategoriesRrecusive(int pId)
        {
            const string sql = @" 
                                        WITH Recursives  AS
                                        (
                                            --定位点成员定义
                                            SELECT *,
                                                   0 AS LEVELs
                                            FROM   HC_Menu
                                            WHERE  id = @0 
                                            UNION ALL
                                            --递归成员定义
                                            SELECT E.*,
                                                   LEVELs + 1
                                            FROM   HC_Menu AS E
                                                   INNER JOIN Recursives AS D
                                                        ON  E.ParentId = D.id
                                        )
                                        SELECT *
                                        FROM   Recursives OPTION(MAXRECURSION 100)";

            DataSet dt = Query(sql, pId);

            var cates = new List<Menu>();
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                var cate = new Menu
                    {
                        Id = row["Id"].ToInt(),
                        Name = row["Name"].ToStr(),
                        Description = row["Description"].ToStr(),
                        ParentId = row["ParentId"].ToInt(),
                        Level = row["Level"].ToInt()
                    };
                cates.Add(cate);
            }

            cates.Sort((x, y) => x.Sort.ToInt() - y.Sort.ToInt());
            return cates;
        }

        #region 栏目菜单排序

        /// <summary>
        ///     栏目菜单排序
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public bool Sort(int pid, string sorts)
        {
            string[] cIds = sorts.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            int count = 0;
            foreach (string cId in cIds)
            {
                string[] kv = cId.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (kv.Length == 2)
                {
                    const string sql = "UPDATE HC_Menu SET SORT=@0 WHERE Id=@1";
                    Execute(sql, kv[1].ToInt(), kv[0].ToInt());
                    count++;
                }
            }
            return cIds.Length == count;
        }

        #endregion

        #region 菜单栏目树

        private readonly StringBuilder _result = new StringBuilder();
        private StringBuilder _sb = new StringBuilder();

        #region 01.生成整站菜单栏目树结构（菜单管理使用）

        /// <summary>
        ///  生成整站菜单栏目树结构（菜单管理使用）
        /// </summary>
        /// <returns></returns>
        public string GetJqueryEasyUiTree()
        {
            _sb.Clear();
            _result.Clear();
            DataTable dt =
                DbHelper.CurrentDb.Query("SELECT Id,Name,Url,ParentId FROM HC_Menu  Order by Sort ASC").Tables[
                    0];
            dt.Columns.Add("enable");
            string json = GetTreeJsonByTable(dt, "Id", "Name", "Url", "ParentId", "enable", "0");
            return json;
        }

        #endregion

        #region  02.生成单个管理员菜单栏目树（设置单个管理员菜单权限时使用）

        /// <summary>
        /// 02.生成单个管理员菜单栏目树（设置单个管理员菜单权限时使用）
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public string InitAdminMenuTree(int adminId)
        {
            _sb.Clear();
            _result.Clear();
            var adminPurview =
                DbHelper.CurrentDb.ExecuteScalar<object>("SELECT MenuPurview FROM HC_Administrator WHERE Id=@0",
                                                         adminId).ToStr();
            adminPurview = "," + adminPurview + ",";
            DataTable dt =
                DbHelper.CurrentDb.Query("SELECT Id,Name,Url,ParentId FROM HC_Menu  Order by Sort ASC").Tables[
                    0];
            dt.Columns.Add("enable");
            foreach (DataRow row in dt.Rows)
            {
                if (adminPurview.IndexOf("," + row["Id"] + ",", StringComparison.Ordinal) >= 0)
                {
                    row["enable"] = "true";
                }
                else
                {
                    row["enable"] = "false";
                }
            }

            string json = GetTreeJsonByTable(dt, "Id", "Name", "Url", "ParentId", "enable", "0");
            return json;
        }

        #endregion

        #region 03.生成单个部门菜单栏目树（设置单个部门菜单权限时使用）


        /// <summary>
        /// 03.生成单个部门菜单栏目树（设置单个部门菜单权限时使用）
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public string InitRoleMenuTree(int adminId)
        {
            _sb.Clear();
            _result.Clear();
            var purview =
                DbHelper.CurrentDb.ExecuteScalar<object>("SELECT MenuPurview FROM HC_Role WHERE Id=@0",
                                                         adminId).ToStr();
            purview = "," + purview + ",";
            DataTable dt = DbHelper.CurrentDb.Query("SELECT Id,Name,Url,ParentId FROM HC_Menu  Order by Sort ASC").Tables[0];
            dt.Columns.Add("enable");
            foreach (DataRow row in dt.Rows)
            {
                if (purview.IndexOf("," + row["Id"] + ",", StringComparison.Ordinal) >= 0)
                {
                    row["enable"] = "true";
                }
                else
                {
                    row["enable"] = "false";
                }
            }
            string json = GetTreeJsonByTable(dt, "Id", "Name", "Url", "ParentId", "enable", "0");
            return json;
        }

        #endregion

        #region 公共方法：根据DataTable生成EasyUI Tree Json树结构

        /// <summary>
        ///     根据DataTable生成EasyUI Tree Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="url">节点Url</param>
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
        private string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string url, string rela,
                                          string enable,
                                          object pId)
        {
            _result.Append(_sb);
            _sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                _sb.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        _sb.Append("{" +
                                   "\"id\":\"" + row[idCol] + "\"," +
                                   "\"text\":\"" + row[txtCol] + "\"," +
                                   "\"enable\":\"" + row[enable] + "\"," +
                                   "\"attributes\":\"" + row[url] + "\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            //点击展开
                            _sb.Append(",\"state\":\"expanded\",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, url, rela, enable, row[idCol]);
                            _result.Append(_sb);
                            _sb.Clear();
                        }
                        _result.Append(_sb);
                        _sb.Clear();
                        _sb.Append("},");
                    }
                    _sb = _sb.Remove(_sb.Length - 1, 1);
                }
                _sb.Append("]");
                _result.Append(_sb);
                _sb.Clear();
            }
            return _result.ToString();
        }

        #endregion

        #endregion
    }
}