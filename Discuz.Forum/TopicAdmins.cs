using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using System.Collections;

namespace Discuz.Forum
{
    /// <summary>
    /// 主题管理操作类
    /// </summary>
    public class TopicAdmins
    {
        /// <summary>
        /// 设置主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        private static int SetTopicStatus(string topiclist, string field, int intValue)
        {
            return SetTopicStatus(topiclist, field, intValue.ToString());
        }


        /// <summary>
        /// 设置主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        private static int SetTopicStatus(string topiclist, string field, byte intValue)
        {
            return SetTopicStatus(topiclist, field, intValue.ToString());
        }


        /// <summary>
        /// 设置主题指定字段的属性值(字符型)
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        private static int SetTopicStatus(string topiclist, string field, string intValue)
        {
            if ((",displayorder,highlight,digest,").IndexOf("," + field.ToLower().Trim() + ",") < 0)
                return -1;

            if (!Utils.IsNumericList(topiclist))
                return -1;

            return Discuz.Data.TopicAdmins.SetTopicStatus(topiclist, field, intValue);
        }

        private static MemCachedConfigInfo mcci = MemCachedConfigs.GetConfig();
        private static RedisConfigInfo rci = RedisConfigs.GetConfig();


        /// <summary>
        /// 将主题置顶/解除置顶
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">置顶级别( 0 为解除置顶)</param>
        /// <returns>更新主题个数</returns>
        public static int SetTopTopicList(int fid, string topiclist, short intValue)
        {
            //只有在应用memcached的情况下才可以使用主题缓存
            if ((mcci != null && mcci.ApplyMemCached) || (rci != null && rci.ApplyRedis))
            {
                //因为考虑到某些置顶主题是全局置顶所以这里一旦出现置顶操作，则清除所有置顶缓存信息
                foreach (ForumInfo forumInfo in Forums.GetForumList())
                {
                    if (forumInfo.Layer > 0)
                        Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ShowTopic/TopList/" + forumInfo.Fid + "/");
                }
            }

            if (SetTopicStatus(topiclist, "displayorder", intValue) > 0 && ResetTopTopicList() == 1)
                return 1;

            if (Utils.FileExists(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/topic/" + fid.ToString() + ".xml")))
                File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/topic/" + fid.ToString() + ".xml"));

            return -1;
        }

        /// <summary>
        /// 重新生成置顶主题
        /// </summary>
        /// <param name="fid">主题ID</param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        public static int ResetTopTopicList()
        {
            DataSet ds = Discuz.Data.TopicAdmins.GetTopTopicList();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable topTable = Discuz.Data.TopicAdmins.GetShortForums();
                int[] fidIndex = null;

                if (topTable != null && topTable.Rows.Count > 0)
                {
                    fidIndex = new int[TypeConverter.ObjectToInt(topTable.Rows[0]["fid"]) + 1];
                    for (int i = 0; i < topTable.Rows.Count; i++)
                    {
                        fidIndex[TypeConverter.ObjectToInt(topTable.Rows[i]["fid"])] = i;
                    }
                }

                ds.DataSetName = "topics";
                ds.Tables[0].TableName = "topic";
                int tidCount = 0, tid0Count = 0, tid1Count, tid2Count, tid3Count = 0;

                StringBuilder sbTop3 = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (TypeConverter.ObjectToInt(dr["displayorder"]) == 3)
                    {
                        if (sbTop3.Length > 0)
                            sbTop3.Append(",");

                        if (fidIndex != null && fidIndex.Length >= TypeConverter.ObjectToInt(dr["fid"]))
                        {
                            sbTop3.Append(dr["tid"]);
                            tidCount++;
                            topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tid3count"] = TypeConverter.ObjectToInt(topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tid3count"]) + 1;
                        }

                    }
                    else
                    {
                        if (fidIndex != null && fidIndex.Length >= TypeConverter.ObjectToInt(dr["fid"]))
                        {
                            if (TypeConverter.ObjectToInt(dr["displayorder"]) != 2)
                            {
                                topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tidlist"] = topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tidlist"].ToString() + "," + dr["tid"].ToString();
                                topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tidcount"] = TypeConverter.ObjectToInt(topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tidcount"]) + 1;
                            }
                            else
                            {
                                topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tid2list"] = topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tid2list"].ToString() + "," + dr["tid"].ToString();
                                topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tid2count"] = TypeConverter.ObjectToInt(topTable.Rows[fidIndex[TypeConverter.ObjectToInt(dr["fid"])]]["tid2count"]) + 1;
                            }
                        }
                    }
                }

                if (topTable != null && topTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in topTable.Rows)
                    {
                        dr["temptidlist"] = sbTop3.ToString() + dr["tidlist"].ToString() + dr["tid2list"].ToString();

                        tid1Count = TypeConverter.ObjectToInt(dr["tidcount"]);
                        tid2Count = TypeConverter.ObjectToInt(dr["tid2count"]);
                        tid3Count = TypeConverter.ObjectToInt(dr["tid3count"]);

                        tid0Count = tid1Count + tid2Count + tid3Count;

                        dr["tidcount"] = tid1Count + tidCount + TypeConverter.ObjectToInt(dr["tid2count"]);

                        string filterexpress = Discuz.Data.TopicAdmins.ResetTopTopicListSql(TypeConverter.ObjectToInt(dr["layer"]), dr["fid"].ToString(), dr["parentidlist"].ToString().Trim());

                        foreach (DataRow drTemp in topTable.Select(filterexpress))
                        {
                            if (!drTemp["tid2list"].ToString().Equals(""))
                            {
                                dr["temptidlist"] = dr["temptidlist"].ToString() + drTemp["tid2list"].ToString();
                                dr["tidcount"] = TypeConverter.ObjectToInt(drTemp["tid2count"]) + TypeConverter.ObjectToInt(dr["tidcount"]);
                                tid2Count = tid2Count + TypeConverter.ObjectToInt(drTemp["tid2count"]);
                            }
                        }

                        tid0Count = TypeConverter.ObjectToInt(dr["tidcount"]) - tid0Count;
                        if (ds.Tables.Count == 1)
                        {
                            ds.Tables.Add("fidtopic");
                            ds.Tables[1].Columns.Add("tid", Type.GetType("System.String"));
                            ds.Tables[1].Columns.Add("tidCount", Type.GetType("System.Int32"));
                            ds.Tables[1].Columns.Add("tid0Count", Type.GetType("System.Int32"));
                            ds.Tables[1].Columns.Add("tid1Count", Type.GetType("System.Int32"));
                            ds.Tables[1].Columns.Add("tid2Count", Type.GetType("System.Int32"));
                            ds.Tables[1].Columns.Add("tid3Count", Type.GetType("System.Int32"));
                            ds.Tables[1].Rows.Add(ds.Tables[1].NewRow());
                        }
                        ds.Tables[1].Rows[0]["tid"] = dr["temptidlist"];
                        ds.Tables[1].Rows[0]["tidCount"] = dr["tidcount"];
                        ds.Tables[1].Rows[0]["tid0Count"] = tid0Count;
                        ds.Tables[1].Rows[0]["tid1Count"] = tid1Count;
                        ds.Tables[1].Rows[0]["tid2Count"] = tid2Count;
                        ds.Tables[1].Rows[0]["tid3Count"] = tidCount;

                        DataSet tempDS = new DataSet("topics");
                        tempDS.Tables.Add(ds.Tables[1].Copy());
                        tempDS.Tables[0].TableName = "topic";
                        if (!Directory.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/topic/")))
                            Directory.CreateDirectory(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/topic/"));

                        tempDS.WriteXml(@Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/topic/" + dr["fid"].ToString() + ".xml"), XmlWriteMode.WriteSchema);
                        tempDS.Clear();
                        tempDS.Dispose();
                    }
                }
                topTable.Dispose();
                ds.Clear();
                ds.Dispose();
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 将主题高亮显示
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">高亮样式及颜色( 0 为解除高亮显示)</param>
        /// <returns>更新主题个数</returns>
        public static int SetHighlight(string topiclist, string intValue)
        {
            return SetTopicStatus(topiclist, "highlight", intValue);
        }

        /// <summary>
        /// 根据得到给定主题的用户列表
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="digestType">操作源(0:精华,1:删除)</param>
        /// <returns>用户列表</returns>
        private static string GetUserListWithDigestTopiclist(string topiclist, int digestType)
        {
            if (!Utils.IsNumericList(topiclist))
                return "";

            StringBuilder useridlist = new StringBuilder();
            List<ShortUserInfo> shortUserInfoList = Discuz.Data.TopicAdmins.GetUserListWithDigestTopicList(topiclist, digestType);

            foreach (ShortUserInfo shortUserInfo in shortUserInfoList)
            {
                if (!Utils.StrIsNullOrEmpty(useridlist.ToString()))
                    useridlist.Append(",");

                useridlist.Append(shortUserInfo.Uid);
            }
            return useridlist.ToString();
        }


        /// <summary>
        /// 将主题设置精华/解除精华
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">精华级别( 0 为解除精华)</param>
        /// <returns>更新主题个数</returns>
        public static int SetDigest(string topiclist, int intValue)
        {
            string useridlist = GetUserListWithDigestTopiclist(topiclist, intValue > 0 ? 1 : 0);
            int mount = SetTopicStatus(topiclist, "digest", intValue);

            if (mount > 0)
            {
                if (Utils.IsNumericList(useridlist))
                    Discuz.Data.Users.UpdateUserDigest(useridlist);

                if (!string.IsNullOrEmpty(useridlist) && Utils.IsNumericList(useridlist))
                    UserCredits.UpdateUserCreditsAndExtCredits(useridlist, CreditsOperationType.Digest, intValue > 0 ? 1 : -1);
            }
            return mount;
        }

        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        public static int SetClose(string topiclist, short intValue)
        {
            if (!Utils.IsNumericList(topiclist))
                return -1;

            return Discuz.Data.TopicAdmins.SetTopicClose(topiclist, intValue);
        }



        /// <summary>
        /// 获得主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="field">要获得值的字段</param>
        /// <returns>主题指定字段的状态</returns>
        public static int GetTopicStatus(string topiclist, string field)
        {
            if (!Utils.IsNumericList(topiclist) ||
                (",displayorder,digest,").IndexOf("," + field.ToLower().Trim() + ",") < 0)
                return -1;

            return DatabaseProvider.GetInstance().GetTopicStatus(topiclist, field);
        }


        /// <summary>
        /// 获得主题置顶状态
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>置顶状态(单个主题返回真实状态,多个主题返回状态值累计)</returns>
        public static int GetDisplayorder(string topiclist)
        {
            return GetTopicStatus(topiclist, "displayorder");
        }


        /// <summary>
        /// 获得主题精华状态
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>精华状态(单个主题返回真实状态,多个主题返回状态值累计)</returns>
        public static int GetDigest(string topiclist)
        {
            return GetTopicStatus(topiclist, "digest");
        }


        /// <summary>
        /// 在数据库中删除指定主题
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="subtractCredits">是否减少用户积分(0不减少,1减少)</param>
        /// <returns>删除个数</returns>
        public static int DeleteTopics(string topicList, int subTractCredits, bool reserveAttach)
        {
            if (!Utils.IsNumericList(topicList))
                return -1;

            GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();
            DataTable dt = Topics.GetTopicList(topicList);

            if (dt == null)
                return -1;

            foreach (DataRow dr in dt.Rows)
            {
                if (TypeConverter.ObjectToInt(dr["digest"]) > 0)
                {
                    UserCredits.UpdateUserExtCredits(TypeConverter.ObjectToInt(dr["posterid"]), -1, CreditsOperationType.Digest, 1, true);
                    UserCredits.UpdateUserCredits(TypeConverter.ObjectToInt(dr["posterid"]));
                }
            }

            dt = Posts.GetPostList(topicList);
            if (dt != null)
            {
                Hashtable attUidCount = new Hashtable();
                foreach (DataRow dr in dt.Rows)
                {
                    //后台设置的项为多少天外的老帖删除不减积分，而不是多少天内删帖可以不减分
                    if (configinfo.Losslessdel == 0 || Utils.StrDateDiffHours(dr["postdatetime"].ToString(), configinfo.Losslessdel * 24) < 0)
                    {
                        CreditsOperationType creditsOperationType = TypeConverter.ObjectToInt(dr["layer"]) == 0 ? CreditsOperationType.PostTopic : CreditsOperationType.PostReply;
                        //获取版块积分规则
                        float[] creditsValue = Forums.GetValues(
                            creditsOperationType == CreditsOperationType.PostTopic ? 
                            Forums.GetForumInfo(TypeConverter.ObjectToInt(dr["fid"])).Postcredits : 
                            Forums.GetForumInfo(TypeConverter.ObjectToInt(dr["fid"])).Replycredits
                            );

                        //如果未定义版块积分规则
                        if (creditsValue == null)
                            creditsValue = Scoresets.GetUserExtCredits(creditsOperationType);
                        UserCredits.UpdateUserExtCredits(TypeConverter.ObjectToInt(dr["posterid"]), creditsValue, 1, creditsOperationType, -1, true);
                        int attCount = Attachments.GetAttachmentCountByPid(TypeConverter.ObjectToInt(dr["pid"]));
                        if (attCount > 0)
                        {
                            int posterid = TypeConverter.ObjectToInt(dr["posterid"]);
                            if (attUidCount.ContainsKey(posterid))
                                attUidCount[posterid] = (int)attUidCount[posterid] + attCount;
                            else
                                attUidCount.Add(TypeConverter.ObjectToInt(dr["posterid"]), attCount);
                        }
                    }
                    UserCredits.UpdateUserCredits(TypeConverter.ObjectToInt(dr["posterid"]));
                }

                int i = 0;
                int[] tuidlist = new int[attUidCount.Count];
                int[] attcountlist = new int[attUidCount.Count];
                foreach (DictionaryEntry de in attUidCount)
                {
                    tuidlist[i] = (int)de.Key;
                    attcountlist[i] = (int)de.Value;
                    i++;
                }

                UserCredits.UpdateUserCredits(tuidlist, attcountlist, CreditsOperationType.UploadAttachment, -1);
            }

            int reval = 0;

            foreach (string posttableid in Posts.GetPostTableIdArray(topicList))
            {
                reval = Discuz.Data.TopicAdmins.DeleteTopicByTidList(topicList, posttableid);
            }
            if (reval > 0 && !reserveAttach)
                Attachments.DeleteAttachmentByTid(topicList);
            return reval;

        }
        //public static int DeleteTopics(string topicList, int subTractCredits, bool reserveAttach)
        //{
        //    if (!Utils.IsNumericList(topicList))
        //        return -1;

        //    GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();
        //    DataTable dt = Topics.GetTopicList(topicList);

        //    if (dt == null)
        //        return -1;

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        if (TypeConverter.ObjectToInt(dr["digest"]) > 0)
        //        {
        //            UserCredits.UpdateUserExtCredits(TypeConverter.ObjectToInt(dr["posterid"]), -1, CreditsOperationType.Digest, 1, true);
        //            UserCredits.UpdateUserCredits(TypeConverter.ObjectToInt(dr["posterid"]));
        //        }
        //    }

        //    dt = Posts.GetPostList(topicList);
        //    if (dt != null)
        //    {
        //        ArrayList tUidList = new ArrayList();
        //        ArrayList pUidList = new ArrayList();
        //        Hashtable attUidCount = new Hashtable();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            //后台设置的项为多少天外的老帖删除不减积分，而不是多少天内删帖可以不减分
        //            if (configinfo.Losslessdel == 0 || Utils.StrDateDiffHours(dr["postdatetime"].ToString(), configinfo.Losslessdel * 24) < 0)
        //            {
        //                if (TypeConverter.ObjectToInt(dr["layer"]) == 0)
        //                    tUidList.Add(TypeConverter.ObjectToInt(dr["posterid"]));
        //                else
        //                    pUidList.Add(TypeConverter.ObjectToInt(dr["posterid"]));
        //                int attCount = Attachments.GetAttachmentCountByPid(TypeConverter.ObjectToInt(dr["pid"]));
        //                if (attCount > 0)
        //                {
        //                    //attUidCount.Add(new int[] {TypeConverter.ObjectToInt(dr["posterid"]),attCount});
        //                    int posterid = TypeConverter.ObjectToInt(dr["posterid"]);
        //                    if (attUidCount.ContainsKey(posterid))
        //                        attUidCount[posterid] = (int)attUidCount[posterid] + attCount;
        //                    else
        //                        attUidCount.Add(TypeConverter.ObjectToInt(dr["posterid"]), attCount);
        //                }
        //            }
        //        }
        //        int[,] att = new int[attUidCount.Count, 2];
        //        int i = 0;
        //        foreach (DictionaryEntry de in attUidCount)
        //        {
        //            att[i, 0] = (int)de.Key;
        //            att[i, 1] = (int)de.Value;
        //            i++;
        //        }

        //        UserCredits.UpdateUserCreditsByDeleteTopic((int[])tUidList.ToArray(typeof(int)), (int[])pUidList.ToArray(typeof(int)), att);
        //    }

        //    int reval = 0;

        //    foreach (string posttableid in Posts.GetPostTableIdArray(topicList))
        //    {
        //        reval = Discuz.Data.TopicAdmins.DeleteTopicByTidList(topicList, posttableid);
        //    }
        //    if (reval > 0 && !reserveAttach)
        //        Attachments.DeleteAttachmentByTid(topicList);
        //    return reval;

        //}

        /// <summary>
        /// 删除主题并且改变积分
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="reserveAttach">是否保留附件</param>
        /// <returns>删除主题数</returns>
        public static int DeleteTopicsWithoutChangingCredits(string topiclist, bool reserveAttach)
        {
            if (!Utils.IsNumericList(topiclist))
                return -1;

            int reval = -1;
            foreach (string posttableid in Posts.GetPostTableIdArray(topiclist))
            {
                reval = Discuz.Data.TopicAdmins.DeleteTopicByTidList(topiclist, posttableid);
            }
            if (reval > 0 && !reserveAttach)
            {
                Attachments.DeleteAttachmentByTid(topiclist);
            }
            return reval;
        }

        /// <summary>
        /// 在数据库中删除指定主题
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>删除个数</returns>
        public static int DeleteTopics(string topiclist, bool reserveAttach)
        {
            return DeleteTopics(topiclist, 1, reserveAttach);
        }

        /// <summary>
        /// 在删除指定的主题
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="toDustbin">指定主题删除形式(0：直接从数据库中删除,并删除与之关联的信息  1：只将其从论坛列表中删除(将displayorder字段置为-1)将其放入回收站中</param>
        /// <returns>删除个数</returns>
        public static int DeleteTopics(string topiclist, byte toDustbin, bool reserveAttach)
        {
            return toDustbin == 0 ? DeleteTopics(topiclist, reserveAttach) : SetTopicStatus(topiclist, "displayorder", -1);
        }


        /// <summary>
        /// 恢复回收站中的主题。
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <returns>恢复个数</returns>
        public static int RestoreTopics(string topiclist)
        {
            return SetTopicStatus(topiclist, "displayorder", 0);
        }


        /// <summary>
        /// 移动主题到指定版块
        /// </summary>
        /// <param name="topiclist">要移动的主题列表</param>
        /// <param name="fid">转到的版块ID</param>
        /// <param name="savelink">是否在原版块保留连接</param>
        /// <returns>更新记录数</returns>
        public static int MoveTopics(string topiclist, int fid, int oldfid, bool savelink, int topicType)
        {
            if (!Utils.IsNumericList(topiclist))
                return -1;
            string tidList = "";
            DataTable dt = Topics.GetTopicList(topiclist);
            foreach (DataRow dr in dt.Rows)
            {
                if (TypeConverter.ObjectToInt(dr["closed"]) <= 1 || TypeConverter.ObjectToInt(dr["fid"]) != oldfid)
                {
                    tidList += dr["tid"].ToString() + ",";
                }               
            }
            tidList = tidList.TrimEnd(',');

            if (string.IsNullOrEmpty(tidList))
                return -1;
            Discuz.Data.TopicAdmins.DeleteClosedTopics(fid, tidList);

            //转移帖子
            MoveTopics(tidList, fid, oldfid, topicType);

            //如果保存链接则复制一条记录到原版块
            if (savelink)
            {
                if (Discuz.Data.TopicAdmins.CopyTopicLink(oldfid, tidList) <= 0)
                    return -2;

                AdminForumStats.ReSetFourmTopicAPost(oldfid);
                Forums.SetRealCurrentTopics(oldfid);
            }
            return 1;
        }

        /// <summary>
        /// 移动主题到指定版块
        /// </summary>
        /// <param name="topiclist">要移动的主题列表</param>
        /// <param name="fid">转到的版块ID</param>
        /// <returns>更新记录数</returns>
        public static int MoveTopics(string topiclist, int fid, int oldfid, int topicType)
        {
            if (!Utils.IsNumericList(topiclist))
                return -1;

            //更新帖子
            foreach (string tid in topiclist.Split(','))
            {
                    DatabaseProvider.GetInstance().UpdatePost(topiclist, fid, PostTables.GetPostTableName(TypeConverter.StrToInt(tid)));
            }

            //更新主题
            int reval = Discuz.Data.Topics.UpdateTopic(topiclist, fid, topicType);
            if (reval > 0)
            {
                AdminForumStats.ReSetFourmTopicAPost(fid);
                AdminForumStats.ReSetFourmTopicAPost(oldfid);
                Forums.SetRealCurrentTopics(fid);
                Forums.SetRealCurrentTopics(oldfid);
            }

            //生成置顶帖
            ResetTopTopicList();
            return reval;
        }



        /// <summary>
        /// 复制主题
        /// </summary>
        /// <param name="topiclist">主题id列表</param>
        /// <param name="fid">目标版块id</param>
        /// <returns>更新记录数</returns>
        public static int CopyTopics(string topiclist, int fid)
        {
            if (!Utils.IsNumericList(topiclist))
                return -1;

            int tid;
            int reval = 0;
            TopicInfo topicinfo = null;
            foreach (string topicid in topiclist.Split(','))
            {
                topicinfo = Topics.GetTopicInfo(TypeConverter.StrToInt(topicid));
                if (topicinfo != null)
                {
                    topicinfo.Fid = fid;
                    topicinfo.Readperm = 0;
                    topicinfo.Price = 0;
                    topicinfo.Postdatetime = Utils.GetDateTime();
                    topicinfo.Lastpost = Utils.GetDateTime();
                    topicinfo.Lastposter = Utils.GetDateTime();
                    topicinfo.Views = 0;
                    topicinfo.Replies = 0;
                    topicinfo.Displayorder = 0;
                    topicinfo.Highlight = "";
                    topicinfo.Digest = 0;
                    topicinfo.Rate = 0;
                    topicinfo.Hide = 0;
                    topicinfo.Special = 0;
                    topicinfo.Attachment = 0;
                    topicinfo.Moderated = 0;
                    topicinfo.Closed = 0;
                    tid = Topics.CreateTopic(topicinfo);

                    if (tid > 0)
                    {
                        PostInfo postinfo = Posts.GetPostInfo(tid, Posts.GetFirstPostId(TypeConverter.StrToInt(topicid)));
                        postinfo.Fid = topicinfo.Fid;
                        postinfo.Tid = tid;
                        postinfo.Parentid = 0;
                        postinfo.Layer = 0;
                        postinfo.Postdatetime = Utils.GetDateTime();
                        postinfo.Invisible = 0;
                        postinfo.Attachment = 0;
                        postinfo.Rate = 0;
                        postinfo.Ratetimes = 0;
                        postinfo.Message = UBB.ClearAttachUBB(postinfo.Message);
                        postinfo.Topictitle = topicinfo.Title;

                        if (Posts.CreatePost(postinfo) > 0)
                            reval++;
                    }
                }
            }
            return reval;
        }


        /// <summary>
        /// 分割主题
        /// </summary>
        /// <param name="postidlist">帖子id列表</param>
        /// <param name="subject">主题</param>
        /// <param name="topicId">主题id列表</param>
        /// <returns>更新记录数</returns>
        public static int SplitTopics(string postidlist, string subject, string topicId)
        {
            //验证要分割的帖子是否为有效PID号
            string[] postIdArray = postidlist.Split(',');
            if (Utils.StrIsNullOrEmpty(postidlist) || !Utils.IsNumericArray(postIdArray))
                return -1;

            int tid = 0;
            int lastPostId = TypeConverter.StrToInt(postIdArray[postIdArray.Length - 1]);

            //将要被分割主题的tid	
            TopicInfo originalTopicInfo = Topics.GetTopicInfo(TypeConverter.StrToInt(topicId));  //原主题信息
            TopicInfo newTopicInfo = new TopicInfo();  //新主题信息
            PostInfo lastPostInfo = Posts.GetPostInfo(originalTopicInfo.Tid, lastPostId);
            PostInfo firstPostInfo = Posts.GetPostInfo(originalTopicInfo.Tid, TypeConverter.StrToInt(postIdArray[0]));

            newTopicInfo.Poster = firstPostInfo.Poster;
            newTopicInfo.Posterid = firstPostInfo.Posterid;
            newTopicInfo.Postdatetime = Utils.GetDateTime();
            newTopicInfo.Displayorder = 0;
            newTopicInfo.Highlight = "";
            newTopicInfo.Digest = 0;
            newTopicInfo.Rate = 0;
            newTopicInfo.Hide = 0;
            newTopicInfo.Special = 0;
            newTopicInfo.Attachment = 0;
            newTopicInfo.Moderated = 0;
            newTopicInfo.Closed = 0;
            newTopicInfo.Views = 0;
            newTopicInfo.Fid = originalTopicInfo.Fid;
            newTopicInfo.Forumname = originalTopicInfo.Forumname;
            newTopicInfo.Iconid = originalTopicInfo.Iconid;
            newTopicInfo.Typeid = originalTopicInfo.Typeid;
            newTopicInfo.Replies = postIdArray.Length - 1;
            newTopicInfo.Title = Utils.HtmlEncode(subject);
            newTopicInfo.Lastposterid = lastPostInfo.Posterid;
            newTopicInfo.Lastpost = lastPostInfo.Postdatetime;
            newTopicInfo.Lastposter = lastPostInfo.Poster;

            tid = Topics.CreateTopic(newTopicInfo);
            DatabaseProvider.GetInstance().UpdatePostTid(postidlist, tid, Data.PostTables.GetPostTableId(tid));
            DatabaseProvider.GetInstance().SetPrimaryPost(subject, tid, postIdArray, Discuz.Data.PostTables.GetPostTableId(tid));

            newTopicInfo.Tid = tid;
            newTopicInfo.Lastpostid = lastPostId;
            if (originalTopicInfo.Lastpostid == lastPostId)//当需要将原主题的最后一个发帖分割走时(即分割列表中有和原主题Lastpostid相同的值)
            {
                newTopicInfo.Lastposterid = originalTopicInfo.Posterid;
                newTopicInfo.Lastpost = originalTopicInfo.Lastpost;
                newTopicInfo.Lastposter = originalTopicInfo.Poster;
                DataTable dt = DatabaseProvider.GetInstance().GetLastPostNotInPidList(postidlist, originalTopicInfo.Tid, int.Parse(Posts.GetPostTableId()));
                originalTopicInfo.Lastpostid = TypeConverter.ObjectToInt(dt.Rows[0]["pid"]);
                originalTopicInfo.Lastposterid = TypeConverter.ObjectToInt(dt.Rows[0]["Posterid"].ToString());
                originalTopicInfo.Lastpost = dt.Rows[0]["Postdatetime"].ToString();
                originalTopicInfo.Lastposter = dt.Rows[0]["Poster"].ToString();
            }
            originalTopicInfo.Replies = originalTopicInfo.Replies - postIdArray.Length;

            Topics.UpdateTopic(originalTopicInfo);//更新原主题的信息
            Topics.UpdateTopicReplyCount(originalTopicInfo.Tid);

            Topics.UpdateTopic(newTopicInfo);//由于数据库中对lastpostid有list约束，所以不能有重复值，则必须在原主题的lastpostid修改之后再次修改才能将数据最终修正完毕
            Topics.UpdateTopicReplyCount(tid);

            return tid;
        }

        /// <summary>
        /// 合并主题
        /// </summary>
        /// <param name="topiclist">主题id列表</param>
        /// <param name="othertid">被合并tid</param>
        /// <returns>更新记录数</returns>
        public static int MerrgeTopics(string topicId, int othertid)
        {
            int tid = TypeConverter.StrToInt(topicId);
            int reval = 0;
            //获得要被合并的主题的信息
            TopicInfo topicinfo = Topics.GetTopicInfo(othertid);
            TopicInfo newTopicInfo = Topics.GetTopicInfo(tid);

            //TODO: Move them to posts
            Discuz.Data.TopicAdmins.UpdatePostTidToAnotherTopic(othertid, tid);
            Discuz.Data.TopicAdmins.UpdatePostTidToAnotherTopic(tid, tid);
            //更新附件从属
            Discuz.Data.TopicAdmins.UpdateAttachmentTidToAnotherTopic(othertid, tid);

            reval = Discuz.Data.Topics.DeleteTopic(othertid);

            if (topicinfo != null)
            {
                if (newTopicInfo.Lastpostid < topicinfo.Lastpostid)
                {
                    newTopicInfo.Lastpostid = topicinfo.Lastpostid;
                    newTopicInfo.Lastposterid = topicinfo.Lastposterid;
                    newTopicInfo.Lastpost = topicinfo.Lastpost;
                    newTopicInfo.Lastposter = topicinfo.Lastposter;
                    newTopicInfo.Replies += topicinfo.Replies;
                }
                else
                    newTopicInfo.Replies += topicinfo.Replies;
            }

            //更新主题信息
            PostInfo topicPost = Posts.GetPostInfo(tid, Posts.GetFirstPostId(tid));
            Discuz.Data.Topics.SetPrimaryPost(topicPost.Title, tid, new string[] { topicPost.Pid.ToString() });

            newTopicInfo.Title = topicPost.Title;
            newTopicInfo.Posterid = topicPost.Posterid;
            newTopicInfo.Poster = topicPost.Poster;
            Topics.UpdateTopic(newTopicInfo);

            if (topicinfo.Lastpostid == 0)
                Discuz.Data.Topics.UpdateTopicLastPosterId(topicinfo.Tid);

            if (newTopicInfo.Lastpostid == 0)
                Discuz.Data.Topics.UpdateTopicLastPosterId(newTopicInfo.Tid);

            return reval;
        }

        /// <summary>
        /// 修复主题列表
        /// </summary>
        /// <param name="topicList">主题id列表</param>
        /// <returns>更新记录数</returns>
        public static int RepairTopicList(string topicList)
        {
            if (!Utils.IsNumericList(topicList))
                return 0;

            int revalcount = 0;
            string[] idlist = Posts.GetPostTableIdArray(topicList);
            string[] tidlist = topicList.Split(',');
            for (int i = 0; i < idlist.Length; i++)
            {
                int reval = Discuz.Data.TopicAdmins.RepairTopics(tidlist[i], BaseConfigs.GetTablePrefix + "posts" + (TypeConverter.StrToInt(idlist[i])));
                if (reval > 0)
                {
                    revalcount = reval + revalcount;
                    Attachments.UpdateTopicAttachment(topicList);
                }
            }
            return revalcount;
        }

        /// <summary>
        /// 根据得到给定帖子id的用户列表
        /// </summary>
        /// <param name="postlist">帖子列表</param>
        /// <returns>用户列表</returns>
        private static string GetUserListWithPostlist(int tid, string postList)
        {
            if (!Utils.IsNumericList(postList))
                return "";

            return Discuz.Data.TopicAdmins.GetUserListWithPostlist(tid, postList);
        }

        /// <summary>
        /// 给指定帖子评分
        /// </summary>
        /// <param name="postidlist">帖子列表</param>
        /// <param name="score">要加／减的分值列表</param>
        /// <param name="extcredits">对应的扩展积分列表</param>
        /// <returns>更新数量</returns>
        public static int RatePosts(int tid, string postidlist, string score, string extcredits, int userid, string username, string reason)
        {
            if (!Utils.IsNumericList(postidlist))
                return 0;

            float[] extcreditslist = new float[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            string[] tmpScorelist = Utils.SplitString(score, ",");
            string[] tmpExtcreditslist = Utils.SplitString(extcredits, ",");
            int tempExtc = 0;
            string posttableid = Data.PostTables.GetPostTableId(tid);
            for (int i = 0; i < tmpExtcreditslist.Length; i++)
            {
                tempExtc = TypeConverter.StrToInt(tmpExtcreditslist[i], -1);
                if (tempExtc > 0 && tempExtc < extcreditslist.Length)
                {
                    extcreditslist[tempExtc - 1] = TypeConverter.StrToInt(tmpScorelist[i]);

                    //更新相应帖子的积分数
                    foreach (string pid in Utils.SplitString(postidlist, ","))
                    {
                        if (pid.Trim() != string.Empty)
                        {
                            SetPostRate(posttableid,
                                        TypeConverter.StrToInt(pid),
                                        TypeConverter.StrToInt(tmpExtcreditslist[i]),
                                        TypeConverter.StrToFloat(tmpScorelist[i]),
                                        true);
                        }
                    }
                    AdminRateLogs.InsertLog(postidlist,
                                            userid,
                                            username,
                                            tempExtc,
                                            TypeConverter.StrToFloat(tmpScorelist[i]),
                                            reason);
                }
            }
            return UserCredits.UpdateUserExtCredits(GetUserListWithPostlist(tid, postidlist), extcreditslist);
        }


        /// <summary>
        /// 用当前的评分值通过一定兑换比例换算成积分后，更新相应帖子中的rate字段.
        /// </summary>
        /// <param name="postid">帖子ID</param>
        /// <param name="extid">扩展积分ID</param>
        /// <param name="score">分数</param>
        /// <param name="israte">true为评分，false为撤消评分</param>
        public static void SetPostRate(string posttableid, int postid, int extid, float score, bool israte)
        {
            if (score == 0)
                return;
            float rate = israte ? score : -1 * score;

            Discuz.Data.Posts.UpdatePostRate(postid, rate, posttableid);
            PostInfo postInfo = Discuz.Data.Posts.GetPostInfo(posttableid, postid);

            if (postInfo != null && postInfo.Layer == 0)
                Discuz.Data.TopicAdmins.SetTopicStatus(postInfo.Tid.ToString(), "rate", postInfo.Rate.ToString());
        }

        /// <summary>
        /// 检查评分状态
        /// </summary>
        /// <param name="postidlist">帖子id列表</param>
        /// <param name="userid">用户id</param>
        /// <returns>被评分的帖子id字符串</returns>
        public static string CheckRateState(string postidlist, int userid)
        {
            if (!Utils.IsNumericList(postidlist))
                return "";

            string reval = "";
            string tempreval = "";
            foreach (string pid in Utils.SplitString(postidlist, ","))
            {
                tempreval = Discuz.Data.TopicAdmins.CheckRateState(userid, pid);
                if (!Utils.StrIsNullOrEmpty(tempreval))
                {
                    if (!Utils.StrIsNullOrEmpty(reval))
                    {
                        reval = reval + ",";
                    }
                    reval = reval + tempreval;
                }
            }
            return reval;
        }


        /// <summary>
        /// 返回指定主题的最后一次操作
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>管理日志内容</returns>
        public static string GetTopicListModeratorLog(int tid)
        {
            return Discuz.Data.TopicAdmins.GetTopicListModeratorLog(tid);
        }



        /// <summary>
        /// 重设主题类型
        /// </summary>
        /// <param name="topictypeid">主题类型</param>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <returns></returns>
        public static int ResetTopicTypes(int topictypeid, string topiclist)
        {
            return Discuz.Data.TopicAdmins.ResetTopicTypes(topictypeid, topiclist);
        }


        public static void IdentifyTopic(string topiclist, int identify)
        {
            if (!Utils.IsNumericList(topiclist))
                return;

            Discuz.Data.TopicAdmins.IdentifyTopic(topiclist, identify);
        }

        /// <summary>
        /// 撤消评分
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="postidlist"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="reason"></param>
        public static void CancelRatePosts(string ratelogidlist, int tid, string pid, int userid, string username, int groupid, string grouptitle, int forumid, string forumname, string reason)
        {
            if (!Utils.IsNumeric(pid))
                return;

            int rateduserid = Posts.GetPostInfo(tid, Utils.StrToInt(pid, 0)).Posterid; //被评分的用户的UID
            if (rateduserid <= 0)
                return;

            string posttableid = Data.PostTables.GetPostTableId(tid);
            DataTable dt = AdminRateLogs.LogList(ratelogidlist.Split(',').Length, 1, "id IN(" + ratelogidlist + ")");//得到要删除的评分日志列表
            foreach (DataRow dr in dt.Rows)
            {
                SetPostRate(posttableid,
                             TypeConverter.StrToInt(pid),
                             TypeConverter.ObjectToInt(dr["extcredits"]),
                             TypeConverter.ObjectToInt(dr["score"]),
                             false);

                //乘-1是要进行分值的反向操作
                Discuz.Data.Users.UpdateUserExtCredits(rateduserid, TypeConverter.ObjectToInt(dr["extcredits"]), (-1) * TypeConverter.ObjectToFloat(dr["score"]));
            }

            AdminRateLogs.DeleteLog("[id] IN(" + ratelogidlist + ")");

            //当帖子已无评分记录时，则清空帖子相关的评分信息字段(rate,ratetimes)
            if (AdminRateLogs.LogList(1, 1, "pid = " + pid).Rows.Count == 0)
                Discuz.Data.Posts.CancelPostRate(pid, posttableid);

            TopicInfo topicinfo = Topics.GetTopicInfo(tid);

            Discuz.Data.ModeratorManageLog.InsertModeratorLog(userid.ToString(),
                                                              username,
                                                              groupid,
                                                              grouptitle,
                                                              Utils.GetRealIP(),
                                                              Utils.GetDateTime(),
                                                              forumid.ToString(),
                                                              forumname,
                                                              tid.ToString(),
                                                              topicinfo == null ? "暂无标题" : topicinfo.Title,
                                                              "撤消评分",
                                                              reason);
        }

        /// <summary>
        /// 提升/下沉主题的权限
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="bumptype"></param>
        public static void BumpTopics(string topiclist, int bumptype)
        {
            if (!Utils.IsNumericList(topiclist))
                return;

            if (bumptype == 1)
            {
                foreach (string tid in topiclist.Split(','))
                {
                    Discuz.Data.TopicAdmins.SetTopicsBump(tid, Discuz.Data.TopicAdmins.GetPostId());
                }
            }
            else
                Discuz.Data.TopicAdmins.SetTopicsBump(topiclist, 0);
        }


    } //class end
}