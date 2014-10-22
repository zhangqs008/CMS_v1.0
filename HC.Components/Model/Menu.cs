//--------------------------------------------------------------------------------
// 文件描述：系统菜单表实体类
// 文件作者：张清山
// 创建日期：2014-08-11 17:06:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Components.Model
{
    /// <summary>
    ///     系统菜单表实体类
    /// </summary>
    [TableName("HC_Menu")]
    [PrimaryKey("Id")]
    [Serializable]
    public class Menu : ModelBase
    {
        /// <summary>
        ///     Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     父级Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        ///     层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     所属系统枚举值:1会员系统，2项目系统，3工作单系统
        /// </summary>
        public int InternalSystem { get; set; }

        /// <summary>
        ///     图标
        /// </summary>
        public string Ico { get; set; }

        /// <summary>
        ///     链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { get; set; }
    }
}