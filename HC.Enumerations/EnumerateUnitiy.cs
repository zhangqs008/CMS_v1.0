using HC.Enumerations.Content;

namespace HC.Enumerations
{
    public static class EnumerateUnitiy
    {
        /// <summary>
        ///     模型字段类型
        /// </summary> 
        /// <returns></returns>
        public static string GetResource(this ModuleFieldType enumType)
        {
            switch (enumType)
            {
                case ModuleFieldType.TextBox:
                    return "单行文本";
                case ModuleFieldType.TextArea:
                    return "多行文本";
                case ModuleFieldType.Radio:
                    return "单选按钮";
                case ModuleFieldType.CheckBox:
                    return "复选框";
                case ModuleFieldType.Select:
                    return "下拉选项";
                case ModuleFieldType.Editer:
                    return "编辑器";
                case ModuleFieldType.File:
                    return "文件上传";
                case ModuleFieldType.DataPicker:
                    return "时间选择器";
                default:
                    return "无枚举值";
            }
        }

    }
}