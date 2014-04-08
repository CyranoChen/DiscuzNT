using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Space.Manage;
using Discuz.Space.Data;


namespace Discuz.Space.Admin
{

    /// <summary>
    ///	ajax 读取帖子信息
    /// </summary>
    public class AjaxSpacePostInfo : DiscuzSpaceUCBase
    {
        public DataTable dt;
        public string pagelink;
        public int currentpage = 0;
        string posterlist;
        string keylist;
        string startdate;
        string enddate;

        public AjaxSpacePostInfo()
        {
            //获取查询信息
            posterlist = DNTRequest.GetString("poster");
            keylist = DNTRequest.GetString("title");
            startdate = DNTRequest.GetString("postdatetimeStart:postdatetimeStart");
            enddate = DNTRequest.GetString("postdatetimeEnd:postdatetimeEnd");
            currentpage = DNTRequest.GetInt("currentpage", 1);
            //获取当前页数
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                pagesize = DNTRequest.GetInt("postnumber", 0);
            }
            //获取总页数
            int recordcount = DbProvider.GetInstance().GetSpacePostCountByCondition(posterlist, keylist, startdate, enddate);
            dt = DbProvider.GetInstance().GetSpacePostByCondition(posterlist, keylist, startdate, enddate, 10, currentpage);
            pagelink = AjaxPagination(recordcount, 10, currentpage);
        
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
                return base.AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxspacepostinfo.ascx", "poster=" + posterlist + "&title=" + keylist + "&postdatetimeStart:postdatetimeStart=" + startdate + "&postdatetimeEnd:postdatetimeEnd=" + enddate + "&postnumber=" + DNTRequest.GetInt("postnumber", 0), "postlistgrid");
            }
            else
            {
                return base.AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxspacepostinfo.ascx", "poster=" + posterlist + "&title=" + keylist + "&postdatetimeStart:postdatetimeStart=" + startdate + "&postdatetimeEnd:postdatetimeEnd=" + enddate, "postlistgrid");
            }
        }
    }
}