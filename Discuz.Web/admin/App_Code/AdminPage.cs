using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using System.Xml;
using Discuz.Forum;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// AdminPage 的摘要说明。
    /// 后台管理页面基类
    /// </summary>
    public class AdminPage : Page
    {
        protected internal string username;

        /// <summary>
        /// 当前用户的用户ID
        /// </summary>
        protected internal int userid;

        /// <summary>
        /// 当前用户的用户组ID
        /// </summary>
        protected internal int usergroupid;

        /// <summary>
        /// 当前用户的管理组ID
        /// </summary>
        protected internal short useradminid = 0;

        protected internal string grouptitle;

        protected internal string ip;

        protected internal GeneralConfigInfo config;

        private const int MaxShortcutMenuCount = 15;    //快捷菜单最大收藏数

        public string footer = "<div align=\"center\" style=\" padding-top:60px;font-size:11px; font-family: Arial;\">Powered by <a style=\"COLOR: #000000\" href=\"http://nt.discuz.net\" target=\"_blank\">" + Utils.GetAssemblyProductName() + "</a> &nbsp;&copy; 2001-" + DateTime.Now.Year + ", <a style=\"COLOR: #000000;font-weight:bold\" href=\"http://www.comsenz.com\" target=\"_blank\">Comsenz Inc.</a></div>";

        public bool AllowShowNavigation = true; //是否允许管理导航链接，默认允许显示
        /// <summary>
        /// 控件初始化时计算执行时间
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            m_processtime = (Environment.TickCount - m_starttick) / 1000;
            base.OnInit(e);
        }


        /// <summary>
        /// 得到当前页面的载入时间供模板中调用(单位:毫秒)
        /// </summary>
        /// <returns>当前页面的载入时间</returns>
        public float Processtime
        {
            get { return m_processtime; }
        }



        /// <summary>
        /// 当前页面开始载入时间(毫秒)
        /// </summary>
        public float m_starttick = Environment.TickCount;

        /// <summary>
        /// 当前页面执行时间(毫秒)
        /// </summary>
        public float m_processtime;

        public AdminPage()
        {
            if (!Page.IsPostBack)
            {
                this.RegisterAdminPageClientScriptBlock();
            }


            config = GeneralConfigs.GetConfig();


            // 如果IP访问列表有设置则进行判断
            if (config.Adminipaccess.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                {
                    Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }
            }

            // 获取用户信息
            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
            UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid);
            if (oluserinfo.Userid <= 0 || usergroupinfo.Radminid != 1)
            {
                Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }

            string secques = Users.GetUserInfo(oluserinfo.Userid).Secques;

            // 管理员身份验证
            if (Context.Request.Cookies["dntadmin"] == null || Context.Request.Cookies["dntadmin"]["key"] == null || 
                ForumUtils.GetCookiePassword(Context.Request.Cookies["dntadmin"]["key"].ToString(), config.Passwordkey) != (oluserinfo.Password + secques + oluserinfo.Userid.ToString()))
            {
                Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }
            else
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["dntadmin"];
                cookie.Values["key"] = ForumUtils.SetCookiePassword(oluserinfo.Password + secques + oluserinfo.Userid.ToString(), config.Passwordkey);
                cookie.Values["userid"] = oluserinfo.Userid.ToString();
                cookie.Expires = DateTime.Now.AddMinutes(30);
                HttpContext.Current.Response.AppendCookie(cookie);

            }

            this.userid = oluserinfo.Userid;
            this.username = oluserinfo.Username;
            this.usergroupid = oluserinfo.Groupid;
            this.useradminid = (short)usergroupinfo.Radminid;
            this.grouptitle = usergroupinfo.Grouptitle;
            this.ip = DNTRequest.GetIP();

     
            //当前窗口不在Index.aspx页面上的FRAME中,则重定向到该框架中
            //if (DNTRequest.GetPageName() != "runforumstatic.aspx")
            //{
            //    Context.Response.Write("<script>if(top.mainFrame==null) top.location.href='/admin/index.aspx?fromurl=" + Context.Request.RawUrl + "';</script>");
            //    Context.Response.End();
            //    return;
            //}
            
        }

        /// <summary>
        /// 注册提示信息JS脚本
        /// </summary>
        public void RegisterAdminPageClientScriptBlock()
        {
            string script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                "		<div id=\"Layer4\" style=\"height:26px;background:#f1f1f1;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;\">操作提示</div>\r\n" +
                "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\"><BR /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在执行当前操作, 请稍等...<BR /></td></tr></table><BR /></div>\r\n" +
                "	</div>\r\n" +
                "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #E8E8E8;\"></div>\r\n" +
                "</div>\r\n" +
                "<script> \r\n" +
                "document.getElementById('success').style.display = \"none\"; \r\n" +
                "</script> \r\n" +
                "<script type=\"text/javascript\" src=\"../js/divcover.js\"></script>\r\n";

            ClientScript.RegisterClientScriptBlock(this.GetType(), "Page", script);
        }


        public new void RegisterStartupScript(string key, string scriptstr)
        {
            key = key.ToLower();
            if ((key == "pagetemplate") || (key == "page"))
            {
                string script = "";

                if (key == "page")
                {
                    script = "<script> \r\n" +
                        "var bar=0;\r\n" +
                        "document.getElementById('success').style.display = \"block\";  \r\n" +
                        "document.getElementById('Layer5').innerHTML ='<BR>操作成功执行<BR>';  \r\n" +
                        "count() ; \r\n" +
                        "function count(){ \r\n" +
                        "bar=bar+4; \r\n" +
                        "if (bar<99) \r\n" +
                        "{setTimeout(\"count()\",100);} \r\n" +
                        "else { \r\n" +
                        "document.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" +
                        scriptstr + "} \r\n" +
                        "} \r\n" +
                        "</script> \r\n" +
                        "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
                }

                if (key == "pagetemplate")
                {
                    script = "<script> \r\n" +
                        "var bar=0;\r\n success.style.display = \"block\";  \r\n" +
                        "document.getElementById('Layer5').innerHTML = '<BR>" + scriptstr + "<BR>';  \r\n" +
                        "count() ; \r\n" +
                        "function count(){ \r\n" +
                        "bar=bar+4; \r\n" +
                        "if (bar<99) \r\n" +
                        "{setTimeout(\"count()\",100);} \r\n" +
                        "else { \r\n" +
                        "document.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" +
                        "}} \r\n" +
                        "</script> \r\n" +
                        "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
                }
                ClientScript.RegisterStartupScript(this.GetType(), key, script);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), key, scriptstr);
            }
        }


        public void CallBaseRegisterStartupScript(string key, string scriptstr)
        {
            ClientScript.RegisterStartupScript(this.GetType(), key, scriptstr);
        }



        /// <summary>
        /// 检查cookie是否有效
        /// </summary>
        /// <returns></returns>
        public bool CheckCookie()
        {
            config = GeneralConfigs.GetConfig();

            // 如果IP访问列表有设置则进行判断
            if (config.Adminipaccess.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                {
                    Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return false;
                }
            }
            // 获取用户信息
            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
            UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid);
            if (oluserinfo.Userid <= 0 || usergroupinfo.Radminid != 1)
            {
                Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return false;
            }

            string secques = Users.GetUserInfo(oluserinfo.Userid).Secques;
            // 管理员身份验证
            if (Context.Request.Cookies["dntadmin"] == null || Context.Request.Cookies["dntadmin"]["key"] == null || ForumUtils.GetCookiePassword(Context.Request.Cookies["dntadmin"]["key"].ToString(), config.Passwordkey) != (oluserinfo.Password + secques + oluserinfo.Userid.ToString()))
            {
                Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return false;
            }
            else
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["dntadmin"];
                cookie.Values["key"] = ForumUtils.SetCookiePassword(oluserinfo.Password + secques + oluserinfo.Userid.ToString(), config.Passwordkey);
                cookie.Expires = DateTime.Now.AddMinutes(30);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
            return true;
        }


        /// <summary>
        /// 当前用户是否是创始人
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsFounderUid(int uid)
        {
            if (BaseConfigs.GetBaseConfig().Founderuid == 0) return true;
            else
            {
                //如果当前登陆后台的用户就是论坛的创始人
                if (BaseConfigs.GetBaseConfig().Founderuid == uid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 论坛提示信息
        /// </summary>
        /// <returns></returns>
        protected string GetShowMessage()
        {
            string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>您没有权限运行当前程序!</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
            message += "<link href=\"../styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><body><br><br><div style=\"width:100%\" align=\"center\">";
            message += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"../images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
            message += "您没有权限运行当前程序,请您以论坛创始人身份登陆后台进行操作!</div></div></body></html>";
            return message;
        }


        public void LoadRegisterStartupScript(string key, string scriptstr)
        {
            string message = "程序执行中... <BR /> 当前操作可能要运行一段时间.<BR />您可在此期间进行其它操作<BR /><BR />";

            string script = "<script> \r\n" +
                "var bar=0;\r\n success.style.display = \"block\";  \r\n" +
                "document.getElementById('Layer5').innerHTML ='" + message + "';  \r\n" +
                "count() ; \r\n" +
                "function count(){ \r\n" +
                "bar=bar+2; \r\n" +
                "if (bar<99) \r\n" +
                "{setTimeout(\"count()\",100);} \r\n" +
                "else { \r\n" +
                "	document.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" +
                scriptstr + "} \r\n" +
                "} \r\n" +
                "</script> \r\n" +
                "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";

            CallBaseRegisterStartupScript(key, script);

        }

        private bool saveState = false;

        private bool IsRestore
        {
            get
            {
                if (Request.QueryString["IsRestore"] != null && Request.QueryString["IsRestore"] == "1" && Request.Form["__VIEWSTATE"] == null)
                    return true;
                else
                    return false;
            }
        }

        private string RestoreKey
        {
            get { return Request.QueryString["key"]; }
        }

        public bool SavePageState
        {
            get { return this.saveState; }
            set { this.saveState = value; }
        }

        private NameValueCollection postData = null;

        private NameValueCollection PostData
        {
            get
            {
                if (this.IsRestore)
                    return this.postData;
                return Request.Form;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.IsRestore)
            {
                ArrayList modifiedControls = new ArrayList();
                foreach (string key in PostData.AllKeys)
                {
                    System.Web.UI.Control control = FindControl(key);
                    if (control is IPostBackDataHandler)
                        if (((IPostBackDataHandler)control).LoadPostData(key, PostData))
                            modifiedControls.Add(control);
                }
                // 发生 PostDataChanged 事件在所有已变动的控件上:
                foreach (IPostBackDataHandler control in modifiedControls)
                    control.RaisePostDataChangedEvent();
            }
            base.OnLoad(e);
            string headerStr = "<script type=\"text/javascript\" src=\"../js/AjaxHelper.js\"></script><script type='text/javascript'>\nfunction ResetShortcutMenu(){window.parent.LoadShortcutMenu();}\nfunction FavoriteFunction(url){\nAjaxHelper.Updater('../UserControls/favoritefunction','resultmessage','url='+url,ResetShortcutMenu);\n}\n</script>\n";
            headerStr += "<div align='right' style=''>";
            //获取当前页面在收藏夹中的状态
            FavoriteStatus status = GetFavoriteStatus();
            //根据当前页面收藏夹状态生成收藏快捷操作的链接
            if (status != FavoriteStatus.Hidden)
            {
                if (status == FavoriteStatus.Exist)
                {
                    headerStr += headerStr += "<span id='resultmessage' title='已经将该页面加入到快捷操作菜单中'><img src='../images/existmenu.gif' style='vertical-align:middle' /> 已经收藏</span>";
                }
                else if (status == FavoriteStatus.Full)
                {
                    headerStr += headerStr += "<span id='resultmessage' title='快捷操作菜单最大收藏数为" + MaxShortcutMenuCount + "项'><img src='../images/fullmenu.gif' style='vertical-align:middle' /> 收藏已满</span>\n</b>";
                }
                else if (status == FavoriteStatus.Show)
                {
                    headerStr += "<span align='right' id='resultmessage'>\n<a href='javascript:void(0);' title='将该页面加入快捷操作菜单' onclick='FavoriteFunction(window.location.pathname.toLowerCase().replace(\"" + BaseConfigs.GetForumPath + "admin/\",\"\") + window.location.search.toLowerCase());' style='text-decoration:none;color:#333;' onfocus=\"this.blur();\"><img src='../images/addmenu.gif' align='absmiddle' /> 加入常用功能</a>\n</span>";
                }
            }
            if(AllowShowNavigation)
                headerStr += "<span><a href='javascript:void(0);' onclick='window.parent.showNavigation()' title='按ESC键或点击链接显示导航菜单' style='text-decoration:none;color:#333;'><img src='../images/navigation.gif' style='vertical-align:middle'> 管理导航</a></span>";
            headerStr += "</div>";
#if NET1                
			this.RegisterClientScriptBlock("Form1", headerStr);
#else
			this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Form1", headerStr);
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Navigation", "<script type='text/javascript'>if(document.documentElement.addEventListener){document.documentElement.addEventListener('keydown', window.parent.resetEscAndF5, false);}else if(document.documentElement.attachEvent){document.documentElement.attachEvent('onkeydown', window.parent.resetEscAndF5);}</script>");
#endif
        }
        
        // 收藏夹的状态，Show正常显示，Hidden不允许收藏，Full收藏夹已满，Exist收藏项已经存在
        private enum FavoriteStatus { Show, Hidden, Full, Exist }
        
        /// <summary>
        /// 获取收藏夹的状态
        /// </summary>
        /// <returns></returns>
        private FavoriteStatus GetFavoriteStatus()
        {
            string configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            //string pagename = DNTRequest.GetPageName().ToLower() + DNTRequest.GetUrl().Substring(DNTRequest.GetUrl().IndexOf('?')).ToLower();
            string url = DNTRequest.GetUrl().ToLower();
            string pagename = url.Substring(url.LastIndexOf('/') + 1);
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            XmlNodeList submains = doc.SelectNodes("/dataset/submain");
            bool findmenu = false;
            foreach (XmlNode submain in submains)
            {
                if (submain.SelectSingleNode("link").InnerText.IndexOf('/') == -1) continue;
                if (submain.SelectSingleNode("link").InnerText.Split('/')[1].ToLower() == pagename)
                    findmenu = true;                   
            }
            //当前链接不在菜单文件中，则不允许显示
            if(!findmenu)
                return FavoriteStatus.Hidden;   //不允许收藏
            XmlNodeList shortcuts = doc.SelectNodes("/dataset/shortcut");
            foreach (XmlNode singleshortcut in shortcuts)
            {
                if (singleshortcut.SelectSingleNode("link").InnerText.IndexOf(pagename) != -1) return FavoriteStatus.Exist; //在收藏夹中已存在
            }
            if (shortcuts.Count >= MaxShortcutMenuCount) return FavoriteStatus.Full;  //快捷菜单收藏最多收藏15条
            return FavoriteStatus.Show; //正常收藏
        }

        private void RegisterMessage(string scriptstr,bool autoHidd,string autoJumpUrl)
        {
            string script = "<script type='text/javascript'>\r\n" +
                    "var bar=0;\r\n" +
                    "document.getElementById('success').style.display = \"block\";\r\n" +
                    "document.getElementById('Layer5').innerHTML = '<BR>" + scriptstr + "<BR>';\r\n";
            if (autoHidd)
            {
                script += "count();\r\n" +
                "function count()\r\n" +
                "{\r\n" +
                "\tbar=bar+4;\r\n" +
                "\tif (bar<99)\r\n" +
                "\t{\r\n" +
                "\t\tsetTimeout(\"count()\",200);\r\n" +
                "\t}\r\n" +
                "\telse\r\n" +
                "\t{\r\n";
                if (autoJumpUrl == "")
                {
                    script += "\t\tdocument.getElementById('success').style.display = \"none\";HideOverSels('success');\r\n";
                }
                else
                {
                    script += "\t\twindow.location='" + autoJumpUrl + "';\r\n";
                }
                script += "\t}\r\n" +
                          "}\r\n";
            }
            script += "</script>\r\n" +
                      "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
#if NET1
			base.RegisterStartupScript(key, script);
#else
            ClientScript.RegisterStartupScript(this.GetType(), "resultMessage", script);
#endif
        }

        protected void RegisterMessage(string scriptstr, string autoJumpUrl)
        {
            RegisterMessage(scriptstr, true, autoJumpUrl);
        }

        protected void RegisterMessage(string scriptstr, bool autoHidd)
        {
            RegisterMessage(scriptstr, autoHidd, "");
        }

        protected void RegisterMessage(string scriptstr)
        {
            RegisterMessage(scriptstr, false);
        }


        #region 调用自定义的方法对VIEWSTATE进行操作

        protected void DiscuzForumSavePageState(object viewState)
        {
            string keyid = userid + "_" + this.GetType().Name.Trim();
            DiscuzControlContainer dcc = DiscuzControlContainer.GetContainer();
            dcc.AddNormalComponent(keyid, viewState);
        }

        protected object DiscuzForumLoadPageState()
        {
            string keyid = userid + "_" + this.GetType().Name.Trim();
            DiscuzControlContainer dcc = DiscuzControlContainer.GetContainer();
            object viewState = (object)dcc.GetNormalComponentDataObject(keyid);
            dcc.RemoveComponentByName(keyid);
            if (viewState != null) return viewState;
            else return null;
        }

        #endregion

        #region 把VIEWSTATE以组件的形式存入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.SavePageStateToPersistenceMedium(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            object o = new object();
            try
            {
                o = base.LoadPageStateFromPersistenceMedium();
            }
            catch
            {
                o = null;
            }
            return o;
        }

        #endregion
    }


}