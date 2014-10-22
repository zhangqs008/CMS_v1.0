//--------------------------------------------------------------------------------
// 文件描述：网站信息配置类
// 文件作者：张清山
// 创建日期：2013-12-10 14:54:01
// 修改记录：
//--------------------------------------------------------------------------------


namespace HC.Foundation
{
    /// <summary>
    /// 网站信息配置类
    /// </summary>
    public sealed class SiteConfig : BaseSiteConfig
    {
        /// <summary>
        /// 网站信息配置
        /// </summary>
        public static SiteInfo SiteInfo
        {
            get { return GetConfig<SiteInfo>(); }
        }
        
        /// <summary>
        /// 网站信息配置
        /// </summary>
        public static EmailConfig EmailConfig
        {
            get { return GetConfig<EmailConfig>(); }
        }


        /// <summary>
        /// 平台文件上传配置
        /// </summary>
        public static UploadFilesConfig UploadFilesConfig
        {
            get { return GetConfig<UploadFilesConfig>(); }
        }
    }
}