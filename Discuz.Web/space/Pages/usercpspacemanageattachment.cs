using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 个人空间管理附件
    /// </summary>
    public class usercpspacemanageattachment : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 附件总数
        /// </summary>
        public int attachmentcount;
        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;

        private int pagesize = 15;
        /// <summary>
        /// 附件列表
        /// </summary>
        public DataTable attachmentlist = new DataTable();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
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
                if (DNTRequest.GetString("aid") != "")
                {
                    Space.Data.DbProvider.GetInstance().DeleteSpaceAttachmentByIDList(DNTRequest.GetString("aid"), userid);
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
            attachmentcount = Space.Data.DbProvider.GetInstance().GetSpaceAttachmentCount(userid);
            //获取总页数
            pagecount = (attachmentcount%pagesize == 0) ? (attachmentcount/pagesize) : (attachmentcount/pagesize + 1);
            if (pagecount == 0)
                pagecount = 1;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            if (pageid > pagecount)
                pageid = pagecount;

            attachmentlist = Space.Data.DbProvider.GetInstance().GetSpaceAttachmentList(pagesize, pageid, userid);
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpspacemanageattachment.aspx", 8);
            attachmentlist.Columns.Add(new DataColumn("isimg", Type.GetType("System.Boolean")));

            foreach (DataRow dr in attachmentlist.Rows)
            {
                string filename = dr["filename"].ToString().Trim().ToLower();
                int at = filename.LastIndexOf(".");
                string extname = filename.Substring(at, filename.Length - at);
                if (extname == ".jpg" || extname == ".gif" || extname == ".png")
                    dr["isimg"] = true;
            }
        }
    }
}