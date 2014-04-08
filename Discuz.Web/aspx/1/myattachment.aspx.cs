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
    public partial class myattachment : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ���������������б�
        /// </summary>
        public List<MyAttachmentInfo> myattachmentlist;
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
        public int attachmentcount;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// ��ǰ��¼���û���Ϣ
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// �ļ�����
        /// </summary>
        public int typeid = DNTRequest.GetInt("typeid", 0);
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
            attachmentcount = typeid > 0 ? Attachments.GetUserAttachmentCount(userid,typeid) : Attachments.GetUserAttachmentCount(userid);

            pagecount = attachmentcount % 16 == 0 ? attachmentcount / 16 : attachmentcount / 16 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;

            //��������ҳ���п��ܵĴ���
            pageid = pageid < 1 ? 1 : pageid;
            pageid = pageid > pagecount ? pagecount :pageid;

            myattachmentlist = Attachments.GetAttachmentByUid(userid, typeid, pageid, 16);
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount,typeid > 0 ? "myattachment.aspx?typeid=" + typeid : "myattachment.aspx", 10);
        }
    }
}