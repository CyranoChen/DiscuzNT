using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Album.Data;

namespace Discuz.Album.Admin
{

    public class AjaxAlbumList : System.Web.UI.UserControl
    {
        public DataTable dt;
        public string pagelink;
        public int currentpage = 0;
        string title;
        string description;
        string postusername;
        string startdate;
        string enddate;
        public bool isShowPrivateAlbum = false;
        //页面大小
        public int pagesize = 16;


        public AjaxAlbumList()
        {
            //获取查询信息
            description = DNTRequest.GetString("albumdescription");
            postusername = DNTRequest.GetString("albumusername");
            title = DNTRequest.GetString("albumtitle");
            startdate = DNTRequest.GetString("albumdatetimeStart");
            enddate = DNTRequest.GetString("albumdatetimeEnd");
            currentpage = DNTRequest.GetInt("currentpage", 1);
            //获取分页数
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                pagesize = DNTRequest.GetInt("postnumber", 0);
            }
            if (DNTRequest.GetUrl().ToLower().IndexOf("album_manage.aspx") != -1 || DNTRequest.GetUrl().ToLower().IndexOf("album_config.aspx") != -1)
                isShowPrivateAlbum = true;
            //获取总页数
            int recordcount = DbProvider.GetInstance().GetAlbumListCountByCondition(postusername, title, description, startdate, enddate,isShowPrivateAlbum);
            dt = DbProvider.GetInstance().GetAlbumListByCondition(postusername, title, description, startdate, enddate, 12, currentpage,isShowPrivateAlbum);
            pagelink = AjaxPagination(recordcount, 12, currentpage);
        }


        //// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="recordcount">总记录数</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="currentpage">当前页数</param>
        public string AjaxPagination(int recordcount, int pagesize, int currentpage)
        {
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxalbumlist.ascx", "albumusername=" + postusername + "&albumtitle=" + title + "&albumdescription=" + description + "&albumdatetimeStart=" + startdate + "&albumdatetimeEnd=" + enddate + "&postnumber=" + DNTRequest.GetInt("postnumber", 0), "albumslist");
            }
            else
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxalbumlist.ascx", "albumusername=" + postusername + "&albumtitle=" + title + "&albumdescription=" + description + "&albumdatetimeStart=" + startdate + "&albumdatetimeEnd=" + enddate, "albumslist");
            }
        }

        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="recordcount">总记录数</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="currentpage">当前页数</param>
        public string AjaxPagination(int recordcount, int pagesize, int currentpage, string usercontrolname, string paramstr, string divname)
        {
            int allcurrentpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string currentpagestr = "<BR />";

            if (currentpage < 1)
            {
                currentpage = 1;
            }

            //计算总页数
            if (pagesize != 0)
            {
                allcurrentpage = (recordcount / pagesize);
                allcurrentpage = ((recordcount % pagesize) != 0 ? allcurrentpage + 1 : allcurrentpage);
                allcurrentpage = (allcurrentpage == 0 ? 1 : allcurrentpage);
            }
            next = currentpage + 1;
            pre = currentpage - 1;

            //中间页起始序号
            startcount = (currentpage + 5) > allcurrentpage ? allcurrentpage - 9 : currentpage - 4;

            //中间页终止序号
            endcount = currentpage < 5 ? 10 : currentpage + 5;

            //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (startcount < 1)
            {
                startcount = 1;
            }

            //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            if (allcurrentpage < endcount)
            {
                endcount = allcurrentpage;
            }

            if (startcount > 1)
            {
                currentpagestr += currentpage > 1 ? "&nbsp;&nbsp;<a href=\"###\"  onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + pre + "');\" title=\"上一页\">上一页</a>" : "";
            }

            //当页码数大于1时, 则显示页码
            if (endcount > 1)
            {
                //中间页处理, 这个增加时间复杂度，减小空间复杂度
                for (int i = startcount; i <= endcount; i++)
                {
                    currentpagestr += currentpage == i ? "&nbsp;" + i + "" : "&nbsp;<a href=\"###\"  onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + i + "');\">" + i + "</a>";
                }
            }

            if (endcount < allcurrentpage)
            {
                currentpagestr += currentpage != allcurrentpage ? "&nbsp;&nbsp;<a href=\"###\" onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + next + "');\" title=\"下一页\">下一页</a>&nbsp;&nbsp;" : "";
            }

            if (endcount > 1)
            {
                currentpagestr += "&nbsp; &nbsp; &nbsp; &nbsp;";
            }

            currentpagestr += "共 " + allcurrentpage + " 页, 当前第 " + currentpage + " 页, 共 " + recordcount + " 条记录";

            return currentpagestr;

        }
        
        /// <summary>
        /// 返回相册封面
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetLogo(string filename)
        {
            if (filename != "")
                return BaseConfigs.GetForumPath + filename;
            else
                return BaseConfigs.GetForumPath + "space/images/nopic.gif";
        }
    }
}