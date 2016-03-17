using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Web;

using Discuz.Data;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    public class Stats
    {
        private Stats() { }

        #region Public Methods

        /// <summary>
        /// 更新特定统计
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        public static void UpdateStatVars(string type, string variable, string value)
        {
            Discuz.Data.Stats.UpdateStatVars(type, variable, value);
        }

        /// <summary>
        /// 获取所有统计数据
        /// </summary>
        /// <returns></returns>
        public static List<StatInfo> GetAllStats()
        {
            return Discuz.Data.Stats.GetAllStats();
        }

        /// <summary>
        /// 获取所有统计数据
        /// </summary>
        /// <returns></returns>
        public static List<StatVarInfo> GetAllStatVars()
        {
            return Discuz.Data.Stats.GetAllStatVars();
        }

        /// <summary>
        /// 线程更新统计信息
        /// </summary>
        /// <param name="isguest">是否游客</param>
        public static void UpdateStatCount(bool isguest, bool sessionexists)
        {
            string browser = Utils.GetClientBrower();
            string os = Utils.GetClientOS();
            string visitorsadd = string.Empty;
            if (!sessionexists)
            {
                visitorsadd = "OR ([type]='browser' AND [variable]='" + browser + "') OR ([type]='os' AND [variable]='" + os + "')"
                + (isguest ? " OR ([type]='total' AND [variable]='guests')" : " OR ([type]='total' AND [variable]='members')");
            }
            ProcessStats ps = new ProcessStats(browser, os, visitorsadd);
            ps.Enqueue();
        }

        /// <summary>
        /// 删除过期的日发帖记录
        /// </summary>
        public static void DeleteOldDayposts()
        {
            Discuz.Data.Stats.DeleteOldDayposts();
        }

        /// <summary>
        /// 获得板块总数
        /// </summary>
        /// <returns></returns>
        public static int GetForumCount()
        {
            return Discuz.Data.Stats.GetForumCount();
        }

        /// <summary>
        /// 获得主题总数
        /// </summary>
        /// <returns></returns>
        public static int GetTopicCount()
        {
            return TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("totaltopic"));
        }

        /// <summary>
        /// 获得帖子总数
        /// </summary>
        /// <returns></returns>
        public static int GetPostCount()
        {
            return TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("totalpost"));
        }

        /// <summary>
        /// 获得会员总数
        /// </summary>
        /// <returns></returns>
        public static int GetMemberCount()
        {
            return TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("totalusers"));
        }

        /// <summary>
        /// 获得今日发帖数
        /// </summary>
        /// <returns></returns>
        public static int GetTodayPostCount()
        {
            return Discuz.Data.Stats.GetTodayPostCount(Posts.GetPostTableId());
        }

        /// <summary>
        /// 获得今日新会员数
        /// </summary>
        /// <returns></returns>
        public static int GetTodayNewMemberCount()
        {
            return Discuz.Data.Stats.GetTodayNewMemberCount();
        }

        /// <summary>
        /// 获得管理员数
        /// </summary>
        /// <returns></returns>
        public static int GetAdminCount()
        {
            return Discuz.Data.Stats.GetAdminCount();
        }

        /// <summary>
        /// 获得未发帖会员数
        /// </summary>
        /// <returns></returns>
        public static int GetNonPostMemCount()
        {
            return Discuz.Data.Stats.GetNonPostMemCount();
        }

        /// <summary>
        /// 获得最热论坛
        /// </summary>
        /// <returns></returns>
        public static ForumInfo GetHotForum()
        {
            ForumInfo forum = null;
            int posts = 0;
            foreach (ForumInfo f in Forums.GetForumList())
            {
                if (f.Layer > 0 && f.Status > 0 && f.Posts > posts)
                {
                    posts = f.Posts;
                    forum = f;
                }
            }

            if (posts > 0)
                return forum;

            foreach (ForumInfo f in Forums.GetForumList())
            {
                if (f.Layer > 0 && f.Status > 0)
                    return f;
            }
            return null;
        }

        /// <summary>
        /// 获得今日最佳用户
        /// </summary>
        /// <param name="bestmem"></param>
        /// <param name="bestmemposts"></param>
        public static void GetBestMember(out string bestmem, out int bestmemposts)
        {
            Discuz.Data.Stats.GetBestMember(out bestmem, out bestmemposts, Posts.GetPostTableId());
        }

        /// <summary>
        /// 获得每月发帖统计
        /// </summary>
        /// <param name="monthpostsstats"></param>
        /// <returns></returns>
        public static Hashtable GetMonthPostsStats(Hashtable monthpostsstats)
        {
            return Discuz.Data.Stats.GetMonthPostsStats(monthpostsstats, Posts.GetPostTableId());
        }

        /// <summary>
        /// 获得每日发帖统计
        /// </summary>
        /// <param name="daypostsstats"></param>
        /// <returns></returns>
        public static Hashtable GetDayPostsStats(Hashtable daypostsstats)
        {
            return Discuz.Data.Stats.GetDayPostsStats(daypostsstats, Posts.GetPostTableId());
        }

        /// <summary>
        /// 获得热门主题html
        /// </summary>
        /// <returns></returns>
        public static string GetHotTopicsHtml()
        {
            StringBuilder sbhtml = new StringBuilder();
            foreach (TopicInfo topicInfo in Discuz.Data.Stats.GetHotTopicsList())
            {
                sbhtml.AppendFormat("<li><em>{0}</em><a href=\"{1}\">{2}</a>\r\n",
                    topicInfo.Views,
                    Urls.ShowTopicAspxRewrite(topicInfo.Tid, 0),
                    topicInfo.Title);
            }
            return sbhtml.ToString();
        }

        /// <summary>
        /// 获得热门回复主题html
        /// </summary>
        /// <returns></returns>
        public static string GetHotReplyTopicsHtml()
        {
            StringBuilder sbhtml = new StringBuilder();
            foreach (TopicInfo topicInfo in Discuz.Data.Stats.GetHotReplyTopicsHtml())
            {
                sbhtml.AppendFormat("<li><em>{0}</em><a href=\"{1}\">{2}</a>\r\n",
                    topicInfo.Replies,
                    Urls.ShowTopicAspxRewrite(topicInfo.Tid, 0),
                    topicInfo.Title);
            }
            return sbhtml.ToString();
        }

        /// <summary>
        /// 获得板块列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<ForumInfo> GetForumArray(string type)
        {
            return Discuz.Data.Stats.GetForumArray(type, Posts.GetPostTableId());
        }

        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ShortUserInfo[] GetUserArray(string type)
        {
            return Discuz.Data.Stats.GetUserArray(type, Posts.GetPostTableId());
        }

        /// <summary>
        /// 获得扩展积分排行数组
        /// </summary>
        /// <returns></returns>
        public static ShortUserInfo[][] GetExtsRankUserArray()
        {
            List<ShortUserInfo[]> list = new List<ShortUserInfo[]>();
            string[] score = Scoresets.GetValidScoreName();

            for (int i = 1; i < 9; i++)
            {
                if (score[i] == string.Empty)
                    list.Add(new ShortUserInfo[0]);
                else
                    list.Add(GetUserArray("extcredits" + i.ToString()));
            }

            return list.ToArray();
        }


        public static ShortUserInfo[] GetUserOnlinetime(string field)
        {
            return Discuz.Data.Stats.GetUserOnlinetime(field);
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
            return Data.Stats.GetTrendGraph(graph, field, begin, end, type);
        }
        #endregion

        #region Helper
        /// <summary>
        /// 获得统计数据html通用方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="statht"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string GetStatsDataHtml(string type, Hashtable statht, int max)
        {
            string statsbar = string.Empty;
            int sum = 0;
            ArrayList list;

            if (type == "os" || type == "browser")
            {
                ArrayList al = new ArrayList(statht);

                al.Sort(new Discuz.Data.Stats.StatSorter());
                al.Reverse();
                list = new ArrayList();
                foreach (DictionaryEntry de in al)
                {
                    list.Add(de.Key);
                }
            }
            else
            {
                list = new ArrayList(statht.Keys);
                list.Sort();
            }
            foreach (string key in list)
            {
                sum += Utils.StrToInt(statht[key], 0);
            }

            foreach (string key in list)
            {
                string variable = "";
                int count = 0;
                switch (type)
                {
                    case "month":
                    case "monthposts":
                        if (key.Length < 4)
                            break;
                        variable = key.Substring(0, 4) + "-" + key.Substring(4);
                        break;
                    case "dayposts":
                        variable = key.Substring(0, 4) + "-" + key.Substring(4, 2) + "-" + key.Substring(6);
                        break;
                    case "week":
                        switch (key)
                        {
                            case "0": variable = "星期日"; break;
                            case "1": variable = "星期一"; break;
                            case "2": variable = "星期二"; break;
                            case "3": variable = "星期三"; break;
                            case "4": variable = "星期四"; break;
                            case "5": variable = "星期五"; break;
                            case "6": variable = "星期六"; break;
                            default: continue;
                        }
                        break;
                    case "hour":
                        variable = key;
                        break;
                    default:
                        variable = "<img src='images/stats/" + key.Replace("/", "") + ".gif ' border='0' alt='" + key + "' title='" + key + "' />&nbsp;" + key;
                        break;
                }
                count = Utils.StrToInt(statht[key], 0);
                int width = (int)(370 * (max == 0 ? 0.0 : ((double)count / (double)max)));
                double percent = ((double)Math.Round((double)count * 100 / (double)(sum == 0 ? 1 : sum), 2));
                if (width <= 0)
                    width = 2;
                variable = count == max ? "<strong>" + variable + "</strong>" : variable;
                string countstr = "<div class='optionbar left'><div class='pollcolor5' style='width: " + width.ToString() + "px'>&nbsp;</div></div>&nbsp;<strong>" + count.ToString() + "</strong> (" + percent + "%)";
                statsbar += "<tr><th width=\"100\">" + variable + "</th><td style='text-align:left'>" + countstr + "</td></tr>\r\n";
            }
            return statsbar;
        }

        /// <summary>
        /// 获得板块排行的html
        /// </summary>
        /// <param name="forums"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetForumsRankHtml(List<ForumInfo> forums, string type, int maxrows)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ForumInfo f in forums)
            {
                builder.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", type == "topics" ? f.Topics : f.Posts, Urls.ShowForumAspxRewrite(f.Fid, 0), f.Name);
                maxrows--;
            }
            for (int i = 0; i < maxrows; i++)
            {
                builder.Append("<li>&nbsp;</li>");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获得用户排行的html
        /// </summary>
        /// <param name="users"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUserRankHtml(ShortUserInfo[] users, string type, int maxrows)
        {
            StringBuilder builder = new StringBuilder();
            string unit = "";
            int blankrows = maxrows;
            foreach (ShortUserInfo u in users)
            {
                string count = string.Empty;
                switch (type)
                {
                    case "credits":
                        count = u.Credits.ToString();
                        break;
                    case "extcredits1":
                        count = u.Extcredits1.ToString();
                        unit = Scoresets.GetValidScoreUnit()[1];
                        break;
                    case "extcredits2":
                        count = u.Extcredits2.ToString();
                        unit = Scoresets.GetValidScoreUnit()[2];
                        break;
                    case "extcredits3":
                        count = u.Extcredits3.ToString();
                        unit = Scoresets.GetValidScoreUnit()[3];
                        break;
                    case "extcredits4":
                        count = u.Extcredits4.ToString();
                        unit = Scoresets.GetValidScoreUnit()[4];
                        break;
                    case "extcredits5":
                        count = u.Extcredits5.ToString();
                        unit = Scoresets.GetValidScoreUnit()[5];
                        break;
                    case "extcredits6":
                        count = u.Extcredits6.ToString();
                        unit = Scoresets.GetValidScoreUnit()[6];
                        break;
                    case "extcredits7":
                        count = u.Extcredits7.ToString();
                        unit = Scoresets.GetValidScoreUnit()[7];
                        break;
                    case "extcredits8":
                        count = u.Extcredits8.ToString();
                        unit = Scoresets.GetValidScoreUnit()[8];
                        break;
                    case "digestposts":
                        count = u.Digestposts.ToString();
                        break;
                    case "onlinetime":
                        count = Math.Round(((double)u.Oltime) / 60, 2).ToString();
                        unit = "小时";
                        break;
                    default:
                        count = u.Posts.ToString();
                        break;
                }

                builder.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", count + (unit == string.Empty ? string.Empty : " " + unit), Urls.UserInfoAspxRewrite(u.Uid), u.Username);
                blankrows--;
            }
            for (int i = 0; i < blankrows; i++)
            {
                builder.Append("<li>&nbsp;</li>");
            }
            return builder.ToString();
        }

        #endregion

        private class ProcessStats
        {
            public ProcessStats(string browser, string os, string visitorsadd)
            {
                _browser = browser;
                _os = os;
                _visitorsadd = visitorsadd;
            }
            protected string _browser;
            protected string _os;
            protected string _visitorsadd;

            /// <summary>
            /// 执行统计操作
            /// </summary>
            public void Enqueue()
            {
                ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
            }

            /// <summary>
            /// 处理当前操作
            /// </summary>
            /// <param name="state"></param>
            private void Process(object state)
            {
                Discuz.Data.Stats.UpdateStatCount(this._browser, this._os, this._visitorsadd);
            }
        }
    }
}
