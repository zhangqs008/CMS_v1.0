//--------------------------------------------------------------------------------
// 文件描述：gzip模块
// 文件作者：全体开发人员
// 创建日期：2014-2-12
//--------------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HC.Foundation.HttpModules.HttpCompress
{
    /// <summary>
    ///     This is a little filter to support HTTP compression using GZip
    /// </summary>
    public class GZipFilter : CompressingFilter
    {
        /// <summary>
        ///     compression stream member
        ///     has to be a member as we can only have one instance of the
        ///     actual filter class
        /// </summary>
        private readonly GZipStream _stream;

        /// <summary>
        ///     Primary constructor.  Need to pass in a stream to wrap up with gzip.
        /// </summary>
        /// <param name="baseStream">The stream to wrap in gzip.  Must have CanWrite.</param>
        public GZipFilter(Stream baseStream)
            : base(baseStream, CompressionLevels.Normal)
        {
            _stream = new GZipStream(baseStream, CompressionMode.Compress);
        }

        /// <summary>
        ///     The Http name of this encoding.  Here, gzip.
        /// </summary>
        public override string ContentEncoding
        {
            get { return "gzip"; }
        }

        /// <summary>
        ///     Write content to the stream and have it compressed using gzip.
        /// </summary>
        /// <param name="buffer">The bytes to write</param>
        /// <param name="offset">The offset into the buffer to start reading bytes</param>
        /// <param name="count">The number of bytes to write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!HasWrittenHeaders)
            {
                WriteHeaders();
            }

            #region 移除空格和空行

            //string path = HttpContext.Current.Request.Path;
            //string extension = Path.GetExtension(path);
            //if (extension != null)
            //{
            //    HttpContext context = HttpContext.Current;
            //    extension = extension.ToLower();
            //    if (extension == ".aspx" || extension == ".html" || extension == ".htm")
            //    {
            //        if (context.Request.Url.AbsolutePath.ToLower().IndexOf("ajax.aspx", StringComparison.Ordinal) == -1 &&
            //            context.Request.Url.AbsolutePath.ToLower().IndexOf("verifiycode.aspx", StringComparison.Ordinal) ==
            //            -1
            //            )
            //        {
            //            var data = new byte[count + 1];
            //            Buffer.BlockCopy(buffer, offset, data, 0, count);
            //            string s = Encoding.UTF8.GetString(data);
            //            s = Regex.Replace(s, "^\\s*", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            //            s = Regex.Replace(s, "\\r\\n", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            //            s = Regex.Replace(s, "<!--*.*?-->", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            //            byte[] outdata = Encoding.UTF8.GetBytes(s);
            //            _stream.Write(outdata, 0, outdata.GetLength(0));
            //        }
            //    }
            //    else
            //    {
            //        _stream.Write(buffer, offset, count);
            //    }
            //}
            //else
            //{
            //    _stream.Write(buffer, offset, count);
            //}

            #endregion

            _stream.Write(buffer, offset, count);
        }

        /// <summary>
        ///     Closes this Filter and calls the base class implementation.
        /// </summary>
        public override void Close()
        {
            _stream.Close(); // this will close the gzip stream along with the underlying stream

            // no need for call to base.Close() here.
        }

        /// <summary>
        ///     Flushes the stream out to underlying storage
        /// </summary>
        public override void Flush()
        {
            _stream.Flush();
        }
    }
}