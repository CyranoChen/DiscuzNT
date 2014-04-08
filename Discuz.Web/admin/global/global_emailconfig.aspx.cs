using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Plugin.Mail;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑邮件配置
    /// </summary>
    public partial class emailconfig : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                EmailConfigInfo emailInfo = EmailConfigs.GetConfig();
                smtp.Text = emailInfo.Smtp;
                port.Text = emailInfo.Port.ToString();
                sysemail.Text = emailInfo.Sysemail;
                userName.Text = emailInfo.Username;
                password.Text = emailInfo.Password;

                try
                {
                    GetSmtpEmailPlugIn(HttpRuntime.BinDirectory);
                }
                catch 
                {
                    smtpemail.Items.Clear();
                    try
                    {
                        GetSmtpEmailPlugIn(Utils.GetMapPath("/bin/"));
                    }
                    catch 
                    {
                        smtpemail.Items.Add(new ListItem(".net邮件发送程序", "Discuz.Common.SysMailMessage,Discuz.Common"));
                        smtpemail.Items.Add(new ListItem("Discuz!NT邮件发送程序", "Discuz.Common.SmtpMail,Discuz.Common"));
                    }
                }

                try
                {
                    smtpemail.SelectedValue = emailInfo.PluginNameSpace + "," + emailInfo.DllFileName;
                }
                catch
                {
                    ;// smtpemail.SelectedIndex = 1;
                }
            }
        }


        public void GetSmtpEmailPlugIn(string filepath)
        {
            #region 获取邮件发送插件

            DirectoryInfo dirinfo = new DirectoryInfo(filepath);
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    if (file.Extension.ToLower().Equals(".dll"))
                    {
                        try
                        {
                            Assembly a = Assembly.LoadFrom(HttpRuntime.BinDirectory + file);

                            foreach (Module m in a.GetModules())
                            {
                                foreach (Type t in m.FindTypes(Module.FilterTypeName, "*")) //采用过滤器进行类名过滤
                                {
                                    foreach (object arr in t.GetCustomAttributes(typeof(SmtpEmailAttribute), true))
                                    {
                                        SmtpEmailAttribute sea = (SmtpEmailAttribute)arr;

                                        smtpemail.Items.Add(new ListItem(sea.PlugInName, t.FullName + "," + (sea.DllFileName != "" ? sea.DllFileName : file.ToString().Replace(".dll", ""))));
                                    }
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }

            #endregion
        }

        private void SaveEmailInfo_Click(object sender, EventArgs e)
        {
            #region 保存邮箱设置

            if (this.CheckCookie())
            {
                EmailConfigInfo __emailinfo = EmailConfigs.GetConfig();
                __emailinfo.Smtp = smtp.Text;
                __emailinfo.Port = Convert.ToInt32(port.Text);
                __emailinfo.Sysemail = sysemail.Text;
                __emailinfo.Username = userName.Text;
                __emailinfo.Password = password.Text;

                try
                {
                    __emailinfo.PluginNameSpace = smtpemail.SelectedValue.Split(',')[0];
                    __emailinfo.DllFileName = smtpemail.SelectedValue.Split(',')[1];
                }
                catch
                {
                    ;
                }

                EmailConfigs.SaveConfig(__emailinfo);

                Emails.ReSetISmtpMail();

                base.RegisterStartupScript( "PAGE",  "window.location.href='global_emailconfig.aspx';");
            }

            #endregion
        }

        private void sendTestEmail_Click(object sender, EventArgs e)
        {
            #region 发送测试邮件

            if (this.CheckCookie())
            {
                if (testEmail.Text != "")
                {
                    Emails.DiscuzSmtpMailToUser(testEmail.Text, "测试邮件", "这是一封Discuz!NT邮箱设置页面发送的测试邮件!");

                    base.RegisterStartupScript("PAGE",  "window.location.href='global_emailconfig.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('请输入测试发送EMAIL地址!');</script>");
                }
            }

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
            this.SaveEmailInfo.Click += new EventHandler(this.SaveEmailInfo_Click);
            this.sendTestEmail.Click += new EventHandler(this.sendTestEmail_Click);
        }

        #endregion

    }
}