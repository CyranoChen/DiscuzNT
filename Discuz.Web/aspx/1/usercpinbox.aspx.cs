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
    /// 用户收件箱页面
    /// </summary>
    public class usercpinbox : UserCpPage
    {
        protected override void ShowPage()
        {
            pagetitle = "短消息收件箱";

            if (!IsLogin()) return;

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                if (Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("pmitemid")))
                {
                    AddErrLine("您未选中任何短消息，当前操作失败！");
                    return;
                }
                if (!Utils.IsNumericList(DNTRequest.GetFormString("pmitemid")))
                {
                    AddErrLine("参数信息错误！");
                    return;
                }

                string[] pmitemid = Utils.SplitString(DNTRequest.GetFormString("pmitemid"), ",");
             
                if (!Utils.IsNumericArray(pmitemid) || PrivateMessages.DeletePrivateMessage(userid, pmitemid) == -1)
                {
                    AddErrLine("参数无效");
                    return;
                }
                Users.UpdateUserNewPMCount(userid, olid);


                SetUrl("usercpinbox.aspx");
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("删除完毕");
            }
            else
                BindPrivateMessage(0);

            newnoticecount = Notices.GetNewNoticeCountByUid(userid);
        }
    }
}