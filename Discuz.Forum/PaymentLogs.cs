using System;
using System.Data;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
	/// <summary>
	/// 交易日志操作类
	/// </summary>
	public class PaymentLogs
	{
		/// <summary>
		/// 购买主题
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="tid">主题id</param>
		/// <param name="posterid">发帖者用户id</param>
		/// <param name="price">价格</param>
		/// <param name="netamount"></param>
		/// <returns></returns>
		public static int BuyTopic(int uid, int tid, int posterid, int price, float netamount)
		{
			int tmpprice = price;
            if (price > Scoresets.GetMaxIncPerTopic())
                tmpprice = Scoresets.GetMaxIncPerTopic();

            ShortUserInfo userInfo = Discuz.Data.Users.GetShortUserInfo(uid);
            if (userInfo == null)
				return -2;

            if (GetUserExtCredits(userInfo,Scoresets.GetTopicAttachCreditsTrans()) < price)
				return -1;

            Discuz.Data.Users.BuyTopic(uid, tid, posterid, price, netamount, Scoresets.GetTopicAttachCreditsTrans());
            UserCredits.UpdateUserCredits(uid);
			UserCredits.UpdateUserCredits(posterid);
            return Discuz.Data.PaymentLogs.CreatePaymentLog(uid, tid, posterid, price, netamount);			
		}

        /// <summary>
        /// 返回指定扩展积分
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="extCreditsId">扩展积分ID</param>
        /// <returns></returns>
        private static float GetUserExtCredits(ShortUserInfo userInfo, int extCreditsId)
        {
            switch (extCreditsId)
            {
                case 1: return userInfo.Extcredits1;
                case 2: return userInfo.Extcredits2;
                case 3: return userInfo.Extcredits3;
                case 4: return userInfo.Extcredits4;
                case 5: return userInfo.Extcredits5;
                case 6: return userInfo.Extcredits6;
                case 7: return userInfo.Extcredits7;
                case 8: return userInfo.Extcredits8;
                default: return 0;
            }
        }


		/// <summary>
		/// 判断用户是否已购买主题
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static bool IsBuyer(int tid, int uid)
		{
            return Discuz.Data.PaymentLogs.IsBuyer(tid, uid);
		}

		/// <summary>
		/// 获取指定用户的交易日志
		/// </summary>
		/// <param name="pagesize">每页条数</param>
		/// <param name="currentpage">当前页</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static DataTable GetPayLogInList(int pagesize,int currentpage , int uid)
		{
            return LoadPayLogForumName(Discuz.Data.PaymentLogs.GetPayLogInList(pagesize, currentpage, uid));
		}

		/// <summary>
		/// 获取指定用户的收入日志记录数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static int GetPaymentLogInRecordCount(int uid)
		{
            return Discuz.Data.PaymentLogs.GetPaymentLogInRecordCount(uid);	
        }

		/// <summary>
		/// 返回指定用户的支出日志记录数
		/// </summary>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static DataTable GetPayLogOutList(int pagesize,int currentpage , int uid)
		{
			return LoadPayLogForumName(DatabaseProvider.GetInstance().GetPayLogOutList(pagesize, currentpage, uid));
		}

        /// <summary>
        /// 加载交易日志列表板块名称
        /// </summary>
        /// <param name="dt">交易日志列表</param>
        /// <returns></returns>
        private static DataTable LoadPayLogForumName(DataTable dt)
        {
            if (dt != null)
            {
                DataColumn dc = new DataColumn("forumname", System.Type.GetType("System.String"));
                dc.DefaultValue = "";
                dc.AllowDBNull = false;
                dt.Columns.Add(dc);
                List<ForumInfo> forumList = Discuz.Data.Forums.GetForumList();

                foreach (DataRow dr in dt.Rows)
                {
                    if (!Utils.StrIsNullOrEmpty(dr["fid"].ToString().Trim()))
                    {
                        Predicate<ForumInfo> match = new Predicate<ForumInfo>(delegate(ForumInfo forumInfo) { return forumInfo.Parentid == TypeConverter.ObjectToInt(dr["fid"]); });

                        foreach (ForumInfo forumInfo in forumList.FindAll(match))
                        {
                            dr["forumname"] = forumInfo.Name;
                            break;
                        }
                    }
                }
            }
            return dt;
        }

		/// <summary>
		/// 返回指定用户支出日志总数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static int GetPaymentLogOutRecordCount(int uid)
		{
            return uid > 0 ? Discuz.Data.PaymentLogs.GetPaymentLogOutRecordCount(uid) : 0;	
        }

		/// <summary>
		/// 获取指定主题的购买记录
		/// </summary>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>
		/// <param name="tid">主题id</param>
		/// <returns></returns>
		public static DataTable GetPaymentLogByTid(int pagesize,int currentpage , int tid)
		{
            return (tid > 0 && currentpage > 0) ? Discuz.Data.PaymentLogs.GetPaymentLogByTid(pagesize, currentpage, tid) : new DataTable();
		}

		/// <summary>
		/// 主题购买总次数
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <returns></returns>
		public static int GetPaymentLogByTidCount(int tid)
		{
            return tid > 0 ? Discuz.Data.PaymentLogs.GetPaymentLogByTidCount(tid) : 0;	
        }
	}
}
