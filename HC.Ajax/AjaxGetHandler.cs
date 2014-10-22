using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace HC.Ajax
{
    public class AjaxGetHandler : AjaxBase, IHttpHandler, IRequiresSessionState
    {
        private static readonly Hashtable Hash = new Hashtable();

        public void ProcessRequest(HttpContext context)
        {
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
                if (
                    string.Compare(context.Request.Url.Host, context.Request.UrlReferrer.Host,
                                   StringComparison.OrdinalIgnoreCase) != 0)
                {
                    context.Response.Write("禁止跨域访问本文件");
                    context.Response.End();
                    return;
                }
            }

            if (context.Request.QueryString.Count > 0)
            {
                string action = context.Request.QueryString["_action"];
                var dic = new Dictionary<string, object>();
                foreach (string key in context.Request.QueryString.Keys)
                {
                    if (key.ToLower() != "_action" && key.ToLower() != "_t")
                    {
                        dic.Add(key, context.Request.QueryString[key]);
                    }
                }

                string[] typeArr = action.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (typeArr.Length == 2)
                {
                    string type = typeArr[0];
                    string method = typeArr[1];
                    var parms = new List<object>();
                    foreach (KeyValuePair<string, object> pair in dic)
                    {
                        parms.Add(pair.Value);
                    }
                    string result = PressRequest(action, type, method, parms);
                    WriteMsg(context, result);
                }
            }
            else
            {
                WriteMsg(context, "未指定参数");
            }
        }

        public bool IsReusable { get; set; }

        /// <summary>
        ///     判断类是否继承自指定类
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

        private static string PressRequest(string type, string className, string method,
                                           List<object> parms)
        {
            string result = "未找到类型：" + type;
            type = type.ToLower();
            try
            {
                #region 从缓存中读

                if (Hash.ContainsKey(type))
                {
                    var target = Hash[type] as AjaxGetHandler;
                    if (target != null)
                    {
                        MethodInfo function = target.GetType()
                                                    .GetMethod(method,
                                                               BindingFlags.Public | BindingFlags.IgnoreCase |
                                                               BindingFlags.Instance | BindingFlags.NonPublic |
                                                               BindingFlags.Static);
                        if (function != null)
                        {
                            result = (string)function.Invoke(target, parms.ToArray());
                        }
                    }
                }

                #endregion

                #region 反射调用

                else
                {
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly assembly in assemblies)
                    {
                        Type[] classes = assembly.GetTypes();
                        foreach (Type @class in classes)
                        {
                            if (IsInherit(@class, typeof(AjaxGetHandler)) &&
                                String.Compare(@class.Name, className, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                var target = Activator.CreateInstance(@class) as AjaxGetHandler;
                                if (target != null)
                                {
                                    Hash.Add(type.ToLower(), target);

                                    MethodInfo function = @class.GetMethod(method,
                                                                           BindingFlags.Public |
                                                                           BindingFlags.IgnoreCase |
                                                                           BindingFlags.Instance |
                                                                           BindingFlags.NonPublic |
                                                                           BindingFlags.Static);
                                    if (function != null)
                                    {
                                        return (string)function.Invoke(target, parms.ToArray());
                                    }
                                    return string.Format("在类型 {0} 中未找到方法:{1}", target.GetType(), method);
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private static void WriteMsg(HttpContext context, string message)
        {
            context.Response.Clear();
            context.Response.Write(message);
            context.Response.End();
        }

        /// <summary>
        ///  bug:处理Json串里的日期格式，日期格式字段序列化后，前台无法解析
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected static string ProcressJson(string data)
        {
            return data;
            //var regex = new Regex("(?i)new Date\\(\\d+\\)", RegexOptions.CultureInvariant | RegexOptions.Compiled);
            //return regex.Replace(data, "\"\"");
        }
    }
}