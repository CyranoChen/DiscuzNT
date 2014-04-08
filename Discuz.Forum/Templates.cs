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
    /// 论坛模板操作类
    /// </summary>
    public class Templates
    {
        private static object SynObject = new object();

        /// <summary>
        /// 从模板变量文件中获得模板变量值的信息
        /// </summary>
        /// <param name="templatename">模板变量文件名)</param>
        /// <returns>模板变量表</returns>
        public static DataTable GetTemplateVariable1(string templatename)
        {
            string path = Utils.GetMapPath("../../templates/" + templatename + "/templatevariable.xml");
            ///存放变量信息的文件 templatevariable.xml是否存在,不存在返回空表
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
        /// 获取指定路径下模板的宽度
        /// </summary>
        /// <param name="templatePath">模板名称</param>
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
        /// 从模板说明文件中获得模板说明信息
        /// </summary>
        /// <param name="xmlPath">模板路径(不包含文件名)</param>
        /// <returns>模板说明信息</returns>
        public static TemplateAboutInfo GetTemplateAboutInfo(string xmlPath)
        {
            TemplateAboutInfo aboutInfo = new TemplateAboutInfo();

            ///存放关于信息的文件 about.xml是否存在,不存在返回空串
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
        /// 获得前台有效的模板列表
        /// </summary>
        /// <returns>模板列表</returns>
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
        /// 获得前台有效的模板ID列表
        /// </summary>
        /// <returns>模板ID列表</returns>
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
        /// 获得指定的模板信息
        /// </summary>
        /// <param name="templateid">皮肤id</param>
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
        /// 添加模板
        /// </summary>
        /// <param name="name">模版名称</param>
        /// <param name="directory">模版目录</param>
        /// <param name="copyright">版权信息</param>
        /// <param name="author">作者</param>
        /// <param name="createdate">创建日期</param>
        /// <param name="ver">版本</param>
        /// <param name="fordntver">适用论坛版本</param>
        public static void CreateTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
        {
            Data.Templates.CreateTemplate(name, directory, copyright, author, createdate, ver, fordntver);
        }
    }
}
