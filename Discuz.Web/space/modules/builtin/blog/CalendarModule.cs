using System;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Utilities;
using Discuz.Config;

namespace Discuz.Space.Modules
{
	/// <summary>
	/// CalendarModule 的摘要说明。
	/// </summary>
	public class CalendarModule: BlogModule
	{
		public CalendarModule()
		{
			modulename = "日历";
		}

		public override string OnEditBoxLoad(string editbox)
		{
			this.ModulePref.Title = this.modulename;
			return "";	//Globals.GetFileContent(Utils.GetMapPath(filename)).Replace("m___MODULE_ID___val", "m___MODULE_ID___val\" value=\""+this.Module.Val+"\""); 
		}

		public override string OnMouduleLoad(string content)
		{
			IsScalableAndEditable();

			this.Editable = false;

			this.ModulePref.Title = this.modulename;

			//output.Append("<h3 class=\"NtSpace-blocktitle\">日历</h3>\r\n");
			output.Append("<div id=\"usercalendar\" class=\"NtSpace-sideblock\">\r\n");
			output.Append("</div>\r\n");
			output.Append("<script type=\"text/javascript\" src=\"" + BaseConfigs.GetForumPath + "space/manage/js/spacecalender.js\"></script>\r\n");
			output.Append("<script language=\"javascript1.2\" type=\"text/javascript\">\r\n");
			output.Append("//加载日历\r\n");
			output.Append("var spaceid = '"+this.SpaceConfig.SpaceID+"';\r\n");
			output.Append("var hidetitle = '1';\r\n");
			
			output.Append("HS_setDate(document.getElementById('usercalendar'));\r\n");
			output.Append("</script>\r\n");

//			output.Append("<div id=\"calendar\">正在加载数据...</div>\r\n");
//			output.Append("<script language=\"javascript1.2\" type=\"text/javascript\">AjaxProxyUrl = new String(\"manage/ajax.aspx\"); \r\n AjaxHelper.Updater('usercontrols/spacecalendar.ascx','calendar','load=true&hidetitle=1');</script>\r\n");
			return output.ToString();
																
		}
	}
}
