using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Data;

namespace Discuz.Space.Manage
{
	/// <summary>
    ///	最新日志列表控件
	/// </summary>
	public class ajaxtopnewpost : DiscuzSpaceUCBase
	{
		public SpacePostInfo[] __spacepostinfos;

		public ajaxtopnewpost()
		{
			if(DNTRequest.GetString("load")=="true")
			{
				pagesize = DNTRequest.GetInt("postnumber",10);
                __spacepostinfos = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().SpacePostsList(pagesize, 1, spaceconfiginfo.UserID, 1));
			}
		}
	}
}
