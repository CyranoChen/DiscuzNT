using Discuz.Common;
using Discuz.Forum;
 
using Discuz.Space.Utilities;
using Discuz.Config;
using Discuz.Space.Provider;


namespace Discuz.Space.Modules
{
	/// <summary>
	/// NewCommentModule 的摘要说明。
	/// </summary>
	public class NewCommentModule : BlogModule
	{
		private string filename = BaseConfigs.GetForumPath + "space/Modules/builtin/blog/defaultmodule.config";

		public NewCommentModule()
		{
			modulename = "最新评论";
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
			output.Append("<div id=\"ajaxtopnewcomment\">正在加载数据...</div>\r\n");
            output.Append("<script language=\"javascript1.2\" type=\"text/javascript\">AjaxProxyUrl = new String(\"" + BaseConfigs.GetForumPath + "space/manage/ajax.aspx\"); \r\n AjaxHelper.Updater('" + BaseConfigs.GetForumPath + "space/manage/usercontrols/ajaxtopnewcomment.ascx','ajaxtopnewcomment','load=true&hidetitle=1&spaceid=" + this.SpaceConfig.SpaceID + "&commentnumber=" + this.Module.Val + "');</script>\r\n");
			return output.ToString();																
		}
	}
}
