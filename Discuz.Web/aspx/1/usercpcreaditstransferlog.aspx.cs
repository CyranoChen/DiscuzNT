using System;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 兑换与转账记录
    /// </summary>
    public class usercpcreaditstransferlog : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 积分日志总数
        /// </summary>
        public int creditslogcount;
        /// <summary>
        /// 积分日志列表
        /// </summary>
        public DataTable creditsloglist = new DataTable();
        /// <summary>
        /// 是否显示充值成功
        /// </summary>
        public bool isshowmsg = !string.IsNullOrEmpty(DNTRequest.GetString("paysuccess"));
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return;

            if (isshowmsg)
            {
                SetUrl("usercpcreaditstransferlog.aspx");
                SetMetaRefresh(5);
                SetShowBackLink(false);
                AddMsgLine("积分充值操作完成，充值成功后会发送站内通知告知");
                return;
            }

            //获取主题总数
            creditslogcount = CreditsLogs.GetCreditsLogRecordCount(userid);
            //获取收入记录并分页显示
            BindItems(creditslogcount);
            creditsloglist = CreditsLogs.GetCreditsLogList(16, pageid, userid);
        }
    }
}