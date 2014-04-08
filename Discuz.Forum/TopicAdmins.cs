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
    /// ������������
    /// </summary>
    public class TopicAdmins
    {
        /// <summary>
        /// ��������ָ���ֶε�����ֵ
        /// </summary>
        /// <param name="topiclist">Ҫ���õ������б�</param>
        /// <param name="field">Ҫ���õ��ֶ�</param>
        /// <param name="intValue">����ֵ</param>
        /// <returns>�����������</returns>
        private static int SetTopicStatus(string topiclist, string field, int intValue)
        {
            return SetTopicStatus(topiclist, field, intValue.ToString());
        }


        /// <summary>
        /// ��������ָ���ֶε�����ֵ
        /// </summary>
        /// <param name="topiclist">Ҫ���õ������б�</param>
        /// <param name="field">Ҫ���õ��ֶ�</param>
        /// <param name="intValue">����ֵ</param>
        /// <returns>�����������</returns>
        private static int SetTopicStatus(string topiclist, string field, byte intValue)
        {
            return SetTopicStatus(topiclist, field, intValue.ToString());
        }


        /// <summary>
        /// ��������ָ���ֶε�����ֵ(�ַ���)
        /// </summary>
        /// <param name="topiclist">Ҫ���õ������б�</param>
        /// <param name="field">Ҫ���õ��ֶ�</param>
        /// <param name="intValue">����ֵ</param>
        /// <returns>�����������</returns>
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
        /// �������ö�/����ö�
        /// </summary>
        /// <param name="topiclist">Ҫ���õ������б�</param>
        /// <param name="intValue">�ö�����( 0 Ϊ����ö�)</param>
        /// <returns>�����������</returns>
        public static int SetTopTopicList(int fid, string topiclist, short intValue)
        {
            //ֻ����Ӧ��memcached������²ſ���ʹ�����⻺��
            if ((mcci != null && mcci.ApplyMemCached) || (rci != null && rci.ApplyRedis))
            {
                //��Ϊ���ǵ�ĳЩ�ö�������ȫ���ö���������һ�������ö�����������������ö�������Ϣ
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
        /// ���������ö�����
        /// </summary>
        /// <param name="fid">����ID</param>
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
        /// �����������ʾ
        /// </summary>
        /// <param name="topiclist">Ҫ���õ������б�</param>
        /// <param name="intValue">������ʽ����ɫ( 0 Ϊ���������ʾ)</param>
        /// <returns>�����������</returns>
        public static int SetHighlight(string topiclist, string intValue)
        {
            return SetTopicStatus(topiclist, "highlight", intValue);
        }

        /// <summary>
        /// ���ݵõ�����������û��б�
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <param name="digestType">����Դ(0:����,1:ɾ��)</param>
        /// <returns>�û��б�</returns>
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
        /// ���������þ���/�������
        /// </summary>
        /// <param name="topiclist">Ҫ���õ������б�</param>
        /// <param name="intValue">��������( 0 Ϊ�������)</param>
        /// <returns>�����������</returns>
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
        /// ���������ùر�/��
        /// </summary>
        /// <param name="topiclist">Ҫ���õ������б�</param>
        /// <param name="intValue">�ر�/�򿪱�־( 0 Ϊ��,1 Ϊ�ر�)</param>
        /// <returns>�����������</returns>
        public static int SetClose(string topiclist, short intValue)
        {
            if (!Utils.IsNumericList(topiclist))
                return -1;

            return Discuz.Data.TopicAdmins.SetTopicClose(topiclist, intValue);
        }



        /// <summary>
        /// �������ָ���ֶε�����ֵ
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <param name="field">Ҫ���ֵ���ֶ�</param>
        /// <returns>����ָ���ֶε�״̬</returns>
        public static int GetTopicStatus(string topiclist, string field)
        {
            if (!Utils.IsNumericList(topiclist) ||
                (",displayorder,digest,").IndexOf("," + field.ToLower().Trim() + ",") < 0)
                return -1;

            return DatabaseProvider.GetInstance().GetTopicStatus(topiclist, field);
        }


        /// <summary>
        /// ��������ö�״̬
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <returns>�ö�״̬(�������ⷵ����ʵ״̬,������ⷵ��״ֵ̬�ۼ�)</returns>
        public static int GetDisplayorder(string topiclist)
        {
            return GetTopicStatus(topiclist, "displayorder");
        }


        /// <summary>
        /// ������⾫��״̬
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <returns>����״̬(�������ⷵ����ʵ״̬,������ⷵ��״ֵ̬�ۼ�)</returns>
        public static int GetDigest(string topiclist)
        {
            return GetTopicStatus(topiclist, "digest");
        }


        /// <summary>
        /// �����ݿ���ɾ��ָ������
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <param name="subtractCredits">�Ƿ�����û�����(0������,1����)</param>
        /// <returns>ɾ������</returns>
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
                    //��̨���õ���Ϊ�������������ɾ���������֣������Ƕ�������ɾ�����Բ�����
                    if (configinfo.Losslessdel == 0 || Utils.StrDateDiffHours(dr["postdatetime"].ToString(), configinfo.Losslessdel * 24) < 0)
                    {
                        CreditsOperationType creditsOperationType = TypeConverter.ObjectToInt(dr["layer"]) == 0 ? CreditsOperationType.PostTopic : CreditsOperationType.PostReply;
                        //��ȡ�����ֹ���
                        float[] creditsValue = Forums.GetValues(
                            creditsOperationType == CreditsOperationType.PostTopic ? 
                            Forums.GetForumInfo(TypeConverter.ObjectToInt(dr["fid"])).Postcredits : 
                            Forums.GetForumInfo(TypeConverter.ObjectToInt(dr["fid"])).Replycredits
                            );

                        //���δ��������ֹ���
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
        //            //��̨���õ���Ϊ�������������ɾ���������֣������Ƕ�������ɾ�����Բ�����
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
        /// ɾ�����Ⲣ�Ҹı����
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <param name="reserveAttach">�Ƿ�������</param>
        /// <returns>ɾ��������</returns>
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
        /// �����ݿ���ɾ��ָ������
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <returns>ɾ������</returns>
        public static int DeleteTopics(string topiclist, bool reserveAttach)
        {
            return DeleteTopics(topiclist, 1, reserveAttach);
        }

        /// <summary>
        /// ��ɾ��ָ��������
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <param name="toDustbin">ָ������ɾ����ʽ(0��ֱ�Ӵ����ݿ���ɾ��,��ɾ����֮��������Ϣ  1��ֻ�������̳�б���ɾ��(��displayorder�ֶ���Ϊ-1)����������վ��</param>
        /// <returns>ɾ������</returns>
        public static int DeleteTopics(string topiclist, byte toDustbin, bool reserveAttach)
        {
            return toDustbin == 0 ? DeleteTopics(topiclist, reserveAttach) : SetTopicStatus(topiclist, "displayorder", -1);
        }


        /// <summary>
        /// �ָ�����վ�е����⡣
        /// </summary>
        /// <param name="topiclist">�����б�</param>
        /// <returns>�ָ�����</returns>
        public static int RestoreTopics(string topiclist)
        {
            return SetTopicStatus(topiclist, "displayorder", 0);
        }


        /// <summary>
        /// �ƶ����⵽ָ�����
        /// </summary>
        /// <param name="topiclist">Ҫ�ƶ��������б�</param>
        /// <param name="fid">ת���İ��ID</param>
        /// <param name="savelink">�Ƿ���ԭ��鱣������</param>
        /// <returns>���¼�¼��</returns>
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

            //ת������
            MoveTopics(tidList, fid, oldfid, topicType);

            //���������������һ����¼��ԭ���
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
        /// �ƶ����⵽ָ�����
        /// </summary>
        /// <param name="topiclist">Ҫ�ƶ��������б�</param>
        /// <param name="fid">ת���İ��ID</param>
        /// <returns>���¼�¼��</returns>
        public static int MoveTopics(string topiclist, int fid, int oldfid, int topicType)
        {
            if (!Utils.IsNumericList(topiclist))
                return -1;

            //��������
            foreach (string tid in topiclist.Split(','))
            {
                    DatabaseProvider.GetInstance().UpdatePost(topiclist, fid, PostTables.GetPostTableName(TypeConverter.StrToInt(tid)));
            }

            //��������
            int reval = Discuz.Data.Topics.UpdateTopic(topiclist, fid, topicType);
            if (reval > 0)
            {
                AdminForumStats.ReSetFourmTopicAPost(fid);
                AdminForumStats.ReSetFourmTopicAPost(oldfid);
                Forums.SetRealCurrentTopics(fid);
                Forums.SetRealCurrentTopics(oldfid);
            }

            //�����ö���
            ResetTopTopicList();
            return reval;
        }



        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="topiclist">����id�б�</param>
        /// <param name="fid">Ŀ����id</param>
        /// <returns>���¼�¼��</returns>
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
        /// �ָ�����
        /// </summary>
        /// <param name="postidlist">����id�б�</param>
        /// <param name="subject">����</param>
        /// <param name="topicId">����id�б�</param>
        /// <returns>���¼�¼��</returns>
        public static int SplitTopics(string postidlist, string subject, string topicId)
        {
            //��֤Ҫ�ָ�������Ƿ�Ϊ��ЧPID��
            string[] postIdArray = postidlist.Split(',');
            if (Utils.StrIsNullOrEmpty(postidlist) || !Utils.IsNumericArray(postIdArray))
                return -1;

            int tid = 0;
            int lastPostId = TypeConverter.StrToInt(postIdArray[postIdArray.Length - 1]);

            //��Ҫ���ָ������tid	
            TopicInfo originalTopicInfo = Topics.GetTopicInfo(TypeConverter.StrToInt(topicId));  //ԭ������Ϣ
            TopicInfo newTopicInfo = new TopicInfo();  //��������Ϣ
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
            if (originalTopicInfo.Lastpostid == lastPostId)//����Ҫ��ԭ��������һ�������ָ���ʱ(���ָ��б����к�ԭ����Lastpostid��ͬ��ֵ)
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

            Topics.UpdateTopic(originalTopicInfo);//����ԭ�������Ϣ
            Topics.UpdateTopicReplyCount(originalTopicInfo.Tid);

            Topics.UpdateTopic(newTopicInfo);//�������ݿ��ж�lastpostid��listԼ�������Բ������ظ�ֵ���������ԭ�����lastpostid�޸�֮���ٴ��޸Ĳ��ܽ����������������
            Topics.UpdateTopicReplyCount(tid);

            return tid;
        }

        /// <summary>
        /// �ϲ�����
        /// </summary>
        /// <param name="topiclist">����id�б�</param>
        /// <param name="othertid">���ϲ�tid</param>
        /// <returns>���¼�¼��</returns>
        public static int MerrgeTopics(string topicId, int othertid)
        {
            int tid = TypeConverter.StrToInt(topicId);
            int reval = 0;
            //���Ҫ���ϲ����������Ϣ
            TopicInfo topicinfo = Topics.GetTopicInfo(othertid);
            TopicInfo newTopicInfo = Topics.GetTopicInfo(tid);

            //TODO: Move them to posts
            Discuz.Data.TopicAdmins.UpdatePostTidToAnotherTopic(othertid, tid);
            Discuz.Data.TopicAdmins.UpdatePostTidToAnotherTopic(tid, tid);
            //���¸�������
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

            //����������Ϣ
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
        /// �޸������б�
        /// </summary>
        /// <param name="topicList">����id�б�</param>
        /// <returns>���¼�¼��</returns>
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
        /// ���ݵõ���������id���û��б�
        /// </summary>
        /// <param name="postlist">�����б�</param>
        /// <returns>�û��б�</returns>
        private static string GetUserListWithPostlist(int tid, string postList)
        {
            if (!Utils.IsNumericList(postList))
                return "";

            return Discuz.Data.TopicAdmins.GetUserListWithPostlist(tid, postList);
        }

        /// <summary>
        /// ��ָ����������
        /// </summary>
        /// <param name="postidlist">�����б�</param>
        /// <param name="score">Ҫ�ӣ����ķ�ֵ�б�</param>
        /// <param name="extcredits">��Ӧ����չ�����б�</param>
        /// <returns>��������</returns>
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

                    //������Ӧ���ӵĻ�����
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
        /// �õ�ǰ������ֵͨ��һ���һ���������ɻ��ֺ󣬸�����Ӧ�����е�rate�ֶ�.
        /// </summary>
        /// <param name="postid">����ID</param>
        /// <param name="extid">��չ����ID</param>
        /// <param name="score">����</param>
        /// <param name="israte">trueΪ���֣�falseΪ��������</param>
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
        /// �������״̬
        /// </summary>
        /// <param name="postidlist">����id�б�</param>
        /// <param name="userid">�û�id</param>
        /// <returns>�����ֵ�����id�ַ���</returns>
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
        /// ����ָ����������һ�β���
        /// </summary>
        /// <param name="tid">����id</param>
        /// <returns>������־����</returns>
        public static string GetTopicListModeratorLog(int tid)
        {
            return Discuz.Data.TopicAdmins.GetTopicListModeratorLog(tid);
        }



        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="topictypeid">��������</param>
        /// <param name="topiclist">Ҫ���õ������б�</param>
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
        /// ��������
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

            int rateduserid = Posts.GetPostInfo(tid, Utils.StrToInt(pid, 0)).Posterid; //�����ֵ��û���UID
            if (rateduserid <= 0)
                return;

            string posttableid = Data.PostTables.GetPostTableId(tid);
            DataTable dt = AdminRateLogs.LogList(ratelogidlist.Split(',').Length, 1, "id IN(" + ratelogidlist + ")");//�õ�Ҫɾ����������־�б�
            foreach (DataRow dr in dt.Rows)
            {
                SetPostRate(posttableid,
                             TypeConverter.StrToInt(pid),
                             TypeConverter.ObjectToInt(dr["extcredits"]),
                             TypeConverter.ObjectToInt(dr["score"]),
                             false);

                //��-1��Ҫ���з�ֵ�ķ������
                Discuz.Data.Users.UpdateUserExtCredits(rateduserid, TypeConverter.ObjectToInt(dr["extcredits"]), (-1) * TypeConverter.ObjectToFloat(dr["score"]));
            }

            AdminRateLogs.DeleteLog("[id] IN(" + ratelogidlist + ")");

            //�������������ּ�¼ʱ�������������ص�������Ϣ�ֶ�(rate,ratetimes)
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
                                                              topicinfo == null ? "���ޱ���" : topicinfo.Title,
                                                              "��������",
                                                              reason);
        }

        /// <summary>
        /// ����/�³������Ȩ��
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