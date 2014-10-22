//--------------------------------------------------------------------------------
// 文件描述：实体基类
// 文件作者：张清山
// 创建日期：2014-08-11 17:03:58
// 修改记录： 
//--------------------------------------------------------------------------------
using System;

namespace HC.Repository
{
    [Serializable]
    public class ModelBase
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public long Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 最后更新人ID
        /// </summary>
        public string UpdateUser { get; set; }
    }
}
