using System;

namespace Discuz.Entity
{
  

    /// <summary>
    /// ShowtopicPagePostInfo ��ժҪ˵����
    /// </summary>
    [Serializable]
    public class ShowtopicPagePostInfo : ICloneable
    {
        private string m_ubbmessage;
        private int m_id; //���(¥��)
        private string m_postnocustom = "";
        private int m_pid; //����PID
        private int m_fid; //�������ID
        private string m_title; //����
        private int m_layer; //�����������
        private string m_message; //����
        private string m_ip; //IP��ַ
        private string m_lastedit; //���༭
        private string m_postdatetime; //����ʱ��
        private int m_attachment; //�Ƿ��и���
        private string m_poster; //��������
        private int m_posterid; //����UID
        private int m_invisible; //�Ƿ�����, ���δͨ�������Ϊ����
        private int m_usesig; //�Ƿ�����ǩ��
        private int m_htmlon; //�Ƿ�֧��html
        private int m_smileyoff; //�Ƿ�ر�smaile����
        private int m_parseurloff; //�Ƿ�ر�url�Զ�����
        private int m_bbcodeoff; //�Ƿ�֧��html
        private int m_rate; //���ַ���
        private int m_ratetimes; //���ִ���
        private string m_nickname; //�ǳ�
        private string m_username; //�û���
        private int m_groupid; //�û���ID
        private string m_email; //�ʼ���ַ
        private int m_showemail; //�Ƿ���ʾ����
        private int m_digestposts; //��������
        private int m_credits; //������
        private float m_extcredits1; //��չ����1
        private float m_extcredits2; //��չ����2
        private float m_extcredits3; //��չ����3
        private float m_extcredits4; //��չ����4
        private float m_extcredits5; //��չ����5
        private float m_extcredits6; //��չ����6
        private float m_extcredits7; //��չ����7
        private float m_extcredits8; //��չ����8
        private int m_posts; //������
        private string m_joindate; //ע��ʱ��
        private int m_onlinestate; //����״̬, 1Ϊ����, 0Ϊ������
        private string m_lastactivity; //���ʱ��
        private int m_userinvisible; //�Ƿ�����
        private string m_avatar; //ͷ��
        private int m_avatarwidth; //ͷ����
        private int m_avatarheight; //ͷ��߶�
        private string m_medals; //ѫ���б�
        private string m_signature; //ǩ��Html
        private string m_location; //����
        private string m_customstatus; //�Զ���ͷ��
        private string m_website; //��վ
        private string m_icq; //ICQ�ʺ�
        private string m_qq; //QQ�ʺ�
        private string m_msn; //MSN messenger�ʺ�
        private string m_yahoo; //Yahoo messenger�ʺ�
        private string m_skype; //skype�ʺ�
        private string m_oltime;
        private string m_lastvisit;
        //��չ����
        private string m_status; //ͷ��
        private int m_stars; //����
        private int m_adindex; //������
        private int m_spaceid;//�ռ�Id
        private int m_gender;//�Ա�
        private string m_bday;//����
        private int m_debateopinion;//���۹۵�,1,�����۵㣬2�����۵�
        private int m_diggs;//��Ϊ���۹۵��֧����
        private bool m_digged = true;


        /// <summary>
        /// ����¼ʱ��
        /// </summary>
        public string Lastvisit
        {
            get { return m_lastvisit; }
            set { m_lastvisit = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public string Oltime
        {
            get { return m_oltime; }
            set { m_oltime = value; }
        }
        /// <summary>
        /// UBB����
        /// </summary>
        public string Ubbmessage
        {
            get { return m_ubbmessage; }
            set { m_ubbmessage = value; }
        }

        /// <summary>
        /// �Ƿ񱻶�
        /// </summary>
        public bool Digged
        {
            get { return m_digged; }
            set { m_digged = value; }
        }

        /// <summary>
        /// ���(¥��)
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// �Զ���¥������
        /// </summary>
        public string Postnocustom
        {
            get { return m_postnocustom; }
            set { m_postnocustom = value; }
        }
        /// <summary>
        /// ����PID
        /// </summary>
        public int Pid
        {
            get { return m_pid; }
            set { m_pid = value; }
        }

        /// <summary>
        /// �������ID
        /// </summary>
        public int Fid
        {
            get { return m_fid; }
            set { m_fid = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        /// <summary>
        /// �����������
        /// </summary>
        public int Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        /// <summary>
        /// IP��ַ
        /// </summary>
        public string Ip
        {
            get { return m_ip; }
            set { m_ip = value; }
        }

        /// <summary>
        /// ���༭
        /// </summary>
        public string Lastedit
        {
            get { return m_lastedit; }
            set { m_lastedit = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public string Postdatetime
        {
            get { return m_postdatetime; }
            set { m_postdatetime = value; }
        }

        /// <summary>
        /// �Ƿ��и���
        /// </summary>
        public int Attachment
        {
            get { return m_attachment; }
            set { m_attachment = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string Poster
        {
            get { return m_poster; }
            set { m_poster = value; }
        }

        /// <summary>
        /// ����UID
        /// </summary>
        public int Posterid
        {
            get { return m_posterid; }
            set { m_posterid = value; }
        }

        /// <summary>
        /// �Ƿ�����, ���δͨ�������Ϊ���� 1:����ʾ   0����ʾ  -1�������  -2������
        /// </summary>
        public int Invisible
        {
            get { return m_invisible; }
            set { m_invisible = value; }
        }

        /// <summary>
        /// �Ƿ�����ǩ��
        /// </summary>
        public int Usesig
        {
            get { return m_usesig; }
            set { m_usesig = value; }
        }

        /// <summary>
        /// �Ƿ�֧��html
        /// </summary>
        public int Htmlon
        {
            get { return m_htmlon; }
            set { m_htmlon = value; }
        }

        /// <summary>
        /// �Ƿ�ر�smaile����
        /// </summary>
        public int Smileyoff
        {
            get { return m_smileyoff; }
            set { m_smileyoff = value; }
        }

        /// <summary>
        /// �Ƿ�ر�url�Զ�����
        /// </summary>
        public int Parseurloff
        {
            get { return m_parseurloff; }
            set { m_parseurloff = value; }
        }

        /// <summary>
        /// �Ƿ�֧��html
        /// </summary>
        public int Bbcodeoff
        {
            get { return m_bbcodeoff; }
            set { m_bbcodeoff = value; }
        }

        /// <summary>
        /// ���ַ���
        /// </summary>
        public int Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }

        /// <summary>
        /// ���ִ���
        /// </summary>
        public int Ratetimes
        {
            get { return m_ratetimes; }
            set { m_ratetimes = value; }
        }

        /// <summary>
        /// �ǳ�
        /// </summary>
        public string Nickname
        {
            get { return m_nickname; }
            set { m_nickname = value; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        /// <summary>
        /// �û���ID
        /// </summary>
        public int Groupid
        {
            get { return m_groupid; }
            set { m_groupid = value; }
        }

        /// <summary>
        /// �ʼ���ַ
        /// </summary>
        public string Email
        {
            get { return m_email; }
            set { m_email = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ����
        /// </summary>
        public int Showemail
        {
            get { return m_showemail; }
            set { m_showemail = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public int Digestposts
        {
            get { return m_digestposts; }
            set { m_digestposts = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public int Credits
        {
            get { return m_credits; }
            set { m_credits = value; }
        }

        /// <summary>
        /// ��չ����1
        /// </summary>
        public float Extcredits1
        {
            get { return m_extcredits1; }
            set { m_extcredits1 = value; }
        }

        /// <summary>
        /// ��չ����2
        /// </summary>
        public float Extcredits2
        {
            get { return m_extcredits2; }
            set { m_extcredits2 = value; }
        }

        /// <summary>
        /// ��չ����3
        /// </summary>
        public float Extcredits3
        {
            get { return m_extcredits3; }
            set { m_extcredits3 = value; }
        }

        /// <summary>
        /// ��չ����4
        /// </summary>
        public float Extcredits4
        {
            get { return m_extcredits4; }
            set { m_extcredits4 = value; }
        }

        /// <summary>
        /// ��չ����5
        /// </summary>
        public float Extcredits5
        {
            get { return m_extcredits5; }
            set { m_extcredits5 = value; }
        }

        /// <summary>
        /// ��չ����6
        /// </summary>
        public float Extcredits6
        {
            get { return m_extcredits6; }
            set { m_extcredits6 = value; }
        }

        /// <summary>
        /// ��չ����7
        /// </summary>
        public float Extcredits7
        {
            get { return m_extcredits7; }
            set { m_extcredits7 = value; }
        }

        /// <summary>
        /// ��չ����8
        /// </summary>
        public float Extcredits8
        {
            get { return m_extcredits8; }
            set { m_extcredits8 = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public int Posts
        {
            get { return m_posts; }
            set { m_posts = value; }
        }

        /// <summary>
        /// ע��ʱ��
        /// </summary>
        public string Joindate
        {
            get { return m_joindate; }
            set { m_joindate = value; }
        }

        /// <summary>
        /// ����״̬, 1Ϊ����, 0Ϊ������
        /// </summary>
        public int Onlinestate
        {
            get { return m_onlinestate; }
            set { m_onlinestate = value; }
        }

        /// <summary>
        /// ���ʱ��
        /// </summary>
        public string Lastactivity
        {
            get { return m_lastactivity; }
            set { m_lastactivity = value; }
        }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public int Userinvisible
        {
            get { return m_userinvisible; }
            set { m_userinvisible = value; }
        }

        /// <summary>
        /// ͷ��
        /// </summary>
        public string Avatar
        {
            get { return m_avatar; }
            set { m_avatar = value; }
        }

        /// <summary>
        /// ͷ����
        /// </summary>
        public int Avatarwidth
        {
            get { return m_avatarwidth; }
            set { m_avatarwidth = value; }
        }

        /// <summary>
        /// ͷ��߶�
        /// </summary>
        public int Avatarheight
        {
            get { return m_avatarheight; }
            set { m_avatarheight = value; }
        }

        /// <summary>
        /// ѫ���б�
        /// </summary>
        public string Medals
        {
            get { return m_medals; }
            set { m_medals = value; }
        }

        /// <summary>
        /// ǩ��Html
        /// </summary>
        public string Signature
        {
            get { return m_signature; }
            set { m_signature = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        /// <summary>
        /// �Զ���ͷ��
        /// </summary>
        public string Customstatus
        {
            get { return m_customstatus; }
            set { m_customstatus = value; }
        }

        /// <summary>
        /// ��վ
        /// </summary>
        public string Website
        {
            get { return m_website; }
            set { m_website = value; }
        }

        /// <summary>
        /// ICQ�ʺ�
        /// </summary>
        public string Icq
        {
            get { return m_icq; }
            set { m_icq = value; }
        }

        /// <summary>
        /// QQ�ʺ�
        /// </summary>
        public string Qq
        {
            get { return m_qq; }
            set { m_qq = value; }
        }

        /// <summary>
        /// MSN messenger�ʺ�
        /// </summary>
        public string Msn
        {
            get { return m_msn; }
            set { m_msn = value; }
        }

        /// <summary>
        /// Yahoo messenger�ʺ�
        /// </summary>
        public string Yahoo
        {
            get { return m_yahoo; }
            set { m_yahoo = value; }
        }

        /// <summary>
        /// skype�ʺ�
        /// </summary>
        public string Skype
        {
            get { return m_skype; }
            set { m_skype = value; }
        }

        /// <summary>
        /// ͷ��
        /// </summary>
        public string Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Stars
        {
            get { return m_stars; }
            set { m_stars = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public int Adindex
        {
            get { return m_adindex; }
            set { m_adindex = value; }
        }

        /// <summary>
        /// �ռ�Id
        /// </summary>
        public int Spaceid
        {
            get { return m_spaceid; }
            set { m_spaceid = value; }
        }

        /// <summary>
        /// �Ա�
        /// </summary>
        public int Gender
        {
            get { return m_gender; }
            set { m_gender = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Bday
        {
            get { return m_bday; }
            set { m_bday = value; }
        }

        /// <summary>
        /// ���۹۵�,1,�����۵㣬2�����۵�
        /// </summary>
        public int Debateopinion
        {
            get { return m_debateopinion; }
            set { m_debateopinion = value; }
        }

        /// <summary>
        /// ��Ϊ���۹۵��֧����
        /// </summary>
        public int Diggs
        {
            get { return m_diggs; }
            set { m_diggs = value; }
        }

        public object Clone()
        {
           return this.MemberwiseClone(); //ǳ����      
        }
    }
}
