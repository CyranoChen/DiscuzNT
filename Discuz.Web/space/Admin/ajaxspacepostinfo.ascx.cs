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
    ///	ajax ��ȡ������Ϣ
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
            //��ȡ��ѯ��Ϣ
            posterlist = DNTRequest.GetString("poster");
            keylist = DNTRequest.GetString("title");
            startdate = DNTRequest.GetString("postdatetimeStart:postdatetimeStart");
            enddate = DNTRequest.GetString("postdatetimeEnd:postdatetimeEnd");
            currentpage = DNTRequest.GetInt("currentpage", 1);
            //��ȡ��ǰҳ��
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                pagesize = DNTRequest.GetInt("postnumber", 0);
            }
            //��ȡ��ҳ��
            int recordcount = DbProvider.GetInstance().GetSpacePostCountByCondition(posterlist, keylist, startdate, enddate);
            dt = DbProvider.GetInstance().GetSpacePostByCondition(posterlist, keylist, startdate, enddate, 10, currentpage);
            pagelink = AjaxPagination(recordcount, 10, currentpage);
        
        }

        //// <summary>
        /// ��ҳ����
        /// </summary>
        /// <param name="recordcount">�ܼ�¼��</param>
        /// <param name="pagesize">ÿҳ��¼��</param>
        /// <param name="currentpage">��ǰҳ��</param>
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