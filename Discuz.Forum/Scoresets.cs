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
    /// ���ֹ�����
    /// </summary>
    public class Scoresets
    {
        private static object lockHelper = new object();

        private static string scoreFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/scoreset.config");

        /// <summary>
        /// ��û��ֲ���
        /// </summary>
        /// <returns>���ֲ���</returns>
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
        /// ��û��ֲ���
        /// </summary>
        /// <returns>���ֲ�������</returns>
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
        /// ��þ��жһ����ʵĿɽ��׻��ֲ���
        /// </summary>
        /// <returns>�һ����ʵĿɽ��׻��ֲ���</returns>
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
        /// ��ȡ���ֲ���ר�õĻ��ֲ���
        /// </summary>
        /// <returns>�ֲ���ר�õĻ��ֲ���</returns>
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
        /// ����ǰ̨����ʹ�õ���չ�ֶε�λ
        /// </summary>
        /// <returns>����ǰ̨����ʹ�õ���չ�ֶε�λ</returns>
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
        /// ��ȡ��Ч�����ֶ�
        /// </summary>
        /// <param name="validid">�ֶ�����: 0Ϊ�ֶ�id�� 1Ϊ�ֶ�����</param>
        /// <returns></returns>
        private static string[] GetValidScore(int validid)
        {
            // Ϊ��ǰ̨ģ���еĿɶ���, scoreunit����ЧԪ��Ҳ��Ӧextcredits1 - 8�ֶ�, score[0]����
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
        /// ����ǰ̨����ʹ�õ���չ�ֶ�������ʾ����
        /// </summary>
        /// <returns>ǰ̨����ʹ�õ���չ�ֶ���������ʾ����</returns>
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
        /// ��û��ֹ���
        /// </summary>
        /// <returns></returns>
        public static string GetScoreCalFormula()
        {
            //�ع�֮ǰ�����Ϊ/Forum/Scoreset/Formula
            return GetScoresCache("FormulaContext");
        }

        /// <summary>
        /// ���ؽ��׻���
        /// </summary>
        /// <returns>���׻���</returns>
        public static int GetCreditsTrans()
        {
            return TypeConverter.StrToInt(GetScoresCache("CreditsTrans"));
        }

        /// <summary>
        /// ���ػ��ֽ���˰
        /// </summary>
        /// <returns>���ֽ���˰</returns>
        public static float GetCreditsTax()
        {
            return TypeConverter.StrToFloat(GetScoresCache("CreditsTax"));
        }

        /// <summary>
        /// ת��������
        /// </summary>
        /// <returns>ת��������</returns>
        public static int GetTransferMinCredits()
        {
            return TypeConverter.StrToInt(GetScoresCache("TransferMinCredits"));
        }

        /// <summary>
        /// ���ضһ�������
        /// </summary>
        /// <returns>�һ�������</returns>
        public static int GetExchangeMinCredits()
        {
            return TypeConverter.StrToInt(GetScoresCache("ExchangeMinCredits"));
        }

        /// <summary>
        /// �������������
        /// </summary>
        /// <returns></returns>
        public static int GetMaxIncPerTopic()
        {
            return TypeConverter.StrToInt(GetScoresCache("MaxIncPerThread"));
        }

        /// <summary>
        /// ��������߳���ʱ��(Сʱ)
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChargeSpan()
        {
            return TypeConverter.StrToInt(GetScoresCache("MaxChargeSpan"));
        }

        /// <summary>
        /// ȷ�ϵ�ǰʱ���Ƿ���ָ����ʱ���б���
        /// </summary>
        /// <param name="timelist">һ���������ʱ��ε��б�(��ʽΪhh:mm-hh:mm)</param>
        /// <param name="vtime">������������������ĵ�һ����ʱ���</param>
        /// <returns>ʱ��δ����򷵻�true,���򷵻�false</returns>
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

                            if (DateTime.Parse(starttime) < DateTime.Parse(endtime)) //��ʼʱ��С�ڽ���ʱ��,��Ϊδ��Խ0��
                            {
                                if (s > 0 && e < 0)
                                {
                                    vtime = visittime;
                                    return true;
                                }
                            }
                            else //��ʼʱ����ڽ���ʱ��,��Ϊ��Խ0��
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
        /// ȷ�ϵ�ǰʱ���Ƿ���ָ����ʱ���б���
        /// </summary>
        /// <param name="timelist">һ���������ʱ��ε��б�(��ʽΪhh:mm-hh:mm)</param>
        /// <returns>ʱ��δ����򷵻�true,���򷵻�false</returns>
        public static bool BetweenTime(string timelist)
        {
            string visittime = "";
            return BetweenTime(timelist, out visittime);
        }



        /// <summary>
        /// ��ȡ����(����)����ʹ�õĻ����ֶ�
        /// </summary>
        /// <returns>ʹ�õ���չ�����ֶ�</returns>
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
        /// ��ȡ���⸽�����׻����ֶ�����
        /// </summary>
        /// <returns></returns>
        public static string GetTopicAttachCreditsTransName()
        {
            return Scoresets.GetValidScoreName()[Scoresets.GetTopicAttachCreditsTrans()];
        }

        /// <summary>
        /// ��ȡ����ʹ�õĻ����ֶ�
        /// </summary>
        /// <returns>ʹ�õ���չ�����ֶ�,��ʽ�磺</returns>
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
        /// ���������ƺ�ID��ɵ�һ���ַ���������1|����|,2|��Ǯ|,3|����|
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