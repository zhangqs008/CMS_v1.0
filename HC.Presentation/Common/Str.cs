using System.Text.RegularExpressions;

namespace HC.Presentation.Common
{
    public class Str : PresentBase
    {
        /// <summary>
        /// 将输入字符转换为大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ToUper(string input)
        {
            return input.ToUpper();
        }

        /// <summary>
        /// 将输入字符转换为小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ToLower(string input)
        {
            return input.ToLower();
        }

        /// <summary>
        ///     替换所有HTML标签为空
        /// </summary>
        /// <param name="input">The string whose values should be replaced.</param>
        /// <returns>A string.</returns>
        public static string RemoveHtml(string input)
        {
            var stripTags = new Regex("</?[a-z][^<>]*>", RegexOptions.IgnoreCase);
            return stripTags.Replace(input, string.Empty);
        }

        /// <summary>
        /// 截取指定长度的字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Substr(string input)
        {
            const int length = 20;
            if (input.Length > length)
            {
                return input.Substring(0, length);
            }
            return input;
        }

        /// <summary>
        /// 截取指定长度的字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Substr(string input, int length)
        {
            if (input.Length > length)
            {
                return input.Substring(0, length) + "...";
            }
            return input;
        }
    }
}