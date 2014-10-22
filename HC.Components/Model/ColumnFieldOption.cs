//--------------------------------------------------------------------------------
// 文件描述：栏目字段配置实体类
// 文件作者：张清山
// 创建日期：2014-08-31 22:23:06
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using HC.Repository;

namespace HC.Components.Model
{
    /// <summary> 
    /// 栏目字段配置实体类
    /// </summary>
    [TableName("HC_ColumnFieldOption")]
    [PrimaryKey("Id")]
    [Serializable]
    public class ColumnFieldOption : ModelBase
    {
        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 栏目Id 
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary> 
        /// 栏目字段Id 
        /// </summary>
        public int FieldId { get; set; }

        /// <summary> 
        /// 模型Id 
        /// </summary>
        public int ModuleId { get; set; }

        #region 单行文本

        /// <summary> 
        /// 单行文本-长度 
        /// </summary>
        public int TextBoxLength { get; set; }

        /// <summary>
        /// 单行文本-是否允许为空
        /// </summary>
        public bool TextBoxAllowNull { get; set; }

        /// <summary> 
        /// 单行文本-验证规则 
        /// </summary>
        public string TextBoxRegex { get; set; }

        #endregion

        #region 多行文本

        /// <summary> 
        /// 多行文本-宽度 
        /// </summary>
        public int TextAreaWidth { get; set; }

        /// <summary> 
        /// 多行文本-高度 
        /// </summary>
        public int TextAreaHeight { get; set; }

        /// <summary>
        ///多行文本- 是否允许为空
        /// </summary>
        public bool TextAreaAllowNull { get; set; }

        #endregion

        #region 单选按钮

        /// <summary> 
        /// 单选按钮-文本 
        /// </summary>
        public string RadioText { get; set; }

        /// <summary> 
        /// 单选按钮-值 
        /// </summary>
        public string RadioValue { get; set; }

        #endregion

        #region 复选框

        /// <summary> 
        /// 复选框-值 
        /// </summary>
        public string CheckBoxText { get; set; }

        /// <summary> 
        /// 复选框-值 
        /// </summary>
        public string CheckBoxValue { get; set; }


        #endregion

        #region 编辑器

        /// <summary>
        /// 编辑器宽度
        /// </summary>
        public int EditerWidth { get; set; }

        /// <summary>
        /// 编辑器高度
        /// </summary>
        public int EditerHeight { get; set; }

        /// <summary>
        /// 编辑器模式
        /// </summary>
        public string EditerStyle { get; set; }

        #endregion
    }
}