//--------------------------------------------------------------------------------
// 文件描述：配置文件父类
// 文件作者：张清山
// 创建日期：2013-12-10 15:05:59
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.IO;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;

namespace HC.Foundation  
{
    /// <summary>
    /// 配置文件父类
    /// </summary>
    public class BaseSiteConfig
    {
        /// <summary>
        /// 更新配置信息，将配置信息对象序列化至相应的配置文件中，文件格式为带签名的UTF-8
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <param name="config">配置信息</param>
        public static void UpdateConfig<T>(T config)
        {
            if (config == null)
            {
                return;
            }

            Type configClassType = typeof(T);
            string configFilePath = GetConfigPath<T>();
            try
            {
                var xmlSerializer = new XmlSerializer(configClassType);
                using (var xmlTextWriter = new XmlTextWriter(configFilePath, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    var xmlNamespace = new XmlSerializerNamespaces();
                    xmlNamespace.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(xmlTextWriter, config, xmlNamespace);
                }


            }
            catch (SecurityException ex)
            {
                throw new SecurityException(ex.Message, ex.DenySetInstance, ex.PermitOnlySetInstance, ex.Method,
                                            ex.Demanded, ex.FirstPermissionThatFailed);
            }
        }

        /// <summary>
        /// 获取配置文件的服务器物理文件路径
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置文件路径</returns>
        private static string GetConfigPath<T>()
        {
            string path = HttpContext.Current.Server.MapPath("~/Config/");
            return path + typeof(T).Name + ".config";
        }

        /// <summary>
        /// 获取配置信息，首先从缓存中读取，如果读取失败则从配置文件中反序列化配置对象并写入缓存
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置信息</returns>
        public static T GetConfig<T>() where T : class, new()
        {
            Type configClassType = typeof(T);
            string configCacheKey = "CK_SiteConfigCode_" + configClassType.Name;
            object configObject = SiteCache.Get(configCacheKey);
            if (configObject == null)
            {
                string configFilePath = GetConfigPath<T>();
                if (File.Exists(configFilePath))
                {
                    using (var xmlTextReader = new XmlTextReader(configFilePath))
                    {
                        var xmlSerializer = new XmlSerializer(configClassType);
                        configObject = xmlSerializer.Deserialize(xmlTextReader);
                    }

                    SiteCache.Insert(configCacheKey, configObject,
                                     new CacheDependency(configFilePath));
                }
            }

            var config = configObject as T;
            if (config == null)
            {
                return new T();
            }
            return config;
        }
    }
}