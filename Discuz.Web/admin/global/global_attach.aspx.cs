using System;
using System.Drawing;
using System.Drawing.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��������. 
    /// </summary> 
    public partial class attach : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                LoadConfigInfo();
        }

        public void LoadConfigInfo()
        {
            #region ����������Ϣ

            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            attachrefcheck.SelectedValue = configInfo.Attachrefcheck.ToString();
            attachsave.SelectedValue = configInfo.Attachsave.ToString();
            //attachimgpost.SelectedValue = configInfo.Attachimgpost.ToString();
            watermarktype.SelectedValue = configInfo.Watermarktype.ToString();
            showattachmentpath.SelectedValue = configInfo.Showattachmentpath.ToString();
            attachimgmaxheight.Text = configInfo.Attachimgmaxheight.ToString();
            attachimgmaxwidth.Text = configInfo.Attachimgmaxwidth.ToString();
            attachimgquality.Text = configInfo.Attachimgquality.ToString();
            watermarkfontsize.Text = configInfo.Watermarkfontsize.ToString();
            watermarktext.Text = configInfo.Watermarktext.ToString();
            watermarkpic.Text = configInfo.Watermarkpic.ToString();
            watermarktransparency.Text = configInfo.Watermarktransparency.ToString();
            LoadPosition(configInfo.Watermarkstatus);
            LoadSystemFont();

            try
            {
                watermarkfontname.SelectedValue = configInfo.Watermarkfontname.ToString();
            }
            catch
            {
                watermarkfontname.SelectedIndex = 0;
            }

            #endregion
        }

        private void LoadSystemFont()
        {
            #region ����ϵͳ����
            watermarkfontname.Items.Clear();
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
                watermarkfontname.Items.Add(new ListItem(family.Name, family.Name));
            #endregion
        }

        public void LoadPosition(int selectid)
        {
            #region ����ˮӡ���ý���

            position.Text = "<table width=\"256\" height=\"207\" border=\"0\" background=\"../images/flower.jpg\">";
            for (int i = 1; i < 10; i++)
            {
                if (i % 3 == 1) 
                    position.Text += "<tr>";
                position.Text += (selectid == i ?
                    "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" checked><b>#" + i + "</b></td>" :
                    "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" ><b>#" + i + "</b></td>");
                if (i % 3 == 0) 
                    position.Text += "</tr>";
            }

            position.Text += "</table><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"0\" ";
            if (selectid == 0)
                position.Text += " checked";
            position.Text += ">������ˮӡ����";

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ����������Ϣ

            if (CheckCookie())
            {
                Hashtable ht = new Hashtable();
                ht.Add("ͼƬ��������ˮӡ��С", watermarkfontsize.Text);
                ht.Add("JPGͼƬ����", attachimgquality.Text);
                ht.Add("ͼƬ���߶�", attachimgmaxheight.Text);
                ht.Add("ͼƬ�����", attachimgmaxwidth.Text);
               
                foreach (DictionaryEntry de in ht)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", GetMessageScript("�������," + de.Key.ToString() + "ֻ����0����������"));
                        return;
                    }
                }

                if (Convert.ToInt16(attachimgquality.Text) > 100 || (Convert.ToInt16(attachimgquality.Text) < 0))
                {
                    base.RegisterStartupScript( "", GetMessageScript("JPGͼƬ����ֻ����0-100֮��"));
                    return;
                }

                if (Convert.ToInt16(watermarktransparency.Text) > 10 || Convert.ToInt16(watermarktransparency.Text) < 1)
                {
                    base.RegisterStartupScript( "", GetMessageScript("ͼƬˮӡ͸����ȡֵ��Χ1-10"));
                    return;
                }

                if (Convert.ToInt16(watermarkfontsize.Text) <= 0)
                {
                    base.RegisterStartupScript( "", GetMessageScript("ͼƬ�����������ˮӡ�Ĵ�С�������0"));
                    return;
                }


                if (Convert.ToInt16(attachimgmaxheight.Text) < 0)
                {
                    base.RegisterStartupScript( "", GetMessageScript("ͼƬ���߶ȱ�����ڻ����0"));
                    return;
                }

                if (Convert.ToInt16(attachimgmaxwidth.Text) < 0)
                {
                    base.RegisterStartupScript( "", GetMessageScript("ͼƬ����ȱ�����ڻ����0"));
                    return;
                }


                SaveGeneralConfigInfo();
                base.RegisterStartupScript( "PAGE", "window.location.href='global_attach.aspx';");
            }

            #endregion
        }

        private void SaveGeneralConfigInfo()
        {
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            configInfo.Attachrefcheck = Convert.ToInt32(attachrefcheck.SelectedValue);
            configInfo.Attachsave = Convert.ToInt32(attachsave.SelectedValue);
            configInfo.Watermarkstatus = DNTRequest.GetInt("watermarkstatus", 0);
            //configInfo.Attachimgpost = Convert.ToInt32(attachimgpost.SelectedValue);
            configInfo.Watermarktype = Convert.ToInt16(watermarktype.SelectedValue);
            configInfo.Showattachmentpath = Convert.ToInt32(showattachmentpath.SelectedValue);
            configInfo.Attachimgmaxheight = Convert.ToInt32(attachimgmaxheight.Text);
            configInfo.Attachimgmaxwidth = Convert.ToInt32(attachimgmaxwidth.Text);
            configInfo.Attachimgquality = Convert.ToInt32(attachimgquality.Text);
            configInfo.Watermarktext = watermarktext.Text;
            configInfo.Watermarkpic = watermarkpic.Text;
            configInfo.Watermarkfontname = watermarkfontname.SelectedValue;
            configInfo.Watermarkfontsize = Convert.ToInt32(watermarkfontsize.Text);
            configInfo.Watermarktransparency = Convert.ToInt16(watermarktransparency.Text);

            GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));

            AdminVistLogs.InsertLog(userid, username, usergroupid, grouptitle, ip, "��������", "");
        }

        private string GetMessageScript(string message)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_attach.aspx';</script>",message);
        }

        #region ��VIEWSTATEд������

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            SaveInfo.Click += new EventHandler(SaveInfo_Click);
        }

        #endregion
    }
}