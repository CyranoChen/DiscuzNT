using System;
using System.Collections;
using System.Web;
using Discuz.Web.UI;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Common;
using System.Text;

namespace Discuz.Web
{
    /// <summary>
    /// 标签列表页
    /// </summary>
    public class stats : PageBase
    {
        #region Fields
        //public Dictionary<string, int> totalstats = new Dictionary<string,int>();
        /// <summary>
        /// 总体统计
        /// </summary>
        public Hashtable totalstats = new Hashtable();
        /// <summary>
        /// 操作系统统计
        /// </summary>
        public Hashtable osstats = new Hashtable();
        /// <summary>
        /// 浏览器统计
        /// </summary>
        public Hashtable browserstats = new Hashtable();
        /// <summary>
        /// 月统计
        /// </summary>
        public Hashtable monthstats = new Hashtable();
        /// <summary>
        /// 周统计
        /// </summary>
        public Hashtable weekstats = new Hashtable();
        /// <summary>
        /// 小时统计
        /// </summary>
        public Hashtable hourstats = new Hashtable();
        /// <summary>
        /// （总统计）
        /// </summary>
        public Hashtable mainstats = new Hashtable();
        /// <summary>
        /// 日发帖统计
        /// </summary>
        public Hashtable daypostsstats = new Hashtable();
        /// <summary>
        /// 月发帖统计
        /// </summary>
        public Hashtable monthpostsstats = new Hashtable();
        /// <summary>
        /// 板块排行统计
        /// </summary>
        public Hashtable forumsrankstats = new Hashtable();
        /// <summary>
        /// 在线统计
        /// </summary>
        public Hashtable onlinesstats = new Hashtable();
        /// <summary>
        /// 帖子排行统计
        /// </summary>
        public Hashtable postsrankstats = new Hashtable();
        /// <summary>
        /// 管理团队统计
        /// </summary>
        public Hashtable teamstats = new Hashtable();
        /// <summary>
        /// 积分排行统计
        /// </summary>
        public Hashtable creditsrankstats = new Hashtable();
        /// <summary>
        /// 交易统计
        /// </summary>
        public Hashtable tradestats = new Hashtable();
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string lastupdate = "";
        /// <summary>
        /// 下次更新时间
        /// </summary>
        public string nextupdate = "";
        /// <summary>
        /// 统计类型
        /// </summary>
        public string type = "";
        /// <summary>
        /// 统计Flash参数
        /// </summary>
        public string statuspara = "";
        /// <summary>
        /// 统计起始日期
        /// </summary>
        public string primarybegin = DNTRequest.GetString("primarybegin") == "" ? DateTime.Now.AddMonths(-1).AddDays(1).ToString("yyyy-MM-dd") : DNTRequest.GetString("primarybegin");
        /// <summary>
        /// 统计结束日期
        /// </summary>
        public string primaryend = DNTRequest.GetString("primaryend") == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DNTRequest.GetString("primaryend");

        #region Main
        /// <summary>
        /// 注册会员
        /// </summary>
        public int members;
        /// <summary>
        /// 发帖会员
        /// </summary>
        public int mempost;
        /// <summary>
        /// 管理成员
        /// </summary>
        public string admins;
        /// <summary>
        /// 未发帖会员
        /// </summary>
        public int memnonpost;
        /// <summary>
        /// 新会员
        /// </summary>
        public string lastmember;
        /// <summary>
        /// 发帖会员占总数
        /// </summary>
        public double mempostpercent;
        /// <summary>
        /// 今日论坛之星
        /// </summary>
        public string bestmem;
        /// <summary>
        /// 最佳帖
        /// </summary>
        public int bestmemposts;
        /// <summary>
        /// 板块数
        /// </summary>
        public int forums;
        /// <summary>
        /// 平均每人发帖数
        /// </summary>
        public double mempostavg;
        /// <summary>
        /// 平均每日新增帖子数
        /// </summary>
        public double postsaddavg;
        /// <summary>
        /// 平均每日注册会员数
        /// </summary>
        public double membersaddavg;
        /// <summary>
        /// 平均每个主题被回复次数
        /// </summary>
        public double topicreplyavg;
        /// <summary>
        /// 平均PV
        /// </summary>
        public double pageviewavg;
        /// <summary>
        /// 最热门版块
        /// </summary>
        public ForumInfo hotforum;
        /// <summary>
        /// 主题数
        /// </summary>
        public int topics;
        /// <summary>
        /// 帖子数
        /// </summary>
        public int posts;
        /// <summary>
        /// 最近24小时新增帖子数
        /// </summary>
        public string postsaddtoday;
        /// <summary>
        /// 今日新增会员数
        /// </summary>
        public string membersaddtoday;
        /// <summary>
        /// 
        /// </summary>
        public string activeindex;
        /// <summary>
        /// 是否开启页面计数
        /// </summary>
        public bool statstatus;
        /// <summary>
        /// 月发帖量
        /// </summary>
        public string monthpostsofstatsbar = "";
        /// <summary>
        /// 日发帖量
        /// </summary>
        public string daypostsofstatsbar = "";
        /// <summary>
        /// 月份
        /// </summary>
        public string monthofstatsbar = "";
        /// <summary>
        /// 运行时间
        /// </summary>
        public int runtime;
        #endregion

        #region Views
        public string weekofstatsbar = string.Empty;
        public string hourofstatsbar = string.Empty;
        #endregion

        #region Client
        public string browserofstatsbar = string.Empty;
        public string osofstatsbar = string.Empty;
        #endregion

        #region TopicsRank
        public string hotreplytopics;
        public string hottopics;
        #endregion

        #region PostsRank
        public string postsrank;
        public string digestpostsrank;
        public string thismonthpostsrank;
        public string todaypostsrank;
        #endregion

        #region ForumsRank
        public string topicsforumsrank;
        public string postsforumsrank;
        public string thismonthforumsrank;
        public string todayforumsrank;
        #endregion

        #region CreditsRank
        public string[] score;
        public string creditsrank;
        public string extcreditsrank1;
        public string extcreditsrank2;
        public string extcreditsrank3;
        public string extcreditsrank4;
        public string extcreditsrank5;
        public string extcreditsrank6;
        public string extcreditsrank7;
        public string extcreditsrank8;
        #endregion

        #region OnlineRank
        public string totalonlinerank;
        public string thismonthonlinerank;
        #endregion


        public int maxos = 0;
        public int maxbrowser = 0;
        public int maxmonth = 0;
        public int yearofmaxmonth = 0;
        public int monthofmaxmonth = 0;
        public int maxweek = 0;
        public string dayofmaxweek;
        public int maxhour = 0;
        public int maxhourfrom = 0;
        public int maxhourto = 0;

        public int maxmonthposts = 0;
        public int maxdayposts = 0;

        public int statscachelife = 120;

        Dictionary<string, string> statvars = new Dictionary<string, string>();
        #endregion

        public bool needlogin = false;

        protected override void ShowPage()
        {
            pagetitle = "统计";
            if (usergroupinfo.Allowviewstats == 0)
            {
                if (!(DNTRequest.GetString("type") == "trend" && DNTRequest.GetString("xml") == "1"))
                {
                    AddErrLine("您所在的用户组 ( <b>" + usergroupinfo.Grouptitle + "</b> ) 没有查看统计信息的权限");
                    needlogin = (userid < 1);
                    return;
                }
            }

            //判断权限 
            statscachelife = (statscachelife <= 0) ? statscachelife : config.Statscachelife;//
            List<StatInfo> stats = Stats.GetAllStats();
            statstatus = config.Statstatus == 1;

            totalstats["hits"] = 0;
            totalstats["maxmonth"] = 0;
            totalstats["guests"] = 0;
            totalstats["visitors"] = 0;


            foreach (StatInfo stat in stats)
            {
                switch (stat.Type)
                {
                    case "total":
                        SetValue(stat, totalstats);
                        break;
                    case "os":
                        SetValue(stat, osstats);
                        maxos = (stat.Count > maxos) ? stat.Count : maxos;//
                        break;
                    case "browser":
                        SetValue(stat, browserstats);
                        maxbrowser = (stat.Count > maxbrowser) ? stat.Count : maxbrowser;//
                        break;
                    case "month":
                        SetValue(stat, monthstats);
                        if (stat.Count > maxmonth)
                        {
                            maxmonth = stat.Count;
                            yearofmaxmonth = Utils.StrToInt(stat.Variable, 0) / 100;
                            monthofmaxmonth = Utils.StrToInt(stat.Variable, 0) - yearofmaxmonth * 100;
                        }
                        break;
                    case "week":
                        SetValue(stat, weekstats);
                        if (stat.Count > maxweek)
                        {
                            maxweek = stat.Count;
                            dayofmaxweek = stat.Variable;
                        }
                        break;
                    case "hour":
                        SetValue(stat, hourstats);
                        if (stat.Count > maxhour)
                        {
                            maxhour = stat.Count;
                            maxhourfrom = Utils.StrToInt(stat.Variable, 0);
                            maxhourto = maxhourfrom + 1;
                        }
                        break;
                }
            }

            List<StatVarInfo> statvars = Stats.GetAllStatVars();
            foreach (StatVarInfo statvar in statvars)
            {
                if (statvar.Variable == "lastupdate" && Utils.IsNumeric(statvar.Value))
                    continue;
                switch (statvar.Type)
                {
                    case "dayposts":
                        SetValue(statvar, daypostsstats);
                        break;
                    case "creditsrank":
                        SetValue(statvar, creditsrankstats);
                        break;
                    case "forumsrank":
                        SetValue(statvar, forumsrankstats);
                        break;
                    case "postsrank":
                        SetValue(statvar, postsrankstats);
                        break;
                    case "main":
                        SetValue(statvar, mainstats);
                        break;
                    case "monthposts":
                        SetValue(statvar, monthpostsstats);
                        break;
                    case "onlines":
                        SetValue(statvar, onlinesstats);
                        break;
                    case "team":
                        SetValue(statvar, teamstats);
                        break;
                    case "trade":
                        SetValue(statvar, tradestats);
                        break;
                }
            }

            type = DNTRequest.GetString("type");

            if ((type == "" && !statstatus) || type == "posts")
            {
                Stats.DeleteOldDayposts();

                //monthposts
                monthpostsstats = Stats.GetMonthPostsStats(monthpostsstats);
                maxmonthposts = (int)monthpostsstats["maxcount"];
                monthpostsstats.Remove("maxcount");
                //dayposts
                daypostsstats = Stats.GetDayPostsStats(daypostsstats);
                maxdayposts = (int)daypostsstats["maxcount"];
                daypostsstats.Remove("maxcount");
            }

            #region 选择统计类型
            switch (type)
            {
                case "views":
                    GetViews();
                    break;
                case "client":
                    GetClient();
                    break;
                case "posts":
                    GetPosts();
                    break;
                case "forumsrank":
                    GetForumsRank();
                    break;
                case "topicsrank":
                    GetTopicsRank();
                    break;
                case "postsrank":
                    GetPostsRank();
                    break;
                case "creditsrank":
                    GetCreditsRank();
                    break;
                case "trade":
                    GetTrade();
                    break;
                case "onlinetime":
                    GetOnlinetime();
                    break;
                case "team":
                    GetTeam();
                    break;
                case "modworks":
                    GetModWorks();
                    break;
                case "trend":
                    GetTrend();
                    break;
                case "":
                    Default();
                    break;
                default:
                    AddErrLine("未定义操作请返回");
                    SetShowBackLink(false);
                    return;
            #endregion
            }
        }

        /// <summary>
        /// 统计趋势
        /// </summary>
        private void GetTrend()
        {
            string stat_hash = Utils.MD5(config.Passwordkey + "\t" + DateTime.Now.Day);
            if (stat_hash != DNTRequest.GetString("hash") && DNTRequest.GetString("xml") != "")
            {
                AddErrLine("参数验证不正确");
                return;
            }
            if (!Utils.IsDateString(primarybegin))
            {
                AddErrLine("起始日期格式不正确");
                return;
            }
            if (!Utils.IsDateString(primaryend))
            {
                AddErrLine("结束日期格式不正确");
                return;
            }

            string types = DNTRequest.GetString("types") == "" ? "all" : DNTRequest.GetString("types");
            string trendtype = types == "all" ? "login,register,topic,poll,bonus,debate,post" : types;
            foreach (string t in trendtype.Split(','))
            {
                if (!Utils.InArray(t, "all,login,register,topic,poll,bonus,debate,post"))
                {
                    AddErrLine("参数验证不正确");
                    return;
                }
            }
            DateTime primarybeginDate = TypeConverter.StrToDateTime(primarybegin,DateTime.Now.AddMonths(-1).AddDays(1));
            DateTime primaryendDate = TypeConverter.StrToDateTime(primaryend);
            if (primarybeginDate > primaryendDate)
            {
                AddErrLine("统计开始日期不能小于结束日期");
                return;
            }
            primarybegin = primarybeginDate.ToString("yyyy-MM-dd");
            primaryend = primaryendDate.ToString("yyyy-MM-dd");
            if (DNTRequest.GetString("xml") != "")
            {
                string xaxis = "";
                Hashtable graph = new Hashtable();
                string field = "*";
                if (DNTRequest.GetString("merge") == "1")
                {
                    field = "[daytime],[" + trendtype.Replace(",", "]+[") + "] AS [statistic]";
                    trendtype = "statistic";
                }
                Hashtable ks = new Hashtable();
                ks.Add("login", "登录用户");
                ks.Add("register", "新注册用户");
                ks.Add("topic", "主题");
                ks.Add("poll", "投票");
                ks.Add("bonus", "悬赏");
                ks.Add("debate", "辩论");
                ks.Add("post", "主题回帖");
                xaxis = Stats.GetTrendGraph(graph, field, primarybeginDate.ToString("yyyyMMdd"), primaryendDate.ToString("yyyyMMdd"), trendtype);
                StringBuilder xml = new StringBuilder(2048);
                xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xml.AppendFormat("<chart><xaxis>{0}</xaxis><graphs>", xaxis);
                int count = 0;
                foreach (string key in graph.Keys)
                {
                    xml.AppendFormat("<graph gid=\"{0}\" title=\"{1}\">{2}</graph>", count, ks[key], graph[key]);
                    count++;
                }
                xml.Append("</graphs></chart>");
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Expires = -1;
                HttpContext.Current.Response.ContentType = "application/xml";
                HttpContext.Current.Response.Write(xml.ToString());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }
            statuspara = "path=&settings_file=config/stat_setting.xml&data_file=" + Utils.UrlEncode("stats.aspx?type=trend&xml=1&types=" + types + "&primarybegin=" + primarybegin + "&primaryend=" + primaryend + "&merge=" + DNTRequest.GetInt("merge",0) + "&hash=" + stat_hash);
        }


        #region Helper

        private void SetValue(StatInfo stat, Hashtable ht)
        {
            ht[stat.Variable] = stat.Count;
        }

        private void SetValue(StatVarInfo statvar, Hashtable ht)
        {
            ht[statvar.Variable] = statvar.Value;
        }

        #endregion

        /// <summary>
        /// 基本状况
        /// </summary>
        private void Default()
        {
            lastmember = Statistics.GetStatisticsRowItem("lastusername");
            foreach (string key in mainstats.Keys)
            {
                statvars[key] = mainstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = Utils.GetDateTime();
                Stats.UpdateStatVars("main", "lastupdate", statvars["lastupdate"]);
            }

            forums = Stats.GetForumCount();
            topics = Stats.GetTopicCount();
            posts = Stats.GetPostCount();
            members = Stats.GetMemberCount();

            //运行时间 从第一帖到现在
            if (statvars.ContainsKey("runtime"))
                runtime = Utils.StrToInt(statvars["runtime"], 0);
            else
            {
                runtime = (DateTime.Now - Convert.ToDateTime(monthpostsstats["starttime"])).Days;
                Stats.UpdateStatVars("main", "runtime", runtime.ToString());
            }

            //今日新增帖数
            if (statvars.ContainsKey("postsaddtoday"))
                postsaddtoday = statvars["postsaddtoday"];
            else
            {
                postsaddtoday = Stats.GetTodayPostCount().ToString();
                Stats.UpdateStatVars("main", "postsaddtoday", postsaddtoday);
            }

            //今日新增会员数
            if (statvars.ContainsKey("membersaddtoday"))
                membersaddtoday = statvars["membersaddtoday"];
            else
            {
                membersaddtoday = Stats.GetTodayNewMemberCount().ToString();
                Stats.UpdateStatVars("main", "membersaddtoday", membersaddtoday);
            }

            //管理人员数
            if (statvars.ContainsKey("admins"))
                admins = statvars["admins"];
            else
            {
                admins = Stats.GetAdminCount().ToString();
                Stats.UpdateStatVars("main", "admins", admins);
            }

            //未发帖会员数
            if (statvars.ContainsKey("memnonpost"))
                memnonpost = Utils.StrToInt(statvars["memnonpost"], 0);
            else
            {
                memnonpost = Stats.GetNonPostMemCount();
                Stats.UpdateStatVars("main", "memnonpost", memnonpost.ToString());
            }

            //热门论坛
            if (statvars.ContainsKey("hotforum"))
                hotforum = (ForumInfo)SerializationHelper.DeSerialize(typeof(ForumInfo), statvars["hotforum"]);
            else
            {
                hotforum = Stats.GetHotForum();
                Stats.UpdateStatVars("main", "hotforum", SerializationHelper.Serialize(hotforum));
            }

            //今日最佳会员及其今日帖数
            if (statvars.ContainsKey("bestmem") && statvars.ContainsKey("bestmemposts"))
            {
                bestmem = statvars["bestmem"];
                bestmemposts = Utils.StrToInt(statvars["bestmemposts"], 0);
            }
            else
            {
                Stats.GetBestMember(out bestmem, out bestmemposts);
                Stats.UpdateStatVars("main", "bestmem", bestmem);
                Stats.UpdateStatVars("main", "bestmemposts", bestmemposts.ToString());
            }
            mempost = members - memnonpost;
            mempostavg = (double)Math.Round((double)posts / (double)members, 2);
            topicreplyavg = (double)Math.Round((double)(posts - topics) / (double)topics, 2);
            mempostpercent = (double)Math.Round((double)(mempost * 100) / (double)members, 2);
            postsaddavg = (double)Math.Round((double)posts / (double)runtime, 2);
            membersaddavg = members / runtime;

            int visitors = Utils.StrToInt(totalstats["members"], 0) + Utils.StrToInt(totalstats["guests"], 0);
            totalstats["visitors"] = visitors;
            pageviewavg = (double)Math.Round((double)Utils.StrToInt(totalstats["hits"], 0) / (double)(visitors == 0 ? 1 : visitors), 2);
            activeindex = ((Math.Round(membersaddavg / (double)(members == 0 ? 1 : members), 2) + Math.Round(postsaddavg / (double)(posts == 0 ? 1 : posts), 2)) * 1500.00 + topicreplyavg * 10.00 + mempostavg + Math.Round(mempostpercent / 10.00, 2) + pageviewavg).ToString();

            if (statstatus)
                monthofstatsbar = Stats.GetStatsDataHtml("month", monthstats, maxmonth);
            else
            {
                monthpostsofstatsbar = Stats.GetStatsDataHtml("monthposts", monthpostsstats, maxmonthposts);
                daypostsofstatsbar = Stats.GetStatsDataHtml("dayposts", daypostsstats, maxdayposts);
            }

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 管理统计
        /// </summary>
        private void GetModWorks()
        {
        }

        /// <summary>
        /// 管理团队
        /// </summary>
        private void GetTeam()
        {
            foreach (string key in teamstats.Keys)
            {
                statvars[key] = teamstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = Utils.GetDateTime();
                Stats.UpdateStatVars("team", "lastupdate", statvars["lastupdate"]);
            }

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 在线时间
        /// </summary>
        private void GetOnlinetime()
        {
            if (config.Oltimespan == 0)
            {
                totalonlinerank = "<li>未开启在线时长统计</li>";
                thismonthonlinerank = "<li></li>";
                return;
            }

            foreach (string key in onlinesstats.Keys)
            {
                statvars[key] = onlinesstats[key].ToString();
            }
            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = Utils.GetDateTime();
                Stats.UpdateStatVars("onlines", "lastupdate", statvars["lastupdate"]);
            }
            ShortUserInfo[] total;
            if (statvars.ContainsKey("total"))
                total = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["total"]);
            else
            {
                total = Stats.GetUserOnlinetime("total");
                Stats.UpdateStatVars("onlines", "total", SerializationHelper.Serialize(total));
            }

            ShortUserInfo[] thismonth;
            if (statvars.ContainsKey("thismonth"))
                thismonth = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["thismonth"]);
            else
            {
                thismonth = Stats.GetUserOnlinetime("thismonth");
                Stats.UpdateStatVars("onlines", "thismonth", SerializationHelper.Serialize(thismonth));
            }

            int maxrows = Math.Max(total.Length, thismonth.Length);

            totalonlinerank = Stats.GetUserRankHtml(total, "onlinetime", maxrows);
            thismonthonlinerank = Stats.GetUserRankHtml(thismonth, "onlinetime", maxrows);

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 交易记录
        /// </summary>
        private void GetTrade()
        {
        }

        /// <summary>
        /// 信用记录
        /// </summary>
        private void GetCreditsRank()
        {
            score = Scoresets.GetValidScoreName();
            foreach (string key in creditsrankstats.Keys)
            {
                statvars[key] = creditsrankstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = Utils.GetDateTime();
                Stats.UpdateStatVars("creditsrank", "lastupdate", statvars["lastupdate"]);
            }

            ShortUserInfo[] credits;
            ShortUserInfo[][] extendedcredits;
            if (statvars.ContainsKey("credits"))
                credits = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["credits"]);
            else
            {
                credits = Stats.GetUserArray("credits");
                Stats.UpdateStatVars("creditsrank", "credits", SerializationHelper.Serialize(credits));
            }

            if (statvars.ContainsKey("extendedcredits"))
                extendedcredits = (ShortUserInfo[][])SerializationHelper.DeSerialize(typeof(ShortUserInfo[][]), statvars["extendedcredits"]);
            else
            {
                extendedcredits = Stats.GetExtsRankUserArray();
                Stats.UpdateStatVars("creditsrank", "extendedcredits", SerializationHelper.Serialize(extendedcredits));
            }

            int maxrows = credits.Length;
            for (int i = 1; i < 8; i++)
            {
                maxrows = Math.Max(extendedcredits[i].Length, maxrows);
            }

            creditsrank = Stats.GetUserRankHtml(credits, "credits", maxrows);
            extcreditsrank1 = Stats.GetUserRankHtml(extendedcredits[0], "extcredits1", maxrows);
            extcreditsrank2 = Stats.GetUserRankHtml(extendedcredits[1], "extcredits2", maxrows);
            extcreditsrank3 = Stats.GetUserRankHtml(extendedcredits[2], "extcredits3", maxrows);
            extcreditsrank4 = Stats.GetUserRankHtml(extendedcredits[3], "extcredits4", maxrows);
            extcreditsrank5 = Stats.GetUserRankHtml(extendedcredits[4], "extcredits5", maxrows);
            extcreditsrank6 = Stats.GetUserRankHtml(extendedcredits[5], "extcredits6", maxrows);
            extcreditsrank7 = Stats.GetUserRankHtml(extendedcredits[6], "extcredits7", maxrows);
            extcreditsrank8 = Stats.GetUserRankHtml(extendedcredits[7], "extcredits8", maxrows);

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 发帖排行
        /// </summary>
        private void GetPostsRank()
        {
            foreach (string key in postsrankstats.Keys)
            {
                statvars[key] = postsrankstats[key].ToString();
            }

            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = Utils.GetDateTime();
                Stats.UpdateStatVars("postsrank", "lastupdate", statvars["lastupdate"]);
            }

            ShortUserInfo[] posts;
            ShortUserInfo[] digestposts;
            ShortUserInfo[] thismonth;
            ShortUserInfo[] today;

            if (statvars.ContainsKey("posts"))
                posts = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["posts"]);
            else
            {
                posts = Stats.GetUserArray("posts");
                Stats.UpdateStatVars("postsrank", "posts", SerializationHelper.Serialize(posts));
            }

            if (statvars.ContainsKey("digestposts"))
                digestposts = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["digestposts"]);
            else
            {
                digestposts = Stats.GetUserArray("digestposts");
                Stats.UpdateStatVars("postsrank", "digestposts", SerializationHelper.Serialize(digestposts));
            }

            if (statvars.ContainsKey("thismonth"))
                thismonth = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["thismonth"]);
            else
            {
                thismonth = Stats.GetUserArray("thismonth");
                Stats.UpdateStatVars("postsrank", "thismonth", SerializationHelper.Serialize(thismonth));
            }

            if (statvars.ContainsKey("today"))
                today = (ShortUserInfo[])SerializationHelper.DeSerialize(typeof(ShortUserInfo[]), statvars["today"]);
            else
            {
                today = Stats.GetUserArray("today");
                Stats.UpdateStatVars("postsrank", "today", SerializationHelper.Serialize(today));
            }

            int maxrows = posts.Length;
            maxrows = Math.Max(digestposts.Length, maxrows);
            maxrows = Math.Max(thismonth.Length, maxrows);
            maxrows = Math.Max(today.Length, maxrows);

            postsrank = Stats.GetUserRankHtml(posts, "posts", maxrows);
            digestpostsrank = Stats.GetUserRankHtml(digestposts, "digestposts", maxrows);
            thismonthpostsrank = Stats.GetUserRankHtml(thismonth, "thismonth", maxrows);
            todaypostsrank = Stats.GetUserRankHtml(today, "today", maxrows);

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 主题排行
        /// </summary>
        private void GetTopicsRank()
        {
            hottopics = Stats.GetHotTopicsHtml();
            hotreplytopics = Stats.GetHotReplyTopicsHtml();
        }

        /// <summary>
        /// 板块排行
        /// </summary>
        private void GetForumsRank()
        {
            foreach (string key in forumsrankstats.Keys)
            {
                statvars[key] = forumsrankstats[key].ToString();
            }
            if (!statvars.ContainsKey("lastupdate") || (DateTime.Now - DateTime.Parse(statvars["lastupdate"])).TotalMinutes > statscachelife)
            {
                statvars.Clear();
                statvars["lastupdate"] = Utils.GetDateTime();
                Stats.UpdateStatVars("forumsrank", "lastupdate", statvars["lastupdate"]);
            }

            List<ForumInfo> topics;
            List<ForumInfo> posts;
            List<ForumInfo> thismonth;
            List<ForumInfo> today;

            if (statvars.ContainsKey("topics"))
                topics = (List<ForumInfo>)SerializationHelper.DeSerialize(typeof(List<ForumInfo>), statvars["topics"]);
            else
            {
                topics = Stats.GetForumArray("topics");
                Stats.UpdateStatVars("forumsrank", "topics", SerializationHelper.Serialize(topics));
            }

            if (statvars.ContainsKey("posts"))
                posts = (List<ForumInfo>)SerializationHelper.DeSerialize(typeof(List<ForumInfo>), statvars["posts"]);
            else
            {
                posts = Stats.GetForumArray("posts");
                Stats.UpdateStatVars("forumsrank", "posts", SerializationHelper.Serialize(posts));
            }

            if (statvars.ContainsKey("thismonth"))
                thismonth = (List<ForumInfo>)SerializationHelper.DeSerialize(typeof(List<ForumInfo>), statvars["thismonth"]);
            else
            {
                thismonth = Stats.GetForumArray("thismonth");
                Stats.UpdateStatVars("forumsrank", "thismonth", SerializationHelper.Serialize(thismonth));
            }

            if (statvars.ContainsKey("today"))
                today = (List<ForumInfo>)SerializationHelper.DeSerialize(typeof(List<ForumInfo>), statvars["today"]);
            else
            {
                today = Stats.GetForumArray("today");
                Stats.UpdateStatVars("forumsrank", "today", SerializationHelper.Serialize(today));
            }

            int maxrows = topics.Count;
            maxrows = Math.Max(posts.Count, maxrows);
            maxrows = Math.Max(thismonth.Count, maxrows);
            maxrows = Math.Max(today.Count, maxrows);

            topicsforumsrank = Stats.GetForumsRankHtml(topics, "topics", maxrows);
            postsforumsrank = Stats.GetForumsRankHtml(posts, "posts", maxrows);
            thismonthforumsrank = Stats.GetForumsRankHtml(thismonth, "thismonth", maxrows);
            todayforumsrank = Stats.GetForumsRankHtml(today, "today", maxrows);

            lastupdate = statvars["lastupdate"];
            nextupdate = DateTime.Parse(statvars["lastupdate"]).AddMinutes(statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 发帖量记录
        /// </summary>
        private void GetPosts()
        {
            monthpostsofstatsbar = Stats.GetStatsDataHtml("monthposts", monthpostsstats, maxmonthposts);
            daypostsofstatsbar = Stats.GetStatsDataHtml("dayposts", daypostsstats, maxdayposts);
        }

        /// <summary>
        /// 客户软件
        /// </summary>
        private void GetClient()
        {
            if (!statstatus) return;
            browserofstatsbar = Stats.GetStatsDataHtml("browser", browserstats, maxbrowser);
            osofstatsbar = Stats.GetStatsDataHtml("os", osstats, maxos);
        }

        /// <summary>
        /// 流量统计
        /// </summary>
        private void GetViews()
        {
            if (!statstatus) return;
            weekofstatsbar = Stats.GetStatsDataHtml("week", weekstats, maxweek);
            hourofstatsbar = Stats.GetStatsDataHtml("hour", hourstats, maxhour);
        }

        public string IsChecked(string op)
        {
            if (op == "merge")
            {
                return DNTRequest.GetString("merge") != "1" ? "" : " checked=\"checked\"";
            }
            return DNTRequest.GetString("types").Contains(op) ? " checked=\"checked\"" : "";
        }
    }
}
