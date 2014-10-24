using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HC.Components.AjaxGet;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Menu
{
    public partial class MenuJs : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Write("var _menus = eval(" + MenuGetHandler.InitTreeByParentId("1") + ");");
            Response.End();
        }
    }
}