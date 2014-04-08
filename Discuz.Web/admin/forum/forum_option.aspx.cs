using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class option : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                quickforward.Items[0].Attributes.Add("onclick", "setStatus(true)");
                quickforward.Items[1].Attributes.Add("onclick", "setStatus(false)");
            }
        }

        public void LoadConfigInfo()
        {
            #region ����������Ϣ

            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            modworkstatus.SelectedValue = configInfo.Modworkstatus.ToString();
            userstatusby.SelectedValue = (configInfo.Userstatusby.ToString() != "0") ? "1" : "0";
            rssttl.Text = configInfo.Rssttl.ToString();
            losslessdel.Text = configInfo.Losslessdel.ToString();
            editedby.SelectedValue = configInfo.Editedby.ToString();
            allowswitcheditor.SelectedValue = configInfo.Allowswitcheditor.ToString();
            reasonpm.SelectedValue = configInfo.Reasonpm.ToString();
            hottopic.Text = configInfo.Hottopic.ToString();
            starthreshold.Text = configInfo.Starthreshold.ToString();
            fastpost.SelectedValue = configInfo.Fastpost.ToString();
            tpp.Text = configInfo.Tpp.ToString();
            ppp.Text = configInfo.Ppp.ToString();
            enabletag.SelectedValue = configInfo.Enabletag.ToString();
            string[] ratevalveset = configInfo.Ratevalveset.Split(',');
            ratevalveset1.Text = ratevalveset[0];
            ratevalveset2.Text = ratevalveset[1];
            ratevalveset3.Text = ratevalveset[2];
            ratevalveset4.Text = ratevalveset[3];
            ratevalveset5.Text = ratevalveset[4];
            statstatus.SelectedValue = configInfo.Statstatus.ToString();
            hottagcount.Text = configInfo.Hottagcount.ToString();
            maxmodworksmonths.Text = configInfo.Maxmodworksmonths.ToString();
            replynotificationstatus.SelectedValue = configInfo.Replynotificationstatus.ToString();
            replyemailstatus.SelectedValue = configInfo.Replyemailstatus.ToString();
            //allowforumindexposts.SelectedValue = configInfo.Allwoforumindexpost.ToString();��ҳ���ٷ�����Ĺ���
            quickforward.SelectedValue = configInfo.Quickforward.ToString();
            viewnewtopicminute.Text = configInfo.Viewnewtopicminute.ToString();
            rssstatus.SelectedValue = configInfo.Rssstatus.ToString();
            msgforwardlist.Text = configInfo.Msgforwardlist.Replace(",", "\r\n");
            cachelog.SelectedValue = configInfo.Cachelog.ToString();
            silverlight.SelectedValue = config.Silverlight.ToString();
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ����������Ϣ
            string[][] inputrule = new string[2][];
            inputrule[0] = new string[] { losslessdel.Text, tpp.Text, ppp.Text, starthreshold.Text, hottopic.Text };
            inputrule[1] = new string[] { "ɾ����������ʱ��", "ÿҳ������", "ÿҳ������", "����������ֵ", "���Ż����������" };
            for (int j = 0; j < inputrule[0].Length; j++)
            {
                if (!Utils.IsInt(inputrule[0][j].ToString()))
                {
                    base.RegisterStartupScript("", "<script>alert('�������:" + inputrule[1][j].ToString() + ",ֻ����0����������');window.location.href='forum_option.aspx';</script>");
                    return;
                }
            }
            if (Convert.ToInt32(losslessdel.Text) > 9999 || Convert.ToInt32(losslessdel.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('ɾ����������ʱ������ֻ����0-9999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(tpp.Text) > 100 || Convert.ToInt16(tpp.Text) <= 0)
            {
                base.RegisterStartupScript("", "<script>alert('ÿҳ������ֻ����1-100֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(ppp.Text) > 100 || Convert.ToInt16(ppp.Text) <= 0)
            {
                base.RegisterStartupScript("", "<script>alert('ÿҳ������ֻ����1-100֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (Convert.ToInt16(starthreshold.Text) > 9999 || Convert.ToInt16(starthreshold.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('����������ֵֻ����0-9999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(hottopic.Text) > 9999 || Convert.ToInt16(hottopic.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('���Ż����������ֻ����0-9999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(hottagcount.Text) > 60 || Convert.ToInt16(hottagcount.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('��ҳ���ű�ǩ(Tag)����ֻ����0-60֮��');window.location.href='forum_option.aspx';</script>");
            }

            if (TypeConverter.StrToInt(viewnewtopicminute.Text) > 14400 || (TypeConverter.StrToInt(viewnewtopicminute.Text) < 5))
            {
                base.RegisterStartupScript("", "<script>alert('�鿴���������ñ�����5-14400֮��');window.location.href='global_uiandshowstyle.aspx';</script>");
                return;
            }
            if (!ValidateRatevalveset(ratevalveset1.Text)) return;
            if (!ValidateRatevalveset(ratevalveset2.Text)) return;
            if (!ValidateRatevalveset(ratevalveset3.Text)) return;
            if (!ValidateRatevalveset(ratevalveset4.Text)) return;
            if (!ValidateRatevalveset(ratevalveset5.Text)) return;
            if (!(Convert.ToInt16(ratevalveset1.Text) < Convert.ToInt16(ratevalveset2.Text) &&
                  Convert.ToInt16(ratevalveset2.Text) < Convert.ToInt16(ratevalveset3.Text) &&
                  Convert.ToInt16(ratevalveset3.Text) < Convert.ToInt16(ratevalveset4.Text) &&
                  Convert.ToInt16(ratevalveset4.Text) < Convert.ToInt16(ratevalveset5.Text)))
            {
                base.RegisterStartupScript("", "<script>alert('���ַ�ֵ���ǵ���ȡֵ');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.Modworkstatus = Convert.ToInt16(modworkstatus.SelectedValue);
                configInfo.Userstatusby = Convert.ToInt16(userstatusby.SelectedValue);
                configInfo.Rssttl = Convert.ToInt32(rssttl.Text);
                configInfo.Losslessdel = Convert.ToInt16(losslessdel.Text);
                configInfo.Editedby = Convert.ToInt16(editedby.SelectedValue);
                configInfo.Allowswitcheditor = Convert.ToInt16(allowswitcheditor.SelectedValue);
                configInfo.Reasonpm = Convert.ToInt16(reasonpm.SelectedValue);
                configInfo.Hottopic = Convert.ToInt16(hottopic.Text);
                configInfo.Starthreshold = Convert.ToInt16(starthreshold.Text);
                configInfo.Fastpost = Convert.ToInt16(fastpost.SelectedValue);
                configInfo.Tpp = Convert.ToInt16(tpp.Text);
                configInfo.Ppp = Convert.ToInt16(ppp.Text);
                configInfo.Enabletag = Convert.ToInt32(enabletag.SelectedValue);
                configInfo.Ratevalveset = ratevalveset1.Text + "," + ratevalveset2.Text + "," + ratevalveset3.Text + "," + ratevalveset4.Text + "," + ratevalveset5.Text;
                configInfo.Statstatus = Convert.ToInt16(statstatus.SelectedValue);
                configInfo.Hottagcount = Convert.ToInt16(hottagcount.Text);
                configInfo.Maxmodworksmonths = Convert.ToInt16(maxmodworksmonths.Text);
                configInfo.Replynotificationstatus = Convert.ToInt16(replynotificationstatus.SelectedValue);
                configInfo.Replyemailstatus = Convert.ToInt16(replyemailstatus.SelectedValue);
                //configInfo.Allwoforumindexpost = Convert.ToInt16(allowforumindexposts.SelectedValue);��ҳ���ٷ�����Ĺ���
                configInfo.Viewnewtopicminute = TypeConverter.StrToInt(viewnewtopicminute.Text);
                configInfo.Quickforward = TypeConverter.StrToInt(quickforward.SelectedValue);
                configInfo.Msgforwardlist = msgforwardlist.Text.Replace("\r\n", ",");
                configInfo.Rssstatus = Convert.ToInt16(rssstatus.SelectedValue);
                configInfo.Cachelog = Convert.ToInt16(cachelog.SelectedValue);
                configInfo.Silverlight = Convert.ToInt16(silverlight.SelectedValue);
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                Discuz.Forum.TopicStats.SetQueueCount();
                Caches.ReSetConfig();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "վ�㹦��", "");
                base.RegisterStartupScript("PAGE", "window.location.href='forum_option.aspx';");
            }
            #endregion
        }

        private bool ValidateRatevalveset(string val)
        {
            #region ��ֵ֤
            if (!Utils.IsNumeric(val))
            {
                base.RegisterStartupScript("", "<script>alert('���ָ��ֵֻ��������');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            if (Convert.ToInt16(val) > 999 || Convert.ToInt16(val) < 1)
            {
                base.RegisterStartupScript("", "<script>alert('���ָ��ֵֻ����1-999֮��');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            else
                return true;
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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }

        #endregion
    }
}
