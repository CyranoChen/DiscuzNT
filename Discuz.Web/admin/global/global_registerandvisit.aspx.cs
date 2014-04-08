using System.Collections;
using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ע������ʿ���
    /// </summary>
    public partial class registerandvisit : AdminPage
    {
        public string[] extCreditsName = new string[8];
        public string[] extCreditsUnits = new string[9];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                regstatus.Items[0].Attributes.Add("onclick", "setStatus()");
                regstatus.Items[1].Attributes.Add("onclick", "setStatus()");
                regstatus.Items[2].Attributes.Add("onclick", "setStatus()");
                regstatus.Items[3].Attributes.Add("onclick", "setStatus()");
                
            }
        }

        public void LoadConfigInfo()
        {
            #region ����������Ϣ

            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            InvitationConfigInfo invitationConfigInfo = InvitationConfigs.GetConfig();

            regstatus.SelectedValue = configInfo.Regstatus.ToString();

            censoruser.Text = configInfo.Censoruser;
            doublee.SelectedValue = configInfo.Doublee.ToString();
            emaillogin.SelectedValue = configInfo.Emaillogin.ToString();
            regverify.SelectedValue = configInfo.Regverify.ToString();
            accessemail.Text = configInfo.Accessemail;
            censoremail.Text = configInfo.Censoremail;
            hideprivate.SelectedValue = configInfo.Hideprivate.ToString();
            ipdenyaccess.Text = configInfo.Ipdenyaccess;
            ipaccess.Text = configInfo.Ipaccess;
            regctrl.Text = configInfo.Regctrl.ToString();
            ipregctrl.Text = configInfo.Ipregctrl;
            adminipaccess.Text = configInfo.Adminipaccess;
            welcomemsg.SelectedValue = configInfo.Welcomemsg.ToString();
            welcomemsgtxt.Text = configInfo.Welcomemsgtxt;
            rules.SelectedValue = configInfo.Rules.ToString();
            rulestxt.Text = configInfo.Rulestxt;
            newbiespan.Text = configInfo.Newbiespan.ToString();
            realnamesystem.SelectedValue = configInfo.Realnamesystem.ToString();
            invitecodeexpiretime.Text = invitationConfigInfo.InviteCodeExpireTime.ToString();
            invitecodemaxcount.Text = invitationConfigInfo.InviteCodeMaxCount.ToString();
            addextcreditsline.Text = invitationConfigInfo.InviteCodePayCount.ToString();
            invitationuserdescription.Text = invitationConfigInfo.InvitationLoginUserDescription;//�����ֵ��textarea��innnerhtml�ܱ�֤��ʾ�����������������õ�Ч��
            invitationvisitordescription.Text = invitationConfigInfo.InvitationVisitorDescription;
            invitationemailmodel.Text = invitationConfigInfo.InvitationEmailTemplate;
            invitecodeusermaxbuy.Text = invitationConfigInfo.InviteCodeMaxCountToBuy.ToString();
            invitecodeusercreateperday.Text = invitationConfigInfo.InviteCodeUserCreatePerDay.ToString();
            passwordmode.SelectedValue = configInfo.Passwordmode.ToString();
            CookieDomain.Text = configInfo.CookieDomain.ToString();
            string[] extCredits = Utils.SplitString(invitationConfigInfo.InviteCodePrice, ",");
            extCreditsUnits = Scoresets.GetValidScoreUnit();
            DataTable extCreditsTable = Scoresets.GetScorePaySet(0);

            //��ʼ��������۸���ʾ���棬ȫ��Ϊ����
            for (int count = 0; count < 8; count++)
            {
                extCreditsName[count] = "";
                TextBox textbox = this.FindControl("invitecodeprice" + count.ToString()) as TextBox;
                textbox.Text = extCredits[count];
                textbox.Visible = false;
            }

            //���������������Ϣ������Ч����Ϣ��ʾ�ڽ�����
            foreach (DataRow dr in extCreditsTable.Rows)
            {
                extCreditsName[Utils.StrToInt(dr["id"].ToString(), 0) - 1] = dr["name"].ToString() + ":";
                TextBox textbox = this.FindControl("invitecodeprice" + (Utils.StrToInt(dr[0].ToString(), 0) - 1).ToString()) as TextBox;
                textbox.Visible = true;
            }

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ����������Ϣ

            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                InvitationConfigInfo invitationConfigInfo = InvitationConfigs.GetConfig();

                configInfo.Regstatus = Convert.ToInt16(regstatus.SelectedValue);
                configInfo.Censoruser = DelNullRowOrSpace(censoruser.Text);
                configInfo.Doublee = Convert.ToInt16(doublee.SelectedValue);
                configInfo.Emaillogin = Convert.ToInt16(emaillogin.SelectedValue);
                configInfo.Regverify = Convert.ToInt16(regverify.SelectedValue);
                configInfo.Accessemail = accessemail.Text;
                configInfo.Censoremail = censoremail.Text;
                configInfo.Hideprivate = Convert.ToInt16(hideprivate.SelectedValue);
                configInfo.Ipdenyaccess = ipdenyaccess.Text;
                configInfo.Ipaccess = ipaccess.Text;
                configInfo.Regctrl = Convert.ToInt16(regctrl.Text);
                configInfo.Ipregctrl = ipregctrl.Text;
                configInfo.Adminipaccess = adminipaccess.Text;
                configInfo.Welcomemsg = Convert.ToInt16(welcomemsg.SelectedValue);
                configInfo.Welcomemsgtxt = welcomemsgtxt.Text;
                configInfo.Rules = Convert.ToInt16(rules.SelectedValue);
                configInfo.Rulestxt = rulestxt.Text;
                configInfo.Newbiespan = Convert.ToInt16(newbiespan.Text);
                configInfo.Realnamesystem = Convert.ToInt16(realnamesystem.SelectedValue);
                configInfo.Passwordmode = Convert.ToInt16(passwordmode.SelectedValue);
                configInfo.CookieDomain = CookieDomain.Text;
                invitationConfigInfo.InviteCodePayCount = Utils.StrToInt(addextcreditsline.Text, 0);
                invitationConfigInfo.InviteCodeExpireTime = Utils.StrToInt(invitecodeexpiretime.Text, 0);
                invitationConfigInfo.InviteCodeMaxCount = Utils.StrToInt(invitecodemaxcount.Text, 0);
                invitationConfigInfo.InviteCodePrice = CreateInviteCodePriceString();
                invitationConfigInfo.InvitationLoginUserDescription = DNTRequest.GetString("invitationuserdescriptionmessage_hidden") ;//�����ʱ��ȡtextarea��valueֵ���Ա�֤ǰ̨��ʾ����
                invitationConfigInfo.InvitationVisitorDescription = DNTRequest.GetString("invitationvisitordescriptionmessage_hidden");
                invitationConfigInfo.InvitationEmailTemplate = RepairEmailTemplateCodeParameter(DNTRequest.GetString("invitationemailmodelmessage_hidden"));
                invitationConfigInfo.InviteCodeMaxCountToBuy = Convert.ToInt16(invitecodeusermaxbuy.Text);
                invitationConfigInfo.InviteCodeUserCreatePerDay = Convert.ToInt16(invitecodeusercreateperday.Text);
                
                Hashtable IPHash = new Hashtable();
                IPHash.Add("���� IP ע������", ipregctrl.Text);
                IPHash.Add("IP ��ֹ�����б�", ipdenyaccess.Text);
                IPHash.Add("IP �����б�", ipaccess.Text);
                IPHash.Add("����Ա��̨IP�����б�", adminipaccess.Text);

                string ipkey = "";
                if (Utils.IsRuleTip(IPHash, "ip", out ipkey) == false)
                {
                    base.RegisterStartupScript("erro", "<script>alert('" + ipkey.ToString() + ",IP��ʽ����');</script>");
                    return;
                }

                Hashtable Emailhash = new Hashtable();
                Emailhash.Add("Email �����ַ", accessemail.Text);
                Emailhash.Add("Email ��ֹ��ַ", censoremail.Text);

                string key = "";
                if (Utils.IsRuleTip(Emailhash, "email", out key) == false)
                {
                    base.RegisterStartupScript("erro", "<script>alert('" + key.ToString() + ",Email��ʽ����');</script>");
                    return;
                }

                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                InvitationConfigs.Serialiaze(invitationConfigInfo, Server.MapPath("../../config/invitation.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ע������ʿ�������", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_registerandvisit.aspx';");
            }

            #endregion
        }

        public string CreateInviteCodePriceString()
        {
            string str = "";
            for (int i = 0; i < 8; i++)
            {
                TextBox t = this.FindControl("invitecodeprice" + i.ToString()) as TextBox;
                float f = Utils.StrToFloat(t.Text, 0);
                str += f.ToString() + ",";
            }
            return str.Trim(',');
        }

        /// <summary>
        /// ���ʼ�ģ���б�ת��ΪHTMLת���ַ���������֤��������
        /// </summary>
        /// <param name="tmpStr"></param>
        /// <returns></returns>
        public string RepairEmailTemplateCodeParameter(string tmpStr)
        {
            return tmpStr.Replace("%7B", "{").Replace("%7D", "}");
        }

        public string DelNullRowOrSpace(string desStr)
        {
            #region ɾ������
            string result = "";
            foreach (string str in Utils.SplitString(desStr.Replace(" ", ""), "\r\n"))
            {
                if (str.Trim() != "")
                {
                    if (result == "")
                    {
                        result = str;
                    }
                    else
                    {
                        result = result + "\r\n" + str;
                    }
                }
            }
            return result;
            #endregion
        }

        #region Web ������������ɵĴ���

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.ValidateForm = false;
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }

        #endregion

    }
}