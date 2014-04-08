using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Data;

namespace Discuz.Space.Manage
{
	/// <summary>
	///	友情链接控件
	/// </summary>
	public class ajaxspacelink : DiscuzSpaceUCBase
	{
		public SpaceLinkInfo[] __spacelinkinfos;

        public ajaxspacelink()
		{
			if(DNTRequest.GetString("load") =="true")
			{
				pagesize = DNTRequest.GetInt("linknumber",10);
                __spacelinkinfos = BlogProvider.GetSpaceLinksInfo(Space.Data.DbProvider.GetInstance().GetSpaceLinkList(pagesize, 1, spaceconfiginfo.UserID));
			}
		}
	}
}
