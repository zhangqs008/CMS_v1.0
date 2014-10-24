using System;
using System.Collections.Generic;
using HC.Components.Service;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Purview
{
    public partial class PurviewSort : AdminPage
    {
        protected string Html = "";
        protected int ParentId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ParentId = RequestInt32("id");
            if (!IsPostBack)
            {
                Html = "";
                List<Components.Model.Purview> purviews = PurviewService.Instance.GetListByParentId(ParentId);
                foreach (Components.Model.Purview purview in purviews)
                {
                    Html += string.Format("<li class=\"sortItem\" cid=\"{0}\">{1}</li>{2}", purview.Id, purview.Name,
                                          Environment.NewLine);
                }
            }
        }
    }
}