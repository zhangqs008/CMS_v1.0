//--------------------------------------------------------------------------------
// 文件描述：系统权限表实体类
// 文件作者：张清山
// 创建日期：2014-08-21 09:25:24
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Components.Model
{
    /// <summary> 
    /// 系统权限表实体类
    /// </summary>
    [TableName("HC_Purview")]
    [PrimaryKey("Id")]
    [Serializable]
    public class Purview : ModelBase
    {

        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 权限名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary> 
        /// 上级权限 
        /// </summary>
        public int ParentId { get; set; }

        /// <summary> 
        /// 权限层级 
        /// </summary>
        public int Level { get; set; }

        /// <summary> 
        /// 权限操作码 
        /// </summary>
        public string OperateCode { get; set; }

        /// <summary> 
        /// 权限描述 
        /// </summary>
        public string Description { get; set; }

    }
}


