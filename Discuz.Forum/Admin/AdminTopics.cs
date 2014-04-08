using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// AdminTopicFactory 
    /// </summary>
    public class AdminTopics : TopicAdmins
    {
        public AdminTopics()
        { }


        /// <summary>
        ///
        /// </summary>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        public static bool UpdateTopicAllInfo(TopicInfo topicinfo)
        {
            try
            {
                Topics.UpdateTopic(topicinfo);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static bool DeleteTopicByTid(int tid)
        {
            return Discuz.Data.Posts.DeleteTopicByTid(tid, Posts.GetPostTableName());
        }


        public static bool SetTypeid(string topiclist, int value)
        {
            return Discuz.Data.Topics.SetTypeid(topiclist, value);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static DataSet AdminGetPostList(int tid, int pagesize, int pageindex)
        {
            DataSet ds = Discuz.Data.Posts.GetPosts(tid, pagesize, pageindex, Discuz.Data.PostTables.GetPostTableId(tid));

            if (ds == null)
            {
                ds = new DataSet();
                ds.Tables.Add("post");
                ds.Tables.Add();
                return ds;
            }
            ds.Tables[0].TableName = "post";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["attachment"].ToString().Equals("1"))
                    dr["attachment"] = Attachments.GetAttachmentCountByPid(Utils.StrToInt(dr["pid"], 0));
            }
            return ds;
        }

        //public static DataTable GetUnauditNewTopic()
        //{
        //    return Discuz.Data.Topics.GetUnauditNewTopic();
        //}

        /// <summary>
        /// 获取未审核的帖子
        /// </summary>
        /// <param name="currentPostTableId">分表ID</param>
        /// <returns></returns>
        //public static DataTable GetUnauditPost(int currentPostTableId)
        //{
        //    return Discuz.Data.Posts.GetUnauditPost(currentPostTableId);
        //}

        /// <summary>
        /// 批量移动主题
        /// </summary>
        /// <param name="tidList">移动的主题Id列表</param>
        /// <param name="targetForumId">目标版块Id</param>
        /// <param name="adminUid">管理员Uid</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGroupId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员Ip</param>
        public static void BatchMoveTopics(string tidList, int targetForumId, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            //先找出当前主题列表中所属的FID
            foreach (DataRow olddr in Data.Topics.GetTopicFidByTid(tidList).Rows)
            {
                string oldtidlist = "0";
                //以FID和列表为条件列出在当前FID下的主题列表
                foreach (DataRow mydr in Data.Topics.GetTopicTidByFid(tidList, int.Parse(olddr["fid"].ToString())).Rows)
                {
                    oldtidlist += "," + mydr["tid"].ToString();
                }
                //调用前台操作函数,后台暂时不支持移动主题定义主题分类
                TopicAdmins.MoveTopics(oldtidlist, targetForumId, Convert.ToInt16(olddr["fid"].ToString()), 0);
            }
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量移动主题", "主题ID:" + tidList + " <br />目标论坛fid:" + targetForumId);
        }

        /// <summary>
        /// 批量删除主题
        /// </summary>
        /// <param name="tidList">主题Id列表</param>
        /// <param name="isChagePostNumAndCredits">是否要更新用户的主题数与积分</param>
        /// <param name="adminUid">管理员Uid</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGroupId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员Ip</param>
        public static void BatchDeleteTopics(string tidList, bool isChagePostNumAndCredits, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            DeleteTopics(tidList, isChagePostNumAndCredits ? 1 : 0, false);
            Attachments.UpdateTopicAttachment(tidList);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量删除主题", "主题ID:" + tidList);
        }

        /// <summary>
        /// 批量主题置顶
        /// </summary>
        /// <param name="tidList">主题Id列表</param>
        /// <param name="displayOrder">置顶级别</param>
        /// <param name="adminUid">管理员Uid</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGroupId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员Ip</param>
        public static void BatchChangeTopicsDisplayOrderLevel(string tidList, int displayOrderLevel, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            Data.Topics.SetDisplayorder(tidList, displayOrderLevel);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量置顶主题", "主题ID:" + tidList + "<br /> 置顶级为:" + displayOrderLevel);
        }

        /// <summary>
        /// 批量设置主题精华
        /// </summary>
        /// <param name="tidList">主题Id列表</param>
        /// <param name="digestLevel">精华级别</param>
        /// <param name="adminUid">管理员Uid</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGroupId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员Ip</param>
        public static void BatchChangeTopicsDigest(string tidList, int digestLevel, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            TopicAdmins.SetDigest(tidList, digestLevel);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量加精主题", "主题ID:" + tidList + "<br /> 加精级为:" + digestLevel);
        }

        /// <summary>
        /// 批量删除主题附件
        /// </summary>
        /// <param name="tidList">主题Id列表</param>
        /// <param name="adminUid">管理员Uid</param>
        /// <param name="adminUserName">管理员用户名</param>
        /// <param name="adminUserGroupId">管理员用户组Id</param>
        /// <param name="adminUserGroupTitle">管理员用户组名称</param>
        /// <param name="adminIp">管理员Ip</param>
        public static void BatchDeleteTopicAttachs(string tidList, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            Attachments.DeleteAttachmentByTid(tidList);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "删除主题中的附件", "主题ID:" + tidList);
        }
    }
}
