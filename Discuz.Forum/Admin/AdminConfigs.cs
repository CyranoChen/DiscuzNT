using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminConfigFactory ��ժҪ˵����
	/// </summary>
	public class AdminConfigs : GeneralConfigs
	{
        /// <summary>
        /// ��ȡԭʼ��ȱʡ��̳����
        /// </summary>
        /// <returns></returns>
        public static GeneralConfigInfo GetDefaultConifg()
        {
            GeneralConfigInfo configInfo = new GeneralConfigInfo();

            configInfo.Forumtitle = "��̳����"; //��̳����
            configInfo.Forumurl = "/"; //��̳url��ַ
            configInfo.Webtitle = "��վ����"; //��վ����
            configInfo.Weburl = "/"; //��̳��վurl��ַ
            configInfo.Licensed = 1; //�Ƿ���ʾ��ҵ��Ȩ����
            configInfo.Icp = ""; //��վ������Ϣ
            configInfo.Closed = 0; //��̳�ر�
            configInfo.Closedreason = "��Ǹ!��̳��ʱ�ر�,�Ժ���ܷ���."; //��̳�ر���ʾ��Ϣ
            configInfo.Passwordkey = ForumUtils.CreateAuthStr(16); //�û�����Key
            configInfo.Regstatus = 1; //�Ƿ��������û�ע��
            configInfo.Regadvance = 1; //ע��ʱ���Ƿ���ʾ�߼�ѡ��
            configInfo.Censoruser = "Administrator\r\nAdmin\r\n����Ա\r\n����"; //�û���Ϣ�����ؼ���
            configInfo.Doublee = 0; //����ͬһ Email ע�᲻ͬ�û�
            configInfo.Regverify = 0; //���û�ע����֤ 0=����֤ 1=email��֤ 2=�˹���֤
            configInfo.Accessemail = ""; //Email�����ַ
            configInfo.Censoremail = ""; //Email��ֹ��ַ
            configInfo.Hideprivate = 1; //������Ȩ���ʵ���̳
            configInfo.Regctrl = 0; //IP ע��������(Сʱ)
            configInfo.Ipregctrl = ""; //���� IP ע������
            configInfo.Ipaccess = ""; //IP�����б�
            configInfo.Adminipaccess = ""; //����Ա��̨IP�����б�
            configInfo.Newbiespan = 0; //���ּ�ϰ����(��λ:Сʱ)
            configInfo.Welcomemsg = 1; //���ͻ�ӭ����Ϣ
            configInfo.Welcomemsgtxt = "��ӭ��ע����뱾��̳!"; //��ӭ����Ϣ����
            configInfo.Rules = 1; //�Ƿ���ʾע�����Э��
            configInfo.Rulestxt = ""; //���Э������

            configInfo.Templateid = 1; //Ĭ����̳���
            configInfo.Hottopic = 15; //���Ż����������
            configInfo.Starthreshold = 5; //����������ֵ
            configInfo.Visitedforums = 10; //��ʾ���������̳����
            configInfo.Maxsigrows = 20; //���ǩ���߶�(��)
            configInfo.Moddisplay = 0; //������ʾ��ʽ 0=ƽ����ʾ 1=�����˵�
            configInfo.Subforumsindex = 0; //��ҳ�Ƿ���ʾ��̳���¼�����̳
            configInfo.Stylejump = 0; //��ʾ��������˵�
            configInfo.Fastpost = 1; //���ٷ���
            configInfo.Showsignatures = 1; //�Ƿ���ʾǩ��
            configInfo.Showavatars = 1; //�Ƿ���ʾͷ��
            configInfo.Showimages = 1; //�Ƿ�����������ʾͼƬ

            configInfo.Archiverstatus = 1; //���� Archiver
            configInfo.Seotitle = ""; //���⸽����
            configInfo.Seokeywords = ""; //Meta Keywords
            configInfo.Seodescription = ""; //Meta Description
            configInfo.Seohead = ""; //����ͷ����Ϣ

            configInfo.Rssstatus = 1; //rssstatus
            configInfo.Rssttl = 60; //RSS TTL(����)
            configInfo.Nocacheheaders = 0; //��ֹ���������
            configInfo.Fullmytopics = 0; //�ҵĻ���ȫ������ 0=ֻ�����û������ⷢ���ߵ����� 1=�����û������ⷢ���߻�ظ��ߵ�����
            configInfo.Debug = 1; //��ʾ����������Ϣ
            configInfo.Rewriteurl = ""; //α��̬url���滻����

            configInfo.Whosonlinestatus = 3; //��ʾ�����û� 0=����ʾ 1=������ҳ��ʾ 2=���ڷ���̳��ʾ 3=����ҳ�ͷ���̳��ʾ
            configInfo.Maxonlinelist = 300; //�����ʾ��������
            configInfo.Userstatusby = 2; //��������ʾ�û�ͷ��
            configInfo.Forumjump = 1; //��ʾ��̳��ת�˵�
            configInfo.Modworkstatus = 1; //��̳������ͳ��
            configInfo.Maxmodworksmonths = 3; //�����¼����ʱ��(��)

            configInfo.Seccodestatus = "register.aspx"; //ʹ����֤���ҳ���б�,��","�ָ� ����:register.aspx,login.aspx
            configInfo.Maxonlines = 9000; //�����������
            configInfo.Postinterval = 20; //������ˮԤ��(��)
            configInfo.Searchctrl = 0; //����ʱ������(��)
            configInfo.Maxspm = 0; //60 �������������

            configInfo.Visitbanperiods = ""; //��ֹ����ʱ���
            configInfo.Postbanperiods = ""; //��ֹ����ʱ���
            configInfo.Postmodperiods = ""; //�������ʱ���
            configInfo.Attachbanperiods = ""; //��ֹ���ظ���ʱ���
            configInfo.Searchbanperiods = ""; //��ֹȫ������ʱ���

            configInfo.Memliststatus = 1; //����鿴��Ա�б�
            configInfo.Dupkarmarate = 0; //�����ظ�����
            configInfo.Minpostsize = 10; //������С����(��)
            configInfo.Maxpostsize = 500000; //�����������(��)
            configInfo.Tpp = 25; //ÿҳ������
            configInfo.Ppp = 20; //ÿҳ������
            configInfo.Maxfavorites = 100; //�ղؼ�����
            //configInfo.Maxavatarsize = 20480; //ͷ�����ߴ�(�ֽ�)
            //configInfo.Maxavatarwidth = 120; //ͷ�������(����)
            //configInfo.Maxavatarheight = 120; //ͷ�����߶�(����);
            configInfo.Maxpolloptions = 10; //ͶƱ���ѡ����
            configInfo.Maxattachments = 10; //���������ϴ�������

            configInfo.Attachimgpost = 1; //��������ʾͼƬ����
            configInfo.Attachrefcheck = 0; //���ظ�����·���
            configInfo.Attachsave = 3; //�������淽ʽ 0=ȫ������ͬһĿ¼ 1=����̳���벻ͬĿ¼ 2=���ļ����ʹ��벻ͬĿ¼ 3=�������մ��벻ͬĿ¼
            configInfo.Watermarkstatus = 0; //ͼƬ�������ˮӡ 0=��ʹ�� 1=���� 2=���� 3=���� 4=���� ... 9=����

            configInfo.Karmaratelimit = 10; //����ʱ������(Сʱ)
            configInfo.Losslessdel = 5; //ɾ����������ʱ������(��)
            configInfo.Edittimelimit = 0; //�༭����ʱ������(����)
            configInfo.Editedby = 1; //�༭���Ӹ��ӱ༭��¼
            configInfo.Defaulteditormode = 1; //Ĭ�ϵı༭��ģʽ 0=ubb����༭�� 1=���ӻ��༭��
            configInfo.Allowswitcheditor = 1; //�Ƿ������л��༭��ģʽ
            configInfo.Smileyinsert = 1; //��ʾ�ɵ������

            return configInfo;
        }
	}
}