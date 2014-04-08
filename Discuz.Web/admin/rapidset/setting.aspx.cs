using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����������. 
    /// </summary>
    public partial class setting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                forumtitle.Text = configInfo.Forumtitle.ToString();
                forumurl.Text = configInfo.Forumurl.ToString();
                webtitle.Text = configInfo.Webtitle.ToString();
                weburl.Text = configInfo.Weburl.ToString().ToLower();

                SetOption(configInfo);
            }
        }

        public void SetOption(GeneralConfigInfo configInfo)
        {
            if (configInfo.Maxonlines == 500) size.SelectedValue = "1";
            if (configInfo.Maxonlines == 5000) size.SelectedValue = "2";
            if (configInfo.Maxonlines == 50000) size.SelectedValue = "3";

            if (configInfo.Regctrl == 0) safe.SelectedValue = "1";
            if (configInfo.Regctrl == 12) safe.SelectedValue = "2";
            if (configInfo.Regctrl == 48) safe.SelectedValue = "3";

            if (configInfo.Visitedforums == 0) func.SelectedValue = "1";
            if (configInfo.Visitedforums == 10) func.SelectedValue = "2";
            if (configInfo.Visitedforums == 20) func.SelectedValue = "3";
        }

        private void submitsetting_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();

                #region

                switch (size.SelectedValue)
                {
                    case "1":
                        {
                            configInfo.Attachsave = 0;
                            configInfo.Fullmytopics = 0;
                            configInfo.Maxonlines = 500;
                            configInfo.Starthreshold = 2;
                            configInfo.Searchctrl = 10;
                            configInfo.Hottopic = 10;
                            configInfo.Losslessdel = 365;
                            configInfo.Maxmodworksmonths = 5;
                            configInfo.Moddisplay = 0;
                            configInfo.Tpp = 30;
                            configInfo.Ppp = 20;
                            configInfo.Maxpolloptions = 10;
                            configInfo.Maxpostsize = 10000;
                            configInfo.Maxfavorites = 500;
                            configInfo.Nocacheheaders = 1;
                            configInfo.Guestcachepagetimeout = 0;
                            configInfo.Topiccachemark = 0;
                            configInfo.Postinterval = 5;
                            configInfo.Maxspm = 5;
                            configInfo.Fulltextsearch = 0;
                            configInfo.TopicQueueStats = 0;
                            configInfo.TopicQueueStatsCount = 20;
                            break;
                        }
                    case "2":
                        {
                            configInfo.Attachsave = 1;
                            configInfo.Fullmytopics = 1;
                            configInfo.Maxonlines = 5000;
                            configInfo.Starthreshold = 2;
                            configInfo.Searchctrl = 30;
                            configInfo.Hottopic = 20;
                            configInfo.Losslessdel = 200;
                            configInfo.Maxmodworksmonths = 3;
                            configInfo.Moddisplay = 0;
                            configInfo.Tpp = 20;
                            configInfo.Ppp = 15;
                            configInfo.Maxpolloptions = 1000;
                            configInfo.Maxpostsize = 10000;
                            configInfo.Maxfavorites = 200;
                            configInfo.Nocacheheaders = 0;
                            configInfo.Guestcachepagetimeout = 10;
                            configInfo.Topiccachemark = 20;
                            configInfo.Postinterval = 10;
                            configInfo.Maxspm = 4;
                            configInfo.Fulltextsearch = 0;
                            configInfo.TopicQueueStats = 1;
                            configInfo.TopicQueueStatsCount = 30;
                            break;
                        }
                    case "3":
                        {
                            configInfo.Attachsave = 2;
                            configInfo.Fullmytopics = 1;
                            configInfo.Maxonlines = 50000;
                            configInfo.Starthreshold = 2;
                            configInfo.Searchctrl = 60;
                            configInfo.Hottopic = 100;
                            configInfo.Maxmodworksmonths = 1;
                            configInfo.Moddisplay = 1;
                            configInfo.Tpp = 15;
                            configInfo.Ppp = 10;
                            configInfo.Maxpolloptions = 20000;
                            configInfo.Maxfavorites = 100;
                            configInfo.Nocacheheaders = 0;
                            configInfo.Guestcachepagetimeout = 20;
                            configInfo.Topiccachemark = 50;
                            configInfo.Postinterval = 15;
                            configInfo.Maxspm = 3;
                            configInfo.Fulltextsearch = 0;
                            configInfo.TopicQueueStats = 1;
                            configInfo.TopicQueueStatsCount = 100;
                            break;
                        }
                }

                #endregion

                #region

                switch (safe.SelectedValue)
                {
                    case "1":
                        {
                            configInfo.Doublee = 1; //����ͬһ Email ע�᲻ͬ�û�
                            configInfo.Dupkarmarate = 1; //debug ��ȫ	�����ظ����� (����)
                            configInfo.Hideprivate = 0; //debug ��ȫ	������Ȩ���ʵ���̳ (����)
                            configInfo.Memliststatus = 1; //debug ��ȫ	����鿴��Ա�б� (����)
                            configInfo.Seccodestatus = ""; //debug ��ȫ	������֤��
                            configInfo.Rules = 0; //debug ��ȫ	ע�����Э��, ���bbrulestxtʹ�� (����)
                            configInfo.Edittimelimit = 0; //debug ��ȫ	�༭����ʱ������ (����)
                            configInfo.Karmaratelimit = 0; //debug ��ȫ	����ʱ������ (Сʱ)
                            configInfo.Regctrl = 0; //debug ��ȫ	ͬһIP ע��������(Сʱ)
                            configInfo.Regstatus = 1; //debug ��ȫ	�������û�ע�� (����) ?
                            configInfo.Regverify = 0; //debug ����	���û�ע����֤ (0:ֱ��ע��ɹ� 1:Email ��֤ 2:�˹����)
                            configInfo.Secques = 5;
                            configInfo.Defaulteditormode = 0;
                            configInfo.Allowswitcheditor = 0;
                            configInfo.Watermarktype = 0;
                            configInfo.Attachimgquality = 80;
                            break;
                        }
                    case "2":
                        {
                            configInfo.Attachrefcheck = 1;
                            configInfo.Doublee = 0; //����ͬһ Email ע�᲻ͬ�û�
                            configInfo.Dupkarmarate = 0; //debug ��ȫ	�����ظ����� (����)
                            configInfo.Hideprivate = 1; //debug ��ȫ	������Ȩ���ʵ���̳ (����)
                            configInfo.Memliststatus = 1; //debug ��ȫ	����鿴��Ա�б� (����)
                            configInfo.Seccodestatus = "login.aspx"; //debug ��ȫ	������֤��
                            configInfo.Rules = 1; //debug ��ȫ	ע�����Э��, ���bbrulestxtʹ�� (����)
                            configInfo.Edittimelimit = 20; //debug ��ȫ	�༭����ʱ������ (����)
                            configInfo.Karmaratelimit = 1; //debug ��ȫ	����ʱ������ (Сʱ)
                            configInfo.Newbiespan = 1; //debug ��ȫ	���ּ�ϰ���� (Сʱ)
                            configInfo.Regctrl = 12; //debug ��ȫ	ͬһIP ע��������(Сʱ)
                            configInfo.Regstatus = 1; //debug ��ȫ	�������û�ע�� (����) ?
                            configInfo.Regverify = 1; //debug ����	���û�ע����֤ (0:ֱ��ע��ɹ� 1:Email ��֤ 2:�˹����)
                            configInfo.Secques = 10;
                            configInfo.Defaulteditormode = 0;
                            configInfo.Allowswitcheditor = 1;
                            configInfo.Watermarktype = 1;
                            configInfo.Attachimgquality = 85;
                            break;
                        }
                    case "3":
                        {
                            configInfo.Attachrefcheck = 1;
                            configInfo.Doublee = 0; //����ͬһ Email ע�᲻ͬ�û�
                            configInfo.Dupkarmarate = 0; //debug ��ȫ	�����ظ����� (����)
                            configInfo.Hideprivate = 1; //debug ��ȫ	������Ȩ���ʵ���̳ (����)
                            configInfo.Memliststatus = 0; //debug ��ȫ	����鿴��Ա�б� (����)
                            configInfo.Seccodestatus = "login.aspx"; //debug ��ȫ	������֤��
                            configInfo.Rules = 1; //debug ��ȫ	ע�����Э��, ���bbrulestxtʹ�� (����)
                            configInfo.Edittimelimit = 10; //debug ��ȫ	�༭����ʱ������ (����)
                            configInfo.Karmaratelimit = 4; //debug ��ȫ	����ʱ������ (Сʱ)
                            configInfo.Newbiespan = 4; //debug ��ȫ	���ּ�ϰ���� (Сʱ)
                            configInfo.Regctrl = 48; //debug ��ȫ	ͬһIP ע��������(Сʱ)
                            configInfo.Regstatus = 1; //debug ��ȫ	�������û�ע�� (����) ?
                            configInfo.Regverify = 1; //debug ����	���û�ע����֤ (0:ֱ��ע��ɹ� 1:Email ��֤ 2:�˹����)
                            configInfo.Secques = 20;
                            configInfo.Defaulteditormode = 1;
                            configInfo.Allowswitcheditor = 1;
                            configInfo.Watermarktype = 1;
                            configInfo.Attachimgquality = 100;
                            break;
                        }
                }

                #endregion

                #region

                switch (func.SelectedValue)
                {
                    case "1":
                        {
                            configInfo.Archiverstatus = 0; //debug ����	���� Archiver (����)
                            configInfo.Attachimgpost = 0; //debug ����	��������ʾͼƬ���� (����)
                            configInfo.Fastpost = 0; //debug ����	���ٷ��� (����)
                            configInfo.Editedby = 0; //debug ����	��ʾ�༭��Ϣ (����)
                            configInfo.Forumjump = 0; //debug ����	��ʾ��̳��ת�˵� (����)
                            configInfo.Modworkstatus = 0; //debug ����	��̳������ͳ�� (����)
                            configInfo.Rssstatus = 0; //debug ����	���� RSS
                            configInfo.Smileyinsert = 0; //debug ����	��ʾ�ɵ�� Smilies , ��smcols����ʹ�ÿ��Կ����Ƿ���ʾ�༭��
                            configInfo.Stylejump = 0; //debug ����	��ʾ��������˵�
                            configInfo.Subforumsindex = 0; //debug ����	��ҳ��ʾ��̳���¼�����̳
                            configInfo.Visitedforums = 0; //debug ����	��ʾ���������̳����
                            configInfo.Welcomemsg = 0; //debug ����	���ͻ�ӭ����Ϣ
                            configInfo.Watermarkstatus = 0; //debug ����	ͼƬ�������ˮӡ
                            configInfo.Whosonlinestatus = 0; //debug ����	������ʾ״̬
                            configInfo.Debug = 0; //debug ����	debug ��ģʽ
                            configInfo.Regadvance = 0; //debug ����	�Ƿ���ʾ�߼�ע��ѡ��
                            configInfo.Showsignatures = 0; //debug ����	�Ƿ���ʾǩ��, ͷ��
                            break;
                        }

                    case "2":
                        {
                            configInfo.Archiverstatus = 1; //debug ����	���� Archiver (����)
                            configInfo.Attachimgpost = 1; //debug ����	��������ʾͼƬ���� (����)
                            configInfo.Fastpost = 1; //debug ����	���ٷ��� (����)
                            configInfo.Editedby = 1; //debug ����	��ʾ�༭��Ϣ (����)
                            configInfo.Forumjump = 1; //debug ����	��ʾ��̳��ת�˵� (����)
                            configInfo.Modworkstatus = 0; //debug ����	��̳������ͳ�� (����)
                            configInfo.Rssstatus = 1; //debug ����	���� RSS
                            configInfo.Smileyinsert = 1; //debug ����	��ʾ�ɵ�� Smilies , ��smcols����ʹ�ÿ��Կ����Ƿ���ʾ�༭��
                            configInfo.Stylejump = 0; //debug ����	��ʾ��������˵�
                            configInfo.Subforumsindex = 0; //debug ����	��ҳ��ʾ��̳���¼�����̳
                            configInfo.Visitedforums = 10; //debug ����	��ʾ���������̳����
                            configInfo.Welcomemsg = 0; //debug ����	���ͻ�ӭ����Ϣ
                            configInfo.Watermarkstatus = 0; //debug ����	ͼƬ�������ˮӡ
                            configInfo.Whosonlinestatus = 1; //debug ����	������ʾ״̬
                            configInfo.Debug = 1; //debug ����	debug ��ģʽ
                            configInfo.Regadvance = 0; //debug ����	�Ƿ���ʾ�߼�ע��ѡ��
                            configInfo.Showsignatures = 1; //debug ����	�Ƿ���ʾǩ��, ͷ��
                            break;
                        }
                    case "3":
                        {
                            configInfo.Archiverstatus = 1; //debug ����	���� Archiver (����)
                            configInfo.Attachimgpost = 1; //debug ����	��������ʾͼƬ���� (����)
                            configInfo.Fastpost = 1; //debug ����	���ٷ��� (����)
                            configInfo.Editedby = 1; //debug ����	��ʾ�༭��Ϣ (����)
                            configInfo.Forumjump = 1; //debug ����	��ʾ��̳��ת�˵� (����)
                            configInfo.Modworkstatus = 1; //debug ����	��̳������ͳ�� (����)
                            configInfo.Rssstatus = 1; //debug ����	���� RSS
                            configInfo.Smileyinsert = 1; //debug ����	��ʾ�ɵ�� Smilies , ��smcols����ʹ�ÿ��Կ����Ƿ���ʾ�༭��
                            configInfo.Stylejump = 1; //debug ����	��ʾ��������˵�
                            configInfo.Subforumsindex = 1; //debug ����	��ҳ��ʾ��̳���¼�����̳
                            configInfo.Visitedforums = 20; //debug ����	��ʾ���������̳����
                            configInfo.Welcomemsg = 1; //debug ����	���ͻ�ӭ����Ϣ
                            configInfo.Watermarkstatus = 1; //debug ����	ͼƬ�������ˮӡ
                            configInfo.Whosonlinestatus = 1; //debug ����	������ʾ״̬
                            configInfo.Debug = 1; //debug ����	debug ��ģʽ
                            configInfo.Regadvance = 1; //debug ����	�Ƿ���ʾ�߼�ע��ѡ��
                            configInfo.Showsignatures = 1; //debug ����	�Ƿ���ʾǩ��, ͷ��
                            break;
                        }
                }

                #endregion

                configInfo.Forumtitle = forumtitle.Text.Trim();
                configInfo.Forumurl = forumurl.Text.Trim().ToLower();
                configInfo.Webtitle = webtitle.Text.Trim();
                configInfo.Weburl = weburl.Text.Trim().ToLower();

                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��������", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='setting.aspx';");
            }
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.submitsetting.Click += new EventHandler(this.submitsetting_Click);

            forumtitle.IsReplaceInvertedComma = false;
            forumurl.IsReplaceInvertedComma = false;
            webtitle.IsReplaceInvertedComma = false;
            weburl.IsReplaceInvertedComma = false;
        }

        #endregion

    }
}