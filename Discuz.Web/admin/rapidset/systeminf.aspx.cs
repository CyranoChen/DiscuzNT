using System;
using System.Web;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 系统信息
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
            #region 检测系统信息

            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            //取得页面执行开始时间
            DateTime stime = DateTime.Now;

            //取得服务器相关信息
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
            //语言应该是浏览者信息, 1.0 final 修改
            cl.Text = Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            servertime.Text = DateTime.Now.ToString();
            //serverppath.Text = Request.ServerVariables["APPL_PHYSICAL_PATH"];
            servernpath.Text = Request.ServerVariables["PATH_TRANSLATED"];
            serverhttps.Text = Request.ServerVariables["HTTPS"];

            //取得用户浏览器信息
            HttpBrowserCapabilities bc = Request.Browser;
            ie.Text = bc.Browser.ToString();
            cookies.Text = bc.Cookies.ToString();
            frames.Text = bc.Frames.ToString();
            javaa.Text = bc.JavaApplets.ToString();
            javas.Text = bc.EcmaScriptVersion.ToString();
            ms.Text = bc.Platform.ToString();
            vbs.Text = bc.VBScript.ToString();
            vi.Text = bc.Version.ToString();

            //取得浏览者ip地址,1.0 final 加入
            cip.Text = DNTRequest.GetIP(); // Request.ServerVariables["REMOTE_ADDR"];

            //取得页面执行结束时间
            DateTime etime = DateTime.Now;

            //计算页面执行时间
            runtime.Text = ((etime - stime).TotalMilliseconds).ToString();

            #endregion
        }

        public bool chkobj(string obj)
        {
            #region 组件支持验证代码

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
            #region 5000万次加法循环测试

            DateTime ontime = DateTime.Now;
            int sum = 0;
            for (int i = 1; i <= 50000000; i++)
            {
                sum = sum + i;
            }
            DateTime endtime = DateTime.Now;
            l5000.Text = ((endtime - ontime).TotalMilliseconds).ToString() + "毫秒";

            #endregion
        }

        #region Web 窗体设计器生成的代码

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