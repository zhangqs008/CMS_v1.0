//--------------------------------------------------------------------------------
// 文件描述：系统栏目服务类
// 文件作者：张清山
// 创建日期：2014-08-31 23:02:42
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
    ///系统栏目服务类
    /// </summary>
    public class ColumnService : ServiceBase<Column>
    {
        #region 单例实例

        private static ColumnService _instance;
        private static readonly object SynObject = new object();

        private ColumnService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static ColumnService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new ColumnService());
                }
            }
        }

        #endregion

        #region 添加

        /// <summary>
        /// 添加系统栏目
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(Column instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 添加系统栏目记录", instance.CreateUser),
                                        string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        #endregion

        #region 查询分页

        /// <summary>
        ///     获取分页数据
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="conditions">搜索条件</param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public Page<Column> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
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
            return DbHelper.CurrentDb.Page<Column>(pageIndex, pageSize, sql);
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改系统栏目
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(Column instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                LogService.Instance.Log(string.Format("{0} 修改系统栏目记录", instance.CreateUser),
                                        string.Format("{0}", instance.ToXml()));
                return true;
            }
            return false;
        }

        #endregion

        #region 删除

        /// <summary>
        ///     删除系统栏目记录
        /// </summary>
        /// <param name="id">根据ID删除系统栏目记录</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            try
            {
                const string sql = "DELETE FROM HC_Column WHERE Id=@0";
                var item = DbHelper.CurrentDb.SingleOrDefault<Column>(id);
                LogService.Instance.Log(string.Format("{0} 删除系统栏目记录", HCContext.Current.Admin.LoginName),
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
        ///     批量删除系统栏目实体
        /// </summary>
        /// <param name="ids">根据ID批量删除系统栏目实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(string ids)
        {
            try
            {
                string[] idArr = ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in idArr)
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
        ///     取得节点下所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Column> GetListByParentId(int parentId)
        {
            return Query<Column>(" WHERE ParentId=@0 ORDER BY Sort ASC", parentId).ToList();
        }

        #region 栏目树

        private readonly StringBuilder _result = new StringBuilder();
        private StringBuilder _sb = new StringBuilder();

        #region 01.生成整站栏目树结构

        /// <summary>
        ///  生成整站栏目树结构 
        /// </summary>
        /// <returns></returns>
        public string GetJqueryEasyUiTree()
        {
            _sb.Clear();
            _result.Clear();
            DataTable dt =
                DbHelper.CurrentDb.Query("SELECT Id,Name,ParentId FROM HC_Column Order by Sort ASC").Tables[
                    0];
            dt.Columns.Add("enable");
            string json = GetTreeJsonByTable(dt, "Id", "Name", "ParentId", "0");
            return json;
        }

        /// <summary>
        ///     根据DataTable生成EasyUI Tree Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param> 
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
        private string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela,
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
                                   "\"text\":\"" + row[txtCol] + "\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            //点击展开
                            _sb.Append(",\"state\":\"expanded\",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol]);
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
        /// <summary>
        ///     取得节点下所有子节点（递归，含当前节点）
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public List<Column> GetChildrenCategoriesRrecusive(int pId)
        {
            const string sql = @" 
                                        WITH Recursives  AS
                                        (
                                            --定位点成员定义
                                            SELECT *,
                                                   0 AS LEVELs
                                            FROM   HC_Column
                                            WHERE  id = @0 
                                            UNION ALL
                                            --递归成员定义
                                            SELECT E.*,
                                                   LEVELs + 1
                                            FROM   HC_Column AS E
                                                   INNER JOIN Recursives AS D
                                                        ON  E.ParentId = D.id
                                        )
                                        SELECT *
                                        FROM   Recursives OPTION(MAXRECURSION 100)";

            DataSet dt = Query(sql, pId);

            var cates = new List<Column>();
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                var cate = new Column
                {
                    Id = row["Id"].ToInt(),
                    Name = row["Name"].ToStr(),
                    ParentId = row["ParentId"].ToInt(),
                    Level = row["Level"].ToInt()
                };
                cates.Add(cate);
            }

            cates.Sort((x, y) => x.Sort.ToInt() - y.Sort.ToInt());
            return cates;
        }

        /// <summary>
        ///     栏目排序
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
                    const string sql = "UPDATE HC_Column SET SORT=@0 WHERE Id=@1";
                    Execute(sql, kv[1].ToInt(), kv[0].ToInt());
                    count++;
                }
            }
            return cIds.Length == count;
        }

        /// <summary>
        ///     获取栏目列表的下级栏目, 供下拉列表使用, 递归
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<Column> GetListForDropDownList(int parentId, string str)
        {
            var list = new List<Column>();
            List<Column> childrens = GetListByParentId(parentId);

            for (int i = 0; i < childrens.Count; i++)
            {
                if (i == (childrens.Count - 1)) //当前层次最后一个栏目时
                {
                    str = str.Replace("├─", "└─");
                }
                Column column = childrens[i];

                column.Name = str + column.Name;
                list.Add(column);

                string newStr = str;
                if (str.IsEmpty())
                    newStr = "　├─";
                else
                    newStr = (newStr.Replace("├─", "│").Replace("└─", "　") + "　├─");
                List<Column> childList = GetListForDropDownList(column.Id, newStr).ToList();
                list.AddRange(childList);
            }
            return list;
        }

    }
}