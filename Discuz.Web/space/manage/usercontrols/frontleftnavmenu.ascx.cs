using System;
using Discuz.Space.Manage;
using Discuz.Config;

namespace  Discuz.Space.Manage
{	
	/// <summary>
	///	左侧用户菜单控件
	/// </summary>
	public class frontleftnavmenu : DiscuzSpaceUCBase
	{
		/// <summary>
        /// 是否显示用户面板
		/// </summary>
		public bool isshowuserpanel  = false;

        public string forumpath = BaseConfigs.GetForumPath;

		public frontleftnavmenu()
		{
			if(spaceid > 0)
			{
				isshowuserpanel= base.IsHolder();
			}
		}
	}
}
