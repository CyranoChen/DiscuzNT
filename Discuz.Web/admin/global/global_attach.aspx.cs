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
    /// 附件设置. 
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
            #region 加载配置信息

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
            #region 加载系统字体
            watermarkfontname.Items.Clear();
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
                watermarkfontname.Items.Add(new ListItem(family.Name, family.Name));
            #endregion
        }

        public void LoadPosition(int selectid)
        {
            #region 加载水印设置界面

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
            position.Text += ">不启用水印功能";

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (CheckCookie())
            {
                Hashtable ht = new Hashtable();
                ht.Add("图片附件文字水印大小", watermarkfontsize.Text);
                ht.Add("JPG图片质量", attachimgquality.Text);
                ht.Add("图片最大高度", attachimgmaxheight.Text);
                ht.Add("图片最大宽度", attachimgmaxwidth.Text);
               
                foreach (DictionaryEntry de in ht)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", GetMessageScript("输入错误," + de.Key.ToString() + "只能是0或者正整数"));
                        return;
                    }
                }

                if (Convert.ToInt16(attachimgquality.Text) > 100 || (Convert.ToInt16(attachimgquality.Text) < 0))
                {
                    base.RegisterStartupScript( "", GetMessageScript("JPG图片质量只能在0-100之间"));
                    return;
                }

                if (Convert.ToInt16(watermarktransparency.Text) > 10 || Convert.ToInt16(watermarktransparency.Text) < 1)
                {
                    base.RegisterStartupScript( "", GetMessageScript("图片水印透明度取值范围1-10"));
                    return;
                }

                if (Convert.ToInt16(watermarkfontsize.Text) <= 0)
                {
                    base.RegisterStartupScript( "", GetMessageScript("图片附件添加文字水印的大小必须大于0"));
                    return;
                }


                if (Convert.ToInt16(attachimgmaxheight.Text) < 0)
                {
                    base.RegisterStartupScript( "", GetMessageScript("图片最大高度必须大于或等于0"));
                    return;
                }

                if (Convert.ToInt16(attachimgmaxwidth.Text) < 0)
                {
                    base.RegisterStartupScript( "", GetMessageScript("图片最大宽度必须大于或等于0"));
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

            AdminVistLogs.InsertLog(userid, username, usergroupid, grouptitle, ip, "附件设置", "");
        }

        private string GetMessageScript(string message)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_attach.aspx';</script>",message);
        }

        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web 窗体设计器生成的代码

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