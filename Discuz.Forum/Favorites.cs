using System;
using System.Data;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 收藏夹操作类
	/// </summary>
	public class Favorites
	{
		/// <summary>
		/// 创建收藏信息
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="tid">主题ID</param>
		/// <returns>创建成功返回 1 否则返回 0</returns>	
		public static int CreateFavorites(int uid,int tid)
		{
            if (uid < 0)
                return 0;

            return CreateFavorites(uid, tid, FavoriteType.ForumTopic);
		}

        /// <summary>
        /// 创建收藏信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="type">收藏类型</param>
        /// <returns>创建成功返回 1 否则返回 0</returns>	
        public static int CreateFavorites(int uid, int tid, FavoriteType type)
        {
            return Discuz.Data.Favorites.CreateFavorites(uid, tid, (byte)type);
        }
	
		/// <summary>
		/// 删除指定用户的收藏信息
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="fitemid">要删除的收藏信息id列表</param>
        /// <param name="type">收藏类型</param>
		/// <returns>删除的条数．出错时返回 -1</returns>
        public static int DeleteFavorites(int uid, string[] fitemid, FavoriteType type)
		{
			foreach (string id in fitemid)
			{
				if (!Utils.IsNumeric(id))
					return -1;
			}

            return Discuz.Data.Favorites.DeleteFavorites(uid, String.Join(",",fitemid), (byte)type);
		}

		       
		/// <summary>
		/// 得到用户收藏信息列表
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="pagesize">分页时每页的记录数</param>
		/// <param name="pageindex">当前页码</param>
		/// <param name="type">收藏类型id</param>
		/// <returns>用户信息列表</returns>
        public static DataTable GetFavoritesList(int uid, int pagesize, int pageindex, FavoriteType type)
        {
            return Discuz.Data.Favorites.GetFavoritesList(uid, pagesize, pageindex, (int)type);
        }


        /// <summary>
        /// 得到用户单个类型收藏的总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>收藏总数</returns>
        public static int GetFavoritesCount(int uid, FavoriteType type)
        {
            return uid > 0 ? Discuz.Data.Favorites.GetFavoritesCount(uid, (int)type) : 0;
        }


		/// <summary>
		/// 收藏夹里是否包含了指定的项
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="tid">项Id</param>
        /// <param name="type">类型: 相册, 日志, 主题</param>
		/// <returns></returns>
		public static int CheckFavoritesIsIN(int uid,int tid, FavoriteType type)
		{
            return Discuz.Data.Favorites.CheckFavoritesIsIN(uid, tid, (byte)type);	
        }

        /// <summary>
        /// 更新用户收藏条目的查看时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public static int UpdateUserFavoriteViewTime(int uid, int tid)
        {
            return Discuz.Data.Favorites.UpdateUserFavoriteViewTime(uid, tid);
        }
	}//class end
}
