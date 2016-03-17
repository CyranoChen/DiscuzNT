using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;
using System.IO;
using System.Web;

namespace Discuz.Forum
{
    /// <summary>
    /// AdminForumFactory 的摘要说明。
    /// 后台论坛版块管理类
    /// </summary>
    public class AdminForums : Forums
    {
        public static ForumInfo[] GetForumSpecialUser(string username)
        {
            DataTable dt = new DataTable();
            if (username == "")
                dt = Data.Forums.GetForumTableBySpecialUser("");
            else
                dt = Data.Forums.GetForumTableBySpecialUser(username);

            ForumInfo[] foruminfo = null;

            if (dt.Rows.Count > 0)
            {
                foruminfo = new ForumInfo[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foruminfo[i] = new ForumInfo();

                    if (dt.Rows[i]["permuserlist"].ToString() != "")
                    {
                        if (username != "")
                        {
                            foreach (string s in dt.Rows[i]["permuserlist"].ToString().Split('|'))
                            {
                                if (username == s.Split(',')[0])
                                {
                                    foruminfo[i].Permuserlist = s;
                                }
                            }
                        }
                        else
                        {
                            if (dt.Rows[i]["permuserlist"].ToString().Split('|').Length == 1)
                            {
                                foruminfo[i].Permuserlist = dt.Rows[i]["permuserlist"].ToString();
                            }
                            else
                            {
                                for (int j = 0; j < dt.Rows[i]["permuserlist"].ToString().Split('|').Length; j++)
                                {
                                    foruminfo[i].Permuserlist += dt.Rows[i]["permuserlist"].ToString().Split('|')[j] + "|";
                                }
                                foruminfo[i].Permuserlist = foruminfo[i].Permuserlist.ToString().Substring(0, foruminfo[i].Permuserlist.ToString().Length - 1);
                            }
                        }

                        foruminfo[i].Fid = Utils.StrToInt(dt.Rows[i]["fid"].ToString(), 0);
                        foruminfo[i].Name = dt.Rows[i]["name"].ToString();
                        foruminfo[i].Moderators = dt.Rows[i]["moderators"].ToString();
                    }
                }
            }
            return foruminfo;
        }

        public static ForumInfo[] GetForumSpecialUser(int fid)
        {
            //TODO:这个方法有问题，稍后研究
            DataTable dt = Data.Forums.GetForumTableWithSpecialUser(fid);

            ForumInfo[] foruminfo = null;

            if (dt.Rows.Count > 0)
            {
                foruminfo = new ForumInfo[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foruminfo[i] = new ForumInfo();

                    if (dt.Rows[i]["permuserlist"].ToString() != "")
                    {

                        if (dt.Rows[i]["permuserlist"].ToString().Split('|').Length == 1)
                        {
                            foruminfo[i].Permuserlist = dt.Rows[i]["permuserlist"].ToString();
                        }
                        else
                        {
                            for (int j = 0; j < dt.Rows[i]["permuserlist"].ToString().Split('|').Length; j++)
                            {

                                foruminfo[i].Permuserlist += dt.Rows[i]["permuserlist"].ToString().Split('|')[j] + "|";
                            }
                            foruminfo[i].Permuserlist = foruminfo[i].Permuserlist.ToString().Substring(0, foruminfo[i].Permuserlist.ToString().Length - 1);
                        }

                        foruminfo[i].Fid = Utils.StrToInt(dt.Rows[i]["fid"].ToString(), 0);
                        foruminfo[i].Name = dt.Rows[i]["name"].ToString();
                        foruminfo[i].Moderators = dt.Rows[i]["moderators"].ToString();
                    }
                }
            }
            return foruminfo;
        }


        /// <summary>
        /// 更新论坛版块(分类)的相关信息
        /// </summary>
        /// <param name="forumInfo"></param>
        /// <returns></returns>
        public static string UpdateForumInfo(ForumInfo forumInfo)
        {
            Data.Forums.UpdateForumInfo(forumInfo);
            DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
            SetForumsPathList();
            string result = SetForumsModerators(forumInfo.Fid.ToString(), forumInfo.Moderators, forumInfo.Inheritedmod);
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/ForumListBoxOptions");
            DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
            DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypesOption" + forumInfo.Fid);
            DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypesLink" + forumInfo.Fid);
            DNTCache.GetCacheService().RemoveObject("/Aggregation/HotForumList");
            DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumHotTopicList");
            DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumNewTopicList");
            return result;
        }


        /// <summary>
        /// 向版块列表中插入新的版块信息
        /// </summary>
        /// <param name="foruminfo"></param>
        /// <param name="moderatorsInfo">版主信息</param>
        /// <param name="adminUid">管理员Id</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGruopId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员IP</param>
        /// <returns></returns>
        public static int CreateForums(ForumInfo forumInfo, out string moderatorsInfo, int adminUid, string adminUserName, int adminUserGruopId, string adminUserGroupTitle, string adminIp)
        {
            int fid = Data.Forums.CreateForumInfo(forumInfo);
            SetForumsPathList();
            moderatorsInfo = SetForumsModerators(fid.ToString(), forumInfo.Moderators, forumInfo.Inheritedmod).Replace("'", "’");
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/ForumListBoxOptions");
            DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
            DNTCache.GetCacheService().RemoveObject("/Aggregation/HotForumList");
            DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumHotTopicList");
            DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumNewTopicList");
            DNTCache.GetCacheService().RemoveObject("/Forum/DropdownOptions");
            DNTCache.GetCacheService().RemoveObject("/Forum/ForumListMenuDiv");
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "添加论坛版块", "添加论坛版块,名称为:" + forumInfo.Name);
            return fid;
        }

        /// <summary>
        /// 向版块列表中插入新的版块信息
        /// </summary>
        /// <param name="foruminfo"></param>
        /// <returns></returns>
        public static int CreateForums(ForumInfo forumInfo)
        {
            string moderatorsInfo;
            return CreateForums(forumInfo, out moderatorsInfo, 0, "API", 0, "API", "");
        }

        /// <summary>
        /// 设置版块列表中论坛路径(pathlist)字段
        /// </summary>
        public static void SetForumsPathList()
        {
            GeneralConfigInfo config = GeneralConfigs.Deserialize(Utils.GetMapPath(BaseConfigs.GetForumPath + "config/general.config"));
            SetForumsPathList(config.Aspxrewrite == 1, config.Extname);
        }


        /// <summary>
        /// 按指定的文件扩展名称设置版块列表中论坛路径(pathlist)字段
        /// </summary>
        /// <param name="extname">扩展名称,如:aspx , html 等</param>
        public static void SetForumsPathList(bool isaspxrewrite, string extname)
        {
            DataTable dt = Forums.GetForumListForDataTable();
            string forumPath = BaseConfigs.GetForumPath;

            foreach (DataRow dr in dt.Rows)
            {
                string pathList = "";

                if (dr["parentidlist"].ToString().Trim() == "0")
                {
                    pathList = "<a href=\"" + (dr["rewritename"].ToString().Trim() == string.Empty ? string.Empty : forumPath) + Urls.ShowForumAspxRewrite(Utils.StrToInt(dr["fid"], 0), 0, dr["rewritename"].ToString()) + "\">" + dr["name"].ToString().Trim() + "</a>";
                }
                else
                {
                    foreach (string parentid in dr["parentidlist"].ToString().Trim().Split(','))
                    {
                        if (parentid.Trim() != "")
                        {
                            DataRow[] drs = dt.Select("[fid]=" + parentid);
                            if (drs.Length > 0)
                            {
                                pathList += "<a href=\"" + (drs[0]["rewritename"].ToString().Trim() == string.Empty ? string.Empty : forumPath) + Urls.ShowForumAspxRewrite(Utils.StrToInt(drs[0]["fid"], 0), 0, drs[0]["rewritename"].ToString()) + "\">" + drs[0]["name"].ToString().Trim() + "</a>";
                            }
                        }
                    }
                    string url = Urls.ShowForumAspxRewrite(Utils.StrToInt(dr["fid"], 0), 0, dr["rewritename"].ToString());
                    pathList += "<a href=\"" + (dr["rewritename"].ToString().Trim() == "" ? "" : forumPath) + Urls.ShowForumAspxRewrite(Utils.StrToInt(dr["fid"], 0), 0, dr["rewritename"].ToString()) + "\">" + dr["name"].ToString().Trim() + "</a>";
                }
                foreach (ForumInfo forumInfo in Discuz.Data.Forums.GetForumList())
                {
                    if (forumInfo.Fid == int.Parse(dr["fid"].ToString()))
                    {
                        forumInfo.Pathlist = pathList;
                        Data.Forums.UpdateForumInfo(forumInfo);
                    }
                }
            }
        }


        /// <summary>
        /// 设置版块列表中层数(layer)和父列表(parentidlist)字段
        /// </summary>
        public static void SetForumslayer()
        {
            foreach (ForumInfo singleForumInfo in Forums.GetForumList())
            {
                int layer = 0;
                string parentidlist = "";
                int parentid = singleForumInfo.Parentid;

                //如果是(分类)顶层则直接更新数据库
                if (parentid == 0)
                {
                    ForumInfo forumInfo = Forums.GetForumInfo(singleForumInfo.Fid);
                    if (forumInfo.Layer != layer)
                    {
                        forumInfo.Layer = layer;
                        forumInfo.Parentidlist = "0";
                        UpdateForumInfo(forumInfo);
                    }
                    continue;
                }

                do
                { //更新子版块的层数(layer)和父列表(parentidlist)字段
                    int temp = parentid;

                    parentid = Forums.GetForumInfo(parentid).Parentid;
                    layer++;
                    if (parentid != 0)
                    {
                        parentidlist = temp + "," + parentidlist;
                    }
                    else
                    {
                        parentidlist = (temp + "," + parentidlist).TrimEnd(',');
                        ForumInfo forumInfo = Forums.GetForumInfo(singleForumInfo.Fid);
                        if (forumInfo.Layer != layer || forumInfo.Parentidlist != parentidlist)
                        {
                            forumInfo.Layer = layer;
                            forumInfo.Parentidlist = parentidlist;
                            UpdateForumInfo(forumInfo);
                        }
                        break;
                    }
                } while (true);
            }

        }


        /// <summary>
        /// 移动论坛版块
        /// </summary>
        /// <param name="currentfid">当前论坛版块id</param>
        /// <param name="targetfid">目标论坛版块id</param>
        /// <param name="isaschildnode">是否作为子论坛移动</param>
        /// <returns></returns>
        public static bool MovingForumsPos(string currentfid, string targetfid, bool isaschildnode)
        {
            string extname = GeneralConfigs.Deserialize(Utils.GetMapPath(BaseConfigs.GetForumPath + "config/general.config")).Extname;

            Discuz.Data.Forums.MovingForumsPos(currentfid, targetfid, isaschildnode, extname);
            AdminForums.SetForumslayer();
            AdminForums.SetForumsSubForumCountAndDispalyorder();
            AdminForums.SetForumsPathList();

            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/ForumListBoxOptions");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");

            return true;
        }


        /// <summary>
        /// 设置指定论坛版块版主
        /// </summary>
        /// <param name="fid">指定的论坛版块id</param>
        /// <param name="moderators">相关要设置的版主名称(注:用","号分割)</param>
        /// <param name="inherited">是否使用继承选项 1为使用  0为不使用</param>
        /// <returns></returns>
        public static string SetForumsModerators(string fid, string moderators, int inherited)
        {
            //清除已有论坛的版主设置
            Data.Moderators.DeleteModeratorByFid(int.Parse(fid));

            //使用继承机制时
            if (inherited == 1)
            {
                string parentid = fid;
                string parendidlist = "-1";
                while (true)
                {
                    DataTable dt = Discuz.Data.Forums.GetParentIdByFid(int.Parse(parentid));
                    if (dt.Rows.Count > 0) parentid = dt.Rows[0][0].ToString();
                    else
                        break;

                    if ((parentid == "0") || (parentid == ""))
                        break;

                    parendidlist = parendidlist + "," + parentid;
                }

                int count = 1;
                foreach (DataRow dr in Discuz.Data.Moderators.GetUidModeratorByFid(parendidlist).Rows)
                {
                    Discuz.Data.Moderators.AddModerator(int.Parse(dr[0].ToString()), int.Parse(fid), count, 1);

                    count++;
                }
            }

            InsertForumsModerators(fid, moderators, 1, 0);

            return UpdateUserInfoWithModerator(moderators);
        }


        /// <summary>
        /// 更新当前已设置为指定版块版主的相关用户信息.
        /// </summary>
        /// <param name="moderators">相关要设置的版主名称(注:用","号分割)</param>
        /// <returns>返回不存在用户的字符串</returns>
        public static string UpdateUserInfoWithModerator(string moderators)
        {
            moderators = moderators == null ? "" : moderators;
            string usernamenoexsit = "";
            DataTable dt = new DataTable();
            foreach (string moderator in moderators.Split(','))
            {
                if (moderator != "")
                {
                    //当用户名是系统保留的用户名,请您输入其它的用户名
                    if (PrivateMessages.SystemUserName == moderator)
                        continue;

                    UserInfo userInfo = Users.GetUserInfo(moderator);

                    if (userInfo != null && userInfo.Groupid != 7 && userInfo.Groupid != 8)
                    {
                        if ((userInfo.Groupid <= 3) && (userInfo.Groupid > 0)) continue; //当为管理员,超级版主,版主时
                        else
                        {
                            int radminid = UserGroups.GetUserGroupInfo(userInfo.Groupid).Radminid;
                            if (radminid <= 0)
                                Data.Users.SetUserToModerator(moderator);
                            else
                                continue;
                        }
                    }
                    else
                        usernamenoexsit = usernamenoexsit + moderator + ",";
                }
            }

            Caches.ReSetModeratorList();
            return usernamenoexsit;
        }


        /// <summary>
        /// 向版主列表中插入相关的版主信息
        /// </summary>
        /// <param name="fid">指定的论坛版块</param>
        /// <param name="moderators">相关要设置的版主名称(注:用","号分割)</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="inherited">是否使用继承机制</param>
        public static void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited)
        {
            moderators = moderators == null ? "" : moderators;
            Discuz.Data.Moderators.InsertForumsModerators(fid, moderators, displayorder, inherited);

            Caches.ReSetModeratorList();
        }


        /// <summary>
        /// 对比指定的论坛版块的新老信息,将作出相应的调整
        /// </summary>
        /// <param name="oldmoderators">老版主名称(注:用","号分割)</param>
        /// <param name="newmoderators">新版主名称(注:用","号分割)</param>
        /// <param name="currentfid">当前论坛版块的fid</param>
        public static void CompareOldAndNewModerator(string oldmoderators, string newmoderators, int currentfid)
        {
            if ((oldmoderators == null) || (oldmoderators == ""))
                return;

            //在新的版主名单中查找老的版主是否存在
            foreach (string oldmoderator in oldmoderators.Split(','))
            {
                if ((oldmoderator != "") &&
                    ("," + newmoderators + ",").IndexOf("," + oldmoderator + ",") < 0) //当不存在，则表示当前老的版主已被删除，则执行删除当前老版主
                {
                    UserInfo info = Users.GetUserInfo(oldmoderator);
                    if (info != null) //当前用户存在
                    {
                        int uid = info.Uid;
                        int radminid = info.Adminid;
                        DataTable dt = Discuz.Data.Moderators.GetUidInModeratorsByUid(currentfid, uid);

                        //在其他版块未曾设置为版主  注:(当大于0时则表示在其它版还有相应的版主身份,则不作任何处理)
                        if ((dt.Rows.Count == 0) && (radminid != 1))
                        {
                            UserGroupInfo userGroupInfo = UserCredits.GetCreditsUserGroupId(info.Credits);

                            Discuz.Data.Users.UpdateUserOnlineInfo(userGroupInfo.Groupid, uid);
                            Discuz.Data.Users.UpdateUserOtherInfo(userGroupInfo.Groupid, uid);
                        }
                    }
                }
            }
        }

        #region  递归指定论坛版块下的所有子版块

        public static string ChildNode = "0";

        /// <summary>
        /// 递归所有子节点并返回字符串
        /// </summary>
        /// <param name="correntfid">当前</param>
        /// <returns>子版块的集合,格式:1,2,3,4,</returns>
        public static string FindChildNode(string correntfid)
        {
            lock (ChildNode)
            {
                DataTable dt = Discuz.Data.Forums.GetForumByParentid(int.Parse(correntfid));

                ChildNode = ChildNode + "," + correntfid;

                if (dt.Rows.Count > 0)
                {
                    //有子节点
                    foreach (DataRow dr in dt.Rows)
                    {
                        FindChildNode(dr["fid"].ToString());
                    }
                }
                dt.Dispose();
                return ChildNode;
            }
        }

        #endregion

        /// <summary>
        /// 合并版块
        /// </summary>
        /// <param name="sourcefid">源论坛版块</param>
        /// <param name="targetfid">目标论坛版块</param>
        /// <returns></returns>
        public static bool CombinationForums(string sourcefid, string targetfid)
        {
            if (Discuz.Data.Forums.IsExistSubForum(int.Parse(sourcefid)))
            {
                return false;
            }
            else
            {
                ChildNode = "0";
                string fidlist = ("," + FindChildNode(targetfid)).Replace(",0,", "");
                Discuz.Data.Forums.CombinationForums(sourcefid, targetfid, fidlist);

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/ForumListBoxOptions");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
                return true;
            }
        }


        /// <summary>
        /// 设置论坛字版数和显示顺序
        /// </summary>
        public static void SetForumsSubForumCountAndDispalyorder()
        {
            DataTable dt = Forums.GetForumListForDataTable();

            foreach (DataRow dr in dt.Rows)
            {
                Discuz.Data.Forums.UpdateSubForumCount(int.Parse(dt.Select("parentid=" + dr["fid"].ToString()).Length.ToString()), int.Parse(dr["fid"].ToString()));
            }

            if (dt.Rows.Count == 1) return;
            int displayorder = 1;
            string fidlist;
            foreach (DataRow dr in dt.Select("parentid=0"))
            {
                if (dr["parentid"].ToString() == "0")
                {
                    ChildNode = "0";
                    fidlist = ("," + FindChildNode(dr["fid"].ToString())).Replace(",0,", "");

                    foreach (string fidstr in fidlist.Split(','))
                    {
                        Data.Forums.UpdateDisplayorderInForumByFid(displayorder, TypeConverter.StrToInt(fidstr));
                        displayorder++;
                    }
                }
            }
        }


        /// <summary>
        /// 设置论坛版块的状态
        /// </summary>
        public static void SetForumsStatus()
        {
            DataTable dt = Discuz.Data.Forums.GetMainForum();


            foreach (DataRow dr in dt.Rows)
            {
                ChildNode = "0";
                string fidlist = ("," + FindChildNode(dr["fid"].ToString())).Replace(",0,", "");

                if (dr["status"].ToString() == "0")
                {
                    Discuz.Data.Forums.UpdateStatusByFidlist(fidlist);
                }
                else if (dr["status"].ToString() == "1")
                {
                    Discuz.Data.Forums.UpdateStatusByFidlistOther(fidlist);
                }
                else
                {
                    Discuz.Data.Forums.SetStatusInForum(4, int.Parse(dr["fid"].ToString()));

                    int i = 5;
                    foreach (DataRow currentdr in Discuz.Data.Forums.GetForumByParentid(int.Parse(dr["fid"].ToString())).Rows)
                    {
                        Discuz.Data.Forums.SetStatusInForum(i, int.Parse(currentdr["fid"].ToString()));
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// 批理设置论坛信息
        /// </summary>
        /// <param name="__foruminfo">复制的论坛信息</param>
        /// <param name="bsp">是否要批量设置的信息字段</param>
        /// <param name="fidlist">目标论坛(fid)串</param>
        /// <returns></returns>
        public static bool BatchSetForumInf(ForumInfo forumInfo, BatchSetParams bsp, string fidlist)
        {
            return Discuz.Data.Forums.BatchSetForumInf(forumInfo, bsp, fidlist);
        }
      

        /// <summary>
        /// 获得forum的最大排序号
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static DataTable GetMaxDisplayOrder(int fid)
        {
            return Discuz.Data.Forums.GetMaxDisplayOrder(fid);
        }

        /// <summary>
        /// 获得forum的最大排序号
        /// </summary>
        /// <returns></returns>
        public static int GetMaxDisplayOrder()
        {
            return Discuz.Data.Forums.GetMaxDisplayOrder();
        }

        /// <summary>
        /// 更新论坛排序号
        /// </summary>
        /// <param name="currentdisplayorder"></param>
        public static void UpdateForumsDisplayOrder(int currentdisplayorder)
        {
            Discuz.Data.Forums.UpdateForumsDisplayOrder(currentdisplayorder);
        }

        /// <summary>
        /// 设置子版块数量
        /// </summary>
        /// <param name="fid"></param>
        public static void SetSubForumCount(int fid)
        {
            Discuz.Data.Forums.SetSubForumCount(fid);
        }

        /// <summary>
        /// 创建表情
        /// </summary>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="type">分类</param>
        /// <param name="code">快捷编码</param>
        /// <param name="url">图片地址</param>
        /// <param name="adminUid">管理员Id</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGruopId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员IP</param>
        public static void CreateSmilies(int displayOrder, int type, string code, string url, int adminUid, string adminUserName, int adminUserGruopId,
            string adminUserGroupTitle, string adminIp)
        {
            Discuz.Data.Smilies.CreateSmilies(Smilies.GetMaxSmiliesId(), displayOrder, type, code, url);
            ResetCacheObjectAboutSmilies();
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "表情文件添加", code);
        }

        /// <summary>
        /// 修改表情
        /// </summary>
        /// <param name="id">表情Id</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="type">分类</param>
        /// <param name="code">快捷编码</param>
        /// <param name="url">图片地址</param>
        /// <param name="adminUid">管理员Id</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGruopId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员IP</param>
        public static void UpdateSmilies(int id, int displayOrder, int type, string code, string url, int adminUid, string adminUserName, int adminUserGruopId,
            string adminUserGroupTitle, string adminIp)
        {
            Discuz.Data.Smilies.UpdateSmilies(id, displayOrder, type, code);
            ResetCacheObjectAboutSmilies();
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "表情文件修改", code);
        }

        /// <summary>
        /// 删除表情
        /// </summary>
        /// <param name="idList">表情Id列表</param>
        /// <param name="adminUid">管理员Id</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGruopId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员IP</param>
        public static void DeleteSmilies(string idList, int adminUid, string adminUserName, int adminUserGruopId, string adminUserGroupTitle, string adminIp)
        {
            Discuz.Data.Smilies.DeleteSmilies(idList);
            ResetCacheObjectAboutSmilies();
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "表情文件删除", "ID:" + idList);
        }

        private static void ResetCacheObjectAboutSmilies()
        {
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesList");
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListFirstPage");
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListWithInfo");
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesTypeList");
        }

        /// <summary>
        /// 建立更新用户积分存储过程的方法
        /// </summary>
        /// <param name="creditExpression">总积分计算公式</param>
        /// <param name="testCreditExpression">是否需要测试总积分计算公式是否正确</param>
        /// <returns></returns>
        public static bool CreateUpdateUserCreditsProcedure(string creditExpression, bool testCreditExpression)
        {
            return Discuz.Data.Forums.CreateUpdateUserCreditsProcedure(creditExpression, testCreditExpression);
        }

        /// <summary>
        /// 检查表达式是否正确,建立更新用户积分存储过程的方法
        /// </summary>
        /// <param name="creditExpression">总积分计算公式</param>
        /// <returns></returns>
        public static bool CreateUpdateUserCreditsProcedure(string creditExpression)
        {
            return CreateUpdateUserCreditsProcedure(creditExpression, true);
        }

        /// <summary>
        /// 上传版块图标
        /// </summary>
        /// <returns></returns>
        public static string UploadForumIcon(int fId)
        {
            if (fId <= 0)
                return "";

            string fileName = Path.GetFileName(HttpContext.Current.Request.Files[0].FileName);
            string fileExtName = Utils.CutString(fileName, fileName.LastIndexOf(".") + 1).ToLower();

            if (!Utils.IsImgFilename(fileName))
                return "";

            fileName = string.Format("forumicon_{0}.{1}", fId, fileExtName);
            string iconPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/forumicons/");

            if (!Directory.Exists(iconPath))
                Utils.CreateDir(iconPath);

            HttpContext.Current.Request.Files[0].SaveAs(iconPath + fileName);

            return "upload/forumicons/" + fileName;
        }

        /// <summary>
        /// 批理设置版块模板信息
        /// </summary>
        /// <param name="templateID">新的模板id</param>
        /// <param name="fidlist">要更新的版块id列表</param>
        /// <returns></returns>
        public static int UpdateForumTemplateID(ForumInfo forumInfo)
        {
            string fidlist = FindChildNode(forumInfo.Fid.ToString());
            if (Utils.IsNumericList(fidlist) && forumInfo.Templateid >= 0)
                return Discuz.Data.Forums.UpdateForumTemplateID(forumInfo.Templateid, fidlist);
            else
                return 0;
        }
    }
}