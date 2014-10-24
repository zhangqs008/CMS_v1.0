//--------------------------------------------------------------------------------
// 文件描述：站点配置
// 文件作者：张清山
// 创建日期：2014-08-13 10:14:46
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Foundation;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Configuration
{
    public partial class EmailConfig : AdminPage
    {
        protected Foundation.EmailConfig Instance = SiteConfig.EmailConfig; 
        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}