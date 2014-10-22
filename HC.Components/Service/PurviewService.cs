//--------------------------------------------------------------------------------
// 文件描述：系统权限表服务类
// 文件作者：张清山
// 创建日期：2014-08-21 09:26:12
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HC.Foundation;
using HC.Foundation.Log;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.Service
{
    /// <summary>
    ///系统权限表服务类
    /// </summary>
    public class PurviewService : ServiceBase<Model.Purview>
    {
        private static PurviewService _instance;
        private static readonly object SynObject = new object();

        private PurviewService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static PurviewService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new PurviewService());
                }
            }
        }

        /// <summary>
        /// 添加系统权限表
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(Model.Purview instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 添加系统权限记录", instance.CreateUser),
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
        public Page<Model.Purview> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
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
            return DbHelper.CurrentDb.Page<Model.Purview>(pageIndex, pageSize, sql);
        }


        /// <summary>
        /// 修改系统权限表
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(Model.Purview instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 修改系统权限记录", instance.CreateUser),
                string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        /// <summary>
        ///     删除系统权限记录
        /// </summary>
        /// <param name="id">根据ID删除系统权限记录</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            try
            {
                const string sql = "DELETE FROM HC_Purview WHERE Id=@0";
                var item = DbHelper.CurrentDb.SingleOrDefault<Model.Purview>(id);
                LogService.Instance.Log(string.Format("{0} 删除系统权限记录", HCContext.Current.Admin.LoginName),
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
        ///     批量删除系统权限记录
        /// </summary>
        /// <param name="ids">根据ID批量删除系统权限记录</param>
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
        ///     取得节点下所有子节点（递归，含当前节点）
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<Model.Purview> GetChildrensRrecusive(int pid)
        {
            const string sql = @" 
                                        WITH Recursives  AS
                                        (
                                            --定位点成员定义
                                            SELECT *,
                                                   0 AS LEVELs
                                            FROM   HC_Purview
                                            WHERE  id = @0 
                                            UNION ALL
                                            --递归成员定义
                                            SELECT E.*,
                                                   LEVELs + 1
                                            FROM   HC_Purview AS E
                                                   INNER JOIN Recursives AS D
                                                        ON  E.ParentId = D.id
                                        )
                                        SELECT *
                                        FROM   Recursives OPTION(MAXRECURSION 100)";

            DataSet dt = Query(sql, pid);

            var cates = new List<Model.Purview>();
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                var cate = new Model.Purview
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
                    const string sql = "UPDATE HC_Purview SET SORT=@0 WHERE Id=@1";
                    Execute(sql, split[1].ToInt(), split[0].ToInt());
                    count++;
                }
            }
            return ids.Length == count;
        }

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
                DbHelper.CurrentDb.Query("SELECT Id,Name,OperateCode,Description,ParentId FROM HC_Purview  Order by Sort ASC").Tables[
                    0];
            dt.Columns.Add("enable");
            string json = GetTreeJsonByTable(dt, "Id", "Name", "OperateCode", "ParentId", "enable", "Description", "0");
            return json;
        }

        #endregion

        #region  02.生成单个管理员菜单栏目树（设置单个管理员菜单权限时使用）

        /// <summary>
        /// 02.生成单个管理员菜单栏目树（设置单个管理员菜单权限时使用）
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public string InitAdminPurviewTree(int adminId)
        {
            _sb.Clear();
            _result.Clear();
            string adminPurview =
                DbHelper.CurrentDb.ExecuteScalar<object>("SELECT OperatePurview FROM Whir_Administrator WHERE Id=@0", adminId).ToStr();
            adminPurview = "," + adminPurview + ",";
            DataTable dt =
                DbHelper.CurrentDb.Query("SELECT Id,Name,OperateCode,Description,ParentId FROM HC_Purview  Order by Sort ASC").Tables[
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

            string json = GetTreeJsonByTable(dt, "Id", "Name", "OperateCode", "ParentId", "enable", "Description", "0");
            return json;
        }

        #endregion

        #region 03.生成单个部门菜单栏目树（设置单个部门菜单权限时使用）

        /// <summary>
        /// 03.生成单个部门菜单栏目树（设置单个部门菜单权限时使用）
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public string InitDepartmentPurviewTree(int adminId)
        {
            _sb.Clear();
            _result.Clear();
            string purview =
                DbHelper.CurrentDb.ExecuteScalar<object>("SELECT OperatePurview FROM Whir_Member_Department WHERE Id=@0",
                                                         adminId).ToStr();
            purview = "," + purview + ",";
            DataTable dt = DbHelper.CurrentDb.Query("SELECT Id,Name,OperateCode,Description,ParentId FROM HC_Purview  Order by Sort ASC").Tables[0];
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
            string json = GetTreeJsonByTable(dt, "Id", "Name", "OperateCode", "ParentId", "enable", "Description", "0");
            return json;
        }

        #endregion

        #region 公共方法：根据DataTable生成EasyUI Tree Json树结构

        /// <summary>
        ///     根据DataTable生成EasyUI Tree Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="id">ID列</param>
        /// <param name="name">Text列</param>
        /// <param name="code">节点Url</param>
        /// <param name="rela">关系字段</param>
        /// <param name="description"> </param>
        /// <param name="pId">父ID</param>
        /// <param name="enable"> </param>
        private string GetTreeJsonByTable(DataTable tabel, string id, string name, string code, string rela,
                                          string enable, string description,
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
                                   "\"Id\":\"" + row[id] + "\"," +
                                   "\"Name\":\"" + row[name] + "\"," +
                                   "\"Enable\":\"" + row[enable] + "\"," +
                                   "\"Description\":\"" + row[description] + "\"," +
                                   "\"OperateCode\":\"" + row[code] + "\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[id])).Length > 0)
                        {
                            //点击展开
                            _sb.Append(",\"state\":\"expanded\",\"children\":");
                            GetTreeJsonByTable(tabel, id, name, code, rela, enable, description, row[id]);
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
        ///     取得节点下所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Model.Purview> GetListByParentId(int parentId)
        {
            return Query<Model.Purview>(" WHERE ParentId=@0 ORDER BY Sort ASC", parentId).ToList();
        }
        #endregion

        /// <summary>
        ///     获取栏目列表的下级栏目, 供下拉列表使用, 递归
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<Model.Purview> GetListForDropDownList(int parentId, string str)
        {
            var list = new List<Model.Purview>();
            List<Model.Purview> childrens = GetListByParentId(parentId);

            for (int i = 0; i < childrens.Count; i++)
            {
                if (i == (childrens.Count - 1)) //当前层次最后一个栏目时
                {
                    str = str.Replace("├─", "└─");
                }
                Model.Purview item = childrens[i];

                item.Name = str + item.Name;
                list.Add(item);

                string newStr = str;
                if (str.IsEmpty())
                    newStr = "　├─";
                else
                    newStr = (newStr.Replace("├─", "│").Replace("└─", "　") + "　├─");
                List<Model.Purview> childList = GetListForDropDownList(item.Id, newStr).ToList();
                list.AddRange(childList);
            }
            return list;
        }

    }
}


