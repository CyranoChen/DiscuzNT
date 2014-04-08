using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// AdminTopicFactory 
    /// </summary>
    public class AdminTopics : TopicAdmins
    {
        public AdminTopics()
        { }


        /// <summary>
        ///
        /// </summary>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        public static bool UpdateTopicAllInfo(TopicInfo topicinfo)
        {
            try
            {
                Topics.UpdateTopic(topicinfo);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static bool DeleteTopicByTid(int tid)
        {
            return Discuz.Data.Posts.DeleteTopicByTid(tid, Posts.GetPostTableName());
        }


        public static bool SetTypeid(string topiclist, int value)
        {
            return Discuz.Data.Topics.SetTypeid(topiclist, value);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static DataSet AdminGetPostList(int tid, int pagesize, int pageindex)
        {
            DataSet ds = Discuz.Data.Posts.GetPosts(tid, pagesize, pageindex, Discuz.Data.PostTables.GetPostTableId(tid));

            if (ds == null)
            {
                ds = new DataSet();
                ds.Tables.Add("post");
                ds.Tables.Add();
                return ds;
            }
            ds.Tables[0].TableName = "post";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["attachment"].ToString().Equals("1"))
                    dr["attachment"] = Attachments.GetAttachmentCountByPid(Utils.StrToInt(dr["pid"], 0));
            }
            return ds;
        }

        //public static DataTable GetUnauditNewTopic()
        //{
        //    return Discuz.Data.Topics.GetUnauditNewTopic();
        //}

        /// <summary>
        /// ��ȡδ��˵�����
        /// </summary>
        /// <param name="currentPostTableId">�ֱ�ID</param>
        /// <returns></returns>
        //public static DataTable GetUnauditPost(int currentPostTableId)
        //{
        //    return Discuz.Data.Posts.GetUnauditPost(currentPostTableId);
        //}

        /// <summary>
        /// �����ƶ�����
        /// </summary>
        /// <param name="tidList">�ƶ�������Id�б�</param>
        /// <param name="targetForumId">Ŀ����Id</param>
        /// <param name="adminUid">����ԱUid</param>
        /// <param name="adminUserName">����Ա�û���</param>
        /// <param name="adminUserGroupId">����Ա�û���Id</param>
        /// <param name="adminUserGroupTitle">����Ա�û�������</param>
        /// <param name="adminIp">����ԱIp</param>
        public static void BatchMoveTopics(string tidList, int targetForumId, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            //���ҳ���ǰ�����б���������FID
            foreach (DataRow olddr in Data.Topics.GetTopicFidByTid(tidList).Rows)
            {
                string oldtidlist = "0";
                //��FID���б�Ϊ�����г��ڵ�ǰFID�µ������б�
                foreach (DataRow mydr in Data.Topics.GetTopicTidByFid(tidList, int.Parse(olddr["fid"].ToString())).Rows)
                {
                    oldtidlist += "," + mydr["tid"].ToString();
                }
                //����ǰ̨��������,��̨��ʱ��֧���ƶ����ⶨ���������
                TopicAdmins.MoveTopics(oldtidlist, targetForumId, Convert.ToInt16(olddr["fid"].ToString()), 0);
            }
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "�����ƶ�����", "����ID:" + tidList + " <br />Ŀ����̳fid:" + targetForumId);
        }

        /// <summary>
        /// ����ɾ������
        /// </summary>
        /// <param name="tidList">����Id�б�</param>
        /// <param name="isChagePostNumAndCredits">�Ƿ�Ҫ�����û��������������</param>
        /// <param name="adminUid">����ԱUid</param>
        /// <param name="adminUserName">����Ա�û���</param>
        /// <param name="adminUserGroupId">����Ա�û���Id</param>
        /// <param name="adminUserGroupTitle">����Ա�û�������</param>
        /// <param name="adminIp">����ԱIp</param>
        public static void BatchDeleteTopics(string tidList, bool isChagePostNumAndCredits, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            DeleteTopics(tidList, isChagePostNumAndCredits ? 1 : 0, false);
            Attachments.UpdateTopicAttachment(tidList);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "����ɾ������", "����ID:" + tidList);
        }

        /// <summary>
        /// ���������ö�
        /// </summary>
        /// <param name="tidList">����Id�б�</param>
        /// <param name="displayOrder">�ö�����</param>
        /// <param name="adminUid">����ԱUid</param>
        /// <param name="adminUserName">����Ա�û���</param>
        /// <param name="adminUserGroupId">����Ա�û���Id</param>
        /// <param name="adminUserGroupTitle">����Ա�û�������</param>
        /// <param name="adminIp">����ԱIp</param>
        public static void BatchChangeTopicsDisplayOrderLevel(string tidList, int displayOrderLevel, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            Data.Topics.SetDisplayorder(tidList, displayOrderLevel);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "�����ö�����", "����ID:" + tidList + "<br /> �ö���Ϊ:" + displayOrderLevel);
        }

        /// <summary>
        /// �����������⾫��
        /// </summary>
        /// <param name="tidList">����Id�б�</param>
        /// <param name="digestLevel">��������</param>
        /// <param name="adminUid">����ԱUid</param>
        /// <param name="adminUserName">����Ա�û���</param>
        /// <param name="adminUserGroupId">����Ա�û���Id</param>
        /// <param name="adminUserGroupTitle">����Ա�û�������</param>
        /// <param name="adminIp">����ԱIp</param>
        public static void BatchChangeTopicsDigest(string tidList, int digestLevel, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            TopicAdmins.SetDigest(tidList, digestLevel);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "�����Ӿ�����", "����ID:" + tidList + "<br /> �Ӿ���Ϊ:" + digestLevel);
        }

        /// <summary>
        /// ����ɾ�����⸽��
        /// </summary>
        /// <param name="tidList">����Id�б�</param>
        /// <param name="adminUid">����ԱUid</param>
        /// <param name="adminUserName">����Ա�û���</param>
        /// <param name="adminUserGroupId">����Ա�û���Id</param>
        /// <param name="adminUserGroupTitle">����Ա�û�������</param>
        /// <param name="adminIp">����ԱIp</param>
        public static void BatchDeleteTopicAttachs(string tidList, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            Attachments.DeleteAttachmentByTid(tidList);
            AdminVistLogs.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "ɾ�������еĸ���", "����ID:" + tidList);
        }
    }
}
