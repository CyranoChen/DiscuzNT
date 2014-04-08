using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
	/// <summary>
	/// 版主操作类
	/// </summary>
	public class Moderators
	{
		/// <summary>
		/// 获得所有版主信息
		/// </summary>
		/// <returns>所有版主信息</returns>
		public static List<ModeratorInfo> GetModeratorList()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            List<ModeratorInfo> morderatorList = cache.RetrieveObject("/Forum/ModeratorList") as List<ModeratorInfo>;
            if (morderatorList == null)
			{
                morderatorList = Discuz.Data.Moderators.GetModeratorList();
                cache.AddObject("/Forum/ModeratorList", morderatorList);
			}
            return morderatorList;
		}

		/// <summary>
		/// 判断指定用户是否是指定版块的版主
		/// </summary>
        /// <param name="adminId">用户级别(1为管理员，2为超版，3为版主，0为普通用户)</param>
		/// <param name="uid">用户id</param>
		/// <param name="fid">论坛id</param>
		/// <returns>如果是版主返回true, 如果不是则返回false</returns>
		public static bool IsModer(int adminId, int uid, int fid)
		{
			if (adminId == 0)
				return false;

            // 用户为管理员或总版主直接返回真
			if (adminId == 1 || adminId == 2)
				return true;

            if (adminId == 3)
			{
				// 如果是管理员或总版主, 或者是普通版主且在该版块有版主权限
				foreach(ModeratorInfo moderInfo in GetModeratorList())
				{
					// 论坛版主表中存在,则返回真
                    if (moderInfo.Uid == uid && moderInfo.Fid == fid)
						return true;
				}
			}
			return false;
		}

        /// <summary>
        /// 通过版主用户名获取其管理的版块列表
        /// </summary>
        /// <param name="moderatorUserName"></param>
        /// <returns></returns>
        public static string GetFidListByModerator(string moderatorUserName)
        {
            string fidList = "";
            foreach (ForumInfo forumInfo in Forums.GetForumList())
            {
                if (("," + forumInfo.Moderators + ",").Contains("," + moderatorUserName + ","))
                    fidList += forumInfo.Fid + ",";
            }
            return fidList.TrimEnd(',');
        }
	}
}
