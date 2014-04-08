using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 安全控制
    /// </summary>
    public partial class safecontrol : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                seccodestatus.Attributes.Add("style", "line-height:16px");
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息
            //加载验证码的显示
            string[] dllFiles = System.IO.Directory.GetFiles(HttpRuntime.BinDirectory, "Discuz.Plugin.VerifyImage.*.dll");
            foreach (string dllFile in dllFiles)
            {
                string filename = dllFile.ToString().Substring(dllFile.ToString().IndexOf("Discuz.Plugin.VerifyImage")).Replace("Discuz.Plugin.VerifyImage.", "").Replace(".dll", "");
                VerifyImage.Items.Add(filename);
            }
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            postinterval.Text = configInfo.Postinterval.ToString();
            maxspm.Text = configInfo.Maxspm.ToString();
            seccodestatus.AddAttributes("readonly", "");
            seccodestatus.Attributes.Add("onfocus", "this.className='';");
            seccodestatus.Attributes.Add("onblur", "this.className='';");
            admintools.SelectedValue = configInfo.Admintools.ToString();
            VerifyImage.Items.Add(new ListItem("系统默认验证码", ""));            
            seccodestatus.Text = configInfo.Seccodestatus.Replace(",", "\r\n");
            ViewState["Seccodestatus"] = configInfo.Seccodestatus.ToString();
            VerifyImage.SelectedValue = configInfo.VerifyImageAssemly;
            antispamusername.Text = configInfo.Antispamregisterusername;
            antispamemail.Text = configInfo.Antispamregisteremail;
            antispamtitle.Text = configInfo.Antispamposttitle;
            antispammessage.Text = configInfo.Antispampostmessage;
            disablepostad.SelectedValue = configInfo.Disablepostad.ToString();
            disablepostad.Items[0].Attributes.Add("onclick", "$('" + postadstatus.ClientID + "').style.display='';");
            disablepostad.Items[1].Attributes.Add("onclick", "$('" + postadstatus.ClientID + "').style.display='none';");
            disablepostadregminute.Text = configInfo.Disablepostadregminute.ToString();
            disablepostadpostcount.Text = configInfo.Disablepostadpostcount.ToString();
            disablepostadregular.Text = configInfo.Disablepostadregular.ToString();
            try
            {
                secques.SelectedValue = configInfo.Secques.ToString();
            }
            catch
            {
                secques.SelectedValue = "1";
            }
            if (configInfo.Disablepostad == 0)
            {
                postadstatus.Attributes.Add("style", "display:none");
            }
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                Hashtable HT = new Hashtable();
                HT.Add("发帖灌水预防", postinterval.Text);
                HT.Add("60 秒最大搜索次数", maxspm.Text);
                foreach (DictionaryEntry de in HT)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + de.Key.ToString().Trim() + ",只能是0或者正整数');window.location.href='global_safecontrol.aspx';</script>");
                        return;
                    }
                }
                if (disablepostad.SelectedValue == "1" && disablepostadregular.Text == "")
                {
                    base.RegisterStartupScript("", "<script>alert('新用户广告强力屏蔽正则表达式为空');window.location.href='global_safecontrol.aspx';</script>");
                    return;
                }

                //循环比对四个控件的值是否互不相同
                string antiSpamNameList = string.Concat(antispamusername.Text, antispamemail.Text, antispamtitle.Text, antispammessage.Text);
                string[] nameList = { antispamusername.Text, antispamemail.Text, antispamtitle.Text, antispammessage.Text };
                foreach (string str in nameList)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        base.RegisterStartupScript("", "<script>alert('防注册机设置不可为空 , 请返回重新填写!');window.location.href='global_safecontrol.aspx';</script>");
                        return;
                    }
                    if (antiSpamNameList.IndexOf(str) != antiSpamNameList.LastIndexOf(str))
                    {
                        base.RegisterStartupScript("", "<script>alert('防注册机设置不可重复 , 请返回重新填写!');window.location.href='global_safecontrol.aspx';</script>");
                        return;
                    }
                }

                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.VerifyImageAssemly = VerifyImage.SelectedValue;
                configInfo.Postinterval = Convert.ToInt32(postinterval.Text);
                configInfo.Seccodestatus = seccodestatus.Text.Trim().Replace("\r\n", ",");
                configInfo.Maxspm = Convert.ToInt32(maxspm.Text);
                configInfo.Secques = Convert.ToInt32(secques.SelectedValue);
                configInfo.Admintools = Convert.ToInt16(admintools.SelectedValue);
                configInfo.Antispamregisterusername = antispamusername.Text.Trim();
                configInfo.Antispamregisteremail = antispamemail.Text.Trim();
                configInfo.Antispamposttitle = antispamtitle.Text.Trim();
                configInfo.Antispampostmessage = antispammessage.Text.Trim();
                configInfo.Disablepostad = Convert.ToInt16(disablepostad.SelectedValue);
                configInfo.Disablepostadregminute = Convert.ToInt16(disablepostadregminute.Text);
                configInfo.Disablepostadpostcount = Convert.ToInt16(disablepostadpostcount.Text);
                configInfo.Disablepostadregular = disablepostadregular.Text;
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "安全与防灌水", "");
                base.RegisterStartupScript( "PAGE","window.location.href='global_safecontrol.aspx';");
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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }

        #endregion

    }
}