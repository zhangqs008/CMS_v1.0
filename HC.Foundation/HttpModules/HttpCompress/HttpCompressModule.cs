//--------------------------------------------------------------------------------
// 文件描述：gzip模块
// 文件作者：全体开发人员
// 创建日期：2014-2-12
//--------------------------------------------------------------------------------

using System;
using System.IO;
using System.Web;

namespace HC.Foundation.HttpModules.HttpCompress
{
    /// <summary>
    ///     gzip模块
    /// </summary>
    public class HttpCompressModule : BaseHttpModule
    {
        private const string InstalledKey = "httpcompress.attemptedinstall";
        private static readonly object InstalledTag = new object();

        /// <summary>
        ///     构造函数
        /// </summary>
        public HttpCompressModule()
        {
            LoadEventList = EventOptions.BeginRequest | EventOptions.ReleaseRequestState |
                            EventOptions.PreSendRequestHeaders;
        }

        /// <summary>
        ///     Get ahold of a <see cref="CompressingFilter" /> for the given encoding scheme.
        ///     If no encoding scheme can be found, it returns null.
        /// </summary>
        /// <remarks>
        ///     See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.3 for details
        ///     on how clients are supposed to construct the Accept-Encoding header.  This
        ///     implementation follows those rules, though we allow the server to override
        ///     the preference given to different supported algorithms.  I'm doing this as
        ///     I would rather give the server control over the algorithm decision than
        ///     the client.  If the clients send up * as an accepted encoding with highest
        ///     quality, we use the preferred algorithm as specified in the config file.
        /// </remarks>
        /// <param name="schemes">schemes</param>
        /// <param name="output">输出流</param>
        /// <param name="prefs">参数设置</param>
        /// <returns>过滤规则</returns>
        public static CompressingFilter GetFilterForScheme(string[] schemes, Stream output, Settings prefs)
        {
            bool foundDeflate = false;
            bool foundGZip = false;
            bool foundStar = false;

            float deflateQuality = 0f;
            float gzipQuality = 0f;
            float starQuality = 0f;

            foreach (string t in schemes)
            {
                string acceptEncodingValue = t.Trim().ToLower();

                if (acceptEncodingValue.StartsWith("deflate"))
                {
                    foundDeflate = true;

                    float newDeflateQuality = GetQuality(acceptEncodingValue);
                    if (deflateQuality < newDeflateQuality)
                    {
                        deflateQuality = newDeflateQuality;
                    }
                }
                else if (acceptEncodingValue.StartsWith("gzip") || acceptEncodingValue.StartsWith("x-gzip"))
                {
                    foundGZip = true;

                    float newGZipQuality = GetQuality(acceptEncodingValue);
                    if (gzipQuality < newGZipQuality)
                    {
                        gzipQuality = newGZipQuality;
                    }
                }
                else if (acceptEncodingValue.StartsWith("*"))
                {
                    foundStar = true;

                    float newStarQuality = GetQuality(acceptEncodingValue);
                    if (starQuality < newStarQuality)
                    {
                        starQuality = newStarQuality;
                    }
                }
            }

            bool isAcceptableStar = foundStar && (starQuality > 0);
            bool isAcceptableDeflate = (foundDeflate && (deflateQuality > 0)) || (!foundDeflate && isAcceptableStar);
            bool isAcceptableGZip = (foundGZip && (gzipQuality > 0)) || (!foundGZip && isAcceptableStar);

            if (isAcceptableDeflate && !foundDeflate)
            {
                deflateQuality = starQuality;
            }

            if (isAcceptableGZip && !foundGZip)
            {
                gzipQuality = starQuality;
            }

            // do they support any of our compression methods?
            if (!(isAcceptableDeflate || isAcceptableGZip || isAcceptableStar))
            {
                return null;
            }

            // if deflate is better according to client
            if (isAcceptableDeflate && (!isAcceptableGZip || (deflateQuality > gzipQuality)))
            {
                return new DeflateFilter(output, prefs.CompressionLevel);
            }

            // if gzip is better according to client
            if (isAcceptableGZip && (!isAcceptableDeflate || (deflateQuality < gzipQuality)))
            {
                return new GZipFilter(output);
            }

            // if we're here, the client either didn't have a preference or they don't support compression
            if (isAcceptableDeflate &&
                (prefs.PreferredAlgorithm == Algorithms.Deflate || prefs.PreferredAlgorithm == Algorithms.Default))
            {
                return new DeflateFilter(output, prefs.CompressionLevel);
            }

            if (isAcceptableGZip && prefs.PreferredAlgorithm == Algorithms.GZip)
            {
                return new GZipFilter(output);
            }

            return new DeflateFilter(output, prefs.CompressionLevel);
        }

        public new void Dispose()
        {
        }

        /// <summary>
        ///     响应应用程序请求开始事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal override void Application_BeginRequest(object source, EventArgs e)
        {
            var app = (HttpApplication) source;
            if (app.Request["HTTP_X_MICROSOFTAJAX"] != null || app.Request["Anthem_CallBack"] != null)
            {
                app.Context.Items.Add(InstalledKey, InstalledTag);
            }
        }

        /// <summary>
        ///     当 ASP.NET 执行完成所有请求事件处理程序后发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal override void Application_ReleaseRequestState(object source, EventArgs e)
        {
            CompressContent(source);
        }

        /// <summary>
        ///     恰好在 ASP.NET 向客户端发送 HTTP 标头之前发生
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="e">事件参数</param>
        internal override void Application_PreSendRequestHeaders(object source, EventArgs e)
        {
            CompressContent(source);
        }

        private static float GetQuality(string acceptEncodingValue)
        {
            int qualityParam = acceptEncodingValue.IndexOf("q=", StringComparison.Ordinal);

            if (qualityParam >= 0)
            {
                float val = 0.0f;
                try
                {
                    val =
                        float.Parse(acceptEncodingValue.Substring(qualityParam + 2,
                                                                  acceptEncodingValue.Length - (qualityParam + 2)));
                }
                catch (FormatException)
                {
                }

                return val;
            }
            return 1;
        }

        /// <summary>
        ///     EventHandler that gets ahold of the current request context and attempts to compress the output.
        /// </summary>
        /// <param name="sender">
        ///     The <see cref="HttpApplication" /> that is firing this event.
        /// </param>
        private void CompressContent(object sender)
        {
            var app = (HttpApplication) sender;

            // only do this if we havn't already attempted an install.  This prevents PreSendRequestHeaders from
            // trying to add this item way to late.  We only want the first run through to do anything.
            // also, we use the context to store whether or not we've attempted an add, as it's thread-safe and
            // scoped to the request.  An instance of this module can service multiple requests at the same time,
            // so we cannot use a member variable.
            if (!app.Context.Items.Contains(InstalledKey) && app.Context.Error == null)
            {
                // log the install attempt in the HttpContext
                // must do this first as several IF statements
                // below skip full processing of this method
                app.Context.Items.Add(InstalledKey, InstalledTag);

                // get the config settings
                Settings settings = Settings.GetSettings();

                if (settings.CompressionLevel == CompressionLevels.None)
                {
                    // skip if the CompressionLevel is set to 'None'
                    return;
                }

                if (app.Request.ApplicationPath != null)
                {
                    string realPath = app.Request.Path.Remove(0, app.Request.ApplicationPath.Length);
                    if (realPath.StartsWith("/"))
                    {
                        realPath = realPath.Substring(1);
                    }

                    if (settings.IsExcludedPath(realPath))
                    {
                        // skip if the file path excludes compression
                        return;
                    }
                }

                if (settings.IsExcludedMimeType(app.Response.ContentType))
                {
                    // skip if the MimeType excludes compression
                    return;
                }

                // fix to handle caching appropriately
                // see http://www.pocketsoap.com/weblog/2003/07/1330.html
                // Note, this header is added only when the request
                // has the possibility of being compressed...
                // i.e. it is not added when the request is excluded from
                // compression by CompressionLevel, Path, or MimeType
                app.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

                // grab an array of algorithm;q=x, algorith;q=x style values
                string acceptedTypes = app.Request.Headers["Accept-Encoding"];

                // if we couldn't find the header, bail out
                if (acceptedTypes == null)
                {
                    return;
                }

                // the actual types could be , delimited.  split 'em out.
                string[] types = acceptedTypes.Split(',');

                CompressingFilter filter = GetFilterForScheme(types, app.Response.Filter, settings);

                if (filter == null)
                {
                    // if we didn't find a filter, bail out
                    return;
                }

                // if we get here, we found a viable filter.
                // set the filter and change the Content-Encoding header to match so the client can decode the response
                app.Response.Filter = filter;
            }
        }
    }
}