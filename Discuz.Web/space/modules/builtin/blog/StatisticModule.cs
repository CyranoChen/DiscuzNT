using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Utilities;
using Discuz.Config;

namespace Discuz.Space.Modules
{
	/// <summary>
	/// StatisticModule 的摘要说明。
	/// </summary>
	public class StatisticModule : BlogModule
	{
		public StatisticModule()
		{
			//modulename = "数据统计";
		}

		public override string OnEditBoxLoad(string editbox)
		{
			//this.ModulePref.Title = this.modulename;
			return ""; 
		}

		public override string OnMouduleLoad(string content)
		{
			IsScalableAndEditable();

			this.Editable = false;

			//this.ModulePref.Title = this.modulename;
			output.Append("<div id=\"infomation\">正在加载数据...</div>\r\n");
            output.Append("<script type=\"text/javascript\">AjaxProxyUrl = new String(\"" + BaseConfigs.GetForumPath + "space/manage/ajax.aspx\"); \r\n AjaxHelper.Updater('" + BaseConfigs.GetForumPath + "space/manage/usercontrols/ajaxspaceconfigstatic.ascx','infomation','load=true&hidetitle=1&spaceid=" + this.SpaceConfig.SpaceID + "'); </script>\r\n");
			return output.ToString();
		}
	}
}

