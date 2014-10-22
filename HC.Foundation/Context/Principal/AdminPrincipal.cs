//--------------------------------------------------------------------------------
// 文件描述：管理员用户对象的功能类
// 文件作者： 
// 创建日期：2014-8-11 20:36:59
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Web.Security;

namespace HC.Foundation.Context.Principal
{
    /// <summary>
    ///     管理员用户对象的基本功能类。
    /// </summary>
    [Serializable]
    public class AdminPrincipal : IPrincipal
    {
        /// <summary>
        /// </summary>
        private const string SuperAdminRoleId = "0";

        /// <summary>
        /// </summary>
        [NonSerialized]
        private Administrator _administratorInfo;

        /// <summary>
        /// </summary>
        private IIdentity _identity;

        /// <summary>
        /// </summary>
        private string[] _roles;

        /// <summary>
        /// </summary>
        [NonSerialized]
        private string _rolesArray;

        /// <summary>
        ///     构造函数
        /// </summary>
        public AdminPrincipal()
        {
        }

        /// <summary>
        ///     从 Identity 和角色名称数组（System.Security.Principal.GenericIdentity
        ///     表示的用户属于该数组）初始化 System.Security.Principal.GenericPrincipal 类的新实例。
        /// </summary>
        /// <param name="identity">Identity </param>
        /// <param name="roles">角色名称数组</param>
        public AdminPrincipal(IIdentity identity, string[] roles)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            _identity = identity;
            if (roles != null)
            {
                _roles = new string[roles.Length];
                for (int i = 0; i < roles.Length; i++)
                {
                    _roles[i] = roles[i];
                }
            }
        }

        /// <summary>
        ///     管理员名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        ///     是否超级管理员
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return IsInRole(SuperAdminRoleId); }
        }


        /// <summary>
        ///     角色名称数组
        /// </summary>
        public string Roles
        {
            get { return _rolesArray; }

            set
            {
                _rolesArray = value;
                if (!string.IsNullOrEmpty(value))
                {
                    _roles = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }

        /// <summary>
        ///     当前管理员信息
        /// </summary>
        public Administrator AdministratorInfo
        {
            get { return _administratorInfo; }
            set { _administratorInfo = value; }
        }

        #region IPrincipal Members

        /// <summary>
        ///     获取当前AdminPrincipal表示的用户的IIdentity。
        /// </summary>
        public IIdentity Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        /// <summary>
        ///     确定当前 AdminPrincipal 是否属于指定的角色数组。
        /// </summary>
        /// <param name="role">角色数组字符串</param>
        /// <returns>如果当前 AdminPrincipal 属于指定角色的成员，则为 true；否则为 false。</returns>
        public bool IsInRole(string role)
        {
            if ((role != null) && (_roles != null))
            {
                string[] rolesArr = role.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string roleId in rolesArr)
                {
                    if (_roles.Any(t => (!string.IsNullOrEmpty(t)) &&
                                        (string.Compare(t, roleId, StringComparison.OrdinalIgnoreCase) == 0)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        ///     从FormsAuthenticationTicket身份认证信息中创建一个AdminPrincipal
        /// </summary>
        /// <param name="ticket">FormsAuthenticationTicket身份认证信息</param>
        /// <returns></returns>
        public static AdminPrincipal CreatePrincipal(FormsAuthenticationTicket ticket)
        {
            try
            {
                var binaryFormatter = new BinaryFormatter();
                var memoryStream = new MemoryStream(Convert.FromBase64String(ticket.UserData));
                var adminPrincipal = (AdminPrincipal)binaryFormatter.Deserialize(memoryStream);
                memoryStream.Dispose();
                adminPrincipal.Identity = new FormsIdentity(ticket);
                return adminPrincipal;
            }
            catch (ArgumentNullException)
            {
                return new AdminPrincipal(new NoAuthenticateIdentity(), null);
            }
            catch (FormatException)
            {
                return new AdminPrincipal(new NoAuthenticateIdentity(), null);
            }
            catch (SerializationException)
            {
                return new AdminPrincipal(new NoAuthenticateIdentity(), null);
            }
        }

        /// <summary>
        ///     序列化成字符串
        /// </summary>
        /// <returns>返回序列化字符串</returns>
        public string SerializeToString()
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, this);
            string serializeString = Convert.ToBase64String(memoryStream.ToArray());
            memoryStream.Dispose();
            return serializeString;
        }
    }
}