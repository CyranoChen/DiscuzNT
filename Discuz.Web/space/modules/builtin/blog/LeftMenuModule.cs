using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Utilities;
using Discuz.Config;

namespace Discuz.Space.Modules
{
	/// <summary>
	/// LeftMenuModule 的摘要说明。
	/// </summary>
	public class LeftMenuModule: BlogModule
	{
		public LeftMenuModule()
		{
			modulename = "用户菜单";
		}

		public override string OnEditBoxLoad(string editbox)
		{
			this.ModulePref.Title = this.modulename;
			return "";//Globals.GetFileContent(Utils.GetMapPath(filename)).Replace("m___MODULE_ID___val", "m___MODULE_ID___val\" value=\""+this.Module.Val+"\""); 
		}

		public override string OnMouduleLoad(string content)
		{
			IsScalableAndEditable();

			this.Editable = false;

			this.ModulePref.Title = this.modulename;
		
			output.Append("<div id=\"managenavmenu\">正在加载数据...</div>\r\n");
            output.Append("<script language=\"javascript1.2\" type=\"text/javascript\">AjaxProxyUrl = new String(\"" + BaseConfigs.GetForumPath + "space/manage/ajax.aspx\"); \r\n AjaxHelper.Updater('" + BaseConfigs.GetForumPath + "space/manage/usercontrols/frontleftnavmenu.ascx','managenavmenu','load=true&hidetitle=1&spaceid=" + this.SpaceConfig.SpaceID + "');</script>\r\n");
			return output.ToString();
		}
	}
}


