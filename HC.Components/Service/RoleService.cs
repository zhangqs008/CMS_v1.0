//--------------------------------------------------------------------------------
// 文件描述：角色服务类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:32
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
    ///角色服务类
    /// </summary>
    public class RoleService : ServiceBase<Role>
    {
        private static RoleService _instance;
        private static readonly object SynObject = new object();

        private RoleService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static RoleService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new RoleService());
                }
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(Role instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 添加角色", instance.CreateUser),
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
        public Page<Role> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
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
            return DbHelper.CurrentDb.Page<Role>(pageIndex, pageSize, sql);
        }


        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(Role instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 修改角色", instance.CreateUser),
                                        string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        #region 删除

        /// <summary>
        ///     删除角色表记录
        /// </summary>
        /// <param name="id">根据ID删除角色表记录</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            try
            {
                const string sql = "DELETE FROM HC_Role WHERE Id=@0";
                var item = DbHelper.CurrentDb.SingleOrDefault<Role>(id);
                LogService.Instance.Log(string.Format("{0} 删除角色", HCContext.Current.Admin.LoginName),
                                        item.ToXml());
                DbHelper.CurrentDb.Execute(sql, id);
                return true;
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                return false;
            }
        }

        /// <summary>
        ///     批量删除角色表实体
        /// </summary>
        /// <param name="ids">根据ID批量删除角色表实体</param>
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

        #endregion


        /// <summary>
        ///     获取栏目列表的下级栏目, 供下拉列表使用, 递归
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<Role> GetListForDropDownList(int parentId, string str)
        {
            var list = new List<Role>();
            List<Role> childrens = GetListByParentId(parentId);

            for (int i = 0; i < childrens.Count; i++)
            {
                if (i == (childrens.Count - 1)) //当前层次最后一个栏目时
                {
                    str = str.Replace("├─", "└─");
                }
                Role item = childrens[i];

                item.Name = str + item.Name;
                list.Add(item);

                string newStr = str;
                if (str.IsEmpty())
                    newStr = "　├─";
                else
                    newStr = (newStr.Replace("├─", "│").Replace("└─", "　") + "　├─");
                List<Role> childList = GetListForDropDownList(item.Id, newStr).ToList();
                list.AddRange(childList);
            }
            return list;
        }

        /// <summary>
        ///     取得节点下所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Role> GetListByParentId(int parentId)
        {
            return Query<Role>(" WHERE ParentId=@0 ORDER BY Sort ASC", parentId).ToList();
        }


        #region 根据DataTable生成EasyUI Tree Json树结构

        private readonly StringBuilder _result = new StringBuilder();
        private StringBuilder _sb = new StringBuilder();

        /// <summary>
        ///  生成EasyUI Tree Json树结构
        /// </summary>
        /// <returns></returns>
        public string GetJqueryEasyUiTree()
        {
            _sb.Clear();
            _result.Clear();
            DataTable dt = DbHelper.CurrentDb.Query("SELECT Id,Name,ParentId FROM HC_Role Order by Sort ASC").Tables[0];
            string json = GetTreeJsonByTable(dt, "Id", "Name", "Name", "ParentId", "0");
            return json;
        }

        /// <summary>
        ///     根据DataTable生成EasyUI Tree Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="url">节点Url</param>
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
        private string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string url, string rela, object pId)
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
                                   "\"attributes\":\"" + row[url] + "\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            //点击展开
                            _sb.Append(",\"state\":\"expanded\",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, url, rela, row[idCol]);
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

        /// <summary>
        ///     取得节点下所有子节点（递归，含当前节点）
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<Role> GetChildrensRrecusive(int pid)
        {
            const string sql = @" 
                                        WITH Recursives  AS
                                        (
                                            --定位点成员定义
                                            SELECT *,
                                                   0 AS LEVELs
                                            FROM   HC_Role
                                            WHERE  id = @0 
                                            UNION ALL
                                            --递归成员定义
                                            SELECT E.*,
                                                   LEVELs + 1
                                            FROM   HC_Role AS E
                                                   INNER JOIN Recursives AS D
                                                        ON  E.ParentId = D.id
                                        )
                                        SELECT *
                                        FROM   Recursives OPTION(MAXRECURSION 100)";

            DataSet dt = Query(sql, pid);

            var departments = new List<Role>();
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                var item = new Role
                {
                    Id = row["Id"].ToInt(),
                    Name = row["Name"].ToStr(),
                    ParentId = row["ParentId"].ToInt()
                };
                departments.Add(item);
            }

            departments.Sort((x, y) => x.Sort.ToInt() - y.Sort.ToInt());
            return departments;
        }

        /// <summary>
        /// 栏目菜单排序
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public bool Sort(int pid, string sorts)
        {
            string[] ids = sorts.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            int count = 0;
            foreach (string id in ids)
            {
                string[] split = id.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2)
                {
                    const string sql = "UPDATE HC_Role SET SORT=@0 WHERE Id=@1";
                    Execute(sql, split[1].ToInt(), split[0].ToInt());
                    count++;
                }
            }
            return ids.Length == count;
        }


        public bool SetRoleMenuPurview(int deparmentId, string menuPurview)
        {
            const string sql = "UPDATE HC_Role SET MenuPurview=@0 WHERE Id=@1";
            try
            {
                DbHelper.CurrentDb.BeginTransaction();
                //更新角色栏目权限
                DbHelper.CurrentDb.Execute(sql, menuPurview, deparmentId);
                //更新角色下成员栏目权限
                //const string sql = "UPDATE HC_Administrator SET MenuPurview=@0 WHERE Id=@1";
                //DbHelper.CurrentDb.Execute(sql, menuPurview, adminId) > 0;
                DbHelper.CurrentDb.CompleteTransaction();
                return true;
            }
            catch (Exception ex)
            {
                LogService.Instance.LogException(ex);
                DbHelper.CurrentDb.AbortTransaction();
                return false;
            }
        }
    }
}