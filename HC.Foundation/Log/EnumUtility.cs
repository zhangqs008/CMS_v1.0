//--------------------------------------------------------------------------------
// 文件描述：枚举工具类
// 文件作者：张清山
// 创建日期：2014-8-11 17:20:23
// 修改记录： 
//--------------------------------------------------------------------------------


namespace HC.Foundation.Log
{
    /// <summary>
    ///     枚举工具类
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        ///     将枚举值转换成Html字符串
        /// </summary>
        /// <param name="enumType">日志分类</param>
        /// <returns></returns>
        public static string GetResource(this LogCategory enumType)
        {
            switch (enumType)
            {
                case LogCategory.None:
                    return "暂无分类";
                case LogCategory.Member:
                    return "用户相关";
                case LogCategory.Menu:
                    return "菜单相关";
                case LogCategory.System:
                    return "系统相关";
                case LogCategory.Organization:
                    return "组织架构";
                case LogCategory.DataBase:
                    return "数据库相关";
                case LogCategory.Team:
                    return "团队相关";

                default:
                    return "暂无枚举值";
            }
        }

        /// <summary>
        ///     将枚举值转换成Html字符串
        /// </summary>
        /// <param name="enumType">日志严重级别</param>
        /// <returns></returns>
        public static string GetResource(this LogPriority enumType)
        {
            switch (enumType)
            {
                case LogPriority.No:
                    return "暂无分类";
                case LogPriority.Info:
                    return "普通信息";
                case LogPriority.Exception:
                    return "异常信息";
                case LogPriority.Error:
                    return "错误信息";
                case LogPriority.Fatal:
                    return "严重错误";
                default:
                    return "暂无枚举值";
            }
        }
    }
}