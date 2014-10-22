using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;

namespace HC.Framework 
{
    /// <summary>
    /// 新浪IP查询
    /// </summary>
    public static class IPHelper
    {
        /// <summary>
        /// 取得客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            HttpRequest request = HttpContext.Current.Request;
            const string headerKeyIP = "X-Forwarded-For";
            string ip = string.Empty;
            string ipHeader = request.Headers[headerKeyIP];

            if (!string.IsNullOrEmpty(ipHeader))
            {
                string[] ips = ipHeader.Split(',');
                foreach (string ipItem in ips)
                {
                    if ((!string.IsNullOrEmpty(ipItem)) && (!IsLocalIP(ipItem)))
                    {
                        ip = ipItem;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                string strHostName = Dns.GetHostName();
                ip = Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            return ip;
        }
        static bool IsLocalIP(string ip)
        {
            return ip.StartsWith("192.168.") || ip.StartsWith("172.16.") || ip.StartsWith("10.");
        }

        public static string GetIp138Data()
        {
            const string ip138ComIcAsp = "http://iframe.ip138.com/ic.asp"; //查询IP138得到您当前的外网IP
            var uri = new Uri(ip138ComIcAsp);
            WebRequest wr = WebRequest.Create(uri);
            Stream stream = wr.GetResponse().GetResponseStream();
            if (stream != null)
            {
                //外网IP
                var reader = new StreamReader(stream, Encoding.Default);
                string result = reader.ReadToEnd(); //读取网站的数据
                Match ip = Regex.Match(result, @"(?<=(\[))[\s\S]*?(?=(\]))");
                return ip.ToString();
            }
            return string.Empty;
        }

        public static IpDetail GetIpDetail()
        {
            var ip = GetClientIP();
            if (DataValidator.IsIP(ip))
            {
                return GetIpDetail(ip);
            }
            return new IpDetail();
        }

        /// <summary>
        /// 获取IP地址的详细信息，调用的接口为
        /// http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip={ip}
        /// </summary>
        /// <param name="ipAddress">请求分析得IP地址</param>
        /// <returns>IpUtils.IpDetail</returns>
        public static IpDetail GetIpDetail(string ipAddress)
        {
            string ip = ipAddress;
            Encoding sourceEncoding = Encoding.UTF8;
            using (var receiveStream = WebRequest.Create("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip=" + ipAddress).GetResponse().GetResponseStream())
            {
                if (receiveStream != null)
                    using (var sr = new StreamReader(receiveStream, sourceEncoding))
                    {
                        var readbuffer = new char[256];
                        int n = sr.Read(readbuffer, 0, readbuffer.Length);
                        int realLen = 0;
                        while (n > 0)
                        {
                            realLen = n;
                            n = sr.Read(readbuffer, 0, readbuffer.Length);
                        }
                        ip = ConvertToGb(sourceEncoding.GetString(sourceEncoding.GetBytes(readbuffer, 0, realLen)));
                    }
            }
            var ipDetail = new IpDetail();
            try
            {
                ipDetail = JavaScriptConvert.DeserializeObject<IpDetail>(ip);
                ipDetail.ip = ipAddress;
            }
            catch
            {
                ipDetail.city = "未知";
            }
            return ipDetail;
        }

        /// <summary>
        /// 把Unicode解码为普通文字
        /// </summary>
        /// <param name="unicodeString">要解码的Unicode字符集</param>
        /// <returns>解码后的字符串</returns>
        public static string ConvertToGb(string unicodeString)
        {
            Regex regex = new Regex(@"\\u\w{4}");
            MatchCollection matchs = regex.Matches(unicodeString);
            foreach (Match match in matchs)
            {
                string tempvalue = char.ConvertFromUtf32(Convert.ToInt32(match.Value.Replace(@"\u", ""), 16));
                unicodeString = unicodeString.Replace(match.Value, tempvalue);
            }
            return unicodeString;
        }
    }

    public class IpDetail
    {
        public string ret { get; set; }

        public string start { get; set; }

        public string end { get; set; }

        public string country { get; set; }

        public string province { get; set; }

        public string city { get; set; }

        public string district { get; set; }

        public string isp { get; set; }

        public string type { get; set; }

        public string desc { get; set; }

        public string ip { get; set; }
    }
}
