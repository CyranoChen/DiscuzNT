using System;
using System.Text;
using System.Web;
using System.Web.UI;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Space.Pages;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Space
{
	/// <summary>
	/// IframeÄ£¿é×ª»»Æ÷
	/// </summary>
	public class ifr : SpaceBasePage
	{
		private const string IFRAME_HTML = @"<html><head><style type=""text/css"">
<!--
body,td,div,span,p{{font-family:arial,sans-serif;}}a {{ color:#0000cc; }}a:visited {{ color:#551a8b; }}a:active {{ color:#ff0000; }}body {{margin: 0px;padding: 0px; background-color: white; }}
-->
</style></head><body><script type=""text/javascript"">var forumpath='{2}space/';</script><script src=""{2}space/javascript/space.js""></script><script>function sendRequest(iframe_id, service_name, args_list, remote_relay_url,callback, local_relay_url) {{_IFPC.call(iframe_id, service_name, args_list, remote_relay_url, callback,local_relay_url);}}_DS_Prefs._parseURL(""__MODULE_ID__"");_DS_Prefs._add(""__MODULE_ID__"", """", """");</script>{0}<div id=remote___MODULE_ID__ style=""border:0;padding:0;margin:0;width:100%;overflow:hidden;"">{1}</div><script><!--
_DS_TriggerEvent(""domload"");// -->
		</script></body></html>";
		private const string PREF_SCRIPT_FORMAT = "_DS_Prefs._add(\"{0}\",\"up_{1}\",\"{2}\");";

		public ifr()
		{
			HttpContext.Current.Response.ContentType = "text/html";
//			if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()))
//				return;

//			if (userid < 1)
//				return;
			string url = DNTRequest.GetQueryString("url");
			int mid = DNTRequest.GetQueryInt("mid", 0);
            int uid = DNTRequest.GetQueryInt("uid", 0);
			ModuleInfo moduleinfo = Spaces.GetModuleById(mid, uid);
			if (moduleinfo == null)
			{
				return;
			}
			if (url != moduleinfo.ModuleUrl)
			{
				return;
			}
			UserPrefsSaved ups = new UserPrefsSaved(moduleinfo.UserPref);

			string path = BaseConfigs.GetForumPath + "space/modules/" + url;
			string modulefilepath = Utils.GetMapPath(path);
			ModulePref mp = ModuleXmlHelper.LoadModulePref(modulefilepath);
			ModuleContent mc = ModuleXmlHelper.LoadContent(modulefilepath);

#if NET1			
            UserPrefCollection upc = ModuleXmlHelper.LoadUserPrefs(modulefilepath);
#else
            UserPrefCollection<UserPref> upc = ModuleXmlHelper.LoadUserPrefs(modulefilepath);
#endif
			
            StringBuilder registScript = new StringBuilder("<script type=\"text/javascript\">");
			foreach (UserPref up in upc)
			{
				string userprefvalue = ups.GetValueByName(up.Name);
				userprefvalue = userprefvalue == string.Empty ? up.DefaultValue : userprefvalue;
				registScript.AppendFormat(PREF_SCRIPT_FORMAT, mid, up.Name, userprefvalue);
			}
			registScript.Append(ModuleXmlHelper.GetModuleRequireScript(mp, mc.Type == ModuleContentType.HtmlInline));
			registScript.Append("</script>");
			string html = string.Format(IFRAME_HTML, registScript.ToString(), mc.ContentHtml, BaseConfigs.GetForumPath);
			html = html.Replace("__MODULE_ID__", mid.ToString()).Replace("_IG_", "_DS_");


			HttpContext.Current.Response.Write(html);
			HttpContext.Current.Response.End();




		}




	}


}
