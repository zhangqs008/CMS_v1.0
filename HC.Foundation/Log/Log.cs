//--------------------------------------------------------------------------------
// 文件描述：日志信息实体类
// 文件作者：张清山
// 创建日期：2013-12-10 10:55:05
// 修改记录： 
//--------------------------------------------------------------------------------

using HC.Repository;

namespace HC.Foundation.Log 
{
    /// <summary>
    /// 日志实体类
    /// </summary>
    [TableName("HC_Log")]
    [PrimaryKey("Id")]
    public class Log : ModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public LogCategory Category { get; set; }

        /// <summary>
        /// 优先级别
        /// </summary>
        public LogPriority Priority { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string UserIP { get; set; }

        /// <summary>
        /// 异常源、堆栈跟踪等异常信息
        /// </summary>
        public string Source { get; set; }
    }
}