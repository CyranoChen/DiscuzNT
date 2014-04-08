using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Space.Data;
using Discuz.Config;
using Discuz.Forum;

namespace Discuz.Space.Manage
{
	/// <summary>
    ///	日志评论列表控件
	/// </summary>
	public class ajaxuserspacecommentlist : DiscuzSpaceUCBase
	{
        public int currentpage = DNTRequest.GetInt("currentpage", 1);

		public int postid = 0;

		public string pagelink = "";

		public bool ispostauthor = false;

        public bool isadmin = false;

		public SpaceCommentInfo[] __spacecommentinfos ;
		
		public ajaxuserspacecommentlist()
		{
			postid = DNTRequest.GetInt("postid",0);

			if(postid > 0)
			{
				if(Discuz.Common.DNTRequest.GetString("load") =="true")
				{
					//当前用户是否是日志的作者
                    SpacePostInfo __spacepostinfo = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().GetSpacePost(postid));
					if(__spacepostinfo.Uid == userid)
						ispostauthor = true;

                    if(Forum.AdminGroups.GetAdminGroupInfo(_userinfo.Groupid) != null)
                        isadmin = true;

					//当是发布状态或当前作者的日志时
					if(__spacepostinfo.PostStatus == 0)
					{
						errorinfo = "当前请求的内容无效!";
						return ;
					}					

					//当有要删除的记录时
					int delcommentid = DNTRequest.GetInt("delcommentid",0);
					if(delcommentid > 0)
					{
                        //判断该用户是否为本人或管理组内
                        if ((UserGroups.GetUserGroupInfo(_userinfo.Groupid).Radminid == 1 && this.isadmin) || (this.spaceconfiginfo.UserID == this.userid))
                            Space.Data.DbProvider.GetInstance().DeleteSpaceComment(delcommentid);

						//更新相关评论数
                        Space.Data.DbProvider.GetInstance().CountUserSpaceCommentCountByUserID(__spacepostinfo.Uid, -1);
                        Space.Data.DbProvider.GetInstance().CountSpaceCommentCountByPostID(postid, -1);
					}
	
					//得到当前评论列表
					__spacecommentinfos  = GetSpaceCommentInfoList(currentpage,postid);
					//得到页码链接
                    pagelink = AjaxPagination(Space.Data.DbProvider.GetInstance().GetSpaceCommentsCountByPostid(postid), 16, currentpage);
				}
			}
			else
				errorinfo = "当前请求日志回复信息无效!";
		}

		private SpaceCommentInfo[] GetSpaceCommentInfoList(int currentpage,int postid)
		{
            return BlogProvider.GetSpaceCommentInfo(Space.Data.DbProvider.GetInstance().GetSpaceCommentListByPostid(16, currentpage, postid, true));
		}

		//// <summary>
		/// 分页函数
		/// </summary>
		/// <param name="recordcount">总记录数</param>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>	
		public string AjaxPagination(int recordcount, int pagesize, int currentpage)
		{
			return base.AjaxPagination(recordcount,pagesize,currentpage,"usercontrols/ajaxuserspacecommentlist.ascx", "postid="+postid, "usercommentlist");	
		}
	}
}
