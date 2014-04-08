using System;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DropDownList = Discuz.Control.DropDownList;
using RadioButtonList = Discuz.Control.RadioButtonList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Entity;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// ������ҳ
    /// </summary>

    public partial class addadvs : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                for (int i = 1; i <= configInfo.Ppp; i++)
                {
                    inpostfloor.Items.Add(new ListItem(" >#" + i,i.ToString()));
                }
            }
        }

        private void AddAdInfo_Click(object sender, EventArgs e)
        {
            #region ��ӹ��
            if (this.CheckCookie())
            {
                string targetlist = DNTRequest.GetString("TargetFID");

                if ((targetlist == "" || targetlist == ",") && type.SelectedIndex < 10)//�Ǿۺ�ҳ����
                {
                    base.RegisterStartupScript( "", "<script>alert('������ѡȡ��ص�Ͷ�ŷ�Χ,�ٵ���ύ��ť');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                //��ȡ��Ч���������
                string starttimestr = starttime.SelectedDate.ToString();
                string endtimestr = endtime.SelectedDate.ToString();

                //�з���ʱ�����ƵĹ�棬���鷢�����ڷ�Χ�Ƿ�Ϸ�
                if (starttimestr.IndexOf("1900") < 0 && endtimestr.IndexOf("1900") < 0)
                {
                    if (Convert.ToDateTime(starttime.SelectedDate.ToString()) >= Convert.ToDateTime(endtime.SelectedDate.ToString()))
                    {
                        base.RegisterStartupScript( "", "<script>alert('��Чʱ��Ӧ�����ڽ���ʱ��');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                        return;
                    }
                }
                if (endtime.SelectedDate < DateTime.Now)
                {
                    base.RegisterStartupScript("", "<script>alert('��ѡ��Ľ��������ѹ���,������ѡ��һ�����ڽ��������');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }

                Advertisements.CreateAd(Utils.StrToInt(available.SelectedValue, 0), type.SelectedValue, Utils.StrToInt(displayorder.Text, 0),
                                        title.Text, targetlist, GetParameters(), GetCode(),starttimestr,endtimestr);

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");

                base.RegisterStartupScript( "PAGE", "window.location.href='global_advsgrid.aspx';");
            }
            #endregion
        }

        /// <summary>
        /// ����ѡ��ͬ��չ�ַ�ʽ�����ز�ͬ�Ĵ���, 
        /// ��ʽΪ showtype | src | width | height | link | title | font |
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            #region ����ѡȡ�����͵õ�����ֵ

            string result = "";

            switch (parameters.SelectedValue)
            {
                case "htmlcode":
                    {
                        result = code.Text.Trim();
                        break;
                    }
                case "word":
                    {
                        result = string.Format("<a href=\"{0}\" target=\"_blank\" style=\"font-size:{1}\">{2}</a>", wordlink.Text.Trim(), wordfont.Text, wordcontent.Text.Trim());
                        break;
                    }
                case "image":
                    {
                        result = string.Format("<a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"{2}{3} alt=\"{4}\" border=\"0\" /></a>", 
                            imglink.Text.Trim(), 
                            imgsrc.Text.Trim(), 
                            (imgwidth.Text.Trim() == "" ? "" : " width=\"" + imgwidth.Text.Trim() + "\""), 
                            (imgheight.Text.Trim() == "" ? "" : " height=\"" + imgheight.Text.Trim() + "\""), 
                            imgtitle.Text.Trim());
                        break;
                    }
                case "flash":
                    {
                        result = string.Format("<embed wmode=\"opaque\"{0}{1} src=\"{2}\" type=\"application/x-shockwave-flash\"></embed>", 
                            (flashwidth.Text.Trim() == "" ? "" : " width=\"" + flashwidth.Text.Trim() + "\""),
                            (flashheight.Text.Trim() == "" ? "" : " height=\"" + flashheight.Text.Trim() + "\""), 
                            flashsrc.Text.Trim());
                        break;
                    }
            }
            if (type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
            {
                result = "<script type='text/javascript' src='templates/{0}/mediaad.js'></script><script type='text/javascript'>printMediaAD('{1}', {2});</script>";
            }
            return result;
            #endregion
        }

        public string GetParameters()
        {
            #region ����ѡȡ�����͵õ�����ֵ

            string result = "";

            switch (parameters.SelectedValue)
            {
                case "htmlcode":
                    result = "htmlcode|||||||";
                    break;
                case "word":
                    result = string.Format("word| | | | {0}|{1}|{2}|", wordlink.Text.Trim(), wordcontent.Text.Trim(), wordfont.Text);
                    break;
                case "image":
                    result = string.Format("image|{0}|{1}|{2}|{3}|{4}||", imgsrc.Text.Trim(), imgwidth.Text.Trim(), imgheight.Text.Trim(), imglink.Text.Trim(), imgtitle.Text.Trim());
                    break;
                case "flash":
                    result = string.Format("flash|{0}|{1}|{2}||||", flashsrc.Text.Trim(), flashwidth.Text.Trim(),flashheight.Text);
                    break;
            }

            if (type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
            {
                result = string.Format("silverlight|{0}|{1}|{2}|{3}|{4}|{5}|{6}" ,
                    slwmvsrc.Text.Trim(),slimage.Text.Trim(),slimage.Text,buttomimg.Text,words1.Text,words2.Text,words3.Text);
            }

            if (type.SelectedValue == Convert.ToInt16(AdType.InPostAd).ToString())
            {
                result += string.Format("{0}|{1}|",inpostposition.SelectedValue,GetMultipleSelectedValue(inpostfloor));
            }

            return result;
            #endregion
        }

        private string GetMultipleSelectedValue(Discuz.Control.ListBox lb)
        {
            string result = string.Empty;
            foreach (ListItem li in lb.Items)
            {
                if (li.Selected && li.Value != "-1")
                    result += li.Value + ",";
            }

            if (result.Length > 0)
                result = result.Substring(0, result.Length - 1);

            return result;
        }

        private void type_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region ���ݹ���������ò����ؼ���������

            if ((type.SelectedValue == Convert.ToInt16(AdType.FloatAd).ToString()) || (type.SelectedValue == Convert.ToInt16(AdType.DoubleAd).ToString()))
            {
                if (parameters.Items[1].Value == "word")
                    parameters.Items.RemoveAt(1);
            }
            else
            {
                if (parameters.Items[1].Value != "word")
                    parameters.Items.Insert(1, new ListItem("����", "word"));
            }
            #endregion
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
            this.type.SelectedIndexChanged += new EventHandler(this.type_SelectedIndexChanged);
            this.AddAdInfo.Click += new EventHandler(this.AddAdInfo_Click);

            #region �ؼ����ݰ�

            starttime.SelectedDate = DateTime.Now;
            endtime.SelectedDate = DateTime.Now.AddDays(7);

            title.AddAttributes("maxlength", "40");
            title.AddAttributes("size", "40");

            //������
            type.Items.Clear();
            type.Items.Add(new ListItem("ͷ��������", Convert.ToInt16(AdType.HeaderAd).ToString()));
            type.Items.Add(new ListItem("β��������", Convert.ToInt16(AdType.FooterAd).ToString()));
            type.Items.Add(new ListItem("ҳ�����ֹ��", Convert.ToInt16(AdType.PageWordAd).ToString()));
            type.Items.Add(new ListItem("���ڹ��", Convert.ToInt16(AdType.InPostAd).ToString()));
            type.Items.Add(new ListItem("����ͨ�����", Convert.ToInt16(AdType.PostLeaderboardAd).ToString()));
            type.Items.Add(new ListItem("�������", Convert.ToInt16(AdType.FloatAd).ToString()));
            type.Items.Add(new ListItem("�������", Convert.ToInt16(AdType.DoubleAd).ToString()));
            type.Items.Add(new ListItem("�������", Convert.ToInt16(AdType.InForumAd).ToString()));
            type.Items.Add(new ListItem("���ٷ������Ϸ����", Convert.ToInt16(AdType.QuickEditorAd).ToString()));
            type.Items.Add(new ListItem("���ٱ༭���������", Convert.ToInt16(AdType.QuickEditorBgAd).ToString()));    

            type.Items.Add(new ListItem("�ۺ���ҳͷ�����", Convert.ToInt16(AdType.WebSiteHeaderAd).ToString()));
            type.Items.Add(new ListItem("�ۺ���ҳ�����·����", Convert.ToInt16(AdType.WebSiteHotTopicAd).ToString()));
            type.Items.Add(new ListItem("�ۺ���ҳ���������Ϸ����", Convert.ToInt16(AdType.WebSiteUserPostTopAd).ToString()));
            type.Items.Add(new ListItem("�ۺ���ҳ�Ƽ�����Ϸ����", Convert.ToInt16(AdType.WebSiteRecForumTopAd).ToString()));
            type.Items.Add(new ListItem("�ۺ���ҳ�Ƽ�����·����", Convert.ToInt16(AdType.WebSiteRecForumBottomAd).ToString()));
            type.Items.Add(new ListItem("�ۺ���ҳ�Ƽ�����·����", Convert.ToInt16(AdType.WebSiteRecAlbumAd).ToString()));
            type.Items.Add(new ListItem("�ۺ���ҳ�ײ����", Convert.ToInt16(AdType.WebSiteBottomAd).ToString()));
            type.Items.Add(new ListItem("ҳ�ں�����",Convert.ToInt16(AdType.PageAd).ToString()));
            type.Attributes.Add("onChange", "showadhint();");
            type.SelectedIndex = 0;

            parameters.Items.Clear();
            parameters.Items.Add(new ListItem("����", "htmlcode"));
            parameters.Items.Add(new ListItem("����", "word"));
            parameters.Items.Add(new ListItem("ͼƬ", "image"));
            parameters.Items.Add(new ListItem("flash", "flash"));
            parameters.Attributes.Add("onChange", "showparameters();");
            parameters.SelectedIndex = 0;

            #endregion
        }

        #endregion
    }
}