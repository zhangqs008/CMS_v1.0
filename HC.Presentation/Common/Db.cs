using System.Data;
using HC.Repository;

namespace HC.Presentation.Common
{
    /// <summary>
    ///     数据库相关
    /// </summary>
    public class Db : PresentBase
    {
 
        /// <summary>
        ///     执行数据库查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Execute(string sql)
        {
            return DbHelper.CurrentDb.Query(sql).Tables[0];
        }
        /// <summary>
        ///     执行数据库查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Query(string sql)
        {
            return DbHelper.CurrentDb.Query(sql).Tables[0];
        }
         

        public bool IsNull(object value)
        {
            return value == null;
        }
    }
}