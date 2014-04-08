using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;

namespace Discuz.Forum
{
	/// <summary>
	/// ��̳ͳ����
	/// </summary>
	public class Statistics
	{
		/// <summary>
		/// ���ͳ����
		/// </summary>
		/// <returns>ͳ����</returns>
		public static DataRow GetStatisticsRow()
		{
			DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/Statistics") as DataTable;
            if (dt == null)
			{
                dt = Discuz.Data.Statistics.GetStatisticsRow();
				cache.AddObject("/Forum/Statistics", dt);
			}
            return dt.Rows[0];
		}

	
		/// <summary>
		/// ��ȡָ������е���������ͳ������
		/// </summary>
		/// <param name="fid"></param>
		/// <param name="topiccount"></param>
		/// <param name="postcount"></param>
		/// <param name="todaypostcount"></param>
		public static void GetPostCountFromForum(int fid, out int topiccount,out int postcount, out int todaypostcount)
		{
            if (fid == 0)
                Discuz.Data.Statistics.GetAllForumStatistics(out topiccount, out postcount, out todaypostcount);
            else
                Discuz.Data.Statistics.GetForumStatistics(fid, out topiccount, out postcount, out todaypostcount);
		}



		/// <summary>
		/// ���ָ�����Ƶ�ͳ����
		/// </summary>
		/// <param name="param">��</param>
		/// <returns>ͳ��ֵ</returns>
		public static string GetStatisticsRowItem(string param)
		{
			return GetStatisticsRow()[param].ToString();
		}


		/// <summary>
		/// �õ���һ��ִ������������ʱ��
		/// </summary>
		/// <returns></returns>
		public static string GetStatisticsSearchtime()
		{
			DNTCache cache = DNTCache.GetCacheService();
			string searchtime = cache.RetrieveObject("/Forum/StatisticsSearchtime") as string;
			if (searchtime == null)
			{
				searchtime = DateTime.Now.ToString();
				cache.AddObject("/Forum/StatisticsSearchtime", searchtime);
			}
			return searchtime;
		}

		/// <summary>
		/// �õ��û���һ�����������Ĵ�����
		/// </summary>
		/// <returns></returns>
		public static int GetStatisticsSearchcount()
		{
			DNTCache cache = DNTCache.GetCacheService();
			int searchcount = Utils.StrToInt(cache.RetrieveObject("/Forum/StatisticsSearchcount"),0);
			if (searchcount == 0)
			{
				searchcount = 1;
				cache.AddObject("/Forum/StatisticsSearchcount", searchcount);
			}
            return searchcount;
		}


		/// <summary>
		/// ���������û���һ��ִ������������ʱ��
		/// </summary>
		/// <param name="searchtime">����ʱ��</param>
		public static void SetStatisticsSearchtime(string searchtime)
		{
			DNTCache cache = DNTCache.GetCacheService();
			cache.RemoveObject("/Forum/StatisticsSearchtime");
			cache.AddObject("/Forum/StatisticsSearchtime", searchtime);
		}

		/// <summary>
		/// �����û���һ�����������Ĵ���Ϊ��ʼֵ��
		/// </summary>
		/// <param name="searchcount">��ʼֵ</param>
		public static void SetStatisticsSearchcount(int searchcount)
		{
			DNTCache cache = DNTCache.GetCacheService();
			cache.RemoveObject("/Forum/StatisticsSearchcount");
			cache.AddObject("/Forum/StatisticsSearchcount", searchcount);
		}

	
		/// <summary>
		/// ����ָ�����Ƶ�ͳ����
		/// </summary>
		/// <param name="param">��Ŀ����</param>
		/// <param name="Value">ָ�����ֵ</param>
		/// <returns>������</returns>
		public static int UpdateStatistics(string param,string strValue)
		{
            return Discuz.Data.Statistics.UpdateStatistics(param, strValue);
		}

		/// <summary>
		/// ��鲢����60����ͳ�Ƶ�����
		/// </summary>
		/// <param name="maxspm">60��������������������</param>
		/// <returns>û�г������������������true,���򷵻�false</returns>
		public static bool CheckSearchCount(int maxspm)
		{
			if (maxspm == 0)
				return true;

			int searchcount = GetStatisticsSearchcount();
            if (Utils.StrDateDiffSeconds(GetStatisticsSearchtime(), 60) > 0)
			{
				SetStatisticsSearchtime(DateTime.Now.ToString());
				SetStatisticsSearchcount(1);
			}
			
			if (searchcount > maxspm)
				return false;

			SetStatisticsSearchcount(searchcount + 1);
			return true;			
		}

		/// <summary>
		/// �ؽ�ͳ�ƻ���
		/// </summary>
		public static void ReSetStatisticsCache()
		{
			DNTCache cache = DNTCache.GetCacheService();
			cache.RemoveObject("/Forum/Statistics");
            cache.AddObject("/Forum/Statistics", Discuz.Data.Statistics.GetStatisticsRow());
		}
	}
}
