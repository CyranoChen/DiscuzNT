using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Data
{
    /// <summary>
    /// 版主数据操作类
    /// </summary>
    public class Moderators
    {
        /// <summary>
        /// 获得所有版主信息
        /// </summary>
        /// <returns>所有版主信息</returns>
        public static List<ModeratorInfo> GetModeratorList()
        {
            List<ModeratorInfo> morderatorList = new List<ModeratorInfo>();
            
            foreach (DataRow dr in DatabaseProvider.GetInstance().GetModeratorList().Rows)
            {
                ModeratorInfo info = new ModeratorInfo();
                info.Uid = TypeConverter.ObjectToInt(dr["uid"]);
                info.Fid = TypeConverter.ObjectToInt(dr["fid"]);
                info.Displayorder = TypeConverter.ObjectToInt(dr["Displayorder"]);
                info.Inherited = TypeConverter.ObjectToInt(dr["inherited"]);
                morderatorList.Add(info);
            }
            return morderatorList;
        }

        /// <summary>
        /// 获得版主信息
        /// </summary>
        /// <param name="moderatorId"></param>
        /// <returns></returns>
        public static DataTable GetModeratorInfo(string moderatorId)
        {
            return DatabaseProvider.GetInstance().GetModeratorInfo(moderatorId);
        }

        /// <summary>
        /// 按版块删除版主
        /// </summary>
        /// <param name="fid">版块Id</param>
        public static void DeleteModeratorByFid(int fid)
        {
            DatabaseProvider.GetInstance().DeleteModeratorByFid(fid);
        }

        public static void DeleteModerator(int uid)
        {
            DatabaseProvider.GetInstance().DeleteModerator(uid);
        }


        /// <summary>
        /// 添加版主
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="fid">板块ID</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="inherited"></param>
        public static void AddModerator(int uid, int fid, int displayorder, int inherited)
        {
            DatabaseProvider.GetInstance().AddModerator(uid,fid, displayorder, inherited);
        }

        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <param name="currentfid">当前板块</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static DataTable GetUidInModeratorsByUid(int currentfid, int uid)
        {
            return DatabaseProvider.GetInstance().GetUidInModeratorsByUid(currentfid, uid);
        }

        /// <summary>
        /// 插入版主信息
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="moderators">版主</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="inherited"></param>
        public static void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited)
        {
            DatabaseProvider.GetInstance().InsertForumsModerators(fid, moderators, displayorder, inherited);
        }

        /// <summary>
        /// 获取版主Uid
        /// </summary>
        /// <param name="fidlist">板块ID列表</param>
        /// <returns></returns>
        public static DataTable GetUidModeratorByFid(string fidlist)
        {
            return DatabaseProvider.GetInstance().GetUidModeratorByFid(fidlist);
        }
    }
}
