using System;
using System.IO;
using System.Web.UI;
using Discuz.Cache;
using Discuz.Control;
using Discuz.Forum;

using Discuz.Aggregation;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// �������
    /// </summary>

    public partial class cachemanage : AdminPage
    {
        private void ReSetDigestTopicList_Click(object sender, EventArgs e)
        {
            #region ��������ȫ����龫�������б�

            if (this.CheckCookie())
            {
                Caches.ReSetDigestTopicList(16);
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetHotTopicList_Click(object sender, EventArgs e)
        {
            #region ��������ȫ��������������б�

            if (this.CheckCookie())
            {
                Caches.ReSetHotTopicList(16, 30);
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetAdsList_Click(object sender, EventArgs e)
        {
            #region �������б�

            if (this.CheckCookie())
            {
                Caches.ReSetAdsList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetStatisticsSearchtime_Click(object sender, EventArgs e)
        {
            #region ���������û���һ��ִ������������ʱ��

            if (this.CheckCookie())
            {
                Caches.ReSetStatisticsSearchtime();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetStatisticsSearchcount_Click(object sender, EventArgs e)
        {
            #region ���������û���һ�����������Ĵ���

            if (this.CheckCookie())
            {
                Caches.ReSetStatisticsSearchcount();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetCommonAvatarList_Click(object sender, EventArgs e)
        {
            #region ���������û�ͷ���б�

            if (this.CheckCookie())
            {
                Caches.ReSetCommonAvatarList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetJammer_Click(object sender, EventArgs e)
        {
            #region �������ø������ַ���

            if (this.CheckCookie())
            {
                Caches.ReSetJammer();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetMagicList_Click(object sender, EventArgs e)
        {
            #region ��������ħ���б�

            if (this.CheckCookie())
            {
                Caches.ReSetMagicList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetScorePaySet_Click(object sender, EventArgs e)
        {
            #region �������öһ����ʵĿɽ��׻��ֲ���

            if (this.CheckCookie())
            {
                Caches.ReSetScorePaySet();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetPostTableInfo_Click(object sender, EventArgs e)
        {
            #region �������õ�ǰ���ӱ������Ϣ

            if (this.CheckCookie())
            {
                Caches.ReSetPostTableInfo();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetTopiclistByFid_Click(object sender, EventArgs e)
        {
            #region ����������Ӧ�������б�

            if (this.CheckCookie())
            {
                if (txtTopiclistFid.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('����������Ӧ�����б�İ�������Ч!');window.location.href='global_cachemanage.aspx';</script>");
                    return;
                }
                Caches.ReSetTopiclistByFid(txtTopiclistFid.Text);
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetMGinf_Click(object sender, EventArgs e)
        {
            #region �������ù�������Ϣ

            if (this.CheckCookie())
            {
                Caches.ReSetAdminGroupList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetUGinf_Click(object sender, EventArgs e)
        {
            #region ���������û�����Ϣ

            if (this.CheckCookie())
            {
                Caches.ReSetUserGroupList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumInf_Click(object sender, EventArgs e)
        {
            #region �������ð�����Ϣ

            if (this.CheckCookie())
            {
                Caches.ReSetModeratorList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAnnonceList_Click(object sender, EventArgs e)
        {
            #region ��������ָ��ʱ���ڵĹ����б�

            if (this.CheckCookie())
            {
                Caches.ReSetAnnouncementList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetFirstAnnounce_Click(object sender, EventArgs e)
        {
            #region �������õ�һ������

            if (this.CheckCookie())
            {
                Caches.ReSetSimplifiedAnnouncementList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumDropList_Click(object sender, EventArgs e)
        {
            #region �������ð�������б�

            if (this.CheckCookie())
            {
                Caches.ReSetForumListBoxOptions();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetSmiles_Click(object sender, EventArgs e)
        {
            #region �������ñ���

            if (this.CheckCookie())
            {
                Caches.ReSetSmiliesList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetThemeIcon_Click(object sender, EventArgs e)
        {
            #region ������������ͼ��

            if (this.CheckCookie())
            {
                Caches.ReSetIconsList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumBaseSet_Click(object sender, EventArgs e)
        {
            #region ����������̳��������

            if (this.CheckCookie())
            {
                Caches.ReSetConfig();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAddressRefer_Click(object sender, EventArgs e)
        {
            #region �������õ�ַ���ձ�

            if (this.CheckCookie())
            {
                Caches.ReSetSiteUrls();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumsStaticInf_Click(object sender, EventArgs e)
        {
            #region ����������̳ͳ����Ϣ

            if (this.CheckCookie())
            {
                Caches.ReSetStatistics();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAllCache_Click(object sender, EventArgs e)
        {
            #region �������л���

            if (this.CheckCookie())
            {
                Caches.ReSetAllCache();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetScoreset_Click(object sender, EventArgs e)
        {
            #region ����������̳��������

            if (this.CheckCookie())
            {
                Caches.ReSetScoreset();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAttachSize_Click(object sender, EventArgs e)
        {
            #region ��������ϵͳ����ĸ������ͺʹ�С

            if (this.CheckCookie())
            {
                Caches.ReSetAttachmentTypeArray();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetTemplateDropDown_Click(object sender, EventArgs e)
        {
            #region ��������ģ���б��������html

            if (this.CheckCookie())
            {
                Caches.ReSetTemplateListBoxOptionsCache();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetOnlineInco_Click(object sender, EventArgs e)
        {
            #region �������������û��б�ͼ��

            if (this.CheckCookie())
            {
                Caches.ReSetOnlineGroupIconList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetLink_Click(object sender, EventArgs e)
        {
            #region �����������������б�

            if (this.CheckCookie())
            {
                Caches.ReSetForumLinkList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetWord_Click(object sender, EventArgs e)
        {
            #region �����������ֹ����б�

            if (this.CheckCookie())
            {
                Caches.ReSetBanWordList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumList_Click(object sender, EventArgs e)
        {
            #region ����������̳�б�

            if (this.CheckCookie())
            {
                Caches.ReSetForumList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetRss_Click(object sender, EventArgs e)
        {
            #region ����������̳RSS

            if (this.CheckCookie())
            {
                Caches.ReSetRss();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetOnlineUserInfo_Click(object sender, EventArgs e)
        {
            #region �������������û���Ϣ

            if (this.CheckCookie())
            {
                Caches.ReSetOnlineUserTable();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetRssByFid_Click(object sender, EventArgs e)
        {
            #region ��������ָ�����RSS

            if (this.CheckCookie())
            {
                if (txtRssfid.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('��������ָ�����RSS�İ�������Ч!');window.location.href='global_cachemanage.aspx';</script>");
                    return;
                }

                Caches.ReSetForumRssXml(Convert.ToInt32(txtRssfid.Text));
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetRssAll_Click(object sender, EventArgs e)
        {
            #region ����������̳����RSS

            if (this.CheckCookie())
            {
                Caches.ReSetRssXml();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetTemplateIDList_Click(object sender, EventArgs e)
        {
            #region ����������̳ģ��id�б�

            if (this.CheckCookie())
            {
                Caches.ReSetValidTemplateIDList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetValidUserExtField_Click(object sender, EventArgs e)
        {
            #region ����������Ч���û�����չ�ֶ�

            if (this.CheckCookie())
            {
                Caches.ReSetValidScoreName();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetFlag_Click(object sender, EventArgs e)
        {
            #region ���������Զ����ǩ

            if (this.CheckCookie())
            {
                Caches.ReSetCustomEditButtonList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetMedalList_Click(object sender, EventArgs e)
        {
            #region ��������ѫ���б�

            if (this.CheckCookie())
            {
                Caches.ReSetMedalsList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetAggregation_Click(object sender, EventArgs e)
        {
            #region �������þۺ�
            if (this.CheckCookie())
            {
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                SubmitReturnInf();
            }
            #endregion
        }

        protected void ReSetNavPopupMenu_Click(object sender, EventArgs e)
        {
            #region ���赼�������˵�
            if (this.CheckCookie())
            {
                Caches.ReSetNavPopupMenu();
            }
            #endregion
        }

        private void SubmitReturnInf()
        {
            if (this.CheckCookie())
            {
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_cachemanage.aspx';");
            }
        }

        private void ReSetTag_Click(object sender, EventArgs e)
        {
            DNTCache cache = DNTCache.GetCacheService();
            cache.RemoveObject("/Forum/Tag/Hot-" + config.Hottagcount);
        }

        protected void ReSetAlbumCategory_Click(object sender, EventArgs e)
        {
            #region ��������������
            if (this.CheckCookie())
            {
                Caches.ResetAlbumCategory();
                SubmitReturnInf();
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
            this.ResetMGinf.Click += new EventHandler(this.ResetMGinf_Click);
            this.ResetUGinf.Click += new EventHandler(this.ResetUGinf_Click);
            this.ResetForumInf.Click += new EventHandler(this.ResetForumInf_Click);
            this.ResetAnnonceList.Click += new EventHandler(this.ResetAnnonceList_Click);
            this.ResetFirstAnnounce.Click += new EventHandler(this.ResetFirstAnnounce_Click);
            this.ResetForumDropList.Click += new EventHandler(this.ResetForumDropList_Click);
            this.ResetSmiles.Click += new EventHandler(this.ResetSmiles_Click);
            this.ResetThemeIcon.Click += new EventHandler(this.ResetThemeIcon_Click);
            this.ResetForumBaseSet.Click += new EventHandler(this.ResetForumBaseSet_Click);
            this.ReSetScoreset.Click += new EventHandler(this.ReSetScoreset_Click);
            this.ResetAddressRefer.Click += new EventHandler(this.ResetAddressRefer_Click);
            this.ResetForumsStaticInf.Click += new EventHandler(this.ResetForumsStaticInf_Click);
            this.ResetAttachSize.Click += new EventHandler(this.ResetAttachSize_Click);
            this.ResetTemplateDropDown.Click += new EventHandler(this.ResetTemplateDropDown_Click);
            this.ResetOnlineInco.Click += new EventHandler(this.ResetOnlineInco_Click);
            this.ResetLink.Click += new EventHandler(this.ResetLink_Click);
            this.ResetWord.Click += new EventHandler(this.ResetWord_Click);
            this.ResetForumList.Click += new EventHandler(this.ResetForumList_Click);
            this.ResetRss.Click += new EventHandler(this.ResetRss_Click);
            this.ResetRssByFid.Click += new EventHandler(this.ResetRssByFid_Click);
            this.ResetRssAll.Click += new EventHandler(this.ResetRssAll_Click);
            this.ResetTemplateIDList.Click += new EventHandler(this.ResetTemplateIDList_Click);
            this.ResetValidUserExtField.Click += new EventHandler(this.ResetValidUserExtField_Click);
            this.ResetOnlineUserInfo.Click += new EventHandler(this.ResetOnlineUserInfo_Click);
            this.ResetAllCache.Click += new EventHandler(this.ResetAllCache_Click);
            this.ResetFlag.Click += new EventHandler(this.ResetFlag_Click);
            this.ResetMedalList.Click += new EventHandler(this.ResetMedalList_Click);
            this.ReSetAdsList.Click += new EventHandler(this.ReSetAdsList_Click);
            this.ReSetStatisticsSearchtime.Click += new EventHandler(this.ReSetStatisticsSearchtime_Click);
            this.ReSetStatisticsSearchcount.Click += new EventHandler(this.ReSetStatisticsSearchcount_Click);
            this.ReSetCommonAvatarList.Click += new EventHandler(this.ReSetCommonAvatarList_Click);
            this.ReSetJammer.Click += new EventHandler(this.ReSetJammer_Click);
            this.ReSetMagicList.Click += new EventHandler(this.ReSetMagicList_Click);
            this.ReSetScorePaySet.Click += new EventHandler(this.ReSetScorePaySet_Click);
            this.ReSetPostTableInfo.Click += new EventHandler(this.ReSetPostTableInfo_Click);
            this.ReSetTopiclistByFid.Click += new EventHandler(this.ReSetTopiclistByFid_Click);
            this.ReSetDigestTopicList.Click += new EventHandler(this.ReSetDigestTopicList_Click);
            this.ReSetHotTopicList.Click += new EventHandler(this.ReSetHotTopicList_Click);
            this.ReSetAggregation.Click += new EventHandler(this.ReSetAggregation_Click);
            this.ReSetTag.Click += new EventHandler(this.ReSetTag_Click);
        }

        #endregion

    }
}