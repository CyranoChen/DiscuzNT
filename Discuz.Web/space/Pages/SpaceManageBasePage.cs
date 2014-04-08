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
    /// spacemanagepage 的摘要说明。
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

            //已登录
            ShortUserInfo _user = Users.GetShortUserInfo(userid);
            if (_user.Spaceid == 0) //用户还未开通个人空间
            {
                Context.Response.Write("<script type='text/javascript'>alert('您还未开通" + config.Spacename + "！');window.location='../../';</script>");
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
        /// 得到当前页面的meta属性字符串
        /// </summary>
        /// <returns></returns>
        public string GetPageHeadMetaContent()
        {
            return "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n<meta name=\"keywords\" content=\"Discuz!NT," + config.Spacename + ",Comsenz \" />\r\n<meta name=\"description\" content=\"最高品质的个人空间\" />\r\n";
        }

        /// <summary>
        /// 得到当前页面linkcss属性字符串
        /// </summary>
        /// <returns></returns>
        public string GetPageHeadLinkCss()
        {
            return "<link rev=\"stylesheet\" media=\"all\" href=\"styles/space.css\" type=\"text/css\" rel=\"stylesheet\">\r\n";
        }


        /// <summary>
        /// 得到当前页面linkcss属性字符串
        /// </summary>
        /// <returns></returns>
        public string GetPageHeadJs()
        {
            return "<script type=\"text/javascript\" src=\"js/common.js\"></script>\r\n <script type=\"text/javascript\" src=\"js/divcover.js\"></script>\r\n";
        }

        /// <summary>
        /// 得到当前页面header属性字符串
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
                "		<div id=\"Layer4\" style=\"height:26px;background:#D3DBDE;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;\">操作提示</div>\r\n" +
                "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\"><br /><table><tr><td valign=top><img border=\"0\" src=\"images/ajax_loading.gif\"  /></td><td valign='middle' style=\"font-size: 14px;\" >正在执行当前操作, 请稍等...<br /></td></tr></table><br /></div>\r\n" +
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
                        "document.getElementById('Layer5').innerHTML ='<BR>操作成功执行<BR>';  \r\n" +
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
        /// 检查cookie是否有效
        /// </summary>
        /// <returns></returns>
        public bool CheckCookie()
        {
            return true;
        }


        /// <summary>
        /// 论坛提示信息
        /// </summary>
        /// <returns></returns>
        protected string GetShowMessage()
        {
            string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>您没有权限运行当前程序!</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
            message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><body><br><br><div style=\"width:100%\" align=\"center\">";
            message += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
            message += "您没有权限运行当前程序,请您登陆后台进行操作!</div></div></body></html>";
            return message;
        }


        public void LoadRegisterStartupScript(string key, string scriptstr)
        {
            string message = "<br />程序执行中... <br /> 当前操作可能要运行一段时间.<br />您可在此期间进行其它操作<br /><br />";

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
                // 发生 PostDataChanged 事件在所有已变动的控件上:
                foreach (IPostBackDataHandler control in modifiedControls)
                    control.RaisePostDataChangedEvent();
            }

            base.OnLoad(e);

        }

        #region 把VIEWSTATE以组件的形式存入容器

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