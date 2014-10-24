using System;
using HC.Foundation.Page;
using HC.Repository;

namespace HC.WebSite.Admin.Account
{
    public partial class AdminPurview : AdminPage
    {
        protected string Name = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Name = DbHelper.CurrentDb.ExecuteScalar<object>("SELECT LoginName FROM HC_Administrator WHERE Id=@0", RequestInt32("id")).ToString();

        }
    }
}