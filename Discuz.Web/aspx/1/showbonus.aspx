<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.showbonus" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011-6-13 14:18:39.
		本页面代码由Discuz!NT模板引擎生成于 2011-6-13 14:18:39. 
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



	templateBuilder.Append("\r\n<script type=\"text/javascript\">\r\nvar templatepath = \"");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("\";\r\nvar postminchars = parseInt(");
	templateBuilder.Append(config.Minpostsize.ToString().Trim());
	templateBuilder.Append(");\r\nvar postmaxchars = parseInt(");
	templateBuilder.Append(config.Maxpostsize.ToString().Trim());
	templateBuilder.Append(");\r\nvar disablepostctrl = parseInt(");
	templateBuilder.Append(disablepostctrl.ToString());
	templateBuilder.Append(");\r\nvar forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\nvar imagedir = \"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("\";\r\n</");
	templateBuilder.Append("script>\r\n");
	if (enabletag)
	{

	templateBuilder.Append("\r\n<script type=\"text/javascript\" src=\"cache/tag/closedtags.txt\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"cache/tag/colorfultags.txt\"></");
	templateBuilder.Append("script>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_showtopic.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n");
	if (page_err==0)
	{

	templateBuilder.Append("\r\n<div class=\"wrap cl pageinfo\">\r\n	<div id=\"nav\">\r\n	");
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

	templateBuilder.Append("\r\n	<a id=\"forumlist\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\" ");
	if (config.Forumjump==1)
	{

	templateBuilder.Append("onmouseover=\"showMenu(this.id);\" onmouseout=\"showMenu(this.id);\"");
	}	//end if

	templateBuilder.Append(" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a>  &raquo; ");
	templateBuilder.Append(ShowForumAspxRewrite(forum.Pathlist.Trim(),forumid,forumpageid).ToString().Trim());
	templateBuilder.Append("\r\n	");	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("\r\n		  &raquo; <strong>");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</strong>\r\n	");
	}
	else
	{

	templateBuilder.Append("\r\n		  &raquo; <strong>");
	templateBuilder.Append(topic.Title.Trim().ToString().Trim());
	templateBuilder.Append("</strong>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n</div>\r\n<div class=\"wrap cl\">\r\n");	int loopi = 1;
	
	int valuablepostcount = 0;
	
	int valuelesspostcount = 0;
	

	int post__loop__id=0;
	foreach(ShowbonusPagePostInfo post in postlist)
	{
		post__loop__id++;


	if (post.Id!=1 && post.Isbest==1)
	{

	 valuablepostcount = valuablepostcount+1;
	

	}	//end if


	if (post.Id!=1 && post.Isbest==0)
	{

	 valuelesspostcount = valuelesspostcount+1;
	

	}	//end if


	if (post.Id==1)
	{

	templateBuilder.Append("\r\n<div class=\"main viewthread\">\r\n	<div id=\"postsContainer\">\r\n	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"悬赏主题\">\r\n		<tr>\r\n		<td class=\"postauthor\">\r\n			");
	if (post.Posterid!=-1)
	{

	templateBuilder.Append("\r\n			<!-- member menu -->\r\n			<div class=\"popupmenu_popup userinfopanel\" id=\"");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("\" style=\"display:none; clip: rect(auto auto auto auto); position: absolute;\" initialized ctrlkey=\"userinfo2\">\r\n				<div class=\"popavatar\">\r\n					<div id=\"");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("_ma\"></div>\r\n					<ul class=\"profile_side\">\r\n						<li class=\"post_pm\"><a href=\"usercppostpm.aspx?msgtoid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_postpm', this.href, 600, 410, '600,0');doane(event);\" target=\"_blank\">发送短消息</a></li>\r\n					</ul>\r\n				</div>\r\n				<div class=\"popuserinfo\">\r\n					<dl class=\"cl\">\r\n						<dt>UID</dt><dd>");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("</dd>\r\n						<dt>精华</dt><dd>");
	if (post.Digestposts>0)
	{

	templateBuilder.Append("<a href=\"search.aspx?posterid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("&type=digest\">");
	templateBuilder.Append(post.Digestposts.ToString().Trim());
	templateBuilder.Append("</a>");
	}
	else
	{
	templateBuilder.Append(post.Digestposts.ToString().Trim());
	}	//end if

	templateBuilder.Append("</dd>\r\n					");
	if (score[1].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[1].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits1.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[1].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[2].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[2].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits2.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[2].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[3].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[3].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits3.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[3].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[4].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[4].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits4.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[4].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[5].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[5].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits5.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[5].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[6].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[6].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits6.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[6].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[7].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[7].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits7.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[7].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[8].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[8].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits8.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[8].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (post.Location!="")
	{

	templateBuilder.Append("\r\n						<dt>来自</dt><dd>");
	templateBuilder.Append(post.Location.ToString().Trim());
	templateBuilder.Append("</dd>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					</dl>\r\n					<div class=\"imicons cl\">\r\n						");
	if (post.Msn!="")
	{

	templateBuilder.Append("\r\n						<a href=\"mailto:");
	templateBuilder.Append(post.Msn.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\" class=\"msn\">");
	templateBuilder.Append(post.Msn.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (post.Skype!="")
	{

	templateBuilder.Append("\r\n						<a href=\"skype:");
	templateBuilder.Append(post.Skype.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\" class=\"skype\">");
	templateBuilder.Append(post.Skype.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (post.Icq!="")
	{

	templateBuilder.Append("\r\n						<a href=\"http://wwp.icq.com/scripts/search.dll?to=");
	templateBuilder.Append(post.Icq.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\" class=\"icq\">");
	templateBuilder.Append(post.Icq.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (post.Qq!="")
	{

	templateBuilder.Append("\r\n						<a href=\"http://wpa.qq.com/msgrd?V=1&Uin=");
	templateBuilder.Append(post.Qq.ToString().Trim());
	templateBuilder.Append("&Site=");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("&Menu=yes\" target=\"_blank\" class=\"qq\">");
	templateBuilder.Append(post.Qq.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (post.Yahoo!="")
	{

	templateBuilder.Append("\r\n						<a href=\"http://edit.yahoo.com/config/send_webmesg?.target=");
	templateBuilder.Append(post.Yahoo.ToString().Trim());
	templateBuilder.Append("&.src=pg\" target=\"_blank\" class=\"yahoo\">");
	templateBuilder.Append(post.Yahoo.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</div>\r\n					<div class=\"imicons cl\">\r\n						");	 aspxrewriteurl = this.UserInfoAspxRewrite(post.Posterid);
	
	templateBuilder.Append("\r\n						<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"public_info\">查看公共资料</a>\r\n						<a href=\"search.aspx?posterid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\" class=\"all_topic\">搜索帖子</a>\r\n					");
	if (useradminid>0)
	{


	if (admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n						<a onclick=\"floatwin('open_getip', this.href, 400, 170, '600,0');doane(event);\"  href=\"getip.aspx?pid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" title=\"查看IP\" class=\"ip\">查看IP</a>\r\n					");
	}	//end if


	if (admininfo.Allowbanuser==1)
	{

	templateBuilder.Append("\r\n						<a href=\"useradmin.aspx?action=banuser&uid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_mods', this.href, 250, 270, '600,0');doane(event);\" title=\"禁止用户\" class=\"forbid_user\">禁止用户</a>\r\n					");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n			</div>\r\n			<!-- member menu -->\r\n			");
	}	//end if


	if (post.Posterid!=-1)
	{

	templateBuilder.Append("\r\n			<div class=\"poster\">\r\n				<span  ");
	if (post.Onlinestate==1)
	{

	templateBuilder.Append("class=\"onlineyes\" title=\"在线\"");
	}
	else
	{

	templateBuilder.Append("class=\"onlineno\" title=\"未在线\"");
	}	//end if

	templateBuilder.Append(">");
	templateBuilder.Append(post.Poster.ToString().Trim());
	templateBuilder.Append("</span>\r\n			</div>\r\n			<div id=\"");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("_a\">\r\n			");
	if (config.Showavatars==1)
	{

	templateBuilder.Append("\r\n			<div class=\"avatar\">\r\n			");	string avatarurl = Avatars.GetAvatarUrl(post.Posterid);
	
	templateBuilder.Append("\r\n				<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_medium.gif';\"  alt=\"头像\" onmouseover=\"showauthor(this,");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append(")\"/>\r\n			</div>\r\n			");
	}	//end if


	if (post.Nickname!="")
	{

	templateBuilder.Append("\r\n			<p>");
	templateBuilder.Append(post.Nickname.ToString().Trim());
	templateBuilder.Append("</p>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			<p>\r\n			<script type=\"text/javascript\">\r\n				ShowStars(");
	templateBuilder.Append(post.Stars.ToString().Trim());
	templateBuilder.Append(", ");
	templateBuilder.Append(config.Starthreshold.ToString().Trim());
	templateBuilder.Append(");\r\n			</");
	templateBuilder.Append("script>\r\n			</p>\r\n			<ul class=\"otherinfo\">\r\n				");
	if (config.Userstatusby==1)
	{

	templateBuilder.Append("\r\n				<li><label>组别</label>");
	templateBuilder.Append(post.Status.ToString().Trim());
	templateBuilder.Append("</li>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<li><label>性别</label><script type=\"text/javascript\">document.write(displayGender(");
	templateBuilder.Append(post.Gender.ToString().Trim());
	templateBuilder.Append("));</");
	templateBuilder.Append("script></li>\r\n				");
	if (post.Bday!="")
	{

	templateBuilder.Append("\r\n				<li><label>生日</label>");
	templateBuilder.Append(post.Bday.ToString().Trim());
	templateBuilder.Append("</li>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<li><label>积分</label>");
	templateBuilder.Append(post.Credits.ToString().Trim());
	templateBuilder.Append("</li>\r\n				<li><label>帖子</label>");
	templateBuilder.Append(post.Posts.ToString().Trim());
	templateBuilder.Append("</li>\r\n				");
	if (post.Joindate!="")
	{

	templateBuilder.Append("						\r\n				<li><label>注册时间</label>");	templateBuilder.Append(Convert.ToDateTime(post.Joindate).ToString("yyyy-MM-dd"));
	templateBuilder.Append("</li>\r\n				");
	}	//end if

	templateBuilder.Append("	\r\n			</ul>\r\n			");
	if (config.Enablespace==1 || config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n			<ul class=\"plug\">\r\n				");
	if (config.Enablespace==1)
	{

	templateBuilder.Append("\r\n				<li class=\"space\">\r\n				");
	if (post.Spaceid>0)
	{

	templateBuilder.Append("<a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/?uid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\">个人空间</a>");
	}
	else
	{

	templateBuilder.Append("<a href=\"###\" onclick=\"nospace('");
	templateBuilder.Append(post.Poster.ToString().Trim());
	templateBuilder.Append("');\">个人空间</a>");
	}	//end if

	templateBuilder.Append("\r\n				</li>\r\n				");
	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n				<li class=\"album\"><a href=\"showalbumlist.aspx?uid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\">相册</a></li>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</ul>\r\n			");
	}	//end if


	if (post.Medals!="")
	{

	templateBuilder.Append("\r\n				<div class=\"medals\">");
	templateBuilder.Append(post.Medals.ToString().Trim());
	templateBuilder.Append("</div>\r\n			");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n			<div style=\"padding-left:15px;\">\r\n			    <em>");
	templateBuilder.Append(post.Poster.ToString().Trim());
	templateBuilder.Append("-");
	templateBuilder.Append(post.Ip.ToString().Trim());
	templateBuilder.Append("</em>\r\n				");
	if (useradminid>0 && admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n				<a href=\"getip.aspx?pid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" onclick=\"floatwin('open_getip', this.href, 400, 170, '600,0');doane(event);\" title=\"查看IP\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/ip.gif\" alt=\"查看IP\"/></a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			<p><em>未注册</em></p>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n		<td class=\"postcontent\">\r\n			<div class=\"topictitle\">\r\n				<h1>\r\n				");
	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{

	templateBuilder.Append("\r\n					<cite>");
	templateBuilder.Append(topictypes.ToString());
	templateBuilder.Append("</cite>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				");
	templateBuilder.Append(post.Title.ToString().Trim());
	templateBuilder.Append("\r\n				");	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	
	templateBuilder.Append("\r\n				<span><a title=\"点击查看原始版本\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">返回悬赏主题</a></span><span><em>已解决</em> - <a href=\"#bestpost\">最佳答案</a> 悬赏价格: 金钱 <em>");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("</em></span>\r\n				</h1>\r\n			</div>\r\n			<div class=\"pi\">\r\n				<div class=\"postinfo\">\r\n					");	String olimg = OnlineUsers.GetGroupImg(post.Groupid);
	
	templateBuilder.Append("\r\n					");
	templateBuilder.Append(olimg.ToString());
	templateBuilder.Append("\r\n					<em>\r\n					发表于");	templateBuilder.Append(Convert.ToDateTime(post.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
	templateBuilder.Append("\r\n					</em>\r\n				</div>\r\n			</div>\r\n			<div class=\"postmessage defaultpost\">\r\n				");
	if (enabletag)
	{

	templateBuilder.Append("				\r\n				<script type=\"text/javascript\">\r\n					function forumhottag_callback(data)\r\n					{\r\n						tags = data;\r\n					}\r\n				</");
	templateBuilder.Append("script>\r\n				<script type=\"text/javascript\" src=\"cache/hottags_forum_cache_jsonp.txt\"></");
	templateBuilder.Append("script>\r\n				<div id=\"topictag\">\r\n					");	int hastag = Topics.GetMagicValue(topic.Magic, MagicType.TopicTag);
	

	if (hastag==1)
	{

	templateBuilder.Append("\r\n						<script type=\"text/javascript\">getTopicTags(");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(");</");
	templateBuilder.Append("script>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n						<script type=\"text/javascript\">parsetag();</");
	templateBuilder.Append("script>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<div class=\"t_msgfont\">\r\n					<div id=\"firstpost\">\r\n						<h5>补充资料</h5>\r\n						");
	templateBuilder.Append(post.Message.ToString().Trim());
	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n				<div class=\"bonusrate cl\">\r\n					<h5>本帖得分:</h5>\r\n					<div class=\"attachmentinfo\">\r\n						");
	int bonuslog__loop__id=0;
	foreach(BonusLogInfo bonuslog in bonuslogs)
	{
		bonuslog__loop__id++;


	if (bonuslog.Bonus>0)
	{


	if (bonuslog__loop__id!=1)
	{

	templateBuilder.Append("\r\n								,\r\n							");
	}	//end if

	 aspxrewriteurl = this.UserInfoAspxRewrite(bonuslog.Answerid);
	
	string unit = scoreunit[ bonuslog.Extid ];
	
	string name = score[ bonuslog.Extid ];
	
	templateBuilder.Append("\r\n							<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(bonuslog.Answername.ToString().Trim());
	templateBuilder.Append("</a>(");
	templateBuilder.Append(name.ToString());
	templateBuilder.Append(":");
	templateBuilder.Append(bonuslog.Bonus.ToString().Trim());
	templateBuilder.Append(unit.ToString());
	templateBuilder.Append(")\r\n						    ");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n			</div>\r\n		</td>\r\n		</tr>\r\n		<tr>\r\n		<td class=\"postauthor\">&nbsp;</td>\r\n		<td class=\"postactions\">\r\n			<div class=\"p_control\">\r\n				<cite class=\"y\">\r\n					");
	if (userid!=-1)
	{


	templateBuilder.Append("<script type=\"text/javascript\">\r\n    show_report_button(");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(",");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(",");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append(");\r\n</");
	templateBuilder.Append("script>");

	templateBuilder.Append("<span class=\"pipe\">|</span>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<a href=\"#\" onclick=\"window.scrollTo(0,0)\">TOP</a>\r\n				</cite>\r\n			");
	if (ismoder==1)
	{


	if (post.Posterid!=-1)
	{

	templateBuilder.Append("\r\n				<cite class=\"y\">\r\n					<a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("');\">评分</a>\r\n					");
	if (post.Ratetimes>0)
	{

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"###\" onclick=\"action_onchange('cancelrate',$('moderate'),'");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("');\">撤销评分</a>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n				</cite>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n				<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\">删除</a>\r\n			");
	}
	else
	{


	if (usergroupinfo.Raterange!="" && post.Posterid!=-1)
	{

	templateBuilder.Append("<cite class=\"y\"><a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("');\">评分</a></cite>");
	}	//end if


	if (post.Posterid!=-1 && userid==post.Posterid)
	{


	if (topic.Closed==0)
	{

	templateBuilder.Append("\r\n						<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\">删除</a>\r\n				");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n		</td>\r\n		</tr>\r\n		<tbody>\r\n		<tr class=\"threadad\">\r\n			<td class=\"postauthor\"></td>\r\n			<td class=\"adcontent\"></td>\r\n		</tr>\r\n		</tbody>\r\n		</table>\r\n		</div>\r\n		<div class=\"forumcontrol cl\">\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"narrow\">\r\n			<tbody>\r\n			<tr>\r\n			<td class=\"postauthor\">\r\n				<a href=\"showtopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&go=prev\">上一主题</a><span class=\"pipe\">|</span>\r\n				<a href=\"showtopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&go=next\">下一主题</a>\r\n			</td>\r\n			<td class=\"modaction\">\r\n				");
	if (useradminid>0||usergroupinfo.Raterange!=""||config.Forumjump==1||(topic.Special==2&&topic.Posterid==userid))
	{

	templateBuilder.Append("\r\n				<script type=\"text/javascript\">\r\n					function action_onchange(value,objfrm,postid,banstatus){\r\n						if (value != ''){\r\n							objfrm.operat.value = value;\r\n							objfrm.postid.value = postid;\r\n							if (value != \"delete\")\r\n							{\r\n								objfrm.action = objfrm.action + '&referer=' + escape(window.location);\r\n							}\r\n							if (value == 'banpost' && typeof(banstatus) != \"undefined\")\r\n							{\r\n								objfrm.operat.value = value;\r\n								objfrm.action = objfrm.action + \"&banstatus=\" + banstatus;\r\n								objfrm.submit();\r\n								return;\r\n							}\r\n							if(value == 'delposts' || value == 'banpost'){\r\n								$('postsform').operat.value = value; \r\n								$('postsform').action = $('postsform').action + '&referer=' + escape(window.location);\r\n								$('postsform').submit();\r\n							}\r\n							else{\r\n								objfrm.submit();\r\n							}\r\n						}\r\n					}\r\n				</");
	templateBuilder.Append("script>\r\n				");	bool canuseadminfunc = usergroupinfo.Raterange!="" || usergroupinfo.Maxprice>0 || (topic.Special==2&&topic.Posterid==userid);
	

	if (useradminid>0)
	{

	templateBuilder.Append("\r\n					<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"topicadmin.aspx?action=moderate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&infloat=1\">\r\n						<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n						<input name=\"topicid\" type=\"hidden\" value=\"");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" />\r\n						<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n						<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n						<input type=\"hidden\" name=\"winheight\" />\r\n						<input type=\"hidden\" name=\"optgroup\" />\r\n						");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n						<span class=\"drop xg2\" onclick=\"showMenu({'ctrlid':this.id, 'pos':'21'})\" id=\"operatSel\">主题管理</span>\r\n						<ul style=\"width: 180px; display:none;\" id=\"operatSel_menu\" class=\"p_pop inlinelist\">\r\n							<li><a onclick=\"modthreads(1, 'delete');return false;\" href=\"###\">删除</a></li>\r\n							<li><a onclick=\"modthreads(1, 'bump');return false;\" href=\"###\">提沉</a></li>\r\n							<li><a onclick=\"modthreads(1, 'close');return false;\" href=\"###\">关闭</a></li>\r\n							<li><a onclick=\"modthreads(1, 'move');return false;\" href=\"###\">移动</a></li>\r\n							<li><a onclick=\"modthreads(1, 'copy');return false;\" href=\"###\">复制</a></li>\r\n							<li><a onclick=\"modthreads(1, 'highlight');return false;\" href=\"###\">高亮</a></li>\r\n							<li><a onclick=\"modthreads(1, 'digest');return false;\" href=\"###\">精华</a></li>\r\n							<li><a onclick=\"modthreads(1, 'identify');return false;\" href=\"###\">鉴定</a></li>\r\n							<li><a onclick=\"modthreads(1, 'displayorder');return false;\" href=\"###\">置顶</a></li>\r\n							<li><a onclick=\"modthreads(1, 'split');return false;\" href=\"###\">分割</a></li>\r\n							<li><a onclick=\"modthreads(1, 'merge');return false;\" href=\"###\">合并</a></li>\r\n							<li><a onclick=\"modthreads(1, 'repair');return false;\" href=\"###\">修复</a></li>\r\n							<li><a onclick=\"modthreads(1, 'type');return false;\" href=\"###\">分类</a></li>\r\n						</ul>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</form>\r\n				");
	}
	else if (canuseadminfunc)
	{

	templateBuilder.Append("\r\n					<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"topicadmin.aspx?action=moderate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\"  class=\"y\">\r\n						<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n						<input name=\"topicid\" type=\"hidden\" value=\"");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" />\r\n						<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n						<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n					</form>\r\n				");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</td>\r\n			</tr>\r\n			</tbody>\r\n		</table>\r\n		</div>\r\n		</div>\r\n	");
	}
	else if (post.Isbest==2)
	{

	templateBuilder.Append("\r\n		<div class=\"main\">\r\n			<h1><a name=\"bestpost\"></a>最佳答案</h1>\r\n			<div class=\"pi\">\r\n				<div class=\"postinfo bonus\">\r\n					<cite class=\"y\">\r\n					");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n						<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n						<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\">删除</a>\r\n					");
	}
	else
	{


	if (post.Posterid!=-1 && userid==post.Posterid)
	{


	if (topic.Closed==0)
	{

	templateBuilder.Append("\r\n								<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n							");
	}	//end if

	templateBuilder.Append("\r\n							<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n						");
	}	//end if


	}	//end if


	if (canreply)
	{

	templateBuilder.Append("\r\n						<a href=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&quote=yes\" class=\"repquote\">引用</a>\r\n						");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n							<a href=\"###\" onclick=\"replyToFloor('");
	templateBuilder.Append(post.Id.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(post.Poster.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("')\" class=\"fastreply\">回复</a>\r\n						");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n					</cite>\r\n					");	 aspxrewriteurl = this.UserInfoAspxRewrite(post.Posterid);
	
	templateBuilder.Append("\r\n					由<a id=\"memberinfo_");
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" style=\"padding:0px 4px;\" class=\"xg2\">");
	templateBuilder.Append(post.Poster.ToString().Trim());
	templateBuilder.Append("</a>发表于");	templateBuilder.Append(Convert.ToDateTime(post.Postdatetime).ToString("yyyy-MM-dd hh:mm"));
	templateBuilder.Append("\r\n				</div>\r\n			</div>\r\n			<div class=\"postmessage\">\r\n				<div class=\"t_msgfont\">");
	templateBuilder.Append(post.Message.ToString().Trim());
	templateBuilder.Append("</div>\r\n			</div>\r\n		</div>\r\n	");
	}	//end if

	 loopi = loopi+1;
	

	}	//end loop


	if (postlist.Count>=2)
	{


	if (valuablepostcount!=0)
	{

	templateBuilder.Append("\r\n<div class=\"main\">\r\n	<h4>有价值的答案</h4>\r\n");
	int valuablepost__loop__id=0;
	foreach(ShowbonusPagePostInfo valuablepost in postlist)
	{
		valuablepost__loop__id++;


	if (valuablepost.Id!=1 && valuablepost.Isbest==1)
	{

	templateBuilder.Append("\r\n	<div class=\"pi\">\r\n		<div class=\"postinfo bonus\">\r\n			<cite class=\"y\">\r\n			");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n				<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valuablepost.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n				<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valuablepost.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\">删除</a>\r\n			");
	}
	else
	{


	if (valuablepost.Posterid!=-1 && userid==valuablepost.Posterid)
	{


	if (topic.Closed==0)
	{

	templateBuilder.Append("\r\n						<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valuablepost.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valuablepost.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\"  class=\"delpost\">删除</a>\r\n				");
	}	//end if


	}	//end if


	if (canreply)
	{

	templateBuilder.Append("\r\n				<a href=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valuablepost.Pid.ToString().Trim());
	templateBuilder.Append("&quote=yes\" class=\"repquote\">引用</a>\r\n				");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n					<a href=\"###\" onclick=\"replyToFloor('");
	templateBuilder.Append(valuablepost.Id.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(valuablepost.Poster.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(valuablepost.Pid.ToString().Trim());
	templateBuilder.Append("')\" class=\"fastreply\">回复</a>\r\n				");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</cite>\r\n			");	 aspxrewriteurl = this.UserInfoAspxRewrite(valuablepost.Posterid);
	
	templateBuilder.Append("\r\n			由<a id=\"memberinfo_");
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" onmouseover=\"showMenu(this.id,false)\" class=\"drop xg2\"  style=\"padding:0 14px 0 4px;\">");
	templateBuilder.Append(valuablepost.Poster.ToString().Trim());
	templateBuilder.Append("</a>发表于");	templateBuilder.Append(Convert.ToDateTime(valuablepost.Postdatetime).ToString("yyyy-MM-dd hh:mm"));
	templateBuilder.Append("\r\n		</div>\r\n	</div>\r\n	<div class=\"postmessage\">\r\n		<div class=\"t_msgfont\">");
	templateBuilder.Append(valuablepost.Message.ToString().Trim());
	templateBuilder.Append("</div>\r\n	</div>\r\n		");
	if (valuablepost.Posterid!=-1)
	{

	templateBuilder.Append("\r\n		<!-- member menu -->\r\n		<div class=\"popupmenu_popup userinfopanel\" id=\"memberinfo_");
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("_menu\" style=\"display: none; z-index: 50; filter: progid:dximagetransform.microsoft.shadow(direction=135,color=#cccccc,strength=2); left: 19px; clip: rect(auto auto auto auto); position: absolute; top:253px; width:150px; padding:10px;\" initialized ctrlkey=\"userinfo2\" h=\"209\">\r\n			<ul class=\"panelinfo\">\r\n				<li><a href=\"usercppostpm.aspx?msgtoid=");
	templateBuilder.Append(valuablepost.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_postpm', this.href, 600, 410, '600,0');doane(event);\" target=\"_blank\">发送短消息</a></li>\r\n			");
	if (useradminid>0)
	{


	if (admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n				<li><a href=\"getip.aspx?pid=");
	templateBuilder.Append(valuablepost.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" onclick=\"floatwin('open_getip', this.href, 400, 170, '600,0');doane(event);\" title=\"查看IP\">查看IP</a></li>\r\n			");
	}	//end if


	if (admininfo.Allowbanuser==1)
	{

	templateBuilder.Append("\r\n				<li><a href=\"useradmin.aspx?action=banuser&uid=");
	templateBuilder.Append(valuablepost.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_mods', this.href, 250, 270, '600,0');doane(event);\" title=\"禁止用户\">禁止用户</a></li>\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			<li>\r\n				");	 aspxrewriteurl = this.UserInfoAspxRewrite(valuablepost.Posterid);
	
	templateBuilder.Append("\r\n				<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">查看公共资料</a>\r\n			</li>\r\n			<li><a href=\"search.aspx?posterid=");
	templateBuilder.Append(valuablepost.Posterid.ToString().Trim());
	templateBuilder.Append("\">查找该会员全部帖子</a></li>\r\n			</ul>\r\n			<ul class=\"userdetail\">\r\n				<li><label>UID</label>");
	templateBuilder.Append(valuablepost.Posterid.ToString().Trim());
	templateBuilder.Append("</li>\r\n				<li><label>精华</label>\r\n				");
	if (valuablepost.Digestposts>0)
	{

	templateBuilder.Append("<a href=\"search.aspx?posterid=");
	templateBuilder.Append(valuablepost.Posterid.ToString().Trim());
	templateBuilder.Append("&type=digest\">");
	templateBuilder.Append(valuablepost.Digestposts.ToString().Trim());
	templateBuilder.Append("</a>");
	}
	else
	{
	templateBuilder.Append(valuablepost.Digestposts.ToString().Trim());
	}	//end if

	templateBuilder.Append("</li>\r\n			");
	if (score[1].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[1].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits1.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[1].ToString().Trim() + "</li>\r\n			");
	}	//end if


	if (score[2].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[2].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits2.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[2].ToString().Trim() + "</li>\r\n			");
	}	//end if


	if (score[3].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[3].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits3.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[3].ToString().Trim() + "</li>\r\n			");
	}	//end if


	if (score[4].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[4].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits4.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[4].ToString().Trim() + "</li>\r\n			");
	}	//end if


	if (score[5].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[5].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits5.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[5].ToString().Trim() + "</li>\r\n			");
	}	//end if


	if (score[6].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[6].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits6.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[6].ToString().Trim() + "</li>\r\n			");
	}	//end if


	if (score[7].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[7].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits7.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[7].ToString().Trim() + "</li>\r\n			");
	}	//end if


	if (score[8].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n				<li><label>" + score[8].ToString().Trim() + "</label>");
	templateBuilder.Append(valuablepost.Extcredits8.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[8].ToString().Trim() + "</li>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n				<li><label>状态</label>\r\n				");
	if (valuablepost.Onlinestate==1)
	{

	templateBuilder.Append("\r\n				在线\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				离线\r\n				");
	}	//end if

	templateBuilder.Append("</li>\r\n			</ul>\r\n			<ul class=\"tools\">\r\n				");
	if (valuablepost.Msn!="")
	{

	templateBuilder.Append("\r\n				<li><a href=\"mailto:");
	templateBuilder.Append(valuablepost.Msn.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(valuablepost.Msn.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n				");
	}	//end if


	if (valuablepost.Skype!="")
	{

	templateBuilder.Append("\r\n				<li><a href=\"skype:");
	templateBuilder.Append(valuablepost.Skype.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(valuablepost.Skype.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n				");
	}	//end if


	if (valuablepost.Icq!="")
	{

	templateBuilder.Append("\r\n				<li><a href=\"http://wwp.icq.com/scripts/search.dll?to=");
	templateBuilder.Append(valuablepost.Icq.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(valuablepost.Icq.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n				");
	}	//end if


	if (valuablepost.Qq!="")
	{

	templateBuilder.Append("\r\n				<li><a href=\"http://wpa.qq.com/msgrd?V=1&Uin=");
	templateBuilder.Append(valuablepost.Qq.ToString().Trim());
	templateBuilder.Append("&Site=");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("&Menu=yes\" target=\"_blank\">");
	templateBuilder.Append(valuablepost.Qq.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n				");
	}	//end if


	if (valuablepost.Yahoo!="")
	{

	templateBuilder.Append("\r\n				<li><a href=\"http://edit.yahoo.com/config/send_webmesg?.target=");
	templateBuilder.Append(valuablepost.Yahoo.ToString().Trim());
	templateBuilder.Append("&.src=pg\" target=\"_blank\">");
	templateBuilder.Append(valuablepost.Yahoo.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</ul>\r\n		</div>\r\n		<!-- member menu -->\r\n		");
	}	//end if


	}	//end if

	 loopi = loopi+1;
	

	}	//end loop

	templateBuilder.Append("\r\n</div>\r\n");
	}	//end if


	if (valuelesspostcount!=0)
	{

	templateBuilder.Append("\r\n<div class=\"main\">\r\n	<h4>其它答案</h4>\r\n	");
	int valueless__loop__id=0;
	foreach(ShowbonusPagePostInfo valueless in postlist)
	{
		valueless__loop__id++;


	if (valueless.Id>1 && valueless.Isbest==0)
	{

	templateBuilder.Append("\r\n	<div class=\"pi\">\r\n		<div class=\"postinfo bonus\">\r\n			<cite class=\"y\">\r\n		");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n			<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valueless.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n			<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valueless.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\">删除</a>\r\n		");
	}
	else
	{


	if (valueless.Posterid!=-1 && userid==valueless.Posterid)
	{


	if (topic.Closed==0)
	{

	templateBuilder.Append("\r\n					<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valueless.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valueless.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\">删除</a>\r\n			");
	}	//end if


	}	//end if


	if (canreply)
	{

	templateBuilder.Append("\r\n			<a href=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(valueless.Pid.ToString().Trim());
	templateBuilder.Append("&quote=yes\" class=\"repquote\">引用</a>\r\n			");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n				<a href=\"###\" onclick=\"replyToFloor('");
	templateBuilder.Append(valueless.Id.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(valueless.Poster.ToString().Trim());
	templateBuilder.Append("', '");
	templateBuilder.Append(valueless.Pid.ToString().Trim());
	templateBuilder.Append("')\" class=\"fastreply\">回复</a>\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</cite>\r\n			");	 aspxrewriteurl = this.UserInfoAspxRewrite(valueless.Posterid);
	
	templateBuilder.Append("\r\n			由<a id=\"memberinfo_");
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" onmouseover=\"showMenu(this.id,false)\" class=\"drop xg2\"  style=\"padding:0 14px 0 4px;\">");
	templateBuilder.Append(valueless.Poster.ToString().Trim());
	templateBuilder.Append("</a>发表于");	templateBuilder.Append(Convert.ToDateTime(valueless.Postdatetime).ToString("yyyy-MM-dd hh:mm"));
	templateBuilder.Append("\r\n		</div>\r\n	</div>\r\n	<div class=\"postmessage\">\r\n		<div class=\"t_msgfont\">");
	templateBuilder.Append(valueless.Message.ToString().Trim());
	templateBuilder.Append("</div>\r\n	</div>\r\n");
	if (valueless.Posterid!=-1)
	{

	templateBuilder.Append("\r\n<!-- member menu -->\r\n<div class=\"popupmenu_popup userpanelmenu\" id=\"memberinfo_");
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("_menu\" style=\"display: none; z-index: 50; filter: progid:dximagetransform.microsoft.shadow(direction=135,color=#cccccc,strength=2); left: 19px; clip: rect(auto auto auto auto); position: absolute; top: 253px; width:150px;padding:10px;\" initialized ctrlkey=\"userinfo2\">\r\n	<ul class=\"panelinfo\">\r\n		<li><a href=\"usercppostpm.aspx?msgtoid=");
	templateBuilder.Append(valueless.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_postpm', this.href, 600, 410, '600,0');doane(event);\" target=\"_blank\">发送短消息</a></li>\r\n	");
	if (useradminid>0)
	{


	if (admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n		<li><a href=\"getip.aspx?pid=");
	templateBuilder.Append(valueless.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" onclick=\"floatwin('open_getip', this.href, 400, 170, '600,0');doane(event);\" title=\"查看IP\">查看IP</a></li>\r\n	");
	}	//end if


	if (admininfo.Allowbanuser==1)
	{

	templateBuilder.Append("\r\n		<li><a href=\"useradmin.aspx?action=banuser&uid=");
	templateBuilder.Append(valueless.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_mods', this.href, 250, 270, '600,0');doane(event);\" title=\"禁止用户\">禁止用户</a></li>\r\n	");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	<li>\r\n		");	 aspxrewriteurl = this.UserInfoAspxRewrite(valueless.Posterid);
	
	templateBuilder.Append("\r\n		<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">查看公共资料</a>\r\n	</li>\r\n	<li><a href=\"search.aspx?posterid=");
	templateBuilder.Append(valueless.Posterid.ToString().Trim());
	templateBuilder.Append("\">查找该会员全部帖子</a></li>\r\n	</ul>\r\n	<ul class=\"userdetail\">\r\n		<li><label>UID</label>");
	templateBuilder.Append(valueless.Posterid.ToString().Trim());
	templateBuilder.Append("</li>\r\n		<li><label>精华</label>\r\n		");
	if (valueless.Digestposts>0)
	{

	templateBuilder.Append("<a href=\"search.aspx?posterid=");
	templateBuilder.Append(valueless.Posterid.ToString().Trim());
	templateBuilder.Append("&type=digest\">");
	templateBuilder.Append(valueless.Digestposts.ToString().Trim());
	templateBuilder.Append("</a>");
	}
	else
	{
	templateBuilder.Append(valueless.Digestposts.ToString().Trim());
	}	//end if

	templateBuilder.Append("</li>\r\n	");
	if (score[1].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[1].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits1.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[1].ToString().Trim() + "</li>\r\n	");
	}	//end if


	if (score[2].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[2].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits2.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[2].ToString().Trim() + "</li>\r\n	");
	}	//end if


	if (score[3].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[3].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits3.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[3].ToString().Trim() + "</li>\r\n	");
	}	//end if


	if (score[4].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[4].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits4.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[4].ToString().Trim() + "</li>\r\n	");
	}	//end if


	if (score[5].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[5].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits5.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[5].ToString().Trim() + "</li>\r\n	");
	}	//end if


	if (score[6].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[6].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits6.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[6].ToString().Trim() + "</li>\r\n	");
	}	//end if


	if (score[7].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[7].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits7.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[7].ToString().Trim() + "</li>\r\n	");
	}	//end if


	if (score[8].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li><label>" + score[8].ToString().Trim() + "</label>");
	templateBuilder.Append(valueless.Extcredits8.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[8].ToString().Trim() + "</li>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n		<li><label>来自</label>");
	templateBuilder.Append(valueless.Location.ToString().Trim());
	templateBuilder.Append("</li>\r\n		<li><label>状态</label>\r\n		");
	if (valueless.Onlinestate==1)
	{

	templateBuilder.Append("\r\n		在线\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n		离线\r\n		");
	}	//end if

	templateBuilder.Append("</li>\r\n	</ul>\r\n	<ul class=\"tools\">\r\n		");
	if (valueless.Msn!="")
	{

	templateBuilder.Append("\r\n		<li><a href=\"mailto:");
	templateBuilder.Append(valueless.Msn.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(valueless.Msn.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end if


	if (valueless.Skype!="")
	{

	templateBuilder.Append("\r\n		<li><a href=\"skype:");
	templateBuilder.Append(valueless.Skype.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(valueless.Skype.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end if


	if (valueless.Icq!="")
	{

	templateBuilder.Append("\r\n		<li><a href=\"http://wwp.icq.com/scripts/search.dll?to=");
	templateBuilder.Append(valueless.Icq.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(valueless.Icq.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end if


	if (valueless.Qq!="")
	{

	templateBuilder.Append("\r\n		<li><a href=\"http://wpa.qq.com/msgrd?V=1&Uin=");
	templateBuilder.Append(valueless.Qq.ToString().Trim());
	templateBuilder.Append("&Site=");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("&Menu=yes\" target=\"_blank\">");
	templateBuilder.Append(valueless.Qq.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end if


	if (valueless.Yahoo!="")
	{

	templateBuilder.Append("\r\n		<li><a href=\"http://edit.yahoo.com/config/send_webmesg?.target=");
	templateBuilder.Append(valueless.Yahoo.ToString().Trim());
	templateBuilder.Append("&.src=pg\" target=\"_blank\">");
	templateBuilder.Append(valueless.Yahoo.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n	</ul>\r\n	</div>\r\n	<!-- member menu -->\r\n	");
	}	//end if


	}	//end if

	 loopi = loopi+1;
	

	}	//end loop

	templateBuilder.Append("\r\n</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n");
	if (canreply && userid!=-1)
	{





	}	//end if


	}	//end if


	}
	else
	{


	if (needlogin)
	{



	if (infloat!=1)
	{

	templateBuilder.Append("\r\n<div class=\"wrap cl\">\r\n	<div class=\"blr\">\r\n	<div class=\"msgbox\" style=\"margin:4px auto;padding:0 !important;margin-left:0;background:none;\">\r\n		<div class=\"msg_inner error_msg\">\r\n			<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n			<p><b>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</b></p>\r\n			<p>您还没有登录，请填写下面的登录表单后再尝试访问。</p>\r\n		</div>\r\n	</div>\r\n	<hr class=\"solidline\"/>\r\n	<form id=\"formlogin\" name=\"formlogin\" method=\"post\" action=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx\" onsubmit=\"submitLogin(this);\">\r\n	<div class=\"c cl\">\r\n		<div style=\"overflow:hidden;overflow-y:auto\" class=\"lgfm\">\r\n		<input type=\"hidden\" value=\"2592000\" name=\"cookietime\"/>\r\n			<div class=\"sipt lpsw\">\r\n				<label for=\"username\">用户名　：</label>\r\n				<input type=\"text\" id=\"username\" name=\"username\" size=\"25\" maxlength=\"40\" tabindex=\"2\" class=\"txt\" />\r\n			</div>\r\n			<div class=\"sipt lpsw\">\r\n				<label for=\"password\">密　码　：</label>\r\n				<input type=\"password\" name=\"password\" size=\"25\" tabindex=\"3\" class=\"txt\"/>\r\n			</div>\r\n        ");
	if (isLoginCode)
	{

	templateBuilder.Append("\r\n			<div class=\"lpsw\" style=\"position: relative;margin-bottom:10px;\">\r\n				");
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

	templateBuilder.Append("\r\n			</div>\r\n        ");
	}	//end if


	if (config.Secques==1)
	{

	templateBuilder.Append("\r\n				<div class=\"ftid sltp\" style=\"margin-bottom:10px\">\r\n					<select name=\"question\" id=\"question_login\" change=\"changequestion();\" tabindex=\"5\">\r\n						<option value=\"0\">安全提问（未设置请忽略）</option>\r\n						<option onclick=\"loadselect_liset('question_login', 0, 'question_login','1',this.innerHTML, 1)\" value=\"1\" k_id=\"question_login\">母亲的名字</option>\r\n						<option onclick=\"loadselect_liset('question_login', 0, 'question_login','2',this.innerHTML, 2)\" value=\"2\" k_id=\"question_login\">爷爷的名字</option>\r\n						<option onclick=\"loadselect_liset('question_login', 0, 'question_login','3',this.innerHTML, 3)\" value=\"3\" k_id=\"question_login\">父亲出生的城市</option>\r\n						<option onclick=\"loadselect_liset('question_login', 0, 'question_login','4',this.innerHTML, 4)\" value=\"4\" k_id=\"question_login\">您其中一位老师的名字</option>\r\n						<option onclick=\"loadselect_liset('question_login', 0, 'question_login','5',this.innerHTML, 5)\" value=\"5\" k_id=\"question_login\">您个人计算机的型号</option>\r\n						<option onclick=\"loadselect_liset('question_login', 0, 'question_login','6',this.innerHTML, 6)\" value=\"6\" k_id=\"question_login\">您最喜欢的餐馆名称</option>\r\n						<option onclick=\"loadselect_liset('question_login', 0, 'question_login','7',this.innerHTML, 7)\" value=\"7\" k_id=\"question_login\">驾驶执照的最后四位数字</option>\r\n					</select>\r\n					<script type=\"text/javascript\">simulateSelect('question_login', '214');</");
	templateBuilder.Append("script>\r\n					<script type=\"text/javascript\">\r\n					    window.onload = function(){setselect(");
	templateBuilder.Append(question.ToString());
	templateBuilder.Append(");}\r\n				        function changequestion() {\r\n				            if ($('question_login').getAttribute(\"selecti\") != \"0\") {\r\n				                $('answer_login').style.display = '';\r\n						        $('answer_login').focus();\r\n				            }\r\n				            else {\r\n				                $('answer_login').style.display = 'none';\r\n				            }\r\n				        }\r\n				        function setselect(value) {\r\n				            try {\r\n                                var questionarray = new Array('安全提问','母亲的名字','爷爷的名字','父亲出生的城市','您其中一位老师的名字','您个人计算机的型号','您最喜欢的餐馆名称','驾驶执照的最后四位数字');\r\n                                $('question_login').setAttribute(\"selecti\",value);\r\n                                $('question_login').options[0].value = value;\r\n                                $('question_ctrl').innerHTML = questionarray[value];\r\n                                changequestion();\r\n				            }\r\n				            catch (e) {\r\n				            }\r\n				        }\r\n\r\n					</");
	templateBuilder.Append("script>\r\n				</div>\r\n				<div class=\"sltp\" style=\"clear:both;\"><input type=\"text\" tabindex=\"6\" class=\"txt\" size=\"36\" autocomplete=\"off\" style=\"display: none;\" id=\"answer_login\" name=\"answer\"/></div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<div class=\"sltp\"  style=\"display:none\">\r\n				<label for=\"templateid\">界面风格</label>\r\n				<select name=\"templateid\" tabindex=\"7\">\r\n				<option value=\"0\">- 使用默认 -</option>\r\n					");
	templateBuilder.Append(templatelistboxoptions.ToString());
	templateBuilder.Append("\r\n				</select>\r\n			</div>\r\n		</div>\r\n		<div class=\"lgf\">\r\n			<h4>没有帐号？\r\n				");
	if (infloat==1)
	{

	templateBuilder.Append("\r\n				<a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx\"  onclick=\"hideWindow('login');showWindow('register', this.href);\" class=\"xg2\">立即注册</a>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				<a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx\" tabindex=\"-1\" accesskey=\"r\" title=\"立即注册 (ALT + R)\" class=\"xg2\">立即注册</a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</h4>\r\n			<p>\r\n				");
	if (infloat==1)
	{

	templateBuilder.Append("\r\n				<a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("getpassword.aspx\" onclick=\"hideWindow('login');showWindow('getpassword', this.href);\" accesskey=\"g\" title=\"忘记密码 (ALT + G)\" class=\"xg2\">找回密码</a>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				<a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("getpassword.aspx\" accesskey=\"g\" title=\"找回密码\" class=\"xg2\">找回密码</a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</p>\r\n		</div>\r\n	</div>\r\n	<p class=\"fsb pns cl\">\r\n		<input type=\"submit\" style=\"width:0;filter:alpha(opacity=0);-moz-opacity:0;opacity:0;\"/>\r\n		<button name=\"login\" type=\"submit\" id=\"login\" tabindex=\"8\" ");
	if (infloat!=1)
	{

	templateBuilder.Append("onclick=\"javascript:window.location.replace('?agree=yes')\"");
	}	//end if

	templateBuilder.Append(" class=\"pn\"><span>登录</span></button>\r\n		<input type=\"checkbox\" value=\"43200\" tabindex=\"9\" id=\"expires\" name=\"expires\" checked/>\r\n		<label for=\"expires\"><span title=\"下次访问自动登录\">记住我</span></label>\r\n	</p>\r\n	<script type=\"text/javascript\">\r\n		document.getElementById(\"username\").focus();\r\n	</");
	templateBuilder.Append("script>\r\n	</form>\r\n</div>\r\n</div>\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n<div class=\"main\">\r\n	<div class=\"msgbox\">\r\n		<h1>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" 提示信息</h1>\r\n		<hr class=\"solidline\"/>\r\n		<div class=\"msg_inner error_msg\">\r\n			<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n			<p><b>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</b></p>\r\n			<p>您还没有登录，请填写下面的登录表单后再尝试访问。</p>\r\n		</div>\r\n	</div>\r\n</div>\r\n<script type=\"text/javascript\" reload=\"1\">\r\nsetTimeout(\"floatwin('close_newthread');floatwin('close_reply');floatwin('close_edit');floatwin('open_login', '");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("login.aspx', 600, 410)\",1000);\r\n</");
	templateBuilder.Append("script>\r\n");
	}	//end if

	templateBuilder.Append("	\r\n<script type=\"text/javascript\">\r\n        ");
	if (infloat!=1)
	{

	templateBuilder.Append("\r\n		document.getElementById(\"username\").focus();\r\n        ");
	}	//end if

	templateBuilder.Append("\r\n		function submitLogin(loginForm)\r\n		{\r\n//		    loginForm.action = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?loginsubmit=true&reurl=' + escape(window.location);\r\n            loginForm.action = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?reurl=' + escape(window.location);\r\n            \r\n			loginForm.submit();\r\n		}\r\n</");
	templateBuilder.Append("script>");


	}
	else
	{


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


	}	//end if


	if (config.Forumjump==1)
	{

	templateBuilder.Append("\r\n	");
	templateBuilder.Append(navhomemenu.ToString());
	templateBuilder.Append("\r\n");
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





	if (infloat!=1)
	{


	if (floatad!="")
	{

	templateBuilder.Append("\r\n	<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_floatadv.js\"></");
	templateBuilder.Append("script>\r\n	");
	templateBuilder.Append(floatad.ToString());
	templateBuilder.Append("\r\n	<script type=\"text/javascript\">theFloaters.play();</");
	templateBuilder.Append("script>\r\n");
	}
	else if (doublead!="")
	{

	templateBuilder.Append("\r\n	<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_floatadv.js\"></");
	templateBuilder.Append("script>\r\n	");
	templateBuilder.Append(doublead.ToString());
	templateBuilder.Append("\r\n	<script type=\"text/javascript\">theFloaters.play();</");
	templateBuilder.Append("script>\r\n");
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
