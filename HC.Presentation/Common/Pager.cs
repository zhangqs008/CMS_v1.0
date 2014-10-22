using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HC.Framework.Extension;
using HC.Framework.Helper;
using HC.Repository;

namespace HC.Presentation.Common
{
    public class Pager : PresentBase
    {
        public string Init(string sql, int currentPage)
        {
            return Init(sql, currentPage, 10, string.Empty, "include/pager_default.html");
        }

        public string Init(string sql, int currentPage, int pagesize)
        {
            return Init(sql, currentPage, pagesize, string.Empty, "include/pager_default.html");
        }

        public string Init(string sql, int currentPage, int pagesize, string pageUrl)
        {
            return Init(sql, currentPage, pagesize, pageUrl, "include/pager_default.html");
        }

        public string AppendPageQueryString(string url, string query, int page)
        {
            var regex = new Regex("(?i)[\\?|&]page=(?<page>\\d+)", RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (regex.IsMatch(query))
            {
                string id = regex.Match(query).Value;
                string[] arr = id.Split('=');
                if (arr.Length == 2)
                {
                    id = arr[0] + "=" + page;
                }
                query = regex.Replace(query, id);
            }
            return url + query;
        }

        public string Init(string sql, int currentPage, int pagesize, string pageUrl, string templatePath)
        {
            string url = HttpContext.Current.Request.Url.LocalPath;
            string query = HttpContext.Current.Request.Url.Query;

            pageUrl = pageUrl.IsEmpty() ? url : pageUrl;

            templatePath = templatePath.Replace('/', '\\');
            string temp = TemplatePath.Combine(templatePath.TrimStart('\\'));
            if (File.Exists(temp))
            {
                string template = FileHelper.ReadFile(temp);
                var re = DbHelper.CurrentDb.ExecuteScalar<object>(sql);
                int count = re.ToInt();
                if (count > 0 & pagesize > 0)
                {
                    var pages = (int) Math.Ceiling((double) count/pagesize);
                    var pageItems = new StringBuilder();
                    //页数少于10页时，全部显示
                    if (pages < 10)
                    {
                        for (int i = 1; i <= pages; i++)
                        {
                            string html = i != currentPage
                                              ? "<a href='{0}' class='a_num' id='pageNum{1}'>{1}</a>"
                                              : "<a href='{0}' class='a_cur' id='pageNum{1}'>{1}</a>";
                            string tempurl = AppendPageQueryString(pageUrl, query, i);
                            pageItems.AppendFormat(html, tempurl, i).Append(Environment.NewLine);
                        }
                    }
                    else if (pages > 10)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            string html = i != currentPage
                                              ? "<a href='{0}' class='a_num' id='pageNum{1}'>{1}</a>"
                                              : "<a href='{0}' class='a_cur' id='pageNum{1}'>{1}</a>";
                            string tempurl = AppendPageQueryString(pageUrl, query, i);
                            pageItems.AppendFormat(html, tempurl, i).Append(Environment.NewLine);
                        }
                        pageItems.AppendFormat("...").Append(Environment.NewLine);
                        for (int i = 1; i <= pages - 4; i++)
                        {
                            string html = i != currentPage
                                              ? "<a href='{0}' class='a_num' id='pageNum{1}'>{1}</a>"
                                              : "<a href='{0}' class='a_cur' id='pageNum{1}'>{1}</a>";
                            string tempurl = AppendPageQueryString(pageUrl, query, i);
                            pageItems.AppendFormat(html, tempurl, i).Append(Environment.NewLine);
                        }
                    }

                    template = template.Replace("${pageItems}", pageItems.ToStr());

                    template = template.Replace("${current}", currentPage.ToStr());
                    template = template.Replace("${totalPage}", pages.ToStr());
                    template = template.Replace("${totalCount}", count.ToStr());
                    template = template.Replace("${firstPage}", AppendPageQueryString(pageUrl, query, 1));
                    template = template.Replace("${prevPage}",
                                                pageUrl + "?page=" + ((currentPage - 1) > 0 ? (currentPage - 1) : 1));
                    template = template.Replace("${nextPage}",
                                                pageUrl + "?page=" +
                                                ((currentPage + 1) > pages ? pages : currentPage + 1));
                    template = template.Replace("${lastPage}", AppendPageQueryString(pageUrl, query, pages));
                    return template;
                }
                return string.Empty;
            }
            return "文件：" + temp + "不存在！";
        }
    }
}