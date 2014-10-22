 
namespace HC.Foundation 
{
    /// <summary>
    /// 不通过认证Identity类，永远返回未认证
    /// </summary>
    public class AnonymousAuthenticateIdentity : System.Security.Principal.IIdentity
    {
        /// <summary>
        /// 认证类型
        /// </summary>
        public string AuthenticationType
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 是否通过认证
        /// </summary>
        public bool IsAuthenticated
        {
            get { return false; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get { return "Anonymous"; }
        }
    }
}
