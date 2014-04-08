using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Pages;
using Discuz.Entity;

namespace Discuz.Space.Manage
{
    /// <summary>
    /// spacemanagepage ��ժҪ˵����
    /// </summary>
    public class SpaceManageBasePage : SpaceBasePage
    {
        protected internal string userkey;

        public SpaceManageBasePage()
        {
            if (userid == -1)
            {
                Context.Response.Redirect("../../login.aspx");
                return;
            }

            //�ѵ�¼
            ShortUserInfo _user = Users.GetShortUserInfo(userid);
            if (_user.Spaceid == 0) //�û���δ��ͨ���˿ռ�
            {
                Context.Response.Write("<script type='text/javascript'>alert('����δ��ͨ" + config.Spacename + "��');window.location='../../';</script>");
                Context.Response.End();
                return;
            }

            username = _user.Username;
            spaceid = _user.Spaceid;
            if (_user.Password.Length > 16)
                userkey = _user.Password.Substring(4, 8).Trim();
            if (!Page.IsPostBack)
                this.RegisterAdminPageClientScriptBlock();
        }

        /// <summary>
        /// �õ���ǰҳ���meta�����ַ���
        /// </summary>
        /// <returns></returns>
        public string GetPageHeadMetaContent()
        {
            return "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n<meta name=\"keywords\" content=\"Discuz!NT," + config.Spacename + ",Comsenz \" />\r\n<meta name=\"description\" content=\"���Ʒ�ʵĸ��˿ռ�\" />\r\n";
        }

        /// <summary>
        /// �õ���ǰҳ��linkcss�����ַ���
        /// </summary>
        /// <returns></returns>
        public string GetPageHeadLinkCss()
        {
            return "<link rev=\"stylesheet\" media=\"all\" href=\"styles/space.css\" type=\"text/css\" rel=\"stylesheet\">\r\n";
        }


        /// <summary>
        /// �õ���ǰҳ��linkcss�����ַ���
        /// </summary>
        /// <returns></returns>
        public string GetPageHeadJs()
        {
            return "<script type=\"text/javascript\" src=\"js/common.js\"></script>\r\n <script type=\"text/javascript\" src=\"js/divcover.js\"></script>\r\n";
        }

        /// <summary>
        /// �õ���ǰҳ��header�����ַ���
        /// </summary>
        /// <returns></returns>
        public string GetPageHeadContent()
        {
            return GetPageHeadMetaContent() + GetPageHeadLinkCss() + GetPageHeadJs();
        }

        public void RegisterAdminPageClientScriptBlock()
        {
            string script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                "		<div id=\"Layer4\" style=\"height:26px;background:#D3DBDE;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;\">������ʾ</div>\r\n" +
                "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\"><br /><table><tr><td valign=top><img border=\"0\" src=\"images/ajax_loading.gif\"  /></td><td valign='middle' style=\"font-size: 14px;\" >����ִ�е�ǰ����, ���Ե�...<br /></td></tr></table><br /></div>\r\n" +
                "	</div>\r\n" +
                "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #E8E8E8;\"></div>\r\n" +
                "</div>\r\n" +
                "<script> \r\n" +
                "document.getElementById('success').style.display = \"none\"; \r\n" +
                "</script> \r\n" +
                "<script type=\"text/javascript\" src=\"js/divcover.js\"></script>\r\n";

			this.ClientScript.RegisterStartupScript(typeof(Page), "Page", script);
        }


        public new void RegisterStartupScript(string key, string scriptstr)
        {
            if ((key == "PAGETemplate") || (key == "PAGE"))
            {
                string script = "";

                if (key == "PAGE")
                {
                    script = "<script> \r\n" +
                        "var bar=0;\r\n" +
                        "document.getElementById('success').style.display = \"block\";  \r\n" +
                        "document.getElementById('Layer5').innerHTML ='<BR>�����ɹ�ִ��<BR>';  \r\n" +
                        "count() ; \r\n" +
                        "function count(){ \r\n" +
                        "bar=bar+4; \r\n" +
                        "if (bar<99) \r\n" +
                        "{setTimeout(\"count()\",100);} \r\n" +
                        "else { \r\n" +
                        "document.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" +
                        scriptstr + "} \r\n" +
                        "} \r\n" +
                        "</script> \r\n" +
                        "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
                }

                if (key == "PAGETemplate")
                {
                    script = "<script> \r\n" +
                        "var bar=0;\r\n success.style.display = \"block\";  \r\n" +
                        "document.getElementById('Layer5').innerHTML = '<BR>" + scriptstr + "<BR>';  \r\n" +
                        "count() ; \r\n" +
                        "function count(){ \r\n" +
                        "bar=bar+4; \r\n" +
                        "if (bar<99) \r\n" +
                        "{setTimeout(\"count()\",100);} \r\n" +
                        "else { \r\n" +
                        "document.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" +
                        "}} \r\n" +
                        "</script> \r\n" +
                        "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
                }
                base.ClientScript.RegisterStartupScript(typeof(Page), key, script);
            }
            else
				base.ClientScript.RegisterStartupScript(typeof(Page), key, scriptstr);
        }


        public void CallBaseRegisterStartupScript(string key, string scriptstr)
        {
			base.ClientScript.RegisterStartupScript(typeof(Page), key, scriptstr);
        }


        /// <summary>
        /// ���cookie�Ƿ���Ч
        /// </summary>
        /// <returns></returns>
        public bool CheckCookie()
        {
            return true;
        }


        /// <summary>
        /// ��̳��ʾ��Ϣ
        /// </summary>
        /// <returns></returns>
        protected string GetShowMessage()
        {
            string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>��û��Ȩ�����е�ǰ����!</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
            message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><body><br><br><div style=\"width:100%\" align=\"center\">";
            message += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"��ʾ:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
            message += "��û��Ȩ�����е�ǰ����,������½��̨���в���!</div></div></body></html>";
            return message;
        }


        public void LoadRegisterStartupScript(string key, string scriptstr)
        {
            string message = "<br />����ִ����... <br /> ��ǰ��������Ҫ����һ��ʱ��.<br />�����ڴ��ڼ������������<br /><br />";

            string script = "<script> \r\n" +
                "var bar=0;\r\n success.style.display = \"block\";  \r\n" +
                "document.getElementById('Layer5').innerHTML ='" + message + "';  \r\n" +
                "count() ; \r\n" +
                "function count(){ \r\n" +
                "bar=bar+2; \r\n" +
                "if (bar<99) \r\n" +
                "{setTimeout(\"count()\",100);} \r\n" +
                "else { \r\n" +
                "	document.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" +
                scriptstr + "} \r\n" +
                "} \r\n" +
                "</script> \r\n" +
                "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";

            CallBaseRegisterStartupScript(key, script);

        }

        private bool saveState = false;

        private bool IsRestore
        {
            get
            {
                if (DNTRequest.GetString("IsRestore") != null && DNTRequest.GetString("IsRestore") == "1" && DNTRequest.GetFormString("__VIEWSTATE") == null)
                    return true;
                else
                    return false;
            }
        }


        public bool SavePageState
        {
            get { return this.saveState; }
            set { this.saveState = value; }
        }

        private NameValueCollection postData = null;

        private NameValueCollection PostData
        {
            get
            {
                if (this.IsRestore)
                    return this.postData;
                return Request.Form;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.IsRestore)
            {
                ArrayList modifiedControls = new ArrayList();
                foreach (string key in PostData.AllKeys)
                {
                    System.Web.UI.Control control = FindControl(key);
                    if (control is IPostBackDataHandler)
                        if (((IPostBackDataHandler)control).LoadPostData(key, PostData))
                            modifiedControls.Add(control);
                }
                // ���� PostDataChanged �¼��������ѱ䶯�Ŀؼ���:
                foreach (IPostBackDataHandler control in modifiedControls)
                    control.RaisePostDataChangedEvent();
            }

            base.OnLoad(e);

        }

        #region ��VIEWSTATE���������ʽ��������

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.SavePageStateToPersistenceMedium(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            object o = new object();
            try
            {
                o = base.LoadPageStateFromPersistenceMedium();
            }
            catch
            {
                o = null;
            }
            return o;
        }

        #endregion
    }
}