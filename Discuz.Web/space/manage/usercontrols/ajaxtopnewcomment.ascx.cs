using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Config;

namespace Discuz.Space.Manage
{
	/// <summary>
	///	最新评论控件
	/// </summary>
	public class ajaxtopnewcomment : DiscuzSpaceUCBase
	{
		public SpaceCommentInfo[] __spacecommentinfos ;
        public string forumpath = BaseConfigs.GetForumPath;

		public ajaxtopnewcomment()
		{
			if(DNTRequest.GetString("load") =="true")
			{
				//得到当前评论列表				
				pagesize = DNTRequest.GetInt("commentnumber",10);
                __spacecommentinfos = BlogProvider.GetSpaceCommentInfo(Space.Data.DbProvider.GetInstance().GetSpaceNewComments(pagesize, spaceconfiginfo.UserID));
			}
		}
	}
}
