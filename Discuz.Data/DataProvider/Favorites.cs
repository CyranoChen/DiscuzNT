using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class Favorites
    {
        /// <summary>
        /// 创建收藏信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="type">收藏类型</param>
        /// <returns>创建成功返回 1 否则返回 0</returns>	
        public static int CreateFavorites(int uid, int tid, byte type)
        {
            return DatabaseProvider.GetInstance().CreateFavorites(uid, tid, type);
        }


        /// <summary>
        /// 删除指定用户的收藏信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fidlist">要删除的收藏信息id列表</param>
        /// <param name="type">收藏类型</param>
        /// <returns>删除的条数．出错时返回 -1</returns>
        public static int DeleteFavorites(int uid, string fidlist, byte type)
        {
            return DatabaseProvider.GetInstance().DeleteFavorites(uid, fidlist, type);
        }


        /// <summary>
        /// 得到用户收藏信息列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pagesize">分页时每页的记录数</param>
        /// <param name="pageindex">当前页码</param>
        /// <param name="type">收藏类型id</param>
        /// <returns>用户信息列表</returns>
        public static DataTable GetFavoritesList(int uid, int pagesize, int pageindex, int type)
        {
            return DatabaseProvider.GetInstance().GetFavoritesList(uid, pagesize, pageindex, type);
        }

        /// <summary>
        /// 得到用户单个类型收藏的总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>收藏总数</returns>
        public static int GetFavoritesCount(int uid, int type)
        {
            return DatabaseProvider.GetInstance().GetFavoritesCount(uid, type);
        }

        /// <summary>
        /// 收藏夹里是否包含了指定的项
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="tid">项Id</param>
        /// <param name="type">类型: 相册, 日志, 主题</param>
        /// <returns></returns>
        public static int CheckFavoritesIsIN(int uid, int tid, byte type)
        {
            return DatabaseProvider.GetInstance().CheckFavoritesIsIN(uid, tid, type);
        }

        /// <summary>
        /// 更新用户收藏条目的查看时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public static int UpdateUserFavoriteViewTime(int uid, int tid)
        {
            return DatabaseProvider.GetInstance().UpdateUserFavoriteViewTime(uid, tid);
        }
    }
}
