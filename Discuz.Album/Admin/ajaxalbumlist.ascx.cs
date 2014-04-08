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
        //ҳ���С
        public int pagesize = 16;


        public AjaxAlbumList()
        {
            //��ȡ��ѯ��Ϣ
            description = DNTRequest.GetString("albumdescription");
            postusername = DNTRequest.GetString("albumusername");
            title = DNTRequest.GetString("albumtitle");
            startdate = DNTRequest.GetString("albumdatetimeStart");
            enddate = DNTRequest.GetString("albumdatetimeEnd");
            currentpage = DNTRequest.GetInt("currentpage", 1);
            //��ȡ��ҳ��
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                pagesize = DNTRequest.GetInt("postnumber", 0);
            }
            if (DNTRequest.GetUrl().ToLower().IndexOf("album_manage.aspx") != -1 || DNTRequest.GetUrl().ToLower().IndexOf("album_config.aspx") != -1)
                isShowPrivateAlbum = true;
            //��ȡ��ҳ��
            int recordcount = DbProvider.GetInstance().GetAlbumListCountByCondition(postusername, title, description, startdate, enddate,isShowPrivateAlbum);
            dt = DbProvider.GetInstance().GetAlbumListByCondition(postusername, title, description, startdate, enddate, 12, currentpage,isShowPrivateAlbum);
            pagelink = AjaxPagination(recordcount, 12, currentpage);
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
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxalbumlist.ascx", "albumusername=" + postusername + "&albumtitle=" + title + "&albumdescription=" + description + "&albumdatetimeStart=" + startdate + "&albumdatetimeEnd=" + enddate + "&postnumber=" + DNTRequest.GetInt("postnumber", 0), "albumslist");
            }
            else
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxalbumlist.ascx", "albumusername=" + postusername + "&albumtitle=" + title + "&albumdescription=" + description + "&albumdatetimeStart=" + startdate + "&albumdatetimeEnd=" + enddate, "albumslist");
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
        
        /// <summary>
        /// ����������
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