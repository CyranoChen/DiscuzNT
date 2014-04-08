using System;
using System.Xml.Serialization;

namespace Discuz.Config
{
	/// <summary>
	/// 论坛基本设置描述类, 加[Serializable]标记为可序列化
	/// </summary>
	[Serializable]
    public class MyAttachmentsTypeConfigInfo : IConfigInfo
    {
        [XmlArray("attachtypes")]
        public AttachmentType[] AttachmentType;
    }


    [Serializable]
    public class AttachmentType
    {
        public AttachmentType()
        { }


        [XmlElement("TypeName")]
        public string TypeName;

        [XmlElement("TypeId")]
        public int TypeId;

        [XmlElement("ExtName")]
        public string ExtName;



    }
}
