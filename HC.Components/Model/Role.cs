//--------------------------------------------------------------------------------
// 文件描述：角色组织架构实体类
// 文件作者：张清山
// 创建日期：2014-08-13 10:09:13
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Components.Model
{
    /// <summary> 
    /// 角色组织架构实体类
    /// </summary>
    [TableName("HC_Role")]
    [PrimaryKey("Id")]
    [Serializable]
    public class Role : ModelBase
    {
        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 角色名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary> 
        /// 上级角色 
        /// </summary>
        public int ParentId { get; set; }

    }
}


