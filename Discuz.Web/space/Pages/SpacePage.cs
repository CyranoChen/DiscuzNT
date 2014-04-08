using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Space.Utilities;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Space.Pages
{
	/// <summary>
	/// 空间页面
	/// </summary>
	public class SpacePage : Page
	{
		private bool spaceEditable = false;
		private bool isLogged = false;
		private SpaceConfigInfo spaceConfig;
		/// <summary>
		/// 当前页面开始载入时间(毫秒)
		/// </summary>
		private TimeSpan begints = new TimeSpan(DateTime.Now.Ticks);
		private double processtime = 0;
		private int userid;
		private int olid;
		private string username;
        private SpaceActiveConfigInfo config;
		private string password;
		private string userkey;
        /// <summary>
        /// 声明并设置空间名称变量
        /// </summary>
        protected string spacename = GeneralConfigs.GetConfig().Spacename;
        /// <summary>
        /// 声明并设置相册名称变量
        /// </summary>
        protected string albumname = GeneralConfigs.GetConfig().Albumname;
        /// <summary>
        /// 声明并设置空间url
        /// </summary>
        protected string configspaceurl = GeneralConfigs.GetConfig().Spaceurl;
        /// <summary>
        /// 声明不带页面名字的空间url
        /// </summary>
        protected string configspaceurlnopage;
        /// <summary>
        /// 声明并设置相册url
        /// </summary>
        protected string configalbumurl = GeneralConfigs.GetConfig().Albumurl;
        /// <summary>
        /// 不带文件名的forumurl地址
        /// </summary>
        protected string forumurlnopage = BaseConfigs.GetForumPath;

        protected string forumurl = GeneralConfigs.GetConfig().Forumurl;
        

		/// <summary>
		/// 得到当前页面的载入时间供模板中调用(单位:毫秒)
		/// </summary>
		/// <returns>当前页面的载入时间</returns>
		public double Processtime
		{
			get { return processtime; }
		}

		public SpacePage()
		{
            //去掉http地址中的文件名称
            if (forumurl.ToLower().IndexOf("http://") == 0)
                forumurlnopage = forumurl.Substring(0, forumurl.LastIndexOf('/')) + "/";
            else
                forumurl = "../" + forumurl;

            if (GeneralConfigs.GetConfig().Enablespace != 1)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("<script>alert('" + spacename + "功能已被关闭！');document.location = '" + forumurl + "';</script>");
                HttpContext.Current.Response.End();
                return;
            }
			userid = Utils.StrToInt(ForumUtils.GetCookie("userid"), -1);
            config = SpaceActiveConfigs.GetConfig();
            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(GeneralConfigs.GetConfig().Passwordkey, GeneralConfigs.GetConfig().Onlinetimeout);
			olid = oluserinfo.Olid;
			userid = oluserinfo.Userid;
			username = oluserinfo.Username;
			password = oluserinfo.Password;
			if (password.Length > 16)
				userkey = password.Substring(4, 8).Trim();
			else
				userkey = "";

			if (!IsPostBack)
			{
				OutputSpacePage();
			}
		}


		/// <summary>
		/// 输出页面
		/// </summary>
		private void OutputSpacePage()
		{
			string root = BaseConfigs.GetForumPath;    
            string user = DNTRequest.GetQueryString("user");   
			int currentuid = DNTRequest.GetQueryInt("uid", -1);
			if (currentuid < 1)
			{
                if (user != string.Empty)
                    currentuid = Users.GetUserIdByRewriteName(user);

                if (currentuid < 1)
                    currentuid = this.userid;
			}

			if (userid > 0)
			{
				isLogged = true;
				if (userid == currentuid)
					spaceEditable = true;
			}

            if (currentuid < 1 && this.userid < 1)
                HttpContext.Current.Response.Redirect(forumurlnopage + "login.aspx?reurl=space");
            else//对用户空间访问量加1
                Data.DbProvider.GetInstance().CountUserSpaceVisitedTimesByUserID(currentuid);

			int currenttabid = DNTRequest.GetQueryInt("tab", 0);
			spaceConfig = Spaces.GetSpaceConfigByUserId(currentuid);
            ShortUserInfo sui = Users.GetShortUserInfo(currentuid);

            if (spaceConfig == null || sui == null || spaceConfig.Status != SpaceStatusType.Natural || sui.Spaceid <= 0)
			{
                if (userid == currentuid && userid > 0)
                    HttpContext.Current.Response.Redirect(forumurlnopage + "spaceregister.aspx");
                else
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Write("<script>alert('当前用户未开通" + spacename + "！');document.location = '" + forumurl + "';</script>");
                    HttpContext.Current.Response.End();
                }
				return;
			}
            
            Discuz.Common.Generic.List<TabInfo> tc = Spaces.GetTabInfoCollectionByUserID(currentuid);
			if (tc == null)
                tc = new Discuz.Common.Generic.List<TabInfo>();

			int defaulttabid = 0;
			if (tc.Count > 0)
			{
                //此处修改为始终显示第一个tab
				//defaulttabid = tc[tc.Count - 1].TabID;
                defaulttabid = tc[0].TabID;
                spaceConfig.DefaultTab = defaulttabid;
			}

			StringBuilder tabsHtml = new StringBuilder();
            TabInfo currentTab = null;

            foreach (TabInfo tab in tc)
            {
                if (tab.TabID == (currenttabid == 0 ? spaceConfig.DefaultTab : currenttabid))
                {
                    defaulttabid = tab.TabID;
                    break;
                }
            }            

			foreach (TabInfo tab in tc)
			{
				tabsHtml.Append("<li class=\"tab unselectedtab_l\">&nbsp;</li>");
				if (tab.TabID == defaulttabid)
				{
					currentTab = tab;
					string tabformat = "";
					if (spaceEditable)
					{
						tabformat += @"<li id=""tab{0}_edit"" class=""tab edittab selectedtab"" style=""display:none"" onclick=""_gel('tab{0}_title').focus();"" onmouseover=""_disable_onblur=true;"" onmouseout=""_disable_onblur=false;""><script>_disable_onblur = false;</script><form id=""tab{0}_rename_form"" name=""tab{0}_rename_form"" onsubmit=""return _renameTab();""><span><input id=""tab{0}_title"" name=""tab_title"" value=""{1}"" onblur=""_disable_onblur ? void(0) : _renameTab();"">";
						if (tc.Count > 1)
							tabformat += @"<a href=""#"" onclick=""if (confirm('您确定要删除 &quot;TAB_NAME&quot; 页面及其所有内容吗?'.replace('TAB_NAME', _editedTabName()))) {{_dlsetp('dt={0}');}}return false;""><img src=""" + BaseConfigs.GetForumPath + "space/images/clear.gif\" border=\"0\" /></a>";

                        tabformat += "</span></form></li>";
					}
					tabformat += @"<li id=""tab{0}_view"" class=""tab selectedtab"" style=""display:block"" ";
					if (spaceEditable)
						tabformat += @"onclick=""_editTab('{0}')"" title=""单击修改页面名称""";

                    tabformat += @"><span id=""tab{0}_view_title"">{1}</span></li>";
					tabsHtml.AppendFormat(tabformat, tab.TabID, tab.TabName);
				}
				else
				{
					string tabformat = "<li id=\"tab{0}_view\" class=\"tab unselectedtab\" style=\"display:block\" onclick=\"";
					tabformat += "document.location=ds_i()+'tab={0}'";
					tabformat += "\"><span id=\"tab{0}_view_title\">{1}</span></li>";
					tabsHtml.AppendFormat(/*"<li id=\"tab{0}_view\" class=\"tab unselectedtab\" style=\"display:block\" onclick=\"_dlsetp('ct={0}')\"><span id=\"tab{0}_view_title\">{1}</span></li>"*/tabformat, tab.TabID, tab.TabName);
				}
				tabsHtml.Append("<li class=\"tab unselectedtab_r\">&nbsp;</li>");
			}

			// for test use the specific template
			string template = "";
			string tempPath = "";
			if (currentTab != null && currentTab.Template != string.Empty)
				tempPath = Utils.GetMapPath(root + "/space/skins/templates/" + currentTab.Template + "");

            if (currentTab == null || currentTab.Template == string.Empty || !File.Exists(tempPath))
				template = "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" id=\"t_1\"><tr><td style=\"width: 67%;\" id=\"col_1\" class=\"col\">{pane1}</td></tr></table><script>alert('您选择的板式不存在或者已经被删除，请重新选择板式。');</script>";
			else
				template = StaticFileProvider.GetContent(tempPath);

            Discuz.Common.Generic.List<ModuleInfo> mc = Spaces.GetModuleCollectionByTabId(defaulttabid, currentuid);

			template = ParseModules(mc, template);

			Hashtable ht = new Hashtable();
			ht.Add("modules", template);
			ht.Add("islogged", isLogged);
			ht.Add("editable", spaceEditable);
			ht.Add("userkey", this.userkey);
			ht.Add("username", this.username);
			ht.Add("userid", this.userid);
			ht.Add("config", spaceConfig);
			ht.Add("tabs", tabsHtml.ToString());
			if (currentTab != null)
				ht.Add("currenttab", currentTab);

            ht.Add("tabid", defaulttabid);
			ht.Add("tabcount", tc.Count);
			ht.Add("can_be_added", tc.Count < 5 && spaceEditable);
            ht.Add("footer", SpaceActiveConfigs.GetConfig().SpaceFooterInfo);
            ht.Add("forumpath", BaseConfigs.GetForumPath);
       
            //在模板中设置相册名称变量
            ht.Add("spacename", spacename);

            ht.Add("albumname", albumname);


            if (configspaceurl.ToLower().IndexOf("http://") < 0)
            {
                configspaceurl = forumurlnopage + configspaceurl;
            }
            ht.Add("configspaceurl", configspaceurl);

            if (configspaceurl.ToLower().IndexOf("http://") < 0)
                configspaceurlnopage = forumurlnopage;
            else
                configspaceurlnopage = configspaceurl.Substring(0, configspaceurl.LastIndexOf('/')) + "/";

            ht.Add("configspaceurlnopage", configspaceurlnopage);
          
            //获取配置文件中的space空间路径
            string spaceurl = GeneralConfigs.GetConfig().Spaceurl;
            if (spaceurl.ToLower().IndexOf("http://") == 0)
                spaceurl = spaceurl.Substring(0, spaceurl.LastIndexOf('/')) + "/space/" ;
            else
            {
                //从当前的URL请求中获取相关站点及(虚拟)路径信息
                spaceurl = HttpContext.Current.Request.Url.ToString();
                spaceurl = spaceurl.Substring(0, spaceurl.LastIndexOf('/')) + "/";
            }
            if (config.Enablespacerewrite > 0 && spaceConfig.Rewritename != string.Empty)
                spaceurl += spaceConfig.Rewritename;
            else
                spaceurl += "?uid=" + spaceConfig.UserID;

            ht.Add("spaceurl", spaceurl);

            if (configalbumurl.ToLower().IndexOf("http://") < 0)
                configalbumurl = forumurlnopage + configalbumurl;

            ht.Add("configalbumurl", configalbumurl);
            ht.Add("forumurl", forumurl);
            ht.Add("forumurlnopage", forumurlnopage);
            ht.Add("editbar", EditbarTemplate.Instance.GetHtml(ht));

            string content = MainTemplate.Instance.GetHtml(ht);
			HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(content.Replace("IG_", "DS_").Replace("ig_", "dnt_"));
			HttpContext.Current.Response.End();

		}

		/// <summary>
		/// 合并所有模块的html
		/// </summary>
		/// <param name="mc">模块集合</param>
		/// <param name="template">版式内容</param>
		/// <returns></returns>
        private string ParseModules(Discuz.Common.Generic.List<ModuleInfo> mc, string template)
  		{
			string hiddenDiv = "<div style='width: 100%'>&nbsp;</div>";
			if (mc == null || mc.Count < 1)
			{
				template = template.Replace("{pane1}", hiddenDiv);
				template = template.Replace("{pane2}", hiddenDiv);
				template = template.Replace("{pane3}", hiddenDiv);
				template = template.Replace("{pane4}", hiddenDiv);
                return template + "<script type='text/javascript'>var currentTabModule = new Array();</script>";
			}
	
			Hashtable htPane = new Hashtable();
			string paneName = "pane1";//mc[0].PaneName;
	
			//先塞到ht
			string currentKey = Guid.NewGuid().ToString();
			string firstKey = currentKey;
            string currentTabModule = "<script type='text/javascript'>var currentTabModule = new Array({0});</script>";
            string[] currentTabModuleArray = new string[mc.Count];
			template = template.Replace("{" + paneName + "}", currentKey);
			htPane.Add(currentKey, string.Empty);

            for (int i = 0; i < mc.Count; i++)
            {
                ModuleBase desktopModule = Spaces.SetModuleBase(mc[i]);
                desktopModule.UserID = this.userid;
                desktopModule.SpaceConfig = spaceConfig;
                currentTabModuleArray[i] = string.Format("'{0}'", mc[i].ModuleUrl);

                if (template.IndexOf(mc[i].PaneName) == -1 && paneName != mc[i].PaneName)
                {
                    htPane[firstKey] = htPane[firstKey] + desktopModule.GetModuleHtml();
                }
                else
                {
                    if (paneName != mc[i].PaneName)
                    {
                        currentKey = Guid.NewGuid().ToString();
                        template = template.Replace("{" + mc[i].PaneName + "}", currentKey);
                        htPane.Add(currentKey, string.Empty);

                        paneName = mc[i].PaneName;
                    }
                    htPane[currentKey] = htPane[currentKey] + desktopModule.GetModuleHtml();
                }
            }

            currentTabModule = string.Format(currentTabModule, string.Join(",", currentTabModuleArray));
			template = template.Replace("{pane1}", hiddenDiv);
			template = template.Replace("{pane2}", hiddenDiv);
			template = template.Replace("{pane3}", hiddenDiv);
			template = template.Replace("{pane4}", hiddenDiv);
	
			foreach (DictionaryEntry de in htPane)
			{
				template = template.Replace(de.Key.ToString(), de.Value.ToString() + hiddenDiv);
			}

            template += currentTabModule;
			return template;
		}
	}
}
