//--------------------------------------------------------------------------------
// 文件描述：内容模型表单提交处理类
// 文件作者：张清山
// 创建日期：2014-08-30 12:10:33
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using HC.Ajax;
using HC.Components.Model;
using HC.Components.Service;
using HC.Foundation;
using HC.Framework.DataBase.DBManager;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.Components.AjaxForm
{
    public class ModuleFormHandler : AjaxFormHandler
    {
        protected new string Procress()
        {
            try
            {
                if (HCContext.Current.Admin.Identity.IsAuthenticated)
                {
                    int id = HttpContext.Current.Request.Form["Id"].ToInt(); //Id
                    string name = HttpContext.Current.Request.Form["Name"].ToStr(); //模型名称
                    string description = HttpContext.Current.Request.Form["Description"].ToStr(); //描述
                    string tableName = HttpContext.Current.Request.Form["TableName"].ToStr(); //对应物理表名 

                    if (
                        !new Regex("(?i)^([a-zA-Z_]+)$", RegexOptions.CultureInvariant | RegexOptions.Compiled).IsMatch(
                            tableName))
                    {
                        return "对不起，数据库表不符合命名规范，只能是英文字符或下划线";
                    }

                    #region 检查表是否已存在

                    string connectionString = ConfigurationManager.ConnectionStrings["sqlserver"].ConnectionString;
                    string db =
                        new Regex("(?i)(Initial Catalog|database|database)=(?<database>[^;]*);",
                                  RegexOptions.CultureInvariant | RegexOptions.Compiled).Match(connectionString).Groups[
                                      "database"].Value;

                    DataTable dt = DBManager.GetTables(db, connectionString);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (String.Compare(row["表名"].ToStr(), tableName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return "对不起，表：" + tableName + "已存在，请更换表名";
                        }
                    }

                    #endregion

                    Module instance = ModuleService.Instance.SingleOrDefault<Module>(id) ??
                                      ModelFactory<Module>.Insten();

                    instance.Name = name;
                    instance.Description = description;
                    instance.TableName = tableName;

                    #region 创建表

                    string sql = @"CREATE TABLE  {0} (
	                                    Id bigint IDENTITY(1,1) NOT NULL PRIMARY KEY, 	
	                                    ColumnId int NULL,
	                                    IsDel bit NULL,
	                                    Sort bigint NULL,
	                                    Status int NULL,
	                                    CreateDate datetime NULL,
	                                    CreateUser nvarchar(64) NULL,
	                                    UpdateDate datetime NULL,
	                                    UpdateUser nvarchar(64) NULL 
                                    )";
                    sql = sql.FormatWith(tableName);
                    DbHelper.CurrentDb.Execute(sql);

                    #endregion

                    if (id.ToInt() > 0)
                    {
                        ModuleService.Instance.Modify(instance);
                    }
                    else
                    {
                        ModuleService.Instance.Add(instance);
                    }
                    return "true";
                }
                return "用户尚未登录，操作被拒绝";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}