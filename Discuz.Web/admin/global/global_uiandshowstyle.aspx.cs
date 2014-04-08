using System;
using System.Web.UI;
using System.Collections;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using System.Text;
using System.Web.UI.WebControls;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��������ʾ��ʽ����
    /// </summary>
    public partial class uiandshowstyle : AdminPage
    {
        /// <summary>
        /// ���õ���չ���������б�
        /// </summary>
        public string[] score;
        public string[] postleftarray, userfacearray;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCustomauthorinfo();
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                quickforward.Items[0].Attributes.Add("onclick", "setStatus(true)");
                quickforward.Items[1].Attributes.Add("onclick", "setStatus(false)");

                openshare.Items[0].Attributes.Add("onclick", "openShare(true)");
                openshare.Items[1].Attributes.Add("onclick", "openShare(false)");
            }
        }


        private void LoadCustomauthorinfo()
        {
            score = Scoresets.GetValidScoreName();
            GeneralConfigInfo configinfos = GeneralConfigs.GetConfig();
            string customauthorinfo = configinfos.Customauthorinfo;
            string postleft = Utils.SplitString(customauthorinfo, "|")[0];//�������Ҫ��ʾ���û���Ϣ��Ŀ
            string userface = Utils.SplitString(customauthorinfo, "|")[1];//ͷ���Ϸ�Ҫ��ʾ����Ŀ
            postleftarray = postleft.Split(',');
            userfacearray = userface.Split(',');
        }

        public void LoadConfigInfo()
        {
            #region ����������Ϣ
            templateid.Attributes.Add("onchange", "LoadImage(this.selectedIndex)");
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            stylejump.SelectedValue = configInfo.Stylejump.ToString();
            browsecreatetemplate.SelectedValue = configInfo.BrowseCreateTemplate.ToString();
            templateid.AddTableData(Templates.GetValidTemplateList(), "name", "templateid");
            debug.SelectedValue = configInfo.Debug.ToString();


            templateid.SelectedValue = configInfo.Templateid.ToString();

            templateid.Items.RemoveAt(0);
            string scriptstr = "<script type=\"text/javascript\">\r\n";
            scriptstr += "images = new Array();\r\n";
            for (int i = 0; i < templateid.Items.Count; i++)
            {
                scriptstr += "images[" + i + "]=\"../../templates/" + templateid.Items[i].Text + "/about.png\";\r\n";
            }
            scriptstr += "</script>";
            base.RegisterStartupScript("", scriptstr);
            preview.Src = "../../templates/" + templateid.SelectedItem.Text + "/about.png";
            isframeshow.SelectedValue = configInfo.Isframeshow.ToString();
            whosonlinestatus.SelectedValue = configInfo.Whosonlinestatus.ToString();
            maxonlinelist.Text = configInfo.Maxonlinelist.ToString();
            forumjump.SelectedValue = configInfo.Forumjump.ToString();
            if (configInfo.Onlinetimeout >= 0) showauthorstatusinpost.SelectedValue = "2";
            else showauthorstatusinpost.SelectedValue = "1";
            onlinetimeout.Text = Math.Abs(configInfo.Onlinetimeout).ToString();
            smileyinsert.SelectedValue = configInfo.Smileyinsert.ToString();
            visitedforums.Text = configInfo.Visitedforums.ToString();
            moddisplay.SelectedValue = configInfo.Moddisplay.ToString();
            showsignatures.SelectedValue = configInfo.Showsignatures.ToString();
            showavatars.SelectedValue = configInfo.Showavatars.ToString();
            showimages.SelectedValue = configInfo.Showimages.ToString();
            maxsigrows.Text = configInfo.Maxsigrows.ToString();
            smiliesmax.Text = configInfo.Smiliesmax.ToString();
            viewnewtopicminute.Text = configInfo.Viewnewtopicminute.ToString();
            whosonlinecontact.SelectedValue = configInfo.Whosonlinecontract.ToString();
            postnocustom.Text = configInfo.Postnocustom;
            quickforward.SelectedValue = configInfo.Quickforward.ToString();
            msgforwardlist.Text = configInfo.Msgforwardlist.Replace(",", "\r\n");
            foreach (ListItem item in allowfloatwin.Items)
            {
                item.Selected = !configInfo.Disallowfloatwin.Contains(item.Value);
            }
            openshare.SelectedValue = configInfo.Disableshare.ToString();
            shownewposticon.SelectedValue = configInfo.Shownewposticon.ToString();
            ratelisttype.SelectedValue = configInfo.Ratelisttype.ToString();
            showratecount.Text = configInfo.DisplayRateCount.ToString();
            moderactions.SelectedValue = configInfo.Moderactions.ToString();
            memliststatus.SelectedValue = configInfo.Memliststatus.ToString();
            Indexpage.SelectedIndex = Convert.ToInt32(configInfo.Indexpage.ToString());
            tpp.Text = configInfo.Tpp.ToString();
            ppp.Text = configInfo.Ppp.ToString();
            fastpost.SelectedValue = configInfo.Fastpost.ToString();
            userstatusby.SelectedValue = (configInfo.Userstatusby.ToString() != "0") ? "1" : "0";
            allowchangewidth.SelectedValue = configInfo.Allowchangewidth.ToString();
            showwidthmode.SelectedValue = configInfo.Showwidthmode.ToString();
            datediff.SelectedValue = configInfo.DateDiff.ToString();
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ����������Ϣ

            if (this.CheckCookie())
            {
                SortedList sl = new SortedList();
                sl.Add("�޶�������ʱ��", onlinetimeout.Text);
                sl.Add("���ǩ���߶�", maxsigrows.Text);
                sl.Add("��ʾ���������̳����", visitedforums.Text);
                sl.Add("������ͬһ��������ֵ�������", smiliesmax.Text);
                string[] postleft = Utils.SplitString(DNTRequest.GetFormString("postleft"), ",");
                string[] userface = Utils.SplitString(DNTRequest.GetFormString("userface"), ",");
                string postleftstr = "", userfacestr = "";
                foreach (DictionaryEntry s in sl)
                {
                    if (!Utils.IsInt(s.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('�������:" + s.Key.ToString() + ",ֻ����0����������');window.location.href='global_uiandshowstyle.aspx';</script>");
                        return;
                    }
                }

                if (Convert.ToInt32(onlinetimeout.Text) <= 0)
                {
                    base.RegisterStartupScript("", "<script>alert('�޶�������ʱ��������0');</script>");
                    return;
                }
                if (TypeConverter.StrToInt(maxsigrows.Text) > 9999 || (TypeConverter.StrToInt(maxsigrows.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('���ǩ���߶�ֻ����0-9999֮��');window.location.href='.aspx';</script>");
                    return;
                }


                if (TypeConverter.StrToInt(visitedforums.Text) > 9999 || (TypeConverter.StrToInt(visitedforums.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('��ʾ���������̳����ֻ����0-9999֮��');window.location.href='global_uiandshowstyle.aspx';</script>");
                    return;
                }


                if (TypeConverter.StrToInt(smiliesmax.Text) > 1000 || (TypeConverter.StrToInt(smiliesmax.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('������ͬһ��������ֵ�������ֻ����0-1000֮��');window.location.href='global_uiandshowstyle.aspx';</script>");
                    return;
                }

                if (TypeConverter.StrToInt(viewnewtopicminute.Text) > 14400 || (TypeConverter.StrToInt(viewnewtopicminute.Text) < 5))
                {
                    base.RegisterStartupScript("", "<script>alert('�鿴���������ñ�����5-14400֮��');window.location.href='global_uiandshowstyle.aspx';</script>");
                    return;
                }

                if (TypeConverter.StrToInt(showratecount.Text) > 100 || TypeConverter.StrToInt(showratecount.Text) < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('��ʾ�������ֵ�����������0-100֮��');window.location.href='global_uiandshowstyle.aspx';</script>");
                    return;
                }

                if (TypeConverter.StrToInt(tpp.Text) > 100 || TypeConverter.StrToInt(tpp.Text) <= 0)
                {
                    base.RegisterStartupScript("", "<script>alert('ÿҳ������ֻ����1-100֮��');window.location.href='forum_option.aspx';</script>");
                    return;
                }

                if (TypeConverter.StrToInt(ppp.Text) > 100 || TypeConverter.StrToInt(ppp.Text) <= 0)
                {
                    base.RegisterStartupScript("", "<script>alert('ÿҳ������ֻ����1-100֮��');window.location.href='forum_option.aspx';</script>");
                    return;
                }

                foreach (string left in postleft)
                {
                    postleftstr += left;
                    postleftstr += ",";
                }

                foreach (string face in userface)
                {
                    userfacestr += face;
                    userfacestr += ",";
                }


                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();

                configInfo.Customauthorinfo = postleftstr.TrimEnd(',') + "|" + userfacestr.TrimEnd(',');
                configInfo.Templateid = TypeConverter.StrToInt(templateid.SelectedValue);
                configInfo.Subforumsindex = 1;
                configInfo.Stylejump = TypeConverter.StrToInt(stylejump.SelectedValue);
                configInfo.BrowseCreateTemplate = Convert.ToInt32(browsecreatetemplate.SelectedValue);
                configInfo.Isframeshow = TypeConverter.StrToInt(isframeshow.SelectedValue);
                configInfo.Whosonlinestatus = TypeConverter.StrToInt(whosonlinestatus.SelectedValue);

                if (showauthorstatusinpost.SelectedValue == "1") configInfo.Onlinetimeout = 0 - Convert.ToInt32(onlinetimeout.Text);
                else configInfo.Onlinetimeout = TypeConverter.StrToInt(onlinetimeout.Text);

                configInfo.Maxonlinelist = TypeConverter.StrToInt(maxonlinelist.Text);
                configInfo.Forumjump = TypeConverter.StrToInt(forumjump.SelectedValue);
                configInfo.Smileyinsert = TypeConverter.StrToInt(smileyinsert.SelectedValue);
                configInfo.Visitedforums = TypeConverter.StrToInt(visitedforums.Text);
                configInfo.Moddisplay = TypeConverter.StrToInt(moddisplay.SelectedValue);
                configInfo.Showsignatures = TypeConverter.StrToInt(showsignatures.SelectedValue);
                configInfo.Showavatars = TypeConverter.StrToInt(showavatars.SelectedValue);
                configInfo.Showimages = TypeConverter.StrToInt(showimages.SelectedValue);
                configInfo.Smiliesmax = TypeConverter.StrToInt(smiliesmax.Text);
                configInfo.Maxsigrows = TypeConverter.StrToInt(maxsigrows.Text);
                configInfo.Viewnewtopicminute = TypeConverter.StrToInt(viewnewtopicminute.Text);
                configInfo.Whosonlinecontract = TypeConverter.StrToInt(whosonlinecontact.SelectedValue);
                configInfo.Postnocustom = postnocustom.Text;
                configInfo.Quickforward = TypeConverter.StrToInt(quickforward.SelectedValue);
                configInfo.Msgforwardlist = msgforwardlist.Text.Replace("\r\n", ",");
                string selectValue = "";
                foreach (ListItem item in allowfloatwin.Items)
                {
                    selectValue += !item.Selected ? item.Value + "|" : "";
                }
                configInfo.Disallowfloatwin = selectValue.TrimEnd('|');
                configInfo.Disableshare = TypeConverter.StrToInt(openshare.SelectedValue);
                configInfo.Shownewposticon = TypeConverter.StrToInt(shownewposticon.SelectedValue);
                configInfo.Ratelisttype = TypeConverter.StrToInt(ratelisttype.SelectedValue);
                configInfo.DisplayRateCount = TypeConverter.StrToInt(showratecount.Text);
                configInfo.Moderactions = TypeConverter.StrToInt(moderactions.SelectedValue);
                configInfo.Memliststatus = Convert.ToInt32(memliststatus.SelectedValue);
                configInfo.Indexpage = Convert.ToInt32(Indexpage.SelectedValue);
                configInfo.Tpp = TypeConverter.StrToInt(tpp.Text);
                configInfo.Ppp = TypeConverter.StrToInt(ppp.Text);
                configInfo.Fastpost = TypeConverter.StrToInt(fastpost.SelectedValue);
                configInfo.Userstatusby = TypeConverter.StrToInt(userstatusby.SelectedValue);
                configInfo.Allowchangewidth = TypeConverter.StrToInt(allowchangewidth.SelectedValue);
                configInfo.Showwidthmode = TypeConverter.StrToInt(showwidthmode.SelectedValue);
                configInfo.Debug = Convert.ToInt16(debug.SelectedValue);
                configInfo.DateDiff = TypeConverter.StrToInt(datediff.SelectedValue);

                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��������ʾ��ʽ����", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_uiandshowstyle.aspx';");
            }

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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }

        #endregion

    }
}