//--------------------------------------------------------------------------------
// 文件描述：站点配置实体类
// 文件作者：张清山
// 创建日期：2013-12-10 15:09:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;

namespace HC.Foundation
{
    /// <summary>
    ///     站点配置实体
    /// </summary>
    [Serializable]
    public class SiteInfo
    {
        /// <summary>
        ///     站点名称
        /// </summary>
        public string SiteName { get; set; }


        /// <summary>
        ///     用户身份票据有效期
        /// </summary>
        public int TicketTime { get; set; }

        /// <summary>
        ///     版权信息
        /// </summary>
        public string Copyright { get; set; }


        /// <summary>
        ///     管理员登录时，是否自动登录前台用户
        /// </summary>
        public bool AdminIsLogoutUser { get; set; }

        /// <summary>
        /// 站点域名（Cookie用到）
        /// </summary>
        public string MainDomain { get; set; }

        /// <summary>
        /// 网站Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 网站Icon
        /// </summary>
        public string Icon { get; set; }
    }
}