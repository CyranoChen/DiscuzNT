<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.editpost" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/6/2 16:12:48.
		本页面代码由Discuz!NT模板引擎生成于 2011/6/2 16:12:48. 
	*/

	base.OnInit(e);

	templateBuilder.Capacity = 220000;



	if (infloat!=1)
	{

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    ");
	if (pagetitle=="首页")
	{

	templateBuilder.Append("\r\n        <title>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by Discuz!NT</title>\r\n    ");
	}
	else
	{

	templateBuilder.Append("\r\n        <title>");
	templateBuilder.Append(pagetitle.ToString());
	templateBuilder.Append(" - ");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by Discuz!NT</title>\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n    ");
	templateBuilder.Append(meta.ToString());
	templateBuilder.Append("\r\n    <meta name=\"generator\" content=\"Discuz!NT 3.6.601\" />\r\n    <meta name=\"author\" content=\"Discuz!NT Team and Comsenz UI Team\" />\r\n    <meta name=\"copyright\" content=\"2001-2011 Comsenz Inc.\" />\r\n    <meta http-equiv=\"x-ua-compatible\" content=\"ie=7\" />\r\n    <link rel=\"icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n    <link rel=\"shortcut icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n    ");
	if (pagename!="website.aspx")
	{

	templateBuilder.Append("\r\n        <link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/dnt.css\" type=\"text/css\" media=\"all\" />\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n    <link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/float.css\" type=\"text/css\" />\r\n    ");
	if (isnarrowpage)
	{

	templateBuilder.Append("\r\n        <link type=\"text/css\" rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/widthauto.css\" id=\"css_widthauto\" />\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n    ");
	templateBuilder.Append(link.ToString());
	templateBuilder.Append("\r\n    <script type=\"text/javascript\">\r\n        var creditnotice='");
	templateBuilder.Append(Scoresets.GetValidScoreNameAndId().ToString().Trim());
	templateBuilder.Append("';	\r\n        var forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\n    </");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(config.Jqueryurl.ToString().Trim());
	templateBuilder.Append("\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\">jQuery.noConflict();</");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/common.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_report.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_utils.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\">\r\n	    var aspxrewrite = ");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append(";\r\n	    var IMGDIR = '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("';\r\n        var disallowfloat = '");
	templateBuilder.Append(config.Disallowfloatwin.ToString().Trim());
	templateBuilder.Append("';\r\n	    var rooturl=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("\";\r\n	    var imagemaxwidth='");
	templateBuilder.Append(Templates.GetTemplateWidth(templatepath).ToString().Trim());
	templateBuilder.Append("';\r\n	    var cssdir='");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("';\r\n    </");
	templateBuilder.Append("script>\r\n    ");
	templateBuilder.Append(script.ToString());
	templateBuilder.Append("\r\n</head>");

	templateBuilder.Append("\r\n<body onkeydown=\"if(event.keyCode==27) return false;\">\r\n<div id=\"append_parent\"></div><div id=\"ajaxwaitid\"></div>\r\n");
	if (headerad!="")
	{

	templateBuilder.Append("\r\n	<div id=\"ad_headerbanner\">");
	templateBuilder.Append(headerad.ToString());
	templateBuilder.Append("</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<div id=\"hd\">\r\n	<div class=\"wrap\">\r\n		<div class=\"head cl\">\r\n			<h2><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\" title=\"");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/logo.png\" alt=\"");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("\"/></a></h2>\r\n			");
	if (userid==-1)
	{


	if (pagename!="login.aspx"&&pagename!="register.aspx")
	{

	templateBuilder.Append("\r\n			<form onsubmit=\"if ($('ls_username').value == '' || $('ls_username').value == '用户名/Email') showWindow('login', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');return\" action=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?referer=");
	templateBuilder.Append(pagename.ToString());
	templateBuilder.Append("\" id=\"lsform\" autocomplete=\"off\" method=\"post\">\r\n				<div class=\"fastlg c1\">\r\n					<div class=\"y pns\">\r\n						<p>\r\n							<label for=\"ls_username\">帐号</label> <input type=\"text\" tabindex=\"901\" value=\"用户名/Email\" id=\"ls_username\" name=\"username\" class=\"txt\" onblur=\"if(this.value == '') this.value = '用户名/Email';\" onfocus=\"if(this.value == '用户名/Email') this.value = '';\"/><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onClick=\"showWindow('register', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx');hideWindow('login');\" style=\"margin-left: 7px;\" class=\"xg2\">注册</a>							\r\n						</p>\r\n						<p>\r\n							<label for=\"ls_password\">密码</label> <input type=\"password\" onfocus=\"lsShowmore();innerVcode();\" tabindex=\"902\" autocomplete=\"off\" id=\"ls_password\" name=\"password\"  class=\"txt\"/>\r\n							&nbsp;<input type=submit style=\"width:0px;filter:alpha(opacity=0);-moz-opacity:0;opacity:0;display:none;\"/><button class=\"pn\" type=\"submit\"><span>登录</span></button>\r\n						</p>\r\n					</div>\r\n				</div>\r\n                <div id=\"ls_more\" style=\"position:absolute;display:none;\">\r\n                <h3 class=\"cl\"><em class=\"y\"><a href=\"###\" class=\"flbc\" title=\"关闭\" onclick=\"closeIsMore();return false;\">关闭</a></em>安全选项</h3>\r\n                ");
	if (isLoginCode)
	{

	templateBuilder.Append("\r\n                    <div id=\"vcode_header\"></div>\r\n                    <script type=\"text/javascript\" reload=\"1\">\r\n                        if (typeof vcodeimgid == 'undefined') {\r\n                            var vcodeimgid = 1;\r\n                        }\r\n                        else\r\n                            vcodeimgid++;\r\n                        var secclick = new Array();\r\n                        var seccodefocus = 0;\r\n                        var optionVcode = function (id, type) {\r\n                            id = vcodeimgid;\r\n                            if ($('vcode')) {\r\n                                $('vcode').parentNode.removeChild($('vcode'));\r\n                            }\r\n\r\n                            if (!secclick['vcodetext_header' + id]) {\r\n                                if ($('vcodetext_header' + id) != null)\r\n                                    $('vcodetext_header' + id).value = '';\r\n                                secclick['vcodetext_header' + id] = 1;\r\n                                if (type)\r\n                                    $('vcodetext_header' + id + '_menu').style.top = parseInt($('vcodetext_header' + id + '_menu').style.top) - parseInt($('vcodetext_header' + id + '_menu').style.height) + 'px';\r\n                            }\r\n                            $('vcodetext_header' + id + '_menu').style.display = '';\r\n                            $('vcodetext_header' + id).unselectable = 'off';\r\n                            $('vcodeimg' + id).src = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=' + Math.random();\r\n                        }\r\n\r\n                        function innerVcode() {\r\n                            if ($('vcodetext_header1') == null) {\r\n                                $('vcode_header').innerHTML = '<input name=\"vcodetext\" tabindex=\"903\" size=\"20\" onkeyup=\"changevcode(this.form, this.value);\" class=\"txt\" style=\"width:50px;\" id=\"vcodetext_header' + vcodeimgid + '\" value=\"\" autocomplete=\"off\"/>' +\r\n                                                            '<span><a href=\"###\" onclick=\"vcodeimg' + vcodeimgid + '.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();return false;\" style=\"margin-left: 7px;\">看不清</a></span>' + '<p style=\"margin:6px 0\">输入下图中的字符</p>' +\r\n	                                                        '<div  style=\"cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;\" id=\"vcodetext_header' + vcodeimgid + '_menu\" onmouseout=\"seccodefocus = 0\" onmouseover=\"seccodefocus = 1\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?time=");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append("\" class=\"cursor\" id=\"vcodeimg' + vcodeimgid + '\" onclick=\"this.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();\"/></div>';\r\n                                optionVcode();\r\n                            }\r\n                        }\r\n\r\n                        function changevcode(form, value) {\r\n                            if (!$('vcode')) {\r\n                                var vcode = document.createElement('input');\r\n                                vcode.id = 'vcode';\r\n                                vcode.name = 'vcode';\r\n                                vcode.type = 'hidden';\r\n                                vcode.value = value;\r\n                                form.appendChild(vcode);\r\n                            } else {\r\n                                $('vcode').value = value;\r\n                            }\r\n                        }\r\n                    </");
	templateBuilder.Append("script>\r\n                ");
	}
	else
	{

	templateBuilder.Append("\r\n                    <script type=\"text/javascript\">\r\n                        function innerVcode() {\r\n                        }\r\n                    </");
	templateBuilder.Append("script>\r\n                ");
	}	//end if


	if (config.Secques==1)
	{

	templateBuilder.Append("\r\n			    <div id=\"floatlayout_login\" class=\"pbm\">\r\n					<select style=\"width:156px;margin-bottom:8px;\" id=\"question\" name=\"question\" name=\"question\" onchange=\"displayAnswer();\" tabindex=\"904\">\r\n						<option id=\"question\" value=\"0\" selected=\"selected\">安全提问(未设置请忽略)</option>\r\n						<option id=\"question\" value=\"1\">母亲的名字</option>\r\n						<option id=\"question\" value=\"2\">爷爷的名字</option>\r\n						<option id=\"question\" value=\"3\">父亲出生的城市</option>\r\n						<option id=\"question\" value=\"4\">您其中一位老师的名字</option>\r\n						<option id=\"question\" value=\"5\">您个人计算机的型号</option>\r\n						<option id=\"question\" value=\"6\">您最喜欢的餐馆名称</option>\r\n						<option id=\"question\" value=\"7\">驾驶执照的最后四位数字</option>\r\n					</select>\r\n					<input type=\"text\" tabindex=\"905\" class=\"txt\" size=\"20\" autocomplete=\"off\" style=\"width:140px;display:none;\"  id=\"answer\" name=\"answer\"/>\r\n		        </div>\r\n                ");
	}	//end if

	templateBuilder.Append("\r\n                <script type=\"text/javascript\">\r\n                    function closeIsMore() {\r\n                        $('ls_more').style.display = 'none';\r\n                    }\r\n                    function displayAnswer() {\r\n                        if ($(\"question\").value > 0)\r\n                            $(\"answer\").style.display = \"\";\r\n                        else\r\n                            $(\"answer\").style.display = \"none\";\r\n                    }\r\n                </");
	templateBuilder.Append("script>\r\n				<div class=\"ptm cl\" style=\"border-top:1px dashed #CDCDCD;\">\r\n					<a class=\"y xg2\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("getpassword.aspx\" onclick=\"hideWindow('register');hideWindow('login');showWindow('getpassword', this.href);\">找回密码</a>\r\n					<label class=\"z\" for=\"ls_cookietime\"><input type=\"checkbox\" tabindex=\"906\" value=\"2592000\" id=\"ls_cookietime\" name=\"expires\" checked=\"checked\" tabindex=\"906\"><span title=\"下次访问自动登录\">记住我</span></label>\r\n				</div>\r\n            </div>\r\n			</form>\r\n            ");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n			<div id=\"um\">\r\n				<div class=\"avt y\"><a alt=\"用户名称\" target=\"_blank\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\"><img src=\"");
	templateBuilder.Append(useravatar.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_small.gif';\" /></a></div>\r\n				<p>\r\n					<strong><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("userinfo.aspx?userid=");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("\" class=\"vwmy\">");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</a></strong><span class=\"xg1\">在线</span><span class=\"pipe\">|</span>\r\n                    ");	string linktitle = "";
	
	string showoverflow = "";
	

	if (oluserinfo.Newpms>0)
	{


	if (oluserinfo.Newpms>=1000)
	{

	 showoverflow = "大于";
	

	}	//end if

	 linktitle = "您有"+showoverflow+oluserinfo.Newpms+"条新短消息";
	

	}
	else
	{

	 linktitle = "您没有新短消息";
	

	}	//end if

	templateBuilder.Append("\r\n					<a id=\"pm_ntc\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpinbox.aspx\" title=\"");
	templateBuilder.Append(linktitle.ToString());
	templateBuilder.Append("\">短消息\r\n                    ");
	if (oluserinfo.Newpms>0 && oluserinfo.Newpms<=1000)
	{

	templateBuilder.Append("\r\n                                (");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	if (oluserinfo.Newpms>=1000)
	{

	templateBuilder.Append("1000+");
	}	//end if

	templateBuilder.Append(")\r\n                    ");
	}	//end if

	templateBuilder.Append("</a>\r\n                    <span class=\"pipe\">|</span>\r\n                    ");	 showoverflow = "";
	

	if (oluserinfo.Newnotices>0)
	{


	if (oluserinfo.Newnotices>=1000)
	{

	 showoverflow = "大于";
	

	}	//end if

	 linktitle = "您有"+showoverflow+oluserinfo.Newnotices+"条新通知";
	

	}
	else
	{

	 linktitle = "您没有新通知";
	

	}	//end if

	templateBuilder.Append("\r\n					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpnotice.aspx?filter=all\" title=\"");
	templateBuilder.Append(linktitle.ToString());
	templateBuilder.Append("\">\r\n                        通知");
	if (oluserinfo.Newnotices>0)
	{

	templateBuilder.Append("\r\n                                (");
	templateBuilder.Append(oluserinfo.Newnotices.ToString().Trim());
	if (oluserinfo.Newnotices>=1000)
	{

	templateBuilder.Append("+");
	}	//end if

	templateBuilder.Append(")\r\n                            ");
	}	//end if

	templateBuilder.Append("\r\n                    </a>\r\n                    <span class=\"pipe\">|</span>\r\n					<a id=\"usercenter\" class=\"drop\" onmouseover=\"showMenu(this.id);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\">用户中心</a>\r\n				");
	if (config.Regstatus==2||config.Regstatus==3)
	{


	if (userid>0)
	{

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("invite.aspx\">邀请</a>\r\n					");
	}	//end if


	}	//end if


	if (useradminid==1)
	{

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("admin/index.aspx\" target=\"_blank\">系统设置</a>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("logout.aspx?userkey=");
	templateBuilder.Append(userkey.ToString());
	templateBuilder.Append("\">退出</a>\r\n				</p>\r\n				");
	templateBuilder.Append(userinfotips.ToString());
	templateBuilder.Append("\r\n			</div> \r\n			<div id=\"pm_ntc_menu\" class=\"g_up\" style=\"display:none;\">\r\n				<div class=\"mncr\"></div>\r\n				<div class=\"crly\">\r\n					<div style=\"clear:both;font-size:0;\"></div>\r\n					<span class=\"y\"><a onclick=\"javascript:$('pm_ntc_menu').style.display='none';closenotice(");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append(");\" href=\"javascript:;\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/delete.gif\" alt=\"关闭\"/></a></span>\r\n					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpinbox.aspx\">您有");
	if (oluserinfo.Newpms>=1000)
	{

	templateBuilder.Append("大于");
	}	//end if
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("条新消息</a>\r\n				</div>\r\n			</div>\r\n            <script type=\"text/javascript\">\r\n            setMenuPosition('pm_ntc', 'pm_ntc_menu', '43');\r\n            if(");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append(" > 0 && (getcookie(\"shownotice\") != \"0\" || getcookie(\"newpms\") != ");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("))\r\n            {\r\n                $(\"pm_ntc_menu\").style.display='';\r\n            }            \r\n            </");
	templateBuilder.Append("script>\r\n            ");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n		<div id=\"menubar\">\r\n			<a onMouseOver=\"showMenu(this.id, false);\" href=\"javascript:void(0);\" id=\"mymenu\">我的中心</a>\r\n            <div class=\"popupmenu_popup headermenu_popup\" id=\"mymenu_menu\" style=\"display: none\">\r\n            ");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n			<ul class=\"sel_my\">\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("mytopics.aspx\">我的主题</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("myposts.aspx\">我的帖子</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("search.aspx?posterid=current&type=digest&searchsubmit=1\">我的精华</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("myattachment.aspx\">我的附件</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpsubscribe.aspx\">我的收藏</a></li>\r\n			");
	if (config.Enablespace==1)
	{

	templateBuilder.Append("\r\n				<li class=\"myspace\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("space/\">我的空间</a></li>\r\n			");
	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n				<li class=\"myalbum\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showalbumlist.aspx?uid=");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("\">我的相册</a></li>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n            </ul>\r\n            ");
	}
	else
	{

	templateBuilder.Append("\r\n			<p class=\"reg_tip\">\r\n				<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onClick=\"showWindow('register', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx');hideWindow('login');\" class=\"xg2\">登录或注册新用户,开通自己的个人中心</a>\r\n			</p>\r\n            ");
	}	//end if


	if (config.Allowchangewidth==1&&pagename!="website.aspx")
	{

	templateBuilder.Append("\r\n           <ul class=\"sel_mb\">\r\n				<li><a href=\"javascript:;\" onclick=\"widthauto(this,'");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("')\">");
	if (isnarrowpage)
	{

	templateBuilder.Append("切换到宽版");
	}
	else
	{

	templateBuilder.Append("切换到窄版");
	}	//end if

	templateBuilder.Append("</a></li>\r\n 			</ul>\r\n        ");
	}	//end if

	templateBuilder.Append("\r\n            </div>\r\n			<ul id=\"menu\" class=\"cl\">\r\n				");
	templateBuilder.Append(mainnavigation.ToString());
	templateBuilder.Append("\r\n			</ul>\r\n		</div>\r\n	</div>\r\n</div>\r\n");
	}
	else
	{


	Response.Clear();
	Response.ContentType = "Text/XML";
	Response.Expires = 0;
	Response.Cache.SetNoStore();
	
	templateBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?><root><![CDATA[\r\n");
	}	//end if



	templateBuilder.Append("\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/bbcode.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/editor.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_calendar.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src= \"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_attach.js\"></");
	templateBuilder.Append("script>\r\n<div class=\"wrap cl pageinfo\">\r\n	<div id=\"nav\">\r\n	");
	if (usergroupinfo.Allowsearch>0)
	{


	templateBuilder.Append("<form method=\"post\" action=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("search.aspx\" target=\"_blank\" onsubmit=\"bind_keyword(this);\" class=\"y\">\r\n	<input type=\"hidden\" name=\"poster\" />\r\n	<input type=\"hidden\" name=\"keyword\" />\r\n	<input type=\"hidden\" name=\"type\" value=\"\" />\r\n	<input id=\"keywordtype\" type=\"hidden\" name=\"keywordtype\" value=\"0\" />\r\n	<a href=\"javascript:void(0);\" class=\"drop s_type\" id=\"quicksearch\" onclick=\"showMenu(this.id, false);\" onmouseover=\"MouseCursor(this);\">快速搜索</a>\r\n	<input type=\"text\" name=\"keywordf\" value=\"输入搜索关键字\" onblur=\"if(this.value=='')this.value=defaultValue\" onclick=\"if(this.value==this.defaultValue)this.value = ''\" onkeydown=\"if(this.value==this.defaultValue)this.value = ''\" class=\"txt\"/>\r\n	<input name=\"searchsubmit\" type=\"submit\" value=\"\" class=\"btnsearch\"/>\r\n</form>\r\n<ul id=\"quicksearch_menu\" class=\"p_pop\" style=\"display: none;\">\r\n	<li><a href=\"###\" onclick=\"$('keywordtype').value='0';$('quicksearch').innerHTML='帖子标题';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">帖子标题</a></li>\r\n	");
	if (config.Enablespace==1)
	{

	templateBuilder.Append("\r\n	<li><a href=\"###\" onclick=\"$('keywordtype').value='2';$('quicksearch').innerHTML='空间日志';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">空间日志</a></li>\r\n	");
	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n	<li><a href=\"###\" onclick=\"$('keywordtype').value='3';$('quicksearch').innerHTML='相册标题';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">相册标题</a></li>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<li><a href=\"###\" onclick=\"$('keywordtype').value='8';$('quicksearch').innerHTML='作者';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">作者</a></li>\r\n	<li><a href=\"###\" onclick=\"$('keywordtype').value='9';$('quicksearch').innerHTML='版块';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">版块</a></li>\r\n</ul>\r\n<script type=\"text/javascript\">\r\n    function bind_keyword(form) {\r\n        if (form.keywordtype.value == '9') {\r\n            form.action = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("forumsearch.aspx?q=' + escape(form.keywordf.value);\r\n        } else if (form.keywordtype.value == '8') {\r\n            form.keyword.value = '';\r\n            form.poster.value = form.keywordf.value != form.keywordf.defaultValue ? form.keywordf.value : '';\r\n        } else {\r\n            form.poster.value = '';\r\n            form.keyword.value = form.keywordf.value != form.keywordf.defaultValue ? form.keywordf.value : '';\r\n            if (form.keywordtype.value == '2')\r\n                form.type.value = 'spacepost';\r\n            if (form.keywordtype.value == '3')\r\n                form.type.value = 'album';\r\n        }\r\n    }\r\n</");
	templateBuilder.Append("script>");


	}	//end if

	templateBuilder.Append("\r\n		<a href=\"");
	templateBuilder.Append(config.Forumurl.ToString().Trim());
	templateBuilder.Append("\" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a> &raquo; ");
	if (forum.Pathlist!="")
	{
	templateBuilder.Append(ShowForumAspxRewrite(forum.Pathlist.Trim(),forumid,forumpageid).ToString().Trim());
	templateBuilder.Append(" &raquo; ");
	}	//end if


	if (topic!=null)
	{

	templateBuilder.Append("\r\n            <a href=\"");
	templateBuilder.Append(ShowTopicAspxRewrite(topicid,0).ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a> &raquo; &nbsp;\r\n        ");
	}	//end if

	templateBuilder.Append("\r\n        <strong>编辑帖子</strong>\r\n    </div>\r\n</div>\r\n<script type=\"text/javascript\">\r\nvar postminchars = parseInt(");
	templateBuilder.Append(config.Minpostsize.ToString().Trim());
	templateBuilder.Append(");\r\nvar postmaxchars = parseInt(");
	templateBuilder.Append(config.Maxpostsize.ToString().Trim());
	templateBuilder.Append(");\r\nvar disablepostctrl = parseInt(");
	templateBuilder.Append(disablepostctrl.ToString());
	templateBuilder.Append(");\r\nvar tempaccounts = false;\r\nvar forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\nvar posturl=forumpath+'editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("';\r\n</");
	templateBuilder.Append("script>\r\n");
	if (page_err==0)
	{


	if (ispost)
	{


	templateBuilder.Append("<div class=\"wrap s_clear\" id=\"wrap\">\r\n<div class=\"main\">\r\n	<div class=\"msgbox\">\r\n		<h1>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("　提示信息</h1>\r\n		<hr class=\"solidline\"/>\r\n		<div class=\"msg_inner\">\r\n			<p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n			");
	if (msgbox_url!="")
	{

	templateBuilder.Append("\r\n			<p><a href=\"");
	templateBuilder.Append(msgbox_url.ToString());
	templateBuilder.Append("\">如果浏览器没有转向, 请点击这里.</a></p>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n	</div>\r\n</div>\r\n</div>");


	}
	else
	{

	templateBuilder.Append("\r\n<div class=\"wrap cl post\">\r\n	<script type=\"text/javascript\">\r\n		function geteditormessage(theform)\r\n		{\r\n			var message = wysiwyg ? html2bbcode(getEditorContents()) : (!theform.parseurloff.checked ? parseurl(theform.message.value) : theform.message.value);\r\n			theform.message.value = message;\r\n		}\r\n	</");
	templateBuilder.Append("script>\r\n    <form method=\"post\" name=\"postform\" id=\"postform\" action=\"\" onsubmit=\"return validate(this);\">\r\n    ");
	    string formatNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
	    
	templateBuilder.Append("\r\n    <input type=\"hidden\" name=\"posttime\" id=\"posttime\" value=\"");
	templateBuilder.Append(formatNow.ToString());
	templateBuilder.Append("\" />\r\n	");
	templateBuilder.Append("<div id=\"editorbox\">\r\n	<div class=\"edt_main\">\r\n	<div class=\"edt_content cl\">\r\n		");	string special = DNTRequest.GetString("type").ToLower();;
	

	if (special=="" && topic.Special>0)
	{


	if (topic.Special==1)
	{

	 special = "poll";
	

	}	//end if


	if (topic.Special==2 || topic.Special==3)
	{

	 special = "bonus";
	

	}	//end if


	if (topic.Special==4)
	{

	 special = "debate";
	

	}	//end if


	}	//end if

	bool adveditor = (special!="" || topic.Special>0)&&isfirstpost;
	
	string action = pagename.Replace("post","").Replace(".aspx","").Replace("topic","newthread");
	
	string actiontitle = "";
	
	templateBuilder.Append("\r\n		<ul class=\"f_tab cl mbw\">\r\n			");
	if (pagename=="posttopic.aspx")
	{


	if (special=="bonus")
	{

	 actiontitle = "发布悬赏";
	

	}
	else if (special=="poll")
	{

	 actiontitle = "发布投票";
	

	}
	else if (special=="debate")
	{

	 actiontitle = "发布辩论";
	

	}
	else
	{

	 actiontitle = "发新主题";
	

	}	//end if

	templateBuilder.Append("\r\n				<li");
	if (special=="")
	{

	templateBuilder.Append(" class=\"cur_tab\"");
	}	//end if

	templateBuilder.Append("><a onclick=\"switchpost('posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&cedit=yes')\" href=\"javascript:;\">发表帖子</a></li>\r\n                ");
	if (forum.Allowspecialonly<=0)
	{


	if (1==(forum.Allowpostspecial&1)&&usergroupinfo.Allowpostpoll==1)
	{

	templateBuilder.Append("\r\n                <li");
	if (special=="poll")
	{

	templateBuilder.Append(" class=\"cur_tab\"");
	}	//end if

	templateBuilder.Append("><a onclick=\"switchpost('posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&type=poll&cedit=yes')\" href=\"javascript:;\">发起投票</a></li>\r\n                ");
	}	//end if


	if (4==(forum.Allowpostspecial&4)&&usergroupinfo.Allowbonus==1)
	{

	templateBuilder.Append("\r\n                <li");
	if (special=="bonus")
	{

	templateBuilder.Append(" class=\"cur_tab\"");
	}	//end if

	templateBuilder.Append("><a onclick=\"switchpost('posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&type=bonus&cedit=yes')\" href=\"javascript:;\">发布悬赏</a></li>\r\n                ");
	}	//end if


	if (16==(forum.Allowpostspecial&16)&&usergroupinfo.Allowdebate==1)
	{

	templateBuilder.Append("\r\n                <li");
	if (special=="debate")
	{

	templateBuilder.Append(" class=\"cur_tab\"");
	}	//end if

	templateBuilder.Append("><a onclick=\"switchpost('posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&type=debate&cedit=yes')\" href=\"javascript:;\">发起辩论</a></li>\r\n                ");
	}	//end if


	}	//end if


	}
	else if (pagename=="postreply.aspx")
	{

	int postlayer = DNTRequest.GetInt("postlayer", -1);
	
	string replyposter = Utils.HtmlEncode(DNTRequest.GetString("poster"));
	

	if (postlayer>0)
	{

	 actiontitle = "回复帖子";
	
	templateBuilder.Append("\r\n                    <div style=\"display:none;\">\r\n                        <div id=\"toreplay_div\"><strong>回复 <a target=\"_blank\" href=\"");
	templateBuilder.Append(DNTRequest.GetUrlReferrer().ToString().Trim());
	templateBuilder.Append("#");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(postlayer.ToString());
	templateBuilder.Append("楼<font color=\"Olive\">");
	templateBuilder.Append(replyposter.ToString());
	templateBuilder.Append("</font>的帖子</a></strong></div>\r\n                        <input type=\"hidden\" id=\"toreplay_user\" name=\"toreplay_user\"/>\r\n                        <script type=\"text/javascript\">\r\n                            String.prototype.trim = function(){return this.replace(/(^\\s*)|(\\s*$)/g, \"\");}\r\n                            ");
	if (replyposter!="")
	{

	templateBuilder.Append("$(\"toreplay_user\").value=trim(html2bbcode1($(\"toreplay_div\").innerHTML)).trim();");
	}	//end if

	templateBuilder.Append("\r\n                        </");
	templateBuilder.Append("script>\r\n                    </div>\r\n                    ");	string url = "postreply.aspx?topicid="+topicid+"&forumpage="+forumpageid+"&postid="+DNTRequest.GetString("pageid")+"&postlayer="+DNTRequest.GetString("postlayer")+"&poster="+DNTRequest.GetString("poster");
	
	templateBuilder.Append("\r\n				    <li class=\"a\"><a onclick=\"switchpost('");
	templateBuilder.Append(url.ToString());
	templateBuilder.Append("')\" href=\"javascript:;\">回复帖子</a></li>\r\n                ");
	}
	else
	{

	 actiontitle = "回复主题";
	
	string url = "postreply.aspx?topicid="+topicid+"&forumpage="+forumpageid+"&postid="+DNTRequest.GetString("pageid")+"&postlayer="+DNTRequest.GetString("postlayer")+"&poster="+DNTRequest.GetString("poster");
	
	templateBuilder.Append("\r\n				    <li class=\"a\"><a onclick=\"switchpost('");
	templateBuilder.Append(url.ToString());
	templateBuilder.Append("')\" href=\"javascript:;\">回复主题</a></li>\r\n                ");
	}	//end if


	}
	else if (pagename=="editpost.aspx")
	{

	 actiontitle = "编辑帖子";
	
	string url = "editpost.aspx?topicid="+topicid+"&postid="+DNTRequest.GetString("postid")+"&forumpage="+forumpageid;
	
	templateBuilder.Append("\r\n				<li class=\"a\"><a onclick=\"switchpost('");
	templateBuilder.Append(url.ToString());
	templateBuilder.Append("')\" href=\"javascript:;\">编辑帖子</a></li>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</ul>\r\n		");	char comma = ',';
	
	string editorid = "e";
	
	int thumbwidth = 400;
	
	int thumbheight = 300;
	
	templateBuilder.Append("\r\n		<div id=\"postbox\">\r\n		<div class=\"pbt cl\">\r\n			<input type=\"hidden\" name=\"iconid\" id=\"iconid\" value=\"");
	templateBuilder.Append(topic.Iconid.ToString().Trim());
	templateBuilder.Append("\" />\r\n		");
	if (special=="" && isfirstpost)
	{

	templateBuilder.Append("\r\n		<div class=\"ftid\">\r\n			<a id=\"icon\" class=\"z\" onmouseover=\"InFloat='floatlayout_");
	templateBuilder.Append(action.ToString());
	templateBuilder.Append("';showMenu(this.id)\"><img id=\"icon_img\" src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(topic.Iconid.ToString().Trim());
	templateBuilder.Append(".gif\" style=\"margin-top:4px;\"/></a>\r\n		</div>\r\n		<ul id=\"icon_menu\" class=\"sltm\" style=\"display:none\">\r\n		");	string icons = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";
	

	int id__loop__id=0;
	foreach(string id in icons.Split(comma))
	{
		id__loop__id++;

	templateBuilder.Append("\r\n			<li><a href=\"javascript:;\"><img onclick=\"switchicon(");
	templateBuilder.Append(id.ToString());
	templateBuilder.Append(", this)\" src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(id.ToString());
	templateBuilder.Append(".gif\" alt=\"\" /></a></li>\r\n		");
	}	//end loop

	templateBuilder.Append("\r\n		</ul>\r\n		");
	}	//end if


	if (forum.Applytopictype==1 && topictypeselectoptions!=""&&isfirstpost)
	{

	templateBuilder.Append("\r\n			<div class=\"ftid\">\r\n				<select name=\"typeid\" id=\"typeid\">");
	templateBuilder.Append(topictypeselectoptions.ToString());
	templateBuilder.Append("</select>\r\n			</div>\r\n            <script type=\"text/javascript\" reload=\"1\">$('typeid').value = '");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("';</");
	templateBuilder.Append("script>\r\n			<script type=\"text/javascript\">simulateSelect(\"typeid\");</");
	templateBuilder.Append("script>\r\n		");
	}	//end if


	if (!isfirstpost && (topic.Special==4||special=="debate"))
	{

	templateBuilder.Append("\r\n			<div class=\"ftid\">\r\n				<select id=\"debateopinion\" name=\"debateopinion\">\r\n					<option value=\"0\">观点</option>\r\n					<option value=\"1\">正方</option>\r\n					<option value=\"2\">反方</option>\r\n				</select>\r\n			</div>\r\n			<script type=\"text/javascript\">simulateSelect(\"debateopinion\");</");
	templateBuilder.Append("script>\r\n			<script type=\"text/javascript\" reload=\"1\">$('debateopinion').selectedIndex = parseInt(getQueryString(\"debate\"));</");
	templateBuilder.Append("script>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n			<span class=\"z\">\r\n            <input name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" type=\"text\" id=\"title\" value=\"");
	templateBuilder.Append(postinfo.Title.ToString().Trim());
	templateBuilder.Append("\" class=\"txt postpx\"/>\r\n		");
	if (action=="reply" || postinfo.Layer>0)
	{

	templateBuilder.Append("\r\n			<cite class=\"tips\">(可选)</cite>\r\n		");
	}	//end if

	templateBuilder.Append("标题最多为60个字符，还可输入<b><span id=\"chLeft\">60</span></b>\r\n		");
	if (canhtmltitle)
	{

	templateBuilder.Append("<a href=\"###\" id=\"titleEditorButton\" onclick=\"RootTitleEditor();\" class=\"xg2\" style=\"margin-left:10px;\">高级编辑</a>");
	}	//end if

	templateBuilder.Append("\r\n            </span>\r\n		");
	if (needaudit)
	{

	templateBuilder.Append("<em class=\"needverify\">需审核</em>");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n		");
	if (canhtmltitle)
	{

	templateBuilder.Append("\r\n		<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/simplyeditor.js\" reload=\"1\"></");
	templateBuilder.Append("script>\r\n		<div class=\"pbt cl\" id=\"editorDiv\" style=\"display: none;\">\r\n			<div class=\"title_editor\" id=\"titleEditorDiv\">\r\n                <script type=\"text/javascript\">\r\n                    var titleEditor;\r\n                    function RootTitleEditor(){\r\n                        if($('editorDiv').style.display == 'none')\r\n                        AdvancedTitleEditor();\r\n                        else\r\n                        TextTitleBox();\r\n                    }\r\n                    function AdvancedTitleEditor() {\r\n                        $('editorDiv').style.display = '';\r\n                        $('title').style.display = 'none';\r\n                        if(titleEditor==null){\r\n                            titleEditor = new SimplyEditor('htmltitle', 'titleEditorDiv', '");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("', '");
	templateBuilder.Append(htmltitle.ToString());
	templateBuilder.Append("');\r\n                        }\r\n                        $('titleEditorButton').innerHTML='普通编辑';\r\n                    }\r\n                    function TextTitleBox(){\r\n                        $('editorDiv').style.display = 'none';\r\n                        $('title').style.display = '';\r\n                        $('titleEditorButton').innerHTML='高级编辑';\r\n                    }\r\n//                    $('titleEditorButton').onclick = function () {\r\n//                        AdvancedTitleEditor();\r\n//                    };\r\n                    ");
	if (htmltitle!="")
	{

	templateBuilder.Append("\r\n			            AdvancedTitleEditor();\r\n			        ");
	}	//end if

	templateBuilder.Append("\r\n                </");
	templateBuilder.Append("script>\r\n			</div>\r\n		</div>\r\n		");
	}	//end if


	if (adveditor)
	{

	templateBuilder.Append("\r\n		<div id=\"specialpost\" class=\"pbt cl\"></div>\r\n		<script type=\"text/javascript\" reload=\"1\">\r\n			_attachEvent(window, \"load\", function(){ \r\n			if($('specialposttable')) {\r\n				$('specialpost').innerHTML = $('specialposttable').innerHTML;\r\n				$('specialposttable').innerHTML = '';\r\n			}\r\n			});\r\n		</");
	templateBuilder.Append("script>\r\n		");
	}	//end if


	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\" reload=\"1\" ></");
	templateBuilder.Append("script>\r\n");
	    /*下方代码会在_postattachment中的大量程序中使用*/
	
	templateBuilder.Append("\r\n<script type=\"text/javascript\">\r\n    var TABLEBG = '#FFF';//'{  WRAPBG  }';\r\n    var uid = parseInt('");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("');\r\n\r\n    var special = parseInt('0');\r\n    var charset = 'utf-8';\r\n    var thumbwidth = parseInt(400);\r\n    var thumbheight = parseInt(300);\r\n    var extensions = '");
	templateBuilder.Append(attachextensions.ToString());
	templateBuilder.Append("';\r\n    var ATTACHNUM = {'imageused':0,'imageunused':0,'attachused':0,'attachunused':0};\r\n    var pid = parseInt('");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("');\r\n    var fid = ");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(";\r\n    var custombbcodes = { ");
	templateBuilder.Append(customeditbuttons.ToString());
	templateBuilder.Append(" };\r\n</");
	templateBuilder.Append("script>\r\n<div class=\"edt cl\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_body\">\r\n	<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_controls\" class=\"bar\">\r\n		<div class=\"y\">\r\n			<div class=\"b2r nbl nbr\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_5\">\r\n				<p><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_undo\" title=\"撤销\">Undo</a></p>\r\n				<p><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_redo\" title=\"重做\">Redo</a></p>\r\n			</div>\r\n			<div class=\"z\">\r\n				<span class=\"mbn\"><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_simple\"></a><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fullswitcher\"></a></span>\r\n				<label id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_switcher\" class=\"bar_swch ptn\"><input type=\"checkbox\" class=\"pc\" name=\"checkbox\" value=\"0\" ");
	if (config.Defaulteditormode==0)
	{

	templateBuilder.Append("checked=\"checked\"");
	}	//end if

	templateBuilder.Append(" onclick=\"switchEditor(this.checked?0:1)\" />代码模式</label>\r\n			</div>\r\n		</div>\r\n		<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_button\" class=\"cl\">\r\n			<div class=\"b1r\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_s0\">\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_paste\" title=\"粘贴\">粘贴</a>\r\n			</div>\r\n			<div class=\"b2r nbr\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_s2\">\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontname\" class=\"dp\" title=\"设置字体\"><span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_font\">字体</span></a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontsize\" class=\"dp\" title=\"设置文字大小\"><span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_size\">大小</span></a>\r\n				<br id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_1\" />\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_bold\" title=\"粗体\">B</a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_italic\" title=\"文字斜体\">I</a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_underline\" title=\"文字加下划线\">U</a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_forecolor\" title=\"设置文字颜色\">Color</a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_url\" title=\"添加链接\">Url</a>\r\n				<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_8\">\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_unlink\" title=\"移除链接\">Unlink</a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_inserthorizontalrule\" title=\"分隔线\">Hr</a>\r\n				</span>\r\n			</div>\r\n			<div class=\"b2r nbl\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_2\">\r\n				<p id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_3\"><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_tbl\" title=\"添加表格\">Table</a></p>\r\n				<p>	<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_removeformat\" title=\"清除文本格式\">Removeformat</a></p>\r\n			</div>\r\n			<div class=\"b2r\">\r\n				<p>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_justifyleft\" title=\"居左\">Left</a>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_justifycenter\" title=\"居中\">Center</a>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_justifyright\" title=\"居右\">Right</a>\r\n				</p>\r\n				<p id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_4\">\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_autotypeset\" title=\"自动排版\">Autotypeset</a>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_insertorderedlist\" title=\"排序的列表\">Orderedlist</a>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_insertunorderedlist\" title=\"未排序列表\">Unorderedlist</a>\r\n				</p>\r\n			</div>\r\n			<div class=\"b1r\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_s1\">\r\n                ");
	if (config.Smileyinsert==1 && forum.Allowsmilies==1)
	{

	templateBuilder.Append("\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_sml\" title=\"添加表情\">表情</a>\r\n                ");
	}	//end if

	templateBuilder.Append("\r\n				<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_imagen\" style=\"display:none\">!</div>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image\" title=\"添加图片\">图片</a>\r\n				");
	if (canpostattach)
	{

	templateBuilder.Append("\r\n				<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attachn\" style=\"display:none\">!</div>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attach\" title=\"添加附件\">附件</a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_aud\" title=\"添加音乐\">音乐</a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_vid\" title=\"添加视频\">视频</a>\r\n				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fls\" title=\"添加 Flash\">Flash</a>\r\n			</div>\r\n			<div class=\"b2r nbr\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_6\">\r\n				<p>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_code\" title=\"添加代码文字\">代码</a>	\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_hide\" title=\"隐藏内容\">隐藏内容</a>				\r\n				</p>\r\n				<p>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_quote\" title=\"添加引用文字\">引用</a>\r\n					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_free\" title=\"免费信息\">免费信息</a>\r\n				</p>\r\n			</div>\r\n			<div class=\"b2r nbl\">\r\n			<script type=\"text/javascript\">\r\n			    //自定义按扭显示\r\n			    if (typeof (custombbcodes) != 'undefined') {\r\n			        var i = 0;\r\n			        var firstlayer = \"\";\r\n			        var secondlayer = \"\";\r\n			        for (var id in custombbcodes) {\r\n			            if (custombbcodes[id][1] == '') {\r\n			                continue;\r\n			            }\r\n			            if (i % 2 == 0)\r\n			                firstlayer += '<a class=\"cst\" id=\"e_cst' + custombbcodes[id][5] + '_' + custombbcodes[id][0] + '\"><img title=\"' + custombbcodes[id][2] + '\" alt=\"' + custombbcodes[id][2] + '\" src = \"editor/images/' + custombbcodes[id][1] + '\" /></a>';\r\n			            else\r\n			                secondlayer += '<a class=\"cst\" id=\"e_cst' + custombbcodes[id][5] + '_' + custombbcodes[id][0] + '\"><img title=\"' + custombbcodes[id][2] + '\" alt=\"' + custombbcodes[id][2] + '\" src = \"editor/images/' + custombbcodes[id][1] + '\" /></a>';\r\n			            i++;\r\n			            //document.writeln('<a class=\"cst\" id=\"e_cst' + custombbcodes[id][5] + '_' + custombbcodes[id][0] + '\"><img title=\"' + custombbcodes[id][2] + '\" alt=\"' + custombbcodes[id][2] + '\" src = \"editor/images/' + custombbcodes[id][1] + '\" /></a>');\r\n			        }\r\n\r\n			        document.writeln(firstlayer + \"<br id=\\\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_7\\\" />\" + secondlayer);\r\n			    }\r\n			</");
	templateBuilder.Append("script>\r\n			</div>			\r\n		</div>\r\n	</div>\r\n	\r\n	<div id=\"rstnotice\" class=\"ntc_l bbs\" style=\"display:none\">\r\n        <a href=\"javascript:;\" title=\"清除内容\" class=\"d y\" onclick=\"userdataoption(0)\">close</a>\r\n        您有上次未提交成功的数据 <a class=\"xi2\" href=\"javascript:;\" onclick=\"userdataoption(1,'");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("')\"><strong>恢复数据</strong></a>\r\n    </div>\r\n	\r\n	<div class=\"area cl\">\r\n		<textarea name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_textarea\" class=\"pt\" tabindex=\"1\" rows=\"15\">");
	templateBuilder.Append(message.ToString());
	templateBuilder.Append("</textarea>\r\n	</div>\r\n	");
	templateBuilder.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/editor.css\" />\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post_editor.js\" ></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\n	var infloat = ");
	templateBuilder.Append(infloat.ToString());
	templateBuilder.Append(";\r\n	var InFloat_Editor = 'floatlayout_");
	templateBuilder.Append(action.ToString());
	templateBuilder.Append("';\r\n	var editoraction = '");
	templateBuilder.Append(action.ToString());
	templateBuilder.Append("';\r\n	var lang	= new Array();\r\n	lang['post_discuzcode_code'] = '请输入要插入的代码';\r\n	lang['post_discuzcode_quote'] = '请输入要插入的引用';\r\n	lang['post_discuzcode_free'] = '请输入要插入的免费信息';\r\n	lang['post_discuzcode_hide'] = '请输入要插入的隐藏内容';\r\n	lang['board_allowed'] = '系统限制';\r\n	lang['lento'] = '到';\r\n	lang['bytes'] = '字节';\r\n	lang['post_curlength'] = '当前长度';\r\n	lang['post_title_and_message_isnull'] = '请完成标题或内容栏。';\r\n	lang['post_title_toolong'] = '您的标题超过 60 个字符的限制。';\r\n	lang['post_message_length_invalid'] = '您的帖子长度不符合要求。';\r\n	lang['post_type_isnull'] = '请选择主题对应的分类。';\r\n	lang['post_reward_credits_null'] = '对不起，您输入悬赏积分。';\r\n	lang['post_attachment_ext_notallowed']	= '对不起，不支持上传此类扩展名的附件。';\r\n	lang['post_attachment_img_invalid']		= '无效的图片文件。';\r\n	lang['post_attachment_deletelink']		= '删除';\r\n	lang['post_attachment_insert']			= '点击这里将本附件插入帖子内容中当前光标的位置';\r\n	lang['post_attachment_insertlink']		= '插入';\r\n\r\n	lang['enter_list_item']			= \"输入一个列表项目.\\r\\n留空或者点击取消完成此列表.\";\r\n	lang['enter_link_url']			= \"请输入链接的地址:\";\r\n	lang['enter_image_url']			= \"请输入图片链接地址:\";\r\n	lang['enter_email_link']		= \"请输入此链接的邮箱地址:\";\r\n	lang['fontname']				= \"字体\";\r\n	lang['fontsize']				= \"大小\";\r\n	lang['post_advanceeditor']		= \"全部功能\";\r\n	lang['post_simpleeditor']		= \"简单功能\";\r\n	lang['submit']					= \"提交\";\r\n	lang['cancel']					= \"取消\";\r\n	lang['post_autosave_none'] = \"没有可以恢复的数据\";\r\n	lang['post_autosave_confirm'] = \"本操作将覆盖当前帖子内容，确定要恢复数据吗？\";\r\n	lang['enter_tag_option']		= \"请输入 %1 标签的选项:\";\r\n	lang['enter_table_rows']		= \"请输入行数，最多 30 行:\";\r\n	lang['enter_table_columns']		= \"请输入列数，最多 30 列:\";\r\nvar editorid = '");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("';\r\n	var editorcss = 'templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/editor.css';\r\n	var textobj = $(editorid + '_textarea');\r\n	var typerequired = parseInt('0');\r\n	var seccodecheck = parseInt('0');\r\n	var secqaacheck = parseInt('0');\r\n	var special = 1;\r\n	");
	if (special=="")
	{

	templateBuilder.Append("\r\n	special = 0;\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	var isfirstpost = 0;\r\n	");
	if (isfirstpost)
	{

	templateBuilder.Append("\r\n	isfirstpost = 1;\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	var allowposttrade = parseInt('1');\r\n	var allowpostreward = parseInt('1');\r\n	var allowpostactivity = parseInt('1');\r\n\r\n	var bbinsert = parseInt('1');\r\n	\r\n	var allowhtml = parseInt('");
	templateBuilder.Append(htmlon.ToString());
	templateBuilder.Append("');\r\n	var forumallowhtml = parseInt('1');\r\n	var allowsmilies = 1 - parseInt('");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("');\r\n	var allowbbcode = parseInt('");
	templateBuilder.Append(usergroupinfo.Allowcusbbcode.ToString().Trim());
	templateBuilder.Append("') == 1 && parseInt('");
	templateBuilder.Append(forum.Allowbbcode.ToString().Trim());
	templateBuilder.Append("') == 1;\r\n	var allowimgcode = parseInt('");
	templateBuilder.Append(forum.Allowimgcode.ToString().Trim());
	templateBuilder.Append("');\r\n\r\n	//var wysiwyg = (is_ie || is_moz || (is_opera && opera.version() >= 9)) && parseInt('");
	templateBuilder.Append(config.Defaulteditormode.ToString().Trim());
	templateBuilder.Append("') && allowbbcode == 1 ? 1 : 0;//bbinsert == 1 ? 1 : 0;\r\n	var wysiwyg = (BROWSER.ie || BROWSER.firefox || (BROWSER.opera >= 9)) && parseInt('");
	templateBuilder.Append(config.Defaulteditormode.ToString().Trim());
	templateBuilder.Append("') && allowbbcode == 1 ? 1 : 0;//bbinsert == 1 ? 1 : 0;\r\n	var allowswitcheditor = parseInt('");
	templateBuilder.Append(config.Allowswitcheditor.ToString().Trim());
	templateBuilder.Append("') && allowbbcode == 1 ;\r\n\r\n	var custombbcodes = { ");
	templateBuilder.Append(Caches.GetCustomEditButtonList().ToString().Trim());
	templateBuilder.Append(" };\r\n\r\n	var smileyinsert = parseInt('1');\r\n	var smiliesCount = 32;//显示表情总数\r\n	var colCount = 8; //每行显示表情个数\r\n	var title = \"\";				   //标题\r\n	var showsmiliestitle = 1;        //是否显示标题（0不显示 1显示）\r\n	var smiliesIsCreate = 0;		   //编辑器是否已被创建(0否，1是）\r\n	\r\n	var maxpolloptions = parseInt('");
	templateBuilder.Append(config.Maxpolloptions.ToString().Trim());
	templateBuilder.Append("');\r\n	function alloweditorhtml() {\r\n		if($('htmlon').checked) {\r\n			allowhtml = 1;\r\n			forumallowhtml = 1;\r\n		} else {\r\n			allowhtml = 0;\r\n			forumallowhtml = 0;\r\n		}\r\n	}\r\n	var simplodemode = parseInt('1');\r\n		editorsimple();\r\n</");
	templateBuilder.Append("script>\r\n<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_bbar\" class=\"bbar\">\r\n	<em id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_tip\"></em>\r\n	<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_svdsecond\"></span>\r\n	<a href=\"javascript:;\" onclick=\"discuzcode('svd',{'titlename':'");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("','contentname':'");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("'});return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_svd\">保存数据</a> |\r\n	<a href=\"javascript:;\" onclick=\"discuzcode('rst','");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("');return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_rst\">恢复数据</a> &nbsp;&nbsp;\r\n	<a href=\"javascript:;\" onclick=\"discuzcode('chck');return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_chck\">字数检查</a> |\r\n	<a href=\"javascript:;\" onclick=\"discuzcode('tpr');return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_tpr\">清空内容</a> &nbsp;&nbsp;\r\n	<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_resize\"><a href=\"javascript:;\" onclick=\"editorsize('+')\">加大编辑框</a> | <a href=\"javascript:;\" onclick=\"editorsize('-')\">缩小编辑器</a><img src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("editor/images/resize.gif\" onmousedown=\"editorresize(event)\" /></span>\r\n</div>");

	templateBuilder.Append("\r\n</div>\r\n<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_menus\" class=\"editorrow\" style=\"overflow: hidden; margin-top: -5px; height: 0; border: none; background: transparent;\">\r\n	");
	templateBuilder.Append("<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_editortoolbar\" class=\"editortoolbar\">\r\n	<div class=\"p_pop fnm\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontname_menu\" style=\"display: none\">\r\n	<ul unselectable=\"on\">\r\n	");	string fontoptions = "仿宋_GB2312,黑体,楷体_GB2312,宋体,新宋体,微软雅黑,TrebuchetMS,Tahoma,Arial,Impact,Verdana,TimesNewRoman";
	

	int fontname__loop__id=0;
	foreach(string fontname in fontoptions.Split(comma))
	{
		fontname__loop__id++;

	templateBuilder.Append("\r\n	    <li onclick=\"discuzcode('fontname', '");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("')\" style=\"font-family: ");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("\" unselectable=\"on\"><a href=\"javascript:;\" title=\"");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end loop

	templateBuilder.Append("\r\n	</ul>\r\n	</div>\r\n	");	string sizeoptions = "1,2,3,4,5,6,7";
	
	templateBuilder.Append("\r\n	<div class=\"p_pop fszm\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontsize_menu\" style=\"display: none\">\r\n	<ul unselectable=\"on\">\r\n		");
	int size__loop__id=0;
	foreach(string size in sizeoptions.Split(comma))
	{
		size__loop__id++;

	templateBuilder.Append("\r\n			<li onclick=\"discuzcode('fontsize', ");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append(")\" unselectable=\"on\"><a href=\"javascript:;\" title=\"");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append("\"><font size=\"");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append("\" unselectable=\"on\">");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append("</font></a></li>\r\n		");
	}	//end loop

	templateBuilder.Append("\r\n	</ul>\r\n	</div>\r\n</div>\r\n");
	if (config.Smileyinsert==1)
	{

	templateBuilder.Append("\r\n	<div class=\"p_pof upf\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_sml_menu\" style=\"display: none;width:320px;\">\r\n		");
	templateBuilder.Append("<div class=\"smilieslist\">\r\n	");	string defaulttypname = string.Empty;
	
	templateBuilder.Append("\r\n	<div id=\"smiliesdiv\">\r\n		<div class=\"smiliesgroup\" style=\"margin-right: 0pt;\">\r\n			<ul>\r\n			");
	int stype__loop__id=0;
	foreach(DataRow stype in Caches.GetSmilieTypesCache().Rows)
	{
		stype__loop__id++;


	if (stype__loop__id==1)
	{

	 defaulttypname = stype["code"].ToString().Trim();
	

	}	//end if


	if (stype__loop__id==1)
	{

	templateBuilder.Append("\r\n				<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles1(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "');\" class=\"current\">" + stype["code"].ToString().Trim() + "</a></li>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles1(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "');\">" + stype["code"].ToString().Trim() + "</a></li>\r\n				");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n			</ul>\r\n		 </div>\r\n		 <div style=\"clear: both;\" id=\"showsmilie\"></div>\r\n		 <table class=\"smilieslist_table\" id=\"s_preview_table\" style=\"display: none\"><tr><td class=\"smilieslist_preview\" id=\"s_preview\"></td></tr></table>\r\n		 <div id=\"showsmilie_pagenum\" class=\"smilieslist_page\">&nbsp;</div>\r\n	</div>\r\n</div>\r\n<script type=\"text/javascript\" reload=\"1\">\r\n	function getSmilies(func){\r\n		if($('showsmilie').innerHTML !='' && $('showsmilie').innerHTML != '正在加载表情...')\r\n			return;\r\n		var c = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=smilies\";\r\n		_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true);\r\n		setTimeout(\"if($('showsmilie').innerHTML=='')$('showsmilie').innerHTML = '正在加载表情...'\", 2000);\r\n	}\r\n	\r\n	function getSmilies_callback(obj) {\r\n		smilies_HASH = obj; \r\n		showsmiles1(1, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("');\r\n	}\r\n	//_attachEvent($('");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_popup_smilies'), 'click', function(){\r\n		getSmilies(getSmilies_callback);\r\n	//});\r\n</");
	templateBuilder.Append("script>");

	templateBuilder.Append("\r\n	</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<!-- <script type=\"text/javascript\">smilies_show('smiliesdiv', 8, editorid + '_');</");
	templateBuilder.Append("script> -->");

	templateBuilder.Append("\r\n</div>\r\n<div class=\"p_pof uploadfile\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_menu\" style=\"display: none\" unselectable=\"on\">\r\n	<span class=\"y\"><a href=\"javascript:;\" class=\"flbc\" onclick=\"hideMenu()\">关闭</a></span>\r\n	<ul id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_ctrl\" class=\"imguptype\" style=\"cursor: move;\">\r\n        <li><a onclick=\"switchImagebutton('www');\" id=\"e_btn_www\" class=\"\" hidefocus=\"true\" href=\"javascript:;\">网络图片</a></li>\r\n        <li><a onclick=\"switchImagebutton('imgattachlist');\" id=\"e_btn_imgattachlist\" hidefocus=\"true\" href=\"javascript:;\" class=\"\">图片列表</a></li>\r\n        <li><a onclick=\"switchImagebutton('multi');\" id=\"e_btn_multi\" hidefocus=\"true\" href=\"javascript:;\" class=\"current\">批量上传</a></li>\r\n    </ul>\r\n    \r\n    <div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_www\" unselectable=\"on\" class=\"p_opt popupfix\" style=\"display:none;\">\r\n        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n			<tr>\r\n				<th width=\"74%\">请输入图片地址<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_status\" class=\"xi1\"></span></th>\r\n				<th width=\"13%\">宽(可选)</th>\r\n				<th width=\"13%\">高(可选)</th>\r\n			</tr>\r\n			<tr>\r\n				<td><input type=\"text\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_param_1\" onchange=\"loadimgsize(this.value)\" style=\"width: 95%;\" value=\"\" class=\"px\" autocomplete=\"off\" /></td>\r\n				<td><input id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_param_2\" size=\"1\" value=\"\" class=\"px p_fre\" autocomplete=\"off\" /></td>\r\n				<td><input id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_param_3\" size=\"1\" value=\"\" class=\"px p_fre\" autocomplete=\"off\" /></td>\r\n			</tr>\r\n			<tr>\r\n				<td colspan=\"3\" class=\"pns mtn\">\r\n					<button type=\"button\" class=\"pn pnc\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_submit\"><strong>提交</strong></button>\r\n					<button type=\"button\" class=\"pn\" onclick=\"hideMenu();\"><em>取消</em></button>\r\n				</td>\r\n			</tr>\r\n		</table>\r\n	</div>\r\n  \r\n    <div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_multi\" unselectable=\"on\" class=\"p_opt\" style=\"\">\r\n        <div id=\"e_multiimg\" class=\"fswf\">\r\n            <embed width=\"470\" height=\"268\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" allowscriptaccess=\"always\" menu=\"false\" quality=\"high\" \r\n            src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("images/common/upload.swf?site=");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/attachupload.aspx%3fmod=swfupload%26type=image%26forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&amp;type=image&amp;random=");
	templateBuilder.Append(DateTime.Now.Ticks.ToString().Trim());
	templateBuilder.Append("\" />\r\n        </div>\r\n        <div class=\"notice uploadinfo\">\r\n            文件尺寸: <strong>");
	templateBuilder.Append(FormatBytes(usergroupinfo.Maxattachsize).ToString().Trim());
	templateBuilder.Append("</strong>&nbsp;\r\n			上传限制: <strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong> 个文件&nbsp;&nbsp;\r\n			<br />可用扩展名: <strong>");
	templateBuilder.Append(Attachments.GetImageAttachmentTypeString(attachextensionsnosize).ToString().Trim());
	templateBuilder.Append("</strong>&nbsp;\r\n        </div>\r\n    </div>\r\n	\r\n	<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_imgattachlist\" unselectable=\"on\" class=\"p_opt\" style=\"display:none;\">\r\n        <div class=\"upfilelist\">\r\n            <div id=\"usedimgattachlist\"></div>\r\n            <div id=\"imgattachlist\">\r\n                <p class=\"notice\">本帖还没有图片附件, <a onclick=\"switchImagebutton('multi');\" href=\"javascript:;\">点击这里</a>上传</p>\r\n            </div>\r\n            <div id=\"unusedimgattachlist\"></div>\r\n            ");
	if (action=="edit")
	{

	templateBuilder.Append("\r\n            <script type=\"text/javascript\">\r\n                var uploadedimagelist = eval('");
	templateBuilder.Append(AttachmentList().ToString());
	templateBuilder.Append("');\r\n                updateimagelistHTML(uploadedimagelist, 3); //加载已使用图片列表\r\n            </");
	templateBuilder.Append("script>\r\n		    ");
	}	//end if

	templateBuilder.Append("\r\n        </div>\r\n        <p style=\"display: none;\" id=\"imgattach_notice\" class=\"noticetip\">\r\n            <span class=\"xi1 xw1\">点击图片添加到帖子内容中</span>\r\n        </p>\r\n    </div>\r\n      \r\n</div>\r\n<input type=\"hidden\" name=\"wysiwyg\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_mode\" value=\"1\" />\r\n<input type=\"hidden\" id=\"testsubmit\" />\r\n");


	if (pagename=="posttopic.aspx" || (pagename=="editpost.aspx"&&isfirstpost))
	{


	if (enabletag)
	{

	templateBuilder.Append("\r\n			<div class=\"pbt cl margint\">\r\n				<p><strong>标签(Tags):</strong>(用空格隔开多个标签，最多可填写 5 个)</p>\r\n				<p><input type=\"text\" name=\"tags\" id=\"tags\" class=\"txt\" value=\"");
	templateBuilder.Append(topictags.ToString());
	templateBuilder.Append("\" tabindex=\"1\" /><button name=\"addtags\" type=\"button\" onclick=\"relatekw();return false\">+可用标签</button> <span id=\"tagselect\"></span></p>\r\n			</div>\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		<div id=\"moreinfo\"></div>\r\n		<div style=\"clear:both;\"></div>\r\n		<div class=\"pbt cl margint\">\r\n			<div class=\"custominfoarea\" id=\"custominfoarea\" style=\"display: none;\"></div>\r\n			");
	if (postinfo.Layer==0 && forum.Applytopictype==1)
	{

	templateBuilder.Append("\r\n			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"");
	templateBuilder.Append(forum.Postbytopictype.ToString().Trim());
	templateBuilder.Append("\" tabindex=\"3\">\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<script type=\"text/javascript\">\r\n				function RunMutiUpload() {\r\n				if ($('MultiUploadFile').content != null)\r\n					$('MultiUploadFile').content.MultiFileUpload.GetAttachmentList();	\r\n				}\r\n				checkLength($('title'), 60);//检查标题长度\r\n				function switchpost(href) {\r\n				    editchange = false;\r\n				    saveData(undefined,'postform','");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("');\r\n				    location.href = href;\r\n				    doane();\r\n				}\r\n				\r\n                if (getQueryString('cedit') == '' && loadUserdata('forum')){\r\n                    $('rstnotice').style.display = '';\r\n                }\r\n			</");
	templateBuilder.Append("script>\r\n			<button type=\"submit\" id=\"postsubmit\" value=\"true\"");
	if (pagename=="posttopic.aspx")
	{

	templateBuilder.Append(" name=\"topicsubmit\"");
	}
	else if (pagename=="postreply.aspx")
	{

	templateBuilder.Append(" name=\"replysubmit\"");
	}
	else if (pagename=="editpost.aspx")
	{

	templateBuilder.Append(" name=\"editsubmit\"");
	}	//end if

	templateBuilder.Append(" tabindex=\"1\" class=\"pn\"><span>");
	templateBuilder.Append(actiontitle.ToString());
	templateBuilder.Append("</span></button>\r\n			<span id=\"more_2\">\r\n			");
	if (userinfo.Spaceid>0 && special==""&&action=="newthread"&&config.Enablespace==1)
	{

	templateBuilder.Append("<input type=\"checkbox\" name=\"addtoblog\" /> 添加到个人空间");
	}	//end if

	templateBuilder.Append("\r\n			</span>\r\n			");
	if (isseccode)
	{

	templateBuilder.Append("<span style=\"position:relative\">");
	templateBuilder.Append("<div id=\"vcode_temp\"></div>\r\n<script type=\"text/javascript\" reload=\"1\">\r\n	var infloat = ");
	templateBuilder.Append(infloat.ToString());
	templateBuilder.Append(";\r\n	if (typeof vcodeimgid == 'undefined'){\r\n		var vcodeimgid = 1;\r\n	}\r\n	else\r\n	    vcodeimgid++;\r\n\r\n    $('vcode_temp').parentNode.innerHTML = '<input name=\"vcodetext\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"4\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"7\"");
	}	//end if

	templateBuilder.Append(" size=\"20\" onkeyup=\"changevcode(this.form, this.value);\" class=\"txt\" style=\"width:90px;\" id=\"vcodetext' + vcodeimgid + '\"  onblur=\"if(!seccodefocus) {display(this.id + \\'_menu\\')};\"  onfocus=\"opensecwin('+vcodeimgid+',1)\"   value=\"验证码\" autocomplete=\"off\"/>' +\r\n	                                       '<div class=\"seccodecontent\"  style=\"display:none;cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;\" id=\"vcodetext' + vcodeimgid + '_menu\" onmouseout=\"seccodefocus = 0\" onmouseover=\"seccodefocus = 1\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?time=");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append("\" class=\"cursor\" id=\"vcodeimg' + vcodeimgid + '\" onclick=\"this.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();\"/></div>';\r\n	\r\n	function changevcode(form, value){\r\n		if (!$('vcode')){\r\n			var vcode = document.createElement('input');\r\n			vcode.id = 'vcode';\r\n			vcode.name = 'vcode';\r\n			vcode.type = 'hidden';\r\n			vcode.value = value;\r\n			form.appendChild(vcode);\r\n		}else{\r\n			$('vcode').value = value;\r\n		}\r\n	}\r\n</");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\nvar secclick = new Array();\r\nvar seccodefocus = 0;\r\nfunction opensecwin(id,type) {\r\n	if($('vcode')){\r\n	$('vcode').parentNode.removeChild($('vcode'));}\r\n\r\n	if (!secclick['vcodetext' + id]) {\r\n	    $('vcodetext' + id).value = '';\r\n	    secclick['vcodetext' + id] = 1;\r\n	    if(type)\r\n	        $('vcodetext' + id + '_menu').style.top = parseInt($('vcodetext' + id + '_menu').style.top) - parseInt($('vcodetext' + id + '_menu').style.height) + 'px';\r\n	}\r\n\r\n	$('vcodetext' + id + '_menu').style.position = 'absolute';\r\n	$('vcodetext' + id + '_menu').style.top = (-parseInt($('vcodetext' + id + '_menu').style.height) - 2) + 'px';\r\n	$('vcodetext' + id + '_menu').style.left = '0px';\r\n	$('vcodetext' + id + '_menu').style.display = '';\r\n	$('vcodetext' + id).focus();\r\n	$('vcodetext' + id).unselectable = 'off';\r\n	$('vcodeimg' + id).src = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=' + Math.random();\r\n}\r\n</");
	templateBuilder.Append("script>");

	templateBuilder.Append("</span>");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n		</div>\r\n	</div>\r\n	</div>\r\n    <script type=\"text/javascript\">\r\n        var topicreadperm = [\r\n            {readaccess:'',grouptitle:'不限'},\r\n            ");
	int userGroupInfo__loop__id=0;
	foreach(UserGroupInfo userGroupInfo in userGroupInfoList)
	{
		userGroupInfo__loop__id++;


	if (userGroupInfo.Readaccess!=0)
	{

	templateBuilder.Append("\r\n            {readaccess:'");
	templateBuilder.Append(userGroupInfo.Readaccess.ToString().Trim());
	templateBuilder.Append("',grouptitle:'");
	templateBuilder.Append(Utils.RemoveHtml(userGroupInfo.Grouptitle).ToString().Trim());
	templateBuilder.Append("'},\r\n            ");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n            {readaccess:'255',grouptitle:'最高权限'}\r\n        ];\r\n    </");
	templateBuilder.Append("script>\r\n	<div class=\"edt_app\">\r\n		");
	if (pagename=="posttopic.aspx" || (pagename=="editpost.aspx"&&isfirstpost))
	{


	if (userid!=-1 && usergroupinfo.Allowsetreadperm==1)
	{

	templateBuilder.Append("\r\n			<p><strong>阅读权限:</strong></p>\r\n			<p class=\"mbn\">\r\n                <em class=\"ftid\">\r\n                    <select name=\"topicreadperm\" id=\"topicreadperm\" class=\"ps\" style=\"width:90px\"></select>\r\n                </em>\r\n                <script type=\"text/javascript\">\r\n                    for (var i = 0 ; i < topicreadperm.length ; i++) {\r\n                        var option = new Option(topicreadperm[i].grouptitle, topicreadperm[i].readaccess);\r\n                        option.title = \"阅读权限:\" + topicreadperm[i].readaccess;\r\n                        $('topicreadperm').options.add(option);\r\n                        if(topicreadperm[i].readaccess == ");
	templateBuilder.Append(topic.Readperm.ToString().Trim());
	templateBuilder.Append(")\r\n                            $('topicreadperm').options.selectedIndex = i;\r\n                    }\r\n                    simulateSelect(\"topicreadperm\");\r\n                </");
	templateBuilder.Append("script>\r\n                <img src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/faq.gif\" alt=\"Tip\" class=\"mtn vm\" style=\"margin: 0;\" onmouseover=\"showTip(this)\" tip=\"阅读权限按由高到低排列，高于或等于选中组的用户才可以阅读。\" />\r\n            </p>\r\n		");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		<h4 style=\"clear:both;\">发帖选项:</h4>\r\n		<p class=\"mbn\">\r\n            <input type=\"checkbox\" value=\"1\" name=\"htmlon\" id=\"htmlon\"  onclick=\"alloweditorhtml()\" ");
	if (usergroupinfo.Allowhtml!=1)
	{

	templateBuilder.Append("disabled");
	}	//end if


	if (htmlon==1)
	{

	templateBuilder.Append("checked");
	}	//end if

	templateBuilder.Append(" /><label for=\"htmlon\">html 代码</label>\r\n            <img src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/warning.gif\" alt=\"Tip\" class=\"mtn vm\" style=\"margin: 0;vertical-align: top;\" onmouseover=\"showTip(this)\" tip=\"使用html代码可能会与表情符冲突，如“:D”等表情符。建议在使用html代码时勾选“禁用表情”。\" />\r\n        </p>\r\n		<p class=\"mbn\"><input type=\"checkbox\" id=\"allowimgcode\" disabled");
	if (allowimg==1)
	{

	templateBuilder.Append(" checked=\"checked\"");
	}	//end if

	templateBuilder.Append(" /><label for=\"allowimgcode\">[img] 代码</label></p>\r\n		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"parseurloff\" id=\"parseurloff\" ");
	if (parseurloff==1)
	{

	templateBuilder.Append("checked");
	}	//end if

	templateBuilder.Append(" /><label for=\"parseurloff\">禁用 网址自动识别</label></p>\r\n		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"smileyoff\" id=\"smileyoff\" ");
	if (smileyoff==1)
	{

	templateBuilder.Append("checked");
	}	//end if


	if (forum.Allowsmilies!=1)
	{

	templateBuilder.Append("disabled");
	}	//end if

	templateBuilder.Append(" /><label for=\"smileyoff\">禁用 表情</label></p>\r\n		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"bbcodeoff\" id=\"bbcodeoff\" ");
	if (bbcodeoff==1)
	{

	templateBuilder.Append(" checked");
	}	//end if


	if (usergroupinfo.Allowcusbbcode!=1)
	{

	templateBuilder.Append(" disabled");
	}
	else if (forum.Allowbbcode!=1)
	{

	templateBuilder.Append(" disabled");
	}	//end if

	templateBuilder.Append(" /><label for=\"bbcodeoff\">禁用 论坛代码</label></p>\r\n		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"usesig\" id=\"usesig\" ");
	if (usesig==1)
	{

	templateBuilder.Append("checked");
	}	//end if

	templateBuilder.Append(" /><label for=\"usesig\">使用个人签名</label></p>\r\n		");
	if (pagename=="postreply.aspx")
	{

	templateBuilder.Append("\r\n		<p class=\"mbn\"><input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" ");
	if (config.Replyemailstatus==1)
	{

	templateBuilder.Append(" checked");
	}	//end if

	templateBuilder.Append(" /><label for=\"emailnotify\">发送邮件告知楼主</label></p>\r\n		<p class=\"mbn\"><input type=\"checkbox\" name=\"postreplynotice\" id=\"postreplynotice\" ");
	if (config.Replynotificationstatus==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/><label for=\"emailnotify\">发送论坛通知给楼主</label></p>\r\n		");
	}	//end if


	if (pagename=="posttopic.aspx" || (pagename=="editpost.aspx"&&isfirstpost))
	{


	if (special==""&&Scoresets.GetCreditsTrans()!=0 && usergroupinfo.Maxprice>0)
	{

	templateBuilder.Append("\r\n			<p style=\"clear:both;\"><strong>售价</strong>(");
	templateBuilder.Append(userextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append("):</p>\r\n			<p><input type=\"text\" name=\"topicprice\" value=\"");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("\" class=\"txt\"  size=\"6\"/> ");
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append(" <br/>最高 ");
	templateBuilder.Append(usergroupinfo.Maxprice.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("售价只允许非负整数, 单个主题最大收入 ");
	templateBuilder.Append(Scoresets.GetMaxIncPerTopic().ToString().Trim());
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("</p>\r\n		");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n</div>");


	if (postinfo.Layer==0)
	{

	templateBuilder.Append("\r\n	<div style=\"display: none;\" id=\"specialposttable\">\r\n	");
	if (topic.Special==1)
	{

	templateBuilder.Append("\r\n	<div class=\"exfm cl\">\r\n		<div class=\"sinf z\">\r\n			<div class=\"cl\">\r\n				<h4><em>选项:</em>每行填写 1 个选项</h4>\r\n			</div>\r\n			<div id=\"polloptions\" class=\"mbm\">\r\n			<input id=\"PollItemname\" type=\"hidden\" name=\"PollItemname\" value=\"\" />\r\n			<input id=\"PollOptionDisplayOrder\" type=\"hidden\" name=\"PollOptionDisplayOrder\" value=\"\" />\r\n			<input id=\"PollOptionID\" type=\"hidden\" name=\"PollOptionID\" value=\"\" />\r\n            <ul>\r\n                <li>序号</li>\r\n                <li>投票项</li>\r\n            </ul>\r\n			");
	int poll__loop__id=0;
	foreach(DataRow poll in polloptionlist.Rows)
	{
		poll__loop__id++;

	templateBuilder.Append("\r\n				<p ");
	if (poll__loop__id==1)
	{

	templateBuilder.Append("id=\"divPollItem\" ");
	}	//end if

	templateBuilder.Append(" name=\"PollItem\">\r\n					<input type=\"hidden\" name=\"optionid\" value=\"" + poll["polloptionid"].ToString().Trim() + "\">\r\n					<input type=\"text\" class=\"txt\" style=\"margin-right:2px\" size=\"2\" name=\"displayorder\" maxlength=\"4\" value=\"" + poll["displayorder"].ToString().Trim() + "\" tabindex=\"1\" />\r\n					<input type=\"text\" name=\"pollitemid\" value=\"" + poll["name"].ToString().Trim() + "\" class=\"optioninfo txt\" tabindex=\"1\" />\r\n					<a href=\"javascript:;\" class=\"del y\" title=\"删除投票项\" onclick=\"if(!delObj(document.getElementById('polloptions'),2,this.parentNode)){alert('投票项不能少于2个');}\">del</a>\r\n				</p>\r\n			");
	}	//end loop

	templateBuilder.Append("\r\n			</div>\r\n			<p><a onclick=\"clonePoll('");
	templateBuilder.Append(config.Maxpolloptions.ToString().Trim());
	templateBuilder.Append("')\" href=\"javascript:;\">+增加投票项</a><a onclick=\"if(!delObj(document.getElementById('polloptions'), 2)){alert('投票项不能少于2个');}\" href=\"javascript:;\">删除投票项</a></p>\r\n		</div>\r\n		<div class=\"sadd z\">\r\n			<p class=\"mbn\"><label for=\"polldatas\">投票结束日期</label>\r\n				<input name=\"enddatetime\" type=\"text\" id=\"enddatetime\" class=\"txt\" size=\"10\" value=\"");
	templateBuilder.Append(pollinfo.Expiration.ToString().Trim());
	templateBuilder.Append("\" style=\"cursor: default\" onclick=\"showcalendar(event, 'enddatetime', 'cal_startdate', 'cal_enddate', '");
	templateBuilder.Append(nowdate.ToString());
	templateBuilder.Append("');\" readonly=\"readonly\" />\r\n				<input type=\"hidden\" name=\"cal_startdate\" id=\"cal_startdate\" size=\"10\" value=\"");
	templateBuilder.Append(nowdate.ToString());
	templateBuilder.Append("\">\r\n				<input type=\"hidden\" name=\"cal_enddate\" id=\"cal_enddate\" size=\"10\" value=\"\">\r\n			</p>\r\n			<p class=\"mbn\"><label for=\"pollnum\">\r\n				<input ");
	if (pollinfo.Multiple==1)
	{

	templateBuilder.Append("checked=\"checked\" ");
	}	//end if

	templateBuilder.Append(" type=\"checkbox\" name=\"multiple\"\r\n				onclick=\"this.checked?$('maxchoicescontrol').style.display='':$('maxchoicescontrol').style.display='none';\" />多选投票\r\n				</label>\r\n				<span id=\"maxchoicescontrol\" ");
	if (pollinfo.Multiple==0)
	{

	templateBuilder.Append("style=\"display: none;\"");
	}	//end if

	templateBuilder.Append(">最多可选项数: <input type=\"text\" tabindex=\"1\" value=\"");
	templateBuilder.Append(pollinfo.Maxchoices.ToString().Trim());
	templateBuilder.Append("\" class=\"spshortinput txt\" size=\"8\" name=\"maxchoices\"/></span>\r\n			</p>\r\n			<p class=\"mbn\">\r\n				<input name=\"updatepoll\" type=\"hidden\" id=\"updatepoll\" value=\"1\" />\r\n				<input type=\"checkbox\" name=\"visiblepoll\" ");
	if (pollinfo.Visible==1)
	{

	templateBuilder.Append("checked=\"checked\"");
	}	//end if

	templateBuilder.Append(" />提交投票后结果才可见\r\n			</p>\r\n			<p class=\"mbn\"><input type=\"checkbox\" tabindex=\"1\" name=\"allowview\" ");
	if (pollinfo.Allowview==1)
	{

	templateBuilder.Append("checked=\"checked\"");
	}	//end if

	templateBuilder.Append(" />公开投票参与人</p>\r\n		</div>\r\n	</div>\r\n	");
	}	//end if


	if (topic.Special==2)
	{

	templateBuilder.Append("\r\n	<div class=\"exfm cl\">\r\n		<label for=\"rewardprice\">悬赏价格:</label>\r\n		<input name=\"topicprice\" type=\"text\" class=\"txt\" id=\"topicprice\" value=\"");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("\" size=\"5\" maxlength=\"5\" onkeyup=\"getrealprice(this.value);\"/>\r\n		");
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(userextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append("\r\n		[ 悬赏范围 ");
	templateBuilder.Append(usergroupinfo.Minbonusprice.ToString().Trim());
	templateBuilder.Append(" - ");
	templateBuilder.Append(usergroupinfo.Maxbonusprice.ToString().Trim());
	templateBuilder.Append("  \r\n		");
	templateBuilder.Append(bonusextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("\r\n		");
	templateBuilder.Append(bonusextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(", 当前可用 ");
	templateBuilder.Append(mybonustranscredits.ToString());
	templateBuilder.Append(" ");
	templateBuilder.Append(bonusextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append(bonusextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append("]\r\n		[ 税后支付 <span id=\"realprice\">0</span>]\r\n		<script type=\"text/javascript\" reload=\"1\">\r\n			function getrealprice(price) {\r\n				if (!price.search(/^\\d+$/)) {\r\n					n = Math.ceil(parseInt(price) + price * ");
	templateBuilder.Append(Scoresets.GetCreditsTax().ToString().Trim());
	templateBuilder.Append(");\r\n					if (price > 32767) {\r\n						$('realprice').innerHTML = '<b>悬赏不能高于 32767</b>';\r\n					} else if (price < ");
	templateBuilder.Append(usergroupinfo.Minbonusprice.ToString().Trim());
	templateBuilder.Append(" || (price > ");
	templateBuilder.Append(usergroupinfo.Maxbonusprice.ToString().Trim());
	templateBuilder.Append(")) {\r\n						$('realprice').innerHTML = '<b>悬赏超出范围</b>';\r\n					} else {\r\n						$('realprice').innerHTML = n;\r\n					}\r\n				} else {\r\n					$('realprice').innerHTML = '<b>填写无效</b>';\r\n				}\r\n			}\r\n		</");
	templateBuilder.Append("script>\r\n	</div>\r\n	");
	}
	else if (topic.Special==3)
	{

	templateBuilder.Append("\r\n	<div class=\"exfm cl\">\r\n		<label for=\"rewardprice\">悬赏价格:</label>\r\n		<input name=\"topicprice\" type=\"hidden\" id=\"topicprice\" value=\"");
	templateBuilder.Append(usergroupinfo.Minbonusprice.ToString().Trim());
	templateBuilder.Append("\" />\r\n		");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(userextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" (只允许正整数)<span class=\"xg2\">已经结帖无法修改悬赏金额</span>\r\n	</div>\r\n	");
	}
	else if (topic.Special==4)
	{

	templateBuilder.Append("\r\n		<div class=\"exfm cl\">\r\n			<div class=\"sinf z\">\r\n				<dl>\r\n					<dt><strong class=\"rq\">*</strong><label for=\"affirmpoint\">正方:</label></dt>\r\n					<dd><textarea tabindex=\"1\" class=\"txtarea\" id=\"positiveopinion\" name=\"positiveopinion\" style=\"width: 210px;\" >");
	templateBuilder.Append(debateinfo.Positiveopinion.ToString().Trim());
	templateBuilder.Append("</textarea></dd>\r\n					<dt><strong class=\"rq\">*</strong><label for=\"negapoint\">反方:</label></dt>				\r\n					<dd><textarea tabindex=\"1\" class=\"txtarea\" id=\"negativeopinion\" name=\"negativeopinion\" style=\"width: 210px;\" >");
	templateBuilder.Append(debateinfo.Negativeopinion.ToString().Trim());
	templateBuilder.Append("</textarea></dd>\r\n				</dl>\r\n			</div>\r\n			<div class=\"sadd\">\r\n				<label for=\"endtime\">结束时间:</label>\r\n				<p>\r\n					<input type=\"text\" name=\"terminaltime\" id=\"terminaltime\" style=\"cursor:default;\" class=\"txt\" size=\"16\" value=\"");
	templateBuilder.Append(FormatDateTimeString(debateinfo.Terminaltime).ToString().Trim());
	templateBuilder.Append("\" onclick=\"showcalendar(event, 'terminaltime', 'cal_startdate', 'cal_enddate', '");
	templateBuilder.Append(debateinfo.Terminaltime.ToString().Trim());
	templateBuilder.Append("');\" readonly=\"readonly\" />\r\n					<input type=\"hidden\" name=\"cal_startdate\" id=\"cal_startdate\" value=\"");
	templateBuilder.Append(FormatDateTimeString(debateinfo.Terminaltime).ToString().Trim());
	templateBuilder.Append("\" />\r\n					<input type=\"hidden\" name=\"cal_enddate\" id=\"cal_enddate\" value=\"\" />\r\n					<input type=\"hidden\" name=\"updatedebate\" id=\"updatedebate\" value=\"1\" />\r\n				</p>\r\n			</div>\r\n		</div>\r\n		<script type=\"text/javascript\">\r\n		    function doadvdebate() {\r\n		        var adv_open = $('advdebate_open');\r\n		        var adv_close = $('advdebate_close');\r\n		        if (adv_open && adv_close) {\r\n		            if (adv_open.style.display != 'none') {\r\n		                adv_open.style.display = 'none';\r\n		                adv_close.style.display = '';\r\n		            }\r\n		            else {\r\n		                adv_open.style.display = '';\r\n		                adv_close.style.display = 'none';\r\n		            }\r\n		        }\r\n		    }\r\n		</");
	templateBuilder.Append("script>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n    <input type=\"hidden\" name=\"aid\" id=\"aid\" value=\"0\">\r\n    <input type=\"hidden\" name=\"isdeleteatt\" id=\"isdeleteatt\" value=\"0\">\r\n    <p class=\"textmsg\" id=\"divshowuploadmsg\" style=\"display: none\"></p>\r\n    <p class=\"textmsg succ\" id=\"divshowuploadmsgok\" style=\"display: none\"></p>\r\n    <input type=\"hidden\" name=\"uploadallowmax\" value=\"10\">\r\n    <input type=\"hidden\" name=\"uploadallowtype\" value=\"jpg,gif\">\r\n    <input type=\"hidden\" name=\"thumbwidth\" value=\"300\">\r\n    <input type=\"hidden\" name=\"thumbheight\" value=\"250\">\r\n    <input type=\"hidden\" name=\"noinsert\" value=\"0\">\r\n	<script type=\"text/javascript\">\r\n		isfirstpost  = ");
	templateBuilder.Append(postinfo.Layer.ToString().Trim());
	templateBuilder.Append(" == 0 ? 1 : 0;\r\n		$('postform').onsubmit = function() { return validate($('postform'));};\r\n		function deleteatt(aid){\r\n			document.getElementById('isdeleteatt').value = 1;\r\n			document.getElementById('aid').value = aid;\r\n			document.getElementById('isdeleteatt').form.submit();\r\n		}\r\n    </");
	templateBuilder.Append("script>\r\n    </form>\r\n");
	templateBuilder.Append("            <div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_local\" style=\"display: none;\">\r\n				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n					<tbody id=\"imgattachbodyhidden\" style=\"display:none\"><tr>\r\n						<td class=\"atnu\"><span id=\"imglocalno[]\"><img src=\"images/attachicons/common_new.gif\" /></span></td>\r\n						<td class=\"atna\">\r\n							<span id=\"imgdeschidden[]\" style=\"display:none\">\r\n								<span id=\"imglocalfile[]\"></span>\r\n							</span>\r\n							<input type=\"hidden\" name=\"imglocalid[]\" />\r\n						</td>\r\n						<td class=\"attc delete_msg\"><span id=\"imgcpdel[]\"></span></td>\r\n					</tr></tbody>\r\n				</table>\r\n				<div class=\"p_tbl\"><table cellpadding=\"0\" cellspacing=\"0\" summary=\"post_attachbody\" border=\"0\" width=\"100%\"><tbody id=\"imgattachbody\"></tbody></table></div>\r\n				<div class=\"upbk\">\r\n					<div id=\"imgattachbtnhidden\" style=\"display:none\"><span><form name=\"imgattachform\" id=\"imgattachform\" method=\"post\" autocomplete=\"off\" action=\"tools/attachupload.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" target=\"attachframe\" enctype=\"multipart/form-data\"><input type=\"file\" name=\"Filedata\" size=\"45\" class=\"filedata\" /></form></span></div>\r\n					<div id=\"imgattachbtn\"></div>\r\n					<p id=\"imguploadbtn\">						\r\n						<button class=\"pn\" type=\"button\" onclick=\"hideMenu();\"><span>取消</span></button>\r\n						<button class=\"pn pnc\" type=\"button\" onclick=\"uploadAttach(0, 0, 'img')\"><span>上传</span></button>\r\n					</p>\r\n					<p id=\"imguploading\" style=\"display: none;\">上传中</p>\r\n				</div>\r\n				<div class=\"notice upnf\">\r\n					文件尺寸: <strong>");
	templateBuilder.Append(FormatBytes(usergroupinfo.Maxattachsize).ToString().Trim());
	templateBuilder.Append("</strong>&nbsp;\r\n					上传限制: <strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong> 个文件&nbsp;&nbsp;\r\n					<br />可用扩展名: <strong>");
	templateBuilder.Append(attachextensionsnosize.ToString());
	templateBuilder.Append("</strong>&nbsp;\r\n				</div>\r\n			</div>\r\n	<iframe name=\"attachframe\" id=\"attachframe\" style=\"display: none;\" onload=\"uploadNextAttach();\"></iframe>\r\n\r\n<script type=\"text/javascript\"  reload=\"1\">\r\n     //获取silverlight插件已经上传的附件列表  //sl上传完返回\r\n    function getAttachmentList(sender, args) {\r\n        var attachment = args.AttchmentList;\r\n        if (isUndefined(attachment) || attachment == '[]') {\r\n            if (infloat == 1) {\r\n                pagescrolls('swfreturn'); return false;\r\n            }\r\n            else { swfuploadwin(); return; }\r\n        }\r\n        var attachmentList = eval(\"(\" + attachment + \")\");\r\n        switchAttachbutton('attachlist');\r\n        updateAttachList();\r\n       \r\n    }\r\n\r\n    function onLoad(plugin, userContext, sender) {\r\n        //只读属性,标识 Silverlight 插件是否已经加载。\r\n        //if (sender.getHost().IsLoaded) {\r\n        $(\"MultiUploadFile\").content.JavaScriptObject.UploadAttchmentList = getAttachmentList;\r\n        // }\r\n    }\r\n\r\n</");
	templateBuilder.Append("script>\r\n<div class=\"p_pof upf\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attach_menu\" style=\"display: none;width:600px;\" unselectable=\"on\">\r\n		<span class=\"y\"><a href=\"javascript:;\" class=\"flbc\" onclick=\"hideMenu()\">关闭</a></span>\r\n		<ul class=\"imguptype\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attach_ctrl\">\r\n			<li><a href=\"javascript:;\" hidefocus=\"true\" class=\"current\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_btn_attachlist\" onclick=\"switchAttachbutton('attachlist');\">附件列表</a></li>\r\n			<li><a href=\"javascript:;\" hidefocus=\"true\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_btn_upload\" onclick=\"switchAttachbutton('upload');\">普通上传</a></li>\r\n			<li><a href=\"javascript:;\" hidefocus=\"true\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_btn_swfupload\" onclick=\"switchAttachbutton('swfupload');\">批量上传</a></li>\r\n		</ul>\r\n			<div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_upload\" style=\"display: none;\">\r\n				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n					<tbody id=\"attachbodyhidden\" style=\"display:none\"><tr>\r\n						<td class=\"atnu\"><span id=\"localno[]\"><img src=\"images/attachicons/common_new.gif\" /></span></td>\r\n						<td class=\"atna\">\r\n							<span id=\"deschidden[]\" style=\"display:none\">\r\n								<span id=\"localfile[]\"></span>\r\n							</span>\r\n							<input type=\"hidden\" name=\"localid\" />\r\n						</td>\r\n						<td class=\"attc delete_msg\"><span id=\"cpdel[]\"></span></td>\r\n					</tr></tbody>\r\n				</table>\r\n				<div class=\"p_tbl\"><table cellpadding=\"0\" cellspacing=\"0\" summary=\"post_attachbody\" border=\"0\" width=\"100%\"><tbody id=\"attachbody\"></tbody></table></div>\r\n				<div class=\"upbk\">\r\n					<div id=\"attachbtnhidden\" style=\"display:none\"><span><form name=\"attachform\" id=\"attachform\" method=\"post\" autocomplete=\"off\" action=\"tools/attachupload.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" target=\"attachframe\" enctype=\"multipart/form-data\"><input type=\"hidden\" name=\"uid\" value=\"$_G['uid']\"><input type=\"hidden\" name=\"hash\" value=\"{echo md5(substr(md5($_G['config']['security']['authkey']), 8).$_G['uid'])}\"><input type=\"file\" name=\"Filedata\" size=\"45\" class=\"fldt\" /></form></span></div>\r\n					<div id=\"attachbtn\"></div>\r\n					<p id=\"uploadbtn\">\r\n						<button type=\"button\" class=\"pn\" onclick=\"hideMenu();\"><span>取消</span></button>\r\n						<button type=\"button\" class=\"pn pnc\" onclick=\"uploadAttach(0, 0)\"><span>上传</span></button>\r\n					</p>\r\n					<p id=\"uploading\" style=\"display: none;\"><img src=\"images/common/uploading.gif\" style=\"vertical-align: middle;\" /> 上传中，请稍候，您可以<a href=\"javascript:;\" onclick=\"hideMenu()\">暂时关闭这个小窗口</a>，上传完成后您会收到通知。</p>\r\n				</div>\r\n				<div class=\"notice upnf\">\r\n					文件尺寸: <strong>");
	templateBuilder.Append(FormatBytes(usergroupinfo.Maxattachsize).ToString().Trim());
	templateBuilder.Append("</strong>&nbsp;\r\n					上传限制: <strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong> 个文件&nbsp;&nbsp;\r\n					<br />可用扩展名: <strong>");
	templateBuilder.Append(attachextensionsnosize.ToString());
	templateBuilder.Append("</strong>&nbsp;\r\n				</div>				\r\n			</div>\r\n			<div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_swfupload\" style=\"display: none;\">\r\n				<div class=\"floatboxswf\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_multiattach\">\r\n			    ");
	if (config.Swfupload==1)
	{

	templateBuilder.Append("\r\n			    <embed width=\"470\" height=\"268\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" allowscriptaccess=\"always\" menu=\"false\" quality=\"high\" src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("images/common/upload.swf?site=");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/attachupload.aspx%3fmod=swfupload%26forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&amp;random=");
	templateBuilder.Append(DateTime.Now.Ticks.ToString().Trim());
	templateBuilder.Append("\">\r\n			    ");
	}
	else
	{


						string authToken=Discuz.Common.DES.Encode(oluserinfo.Olid.ToString() + "," + oluserinfo.Username.ToString(), oluserinfo.Password.Substring(0, 10)).Replace("+", "[");
						

	if (pagename.IndexOf("goods")<0 && config.Silverlight==1)
	{

	templateBuilder.Append("\r\n					<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/uploadfile/silverlight.js\" reload=\"1\"></");
	templateBuilder.Append("script> \r\n					<div id=\"swfbox\"> \r\n					<object  id=\"MultiUploadFile\" data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" Width=\"100%\" Height=\"340\">\r\n					<param name=\"source\" value=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/UploadFile/ClientBin/MultiFileUpload.xap\"/>\r\n					<param name=\"onError\" value=\"onSilverlightError\" />\r\n					<param name=\"onLoad\" value=\"onLoad\" />\r\n					<param name=\"background\" value=\"aliceblue\" />\r\n					<param name=\"minRuntimeVersion\" value=\"4.0.50401.0\" />\r\n					<param name=\"autoUpgrade\" value=\"true\" />\r\n					<param name=\"initParams\" value=\"forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(",authToken=");
	templateBuilder.Append(authToken.ToString());
	templateBuilder.Append(",max=");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("\" />		  \r\n					<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0\" style=\"text-decoration:none\" target=\"_blank\">\r\n					<img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/uploadfile/uploadfile.jpg\" alt=\"安装微软Silverlight控件,即刻使用批量上传附件\" style=\"border-style:none\"/>\r\n					</a>\r\n					</object></div>\r\n					");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n				<div class=\"notice upnf\">\r\n					文件尺寸: <strong>");
	templateBuilder.Append(FormatBytes(usergroupinfo.Maxattachsize).ToString().Trim());
	templateBuilder.Append("</strong>&nbsp;\r\n					上传限制: <strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong> 个文件&nbsp;&nbsp;\r\n					<br />可用扩展名: <strong>");
	templateBuilder.Append(attachextensionsnosize.ToString());
	templateBuilder.Append("</strong>&nbsp;\r\n				</div>\r\n			</div>\r\n		<div class=\"p_opt post_tablelist\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attachlist\">\r\n				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" id=\"attachlist_edittablist\">\r\n					<tbody>\r\n					    <tr>\r\n						<td class=\"atnu\">&nbsp;</td>\r\n						<td class=\"atna\">文件名&nbsp;(<a class=\"xg2\" href=\"javascript:;\" onclick=\"insertAllAttachTag();return false;\" style=\"margin:0 4px;\">插入全部附件</a>)</td>\r\n						<td class=\"atds\">描述</td>\r\n						");
	if (userid!=-1 && usergroupinfo.Allowsetattachperm==1)
	{

	templateBuilder.Append("<td class=\"attv\">阅读权限</td>");
	}	//end if


	if (topicattachscorefield>0 && usergroupinfo.Maxprice>0)
	{

	templateBuilder.Append("<td class=\"attp\">");
	templateBuilder.Append(Scoresets.GetTopicAttachCreditsTransName().ToString().Trim());
	templateBuilder.Append("</td>");
	}	//end if

	templateBuilder.Append("\r\n						<td class=\"attc delete_msg\"></td>\r\n					   </tr>\r\n					</tbody>\r\n					");
	if (action=="edit")
	{


	int attachment__loop__id=0;
	foreach(DataRow attachment in attachmentlist.Rows)
	{
		attachment__loop__id++;


	if (Utils.StrToInt(attachment["pid"].ToString().Trim(), 0)==postinfo.Pid && attachment["isimage"].ToString().Trim()=="0")
	{

	string filetypeimage = "";
	
	int isimage = 0;
	
	string inserttype = "";
	

	if (attachment["filetype"].ToString().Trim().IndexOf("image")>-1)
	{

	 filetypeimage = "image.gif";
	
	 inserttype = "insertAttachimgTag";
	
	 isimage = 1;
	

	}
	else
	{

	 inserttype = "insertAttachTag";
	

	if (Utils.GetFileExtName(attachment["attachment"].ToString().Trim())=="rar" || Utils.GetFileExtName(attachment["attachment"].ToString().Trim())=="zip")
	{

	 filetypeimage = "rar.gif";
	

	}
	else
	{

	 filetypeimage = "attachment.gif";
	

	}	//end if


	}	//end if

	templateBuilder.Append("		\r\n					        <tbody id=\"attach_" + attachment["aid"].ToString().Trim() + "\">\r\n					        <tr>\r\n					        <td class=\"atnu\">\r\n					        <img id=\"attach" + attachment["aid"].ToString().Trim() + "_type\" border=\"0\" src=\"images/attachicons/");
	templateBuilder.Append(filetypeimage.ToString());
	templateBuilder.Append("\" class=\"vm\" alt=\"\"/>\r\n					        </td>\r\n					        <td class=\"atna\">\r\n					        <span id=\"attach" + attachment["aid"].ToString().Trim() + "\">\r\n					        <a id=\"attachname" + attachment["aid"].ToString().Trim() + "\" onclick=\"");
	templateBuilder.Append(inserttype.ToString());
	templateBuilder.Append("(" + attachment["aid"].ToString().Trim() + ")\" href=\"javascript:;\" isimage=\"");
	templateBuilder.Append(isimage.ToString());
	templateBuilder.Append("\" title=\"" + attachment["attachment"].ToString().Trim() + "\">");	templateBuilder.Append(Utils.GetUnicodeSubString(attachment["attachment"].ToString().Trim(),25,"..."));
	templateBuilder.Append("</a> \r\n 					        <a href=\"javascript:;\" class=\"atturl\" title=\"添加附件地址\" onclick=\"insertText('attach://')\">\r\n					        <img alt=\"\" src=\"images/attachicons/attachurl.gif\"/>\r\n					        </a>\r\n					        </span>\r\n					        <span id=\"attachupdate" + attachment["aid"].ToString().Trim() + "\" style=\"display:none;\">\r\n					        <form enctype=\"multipart/form-data\" target=\"attachframe\" action=\"tools/attachupload.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&aid=" + attachment["aid"].ToString().Trim() + "\" method=\"post\" id=\"attachform_" + attachment["aid"].ToString().Trim() + "\" name=\"attachform_" + attachment["aid"].ToString().Trim() + "\" style=\"float:left;\">\r\n					            <input type=\"file\" name=\"Filedata\" size=\"8\" />\r\n					            <input type=\"hidden\" value=\"" + attachment["aid"].ToString().Trim() + "\" name=\"attachupdatedid\" />\r\n					            <input type=\"submit\" value=\"上传\" />\r\n					        </form>\r\n					        </span>\r\n					        <a id=\"attach" + attachment["aid"].ToString().Trim() + "_opt\" href=\"javascript:;\" class=\"right\" onclick=\"attachupdate('" + attachment["aid"].ToString().Trim() + "', this)\">更新</a>\r\n					        <input type=\"hidden\" value=\"" + attachment["aid"].ToString().Trim() + "\" name=\"attachid\" />\r\n                            ");
	if (isimage==1)
	{

	string attachkey = Thumbnail.GetKey(TypeConverter.StrToInt(attachment["aid"].ToString().Trim()));
	
	templateBuilder.Append("\r\n					            <img src=\"tools/ajax.aspx?t=image&aid=" + attachment["aid"].ToString().Trim() + "&size=300x300&key=");
	templateBuilder.Append(attachkey.ToString());
	templateBuilder.Append("&nocache=yes&type=fixnone\" id=\"image_" + attachment["aid"].ToString().Trim() + "\" cwidth=\"" + attachment["width"].ToString().Trim() + "\" style=\"position: absolute; top: -10000px;\"/>\r\n                            ");
	}	//end if

	templateBuilder.Append("\r\n                            <script type=\"text/javascript\">ATTACHNUM['attachused']++;</");
	templateBuilder.Append("script>\r\n					        </td>\r\n					        <td class=\"atds\"><input type=\"text\" name=\"attachdesc_" + attachment["aid"].ToString().Trim() + "\" size=\"18\" class=\"txt\" value=\"" + attachment["description"].ToString().Trim() + "\"/></td>\r\n					        <td class=\"attv\">\r\n                                <select id=\"readperm_" + attachment["aid"].ToString().Trim() + "\" onchange=\"$('readperm_hidden_" + attachment["aid"].ToString().Trim() + "').value = this.value;\" size=\"1\">\r\n                                    <option value=\"\">不限</option>\r\n                                </select>\r\n                                <script type=\"text/javascript\">getreadpermoption($('readperm_" + attachment["aid"].ToString().Trim() + "'), " + attachment["readperm"].ToString().Trim() + ");</");
	templateBuilder.Append("script>\r\n                                <input type=\"hidden\" id=\"readperm_hidden_" + attachment["aid"].ToString().Trim() + "\" value=\"" + attachment["readperm"].ToString().Trim() + "\" name=\"readperm_" + attachment["aid"].ToString().Trim() + "\"/>\r\n                            </td>\r\n					        <td class=\"attp\"><input type=\"text\" size=\"1\" value=\"" + attachment["attachprice"].ToString().Trim() + "\" name=\"attachprice_" + attachment["aid"].ToString().Trim() + "\"/></td>\r\n					        <td class=\"attp\"><input type=\"text\" size=\"1\" value=\"" + attachment["attachprice"].ToString().Trim() + "\" name=\"attachprice_" + attachment["aid"].ToString().Trim() + "\"/></td>\r\n					        <td class=\"attc delete_msg\"><a href=\"javascript:;\" class=\"d\" onclick=\"delAttach('" + attachment["aid"].ToString().Trim() + ",");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("',0)\">删除</a></td>\r\n					        </tr></tbody>\r\n						   ");
	}	//end if


	}	//end loop


	}	//end if

	templateBuilder.Append("\r\n\r\n				</table>\r\n				<div id=\"attachlist_tablist_current\"></div>\r\n				<div id=\"attachlist_tablist\"></div>\r\n				<p class=\"ptm\" id=\"attach_notice\" style=\"display: none\" >点击文件名插入到帖子内容中</p>\r\n\r\n				");
	if (infloat==0)
	{

	templateBuilder.Append("\r\n				<div id=\"uploadlist\" class=\"upfilelist\" style=\"height:auto\">\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				<div id=\"uploadlist\" class=\"upfilelist\">\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n					");
	if (pagename.IndexOf("goods")<0 && config.Silverlight==1)
	{

	templateBuilder.Append("\r\n					<tbody id=\"attachuploadedhidden\" style=\"display:none\"><tr>\r\n						<td class=\"attachnum\"><span id=\"sl_localno[]\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("images/attachicons/common_new.gif\" /></span></td>\r\n						<td class=\"attachctrl\"><span id=\"sl_cpadd[]\"></span></td>\r\n						<td class=\"attachname\">\r\n							<span id=\"sl_deschidden[]\" style=\"display:none\">\r\n								<a href=\"javascript:;\" onclick='parentNode.innerHTML=\"<input type=\\\"text\\\" name=\\\"attachdesc\\\" size=\\\"25\\\" class=\\\"txt\\\" />\"'>描述</a>\r\n								<span id=\"attachfile[]\"></span>\r\n								<input type=\"text\" name=\"sl_attachdesc\" style=\"display:none\" />\r\n							</span>\r\n						</td>\r\n						");
	if (userid!=-1 && usergroupinfo.Allowsetattachperm==1)
	{

	templateBuilder.Append("<td class=\"attachview\"><input type=\"text\" name=\"sl_readperm\" value=\"0\"size=\"1\" class=\"txt\" /></td>");
	}	//end if


	if (topicattachscorefield>0 && usergroupinfo.Maxprice>0)
	{

	templateBuilder.Append("<td class=\"attachpr\"><input type=\"text\" name=\"sl_attachprice\" value=\"0\" size=\"1\" class=\"txt\" /></td>");
	}	//end if


	if (config.Enablealbum==1 && caninsertalbum)
	{

	templateBuilder.Append("\r\n							<td  style=\"vertical-align:top;\">\r\n								<select name=\"sl_albums\" style=\"display:none\">\r\n								<option value=\"0\"></option>\r\n								");
	int album__loop__id=0;
	foreach(DataRow album in albumlist.Rows)
	{
		album__loop__id++;

	templateBuilder.Append("\r\n								<option value=\"" + album["albumid"].ToString().Trim() + "\">" + album["title"].ToString().Trim() + "</option>\r\n								");
	}	//end loop

	templateBuilder.Append("\r\n								</select>\r\n							</td>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n						<td class=\"attachdel\"><span id=\"sl_cpdel[]\"></span></td>\r\n					</tr></tbody>\r\n\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<tbody id=\"attachbodyhidden\" style=\"display:none\"><tr>\r\n						<td class=\"attachnum\"><span id=\"localno[]\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("images/attachicons/common_new.gif\" /></span></td>\r\n						<td class=\"attachctrl\"><span id=\"cpadd[]\"></span></td>\r\n						<td class=\"attachname\">\r\n							<span id=\"deschidden[]\" style=\"display:none\">\r\n								<a href=\"javascript:;\" onclick='parentNode.innerHTML=\"<input type=\\\"text\\\" name=\\\"attachdesc\\\" size=\\\"25\\\" class=\\\"txt\\\" />\"'>描述</a>\r\n								<span id=\"localfile[]\"></span>\r\n							</span>\r\n							<input type=\"hidden\" name=\"localid\" />\r\n						</td>\r\n\r\n						");
	if (config.Enablealbum==1 && caninsertalbum)
	{

	templateBuilder.Append("\r\n							<td  style=\"vertical-align:top;\">\r\n							");
	if (albumlist.Rows.Count!=0)
	{

	templateBuilder.Append("\r\n								<select name=\"albums\"  style=\"display:none\">\r\n								<option value=\"0\"></option>\r\n								");
	int album__loop__id=0;
	foreach(DataRow album in albumlist.Rows)
	{
		album__loop__id++;

	templateBuilder.Append("\r\n								<option value=\"" + album["albumid"].ToString().Trim() + "\">" + album["title"].ToString().Trim() + "</option>\r\n								");
	}	//end loop

	templateBuilder.Append("\r\n								</select>\r\n							");
	}	//end if

	templateBuilder.Append("\r\n							</td>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n						<td class=\"attachdel\"><span id=\"cpdel[]\"></span></td>\r\n					</tr></tbody>\r\n				</table>\r\n				");
	if (pagename.IndexOf("goods")<0 && config.Silverlight==1)
	{

	templateBuilder.Append("\r\n				<div id=\"swfattachlist\">\r\n					<table cellspacing=\"0\" cellpadding=\"0\" id=\"attachuploadednote\" style=\"display:none;\">\r\n						<tbody>\r\n							<tr>\r\n								<td class=\"attachnum\"></td>\r\n								<td>您有 <span id=\"attachuploadednotenum\"></span> 个已经上传的附件<span id=\"maxattachnote\" style=\"display: none;\">, 只能使用前<span id=\"num2upload2\"><strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong></span>个</span>  \r\n								<a onclick=\"addAttachUploaded(attaches);\" href=\"javascript:;\">使用</a>   <a onclick=\"attachlist()\" href=\"javascript:;\">忽略</a>\r\n								</td>\r\n							</tr>\r\n						</tbody>\r\n					</table>\r\n				</div>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<table cellpadding=\"0\" cellspacing=\"0\" summary=\"post_attachbody\" border=\"0\" width=\"100%\"><tbody id=\"attachuploaded\"></tbody><tbody id=\"attachbody\"></tbody></table>\r\n			</div>\r\n		</div>\r\n<div id=\"img_hidden\" alt=\"1\" style=\"position:absolute;top:-100000px;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image');width:");
	templateBuilder.Append(thumbwidth.ToString());
	templateBuilder.Append("px;height:");
	templateBuilder.Append(thumbheight.ToString());
	templateBuilder.Append("px\"></div>		</div>\r\n	</div>\r\n<script type=\"text/javascript\">\r\n	var editorform = $('testform');\r\n	var editorsubmit = $('testsubmit');\r\n	if (wysiwyg) {\r\n	    newEditor(1, bbcode2html(textobj.value));\r\n	} else {\r\n	    newEditor(0, textobj.value);\r\n	}\r\n	if (getQueryString('cedit') == 'yes') {\r\n	    loadData(true, '");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("');\r\n	}\r\n</");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\nfunction switchImagebutton(btn) {\r\n        var btns = ['www', 'imgattachlist'];\r\n        btns.push('multi'); switchButton(btn, btns);\r\n        $(editorid + '_image_menu').style.height = '';\r\n    }\r\n\r\n\r\n/*function switchImagebutton(btn) {\r\nvar btns = ['www', 'albumlist'];\r\nswitchButton(btn, btns);\r\n$(editorid + '_image_menu').style.height = '';\r\n}*/\r\n\r\nfunction switchAttachbutton(btn) {\r\nvar btns = ['attachlist'];\r\nbtns.push('upload');btns.push('swfupload');switchButton(btn, btns);\r\n}\r\n\r\n");
	if (action=="edit")
	{

	templateBuilder.Append("\r\n    ATTACHNUM['imageused'] = uploadedimagelist?uploadedimagelist.length:ATTACHNUM['imageused'];//更新已使用图片的数量\r\n");
	}	//end if

	templateBuilder.Append("\r\nupdateattachnum('attach');\r\nupdateattachnum('image');\r\n\r\n");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\nupdateAttachList();\r\nupdateImageList();\r\n");
	}	//end if

	templateBuilder.Append("\r\n\r\nif(ATTACHNUM['attachused'] + ATTACHNUM['attachunused']<=0)\r\n    switchAttachbutton('swfupload');\r\nelse\r\n    switchAttachbutton('attachlist');\r\nsetCaretAtEnd();\r\n\r\nif(!$('usedimgattachlist').childNodes.length && !$('unusedimgattachlist').childNodes.length)\r\n    switchImagebutton('multi');\r\nif(BROWSER.ie >= 5 || BROWSER.firefox >= '2') {\r\n    _attachEvent(window, 'beforeunload',function on(){ saveData(undefined, 'postform','");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("')});\r\n}\r\n");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\ngetunusedattachlist_dialog();\r\n");
	}	//end if

	templateBuilder.Append("\r\naddAttach();\r\n</");
	templateBuilder.Append("script>");


	}	//end if


	}
	else
	{


	if (ispost)
	{


	            string backLink = HttpContext.Current.Request.UrlReferrer.ToString();
	            SetBackLink(backLink.Contains("&cedit=yes") ? backLink : backLink + "&cedit=yes");
	        

	}	//end if


	templateBuilder.Append("<div class=\"wrap cl\">\r\n<div class=\"main\">\r\n	<div class=\"msgbox\">\r\n		<h1>出现了");
	templateBuilder.Append(page_err.ToString());
	templateBuilder.Append("个错误</h1>\r\n		<hr class=\"solidline\"/>\r\n		<div class=\"msg_inner error_msg\">\r\n			<p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n			<p class=\"errorback\">\r\n				<script type=\"text/javascript\">\r\n					if(");
	templateBuilder.Append(msgbox_showbacklink.ToString());
	templateBuilder.Append(")\r\n					{\r\n						document.write(\"<a href=\\\"");
	templateBuilder.Append(msgbox_backlink.ToString());
	templateBuilder.Append("\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n					}\r\n				</");
	templateBuilder.Append("script>\r\n				<a href=\"forumindex.aspx\">论坛首页</a>\r\n				");
	if (usergroupid==7)
	{

	templateBuilder.Append("\r\n				 &nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"login.aspx\">登录</a>&nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</p>\r\n		</div>\r\n	</div>\r\n</div>\r\n</div>");


	}	//end if



	if (infloat!=1)
	{


	if (pagename=="website.aspx")
	{

	templateBuilder.Append("    \r\n       <div id=\"websitebottomad\"></div>\r\n");
	}
	else if (footerad!="")
	{

	templateBuilder.Append(" \r\n     <div id=\"ad_footerbanner\">");
	templateBuilder.Append(footerad.ToString());
	templateBuilder.Append("</div>   \r\n");
	}	//end if

	templateBuilder.Append("\r\n<div id=\"footer\">\r\n	<div class=\"wrap\"  id=\"wp\">\r\n		<div id=\"footlinks\">\r\n			<p><a href=\"");
	templateBuilder.Append(config.Weburl.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(config.Webtitle.ToString().Trim());
	templateBuilder.Append("</a> - ");
	templateBuilder.Append(config.Linktext.ToString().Trim());
	templateBuilder.Append(" - <a target=\"_blank\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("stats.aspx\">统计</a> - ");
	if (config.Sitemapstatus==1)
	{

	templateBuilder.Append("&nbsp;<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("tools/sitemap.aspx\" target=\"_blank\" title=\"百度论坛收录协议\">Sitemap</a>");
	}	//end if

	templateBuilder.Append("\r\n			");
	templateBuilder.Append(config.Statcode.ToString().Trim());
	templateBuilder.Append(config.Icp.ToString().Trim());
	templateBuilder.Append("\r\n			</p>\r\n			<div>\r\n				<a href=\"http://www.comsenz.com/\" target=\"_blank\">Comsenz Technology Ltd</a>\r\n				- <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("archiver/index.aspx\" target=\"_blank\">简洁版本</a>\r\n			");
	if (config.Stylejump==1)
	{


	if (userid!=-1 || config.Guestcachepagetimeout<=0)
	{

	templateBuilder.Append("\r\n				- <span id=\"styleswitcher\" class=\"drop\" onmouseover=\"showMenu({'ctrlid':this.id, 'pos':'21'})\" onclick=\"window.location.href='");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtemplate.aspx'\">界面风格</span>\r\n				");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n		</div>\r\n		<a title=\"Powered by Discuz!NT\" target=\"_blank\" href=\"http://nt.discuz.net\"><img border=\"0\" alt=\"Discuz!NT\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/discuznt_logo.gif\"/></a>\r\n		<p id=\"copyright\">\r\n			Powered by <strong><a href=\"http://nt.discuz.net\" target=\"_blank\" title=\"Discuz!NT\">Discuz!NT</a></strong> <em class=\"f_bold\">3.6.601</em>\r\n			");
	if (config.Licensed==1)
	{

	templateBuilder.Append("\r\n				(<a href=\"\" onclick=\"this.href='http://nt.discuz.net/certificate/?host='+location.href.substring(0, location.href.lastIndexOf('/'))\" target=\"_blank\">Licensed</a>)\r\n			");
	}	//end if

	templateBuilder.Append("\r\n				");
	templateBuilder.Append(config.Forumcopyright.ToString().Trim());
	templateBuilder.Append("\r\n		</p>\r\n		<p id=\"debuginfo\" class=\"grayfont\">\r\n		");
	if (config.Debug!=0)
	{

	templateBuilder.Append("\r\n			Processed in ");
	templateBuilder.Append(this.Processtime.ToString().Trim());
	templateBuilder.Append(" second(s)\r\n			");
	if (isguestcachepage==1)
	{

	templateBuilder.Append("\r\n				(Cached).\r\n			");
	}
	else if (querycount>1)
	{

	templateBuilder.Append("\r\n				 , ");
	templateBuilder.Append(querycount.ToString());
	templateBuilder.Append(" queries.\r\n			");
	}
	else
	{

	templateBuilder.Append("\r\n				 , ");
	templateBuilder.Append(querycount.ToString());
	templateBuilder.Append(" query.\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		</p>\r\n	</div>\r\n</div>\r\n<a id=\"scrolltop\" href=\"javascript:;\" style=\"display:none;\" class=\"scrolltop\" onclick=\"setScrollToTop(this.id);\">TOP</a>\r\n<ul id=\"usercenter_menu\" class=\"p_pop\" style=\"display:none;\">\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpprofile.aspx?action=avatar\">设置头像</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpprofile.aspx\">个人资料</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpnewpassword.aspx\">更改密码</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\">用户组</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpsubscribe.aspx\">收藏夹</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpcreditspay.aspx\">积分</a></li>\r\n</ul>\r\n\r\n");
	int prentid__loop__id=0;
	foreach(string prentid in mainnavigationhassub)
	{
		prentid__loop__id++;

	templateBuilder.Append("\r\n<ul class=\"p_pop\" id=\"menu_");
	templateBuilder.Append(prentid.ToString());
	templateBuilder.Append("_menu\" style=\"display: none\">\r\n");
	int subnav__loop__id=0;
	foreach(DataRow subnav in subnavigation.Rows)
	{
		subnav__loop__id++;

	bool isoutput = false;
	

	if (subnav["parentid"].ToString().Trim()==prentid)
	{


	if (subnav["level"].ToString().Trim()=="0")
	{

	 isoutput = true;
	

	}
	else
	{


	if (subnav["level"].ToString().Trim()=="1" && userid!=-1)
	{

	 isoutput = true;
	

	}
	else
	{

	bool leveluseradmindi = true;
	
	 leveluseradmindi = (useradminid==3 || useradminid==1 || useradminid==2);
	

	if (subnav["level"].ToString().Trim()=="2" &&  leveluseradmindi)
	{

	 isoutput = true;
	

	}	//end if


	if (subnav["level"].ToString().Trim()=="3" && useradminid==1)
	{

	 isoutput = true;
	

	}	//end if


	}	//end if


	}	//end if


	}	//end if


	if (isoutput)
	{


	if (subnav["id"].ToString().Trim()=="11" || subnav["id"].ToString().Trim()=="12")
	{


	if (config.Statstatus==1)
	{

	templateBuilder.Append("\r\n	" + subnav["nav"].ToString().Trim() + "\r\n        ");	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="18")
	{


	if (config.Oltimespan>0)
	{

	templateBuilder.Append("\r\n    " + subnav["nav"].ToString().Trim() + "\r\n	");	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="24")
	{


	if (config.Enablespace==1)
	{

	templateBuilder.Append("\r\n    " + subnav["nav"].ToString().Trim() + "\r\n 	");	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="25")
	{


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n    " + subnav["nav"].ToString().Trim() + "\r\n 	");	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="26")
	{


	if (config.Enablemall>=1)
	{

	templateBuilder.Append("\r\n    " + subnav["nav"].ToString().Trim() + "\r\n   	");	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n    " + subnav["nav"].ToString().Trim() + "\r\n");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n</ul>\r\n");
	}	//end loop


	if (config.Stylejump==1)
	{


	if (userid!=-1 || config.Guestcachepagetimeout<=0)
	{

	templateBuilder.Append("\r\n	<ul id=\"styleswitcher_menu\" class=\"popupmenu_popup s_clear\" style=\"display: none;\">\r\n	");
	templateBuilder.Append(templatelistboxoptions.ToString());
	templateBuilder.Append("\r\n	</ul>\r\n	");
	}	//end if


	}	//end if




	templateBuilder.Append("</body>\r\n</html>\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n]]></root>\r\n");
	}	//end if




	Response.Write(templateBuilder.ToString());
}
</script>
