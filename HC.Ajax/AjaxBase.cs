using System.Web;
using HC.Foundation;

namespace HC.Ajax
{
    public class AjaxBase
    {
        /// <summary>
        ///     网站根目录
        /// </summary>
        protected static string BasePath =
            VirtualPathUtility.AppendTrailingSlash(HCContext.Current.Context.Request.ApplicationPath);

        /// <summary>
        ///     当前登录用户，用于记录日志
        /// </summary>
        public static string CurrentUser
        {
            get
            {
                if (HCContext.Current != null)
                {
                    if (HCContext.Current.Admin.Identity.IsAuthenticated)
                    {
                        return HCContext.Current.Admin.Identity.Name;
                    }
                }
                return "游客";
            }
        }
    }
}