using System.Web;
using HC.Framework.Extension;

namespace HC.Presentation.Common
{
    /// <summary>
    ///     请求相关
    /// </summary>
    public class Request : PresentBase
    { 
        /// <summary>
        ///     取得查询字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return GetString(key, string.Empty);
        }

        /// <summary>
        ///     取得查询字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue)
        {
            if (HttpContext.Current.Request.QueryString[key] != null)
            {
                return HttpContext.Current.Request.QueryString[key];
            }
            return defaultValue;
        }

        /// <summary>
        ///     取得查询字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        /// <summary>
        ///     取得查询字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue)
        {
            if (HttpContext.Current.Request.QueryString[key] != null)
            {
                return HttpContext.Current.Request.QueryString[key].ToInt();
            }
            return defaultValue;
        }
    }
}