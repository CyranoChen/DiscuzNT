using System;
using System.Web;
using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Pages;
using Discuz.Entity;

namespace Discuz.Space
{
	/// <summary>
	/// setp 的摘要说明。
	/// </summary>
	public class setp : SpaceBasePage
	{
		public setp()
		{
//			if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()))
//				return;
			string url = DNTRequest.GetString("url");//choose tab

			if (userid < 1)
				return;


			int ct = DNTRequest.GetInt("ct", 0);//choose tab
			int dt = DNTRequest.GetInt("dt", 0);//delete tab
			string mt = DNTRequest.GetString("mt");//move module to tab
			if (ct > 0)
			{
				Spaces.SetDefaultTab(this.userid, ct);
			}

			if (dt > 0)
			{
				TabInfo tabinfo = Spaces.GetTabById(dt, this.userid);
				if (tabinfo.UserID == this.userid)
				{
					Spaces.DeleteTabById(dt, this.userid);
				}
			}
			
			if (mt != "")
			{
				string[] moduleTab = mt.Split(':');
				if (moduleTab.Length != 2)
				{
					return;
				}
                if (!Utils.IsNumeric(moduleTab[0]) || !Utils.IsNumeric(moduleTab[1]))
				{
					return;
				}
				Spaces.MoveModule(Utils.StrToInt(moduleTab[0], 0), this.userid, Utils.StrToInt(moduleTab[1], 0));

			}

			string action = DNTRequest.GetString("action"); 

			if (ct == 0 && dt == 0 && mt == "")
			{

				switch (action)
				{
					case "move" :
						MoveAction();
						break;
					case "renametab" :
						RenameTabAction();
						break;
					case "addtab" : AddTabAction();
						break;
					case "changetheme" : ChangeThemeAction();
						break;
					case "changetemplate" : ChangeTemplateAction();
						break;
					case "addmodule" : AddModuleAction();
						break;
					case "delmodule" : DelModuleAction();
						break;
                    case "updatecustomizepanel" :
                        UpdateCustomizePanel();
                        break;
					default : DefaultAction();
						break;
				}

			}
//			if (action != "renametab" && mt == "")
//				HttpContext.Current.Response.Redirect(DNTRequest.GetUrlReferrer());
			if (url != "")
				HttpContext.Current.Response.Redirect(url);
		}


        protected override void OnInit(EventArgs e)
        {
            ResponseXML("<?xml version=\"1.0\" encoding=\"UTF-8\" ?> \r\n<empty />");
        }
        private static void ResponseXML(string xmlcontent)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "Text/XML";
            HttpContext.Current.Response.Expires = 0;
            
            HttpContext.Current.Response.Cache.SetNoStore();            
            HttpContext.Current.Response.Write(xmlcontent);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 更新自定义面板
        /// </summary>
        private void UpdateCustomizePanel()
        {
            int moduleid = DNTRequest.GetInt("cp_module_id", 0);
            string modulecontent = Utils.UrlDecode(DNTRequest.GetString("cp_module_content"));
            if (moduleid > 0 && modulecontent != string.Empty)
            {
                if (Spaces.ExistCustomizePanelContent(moduleid, this.userid))
                {
                    Spaces.UpdateCustomizePanelContent(moduleid, this.userid, modulecontent);
                }
                else
                {
                    Spaces.AddCustomizePanelContent(moduleid, this.userid, modulecontent);
                }
            }
        }



		private void MoveAction()
		{
			string panename = Utils.UrlDecode(DNTRequest.GetQueryString("panename"));
			string modules_mp = Utils.UrlDecode(DNTRequest.GetQueryString("modules_mp"));
			if (!Utils.IsNumericArray(modules_mp.Split(',')))
			{
				return;
			}
			string[] moduleArray = modules_mp.Split(',');
			int order = 1;
			foreach(string mid in moduleArray)
			{
				Spaces.UpdateModuleOrder(Convert.ToInt32(mid), this.userid, panename, order);
				order += 2;
			}
		}

		private void DefaultAction()
		{
			//			string url = HttpContext.Current.Request.QueryString["url"].ToString();
			//			string et = HttpContext.Current.Request.QueryString["et"].ToString();
			//			string source = HttpContext.Current.Request.QueryString["source"].ToString();
			//			string pid = HttpContext.Current.Request.QueryString["pid"].ToString();
			//			string ap = HttpContext.Current.Request.QueryString["ap"].ToString();
			//			string prefid = HttpContext.Current.Request.QueryString["prefid"].ToString();
			int mid = DNTRequest.GetQueryInt("mid", 0);
/*
			int tabid = DNTRequest.GetQueryInt("m_" + mid + "_t", 0);
			TabInfo tab = Spaces.GetTabById(tabid);
			if (tab == null || tab.UserID != userid)
			{
				//非本人模块不可修改
				return;
			}
*/

			//			string host = HttpContext.Current.Request.QueryString["host"].ToString();
			//			string h1 = HttpContext.Current.Request.QueryString["h1"].ToString();
			
			//			string upPrefix = "m_" + mid + "_up_";
			//
			//			Hashtable ht = new Hashtable();
			//			NameValueCollection nvc = Request.QueryString;
			//			foreach (string s in nvc.Keys)
			//			{
			//				if (s.StartsWith(upPrefix))
			//				{
			//					ht.Add(s.Replace(upPrefix, string.Empty), nvc[s]);
			//				}
			//			}

			ModuleInfo module = Spaces.GetModuleById(mid, this.userid);
/*			if (module == null || module.TabID != tabid)
			{
				return;
			}
*/
			if (module == null || module.Uid != this.userid)
			{
				return;
			}
				//			ModuleBase desktopModule = new ModuleBase();
				//			//create instance accord to the moduledef 
				//			if (module.ModuleDefID > 0)
				//			{
				//				ModuleDefInfo md = Spaces.GetModuleDefByID(module.ModuleDefID);
				//				desktopModule = (ModuleBase)GetInstance(md.BussinessController);
				//				desktopModule.ModuleDef = md;
				//
				//
				//
				//
				//				//get module xml and isremote
				////				if (module.ModuleUrl.StartsWith("http://"))
				////				{
				////					desktopModule.IsRemote = true;
				////				}
				//
				//				//can not support remote module yet.
				//
				//
				//			}
				//			desktopModule.Module = module;
			//SpaceConfigInfo config = Spaces.GetSpaceConfigByUserId(this.userid);
			ModuleBase desktopModule = Spaces.SetModuleBase(module);
			desktopModule.UserID = this.userid;
			//desktopModule.SpaceConfig = config;



		






		

			ISpaceCommand command = desktopModule;
			command.GetModulePost(HttpContext.Current);

		}

		private void RenameTabAction()
		{
			int tabid = DNTRequest.GetQueryInt("t", 0);
			string tabname = DNTRequest.GetQueryString("rt_" + tabid);
			tabname = Utils.UrlDecode(tabname);
			TabInfo tab = Spaces.GetTabById(tabid, this.userid);
			if (tab.UserID != this.userid)
			{
				return;
			}
			tab.TabName = tabname;
			Spaces.UpdateTab(tab);
		}

		private void AddTabAction()
		{
			int tabcount = Spaces.GetTabCountByUserId(this.userid);
			int maxdisplayorder = DNTRequest.GetQueryInt("mo", 0);
			if (tabcount >= 5)
				return;
			TabInfo tabinfo = new TabInfo();
			tabinfo.TabID = Spaces.GetNewTabId(this.userid);
			tabinfo.DisplayOrder = maxdisplayorder;
			tabinfo.TabName = "新建页面";
			tabinfo.Template = "template_33_33_33.htm";
			tabinfo.UserID = this.userid;

			Spaces.AddTab(tabinfo);

			//清除config中的默认页面，交给spacepage去处理
			Spaces.ClearDefaultTab(this.userid);
		}

		private void ChangeThemeAction()
		{
			int themeid = DNTRequest.GetQueryInt("themeid", 0);
			string themepath = DNTRequest.GetQueryString("themepath", true);
			Spaces.SetTheme(this.userid, themeid, themepath);
		}

		private void ChangeTemplateAction()
		{
			int tabid = DNTRequest.GetQueryInt("t", 0);
			string template = DNTRequest.GetQueryString("template");

			TabInfo tab = Spaces.GetTabById(tabid, this.userid);
			if (tab.UserID != this.userid)
			{
				return;
			}
			tab.Template = template;
			Spaces.UpdateTab(tab);
		}

		private void AddModuleAction()
		{
			int tabid = DNTRequest.GetQueryInt("t", 0);
			int tabModuleCount = Spaces.GetModulesCountByTabId(tabid, this.userid);

			if (tabModuleCount > 20)
				return;
			string url = Utils.UrlDecode(DNTRequest.GetQueryString("x"));
            Random random = new Random();
            int col = random.Next(1, 5);//DNTRequest.GetQueryInt("col", 1);

			TabInfo tab = Spaces.GetTabById(tabid, this.userid);
			if (tab == null || tab.UserID != this.userid)
			{
				return;
			}
			int moduledef = 0;
			if (url.StartsWith("builtin_"))
			{
				moduledef = Spaces.GetModuleDefIdByUrl(url);
			}
			ModuleType mt = Utilities.ModuleValidate.ValidateModuleType(url);
			if (moduledef > 0)
			{
				mt = ModuleType.Local;
			}
			ModuleInfo moduleinfo = new ModuleInfo();
			moduleinfo.ModuleID = Spaces.GetNewModuleId(this.userid);
			moduleinfo.DisplayOrder = 0;
			moduleinfo.ModuleDefID = moduledef;
			moduleinfo.ModuleType = mt;
			moduleinfo.ModuleUrl = url;
			moduleinfo.PaneName = "pane" + col;
			moduleinfo.TabID = tabid;
			moduleinfo.Uid = this.userid;
			moduleinfo.Val = 6;

			Spaces.AddModule(moduleinfo);

		}

		private void DelModuleAction()
		{
			int moduleid = DNTRequest.GetQueryInt("m", 0);
			if (moduleid == 0)
			{
				return;
			}
			int tabid = DNTRequest.GetQueryInt("m_" + moduleid + "_t", 0);
			ModuleInfo moduleinfo = Spaces.GetModuleById(moduleid, this.userid);

			if (moduleinfo.TabID != tabid || moduleinfo.Uid != this.userid)
			{
				return;
			}
            ISpaceCommand isc = Spaces.SetModuleBase(moduleinfo);
            isc.RemoveModule();			
		}
	}


}
