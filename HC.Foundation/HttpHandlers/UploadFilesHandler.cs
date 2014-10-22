
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using LitJson;

namespace HC.Foundation.HttpHandlers
{
    public class UploadFilesHandler : IHttpHandler
    {
        private HttpContext _context;

        /// <summary>
        ///   网站根目录路径，末尾已包含“/”
        /// </summary>
        public string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }

        public void ProcessRequest(HttpContext context)
        {
            //文件保存目录路径
            String savePath = "~/UploadFiles/";

            //文件保存目录URL
            String saveUrl = BasePath + "UploadFiles/";

            //定义允许上传的文件扩展名
            var extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp,ico");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            const int maxSize = 1000000;

            _context = context;

            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                ShowError("请选择文件。");
            }

            String dirPath = context.Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                ShowError("上传目录不存在。");
            }

            String dirName = context.Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                ShowError("目录名不正确。");
            }

            if (imgFile != null)
            {
                String fileName = imgFile.FileName;
                string extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    String fileExt = extension.ToLower();

                    if (imgFile.InputStream.Length > maxSize)
                    {
                        ShowError("上传文件大小超过限制。");
                    }

                    if (String.IsNullOrEmpty(fileExt) ||
                        Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                    {
                        ShowError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
                    }

                    //创建文件夹
                    dirPath += dirName + "/";
                    saveUrl += dirName + "/";
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                    dirPath += ymd + "/";
                    saveUrl += ymd + "/";
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    String newFileName =
                        DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) +
                        fileExt;
                    String filePath = dirPath + newFileName;

                    imgFile.SaveAs(filePath);

                    String fileUrl = (saveUrl + newFileName).Replace("//", "/");

                    var hash = new Hashtable();
                    hash["error"] = 0;
                    hash["url"] = fileUrl;
                    context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                    context.Response.Write(JsonMapper.ToJson(hash));
                }
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        private void ShowError(string message)
        {
            var hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            _context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            _context.Response.Write(JsonMapper.ToJson(hash));
            _context.Response.End();
        }
    }
}
