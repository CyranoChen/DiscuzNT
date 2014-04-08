using System;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Data;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    public class Debates
    {
        /// <summary>
        /// ��ȡ���ӹ۵�
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <returns>Dictionary����</returns>
        public static Dictionary<int, int> GetPostDebateList(int tid)
        {
            if (tid <= 0)
                return null;

            return Discuz.Data.Debates.GetPostDebateList(tid);
        }

        /// <summary>
        /// ��ȡ���۵���չ��Ϣ
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <returns>����������չ��Ϣ</returns>
        public static DebateInfo GetDebateTopic(int tid)
        {
            if (tid <= 0)
                return null;

            return Discuz.Data.Debates.GetDebateTopic(tid);
        }

        /// <summary>
        /// ���±�����Ϣ
        /// </summary>
        /// <param name="debateInfo">������Ϣ</param>
        /// <returns></returns>
        public static bool UpdateDebateTopic(DebateInfo debateInfo)
        {
            if (debateInfo.Tid <= 0)
                return false;
            return Discuz.Data.Debates.UpdateDebateTopic(debateInfo);
        }

        /// <summary>
        /// ���ص��õ�JSON����
        /// </summary>
        /// <param name="callback">JS�ص�����</param>
        /// <param name="tidlist">����ID�б�</param>
        /// <returns>JS����</returns>
        public static string GetDebatesJsonList(string callback, string tidlist)
        {
            switch (callback)
            {
                //��ȡ������Ϣ
                case "gethotdebatetopic":
                    {
                        string[] debatesrule = Utils.StrIsNullOrEmpty(tidlist) ? new string[0] : tidlist.Split(',');

                        if (debatesrule.Length < 3)
                            break;
                        else if (debatesrule[0] != "views" && debatesrule[0] != "replies" && Utils.IsNumeric(debatesrule[1]) && Utils.IsNumeric(debatesrule[2]))
                            break;

                        return Discuz.Data.Debates.GetDebatesJsonList(callback, debatesrule[0], TypeConverter.StrToInt(debatesrule[1]), TypeConverter.StrToInt(debatesrule[2]));
                    }

                //��ȡ�Ƽ�����������Ϣ
                case "recommenddebates":
                    {
                        if (!Utils.IsNumericList(tidlist))
                            break;

                        if (Utils.StrIsNullOrEmpty(tidlist))
                            tidlist = GeneralConfigs.GetConfig().Recommenddebates;

                        return Discuz.Data.Debates.GetRecommendDebates(callback, tidlist);
                    }

                default:
                    break;
            }
            return "0";
        }
        /// <summary>
        /// ��ӵ���
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <param name="message">��������</param>
        public static StringBuilder CommentDabetas(int tid, string message, bool ispost)
        {
            StringBuilder xmlnode = IsValidDebates(tid, message, ispost);
            if (!xmlnode.ToString().Contains("<error>"))
            {
                xmlnode.Append("<message>" + message + "</message>");
                Discuz.Data.Debates.CommentDabetas(tid, TypeConverter.ObjectToInt(Data.PostTables.GetPostTableId(tid)), Utils.HtmlEncode(ForumUtils.BanWordFilter(message)));
            }
            return xmlnode;
        }

        private static StringBuilder IsValidDebates(int tid, string message, bool ispost)
        {
            StringBuilder xmlnode = new StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>����������·����ȷ���޷��ύ���������װ��ĳ��Ĭ��������·��Ϣ�ĸ��˷���ǽ���(�� Norton Internet Security)���������䲻Ҫ��ֹ��·��Ϣ�����ԡ�</error>");
                return xmlnode;
            }

            Regex r = new Regex(@"\[area=([\s\S]+?)\]([\s\S]+?)\[/area\]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(message);
            if (m.Count == 0)
            {
                xmlnode.Append("<error>�������ݲ���Ϊ��</error>");
                return xmlnode;
            }

            TopicInfo topicinfo = Topics.GetTopicInfo(tid);
            if (tid == 0 || topicinfo.Special != 4)
            {
                xmlnode.Append("<error>�����ⲻ�Ǳ��������޷�����</error>");
                return xmlnode;
            }
            if (Debates.GetDebateTopic(tid).Terminaltime > DateTime.Now)
            {
                xmlnode.Append("<error>������������ʱ��δ�����޷�����</error>");
                return xmlnode;
            }
            return xmlnode;
        }


        /// <summary>
        /// ��֤�û����Ƿ�����
        /// </summary>
        /// <param name="userid">�û�id</param>
        /// <param name="tips">��ʾ��Ϣ</param>
        /// <returns>�Ƿ���Զ�</returns>
        public static bool AllowDiggs(int userid)
        {
            //�ж��ο��Ƿ���Զ�
            if (UserGroups.GetUserGroupInfo(7).Allowdiggs == 0 && userid == -1)
                return false;

            //�жϵ�ǰ�û��Ƿ���Զ�
            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(Discuz.Data.Users.GetUserInfo(userid).Groupid);
            if (usergroupinfo.Allowdiggs == 0)
                return false;

            return true;
        }


        /// <summary>
        /// ����Digg
        /// </summary>
        /// <param name="tid">����id</param>
        /// <param name="pid">����ID</param>
        /// <param name="type">�������۵�</param>
        /// <param name="userid">�û�ID</param>
        public static void AddDebateDigg(int tid, int pid, int type, int userid)
        {
            if (userid < 0)
                return;

            UserInfo userinfo = Discuz.Data.Users.GetUserInfo(userid);
            if (userinfo == null)
                return;

            Discuz.Data.Debates.AddDebateDigg(tid, pid, type, Utils.GetRealIP(), userinfo);
        }

        /// <summary>
        /// �ж��Ƿ񶥹�
        /// </summary>
        /// <param name="pid">����ID</param>
        /// <param name="userid">�û�ID</param>
        /// <returns>�ж��Ƿ񶥹�</returns>
        public static bool IsDigged(int pid, int userid)
        {
            //�����οͺ���֤��ʽΪ��ɢ��֤,24Сʱ��ֻ�ܶ�һ��
            if (UserGroups.GetUserGroupInfo(7).Allowdiggs != 1)
                return !DatabaseProvider.GetInstance().AllowDiggs(pid, userid);
            else
            {
                if (Utils.StrIsNullOrEmpty(Utils.GetCookie("debatedigged")))
                    return false;

                foreach (string s in Utils.GetCookie("debatedigged").Split(','))
                {
                    if (pid == Utils.StrToInt(s, 0))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// д���Ѷ���COOKIES
        /// </summary>
        /// <param name="pid">����ID</param>
        public static void WriteCookies(int pid)
        {
            if (Utils.StrIsNullOrEmpty((Utils.GetCookie("debatedigged"))))
                Utils.WriteCookie("debatedigged", pid.ToString(), 1440);
            else
                Utils.WriteCookie("debatedigged", Utils.GetCookie("debatedigged") + "," + pid, 1440);
        }

        /// <summary>
        /// ���ر������������һ����������
        /// </summary>
        /// <param name="postpramsInfo">���ӵĸ�����Ϣ</param>
        /// <param name="debateOpinion">���ӹ۵�</param>
        /// <returns>������</returns>
        public static int GetDebatesPostCount(PostpramsInfo postpramsInfo, int debateOpinion)
        {
            return Discuz.Data.Debates.GetDebatesPostCount(postpramsInfo.Tid, debateOpinion);
        }

        /// <summary>
        /// ��ȡ�������������б�
        /// </summary>
        /// <param name="postpramsInfo">���ӵĸ�����Ϣ</param>
        /// <param name="attachmentlist">�����б�</param>
        /// <param name="ismoder">�Ƿ��й���Ȩ��</param>
        /// <returns>���������б�</returns>
        public static List<ShowtopicPagePostInfo> GetPositivePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 1, new PostOrderType());
        }

        private static List<ShowtopicPagePostInfo> GetDebatePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachList, 
            bool isModer, int debateOpinion, PostOrderType postOrderType)
        {
            List<ShowtopicPagePostInfo> postList = new List<ShowtopicPagePostInfo>();
            attachList = new List<ShowtopicPageAttachmentInfo>();
            StringBuilder attachmentpidlist = new StringBuilder();
            StringBuilder pidList = new StringBuilder();
            postList = Data.Debates.GetDebatePostList(postpramsInfo, debateOpinion, postOrderType);

            //���������ֶβ�׼����δȡ�÷�ҳ��Ϣʱ�����������ֶΣ���ȡ���һҳ
            if (postList.Count == 0 && postpramsInfo.Pageindex > 1)
            {
                int postcount = Data.Debates.GetRealDebatePostCount(postpramsInfo.Tid, debateOpinion);

                postpramsInfo.Pageindex = postcount % postpramsInfo.Pagesize == 0 ? postcount / postpramsInfo.Pagesize : postcount / postpramsInfo.Pagesize + 1;

                postList = Data.Debates.GetDebatePostList(postpramsInfo, debateOpinion, postOrderType);
            }

            StringBuilder attachPidList = new StringBuilder();

            foreach (ShowtopicPagePostInfo post in postList)
            {
                pidList.AppendFormat("{0},", post.Pid);
                if (post.Attachment > 0)
                    attachPidList.AppendFormat("{0},", post.Pid);
            }

            attachList = Attachments.GetAttachmentList(postpramsInfo, attachPidList.ToString().TrimEnd(','));

            Dictionary<int, int> postdiggs = GetPostDiggs(pidList.ToString().Trim(','));
            foreach (ShowtopicPagePostInfo post in postList)
            {
                if (postdiggs.ContainsKey(post.Pid))
                    post.Diggs = postdiggs[post.Pid];
            }

            Posts.ParsePostListExtraInfo(postpramsInfo, attachList, isModer, postList);

            return postList;
        }
        /// <summary>
        /// �������ӱ�����
        /// </summary>
        /// <param name="pidlist">����ID����</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<int, int> GetPostDiggs(string pidlist)
        {
            if (!Utils.IsNumericList(pidlist))
                return new Dictionary<int, int>();

            return Discuz.Data.Debates.GetPostDiggs(pidlist);
        }

        /// <summary>
        /// �����������б�
        /// </summary>
        /// <param name="postpramsInfo">���ӵĸ�����Ϣ</param>
        /// <param name="attachmentlist">�����б�</param>
        /// <param name="ismoder">�Ƿ��й���Ȩ��</param>
        /// <returns>���������б�</returns>
        public static List<ShowtopicPagePostInfo> GetNegativePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 2, new PostOrderType());
        }
     

        /// <summary>
        /// ɾ������������Ϣ
        /// </summary>
        /// <param name="tid">����Id</param>
        /// <param name="opinion">�������ֶΣ�1������ 2������</param>
        /// <param name="pid">����Id</param>
        public static void DeleteDebatePost(int tid, int opinion, int pid)
        {
            switch (opinion)
            {
                case 1: Discuz.Data.Debates.DeleteDebatePost(tid, "positivediggs", pid); break;
                case 2: Discuz.Data.Debates.DeleteDebatePost(tid, "negativediggs", pid); break;
            }
        }
    }
}
