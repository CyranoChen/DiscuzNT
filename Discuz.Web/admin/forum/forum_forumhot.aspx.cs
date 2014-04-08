using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Cache;
using Discuz.Common.Generic;

namespace Discuz.Web.Admin
{
    public partial class forumhot : AdminPage
    {
        public string action = DNTRequest.GetString("action");
        public int id = DNTRequest.GetInt("id", -1);
        protected global::Discuz.Web.Admin.pageinfo info1;
        public ForumHotConfigInfo forumHotConfigInfo = ForumHotConfigs.GetConfig();
        public ForumHotItemInfo forumHotItem = new ForumHotItemInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (action)
            {
                case "setenabled":
                    forumHotConfigInfo.Enable = DNTRequest.GetInt("enabled", 0) == 1;
                    ForumHotConfigs.SaveConfig(forumHotConfigInfo);
                    Response.Redirect("forum_forumhot.aspx");
                    break;

                case "edit":
                    forumHotItem = forumHotConfigInfo.ForumHotCollection[id - 1];
                    break;
                case "editsave":
                    //������Դ����趨��Χ
                    string forumlist = DNTRequest.GetString("forumlist");
                    //��ʾ������Ƴ�������
                    int forumLength = DNTRequest.GetInt("forumnamelength", 0);
                    //��ʾ������ⳤ������
                    int titleLength = DNTRequest.GetInt("topictitlelength", 0);
                    //������Դʱ���趨��Χ
                    string dataTimeType = DNTRequest.GetString("datatimetype");
                    //�ȵ���Ϣ����
                    string dataType = DNTRequest.GetString("datatype");
                    //�ȵ���Ϣ��������
                    string sortType = DNTRequest.GetString("sorttype");
                    //�ȵ���Ϣ����
                    string forumHotName = DNTRequest.GetString("forumhotitemname");
                    //�Ƿ�����
                    int enabled = DNTRequest.GetInt("itemenabled", 0);
                    //��ȡ�����������
                    int dataCount = DNTRequest.GetInt("datacount", 0);
                    //���ݻ���ʱ��
                    int cacheTimeOut = DNTRequest.GetInt("cachetime", 0);

                    //����enabledֵ��0-1֮��
                    enabled = enabled < 0 ? 0 : (enabled > 1 ? 1 : enabled);

                    cacheTimeOut = cacheTimeOut < 0 ? 1 : cacheTimeOut;


                    switch (dataType)
                    {
                        case "topics":
                            forumLength = forumLength < 0 ? 0 : forumLength;
                            titleLength = titleLength < 0 ? 0 : titleLength;
                            break;
                        case "forums":
                            forumlist = string.Empty;
                            dataTimeType = string.Empty;
                            forumLength = forumLength < 0 ? 0 : forumLength;
                            titleLength = 0;
                            break;
                        case "users":
                            forumlist = string.Empty;
                            forumLength = 0;
                            titleLength = 0;
                            //�����ʱsortType=posts,���������ֵΪdataTimeType��ֵ��ǰ̨��������
                            sortType = sortType == "posts" ? dataTimeType : sortType;
                            break;
                        case "pictures":
                            titleLength = titleLength < 0 ? 0 : titleLength;
                            forumLength = 0;
                            break;
                    }
                    forumHotConfigInfo.ForumHotCollection[id - 1].Name = forumHotName;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Enabled = enabled;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Datatype = dataType;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Sorttype = sortType;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Forumlist = forumlist;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Dataitemcount = dataCount;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Datatimetype = dataTimeType;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Cachetimeout = cacheTimeOut;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Forumnamelength = forumLength;
                    forumHotConfigInfo.ForumHotCollection[id - 1].Topictitlelength = titleLength;

                    ForumHotConfigs.SaveConfig(forumHotConfigInfo);
                    DNTCache.GetCacheService().RemoveObject("/Forum/ForumHot");
                    DNTCache.GetCacheService().RemoveObject("/Forum/ForumHostList-" + id);
                    DNTCache.GetCacheService().RemoveObject("/Aggregation/HotForumList" + id);
                    DNTCache.GetCacheService().RemoveObject("/Aggregation/Users_" + id + "List");
                    DNTCache.GetCacheService().RemoveObject("/Aggregation/HotImages_" + id + "List");
                    Response.Redirect("forum_forumhot.aspx");
                    break;
            }
        }
    }
}
