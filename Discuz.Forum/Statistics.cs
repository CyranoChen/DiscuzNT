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
	/// 论坛统计类
	/// </summary>
	public class Statistics
	{
		/// <summary>
		/// 获得统计列
		/// </summary>
		/// <returns>统计列</returns>
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
		/// 获取指定版块中的主题帖子统计数据
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
		/// 获得指定名称的统计项
		/// </summary>
		/// <param name="param">项</param>
		/// <returns>统计值</returns>
		public static string GetStatisticsRowItem(string param)
		{
			return GetStatisticsRow()[param].ToString();
		}


		/// <summary>
		/// 得到上一次执行搜索操作的时间
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
		/// 得到用户在一分钟内搜索的次数。
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
		/// 重新设置用户上一次执行搜索操作的时间
		/// </summary>
		/// <param name="searchtime">操作时间</param>
		public static void SetStatisticsSearchtime(string searchtime)
		{
			DNTCache cache = DNTCache.GetCacheService();
			cache.RemoveObject("/Forum/StatisticsSearchtime");
			cache.AddObject("/Forum/StatisticsSearchtime", searchtime);
		}

		/// <summary>
		/// 设置用户在一分钟内搜索的次数为初始值。
		/// </summary>
		/// <param name="searchcount">初始值</param>
		public static void SetStatisticsSearchcount(int searchcount)
		{
			DNTCache cache = DNTCache.GetCacheService();
			cache.RemoveObject("/Forum/StatisticsSearchcount");
			cache.AddObject("/Forum/StatisticsSearchcount", searchcount);
		}

	
		/// <summary>
		/// 更新指定名称的统计项
		/// </summary>
		/// <param name="param">项目名称</param>
		/// <param name="Value">指定项的值</param>
		/// <returns>更新数</returns>
		public static int UpdateStatistics(string param,string strValue)
		{
            return Discuz.Data.Statistics.UpdateStatistics(param, strValue);
		}

		/// <summary>
		/// 检查并更新60秒内统计的数量
		/// </summary>
		/// <param name="maxspm">60秒内允许的最大搜索次数</param>
		/// <returns>没有超过最大搜索次数返回true,否则返回false</returns>
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
		/// 重建统计缓存
		/// </summary>
		public static void ReSetStatisticsCache()
		{
			DNTCache cache = DNTCache.GetCacheService();
			cache.RemoveObject("/Forum/Statistics");
            cache.AddObject("/Forum/Statistics", Discuz.Data.Statistics.GetStatisticsRow());
		}
	}
}
