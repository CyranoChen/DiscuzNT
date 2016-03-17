using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 搜索页面
    /// </summary>
    public class search : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 搜索缓存Id
        /// </summary>
        public int searchid = DNTRequest.GetInt("searchid", -1);
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 主题数量
        /// </summary>
        public int topiccount;
        /// <summary>
        /// 相册数量
        /// </summary>
        public int albumcount;
        /// <summary>
        /// 日志数量
        /// </summary>
        public int blogcount;
        /// <summary>
        /// 分页数量
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 搜索结果数量
        /// </summary>
        public int searchresultcount = 0;
        /// <summary>
        /// 搜索出的主题列表
        /// </summary>
        public DataTable topiclist = new DataTable();
        /// <summary>
        /// 帖子分表列表
        /// </summary>
        public DataTable tablelist;
        /// <summary>
        /// 搜索出的日志列表
        /// </summary>
        public DataTable spacepostlist = new DataTable();
        /// <summary>
        /// 搜索出的相册列表
        /// </summary>
        public DataTable albumlist = new DataTable();
        /// <summary>
        /// 当此值为true时,显示搜索结果提示
        /// </summary>
        public bool searchpost = false;
        /// <summary>
        /// 搜索类型
        /// </summary>
        public string type = Utils.HtmlEncode(DNTRequest.GetString("type").ToLower());
        /// <summary>
        /// 当前主题页码
        /// </summary>
        public int topicpageid = DNTRequest.GetInt("topicpage", 1);
        /// <summary>
        /// 主题分页总数
        /// </summary>
        public int topicpagecount;
        /// <summary>
        /// 主题分页页码链接
        /// </summary>
        public string topicpagenumbers = "";
        /// <summary>
        /// 当前日志分页页码
        /// </summary>
        public int blogpageid = DNTRequest.GetInt("blogpage", 1);
        /// <summary>
        /// 日志分页总数
        /// </summary>
        public int blogpagecount;
        /// <summary>
        /// 日志分页页码链接
        /// </summary>
        public string blogpagenumbers = "";
        /// <summary>
        /// 当前相册页码
        /// </summary>
        public int albumpageid = DNTRequest.GetInt("albumpage", 1);
        /// <summary>
        /// 相册分页总数
        /// </summary>
        public int albumpagecount;
        /// <summary>
        /// 相册分页页码链接
        /// </summary>
        public string albumpagenumbers = "";
        /// <summary>
        /// 提示信息
        /// </summary>
        string msg = "";
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string keyword = Utils.HtmlEncode(DNTRequest.GetString("keyword").Trim());
        /// <summary>
        /// 搜索作者用户名
        /// </summary>
        public string poster = Utils.HtmlEncode(DNTRequest.GetString("poster").Trim());
        /// <summary>
        /// 是否显示高级搜索
        /// </summary>
        public int advsearch = DNTRequest.GetInt("advsearch", 0);
        /// <summary>
        /// 是否是get提交的查询任务
        /// </summary>
        public int searchsubmit = DNTRequest.GetInt("searchsubmit", 0);
        /// <summary>
        /// 帖子分表ID
        /// </summary>
        public int posttableid = DNTRequest.GetInt("posttableid", 0);
        /// <summary>
        /// 查询类别枚举
        /// </summary>
        public SearchType searchType;
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "搜索";

            GetSearchType();

            //判断当前操作是否是用户打开的页面
            if (searchsubmit == 0 && !ispost)
            {
                //用户权限校验
                if (!UserAuthority.Search(usergroupinfo, ref msg))
                {
                    AddErrLine(msg);
                    return;
                }

                //读取分表信息
                if (searchid <= 0)
                {
                    tablelist = Posts.GetAllPostTableName();
                }
                else
                {
                    if (searchType == SearchType.Error)
                    {
                        AddErrLine("非法的参数信息");
                        return;
                    }

                    switch (searchType)
                    {
                        case SearchType.SpacePostTitle:
                            spacepostlist = Searches.GetSearchCacheList(posttableid, searchid, 16, pageid, out topiccount, searchType);
                            break;
                        case SearchType.AlbumTitle:
                            albumlist = Searches.GetSearchCacheList(posttableid, searchid, 16, pageid, out topiccount, searchType);
                            break;
                        case SearchType.ByPoster:
                            topiclist = Searches.GetSearchCacheList(posttableid, searchid, 16, topicpageid, out topiccount, SearchType.TopicTitle);
                            topicpageid = CalculateCurrentPage(topiccount, topicpageid, out topicpagecount);

                            topicpagenumbers = topicpagecount > 1 ? Utils.GetPageNumbers(topicpageid, topicpagecount, "search.aspx?type=" + type + "&searchid=" + searchid.ToString() + "&keyword=" + keyword + "&poster=" + poster, 8, "topicpage", "#1") : "";
                            return;

                        case SearchType.PostContent:
                        default:
                            topiclist = Searches.GetSearchCacheList(posttableid, searchid, 16, pageid, out topiccount, searchType);
                            break;
                    }

                    if (topiccount == 0)
                    {
                        AddErrLine("不存在的searchid");
                        return;
                    }
                    CalculateCurrentPage();
                    //得到页码链接
                    pagenumbers = pagecount > 1 ? Utils.GetPageNumbers(pageid, pagecount, "search.aspx?type=" + type + "&searchid=" + searchid.ToString() + "&keyword=" + keyword + "&poster=" + poster, 8) : "";
                }
            }
            else
            {
                //检查用户的搜索权限，包括搜索时间间隔的限制
                if (!UserAuthority.Search(userid, lastsearchtime, useradminid, usergroupinfo, ref msg))
                {
                    AddErrLine(msg);
                    return;
                }

                if (searchType == SearchType.Error)
                {
                    AddErrLine("非法的参数信息");
                    return;
                }

                searchpost = true;
                string searchforumid = DNTRequest.GetString("searchforumid").Trim();
                int posterid = CheckSearchInfo(searchforumid);
                if (IsErr()) return;

                //if (Utils.StrIsNullOrEmpty(keyword) && posterid > 0 && Utils.StrIsNullOrEmpty(type))
                //{
                //    type = "author";
                //    searchType = SearchType.ByPoster;
                //}

                searchid = Searches.Search(posttableid, userid, usergroupid, keyword, posterid, searchType, searchforumid, DNTRequest.GetInt("searchtime", 0), DNTRequest.GetInt("searchtimetype", 0), DNTRequest.GetInt("resultorder", 0), DNTRequest.GetInt("resultordertype", 0));
                if (searchid > 0)
                    System.Web.HttpContext.Current.Response.Redirect(forumpath + "search.aspx?type=" + type + "&searchid=" + searchid + "&keyword=" + keyword + "&poster=" + poster + "&posttableid=" + posttableid, false);
                else
                {
                    AddErrLine("抱歉, 没有搜索到符合要求的记录");
                    return;
                }
            }
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        private void CalculateCurrentPage()
        {
            //获取总页数
            pagecount = topiccount % 16 == 0 ? topiccount / 16 : topiccount / 16 + 1;
            pagecount = (pagecount == 0 ? 1 : pagecount);
            //修正请求页数中可能的错误
            pageid = (pageid < 1 ? 1 : pageid);
            pageid = (pageid > pagecount ? pagecount : pageid);
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        private int CalculateCurrentPage(int listcount, int pageid, out int pagecount)
        {
            //获取总页数
            pagecount = listcount % 16 == 0 ? listcount / 16 : listcount / 16 + 1;
            pagecount = (pagecount == 0 ? 1 : pagecount);
            //修正请求页数中可能的错误
            pageid = (pageid < 1 ? 1 : pageid);
            pageid = (pageid > pagecount ? pagecount : pageid);
            return pageid;
        }

        /// <summary>
        /// 获取提交的查询枚举类型
        /// </summary>
        private void GetSearchType()
        {
            switch (type)
            {
                case "":
                case "topic": searchType = SearchType.TopicTitle; break;
                case "author": searchType = SearchType.ByPoster; break;
                case "post": searchType = SearchType.PostContent; break;
                case "spacepost": searchType = SearchType.SpacePostTitle; break;
                case "album": searchType = SearchType.AlbumTitle; break;
                case "digest": searchType = SearchType.DigestTopic; break;
                default: searchType = SearchType.Error; break;
            }
        }

        public string LightKeyWord(string str, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return str;
            return str.Replace(keyword, "<font color=\"#ff0000\">" + keyword + "</font>");
        }

        private int CheckSearchInfo(string searchforumid)
        {
            //如果posterid中的值是current，则代表当前登录用户，否则搜索指定的posterid
            int posterid = DNTRequest.GetString("posterid").ToLower().Trim() == "current" ? userid : DNTRequest.GetInt("posterid", -1);

            if (Utils.StrIsNullOrEmpty(keyword) && Utils.StrIsNullOrEmpty(poster) && Utils.StrIsNullOrEmpty(DNTRequest.GetString("posterid")))
            {
                AddErrLine("关键字和用户名不能同时为空");
                return posterid;
            }

            if (posterid > 0 && Users.GetShortUserInfo(posterid) == null)
            {
                AddErrLine("指定的用户ID不存在");
                return posterid;
            }
            else if (!Utils.StrIsNullOrEmpty(poster))
            {
                posterid = Users.GetUserId(poster);
                if (posterid == -1)
                {
                    AddErrLine("搜索用户名不存在");
                    return posterid;
                }
            }
            if (!Utils.StrIsNullOrEmpty(searchforumid))
            {
                foreach (string forumId in Utils.SplitString(searchforumid, ","))
                {
                    if (!Utils.IsNumeric(forumId))
                    {
                        AddErrLine("非法的搜索版块ID");
                        return posterid;
                    }
                }
            }
            return posterid;
        }
    }
}
