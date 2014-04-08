using System;
using System.Data;
using System.Data.Common;

using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminPaymentLogFactory ��ժҪ˵����
	/// ���ֽ�����־���������
	/// </summary>
	public class AdminPaymentLogs
	{
		/// <summary>
		/// ɾ����־
		/// </summary>
		/// <returns></returns>
		public static bool DeleteLog()
		{
            return Discuz.Data.PaymentLogs.DeleteLog();
		}

		/// <summary>
		/// ��ָ������ɾ����־
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
            return  Discuz.Data.PaymentLogs.DeleteLog(condition);
		}

		/// <summary>
		/// �õ���ǰָ��ҳ���Ļ��ֽ�����־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize,int currentpage)
		{
            DataTable dt = Discuz.Data.PaymentLogs.GetPaymentLogList(pagesize, currentpage);
			if (dt!=null)
			{

				DataColumn dc=new DataColumn();
				dc.ColumnName="forumname";
				dc.DataType=System.Type.GetType("System.String");
				dc.DefaultValue="";
				dc.AllowDBNull=false;
				dt.Columns.Add(dc);
                DataTable ForumList = Forums.GetForumListForDataTable();
				foreach(DataRow dr in dt.Rows)
				{
					if(dr["fid"].ToString().Trim()!="")
					{
						foreach(DataRow forumdr in ForumList.Select("fid="+dr["fid"].ToString()))
						{
							dr["forumname"]=forumdr["name"].ToString();
							break;
						}
					}
				}
			}
			return dt;
		}		

		/// <summary>
		/// �õ���ǰָ��������ҳ���Ļ��ֽ�����־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize,int currentpage , string condition)
		{
            DataTable dt = Discuz.Data.PaymentLogs.GetPaymentLogList(pagesize, currentpage, condition);
			if (dt!=null)
			{
				DataColumn dc=new DataColumn();
				dc.ColumnName="forumname";
				dc.DataType=System.Type.GetType("System.String");
				dc.DefaultValue="";
				dc.AllowDBNull=false;
				dt.Columns.Add(dc);
                DataTable ForumList = Forums.GetForumListForDataTable();
				foreach(DataRow dr in dt.Rows)
				{
					if(dr["fid"].ToString().Trim()!="")
					{
						foreach(DataRow forumdr in ForumList.Select("fid="+dr["fid"].ToString()))
						{
							dr["forumname"]=forumdr["name"].ToString();
							break;
						}
					}
				}
			}
			return dt;
		}


		/// <summary>
		/// �õ�ָ����ѯ�����µĻ��ֽ�����־��
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return Discuz.Common.Utils.StrIsNullOrEmpty(condition) ? Discuz.Data.PaymentLogs.RecordCount() : Discuz.Data.PaymentLogs.RecordCount(condition);
		}

        /// <summary>
        /// ��ȡ������־��������
        /// </summary>
        /// <param name="postDateTimeStart">��ʼ����</param>
        /// <param name="postDateTimeEnd">��������</param>
        /// <param name="userName">�û���</param>
        /// <returns></returns>
        public static string GetSearchPaymentLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName)
        {
            return Data.PaymentLogs.GetSearchPaymentLogCondition(postDateTimeStart, postDateTimeEnd, userName);
        }
	}
}
