using System;
using HC.Foundation.Page;
using HC.Repository;

namespace HC.WebSite.Admin.Role
{
    public partial class RolePurview : AdminPage
    {
        protected string Name = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Name =
                DbHelper.CurrentDb.ExecuteScalar<object>("SELECT Name FROM HC_Role WHERE Id=@0",
                                                         RequestInt32("id")).ToString();
        }
    }
}