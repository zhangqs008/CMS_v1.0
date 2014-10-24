using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HC.Foundation.Page;
using HC.Framework.Extension;
using HC.Repository;

namespace HC.WebSite
{
    public partial class DataInit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonSetUserPurview(object sender, EventArgs e)
        {
            var menus = DbHelper.CurrentDb.Query("SELECT Id FROM HC_Menu").Tables[0];
            var purviews = DbHelper.CurrentDb.Query("SELECT Id FROM HC_Purview").Tables[0];
            var users = txtUserName.Text.TrimEnd(',');
            var menuIds = "";
            foreach (DataRow row in menus.Rows)
            {
                menuIds += row["Id"] + ",";
            }
            var purviewIds = "";
            foreach (DataRow row in purviews.Rows)
            {
                purviewIds += row["Id"] + ",";
            }

            //设定初始化操作权限
            var setMemberPurview = @"UPDATE  HC_Administrator SET OperatePurview = '{0}' WHERE LoginName IN ({1})";
            setMemberPurview = setMemberPurview.FormatWith(purviewIds.TrimEnd(','), users);
            DbHelper.CurrentDb.Execute(setMemberPurview); 

            //设定初始化菜单
            var setMemberMenus = @"UPDATE  HC_Administrator SET MenuPurview = '{0}' WHERE LoginName IN ({1})";
            setMemberMenus = setMemberMenus.FormatWith(menuIds, users);
            DbHelper.CurrentDb.Execute(setMemberMenus);

            Response.Clear();
            Response.Write("操作成功！");
            Response.End();
        }
    }
}