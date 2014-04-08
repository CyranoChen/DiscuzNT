using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

using Discuz.Config;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ģ������ҳ��
    /// </summary>
    public partial class templatetree : AdminPage
    {
        private string skinpath;
        private int templateCounter = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            skinpath = DNTRequest.GetString("path");
            DeleteTemplateFile.Enabled = CreateTemplate.Enabled = !(TreeView1.SelectedIndex == -1);
        }

        private DataTable LoadTemplateFileDT()
        {
            #region װ��ģ���ļ�
            DataTable templateFileList = new DataTable("templatefilelist");

            templateFileList.Columns.Add("fullfilename", Type.GetType("System.String"));
            templateFileList.Columns.Add("filename", Type.GetType("System.String"));
            templateFileList.Columns.Add("id", Type.GetType("System.Int32"));
            templateFileList.Columns.Add("extension", Type.GetType("System.String"));
            templateFileList.Columns.Add("parentid", Type.GetType("System.String"));
            templateFileList.Columns.Add("filepath", Type.GetType("System.String"));
            templateFileList.Columns.Add("filedescription", Type.GetType("System.String"));

            string path = DNTRequest.GetString("path");
            //����Ĭ��ģ��Ŀ¼�����ļ��б�
            CreateTemplateFileList(templateFileList, "default", false);
            if (path.ToLower() != "defalut")
            {
                CreateTemplateFileList(templateFileList, path, true);
            }
            foreach (DataRow dr in templateFileList.Rows)
            {
                foreach (DataRow childdr in templateFileList.Select("filename like '" + dr["filename"] + "_%%'"))
                {
                    if (dr["filename"].ToString() != childdr["filename"].ToString())
                    {
                        childdr["parentid"] = dr["id"].ToString();
                    }
                }
            }

            return templateFileList;
            #endregion
        }

        private void CreateTemplateFileList(DataTable templateFileList, string path, bool resetList)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("..\\..\\templates\\" + path));
            foreach (FileSystemInfo file in dirInfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    if (file.Name == "images")
                        continue;
                    if (file.Attributes == FileAttributes.Directory)
                    {
                        CreateTemplateFileList(templateFileList, path + "\\" + file.Name, resetList);
                    }
                    else
                    {
                        if (file.Name.ToLower().EndsWith(".htm"))
                        {
                            if (resetList)
                            {
                                DataRow[] drs = templateFileList.Select("filename='" + file.Name.Substring(0, file.Name.LastIndexOf(".")) + "'");
                                if (drs.Length != 0)
                                {
                                    drs[0]["filename"] = file.Name.Substring(0, file.Name.LastIndexOf("."));
                                    drs[0]["fullfilename"] = path + "\\" + drs[0]["filename"] + file.Extension.ToLower();
                                    drs[0]["extension"] = file.Extension.ToLower();
                                    drs[0]["filepath"] = path;
                                    continue;
                                }
                            }
                            DataRow dr = templateFileList.NewRow();
                            dr["id"] = templateCounter;
                            dr["filename"] = file.Name.Substring(0, file.Name.LastIndexOf("."));
                            dr["fullfilename"] = path + "\\" + dr["filename"] + file.Extension.ToLower();
                            dr["extension"] = file.Extension.ToLower();
                            dr["parentid"] = "0";
                            dr["filepath"] = path;
                            dr["filedescription"] = "";
                            templateFileList.Rows.Add(dr);
                            templateCounter++;
                        }
                    }
                }
            }
        }


        private DataTable LoadOtherFileDT()
        {
            #region װ�������ļ�
            DataTable otherfilelist = new DataTable("otherfilelist");

            otherfilelist.Columns.Add("id", Type.GetType("System.Int32"));
            otherfilelist.Columns.Add("filename", Type.GetType("System.String"));
            otherfilelist.Columns.Add("extension", Type.GetType("System.String"));
            otherfilelist.Columns.Add("parentid", Type.GetType("System.String"));
            otherfilelist.Columns.Add("filepath", Type.GetType("System.String"));
            otherfilelist.Columns.Add("filedescription", Type.GetType("System.String"));

            string path = DNTRequest.GetString("path");
            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../templates/" + path));
            int i = 1;
            string extname;
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    extname = file.Extension.ToLower();
                    if (extname.IndexOf("js") > 0 || extname.IndexOf("css") > 0 || extname.IndexOf("xml") > 0 || extname.IndexOf(".html") > 0)
                    {
                        DataRow dr = otherfilelist.NewRow();

                        dr["id"] = i;
                        dr["filename"] = file.Name.Substring(0, file.Name.LastIndexOf("."));
                        dr["extension"] = file.Extension.ToLower();
                        dr["parentid"] = "0";
                        dr["filepath"] = path;
                        dr["filedescription"] = "";
                        otherfilelist.Rows.Add(dr);
                        i++;
                    }
                }
            }

            foreach (DataRow dr in otherfilelist.Rows)
            {
                foreach (DataRow childdr in otherfilelist.Select("filename like '" + dr["filename"] + "_%%'"))
                {
                    if (dr["filename"].ToString() != childdr["filename"].ToString())
                    {
                        childdr["parentid"] = dr["id"].ToString();
                    }
                }
            }

            foreach (DataRow dr in otherfilelist.Rows)
            {
                //string imgstr = "";
                //if (dr["extension"].ToString().IndexOf("js") > 0) imgstr = "../images/js.gif";
                //if (dr["extension"].ToString().IndexOf("xml") > 0) imgstr = "../images/xml.gif";
                //if (dr["extension"].ToString().IndexOf("css") > 0) imgstr = "../images/css.gif";
                //if (dr["extension"].ToString().IndexOf("aspx") > 0) imgstr = "../images/aspx.gif";
                //if (dr["extension"].ToString().IndexOf("ascx") > 0) imgstr = "../images/ascx.gif";
                string ext = dr["extension"].ToString().Substring(1);
                dr["filename"] = "<img src=\"../images/" + ext + ".gif\" border=\"0\"> <a href=\"global_templatesedit.aspx?path=" + dr["filepath"].ToString().Replace(" ", "%20") + "&filename=" + dr["filename"] + dr["extension"] + "&templateid=" + DNTRequest.GetString("templateid") + "&templatename=" + DNTRequest.GetString("templatename").Replace(" ", "%20") + "\" title=\"" + ext + "�ļ�\">" + dr["filename"].ToString().Trim() + "</a>";
            }

            return otherfilelist;
            #endregion
        }

        protected void DeleteTemplateFile_Click(object sender, EventArgs e)
        {
            #region ɾ��ģ���ļ�
            if (CheckCookie())
            {
                string templatepathlist = TreeView1.GetSelectString(",");

                if (templatepathlist == "")
                {
                    RegisterStartupScript( "", "<script>alert('��δѡ���κ�ģ��');</script>");
                    return;
                }
                try
                {
                    foreach (string templatepath in templatepathlist.Split(','))
                    {
                        DeleteFile(templatepath);
                    }
                }
                catch(UnauthorizedAccessException)
                {
                    RegisterStartupScript("", "<script>alert('����Ŀ¼������Ȩ�޵����޷��ڴ�ɾ�����ļ�');</script>");
                    return;
                }
                RegisterStartupScript( "PAGE", "window.location.href='global_templatetree.aspx?templateid=" + Request.Params["templateid"] + "&path=" + Request.Params["path"] + "&templatename=" + Request.Params["templatename"] + "';");
            }
            #endregion
        }


        public bool DeleteFile(string filename)
        {
            #region ɾ���ļ�
            if (Utils.FileExists(Utils.GetMapPath(@"..\..\templates\" + filename)))
            {
                File.Delete(Utils.GetMapPath(@"..\..\templates\" + filename));
                return true;
            }
            return false;
            #endregion
        }


        protected void CreateTemplate_Click(object sender, EventArgs e)
        {
            #region �����ļ�
            if (CheckCookie())
            {
                string templatePathList = TreeView1.GetSelectString(",");   //ȡ�ù�ѡ�ļ��б�

                if (templatePathList == "")
                {
                    RegisterStartupScript( "", "<script>alert('��δѡ���κ�ģ��');</script>");
                    return;
                }
                if (DNTRequest.GetString("chkall") == "" && templatePathList.Contains("_"))   //��ȫ������
                {
                    templatePathList = RemadeTemplatePathList(templatePathList);
                }
                int templateId = DNTRequest.GetInt("templateid", 1);
                int updateCount = 0;
                string forumPath = BaseConfigs.GetForumPath;
                ForumPageTemplate forumPageTemplate = new ForumPageTemplate();

                foreach (string templatePath in templatePathList.Split(','))
                {
                    string templateFileName = Path.GetFileName(templatePath).ToLower();//tempstr[tempstr.Length - 1];
                    string tempplaeExtName = Path.GetExtension(templateFileName); //tempstr = templateName.Split('.');
                    if ((tempplaeExtName.Equals(".htm") || (tempplaeExtName.Equals(".config"))) && !templateFileName.Contains("_"))
                    {
                        string subTemplateDirectory = "";
                        if (templatePath.Split('\\').Length >= 3)
                        {
                            subTemplateDirectory = Path.GetDirectoryName(templatePath).Substring(Path.GetDirectoryName(templatePath).LastIndexOf("\\") + 1);
                        }
                        forumPageTemplate.GetTemplate(forumPath, skinpath, Path.GetFileNameWithoutExtension(templateFileName), 
                           subTemplateDirectory , 1, templateId);
                        updateCount++;
                    }
                }
                RegisterStartupScript( "PAGETemplate", "��" + updateCount + " ��ģ���Ѹ���");
            }
            #endregion
        }
        /// <summary>
        /// �������ɰ�����ͷ�ļ����ļ��б�
        /// </summary>
        /// <param name="templatePathList">�Ѿ�ѡ����ļ��б�</param>
        /// <returns>���ش�����ϵ��ļ��б�</returns>
        private string RemadeTemplatePathList(string templatePathList)
        {
            #region ����ͷ�ļ����б��ļ�
            if(!templatePathList.Contains("\\_"))  //�б������û��ͷ�ļ���ֱ�ӷ���
            {
                return templatePathList;
            }
            StringBuilder result = new StringBuilder();
            foreach (string templatePath in templatePathList.Split(','))
            {
                string templateFileName = Path.GetFileName(templatePath).ToLower();
                string templateMainFileName = Path.GetFileNameWithoutExtension(templateFileName).ToLower();
                string templateExtName = Path.GetExtension(templateFileName).ToLower();
                if (!(templateExtName.Equals(".htm") || templateExtName.Equals(".config"))) continue; //��ģ���ļ�����
                if (!templateFileName.StartsWith("_"))   //��ͷ�ļ����������¸��ļ�
                {
                    if (!result.ToString().Contains(templateFileName))
                    {
                        result.Append(templatePath + ","); 
                    }
                    continue;
                }
                string[] defaultFiles = Directory.GetFiles(Utils.GetMapPath("../../templates/default"));
                string[] findFiles = Directory.GetFiles(Utils.GetMapPath("../../templates/" + skinpath));
                string findContent = "<%template " + templateMainFileName + "%>";
                foreach(string fullFileName in defaultFiles)
                {
                    string file = Path.GetFileName(fullFileName);
                    if (file.StartsWith("_") || !(Path.GetExtension(file) == ".htm" || Path.GetExtension(file) == ".config")) continue; //�����ͷ�ļ��򲻴���
                    using (StreamReader objReader = new StreamReader(Utils.GetMapPath("../../templates/default/" + file), Encoding.UTF8))
                    {
                        if (objReader.ReadToEnd().IndexOf(findContent) != -1)  //�ҵ�������ͷ�ļ����ļ�
                        {
                            if (!result.ToString().Contains(file))
                            {
                                if(File.Exists(Utils.GetMapPath("../../templates/" + skinpath + "/" + file)))
                                {
                                    result.Append(skinpath + "\\" + file + ",");
                                }
                                else
                                {
                                    result.Append("default\\" + file + ",");
                                }
                            }
                        }
                        objReader.Close();
                    }
                }
            }
            return result.ToString().TrimEnd(',');
            #endregion
        }


        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            DataTable dt = LoadTemplateFileDT();
            string templateid = DNTRequest.GetString("templateid");
            string templatename = DNTRequest.GetString("templatename").Replace(" ", "%20");
            foreach (DataRow dr in dt.Rows)
            {
                string ext = dr["extension"].ToString().Substring(1);
                dr["filename"] = String.Format("<img src=../images/{0}.gif border=\"0\"  style=\"position:relative;top:5 px;height:16 px\"> {1} "
                + "<a href=\"global_templatesedit.aspx?path={2}&filename={1}{3}&templateid={4}&templatename={5}\" title=\"�༭{1}.{0}ģ���ļ�\"><img src='../images/editfile.gif' border='0'/></a>",
                    ext, dr["filename"].ToString().Trim(), dr["filepath"].ToString().Replace(" ", "%20"), dr["extension"].ToString().Trim(),
                    templateid,templatename);
            }
            TreeView1.AddTableData(dt);
            for (int i = 0; i < TreeView1.Items.Count; i++)
            {
                TreeView1.Items[i].Attributes.Add("onclick", "checkedEnabledButton1(form,'TabControl1:tabPage22:CreateTemplate','TabControl1:tabPage22:DeleteTemplateFile')");
                TreeView1.Items[i].Attributes.Add("value", TreeView1.Items[i].Value);
            }

            TreeView2.DataSource = LoadOtherFileDT();
            TreeView2.DataBind();

        }

        #endregion
    }
}