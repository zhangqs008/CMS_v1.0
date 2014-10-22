//--------------------------------------------------------------------------------
// 文件描述：Ajax请求处理类PangeHandlerFactory
// 文件作者：张清山
// 创建日期：2013-12-6 
//--------------------------------------------------------------------------------

using System.Web;
using System.Web.UI;

namespace HC.Ajax
{
    /// <summary>
    ///     实现重写PangeHandlerFactory
    /// </summary>
    public class AjaxPageHandlerFactory : PageHandlerFactory
    {
        /// <summary>
        ///     重写基类的方法，返回 System.Web.IHttpHandler 接口的实例以处理请求的资源。
        /// </summary>
        /// <param name="context">System.Web.HttpContext 类的实例</param>
        /// <param name="requestType">客户端使用的 HTTP 数据传输方法（GET 或 POST）</param>
        /// <param name="virtualPath">所请求资源的虚拟路径</param>
        /// <param name="path">所请求资源的 System.Web.HttpRequest.PhysicalApplicationPath 属性</param>
        /// <returns>处理该请求的新 System.Web.IHttpHandler；如果不存在，则为 null</returns>
        public override IHttpHandler GetHandler(HttpContext context, string requestType, string virtualPath, string path)
        {
            if (path.ToLower().Contains("ajaxpost.aspx"))
            {
                return new AjaxPostHandler();
            }
            else if (path.ToLower().Contains("ajaxget.aspx"))
            {
                return new AjaxGetHandler();
            }
            else if (path.ToLower().Contains("ajaxform.aspx"))
            {
                return new AjaxFormHandler();
            }

            return base.GetHandler(context, requestType, virtualPath, path);
        }
    }
}