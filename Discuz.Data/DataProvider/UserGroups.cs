using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Data
{
    public class UserGroups
    {
        /// <summary>
        /// 获得用户组数据
        /// </summary>
        /// <returns>用户组数据</returns>
        public static List<UserGroupInfo> GetUserGroupList()
        {
            DataTable dt = GetUserGroupForDataTable();
            List<UserGroupInfo> userGruopInfoList = new List<UserGroupInfo>();

            foreach (DataRow dr in dt.Rows)
            {
                UserGroupInfo info = new UserGroupInfo();
                info.Groupid = TypeConverter.StrToInt(dr["groupid"].ToString());
                info.Radminid = TypeConverter.StrToInt(dr["radminid"].ToString());
                info.Type = TypeConverter.StrToInt(dr["type"].ToString());
                info.System = TypeConverter.StrToInt(dr["system"].ToString());
                info.Color = dr["color"].ToString().Trim();
                info.Grouptitle = dr["grouptitle"].ToString().Trim();
                info.Creditshigher = TypeConverter.StrToInt(dr["creditshigher"].ToString());
                info.Creditslower = TypeConverter.StrToInt(dr["creditslower"].ToString());
                info.Stars = TypeConverter.StrToInt(dr["stars"].ToString());
                info.Groupavatar = dr["groupavatar"].ToString();
                info.Readaccess = TypeConverter.StrToInt(dr["readaccess"].ToString());
                info.Allowvisit = TypeConverter.StrToInt(dr["allowvisit"].ToString());
                info.Allowpost = TypeConverter.StrToInt(dr["allowpost"].ToString());
                info.Allowreply = TypeConverter.StrToInt(dr["allowreply"].ToString());
                info.Allowpostpoll = TypeConverter.StrToInt(dr["allowpostpoll"].ToString());
                info.Allowdirectpost = TypeConverter.StrToInt(dr["allowdirectpost"].ToString());
                info.Allowgetattach = TypeConverter.StrToInt(dr["allowgetattach"].ToString());
                info.Allowpostattach = TypeConverter.StrToInt(dr["allowpostattach"].ToString());
                info.Allowvote = TypeConverter.StrToInt(dr["allowvote"].ToString());
                info.Allowmultigroups = TypeConverter.StrToInt(dr["allowmultigroups"].ToString());
                info.Allowsearch = TypeConverter.StrToInt(dr["allowsearch"].ToString());
                info.Allowavatar = TypeConverter.StrToInt(dr["allowavatar"].ToString());
                info.Allowcstatus = TypeConverter.StrToInt(dr["allowcstatus"].ToString());
                info.Allowuseblog = TypeConverter.StrToInt(dr["allowuseblog"].ToString());
                info.Allowinvisible = TypeConverter.StrToInt(dr["allowinvisible"].ToString());
                info.Allowtransfer = TypeConverter.StrToInt(dr["allowtransfer"].ToString());
                info.Allowsetreadperm = TypeConverter.StrToInt(dr["allowsetreadperm"].ToString());
                info.Allowsetattachperm = TypeConverter.StrToInt(dr["allowsetattachperm"].ToString());
                info.Allowhidecode = TypeConverter.StrToInt(dr["allowhidecode"].ToString());
                info.Allowhtmltitle = TypeConverter.StrToInt(dr["allowhtmltitle"].ToString());
                info.Allowhtml = TypeConverter.StrToInt(dr["allowhtml"].ToString());
                info.Allowcusbbcode = TypeConverter.StrToInt(dr["allowcusbbcode"].ToString());
                info.Allownickname = TypeConverter.StrToInt(dr["allownickname"].ToString());
                info.Allowsigbbcode = TypeConverter.StrToInt(dr["allowsigbbcode"].ToString());
                info.Allowsigimgcode = TypeConverter.StrToInt(dr["allowsigimgcode"].ToString());
                info.Allowviewpro = TypeConverter.StrToInt(dr["allowviewpro"].ToString());
                info.Allowviewstats = TypeConverter.StrToInt(dr["allowviewstats"].ToString());
                info.Disableperiodctrl = TypeConverter.StrToInt(dr["disableperiodctrl"].ToString());
                info.Reasonpm = TypeConverter.StrToInt(dr["reasonpm"].ToString());
                info.Maxprice = TypeConverter.StrToInt(dr["maxprice"].ToString());
                info.Maxpmnum = TypeConverter.StrToInt(dr["maxpmnum"].ToString());
                info.Maxsigsize = TypeConverter.StrToInt(dr["maxsigsize"].ToString());
                info.Maxattachsize = TypeConverter.StrToInt(dr["maxattachsize"].ToString());
                info.Maxsizeperday = TypeConverter.StrToInt(dr["maxsizeperday"].ToString());
                info.Attachextensions = dr["attachextensions"].ToString().Trim();
                info.Raterange = dr["raterange"].ToString().Trim();
                info.Allowspace = TypeConverter.StrToInt(dr["allowspace"].ToString());
                info.Maxspaceattachsize = TypeConverter.StrToInt(dr["maxspaceattachsize"].ToString());
                info.Maxspacephotosize = TypeConverter.StrToInt(dr["maxspacephotosize"].ToString());
                info.Allowbonus = TypeConverter.StrToInt(dr["allowbonus"].ToString());
                info.Allowdebate = TypeConverter.StrToInt(dr["allowdebate"].ToString());
                info.Minbonusprice = TypeConverter.StrToInt(dr["minbonusprice"].ToString());
                info.Maxbonusprice = TypeConverter.StrToInt(dr["maxbonusprice"].ToString());
                info.Allowtrade = TypeConverter.StrToInt(dr["allowtrade"].ToString());
                info.Allowdiggs = TypeConverter.StrToInt(dr["allowdiggs"].ToString());
                //info.MaxFriendsCount = TypeConverter.StrToInt(dr["maxfriendscount"].ToString());
                info.ModNewTopics = TypeConverter.StrToInt(dr["modnewtopics"].ToString());
                info.ModNewPosts = TypeConverter.StrToInt(dr["modnewposts"].ToString());
                info.Ignoreseccode = TypeConverter.ObjectToInt(dr["ignoreseccode"]);
                userGruopInfoList.Add(info);
            }
            return userGruopInfoList;
        }

        /// <summary>
        /// 通过指定用户组ID允许的评分范围
        /// </summary>
        /// <param name="groupid">组ID</param>
        /// <returns>评分范围</returns>
        public static DataTable GetUserGroupRateRange(int groupId)
        {
            return DatabaseProvider.GetInstance().GetUserGroupRateRange(groupId);
        }


        /// <summary>
        /// 通过组ID和UID得到允许的评分范围,如果无设置则返回空表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="gid">用户组别</param>
        /// <returns>ID和UID允许的评分范围</returns>
        public static int[] GroupParticipateScore(int uid)
        {
            int[] extcredits = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            IDataReader reader = DatabaseProvider.GetInstance().GetUserTodayRate(uid);
            while (reader.Read())
            {
                extcredits[TypeConverter.ObjectToInt(reader["extcredits"])] = TypeConverter.ObjectToInt(reader["todayrate"]);
            }
            reader.Close();
            return extcredits;
        }

        /// <summary>
        /// 更新用户组积分设置信息
        /// </summary>
        /// <param name="raterange">积分设置信息</param>
        /// <param name="groupid">用户组Id</param>
        public static void UpdateUserGroupRaterange(string raterange, int groupid)
        {
            DatabaseProvider.GetInstance().UpdateRaterangeByGroupid(raterange, groupid);
        }

        /// <summary>
        /// 更新用户组信息
        /// </summary>
        /// <param name="info">用户组信息</param>
        public static void UpdateUserGroup(UserGroupInfo info)
        {
            DatabaseProvider.GetInstance().UpdateUserGroup(info);
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupid">用户组Id</param>
        public static void DeleteUserGroupInfo(int groupid)
        {
            DatabaseProvider.GetInstance().DeleteUserGroupInfo(groupid);
        }

        /// <summary>
        /// 更新全部用户的组Id
        /// </summary>
        /// <param name="sourceGroupId">源组Id</param>
        /// <param name="targetGroupId">目标组Id</param>
        public static void ChangeAllUserGroupId(int sourceGroupId, int targetGroupId)
        {
            DatabaseProvider.GetInstance().ChangeUsergroup(sourceGroupId, targetGroupId);
        }

        /// <summary>
        /// 获取除指定id的用户组外的普通用户组
        /// </summary>
        /// <param name="groupid">用户组</param>
        /// <returns></returns>
        public static DataTable GetUserGroupExceptGroupid(int groupid)
        {
            return DatabaseProvider.GetInstance().GetUserGroupExceptGroupid(groupid);
        }

        /// <summary>
        /// 获取管理组
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminGroups()
        {
            return DatabaseProvider.GetInstance().GetAdminGroups();
        }

        /// <summary>
        /// 获取在线列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOnlineList()
        {
            return DatabaseProvider.GetInstance().GetOnlineList();
        }

        /// <summary>
        /// 更新在线表
        /// </summary>
        /// <param name="groupid">用户组ID</param>
        /// <param name="displayOrder">序号</param>
        /// <param name="img">图片</param>
        /// <param name="title">名称</param>
        /// <returns></returns>
        public static int UpdateOnlineList(int groupid, int displayorder, string img, string title)
        {
            return DatabaseProvider.GetInstance().UpdateOnlineList(groupid, displayorder, img, title);
        }


        public static void UpdateOnlineList(UserGroupInfo info)
        {
            DatabaseProvider.GetInstance().UpdateOnlineList(info);
        }

        /// <summary>
        /// 获取评分范围
        /// </summary>
        /// <param name="scoreid"></param>
        /// <returns></returns>
        public static DataTable GetRateRange(int scoreid)
        {
            return DatabaseProvider.GetInstance().GetRateRange(scoreid);
        }

        /// <summary>
        /// 更新评分范围
        /// </summary>
        /// <param name="raterange"></param>
        /// <param name="groupid"></param>
        public static void UpdateRateRange(string raterange, int groupid)
        {
            DatabaseProvider.GetInstance().UpdateRateRange(raterange, groupid);
        }

        /// <summary>
        /// 获取除游客组之外的用户组名称
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserGroupWithOutGuestTitle()
        {
            return DatabaseProvider.GetInstance().GetUserGroupWithOutGuestTitle();
        }

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserGroupForDataTable()
        {
            return DatabaseProvider.GetInstance().GetUserGroups();
        }

        /// <summary>
        /// 获取积分用户组
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCreditUserGroup()
        {
            return DatabaseProvider.GetInstance().GetUserGroup();
        }

        /// <summary>
        /// 获取特殊用户组
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSpecialUserGroup()
        {
            return DatabaseProvider.GetInstance().GetSpecialUserGroup();
        }

        /// <summary>
        /// 创建用户组信息
        /// </summary>
        /// <param name="userGroupInfo">用户组信息</param>
        public static void CreateUserGroup(UserGroupInfo userGroupInfo)
        {
            DatabaseProvider.GetInstance().AddUserGroup(userGroupInfo);
        }

        /// <summary>
        /// 获取最大积分下限
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaxCreditLower()
        {
            return DatabaseProvider.GetInstance().GetMaxCreditLower();
        }

        /// <summary>
        /// 是否是系统组
        /// </summary>
        /// <param name="groupid">用户组ID</param>
        /// <returns></returns>
        public static bool IsSystemOrTemplateUserGroup(int groupid)
        {
            return DatabaseProvider.GetInstance().IsSystemOrTemplateUserGroup(groupid);
        }

        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="Creditshigher">积分上限</param>
        /// <param name="Creditslower">积分下限</param>
        /// <returns></returns>
        public static DataTable GetUserGroupByCreditsHigherAndLower(int Creditshigher, int Creditslower)
        {
            return DatabaseProvider.GetInstance().GetUserGroupByCreditsHigherAndLower(Creditshigher, Creditslower);
        }

        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="Creditshigher">积分上限</param>
        /// <returns></returns>
        public static DataTable GetUserGroupByCreditshigher(int Creditshigher)
        {
            return DatabaseProvider.GetInstance().GetUserGroupByCreditshigher(Creditshigher);
        }

        /// <summary>
        /// 更新用户组积分上下限
        /// </summary>
        /// <param name="currentCreditsHigher"></param>
        /// <param name="Creditshigher"></param>
        public static void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int Creditshigher)
        {
            DatabaseProvider.GetInstance().UpdateUserGroupCreidtsLower(currentCreditsHigher, Creditshigher);
        }


        public static void UpdateUserGroupsCreditsLowerByCreditsLower(int Creditslower, int Creditshigher)
        {
            DatabaseProvider.GetInstance().UpdateUserGroupsCreditsLowerByCreditsLower(Creditslower, Creditshigher);
        }


        public static void UpdateUserGroupsCreditsHigherByCreditsHigher(int Creditshigher, int Creditslower)
        {
            DatabaseProvider.GetInstance().UpdateUserGroupsCreditsHigherByCreditsHigher(Creditshigher, Creditslower);
        }

        /// <summary>
        /// 获取用户组数
        /// </summary>
        /// <param name="Creditshigher">积分上限</param>
        /// <returns></returns>
        public static int GetGroupCountByCreditsLower(int Creditshigher)
        {
            return DatabaseProvider.GetInstance().GetGroupCountByCreditsLower(Creditshigher);
        }

        /// <summary>
        /// 获取最小的积分上限
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMinCreditHigher()
        {
            return DatabaseProvider.GetInstance().GetMinCreditHigher();
        }

        /// <summary>
        /// 更新用户组积分上下限
        /// </summary>
        /// <param name="groupid"></param>
        public static void UpdateUserGroupLowerAndHigherToLimit(int groupid)
        {
            DatabaseProvider.GetInstance().UpdateUserGroupLowerAndHigherToLimit(groupid);
        }
    }
}
