using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace HC.Foundation.HttpModules.UrlRewrite
{
    internal class UrlRewriteModule : BaseHttpModule
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public UrlRewriteModule()
        {
            LoadEventList = EventOptions.BeginRequest;
        }

        /// <summary>
        ///     响应应用程序请求开始事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal override void Application_BeginRequest(object source, EventArgs e)
        {
            var rewrite = false;
            var application = (HttpApplication)source;
            HttpContext context = application.Context;
            string path = context.Request.Path;
            string file = Path.GetFileName(path);
            if (file != null && HttpContext.Current != null)
            {
                var rewriteConfig = HttpContext.Current.Server.MapPath("~/Config/RewriterConfig.config");
                if (File.Exists(rewriteConfig))
                {
                    var xml = new XmlDocument();
                    xml.Load(rewriteConfig);
                    var rules = xml.SelectNodes("RewriterConfig/Rules/RewriterRule");
                    if (rules != null)
                    {
                        foreach (XmlNode rule in rules)
                        {
                            var lookFor = "";
                            var sendTo = "";
                            var lookForNode = rule.SelectSingleNode("LookFor");
                            if (lookForNode != null)
                            {
                                lookFor = lookForNode.InnerText;
                            }
                            var sendToNode = rule.SelectSingleNode("SendTo");
                            if (sendToNode != null)
                            {
                                sendTo = sendToNode.InnerText;
                            }
                            if (!string.IsNullOrEmpty(lookFor) && !string.IsNullOrEmpty(sendTo))
                            {
                                var regeRule = Regex.Escape(lookFor);
                                var regex = new Regex("^(?i)" + regeRule + "$", RegexOptions.Compiled);
                                if (regex.Match(file).Success)
                                {
                                    context.RewritePath(sendTo);
                                    rewrite = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (!rewrite)
            {
                base.Application_BeginRequest(source, e);
            }
        }
    }
}