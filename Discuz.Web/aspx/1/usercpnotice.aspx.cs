using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 用户通知
    /// </summary>
    public class usercpnotice : UserCpPage
    {
        /// <summary>
        /// 通知列表
        /// </summary>
        public NoticeinfoCollection noticeinfolist = new NoticeinfoCollection();
        /// <summary>
        /// 消息过滤参数
        /// </summary>
        public string filter = DNTRequest.GetString("filter", true).ToLower();
        /// <summary>
        /// 记录总数
        /// </summary>
        public int reccount = 0;
    
        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return; 
            
            if (filter == "spacecomment" && config.Enablespace == 0)
            {
                AddErrLine("系统未开启" + config.Spacename + "服务, 当前页面暂时无法访问!");
                return;
            }
            if (filter == "albumcomment" && config.Enablealbum == 0)
            {
                AddErrLine("系统未开启" + config.Albumname + "服务, 当前页面暂时无法访问!");
                return;
            }
            if ((filter == "goodstrade" || filter == "goodsleaveword") && config.Enablemall == 0)
            {
                AddErrLine("系统未开启交易服务, 当前页面暂时无法访问!");
                return;
            }

            NoticeType noticetype = Notices.GetNoticetype(filter);            
            reccount = Notices.GetNoticeCountByUid(userid, noticetype);
            
            BindItems(reccount, "usercpnotice.aspx?filter=" + filter);
            noticeinfolist = Notices.GetNoticeinfoCollectionByUid(userid, noticetype, pageid, 16);
            newnoticecount = Notices.GetNewNoticeCountByUid(userid);
            Notices.UpdateNoticeNewByUid(userid, 0);
            OnlineUsers.UpdateNewNotices(olid);
        }
    }
}
