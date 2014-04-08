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
    /// 基本设置
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
            #region 加载配置信息
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

            //如果无动作离线时间是大于等于0的，则代表帖子显示作者状态是精确判断的，否则就是简单判断的
            showauthorstatusinpost.SelectedValue = configInfo.Onlinetimeout >= 0 ? "2" : "1";
            //如果无动作离线时间是负数，则需要将它以正数形式显示出来
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
            #region 保存信息
            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                Hashtable HT = new Hashtable();
                HT.Add("最大在线人数", maxonlines.Text);
                HT.Add("搜索时间限制", searchctrl.Text);
                foreach (DictionaryEntry de in HT)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + de.Key.ToString().Trim() + ",只能是0或者正整数');window.location.href='global_safecontrol.aspx';</script>");
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
                      base.RegisterStartupScript("", "<script>alert('通知保留天数只能为正数或0!');</script>");
                      return;
                  }

                  if (!Utils.IsInt(maxindexsubforumcount.Text) || Utils.StrToInt(maxindexsubforumcount.Text, -1) < 0)
                  {
                      base.RegisterStartupScript("", "<script>alert('首页每个分类下最多显示版块数只能为正数或0!');</script>");
                      return;
                  }

                  if (!Utils.IsInt(deletingexpireduserfrequency.Text) || Utils.StrToInt(deletingexpireduserfrequency.Text, 0) < 1)
                  {
                      base.RegisterStartupScript("", "<script>alert('删除离线用户频率只能为正数!');</script>");
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

                //如果帖子显示作者状态是简单判断，则保存无动作离线时间为负数值
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
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "站点优化", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_siteoptimization.aspx';");
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