using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{

    /// <summary>
    ///	ajax ��ȡ������Ϣ
    /// </summary>
    public class ajaxtopicinfo : System.Web.UI.UserControl
    {
        public DataTable dt;
        public string pagelink;
        public int currentpage = 0;
        public string postname;
        int tablelist;
        int forumid;
        string posterlist;
        string keylist;
        string startdate;
        string enddate;
        //ҳ���С
        public int pagesize = 4;
        public int fid = 0;
        
        public ajaxtopicinfo()
        {
            //��ȡ��ѯ��Ϣ
            if (Context.Request.Url.ToString().Contains("forumhottopic") || DNTRequest.GetString("pagename") == "forumhottopic")
            {
                //��ȡ��ǰҳ��
                if (DNTRequest.GetInt("postnumber", 0) > 0)
                {
                    pagesize = DNTRequest.GetInt("postnumber", 0);
                }
                currentpage = DNTRequest.GetInt("currentpage", 1);
                //��ȡ��ҳ��
                int forumid = 0;
                string showtype = "replies";
                int timebetween = 7;
                
                if (DNTRequest.GetString("search") != "")
                {
                    forumid = DNTRequest.GetInt("forumid", 0);
                    showtype = DNTRequest.GetString("showtype") == "" ? "replies" : DNTRequest.GetString("showtype");
                    timebetween = DNTRequest.GetInt("timebetween", 7);
                }

                int recordcount = Topics.GetHotTopicsCount(forumid, timebetween);
                dt = Topics.GetHotTopicsList(20, currentpage, forumid, showtype, timebetween);
                pagelink = AjaxHotTopicPagination(recordcount, 20, currentpage, string.Format("&forumid={0}&showtype={1}&timebetween={2}",forumid,showtype,timebetween));
            }
            else
            {
                forumid = DNTRequest.GetInt("_ctl0", 0);
                if (forumid == 0)
                    forumid = DNTRequest.GetInt("fid", 0);
                posterlist = DNTRequest.GetString("poster");
                keylist = DNTRequest.GetString("title");
                startdate = DNTRequest.GetString("postdatetimeStart:postdatetimeStart");
                enddate = DNTRequest.GetString("postdatetimeEnd:postdatetimeEnd");
                currentpage = DNTRequest.GetInt("currentpage", 1);
                tablelist = DNTRequest.GetInt("tablelist", Posts.GetMaxPostTableId());
                postname = BaseConfigs.GetTablePrefix + "posts" + tablelist;
                //��ȡ��ǰҳ��
                if (DNTRequest.GetInt("postnumber", 0) > 0)
                {
                    pagesize = DNTRequest.GetInt("postnumber", 0);
                }
                //��ȡ��ҳ��
                int recordcount = Topics.GetTopicListCount(postname, forumid, posterlist, keylist, startdate, enddate);
                dt = Topics.GetTopicList(postname, forumid, posterlist, keylist, startdate, enddate, 10, currentpage);
                pagelink = AjaxPagination(recordcount, 10, currentpage);
            }
            
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
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "tablelist=" + tablelist + "&_ctl0=" + forumid + "&poster=" + posterlist + "&title=" + keylist + "&postdatetimeStart:postdatetimeStart=" + startdate + "&postdatetimeEnd:postdatetimeEnd=" + enddate + "&postnumber=" + DNTRequest.GetInt("postnumber", 0), "topiclistgrid");
            }
            else
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "tablelist=" + tablelist + "&_ctl0=" + forumid + "&poster=" + posterlist + "&title=" + keylist + "&postdatetimeStart:postdatetimeStart=" + startdate + "&postdatetimeEnd:postdatetimeEnd=" + enddate, "topiclistgrid");
            }
        }

        public string AjaxHotTopicPagination(int recordcount, int pagesize, int currentpage,string condition)
        {
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "pagename=forumhottopic&postnumber=" + DNTRequest.GetInt("postnumber", 0)  +condition, "topiclistgrid");
            }
            else
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "pagename=forumhottopic" + condition, "topiclistgrid");
            }
        }

        /// <summary>
        /// ��ҳ����
        /// </summary>
        /// <param name="recordcount">�ܼ�¼��</param>
        /// <param name="pagesize">ÿҳ��¼��</param>
        /// <param name="currentpage">��ǰҳ��</param>
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

            //������ҳ��
            if (pagesize != 0)
            {
                allcurrentpage = (recordcount / pagesize);
                allcurrentpage = ((recordcount % pagesize) != 0 ? allcurrentpage + 1 : allcurrentpage);
                allcurrentpage = (allcurrentpage == 0 ? 1 : allcurrentpage);
            }
            next = currentpage + 1;
            pre = currentpage - 1;

            //�м�ҳ��ʼ���
            startcount = (currentpage + 5) > allcurrentpage ? allcurrentpage - 9 : currentpage - 4;

            //�м�ҳ��ֹ���
            endcount = currentpage < 5 ? 10 : currentpage + 5;

            //Ϊ�˱��������ʱ������������������С��1�ʹ����1��ʼ
            if (startcount < 1)
            {
                startcount = 1;
            }

            //ҳ��+5�Ŀ����Ծͻ�������������Ŵ�����ҳ�룬��ô��Ҫ���������ҳ����֮��
            if (allcurrentpage < endcount)
            {
                endcount = allcurrentpage;
            }

            if (startcount > 1)
            {
                currentpagestr += currentpage > 1 ? "&nbsp;&nbsp;<a href=\"###\"  onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + pre + "');\" title=\"��һҳ\">��һҳ</a>" : "";
            }

            //��ҳ��������1ʱ, ����ʾҳ��
            if (endcount > 1)
            {
                //�м�ҳ����, �������ʱ�临�Ӷȣ���С�ռ临�Ӷ�
                for (int i = startcount; i <= endcount; i++)
                {
                    currentpagestr += currentpage == i ? "&nbsp;" + i + "" : "&nbsp;<a href=\"###\"  onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + i + "');\">" + i + "</a>";
                }
            }

            if (endcount < allcurrentpage)
            {
                currentpagestr += currentpage != allcurrentpage ? "&nbsp;&nbsp;<a href=\"###\" onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + next + "');\" title=\"��һҳ\">��һҳ</a>&nbsp;&nbsp;" : "";
            }

            if (endcount > 1)
            {
                currentpagestr += "&nbsp; &nbsp; &nbsp; &nbsp;";
            }

            currentpagestr += "�� " + allcurrentpage + " ҳ, ��ǰ�� " + currentpage + " ҳ, �� " + recordcount + " ����¼";

            return currentpagestr;

        }
    }
}