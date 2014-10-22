using System.Web;
using HC.Components.Model;

namespace HC.Components.Content
{
    public abstract class Control
    {
        protected ColumnField Field;
        protected ColumnFieldOption FieldOption;

        protected Control(ColumnField field, ColumnFieldOption option)
        {
            FieldOption = option;
            Field = field;
        }

        /// <summary>
        ///     网站根目录路径，末尾已包含“/”
        /// </summary>
        public static string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }

        public abstract string Render(string val);
    }
}