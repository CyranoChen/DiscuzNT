using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Pages;
 
using Discuz.Space.Utilities;
using Discuz.Entity;
using Discuz.Config;
#if NET1
#else
using Discuz.Common.Generic;
#endif

namespace Discuz.Space
{
	/// <summary>
	/// 种子
	/// </summary>
	public class feeds : SpaceBasePage
	{
		protected override void  OnInit(EventArgs e)
		{
			//tabid先取config，找到defaulttab,检测defaulttab是否属于这个用户，是就用，不是就取这个用户的tab的最后一个，module必须==1

			int module = DNTRequest.GetQueryInt("module", 0);
			if (module != 1)
				return;
			string url = DNTRequest.GetQueryString("url");
			SpaceConfigInfo spaceconfig = Spaces.GetSpaceConfigByUserId(this.userid);
		
            //改为始终为第一是默认,因此需要传递tabid
            int tabid = DNTRequest.GetInt("tab", 0);
#if NET1
		    TabInfoCollection usertabs = Spaces.GetTabInfoCollectionByUserID(this.userid);
#else
            List<TabInfo> usertabs = Spaces.GetTabInfoCollectionByUserID(this.userid);
#endif
            tabid = IsUserTab(tabid, usertabs) ? tabid : 0;

			if (tabid == 0)
				tabid = usertabs[usertabs.Count - 1].TabID;

			TabInfo tab = Spaces.GetTabById(tabid, this.userid);
			if (tab == null || tab.UserID != this.userid)
			{
				return;
			}
			int moduledef = 0;
			string modulepath = BaseConfigs.GetForumPath + "/space/modules/";
			if (url.StartsWith("builtin_"))
			{
				string file = Utils.GetMapPath(modulepath + url);
				if (File.Exists(file))
				{
					moduledef = Spaces.GetModuleDefIdByUrl(url);
				}
				else
				{
					string errmsg = "alert(\"模块文件不存在\")";
					ResponseXML(errmsg);
					return;
				}
			}
			if (moduledef > 0)
			{
				ModuleType mt = Utilities.ModuleValidate.ValidateModuleType(url);//异常或者远程，返回xml说未知模块或不支持的模块
				if (mt == ModuleType.Error || mt == ModuleType.Remote)
				{
					string errmsg = "alert(\"未知模块或不支持的模块\")";
					ResponseXML(errmsg);
					return;
				}
			}
			ResponseXML(string.Format("_add_m(\"x={0}&action=addmodule&t={1}\")", Utils.UrlEncode(url), tabid));
		}

        /// <summary>
        /// 是不是用户的Tab
        /// </summary>
        /// <param name="tabid"></param>
        /// <param name="usertabs"></param>
        /// <returns></returns>
#if NET1
        private bool IsUserTab(int tabid, TabInfoCollection usertabs)
#else
        private bool IsUserTab(int tabid, List<TabInfo> usertabs)
#endif
        {
            foreach (TabInfo info in usertabs)
            {
                if (info.TabID == tabid)
                {
                    return true;
                }
            }
            return false;        
        }

        /// <summary>
        /// 输出xml
        /// </summary>
        /// <param name="xmlcontent"></param>
        private static void ResponseXML(string xmlcontent)
		{
            System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.ContentType = "Text/XML";
			System.Web.HttpContext.Current.Response.Expires = 0;
			
			System.Web.HttpContext.Current.Response.Cache.SetNoStore();	
			System.Web.HttpContext.Current.Response.Write(xmlcontent);
			System.Web.HttpContext.Current.Response.End();
		}
	}


}
