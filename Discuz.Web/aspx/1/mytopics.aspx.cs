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
    /// <summary>
    /// 我的主题
    /// </summary>
    public class mytopics : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 主题列表
        /// </summary>
        public List<TopicInfo> topics;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 总页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 主题总数
        /// </summary>
        public int topiccount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 当前登录的用户信息
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
            //得到当前用户请求的页数
            //获取主题总数
            topiccount = Topics.GetTopicsCountbyUserId(userid, false);
            //获取总页数
            pagecount = topiccount % 16 == 0 ? topiccount / 16 : topiccount / 16 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;

            //修正请求页数中可能的错误
            pageid = pageid < 1 ? 1 : pageid;
            pageid = pageid > pagecount ?  pagecount : pageid;

            topics = Topics.GetTopicsByUserId(userid, pageid, 16, 600, config.Hottopic);
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "mytopics.aspx", 8);
        }
    }
}