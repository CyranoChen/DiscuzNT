using System;
using System.Web.UI;
using System.Data;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;
using System.Collections;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��������
    /// </summary>

    public partial class siteoptimization : AdminPage
    {
        protected Discuz.Control.RadioButtonList iisurlrewrite;
        protected bool haveAlbum;
        protected bool haveSpace;
        protected bool haveMall;
        protected void Page_Load(object sender, EventArgs e)
        {
            haveAlbum = AlbumPluginProvider.GetInstance() != null;
            haveSpace = SpacePluginProvider.GetInstance() != null;
            haveMall = MallPluginProvider.GetInstance() != null;
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            #region ����������Ϣ
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            fulltextsearch.SelectedValue = configInfo.Fulltextsearch.ToString();
            nocacheheaders.SelectedValue = configInfo.Nocacheheaders.ToString();
            maxonlines.Text = configInfo.Maxonlines.ToString();
            searchctrl.Text = configInfo.Searchctrl.ToString();
            statscachelife.Text = configInfo.Statscachelife.ToString();
            guestcachepagetimeout.Text = configInfo.Guestcachepagetimeout.ToString();
            oltimespan.Text = configInfo.Oltimespan.ToString();
            topiccachemark.Text = configInfo.Topiccachemark.ToString();
            notificationreserveddays.Text = configInfo.Notificationreserveddays.ToString();
            maxindexsubforumcount.Text = configInfo.Maxindexsubforumcount.ToString();
            deletingexpireduserfrequency.Text = configInfo.Deletingexpireduserfrequency.ToString();
            onlineoptimization.SelectedValue = configInfo.Onlineoptimization.ToString();
            avatarmethod.SelectedValue = configInfo.AvatarMethod.ToString();
            //showattachmentpath.SelectedValue = configInfo.Showattachmentpath.ToString();
            showimgattachmode.SelectedValue = configInfo.Showimgattachmode.ToString();
            onlineusercountcacheminute.Text = configInfo.OnlineUserCountCacheMinute.ToString();
            posttimestoragemedia.SelectedValue = configInfo.PostTimeStorageMedia.ToString();

            //����޶�������ʱ���Ǵ��ڵ���0�ģ������������ʾ����״̬�Ǿ�ȷ�жϵģ�������Ǽ��жϵ�
            showauthorstatusinpost.SelectedValue = configInfo.Onlinetimeout >= 0 ? "2" : "1";
            //����޶�������ʱ���Ǹ���������Ҫ������������ʽ��ʾ����
            onlinetimeout.Text = ((configInfo.Onlinetimeout > 0 ? 1 : -1) * configInfo.Onlinetimeout).ToString();

            if (configInfo.TopicQueueStats == 1)
            {
                Topicqueuestats_1.Checked = true;
                Topicqueuestats_0.Checked = false;
                topicqueuestatscount.AddAttributes("style", "visibility:visible;");
            }
            else
            {
                Topicqueuestats_0.Checked = true;
                Topicqueuestats_1.Checked = false;
                topicqueuestatscount.AddAttributes("style", "visibility:hidden;");
            }

            topicqueuestatscount.Text = configInfo.TopicQueueStatsCount.ToString();
            jqueryurl.Text = configInfo.Jqueryurl;
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ������Ϣ
            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                Hashtable HT = new Hashtable();
                HT.Add("�����������", maxonlines.Text);
                HT.Add("����ʱ������", searchctrl.Text);
                foreach (DictionaryEntry de in HT)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('�������:" + de.Key.ToString().Trim() + ",ֻ����0����������');window.location.href='global_safecontrol.aspx';</script>");
                        return;
                    }
                }

                if (fulltextsearch.SelectedValue == "1")
                {
                    string msg = "";
                    configInfo.Fulltextsearch = Databases.TestFullTextIndex(ref msg);
                }
                else
                    configInfo.Fulltextsearch = 0;

                  if (Topicqueuestats_1.Checked == true)
                {
                    configInfo.TopicQueueStats = 1;
                }
                else
                {
                    configInfo.TopicQueueStats = 0;
                }

                  if (!Utils.IsInt(notificationreserveddays.Text) || Utils.StrToInt(notificationreserveddays.Text, -1) < 0)
                  {
                      base.RegisterStartupScript("", "<script>alert('֪ͨ��������ֻ��Ϊ������0!');</script>");
                      return;
                  }

                  if (!Utils.IsInt(maxindexsubforumcount.Text) || Utils.StrToInt(maxindexsubforumcount.Text, -1) < 0)
                  {
                      base.RegisterStartupScript("", "<script>alert('��ҳÿ�������������ʾ�����ֻ��Ϊ������0!');</script>");
                      return;
                  }

                  if (!Utils.IsInt(deletingexpireduserfrequency.Text) || Utils.StrToInt(deletingexpireduserfrequency.Text, 0) < 1)
                  {
                      base.RegisterStartupScript("", "<script>alert('ɾ�������û�Ƶ��ֻ��Ϊ����!');</script>");
                      return;
                  }
                configInfo.Deletingexpireduserfrequency = Utils.StrToInt(deletingexpireduserfrequency.Text, 1);
                configInfo.Maxindexsubforumcount = Utils.StrToInt(maxindexsubforumcount.Text, 0);
                configInfo.Notificationreserveddays = Utils.StrToInt(notificationreserveddays.Text, 0);

                configInfo.TopicQueueStatsCount = Convert.ToInt32(topicqueuestatscount.Text);

                configInfo.Nocacheheaders = Convert.ToInt16(nocacheheaders.SelectedValue);
                configInfo.Maxonlines = Convert.ToInt32(maxonlines.Text);
                configInfo.Searchctrl = Convert.ToInt32(searchctrl.Text);
                configInfo.Statscachelife = Convert.ToInt16(statscachelife.Text);
                configInfo.Guestcachepagetimeout = Convert.ToInt16(guestcachepagetimeout.Text);
                configInfo.Oltimespan = Convert.ToInt16(oltimespan.Text);
                configInfo.Topiccachemark = Convert.ToInt16(topiccachemark.Text);
                configInfo.Onlineoptimization = Convert.ToInt32(onlineoptimization.SelectedValue);
                configInfo.AvatarMethod = Convert.ToInt16(avatarmethod.SelectedValue);
                //configInfo.Showattachmentpath = Convert.ToInt16(showattachmentpath.SelectedValue);
                configInfo.Showimgattachmode = Convert.ToInt16(showimgattachmode.SelectedValue);
                configInfo.TopicQueueStats = Topicqueuestats_1.Checked ? 1 : 0;
                configInfo.OnlineUserCountCacheMinute = Convert.ToInt32(onlineusercountcacheminute.Text);
                configInfo.PostTimeStorageMedia = Convert.ToInt32(posttimestoragemedia.SelectedValue);

                //���������ʾ����״̬�Ǽ��жϣ��򱣴��޶�������ʱ��Ϊ����ֵ
                configInfo.Onlinetimeout = showauthorstatusinpost.SelectedValue == "1" ? 0 - TypeConverter.StrToInt(onlinetimeout.Text) : TypeConverter.StrToInt(onlinetimeout.Text);
                configInfo.Jqueryurl = jqueryurl.Text;
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                if (configInfo.Aspxrewrite == 1)
                    AdminForums.SetForumsPathList(true, configInfo.Extname);
                else
                    AdminForums.SetForumsPathList(false, configInfo.Extname);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
                Discuz.Forum.TopicStats.SetQueueCount();
                Caches.ReSetConfig();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "վ���Ż�", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_siteoptimization.aspx';");
            }
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