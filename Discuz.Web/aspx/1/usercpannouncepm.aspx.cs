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
    /// 公共消息
    /// </summary>
    public class usercpannouncepm : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 短消息列表
        /// </summary>
        public List<PrivateMessageInfo> announcepmlist = new List<PrivateMessageInfo>();
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "公共消息";

            if (!IsLogin()) return; 
            
            BindItems(announcepmcount);
            announcepmlist = PrivateMessages.GetAnnouncePrivateMessageCollection(16, pageid);
            newnoticecount = Notices.GetNewNoticeCountByUid(userid);
        }
    }
}
