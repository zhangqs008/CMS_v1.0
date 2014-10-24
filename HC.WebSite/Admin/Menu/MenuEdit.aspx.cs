//--------------------------------------------------------------------------------
// 文件描述：系统菜单表添加、编辑页面
// 文件作者：张清山
// 创建日期：2014-08-12 14:41:38
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.IO;
using HC.Components.Service;
using HC.Foundation.Page;
using MenuInfo = HC.Components.Model.Menu;

namespace HC.WebSite.Admin.Menu
{
    public partial class MenuEdit : AdminPage
    {
        protected string Icons = "home.png";
        protected MenuInfo Instance  = new MenuInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestString("action");
            if (!IsPostBack)
            {
                dropParentId.DataSource = MenuService.Instance.GetListForDropDownList(0, "");
                dropParentId.DataTextField = "Name";
                dropParentId.DataValueField = "Id";
                dropParentId.DataBind();
                if (RequestInt32("parentId") > 0)
                {
                    dropParentId.SelectedValue = RequestString("parentId");
                }

                if (string.Compare(action, "modify", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    int id = RequestInt32("Id", 0);
                    Instance = MenuService.Instance.SingleOrDefault<MenuInfo>(id);
                    Icons = Instance.Ico;
                }
            }
        }

        public string RenderIcos()
        {
            string html = "";
            string dir = Server.MapPath("~/Scripts/jquery-easyui-1.4/themes/icon-custom");
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                var fileInfo = new FileInfo(file);
                html +=
                    string.Format(
                        "<input type=\"radio\" name=\"icons\" style='vertical-align: middle;' value=\"{0}\" ><span>" +
                        "<img style='vertical-align: middle;margin-right:3px' src='{1}Scripts/jquery-easyui-1.4/themes/icon-custom/{0}'/ title='{0}'>" +
                        fileInfo.Name + "</span><br />{2}", fileInfo.Name, BasePath, Environment.NewLine);
            }
            return html;
        }
    }
}