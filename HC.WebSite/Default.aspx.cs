using System;
using System.Web.UI;

namespace HC.WebSite
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Default.aspx");
        }
    }
}