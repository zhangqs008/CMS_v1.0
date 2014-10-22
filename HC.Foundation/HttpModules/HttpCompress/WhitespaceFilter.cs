using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace HC.Foundation.HttpModules.HttpCompress
{
    public enum CompressOptions
    {
        GZip,
        Deflate,
        None
    }

    public class WhitespaceFilter : Stream
    {
        private readonly Stream _content;
        private readonly GZipStream _contentGZip;
        private readonly DeflateStream _contentDeflate;
        private CompressOptions _options;

        public WhitespaceFilter(Stream content, CompressOptions options)
        {
            if (options == CompressOptions.GZip)
            {
                _contentGZip = new GZipStream(content, CompressionMode.Compress);
                _content = _contentGZip;
            }
            else if (options == CompressOptions.Deflate)
            {
                _contentDeflate = new DeflateStream(content, CompressionMode.Compress);
                _content = _contentDeflate;
            }
            else
            {
                _content = content;
            }
            _options = options;
        }


        public override bool CanRead
        {
            get { return _content.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _content.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _content.CanWrite; }
        }

        public override long Length
        {
            get { return _content.Length; }
        }

        public override long Position
        {
            get { return _content.Position; }
            set { _content.Position = value; }
        }

        public override void Flush()
        {
            _content.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _content.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _content.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _content.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var data = new byte[count + 1];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            string s = Encoding.UTF8.GetString(data);
            s = Regex.Replace(s, "^\\s*", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            s = Regex.Replace(s, "\\r\\n", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            s = Regex.Replace(s, "<!--*.*?-->", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            byte[] outdata = Encoding.UTF8.GetBytes(s);
            _content.Write(outdata, 0, outdata.GetLength(0));
        }
    }
}