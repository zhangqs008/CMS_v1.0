//--------------------------------------------------------------------------------
// 文件描述：访问统计实体类
// 文件作者：张清山 
// 创建日期：2014-06-14 20:33:59
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Components.Model
{

    /// <summary> 
    /// 访问统计实体类
    /// </summary>
    [TableName("HC_VisitStatistics")]
    [PrimaryKey("Id")]
    [Serializable]
    public class VisitStatistics : ModelBase
    {
        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 页面地址 
        /// </summary>
        public string Url { get; set; }

        /// <summary> 
        /// IP地址 
        /// </summary>
        public string IP { get; set; }

        /// <summary> 
        /// IP定位城市 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Broswer { get; set; }



    }
}


