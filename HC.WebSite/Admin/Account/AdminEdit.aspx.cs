using System;
using HC.Foundation.Context.Principal;
using HC.Foundation.Page;

namespace HC.WebSite.Admin.Account
{
    public partial class AdminEdit : AdminPage
    {
        protected Administrator Instance = new Administrator();

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestString("action");
            if (!IsPostBack)
            {
                if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    int id = RequestInt32("Id", 0);
                    Instance = AdministratorService.Instance.SingleOrDefault<Administrator>(id);
                }
            }
        }
    }
}