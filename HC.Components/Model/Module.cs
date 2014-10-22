//--------------------------------------------------------------------------------
// 文件描述：内容模型实体类
// 文件作者：张清山
// 创建日期：2014-08-30 12:01:42
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Components.Model
{
    /// <summary> 
    /// 内容模型实体类
    /// </summary>
    [TableName("HC_Module")]
    [PrimaryKey("Id")]
    [Serializable]
    public class Module : ModelBase
    {

        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 模型名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary> 
        /// 描述 
        /// </summary>
        public string Description { get; set; }

        /// <summary> 
        /// 对应物理表名 
        /// </summary>
        public string TableName { get; set; }

    }
}


