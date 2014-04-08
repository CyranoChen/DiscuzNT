using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using System.Text;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 用户组操作类
    /// </summary>
    public class UserGroups
    {
        /// <summary>
        /// 获得用户组数据
        /// </summary>
        /// <returns>用户组数据</returns>
        public static List<UserGroupInfo> GetUserGroupList()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            List<UserGroupInfo> userGruopInfoList = cache.RetrieveObject("/Forum/UserGroupList") as List<UserGroupInfo>;

            if (userGruopInfoList == null)
            {
                userGruopInfoList = Discuz.Data.UserGroups.GetUserGroupList();
                cache.AddObject("/Forum/UserGroupList", userGruopInfoList);
            }
            return userGruopInfoList;
        }


        /// <summary>
        /// 读取指定组的信息
        /// </summary>
        /// <param name="groupid">组id</param>
        /// <returns>组信息</returns>
        public static UserGroupInfo GetUserGroupInfo(int groupid)
        {
            List<UserGroupInfo> userGroupInfoList = GetUserGroupList();
            UserGroupInfo info = userGroupInfoList.Find(delegate(UserGroupInfo i) { return i.Groupid == groupid; });
            if (info != null)
            {
                return info;
            }
            else
            {
                // 如果查找不到则为游客
                return userGroupInfoList.Find(delegate(UserGroupInfo i) { return i.Groupid == 7; });
            }
        }


        /// <summary>
        /// 创建临时用户组积分表
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateGroupScoreTable()
        {
            DataTable templateDT = new DataTable("templateDT");

            templateDT.Columns.Add("id", Type.GetType("System.Int32"));
            templateDT.Columns.Add("available", Type.GetType("System.Boolean")); //是否参与积分字段
            templateDT.Columns.Add("ScoreCode", Type.GetType("System.Int32"));//积分代号
            templateDT.Columns.Add("ScoreName", Type.GetType("System.String"));//积分名称
            templateDT.Columns.Add("Min", Type.GetType("System.String"));//评分最小值
            templateDT.Columns.Add("Max", Type.GetType("System.String"));//评分最大值
            templateDT.Columns.Add("MaxInDay", Type.GetType("System.String"));//24小时最大评分数
            templateDT.Columns.Add("Options", Type.GetType("System.String"));//options HTML代码 

            //向表中加载默认设置
            for (int rowcount = 0; rowcount < 8; rowcount++)
            {
                DataRow dr = templateDT.NewRow();
                dr["id"] = rowcount + 1;
                dr["available"] = false;
                dr["ScoreCode"] = rowcount + 1;
                dr["ScoreName"] = "";
                dr["Min"] = "";
                dr["Max"] = "";
                dr["MaxInDay"] = "";
                templateDT.Rows.Add(dr);
            }

            //通过CONFIG文件得到相关的ScoreName名称设置
            DataRow scoresetname = Scoresets.GetScoreSet().Rows[0];
            for (int count = 0; count < 8; count++)
            {
                if ((!Utils.StrIsNullOrEmpty(scoresetname[count + 2].ToString())) && (scoresetname[count + 2].ToString().Trim() != "0"))
                    templateDT.Rows[count]["ScoreName"] = scoresetname[count + 2].ToString().Trim();
            }
            return templateDT;
        }

        /// <summary>
        /// 通过组ID得到允许的评分范围,如果无设置则返回空表
        /// </summary>
        /// <param name="groupid">组ID</param>
        /// <returns>评分范围</returns>
        public static DataTable GroupParticipateScore(int groupid)
        {
            DataTable dt = Discuz.Data.UserGroups.GetUserGroupRateRange(groupid);
            //当用户组未设置允许的评分范围时返回空表
            if (dt.Rows.Count == 0)
                return null;
            //创建并初始化表结构
            DataTable templateDT = CreateGroupScoreTable();
            if (Utils.StrIsNullOrEmpty(dt.Rows[0][0].ToString()))
                return templateDT;

            //用数据库中的记录更新已装入的默认数据
            int i = 0;
            foreach (string raterangestr in dt.Rows[0][0].ToString().Trim().Split('|'))
            {
                if (raterangestr.Trim() != "")
                {
                    string[] scoredata = raterangestr.Split(',');
                    //判断是否参与积分字段的数据判断
                    if (scoredata[1].Trim() == "True")
                        templateDT.Rows[i]["available"] = true;

                    //更新其它字段
                    templateDT.Rows[i]["Min"] = scoredata[4].Trim();
                    templateDT.Rows[i]["Max"] = scoredata[5].Trim();
                    templateDT.Rows[i]["MaxInDay"] = scoredata[6].Trim();
                }
                i++;
            }
            return templateDT;
        }

        /// <summary>
        /// 通过组ID和UID得到允许的评分范围,如果无设置则返回空表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="gid">用户组别</param>
        /// <returns>ID和UID允许的评分范围</returns>
        public static DataTable GroupParticipateScore(int uid, int gid)
        {
            DataTable dt = GroupParticipateScore(gid);
            int[] extcredits = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            StringBuilder sb = new StringBuilder();
            int offset = 1;
            if (dt != null)
            {
                extcredits = Discuz.Data.UserGroups.GroupParticipateScore(uid);

                int max = 0;
                int min = 0;
                int maxInDay = 0;
                int maxSelect = 0;

                DataRow dr = null;
                for (int count = dt.Rows.Count - 1; count >= 0; count--)
                {
                    dr = dt.Rows[count];

                    max = TypeConverter.ObjectToInt(dr["Max"]);
                    min = TypeConverter.ObjectToInt(dr["Min"]);
                    maxInDay = TypeConverter.ObjectToInt(dr["MaxInDay"]);
                    maxInDay = maxInDay - extcredits[TypeConverter.ObjectToInt(dr["ScoreCode"])];
                    dr["MaxInDay"] = maxInDay;
                    maxSelect = max > maxInDay ? maxInDay : max;

                    if (!Convert.ToBoolean(dr["available"]) || maxInDay <= 0)
                    {
                        dr.Delete();
                        continue;
                    }

                    offset = TypeConverter.ObjectToInt(Math.Abs(Math.Ceiling((maxSelect - min) / 10.0)));
                    offset = offset <= 0 ? 1 : offset;
                    sb.Remove(0, sb.Length);
                    for (int i = maxSelect; i >= TypeConverter.ObjectToFloat(dr["Min"]); i -= offset)
                    {
                        if (i == 0)
                            continue;
                        if (Math.Abs(i) <= maxInDay)
                            sb.AppendFormat("\n<li>{0}{1}</li>", i > 0 ? "+" : "", i);
                    }
                    dr["Options"] = sb.ToString();
                }
                dt.AcceptChanges();
            }

            if (dt == null)
                dt = new DataTable();

            return dt;
        }

        /// <summary>
        /// 检验相关用户组的主题\附件最高售价
        /// </summary>
        /// <param name="usergroupinfo"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static int CheckUserGroupMaxPrice(UserGroupInfo usergroupinfo, int price)
        {
            if (price <= 0)
                return 0;

            if (price > 0 && price <= usergroupinfo.Maxprice)
                return price;
            else
                return usergroupinfo.Maxprice;
        }

        /// <summary>
        /// 获取最大groupid
        /// </summary>
        /// <returns></returns>
        public static int GetMaxUserGroupId()
        {
            List<UserGroupInfo> list = GetUserGroupList();
            int maxUserGroupId = 0;
            foreach (UserGroupInfo userGroupInfo in list)
            {
                if (userGroupInfo.Groupid > maxUserGroupId)
                    maxUserGroupId = userGroupInfo.Groupid;
            }
            return maxUserGroupId;
        }

        /// <summary>
        /// 获取管理组列表
        /// </summary>
        /// <returns></returns>
        public static List<UserGroupInfo> GetAdminUserGroup()
        {
            //List<UserGroupInfo> list = GetUserGroupList();
            //List<UserGroupInfo> adminList = new List<UserGroupInfo>();
            //foreach (UserGroupInfo userGroupInfo in list)
            //{
            //    if (userGroupInfo.Radminid > 0)
            //        adminList.Add(userGroupInfo);
            //}
            //return adminList;
            return GetUserGroupByAdminIdList("1,2,3");
        }

        /// <summary>
        /// 获取管理组和特殊组列表
        /// </summary>
        /// <returns></returns>
        public static List<UserGroupInfo> GetAdminAndSpecialGroup()
        {
            return GetUserGroupByAdminIdList("-1,1,2,3");
        }

        /// <summary>
        /// 获取指定类型用户组列表
        /// </summary>
        /// <param name="adminIdList"></param>
        /// <returns></returns>
        private static List<UserGroupInfo> GetUserGroupByAdminIdList(string adminIdList)
        {
            List<UserGroupInfo> list = GetUserGroupList();
            List<UserGroupInfo> adminList = new List<UserGroupInfo>();
            foreach (UserGroupInfo userGroupInfo in list)
            {
                if (Utils.InArray(userGroupInfo.Radminid.ToString(), adminIdList))
                    adminList.Add(userGroupInfo);
            }
            return adminList;
        }

        /// <summary>
        /// 更新用户组积分设置信息
        /// </summary>
        /// <param name="raterange">积分设置信息</param>
        /// <param name="groupid">用户组Id</param>
        public static void UpdateUserGroupRaterange(string raterange, int groupid)
        {
            Data.UserGroups.UpdateUserGroupRaterange(raterange, groupid);
        }

        /// <summary>
        /// 更新用户组信息
        /// </summary>
        /// <param name="info">用户组信息</param>
        public static void UpdateUserGroup(UserGroupInfo info)
        {
            Data.UserGroups.UpdateUserGroup(info);
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupid">用户组Id</param>
        public static void DeleteUserGroupInfo(int groupid)
        {
            Data.UserGroups.DeleteUserGroupInfo(groupid);
        }

        /// <summary>
        /// 更新全部用户的组Id
        /// </summary>
        /// <param name="sourceGroupId">源组Id</param>
        /// <param name="targetGroupId">目标组Id</param>
        public static void ChangeAllUserGroupId(int sourceGroupId, int targetGroupId)
        {
            Data.UserGroups.ChangeAllUserGroupId(sourceGroupId, targetGroupId);
        }

        /// <summary>
        /// 获取除指定id的用户组外的普通用户组
        /// </summary>
        /// <param name="groupid">用户组</param>
        /// <returns></returns>
        public static DataTable GetUserGroupExceptGroupid(int groupid)
        {
            return Data.UserGroups.GetUserGroupExceptGroupid(groupid);
        }

        /// <summary>
        /// 获取管理组
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminGroups()
        {
            return Data.UserGroups.GetAdminGroups();
        }

        /// <summary>
        /// 获取在线列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOnlineList()
        {
            return Data.UserGroups.GetOnlineList();
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
            return Data.UserGroups.UpdateOnlineList(groupid, displayorder, img, title);
        }

        /// <summary>
        /// 获取评分范围
        /// </summary>
        /// <param name="scoreid"></param>
        /// <returns></returns>
        public static DataTable GetRateRange(int scoreid)
        {
            return Data.UserGroups.GetRateRange(scoreid);
        }

        /// <summary>
        /// 更新评分范围
        /// </summary>
        /// <param name="raterange"></param>
        /// <param name="groupid"></param>
        public static void UpdateRateRange(string raterange, int groupid)
        {
            Data.UserGroups.UpdateRateRange(raterange, groupid);
        }

        /// <summary>
        /// 获取除游客组之外的用户组名称
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserGroupWithOutGuestTitle()
        {
            return Data.UserGroups.GetUserGroupWithOutGuestTitle();
        }

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserGroupForDataTable()
        {
            return Data.UserGroups.GetUserGroupForDataTable();
        }

        /// <summary>
        /// 获取积分用户组
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCreditUserGroup()
        {
            return Data.UserGroups.GetCreditUserGroup();
        }

        /// <summary>
        /// 判断用户组是否为积分用户组
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <returns>是/否</returns>
        public static bool IsCreditUserGroup(int groupid)
        {
            //UserGroupInfo usergroupInfo = GetUserGroupList().Find(
            //    delegate(UserGroupInfo u)
            //    {
            //        return u.Radminid == 0 && u.System == 0 && u.Groupid == groupid;
            //    }
            //    );
            ////如果不为空，说明是属于积分用户组的
            //return usergroupInfo != null;
            return IsCreditUserGroup(GetUserGroupInfo(groupid));
        }
        /// <summary>
        /// 判断用户组是否为积分用户组
        /// </summary>
        /// <param name="usergroup">用户组信息</param>
        /// <returns>是/否</returns>
        public static bool IsCreditUserGroup(UserGroupInfo usergroup)
        {
            if (usergroup.Radminid == 0 && usergroup.System == 0)
                return true;
            return false;
        }
        /// <summary>
        /// 获取特殊用户组
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSpecialUserGroup()
        {
            return Data.UserGroups.GetSpecialUserGroup();
        }
    }
}
