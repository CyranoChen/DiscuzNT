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
    ///	��־�����б�ؼ�
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
					//��ǰ�û��Ƿ�����־������
                    SpacePostInfo __spacepostinfo = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().GetSpacePost(postid));
					if(__spacepostinfo.Uid == userid)
						ispostauthor = true;

                    if(Forum.AdminGroups.GetAdminGroupInfo(_userinfo.Groupid) != null)
                        isadmin = true;

					//���Ƿ���״̬��ǰ���ߵ���־ʱ
					if(__spacepostinfo.PostStatus == 0)
					{
						errorinfo = "��ǰ�����������Ч!";
						return ;
					}					

					//����Ҫɾ���ļ�¼ʱ
					int delcommentid = DNTRequest.GetInt("delcommentid",0);
					if(delcommentid > 0)
					{
                        //�жϸ��û��Ƿ�Ϊ���˻��������
                        if ((UserGroups.GetUserGroupInfo(_userinfo.Groupid).Radminid == 1 && this.isadmin) || (this.spaceconfiginfo.UserID == this.userid))
                            Space.Data.DbProvider.GetInstance().DeleteSpaceComment(delcommentid);

						//�������������
                        Space.Data.DbProvider.GetInstance().CountUserSpaceCommentCountByUserID(__spacepostinfo.Uid, -1);
                        Space.Data.DbProvider.GetInstance().CountSpaceCommentCountByPostID(postid, -1);
					}
	
					//�õ���ǰ�����б�
					__spacecommentinfos  = GetSpaceCommentInfoList(currentpage,postid);
					//�õ�ҳ������
                    pagelink = AjaxPagination(Space.Data.DbProvider.GetInstance().GetSpaceCommentsCountByPostid(postid), 16, currentpage);
				}
			}
			else
				errorinfo = "��ǰ������־�ظ���Ϣ��Ч!";
		}

		private SpaceCommentInfo[] GetSpaceCommentInfoList(int currentpage,int postid)
		{
            return BlogProvider.GetSpaceCommentInfo(Space.Data.DbProvider.GetInstance().GetSpaceCommentListByPostid(16, currentpage, postid, true));
		}

		//// <summary>
		/// ��ҳ����
		/// </summary>
		/// <param name="recordcount">�ܼ�¼��</param>
		/// <param name="pagesize">ÿҳ��¼��</param>
		/// <param name="currentpage">��ǰҳ��</param>	
		public string AjaxPagination(int recordcount, int pagesize, int currentpage)
		{
			return base.AjaxPagination(recordcount,pagesize,currentpage,"usercontrols/ajaxuserspacecommentlist.ascx", "postid="+postid, "usercommentlist");	
		}
	}
}
