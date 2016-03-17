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
    /// 草稿箱页面
    /// </summary>
    public class usercpdraftbox : UserCpPage
    {
        protected override void ShowPage()
        {
            pagetitle = "短消息草稿箱";

            if (!IsLogin()) return; 
            
            if (DNTRequest.IsPost())
            {
                if (PrivateMessages.DeletePrivateMessage(userid, Utils.SplitString(DNTRequest.GetFormString("pmitemid"), ",")) == -1)
                {
                    AddErrLine("参数无效<br />");
                    return;
                }
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("删除完毕");
            }
            else
                BindPrivateMessage(2);

            newnoticecount = Notices.GetNewNoticeCountByUid(userid);
        }
    }
}