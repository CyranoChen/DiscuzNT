using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ������Ϣ������
    /// </summary>
    public class PostInfo
    {
        private int m_pid;	//����PID
        private int m_fid;	//�������ID
        private string m_forumname = string.Empty; //�������
        private int m_tid;	//��������ID
        private int m_parentid = 0;	//����ID
        private int m_layer = 0;	//�����������
        private string m_poster = string.Empty;	//��������
        private int m_posterid;	//����UID
        private string m_title = string.Empty;	//����
        private string m_topictitle = string.Empty; //�������
        private string m_postdatetime = string.Empty;	//����ʱ��
        private string m_message = string.Empty;	//����
        private string m_ip = string.Empty;	//IP��ַ
        private string m_lastedit = string.Empty; //���༭
        private int m_invisible;	//�Ƿ�����, ���δͨ�������Ϊ����  -3Ϊ������
        private int m_usesig;	//�Ƿ�����ǩ��
        private int m_htmlon;	//�Ƿ�֧��html
        private int m_bbcodeoff;	//�Ƿ�֧��html
        private int m_smileyoff;	//�Ƿ�ر�smaile����
        private int m_parseurloff;	//�Ƿ�ر�url�Զ�����
        private int m_attachment = 0;	//�Ƿ��и���
        private int m_rate = 0;	//���ַ���
        private int m_ratetimes = 0;	//���ִ���
        private int m_debateopinion; //�������ֹ۵�
        private string m_forumrewritename = string.Empty; //�������ڰ����д��

        ///<summary>
        ///����PID
        ///</summary>
        public int Pid
        {
            get { return m_pid; }
            set { m_pid = value; }
        }
        ///<summary>
        ///�������ID
        ///</summary>
        public int Fid
        {
            get { return m_fid; }
            set { m_fid = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        public string Forumname
        {
            get { return m_forumname; }
            set { m_forumname = value; }
        }

        ///<summary>
        ///��������ID
        ///</summary>
        public int Tid
        {
            get { return m_tid; }
            set { m_tid = value; }
        }
        ///<summary>
        ///����ID
        ///</summary>
        public int Parentid
        {
            get { return m_parentid; }
            set { m_parentid = value; }
        }
        ///<summary>
        ///�����������
        ///</summary>
        public int Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }
        ///<summary>
        ///��������
        ///</summary>
        public string Poster
        {
            get { return m_poster; }
            set { m_poster = value; }
        }
        ///<summary>
        ///����UID
        ///</summary>
        public int Posterid
        {
            get { return m_posterid; }
            set { m_posterid = value; }
        }
        ///<summary>
        ///����
        ///</summary>
        public string Title
        {
            get { return m_title.Trim(); }
            set { m_title = value; }
        }
        ///<summary>
        ///����
        ///</summary>
        public string Topictitle
        {
            get { return m_topictitle.Trim(); }
            set { m_topictitle = value; }
        }
        ///<summary>
        ///����ʱ��
        ///</summary>
        public string Postdatetime
        {
            get { return m_postdatetime; }
            set { m_postdatetime = value; }
        }
        ///<summary>
        ///����
        ///</summary>
        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }
        ///<summary>
        ///IP��ַ
        ///</summary>
        public string Ip
        {
            get { return m_ip.Trim(); }
            set { m_ip = value; }
        }
        ///<summary>
        ///���༭
        ///</summary>
        public string Lastedit
        {
            get { return m_lastedit; }
            set { m_lastedit = value; }
        }
        ///<summary>
        ///�Ƿ�����, ���δͨ�������Ϊ���� 0:��ʾ 1:����ʾ -1:�����  -2:����
        ///</summary>
        public int Invisible
        {
            get { return m_invisible; }
            set { m_invisible = value; }
        }
        ///<summary>
        ///�Ƿ�����ǩ��
        ///</summary>
        public int Usesig
        {
            get { return m_usesig; }
            set { m_usesig = value; }
        }
        ///<summary>
        ///�Ƿ�֧��html
        ///</summary>
        public int Htmlon
        {
            get { return m_htmlon; }
            set { m_htmlon = value; }
        }
        ///<summary>
        ///�Ƿ�ر�smile����
        ///</summary>
        public int Smileyoff
        {
            get { return m_smileyoff; }
            set { m_smileyoff = value; }
        }
        ///<summary>
        ///�Ƿ�ر�Discuz!NT����
        ///</summary>
        public int Bbcodeoff
        {
            get { return m_bbcodeoff; }
            set { m_bbcodeoff = value; }
        }
        ///<summary>
        ///�Ƿ�ر�url�Զ�����
        ///</summary>
        public int Parseurloff
        {
            get { return m_parseurloff; }
            set { m_parseurloff = value; }
        }
        ///<summary>
        ///�Ƿ��и���
        ///</summary>
        public int Attachment
        {
            get { return m_attachment; }
            set { m_attachment = value; }
        }
        ///<summary>
        ///���ַ���
        ///</summary>
        public int Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }
        ///<summary>
        ///���ִ���
        ///</summary>
        public int Ratetimes
        {
            get { return m_ratetimes; }
            set { m_ratetimes = value; }
        }

        /// <summary>
        /// �������ֹ۵�
        /// </summary>
        public int Debateopinion
        {
            get { return m_debateopinion; }
            set { m_debateopinion = value; }
        }

        /// <summary>
        /// ���url��д��
        /// </summary>
        public string ForumRewriteName
        {
            get { return m_forumrewritename; }
            set { m_forumrewritename = value; }
        }
    }
}
