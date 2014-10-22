//--------------------------------------------------------------------------------
// 文件描述：系统栏目实体类
// 文件作者：张清山
// 创建日期：2014-08-31 23:02:06
// 修改记录： 
//--------------------------------------------------------------------------------
using System;
using HC.Repository;
namespace HC.Components.Model
{
    /// <summary> 
    /// 系统栏目实体类
    /// </summary>
    [TableName("HC_Column")]
    [PrimaryKey("Id")]
    [Serializable]
    public class Column : ModelBase
    {

        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 栏目名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary> 
        /// 上级栏目 
        /// </summary>
        public int ParentId { get; set; }

        /// <summary> 
        /// 栏目层级 
        /// </summary>
        public int Level { get; set; }

        /// <summary> 
        /// 是否前台显示 
        /// </summary>
        public bool IsShowFront { get; set; }

        /// <summary> 
        /// 栏目绑定模型Id 
        /// </summary>
        public int ModuleId { get; set; }

    }
}


