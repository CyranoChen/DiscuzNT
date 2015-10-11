using System;
using System.Text;
using System.Data;

using Discuz.Common;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Cache.Data;

namespace Discuz.Data
{
    public class TopicAdmins
    {
        /// <summary>
        /// 设置主题指定字段的属性值(字符型)
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        public static int SetTopicStatus(string topiclist, string field, string intValue)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.SetTopicStatus(topiclist, field, intValue);

            return DatabaseProvider.GetInstance().SetTopicStatus(topiclist, field, intValue);
        }

        /// <summary>
        /// 获取置顶主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static DataSet GetTopTopicList()
        {
            return DatabaseProvider.GetInstance().GetTopTopicList();
        }

        /// <summary>
        /// 获取简要板块信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetShortForums()
        {
            return DatabaseProvider.GetInstance().GetShortForums();
        }

        public static string ResetTopTopicListSql(int layer, string fid, string parentidlist)
        {
            return DatabaseProvider.GetInstance().ResetTopTopicListSql(layer, fid, parentidlist);
        }


        /// <summary>
        ///  根据得到给定主题的用户列表(posterid)
        /// </summary>
        /// <param name="topicList">主题列表</param>
        /// <param name="op">操作源(0:精华,1:删除)</param>      
        /// <param name="losslessdel">删帖不减积分时间期限(天)</param>
        /// <returns></returns>
        public static List<ShortUserInfo> GetUserListWithTopicList(string topicList, int op, int losslessdel)
        {
            IDataReader reader = null;
            if (op == 1 && losslessdel != 0)
                reader = DatabaseProvider.GetInstance().GetUserListWithTopicList(topicList, losslessdel);
            else
                reader = DatabaseProvider.GetInstance().GetUserListWithTopicList(topicList);

            List<ShortUserInfo> users = new List<ShortUserInfo>();
            while (reader.Read())
            {
                ShortUserInfo userInfo = new ShortUserInfo();
                userInfo.Uid = TypeConverter.ObjectToInt(reader["posterid"], -1);
                users.Add(userInfo);
            }
            reader.Close();
            return users;
        }

        public static List<ShortUserInfo> GetUserListWithDigestTopicList(string digestTopicList, int digestType)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserListWithDigestTopicList(digestTopicList, digestType);

            List<ShortUserInfo> users = new List<ShortUserInfo>();
            while (reader.Read())
            {
                ShortUserInfo userInfo = new ShortUserInfo();
                userInfo.Uid = TypeConverter.ObjectToInt(reader["posterid"], -1);
                users.Add(userInfo);
            }
            reader.Close();
            return users;
        }

        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topicList">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        public static int SetTopicClose(string topicList, short intValue)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.SetTopicClose(topicList, intValue);

            return DatabaseProvider.GetInstance().SetTopicClose(topicList, intValue);
        }

        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topiclist">要删除的主题ID列表</param>
        /// <param name="posttableid">所以分表的ID</param>
        /// <returns></returns>
        public static int DeleteTopicByTidList(string topicList, string postTableId)
        {
            if (Users.appDBCache && Users.IUserService != null)
                Users.IUserService.UpdateUserPost(postTableId, topicList);
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.DeleteTopicByTidList(topicList);

            return DatabaseProvider.GetInstance().DeleteTopicByTidList(topicList, postTableId, true);
        }

        /// <summary>
        /// 复制主题链接
        /// </summary>
        /// <param name="oldfid"></param>
        /// <param name="topicList"></param>
        /// <returns></returns>
        public static int CopyTopicLink(int oldfid, string topicList)
        {
            int maxtid = 0;//如启用TTCACHE，则要先获取相应MaxTid
            if (Topics.appDBCache && Topics.ITopicService != null)
                maxtid = TypeConverter.ObjectToInt(DatabaseProvider.GetInstance().GetMaxTid().Rows[0][0], 0);

            int result = DatabaseProvider.GetInstance().CopyTopicLink(oldfid, topicList);

            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.CopyTopicLink(maxtid);

            return result;
        }

        /// <summary>
        /// 更新帖子到另一主题
        /// </summary>
        /// <param name="oldtid"></param>
        /// <param name="newtid"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        public static int UpdatePostTidToAnotherTopic(int oldtid, int newtid)
        {
            return DatabaseProvider.GetInstance().UpdatePostTidToAnotherTopic(oldtid, newtid, Data.PostTables.GetPostTableId(newtid));
        }

        /// <summary>
        /// 更新附件到另一主题
        /// </summary>
        /// <param name="oldtid"></param>
        /// <param name="newtid"></param>
        /// <returns></returns>
        public static int UpdateAttachmentTidToAnotherTopic(int oldtid, int tid)
        {
            return DatabaseProvider.GetInstance().UpdateAttachmentTidToAnotherTopic(oldtid, tid);
        }

        /// <summary>
        /// 修复主题
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="posttable"></param>
        /// <returns></returns>
        public static int RepairTopics(string tid, string posttable)
        {
            int result = DatabaseProvider.GetInstance().RepairTopics(tid, posttable);
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.RepairTopics(tid);

            return result;
        }

        /// <summary>
        /// 根据得到给定帖子id的用户列表
        /// </summary>
        /// <param name="postlist">帖子列表</param>
        /// <returns>用户列表</returns>
        public static string GetUserListWithPostlist(int tid, string postList)
        {
            if (!Utils.IsNumericList(postList))
                return "";

            StringBuilder userIdList = new StringBuilder();
            IDataReader reader = DatabaseProvider.GetInstance().GetUserListWithPostList(postList, Data.PostTables.GetPostTableId(tid));
            if (reader != null)
            {
                while (reader.Read())
                {
                    if (!Utils.StrIsNullOrEmpty(userIdList.ToString()))
                    {
                        userIdList.Append(",");
                    }
                    userIdList.Append(reader["posterid"].ToString());
                }
                reader.Close();
            }
            return userIdList.ToString();
        }

        /// <summary>
        /// 检查评分状态
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="pid">帖子id</param>      
        /// <returns></returns>
        public static string CheckRateState(int userid, string pid)
        {
            return DatabaseProvider.GetInstance().CheckRateState(userid, pid);
        }

        /// <summary>
        /// 返回指定主题的最后一次操作
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>管理日志内容</returns>
        public static string GetTopicListModeratorLog(int tid)
        {
            string str = "";
            IDataReader reader = null;

            reader = DatabaseProvider.GetInstance().GetTopicListModeratorLog(tid);
            if (reader != null && reader.Read())
            {
                str = "本主题由 " + reader["grouptitle"].ToString() + " " + reader["moderatorname"].ToString() + " 于 " + reader["postdatetime"].ToString() + " 执行 " + reader["actions"].ToString() + " 操作";
                reader.Close();
            }
            return str;
        }

        /// <summary>
        /// 重设主题类型
        /// </summary>
        /// <param name="topictypeid">主题类型</param>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <returns></returns>
        public static int ResetTopicTypes(int topictypeid, string topiclist)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.ResetTopicTypes(topictypeid, topiclist);

            return DatabaseProvider.GetInstance().ResetTopicTypes(topictypeid, topiclist);
        }

        /// <summary>
        /// 设置主题鉴定信息
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="identify"></param>
        public static void IdentifyTopic(string topiclist, int identify)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.IdentifyTopic(topiclist, identify);

            DatabaseProvider.GetInstance().IdentifyTopic(topiclist, identify);
        }

        /// <summary>
        /// 设置主题的下沉和提升
        /// </summary>
        /// <param name="tidList"></param>
        /// <param name="lastpostid"></param>
        public static void SetTopicsBump(string tidList, int lastpostid)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.SetTopicsBump(tidList, lastpostid);

            DatabaseProvider.GetInstance().SetTopicsBump(tidList, lastpostid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetPostId()
        {
            return DatabaseProvider.GetInstance().GetPostId();
        }

        public static void DeleteClosedTopics(int fid, string topicIdList)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.DeleteClosedTopics(fid, topicIdList);

            DatabaseProvider.GetInstance().DeleteClosedTopics(fid, topicIdList);
        }
    }
}
