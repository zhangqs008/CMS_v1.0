//--------------------------------------------------------------------------------
// 文件描述：系统管理员实体类
// 文件作者：张清山
// 创建日期：2014-08-14 16:13:44
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Foundation.Context.Principal
{
    /// <summary> 
    /// 系统管理员实体类
    /// </summary>
    [TableName("HC_Administrator")]
    [PrimaryKey("Id")]
    [Serializable]
    public class Administrator : ModelBase
    {

        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 管理员名 
        /// </summary>
        public string LoginName { get; set; }

        /// <summary> 
        /// 用户名 
        /// </summary>
        public string TrueName { get; set; }

        /// <summary> 
        /// 密码 
        /// </summary>
        public string Password { get; set; }

        /// <summary> 
        /// 电话 
        /// </summary>
        public string Phone { get; set; }

        /// <summary> 
        /// 性别 
        /// </summary>
        public bool Sex { get; set; }

        /// <summary> 
        /// 生日 
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary> 
        /// 邮箱 
        /// </summary>
        public string Email { get; set; }

        /// <summary> 
        /// 最后登录时间 
        /// </summary>
        public DateTime? LastLogOffTime { get; set; }

        /// <summary> 
        /// 登录次数 
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// 菜单权限
        /// </summary>
        public string MenuPurview { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 用户操作权限
        /// </summary>
        public string OperatePurview { get; set; }
    }
}


