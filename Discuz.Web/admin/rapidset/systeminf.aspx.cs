using System;
using System.Web;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ϵͳ��Ϣ
    /// </summary>
    public partial class systeminf : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadSystemInf();
            }
        }

        protected void LoadSystemInf()
        {
            #region ���ϵͳ��Ϣ

            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            //ȡ��ҳ��ִ�п�ʼʱ��
            DateTime stime = DateTime.Now;

            //ȡ�÷����������Ϣ
            servername.Text = Server.MachineName;
            serverip.Text = Request.ServerVariables["LOCAL_ADDR"];
            server_name.Text = Request.ServerVariables["SERVER_NAME"];

            int build, major, minor, revision;
            build = Environment.Version.Build;
            major = Environment.Version.Major;
            minor = Environment.Version.Minor;
            revision = Environment.Version.Revision;
            servernet.Text = ".NET CLR  " + major + "." + minor + "." + build + "." + revision;
            serverms.Text = Environment.OSVersion.ToString();

            serversoft.Text = Request.ServerVariables["SERVER_SOFTWARE"];
            serverport.Text = Request.ServerVariables["SERVER_PORT"];
            serverout.Text = Server.ScriptTimeout.ToString();
            //����Ӧ�����������Ϣ, 1.0 final �޸�
            cl.Text = Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            servertime.Text = DateTime.Now.ToString();
            //serverppath.Text = Request.ServerVariables["APPL_PHYSICAL_PATH"];
            servernpath.Text = Request.ServerVariables["PATH_TRANSLATED"];
            serverhttps.Text = Request.ServerVariables["HTTPS"];

            //ȡ���û��������Ϣ
            HttpBrowserCapabilities bc = Request.Browser;
            ie.Text = bc.Browser.ToString();
            cookies.Text = bc.Cookies.ToString();
            frames.Text = bc.Frames.ToString();
            javaa.Text = bc.JavaApplets.ToString();
            javas.Text = bc.EcmaScriptVersion.ToString();
            ms.Text = bc.Platform.ToString();
            vbs.Text = bc.VBScript.ToString();
            vi.Text = bc.Version.ToString();

            //ȡ�������ip��ַ,1.0 final ����
            cip.Text = DNTRequest.GetIP(); // Request.ServerVariables["REMOTE_ADDR"];

            //ȡ��ҳ��ִ�н���ʱ��
            DateTime etime = DateTime.Now;

            //����ҳ��ִ��ʱ��
            runtime.Text = ((etime - stime).TotalMilliseconds).ToString();

            #endregion
        }

        public bool chkobj(string obj)
        {
            #region ���֧����֤����

            try
            {
                object meobj = Server.CreateObject(obj);
                return (true);
            }
            catch (Exception objex)
            {
                string logstr = objex.ToString();
                return (false);
            }

            #endregion
        }

        private void for5000_Click(object sender, EventArgs e)
        {
            #region 5000��μӷ�ѭ������

            DateTime ontime = DateTime.Now;
            int sum = 0;
            for (int i = 1; i <= 50000000; i++)
            {
                sum = sum + i;
            }
            DateTime endtime = DateTime.Now;
            l5000.Text = ((endtime - ontime).TotalMilliseconds).ToString() + "����";

            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.for5000.Click += new EventHandler(this.for5000_Click);
        }

        #endregion

    }
}