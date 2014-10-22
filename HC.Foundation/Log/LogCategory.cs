//--------------------------------------------------------------------------------
// 文件描述：日志分类
// 文件作者：张清山
// 创建日期：2013-12-10 
// 修改记录： 
//--------------------------------------------------------------------------------

namespace HC.Foundation.Log
{
    /// <summary>
    /// 日志分类
    /// </summary>
    public enum LogCategory
    {
        /// <summary>
        /// 没分类
        /// </summary>
        None = 0,

        /// <summary>
        /// 系统相关
        /// </summary>
        System = 1,

        /// <summary>
        /// 数据库相关
        /// </summary>
        DataBase = 2,

        /// <summary>
        /// 用户相关
        /// </summary>
        Member = 3,

        /// <summary>
        /// 菜单相关
        /// </summary>
        Menu = 4,

        /// <summary>
        /// 团队相关
        /// </summary>
        Team = 5,

        /// <summary>
        /// 组织架构
        /// </summary>
        Organization = 7,
    }
}