using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 管理日志
    /// </summary>
    public class usercpspacemanageblog : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 日志总数
        /// </summary>
        public int blogcount;
        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 日志列表
        /// </summary>
        public DataTable bloglist = new DataTable();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        private int pagesize = 15;
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
                if (DNTRequest.GetString("postid") != "")
                {
                    Discuz.Space.Spaces.DeleteSpacePost(DNTRequest.GetString("postid"), userid);
                    SetUrl("usercpspacemanageblog.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("删除完毕");
                }
                else
                {
                    AddErrLine("请选择要删除的日志");
                    return;
                }
            }

            //得到当前用户请求的页数
            pageid = DNTRequest.GetInt("page", 1);
            //获取主题总数
            blogcount = Space.Data.DbProvider.GetInstance().GetSpacePostsCount(userid);
            //获取总页数
            pagecount = (blogcount%pagesize == 0) ? (blogcount/pagesize) : (blogcount/pagesize + 1);
            if (pagecount == 0)
                pagecount = 1;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            if (pageid > pagecount)
                pageid = pagecount;

            bloglist = Space.Data.DbProvider.GetInstance().SpacePostsList(pagesize, pageid, userid);
            bloglist.Columns.Add("shorttitle");
            foreach (DataRow dr in bloglist.Rows)
            {
                dr["category"] = Space.Data.DbProvider.GetInstance().GetCategoryNameByIdList(dr["category"].ToString());
                dr["shorttitle"] = dr["title"];
                if (dr["shorttitle"].ToString().Length > 30)
                {
                    dr["shorttitle"] = dr["shorttitle"].ToString().Substring(0, 30) + "...";
                }
            }
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpspacemanageblog.aspx", 8);
        }
    }
}