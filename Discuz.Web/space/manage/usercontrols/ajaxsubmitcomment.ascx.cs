using System;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Entity;

namespace Discuz.Space.Manage
{
	
	/// <summary>
	///	评论提交控件
	/// </summary>
	public class ajaxsubmitcomment : DiscuzSpaceUCBase
	{

		public string completeinfo = "";
		/// <summary>
        /// 评论信息
		/// </summary>
        public string commentcontent = DNTRequest.GetString("commentcontent");
		/// <summary>
        /// 作者的EMAIL
		/// </summary>
        public string commentemail = DNTRequest.GetString("commentemail");
        /// <summary>
        /// 作者名称
        /// </summary>
        public string commentauthor = DNTRequest.GetString("commentauthor");
        /// <summary>
        /// 作者主页
        /// </summary>
        public string commenturl = DNTRequest.GetString("commenturl");
        /// <summary>
        /// 验证码
        /// </summary>
        public string vcode = DNTRequest.GetString("vcode");
        /// <summary>
        /// 验证码
        /// </summary>
		public int postid =  0;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				if(Discuz.Common.DNTRequest.GetString("load") =="true")
				{
					postid = DNTRequest.GetInt("postid", 0);

					//提交保存时
					if (DNTRequest.GetString("submit") == "true")
					{
						Submit_CategoryInfo();
						return;
					}
					if (userid > 0)
					{
						commentcontent = "";
						commentemail = _userinfo.Email;
						commentauthor = _userinfo.Username;
						commenturl  = _userinfo.Website;
					}
				}
                forumurlnopage = (!forumurlnopage.EndsWith("/"))? forumurlnopage + "/" : forumurlnopage;
			}
		}

		private void Submit_CategoryInfo()
		{
			if (!OnlineUsers.CheckUserVerifyCode(olid, DNTRequest.GetString("vcode")))
			{
				completeinfo = "验证码错误,请重新输入";
				return;
			}
			if (commentcontent == "")
			{
				completeinfo = "请输入评论内容";
				return;
			}

            SpacePostInfo __spacepostinfo = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().GetSpacePost(postid));
			if (__spacepostinfo.CommentStatus == 1)
			{
                completeinfo = "当前日志不允许评论";
				return;
			}		
			if ((__spacepostinfo.CommentStatus == 2)&&(userid < 1))
			{
                completeinfo = "当前日志仅允许注册用户评论";
				return;
			}
	
			SpaceCommentInfo __spacecommentinfo = new SpaceCommentInfo();
			__spacecommentinfo.PostID = postid; 
			__spacecommentinfo.Author = Utils.HtmlEncode(commentauthor != ""?commentauthor:"匿名");
			__spacecommentinfo.Email = Utils.HtmlEncode(commentemail);
			__spacecommentinfo.Url = commenturl;
			__spacecommentinfo.Ip = DNTRequest.GetIP();
			__spacecommentinfo.PostDateTime = DateTime.Now;
			__spacecommentinfo.Content = Utils.HtmlEncode(ForumUtils.BanWordFilter(commentcontent));
			__spacecommentinfo.ParentID = 0;
            __spacecommentinfo.Uid = (commentauthor == username) ? userid: -1;
			__spacecommentinfo.PostTitle = Utils.HtmlEncode(ForumUtils.BanWordFilter(__spacepostinfo.Title));

            Space.Data.DbProvider.GetInstance().AddSpaceComment(__spacecommentinfo);
            Space.Data.DbProvider.GetInstance().CountUserSpaceCommentCountByUserID(__spacepostinfo.Uid, 1);
            Space.Data.DbProvider.GetInstance().CountSpaceCommentCountByPostID(postid, 1);

            if (DNTRequest.GetString("notice") == "true")
                SendSpaceCommentNotice(__spacecommentinfo);
		}

        /// <summary>
        /// 日志评论通知
        /// </summary>
        /// <param name="commentinfo">日志评论信息</param>
        public void SendSpaceCommentNotice(SpaceCommentInfo commentinfo)
        {
            //要回复的用户id
            int replyuserid = DNTRequest.GetInt("userid", 0);
            //当日志有效时
            if (commentinfo.PostID > 0)
            {
                SpacePostInfo __spacepostsinfo = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().GetSpacePost(commentinfo.PostID));
                //当日志有效时
                if (__spacepostsinfo != null)
                {
                    NoticeInfo __noticeinfo = new NoticeInfo();
                    __noticeinfo.Type = NoticeType.SpaceCommentNotice;
                    __noticeinfo.New = 1;
                    __noticeinfo.Posterid = commentinfo.Uid;
                    __noticeinfo.Poster = commentinfo.Author;
                    __noticeinfo.Postdatetime = Utils.GetDateTime();

                    //当回复人与评论作者不是同一人时，则向评论作者发送通知
                    if (__noticeinfo.Posterid != replyuserid && replyuserid > 0)
                    {
                        __noticeinfo.Note = Utils.HtmlEncode(string.Format("<a href=\"userinfo.aspx?userid={0}\">{1}</a> 回复了您的日志信息　<a href =\"viewspacepost.aspx?postid={3}\">{4}</a>.", commentinfo.Uid, commentinfo.Author, config.Spacename, commentinfo.PostID, __spacepostsinfo.Title));
                        __noticeinfo.Uid = replyuserid;
                        Notices.CreateNoticeInfo(__noticeinfo);
                    }

                    //当上面通知的用户与该空间日志作者不同，则还要向主题作者发通知
                    if (__noticeinfo.Posterid != __spacepostsinfo.Uid && __spacepostsinfo.Uid > 0)
                    {
                        __noticeinfo.Note = Utils.HtmlEncode(string.Format("<a href=\"userinfo.aspx?userid={0}\">{1}</a> 评论了您的{2}日志　<a href =\"viewspacepost.aspx?postid={3}\">{4}</a>.", commentinfo.Uid, commentinfo.Author, config.Spacename, commentinfo.PostID, __spacepostsinfo.Title));
                        __noticeinfo.Uid = __spacepostsinfo.Uid;
                        Notices.CreateNoticeInfo(__noticeinfo);
                    }
                }
            }
        }

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
