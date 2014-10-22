using System;
using HC.Ajax;
using HC.Components.Service;

namespace HC.Components.AjaxGet
{
    public class ColumnGetHandler : AjaxGetHandler
    {
        public static string InitTree()
        {
            string tree;
            try
            {
                return ColumnService.Instance.GetJqueryEasyUiTree();
            }
            catch (Exception ex)
            {
                tree = ex.Message;
            }
            return tree;
        }
    }
}