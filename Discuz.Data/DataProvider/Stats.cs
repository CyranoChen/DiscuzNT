using System;
using System.Text;
using System.Data;
using System.Collections;

using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Data
{
    public class Stats
    {
        /// <summary>
        /// 更新特定统计
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        public static void UpdateStatVars(string type, string variable, string value)
        {
            DatabaseProvider.GetInstance().UpdateStatVars(type, variable, value);
        }


        /// <summary>
        /// 获取所有统计数据
        /// </summary>
        /// <returns></returns>
        public static List<StatInfo> GetAllStats()
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetAllStats();
            List<StatInfo> list = new List<StatInfo>();
            while (reader.Read())
            {
                list.Add(LoadSingleStat(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 获取所有统计数据
        /// </summary>
        /// <returns></returns>
        public static List<StatVarInfo> GetAllStatVars()
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetAllStatVars();
            List<StatVarInfo> list = new List<StatVarInfo>();

            while (reader.Read())
            {
                list.Add(LoadSingleStatVar(reader));
            }
            reader.Close();
            return list;
        }


        /// <summary>
        /// 删除过期的日发帖记录
        /// </summary>
        public static void DeleteOldDayposts()
        {
            DatabaseProvider.GetInstance().DeleteOldDayposts();
        }

        /// <summary>
        /// 获得板块总数
        /// </summary>
        /// <returns></returns>
        public static int GetForumCount()
        {
            return DatabaseProvider.GetInstance().GetForumCount();
        }

        /// <summary>
        /// 获得今日发帖数
        /// </summary>
        /// <returns></returns>
        public static int GetTodayPostCount(string postTableId)
        {
            return DatabaseProvider.GetInstance().GetTodayPostCount(postTableId);
        }

        /// <summary>
        /// 获得今日新会员数
        /// </summary>
        /// <returns></returns>
        public static int GetTodayNewMemberCount()
        {
            return DatabaseProvider.GetInstance().GetTodayNewMemberCount();
        }

        /// <summary>
        /// 获得管理员数
        /// </summary>
        /// <returns></returns>
        public static int GetAdminCount()
        {
            return DatabaseProvider.GetInstance().GetAdminCount();
        }

        /// <summary>
        /// 获得未发帖会员数
        /// </summary>
        /// <returns></returns>
        public static int GetNonPostMemCount()
        {
            return DatabaseProvider.GetInstance().GetNonPostMemCount();
        }



        /// <summary>
        /// 获得今日最佳用户
        /// </summary>
        /// <param name="bestmem"></param>
        /// <param name="bestmemposts"></param>
        public static void GetBestMember(out string bestmem, out int bestmemposts, string postTableId)
        {
            bestmem = "";
            bestmemposts = 0;
            IDataReader reader = DatabaseProvider.GetInstance().GetBestMember(postTableId);
            if (reader.Read())
            {
                bestmem = reader["poster"].ToString();
                bestmemposts = Utils.StrToInt(reader["posts"], 0);
            }
            reader.Close();
        }


        /// <summary>
        /// 获得每月发帖统计
        /// </summary>
        /// <param name="monthpostsstats"></param>
        /// <returns></returns>
        public static Hashtable GetMonthPostsStats(Hashtable monthpostsstats, string postTableId)
        {
            Hashtable ht = new Hashtable();
            IDataReader reader = DatabaseProvider.GetInstance().GetMonthPostsStats(postTableId);
            while (reader.Read())
            {
                string key = reader["year"].ToString() + Utils.StrToInt(reader["month"], 1).ToString("00");
                int count = Utils.StrToInt(reader["count"], 0);
                if (!monthpostsstats.ContainsKey(key) || monthpostsstats[key].ToString() != count.ToString())
                {
                    monthpostsstats[key] = count.ToString();
                    ht[key] = count;
                }

            }
            reader.Close();

            foreach (string key in ht.Keys)
            {
                UpdateStatVars("monthposts", key, ht[key].ToString());
            }

            ArrayList list = new ArrayList(monthpostsstats.Values);
            list.Sort(new StatVarSorter());
            monthpostsstats["maxcount"] = list.Count > 0 ? Utils.StrToInt(list[list.Count - 1], 0) : 0;

            return monthpostsstats;
        }

        /// <summary>
        /// 获得每日发帖统计
        /// </summary>
        /// <param name="daypostsstats"></param>
        /// <returns></returns>
        public static Hashtable GetDayPostsStats(Hashtable daypostsstats, string postTableId)
        {
            Hashtable ht = new Hashtable();
            IDataReader reader = DatabaseProvider.GetInstance().GetDayPostsStats(postTableId);
            while (reader.Read())
            {
                string key = reader["year"].ToString() + Utils.StrToInt(reader["month"], 1).ToString("00") + Utils.StrToInt(reader["day"], 1).ToString("00");
                int count = Utils.StrToInt(reader["count"], 0);
                if (!daypostsstats.ContainsKey(key) || daypostsstats[key].ToString() != count.ToString())
                {
                    daypostsstats[key] = count.ToString();
                    ht[key] = count;
                }

            }
            reader.Close();

            foreach (string key in ht.Keys)
            {
                UpdateStatVars("dayposts", key, ht[key].ToString());
            }
            ArrayList list = new ArrayList(daypostsstats.Values);
            list.Sort(new StatVarSorter());
            daypostsstats["maxcount"] = list.Count < 1 ? 0 : Utils.StrToInt(list[list.Count - 1], 0);

            return daypostsstats;
        }


        /// <summary>
        /// 获得热门主题html
        /// </summary>
        /// <returns></returns>
        public static List<TopicInfo> GetHotTopicsList()
        {
            List<TopicInfo> topicInfoList = new List<TopicInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetHotTopics(20);
            while (reader.Read())
            {
                TopicInfo topicInfo = new TopicInfo();
                topicInfo.Views = TypeConverter.ObjectToInt(reader["views"]);
                topicInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
                topicInfo.Title = reader["title"].ToString();
                topicInfoList.Add(topicInfo);
            }
            reader.Close();
            return topicInfoList;
        }

        /// <summary>
        /// 获得热门回复主题html
        /// </summary>
        /// <returns></returns>
        public static List<TopicInfo> GetHotReplyTopicsHtml()
        {
            List<TopicInfo> topicInfoList = new List<TopicInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetHotReplyTopics(20);
            while (reader.Read())
            {
                TopicInfo topicInfo = new TopicInfo();
                topicInfo.Replies = TypeConverter.ObjectToInt(reader["replies"]);
                topicInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
                topicInfo.Title = reader["title"].ToString();
                topicInfoList.Add(topicInfo);
            }
            reader.Close();
            return topicInfoList;
        }


        /// <summary>
        /// 获得板块列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<ForumInfo> GetForumArray(string type, string postTableId)
        {
            IDataReader reader = null;
            switch (type)
            {
                case "topics":
                    reader = DatabaseProvider.GetInstance().GetForumsByTopicCount(20);
                    break;
                case "posts":
                    reader = DatabaseProvider.GetInstance().GetForumsByPostCount(20);
                    break;
                case "thismonth":
                    reader = DatabaseProvider.GetInstance().GetForumsByMonthPostCount(20, postTableId);
                    break;
                case "today":
                    reader = DatabaseProvider.GetInstance().GetForumsByDayPostCount(20, postTableId);
                    break;
            }
            if (reader == null)
                return new List<ForumInfo>();

            List<ForumInfo> list = new List<ForumInfo>();

            while (reader.Read())
            {
                ForumInfo f = new ForumInfo();
                f.Fid = Utils.StrToInt(reader["fid"], 0);
                f.Name = reader["name"].ToString();
                if (type == "topics")
                    f.Topics = Utils.StrToInt(reader["topics"], 0);
                else
                    f.Posts = Utils.StrToInt(reader["posts"], 0);

                list.Add(f);
            }
            reader.Close();

            return list;
        }


        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ShortUserInfo[] GetUserArray(string type, string postTableId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUsersRank(20, postTableId, type);
            if (reader == null)
                return new ShortUserInfo[0];

            List<ShortUserInfo> list = new List<ShortUserInfo>();

            while (reader.Read())
            {
                ShortUserInfo u = new ShortUserInfo();
                u.Username = reader["username"].ToString();
                u.Uid = Utils.StrToInt(reader["uid"], 0);

                switch (type)
                {
                    case "digestposts":
                        u.Digestposts = TypeConverter.ObjectToInt(reader["digestposts"]);
                        break;
                    case "credits":
                        u.Credits = TypeConverter.ObjectToInt(reader["credits"]);
                        break;
                    case "extcredits1":
                        u.Extcredits1 = TypeConverter.ObjectToFloat(reader["extcredits1"]);
                        break;
                    case "extcredits2":
                        u.Extcredits2 = TypeConverter.ObjectToFloat(reader["extcredits2"]);
                        break;
                    case "extcredits3":
                        u.Extcredits3 = TypeConverter.ObjectToFloat(reader["extcredits3"]);
                        break;
                    case "extcredits4":
                        u.Extcredits4 = TypeConverter.ObjectToFloat(reader["extcredits4"]);
                        break;
                    case "extcredits5":
                        u.Extcredits5 = TypeConverter.ObjectToFloat(reader["extcredits5"]);
                        break;
                    case "extcredits6":
                        u.Extcredits6 = TypeConverter.ObjectToFloat(reader["extcredits6"]);
                        break;
                    case "extcredits7":
                        u.Extcredits7 = TypeConverter.ObjectToFloat(reader["extcredits7"]);
                        break;
                    case "extcredits8":
                        u.Extcredits8 = TypeConverter.ObjectToFloat(reader["extcredits8"]);
                        break;
                    case "oltime":
                        u.Oltime = TypeConverter.ObjectToInt(reader["oltime"]);
                        break;
                    default:
                        u.Posts = TypeConverter.ObjectToInt(reader["posts"]);
                        break;
                }

                u.Password = u.Secques = u.Nickname = u.Bday = string.Empty;

                list.Add(u);
            }
            reader.Close();

            return list.ToArray();
        }


        public static ShortUserInfo[] GetUserOnlinetime(string field)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserByOnlineTime(field);

            List<ShortUserInfo> list = new List<ShortUserInfo>();

            while (reader.Read())
            {
                ShortUserInfo u = new ShortUserInfo();
                u.Username = reader["username"].ToString();
                u.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                u.Oltime = TypeConverter.ObjectToInt(reader[field]);
                u.Password = u.Secques = u.Nickname = u.Bday = string.Empty;

                list.Add(u);
            }
            reader.Close();

            return list.ToArray();
        }


        public static void UpdateStatCount(string brower, string os, string visitorsadd)
        {
            DatabaseProvider.GetInstance().UpdateStatCount(brower, os, visitorsadd);
        }


        private static StatInfo LoadSingleStat(IDataReader reader)
        {
            StatInfo statinfo = new StatInfo();
            statinfo.Type = reader["type"].ToString().Trim();
            statinfo.Variable = reader["variable"].ToString().Trim();
            statinfo.Count = TypeConverter.ObjectToInt(reader["count"], 0);
            return statinfo;
        }

        private static StatVarInfo LoadSingleStatVar(IDataReader reader)
        {
            StatVarInfo statvarinfo = new StatVarInfo();
            statvarinfo.Type = reader["type"].ToString().Trim();
            statvarinfo.Variable = reader["variable"].ToString().Trim();
            statvarinfo.Value = reader["value"].ToString().Trim();
            return statvarinfo;
        }

        /// <summary>
        /// Stat排序类
        /// </summary>
        public class StatSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                return (new CaseInsensitiveComparer().Compare(((DictionaryEntry)x).Value, ((DictionaryEntry)y).Value));
            }
        }

        /// <summary>
        /// StatVar排序类
        /// </summary>
        public class StatVarSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                return new CaseInsensitiveComparer().Compare(Utils.StrToInt(x, 0), Utils.StrToInt(y, 0));
            }
        }

        /// <summary>
        /// 获取趋势图形信息
        /// </summary>
        /// <param name="graph">图形信息</param>
        /// <param name="field">读取字段</param>
        /// <param name="begin">起始日期</param>
        /// <param name="end">结束日期</param>
        /// <param name="type">类别</param>
        /// <returns></returns>
        public static string GetTrendGraph(Hashtable graph, string field, string begin, string end, string type)
        {
            StringBuilder xaxis = new StringBuilder(2048);
            IDataReader reader = DatabaseProvider.GetInstance().GetTrendGraph(field, begin, end);
            int count = 1;
            while (reader.Read())
            {
                xaxis.Append(string.Format("<value xid=\"{0}\">{1}</value>", count, reader["daytime"].ToString().Substring(4, 4)));
                if (type == "statistic")
                {
                    if (graph.ContainsKey(type))
                        graph[type] += string.Format("<value xid=\"{0}\">{1}</value>", count, reader[type].ToString());
                    else
                        graph.Add(type, string.Format("<value xid=\"{0}\">{1}</value>", count, reader[type].ToString()));
                }
                else
                {
                    foreach (string t in type.Split(','))
                    {
                        if (graph.ContainsKey(t))
                            graph[t] += string.Format("<value xid=\"{0}\">{1}</value>", count, reader[t].ToString());
                        else
                            graph.Add(t, string.Format("<value xid=\"{0}\">{1}</value>", count, reader[t].ToString()));
                    }
                }
                count++;
            }
            return xaxis.ToString();
        }
    }
}
