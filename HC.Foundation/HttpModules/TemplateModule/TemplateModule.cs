using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Xml;
using HC.Framework.Helper;
using HC.Foundation;
using HC.Presentation;
using HC.TemplateEngine;

namespace HC.Foundation.HttpModules.TemplateModule
{
    public class TemplateModule : BaseHttpModule
    {
        private static readonly object Obj = new object();

        public TemplateModule()
        {
            LoadEventList = EventOptions.BeginRequest;
        }

        /// <summary>
        ///     消息提示模板
        /// </summary>
        public string MsgTemplate
        {
            get { return HttpContext.Current.Server.MapPath("~/Admin/Style/template/msg.html"); }
        }


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

        /// <summary>
        ///     读取模板配置
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetTemplateList()
        {
            var dic = new Dictionary<string, string>();
            if (SiteCache.Get(SiteCacheKey.ConfigFrontTemplate) != null)
            {
                dic = SiteCache.Get(SiteCacheKey.ConfigFrontTemplate) as Dictionary<string, string>;
            }
            else
            {
                lock (Obj)
                {
                    string config = HttpContext.Current.Server.MapPath("~/config/FrontTemplate.config");
                    string xml = FileHelper.ReadFile(config);
                    if (xml.Length > 0)
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(xml);
                        XmlNodeList nodes = doc.SelectNodes("root/page");
                        if (nodes != null)
                        {
                            foreach (XmlNode node in nodes)
                            {
                                if (node.Attributes != null)
                                {
                                    string path = node.Attributes["path"].Value;
                                    string template = node.Attributes["template"].Value;
                                    if (!dic.ContainsKey(path))
                                    {
                                        dic.Add(path.ToLower(), template);
                                    }
                                }
                            }
                        }
                    }
                    SiteCache.Insert(SiteCacheKey.ConfigFrontTemplate, dic, new CacheDependency(config));
                }
            }
            return dic;
        }

        internal override void Application_BeginRequest(object source, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            Dictionary<string, string> dic = GetTemplateList();
            string key = context.Request.Url.AbsolutePath.Replace("/", "").ToLower();
            if (dic.ContainsKey(key))
            {
                var sw = new Stopwatch();
                sw.Start();
                string path = context.Server.MapPath(dic[key]);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    //第一步：加载模板
                    ITemplate template = BuildManager.CreateTemplate(path);

                    //第二步：模板解析
                    var tempObjects = new Dictionary<string, PresentBase>();
                    if (SiteCache.Get(SiteCacheKey.AssemblyPresentation) != null)
                    {
                        tempObjects = SiteCache.Get(SiteCacheKey.AssemblyPresentation) as Dictionary<string, PresentBase>;
                    }
                    else
                    {
                        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (Assembly assembly in assemblies)
                        {
                            //反射调用展现层类库
                            if (assembly.FullName.Contains("HC.Presentation"))
                            {
                                Type[] classes = assembly.GetTypes();
                                foreach (Type type in classes)
                                {
                                    if (IsInherit(type, typeof(PresentBase)))
                                    {
                                        string name = type.Name.ToLower();
                                        var instance = Activator.CreateInstance(type) as PresentBase;
                                        tempObjects.Add(name, instance);
                                    }
                                }
                            }
                        }
                        var dirPath = HttpContext.Current.Server.MapPath("~/bin/HC.Presentation");
                        SiteCache.Insert(SiteCacheKey.AssemblyPresentation, tempObjects, new CacheDependency(dirPath));
                    }
                    if (tempObjects != null)
                    {
                        foreach (var pair in tempObjects)
                        {
                            template.Context.TempData[pair.Key] = pair.Value;
                        }
                        template.Context.TempData["siteconfig"] = SiteConfig.SiteInfo;
                        try
                        {
                            template.Render(context.Response.Output);
                        }
                        catch (Exception ex)
                        {
                            WriteErrMsg("对不起，模板解析异常：" + ex.Message);
                        }
                    }

                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                    #region 页面开启耗时统计

                    string enableDebug = ConfigurationManager.AppSettings["TemplateDebug"];
                    if (enableDebug.ToLower() == "true")
                    {
                        context.Response.Output.Write("<p class='_pageCreateTimespan'>（页面生成时间：" + elapsedTime + "）<p>");
                    }

                    #endregion

                    context.Response.End();
                }
                else
                {
                    WriteErrMsg(string.Format("对不起，模板：{0}未找到!", path));
                }
            }
        }

        /// <summary>
        ///     错误提示信息
        /// </summary>
        /// <param name="title"></param>
        public void WriteErrMsg(string title)
        {
            HttpContext.Current.Response.Clear();
            string html = FileHelper.ReadFile(MsgTemplate);
            html = html.Replace("{$title}", "错误提示信息");
            html = html.Replace("{$content}", "<span style='color:red'>" + title + "</span>");
            html = html.Replace("{$url}", "");
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        internal override void Application_OnError(object source, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Write("<html><body>");
            context.Response.Write("<h4>Code Error:</h4>");
            context.Response.Write("<div style=\"width:80%; height:200px; word-break:break-all\">");
            context.Response.Write(HttpUtility.HtmlEncode(context.Server.GetLastError().ToString()));
            context.Response.Write("</div>");
            context.Response.Write("</body></html>");
            context.Response.End();
        }
    }
}