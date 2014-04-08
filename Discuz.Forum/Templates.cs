using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// ��̳ģ�������
    /// </summary>
    public class Templates
    {
        private static object SynObject = new object();

        /// <summary>
        /// ��ģ������ļ��л��ģ�����ֵ����Ϣ
        /// </summary>
        /// <param name="templatename">ģ������ļ���)</param>
        /// <returns>ģ�������</returns>
        public static DataTable GetTemplateVariable1(string templatename)
        {
            string path = Utils.GetMapPath("../../templates/" + templatename + "/templatevariable.xml");
            ///��ű�����Ϣ���ļ� templatevariable.xml�Ƿ����,�����ڷ��ؿձ�
            if (!System.IO.File.Exists(path))
                return null;
            else
            {
                using (DataSet ds = new DataSet())
                {
                    ds.ReadXml(path);
                    return ds.Tables[0];
                }
            }
        }

        /// <summary>
        /// ��ȡָ��·����ģ��Ŀ��
        /// </summary>
        /// <param name="templatePath">ģ������</param>
        /// <returns></returns>
        public static int GetTemplateWidth(string templatePath)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            string width = cache.RetrieveObject("/Forum/TemplateWidth/" + templatePath) as string;
            if (width == null)
            {
                width = GetTemplateAboutInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "templates/" + templatePath + "/")).width;
                cache.AddObject("/Forum/TemplateWidth/" + templatePath, width);
            }
            return TypeConverter.StrToInt(width);
        }


        /// <summary>
        /// ��ģ��˵���ļ��л��ģ��˵����Ϣ
        /// </summary>
        /// <param name="xmlPath">ģ��·��(�������ļ���)</param>
        /// <returns>ģ��˵����Ϣ</returns>
        public static TemplateAboutInfo GetTemplateAboutInfo(string xmlPath)
        {
            TemplateAboutInfo aboutInfo = new TemplateAboutInfo();

            ///��Ź�����Ϣ���ļ� about.xml�Ƿ����,�����ڷ��ؿմ�
            if (!System.IO.File.Exists(xmlPath + @"\about.xml"))
                return aboutInfo;

            XmlDocument xml = new XmlDocument();

            xml.Load(xmlPath + @"\about.xml");

            try
            {
                XmlNode root = xml.SelectSingleNode("about");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "template")
                    {
                        aboutInfo.name = n.Attributes["name"] != null ? n.Attributes["name"].Value.ToString() : "";
                        aboutInfo.author = n.Attributes["author"] != null ? n.Attributes["author"].Value.ToString() : "";
                        aboutInfo.createdate = n.Attributes["createdate"] != null ? n.Attributes["createdate"].Value.ToString() : "";
                        aboutInfo.ver = n.Attributes["ver"] != null ? n.Attributes["ver"].Value.ToString() : "";
                        aboutInfo.fordntver = n.Attributes["fordntver"] != null ? n.Attributes["fordntver"].Value.ToString() : "";
                        aboutInfo.copyright = n.Attributes["copyright"] != null ? n.Attributes["copyright"].Value.ToString() : "";
                        aboutInfo.width = n.Attributes["width"] != null ? n.Attributes["width"].Value.ToString() : "600";
                    }
                }
            }
            catch
            {
                aboutInfo = new TemplateAboutInfo();
            }
            return aboutInfo;
        }

        /// <summary>
        /// ���ǰ̨��Ч��ģ���б�
        /// </summary>
        /// <returns>ģ���б�</returns>
        public static DataTable GetValidTemplateList()
        {
            lock (SynObject)
            {
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                DataTable dt = cache.RetrieveObject("/Forum/TemplateList") as DataTable;
                if (dt == null)
                {
                    dt = Discuz.Data.Templates.GetValidTemplateList();
                    cache.AddObject("/Forum/TemplateList", dt);
                }
                return dt;
            }
        }


        /// <summary>
        /// ���ǰ̨��Ч��ģ��ID�б�
        /// </summary>
        /// <returns>ģ��ID�б�</returns>
        public static string GetValidTemplateIDList()
        {
            lock (SynObject)
            {
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                string templateidlist = cache.RetrieveObject("/Forum/TemplateIDList") as string;

                if (templateidlist == null)
                {
                    templateidlist = Discuz.Data.Templates.GetValidTemplateIDList();

                    if (!Utils.StrIsNullOrEmpty(templateidlist))
                        templateidlist = templateidlist.Substring(1);

                    cache.AddObject("/Forum/TemplateIDList", templateidlist);
                }
                return templateidlist;
            }
        }

        /// <summary>
        /// ���ָ����ģ����Ϣ
        /// </summary>
        /// <param name="templateid">Ƥ��id</param>
        /// <returns></returns>
        public static TemplateInfo GetTemplateItem(int templateid)
        {
            if (templateid <= 0)
                return null;

            TemplateInfo templateinfo = null;
            DataRow[] dr = GetValidTemplateList().Select("templateid = " + templateid.ToString());

            if (dr.Length > 0)
            {
                templateinfo = new TemplateInfo();
                templateinfo.Templateid = Int16.Parse(dr[0]["templateid"].ToString());
                templateinfo.Name = dr[0]["name"].ToString();
                templateinfo.Directory = dr[0]["directory"].ToString();
                templateinfo.Copyright = dr[0]["copyright"].ToString();
                templateinfo.Templateurl = dr[0]["templateurl"].ToString();
            }

            if (templateinfo == null)
            {
                dr = GetValidTemplateList().Select("templateid = 1");

                if (dr.Length > 0)
                {
                    templateinfo = new TemplateInfo();
                    templateinfo.Templateid = Int16.Parse(dr[0]["templateid"].ToString());
                    templateinfo.Name = dr[0]["name"].ToString();
                    templateinfo.Directory = dr[0]["directory"].ToString();
                    templateinfo.Copyright = dr[0]["copyright"].ToString();
                    templateinfo.Templateurl = dr[0]["templateurl"].ToString();
                }
            }
            return templateinfo;
        }

        /// <summary>
        /// ���ģ��
        /// </summary>
        /// <param name="name">ģ������</param>
        /// <param name="directory">ģ��Ŀ¼</param>
        /// <param name="copyright">��Ȩ��Ϣ</param>
        /// <param name="author">����</param>
        /// <param name="createdate">��������</param>
        /// <param name="ver">�汾</param>
        /// <param name="fordntver">������̳�汾</param>
        public static void CreateTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
        {
            Data.Templates.CreateTemplate(name, directory, copyright, author, createdate, ver, fordntver);
        }
    }
}
