using System;
using System.Text;
using System.Data;

using Discuz.Cache;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// 积分管理类
    /// </summary>
    public class Scoresets
    {
        private static object lockHelper = new object();

        private static string scoreFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/scoreset.config");

        /// <summary>
        /// 获得积分策略
        /// </summary>
        /// <returns>积分策略</returns>
        public static DataTable GetScoreSet()
        {
            lock (lockHelper)
            {
                DNTCache cache = DNTCache.GetCacheService();
                DataTable dt = cache.RetrieveObject("/Forum/ScoreSet") as DataTable;

                if (dt == null)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(scoreFilePath);
                    dt = ds.Tables[0];
                    cache.AddObject("/Forum/ScoreSet", dt, new string[] { scoreFilePath });
                }
                return dt;
            }
        }


        /// <summary>
        /// 获得积分策略
        /// </summary>
        /// <returns>积分策略描述</returns>
        public static UserExtcreditsInfo GetScoreSet(int extcredits)
        {
            UserExtcreditsInfo userextcreditsinfo = new UserExtcreditsInfo();
            string extcreditsname = "extcredits" + extcredits;
            DataTable dt = GetScoreSet();

            if (extcredits > 0)
            {
                userextcreditsinfo.Name = dt.Rows[0][extcreditsname].ToString();
                userextcreditsinfo.Unit = dt.Rows[1][extcreditsname].ToString();
                userextcreditsinfo.Rate = Single.Parse(dt.Rows[2][extcreditsname].ToString());
                userextcreditsinfo.Init = Single.Parse(dt.Rows[3][extcreditsname].ToString());
                userextcreditsinfo.Topic = Single.Parse(dt.Rows[4][extcreditsname].ToString());
                userextcreditsinfo.Reply = Single.Parse(dt.Rows[5][extcreditsname].ToString());
                userextcreditsinfo.Digest = Single.Parse(dt.Rows[6][extcreditsname].ToString());
                userextcreditsinfo.Upload = Single.Parse(dt.Rows[7][extcreditsname].ToString());
                userextcreditsinfo.Download = Single.Parse(dt.Rows[8][extcreditsname].ToString());
                userextcreditsinfo.Pm = Single.Parse(dt.Rows[9][extcreditsname].ToString());
                userextcreditsinfo.Search = Single.Parse(dt.Rows[10][extcreditsname].ToString());
                userextcreditsinfo.Pay = Single.Parse(dt.Rows[11][extcreditsname].ToString());
                userextcreditsinfo.Vote = Single.Parse(dt.Rows[12][extcreditsname].ToString());
            }
            return userextcreditsinfo;
        }

        /// <summary>
        /// 获得具有兑换比率的可交易积分策略
        /// </summary>
        /// <returns>兑换比率的可交易积分策略</returns>
        public static DataTable GetScorePaySet(int type)
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = (type == 0) ? cache.RetrieveObject("/Forum/ScorePaySet") as DataTable : cache.RetrieveObject("/Forum/ScorePaySet1") as DataTable;
            bool pass = true;

            if (dt == null)
            {
                DataTable dtScoreSet = GetScoreSet();
                DataTable dtScorePaySet = new DataTable();
                dtScorePaySet.Columns.Add("id", Type.GetType("System.Int32"));
                dtScorePaySet.Columns.Add("name", Type.GetType("System.String"));
                dtScorePaySet.Columns.Add("rate", Type.GetType("System.Single"));
                for (int i = 1; i <= 8; i++)
                {
                    pass = (!Utils.StrIsNullOrEmpty(dtScoreSet.Rows[0]["extcredits" + i].ToString()));
                    if (type == 0)
                        pass = pass && (dtScoreSet.Rows[2]["extcredits" + i].ToString() != "0");

                    if (pass)
                    {
                        DataRow dr = dtScorePaySet.NewRow();
                        dr["id"] = i;
                        dr["name"] = dtScoreSet.Rows[0]["extcredits" + i.ToString()].ToString().Trim();
                        dr["rate"] = TypeConverter.ObjectToFloat(dtScoreSet.Rows[2]["extcredits" + i]);
                        dtScorePaySet.Rows.Add(dr);
                    }
                }
                if (type == 0)
                    cache.AddObject("/Forum/ScorePaySet", dtScorePaySet);
                else
                    cache.AddObject("/Forum/ScorePaySet1", dtScorePaySet);

                dt = dtScorePaySet;
            }
            return dt;
        }

        /// <summary>
        /// 获取评分操作专用的积分策略
        /// </summary>
        /// <returns>分操作专用的积分策略</returns>
        public static DataTable GetRateScoreSet()
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/RateScoreSet") as DataTable;

            if (dt == null)
            {
                DataTable dtScoreSet = GetScoreSet();
                DataTable dtRateScoreSet = new DataTable();
                dtRateScoreSet.Columns.Add("id", Type.GetType("System.Int32"));
                dtRateScoreSet.Columns.Add("name", Type.GetType("System.String"));
                dtRateScoreSet.Columns.Add("rate", Type.GetType("System.Single"));

                for (int i = 1; i <= 8; i++)
                {
                    DataRow dr = dtRateScoreSet.NewRow();
                    dr["id"] = i;
                    dr["name"] = dtScoreSet.Rows[0]["extcredits" + i].ToString().Trim();
                    dr["rate"] = TypeConverter.ObjectToFloat(dtScoreSet.Rows[2]["extcredits" + i]);
                    dtRateScoreSet.Rows.Add(dr);
                }
                dt = dtRateScoreSet;
                cache.AddObject("/Forum/RateScoreSet", dt);
            }
            return dt;
        }

        /// <summary>
        /// 返回前台可以使用的扩展字段单位
        /// </summary>
        /// <returns>返回前台可以使用的扩展字段单位</returns>
        public static string[] GetValidScoreUnit()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string[] scoreunit = cache.RetrieveObject("/Forum/ValidScoreUnit") as string[];

            if (scoreunit == null)
            {
                scoreunit = GetValidScore(1);
                cache.AddObject("/Forum/ValidScoreUnit", scoreunit);
            }
            return scoreunit;
        }

        public static bool IsSetDownLoadAttachScore()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string isSetScore = cache.RetrieveObject("/Forum/IsSetDownLoadAttachScore") as string;

            if (Utils.StrIsNullOrEmpty(isSetScore))
            {
                float[] extCredits = GetUserExtCredits(CreditsOperationType.DownloadAttachment);

                foreach (float i in extCredits)
                {
                    if (i < 0.00)
                    {
                        cache.AddObject("/Forum/IsSetDownLoadAttachScore", "true");
                        return true;
                    }
                }
            }
            return Utils.StrToBool(isSetScore, false);
        }

        public static float[] GetUserExtCredits(CreditsOperationType creditsOperationType)
        {
            creditsOperationType = creditsOperationType == CreditsOperationType.DeletePost ? CreditsOperationType.PostReply : creditsOperationType;
            DataRow dr = GetScoreSet().Rows[(int)creditsOperationType];
            float[] extCredits = new float[8];
            for (int i = 0; i < 8; i++)
            {
                extCredits[i] = TypeConverter.ObjectToFloat(dr["extcredits" + (i + 1)]);
            }
            return extCredits;
        }

        /// <summary>
        /// 获取有效积分字段
        /// </summary>
        /// <param name="validid">字段类型: 0为字段id， 1为字段名称</param>
        /// <returns></returns>
        private static string[] GetValidScore(int validid)
        {
            // 为了前台模板中的可读性, scoreunit中有效元素也对应extcredits1 - 8字段, score[0]无用
            string[] scoreunit = new string[9];
            scoreunit[0] = "";
            DataTable dt = GetScoreSet();

            for (int i = 1; i < 9; i++)
            {
                if (Utils.StrIsNullOrEmpty(dt.Rows[validid]["extcredits" + i].ToString()))
                    scoreunit[i] = "";
                else
                    scoreunit[i] = dt.Rows[validid]["extcredits" + i].ToString();
            }
            dt.Dispose();
            return scoreunit;
        }

        /// <summary>
        /// 返回前台可以使用的扩展字段名和显示名称
        /// </summary>
        /// <returns>前台可以使用的扩展字段名名和显示名称</returns>
        public static string[] GetValidScoreName()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string[] score = cache.RetrieveObject("/Forum/ValidScoreName") as string[];

            if (score == null)
            {
                score = GetValidScore(0);
                cache.AddObject("/Forum/ValidScoreName", score);
            }
            return score;
        }

        private static string GetScoresCache(string cacheKey)
        {
            DNTCache cache = DNTCache.GetCacheService();
            string creditstrans = cache.RetrieveObject("/Forum/Scoreset/" + cacheKey) as string;

            if (creditstrans == null)
            {
                DataSet ds = new DataSet();
                ds.ReadXml(scoreFilePath);
                creditstrans = ds.Tables["formula"].Rows[0][cacheKey.ToLower()].ToString();
                cache.AddObject("/Forum/Scoreset/" + cacheKey, creditstrans);
            }
            return creditstrans;
        }

        /// <summary>
        /// 获得积分规则
        /// </summary>
        /// <returns></returns>
        public static string GetScoreCalFormula()
        {
            //重构之前缓存键为/Forum/Scoreset/Formula
            return GetScoresCache("FormulaContext");
        }

        /// <summary>
        /// 返回交易积分
        /// </summary>
        /// <returns>交易积分</returns>
        public static int GetCreditsTrans()
        {
            return TypeConverter.StrToInt(GetScoresCache("CreditsTrans"));
        }

        /// <summary>
        /// 返回积分交易税
        /// </summary>
        /// <returns>积分交易税</returns>
        public static float GetCreditsTax()
        {
            return TypeConverter.StrToFloat(GetScoresCache("CreditsTax"));
        }

        /// <summary>
        /// 转账最低余额
        /// </summary>
        /// <returns>转账最低余额</returns>
        public static int GetTransferMinCredits()
        {
            return TypeConverter.StrToInt(GetScoresCache("TransferMinCredits"));
        }

        /// <summary>
        /// 返回兑换最低余额
        /// </summary>
        /// <returns>兑换最低余额</returns>
        public static int GetExchangeMinCredits()
        {
            return TypeConverter.StrToInt(GetScoresCache("ExchangeMinCredits"));
        }

        /// <summary>
        /// 单主题最高收入
        /// </summary>
        /// <returns></returns>
        public static int GetMaxIncPerTopic()
        {
            return TypeConverter.StrToInt(GetScoresCache("MaxIncPerThread"));
        }

        /// <summary>
        /// 单主题最高出售时限(小时)
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChargeSpan()
        {
            return TypeConverter.StrToInt(GetScoresCache("MaxChargeSpan"));
        }

        /// <summary>
        /// 确认当前时间是否在指定的时间列表内
        /// </summary>
        /// <param name="timelist">一个包含多个时间段的列表(格式为hh:mm-hh:mm)</param>
        /// <param name="vtime">输出参数：符合条件的第一个是时间段</param>
        /// <returns>时间段存在则返回true,否则返回false</returns>
        public static bool BetweenTime(string timelist, out string vtime)
        {
            if (!Utils.StrIsNullOrEmpty(timelist))
            {
                string[] enabledvisittime = Utils.SplitString(timelist, "\n");

                if (enabledvisittime.Length > 0)
                {
                    string starttime = "", endtime = "";
                    int s = 0, e = 0;

                    foreach (string visittime in enabledvisittime)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(visittime, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])-(([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9]))$"))
                        {
                            starttime = visittime.Substring(0, visittime.IndexOf("-"));
                            s = Utils.StrDateDiffMinutes(starttime, 0);

                            endtime = Utils.CutString(visittime, visittime.IndexOf("-") + 1, visittime.Length - (visittime.IndexOf("-") + 1));
                            e = Utils.StrDateDiffMinutes(endtime, 0);

                            if (DateTime.Parse(starttime) < DateTime.Parse(endtime)) //起始时间小于结束时间,认为未跨越0点
                            {
                                if (s > 0 && e < 0)
                                {
                                    vtime = visittime;
                                    return true;
                                }
                            }
                            else //起始时间大于结束时间,认为跨越0点
                            {
                                if ((s < 0 && e < 0) || (s > 0 && e > 0 && e > s))
                                {
                                    vtime = visittime;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            vtime = "";
            return false;
        }

        /// <summary>
        /// 确认当前时间是否在指定的时间列表内
        /// </summary>
        /// <param name="timelist">一个包含多个时间段的列表(格式为hh:mm-hh:mm)</param>
        /// <returns>时间段存在则返回true,否则返回false</returns>
        public static bool BetweenTime(string timelist)
        {
            string visittime = "";
            return BetweenTime(timelist, out visittime);
        }



        /// <summary>
        /// 获取主题(附件)买卖使用的积分字段
        /// </summary>
        /// <returns>使用的扩展积分字段</returns>
        public static int GetTopicAttachCreditsTrans()
        {
            if (GetCreditsTrans() == 0)
                return 0;

            DNTCache cache = DNTCache.GetCacheService();
            int transfermincredits = TypeConverter.StrToInt(GetScoresCache("TopicAttachCreditsTrans"));

            if (transfermincredits < 1 || transfermincredits > 8)
                transfermincredits = GetCreditsTrans();

            return transfermincredits;
        }

        /// <summary>
        /// 获取主题附件交易积分字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetTopicAttachCreditsTransName()
        {
            return Scoresets.GetValidScoreName()[Scoresets.GetTopicAttachCreditsTrans()];
        }

        /// <summary>
        /// 获取悬赏使用的积分字段
        /// </summary>
        /// <returns>使用的扩展积分字段,格式如：</returns>
        public static int GetBonusCreditsTrans()
        {
            if (GetCreditsTrans() == 0)
                return 0;

            DNTCache cache = DNTCache.GetCacheService();
            int transfermincredits = TypeConverter.StrToInt(GetScoresCache("BonusCreditsTrans"));

            if (transfermincredits == 0)
                transfermincredits = GetCreditsTrans();

            return transfermincredits;
        }

        /// <summary>
        /// 返回有名称和ID组成的一个字符串，例如1|威望|,2|金钱|,3|贡献|
        /// </summary>
        /// <returns></returns>
        public static string GetValidScoreNameAndId()
        {
            string[] scoreNames = Scoresets.GetValidScoreName();
            StringBuilder validScore = new StringBuilder();
            for (int i = 1; i < 9; i++)
            {
                if (scoreNames[i] == string.Empty)
                    continue;
                validScore.Append(i);
                validScore.Append("|");
                validScore.Append(scoreNames[i]);
                validScore.Append("|");
                validScore.Append(",");
            }
            return validScore.ToString().TrimEnd(',');
        }
    }
}