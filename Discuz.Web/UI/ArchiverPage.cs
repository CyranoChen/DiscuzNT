using System;
using System.Web;
using System.Web.UI;
using System.Text;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.UI
{
	/// <summary>
	/// Discuzҳ�������
	/// </summary>
	public class ArchiverPage : Page
	{
		/// <summary>
		/// ��ǰ�û����û�����Ϣ
		/// </summary>
		protected internal UserGroupInfo usergroupinfo;

        protected internal int userid;
        protected internal int useradminid;
        protected internal GeneralConfigInfo config = GeneralConfigs.GetConfig();
	
		/// <summary>
		/// ���캯��
		/// </summary>
		public ArchiverPage()
		{
            if (config.Archiverstatus == 2 && DNTRequest.IsSearchEnginesGet())//���ã������û�������������ʱ�Զ�ת��̬ҳ��
                HttpContext.Current.Response.Redirect(OrganizeURL(HttpContext.Current.Request.Url));

            if (config.Archiverstatus == 3 && DNTRequest.IsBrowserGet())//���ã������û�ʹ�����������ʱ�Զ�ת��̬ҳ��
                HttpContext.Current.Response.Redirect(OrganizeURL(HttpContext.Current.Request.Url));

            if (OnlineUsers.GetOnlineAllUserCount() >= config.Maxonlines)
                ShowError("��Ǹ,Ŀǰ��������̫��,����ʱ�޷�������̳.", 0);
			
			if (config.Nocacheheaders == 1)
			{
				HttpContext.Current.Response.Buffer = true;
				HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
				HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
				HttpContext.Current.Response.Expires = 0;
				HttpContext.Current.Response.CacheControl = "no-cache";
				HttpContext.Current.Response.Cache.SetNoStore();
			}

			OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);

			userid = oluserinfo.Userid;
			useradminid = oluserinfo.Adminid;
			// �����̳�ر��ҵ�ǰ�û�����ҳ�治�ǵ�¼ҳ�����û��ǹ���Ա, ����ת����̳�ر���Ϣҳ
			if (config.Closed == 1 && oluserinfo.Adminid != 1)
				ShowError("", 1);

			usergroupinfo = UserGroups.GetUserGroupInfo(oluserinfo.Groupid);
		
			// ��������������̳��ת��tools/ban.htm
			if (usergroupinfo.Allowvisit != 1)
				ShowError("��Ǹ, �����ڵ��û��鲻���������̳", 2);

            // ���IP�����б�������������ж�
            if (config.Ipaccess.Trim() != "" && !Utils.InIPArray(DNTRequest.GetIP(), Utils.SplitString(config.Ipaccess, "\n")))
            {
                ShowError("��Ǹ, ϵͳ������IP�����б�����, ���޷����ʱ���̳", 0);
                return;
            }			
			// ���IP�����б�������������ж�
            if (config.Ipdenyaccess.Trim() != "" && Utils.InIPArray(DNTRequest.GetIP(), Utils.SplitString(config.Ipdenyaccess, "\n")))
            {
                ShowError("����������Υ������̳����ع涨, �ѱ���ֹ����.", 2);
                return;
            }
			//�������ǰ�û�����ҳ�治�ǵ�¼ҳ�沢�ҵ�ǰ�û��ǹ���Ա������̳�趨��ʱ��Σ���ʱ�������е�һ��ʱ����ڣ�����ת����̳��¼ҳ��
            if (oluserinfo.Adminid != 1 && DNTRequest.GetPageName() != "login.aspx" && Scoresets.BetweenTime(config.Visitbanperiods))
            {
                ShowError("�ڴ�ʱ����ڲ�������ʱ���̳", 2);
                return;
            }

			HttpContext.Current.Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    ");
		
			if (config.Seokeywords != "")
				HttpContext.Current.Response.Write("<meta name=\"keywords\" content=\"" + config.Seokeywords + "\" />\r\n");

            if (config.Seodescription != "")
				HttpContext.Current.Response.Write("<meta name=\"description\" content=\"" + config.Seodescription + "\" />\r\n");

            HttpContext.Current.Response.Write(config.Seohead.Trim());
			HttpContext.Current.Response.Write("\r\n<link href=\"dntarchiver.css\" rel=\"stylesheet\" type=\"text/css\" />");

			if (config.Archiverstatus == 0)
			{
				ShowError("ϵͳ��ֹʹ��Archiver",3);
				HttpContext.Current.Response.End();
				return;
			}
		}

        private string OrganizeURL(Uri requestURL)
        {
            string[] urlArr = requestURL.AbsolutePath.Replace("archiver/", "").Split(new char[] { '/' });
            string pageName = urlArr[urlArr.Length - 1].ToLower();
            
            switch (pageName)
            {
                case "showforum.aspx":
                    if (config.Aspxrewrite == 1)
                    {
                        StringBuilder sbUrl = new StringBuilder();
                        sbUrl.Append("../" + pageName.Substring(0, pageName.IndexOf('.')));
                        if (DNTRequest.GetQueryInt("forumid") != 0)
                        {
                            sbUrl.Append("-" + DNTRequest.GetQueryString("forumid"));
                            if (DNTRequest.GetQueryInt("page") != 0)
                                sbUrl.Append("-" + DNTRequest.GetQueryString("page"));
                        }
                        return sbUrl.Append(GeneralConfigs.GetConfig().Extname).ToString();
                    }
                    else
                    {
                        return requestURL.PathAndQuery.Replace("archiver/", "../");
                    }
                case "showtopic.aspx":
                    if (config.Aspxrewrite == 1)
                    {
                        StringBuilder sbUrl = new StringBuilder();
                        sbUrl.Append("../" + pageName.Substring(0, pageName.IndexOf('.')));
                        if (DNTRequest.GetQueryInt("topicid") != 0)
                        {
                            sbUrl.Append("-" + DNTRequest.GetQueryInt("topicid"));
                            if (DNTRequest.GetQueryInt("page") != 0)
                                sbUrl.Append("-" + DNTRequest.GetQueryString("page"));
                        }
                        return sbUrl.Append(GeneralConfigs.GetConfig().Extname).ToString();
                    }
                    else
                    {
                        return requestURL.PathAndQuery.Replace("archiver/", "../");
                    }
                default:
                    return "../index.aspx";
            }
        }

	    public void ShowTitle(string title)
		{
            HttpContext.Current.Response.Write(string.Format("\r\n<title>{0}{1}{2} - Powered by Discuz!NT Archiver</title>\r\n", Utils.HtmlEncode(title), Utils.HtmlEncode(config.Seotitle.Trim()), (config.Seotitle.Trim() == "" ? "" : " ")));
		}

		public void ShowBody()
		{
			HttpContext.Current.Response.Write("\r\n</head>\r\n\r\n<body>\r\n");
		}
		
		public void ShowMsg(string msg)
		{
			ShowBody();
			HttpContext.Current.Response.Write("<div class=\"msg\">" + Utils.HtmlEncode(msg) + "</div>");
			ShowFooter();
		}

		public void ShowFooter()
		{
			HttpContext.Current.Response.Write(string.Format("<div class=\"copyright\" align=\"center\">Powered by <a href=\"http://nt.discuz.net\">Discuz!NT</a> Archiver {0} 2001-{1} <a href=\"http://www.comsenz.com\" target=\"_blank\" style=\"color:#000000\">Comsenz Inc.</a></div>\r\n</body>\r\n</html>", ForumUtils.GetAssemblyVersion(), DateTime.Now.Year.ToString()));
		}

		public void ShowError(string hint, byte mode)
		{
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head><title>");
			string title;
			string body;
			switch (mode)
			{
				case 1:
					title = "��̳�ѹر�";
					body = config.Closedreason;
					break;
				case 2:
					title = "��ֹ����";
					body = hint;
					break;
				default:
					title = "��ʾ";
					body = hint;
					break;
			}
			System.Web.HttpContext.Current.Response.Write(title);
			System.Web.HttpContext.Current.Response.Write(" - ");
			System.Web.HttpContext.Current.Response.Write(config.Forumtitle);
			System.Web.HttpContext.Current.Response.Write(" - Powered by Discuz!NT</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
			System.Web.HttpContext.Current.Response.Write("<style type=\"text/css\"><!-- body { margin: 20px; font-family: Tahoma, Verdana; font-size: 14px; color: #333333; background-color: #FFFFFF; }a {color: #1F4881;text-decoration: none;}--></style></head><body><div style=\"border: #cccccc solid 1px; padding: 20px; width: 500px; margin:auto\" align=\"center\">");
			System.Web.HttpContext.Current.Response.Write(body);
			System.Web.HttpContext.Current.Response.Write("</div></body></html>");
			System.Web.HttpContext.Current.Response.End();
		}
	}
}
