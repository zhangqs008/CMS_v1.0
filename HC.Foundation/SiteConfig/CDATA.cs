//--------------------------------------------------------------------------------
// 文件描述：序列化反序列化自定义格式--CDATA
// 文件作者：张清山
// 创建日期：2013-12-10 15:08:27
// 修改记录：
//--------------------------------------------------------------------------------

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HC.Foundation 
{
    /// <summary>
    /// 序列化反序列化自定义格式--CDATA
    /// </summary>
    public class CDATA : IXmlSerializable
    {
        /// <summary>
        /// 
        /// </summary>
        private string _value;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CDATA()
        {
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="value">内容值</param>
        public CDATA(string value)
        {
            _value = value;
        }

        /// <summary>
        /// 内容值
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

        #region IXmlSerializable Members

        /// <summary>
        /// 获取自定义架构，此方法是保留方法。
        /// </summary>
        /// <returns>自定义架构</returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// 从对象的 XML 表示形式生成该对象。
        /// </summary>
        /// <param name="reader">对象从中进行反序列化的 System.Xml.XmlReader 流。</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            _value = reader.ReadElementContentAsString();
        }

        /// <summary>
        /// 将对象转换为其 XML 表示形式。
        /// </summary>
        /// <param name="writer">对象要序列化为的 System.Xml.XmlWriter 流。</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteCData(_value);
        }

        #endregion

        /// <summary>
        /// 将 CDATA 对象隐式转换成 内容 字符串。
        /// </summary>
        /// <param name="element">自定义格式</param>
        /// <returns>文本值</returns>
        public static implicit operator string(CDATA element)
        {
            return (element == null) ? null : element._value;
        }

        /// <summary>
        /// 将 内容 对象隐式转换成 CDATA 字符串。
        /// </summary>
        /// <param name="text">文本值</param>
        /// <returns>自定义格式</returns>
        public static implicit operator CDATA(string text)
        {
            return new CDATA(text);
        }

        /// <summary>
        /// 重写 获取CData节点的 内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value;
        }
    }
}