using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Data
{
    public class Debates
    {
        /// <summary>
        /// 增加辩论主题扩展信息
        /// </summary>
        /// <param name="debatetopic"></param>
        public static void CreateDebateTopic(DebateInfo debateInfo)
        {
            //debatetopic = ReviseDebateTopicColor(debatetopic);
            DatabaseProvider.GetInstance().CreateDebateTopic(debateInfo);
        }

        /// <summary>
        /// 获取帖子观点
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>Dictionary泛型</returns>
        public static Dictionary<int, int> GetPostDebateList(int tid)
        {            
            IDataReader reader = DatabaseProvider.GetInstance().GetPostDebate(tid);
            Dictionary<int, int> debateList = new Dictionary<int, int>();

            while (reader.Read())
            {
                debateList.Add(TypeConverter.ObjectToInt(reader["pid"], 0), TypeConverter.ObjectToInt(reader["opinion"], 0));
            }
            reader.Close();
            return debateList;
        }


        /// <summary>
        /// 获取辩论的扩展信息
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>辩论主题扩展信息</returns>
        public static DebateInfo GetDebateTopic(int tid)
        {            
            IDataReader debatetopic = DatabaseProvider.GetInstance().GetDebateTopic(tid);
            DebateInfo topicexpand = new DebateInfo();
            if (debatetopic.Read())
            {
                topicexpand.Positiveopinion = debatetopic["positiveopinion"].ToString();
                topicexpand.Negativeopinion = debatetopic["negativeopinion"].ToString();
                topicexpand.Terminaltime = DateTime.Parse(debatetopic["terminaltime"].ToString());
                topicexpand.Positivediggs = TypeConverter.ObjectToInt(debatetopic["positivediggs"]);
                topicexpand.Negativediggs = TypeConverter.ObjectToInt(debatetopic["negativediggs"]);
                topicexpand.Tid = tid;
            }
            debatetopic.Close();
            return topicexpand;
        }

        /// <summary>
        /// 更新辩论信息
        /// </summary>
        /// <param name="debateInfo">辩论信息</param>
        /// <returns></returns>
        public static bool UpdateDebateTopic(DebateInfo debateInfo)
        {
            return DatabaseProvider.GetInstance().UpdateDebateTopic(debateInfo);
        }


        /// <summary>
        /// 添加点评
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="message">点评内容</param>
        public static void CommentDabetas(int tid, int postTableId, string message)
        {
            DatabaseProvider.GetInstance().AddCommentDabetas(tid, postTableId, message);
        }


        /// <summary>
        /// 增加Digg
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="type">正反方观点</param>
        /// <param name="userinfo">用户信息</param>
        public static void AddDebateDigg(int tid, int pid, int type, string ip, UserInfo userinfo)
        {
            DatabaseProvider.GetInstance().AddDebateDigg(tid, pid, type, ip, userinfo);
        }

        /// <summary>
        /// 返回辩论主题的帖子一方的帖子数
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="debateOpinion">帖子观点</param>
        /// <returns>帖子数</returns>
        public static int GetDebatesPostCount(int tid, int debateOpinion)
        {
            return DatabaseProvider.GetInstance().GetDebatesPostCount(tid, debateOpinion);
        }

        /// <summary>
        /// 返回帖子被顶数
        /// </summary>
        /// <param name="pidlist">帖子ID数组</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<int, int> GetPostDiggs(string pidlist)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            IDataReader reader = DatabaseProvider.GetInstance().GetDebatePostDiggs(pidlist);
            if (reader != null)
            {
                while (reader.Read())
                {
                    result[TypeConverter.ObjectToInt(reader["pid"])] = TypeConverter.ObjectToInt(reader["diggs"]);
                }
                reader.Close();
            }

            return result;
        }

        /// <summary>
        /// 得到用户已顶过帖子PID
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>用户已顶过帖子PID</returns>
        public static string GetUesrDiggs(int tid, int uid)
        {
            StringBuilder sb = new StringBuilder();
            IDataReader reader = DatabaseProvider.GetInstance().GetUserDiggs(tid, uid);
            while (reader.Read())
            {
                sb.Append("|");
                sb.Append(reader["pid"].ToString());
            }
            reader.Close();
            return sb.ToString();
        }


        /// <summary>
        /// 获取最热辩论的JSON数据
        /// </summary>
        /// <param name="hotfield">按照用户指定方式来获取热帖，</param>
        /// <param name="defhotcount">按照用户指定方式，并且要大于等于用户指定的数量</param>
        /// <param name="getcount">获取热帖的条数</param>
        /// <returns></returns>
        public static string GetDebatesJsonList(string callback, string hotfield, int defhotcount, int getcount)
        {
            return DebateTopicJson(callback, DatabaseProvider.GetInstance().GetHotDebatesList(hotfield, defhotcount, getcount));
        }

        /// <summary>
        /// 获取推荐辩论帖JSON数据
        /// </summary>
        /// <param name="callback">回调函数名称</param>
        /// <param name="tidList">主题ID列表</param>
        /// <returns></returns>
        public static string GetRecommendDebates(string callback, string tidList)
        {
            return DebateTopicJson(callback, DatabaseProvider.GetInstance().GetRecommendDebates(tidList));           
        }

        //TODO:应返回集合后用json序列化
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback">回调函数名称</param>
        /// <param name="reader">生成JSON的原始数据</param>
        /// <returns></returns>
        private static string DebateTopicJson(string callback, IDataReader reader)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(callback);
            sb.Append("([");

            while (reader.Read())
            {
                sb.Append(string.Format("{{'title':'{0}','tid','{1}'}},", reader["title"].ToString(), reader["tid"].ToString()));
            }
            reader.Close();

            if (sb.ToString().IndexOf("title") < 0)
                return "0";

            return sb.ToString().Remove(sb.Length - 1) + "])";
        }

        /// <summary>
        /// 创建参加辩论帖子的扩展信息
        /// </summary>
        /// <param name="dpei">参与辩论的帖子的辩论相关扩展字段实体</param>
        public static void CreateDebateExpandInfo(DebatePostExpandInfo dpei)
        {
            DatabaseProvider.GetInstance().CreateDebatePostExpand(dpei);
        }

        /// <summary>
        /// 返回辩论帖子列表
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="debateOpinion">辩论正反观点</param>
        /// <param name="postOrderType">排序类型</param>
        /// <returns></returns>
        public static List<ShowtopicPagePostInfo> GetDebatePostList(PostpramsInfo postpramsInfo, int debateOpinion, PostOrderType postOrderType)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetDebatePostList(postpramsInfo.Tid, debateOpinion, postpramsInfo.Pagesize, postpramsInfo.Pageindex, PostTables.GetPostTableId(postpramsInfo.Tid), postOrderType);
            return Posts.LoadPostList(postpramsInfo, reader);
        }

        public static int GetRealDebatePostCount(int tid, int debateOpinion)
        {
            return DatabaseProvider.GetInstance().ReviseDebateTopicDiggs(tid, debateOpinion);
        }

        /// <summary>
        /// 删除辩论帖子信息
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="opiniontext">正反方字段，positivediggs：正文 negativediggs：反方</param>
        /// <param name="pid">帖子Id</param>
        public static void DeleteDebatePost(int tid, string opiniontext, int pid)
        {
            Discuz.Data.DatabaseProvider.GetInstance().DeleteDebatePost(tid, opiniontext, pid);
        }
    }
}
