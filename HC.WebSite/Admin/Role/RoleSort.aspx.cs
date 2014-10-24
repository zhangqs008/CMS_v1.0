using System;
using System.Collections.Generic;
using System.Web.ApplicationServices;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Role
{
    public partial class RoleSort : AdminPage
    {
        protected string Html = "";
        protected int ParentId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ParentId = RequestInt32("id");
            if (!IsPostBack)
            {
                Html = "";
                List<Components.Model.Role> list = HC.Components.Service.RoleService.Instance.GetListByParentId(ParentId);
                foreach (Components.Model.Role department in list)
                {
                    Html += string.Format("<li class=\"sortItem\" cid=\"{0}\">{1}</li>{2}", department.Id,
                                          department.Name, Environment.NewLine);
                }
            }
        }
    }
}