using System.Data;
using System.Globalization;
using HC.Repository;

namespace HC.Presentation
{
    public class VisitStatistic : PresentBase
    {
        /// <summary>
        ///     取得点击数
        /// </summary>
        /// <returns></returns>
        public string GetVisitCount()
        {
            return DbHelper.CurrentDb.ExecuteScalar<object>("SELECT COUNT(*) FROM HC_VisitStatistics").ToString();
        }

        /// <summary>
        ///     取得文章数
        /// </summary>
        /// <returns></returns>
        public string GetArticleCount()
        {
            return DbHelper.CurrentDb.ExecuteScalar<object>("SELECT COUNT(*) FROM HC_Article").ToString();
        }

        /// <summary>
        ///     取得评论数
        /// </summary>
        /// <returns></returns>
        public string GetCommentCount()
        {
            return DbHelper.CurrentDb.ExecuteScalar<object>("SELECT COUNT(*) FROM HC_Comment").ToString();
        }

        /// <summary>
        /// 取得阅读排行榜
        /// </summary>
        /// <returns></returns>
        public string GetTop10Article()
        {
            string html = "<ul class='top10Article'>";
            const string sql =
                "SELECT TOP 10 Id,Title,isnull(Hits,0)as Hits FROM HC_Article WHERE ISDEL=0 ORDER BY Hits DESC";
            DataTable items = DbHelper.CurrentDb.Query(sql).Tables[0];
            int count = 0;
            foreach (DataRow item in items.Rows)
            {
                count++;
                html +=
                    string.Format(
                        "<li><a href='{0}Item.aspx?itemId={1}&type=Article' target='_blank' title='{2}'>{3}({4})</a></li>",
                        BasePath, item["Id"], item["Title"],
                        count.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + "." +
                        (item["Title"].ToString().Length > 22
                             ? item["Title"].ToString().Substring(0, 22) + "..."
                             : item["Title"]), item["Hits"]);
            }

            html += "</ul>";
            return html;
        }
    }
}