using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 个人空间评论列表
    /// </summary>
    public class usercpspacecomment : PageBase
    {
        #region 页面变量        
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 空间评论总数
        /// </summary>
        public int commentcount;
        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 评论列表
        /// </summary>
        public DataTable commentlist = new DataTable();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();

        private int pagesize = 10;
        #endregion

        
        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            user = Users.GetUserInfo(userid);
            if (config.Enablespace != 1)
            {
                AddErrLine("个人空间功能已被关闭");
                return;
            }

            if (user.Spaceid <= 0)
            {
                AddErrLine("您尚未开通个人空间");
                return;
            }

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                if (DNTRequest.GetString("commentid") != "")
                {
                    Space.Data.DbProvider.GetInstance().DeleteSpaceComments(DNTRequest.GetString("commentid"), userid);
                    SetUrl("usercpspacecomment.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("删除完毕");
                }
                else
                {
                    AddErrLine("请选择要删除的链接");
                    return;
                }
            }

            //得到当前用户请求的页数
            pageid = DNTRequest.GetInt("page", 1);
            //获取主题总数
            commentcount = Space.Data.DbProvider.GetInstance().GetSpaceCommentsCount(userid);
            //获取总页数
            pagecount = commentcount % pagesize == 0 ? commentcount / pagesize : commentcount / pagesize + 1;
            if (pagecount == 0)
                pagecount = 1;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            if (pageid > pagecount)
                pageid = pagecount;

            //获取评论记录并分页显示
            commentlist = Space.Data.DbProvider.GetInstance().GetSpaceCommentList(pagesize, pageid, userid, false);
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpspacecomment.aspx", 8);
        }
    }
}
