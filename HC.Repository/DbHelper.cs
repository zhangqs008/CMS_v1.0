using System;
using System.Web;
using System.Configuration;
using System.Web.Caching;

namespace HC.Repository
{
    public class DbHelper
    {
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        private static string CurrentDbType
        {
            get
            {
                if (HttpRuntime.Cache["CurrentDbType"] == null)
                {
                    string CurrentDbType = ConfigurationManager.AppSettings.Get("UseDbType");
                    if (!string.IsNullOrEmpty(CurrentDbType))
                    {
                        CacheDependency cdy = new CacheDependency(HttpRuntime.AppDomainAppPath + "web.config");
                        HttpRuntime.Cache.Insert("CurrentDbType", ConfigurationManager.AppSettings.Get("UseDbType"), cdy);
                    }
                    else
                    {
                        throw new ArgumentNullException("找不到数据库配置：请检查web.config节点<appSettings></appSettings>是否存在节点UseDbType");
                    }

                }
                return HttpRuntime.Cache["CurrentDbType"].ToString();
            }
        }
        /// <summary>
        /// 获取当前数据库支持的操作
        /// </summary>
        public static Database CurrentDb
        {
            get
            {
               
                HttpContext context = HttpContext.Current;

                if (null == context)
                {
                    return NewDb;
                }
                else
                {
                    if (HttpContext.Current.Items["CurrentDb"] == null)
                    {
                        Database db = new Database(CurrentDbType);
                        HttpContext.Current.Items["CurrentDb"] = db; 
                        return db; 
                    }
                    return (Database)HttpContext.Current.Items["CurrentDb"];
                }
            }
        }
        /// <summary>
        /// 返回新数据库链接实例
        /// </summary>
        public static Database NewDb
        {
            get
            {
                return new Database(CurrentDbType);
            }
        }
        /// <summary>
        /// 获取当前数据库支持的操作,可定时器调用，没用使用到HttpContext.Current
        /// </summary>
        public static Database RuntimeDb
        {
            get
            {
                return CurrentDb;
                //if (HttpRuntime.Cache["RuntimeDb"] == null)
                //{
                //    CacheDependency cdy = new CacheDependency(HttpRuntime.AppDomainAppPath + "web.config");
                //    // Database db = new Database(ConfigurationManager.AppSettings.Get("UseDbType"));
                //    // HttpRuntime.Cache.Insert("RuntimeDb", db, cdy);
                //    HttpRuntime.Cache.Insert("RuntimeDb", ConfigurationManager.AppSettings.Get("UseDbType"), cdy);
                //}
                //// return (Database)HttpRuntime.Cache["RuntimeDb"];

                //return new Database(HttpRuntime.Cache["RuntimeDb"].ToString());
            }
        }

        
    }
}
