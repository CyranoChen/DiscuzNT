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
                    //数据来源版块设定范围
                    string forumlist = DNTRequest.GetString("forumlist");
                    //显示版块名称长度限制
                    int forumLength = DNTRequest.GetInt("forumnamelength", 0);
                    //显示主题标题长度限制
                    int titleLength = DNTRequest.GetInt("topictitlelength", 0);
                    //数据来源时间设定范围
                    string dataTimeType = DNTRequest.GetString("datatimetype");
                    //热点信息类型
                    string dataType = DNTRequest.GetString("datatype");
                    //热点信息排序类型
                    string sortType = DNTRequest.GetString("sorttype");
                    //热点信息名称
                    string forumHotName = DNTRequest.GetString("forumhotitemname");
                    //是否启用
                    int enabled = DNTRequest.GetInt("itemenabled", 0);
                    //读取最大数据条数
                    int dataCount = DNTRequest.GetInt("datacount", 0);
                    //数据缓存时间
                    int cacheTimeOut = DNTRequest.GetInt("cachetime", 0);

                    //限制enabled值在0-1之间
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
                            //如果此时sortType=posts,另外给它赋值为dataTimeType的值供前台方法调用
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
