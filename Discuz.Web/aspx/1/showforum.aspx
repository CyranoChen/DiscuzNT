<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.showforum" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/6/2 16:12:32.
		本页面代码由Discuz!NT模板引擎生成于 2011/6/2 16:12:32. 
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
	templateBuilder.Append("</a> &raquo; ");
	templateBuilder.Append(forumnav.ToString());
	templateBuilder.Append("\r\n	</div>\r\n</div>\r\n<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n");
	if (page_err==0)
	{

	templateBuilder.Append("\r\n	<script type=\"text/javascript\">\r\n	var templatepath = \"");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("\";\r\n    var imagedir = \"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("\";\r\n	var fid = parseInt(");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append(");\r\n	var postminchars = parseInt(");
	templateBuilder.Append(config.Minpostsize.ToString().Trim());
	templateBuilder.Append(");\r\n	var postmaxchars = parseInt(");
	templateBuilder.Append(config.Maxpostsize.ToString().Trim());
	templateBuilder.Append(");\r\n	var disablepostctrl = parseInt(");
	templateBuilder.Append(disablepostctrl.ToString());
	templateBuilder.Append(");\r\n	var forumurl = forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\n	</");
	templateBuilder.Append("script>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_showforum.js\"></");
	templateBuilder.Append("script>\r\n");
	if (page_err==0)
	{

	templateBuilder.Append("\r\n<div class=\"wrap cl\">\r\n");

	if (pagewordad.Length>0)
	{

	templateBuilder.Append("\r\n<div id=\"ad_text\" class=\"ad_text sclear\">\r\n	<table cellspacing=\"1\" cellpadding=\"0\" width=\"100%\" summary=\"text ad\">\r\n	<tbody>\r\n		<tr>\r\n		");	int adindex = 0;
	

	int pageword__loop__id=0;
	foreach(string pageword in pagewordad)
	{
		pageword__loop__id++;


	if (adindex<4)
	{

	templateBuilder.Append("\r\n			<td>");
	templateBuilder.Append(pageword.ToString());
	templateBuilder.Append("</td>\r\n				");	 adindex = adindex+1;
	

	}
	else
	{

	templateBuilder.Append("\r\n		</tr><tr>\r\n			<td>");
	templateBuilder.Append(pageword.ToString());
	templateBuilder.Append("</td>\r\n				");	 adindex = 1;
	

	}	//end if


	}	//end loop


	if (pagewordad.Length%4>0)
	{


					for (int j = 0; j < (4 - pagewordad.Length % 4); j++)
					{
				
	templateBuilder.Append("\r\n			<td>&nbsp;</td>\r\n			");
					}
				

	}	//end if

	templateBuilder.Append("\r\n		</tr>\r\n	</tbody>\r\n	</table>\r\n</div>\r\n");
	}	//end if


	if (pagead.Count>0)
	{


	int pageadtext__loop__id=0;
	foreach(string pageadtext in pagead)
	{
		pageadtext__loop__id++;

	templateBuilder.Append("\r\n        <div class=\"ad_text sclear\">\r\n            ");
	templateBuilder.Append(pageadtext.ToString());
	templateBuilder.Append("\r\n        </div>\r\n    ");
	}	//end loop


	}	//end if




	if (showforumlogin==1)
	{

	templateBuilder.Append("\r\n	<div class=\"main\">\r\n		<h3>本版块已经被管理员设置了密码</h3>\r\n		<form id=\"forumlogin\" name=\"forumlogin\" method=\"post\" action=\"\">\r\n		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"formtable\">\r\n		<tbody>\r\n		<tr>\r\n			<th><label for=\"forumpassword\">请输入密码</label></th>\r\n			<td><input name=\"forumpassword\" type=\"password\" id=\"forumpassword\" size=\"20\" class=\"txt\"/></td>\r\n		</tr>\r\n		</tbody>\r\n		");
	if (isseccode)
	{

	templateBuilder.Append("	\r\n		<tbody>\r\n		<tr>\r\n			<th><label for=\"vcode\">输入验证码</label></th>\r\n			<td>\r\n				<div style=\"position: relative;\">\r\n				");
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

	templateBuilder.Append("\r\n				</div>\r\n		    </td>\r\n		</tr>\r\n		</tbody>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<tbody>\r\n		<tr>\r\n			<th></th>\r\n			<td><input type=\"submit\"  value=\"确定\"/></td>\r\n		</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n	</div>\r\n</div>\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n<div id=\"forumheader\" class=\"main cl\">\r\n	<span class=\"y o\">\r\n        ");
	if (forum.Rules!="")
	{

	templateBuilder.Append("\r\n        <img id=\"rules_img\"  src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_no.gif\" alt=\"展开/收起\" onclick=\"toggle_collapse('rules');\"/>\r\n        ");
	}	//end if

	templateBuilder.Append("\r\n    </span>\r\n	");
	if (page_err==0)
	{

	templateBuilder.Append("\r\n	<span class=\"y\">\r\n	");
	if (ismoder)
	{

	templateBuilder.Append("<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("modcp.aspx?operation=attention&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"f_bold\">管理面板</a>");
	}	//end if

	templateBuilder.Append("\r\n	</span>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<div class=\"forumaction y\">\r\n		<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=digest\" class=\"digest\">精华</a>\r\n		");
	if (config.Rssstatus!=0&&forum.Allowrss!=0)
	{

	 aspxrewriteurl = this.RssAspxRewrite(forum.Fid);
	
	templateBuilder.Append("	\r\n		<a class=\"feed\" target=\"_blank\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">RSS</a>	\r\n		");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n	<h1>");	 aspxrewriteurl = this.ShowForumAspxRewrite(forum.Fid,0);
	templateBuilder.Append(forum.Name.ToString().Trim());
	templateBuilder.Append("</h1>\r\n	<span class=\"forumstats\">今日: <strong class=\"xi1\">");
	templateBuilder.Append(forum.Todayposts.ToString().Trim());
	templateBuilder.Append("</strong><span class=\"pipe\">|</span>主题: <strong class=\"xi1\">");
	templateBuilder.Append(topiccount.ToString());
	templateBuilder.Append("</strong><span class=\"pipe\">|</span>帖子: <strong class=\"xi1\">");
	templateBuilder.Append(forum.Posts.ToString().Trim());
	templateBuilder.Append("</strong></span>\r\n");
	if (forum.Description!="")
	{

	templateBuilder.Append("\r\n	<p>");
	templateBuilder.Append(forum.Description.ToString().Trim());
	templateBuilder.Append("</p>\r\n");
	}	//end if

	templateBuilder.Append("\r\n	<p id=\"modedby\">\r\n");
	if (page_err==0)
	{

	templateBuilder.Append("版主: <span class=\"f_c\">\r\n	");
	if (forum.Moderators!="")
	{

	templateBuilder.Append("\r\n		");
	templateBuilder.Append(forum.Moderators.ToString().Trim());
	templateBuilder.Append("\r\n	");
	}
	else
	{

	templateBuilder.Append("\r\n		*空缺中*\r\n	");
	}	//end if

	templateBuilder.Append("</span>\r\n");
	}	//end if

	templateBuilder.Append("\r\n	</p>\r\n");
	if (forum.Rules!="")
	{

	templateBuilder.Append("\r\n	<div id=\"rules\">");
	templateBuilder.Append(forum.Rules.ToString().Trim());
	templateBuilder.Append("</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n");
	if (forum.Subforumcount>0&&subforumlist.Count>0)
	{


	templateBuilder.Append("<div id=\"subforum\" class=\"main cl list\">\r\n	<div class=\"titlebar xg2\">\r\n		<span class=\"y\">\r\n		");
	if (forum.Moderators!="")
	{

	templateBuilder.Append("\r\n			分类版主: ");
	templateBuilder.Append(forum.Moderators.ToString().Trim());
	templateBuilder.Append("\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<img id=\"category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("_img\"  src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_no.gif\" alt=\"展开/收起\" onclick=\"toggle_collapse('category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("');\" class=\"cursor\"/>\r\n		</span>\r\n		<h2>子版块</h2>\r\n	</div>\r\n	<div id=\"category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\"  class=\"fi\" summary=\"category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\">\r\n	<table cellspacing=\"0\" cellpadding=\"0\">\r\n	<tbody>	\r\n	");
	if (forum.Colcount==1)
	{


	int subforum__loop__id=0;
	foreach(IndexPageForumInfo subforum in subforumlist)
	{
		subforum__loop__id++;

	templateBuilder.Append("\r\n			<tr>\r\n				");	 aspxrewriteurl = this.ShowForumAspxRewrite(subforum.Fid,0);
	
	templateBuilder.Append("\r\n				<th ");
	if (config.Shownewposticon==1)
	{

	templateBuilder.Append("class=\"notopic ");
	if (subforum.Havenew=="new")
	{

	templateBuilder.Append("new");
	}	//end if

	templateBuilder.Append("\"");
	}	//end if

	templateBuilder.Append(">\r\n					<h2>						\r\n						");
	if (subforum.Redirect=="")
	{

	 aspxrewriteurl = this.ShowForumAspxRewrite(subforum.Fid,0,subforum.Rewritename);
	
	templateBuilder.Append("\r\n							<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n							<a href=\"");
	templateBuilder.Append(subforum.Redirect.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">\r\n						");
	}	//end if


	if (subforum.Icon!="")
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(subforum.Icon.ToString().Trim());
	templateBuilder.Append("\" border=\"0\" align=\"left\" hspace=\"5\" alt=\"");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("\"/>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n						");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("</a>");
	if (subforum.Todayposts>0)
	{

	templateBuilder.Append("<span class=\"today\">(今日:<strong>");
	templateBuilder.Append(subforum.Todayposts.ToString().Trim());
	templateBuilder.Append("</strong>)</span>");
	}	//end if

	templateBuilder.Append("\r\n					</h2>\r\n					");
	if (subforum.Description!="")
	{

	templateBuilder.Append("<p>");
	templateBuilder.Append(subforum.Description.ToString().Trim());
	templateBuilder.Append("</p>");
	}	//end if


	if (subforum.Moderators!="")
	{

	templateBuilder.Append("<p class=\"moderators\">版主:");
	templateBuilder.Append(subforum.Moderators.ToString().Trim());
	templateBuilder.Append("</p>");
	}	//end if

	templateBuilder.Append("\r\n				</th>\r\n				<td class=\"nums\"><em>");
	templateBuilder.Append(subforum.Topics.ToString().Trim());
	templateBuilder.Append("</em> / ");
	templateBuilder.Append(subforum.Posts.ToString().Trim());
	templateBuilder.Append("</td>\r\n				<td class=\"lastpost\">\r\n					");
	if (subforum.Status==-1)
	{

	templateBuilder.Append("\r\n						<p>私密论坛</p>\r\n					");
	}
	else
	{


	if (subforum.Lasttid!=0)
	{

	templateBuilder.Append("\r\n						<p>\r\n							");	 aspxrewriteurl = this.ShowTopicAspxRewrite(subforum.Lasttid,0);
	
	templateBuilder.Append("\r\n							<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");	templateBuilder.Append(Utils.GetUnicodeSubString(subforum.Lasttitle,35,"..."));
	templateBuilder.Append("</a>\r\n						</p>\r\n						<div class=\"topicbackwriter\">by\r\n							");
	if (subforum.Lastposter!="")
	{


	if (subforum.Lastposterid==-1)
	{

	templateBuilder.Append("\r\n									游客\r\n								");
	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(subforum.Lastposterid);
	
	templateBuilder.Append("\r\n									<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(subforum.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n								");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n								匿名\r\n							");
	}	//end if

	 aspxrewriteurl = this.ShowTopicAspxRewrite(subforum.Lasttid,0);
	
	string sublastdatetime = ForumUtils.ConvertDateTime(subforum.Lastpost);
	
	templateBuilder.Append("\r\n						- 	<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(subforum.Lasttid.ToString().Trim());
	templateBuilder.Append("&page=end#lastpost\" title=\"");
	templateBuilder.Append(subforum.Lasttitle.ToString().Trim());
	templateBuilder.Append("\"><span>");
	templateBuilder.Append(sublastdatetime.ToString());
	templateBuilder.Append("</span></a>\r\n						</div>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n							从未\r\n						");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				</td>\r\n			  </tr>\r\n		");
	}	//end loop


	}
	else
	{

	int subforumindex = 0;
	
	double colwidth = 99.6 / forum.Colcount;
	

	int subforum__loop__id=0;
	foreach(IndexPageForumInfo subforum in subforumlist)
	{
		subforum__loop__id++;

	 subforumindex = subforumindex+1;
	

	if (subforumindex==1)
	{

	templateBuilder.Append("\r\n			<tr>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n				<th style=\"width:");
	templateBuilder.Append(colwidth.ToString());
	templateBuilder.Append("%;\" ");
	if (config.Shownewposticon==1)
	{

	templateBuilder.Append("class=\"notopic ");
	if (subforum.Havenew=="new")
	{

	templateBuilder.Append("new");
	}	//end if

	templateBuilder.Append("\"");
	}	//end if

	templateBuilder.Append(">\r\n				<h2>				\r\n				");
	if (forum.Redirect=="")
	{

	 aspxrewriteurl = this.ShowForumAspxRewrite(subforum.Fid,0,subforum.Rewritename);
	
	templateBuilder.Append("\r\n					<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n					<a href=\"");
	templateBuilder.Append(subforum.Redirect.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">\r\n				");
	}	//end if


	if (subforum.Icon!="")
	{

	templateBuilder.Append("\r\n					<img src=\"");
	templateBuilder.Append(subforum.Icon.ToString().Trim());
	templateBuilder.Append("\" alt=\"");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("\" hspace=\"5\" />\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("</a>\r\n				");
	if (subforum.Todayposts>0)
	{

	templateBuilder.Append("\r\n				<span class=\"time\">(今日:<strong>");
	templateBuilder.Append(subforum.Todayposts.ToString().Trim());
	templateBuilder.Append("</strong>)</span>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</h2>\r\n				<p>主题:");
	templateBuilder.Append(subforum.Topics.ToString().Trim());
	templateBuilder.Append(", 帖数:");
	templateBuilder.Append(subforum.Posts.ToString().Trim());
	templateBuilder.Append("</p>\r\n				");
	if (subforum.Status==-1)
	{

	templateBuilder.Append("\r\n				<p>私密版块</p>\r\n				");
	}
	else
	{


	if (subforum.Lasttid!=0)
	{

	string sublastdatetime = ForumUtils.ConvertDateTime(subforum.Lastpost);
	
	templateBuilder.Append("\r\n						<p>最后: <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(subforum.Lasttid.ToString().Trim());
	templateBuilder.Append("&page=end#lastpost\" title=\"");
	templateBuilder.Append(subforum.Lasttitle.ToString().Trim());
	templateBuilder.Append("\"><span>");
	templateBuilder.Append(sublastdatetime.ToString());
	templateBuilder.Append("</span></a> by \r\n							");
	if (subforum.Lastposter!="")
	{


	if (subforum.Lastposterid==-1)
	{

	templateBuilder.Append("\r\n									游客\r\n								");
	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(subforum.Lastposterid);
	
	templateBuilder.Append("\r\n									<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(subforum.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n								");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n								匿名\r\n							");
	}	//end if

	templateBuilder.Append("\r\n						</p>\r\n					");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				 </th>\r\n		");
	if (subforumindex==forum.Colcount)
	{

	templateBuilder.Append("\r\n			</tr>\r\n			");	 subforumindex = 0;
	

	}	//end if


	}	//end loop


	if (subforumindex!=0)
	{

	for (int i = 0; i < forum.Colcount-subforumindex; i++)
	{
		templateBuilder.Append("<td>&nbsp;</td>");
	}

	templateBuilder.Append("\r\n			</tr>\r\n		");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	</tbody>\r\n	</table>\r\n	</div>\r\n</div>");


	}	//end if


	if (forum.Layer!=0)
	{

	templateBuilder.Append("\r\n<div class=\"pages_btns cl\">\r\n	<div class=\"pages\">\r\n		<cite class=\"pageback z\" id=\"visitedforums\"");
	if (showvisitedforumsmenu)
	{

	templateBuilder.Append(" onmouseover=\"$('visitedforums').id = 'visitedforumstmp';this.id = 'visitedforums';showMenu({'ctrlid':this.id, 'pos':'34'});\"");
	}	//end if

	templateBuilder.Append("><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\">返回</a></cite>\r\n		");
	if (pagecount!=1)
	{

	templateBuilder.Append("\r\n			");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n			");
	if (pagecount>8)
	{

	templateBuilder.Append("\r\n			<kbd>\r\n			<input name=\"gopage\" type=\"text\" class=\"txt\" id=\"pageidinput1\" title=\"可以输入页码按回车键自动跳转\" value=\"");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" style=\"text-align:center;\" onfocus=\"this.value=this.defaultValue;this.select();\" onKeyDown=\"pageinputOnKeyDown(this,event);\" size=\"2\" maxlength=\"9\" />/ ");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("</kbd>\r\n            <script type=\"text/javascript\">\r\n                function pageinputOnKeyDown(obj, event) {\r\n                    if (event.keyCode == 13) {\r\n                        var typeid = getQueryString(\"typeid\");\r\n                        typeid = typeid == \"\" ? -1 : parseInt(typeid);\r\n                        if (parseInt('");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append("') == 1 && typeid <0) {\r\n                            window.location = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum-");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("-' + (parseInt(obj.value) > 0 ? parseInt(obj.value) : 1) + '");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("';\r\n                        }\r\n                        else {\r\n                            (typeid>0) ? window.location = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&page=' + (parseInt(obj.value) > 0 ? parseInt(obj.value) : 1) + '&typeid=' + typeid : window.location = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&page=' + (parseInt(obj.value) > 0 ? parseInt(obj.value) : 1);\r\n                        }\r\n                    }\r\n                    return (event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 97 && event.keyCode <= 105) || event.keyCode == 8;\r\n                }\r\n            </");
	templateBuilder.Append("script>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			");
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

	templateBuilder.Append("\r\n	<span ");
	if (userid>0)
	{

	templateBuilder.Append(" onmouseover=\"if($('newspecial_menu')!=null&&$('newspecial_menu').childNodes.length>0) showMenu(this.id);\"");
	}	//end if

	templateBuilder.Append(" id=\"newspecial\">\r\n        <a title=\"发新话题\" id=\"newtopic\" href=\"");
	templateBuilder.Append(newtopicurl.ToString());
	templateBuilder.Append("\" onclick=\"");
	templateBuilder.Append(newtopiconclick.ToString());
	templateBuilder.Append("\">\r\n        <img alt=\"发新话题\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/newtopic.png\" style=\"display:inline\"/></a>\r\n    </span>\r\n");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n");
	if (forum.Allowpostspecial!=0||forum.Applytopictype==1||forum.Viewbytopictype==1)
	{

	templateBuilder.Append("\r\n    <div id=\"headfilter\" class=\"cl\">\r\n        <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(ShowForumAspxRewrite(forumid,0).ToString());
	templateBuilder.Append("\">全部</a>\r\n	    ");
	if (forum.Applytopictype==1 && forum.Viewbytopictype==1)
	{

	templateBuilder.Append("\r\n		    ");
	templateBuilder.Append(topictypeselectlink.ToString());
	templateBuilder.Append("\r\n	    ");
	}	//end if

	templateBuilder.Append("\r\n    </div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<ul id=\"rewardmenu_menu\" class=\"p_pop\"  style=\"display: none\">\r\n	<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=rewarding\">进行中的悬赏</a></li>\r\n	<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=rewarded\">已结束的悬赏</a></li>\r\n</ul>\r\n<div class=\"main thread\">\r\n	<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("topicadmin.aspx?action=moderate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&infloat=1\">\r\n		<div class=\"category\">\r\n		<table summary=\"");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\" cellspacing=\"0\" cellpadding=\"0\">\r\n			<tr>\r\n			<th><span title=\"在新窗口中打开帖子\" id=\"atarget\" style=\"float:right;\">新窗</span>筛选:\r\n			    ");
	if (forum.Allowpostspecial!=0||forum.Applytopictype==1||forum.Viewbytopictype==1)
	{

	templateBuilder.Append("\r\n			    <a id=\"specialmenu\" onclick=\"showMenu(this.id);\" href=\"javascript:;\" class=\"drop xg2\">全部主题</a>\r\n			    <ul id=\"specialmenu_menu\" class=\"p_pop\" style=\"display: none\">\r\n			        <li><a href=\"javascript:selectspecial('all');\">全部主题</a></li>\r\n			        ");
	if ((forum.Allowpostspecial&1)==1)
	{

	templateBuilder.Append("\r\n	                <li><a href=\"javascript:selectspecial('poll');\">投票</a></li>\r\n                    ");
	}	//end if


	if ((forum.Allowpostspecial&4)==4)
	{

	templateBuilder.Append("\r\n	                <li><a href=\"javascript:selectspecial('reward');\">悬赏</a></li>\r\n                    <li><a href=\"javascript:selectspecial('rewarding');\">悬赏(进行中)</a></li>\r\n                    <li><a href=\"javascript:selectspecial('rewarded');\">悬赏(已结束)</a></li>\r\n	                ");
	}	//end if


	if ((forum.Allowpostspecial&16)==16)
	{

	templateBuilder.Append("\r\n                    <li><a href=\"javascript:selectspecial('debate');\">辩论</a></li>\r\n                    ");
	}	//end if

	templateBuilder.Append("\r\n			    </ul>\r\n			    ");
	}	//end if


	if (topictypeid==-1)
	{

	templateBuilder.Append("\r\n					<a id=\"intervalmenu\" onclick=\"showMenu(this.id);\" href=\"javascript:;\" class=\"drop xg2\">全部时间</a>\r\n				    <ul id=\"intervalmenu_menu\" class=\"p_pop\" style=\"display: none\">\r\n				        <li><a href=\"javascript:selectinterval(0);\">全部时间</a></li>\r\n				        <li><a href=\"javascript:selectinterval(1);\">一天</a></li>\r\n				        <li><a href=\"javascript:selectinterval(2);\">二天</a></li>\r\n				        <li><a href=\"javascript:selectinterval(7);\">一周</a></li>\r\n				        <li><a href=\"javascript:selectinterval(30);\">一个月</a></li>\r\n				        <li><a href=\"javascript:selectinterval(90);\">三个月</a></li>\r\n				        <li><a href=\"javascript:selectinterval(180);\">半年</a></li>\r\n				        <li><a href=\"javascript:selectinterval(365);\">一年</a></li>\r\n				    </ul>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<a id=\"ordermenu\" onclick=\"showMenu(this.id);\" href=\"javascript:;\"  class=\"drop xg2\">最后回复时间</a>\r\n				<ul id=\"ordermenu_menu\" class=\"p_pop\" style=\"display: none\">\r\n				    <li><a href=\"javascript:selectorder(1);\">最后回复时间</a></li>\r\n				    <li><a href=\"javascript:selectorder(2);\">发布时间</a></li>\r\n                    <li><a href=\"javascript:selectorder(3);\">查看次数</a></li>\r\n                    <li><a href=\"javascript:selectorder(4);\">回复次数</a></li>\r\n				</ul>\r\n				<span class=\"pipe\">|</span>排序:\r\n				<a id=\"directmenu\" onclick=\"showMenu(this.id);\" href=\"javascript:;\" class=\"drop xg2\">按降序排列</a>\r\n				<ul id=\"directmenu_menu\" class=\"p_pop\" style=\"display: none\">\r\n				    <li><a href=\"javascript:selectdirect(1);\">按降序排列</a></li>\r\n				    <li><a href=\"javascript:selectdirect(0);\">按升序排列</a></li>\r\n				</ul>\r\n                <span class=\"pipe\">|</span>\r\n                <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=digest\" ");
	if (filter!="digest")
	{

	templateBuilder.Append("class=\"xg2\"");
	}	//end if

	templateBuilder.Append(">精华</a>\r\n			    <script type=\"text/javascript\" reload=1>\r\n				    ");
					    string url = forumpath + "showforum.aspx?search=1&forumid=" + forumid + "&typeid=" + topictypeid;
	                    string showforumurl = url;
					    url += filter == "" ? "" : "&filter=" + filter;
					    filter = filter == "" ? "all" : filter;
					    
	templateBuilder.Append("\r\n			        var prefix = '");
	templateBuilder.Append(url.ToString());
	templateBuilder.Append("';\r\n			        function loadsearchconditionlink() {\r\n			            var specialarray = {'all':'全部主题','poll':'投票','reward':'悬赏','debate':'辩论','rewarding':'悬赏(进行中)','rewarded':'悬赏(已结束)'};\r\n                        var intervalarray = {'d1':'一天','d2':'二天','d7':'一周','d30':'一个月','d90':'三个月','d180':'半年','d365':'一年'};\r\n                        if($('specialmenu'))\r\n                            $('specialmenu').innerHTML = specialarray.");
	templateBuilder.Append(filter.ToString());
	templateBuilder.Append(";\r\n                        if(");
	templateBuilder.Append(interval.ToString());
	templateBuilder.Append("!=0)\r\n                            $('intervalmenu').innerHTML = intervalarray.d");
	templateBuilder.Append(interval.ToString());
	templateBuilder.Append(";\r\n                        var ordermenuarray = ['最后回复时间','发布时间','查看次数','回复次数'];\r\n                        $('ordermenu').innerHTML = ordermenuarray[");
	templateBuilder.Append(order.ToString());
	templateBuilder.Append(" - 1];\r\n			            if(");
	templateBuilder.Append(direct.ToString());
	templateBuilder.Append(" == 0)\r\n			                $('directmenu').innerHTML = '按升序排列';\r\n			        }\r\n			        loadsearchconditionlink();\r\n			        function selectspecial(selectvalue){\r\n			            window.location.href = '");
	templateBuilder.Append(showforumurl.ToString());
	templateBuilder.Append("&filter=' + selectvalue;\r\n			        }\r\n			        function selectinterval(selectvalue) {\r\n			            window.location.href = '");
	templateBuilder.Append(showforumurl.ToString());
	templateBuilder.Append("&order=");
	templateBuilder.Append(order.ToString());
	templateBuilder.Append("&direct=");
	templateBuilder.Append(direct.ToString());
	templateBuilder.Append("&interval=' + selectvalue;\r\n			        }\r\n			        function selectorder(selectvalue){\r\n			            window.location.href = '");
	templateBuilder.Append(showforumurl.ToString());
	templateBuilder.Append("&order=' + selectvalue + '&direct=");
	templateBuilder.Append(direct.ToString());
	templateBuilder.Append("&interval=");
	templateBuilder.Append(interval.ToString());
	templateBuilder.Append("';\r\n			        }\r\n			        function selectdirect(selectvalue){\r\n			            window.location.href = '");
	templateBuilder.Append(showforumurl.ToString());
	templateBuilder.Append("&order=");
	templateBuilder.Append(order.ToString());
	templateBuilder.Append("&direct=' + selectvalue + '&interval=");
	templateBuilder.Append(interval.ToString());
	templateBuilder.Append("';\r\n			        }\r\n                </");
	templateBuilder.Append("script>\r\n			</th>\r\n			<td class=\"by\">作者</td>\r\n			<td class=\"num\">回复/查看</td>\r\n			<td class=\"by\">最后发表</td>\r\n			</tr>\r\n		</table>\r\n		</div>\r\n		<div class=\"threadlist\">\r\n		<table summary=\"");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\" id=\"threadlist\" cellspacing=\"0\" cellpadding=\"0\">\r\n			");
	int announcement__loop__id=0;
	foreach(DataRow announcement in announcementlist.Rows)
	{
		announcement__loop__id++;

	templateBuilder.Append("\r\n			<tbody>\r\n			<tr>\r\n				<td class=\"folder\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/icon_announcement.gif\" alt=\"announcement\" /></td>\r\n				<td class=\"icon\">&nbsp;</td>\r\n				");
	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("\r\n				<td class=\"icon\">&nbsp;</td>		\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<th class=\"subject f_bold\">\r\n					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("announcement.aspx#" + announcement["id"].ToString().Trim() + "\">" + announcement["title"].ToString().Trim() + "</a>\r\n				</th>\r\n				<td class=\"by\">\r\n					<cite>");
	if (Utils.StrToInt(announcement["posterid"].ToString().Trim(), 0)==-1)
	{

	templateBuilder.Append("\r\n						游客\r\n					");
	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(announcement["posterid"].ToString().Trim());
	
	templateBuilder.Append("\r\n						<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">" + announcement["poster"].ToString().Trim() + "</a>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					</cite>\r\n					<em>" + announcement["starttime"].ToString().Trim() + "</em>\r\n				</td>\r\n				<td class=\"num\">&nbsp;</td>\r\n				<td class=\"by\">&nbsp;</td>\r\n			</tr>\r\n			</tbody>\r\n			");break;

	}	//end loop

	string tdivimg = "";
	

	int toptopic__loop__id=0;
	foreach(TopicInfo toptopic in toptopiclist)
	{
		toptopic__loop__id++;

	templateBuilder.Append("			\r\n			<tbody>\r\n				<tr>\r\n					<td class=\"folder\">\r\n						");	 aspxrewriteurl = this.ShowTopicAspxRewrite(toptopic.Tid,0);
	
	templateBuilder.Append("\r\n						<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/t_top");
	templateBuilder.Append(toptopic.Displayorder.ToString().Trim());
	templateBuilder.Append(".gif\"/></a>\r\n					</td>\r\n					<td class=\"icon\">\r\n						");
	if (toptopic.Special==0)
	{


	if (toptopic.Iconid!=0)
	{

	templateBuilder.Append("\r\n									<img src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(toptopic.Iconid.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"listicon\" />\r\n							");
	}
	else
	{

	templateBuilder.Append("\r\n									&nbsp;\r\n							");
	}	//end if


	}	//end if

//特殊帖图标

	if (toptopic.Special==1)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/pollsmall.gif\" alt=\"投票\" />\r\n						");
	}	//end if


	if (toptopic.Special==2)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/bonus.gif\" alt=\"悬赏\"/>\r\n						");
	}	//end if


	if (toptopic.Special==3)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/rewardsmallend.gif\" alt=\"悬赏已结束\"/>\r\n						");
	}	//end if


	if (toptopic.Special==4)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/debatesmall.gif\" alt=\"辩论\"/>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</td>\r\n					");
	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("\r\n					<td class=\"icon\">						\r\n						");
	if (toptopic.Fid==forum.Fid && toptopic.Closed!=1)
	{

	templateBuilder.Append("\r\n                        <input type=\"checkbox\" name=\"topicid\" topictype=\"displayorder\" value=\"");
	if (toptopic.Closed>1)
	{
	templateBuilder.Append(toptopic.Closed.ToString().Trim());
	}
	else
	{
	templateBuilder.Append(toptopic.Tid.ToString().Trim());
	}	//end if

	templateBuilder.Append("\" onclick=\"modclick(this);\"/>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n						<input type=\"checkbox\" disabled />\r\n						");
	}	//end if

	templateBuilder.Append("	\r\n					</td>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<th class=\"subject hot\">\r\n						");
	if (toptopic.Digest>0)
	{

	templateBuilder.Append("\r\n							<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/digest");
	templateBuilder.Append(toptopic.Digest.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"digtest\"/></label>\r\n						");
	}	//end if


	if (toptopic.Rate>0)
	{

	templateBuilder.Append("\r\n							<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/agree.gif\" alt=\"正分\"/></label>\r\n						");
	}	//end if


	if (toptopic.Rate<0)
	{

	templateBuilder.Append("\r\n							<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/disagree.gif\" alt=\"负分\"/></label>\r\n						");
	}	//end if


	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{


	if (forum.Viewbytopictype==1 && toptopic.Topictypename!="")
	{

	templateBuilder.Append("\r\n							<em>[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(toptopic.Fid.ToString().Trim());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(toptopic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(toptopic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]</em>\r\n							");
	}
	else if (toptopic.Topictypename!="")
	{

	templateBuilder.Append("\r\n							<em>[");
	templateBuilder.Append(toptopic.Topictypename.ToString().Trim());
	templateBuilder.Append("]</em>\r\n							");
	}	//end if


	}	//end if

	 aspxrewriteurl = this.ShowTopicAspxRewrite(toptopic.Tid,0);
	

	if (toptopic.Special==4)
	{

	 aspxrewriteurl = this.ShowDebateAspxRewrite(toptopic.Tid);
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(toptopic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("\r\n							<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(toptopic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n							<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(toptopic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (toptopic.Attachment==1)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment.gif\" alt=\"附件\"/>\r\n						");
	}
	else if (toptopic.Attachment==2)
	{

	templateBuilder.Append("\r\n						   <img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment_image.gif\" alt=\"图片附件\"/>\r\n						");
	}	//end if


	if (toptopic.Special==2)
	{

	templateBuilder.Append("\r\n							- [悬赏 ");
	templateBuilder.Append(bonusextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(toptopic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(bonusextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n						");
	}
	else if (toptopic.Special==3)
	{

	templateBuilder.Append("\r\n							- [悬赏已结束]\r\n						");
	}
	else if (toptopic.Special==0)
	{


	if (toptopic.Price>0)
	{

	templateBuilder.Append("\r\n								- [售价 ");
	templateBuilder.Append(topicextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(toptopic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(topicextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n							");
	}	//end if


	}	//end if


	if (toptopic.Readperm>0)
	{

	templateBuilder.Append("\r\n							- [阅读权限 <span class=\"bold\">");
	templateBuilder.Append(toptopic.Readperm.ToString().Trim());
	templateBuilder.Append("</span>] \r\n						");
	}	//end if


	if (toptopic.Replies/ppp>0)
	{

	templateBuilder.Append("					\r\n							<span class=\"threadpages\"><script type=\"text/javascript\">getpagenumbers(\"");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("\",");
	templateBuilder.Append(toptopic.Replies.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(ppp.ToString());
	templateBuilder.Append(",0,\"\",");
	templateBuilder.Append(toptopic.Tid.ToString().Trim());
	templateBuilder.Append(",1, \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\", aspxrewrite);</");
	templateBuilder.Append("script></span>				\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</th>\r\n					<td class=\"by\">\r\n						<cite>\r\n						");
	if (toptopic.Posterid==-1)
	{

	templateBuilder.Append("\r\n							游客\r\n						");
	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(toptopic.Posterid);
	
	templateBuilder.Append("\r\n							<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(toptopic.Poster.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if

	templateBuilder.Append("</cite>\r\n						");	string ttpdtime = ForumUtils.ConvertDateTime(toptopic.Postdatetime);
	
	templateBuilder.Append("\r\n						<em>");
	templateBuilder.Append(ttpdtime.ToString());
	templateBuilder.Append("</em>\r\n					</td>\r\n					<td class=\"num\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" class=\"xg2\">");
	templateBuilder.Append(toptopic.Replies.ToString().Trim());
	templateBuilder.Append("</a><em>");
	templateBuilder.Append(toptopic.Views.ToString().Trim());
	templateBuilder.Append("</em></td>\r\n					<td class=\"by\">\r\n						<cite>\r\n							");
	if (toptopic.Lastposterid==-1)
	{

	templateBuilder.Append("\r\n								游客\r\n							");
	}
	else
	{

	templateBuilder.Append("\r\n								<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(UserInfoAspxRewrite(toptopic.Lastposterid).ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(toptopic.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n							");
	}	//end if

	templateBuilder.Append("\r\n						</cite>\r\n						<em><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(toptopic.Tid.ToString().Trim());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("&page=end#lastpost\">\r\n						");	string ttlp = ForumUtils.ConvertDateTime(toptopic.Lastpost);
	
	templateBuilder.Append("\r\n						");
	templateBuilder.Append(ttlp.ToString());
	templateBuilder.Append("</a></em>\r\n					</td>\r\n				</tr>\r\n			</tbody>\r\n			");
	}	//end loop


	if (toptopiclist.Count>0 && topiclist.Count>0)
	{

	templateBuilder.Append("\r\n			<tbody class=\"separation\">\r\n				<tr>\r\n					<td class=\"folder\">&nbsp;</td>\r\n					<td class=\"icon\">&nbsp;</td>\r\n					");
	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("\r\n					<td class=\"icon\">&nbsp;</td>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<th>版块主题</th>\r\n					<td class=\"author\">&nbsp;</td>\r\n					<td class=\"nums\">&nbsp;</td>\r\n					<td class=\"lastpost\">&nbsp;</td>\r\n				</tr>\r\n			</tbody>\r\n			");
	}	//end if


	int topic__loop__id=0;
	foreach(TopicInfo topic in topiclist)
	{
		topic__loop__id++;

	templateBuilder.Append("\r\n			<tbody>\r\n				<tr>\r\n					<td class=\"folder\">\r\n					");
	if (topic.Folder!="")
	{

	 aspxrewriteurl = Urls.ShowTopicAspxRewrite(topic.Tid,0, DNTRequest.GetInt("typeid", -1));
	
	templateBuilder.Append("\r\n						<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\" target=\"_blank\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/folder_");
	templateBuilder.Append(topic.Folder.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"topicicon\" /></a>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					</td>\r\n					<td class=\"icon\">\r\n						");
	if (topic.Special==0)
	{


	if (topic.Iconid!=0)
	{

	templateBuilder.Append("\r\n								<img src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(topic.Iconid.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"listicon\" />\r\n							");
	}
	else
	{

	templateBuilder.Append("\r\n								&nbsp;\r\n							");
	}	//end if


	}	//end if

/*特殊帖图标*/

	if (topic.Special==1)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/pollsmall.gif\" alt=\"投票\" />\r\n						");
	}	//end if


	if (topic.Special==2)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/bonus.gif\" alt=\"悬赏\"/>\r\n						");
	}	//end if


	if (topic.Special==3)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/rewardsmallend.gif\" alt=\"悬赏已结束\"/>\r\n						");
	}	//end if


	if (topic.Special==4)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/debatesmall.gif\" alt=\"辩论\"/>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</td>\r\n                    ");
	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("\r\n					<td class=\"icon\">				\r\n						");
	if (topic.Closed!=1)
	{

	templateBuilder.Append("\r\n                             <input type=\"checkbox\" name=\"topicid\" value=\"");
	if (topic.Closed>1)
	{
	templateBuilder.Append(topic.Closed.ToString().Trim());
	}
	else
	{
	templateBuilder.Append(topic.Tid.ToString().Trim());
	}	//end if

	templateBuilder.Append("\" onclick=\"modclick(this);\"/>\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n						<input type=\"checkbox\" disabled />\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</td>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<th class=\"subject\">\r\n						");
	if (topic.Digest>0)
	{

	templateBuilder.Append("							\r\n						<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/digest");
	templateBuilder.Append(topic.Digest.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"精华\"/></label>\r\n						");
	}	//end if


	if (topic.Rate>0)
	{

	templateBuilder.Append("\r\n						<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/agree.gif\" alt=\"正分\"/></label>\r\n						");
	}	//end if


	if (topic.Rate<0)
	{

	templateBuilder.Append("\r\n						<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/disagree.gif\" alt=\"负分\"/></label>\r\n						");
	}	//end if


	if (pageid<3 && forum.Allowthumbnail==1)
	{


	if (topic.Attachment==2)
	{

	templateBuilder.Append("\r\n								<span id=\"t_thumbnail_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" onmouseover=\"showMenu(this.id, 0, 0, 1, 0)\">\r\n								");
	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{

	templateBuilder.Append("\r\n									<em>\r\n									");
	if (forum.Viewbytopictype==1 && topic.Topictypename!="")
	{

	templateBuilder.Append("\r\n									[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]\r\n									");
	}
	else if (topic.Topictypename!="")
	{

	templateBuilder.Append("\r\n									[");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("]\r\n									");
	}	//end if

	templateBuilder.Append("\r\n									</em>\r\n								");
	}	//end if

	 aspxrewriteurl = Urls.ShowTopicAspxRewrite(topic.Tid,0, DNTRequest.GetInt("typeid", -1));
	

	if (topic.Special==4)
	{

	 aspxrewriteurl = Urls.ShowDebateAspxRewrite(topic.Tid, DNTRequest.GetInt("typeid", -1));
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("\r\n									<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n								");
	}
	else
	{

	templateBuilder.Append("\r\n									<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n								");
	}	//end if


	if (topic.Folder=="new")
	{

	templateBuilder.Append("\r\n									<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/posts_new.gif\" />\r\n								");
	}	//end if

	templateBuilder.Append("\r\n								</span>\r\n							");
	}
	else
	{


	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{


	if (forum.Viewbytopictype==1 && topic.Topictypename!="")
	{

	templateBuilder.Append("\r\n									<em>[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]</em>\r\n									");
	}
	else if (topic.Topictypename!="")
	{

	templateBuilder.Append("\r\n									<em>[");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("]</em>\r\n									");
	}	//end if


	}	//end if

	 aspxrewriteurl = Urls.ShowTopicAspxRewrite(topic.Tid,0, DNTRequest.GetInt("typeid", -1));
	

	if (topic.Special==4)
	{

	 aspxrewriteurl = Urls.ShowDebateAspxRewrite(topic.Tid, DNTRequest.GetInt("typeid", -1));
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("\r\n									<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n								");
	}
	else
	{

	templateBuilder.Append("\r\n									<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n								");
	}	//end if


	if (topic.Folder=="new")
	{

	templateBuilder.Append("\r\n									<span class=\"new\">NEW</span>\r\n								");
	}	//end if


	}	//end if


	}
	else
	{


	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{


	if (forum.Viewbytopictype==1 && topic.Topictypename!="")
	{

	templateBuilder.Append("\r\n									<em>[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]</em>\r\n									");
	}
	else if (topic.Topictypename!="")
	{

	templateBuilder.Append("\r\n									<em>[");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("]</em>\r\n									");
	}	//end if


	}	//end if

	 aspxrewriteurl = Urls.ShowTopicAspxRewrite(topic.Tid,0, DNTRequest.GetInt("typeid", -1));
	

	if (topic.Special==4)
	{

	 aspxrewriteurl = Urls.ShowDebateAspxRewrite(topic.Tid, DNTRequest.GetInt("typeid", -1));
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("\r\n								<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n							");
	}
	else
	{

	templateBuilder.Append("\r\n								<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n							");
	}	//end if


	if (topic.Folder=="new")
	{

	templateBuilder.Append("\r\n								<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/posts_new.gif\"/>\r\n							");
	}	//end if


	}	//end if


	if (topic.Attachment==1)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment.gif\" alt=\"附件\"/>\r\n						");
	}
	else if (topic.Attachment==2)
	{

	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment_image.gif\" alt=\"图片附件\"/>\r\n						");
	}	//end if


	if (topic.Special==2)
	{

	templateBuilder.Append("\r\n							- [悬赏 ");
	templateBuilder.Append(bonusextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(bonusextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n						");
	}
	else if (topic.Special==3)
	{

	templateBuilder.Append("\r\n							- [悬赏已结束]\r\n						");
	}
	else if (topic.Special==0)
	{


	if (topic.Price>0)
	{

	templateBuilder.Append("\r\n								- [售价 ");
	templateBuilder.Append(topicextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(topicextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n							");
	}	//end if


	}	//end if


	if (topic.Readperm>0)
	{

	templateBuilder.Append("\r\n							- [阅读权限 <span class=\"bold\">");
	templateBuilder.Append(topic.Readperm.ToString().Trim());
	templateBuilder.Append("</span>] \r\n						");
	}	//end if


	if (topic.Replies/ppp>0)
	{

	templateBuilder.Append("\r\n							<span class=\"threadpages\"><script type=\"text/javascript\">getpagenumbers(\"");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("\", ");
	templateBuilder.Append(topic.Replies.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(ppp.ToString());
	templateBuilder.Append(",0,\"\",");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",1, \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\", aspxrewrite);</");
	templateBuilder.Append("script></span>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</th>\r\n					<td class=\"by\">\r\n						<cite>\r\n							");
	if (topic.Posterid==-1)
	{

	templateBuilder.Append("\r\n								游客\r\n							");
	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);
	
	templateBuilder.Append("\r\n								<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Poster.ToString().Trim());
	templateBuilder.Append("</a>\r\n							");
	}	//end if

	templateBuilder.Append("\r\n						</cite>\r\n						");	string tpdtime = ForumUtils.ConvertDateTime(topic.Postdatetime);
	
	templateBuilder.Append("\r\n						<em>");
	templateBuilder.Append(tpdtime.ToString());
	templateBuilder.Append("</em>\r\n					</td>\r\n					<td class=\"num\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" class=\"xg2\">");
	templateBuilder.Append(topic.Replies.ToString().Trim());
	templateBuilder.Append("</a><em>");
	templateBuilder.Append(topic.Views.ToString().Trim());
	templateBuilder.Append("</em></td>\r\n					<td class=\"by\">\r\n						<cite>\r\n						");
	if (topic.Lastposterid==-1)
	{

	templateBuilder.Append("\r\n							游客\r\n						");
	}
	else
	{

	templateBuilder.Append("\r\n							<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(UserInfoAspxRewrite(topic.Lastposterid).ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(topic.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n						</cite>\r\n						<em><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("&page=end#lastpost\">\r\n						");	string tlp = ForumUtils.ConvertDateTime(topic.Lastpost);
	
	templateBuilder.Append("\r\n						");
	templateBuilder.Append(tlp.ToString());
	templateBuilder.Append("</a></em>\r\n					</td>\r\n				</tr>\r\n				");
	if (pageid<3 && forum.Allowthumbnail==1)
	{


	if (topic.Attachment==2)
	{

	string timg = Attachments.GetThumbnailByTid(topic.Tid,160,ThumbnailType.Thumbnail);
	

	if (timg!="")
	{

	 tdivimg = tdivimg+"<div id='t_thumbnail_" + topic.Tid + "_menu' style='display: none;' class='popupmenu_popup'><img src='" + timg + "' /></div>";
	

	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</tbody>\r\n			");
	}	//end loop

	templateBuilder.Append("\r\n		</table>\r\n		");
	if (topiclist.Count<=0)
	{

	templateBuilder.Append("\r\n			<div class=\"zerothreads\">当前板块暂无主题</div>\r\n		");
	}	//end if


	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("\r\n            <div id=\"modlayer\" style=\"display: none;\">\r\n				<input type=\"hidden\" name=\"optgroup\" />\r\n				<input type=\"hidden\" name=\"operat\" />\r\n				<input type=\"hidden\" name=\"winheight\" />\r\n				<a class=\"collapse\" href=\"javascript:;\" onclick=\"$('modlayer').className='collapsed'\">最小化</a>\r\n				<label><input class=\"checkbox\" type=\"checkbox\" name=\"chkall\" onclick=\"if(!($('modcount').innerHTML = modclickcount = checkall(this.form, 'topicid'))) {$('modlayer').style.display = 'none';}\" /> 全选</label>\r\n				<h4><span>选中</span><strong onmouseover=\"$('moremodoption').style.display='block'\" onclick=\"$('modlayer').className=''\" id=\"modcount\"></strong><span>篇: </span></h4>\r\n				<p>\r\n					<strong><a href=\"javascript:;\" onclick=\"modthreads(3, 'delete');return false;\">删除</a></strong>\r\n					<span class=\"pipe\">|</span>\r\n					<strong><a href=\"javascript:;\" onclick=\"modthreads(2, 'move');return false;\">移动</a></strong>\r\n					<span class=\"pipe\">|</span>\r\n					<strong><a href=\"javascript:;\" onclick=\"modthreads(2, 'type');return false;\">分类</a></strong>\r\n					<span class=\"pipe\">|</span>\r\n					<strong><a href=\"javascript:;\" onclick=\"modthreads(2, 'identify');return false;\">鉴定</a></strong>\r\n				</p>\r\n				<div id=\"moremodoption\">\r\n					<a href=\"javascript:;\" onclick=\"modthreads(1, 'displayorder');return false;\">置顶</a>\r\n					<a href=\"javascript:;\" onclick=\"modthreads(1, 'highlight');return false;\">高亮</a>\r\n					<a href=\"javascript:;\" onclick=\"modthreads(1, 'digest');return false;\">精华</a>\r\n					<span class=\"pipe\">|</span>\r\n					<a href=\"javascript:;\" onclick=\"modthreads(3, 'bump');return false;\">提升下沉</a>\r\n					<a href=\"javascript:;\" onclick=\"modthreads(4,'close');return false;\">关闭打开</a>\r\n				</div>\r\n            </div>\r\n        ");
	}	//end if

	templateBuilder.Append("\r\n		</form>\r\n	</div>\r\n</div>\r\n<div class=\"pages_btns cl\">\r\n	<div class=\"pages\">\r\n		<cite class=\"pageback z\" id=\"visitedforums\"");
	if (showvisitedforumsmenu)
	{

	templateBuilder.Append(" onmouseover=\"$('visitedforums').id = 'visitedforumstmp';this.id = 'visitedforums';showMenu({'ctrlid':this.id, 'pos':'34'});\"");
	}	//end if

	templateBuilder.Append("><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\">返回</a></cite>\r\n		");
	if (pagecount!=1)
	{

	templateBuilder.Append("\r\n			");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n			");
	if (pagecount>8)
	{

	templateBuilder.Append("\r\n			<kbd>\r\n			<input name=\"gopage\" type=\"text\" class=\"txt\" id=\"pageidinput2\" title=\"可以输入页码按回车键自动跳转\" value=\"");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" style=\"text-align:center;\" onfocus=\"this.value=this.defaultValue;this.select();\" onKeyDown=\"pageinputOnKeyDown(this,event);\" size=\"2\" maxlength=\"9\" />/ ");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("</kbd>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			");
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

	templateBuilder.Append(" onmouseover=\"if($('newspecial2_menu')!=null&&$('newspecial_menu').childNodes.length>0)  showMenu(this.id);\"");
	}	//end if

	templateBuilder.Append(" id=\"newspecial2\">\r\n        <a title=\"发新话题\" id=\"newtopic2\" href=\"");
	templateBuilder.Append(newtopicurl.ToString());
	templateBuilder.Append("\" onclick=\"");
	templateBuilder.Append(newtopiconclick.ToString());
	templateBuilder.Append("\">\r\n            <img alt=\"发新话题\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/newtopic.png\"  style=\"display:inline\"/></a>\r\n    </span>\r\n");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n");
	templateBuilder.Append(tdivimg.ToString());
	templateBuilder.Append("\r\n");
	if (canquickpost)
	{


	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\"></");
	templateBuilder.Append("script>\r\n");	string seditorid = "infloatquickposttopic";
	

	if (infloat!=1)
	{

	 seditorid = "quickposttopic";
	

	}	//end if

	string poster = "";
	
	int postid = 0;
	
	int postlayer = 0;
	
	templateBuilder.Append("\r\n<form method=\"post\" name=\"postform\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\" action=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">\r\n<div id=\"quickpost\" class=\"quickpost cl ");
	if (infloat!=1)
	{

	templateBuilder.Append("main");
	}	//end if

	templateBuilder.Append("\">\r\n	");
	if (infloat!=1)
	{

	templateBuilder.Append("\r\n	<h4 class=\"bm_h\">\r\n	");
	}
	else
	{

	templateBuilder.Append("\r\n	<h4 class=\"flb\">\r\n	");
	}	//end if


	if (infloat==1)
	{

	templateBuilder.Append("\r\n	<span class=\"y\">\r\n		<a title=\"关闭\" onclick=\"hideWindow('newthread')\" class=\"flbc\" href=\"javascript:;\">关闭</a>\r\n	</span>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<em>快速发帖</em>\r\n    ");
	if (infloat==1 && needaudit)
	{

	templateBuilder.Append("\r\n    <span class=\"needverify\">需审核</span>\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n	</h4>\r\n	<div class=\"bm_inner c cl\">\r\n		");
	if (infloat!=1)
	{


	if (quickeditorad!="")
	{

	templateBuilder.Append("\r\n		<div class=\"leaderboard\">");
	templateBuilder.Append(quickeditorad.ToString());
	templateBuilder.Append("</div>\r\n		");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		<div class=\"pbt cl\">\r\n			");
	if (forum.Applytopictype==1 && topictypeselectoptions!="")
	{

	templateBuilder.Append("\r\n			<div class=\"ftid\">\r\n				<select name=\"typeid\" id=\"typeid\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"1\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"5\"");
	}	//end if

	templateBuilder.Append(">");
	templateBuilder.Append(topictypeselectoptions.ToString());
	templateBuilder.Append("</select>\r\n				<script type=\"text/javascript\">simulateSelect('typeid');</");
	templateBuilder.Append("script>\r\n			</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<input type=\"text\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title\" name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" size=\"60\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"2\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"6\"");
	}	//end if

	templateBuilder.Append(" value=\"\" class=\"txt postpx\"/>\r\n            标题最多为60个字符，还可输入<b><span id=\"chLeft\">60</span></b>\r\n            <script type=\"text/javascript\">checkLength($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title'), 60); //检查标题长度</");
	templateBuilder.Append("script>\r\n			<em id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("validatemessage\"></em>\r\n		</div>\r\n		<div class=\"pbt cl\">\r\n			<span>\r\n			<input type=\"hidden\" value=\"usergroupinfo.allowhtml}\" name=\"htmlon\" id=\"htmlon\"/>\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append("\" name=\"parseurloff\" id=\"parseurloff\"/>\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("\" name=\"smileyoff\" id=\"smileyoff\" />\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append("\" name=\"bbcodeoff\" id=\"bbcodeoff\"/>\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(usesig.ToString());
	templateBuilder.Append("\" name=\"usesig\" id=\"usesig\"/>\r\n			</span>\r\n			<script type=\"text/javascript\">\r\n				var bbinsert = parseInt('1');\r\n				var smiliesCount = 24;\r\n				var colCount = 8;\r\n			</");
	templateBuilder.Append("script>\r\n			<div ");
	if (infloat!=1)
	{

	templateBuilder.Append("style=\"margin-right:170px;\" ");
	}
	else
	{

	templateBuilder.Append("style=\"width:600px;\"");
	}	//end if

	templateBuilder.Append(">\r\n			");	char comma = ',';
	

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



	templateBuilder.Append("\r\n			<div class=\"postarea cl\">\r\n				<div class=\"postinner\">\r\n				");
	if (canposttopic)
	{

	templateBuilder.Append("\r\n				<textarea ");
	if (infloat!=1)
	{

	templateBuilder.Append("rows=\"5\"");
	}
	else
	{

	templateBuilder.Append("rows=\"7\"");
	}	//end if

	templateBuilder.Append(" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("message\" onKeyDown=\"seditor_ctlent(event, 'fastvalidate($(\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\\'),\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("\\')','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"3\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"6\"");
	}	//end if

	templateBuilder.Append("  style=\"background-image:url(" + quickbgad[1].ToString().Trim() + ");background-repeat:no-repeat;background-position:50% 50%;\" ");
	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"");
	}	//end if

	templateBuilder.Append("></textarea>\r\n                ");
	}
	else
	{

	templateBuilder.Append("\r\n				<div class=\"hm p_login cl\">你需要登录后才可以发帖 <a class=\"xg2\" onclick=\"hideWindow('register');showWindow('login', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx\">登录</a> | <a class=\"xg2\" onclick=\"hideWindow('login');showWindow('register', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\">注册</a></div>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n			</div>\r\n			</div>\r\n		</div>\r\n		");
	if (isseccode)
	{

	templateBuilder.Append("\r\n		<div class=\"pbt\" style=\"position: relative;\">\r\n		");
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

	templateBuilder.Append("\r\n		</div>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<div class=\"pbt\">\r\n		    ");
	if (canposttopic)
	{

	templateBuilder.Append("\r\n			<button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" onclick=\"fastsubmit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" name=\"topicsubmit\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"5\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"8\"");
	}	//end if

	templateBuilder.Append(" class=\"pn\"><span>发表帖子</span></button> <span class=\"grayfont\">[Ctrl+Enter快速发布]</span>\r\n			");
	}
	else
	{

	templateBuilder.Append("\r\n			<button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"topicsubmit\" ");
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
	templateBuilder.Append("login.aspx');\" class=\"pn\"><span>发表帖子</span></button>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<a href=\"###\" id=\"adlinkbtn\" style=\"display:none;\" onclick=\"window.open('" + quickbgad[0].ToString().Trim() + "','_blank');\">查看背景广告</a>\r\n			<a href=\"###\" id=\"closebtn\" style=\"display:none;\" onclick=\"$('quickpostmessage').style.background='';this.style.display='none';$('adlinkbtn').style.display='none';\">隐藏</a>\r\n			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"");
	templateBuilder.Append(forum.Postbytopictype.ToString().Trim());
	templateBuilder.Append("\" tabindex=\"3\" />\r\n		</div>\r\n	</div>\r\n</div>\r\n</form>");


	}	//end if


	if (config.Whosonlinestatus!=0 && config.Whosonlinestatus!=1)
	{

	templateBuilder.Append("\r\n	<div class=\"bm cl\" id=\"online\">\r\n		<div class=\"bm_h\">		\r\n			<span class=\"l_action\" style=\"display:none\">\r\n				");
	if (DNTRequest.GetString("showonline")=="no")
	{

	templateBuilder.Append("\r\n					<a href=\"showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&showonline=yes#online\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_no.gif\" alt=\"收起\" />\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n					<a href=\"showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&showonline=no#online\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_yes.gif\" alt=\"展开\" />\r\n				");
	}	//end if

	templateBuilder.Append("</a>\r\n			</span>\r\n			<h3>\r\n				<strong>在线用户</strong> - <em id=\"forumtotalonline\">");
	templateBuilder.Append(forumtotalonline.ToString());
	templateBuilder.Append("</em> 人在线<span id=\"invisible\"></span>\r\n			</h3>\r\n		</div>\r\n		<dl id=\"onlinelist\">\r\n			<dt style=\"display:none\">");
	templateBuilder.Append(onlineiconlist.ToString());
	templateBuilder.Append("</dt>\r\n			");
	if (showforumonline)
	{

	templateBuilder.Append("\r\n			<dd>\r\n			<ul class=\"userlist cl\">\r\n				");	int invisiblecount = 0;
	

	if (forumtotalonline!=0)
	{


	int onlineuser__loop__id=0;
	foreach(OnlineUserInfo onlineuser in onlineuserlist)
	{
		onlineuser__loop__id++;


	if (onlineuser.Invisible==1)
	{

	 invisiblecount = invisiblecount + 1;
	
	templateBuilder.Append("\r\n						<li style=\"overflow:hidden;text-align:center;height:70px;width:80px;line-height:60px\">(隐身会员)</li>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n						<li style=\"overflow:hidden;text-align:center;height:70px;width:80px\">\r\n							");	string avatarurl = Avatars.GetAvatarUrl(onlineuser.Userid, AvatarSize.Small);
	
	templateBuilder.Append("\r\n								<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" id=\"memberinfo_" + onlineuser__loop__id.ToString() + "\" style=\"border:1px solid #E8E8E8;padding:1px;width:48px;height:48px;\" />\r\n							");
	if (onlineuser.Userid==-1)
	{

	templateBuilder.Append("\r\n								<p>");
	templateBuilder.Append(onlineuser.Username.ToString().Trim());
	templateBuilder.Append("</p>\r\n							");
	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(onlineuser.Userid);
	
	templateBuilder.Append("\r\n								<p><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(onlineuser.Username.ToString().Trim());
	templateBuilder.Append("</a></p>\r\n							");
	}	//end if

	templateBuilder.Append("\r\n						</li>\r\n					");
	}	//end if


	}	//end loop


	if (invisiblecount>0)
	{

	templateBuilder.Append("\r\n					<script type=\"text/javascript\">$('invisible').innerHTML = '(");
	templateBuilder.Append(invisiblecount.ToString());
	templateBuilder.Append("' + \" 隐身)\";</");
	templateBuilder.Append("script>\r\n				");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n                  <script type=\"text/javascript\">$('forumtotalonline').innerHTML = parseInt($('forumtotalonline').innerHTML)+1;</");
	templateBuilder.Append("script>\r\n					<li style=\"overflow:hidden;text-align:center;height:70px;width:80px\">\r\n						");	string avatarurl = Avatars.GetAvatarUrl(userid, AvatarSize.Small);
	
	templateBuilder.Append("\r\n							<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" style=\"border:1px solid #E8E8E8;padding:1px;width:48px;height:48px;\" />\r\n						");
	if (userid==-1)
	{

	templateBuilder.Append("\r\n							<p>");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</p>\r\n						");
	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(userid);
	
	templateBuilder.Append("\r\n							<p><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</a></p>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</li>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</ul>\r\n			</dd>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</dl>\r\n	</div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n");
	if (userid<0||canposttopic)
	{

	templateBuilder.Append("\r\n	<ul class=\"popupmenu_popup newspecialmenu\" id=\"newspecial_menu\" style=\"display: none\">\r\n	");
	if (forum.Allowspecialonly<=0)
	{

	templateBuilder.Append("\r\n	<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" >发新主题</a></li>\r\n	");
	}	//end if


	if ((forum.Allowpostspecial&1)==1 && usergroupinfo.Allowpostpoll==1)
	{

	templateBuilder.Append("\r\n		<li class=\"poll\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&type=poll&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\">发布投票</a></li>\r\n	");
	}	//end if


	if ((forum.Allowpostspecial&4)==4 && usergroupinfo.Allowbonus==1)
	{

	templateBuilder.Append("\r\n		<li class=\"reward\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&type=bonus&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\">发布悬赏</a></li>\r\n	");
	}	//end if


	if ((forum.Allowpostspecial&16)==16 && usergroupinfo.Allowdebate==1)
	{

	templateBuilder.Append("\r\n		<li class=\"debate\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&type=debate&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" >发起辩论</a></li>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</ul>\r\n	<ul class=\"popupmenu_popup newspecialmenu\" id=\"newspecial2_menu\" style=\"display: none\">\r\n	</ul>\r\n    <ul class=\"popupmenu_popup newspecialmenu\" id=\"seditor_newspecial_menu\" style=\"display: none\">\r\n	</ul>\r\n	<script type=\"text/javascript\">\r\n	    $('newspecial2_menu').innerHTML = $('newspecial_menu').innerHTML;\r\n	    $('seditor_newspecial_menu').innerHTML = $('newspecial_menu').innerHTML;\r\n	</");
	templateBuilder.Append("script>\r\n");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	<script type=\"text/javascript\">\r\n		var maxpage = parseInt('");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("');\r\n		var pageid = parseInt('");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("');\r\n		if(maxpage > 1) {\r\n			document.onkeyup = function(e){\r\n				e = e ? e : window.event;\r\n				var tagname = is_ie ? e.srcElement.tagName : e.target.tagName;\r\n				if(tagname == 'INPUT' || tagname == 'TEXTAREA') return;\r\n				actualCode = e.keyCode ? e.keyCode : e.charCode;\r\n				if(pageid < maxpage && actualCode == 39) {\r\n					window.location = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(Urls.ShowForumAspxRewrite(forumid,pageid+1,forum.Rewritename).ToString().Trim());
	templateBuilder.Append("';\r\n				}\r\n				if(pageid > 1 && actualCode == 37) {\r\n					window.location = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(Urls.ShowForumAspxRewrite(forumid,pageid-1,forum.Rewritename).ToString().Trim());
	templateBuilder.Append("';\r\n				}\r\n			}\r\n		}\r\n	</");
	templateBuilder.Append("script>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n");
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
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(simpforuminfo.Url.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(simpforuminfo.Name.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n		");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n	</ul>\r\n</div>\r\n");
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



	templateBuilder.Append("\r\n");
	templateBuilder.Append(mediaad.ToString());
	templateBuilder.Append("\r\n");
	}
	else
	{


	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\"></");
	templateBuilder.Append("script>\r\n");	string seditorid = "infloatquickposttopic";
	

	if (infloat!=1)
	{

	 seditorid = "quickposttopic";
	

	}	//end if

	string poster = "";
	
	int postid = 0;
	
	int postlayer = 0;
	
	templateBuilder.Append("\r\n<form method=\"post\" name=\"postform\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\" action=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">\r\n<div id=\"quickpost\" class=\"quickpost cl ");
	if (infloat!=1)
	{

	templateBuilder.Append("main");
	}	//end if

	templateBuilder.Append("\">\r\n	");
	if (infloat!=1)
	{

	templateBuilder.Append("\r\n	<h4 class=\"bm_h\">\r\n	");
	}
	else
	{

	templateBuilder.Append("\r\n	<h4 class=\"flb\">\r\n	");
	}	//end if


	if (infloat==1)
	{

	templateBuilder.Append("\r\n	<span class=\"y\">\r\n		<a title=\"关闭\" onclick=\"hideWindow('newthread')\" class=\"flbc\" href=\"javascript:;\">关闭</a>\r\n	</span>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<em>快速发帖</em>\r\n    ");
	if (infloat==1 && needaudit)
	{

	templateBuilder.Append("\r\n    <span class=\"needverify\">需审核</span>\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n	</h4>\r\n	<div class=\"bm_inner c cl\">\r\n		");
	if (infloat!=1)
	{


	if (quickeditorad!="")
	{

	templateBuilder.Append("\r\n		<div class=\"leaderboard\">");
	templateBuilder.Append(quickeditorad.ToString());
	templateBuilder.Append("</div>\r\n		");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		<div class=\"pbt cl\">\r\n			");
	if (forum.Applytopictype==1 && topictypeselectoptions!="")
	{

	templateBuilder.Append("\r\n			<div class=\"ftid\">\r\n				<select name=\"typeid\" id=\"typeid\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"1\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"5\"");
	}	//end if

	templateBuilder.Append(">");
	templateBuilder.Append(topictypeselectoptions.ToString());
	templateBuilder.Append("</select>\r\n				<script type=\"text/javascript\">simulateSelect('typeid');</");
	templateBuilder.Append("script>\r\n			</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<input type=\"text\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title\" name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" size=\"60\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"2\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"6\"");
	}	//end if

	templateBuilder.Append(" value=\"\" class=\"txt postpx\"/>\r\n            标题最多为60个字符，还可输入<b><span id=\"chLeft\">60</span></b>\r\n            <script type=\"text/javascript\">checkLength($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title'), 60); //检查标题长度</");
	templateBuilder.Append("script>\r\n			<em id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("validatemessage\"></em>\r\n		</div>\r\n		<div class=\"pbt cl\">\r\n			<span>\r\n			<input type=\"hidden\" value=\"usergroupinfo.allowhtml}\" name=\"htmlon\" id=\"htmlon\"/>\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append("\" name=\"parseurloff\" id=\"parseurloff\"/>\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("\" name=\"smileyoff\" id=\"smileyoff\" />\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append("\" name=\"bbcodeoff\" id=\"bbcodeoff\"/>\r\n			<input type=\"hidden\" value=\"");
	templateBuilder.Append(usesig.ToString());
	templateBuilder.Append("\" name=\"usesig\" id=\"usesig\"/>\r\n			</span>\r\n			<script type=\"text/javascript\">\r\n				var bbinsert = parseInt('1');\r\n				var smiliesCount = 24;\r\n				var colCount = 8;\r\n			</");
	templateBuilder.Append("script>\r\n			<div ");
	if (infloat!=1)
	{

	templateBuilder.Append("style=\"margin-right:170px;\" ");
	}
	else
	{

	templateBuilder.Append("style=\"width:600px;\"");
	}	//end if

	templateBuilder.Append(">\r\n			");	char comma = ',';
	

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



	templateBuilder.Append("\r\n			<div class=\"postarea cl\">\r\n				<div class=\"postinner\">\r\n				");
	if (canposttopic)
	{

	templateBuilder.Append("\r\n				<textarea ");
	if (infloat!=1)
	{

	templateBuilder.Append("rows=\"5\"");
	}
	else
	{

	templateBuilder.Append("rows=\"7\"");
	}	//end if

	templateBuilder.Append(" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("message\" onKeyDown=\"seditor_ctlent(event, 'fastvalidate($(\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\\'),\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("\\')','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"3\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"6\"");
	}	//end if

	templateBuilder.Append("  style=\"background-image:url(" + quickbgad[1].ToString().Trim() + ");background-repeat:no-repeat;background-position:50% 50%;\" ");
	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"");
	}	//end if

	templateBuilder.Append("></textarea>\r\n                ");
	}
	else
	{

	templateBuilder.Append("\r\n				<div class=\"hm p_login cl\">你需要登录后才可以发帖 <a class=\"xg2\" onclick=\"hideWindow('register');showWindow('login', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("login.aspx\">登录</a> | <a class=\"xg2\" onclick=\"hideWindow('login');showWindow('register', this.href);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\">注册</a></div>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n			</div>\r\n			</div>\r\n		</div>\r\n		");
	if (isseccode)
	{

	templateBuilder.Append("\r\n		<div class=\"pbt\" style=\"position: relative;\">\r\n		");
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

	templateBuilder.Append("\r\n		</div>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<div class=\"pbt\">\r\n		    ");
	if (canposttopic)
	{

	templateBuilder.Append("\r\n			<button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" onclick=\"fastsubmit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" name=\"topicsubmit\" ");
	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"5\"");
	}
	else
	{

	templateBuilder.Append("tabindex=\"8\"");
	}	//end if

	templateBuilder.Append(" class=\"pn\"><span>发表帖子</span></button> <span class=\"grayfont\">[Ctrl+Enter快速发布]</span>\r\n			");
	}
	else
	{

	templateBuilder.Append("\r\n			<button type=\"button\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"topicsubmit\" ");
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
	templateBuilder.Append("login.aspx');\" class=\"pn\"><span>发表帖子</span></button>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<a href=\"###\" id=\"adlinkbtn\" style=\"display:none;\" onclick=\"window.open('" + quickbgad[0].ToString().Trim() + "','_blank');\">查看背景广告</a>\r\n			<a href=\"###\" id=\"closebtn\" style=\"display:none;\" onclick=\"$('quickpostmessage').style.background='';this.style.display='none';$('adlinkbtn').style.display='none';\">隐藏</a>\r\n			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"");
	templateBuilder.Append(forum.Postbytopictype.ToString().Trim());
	templateBuilder.Append("\" tabindex=\"3\" />\r\n		</div>\r\n	</div>\r\n</div>\r\n</form>");


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
