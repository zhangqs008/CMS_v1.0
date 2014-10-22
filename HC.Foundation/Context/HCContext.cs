//--------------------------------------------------------------------------------
// 文件描述：扩展Context类，保存一些网站通用信息和登录用户的信息
// 文件作者：张清山
// 创建日期：2014年7月2日20:14:10
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using HC.Foundation.Context.Principal;
using HC.Foundation.Page;
using HC.Framework;

namespace HC.Foundation
{
    /// <summary>
    ///     HCContext，扩展Context类，用来保存一些网站和用户的通用信息
    /// </summary>
    public class HCContext
    {
        /// <summary>
        /// </summary>
        private const string Datakey = "WhirContextStore";

        /// <summary>
        /// </summary>
        private readonly HttpContext _httpContext;

        /// <summary>
        /// </summary>
        private AdminPrincipal _admin;

        /// <summary>
        /// </summary>
        private Uri _currentUrl;

        /// <summary>
        /// </summary>
        private string _hostPath;

        /// <summary>
        /// </summary>
        private NameValueCollection _queryString;

        /// <summary>
        /// </summary>
        private string _returnurl;

        /// <summary>
        /// </summary>
        private string _siteurl;


        /// <summary>
        ///     自定义上下文
        /// </summary>
        /// <param name="context"></param>
        /// <param name="includeQs">是否包含QueryString</param>
        private HCContext(HttpContext context, bool includeQs)
        {
            _httpContext = context;

            if (includeQs)
            {
                Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url,
                           context.Request.RawUrl, string.Empty);
            }
            else
            {
                Initialize(null, context.Request.Url, context.Request.RawUrl, BasePage.BasePath);
            }
        }

        /// <summary>
        ///     从数据槽中返回一个实例化的上下文
        /// </summary>
        public static HCContext Current
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;
                HCContext context;
                if (httpContext != null)
                {
                    context = httpContext.Items[Datakey] as HCContext;
                }
                else
                {
                    context = Thread.GetData(GetSlot()) as HCContext;
                }

                if (context == null)
                {
                    context = new HCContext(httpContext, true);
                    SaveContextToStore(context);
                }

                return context;
            }
        }

        /// <summary>
        ///     获取远程客户端的IP主机地址
        /// </summary>
        public string UserHostAddress
        {
            get
            {
                if (Context != null && !string.IsNullOrEmpty(Context.Request.UserHostAddress))
                {
                    if (Context.Request.UserHostAddress.IsIP())
                    {
                        return Context.Request.UserHostAddress;
                    }
                }

                return "0.0.0.0";
            }
        }

        /// <summary>
        ///     网站路径
        /// </summary>
        public string HostPath
        {
            get
            {
                if (_hostPath == null)
                {
                    string portInfo = CurrentUri.Port == 80 ? string.Empty : ":" + CurrentUri.Port;
                    _hostPath = string.Format("{0}://{1}{2}", CurrentUri.Scheme, CurrentUri.Host, portInfo);
                }
                return _hostPath;
            }
        }

        /// <summary>
        ///     当前网站地址
        /// </summary>
        public Uri CurrentUri
        {
            get { return _currentUrl ?? (_currentUrl = new Uri("http://localhost/")); }

            set { _currentUrl = value; }
        }

        /// <summary>
        ///     网站地址
        /// </summary>
        public string Siteurl
        {
            get { return _siteurl; }
        }

        /// <summary>
        ///     原来地址
        /// </summary>
        public string Rawurl { get; set; }

        /// <summary>
        ///     来访地址
        /// </summary>
        public string Returnurl
        {
            get { return _returnurl ?? (_returnurl = QueryString["returnUrl"]); }

            set { _returnurl = value; }
        }

        /// <summary>
        ///     QueryString
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return _queryString; }
        }

        /// <summary>
        ///     是否重写网站地址
        /// </summary>
        public bool IsUrlRewritten { get; set; }

        /// <summary>
        ///     检查HttpContext是否为null,如果 HttpContext == null，则返回false
        /// </summary>
        public bool IsWebRequest
        {
            get { return Context != null; }
        }

        /// <summary>
        ///     后台管理员身份认证信息
        /// </summary>
        public AdminPrincipal Admin
        {
            get { return _admin ?? (_admin = new AdminPrincipal(new NoAuthenticateIdentity(), null)); }

            set { _admin = value; }
        }

        /// <summary>
        ///     Http请求上下文
        /// </summary>
        public HttpContext Context
        {
            get { return _httpContext; }
        }

        /// <summary>
        ///     用HttpContext创建一个PEContext实例
        ///     这个方法必需在一个HttpModule的Begin_Request中调用
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>HCContext</returns>
        public static HCContext Create(HttpContext context)
        {
            return Create(context, false);
        }

        /// <summary>
        ///     用HttpContext创建一个PEContext实例
        ///     这个方法必需在一个HttpModule的Begin_Request中调用
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="isRewritten">是否已重写</param>
        /// <returns>HCContext</returns>
        public static HCContext Create(HttpContext context, bool isRewritten)
        {
            var currentContext = new HCContext(context, true) {IsUrlRewritten = isRewritten};
            SaveContextToStore(currentContext);

            return currentContext;
        }

        /// <summary>
        ///     释放内存槽中的数据
        /// </summary>
        public static void Unload()
        {
            Thread.FreeNamedDataSlot(Datakey);
        }

        /// <summary>
        ///     存储上下文
        /// </summary>
        /// <param name="context">上下文</param>
        private static void SaveContextToStore(HCContext context)
        {
            if (context.IsWebRequest)
            {
                context.Context.Items[Datakey] = context;
            }
            else
            {
                Thread.SetData(GetSlot(), context);
            }
        }

        /// <summary>
        ///     得到内存槽中的数据
        /// </summary>
        /// <returns></returns>
        private static LocalDataStoreSlot GetSlot()
        {
            return Thread.GetNamedDataSlot(Datakey);
        }

        /// <summary>
        ///     初始化上下文
        /// </summary>
        /// <param name="qs">参数</param>
        /// <param name="uri">地址</param>
        /// <param name="rawUrl">原来地址</param>
        /// <param name="siteUri">网站地址</param>
        private void Initialize(NameValueCollection qs, Uri uri, string rawUrl, string siteUri)
        {
            _queryString = qs;
            _currentUrl = uri;
            Rawurl = rawUrl;
            _siteurl = siteUri;
        }
    }
}