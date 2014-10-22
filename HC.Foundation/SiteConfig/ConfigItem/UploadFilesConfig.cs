namespace HC.Foundation 
{
    /// <summary>
    /// 平台上传文件配置
    /// </summary>
    public class UploadFilesConfig
    {
        /// <summary>
        /// 允许的文件类型，如：*.jpg|*.png
        /// </summary>
        public string AllowFileType { get; set; }
        /// <summary>
        /// 最大允许文件大小单位 byte
        /// </summary>
        public long MaxFileSize { get; set; }
        /// <summary>
        /// 是否重命名
        /// </summary>
        public bool IsReName { get; set; }
        /// <summary>
        /// 上传文件名方式，0：按日期时间存入，1：按随机数存入
        /// </summary>
        public int ReNameType { get; set; } 
    }
}
