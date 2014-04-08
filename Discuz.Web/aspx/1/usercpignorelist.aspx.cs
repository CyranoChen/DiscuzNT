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
    public class usercpignorelist : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 当忽略全部时
        /// </summary>
        public string ignoreexample = "{ALL}";
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "黑名单";

            if (!IsLogin()) return; 

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                //当修改忽略列表信息
                if (DNTRequest.GetFormString("Ignorepm") != user.Ignorepm)
                {
                    user.Ignorepm = Utils.CutString(DNTRequest.GetFormString("Ignorepm"), 0, 999);
                    Users.UpdateUserPMSetting(user);
                }

                SetUrl("usercpignorelist.aspx");
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("操作完毕");
            }

            newnoticecount = Notices.GetNewNoticeCountByUid(userid);
        }
    }
}
