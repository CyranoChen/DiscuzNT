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
    /// 支出记录
    /// </summary>
    public class usercpcreditspayoutlog : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 积分支出记录列表
        /// </summary>
        public DataTable payloglist = new DataTable();
        /// <summary>
        /// 积分支出记录数
        /// </summary>
        public int payoutlogcount;
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return; 
            
            //获取积分支出记录数
            payoutlogcount = PaymentLogs.GetPaymentLogOutRecordCount(userid);
            BindItems(payoutlogcount);
            payloglist = PaymentLogs.GetPayLogOutList(16, pageid, userid);
        }
    }
}
