using System;
using System.Collections.Generic;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Menu
{
    public partial class MenuSort : AdminPage
    {
        protected string Html = "";
        protected int ParentId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ParentId = RequestInt32("id");
            if (!IsPostBack)
            {
                Html = "";
                List<Components.Model.Menu> menus = MenuService.Instance.GetListByParentId(ParentId);
                foreach (Components.Model.Menu menu in menus)
                {
                    Html += string.Format("<li class=\"sortItem\" cid=\"{0}\">{1}</li>{2}", menu.Id, menu.Name,
                                          Environment.NewLine);
                }
            }
        }
    }
}