using Discuz.Common;
using Discuz.Forum;
 
using Discuz.Space.Utilities;
using Discuz.Config;
using Discuz.Space.Provider;

namespace Discuz.Space.Modules
{
	/// <summary>
	/// LinkModule 的摘要说明。
	/// </summary>
	public class LinkModule : BlogModule
	{
		
		private string filename = BaseConfigs.GetForumPath+"/space/Modules/builtin/blog/defaultmodule.config";

		public LinkModule()
		{
			modulename = "友情链接";
		}

		public override string OnEditBoxLoad(string editbox)
		{
			this.ModulePref.Title = this.modulename;
			return StaticFileProvider.GetContent(Utils.GetMapPath(filename)).Replace("m___MODULE_ID___val", "m___MODULE_ID___val\" value=\""+this.Module.Val+"\""); 
		}

		public override string OnMouduleLoad(string content)
		{
			IsScalableAndEditable();

			this.ModulePref.Title = this.modulename;
            output.Append("<div id=\"addlink\">");
            if (this.Module.Uid == this.UserID)
            {
                output.Append("<span style='float: right;'><a href='" + BaseConfigs.GetForumPath + "usercpspacelinkadd.aspx' target='_blank'>添加</a></span>");
            }
			output.Append("<div id=\"userlink\">正在加载数据...</div></div>\r\n");
            output.Append("<script language=\"javascript1.2\" type=\"text/javascript\">AjaxProxyUrl = new String(\"" + BaseConfigs.GetForumPath + "space/manage/ajax.aspx\"); \r\n AjaxHelper.Updater('" + BaseConfigs.GetForumPath + "space/manage/usercontrols/ajaxspacelink.ascx','userlink','load=true&hidetitle=1&spaceid=" + this.SpaceConfig.SpaceID + "&linknumber=" + this.Module.Val + "');</script>\r\n");
			return output.ToString();															
		}
	}
}
