using System;
using System.Web;
using System.Web.UI;
using HC.Foundation;
using HC.Framework.Extension;

namespace HC.WebSite.Admin
{
    public partial class Admin : MasterPage
    {
        protected string CustomTheme = "gray";

        /// <summary>
        ///     网站根目录路径，末尾已包含“/”
        /// </summary>
        public static string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HCContext.Current.Admin.Identity.IsAuthenticated)
            {
                string theme = HCContext.Current.Admin.AdministratorInfo.Theme;
                if (!theme.IsEmpty())
                {
                    CustomTheme = theme;
                }
            }
        }
    }
}