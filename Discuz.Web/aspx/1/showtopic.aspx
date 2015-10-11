<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.showtopic" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/6/2 16:12:47.
		本页面代码由Discuz!NT模板引擎生成于 2011/6/2 16:12:47. 
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




	if (infloat!=1)
	{

	templateBuilder.Append("\r\n<script type=\"text/javascript\">\r\nvar templatepath = \"");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("\";\r\nvar postminchars = parseInt(");
	templateBuilder.Append(config.Minpostsize.ToString().Trim());
	templateBuilder.Append(");\r\nvar postmaxchars = parseInt(");
	templateBuilder.Append(config.Maxpostsize.ToString().Trim());
	templateBuilder.Append(");\r\nvar disablepostctrl = parseInt(");
	templateBuilder.Append(disablepostctrl.ToString());
	templateBuilder.Append(");\r\nvar attachtransname = \"");
	templateBuilder.Append(Scoresets.GetTopicAttachCreditsTransName().ToString().Trim());
	templateBuilder.Append("\";\r\nvar imagedir = \"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("\";\r\nvar forumtitle = '");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("';\r\nfunction modaction(action, pid, extra) \r\n{\r\n    if(!action) \r\n    {\r\n        return;\r\n    }\r\n    var extra = !extra ? '' : '&' + extra;\r\n    if(!pid && in_array(action, ['delposts', 'banpost'])) \r\n    {\r\n        var checked = 0;\r\n        var pid = '';\r\n        for(var i = 0; i < $('postsform').elements.length; i++) \r\n        {\r\n            if($('postsform').elements[i].name.match('topiclist')) \r\n            {\r\n                checked = 1;\r\n                break;\r\n            }\r\n        }\r\n    } \r\n    else \r\n    {\r\n        var checked = 1;\r\n    }\r\n    if(!checked) \r\n    {\r\n        alert('请选择需要操作的帖子');\r\n    } \r\n    else \r\n    {\r\n        floatwinreset = 1;\r\n		hideWindow('mods');\r\n        $('postsform').action = 'topicadmin.aspx?action='+ action +'&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&infloat=1&nopost=1' + (!$('postsform').pid.value ? '' : '&postid=' + $('postsform').pid.value) + extra;\r\n		showWindow('mods', 'postsform', 'post', 0);\r\n		  if(BROWSER.ie) {\r\n			doane(event);\r\n		}	\r\n    }\r\n}\r\n\r\n\r\nfunction pidchecked(obj) \r\n{\r\n    if(obj.checked) \r\n    {\r\n        if(is_ie && BROWSER.ie != \"9.0\" && !is_opera) \r\n        {\r\n            var inp = document.createElement('<input name=\"topiclist[]\" />');\r\n        } \r\n        else \r\n        {\r\n            var inp = document.createElement('input');\r\n            inp.name = 'topiclist[]';\r\n        }\r\n        inp.id = 'topiclist_' + obj.value;\r\n        inp.value = obj.value;\r\n        inp.style.display = 'none';\r\n        $('postsform').appendChild(inp);\r\n    } \r\n    else\r\n    {\r\n        $('postsform').removeChild($('topiclist_' + obj.value));\r\n    }\r\n}\r\n\r\nvar modclickcount = 0;\r\nfunction modclick(obj, pid) \r\n{\r\n    if(obj.checked) \r\n    {\r\n        modclickcount++;\r\n        if($('postsform').pid.value)\r\n            $('postsform').pid.value += \",\" + pid;\r\n        else\r\n            $('postsform').pid.value = pid;\r\n    } \r\n    else \r\n    {\r\n        modclickcount--;\r\n        if(modclickcount > 0)\r\n        {\r\n            $('postsform').pid.value = $('postsform').pid.value.replace(\",\" + pid + \",\", \",\");\r\n            $('postsform').pid.value = $('postsform').pid.value.replace(\",\" + pid, \"\");\r\n            $('postsform').pid.value = $('postsform').pid.value.replace(pid + \",\", \"\");\r\n        }\r\n        else\r\n            $('postsform').pid.value = '';\r\n    }\r\n    $('modcount').innerHTML = modclickcount;\r\n    if(modclickcount > 0) \r\n    {\r\n        var offset = fetchOffset(obj);				\r\n        $('modtopiclayer').style.top = offset['top'] - 50 + 'px';\r\n        $('modtopiclayer').style.left = offset['left'] - 120 + 'px';\r\n        $('modtopiclayer').style.display = '';\r\n        $('modtopiclayer').className = 'topicwindow';\r\n    } \r\n    else \r\n    {\r\n        $('modtopiclayer').style.display = 'none';\r\n    }\r\n}\r\n</");
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
	templateBuilder.Append("/template_share.js\"></");
	templateBuilder.Append("script>\r\n");	int loopi = 1;
	

	if (page_err==0)
	{

	templateBuilder.Append("\r\n<div class=\"wrap cl pageinfo\">\r\n	<div id=\"nav\">\r\n		");
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

	templateBuilder.Append("\r\n		<a id=\"forumlist\" href=\"");
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
	templateBuilder.Append("  &raquo;  \r\n        <span title=\"");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("\" style=\"cursor:pointer;\">");	templateBuilder.Append(Utils.GetUnicodeSubString(topic.Title,60,"..."));
	templateBuilder.Append("</span>\r\n	</div>\r\n</div>\r\n<div class=\"wrap cl\">\r\n");
	if (config.Forumjump==1)
	{

	templateBuilder.Append("\r\n	");
	templateBuilder.Append(Caches.GetForumListMenuDivCache(usergroupid,userid,config.Extname).ToString().Trim());
	templateBuilder.Append("\r\n");
	}	//end if

	templateBuilder.Append("\r\n    <div class=\"pages_btns cl\">\r\n	    <div class=\"pages\">\r\n		    <cite class=\"pageback\">");
	templateBuilder.Append(listlink.ToString());
	templateBuilder.Append("</cite>\r\n		    ");
	if (pagecount!=1)
	{

	templateBuilder.Append("\r\n		    ");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n		    ");
	if (pagecount>8)
	{

	templateBuilder.Append("\r\n		    <kbd>\r\n		    	<input name=\"gopage\" type=\"text\" class=\"txt\" id=\"pageidinput1\" title=\"可以输入页码按回车键自动跳转\" value=\"");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" style=\"text-align:center;\" onfocus=\"this.value=this.defaultValue;this.select();\" onKeyDown=\"pageinputOnKeyDown(this,event);\" size=\"2\" maxlength=\"9\" />/ ");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("</kbd>\r\n                <script type=\"text/javascript\">\r\n                    function pageinputOnKeyDown(obj, event) {\r\n                        if (event.keyCode == 13) {\r\n                            var typeid = getQueryString(\"typeid\");\r\n                            typeid = typeid == \"\" ? -1 : parseInt(typeid);\r\n                            if (parseInt('");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append("') == 1 && typeid <0) {\r\n                                window.location = 'showtopic-");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("-' + (parseInt(obj.value) > 0 ? parseInt(obj.value) : 1) + '");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("';\r\n                            }\r\n                            else {\r\n                                (typeid > 0) ? window.location = 'showtopic.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&page=' + (parseInt(obj.value) > 0 ? parseInt(obj.value) : 1) + '&typeid=' + typeid : window.location = 'showtopic.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&page=' + (parseInt(obj.value) > 0 ? parseInt(obj.value) : 1);\r\n                            }\r\n                        }\r\n                        return (event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 97 && event.keyCode <= 105) || event.keyCode == 8;\r\n                    }\r\n                </");
	templateBuilder.Append("script>\r\n		    ");
	}	//end if

	templateBuilder.Append("\r\n		    ");
	templateBuilder.Append(nextpage.ToString());
	templateBuilder.Append("\r\n		    ");
	}	//end if

	templateBuilder.Append("\r\n	    </div>\r\n    ");
	if (userid<0||canposttopic)
	{

	string newtopicurl = "";
	

	if (forum.Allowspecialonly<=0)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&forumpage=" + pageid;
	

	}
	else if (1==(forum.Allowpostspecial&1)&&usergroupinfo.Allowpostpoll==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=poll&forumpage=" + pageid;
	

	}
	else if (4==(forum.Allowpostspecial&4)&&usergroupinfo.Allowbonus==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=bonus&forumpage=" + pageid;
	

	}
	else if (16==(forum.Allowpostspecial&16)&&usergroupinfo.Allowdebate==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=debate&forumpage=" + pageid;
	

	}	//end if

	string newtopiconclick = "";
	

	if (forum.Allowspecialonly<=0&&canposttopic&&!isnewbie)
	{

	 newtopiconclick = "showWindow('newthread', '" + forumpath + "showforum.aspx?forumid=" + forum.Fid + "')";
	

	}	//end if

	bool allowpost = userid<=0&&(forum.Postperm==""?usergroupinfo.Allowpost==0:!Utils.InArray(usergroupid.ToString(),forum.Postperm));
	

	if (allowpost)
	{

	 newtopiconclick = "showWindow('login', '" + forumpath + "login.aspx');hideWindow('register');";
	

	}
	else
	{

	 newtopiconclick = "showWindow('newthread', '" + forumpath + "showforum.aspx?forumid=" + forum.Fid + "')";
	

	}	//end if

	templateBuilder.Append("\r\n	    <span ");
	if (userid>0)
	{

	templateBuilder.Append(" onmouseover=\"if($('newspecial_menu')!=null&&$('newspecial_menu').childNodes.length>0)  showMenu(this.id);\"");
	}	//end if

	templateBuilder.Append(" id=\"newspecial\" class=\"postbtn\">\r\n            <a title=\"发新话题\" id=\"newtopic\" href=\"");
	templateBuilder.Append(newtopicurl.ToString());
	templateBuilder.Append("\" onclick=\"");
	templateBuilder.Append(newtopiconclick.ToString());
	templateBuilder.Append("\">\r\n                <img alt=\"发新话题\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/newtopic.png\"/></a>\r\n        </span>\r\n    ");
	}	//end if


	if ((topic.Closed!=1||ismoder==1) && (userid<0||canreply))
	{

	templateBuilder.Append("\r\n	    <span class=\"replybtn\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\"");
	if (canreply)
	{


	if (!isnewbie)
	{

	templateBuilder.Append(" onclick=\"showWindow('reply', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("');\"");
	}	//end if


	}
	else
	{

	templateBuilder.Append(" onclick=\"showWindow('login', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');\"");
	}	//end if

	templateBuilder.Append("><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/reply.png\" alt=\"回复该主题\" /></a></span>\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n    </div>\r\n<div class=\"main viewthread cl\">\r\n	<form id=\"postsform\" name=\"postsform\" method=\"post\" action=\"topicadmin.aspx?action=moderate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">\r\n	<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n	<input name=\"topicid\" type=\"hidden\" value=\"");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" />\r\n	<input name=\"operat\" type=\"hidden\" value=\"delposts\" />\r\n	<input name=\"pid\" type=\"hidden\" />\r\n	");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n	<div id=\"modtopiclayer\" style=\"display:none;\">\r\n		<span>选中</span><strong id=\"modcount\">1</strong><span>篇: </span>\r\n		<a onclick=\"modaction('banpost')\" href=\"javascript:;\" class=\"xg2\">屏蔽</a>\r\n		<a onclick=\"modaction('delposts')\" href=\"javascript:;\" class=\"xg2\">删除</a>\r\n    </div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<div id=\"postsContainer\">\r\n	<table cellspacing=\"0\" cellpadding=\"0\" class=\"plh\">\r\n	<tbody>\r\n	<tr>\r\n		<td class=\"postauthor\">\r\n            ");
	if (pageid==1)
	{

	templateBuilder.Append("\r\n			    <div class=\"hm\" style=\"padding-top:14px;\"><span class=\"xg1\">查看:</span> ");
	templateBuilder.Append(topicviews.ToString());
	templateBuilder.Append("<span class=\"pipe\">|</span><span class=\"xg1\">回复:</span> ");
	templateBuilder.Append(topic.Replies.ToString().Trim());
	templateBuilder.Append("</div>\r\n            ");
	}
	else
	{

	string avatarurl = Avatars.GetAvatarUrl(topic.Posterid);
	
	templateBuilder.Append("\r\n                <div class=\"hm\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("userinfo.aspx?userid=");
	templateBuilder.Append(topic.Posterid.ToString().Trim());
	templateBuilder.Append("\"><img height=\"24\" width=\"24\" src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_small.gif';\" />楼主:");
	templateBuilder.Append(topic.Poster.ToString().Trim());
	templateBuilder.Append("</a></div>\r\n            ");
	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n		<td class=\"posttopic\">\r\n			<h1 class=\"ts z\">\r\n				");
	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{

	templateBuilder.Append("\r\n					<cite>");
	templateBuilder.Append(topictypes.ToString());
	templateBuilder.Append("</cite>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n					<span title=\"");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("\" style=\"cursor:pointer;\">");	templateBuilder.Append(Utils.GetUnicodeSubString(topic.Title,60,"..."));
	templateBuilder.Append("</span>\r\n				");
	if (topic.Special==4)
	{

	templateBuilder.Append("\r\n                    <span class=\"xg2 xs0\">辩论主题 [<a href=\"");
	templateBuilder.Append(ShowDebateAspxRewrite(topicid).ToString());
	templateBuilder.Append("\">辩论模式</a>]</span>\r\n                ");
	}	//end if

	templateBuilder.Append("\r\n                <a title=\"复制本帖链接\" href=\"javascript:;\" onclick=\"copytitle();\" class=\"xg1 xs0\">[复制链接]</a>\r\n                ");
	if (config.Showimgattachmode==1&&Utils.InArray(usergroupid.ToString(),"1,2,3"))
	{

	templateBuilder.Append("\r\n                <a title=\"加载所有图片附件\" href=\"javascript:;\" onclick=\"loadAllImg();\" class=\"xg1 xs0\">[加载所有图片]</a></span>\r\n                ");
	}	//end if

	templateBuilder.Append("\r\n			</h1>\r\n			");	bool canuseadminfunc = usergroupinfo.Raterange!="" || usergroupinfo.Maxprice>0 || (topic.Special==2&&topic.Posterid==userid);
	

	if (useradminid>0)
	{

	templateBuilder.Append("\r\n				<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n				<input name=\"topicid\" type=\"hidden\" value=\"");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" />\r\n				<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n				<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n				<input type=\"hidden\" name=\"winheight\" />\r\n				<input type=\"hidden\" name=\"optgroup\" />\r\n				");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n				<span class=\"drop xg2 y\" onclick=\"showMenu({'ctrlid':this.id, 'pos':'21'})\" id=\"operatSelTop\" style=\"margin-top:16px;margin-right:10px;\">主题管理</span>\r\n				<ul style=\"width: 180px; display:none;\" id=\"operatSelTop_menu\" class=\"p_pop inlinelist\">\r\n					<li><a onclick=\"modthreads(1, 'delete');return false;\" href=\"###\">删除</a></li>\r\n					<li><a onclick=\"modthreads(1, 'bump');return false;\" href=\"###\">提沉</a></li>\r\n					<li><a onclick=\"modthreads(1, 'close');return false;\" href=\"###\">关闭</a></li>\r\n					<li><a onclick=\"modthreads(1, 'move');return false;\" href=\"###\">移动</a></li>\r\n					<li><a onclick=\"modthreads(1, 'copy');return false;\" href=\"###\">复制</a></li>\r\n					<li><a onclick=\"modthreads(1, 'highlight');return false;\" href=\"###\">高亮</a></li>\r\n					<li><a onclick=\"modthreads(1, 'digest');return false;\" href=\"###\">精华</a></li>\r\n					<li><a onclick=\"modthreads(1, 'identify');return false;\" href=\"###\">鉴定</a></li>\r\n					<li><a onclick=\"modthreads(1, 'displayorder');return false;\" href=\"###\">置顶</a></li>\r\n					<li><a onclick=\"modthreads(1, 'split');return false;\" href=\"###\">分割</a></li>\r\n					<li><a onclick=\"modthreads(1, 'merge');return false;\" href=\"###\">合并</a></li>\r\n					<li><a onclick=\"modthreads(1, 'repair');return false;\" href=\"###\">修复</a></li>\r\n					<li><a onclick=\"modthreads(1, 'type');return false;\" href=\"###\">分类</a></li>\r\n				</ul>\r\n				");
	}	//end if


	}
	else if (canuseadminfunc)
	{

	templateBuilder.Append("\r\n				<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n				<input name=\"topicid\" type=\"hidden\" value=\"");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" />\r\n				<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n				<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n	</tr>\r\n	<tr class=\"threadad\">\r\n		<td class=\"postauthor\"></td>\r\n		<td class=\"adcontent\"></td>\r\n	</tr>\r\n	</tbody>\r\n	</table>\r\n	");
	int post__loop__id=0;
	foreach(ShowtopicPagePostInfo post in postlist)
	{
		post__loop__id++;

	templateBuilder.Append("\r\n	<table id=\"");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" summary=\"");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" cellspacing=\"0\" cellpadding=\"0\">\r\n		<tbody>\r\n		<tr>\r\n		<td class=\"postauthor\" rowspan=\"3\">\r\n			");
	if (post.Posterid!=-1)
	{

	templateBuilder.Append("\r\n			<!-- member menu -->\r\n			<div class=\"popupmenu_popup userinfopanel\" id=\"");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("\" style=\"display:none;  position:absolute;\" initialized ctrlkey=\"userinfo2\">\r\n				<div class=\"popavatar\">\r\n					<div id=\"");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("_ma\"></div>\r\n					<ul class=\"profile_side\">\r\n						<li class=\"post_pm\"><a onclick=\"showWindow('postpm', this.href, 'get', 0);doane(event);\" href=\"usercppostpm.aspx?msgtoid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">发送短消息</a></li>\r\n						");
	if (useradminid==1||useradminid==2)
	{

	templateBuilder.Append("\r\n						<li class=\"edit_user\"><a href=\"modcp.aspx?operation=edituser&op=edit&uid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">编辑该用户</a></li>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</ul>\r\n				</div>\r\n				<div class=\"popuserinfo\">\r\n					<dl class=\"cl\">\r\n						");
	if (Utils.InArray("uid",userfaceshow))
	{

	templateBuilder.Append("<dt>UID</dt><dd>");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("</dd>");
	}	//end if


	if (Utils.InArray("bday",userfaceshow))
	{

	templateBuilder.Append("<dt>生日</dt><dd>");
	templateBuilder.Append(post.Bday.ToString().Trim());
	templateBuilder.Append("</dd>");
	}	//end if


	if (Utils.InArray("posts",userfaceshow))
	{

	templateBuilder.Append("<dt>帖子</dt><dd>");
	templateBuilder.Append(post.Posts.ToString().Trim());
	templateBuilder.Append("</dd>");
	}	//end if


	if (Utils.InArray("digestposts",userfaceshow))
	{

	templateBuilder.Append("<dt>精华</dt><dd>");
	templateBuilder.Append(post.Digestposts.ToString().Trim());
	templateBuilder.Append("</dd>");
	}	//end if


	if (Utils.InArray("credits",userfaceshow))
	{

	templateBuilder.Append("<dt>积分</dt><dd>");
	templateBuilder.Append(post.Credits.ToString().Trim());
	templateBuilder.Append("</dd>");
	}	//end if


	if (score[1].ToString().Trim()!="" && Utils.InArray("extcredits1",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[1].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits1.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[1].ToString().Trim() + "</dd>");
	}	//end if


	if (score[2].ToString().Trim()!="" && Utils.InArray("extcredits2",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[2].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits2.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[2].ToString().Trim() + "</dd>");
	}	//end if


	if (score[3].ToString().Trim()!="" && Utils.InArray("extcredits3",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[3].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits3.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[3].ToString().Trim() + "</dd>");
	}	//end if


	if (score[4].ToString().Trim()!="" && Utils.InArray("extcredits4",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[4].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits4.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[4].ToString().Trim() + "</dd>");
	}	//end if


	if (score[5].ToString().Trim()!="" && Utils.InArray("extcredits5",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[5].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits5.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[5].ToString().Trim() + "</dd>");
	}	//end if


	if (score[6].ToString().Trim()!="" && Utils.InArray("extcredits6",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[6].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits6.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[6].ToString().Trim() + "</dd>");
	}	//end if


	if (score[7].ToString().Trim()!="" && Utils.InArray("extcredits7",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[7].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits7.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[7].ToString().Trim() + "</dd>");
	}	//end if


	if (score[8].ToString().Trim()!="" && Utils.InArray("extcredits8",userfaceshow))
	{

	templateBuilder.Append("<dt>" + score[8].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(post.Extcredits8.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[8].ToString().Trim() + "</dd>");
	}	//end if


	if (Utils.InArray("gender",userfaceshow))
	{

	templateBuilder.Append("<dt>性别</dt><dd><script type=\"text/javascript\">document.write(displayGender(");
	templateBuilder.Append(post.Gender.ToString().Trim());
	templateBuilder.Append("));</");
	templateBuilder.Append("script></dd>");
	}	//end if


	if (Utils.InArray("location",userfaceshow))
	{

	templateBuilder.Append("<dt>来自</dt><dd>");
	templateBuilder.Append(post.Location.ToString().Trim());
	templateBuilder.Append("</dd>");
	}	//end if


	if (Utils.InArray("oltime",userfaceshow))
	{

	templateBuilder.Append("<dt>在线时间</dt><dd>");
	templateBuilder.Append(post.Oltime.ToString().Trim());
	templateBuilder.Append("</dd>");
	}	//end if


	if (post.Joindate!=""&&Utils.InArray("joindate",userfaceshow))
	{

	templateBuilder.Append("<dt>注册时间</dt><dd>");	templateBuilder.Append(Convert.ToDateTime(post.Joindate).ToString("yyyy-MM-dd"));
	templateBuilder.Append("</dd>");
	}	//end if


	if (post.Lastvisit!=""&&Utils.InArray("lastvisit",userfaceshow))
	{

	templateBuilder.Append("<dt>最后登录</dt><dd>");	templateBuilder.Append(Convert.ToDateTime(post.Lastvisit).ToString("yyyy-MM-dd"));
	templateBuilder.Append("</dd>");
	}	//end if

	templateBuilder.Append("	\r\n					</dl>\r\n					<div class=\"imicons cl\">\r\n						");
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
	templateBuilder.Append("\" target=\"_blank\" class=\"public_info\">查看公共资料</a>\r\n                        <a href=\"search.aspx?posterid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("&searchsubmit=1\" target=\"_blank\" class=\"all_topic\">搜索主题</a>\r\n						<a href=\"search.aspx?posterid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("&type=author&searchsubmit=1\" target=\"_blank\" class=\"all_topic\">搜索帖子</a>\r\n					");
	if (useradminid>0)
	{


	if (admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n						<a onclick=\"showWindow('getip', this.href, 'get', 0);doane(event);\" href=\"getip.aspx?pid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" title=\"查看IP\" class=\"ip\">查看IP</a>\r\n					");
	}	//end if


	if (admininfo.Allowbanuser==1)
	{

	templateBuilder.Append("\r\n						<a href=\"useradmin.aspx?action=banuser&uid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"showWindow('mods', this.href);doane(event);\" title=\"禁止用户\" class=\"forbid_user\">禁止用户</a>\r\n					");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n			</div>\r\n			<!-- member menu -->\r\n			");
	}	//end if


	if (post__loop__id==postlist.Count)
	{

	templateBuilder.Append("\r\n			<a name=\"lastpost\"></a>\r\n			");
	}	//end if


	if (post.Posterid!=-1)
	{

	templateBuilder.Append("\r\n			<div class=\"poster\">\r\n				<span ");
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
	templateBuilder.Append("images/common/noavatar_medium.gif';\"  alt=\"头像\" id=\"memberinfo_");
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append("\" onmouseover=\"showauthor(this,");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(loopi.ToString());
	templateBuilder.Append(")\"/>			</div>\r\n			");
	}	//end if


	if (post.Nickname!="")
	{

	templateBuilder.Append("\r\n				<p>");
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


	if (Utils.InArray("uid",postleftshow))
	{

	templateBuilder.Append("<li><label>UID</label>");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("</li>");
	}	//end if


	if (Utils.InArray("bday",postleftshow))
	{

	templateBuilder.Append("<li><label>生日</label>");
	templateBuilder.Append(post.Bday.ToString().Trim());
	templateBuilder.Append("</li>");
	}	//end if


	if (Utils.InArray("posts",postleftshow))
	{

	templateBuilder.Append("<li><label>帖子</label>");
	templateBuilder.Append(post.Posts.ToString().Trim());
	templateBuilder.Append("</li>");
	}	//end if


	if (Utils.InArray("digestposts",postleftshow))
	{

	templateBuilder.Append("<li><label>精华</label>");
	templateBuilder.Append(post.Digestposts.ToString().Trim());
	templateBuilder.Append("</li>");
	}	//end if


	if (Utils.InArray("credits",postleftshow))
	{

	templateBuilder.Append("<li><label>积分</label>");
	templateBuilder.Append(post.Credits.ToString().Trim());
	templateBuilder.Append("</li>");
	}	//end if


	if (score[1].ToString().Trim()!="" && Utils.InArray("extcredits1",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[1].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits1.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[1].ToString().Trim() + "</li>");
	}	//end if


	if (score[2].ToString().Trim()!="" && Utils.InArray("extcredits2",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[2].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits2.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[2].ToString().Trim() + "</li>");
	}	//end if


	if (score[3].ToString().Trim()!="" && Utils.InArray("extcredits3",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[3].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits3.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[3].ToString().Trim() + "</li>");
	}	//end if


	if (score[4].ToString().Trim()!="" && Utils.InArray("extcredits4",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[4].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits4.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[4].ToString().Trim() + "</li>");
	}	//end if


	if (score[5].ToString().Trim()!="" && Utils.InArray("extcredits5",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[5].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits5.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[5].ToString().Trim() + "</li>");
	}	//end if


	if (score[6].ToString().Trim()!="" && Utils.InArray("extcredits6",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[6].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits6.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[6].ToString().Trim() + "</li>");
	}	//end if


	if (score[7].ToString().Trim()!="" && Utils.InArray("extcredits7",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[7].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits7.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[7].ToString().Trim() + "</li>");
	}	//end if


	if (score[8].ToString().Trim()!="" && Utils.InArray("extcredits8",postleftshow))
	{

	templateBuilder.Append("<li><label>" + score[8].ToString().Trim() + "</label>");
	templateBuilder.Append(post.Extcredits8.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[8].ToString().Trim() + "</li>");
	}	//end if


	if (Utils.InArray("gender",postleftshow))
	{

	templateBuilder.Append("<li><label>性别</label><script type=\"text/javascript\">document.write(displayGender(");
	templateBuilder.Append(post.Gender.ToString().Trim());
	templateBuilder.Append("));</");
	templateBuilder.Append("script></li>");
	}	//end if


	if (Utils.InArray("location",postleftshow))
	{

	templateBuilder.Append("<li><label>来自</label>");
	templateBuilder.Append(post.Location.ToString().Trim());
	templateBuilder.Append("</li>");
	}	//end if


	if (Utils.InArray("oltime",postleftshow))
	{

	templateBuilder.Append("<li><label>在线时间</label>");
	templateBuilder.Append(post.Oltime.ToString().Trim());
	templateBuilder.Append("</li>");
	}	//end if


	if (post.Joindate!=""&&Utils.InArray("joindate",postleftshow))
	{

	templateBuilder.Append("<li><label>注册时间</label>");	templateBuilder.Append(Convert.ToDateTime(post.Joindate).ToString("yyyy-MM-dd"));
	templateBuilder.Append("</li>");
	}	//end if


	if (post.Lastvisit!=""&&Utils.InArray("lastvisit",postleftshow))
	{

	templateBuilder.Append("<li><label>最后登录</label>");	templateBuilder.Append(Convert.ToDateTime(post.Lastvisit).ToString("yyyy-MM-dd"));
	templateBuilder.Append("</li>");
	}	//end if

	templateBuilder.Append("	\r\n			</ul>\r\n			");
	if (config.Enablespace==1 || config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n			<ul class=\"nt_plug\">\r\n				");
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

	templateBuilder.Append("\r\n			<div class=\"medals\">");
	templateBuilder.Append(post.Medals.ToString().Trim());
	templateBuilder.Append("</div>\r\n			");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n			<div style=\"padding-left:15px;padding-top:6px;\">\r\n			    <em id=\"traveler_ip_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" name=\"traveler_ip\" style=\"display:none\">");
	templateBuilder.Append(post.Ip.ToString().Trim());
	templateBuilder.Append("</em>\r\n				");
	if (useradminid>0 && admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n					<a href=\"getip.aspx?pid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" onclick=\"showWindow('getip', this.href, 'get', 0);doane(event);\" title=\"查看IP\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/ip.gif\" alt=\"查看IP\" class=\"vm\"/></a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			<p><em>未注册</em></p>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n		<td class=\"postcontent\">\r\n			<div class=\"pi\">\r\n				<strong>\r\n					<a href=\"###\" class=\"floor\" title=\"复制帖子链接到剪贴板\" onclick=\"copypostlayer(");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(",");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append(",'");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("')\">\r\n					");
	if (post.Postnocustom!="")
	{

	templateBuilder.Append("\r\n					");
	templateBuilder.Append(post.Postnocustom.ToString().Trim());
	templateBuilder.Append("\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					");
	templateBuilder.Append(post.Id.ToString().Trim());
	templateBuilder.Append("<sup>#</sup>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					</a>\r\n				</strong>\r\n				<div class=\"postinfo\"> \r\n					<div class=\"msgfsize y\">\r\n						");
	if (topic.Special==4)
	{

	templateBuilder.Append("\r\n						<span class=\"xg2 f_bold\">\r\n						");
	if (post.Debateopinion==1)
	{

	templateBuilder.Append("\r\n							正方\r\n						");
	}
	else if (post.Debateopinion==2)
	{

	templateBuilder.Append("\r\n							反方\r\n						");
	}	//end if

	templateBuilder.Append("\r\n						</span><span class=\"pipe\">|</span>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n						<label style=\"margin-left:4px;\">字体大小: </label>\r\n						<small title=\"正常\" onclick=\"fontZoom('message");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("',false);\"><b>t</b></small>\r\n						<big title=\"放大\" onclick=\"fontZoom('message");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("',true);\"><b>T</b></big>\r\n					</div>\r\n					");	String olimg = OnlineUsers.GetGroupImg(post.Groupid);
	
	templateBuilder.Append("\r\n					");
	templateBuilder.Append(olimg.ToString());
	templateBuilder.Append("\r\n					<em>\r\n					");	string postdatec = ForumUtils.ConvertDateTime(post.Postdatetime);
	
	templateBuilder.Append("\r\n					发表于 <span title=\"");	templateBuilder.Append(Convert.ToDateTime(post.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
	templateBuilder.Append("\">");
	templateBuilder.Append(postdatec.ToString());
	templateBuilder.Append("</span>\r\n					</em>\r\n				");
	if (post.Posterid!=-1)
	{

	 aspxrewriteurl = Urls.ShowTopicAspxRewrite(topicid,0,DNTRequest.GetInt("typeid", -1));
	

	if (onlyauthor=="1" || onlyauthor=="2")
	{

	templateBuilder.Append("					\r\n					<span class=\"pipe\">|</span><a href=\"showtopic.aspx?topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\">显示全部</a>\r\n				");
	}
	else
	{


	if (topic.Posterid==post.Posterid)
	{

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"showtopic.aspx?topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&onlyauthor=1&posterid=");
	templateBuilder.Append(topic.Posterid.ToString().Trim());
	templateBuilder.Append("\">只看楼主</a>  \r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"showtopic.aspx?topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&onlyauthor=2&posterid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\">只看该用户</a>  \r\n					");
	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n			</div>\r\n            <!--silverlight slideshow start-->\r\n            ");
	if (post.Id==1)
	{


	if (config.Silverlight==1 && topic.Attachment==2)
	{

	templateBuilder.Append("\r\n                 <span class=\"silverlight\"><a onclick=\"BOX_showsl('slideShowSilverlight', 500);\" href=\"javascript:void(0);\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/btn_silverlight.gif\" alt=\"银光图片\" title=\"银光图片\" style=\"cursor:pointer;\" /></a></div>\r\n                 <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/PostAlbum/silverlight.js\" reload=\"1\"></");
	templateBuilder.Append("script>             \r\n                 <div id=\"BOX_overlay_sl\" style=\"background: black; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;\"></div>                     \r\n                     <div id=\"slideShowSilverlight\" style=\"clear:both;display:none; width:800px;height:500px;background:black;\">\r\n                     <h3 class=\"flb\">	                             \r\n						<em>银光图片</em>\r\n						<span id=\"swfclosebtn\" class=\"y\">\r\n						   <a href=\"javascript:;\" class=\"flbc\" onclick=\"BOX_removesl('slideShowSilverlight');\" title=\"关闭\"> </a>						</span>	                 </h3>\r\n						<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"800px\" height=\"460px\">\r\n							<param name=\"source\" value=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/PostAlbum/ClientBin/PostAlbum.xap\"/>\r\n							<param name=\"minRuntimeVersion\" value=\"4.0.50401.0\" />\r\n							<param name=\"autoUpgrade\" value=\"true\" /> 		\r\n							<param name=\"background\" value=\"black\" />	\r\n							<param name=\"initParams\" value=\"topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(",forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(",posterid=");
	templateBuilder.Append(posterid.ToString());
	templateBuilder.Append(",onlyauthor=");
	templateBuilder.Append(onlyauthor.ToString());
	templateBuilder.Append("\" />	  \r\n							<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0\" style=\"text-decoration:none\" target=\"_blank\">\r\n								<img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/PostAlbum/PostAlbum.jpg\" alt=\"安装微软Silverlight控件,即刻使用帖图浏览功能\" style=\"border-style:none\"/>		                   \r\n							</a>\r\n						 </object>                 \r\n                    </div>  	    	\r\n			    </span>\r\n                ");
	}	//end if


	}	//end if

	templateBuilder.Append("  \r\n            <!--silverlight slideshow end-->\r\n			<div id=\"ad_thread2_" + post__loop__id.ToString() + "\"></div>\r\n			<div id=\"ad_thread3_" + post__loop__id.ToString() + "\"></div>\r\n			<div class=\"postmessage defaultpost\">\r\n				");
	if (topic.Identify>0 && post.Id==1)
	{

	templateBuilder.Append("\r\n					<div class=\"threadstamp\" onclick=\"this.style.display='none';\"><img src=\"");
	templateBuilder.Append(topicidentifydir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(topicidentify.Filename.ToString().Trim());
	templateBuilder.Append("\" alt=\"点击关闭鉴定图章\" title=\"点击关闭鉴定图章\" /></div>\r\n				");
	}	//end if


	if (post.Layer!=0)
	{

	templateBuilder.Append("<h2>");
	templateBuilder.Append(post.Title.ToString().Trim());
	templateBuilder.Append("</h2>");
	}	//end if

	templateBuilder.Append("\r\n				    <div id=\"topictag\"></div>\r\n				    <div id=\"message");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"t_msgfont\">\r\n					");
	if (post.Id==1)
	{

	templateBuilder.Append("\r\n						<div id=\"firstpost\">");
	if (topic.Special!=2 && topic.Special!=3)
	{
	templateBuilder.Append(post.Message.ToString().Trim());
	templateBuilder.Append(" ");
	}	//end if

	templateBuilder.Append("</div>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n                        ");
	templateBuilder.Append(post.Message.ToString().Trim());
	templateBuilder.Append("\r\n                    ");
	}	//end if


	if (post.Id==1 && enabletag)
	{

	templateBuilder.Append("\r\n					    <script type=\"text/javascript\">function forumhottag_callback(data){ tags = data; }</");
	templateBuilder.Append("script>\r\n					    <script type=\"text/javascript\" src=\"cache/tag/hottags_forum_cache_jsonp.txt\"></");
	templateBuilder.Append("script>\r\n						");	int hastag = Topics.GetMagicValue(topic.Magic, MagicType.TopicTag);
	

	if (hastag==1)
	{

	templateBuilder.Append("\r\n							<script type=\"text/javascript\">getTopicTags(");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(");</");
	templateBuilder.Append("script>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n							<script type=\"text/javascript\">parsetag();</");
	templateBuilder.Append("script>\r\n						");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				    </div>\r\n				");
	if (attachmentlist.Count>0)
	{

	int currentattachcount = 0;
	

	int attachtemp__loop__id=0;
	foreach(ShowtopicPageAttachmentInfo attachtemp in attachmentlist)
	{
		attachtemp__loop__id++;


	if (attachtemp.Pid==post.Pid)
	{

	 currentattachcount = currentattachcount + 1;
	

	}	//end if


	}	//end loop


	if (currentattachcount>0)
	{

	int getattachperm = attachmentlist[0].Getattachperm;
	

	if (getattachperm==1)
	{

	templateBuilder.Append("\r\n					<div class=\"postattachlist\">\r\n						<div id=\"BOX_overlay\" style=\"background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;\"></div>\r\n                        <div id=\"attachpaymentlog\" style=\"display: none; background :Aliceblue;  border:0px solid #999; width:503px; height:443px;\"></div>\r\n                        <div id=\"buyattach\" style=\"display: none; background :Aliceblue; border:0px solid #999; width:503px; height:323px;\"></div>\r\n					");
	int attachment__loop__id=0;
	foreach(ShowtopicPageAttachmentInfo attachment in attachmentlist)
	{
		attachment__loop__id++;


	if (attachment.Pid==post.Pid)
	{


	if (attachment.Allowread==1)
	{



	if (attachment.Attachimgpost==1)
	{

	templateBuilder.Append("\r\n<dl class=\"t_attachlist_img attachimg cl\">\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n<dl class=\"t_attachlist attachimg cl\">\r\n");
	}	//end if


	if (attachment.Attachimgpost==1)
	{

	templateBuilder.Append("\r\n	<dt>\r\n	</dt>\r\n	");
	}
	else if (attachment.Filename.Trim().ToLower().EndsWith("rar")||attachment.Filename.Trim().ToLower().EndsWith("zip"))
	{

	templateBuilder.Append("\r\n	<dt>\r\n	<img class=\"absmiddle\" border=\"0\" alt=\"\" src=\"images/attachicons/rar.gif\"/>\r\n	</dt>\r\n	");
	}
	else
	{

	templateBuilder.Append("\r\n	<dt>\r\n	<img class=\"absmiddle\" border=\"0\" alt=\"\" src=\"images/attachicons/attachment.gif\"/>\r\n	</dt>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</dt>\r\n	<dd>\r\n	");
	if (attachment.Attachprice<=0)
	{

	templateBuilder.Append("\r\n	   <a target=\"_blank\" onclick=\"return ShowDownloadTip(");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(");\" href=\"attachment.aspx?attachmentid=");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\" class=\"xg2\">");
	templateBuilder.Append(attachment.Attachment.ToString().Trim());
	templateBuilder.Append("</a>\r\n	");
	}
	else
	{


	if (attachment.Isbought==1 || post.Posterid==userid)
	{

	templateBuilder.Append("\r\n		   <a target=\"_blank\" onclick=\"return ShowDownloadTip(");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(");\"  href=\"attachment.aspx?attachmentid=");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\" class=\"xg2\">");
	templateBuilder.Append(attachment.Attachment.ToString().Trim());
	templateBuilder.Append("</a>\r\n		");
	}
	else
	{


	if (usergroupinfo.Radminid==1)
	{

	templateBuilder.Append("\r\n			  <a target=\"_blank\"  onclick=\"return ShowDownloadTip(");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(");\" href=\"attachment.aspx?attachmentid=");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\" class=\"xg2\">");
	templateBuilder.Append(attachment.Attachment.ToString().Trim());
	templateBuilder.Append("</a>\r\n		   ");
	}
	else
	{

	templateBuilder.Append("\r\n			  ");
	templateBuilder.Append(attachment.Attachment.ToString().Trim());
	templateBuilder.Append("\r\n		   ");
	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	<em class=\"xg1\">(<script type=\"text/javascript\">ShowFormatBytesStr(");
	templateBuilder.Append(attachment.Filesize.ToString().Trim());
	templateBuilder.Append(");</");
	templateBuilder.Append("script>, 下载次数:");
	templateBuilder.Append(attachment.Downloads.ToString().Trim());
	templateBuilder.Append(")</em>\r\n");	bool viewattach = false;
	

	if (attachment.Attachprice>0)
	{

	templateBuilder.Append("\r\n	<p>\r\n	售价(");
	templateBuilder.Append(Scoresets.GetTopicAttachCreditsTransName().ToString().Trim());
	templateBuilder.Append("):<strong>");
	templateBuilder.Append(attachment.Attachprice.ToString().Trim());
	templateBuilder.Append(" </strong>									\r\n	[<a onclick=\"loadattachpaymentlog(");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append(");\" href=\"javascript:void(0);\">记录</a>]\r\n	");	 viewattach = attachment.Isbought==1;
	

	if (post.Posterid!=userid && !viewattach)
	{


	if (usergroupinfo.Radminid!=1)
	{

	templateBuilder.Append("\r\n		[<a onclick=\"loadbuyattach(");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append(");\" href=\"javascript:void(0);\">购买</a>] \r\n	  ");
	}	//end if


	}	//end if

	templateBuilder.Append("	\r\n	</p>\r\n");
	}	//end if

	templateBuilder.Append("\r\n	<p>");
	if (attachment.Description!="")
	{
	templateBuilder.Append(attachment.Description.ToString().Trim());
	}	//end if

	templateBuilder.Append("<span style=\"color:#666\">(");
	templateBuilder.Append(attachment.Postdatetime.ToString().Trim());
	templateBuilder.Append(" 上传)</span></p>\r\n	");
	if (attachment.Preview!="")
	{

	templateBuilder.Append("\r\n	<p>");
	templateBuilder.Append(attachment.Preview.ToString().Trim());
	templateBuilder.Append("</p>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<p>\r\n");
	if (post.Posterid==userid || usergroupinfo.Radminid==1)
	{

	 viewattach = true;
	

	}	//end if

	templateBuilder.Append("									\r\n	<a name=\"attach");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\"></a>\r\n");
	if (UserAuthority.DownloadAttachment(forum,userid,usergroupinfo))
	{

	templateBuilder.Append("<!--当用户有下载附件权限时-->\r\n	");
	if (config.Showimages==1)
	{


	if (attachment.Attachimgpost==1)
	{


	if (attachment.Attachprice<=0 || viewattach)
	{


	if (config.Showimgattachmode==0)
	{

	templateBuilder.Append("\r\n                        <img imageid=\"");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\" alt=\"");
	templateBuilder.Append(attachment.Attachment.ToString().Trim());
	templateBuilder.Append("\" \r\n                        ");
	if (config.Showattachmentpath==1)
	{


	if (attachment.Filename.IndexOf("http")>=0)
	{

	templateBuilder.Append("\r\n		                        src=\"");
	templateBuilder.Append(attachment.Filename.ToString().Trim());
	templateBuilder.Append("\"\r\n	                        ");
	}
	else
	{

	templateBuilder.Append("   \r\n		                        src=\"upload/");
	templateBuilder.Append(attachment.Filename.ToString().Trim());
	templateBuilder.Append("\"\r\n	                        ");
	}	//end if


	}
	else
	{

	templateBuilder.Append(" \r\n	                        src=\"attachment.aspx?attachmentid=");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\"\r\n                        ");
	}	//end if

	templateBuilder.Append("\r\n                        onmouseover=\"attachimg(this, 'mouseover')\" onload=\"attachimg(this, 'load');\" onclick=\"zoom(this, this.src);\" />\r\n                    ");
	}
	else
	{

	templateBuilder.Append("\r\n                        <img imageid=\"");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\" alt=\"点击加载图片\" \r\n                        ");
	if (config.Showattachmentpath==1)
	{


	if (attachment.Filename.IndexOf("http")>=0)
	{

	templateBuilder.Append("\r\n		                        newsrc=\"");
	templateBuilder.Append(attachment.Filename.ToString().Trim());
	templateBuilder.Append("\"\r\n	                        ");
	}
	else
	{

	templateBuilder.Append("   \r\n		                        newsrc=\"upload/");
	templateBuilder.Append(attachment.Filename.ToString().Trim());
	templateBuilder.Append("\"\r\n	                        ");
	}	//end if


	}
	else
	{

	templateBuilder.Append(" \r\n	                        newsrc=\"attachment.aspx?attachmentid=");
	templateBuilder.Append(attachment.Aid.ToString().Trim());
	templateBuilder.Append("\"\r\n                        ");
	}	//end if

	templateBuilder.Append("\r\n                            src=\"/images/common/imgloading.png\"\r\n                        onload=\"attachimg(this, 'load');\" onclick=\"loadImg(this);\" />\r\n                    ");
	}	//end if


	}	//end if


	}	//end if


	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n	<div class=\"hide\">\r\n	   附件:<em><span class=\"attachnotdown\">您需要<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx\" onclick=\"showWindow('login', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');return\">登录</a>才可以下载或查看附件。没有帐号? <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onClick=\"showWindow('register', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx');hideWindow('login');\">注册</a></span></em>\r\n	</div>    \r\n");
	}	//end if

	templateBuilder.Append("	\r\n	</p>\r\n	</dd>										\r\n</dl>");


	}
	else
	{


	if (userid>0)
	{

	templateBuilder.Append("\r\n							<div class=\"hide\"><em><span class=\"attachnotdown\">你的下载权限 ");
	templateBuilder.Append(usergroupinfo.Readaccess.ToString().Trim());
	templateBuilder.Append(" 低于此附件所需权限 ");
	templateBuilder.Append(attachment.Readperm.ToString().Trim());
	templateBuilder.Append(", 你无权查看此附件</span></em></div>\r\n							");
	}
	else
	{

	templateBuilder.Append("\r\n							<div class=\"hide\">附件: <em><span class=\"attachnotdown\">你需要<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx\" onclick=\"hideWindow('register');showWindow('login', this.href);\">登录</a>才可以下载或查看附件。没有帐号? <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onclick=\"hideWindow('login');showWindow('register', this.href);\" title=\"注册帐号\">注册</a></span></em></div>\r\n							");
	}	//end if


	}	//end if


	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n					</div>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n					    <div class=\"hide\">附件:<em><span class=\"attachnotdown\">您需要<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx\" onclick=\"hideWindow('register');showWindow('login', this.href);\">登录</a>才可以下载或查看附件。没有帐号? <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onClick=\"hideWindow('login');showWindow('register', this.href);\" title=\"注册帐号\">注册</a></span></em></div>\r\n						");
	}	//end if


	}	//end if


	}	//end if


	if (post.Ratetimes>0)
	{



	if (Discuz.Config.GeneralConfigs.GetConfig().DisplayRateCount>0)
	{

	templateBuilder.Append("\r\n<div class=\"newrate cl\" id=\"ratedd_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\">\r\n	<ul class=\"cl\" id=\"rate_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\"></ul>\r\n	<div class=\"floatwrap\" id=\"ratetable_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\"></div>\r\n	<p class=\"btn_rate\"><span class=\"f_bold\"><a title=\"本帖最近评分记录\" href=\"#\">已有<cite class=\"xi1\" style=\"padding:0 0.4em;\">");
	templateBuilder.Append(post.Ratetimes.ToString().Trim());
	templateBuilder.Append("</cite>评分</a></span><span class=\"xg2\"><a href=\"topicadmin.aspx?action=rate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&operat=rate\" onclick=\"showWindow('mods', this.href);return false;\">我要评分</a><a target=\"_blank\" href=\"showratelist.aspx?pid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"lightlink\">查看所有评分</a></span></p>\r\n	<script type=\"text/javascript\">\r\n		_attachEvent(window, \"load\", function(){ showrate(");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(post.Ratetimes.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",'");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("',");
	templateBuilder.Append(config.Ratelisttype.ToString().Trim());
	templateBuilder.Append("); });\r\n	</");
	templateBuilder.Append("script>\r\n</div>\r\n");
	}	//end if




	}	//end if


	if (post.Id==1)
	{

	templateBuilder.Append("\r\n                <!--悬赏部分-->\r\n				");
	if (topic.Special==2 || topic.Special==3)
	{

	templateBuilder.Append("				\r\n				<div class=\"special_reward cl\">\r\n                    ");
	if (topic.Special==2)
	{

	templateBuilder.Append("\r\n					<div class=\"rusld z\">\r\n                    ");
	}
	else
	{

	templateBuilder.Append("\r\n                    <div class=\"rsld z\">\r\n                    ");
	}	//end if

	templateBuilder.Append("\r\n						<cite>");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("</cite>");
	templateBuilder.Append(userextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append("\r\n					</div>\r\n					<div class=\"rwdn\">\r\n						<table cellspacing=\"0\" cellpadding=\"0\">\r\n							<tbody>\r\n							<tr>\r\n								<td id=\"postmessage_30\" class=\"t_f\">");
	templateBuilder.Append(post.Message.ToString().Trim());
	templateBuilder.Append("</td>\r\n							</tr>\r\n							</tbody>\r\n						</table>\r\n						<p class=\"pns ptm\"><button");
	if (canreply)
	{

	templateBuilder.Append(" onclick=\"showWindow('reply', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("');\"");
	}
	else
	{

	templateBuilder.Append(" onclick=\"showWindow('login', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');\"");
	}	//end if

	templateBuilder.Append(" class=\"pn\" value=\"ture\" name=\"answer\"><span>我来回答</span></button></p>\r\n					</div>\r\n				</div>\r\n				");
	}	//end if


	if (topic.Special==3)
	{

	templateBuilder.Append("\r\n                <div class=\"rwdbst\">\r\n                    ");	bool isshowbest = false;
	
	bool isshowvaluable = false;
	

	int bonuslog__loop__id=0;
	foreach(BonusLogInfo bonuslog in bonuslogs)
	{
		bonuslog__loop__id++;


	if (bonuslog.Isbest==2)
	{


	if (!isshowbest)
	{

	templateBuilder.Append("\r\n                    <h4 class=\"psth\" style=\"background-color:#fff4dd\">最佳答案</h4>\r\n                    ");	 isshowbest = true;
	

	}	//end if


	}
	else
	{


	if (!isshowvaluable)
	{

	templateBuilder.Append("\r\n                    <h4 class=\"psth\" style=\"background-color:#cce2f8\">有价值答案</h4>\r\n                    ");	 isshowvaluable = true;
	

	}	//end if


	}	//end if

	templateBuilder.Append("\r\n                    <div class=\"pstl\">\r\n                        <div class=\"psta\">\r\n                            ");	string answeravatarurl = Avatars.GetAvatarUrl(bonuslog.Answerid);
	
	templateBuilder.Append("\r\n                            <img src=\"");
	templateBuilder.Append(answeravatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_small.gif';\" />\r\n                        </div>\r\n                        <div class=\"psti\">\r\n                            <p class=\"xg2\">\r\n                                ");	 aspxrewriteurl = this.UserInfoAspxRewrite(bonuslog.Answerid);
	
	string unit = scoreunit[ bonuslog.Extid ];
	
	string name = score[ bonuslog.Extid ];
	
	templateBuilder.Append("\r\n                                <a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(bonuslog.Answername.ToString().Trim());
	templateBuilder.Append("</a>(");
	templateBuilder.Append(name.ToString());
	templateBuilder.Append(":");
	templateBuilder.Append(bonuslog.Bonus.ToString().Trim());
	templateBuilder.Append(unit.ToString());
	templateBuilder.Append(") \r\n                                <a onclick=\"window.open('showtopic.aspx?topicid=");
	templateBuilder.Append(bonuslog.Tid.ToString().Trim());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(bonuslog.Pid.ToString().Trim());
	templateBuilder.Append("#");
	templateBuilder.Append(bonuslog.Pid.ToString().Trim());
	templateBuilder.Append("')\" href=\"javascript:;\">查看完整内容</a>\r\n                            </p>\r\n                            <div class=\"mtn\">");	templateBuilder.Append(Utils.GetUnicodeSubString(bonuslog.Message,100,"..."));
	templateBuilder.Append("</div>\r\n                        </div>\r\n                    </div>\r\n                    ");
	}	//end loop

	templateBuilder.Append("\r\n                </div>\r\n				");
	}
	else if (topic.Special==4)
	{

	templateBuilder.Append("\r\n				<div class=\"debate_show\">\r\n					<div class=\"squaretitle\">\r\n						<p>正方观点</p>\r\n						");
	templateBuilder.Append(debateexpand.Positiveopinion.ToString().Trim());
	templateBuilder.Append("\r\n					</div>\r\n					<div class=\"sidetitle\">\r\n						<p>反方观点</p>\r\n						");
	templateBuilder.Append(debateexpand.Negativeopinion.ToString().Trim());
	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<!--投票部分-->\r\n				");

	if (topic.Special==1)
	{

	templateBuilder.Append("\r\n<!--投票区开始-->\r\n<div class=\"pollpanel\">\r\n	<h4>\r\n	投票：<strong>");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</strong>\r\n	");
	if (pollinfo.Multiple==1)
	{

	templateBuilder.Append("\r\n	- 多选(最多可选");
	templateBuilder.Append(pollinfo.Maxchoices.ToString().Trim());
	templateBuilder.Append("项)\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	- 截止时间：");
	templateBuilder.Append(pollinfo.Expiration.ToString().Trim());
	templateBuilder.Append("\r\n	</h4>\r\n	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" summary=\"pollpanel\">\r\n	");
	int polloption__loop__id=0;
	foreach(DataRow polloption in Polls.GetPollOptionList(topicid).Rows)
	{
		polloption__loop__id++;

	templateBuilder.Append("\r\n	<tbody>\r\n	<tr>\r\n		");
	if (allowvote)
	{

	templateBuilder.Append("\r\n		<td class=\"selector\">\r\n			");
	if (pollinfo.Multiple==1)
	{

	templateBuilder.Append("\r\n				<input type=\"checkbox\" name=\"pollitemid\" value=\"" + polloption["polloptionid"].ToString().Trim() + "\" onclick='checkbox(this)'/>\r\n			");
	}
	else
	{

	templateBuilder.Append("\r\n				<input type=\"radio\" name=\"pollitemid\"  value=\"" + polloption["polloptionid"].ToString().Trim() + "\"  />\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<td colspan=\"2\">\r\n			" + polloption__loop__id.ToString() + ". " + polloption["name"].ToString().Trim() + "\r\n		</td>\r\n	</tr>\r\n	");
	if (showpollresult)
	{

	templateBuilder.Append("\r\n	<tr>\r\n		");
	if (allowvote)
	{

	templateBuilder.Append("\r\n		<td> </td>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<td class=\"optionvessel\">\r\n			<div class=\"optionbar\">\r\n			");	int styleid = polloption__loop__id % 10;
	
	templateBuilder.Append("\r\n				<div class=\"polloptionbar ");
	if (polloption["value"].ToString().Trim()!="0")
	{

	templateBuilder.Append("pollcolor");
	templateBuilder.Append(styleid.ToString());
	}	//end if

	templateBuilder.Append("\" style=\"width:" + polloption["percentwidth"].ToString().Trim() + "px\"></div>\r\n			</div>\r\n		</td>\r\n		<td>\r\n			<strong>" + polloption["value"].ToString().Trim() + "</strong>票 / " + polloption["percent"].ToString().Trim() + "\r\n		</td>\r\n	</tr>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</tbody>\r\n	");
	}	//end loop

	templateBuilder.Append("\r\n	<script language=\"javascript\">\r\n	var max_obj = ");
	templateBuilder.Append(pollinfo.Maxchoices.ToString().Trim());
	templateBuilder.Append(";\r\n	var p = 0;\r\n	\r\n	function checkbox(obj) {\r\n		if(obj.checked) {\r\n			p++;\r\n			for (var i = 0; i < $('postsform').elements.length; i++) {\r\n				var e = $('postsform').elements[i];\r\n				if(p == max_obj) {\r\n					if(e.name.match('pollitemid') && !e.checked) {\r\n						e.disabled = true;\r\n					}\r\n				}\r\n			}\r\n		} else {\r\n			p--;\r\n			for (var i = 0; i < $('postsform').elements.length; i++) {\r\n				var e = $('postsform').elements[i];\r\n				if(e.name.match('pollitemid') && e.disabled) {\r\n					e.disabled = false;\r\n				}\r\n			}\r\n		}\r\n	}	   \r\n	 \r\n	function displayvoter(objid) {\r\n		if(objid.style.display == 'block') {\r\n		   objid.style.display = 'none';\r\n		}\r\n		else {\r\n		   objid.style.display = 'block';\r\n		}\r\n	}	    \r\n	</");
	templateBuilder.Append("script>\r\n	<tr>\r\n		<td colspan=\"2\">\r\n			");
	if (usergroupinfo.Allowvote==1)
	{


	if (allowvote)
	{

	templateBuilder.Append("\r\n					<button  name=\"Submit\" onclick=\"$('postsform').action='poll.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("';$('postsform').submit();\" >马上投票</button>\r\n				");
	}
	else
	{

	templateBuilder.Append("							\r\n					提示: 您已经投过票或者投票已经过期\r\n				");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n				抱歉,您所在的用户组没有参与投票的权限,请注册或登录!\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		 </td>\r\n	</tr>\r\n	");
	if (voters!=""&&(pollinfo.Allowview==1||pollinfo.Uid==userid||ismoder==1))
	{

	templateBuilder.Append("   \r\n	<tbody>\r\n	<tr>\r\n		<td colspan=\"2\">\r\n			<button type=\"button\" onclick=\"showWindow('mods', 'misc.aspx?action=viewvote&tid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("');return false;\" class=\"pn\"/><span>查看投票用户名单>></span></button>\r\n		</td>\r\n	</tr>\r\n	</tbody>\r\n	");
	}	//end if


	if (showpollresult&&config.Silverlight==1)
	{

	templateBuilder.Append("\r\n	<tbody>\r\n		<tr>\r\n		<td colspan=\"2\"><iframe src=\"silverlight/piechart/index.html?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&bg=FFFFFF\" allowtransparency=\"yes\" width=\"95%\" height=\"425\" border=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\" frameborder=\"0\" ></iframe>\r\n		</td>\r\n		</tr>\r\n	</tbody>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</table>\r\n</div>\r\n");
	}	//end if




	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n		</td>\r\n	</tr>\r\n	<tr>\r\n		<td class=\"plc\">\r\n		");
	if (post.Lastedit!="")
	{

	templateBuilder.Append("\r\n		<div class=\"lastediter\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/lastedit.gif\" alt=\"最后编辑\"/>");
	templateBuilder.Append(post.Lastedit.ToString().Trim());
	templateBuilder.Append("</div>\r\n		");
	}	//end if


	if (post.Id==1)
	{


	if (topic.Moderated>0 && config.Moderactions>0)
	{

	templateBuilder.Append("\r\n		<div class=\"manageinfo\">");
	templateBuilder.Append(TopicAdmins.GetTopicListModeratorLog(topicid).ToString().Trim());
	templateBuilder.Append("</div>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<div class=\"useraction\">\r\n		");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n			<a href=\"favorites.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&infloat=1\" onclick=\"ajaxmenu(event, this.id, 3000, 0)\" id=\"ajax_favorite\">收藏</a>\r\n			");
	if (usergroupinfo.Raterange!="" && post.Posterid!=-1)
	{

	templateBuilder.Append("\r\n			<a id=\"ratelink\" href=\"javascript:;\" onclick=\"showWindow('mods', 'topicadmin.aspx?action=rate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&operat=rate');return false;\">评分</a>\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("				\r\n			<a onclick=\"showWindow('mods', this.href);return false;\" href=\"misc.aspx?action=emailfriend&tid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" id=\"share\">分享</a>\r\n			");
	if (config.Disableshare==1)
	{

	templateBuilder.Append("\r\n			<script type=\"text/javascript\">\r\n			function openforward()\r\n			{\r\n			  share.floatwin('");
	templateBuilder.Append(config.Sharelist.ToString().Trim());
	templateBuilder.Append("');\r\n			}\r\n			</");
	templateBuilder.Append("script>\r\n			<a href=\"javascript:void(0)\" onclick=\"openforward()\" id=\"forward\">转发</a>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n		");
	}	//end if


	if (post.Invisible!=-2 || ismoder==1)
	{


	if (config.Showsignatures==1)
	{


	if (post.Usesig==1)
	{


	if (post.Signature!="")
	{

	templateBuilder.Append("\r\n			<!--签名开始-->\r\n			<div class=\"postertext\">\r\n				");
	if (config.Maxsigrows>0)
	{

	int ieheight = config.Maxsigrows*19;
	
	float heightem = config.Maxsigrows*1.5f;
	
	templateBuilder.Append("\r\n					<div class=\"signatures\" style=\"max-height:");
	templateBuilder.Append(heightem.ToString());
	templateBuilder.Append("em;maxHeightIE:");
	templateBuilder.Append(ieheight.ToString());
	templateBuilder.Append("px\">");
	templateBuilder.Append(post.Signature.ToString().Trim());
	templateBuilder.Append("</div>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n					");
	templateBuilder.Append(post.Signature.ToString().Trim());
	templateBuilder.Append("\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			<!--签名结束-->\r\n			");
	}	//end if


	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n	</tr>\r\n	<tr>\r\n		<td class=\"plc\"><div id=\"ad_thread1_" + post__loop__id.ToString() + "\"></div></td>\r\n	</tr>\r\n	<tr>\r\n		<td class=\"postauthor\"></td>\r\n		<td class=\"postactions\">\r\n			<div class=\"p_control\">\r\n			<cite class=\"y\">\r\n			");
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

	templateBuilder.Append("<span class=\"pipe\">|</span>\r\n				");
	if (usergroupinfo.Raterange!="" && post.Posterid!=-1)
	{


	if (post.Layer!=0)
	{

	templateBuilder.Append("\r\n				<a href=\"topicadmin.aspx?action=rate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&operat=rate\" onclick=\"showWindow('mods', this.href);return false;\">评分</a><span class=\"pipe\">|</span>\r\n				");
	}	//end if


	}	//end if


	}	//end if


	if (ismoder==1)
	{


	if (post.Ratetimes>0)
	{

	templateBuilder.Append("\r\n				<a href=\"topicadmin.aspx?action=cancelrate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"showWindow('mods', this.href,'get',-1);return false;\">撤销评分</a><span class=\"pipe\">|</span>\r\n				");
	}	//end if


	if (post.Id==1 && topic.Special==2)
	{


	if (topic.Replies>0)
	{

	templateBuilder.Append("						\r\n				<a href=\"topicadmin.aspx?action=bonus&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&operat=bonus\" onclick=\"showWindow('mods', this.href);return false;\">结帖</a><span class=\"pipe\">|</span>\r\n					");
	}	//end if


	}	//end if


	}
	else
	{


	if (post.Posterid!=-1 && userid==post.Posterid)
	{


	if (post.Id==1 && topic.Special==2)
	{


	if (topic.Replies>0)
	{

	templateBuilder.Append("\r\n				<a href=\"topicadmin.aspx?action=bonus&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("&operat=bonus\" onclick=\"showWindow('mods', this.href);return false;\">结帖</a><span class=\"pipe\">|</span>\r\n						");
	}	//end if


	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			<a href=\"#\" onclick=\"window.scrollTo(0,0)\">TOP</a>\r\n			");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n			<label for=\"manage");
	templateBuilder.Append(post.Id.ToString().Trim());
	templateBuilder.Append("\">\r\n				<input type=\"checkbox\" value=\"");
	templateBuilder.Append(post.Id.ToString().Trim());
	templateBuilder.Append("\" onclick=\"pidchecked(this);modclick(this, ");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append(")\" id=\"manage");
	templateBuilder.Append(post.Id.ToString().Trim());
	templateBuilder.Append("\" class=\"checkbox\"/>管理            </label>\r\n            ");
	}	//end if

	templateBuilder.Append("\r\n			</cite>\r\n		");
	if (canreply)
	{


	if (userid!=-1)
	{

	templateBuilder.Append("\r\n			    <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postlayer=");
	templateBuilder.Append(post.Id.ToString().Trim());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&poster=");
	templateBuilder.Append(Utils.UrlEncode(post.Poster).ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\" ");
	if (!isnewbie)
	{

	templateBuilder.Append(" onclick=\"showWindow('reply', 'showtopic.aspx?poster=");
	templateBuilder.Append(Utils.UrlEncode(post.Poster).ToString().Trim());
	templateBuilder.Append("&postlayer=");
	templateBuilder.Append(post.Id.ToString().Trim());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("')\"");
	}	//end if

	templateBuilder.Append(" class=\"fastreply\">回复</a>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<a href=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&quote=yes\" class=\"repquote\">引用</a>\r\n		");
	}	//end if


	if (ismoder==1)
	{


	if (topic.Special==4)
	{

	templateBuilder.Append("\r\n			<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("&debate=");
	templateBuilder.Append(post.Debateopinion.ToString().Trim());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n			");
	}
	else
	{

	templateBuilder.Append("\r\n			<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\"  class=\"editpost\">编辑</a>\r\n			 ");
	}	//end if


	if (post.Posterid!=-1 && userid==post.Posterid)
	{

	templateBuilder.Append("\r\n			<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\" title=\"删除我的帖子\">删除</a>\r\n			");
	}	//end if


	}
	else
	{


	if (post.Posterid!=-1 && userid==post.Posterid)
	{


	if (topic.Closed==0)
	{


	if (topic.Special==4)
	{

	templateBuilder.Append("\r\n					<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&debate=");
	templateBuilder.Append(post.Debateopinion.ToString().Trim());
	templateBuilder.Append("\"  class=\"editpost\">编辑</a>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("&pageid=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\"   class=\"editpost\">编辑</a>\r\n					 ");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\" title=\"删除我的帖子\">删除</a>\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</div>		</td>\r\n	</tr>\r\n	</tbody>\r\n	<tbody>\r\n	<tr class=\"threadad\">\r\n		<td class=\"postauthor\"></td>\r\n		<td class=\"adcontent\">\r\n			");
	if (post.Id==1 && postleaderboardad!="")
	{

	templateBuilder.Append("\r\n			<div id=\"postleaderboardad\">");
	templateBuilder.Append(postleaderboardad.ToString());
	templateBuilder.Append("</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n	</tr>\r\n	</tbody>\r\n	</table>\r\n	");	 loopi = loopi+1;
	

	}	//end loop

	templateBuilder.Append("\r\n	</div>\r\n	</form>\r\n	<!--ntforumbox end-->\r\n	<div class=\"forumcontrol cl\">\r\n	<table cellspacing=\"0\" cellpadding=\"0\" class=\"narrow\">\r\n		<tbody>\r\n		<tr>\r\n		<td class=\"postauthor\">\r\n			<a href=\"showtopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&go=prev\">上一主题</a><span class=\"pipe\">|</span>\r\n			<a href=\"showtopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&go=next\">下一主题</a>\r\n		</td>\r\n		<td class=\"modaction\">\r\n			");
	if (useradminid>0||usergroupinfo.Raterange!=""||config.Forumjump==1||(topic.Special==2&&topic.Posterid==userid))
	{

	templateBuilder.Append("\r\n			<script type=\"text/javascript\">\r\n				function action_onchange(value,objfrm,postid,banstatus){\r\n					if (value != ''){\r\n						objfrm.operat.value = value;\r\n						objfrm.postid.value = postid;\r\n						if (value != \"delete\")\r\n						{\r\n							objfrm.action = objfrm.action + '&referer=' + escape(window.location);\r\n						}\r\n						if (value == 'banpost' && typeof(banstatus) != \"undefined\")\r\n						{\r\n							objfrm.operat.value = value;\r\n							objfrm.action = objfrm.action + \"&banstatus=\" + banstatus;\r\n							objfrm.submit();\r\n							return;\r\n						}\r\n						if(value == 'delposts' || value == 'banpost'){\r\n							$('postsform').operat.value = value; \r\n							$('postsform').action = $('postsform').action + '&referer=' + escape(window.location);\r\n							$('postsform').submit();\r\n						}\r\n						else{\r\n							objfrm.submit();\r\n						}\r\n					}\r\n				}\r\n			</");
	templateBuilder.Append("script>\r\n			");	 canuseadminfunc = usergroupinfo.Raterange!="" || usergroupinfo.Maxprice>0 || (topic.Special==2&&topic.Posterid==userid);
	

	if (useradminid>0)
	{

	templateBuilder.Append("\r\n				<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"topicadmin.aspx?action=moderate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&infloat=1\">\r\n					<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n					<input name=\"topicid\" type=\"hidden\" value=\"");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" />\r\n					<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n					<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n					<input type=\"hidden\" name=\"winheight\" />\r\n					<input type=\"hidden\" name=\"optgroup\" />\r\n					");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n					<span class=\"drop xg2\" onclick=\"showMenu({'ctrlid':this.id, 'pos':'21'})\" id=\"operatSel\">主题管理</span>\r\n					<ul style=\"width: 180px; display:none;\" id=\"operatSel_menu\" class=\"p_pop inlinelist\">\r\n						<li><a onclick=\"modthreads(1, 'delete');return false;\" href=\"###\">删除</a></li>\r\n						<li><a onclick=\"modthreads(1, 'bump');return false;\" href=\"###\">提沉</a></li>\r\n						<li><a onclick=\"modthreads(1, 'close');return false;\" href=\"###\">关闭</a></li>\r\n						<li><a onclick=\"modthreads(1, 'move');return false;\" href=\"###\">移动</a></li>\r\n						<li><a onclick=\"modthreads(1, 'copy');return false;\" href=\"###\">复制</a></li>\r\n						<li><a onclick=\"modthreads(1, 'highlight');return false;\" href=\"###\">高亮</a></li>\r\n						<li><a onclick=\"modthreads(1, 'digest');return false;\" href=\"###\">精华</a></li>\r\n						<li><a onclick=\"modthreads(1, 'identify');return false;\" href=\"###\">鉴定</a></li>\r\n						<li><a onclick=\"modthreads(1, 'displayorder');return false;\" href=\"###\">置顶</a></li>\r\n						<li><a onclick=\"modthreads(1, 'split');return false;\" href=\"###\">分割</a></li>\r\n						<li><a onclick=\"modthreads(1, 'merge');return false;\" href=\"###\">合并</a></li>\r\n						<li><a onclick=\"modthreads(1, 'repair');return false;\" href=\"###\">修复</a></li>\r\n						<li><a onclick=\"modthreads(1, 'type');return false;\" href=\"###\">分类</a></li>\r\n					</ul>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n				</form>\r\n			");
	}
	else if (canuseadminfunc)
	{

	templateBuilder.Append("\r\n				<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"topicadmin.aspx?action=moderate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\"  class=\"y\">\r\n					<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n					<input name=\"topicid\" type=\"hidden\" value=\"");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" />\r\n					<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n					<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n				</form>\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n		</tr>\r\n		</tbody>\r\n	</table>\r\n	</div>\r\n</div>\r\n<div class=\"pages_btns cl\">\r\n	<div class=\"pages\">\r\n		<cite class=\"pageback\">");
	templateBuilder.Append(listlink.ToString());
	templateBuilder.Append("</cite>\r\n		");
	if (pagecount!=1)
	{

	templateBuilder.Append("\r\n		");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n		");
	if (pagecount>8)
	{

	templateBuilder.Append("\r\n		<kbd>\r\n		<input name=\"gopage\" type=\"text\" class=\"txt\" id=\"pageidinput2\" title=\"可以输入页码按回车键自动跳转\" value=\"");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" style=\"text-align:center;\" onfocus=\"this.value=this.defaultValue;this.select();\" onKeyDown=\"pageinputOnKeyDown(this,event);\" size=\"2\" maxlength=\"9\" />/ ");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("</kbd>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		");
	templateBuilder.Append(nextpage.ToString());
	templateBuilder.Append("\r\n		");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n");
	if (userid<0||canposttopic)
	{

	string newtopicurl = "";
	

	if (forum.Allowspecialonly<=0)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&forumpage=" + pageid;
	

	}
	else if (1==(forum.Allowpostspecial&1)&&usergroupinfo.Allowpostpoll==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=poll&forumpage=" + pageid;
	

	}
	else if (4==(forum.Allowpostspecial&4)&&usergroupinfo.Allowbonus==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=bonus&forumpage=" + pageid;
	

	}
	else if (16==(forum.Allowpostspecial&16)&&usergroupinfo.Allowdebate==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=debate&forumpage=" + pageid;
	

	}	//end if

	string newtopiconclick = "";
	

	if (forum.Allowspecialonly<=0&&canposttopic)
	{

	 newtopiconclick = "showWindow('newthread', '" + forumpath + "showforum.aspx?forumid=" + forum.Fid + "')";
	

	}	//end if

	bool allowpost = userid<=0&&(forum.Postperm==""?usergroupinfo.Allowpost==0:!Utils.InArray(usergroupid.ToString(),forum.Postperm));
	

	if (allowpost)
	{

	 newtopiconclick = "showWindow('login', '" + forumpath + "login.aspx');hideWindow('register');";
	

	}
	else
	{

	 newtopiconclick = "showWindow('newthread', '" + forumpath + "showforum.aspx?forumid=" + forum.Fid + "')";
	

	}	//end if

	templateBuilder.Append("\r\n	<span ");
	if (userid>0)
	{

	templateBuilder.Append(" onmouseover=\"if($('newspecial_menu')!=null&&$('newspecial_menu').childNodes.length>0)  showMenu(this.id);\"");
	}	//end if

	templateBuilder.Append(" id=\"newspecial2\" class=\"postbtn\">\r\n        <a title=\"发新话题\" id=\"newtopic2\" href=\"");
	templateBuilder.Append(newtopicurl.ToString());
	templateBuilder.Append("\" onclick=\"");
	templateBuilder.Append(newtopiconclick.ToString());
	templateBuilder.Append("\">\r\n            <img alt=\"发新话题\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/newtopic.png\"/></a>\r\n    </span>\r\n");
	}	//end if


	if ((topic.Closed!=1||ismoder==1) && (userid<0||canreply))
	{

	templateBuilder.Append("\r\n	<span class=\"replybtn\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\"");
	if (canreply)
	{

	templateBuilder.Append(" onclick=\"showWindow('reply', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("');\"");
	}
	else
	{

	templateBuilder.Append(" onclick=\"showWindow('login', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');\"");
	}	//end if

	templateBuilder.Append("><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/reply.png\" alt=\"回复该主题\" /></a></span>\r\n");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n\r\n");
	if (config.Fastpost==2||config.Fastpost==3)
	{


	if ((topic.Closed!=1||ismoder==1) && (userid<0||canreply))
	{


	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/bbcode.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\"></");
	templateBuilder.Append("script>\r\n");	string seditorid = "";
	
	string poster = DNTRequest.GetString("poster");
	
	int postlayer = DNTRequest.GetInt("postlayer",0);
	
	string urlreferrer = DNTRequest.GetUrlReferrer();
	

	if (infloat!=1)
	{

	 seditorid = "quickpost";
	

	}	//end if


	if (infloat==1)
	{

	templateBuilder.Append("\r\n<form method=\"post\" name=\"postform\" id=\"form\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&poster=");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("&postlayer=");
	templateBuilder.Append(postlayer.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" >\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n<form method=\"post\" name=\"postform\" id=\"quickpostform\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&poster=");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("&postlayer=");
	templateBuilder.Append(postlayer.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" >\r\n");
	}	//end if

	templateBuilder.Append("\r\n<div id=\"quickpost\" class=\"");
	if (infloat!=1)
	{

	templateBuilder.Append("main");
	}	//end if

	templateBuilder.Append(" quickpost\">\r\n	");
	if (infloat==1)
	{

	templateBuilder.Append("\r\n	<h3 class=\"flb\">\r\n		<span class=\"y\">\r\n			<a title=\"关闭\" onclick=\"hideWindow('reply')\" class=\"flbc\" href=\"javascript:;\">关闭</a>\r\n		</span><em>无刷新回复主题</em>\r\n	</h3>\r\n	");
	}	//end if


	if (infloat!=1)
	{

	templateBuilder.Append("\r\n	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"无刷新快速回复\" >\r\n	<tbody>\r\n		<tr>\r\n		<td class=\"postauthor\">\r\n			<div class=\"avatar\">	\r\n			");
	if (canreply||userid>0)
	{

	string avatarurl = Avatars.GetAvatarUrl(userid);
	
	templateBuilder.Append("	\r\n			<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_medium.gif';\" alt=\"回复者\"/>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n		</td>\r\n		<td class=\"postcontent\">\r\n	");
	}	//end if

	templateBuilder.Append("\r\n			<div class=\"post_inner c cl\">\r\n			");
	if (quickeditorad!="")
	{

	templateBuilder.Append("\r\n			<div class=\"leaderboard\">");
	templateBuilder.Append(quickeditorad.ToString());
	templateBuilder.Append("</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<em id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("validatemessage\">");
	if (infloat==1)
	{

	templateBuilder.Append("参与/回复主题");
	}	//end if

	templateBuilder.Append("</em>\r\n			<div class=\"pbt\">\r\n				<input type=\"text\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title\" name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" size=\"84\" tabindex=\"1\" value=\"\" style=\"display:none;\" />\r\n				<input type=\"hidden\" id=\"postlayer\" name=\"postlayer\" value=\"-1\" />\r\n				<input type=\"hidden\" id=\"postid\" name=\"postid\" value=\"");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("\" />\r\n				<span style=\"display:none\">\r\n				<input type=\"checkbox\" value=\"1\" name=\"htmlon\" id=\"htmlon\" ");
	if (usergroupinfo.Allowhtml!=1)
	{

	templateBuilder.Append(" disabled");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"parseurloff\" id=\"parseurloff\" value=\"1\" ");
	if (parseurloff==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"smileyoff\" id=\"smileyoff\" value=\"1\" ");
	if (smileyoff==1)
	{

	templateBuilder.Append(" checked disabled ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"bbcodeoff\" id=\"bbcodeoff\" value=\"1\" ");
	if (bbcodeoff==1)
	{

	templateBuilder.Append(" checked disabled ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"usesig\" id=\"usesig\" value=\"1\" ");
	if (usesig==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" ");
	if (replyemailstatus==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"postreplynotice\" id=\"postreplynotice\" ");
	if (replynotificationstatus==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				</span>\r\n				<script type=\"text/javascript\">\r\n					var bbinsert = parseInt('1');\r\n					var smiliesCount = 24;\r\n					var colCount = 8;\r\n				</");
	templateBuilder.Append("script>\r\n				");	char comma = ',';
	

	if (infloat==1)
	{

	templateBuilder.Append("\r\n				<div class=\"pbt cl\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title_text\">RE:");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("<a href=\"javascript:void(0)\" onclick=\"modifytitle();\" class=\"xg2\" style=\"margin-left:10px;\">修改</a></div>\r\n				<script type=\"text/javascript\">\r\n				function modifytitle(){\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title_text').style.display = 'none';\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title').style.display = '';\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title').ClassName = 'txt postpx';\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title').value = 'RE:");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("';\r\n				}\r\n				</");
	templateBuilder.Append("script>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			");
	if (poster!="")
	{

	templateBuilder.Append("\r\n			<div class=\"pbt cl\" id=\"toreplay_div\">\r\n			<strong>回复 <a target=\"_blank\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("#");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(postlayer.ToString());
	templateBuilder.Append("楼<font color=\"Olive\">");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("</font>的帖子</a></strong>\r\n			</div>\r\n			<input type=\"hidden\" name=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("toreplay_user\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("toreplay_user\"  value=\"\"/>	\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<div ");
	if (infloat!=1)
	{

	templateBuilder.Append("style=\"margin-right:170px;\" ");
	}
	else
	{

	templateBuilder.Append("style=\"width:600px;\"");
	}	//end if

	templateBuilder.Append(">\r\n			<div class=\"pbt\">\r\n				");
	templateBuilder.Append("<link href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/seditor.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<div class=\"editor_tb\">\r\n	<span class=\"y\">\r\n		");
	if (topicid>0)
	{

	string replyurl = rooturl+"postreply.aspx?topicid="+topicid+"&forumpage="+forumpageid;
	

	if (postid>0)
	{

	 replyurl = replyurl+"&postid="+postid+"&postlayer="+postlayer+"&poster="+Utils.UrlEncode(poster);
	

	}	//end if

	templateBuilder.Append("\r\n		    <a onclick=\"switchAdvanceMode(this.href, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');doane(event);\" href=\"");
	templateBuilder.Append(replyurl.ToString());
	templateBuilder.Append("\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/external2.png\" alt=\"高级编辑器\" class=\"vm\"/>高级编辑器</a>\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n		    <a onclick=\"switchAdvanceMode(this.href, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');doane(event);\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/external2.png\" alt=\"高级编辑器\" class=\"vm\"/>高级编辑器</a>\r\n		");
	}	//end if


	if (infloat!=1)
	{


	if (userid<0||canposttopic)
	{

	string newtopicurl = "";
	

	if (forum.Allowspecialonly<=0)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&forumpage=" + pageid;
	

	}
	else if (1==(forum.Allowpostspecial&1)&&usergroupinfo.Allowpostpoll==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=poll&forumpage=" + pageid;
	

	}
	else if (4==(forum.Allowpostspecial&4)&&usergroupinfo.Allowbonus==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=reward&forumpage=" + pageid;
	

	}
	else if (16==(forum.Allowpostspecial&16)&&usergroupinfo.Allowdebate==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=debate&forumpage=" + pageid;
	

	}	//end if

	string newtopiconclick = "";
	

	if (forum.Allowspecialonly<=0&&canposttopic)
	{

	 newtopiconclick = "showWindow('newthread', '" + forumpath + "showforum.aspx?forumid=" + forum.Fid + "')";
	

	}	//end if


	if (userid<=0)
	{

	 newtopiconclick = "showWindow('login', '" + forumpath + "login.aspx');hideWindow('register');";
	

	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	</span>\r\n	<div>\r\n		<a href=\"javascript:;\" title=\"粗体\" class=\"tb_bold\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[b]', '[/b]')\">B</a>\r\n		<a href=\"javascript:;\" title=\"颜色\" class=\"tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor\" onclick=\"showMenu(this.id, true, 0, 2)\">Color</a>\r\n		");	string coloroptions = "Black,Sienna,DarkOliveGreen,DarkGreen,DarkSlateBlue,Navy,Indigo,DarkSlateGray,DarkRed,DarkOrange,Olive,Green,Teal,Blue,SlateGray,DimGray,Red,SandyBrown,YellowGreen,SeaGreen,MediumTurquoise,RoyalBlue,Purple,Gray,Magenta,Orange,Yellow,Lime,Cyan,DeepSkyBlue,DarkOrchid,Silver,Pink,Wheat,LemonChiffon,PaleGreen,PaleTurquoise,LightBlue,Plum,White";
	
	templateBuilder.Append("\r\n		<div class=\"popupmenu_popup tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor_menu\" style=\"display: none\">\r\n			");
	int colorname__loop__id=0;
	foreach(string colorname in coloroptions.Split(comma))
	{
		colorname__loop__id++;

	templateBuilder.Append("\r\n				<input type=\"button\" style=\"background-color: ");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[color=");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("]', '[/color]')\" />");
	if (colorname__loop__id%8==0)
	{

	templateBuilder.Append("<br />");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n		</div>\r\n		<a href=\"javascript:;\" title=\"图片\" class=\"tb_img\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("img\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'img')\">Image</a>\r\n		<a href=\"javascript:;\" title=\"链接\" class=\"tb_link\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("url\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'url')\">Link</a>\r\n		<a href=\"javascript:;\" title=\"引用\" class=\"tb_quote\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[quote]', '[/quote]')\">Quote</a>\r\n		<a href=\"javascript:;\" title=\"代码\" class=\"tb_code\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[code]', '[/code]')\">Code</a>\r\n	");
	if (config.Smileyinsert==1 && forum.Allowsmilies==1)
	{

	templateBuilder.Append("\r\n		<a href=\"javascript:;\" class=\"tb_smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies\" onclick=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback);showMenu({'ctrlid':this.id, 'evt':'click', 'layer':2})\">Smilies</a>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n</div>\r\n");
	if (config.Smileyinsert==1 && forum.Allowsmilies==1)
	{

	templateBuilder.Append("\r\n	<div class=\"smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies_menu\" style=\"display:none;width:315px;\">\r\n		<div class=\"smilieslist\">\r\n			");	string defaulttypname = string.Empty;
	
	templateBuilder.Append("\r\n			<div id=\"smiliesdiv\">\r\n				<div class=\"smiliesgroup\" style=\"margin-right: 0pt;\">\r\n					<ul>\r\n					");
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

	templateBuilder.Append("\r\n						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" class=\"current\">" + stype["code"].ToString().Trim() + "</a></li>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">" + stype["code"].ToString().Trim() + "</a></li>\r\n						");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n					</ul>\r\n				 </div>\r\n				 <div style=\"clear: both;\" class=\"float_typeid\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie\"></div>\r\n				 <table class=\"smilieslist_table\" id=\"s_preview_table\" style=\"display: none\"><tr><td class=\"smilieslist_preview\" id=\"s_preview\"></td></tr></table>\r\n				 <div id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie_pagenum\" class=\"smilieslist_page\">&nbsp;</div>\r\n			</div>\r\n		</div>\r\n		<script type=\"text/javascript\" reload=\"1\">\r\n			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(func){\r\n				if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML !='' && $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML != '正在加载表情...')\r\n					return;\r\n				var c = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=smilies\";\r\n				_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true);\r\n				setTimeout(\"if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML=='')$('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML = '正在加载表情...'\", 2000);\r\n			}\r\n			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback(obj) {\r\n				smilies_HASH = obj; \r\n				showsmiles(1, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\r\n			}\r\n		</");
	templateBuilder.Append("script>\r\n	</div>\r\n");
	}	//end if



	templateBuilder.Append("\r\n				<div class=\"postarea cl\">\r\n					<div class=\"postinner\">\r\n					");
	if (canreply)
	{


	if (infloat==1)
	{

	templateBuilder.Append("\r\n					  <textarea rows=\"7\" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"message\" tabindex=\"3\" style=\"background:url(" + quickbgad[1].ToString().Trim() + ") no-repeat 50% 50%;\" ");
	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"");
	}	//end if

	templateBuilder.Append(" onkeydown=\"ajaxctlent(event, this.form, ");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(", isendpage, '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("',");
	templateBuilder.Append(hide.ToString());
	templateBuilder.Append(");\"></textarea>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					  <textarea rows=\"5\" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"quickpostmessage\" tabindex=\"6\"  style=\"background-image:url(" + quickbgad[1].ToString().Trim() + ");background-repeat:no-repeat;background-position:50% 50%;\" ");
	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"");
	}	//end if

	templateBuilder.Append(" onkeydown=\"ajaxctlent(event, this.form, ");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(", isendpage, '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("',");
	templateBuilder.Append(hide.ToString());
	templateBuilder.Append(");\"></textarea>\r\n					");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n					<div class=\"hm p_login cl\">你需要登录后才可以发帖 <a class=\"xg2\" onclick=\"hideWindow('register');showWindow('login', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx\">登录</a> | <a class=\"xg2\" onclick=\"hideWindow('login');showWindow('register', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\">注册</a></div>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n			</div>\r\n			</div>\r\n			");
	if (isseccode)
	{

	templateBuilder.Append("\r\n			<div class=\"pbt\" style=\"position: relative;\">\r\n				");
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

	templateBuilder.Append("\r\n			</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<div class=\"pbt\">\r\n				");
	if (topic.Special==4 && isenddebate==false)
	{

	templateBuilder.Append("\r\n				<div class=\"ftid\">\r\n					<select name=\"debateopinion\" id=\"debateopinion\">\r\n						<option value=\"0\" selected>辩论观点</option>\r\n						<option value=\"1\">正方</option>\r\n						<option value=\"2\">反方</option>\r\n					</select>\r\n				</div>\r\n				<script type=\"text/javascript\">simulateSelect('debateopinion');</");
	templateBuilder.Append("script>\r\n				");
	}	//end if


	if (canreply)
	{

	templateBuilder.Append("\r\n					<button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"replysubmit\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"5\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"8\"");
	}	//end if

	templateBuilder.Append(" onclick=\"ajaxreply(this.form, ");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(", isendpage, '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("', false,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("',");
	templateBuilder.Append(hide.ToString());
	templateBuilder.Append(");\" class=\"pn\"><span>发表回复</span></button><span class=\"grayfont\">[Ctrl+Enter快速发布]</span>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				    <button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"replysubmit\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"5\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"8\"");
	}	//end if

	templateBuilder.Append(" onclick=\"hideWindow('register');showWindow('login', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx');\" class=\"pn\"><span>发表回复</span></button>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<a href=\"###\" id=\"adlinkbtn\" style=\"display:none;\" onclick=\"window.open('" + quickbgad[0].ToString().Trim() + "','_blank');\">查看背景广告</a>\r\n				<a href=\"###\" id=\"closebtn\" style=\"display:none;\" onclick=\"$('quickpostmessage').style.background='';this.style.display='none';$('adlinkbtn').style.display='none';\">隐藏</a>\r\n			</div>\r\n			</div>\r\n			<script type=\"text/javascript\">\r\n			var isendpage = (");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("==");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append(");\r\n			var textobj = $('quickpostmessage');	\r\n			var smileyinsert = 1;\r\n			var showsmiliestitle = 0;\r\n			var smiliesIsCreate = 0;	\r\n			var smilies_HASH = {};\r\n			</");
	templateBuilder.Append("script>\r\n	");
	if (infloat!=1)
	{

	templateBuilder.Append("\r\n		</td>\r\n		</tr>\r\n	</tbody>\r\n	</table>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n<script type=\"text/javascript\"  reload=\"1\">\r\nString.prototype.trim = function()\r\n{\r\nreturn this.replace(/(^\\s*)|(\\s*$)/g, \"\");\r\n} \r\n");
	if (poster!="")
	{

	templateBuilder.Append("\r\n$(\"toreplay_user\").value=trim(html2bbcode1($(\"toreplay_div\").innerHTML)).trim();\r\n");
	}	//end if

	templateBuilder.Append("\r\n</");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_quickreply.js\"></");
	templateBuilder.Append("script>\r\n</form>\r\n\r\n");


	}	//end if


	}	//end if


	if (userid<0||canposttopic)
	{

	templateBuilder.Append("\r\n	<ul id=\"newspecial_menu\" class=\"popupmenu_popup newspecialmenu\" style=\"display: none\">\r\n	 ");
	if (forum.Allowspecialonly<=0)
	{

	templateBuilder.Append("\r\n		<li><a href=\"posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\">发新主题</a></li>\r\n		");
	}	//end if

	int specialpost = forum.Allowpostspecial&1;
	

	if (specialpost==1 && usergroupinfo.Allowpostpoll==1)
	{

	templateBuilder.Append("\r\n		<li class=\"poll\"><a href=\"posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&type=poll\">发布投票</a></li>\r\n		");
	}	//end if

	 specialpost = forum.Allowpostspecial&4;
	

	if (specialpost==4 && usergroupinfo.Allowbonus==1)
	{

	templateBuilder.Append("\r\n		<li class=\"reward\"><a href=\"posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&type=bonus\">发布悬赏</a></li>\r\n		");
	}	//end if

	 specialpost = forum.Allowpostspecial&16;
	

	if (specialpost==16 && usergroupinfo.Allowdebate==1)
	{

	templateBuilder.Append("\r\n		<li class=\"debate\"><a href=\"posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("&type=debate\">发起辩论</a></li>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n	</ul>\r\n	<ul class=\"popupmenu_popup newspecialmenu\" id=\"newspecial2_menu\" style=\"display: none\">\r\n	</ul>\r\n    <ul class=\"popupmenu_popup newspecialmenu\" id=\"seditor_newspecial_menu\" style=\"display: none\">\r\n	</ul>\r\n	<script type=\"text/javascript\">\r\n	    $('newspecial2_menu').innerHTML = $('newspecial_menu').innerHTML;\r\n	    $('seditor_newspecial_menu').innerHTML = $('newspecial_menu').innerHTML;\r\n	</");
	templateBuilder.Append("script>\r\n    <div id=\"imgcache\" style=\"display:none;\">\r\n    </div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n<script type=\"text/javascript\">\r\n    var topictitle = '");
	templateBuilder.Append(GetJsFormat(topic.Title).ToString().Trim());
	templateBuilder.Append("';\r\n    var maxpage = parseInt('");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("');\r\n    var pageid = parseInt('");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("');\r\n    if (maxpage > 1) {\r\n        document.onkeyup = function (e) {\r\n            e = e ? e : window.event;\r\n            var tagname = is_ie ? e.srcElement.tagName : e.target.tagName;\r\n            if (tagname == 'INPUT' || tagname == 'TEXTAREA') return;\r\n            actualCode = e.keyCode ? e.keyCode : e.charCode;\r\n            if (pageid < maxpage && actualCode == 39) {\r\n                window.location = '");
	templateBuilder.Append(Urls.ShowTopicAspxRewrite(topicid,pageid+1).ToString().Trim());
	templateBuilder.Append("';\r\n            }\r\n            if (pageid > 1 && actualCode == 37) {\r\n                window.location = '");
	templateBuilder.Append(Urls.ShowTopicAspxRewrite(topicid,pageid-1).ToString().Trim());
	templateBuilder.Append("';\r\n            }\r\n        }\r\n    }\r\n</");
	templateBuilder.Append("script>\r\n");	string topicurl = Utils.GetRootUrl(forumpath)+Urls.ShowTopicAspxRewrite(topicid,pageid);
	
	templateBuilder.Append("\r\n<script type=\"text/javascript\">\r\n    function copytitle() {\r\n        var text = '");
	templateBuilder.Append(GetJsFormat(topic.Title).ToString().Trim());
	templateBuilder.Append("\\r\\n");
	templateBuilder.Append(topicurl.ToString());
	templateBuilder.Append("';\r\n        setcopy(text, '帖子地址已经复制到剪贴板');\r\n    }\r\n    function ShowDownloadTip(attachmentownerid) {\r\n        if(attachmentownerid==");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("||");
	templateBuilder.Append(ismoder.ToString());
	templateBuilder.Append("==1)\r\n            return true;\r\n            \r\n        ");
	if (Scoresets.IsSetDownLoadAttachScore())
	{

	templateBuilder.Append("\r\n            return confirm('下载附件需要:");
	templateBuilder.Append(downloadattachmenttip.ToString());
	templateBuilder.Append(".确定下载?');\r\n        ");
	}
	else
	{

	templateBuilder.Append("\r\n            return true;\r\n        ");
	}	//end if

	templateBuilder.Append("\r\n    }\r\n</");
	templateBuilder.Append("script>\r\n");
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

	templateBuilder.Append("\r\n		<div class=\"wrap cl pageinfo\">\r\n	<div id=\"nav\"><a id=\"forumlist\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a>  &raquo; <strong>错误提示</strong></div>\r\n		</div>\r\n		");
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

	templateBuilder.Append("\r\n");
	templateBuilder.Append(inpostad.ToString());
	templateBuilder.Append("\r\n");
	if (config.Forumjump==1)
	{

	templateBuilder.Append("\r\n	");
	templateBuilder.Append(navhomemenu.ToString());
	templateBuilder.Append("\r\n");
	}	//end if


	if (showvisitedforumsmenu)
	{

	templateBuilder.Append("\r\n<div class=\"p_pop\" id=\"visitedforums_menu\" style=\"display: none\">\r\n	<h3 class=\"xi1\">浏览过的版块</h3>\r\n	<ul>\r\n	");
	int simpforuminfo__loop__id=0;
	foreach(SimpleForumInfo simpforuminfo in visitedforums)
	{
		simpforuminfo__loop__id++;


	if (simpforuminfo.Fid!=forumid)
	{

	templateBuilder.Append("\r\n		<li><a href=\"");
	templateBuilder.Append(simpforuminfo.Url.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(simpforuminfo.Name.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n	</ul>\r\n</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<script type=\"text/javascript\">\r\n    getuserips();\r\n");
	if (ForumUtils.GetCookie("clearUserdata")=="forum")
	{

	templateBuilder.Append("\r\n    saveUserdata('forum', '');\r\n");
	}	//end if

	templateBuilder.Append("\r\n</");
	templateBuilder.Append("script>\r\n");
	}
	else
	{


	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/bbcode.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\"></");
	templateBuilder.Append("script>\r\n");	string seditorid = "";
	
	string poster = DNTRequest.GetString("poster");
	
	int postlayer = DNTRequest.GetInt("postlayer",0);
	
	string urlreferrer = DNTRequest.GetUrlReferrer();
	

	if (infloat!=1)
	{

	 seditorid = "quickpost";
	

	}	//end if


	if (infloat==1)
	{

	templateBuilder.Append("\r\n<form method=\"post\" name=\"postform\" id=\"form\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&poster=");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("&postlayer=");
	templateBuilder.Append(postlayer.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" >\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n<form method=\"post\" name=\"postform\" id=\"quickpostform\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&poster=");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("&postlayer=");
	templateBuilder.Append(postlayer.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" >\r\n");
	}	//end if

	templateBuilder.Append("\r\n<div id=\"quickpost\" class=\"");
	if (infloat!=1)
	{

	templateBuilder.Append("main");
	}	//end if

	templateBuilder.Append(" quickpost\">\r\n	");
	if (infloat==1)
	{

	templateBuilder.Append("\r\n	<h3 class=\"flb\">\r\n		<span class=\"y\">\r\n			<a title=\"关闭\" onclick=\"hideWindow('reply')\" class=\"flbc\" href=\"javascript:;\">关闭</a>\r\n		</span><em>无刷新回复主题</em>\r\n	</h3>\r\n	");
	}	//end if


	if (infloat!=1)
	{

	templateBuilder.Append("\r\n	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"无刷新快速回复\" >\r\n	<tbody>\r\n		<tr>\r\n		<td class=\"postauthor\">\r\n			<div class=\"avatar\">	\r\n			");
	if (canreply||userid>0)
	{

	string avatarurl = Avatars.GetAvatarUrl(userid);
	
	templateBuilder.Append("	\r\n			<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_medium.gif';\" alt=\"回复者\"/>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n		</td>\r\n		<td class=\"postcontent\">\r\n	");
	}	//end if

	templateBuilder.Append("\r\n			<div class=\"post_inner c cl\">\r\n			");
	if (quickeditorad!="")
	{

	templateBuilder.Append("\r\n			<div class=\"leaderboard\">");
	templateBuilder.Append(quickeditorad.ToString());
	templateBuilder.Append("</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<em id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("validatemessage\">");
	if (infloat==1)
	{

	templateBuilder.Append("参与/回复主题");
	}	//end if

	templateBuilder.Append("</em>\r\n			<div class=\"pbt\">\r\n				<input type=\"text\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title\" name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" size=\"84\" tabindex=\"1\" value=\"\" style=\"display:none;\" />\r\n				<input type=\"hidden\" id=\"postlayer\" name=\"postlayer\" value=\"-1\" />\r\n				<input type=\"hidden\" id=\"postid\" name=\"postid\" value=\"");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("\" />\r\n				<span style=\"display:none\">\r\n				<input type=\"checkbox\" value=\"1\" name=\"htmlon\" id=\"htmlon\" ");
	if (usergroupinfo.Allowhtml!=1)
	{

	templateBuilder.Append(" disabled");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"parseurloff\" id=\"parseurloff\" value=\"1\" ");
	if (parseurloff==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"smileyoff\" id=\"smileyoff\" value=\"1\" ");
	if (smileyoff==1)
	{

	templateBuilder.Append(" checked disabled ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"bbcodeoff\" id=\"bbcodeoff\" value=\"1\" ");
	if (bbcodeoff==1)
	{

	templateBuilder.Append(" checked disabled ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"usesig\" id=\"usesig\" value=\"1\" ");
	if (usesig==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" ");
	if (replyemailstatus==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				<input type=\"checkbox\" name=\"postreplynotice\" id=\"postreplynotice\" ");
	if (replynotificationstatus==1)
	{

	templateBuilder.Append(" checked ");
	}	//end if

	templateBuilder.Append("/>\r\n				</span>\r\n				<script type=\"text/javascript\">\r\n					var bbinsert = parseInt('1');\r\n					var smiliesCount = 24;\r\n					var colCount = 8;\r\n				</");
	templateBuilder.Append("script>\r\n				");	char comma = ',';
	

	if (infloat==1)
	{

	templateBuilder.Append("\r\n				<div class=\"pbt cl\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title_text\">RE:");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("<a href=\"javascript:void(0)\" onclick=\"modifytitle();\" class=\"xg2\" style=\"margin-left:10px;\">修改</a></div>\r\n				<script type=\"text/javascript\">\r\n				function modifytitle(){\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title_text').style.display = 'none';\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title').style.display = '';\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title').ClassName = 'txt postpx';\r\n				    $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title').value = 'RE:");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("';\r\n				}\r\n				</");
	templateBuilder.Append("script>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			");
	if (poster!="")
	{

	templateBuilder.Append("\r\n			<div class=\"pbt cl\" id=\"toreplay_div\">\r\n			<strong>回复 <a target=\"_blank\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("#");
	templateBuilder.Append(postid.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(postlayer.ToString());
	templateBuilder.Append("楼<font color=\"Olive\">");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("</font>的帖子</a></strong>\r\n			</div>\r\n			<input type=\"hidden\" name=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("toreplay_user\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("toreplay_user\"  value=\"\"/>	\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<div ");
	if (infloat!=1)
	{

	templateBuilder.Append("style=\"margin-right:170px;\" ");
	}
	else
	{

	templateBuilder.Append("style=\"width:600px;\"");
	}	//end if

	templateBuilder.Append(">\r\n			<div class=\"pbt\">\r\n				");
	templateBuilder.Append("<link href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/seditor.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<div class=\"editor_tb\">\r\n	<span class=\"y\">\r\n		");
	if (topicid>0)
	{

	string replyurl = rooturl+"postreply.aspx?topicid="+topicid+"&forumpage="+forumpageid;
	

	if (postid>0)
	{

	 replyurl = replyurl+"&postid="+postid+"&postlayer="+postlayer+"&poster="+Utils.UrlEncode(poster);
	

	}	//end if

	templateBuilder.Append("\r\n		    <a onclick=\"switchAdvanceMode(this.href, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');doane(event);\" href=\"");
	templateBuilder.Append(replyurl.ToString());
	templateBuilder.Append("\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/external2.png\" alt=\"高级编辑器\" class=\"vm\"/>高级编辑器</a>\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n		    <a onclick=\"switchAdvanceMode(this.href, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');doane(event);\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/external2.png\" alt=\"高级编辑器\" class=\"vm\"/>高级编辑器</a>\r\n		");
	}	//end if


	if (infloat!=1)
	{


	if (userid<0||canposttopic)
	{

	string newtopicurl = "";
	

	if (forum.Allowspecialonly<=0)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&forumpage=" + pageid;
	

	}
	else if (1==(forum.Allowpostspecial&1)&&usergroupinfo.Allowpostpoll==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=poll&forumpage=" + pageid;
	

	}
	else if (4==(forum.Allowpostspecial&4)&&usergroupinfo.Allowbonus==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=reward&forumpage=" + pageid;
	

	}
	else if (16==(forum.Allowpostspecial&16)&&usergroupinfo.Allowdebate==1)
	{

	 newtopicurl = forumpath + "posttopic.aspx?forumid=" + forum.Fid + "&type=debate&forumpage=" + pageid;
	

	}	//end if

	string newtopiconclick = "";
	

	if (forum.Allowspecialonly<=0&&canposttopic)
	{

	 newtopiconclick = "showWindow('newthread', '" + forumpath + "showforum.aspx?forumid=" + forum.Fid + "')";
	

	}	//end if


	if (userid<=0)
	{

	 newtopiconclick = "showWindow('login', '" + forumpath + "login.aspx');hideWindow('register');";
	

	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	</span>\r\n	<div>\r\n		<a href=\"javascript:;\" title=\"粗体\" class=\"tb_bold\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[b]', '[/b]')\">B</a>\r\n		<a href=\"javascript:;\" title=\"颜色\" class=\"tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor\" onclick=\"showMenu(this.id, true, 0, 2)\">Color</a>\r\n		");	string coloroptions = "Black,Sienna,DarkOliveGreen,DarkGreen,DarkSlateBlue,Navy,Indigo,DarkSlateGray,DarkRed,DarkOrange,Olive,Green,Teal,Blue,SlateGray,DimGray,Red,SandyBrown,YellowGreen,SeaGreen,MediumTurquoise,RoyalBlue,Purple,Gray,Magenta,Orange,Yellow,Lime,Cyan,DeepSkyBlue,DarkOrchid,Silver,Pink,Wheat,LemonChiffon,PaleGreen,PaleTurquoise,LightBlue,Plum,White";
	
	templateBuilder.Append("\r\n		<div class=\"popupmenu_popup tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor_menu\" style=\"display: none\">\r\n			");
	int colorname__loop__id=0;
	foreach(string colorname in coloroptions.Split(comma))
	{
		colorname__loop__id++;

	templateBuilder.Append("\r\n				<input type=\"button\" style=\"background-color: ");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[color=");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("]', '[/color]')\" />");
	if (colorname__loop__id%8==0)
	{

	templateBuilder.Append("<br />");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n		</div>\r\n		<a href=\"javascript:;\" title=\"图片\" class=\"tb_img\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("img\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'img')\">Image</a>\r\n		<a href=\"javascript:;\" title=\"链接\" class=\"tb_link\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("url\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'url')\">Link</a>\r\n		<a href=\"javascript:;\" title=\"引用\" class=\"tb_quote\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[quote]', '[/quote]')\">Quote</a>\r\n		<a href=\"javascript:;\" title=\"代码\" class=\"tb_code\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[code]', '[/code]')\">Code</a>\r\n	");
	if (config.Smileyinsert==1 && forum.Allowsmilies==1)
	{

	templateBuilder.Append("\r\n		<a href=\"javascript:;\" class=\"tb_smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies\" onclick=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback);showMenu({'ctrlid':this.id, 'evt':'click', 'layer':2})\">Smilies</a>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n</div>\r\n");
	if (config.Smileyinsert==1 && forum.Allowsmilies==1)
	{

	templateBuilder.Append("\r\n	<div class=\"smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies_menu\" style=\"display:none;width:315px;\">\r\n		<div class=\"smilieslist\">\r\n			");	string defaulttypname = string.Empty;
	
	templateBuilder.Append("\r\n			<div id=\"smiliesdiv\">\r\n				<div class=\"smiliesgroup\" style=\"margin-right: 0pt;\">\r\n					<ul>\r\n					");
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

	templateBuilder.Append("\r\n						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" class=\"current\">" + stype["code"].ToString().Trim() + "</a></li>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">" + stype["code"].ToString().Trim() + "</a></li>\r\n						");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n					</ul>\r\n				 </div>\r\n				 <div style=\"clear: both;\" class=\"float_typeid\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie\"></div>\r\n				 <table class=\"smilieslist_table\" id=\"s_preview_table\" style=\"display: none\"><tr><td class=\"smilieslist_preview\" id=\"s_preview\"></td></tr></table>\r\n				 <div id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie_pagenum\" class=\"smilieslist_page\">&nbsp;</div>\r\n			</div>\r\n		</div>\r\n		<script type=\"text/javascript\" reload=\"1\">\r\n			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(func){\r\n				if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML !='' && $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML != '正在加载表情...')\r\n					return;\r\n				var c = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=smilies\";\r\n				_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true);\r\n				setTimeout(\"if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML=='')$('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML = '正在加载表情...'\", 2000);\r\n			}\r\n			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback(obj) {\r\n				smilies_HASH = obj; \r\n				showsmiles(1, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\r\n			}\r\n		</");
	templateBuilder.Append("script>\r\n	</div>\r\n");
	}	//end if



	templateBuilder.Append("\r\n				<div class=\"postarea cl\">\r\n					<div class=\"postinner\">\r\n					");
	if (canreply)
	{


	if (infloat==1)
	{

	templateBuilder.Append("\r\n					  <textarea rows=\"7\" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"message\" tabindex=\"3\" style=\"background:url(" + quickbgad[1].ToString().Trim() + ") no-repeat 50% 50%;\" ");
	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"");
	}	//end if

	templateBuilder.Append(" onkeydown=\"ajaxctlent(event, this.form, ");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(", isendpage, '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("',");
	templateBuilder.Append(hide.ToString());
	templateBuilder.Append(");\"></textarea>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					  <textarea rows=\"5\" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"quickpostmessage\" tabindex=\"6\"  style=\"background-image:url(" + quickbgad[1].ToString().Trim() + ");background-repeat:no-repeat;background-position:50% 50%;\" ");
	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"");
	}	//end if

	templateBuilder.Append(" onkeydown=\"ajaxctlent(event, this.form, ");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(", isendpage, '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("',");
	templateBuilder.Append(hide.ToString());
	templateBuilder.Append(");\"></textarea>\r\n					");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n					<div class=\"hm p_login cl\">你需要登录后才可以发帖 <a class=\"xg2\" onclick=\"hideWindow('register');showWindow('login', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx\">登录</a> | <a class=\"xg2\" onclick=\"hideWindow('login');showWindow('register', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\">注册</a></div>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n			</div>\r\n			</div>\r\n			");
	if (isseccode)
	{

	templateBuilder.Append("\r\n			<div class=\"pbt\" style=\"position: relative;\">\r\n				");
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

	templateBuilder.Append("\r\n			</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<div class=\"pbt\">\r\n				");
	if (topic.Special==4 && isenddebate==false)
	{

	templateBuilder.Append("\r\n				<div class=\"ftid\">\r\n					<select name=\"debateopinion\" id=\"debateopinion\">\r\n						<option value=\"0\" selected>辩论观点</option>\r\n						<option value=\"1\">正方</option>\r\n						<option value=\"2\">反方</option>\r\n					</select>\r\n				</div>\r\n				<script type=\"text/javascript\">simulateSelect('debateopinion');</");
	templateBuilder.Append("script>\r\n				");
	}	//end if


	if (canreply)
	{

	templateBuilder.Append("\r\n					<button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"replysubmit\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"5\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"8\"");
	}	//end if

	templateBuilder.Append(" onclick=\"ajaxreply(this.form, ");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(", isendpage, '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("', false,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("',");
	templateBuilder.Append(hide.ToString());
	templateBuilder.Append(");\" class=\"pn\"><span>发表回复</span></button><span class=\"grayfont\">[Ctrl+Enter快速发布]</span>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				    <button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"replysubmit\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"5\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"8\"");
	}	//end if

	templateBuilder.Append(" onclick=\"hideWindow('register');showWindow('login', '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx');\" class=\"pn\"><span>发表回复</span></button>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<a href=\"###\" id=\"adlinkbtn\" style=\"display:none;\" onclick=\"window.open('" + quickbgad[0].ToString().Trim() + "','_blank');\">查看背景广告</a>\r\n				<a href=\"###\" id=\"closebtn\" style=\"display:none;\" onclick=\"$('quickpostmessage').style.background='';this.style.display='none';$('adlinkbtn').style.display='none';\">隐藏</a>\r\n			</div>\r\n			</div>\r\n			<script type=\"text/javascript\">\r\n			var isendpage = (");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("==");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append(");\r\n			var textobj = $('quickpostmessage');	\r\n			var smileyinsert = 1;\r\n			var showsmiliestitle = 0;\r\n			var smiliesIsCreate = 0;	\r\n			var smilies_HASH = {};\r\n			</");
	templateBuilder.Append("script>\r\n	");
	if (infloat!=1)
	{

	templateBuilder.Append("\r\n		</td>\r\n		</tr>\r\n	</tbody>\r\n	</table>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n<script type=\"text/javascript\"  reload=\"1\">\r\nString.prototype.trim = function()\r\n{\r\nreturn this.replace(/(^\\s*)|(\\s*$)/g, \"\");\r\n} \r\n");
	if (poster!="")
	{

	templateBuilder.Append("\r\n$(\"toreplay_user\").value=trim(html2bbcode1($(\"toreplay_div\").innerHTML)).trim();\r\n");
	}	//end if

	templateBuilder.Append("\r\n</");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_quickreply.js\"></");
	templateBuilder.Append("script>\r\n</form>\r\n\r\n");


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
