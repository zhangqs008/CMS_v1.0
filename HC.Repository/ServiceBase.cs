using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HC.Repository
{
    public class ServiceBase<T> where T : class
    {
        /// <summary>
        ///     网站根目录路径，末尾已包含“/”
        /// </summary>
        public static string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }

        /// <summary>
        /// 放弃事务
        /// </summary>
        public void AbortTransaction()
        {
            DbHelper.CurrentDb.AbortTransaction();
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            DbHelper.CurrentDb.BeginTransaction();
        }
        /// <summary>
        /// 关闭共享事务
        /// </summary>
        public void CloseSharedConnection()
        {
            DbHelper.CurrentDb.CloseSharedConnection();
        }
        /// <summary>
        /// 事务完成
        /// </summary>
        public void CompleteTransaction()
        {
            DbHelper.CurrentDb.CompleteTransaction();
        }

        public void CreateCommand(IDbConnection connection, string sql, params object[] paras)
        {
            DbHelper.CurrentDb.CreateCommand(connection, sql, paras);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="poco">对象实体</param>
        /// <returns></returns>
        public int Delete(object poco)
        {
            return DbHelper.CurrentDb.Delete(poco);
        }
        /// <summary>
        ///  删除记录
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="pocoOrPrimaryKey">主键值</param>
        /// <returns></returns>
        public int Delete<T>(object pocoOrPrimaryKey)
        {
            return DbHelper.CurrentDb.Delete<T>(pocoOrPrimaryKey);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public int Delete(string tableName, string primaryKeyName, object poco)
        {
            return DbHelper.CurrentDb.Delete(tableName, primaryKeyName, poco);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <param name="poco">实体</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        public int Delete(string tableName, string primaryKeyName, object poco, string primaryKeyValue)
        {
            return DbHelper.CurrentDb.Delete(tableName, primaryKeyName, poco, primaryKeyValue);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public int Delete(string sql, params object[] args)
        {
            return DbHelper.CurrentDb.Delete<T>(sql, args);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="sql">SQL条件语句，如："Where id=12"</param>
        /// <returns></returns>
        public int Delete(Sql sql)
        {
            var pd = HC.Repository.Database.PocoData.ForType(typeof(T));
            return Execute(new Sql(string.Format("DELETE FROM {0} ", EscapeTableName(pd.TableInfo.TableName))).Append(sql));
        }
        public void Dispose()
        {
            DbHelper.CurrentDb.Dispose();
        }

        public string EscapeSqlIdentifier(string str)
        {
            return DbHelper.CurrentDb.EscapeSqlIdentifier(str);
        }

        public string EscapeTableName(string str)
        {
            return DbHelper.CurrentDb.EscapeTableName(str);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        public int Execute(Sql sql)
        {
            return DbHelper.CurrentDb.Execute(sql);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数值</param>
        public int Execute(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Execute(sql, paras);
        }

        /// <summary>
        /// 执行sql语句，返回一实体的一属性
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql语句</param>
        public T ExecuteScalar(Sql sql)
        {
            return DbHelper.CurrentDb.ExecuteScalar<T>(sql);
        }
        /// <summary>
        /// 执行sql语句，返回一实体的一属性
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public T ExecuteScalar(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.ExecuteScalar<T>(sql, paras);
        }
        /// <summary>
        /// 执行sql语句，返回一实体的一属性
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.ExecuteScalar<T>(sql, paras);
        }
        /// <summary>
        /// 查询记录是否已存在
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        public bool Exists(object primaryKey)
        {
            return DbHelper.CurrentDb.Exists<T>(primaryKey);
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public List<T> Fetch(Sql sql)
        {
            return DbHelper.CurrentDb.Fetch<T>(sql);
        }
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public List<T> Fetch(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Fetch<T>(sql, paras);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">要获取的页数，从1开始</param>
        /// <param name="itemsPerPage">每页多少条记录</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public List<T> Fetch(long page, long itemsPerPage, Sql sql)
        {
            return DbHelper.CurrentDb.Fetch<T>(page, itemsPerPage, sql);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">要获取的页数，从1开始</param>
        /// <param name="itemsPerPage">每页多少条记录</param>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public List<T> Fetch(long page, long itemsPerPage, string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Fetch<T>(page, itemsPerPage, sql, paras);
        }
        /// <summary>
        /// 获取查询记录的第一条记录
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public T First(Sql sql)
        {
            return DbHelper.CurrentDb.First<T>(sql);
        }
        /// <summary>
        /// 获取查询记录的第一条记录
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public T First(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.First<T>(sql, paras);
        }
        /// <summary>
        /// 获取查询记录的第一条记录,不存在则返回默认值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public T FirstOrDefault(Sql sql)
        {
            return DbHelper.CurrentDb.FirstOrDefault<T>(sql);
        }
        /// <summary>
        /// 获取查询记录的第一条记录,不存在则返回默认值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public T FirstOrDefault(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.FirstOrDefault<T>(sql, paras);
        }

        public string FormatCommand(IDbCommand cmd)
        {
            return DbHelper.CurrentDb.FormatCommand(cmd);
        }


        public string FormatCommand(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.FormatCommand(sql, paras);
        }
        /// <summary>
        /// 得到一个事务范围
        /// </summary>
        /// <returns></returns>
        public Transaction GetTransaction()
        {
            return DbHelper.CurrentDb.GetTransaction();
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public object Insert(object poco)
        {
            return DbHelper.CurrentDb.Insert(poco);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public object Insert(string tableName, string primaryKeyName, object poco)
        {
            return DbHelper.CurrentDb.Insert(tableName, primaryKeyName, poco);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名称</param>
        /// <param name="autoIncrement">主键是否自增值</param>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public object Insert(string tableName, string primaryKeyName, bool autoIncrement, object poco)
        {
            return DbHelper.CurrentDb.Insert(tableName, primaryKeyName, autoIncrement, poco);
        }
        /// <summary>
        /// 直接执行插入的sql语句，返回主键值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <returns></returns>
        public object Insert(string sql, string primaryKeyName)
        {
            return DbHelper.CurrentDb.Insert(sql, primaryKeyName);
        }

        /// <summary>
        /// 直接执行插入的sql语句，返回主键值
        /// </summary>
        ///  <param name="primaryKeyName">主键名</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public object Insert(string primaryKeyName, string sql, params object[] args)
        {
            return DbHelper.CurrentDb.Insert(primaryKeyName, sql, args);
        }

        /// <summary>
        /// 判断某记录是否为最新记录
        /// </summary>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public bool IsNew(object poco)
        {
            return DbHelper.CurrentDb.IsNew(poco);
        }

        /// <summary>
        /// 判断某记录是否为最新记录
        /// </summary>
        /// <param name="primaryKeyName">主键名称</param>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public bool IsNew(string primaryKeyName, object poco)
        {
            return DbHelper.CurrentDb.IsNew(primaryKeyName, poco);
        }

        public void OnBeginTransaction()
        {
            DbHelper.CurrentDb.OnBeginTransaction();
        }

        public void OnConnectionClosing(IDbConnection conn)
        {
            DbHelper.CurrentDb.OnConnectionClosing(conn);
        }

        public IDbConnection OnConnectionOpened(IDbConnection conn)
        {
            return DbHelper.CurrentDb.OnConnectionOpened(conn);
        }

        public void OnEndTransaction()
        {
            DbHelper.CurrentDb.OnEndTransaction();
        }

        /// <summary>
        /// 打开共享连接
        /// </summary>
        public void OpenSharedConnection()
        {
            DbHelper.CurrentDb.OpenSharedConnection();
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="page">要获取的记录页，从1开始</param>
        /// <param name="itemsPerPage">每页多少条记录</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public Page<T> Page(long page, long itemsPerPage, Sql sql)
        {
            return DbHelper.CurrentDb.Page<T>(page, itemsPerPage, sql);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="page">要获取的记录页，从1开始</param>
        /// <param name="itemsPerPage">每页多少条记录</param>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public Page<T> Page(long page, long itemsPerPage, string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Page<T>(page, itemsPerPage, sql, paras);
        }
        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public DataSet Query(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Query(sql, paras);

        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">要获取的记录页，从1开始</param>
        /// <param name="itemPerPage">每页多少条记录</param>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public PageDataSet Query(long page, long itemPerPage, string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Query(page, itemPerPage, sql, paras);

        }
        /// <summary>
        /// 返回一个枚举集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public IEnumerable<T> Query(Sql sql)
        {
            return DbHelper.CurrentDb.Query<T>(sql);

        }
        /// <summary>
        /// 返回一个枚举集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Query<T>(sql, paras);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="types"></param>
        /// <param name="cb"></param>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(Type[] types, object cb, string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Query<T>(types, cb, sql, paras);

        }
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetList()
        {
            return DbHelper.CurrentDb.Query<T>("").ToList<T>();

        }
        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="poco">实体</param>
        public void Save(object poco)
        {
            DbHelper.CurrentDb.Save(poco);
        }
        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名称</param>
        /// <param name="poco">实体</param>
        public void Save(string tableName, string primaryKeyName, object poco)
        {
            DbHelper.CurrentDb.Save(tableName, primaryKeyName, poco);
        }
        /// <summary>
        /// 获取记录集的第一条记录
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public T Single(Sql sql)
        {
            return DbHelper.CurrentDb.Single<T>(sql);
        }
        /// <summary>
        /// 获取记录集的第一条记录
        /// </summary>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        public T Single(object primaryKey)
        {
            return DbHelper.CurrentDb.Single<T>(primaryKey);
        }

        /// <summary>
        /// 获取记录集的第一条记录
        /// </summary>
        /// <param name="sql">主键值</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public T Single(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Single<T>(sql, paras);
        }
        /// <summary>
        /// 获取记录集的第一条记录,不存在则返回默认值
        /// </summary>
        /// <param name="primaryKey">主键值</param>
        /// <returns></returns>
        public T SingleOrDefault<T>(object primaryKey)
        {
            return DbHelper.CurrentDb.SingleOrDefault<T>(primaryKey);
        }
        /// <summary>
        ///  获取记录集的第一条记录,不存在则返回默认值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public T SingleOrDefault(Sql sql)
        {
            return DbHelper.CurrentDb.SingleOrDefault<T>(sql);
        }
        /// <summary>
        /// 获取记录集的第一条记录,不存在则返回默认值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public T SingleOrDefault<T>(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.SingleOrDefault<T>(sql, paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> SkipTake(long skip, long take, Sql sql)
        {
            return DbHelper.CurrentDb.SkipTake<T>(skip, take, sql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public List<T> SkipTake(long skip, long take, string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.SkipTake<T>(skip, take, sql, paras);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public int Update(object poco)
        {
            return DbHelper.CurrentDb.Update(poco);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="poco">实体</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        public int Update(object poco, object primaryKeyValue)
        {
            return DbHelper.CurrentDb.Update(poco, primaryKeyValue);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="poco">实体</param>
        /// <param name="columns">需要更新的列集合</param>
        /// <returns></returns>
        public int Update(object poco, IEnumerable<string> columns)
        {
            return DbHelper.CurrentDb.Update(poco, columns);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="poco">实体</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <param name="columns">需要更新的列集合</param>
        /// <returns></returns>
        public int Update(object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return DbHelper.CurrentDb.Update(poco, primaryKeyValue, columns);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键</param>
        /// <param name="poco">实体</param>
        /// <returns></returns>
        public int Update(string tableName, string primaryKeyName, object poco)
        {
            return DbHelper.CurrentDb.Update(tableName, primaryKeyName, poco);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <param name="poco">实体</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return DbHelper.CurrentDb.Update(tableName, primaryKeyName, poco, primaryKeyValue);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <param name="poco">实体</param>
        /// <param name="columns">主键值</param>
        /// <returns></returns>
        public int Update(string tableName, string primaryKeyName, object poco, IEnumerable<string> columns)
        {
            return DbHelper.CurrentDb.Update(tableName, primaryKeyName, poco, columns);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyName">主键名</param>
        /// <param name="poco">实体</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <param name="columns">要更新的列集合</param>
        /// <returns></returns>
        public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return DbHelper.CurrentDb.Update(tableName, primaryKeyName, poco, primaryKeyValue, columns);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int Update<T>(Sql sql)
        {
            return DbHelper.CurrentDb.Update<T>(sql);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="paras">参数名</param>
        /// <returns></returns>
        public int Update<T>(string sql, params object[] paras)
        {
            return DbHelper.CurrentDb.Update<T>(sql, paras);
        }

        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args) { return Query<T1, T2, TRet>(cb, sql, args).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args) { return Query<T1, T2, T3, TRet>(cb, sql, args).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args) { return Query<T1, T2, T3, T4, TRet>(cb, sql, args).ToList(); }

        // Multi Query
        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args) { return DbHelper.CurrentDb.Query<TRet>(new Type[] { typeof(T1), typeof(T2) }, cb, sql, args); }
        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args) { return DbHelper.CurrentDb.Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, cb, sql, args); }
        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args) { return DbHelper.CurrentDb.Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, sql, args); }

        // Multi Fetch (SQL builder)
        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql) { return Query<T1, T2, TRet>(cb, sql.SQL, sql.Arguments).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql) { return Query<T1, T2, T3, TRet>(cb, sql.SQL, sql.Arguments).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql) { return Query<T1, T2, T3, T4, TRet>(cb, sql.SQL, sql.Arguments).ToList(); }

        // Multi Query (SQL builder)
        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql) { return DbHelper.CurrentDb.Query<TRet>(new Type[] { typeof(T1), typeof(T2) }, cb, sql.SQL, sql.Arguments); }
        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql) { return DbHelper.CurrentDb.Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, cb, sql.SQL, sql.Arguments); }
        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql) { return DbHelper.CurrentDb.Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, sql.SQL, sql.Arguments); }

        // Multi Fetch (Simple)
        public List<T1> Fetch<T1, T2>(string sql, params object[] args) { return Query<T1, T2>(sql, args).ToList(); }
        public List<T1> Fetch<T1, T2, T3>(string sql, params object[] args) { return Query<T1, T2, T3>(sql, args).ToList(); }
        public List<T1> Fetch<T1, T2, T3, T4>(string sql, params object[] args) { return Query<T1, T2, T3, T4>(sql, args).ToList(); }

        // Multi Query (Simple)
        public IEnumerable<T1> Query<T1, T2>(string sql, params object[] args) { return DbHelper.CurrentDb.Query<T1>(new Type[] { typeof(T1), typeof(T2) }, null, sql, args); }
        public IEnumerable<T1> Query<T1, T2, T3>(string sql, params object[] args) { return DbHelper.CurrentDb.Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, null, sql, args); }
        public IEnumerable<T1> Query<T1, T2, T3, T4>(string sql, params object[] args) { return DbHelper.CurrentDb.Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, null, sql, args); }

        // Multi Fetch (Simple) (SQL builder)
        public List<T1> Fetch<T1, T2>(Sql sql) { return Query<T1, T2>(sql.SQL, sql.Arguments).ToList(); }
        public List<T1> Fetch<T1, T2, T3>(Sql sql) { return Query<T1, T2, T3>(sql.SQL, sql.Arguments).ToList(); }
        public List<T1> Fetch<T1, T2, T3, T4>(Sql sql) { return Query<T1, T2, T3, T4>(sql.SQL, sql.Arguments).ToList(); }

        // Multi Query (Simple) (SQL builder)
        public IEnumerable<T1> Query<T1, T2>(Sql sql) { return DbHelper.CurrentDb.Query<T1>(new Type[] { typeof(T1), typeof(T2) }, null, sql.SQL, sql.Arguments); }
        public IEnumerable<T1> Query<T1, T2, T3>(Sql sql) { return DbHelper.CurrentDb.Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, null, sql.SQL, sql.Arguments); }
        public IEnumerable<T1> Query<T1, T2, T3, T4>(Sql sql) { return DbHelper.CurrentDb.Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, null, sql.SQL, sql.Arguments); }
    }
}
