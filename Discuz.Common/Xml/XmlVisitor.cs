using System;
using System.Text;
using System.Xml;

namespace Discuz.Common.Xml
{
    /// <summary>
    /// XmlNode�������ӿ�
    /// </summary>
    interface IXmlNodeVisitor
    {
        /// <summary>
        /// ���õ�ǰ���������
        /// </summary>
        /// <param name="__xmlNode">��ǰҪ���ʵĽڵ���Ϣ</param>
        void SetNode(XmlNode __xmlNode);
        /// <summary>
        /// ���ص�ǰ���������ý����Ϣ
        /// </summary>
        /// <param name="__nodeName"></param>
        /// <returns></returns>
        XmlNode GetNode(string __nodeName);
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="__nodeName">�ڵ�����</param>
        /// <returns></returns>
        string this[string __nodeName]{ get; set; } 
    }


    /// <summary>
    /// ���Խڵ�Value��������
    /// </summary>
    public class XmlNodeAttributeValueVisitor : IXmlNodeVisitor
    {
        private XmlNode xmlNode;
                public XmlNodeAttributeValueVisitor()
        { }


        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="nodename"></param>
        /// <returns></returns>
        public string this[string __nodeName]
        {
            get
            {
                return xmlNode.Attributes[__nodeName].Value;
            }
            set
            {
                xmlNode.Attributes[__nodeName].Value = value;
            }
        }

        /// <summary>
        /// ���õ�ǰ���������
        /// </summary>
        /// <param name="__xmlNode">��ǰҪ���ʵĽڵ���Ϣ</param>
        public void SetNode(XmlNode __xmlNode)
        {
            xmlNode = __xmlNode;
        }

        /// <summary>
        /// ���ص�ǰ���������ý����Ϣ
        /// </summary>
        /// <param name="__nodeName">�ڵ�����</param>
        /// <returns></returns>
        public XmlNode GetNode(string __nodeName)
        {
            return xmlNode.SelectSingleNode(__nodeName);
        }
    }



    /// <summary>
    /// Select�ڵ�InnerText��������
    /// </summary>
    public class XmlNodeInnerTextVisitor : IXmlNodeVisitor
    {
        private XmlNode xmlNode;

        public XmlNodeInnerTextVisitor()
        { }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="nodename"></param>
        /// <returns></returns>
        public string this[string __nodeName]
        {
            get
            {
                return xmlNode.SelectSingleNode(__nodeName).InnerText;
            }
            set
            {
                xmlNode.SelectSingleNode(__nodeName).InnerText = value;
            }
        }

        /// <summary>
        /// ���õ�ǰ���������
        /// </summary>
        /// <param name="__xmlNode">��ǰҪ���ʵĽڵ���Ϣ</param>
        public void SetNode(XmlNode __xmlNode)
        {
            xmlNode = __xmlNode;
        }

        /// <summary>
        /// ���ص�ǰ���������ý����Ϣ
        /// </summary>
        /// <param name="__nodeName">�ڵ�����</param>
        /// <returns></returns>
        public XmlNode GetNode(string __nodeName)
        {
            return xmlNode.SelectSingleNode(__nodeName);
        }
    }
}
