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
	/// ������־������
	/// </summary>
	public class PaymentLogs
	{
		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="uid">�û�id</param>
		/// <param name="tid">����id</param>
		/// <param name="posterid">�������û�id</param>
		/// <param name="price">�۸�</param>
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
        /// ����ָ����չ����
        /// </summary>
        /// <param name="userInfo">�û���Ϣ</param>
        /// <param name="extCreditsId">��չ����ID</param>
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
		/// �ж��û��Ƿ��ѹ�������
		/// </summary>
		/// <param name="tid">����id</param>
		/// <param name="uid">�û�id</param>
		/// <returns></returns>
		public static bool IsBuyer(int tid, int uid)
		{
            return Discuz.Data.PaymentLogs.IsBuyer(tid, uid);
		}

		/// <summary>
		/// ��ȡָ���û��Ľ�����־
		/// </summary>
		/// <param name="pagesize">ÿҳ����</param>
		/// <param name="currentpage">��ǰҳ</param>
		/// <param name="uid">�û�id</param>
		/// <returns></returns>
		public static DataTable GetPayLogInList(int pagesize,int currentpage , int uid)
		{
            return LoadPayLogForumName(Discuz.Data.PaymentLogs.GetPayLogInList(pagesize, currentpage, uid));
		}

		/// <summary>
		/// ��ȡָ���û���������־��¼��
		/// </summary>
		/// <param name="uid">�û�id</param>
		/// <returns></returns>
		public static int GetPaymentLogInRecordCount(int uid)
		{
            return Discuz.Data.PaymentLogs.GetPaymentLogInRecordCount(uid);	
        }

		/// <summary>
		/// ����ָ���û���֧����־��¼��
		/// </summary>
		/// <param name="pagesize">ÿҳ��¼��</param>
		/// <param name="currentpage">��ǰҳ</param>
		/// <param name="uid">�û�id</param>
		/// <returns></returns>
		public static DataTable GetPayLogOutList(int pagesize,int currentpage , int uid)
		{
			return LoadPayLogForumName(DatabaseProvider.GetInstance().GetPayLogOutList(pagesize, currentpage, uid));
		}

        /// <summary>
        /// ���ؽ�����־�б�������
        /// </summary>
        /// <param name="dt">������־�б�</param>
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
		/// ����ָ���û�֧����־����
		/// </summary>
		/// <param name="uid">�û�id</param>
		/// <returns></returns>
		public static int GetPaymentLogOutRecordCount(int uid)
		{
            return uid > 0 ? Discuz.Data.PaymentLogs.GetPaymentLogOutRecordCount(uid) : 0;	
        }

		/// <summary>
		/// ��ȡָ������Ĺ����¼
		/// </summary>
		/// <param name="pagesize">ÿҳ��¼��</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <param name="tid">����id</param>
		/// <returns></returns>
		public static DataTable GetPaymentLogByTid(int pagesize,int currentpage , int tid)
		{
            return (tid > 0 && currentpage > 0) ? Discuz.Data.PaymentLogs.GetPaymentLogByTid(pagesize, currentpage, tid) : new DataTable();
		}

		/// <summary>
		/// ���⹺���ܴ���
		/// </summary>
		/// <param name="tid">����id</param>
		/// <returns></returns>
		public static int GetPaymentLogByTidCount(int tid)
		{
            return tid > 0 ? Discuz.Data.PaymentLogs.GetPaymentLogByTidCount(tid) : 0;	
        }
	}
}
