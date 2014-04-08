using System;
using System.Data;
using Discuz.Common;

using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// �ҵ�����
    /// </summary>
    public class mytopics : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// �����б�
        /// </summary>
        public List<TopicInfo> topics;
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int pagecount;
        /// <summary>
        /// ��������
        /// </summary>
        public int topiccount;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// ��ǰ��¼���û���Ϣ
        /// </summary>
        public UserInfo user = new UserInfo();
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "�û��������";

            if (userid == -1)
            {
                AddErrLine("����δ��¼");
                return;
            }

            user = Users.GetUserInfo(userid);
            //�õ���ǰ�û������ҳ��
            //��ȡ��������
            topiccount = Topics.GetTopicsCountbyUserId(userid, false);
            //��ȡ��ҳ��
            pagecount = topiccount % 16 == 0 ? topiccount / 16 : topiccount / 16 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;

            //��������ҳ���п��ܵĴ���
            pageid = pageid < 1 ? 1 : pageid;
            pageid = pageid > pagecount ?  pagecount : pageid;

            topics = Topics.GetTopicsByUserId(userid, pageid, 16, 600, config.Hottopic);
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "mytopics.aspx", 8);
        }
    }
}