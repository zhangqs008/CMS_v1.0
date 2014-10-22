//--------------------------------------------------------------------------------
// 文件描述：Ajax请求处理类
// 文件作者：张清山
// 创建日期：2013-12-6 
//--------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using HC.Foundation;
using HC.Framework.Extension;

namespace HC.Ajax
{
    /// <summary>
    ///  Ajax请求处理类
    /// </summary>
    public class AjaxPostHandler : AjaxBase, IHttpHandler, IRequiresSessionState
    {
        private static string _outputFormat;

        #region IHttpHandler 成员

        /// <summary>
        ///     实现IHttpHandler接口
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        #endregion

        #region 实现IHttpHandler接口

        /// <summary>
        ///     实现IHttpHandler接口
        /// </summary>
        /// <param name="context">当前上下文环境，当前WhirContext.Current</param>
        public void ProcessRequest(HttpContext context)
        {
            var requestXml = new XmlDocument();
            try
            {
                requestXml.Load(context.Request.InputStream);
                _outputFormat = GetNodeInnerText(requestXml, "format");
            }
            catch (XmlException ex)
            {
                string notefilepath = "~/AppData/ajaxExceptionLog.txt";
                notefilepath = HCContext.Current.Context.Server.MapPath(notefilepath);
                context.Response.Write(File.Exists(notefilepath) ? File.ReadAllText(notefilepath) : ex.Message);
                return;
            }

            // 设置输出类型为纯文本
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.Charset = "utf-8";
            context.Response.AddHeader("contenttype", "text/plain");
            context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            context.Response.ContentType = "text/plain";

            if (context.Request.UrlReferrer != null)
            {
                // 判断访问来源
                if (string.Compare(context.Request.Url.Host, context.Request.UrlReferrer.Host,
                                   StringComparison.OrdinalIgnoreCase) != 0)
                {
                    var dic = new Dictionary<string, object> { { "status", false }, { "body", "禁止跨域访问本文件" } };
                    ResponseResult(context, dic);
                    context.Response.End();
                    return;
                }
            }

            // 如果传入的XML有子节点
            if (requestXml.HasChildNodes)
            {
                Process(context, requestXml);
            }
            else
            {
                var dic = new Dictionary<string, object> { { "status", false }, { "body", "没有Post任何信息" } };
                ResponseResult(context, dic);
            }
            context.Response.End();
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dictionary"></param>
        public static void ResponseResult(HttpContext context, Dictionary<string, object> dictionary)
        {
            var dt = new DataTable { TableName = "result" };
            //列
            foreach (var pair in dictionary)
            {
                dt.Columns.Add(pair.Key);
            }
            //行
            DataRow row = dt.NewRow();
            foreach (var pair in dictionary)
            {
                row[pair.Key] = pair.Value;
            }
            dt.Rows.Add(row);
            var outputFormat = OutputFormat.Json;
            switch (_outputFormat.ToLower())
            {
                case "xml":
                    outputFormat = OutputFormat.Xml;
                    break;
            }
            string output = outputFormat == OutputFormat.Json
                                ? dt.ToJson()
                                : dt.ToXml();

            context.Response.Write(output);
        }

        #endregion

        #region 处理AjaxHandler请求

        private static readonly Hashtable Hash = new Hashtable();

        /// <summary>
        /// 判断类是否继承自指定类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static bool IsInherit(Type type, Type baseType)
        {
            if (type.BaseType == null) return false;
            if (type.BaseType == baseType) return true;
            return IsInherit(type.BaseType, baseType);
        }

        /// <summary>
        ///     创建AjaxHandler对象
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="requestXml">参数封装</param>
        /// <returns>IAjaxHandler接口</returns>
        internal static Dictionary<string, string> GetResult(string className, string methodName, XmlDocument requestXml)
        {
            if (Hash.ContainsKey(className))
            {
                var type = Hash[className] as Type;
                if (type != null)
                {
                    var hande = Activator.CreateInstance(type) as AjaxPostHandler;
                    var parms = new object[] { requestXml };
                    var result =
                        (Dictionary<string, string>)
                        type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, hande,
                                          parms);
                    return result;
                }
            }
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] classes = assembly.GetTypes();
                foreach (Type type in classes)
                {
                    if (IsInherit(type, typeof(AjaxPostHandler)) &&
                        String.Compare(type.Name, className, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var hande = Activator.CreateInstance(type) as AjaxPostHandler;
                        var parms = new object[] { requestXml };
                        var result =
                            (Dictionary<string, string>)
                            type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null,
                                              hande, parms);
                        Hash.Add(className, type);
                        return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     处理AjaxHandler请求 
        /// </summary>
        /// <param name="context">当前上下文环境</param>
        /// <param name="requestXml">XmlDocument对象</param>
        protected virtual void Process(HttpContext context, XmlDocument requestXml)
        {
            string querytype = GetNodeInnerText(requestXml, "_type");
            string[] type = querytype.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (type.Length == 2)
            {
                string className = type[0];
                string methodName = type[1];
                Dictionary<string, object> dic;
                try
                {
                    Dictionary<string, string> result = GetResult(className, methodName, requestXml);
                    dic = new Dictionary<string, object> { { "status", true } };
                    foreach (var pair in result)
                    {
                        if (!dic.ContainsKey(pair.Key))
                        {
                            dic.Add(pair.Key, pair.Value);
                        }
                        else
                        {
                            dic[pair.Key] = pair.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    dic = new Dictionary<string, object> { { "status", false }, { "body", ex.Message } };
                }
                ResponseResult(context, dic);
            }
            else
            {
                var dic = new Dictionary<string, object> { { "status", false }, { "body", "参数错误，参数需为：类名.方法名" } };
                ResponseResult(context, dic);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        ///     获取节点值
        /// </summary>
        /// <param name="xmldoc">XmlDocument</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>返回节点值</returns>
        public static string GetNodeInnerText(XmlDocument xmldoc, string nodeName)
        {
            string innerText = string.Empty;
            if (xmldoc.DocumentElement != null)
            {
                //忽略节点名称大小写
                XmlNode xmlNode =
                    xmldoc.DocumentElement.SelectSingleNode(
                        "//root/node()[translate(local-name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '" +
                        nodeName.ToLower() + "']");
                if (xmlNode != null && !string.IsNullOrEmpty(xmlNode.InnerText))
                {
                    innerText = XmlDecode(xmlNode.InnerText.Trim());
                }
            }

            return innerText;
        }

        public static string XmlDecode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("&amp;", "&");
                value = value.Replace("&lt;", "<");
                value = value.Replace("&gt;", ">");
                value = value.Replace("&apos;", "'");
                value = value.Replace("&quot;", "\"");
            }
            return value;
        }
        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetNodeInnerHtml(XmlDocument xmldoc, string nodeName)
        {
            string innerText = string.Empty;
            if (xmldoc.DocumentElement != null)
            {
                //忽略节点名称大小写
                XmlNode xmlNode =
                    xmldoc.DocumentElement.SelectSingleNode(
                        "//root/node()[translate(local-name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '" +
                        nodeName.ToLower() + "']");
                if (xmlNode != null && !string.IsNullOrEmpty(xmlNode.InnerXml))
                {
                    innerText = XmlDecode(xmlNode.InnerXml.Trim());
                }
            }

            return innerText;
        }

        /// <summary>
        ///     获取节点值
        /// </summary>
        /// <param name="xmldoc">XmlDocument</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回节点值</returns>
        public static string GetNodeInnerText(XmlDocument xmldoc, string nodeName, string defaultValue)
        {
            string innerText = GetNodeInnerText(xmldoc, nodeName);
            if (string.IsNullOrEmpty(innerText))
            {
                innerText = defaultValue;
            }
            return innerText;
        }

        /// <summary>
        /// 将文件路径转换为web路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileWebUrl(string filePath)
        {
            if (filePath.IsEmpty())
            {
                return string.Empty;
            }
            filePath = filePath.Replace("\\", "/");
            if (filePath.StartsWith("/"))
            {
                filePath = filePath.TrimStart('/');
            }
            return BasePath + filePath;
        }

        /// <summary>
        /// 删除目录所有文件，递归
        /// </summary>
        /// <param name="dir"></param>
        protected static void DeleteDir(string dir)
        {
            if (dir.Trim() == "" || !Directory.Exists(dir))
                return;
            var dirInfo = new DirectoryInfo(dir);

            FileInfo[] fileInfos = dirInfo.GetFiles();
            if (fileInfos.Length > 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    File.Delete(fileInfo.FullName); //删除文件
                }
            }

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            if (dirInfos.Length > 0)
            {
                foreach (DirectoryInfo childDirInfo in dirInfos)
                {
                    DeleteDir(childDirInfo.FullName); //递归
                    Directory.Delete(childDirInfo.FullName);
                }
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            try
            {
                using (var sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        ///     写入文件
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="content">文件内容</param>
        public static void WriteFile(string filePath, string content)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                Encoding encode = Encoding.UTF8;
                //获得字节数组
                byte[] data = encode.GetBytes(content);
                //开始写入
                stream.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                stream.Flush();
                stream.Close();
            }
        }

        /// <summary>
        /// 取得板块配置文件 指定单个值的节点，如：xmlNodePath：root/VisualConfig/Html/Columnid
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xmlNodePath"></param>
        /// <returns></returns>
        public static string GetConfigSingleNode(string xml, string xmlNodePath)
        {
            if (xml != null)
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode field = doc.SelectSingleNode(xmlNodePath.ToLower());
                if (field != null)
                {
                    return field.InnerXml.Trim();
                }
            }
            return string.Empty;
        }

        #endregion

    }

    /// <summary>
    /// 输出内容格式
    /// </summary>
    public enum OutputFormat
    {
        Json = 1,
        Xml = 2
    }
}