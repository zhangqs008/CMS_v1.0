//--------------------------------------------------------------------------------
// 文件描述：缓存实体类
// 文件作者：张清山
// 创建日期：2013-12-10 14:58:24
// 修改记录： 
//--------------------------------------------------------------------------------

namespace HC.Foundation 
{
    /// <summary>
    /// 缓存实体
    /// </summary>
    public class CacheInfo
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        public string Key { set; get; }

        /// <summary>
        /// 缓存对象值
        /// </summary>
        public string Value { set; get; }
    }
}