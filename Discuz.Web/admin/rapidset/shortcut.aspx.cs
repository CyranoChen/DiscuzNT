using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.Admin.AutoUpdateManager;
using Discuz.Config;
using Discuz.Entity;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;
using Discuz.Common.Xml;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 快捷操作
    /// </summary>
    public partial class shortcut : AdminPage
    {
        public string filenamelist = "";
        protected string upgradeInfo = "";
        protected bool isNew = false;
        protected bool isHotFix = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isShowUpgradeInfo = false;
            string configPath = Page.Server.MapPath("../xml/user_" + this.userid + ".config");
            if (File.Exists(configPath))
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                isShowUpgradeInfo = doc.SelectSingleNode("/UserConfig/ShowUpgrade") != null ? doc.SelectSingleNode("/UserConfig/ShowUpgrade").InnerText == "1": false;
            }
            else
                isShowUpgradeInfo = true;
            if(isShowUpgradeInfo)
                LoadUpgradeInfo();

            LoadTemplateInfo();
            GetStatInfo();
            //加载论坛版块信息
            forumid.BuildTree(Forums.GetForumListForDataTable(), "name", "fid");
            LinkDiscuzVersionPage();
        }

        private void GetStatInfo()
        {
            StringBuilder statInfo = new StringBuilder();
            string forumname = GeneralConfigs.GetConfig().Forumtitle;
            int member = Convert.ToInt32(Statistics.GetStatisticsRowItem("totalusers"));
            int topics = Convert.ToInt32(Statistics.GetStatisticsRowItem("totaltopic"));
            int posts = Convert.ToInt32(Statistics.GetStatisticsRowItem("totalpost"));
            string serversoft = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
            int dotnetmajor = Environment.Version.Major;
            int dotnetminor = Environment.Version.Minor;
            int dotnetbuild = Environment.Version.Build;
            int dbtype = 0;
            switch (Discuz.Config.BaseConfigs.GetDbType.ToLower())
            {
                case "sqlserver":
                    {
                        dbtype = 0;
                        break;
                    }
                case "access":
                    {
                        dbtype = 101;
                        break;
                    }
                case "mysql":
                    {
                        dbtype = 201;
                        break;
                    }
            }
            string build = string.Empty;
            string strPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config");
            if (System.IO.File.Exists(strPath))
            {
                XmlDocument lastupdate = new XmlDocument();
                lastupdate.Load(strPath);
                build = lastupdate.SelectSingleNode("/localupgrade/requiredupgrade").InnerText;
                XmlNodeList list = lastupdate.SelectNodes("/localupgrade/optionalupgrade/dnt" + Utils.GetAssemblyVersion() + "/item");
                if (list != null)
                {
                    foreach (XmlNode node in list)
                    {
                        if (StrToDateTime(node.InnerText) > StrToDateTime(build))
                            build = node.InnerText;
                    }
                }
            }
            string osversion = Environment.OSVersion.ToString();
            string serverip = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            string servername = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string dbversion = "";
            if (dbtype == 0)
            {
                Regex regex = new Regex(@"\d{4}", RegexOptions.None);
                Match match = regex.Match(Databases.GetDataBaseVersion());
                if (match.Length != 0)
                    dbversion = match.Value;
            }
            string passwordmode = config.Passwordmode.ToString();
            string enablealbum = config.Enablealbum.ToString();
            string enablespace = config.Enablespace.ToString();
            string enablemall = config.Enablemall.ToString();
            string url = Utils.GetRootUrl(BaseConfigs.GetForumPath);

            statInfo.Append(Server.UrlEncode(forumname) + "," + member + "," + topics + "," + posts + "," + serversoft + "," + Utils.AssemblyFileVersion.FileMajorPart + "," + Utils.AssemblyFileVersion.FileMinorPart + "," + Utils.AssemblyFileVersion.FileBuildPart + ",");
            statInfo.Append(dotnetmajor + "," + dotnetminor + "," + dotnetbuild + "," + dbtype + "," + build + "," + osversion + "," + url + "," + servername + ",");
            statInfo.Append(dbversion + "," + passwordmode + "," + enablealbum + "," + enablespace + "," + enablemall + "," + config.Passwordkey);
            base.RegisterStartupScript("", string.Format("<script type='text/javascript' src='http://service.nt.discuz.net/news.aspx?update={0}'></script>", Convert.ToBase64String(Encoding.Default.GetBytes(statInfo.ToString()))));
        }

        private void LoadUpgradeInfo()
        {
            if (!IsPostBack)
            {
                try
                {
                    //合并升级信息
                    MergeUpgradeInfo();
                    //获取官方升级版本列表，并存放到本地upgrade/versionlist.config中
                    AutoUpdate autoUpdate = new AutoUpdate();
                    string fileContnet = autoUpdate.GetVersionList();
                    StreamWriter writer = new StreamWriter(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/upgradeini.config"));
                    writer.Write(fileContnet.Replace("\n", "\r\n"));
                    writer.Close();
                    //取本地更新版本号与官方的最新版本号比较，如果官方有更新，则提示有升级
                    XmlDocument lastupdate = new XmlDocument();
                    lastupdate.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
                    DateTime build = StrToDateTime(lastupdate.SelectSingleNode("/localupgrade/requiredupgrade").InnerText);
                    XmlDocument currentupdate = new XmlDocument();
                    currentupdate.LoadXml(fileContnet);
                    XmlNodeList items = currentupdate.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/requiredupgrade/item");
                    XmlNode lastItem = items.Item(items.Count - 1);
                    //DateTime version = StrToDateTime(items.Item(items.Count - 1).FirstChild.InnerText);
                    DateTime version = StrToDateTime(lastItem["version"].InnerText);
                    isNew = version > build;
                    if (isNew)
                    {
                        //upgradeInfo = "检测到最新版本：" + version.ToShortDateString() + "<br />" + items.Item(items.Count - 1).LastChild.InnerText + "<br />";
                        upgradeInfo = "<span style='font-size:16px;'>检测到最新版本：" + lastItem["versiondescription"].InnerText + "</span><br /><span style='padding:3px 0px 3px 10px;display:block;'>" + lastItem["description"].InnerText + "</span>";
                    }

                    XmlNodeList local = lastupdate.SelectNodes("/localupgrade/optionalupgrade/dnt" + Utils.GetAssemblyVersion() + "/item");
                    XmlNodeList service = currentupdate.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/optionalupgrade/dnt" + Utils.GetAssemblyVersion() + "/item");
                    string hotfix = "";
                    foreach (XmlNode serviceitem in service)
                    {
                        bool exist = false;
                        foreach (XmlNode localitem in local)
                        {
                            if (serviceitem.FirstChild.InnerText == localitem.InnerText)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            isHotFix = true;
                            hotfix += serviceitem["versiondescription"].InnerText + "<br />";
                        }

                    }

                    if (hotfix != "")
                    {
                        upgradeInfo += "<span style='font-size:16px;'>检测到最新补丁：</span><br /><span style='padding:3px 0px 3px 10px;display:block;'>" + hotfix + "</span>";
                    }

                    if (isNew || isHotFix)
                    {
                        base.RegisterStartupScript("", "<script type='text/javascript'>\r\nshowInfo();\r\n</script>\r\n");
                    }
                }
                catch
                { 
                    ;
                }
            }
        }

        private void MergeUpgradeInfo()
        {
            if (!File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config")))
            {
                return;
            }
            XmlDocument configfile = new XmlDocument();
            //读取普通升级的信息文件
            string requiredupgradeversion = "";
            string optionalupgradeversion = "";
            configfile.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));
            if (configfile.SelectSingleNode("/requiredupgrade") != null)
            {
                requiredupgradeversion = configfile.SelectSingleNode("/requiredupgrade").InnerText;
            }
            else
            {
                optionalupgradeversion = configfile.SelectSingleNode("/optionalupgrade").InnerText;
            }
            File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));

            configfile.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if (requiredupgradeversion != "")
            {
                if (StrToDateTime(configfile.SelectSingleNode("/localupgrade/requiredupgrade").InnerText) >= StrToDateTime(requiredupgradeversion))
                {
                    return;
                }
                configfile.SelectSingleNode("/localupgrade/requiredupgrade").InnerText = requiredupgradeversion;
            }
            else
            {
                XmlNode dntver = configfile.SelectSingleNode("/localupgrade/optionalupgrade/dnt" + Utils.GetAssemblyVersion());
                if (dntver == null)
                {
                    dntver = configfile.CreateElement("dnt" + Utils.GetAssemblyVersion());
                }
                else
                {
                    foreach (XmlNode node in dntver.ChildNodes)
                    {
                        //当版本存在时
                        if (node.InnerText == optionalupgradeversion)
                            return;
                    }
                }
                XmlElement item = configfile.CreateElement("item");
                item.InnerText = optionalupgradeversion;
                dntver.AppendChild(item);
                if (configfile.SelectSingleNode("/localupgrade/optionalupgrade") == null)
                    configfile.SelectSingleNode("/localupgrade").AppendChild(configfile.CreateElement("optionalupgrade"));
                configfile.SelectSingleNode("/localupgrade/optionalupgrade").AppendChild(dntver);
            }
            configfile.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
        }

        private DateTime StrToDateTime(string str)
        {
            string date = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
            if (str.Length == 8)
            {
                date += " 00:00:00";
            }
            else
            {
                date += " " + str.Substring(8, 2) + ":" + str.Substring(10, 2) + ":" + str.Substring(12, 2);
            }
            return Convert.ToDateTime(date);
        }

        public void LoadTemplateInfo()
        {
            #region 加载模板路径信息

            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../templates/" + Templatepath.SelectedValue + "/"));

            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    string extname = file.Extension.ToLower();

                    if (extname.Equals(".htm") && (file.Name.IndexOf("_") != 0))
                    {
                        filenamelist += file.Name.Split('.')[0] + "|";
                    }
                }
            }

            #endregion
        }

        public void LinkDiscuzVersionPage()
        {
            #region 链接官方升级页面
            string linkurl = "http://nt.discuz.net/update/?ver=" + Utils.GetAssemblyVersion() + "&dbtype=" + BaseConfigs.GetDbType;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(linkurl);

            try
            {
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                req.AllowAutoRedirect = false;
                req.Timeout = 1500;

                HttpWebResponse Http_Res = (HttpWebResponse)req.GetResponse();

                if (Http_Res.StatusCode.ToString() != "OK")
                {
                    base.RegisterStartupScript( "form1", "<script language=javascript>document.getElementById('checkveriframe').parentNode.style.height=0+'px';document.getElementById('checkveriframe').src='about:blank';</script>");
                }
                else
                {
                    base.RegisterStartupScript("form1", "<script language=javascript>document.getElementById('checkveriframe').parentNode.style.height=0+'px';document.getElementById('checkveriframe').src='" + linkurl + "';</script>");
                }
            }
            catch
            {
                base.RegisterStartupScript( "form1", "<script language=javascript> if(navigator.appName.indexOf('Explorer') > -1){document.getElementById('checkveriframe').parentElement.innerHTML='&nbsp;&nbsp;&nbsp;  <img src=../images/hint.gif> 无法链接到Discuz!NT官方网站,因此无法得到最新官方信息';}else{document.getElementById('checkveriframe').parentNode.innerHTML='&nbsp;&nbsp;&nbsp;  <img src=../images/hint.gif> 无法链接到Discuz!NT官方网站,因此无法得到最新官方信息';}</script>");
            }

            #endregion
        }

        private void EditForum_Click(object sender, EventArgs e)
        {
            #region 重定向到指定的版块编辑页面

            if (forumid.SelectedValue != "0")
            {
                Response.Redirect("../forum/forum_EditForums.aspx?fid=" + forumid.SelectedValue);
            }
            else
            {
                base.RegisterStartupScript( "", "<script>alert('请您选择有效的论坛版块!');</script>");
            }

            #endregion
        }

        private void EditUserGroup_Click(object sender, EventArgs e)
        {
            #region 重定向到指定的用户组编辑页面

            if (Usergroupid.SelectedValue != "0")
            {
                int groupid = Convert.ToInt32(Usergroupid.SelectedValue);
                if (groupid >= 1 && groupid <= 3)
                {
                    Response.Redirect("../global/global_editadminusergroup.aspx?groupid=" + Usergroupid.SelectedValue);
                    return;
                }
                if (groupid >= 4 && groupid <= 8)
                {
                    Response.Redirect("../global/global_editsysadminusergroup.aspx?groupid=" + Usergroupid.SelectedValue);
                    return;
                }

                int radminid = UserGroups.GetUserGroupInfo(Utils.StrToInt(Usergroupid.SelectedValue, 0)).Radminid;
                if (radminid == 0)
                {
                    Response.Redirect("../global/global_editusergroup.aspx?groupid=" + Usergroupid.SelectedValue);
                    return;
                }
                if (radminid > 0)
                {
                    Response.Redirect("../global/global_editadminusergroup.aspx?groupid=" + Usergroupid.SelectedValue);
                    return;
                }
                if (radminid < 0)
                {
                    Response.Redirect("../global/global_editusergroupspecial.aspx?groupid=" + Usergroupid.SelectedValue);
                    return;
                }

            }
            else
            {
                base.RegisterStartupScript( "", "<script>alert('请您选择有效的用户组!');</script>");
            }

            #endregion
        }

        private void UpdateForumStatistics_Click(object sender, EventArgs e)
        {
            #region 更新论坛统计信息

            if (this.CheckCookie())
            {
                Caches.ReSetStatistics();
                base.RegisterStartupScript( "PAGE",  "window.location.href='shortcut.aspx';");
            }

            #endregion
        }

        private void UpdateCache_Click(object sender, EventArgs e)
        {
            #region 更新所有缓存

            if (this.CheckCookie())
            {
                Caches.ReSetAllCache();
                base.RegisterStartupScript( "PAGE",  "window.location.href='shortcut.aspx';");
            }

            #endregion
        }

        private void CreateTemplate_Click(object sender, EventArgs e)
        {
            #region 生成指定模板

            if (this.CheckCookie())
            {
                Globals.BuildTemplate(Templatepath.SelectedValue);

                base.RegisterStartupScript( "PAGE", "window.location.href='shortcut.aspx';");
            }

            #endregion
        }

        private void EditUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("../global/global_usergrid.aspx?username=" + Username.Text.Trim());
        }


        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.EditUser.Click += new EventHandler(this.EditUser_Click);
            this.EditForum.Click += new EventHandler(this.EditForum_Click);
            this.EditUserGroup.Click += new EventHandler(this.EditUserGroup_Click);
            this.UpdateCache.Click += new EventHandler(this.UpdateCache_Click);
            this.CreateTemplate.Click += new EventHandler(this.CreateTemplate_Click);
            this.UpdateForumStatistics.Click += new EventHandler(this.UpdateForumStatistics_Click);

            //装入有效的模板信息项
            foreach (DataRow dr in AdminTemplates.GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\")).Rows)
            {
                if (dr["valid"].ToString() == "1")
                {
                    Templatepath.Items.Add(new ListItem(dr["name"].ToString(), dr["directory"].ToString()));
                }
            }
            Username.AddAttributes("onkeydown", "if(event.keyCode==13) return(document.forms(0).EditUser.focus());");
            Usergroupid.AddTableData(UserGroups.GetUserGroupForDataTable(),"grouptitle","groupid");
        }

        #endregion

    }
}