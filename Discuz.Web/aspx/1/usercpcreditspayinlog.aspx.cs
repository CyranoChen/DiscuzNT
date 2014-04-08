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
    /// 收入记录
    /// </summary>
    public class usercpcreditspayinlog : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 积分收入日志列表
        /// </summary>
        public DataTable payloglist = new DataTable();
        /// <summary>
        /// 积分收入日志数
        /// </summary>
        public int payinlogcount;
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return; 
            
            //获取积分收入日志数
            payinlogcount = PaymentLogs.GetPaymentLogInRecordCount(userid);
            BindItems(payinlogcount);
            payloglist = PaymentLogs.GetPayLogInList(16, pageid, userid);
        }
    }
}
