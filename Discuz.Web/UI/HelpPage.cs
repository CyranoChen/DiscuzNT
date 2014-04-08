using System;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.UI
{
	/// <summary>
	/// ¬€Ã≥∞Ô÷˙“≥√Ê¿‡
	/// </summary>
	public class HelpPage : System.Web.UI.Page
	{
		public string forumtitle;
		public string forumurl;

		public HelpPage()
		{
			GeneralConfigInfo config = GeneralConfigs.GetConfig();
			forumtitle = config.Forumtitle;
			forumurl = config.Forumurl;
		}
	}
}
