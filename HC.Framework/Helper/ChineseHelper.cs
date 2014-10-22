using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HC.Framework.Helper
{
    /// <summary>
    /// 中文辅助类
    /// </summary>
    public class ChineseHelper
    {
        /// <summary>
        /// 取得字符串中的所有中文和英文对应字典
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns>中英文键值对</returns>
        public static Dictionary<string, string> GetChineseAndEnglishDictionary(string content)
        {
            var dic = new Dictionary<string, string>();
            List<string> chineseWordList = GetChineseString(content);
            foreach (string wd in chineseWordList)
            {
                string chineseStr = wd;
                string englishStr = TranslaterHelper.Translate(wd.Replace("\"", ""), "zh-cn", "en");
                if (!dic.ContainsKey(chineseStr))
                {
                    dic.Add(chineseStr.Trim(), englishStr.Replace(" ", "").Trim());
                }
            }
            return dic;
        }

        /// <summary>
        /// 取得字符串中的所有中文 
        /// </summary>
        /// <param name="content">原始串</param>
        /// <returns>原始串中的中文集合</returns>
        public static List<char> GetChineseChar(string content)
        {
            var chineseList = new List<char>();
            const string regStr = "[\u4e00-\u9fa5]+";
            var reg = new Regex(regStr, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (Match mc in reg.Matches(content))
            {
                char[] chars = mc.Groups[0].Value.ToCharArray();
                chineseList.AddRange(chars);
            }
            return chineseList;
        }

        /// <summary>
        /// 取得字符串中的所有中文 
        /// </summary>
        /// <param name="content">原始串</param>
        /// <returns>原始串中的中文集合</returns>
        public static List<string> GetChineseString(string content)
        {
            var chineseList = new List<string>();
            const string regStr = "[\u4e00-\u9fa5]+";
            var reg = new Regex(regStr, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (Match mc in reg.Matches(content))
            {
                chineseList.Add(mc.Groups[0].Value);
            }
            return chineseList;
        }
    }
}