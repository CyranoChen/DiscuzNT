using System.Xml;

namespace Discuz.Forum
{
    /// <summary>
    /// ��̳ģ�������
    /// </summary>
    public class SpaceThemes
    {
        /// <summary>
        /// ģ��˵���ṹ, ÿ��ģ��Ŀ¼�¾���ʹ��ָ���ṹ��xml�ļ���˵����ģ��Ļ�����Ϣ
        /// </summary>
        public struct SpaceThemeAboutInfo
        {
            /// <summary>
            /// ģ������
            /// </summary>
            public string name;
            /// <summary>
            /// ����
            /// </summary>
            public string author;
            /// <summary>
            /// ��������
            /// </summary>
            public string createdate;
            /// <summary>
            /// ģ��汾
            /// </summary>
            public string ver;
            /// <summary>
            /// ģ�����õ���̳�汾
            /// </summary>
            public string fordntver;
            /// <summary>
            /// ��Ȩ����
            /// </summary>
            public string copyright;

        }


        /// <summary>
        /// ��ģ��˵���ļ��л��ģ��˵����Ϣ
        /// </summary>
        /// <param name="xmlPath">ģ��·��(�������ļ���)</param>
        /// <returns>ģ��˵����Ϣ</returns>
        public static SpaceThemeAboutInfo GetThemeAboutInfo(string xmlPath)
        {
            SpaceThemeAboutInfo aboutInfo = new SpaceThemeAboutInfo();
            aboutInfo.name = "";
            aboutInfo.author = "";
            aboutInfo.createdate = "";
            aboutInfo.ver = "";
            aboutInfo.fordntver = "";
            aboutInfo.copyright = "";

            ///��Ź�����Ϣ���ļ� about.xml�Ƿ����,�����ڷ��ؿմ�
            if (!System.IO.File.Exists(xmlPath + @"\about.xml"))
                return aboutInfo;
            
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath + @"\about.xml");

            foreach (XmlNode n in xml.SelectSingleNode("about").ChildNodes)
            {
                if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "template")
                {
                    XmlAttribute name = n.Attributes["name"];
                    XmlAttribute author = n.Attributes["author"];
                    XmlAttribute createdate = n.Attributes["createdate"];
                    XmlAttribute ver = n.Attributes["ver"];
                    XmlAttribute fordntver = n.Attributes["fordntver"];
                    XmlAttribute copyright = n.Attributes["copyright"];

                    if (name != null)
                        aboutInfo.name = name.Value;

                    if (author != null)
                        aboutInfo.author = author.Value;

                    if (createdate != null)
                        aboutInfo.createdate = createdate.Value;

                    if (ver != null)
                        aboutInfo.ver = ver.Value;

                    if (fordntver != null)
                        aboutInfo.fordntver = fordntver.Value;

                    if (copyright != null)
                        aboutInfo.copyright = copyright.Value;
                }
            }
            return aboutInfo;
        }
    }
}
