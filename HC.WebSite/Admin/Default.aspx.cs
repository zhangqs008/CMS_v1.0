using System;
using HC.Foundation;
using HC.Foundation.Page;
using HC.Framework.Extension;

namespace HC.WebSite.Admin
{
    public partial class _Default : AdminPage
    {
        protected string CustomTheme = "gray";
        protected void Page_Load(object sender, EventArgs e)
        {
            var theme = HCContext.Current.Admin.AdministratorInfo.Theme;
            if (theme.IsNotEmpty())
            {
                CustomTheme = theme;
            }
        }

        public string GetLogoPath()
        {
            if (SiteConfig.SiteInfo.Logo.IsNotEmpty())
            {
                return BasePath + SiteConfig.SiteInfo.Logo.TrimStart('/');
            }
            return BasePath + "Style/images/logo.png";
        }

        public string GetIconPath()
        {
            if (SiteConfig.SiteInfo.Icon.IsNotEmpty())
            {
                return BasePath + SiteConfig.SiteInfo.Icon.TrimStart('/');
            }
            return BasePath + "favicon.ico";
        }
    }
}