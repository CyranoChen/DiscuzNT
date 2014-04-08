using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 个人空间友情链接列表
    /// </summary>
    public class usercpspacelinklist : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 友情链接总数
        /// </summary>
        public int linkcount;
        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 友情链接列表
        /// </summary>
        public DataTable linklist = new DataTable();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 分页大小
        /// </summary>
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

                if (DNTRequest.GetString("linkid") != "")
                {
                    Space.Data.DbProvider.GetInstance().DeleteSpaceLink(DNTRequest.GetString("linkid"), userid);
                    SetUrl("usercpspacelinklist.aspx");
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
            linkcount = Space.Data.DbProvider.GetInstance().GetSpaceLinkCount(userid);
            //获取总页数
            pagecount = (linkcount % pagesize == 0) ? (linkcount / pagesize) : (linkcount / pagesize + 1);
            if (pagecount == 0)
                pagecount = 1;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            if (pageid > pagecount)
                pageid = pagecount;

            //获取收入记录并分页显示
            linklist = Space.Data.DbProvider.GetInstance().GetSpaceLinkList(pagesize, pageid, userid);
            linklist.Columns.Add("shortlinktitle");
            linklist.Columns.Add("shortlinkurl");
            foreach (DataRow dr in linklist.Rows)
            {
                dr["shortlinktitle"] = dr["linktitle"];
                if (dr["shortlinktitle"].ToString().Length > 30)
                    dr["shortlinktitle"] = dr["shortlinktitle"].ToString().Substring(0, 30) + "...";
                dr["shortlinkurl"] = dr["linkurl"];
                if (dr["shortlinkurl"].ToString().Length > 30)
                    dr["shortlinkurl"] = dr["shortlinkurl"].ToString().Substring(0, 30) + "...";
                if (dr["description"].ToString().Length > 30)
                    dr["description"] = dr["description"].ToString().Substring(0, 30) + "...";
            }
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpspacelinklist.aspx", 8);
        }
    }
}
