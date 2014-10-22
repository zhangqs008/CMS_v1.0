//--------------------------------------------------------------------------------
// 文件描述：日志处理日志严重级别的枚举
// 文件作者：张清山
// 创建日期：2013-12-10 10:40:23
// 修改记录： 
//--------------------------------------------------------------------------------

namespace HC.Foundation.Log 
{
    /// <summary>
    /// 日志严重级别的枚举
    /// </summary>
    public enum LogPriority
    {

        /// <summary>
        /// 不设优先级
        /// </summary>
        No = 0,

        /// <summary>
        /// 普通信息
        /// </summary>
        Info = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Exception = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,

        /// <summary>
        /// 严重错误
        /// </summary>
        Fatal = 4,
    }
}