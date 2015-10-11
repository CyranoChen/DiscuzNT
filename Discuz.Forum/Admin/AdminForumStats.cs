using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// AdminForumStatFactory 的摘要说明。
    /// 后台论坛统计功能类
    /// </summary>
    public class AdminForumStats
    {
        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public static int GetUserCount()
        {
            return Users.GetUserCount("");
        }


        /// <summary>
        /// 得到论坛中帖子总数;
        /// </summary>
        /// <returns>帖子总数</returns>
        public static int GetPostsCount()
        {
            int postcount = 0;
            foreach (DataRow dr in Discuz.Data.PostTables.GetAllPostTableName().Rows)
            {
                postcount += Data.Posts.GetPostsCount(dr["id"].ToString());
            }
            return postcount;
        }

        /// <summary>
        /// 得到论坛中帖子总数;
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="todaypostcount">输出参数,根据返回指定版块今日发帖总数</param>
        /// <returns>帖子总数</returns>
        public static int GetPostsCountByFid(int fid, out int todaypostcount)
        {
            int postcount = 0;
            todaypostcount = 0;

            ///得到指定版块的最大和最小主题ID
            int maxtid = 0;
            int mintid = 0;
            IDataReader readerGetTid = Data.Forums.GetMaxAndMinTid(fid);
            if (readerGetTid != null)
            {
                if (readerGetTid.Read())
                {
                    maxtid = Utils.StrToInt(readerGetTid["maxtid"], 0);
                    mintid = Utils.StrToInt(readerGetTid["mintid"], 0);
                }
                readerGetTid.Close();
            }
            if (mintid + maxtid == 0)
            {
                //TODO:无主题
                postcount = Data.Posts.GetPostCount(fid, Posts.GetPostTableName());
                todaypostcount = Data.Posts.GetTodayPostCount(fid, Posts.GetPostTableName());
            }
            else
            {
                string[] posttableidA = Posts.GetPostTableIdArray(mintid, maxtid);
                foreach (string posttableid in posttableidA)
                {
                    postcount += Data.Posts.GetPostCount(fid, Data.Posts.GetPostTableName(TypeConverter.StrToInt(posttableid)));
                    todaypostcount += Data.Posts.GetTodayPostCount(fid, Data.Posts.GetPostTableName(TypeConverter.StrToInt(posttableid)));
                }
            }
            return postcount;
        }

        /// <summary>
        /// 返回指定版块的发帖总数
        /// </summary>
        /// <param name="fid">指定的版块id</param>
        /// <returns></returns>
        public static int GetPostsCountByFid(int fid)
        {
            int todaypostcount = 0;
            return GetPostsCountByFid(fid, out todaypostcount);
        }

        /// <summary>
        /// 得到论坛中帖子总数;
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="todaypostcount">输出参数,根据返回指定版块今日发帖总数</param>
        /// <returns>帖子总数</returns>
        public static int GetPostsCountByUid(int uid, out int todaypostcount)
        {
            int postcount = 0;
            todaypostcount = 0;

            ///得到指定版块的最大和最小主题ID
            int maxtid = 0;
            int mintid = 0;
            IDataReader readerGetTid = Data.Topics.GetMaxAndMinTidByUid(uid);
            if (readerGetTid != null)
            {
                if (readerGetTid.Read())
                {
                    maxtid = Utils.StrToInt(readerGetTid["maxtid"], 0);
                    mintid = Utils.StrToInt(readerGetTid["mintid"], 0);
                }
                readerGetTid.Close();
            }

            if (mintid + maxtid == 0)
            {
                postcount = Data.Posts.GetPostCountByUid(uid, Posts.GetPostTableName());
                todaypostcount = Data.Posts.GetTodayPostCountByUid(uid, Posts.GetPostTableName());
            }
            else
            {
                string[] posttableidA = Posts.GetPostTableIdArray(mintid, maxtid);
                foreach (string posttableid in posttableidA)
                {
                    postcount += Data.Posts.GetPostCountByUid(uid, Data.Posts.GetPostTableName(TypeConverter.StrToInt(posttableid)));
                    todaypostcount += Data.Posts.GetTodayPostCountByUid(uid, Data.Posts.GetPostTableName(TypeConverter.StrToInt(posttableid)));
                }
            }
            return postcount;
        }

        /// <summary>
        /// 返回指定版块的发帖总数
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <returns></returns>
        public static int GetPostsCountByUid(int uid)
        {
            int todaypostcount = 0;
            return GetPostsCountByUid(uid, out todaypostcount);
        }


        /// <summary>
        /// 得到论坛中主题总数;
        /// </summary>
        /// <returns>主题总数</returns>
        public static int GetTopicsCount()
        {
            return Discuz.Data.Topics.GetTopicCount();
        }


        /// <summary>
        /// 得到论坛中最后注册的用户ID和用户名
        /// </summary>
        /// <param name="lastuserid">输出参数：最后注册的用户ID</param>
        /// <param name="lastusername">输出参数：最后注册的用户名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public static bool GetLastUserInfo(out string lastuserid, out string lastusername)
        {
            return Discuz.Data.Users.GetLastUserInfo(out lastuserid, out lastusername);
        }

        /// <summary>
        /// 重设论坛统计数据
        /// </summary>
        public static void ReSetStatistic()
        {
            int UserCount = GetUserCount();
            int TopicsCount = GetTopicsCount();
            int PostCount = GetPostsCount();
            string lastuserid;
            string lastusername;

            if (!GetLastUserInfo(out lastuserid, out lastusername))
            {
                lastuserid = "";
                lastusername = "";
            }
            Discuz.Data.Posts.ReSetStatistic(UserCount, TopicsCount, PostCount, lastuserid, lastusername);
        }


        /// <summary>
        /// 重设置用户精华帖数
        /// </summary>
        /// <param name="statcount">要设置的用户数量</param>
        /// <param name="lastuid">输出参数: 最后一个用户ID</param>
        public static void ReSetUserDigestPosts()
        {
            Discuz.Data.Users.ResetUserDigestPosts();
            //if (statcount < 1)
            //{
            //    lastuid = -1;
            //    return;
            //}

            //IDataReader reader = Discuz.Data.Users.GetTopUsers(statcount, lastuid);
            //lastuid = -1;

            //if (reader != null)
            //{
            //    while (reader.Read())
            //    {
            //        lastuid = Utils.StrToInt(reader["uid"], -1);

            //        Discuz.Data.Users.ResetUserDigestPosts(lastuid);

            //    }
            //    reader.Close();
            //}
        }

        /// <summary>
        /// 重设置用户精华帖数
        /// </summary>
        /// <param name="start_uid">开始统计的用户id </param>
        /// <param name="end_uid">结束统计时的用 id </param>
        public static void ReSetUserDigestPosts(int start_uid, int end_uid)
        {
            //IDataReader reader = Discuz.Data.Users.GetUsers(start_uid, end_uid);

            //int current_uid = start_uid;

            //if (reader != null)
            //{
            //    while (reader.Read())
            //    {
            //        current_uid = Utils.StrToInt(reader["uid"], -1);

            //        Discuz.Data.Users.ResetUserDigestPosts(current_uid);
            //    }
            //    reader.Close();
            //}
        }


        /// <summary>
        /// 重设置用户发帖数
        /// </summary>
        /// <param name="statcount">要设置的用户数量</param>
        /// <param name="lastuid">输出参数：最后一个用户ID</param>
        //public static void ReSetUserPosts(int statcount, ref int lastuid)
        //{
        //    if (statcount < 1)
        //    {
        //        lastuid = -1;
        //        return;
        //    }

        //    IDataReader reader = Discuz.Data.Users.GetTopUsers(statcount, lastuid);
        //    lastuid = -1;
        //    if (reader != null)
        //    {
        //        while (reader.Read())
        //        {
        //            lastuid = Utils.StrToInt(reader["uid"], -1);

        //            int postcount = GetPostsCountByUid(lastuid);

        //            Discuz.Data.Users.UpdateUserPostCount(postcount, lastuid);
        //        }
        //        reader.Close();
        //    }
        //}
        /// <summary>
        /// 更新所有用户的帖子数
        /// </summary>
        /// <param name="statcount"></param>
        /// <param name="lastuid"></param>
        public static void ReSetUserPosts(int statcount, ref int lasttableid)
        {
            if (statcount < 1 || lasttableid > TypeConverter.StrToInt(Discuz.Forum.Posts.GetPostTableId()))
            {
                lasttableid = -1;
                return;
            }
            for (int i = 0; i < statcount; i++)
            {
                
                Discuz.Data.Users.UpdateAllUserPostCount(lasttableid);
                lasttableid++;
            }
        }

        /// <summary>
        /// 重建我的帖子
        /// </summary>
        /// <param name="statcount">每次更新分表的数量，默认为1</param>
        /// <param name="lasttableid">更新的分表ID</param>
        public static void UpdateMyPost(int statcount, ref int lasttableid)
        {
            if (statcount < 1 || lasttableid > TypeConverter.StrToInt(Discuz.Forum.Posts.GetPostTableId()))
            {
                lasttableid = -1;
                return;
            }
            for (int i = 0; i < statcount; i++)
            {

                Discuz.Data.Users.UpdateMyPost(lasttableid);
                lasttableid++;
            }
        }

        /// <summary>
        /// 更新所有分表存储过程
        /// </summary>
        /// <param name="statcount">每次更新分表的数量，默认为1</param>
        /// <param name="lasttableid">更新的分表ID</param>
        public static void UpdatePostSP(int statcount, ref int lasttableid)
        {
            if (statcount < 1 || lasttableid > TypeConverter.StrToInt(Discuz.Forum.Posts.GetPostTableId()))
            {
                lasttableid = -1;
                return;
            }
            for (int i = 0; i < statcount; i++)
            {
                Discuz.Data.Databases.UpdatePostSP(lasttableid);
                lasttableid++;
            }
        }

        /// <summary>
        /// 重设置指定uid范围的用户发帖数
        /// </summary>
        /// <param name="start_uid">开始统计的用户id </param>
        /// <param name="end_uid">结束统计时的用 id </param>
        public static void ReSetUserPosts(int start_uid, int end_uid)
        {
            IDataReader reader = Discuz.Data.Users.GetUsers(start_uid, end_uid);
            int current_uid = start_uid;

            if (reader != null)
            {
                while (reader.Read())
                {
                    current_uid = Utils.StrToInt(reader["uid"], -1);

                    int postcount = GetPostsCountByUid(current_uid);

                    Discuz.Data.Users.UpdateUserPostCount(postcount, current_uid);
                }
                reader.Close();
            }
        }


        /// <summary>
        /// 重建主题最后回复等信息
        /// </summary>
        /// <param name="statcount">每次请求要更新的分表数量</param>
        /// <param name="lasttid">输出参数：分表id</param>
        public static void ReSetTopicPosts(int statcount, ref int lasttableid)
        {
            if (statcount < 1 || lasttableid > TypeConverter.StrToInt(Discuz.Forum.Posts.GetPostTableId()))
            {
                lasttableid = -1;
                return;
            }
            for (int i = 0; i < statcount; i++)
            {
                Data.Posts.ResetLastRepliesInfoOfTopics(lasttableid);
                lasttableid++;
            }
            //IDataReader reader = Discuz.Data.Topics.GetTopicTids(statcount, lasttid);
            //lasttid = -1;

            //if (reader != null)
            //{
            //    int postcount = 0;

            //    string posttableid = "";
            //    while (reader.Read())
            //    {
            //        lasttid = Utils.StrToInt(reader["tid"], -1);
            //        posttableid = Posts.GetPostTableId(lasttid);

            //        postcount = Data.Posts.GetPostCount(lasttid);

            //        IDataReader readerTopic = Data.Posts.GetLastPostByFid(lasttid, BaseConfigs.GetTablePrefix + "posts" + posttableid);
            //        if (readerTopic != null)
            //        {
            //            if (readerTopic.Read())
            //            {
            //                if (Utils.StrToInt(readerTopic["pid"], 0) != 0)
            //                {
            //                    Discuz.Data.Topics.UpdateTopic(lasttid, postcount, Utils.StrToInt(readerTopic["pid"], 0), readerTopic["postdatetime"].ToString(), Utils.StrToInt(readerTopic["posterid"], 0), readerTopic["poster"].ToString());
            //                }
            //                else
            //                {
            //                    Discuz.Data.Topics.UpdateTopicLastPosterId(lasttid);
            //                }
            //            }
            //            readerTopic.Close();
            //        }
            //    }
            //    reader.Close();
            //}
        }

        /// <summary>
        /// 根据分表名更新主题最后回复等信息
        /// </summary>
        /// <param name="postTableName">分表名</param>
        public static void ResetLastRepliesInfoOfTopics(int start_tid, int end_tid)
        {
            for (int i = start_tid; i <= end_tid; i++)
            {
                Data.Posts.ResetLastRepliesInfoOfTopics(i);
            }

        }

        /// <summary>
        /// 重建主题帖数
        /// </summary>
        /// <param name="start_tid">开始统计的主题id </param>
        /// <param name="end_tid">结束统计时的主题id </param>
        public static void ReSetTopicPosts(int start_tid, int end_tid)
        {
            IDataReader reader = Discuz.Data.Topics.GetTopics(start_tid, end_tid);
            int current_tid = start_tid;

            if (reader != null)
            {
                int postcount = 0;
                string posttableid = "";
                while (reader.Read())
                {
                    current_tid = Utils.StrToInt(reader["tid"], -1);
                    posttableid = Posts.GetPostTableId(current_tid);

                    postcount = Data.Posts.GetPostCount(current_tid);

                    //IDataReader readerTopic = Data.Posts.GetLastPostByFid(current_tid, BaseConfigs.GetTablePrefix + "posts" + posttableid);
                    PostInfo post = Data.Posts.GetLastPostByTid(current_tid, BaseConfigs.GetTablePrefix + "posts" + posttableid);
                    if (post != null)
                    {
                        if (Utils.StrToInt(post.Pid, 0) != 0)
                        {
                            Discuz.Data.Topics.UpdateTopic(current_tid, postcount, Utils.StrToInt(post.Pid, 0), post.Postdatetime.ToString(), Utils.StrToInt(post.Posterid, 0), post.Poster.ToString());
                        }
                        else
                        {
                            Discuz.Data.Topics.UpdateTopicLastPosterId(current_tid);
                        }
                    }
                }
                reader.Close();
            }
        }


        /// <summary>
        /// 重建论坛全部帖数
        /// </summary>
        public static void ReSetFourmTopicAPost()
        {
            Discuz.Data.Forums.ResetForumsPosts();
        }


        /// <summary>
        /// 重建论坛帖数
        /// </summary>
        /// <param name="start_fid">要设置的起始版块</param>
        /// <param name="end_fid">要设置的终止版块</param>
        public static void ReSetFourmTopicAPost(int start_fid, int end_fid)//重建指定范围论坛帖数
        {
            IDataReader reader = Discuz.Data.Forums.GetForums(start_fid, end_fid);
            int current_fid = start_fid;
            ReSetFourmTopicPost(reader, ref current_fid, true);
        }

        /// <summary>
        /// 获取当前版块及子版块fid列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        private static string SubForumList(int fid)
        {
            string subfidList = fid.ToString() + ",";
            foreach (ForumInfo forumInfo in Forums.GetForumList())
            {
                if (("," + forumInfo.Parentidlist + ",").IndexOf("," + fid + ",") >= 0)
                    subfidList += forumInfo.Fid + ",";
            }
            return subfidList.TrimEnd(',');
        }

        private static void ReSetFourmTopicPost(IDataReader reader, ref int fid, bool fixTopicCount)
        {
            if (reader != null)
            {
                int topiccount = 0;
                int postcount = 0;
                int todaypostcount = 0;
                int lasttid = 0;
                string lasttitle = "";
                string lastpost = "1900-1-1";
                int lastposterid = 0;
                string lastposter = "";

                while (reader.Read())
                {
                    fid = Utils.StrToInt(reader["fid"], -1);
                    topiccount = Data.Topics.GetTopicCountOfForumWithSub(SubForumList(fid));
                    postcount = GetPostsCountByFid(fid, out todaypostcount);
                    if (fixTopicCount)
                    {
                        Forums.SetRealCurrentTopics(fid);
                    }
                    else
                    {

                        lasttid = 0;
                        lasttitle = "";
                        lastpost = "1900-1-1";
                        lastposterid = 0;
                        lastposter = "";
                    }
                    IDataReader postreader = Discuz.Data.Posts.GetForumLastPost(fid, Posts.GetPostTableName(), topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
                    if (postreader.Read())
                    {
                        TopicInfo topic = Topics.GetTopicInfo(TypeConverter.ObjectToInt(postreader["tid"]));
                        if (topic == null)
                            continue;
                        lasttid = topic.Tid;
                        lasttitle = topic.Title;//postreader["title"].ToString();
                        lastpost = postreader["postdatetime"].ToString();
                        lastposterid = Utils.StrToInt(postreader["posterid"], 0);
                        lastposter = postreader["poster"].ToString();
                    }
                    postreader.Close();

                    Discuz.Data.Forums.UpdateForum(fid, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
                }
                reader.Close();
            }
        }

        /// <summary>
        /// 重建指定论坛帖数
        /// </summary>
        public static void ReSetFourmTopicAPost(int fid)
        {
            if (fid < 1)
                return;

            int topiccount = 0;
            int postcount = 0;
            int lasttid = 0;
            string lasttitle = "";
            string lastpost = "1900-1-1";
            int lastposterid = 0;
            string lastposter = "";
            int todaypostcount = 0;
            topiccount = Data.Topics.GetTopicCountOfForumWithSub(SubForumList(fid));

            postcount = GetPostsCountByFid(fid, out todaypostcount);

            IDataReader postreader = Data.Posts.GetLastPostByFid(fid, Posts.GetPostTableName());

            if (postreader.Read())
            {
                lasttid = Utils.StrToInt(postreader["tid"], 0);
                lasttitle = Topics.GetTopicInfo(lasttid).Title;//postreader["title"].ToString();
                lastpost = postreader["postdatetime"].ToString();
                lastposterid = Utils.StrToInt(postreader["posterid"], 0);
                lastposter = postreader["poster"].ToString();
            }

            postreader.Close();

            Discuz.Data.Forums.UpdateForum(fid, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        }

        /// <summary>
        /// 重建指定论坛帖数
        /// </summary>
        public static void ReSetFourmTopicAPost(int fid, out int topiccount, out int postcount, out int lasttid, out string lasttitle, out string lastpost, out int lastposterid, out string lastposter, out int todaypostcount)
        {
            topiccount = 0;
            postcount = 0;
            lasttid = 0;
            lasttitle = "";
            lastpost = "";
            lastposterid = 0;
            lastposter = "";
            todaypostcount = 0;
            if (fid < 1)
                return;

            topiccount = Data.Topics.GetTopicCountOfForumWithSub(SubForumList(fid));
            postcount = GetPostsCountByFid(fid, out todaypostcount);

            IDataReader postreader = Data.Posts.GetLastPostByFid(fid, Posts.GetPostTableName());
            if (postreader.Read())
            {
                lasttid = Utils.StrToInt(postreader["tid"], 0);
                lasttitle = Topics.GetTopicInfo(lasttid).Title;//postreader["title"].ToString();
                lastpost = postreader["postdatetime"].ToString();
                lastposterid = Utils.StrToInt(postreader["posterid"], 0);
                lastposter = postreader["poster"].ToString();
            }

            postreader.Close();
        }


        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        public static void ReSetClearMove()
        {
            Discuz.Data.Forums.ReSetClearMove();
        }
    }
}