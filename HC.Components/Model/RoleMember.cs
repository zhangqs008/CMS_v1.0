//--------------------------------------------------------------------------------
// 文件描述：成员角色关系表实体类
// 文件作者：张清山
// 创建日期：2014-08-14 16:03:38
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Components.Model
{
    /// <summary> 
    /// 成员角色关系表实体类
    /// </summary>
    [TableName("HC_RoleMember")]
    [PrimaryKey("Id")]
    [Serializable]
    public class RoleMember : ModelBase
    {

        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 管理员Id 
        /// </summary>
        public int AdminId { get; set; }

        /// <summary> 
        /// 角色Id 
        /// </summary>
        public int RoleId { get; set; }

    }
}


