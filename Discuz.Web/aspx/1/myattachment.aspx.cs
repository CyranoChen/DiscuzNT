using System;
using System.Data;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    public partial class myattachment : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帖子所属的主题列表
        /// </summary>
        public List<MyAttachmentInfo> myattachmentlist;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 总页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 附件总数
        /// </summary>
        public int attachmentcount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 当前登录的用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 文件类型
        /// </summary>
        public int typeid = DNTRequest.GetInt("typeid", 0);
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
            attachmentcount = typeid > 0 ? Attachments.GetUserAttachmentCount(userid,typeid) : Attachments.GetUserAttachmentCount(userid);

            pagecount = attachmentcount % 16 == 0 ? attachmentcount / 16 : attachmentcount / 16 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;

            //修正请求页数中可能的错误
            pageid = pageid < 1 ? 1 : pageid;
            pageid = pageid > pagecount ? pagecount :pageid;

            myattachmentlist = Attachments.GetAttachmentByUid(userid, typeid, pageid, 16);
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount,typeid > 0 ? "myattachment.aspx?typeid=" + typeid : "myattachment.aspx", 10);
        }
    }
}