using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace HC.Framework.Helper
{
    /// <summary>
    ///     配置文件助手类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        ///     更新配置信息，将配置信息对象序列化至相应的配置文件中，文件格式为带签名的UTF-8
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
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件  
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
        ///     获取配置信息
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置信息</returns>
        public static T GetConfig<T>() where T : class, new()
        {
            Type configClassType = typeof(T);
            var configObject = new object();
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件  
            if (File.Exists(configFilePath))
            {
                using (var xmlTextReader = new XmlTextReader(configFilePath))
                {
                    var xmlSerializer = new XmlSerializer(configClassType);
                    configObject = xmlSerializer.Deserialize(xmlTextReader);
                }
            }
            var config = configObject as T;
            if (config == null)
            {
                return new T();
            }
            return config;
        }

        /// <summary>
        ///     获取配置文件的服务器物理文件路径
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置文件路径</returns>
        public static string GetConfigPath<T>()
        {
            var configDirPath = HttpContext.Current.Server.MapPath("~/config");
            if (!Directory.Exists(configDirPath))
            {
                Directory.CreateDirectory(configDirPath);
            }

            return Path.Combine(configDirPath, typeof(T).Name + ".config");
        }

        /// <summary>
        ///     写入配置文件
        /// </summary>
        /// <param name="config">配置文件内容</param>
        /// <param name="configPath">配置文件名称</param>
        public static void WriteConfig(String config, string configPath)
        {
            configPath = Path.Combine(Thread.GetDomain().BaseDirectory, configPath);
            if (File.Exists(configPath))
            {
                File.Delete(configPath);
            }
            using (StreamWriter w = File.AppendText(configPath))
            {
                w.WriteLine(config);
                w.Flush();
            }
        }

        /// <summary>
        ///     读取配置文件
        /// </summary> 
        /// <returns></returns>
        public static string ReadConfig<T>()
        {
            var configPath = GetConfigPath<T>();
            string configContent = string.Empty;
            if (File.Exists(configPath))
            {
                using (var sr = new StreamReader(configPath))
                {
                    configContent = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return configContent;
        }


        //序列化单个对象：Serialize<UserInfo>(info) ；反序列化：Deserialize(typeof(UserInfo), s) as UserInfo
        //序列化集合对象：Serialize<List<UserInfo>>(list)；反序列化：Deserialize(typeof(List<UserInfo>), s) as List<UserInfo>

        /// <summary>
        ///     序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            using (var sw = new StringWriter())
            {
                var xz = new XmlSerializer(t.GetType());
                xz.Serialize(sw, t);
                return sw.ToString();
            }
        }

        /// <summary>
        ///     反序列化为对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="s">对象序列化后的Xml字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string s)
        {
            using (var sr = new StringReader(s))
            {
                var xz = new XmlSerializer(type);
                return xz.Deserialize(sr);
            }
        }
    }
}