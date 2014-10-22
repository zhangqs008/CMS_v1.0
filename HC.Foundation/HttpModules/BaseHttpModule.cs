//--------------------------------------------------------------------------------
// 文件描述：HttpModule基类
// 文件作者：全体开发人员
// 创建日期：2014-2-12
//--------------------------------------------------------------------------------

using System;
using System.Web;

namespace HC.Foundation.HttpModules
{
    /// <summary>
    ///     BaseHttpModule基类
    /// </summary>
    public class BaseHttpModule : IHttpModule
    {
        /// <summary>
        ///     网站根目录路径，末尾已包含“/”
        /// </summary>
        public static string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        public BaseHttpModule()
        {
            Configurable = true;
            LoadEventList = EventOptions.All;
        }

        /// <summary>
        ///     模块是否根据配置执行
        /// </summary>
        public bool Configurable { get; set; }

        /// <summary>
        ///     事件枚举列表
        /// </summary>
        public EventOptions LoadEventList { get; set; }

        #region IHttpModule Members

        /// <summary>
        ///     初始化模块，并使其为处理请求做好准备。
        /// </summary>
        /// <param name="context">System.Web.HttpApplication，提供对 ASP.NET 应用程序内所有应用程序对象的公用的方法、属性和事件的访问</param>
        public virtual void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (LoadEventList.Contains(EventOptions.BeginRequest))
            {
                context.BeginRequest += Application_BeginRequest_Handle;
            }

            if (LoadEventList.Contains(EventOptions.AuthenticateRequest))
            {
                context.AuthenticateRequest += Application_AuthenticateRequest_Handle;
            }

            if (LoadEventList.Contains(EventOptions.PostAuthenticateRequest))
            {
                context.PostAuthenticateRequest += Application_PostAuthenticateRequest_Handle;
            }

            if (LoadEventList.Contains(EventOptions.AcquireRequestState))
            {
                context.AcquireRequestState += Application_AcquireRequestState_Handle;
            }

            if (LoadEventList.Contains(EventOptions.PreRequestHandlerExecute))
            {
                context.PreRequestHandlerExecute += Application_PreRequestHandlerExecute_Handle;
            }

            if (LoadEventList.Contains(EventOptions.PostRequestHandlerExecute))
            {
                context.PostRequestHandlerExecute += Application_PostRequestHandlerExecute_Handle;
            }

            if (LoadEventList.Contains(EventOptions.ReleaseRequestState))
            {
                context.ReleaseRequestState += Application_ReleaseRequestState_Handle;
            }

            if (LoadEventList.Contains(EventOptions.EndRequest))
            {
                context.EndRequest += Application_EndRequest_Handle;
            }

            if (LoadEventList.Contains(EventOptions.PreSendRequestHeaders))
            {
                context.PreSendRequestHeaders += Application_PreSendRequestHeaders_Handle;
            }

            if (LoadEventList.Contains(EventOptions.Error))
            {
                context.Error += Application_OnError_Handle;
            }
        }

        /// <summary>
        ///     处置模块使用的资源（内存除外）
        /// </summary>
        public virtual void Dispose()
        {
        }

        #endregion

        /// <summary>
        ///     在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_BeginRequest(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     当安全模块已建立用户标识时发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_AuthenticateRequest(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     当安全模块已建立用户标识时发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_PostAuthenticateRequest(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     当ASP.NET获取当前请求所关联的当前状态（如Session）时执行
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_AcquireRequestState(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     恰好在 ASP.NET 开始执行事件处理程序前发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_PreRequestHandlerExecute(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     在ASP.NET 事件处理程序执行完毕时发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_PostRequestHandlerExecute(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     当 ASP.NET 执行完成所有请求事件处理程序后发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_ReleaseRequestState(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_EndRequest(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     恰好在 ASP.NET 向客户端发送 HTTP 标头之前发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_PreSendRequestHeaders(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     异常处理事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal virtual void Application_OnError(object source, EventArgs e)
        {
        }

        /// <summary>
        ///     在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_BeginRequest_Handle(object source, EventArgs e)
        {
            Application_BeginRequest(source, e);
        }

        /// <summary>
        ///     当安全模块已建立用户标识时发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_AuthenticateRequest_Handle(object source, EventArgs e)
        {
            Application_AuthenticateRequest(source, e);
        }

        /// <summary>
        ///     当安全模块已建立用户标识时发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_PostAuthenticateRequest_Handle(object source, EventArgs e)
        {
            Application_PostAuthenticateRequest(source, e);
        }

        /// <summary>
        ///     当ASP.NET获取当前请求所关联的当前状态（如Session）时执行
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_AcquireRequestState_Handle(object source, EventArgs e)
        {
            Application_AcquireRequestState(source, e);
        }

        /// <summary>
        ///     恰好在 ASP.NET 开始执行事件处理程序前发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_PreRequestHandlerExecute_Handle(object source, EventArgs e)
        {
            Application_PreRequestHandlerExecute(source, e);
        }

        /// <summary>
        ///     在ASP.NET 事件处理程序执行完毕时发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_PostRequestHandlerExecute_Handle(object source, EventArgs e)
        {
            Application_PostRequestHandlerExecute(source, e);
        }

        /// <summary>
        ///     当 ASP.NET 执行完成所有请求事件处理程序后发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_ReleaseRequestState_Handle(object source, EventArgs e)
        {
            Application_ReleaseRequestState(source, e);
        }

        /// <summary>
        ///     在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_EndRequest_Handle(object source, EventArgs e)
        {
            Application_EndRequest(source, e);
        }

        /// <summary>
        ///     恰好在 ASP.NET 向客户端发送 HTTP 标头之前发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_PreSendRequestHeaders_Handle(object source, EventArgs e)
        {
            Application_PreSendRequestHeaders(source, e);
        }

        /// <summary>
        ///     异常处理事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        private void Application_OnError_Handle(object source, EventArgs e)
        {
            Application_OnError(source, e);
        }
    }
}