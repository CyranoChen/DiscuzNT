<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.showdebate" %>
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
	templateBuilder.Append("\";\r\nvar ismoder = ");
	templateBuilder.Append(ismoder.ToString());
	templateBuilder.Append(";\r\nvar userid = parseInt('");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("');\r\nvar forumallowhtml =true;\r\nvar imagedir = \"");
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
	templateBuilder.Append("/bbcode.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_debate.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post_editor.js\"></");
	templateBuilder.Append("script>\r\n");
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

	templateBuilder.Append("\r\n	<a id=\"forumlist\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\" ");
	if (config.Forumjump==1)
	{

	templateBuilder.Append("onmouseover=\"showMenu(this.id);\" onmouseout=\"showMenu(this.id);\"");
	}	//end if

	templateBuilder.Append(" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a> &raquo; ");
	templateBuilder.Append(ShowForumAspxRewrite(forum.Pathlist.Trim(),forumid,forumpageid).ToString().Trim());
	templateBuilder.Append("\r\n	");	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("\r\n	  &raquo; <strong>");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</strong>\r\n	");
	}
	else
	{

	templateBuilder.Append("\r\n	  &raquo; <strong>");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</strong>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n</div>\r\n<div class=\"wrap cl\">\r\n");
	if (config.Forumjump==1)
	{

	templateBuilder.Append("\r\n	");
	templateBuilder.Append(Caches.GetForumListMenuDivCache(usergroupid,userid,config.Extname).ToString().Trim());
	templateBuilder.Append("\r\n");
	}	//end if

	templateBuilder.Append("\r\n<div class=\"main viewthread\">\r\n	<div id=\"postsContainer\">\r\n	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"辩论主题\">	\r\n		<tr>\r\n		<td class=\"postauthor\">\r\n		");
	if (debatepost.Posterid!=-1)
	{

	templateBuilder.Append("\r\n			<!-- member menu -->\r\n			<div class=\"popupmenu_popup userinfopanel\" id=\"");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("\" style=\"display:none; clip: rect(auto auto auto auto); position absolute;\" initialized ctrlkey=\"userinfo2\">\r\n				<div class=\"popavatar\">\r\n					<div id=\"");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("_ma\"></div>\r\n					<ul class=\"profile_side\">\r\n						<li class=\"post_pm\"><a href=\"usercppostpm.aspx?msgtoid=");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_postpm', this.href, 600, 410, '600,0');doane(event);\" target=\"_blank\">发送短消息</a></li>\r\n					</ul>\r\n				</div>\r\n				<div class=\"popuserinfo\">\r\n					<dl class=\"cl\">\r\n						<dt>UID</dt><dd>");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("</dd>\r\n						<dt>精华</dt><dd>");
	if (debatepost.Digestposts>0)
	{

	templateBuilder.Append("<a href=\"search.aspx?posterid=");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("&type=digest\">");
	templateBuilder.Append(debatepost.Digestposts.ToString().Trim());
	templateBuilder.Append("</a>");
	}
	else
	{
	templateBuilder.Append(debatepost.Digestposts.ToString().Trim());
	}	//end if

	templateBuilder.Append("</dd>\r\n					");
	if (score[1].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[1].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits1.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[1].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[2].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[2].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits2.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[2].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[3].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[3].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits3.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[3].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[4].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[4].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits4.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[4].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[5].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[5].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits5.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[5].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[6].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[6].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits6.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[6].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[7].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[7].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits7.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[7].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (score[8].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n						<dt>" + score[8].ToString().Trim() + "</dt><dd>");
	templateBuilder.Append(debatepost.Extcredits8.ToString().Trim());
	templateBuilder.Append(" " + scoreunit[8].ToString().Trim() + "</dd>\r\n					");
	}	//end if


	if (debatepost.Location!="")
	{

	templateBuilder.Append("\r\n						<dt>来自</dt><dd>");
	templateBuilder.Append(debatepost.Location.ToString().Trim());
	templateBuilder.Append("</dd>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					</dl>\r\n					<div class=\"imicons cl\">\r\n						");
	if (debatepost.Msn!="")
	{

	templateBuilder.Append("\r\n						<a href=\"mailto:");
	templateBuilder.Append(debatepost.Msn.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\" class=\"msn\">");
	templateBuilder.Append(debatepost.Msn.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (debatepost.Skype!="")
	{

	templateBuilder.Append("\r\n						<a href=\"skype:");
	templateBuilder.Append(debatepost.Skype.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\" class=\"skype\">");
	templateBuilder.Append(debatepost.Skype.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (debatepost.Icq!="")
	{

	templateBuilder.Append("\r\n						<a href=\"http://wwp.icq.com/scripts/search.dll?to=");
	templateBuilder.Append(debatepost.Icq.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\" class=\"icq\">");
	templateBuilder.Append(debatepost.Icq.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (debatepost.Qq!="")
	{

	templateBuilder.Append("\r\n						<a href=\"http://wpa.qq.com/msgrd?V=1&Uin=");
	templateBuilder.Append(debatepost.Qq.ToString().Trim());
	templateBuilder.Append("&Site=");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("&Menu=yes\" target=\"_blank\" class=\"qq\">");
	templateBuilder.Append(debatepost.Qq.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if


	if (debatepost.Yahoo!="")
	{

	templateBuilder.Append("\r\n						<a href=\"http://edit.yahoo.com/config/send_webmesg?.target=");
	templateBuilder.Append(debatepost.Yahoo.ToString().Trim());
	templateBuilder.Append("&.src=pg\" target=\"_blank\" class=\"yahoo\">");
	templateBuilder.Append(debatepost.Yahoo.ToString().Trim());
	templateBuilder.Append("</a>\r\n						");
	}	//end if

	templateBuilder.Append("\r\n					</div>\r\n					<div class=\"imicons cl\">\r\n						");	 aspxrewriteurl = this.UserInfoAspxRewrite(debatepost.Posterid);
	
	templateBuilder.Append("\r\n						<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"public_info\">查看公共资料</a>\r\n						<a href=\"search.aspx?posterid=");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("\" class=\"all_topic\">搜索帖子</a>\r\n					");
	if (useradminid>0)
	{


	if (admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n						<a onclick=\"floatwin('open_getip', this.href, 400, 170, '600,0');doane(event);\"  href=\"getip.aspx?pid=");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" title=\"查看IP\" class=\"ip\">查看IP</a>\r\n					");
	}	//end if


	if (admininfo.Allowbanuser==1)
	{

	templateBuilder.Append("\r\n						<a href=\"useradmin.aspx?action=banuser&uid=");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"floatwin('open_mods', this.href, 250, 270, '600,0');doane(event);\" title=\"禁止用户\" class=\"forbid_user\">禁止用户</a>\r\n					");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n					</div>\r\n				</div>\r\n			</div>\r\n			<!-- member menu -->\r\n			");
	}	//end if


	if (debatepost.Posterid!=-1)
	{

	templateBuilder.Append("\r\n			<div class=\"poster\">\r\n				<span  ");
	if (debatepost.Onlinestate==1)
	{

	templateBuilder.Append("class=\"onlineyes\" title=\"在线\"");
	}
	else
	{

	templateBuilder.Append("class=\"onlineno\" title=\"未在线\"");
	}	//end if

	templateBuilder.Append(">");
	templateBuilder.Append(debatepost.Poster.ToString().Trim());
	templateBuilder.Append("</span>\r\n			</div>\r\n			<div id=\"");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append("_a\">\r\n			");
	if (config.Showavatars==1)
	{

	templateBuilder.Append("\r\n			<div class=\"avatar\">\r\n			");	string avatarurl = Avatars.GetAvatarUrl(debatepost.Posterid);
	
	templateBuilder.Append("\r\n			    <img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_medium.gif';\" onmouseover=\"showauthor(this,");
	templateBuilder.Append(debatepost.Posterid.ToString().Trim());
	templateBuilder.Append(")\"/>\r\n			</div>\r\n			");
	}	//end if


	if (debatepost.Nickname!="")
	{

	templateBuilder.Append("\r\n			<p>昵称<em>:");
	templateBuilder.Append(debatepost.Nickname.ToString().Trim());
	templateBuilder.Append("</em></p>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			<p>\r\n			<script type=\"text/javascript\">\r\n				ShowStars(");
	templateBuilder.Append(debatepost.Stars.ToString().Trim());
	templateBuilder.Append(", ");
	templateBuilder.Append(config.Starthreshold.ToString().Trim());
	templateBuilder.Append(");\r\n			</");
	templateBuilder.Append("script>\r\n			</p>\r\n			<ul class=\"otherinfo\">\r\n		");
	if (config.Userstatusby==1)
	{

	templateBuilder.Append("\r\n			<li><label>组别</label>");
	templateBuilder.Append(debatepost.Status.ToString().Trim());
	templateBuilder.Append("</li>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n			<li><label>性别</label><script type=\"text/javascript\">document.write(displayGender(");
	templateBuilder.Append(debatepost.Gender.ToString().Trim());
	templateBuilder.Append("));</");
	templateBuilder.Append("script></span></li>\r\n		");
	if (debatepost.Bday!="")
	{

	templateBuilder.Append("\r\n			<li><label>生日</label>");
	templateBuilder.Append(debatepost.Bday.ToString().Trim());
	templateBuilder.Append("</li>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n			<li><label>来自</label>");
	templateBuilder.Append(debatepost.Location.ToString().Trim());
	templateBuilder.Append("</li>\r\n			<li><label>积分</label>");
	templateBuilder.Append(debatepost.Credits.ToString().Trim());
	templateBuilder.Append("</li>\r\n			<li><label>帖子</label>");
	templateBuilder.Append(debatepost.Posts.ToString().Trim());
	templateBuilder.Append("</li>\r\n			<li><label>注册时间</label>");
	if (debatepost.Joindate!="")
	{

	templateBuilder.Append(Convert.ToDateTime(debatepost.Joindate).ToString("yyyy-MM-dd"));

	}	//end if

	templateBuilder.Append("</li>\r\n			</ul>\r\n			");
	if (debatepost.Medals!="")
	{

	templateBuilder.Append("\r\n			<div class=\"medals\">");
	templateBuilder.Append(debatepost.Medals.ToString().Trim());
	templateBuilder.Append("</div>\r\n			");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n			<div style=\"padding-left:15px;\">\r\n			    <em>");
	templateBuilder.Append(debatepost.Poster.ToString().Trim());
	templateBuilder.Append("-");
	templateBuilder.Append(debatepost.Ip.ToString().Trim());
	templateBuilder.Append("</em>\r\n				");
	if (useradminid>0 && admininfo.Allowviewip==1)
	{

	templateBuilder.Append("\r\n					<a href=\"getip.aspx?pid=");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" onclick=\"floatwin('open_getip', this.href, 400, 170, '600,0');doane(event);\" title=\"查看IP\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/ip.gif\" alt=\"查看IP\"/></a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			<p><em>未注册</em></p>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		</td>\r\n		<td class=\"postcontent\">\r\n			<div class=\"topictitle\">\r\n				<h1>");
	templateBuilder.Append(debatepost.Title.ToString().Trim());
	templateBuilder.Append("<span>开始时间 <em>");	templateBuilder.Append(Convert.ToDateTime(debatepost.Postdatetime).ToString(" yyyy-MM-dd HH:mm"));
	templateBuilder.Append("</em> -- 结束时间 <em>");	templateBuilder.Append(Convert.ToDateTime(debateexpand.Terminaltime).ToString(" yyyy-MM-dd HH:mm"));
	templateBuilder.Append("</em></span></h1>\r\n			</div>\r\n			<div class=\"pi\">\r\n				<div class=\"postinfo\">\r\n					");
	templateBuilder.Append(debatepost.Poster.ToString().Trim());
	templateBuilder.Append("\r\n					<em>\r\n						发表于");	templateBuilder.Append(Convert.ToDateTime(debatepost.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
	templateBuilder.Append("\r\n					</em>\r\n				</div>\r\n			</div>\r\n			<div class=\"postmessage defaultpost\">\r\n				<div class=\"t_msgfont\">\r\n				    ");
	if (debatepost.Id==1)
	{

	templateBuilder.Append("\r\n					    <div id=\"firstpost\">\r\n			                <div id=\"topictag\"></div>\r\n				    ");
	}	//end if

	templateBuilder.Append("\r\n						");
	templateBuilder.Append(debatepost.Message.ToString().Trim());
	templateBuilder.Append("\r\n						<div class=\"debate_show\">\r\n							<p>正方观点</p>\r\n							");
	templateBuilder.Append(debateexpand.Positiveopinion.ToString().Trim());
	templateBuilder.Append("\r\n							<p>反方观点</p>\r\n							");
	templateBuilder.Append(debateexpand.Negativeopinion.ToString().Trim());
	templateBuilder.Append("\r\n						</div>\r\n					");
	if (debatepost.Id==1)
	{

	templateBuilder.Append("\r\n					</div>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n			");
	if (enabletag)
	{

	templateBuilder.Append("				\r\n				<script type=\"text/javascript\">\r\n					function forumhottag_callback(data)\r\n					{\r\n						tags = data;\r\n					}\r\n				</");
	templateBuilder.Append("script>\r\n				<script type=\"text/javascript\" src=\"cache/hottags_forum_cache_jsonp.txt\"></");
	templateBuilder.Append("script>\r\n					");	int hastag = Topics.GetMagicValue(topic.Magic, MagicType.TopicTag);
	

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


	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n			<div class=\"useraction cl\">\r\n		");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n			<a href=\"favorites.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&infloat=1\" onclick=\"ajaxmenu(event, this.id, 3000, 0)\" id=\"ajax_favorite\">收藏</a>\r\n			");
	if (ismoder==1)
	{


	if (debatepost.Posterid!=-1)
	{

	templateBuilder.Append("\r\n				<a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("');\" id=\"ratelink\" >评分</a>\r\n				");
	}	//end if


	}
	else
	{


	if (usergroupinfo.Raterange!="" && debatepost.Posterid!=-1)
	{

	templateBuilder.Append("\r\n				<a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("');\" id=\"ratelink\" >评分</a>\r\n				");
	}	//end if


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

	templateBuilder.Append("\r\n		</td>\r\n		</tr>\r\n		<tr>\r\n		<td class=\"postauthor\">&nbsp;</td>\r\n		<td class=\"postactions\">\r\n			<div class=\"p_control\">\r\n				<cite class=\"y\">\r\n				");
	if (ismoder==1)
	{


	if (debatepost.Posterid!=-1)
	{


	if (debatepost.Ratetimes>0)
	{

	templateBuilder.Append("\r\n						<a href=\"###\" onclick=\"action_onchange('cancelrate',$('moderate'),'");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("');\">撤销</a><span class=\"pipe\">|</span>\r\n						");
	}	//end if


	}	//end if


	if (debatepost.Layer==0 && topic.Special==4)
	{


	if (isenddebate==true  && userid==debatepost.Posterid)
	{

	templateBuilder.Append("\r\n						<a href=\"###\" onClick=\"showMenu(this.id)\" id=\"commentdebates\" name=\"commentdebates\">点评</a><span class=\"pipe\">|</span>\r\n						");
	}	//end if


	}	//end if


	}	//end if


	if (userid!=-1)
	{

	ShowtopicPagePostInfo post = debatepost;
	

	templateBuilder.Append("<script type=\"text/javascript\">\r\n    show_report_button(");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(",");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append(",");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append(");\r\n</");
	templateBuilder.Append("script>");

	templateBuilder.Append("<span class=\"pipe\">|</span>\r\n				");
	}	//end if

	templateBuilder.Append("	\r\n					<a href=\"#\" onclick=\"window.scrollTo(0,0)\">TOP</a>\r\n				</cite>\r\n		");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n				<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n				<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\" >删除</a>			\r\n		");
	}
	else
	{


	if (debatepost.Posterid!=-1 && userid==debatepost.Posterid)
	{


	if (topic.Closed==0)
	{

	templateBuilder.Append("\r\n				<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"editpost\">编辑</a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(debatepost.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"return confirm('确定要删除吗?');\" class=\"delpost\" >删除</a>\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n		</td>\r\n		</tr>\r\n		<tbody>\r\n		<tr>\r\n			<td class=\"postauthor\"></td>\r\n			<td class=\"adcontent\">\r\n			</td>\r\n		</tr>\r\n		</tbody>\r\n	</table>\r\n	</div>\r\n	</div>\r\n	<div id=\"commentdebates_menu\" style=\"display: none; width:270px;\" class=\"popupmenu_popup\">\r\n		<form id=\"commentform\" >\r\n			<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n			  <tr>\r\n			 <td><textarea name=\"commentdebatesmsg\" cols=\"43\" rows=\"6\" id=\"commentdebatesmsg\"></textarea></td>\r\n			  </tr>                                                      \r\n			  <tr>\r\n				<td><input type=\"button\" value=\"提交\"  onclick=\"commentdebates(");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",'firstpost')\"/></td>\r\n			  </tr>\r\n			</table>\r\n		</form>\r\n	</div>\r\n	<div class=\"main\">\r\n	<div id=\"ajaxdebateposts\">\r\n		<h1>\r\n			");	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	
	templateBuilder.Append("\r\n			 辩论详情 <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	}	//end if

	templateBuilder.Append("\" style=\"font-size:12px;text-decoration:underline;\">普通模式</a>\r\n		</h1>\r\n		<div class=\"debatebox cl\">\r\n			<div class=\"specialtitle\">\r\n				<div class=\"squaretitle\">\r\n					<p>正方观点</p>\r\n					");
	templateBuilder.Append(debateexpand.Positiveopinion.ToString().Trim());
	templateBuilder.Append("\r\n				</div>\r\n				<div class=\"sidetitle right\">\r\n					<p>反方观点</p>\r\n					");
	templateBuilder.Append(debateexpand.Negativeopinion.ToString().Trim());
	templateBuilder.Append("\r\n				</div>\r\n			</div>\r\n			<div class=\"balance\">\r\n				<span class=\"scalevalue1\"><b id=\"positivediggs\">");
	templateBuilder.Append(debateexpand.Positivediggs.ToString().Trim());
	templateBuilder.Append("</b></span>\r\n				<span class=\"scalevalue\"><b id=\"negativediggs\">");
	templateBuilder.Append(debateexpand.Negativediggs.ToString().Trim());
	templateBuilder.Append("</b></span>\r\n				<div id=\"positivepercent\" class=\"squareboll\" style=\"width:");
	templateBuilder.Append(positivepercent.ToString());
	templateBuilder.Append("%;\"></div>\r\n			</div>\r\n			<div class=\"talkinner f_clear\">\r\n				<div class=\"squarebox\">\r\n				");
	if (!isenddebate)
	{

	templateBuilder.Append("\r\n					<div class=\"buttoncontrol\"><button onclick=\"$('positivepostform').style.display='';this.style.display='none';\">加入正方</button></div>\r\n					<div id=\"positivepostform\" style=\"display: none;\">\r\n						<form method=\"post\" name=\"postform_");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" id=\"postform_");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\"	enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this);\" >\r\n							<input type=\"hidden\" id=\"title\" name=\"title\" size=\"84\" tabindex=\"1\" value=\"\" />\r\n							<input type=\"hidden\" id=\"postid\" name=\"postid\" value=\"-1\" />\r\n							<input type=\"hidden\" name=\"debateopinion\" value=\"1\" />\r\n							<input type=\"hidden\" name=\"parseurloff\" value=\"");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"smileyoff\" value=\"");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"bbcodeoff\" value=\"");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"usesig\" value=\"0\" />\r\n							<table cellspacing=\"0\" cellpadding=\"0\" summary=\"正方观点\">\r\n								<tr><td>我的意见：</td></tr>\r\n								<tr>\r\n									<td>\r\n										<textarea name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" cols=\"50\" rows=\"4\" class=\"autosave txtarea\" id=\"message\" tabindex=\"2\" onkeydown=\"debatequickreply(event, this.form);\" onfocus=\"textareachange(this.form.id)\";></textarea>\r\n									</td>\r\n								</tr>\r\n								<tr>\r\n									<td>\r\n									");
	if (isseccode)
	{

	templateBuilder.Append("<div id=\"debate_vcode\" name=\"debate_vcode\"><span style=\"position: relative;\">验证码:");
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

	templateBuilder.Append("</span></div>");
	}	//end if

	templateBuilder.Append("\r\n									<button type=\"submit\" name=\"replysubmit\">我要发表</button>\r\n									</td>\r\n								</tr>\r\n							</table>\r\n						</form>\r\n					</div>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					<div class=\"buttoncontrol\"></div>\r\n					");
	}	//end if


	if (positivepostlist.Count>0)
	{

	templateBuilder.Append("\r\n						<div id=\"positive_pagenumbers_top\" class=\"debatepages\">");
	templateBuilder.Append(positivepagenumbers.ToString());
	templateBuilder.Append("</div>\r\n						<div id=\"positivepage_owner\">\r\n							");
	int positivepost__loop__id=0;
	foreach(ShowtopicPagePostInfo positivepost in positivepostlist)
	{
		positivepost__loop__id++;

	templateBuilder.Append("\r\n								<div class=\"square\">\r\n									<table cellspacing=\"0\" cellpadding=\"0\" summary=\"正方观点\">\r\n									<tbody>\r\n									<tr>\r\n									<td rowspan=\"2\" class=\"supportbox\">\r\n										<p>支持度\r\n										<span class=\"talknum\" id=\"diggs");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(positivepost.Diggs.ToString().Trim());
	templateBuilder.Append("</span>\r\n										");
	if (!isenddebate  && positivepost.Posterid!=userid)
	{


	if (!positivepost.Digged)
	{

	templateBuilder.Append("\r\n										<span class=\"cliktalk\" id=\"cliktalk");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\"><a href=\"###\" onclick=\"digg(");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",1)\">支持</a></span>\r\n										");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n										</p>\r\n									</td>\r\n									<td class=\"comment\">\r\n										<h3>\r\n										<span class=\"y\" style=\"font-size:12px;\">\r\n										");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n											<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("&debate=1\">编辑</a><cite class=\"pipe\">|</cite>\r\n											<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("&opinion=1\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n										");
	}
	else
	{


	if (positivepost.Posterid!=-1 && userid==positivepost.Posterid)
	{

	templateBuilder.Append("\r\n											<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("&debate=1\">编辑</a><cite class=\"pipe\">|</cite>\r\n											<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("&opinion=1\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n											");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n										</span>\r\n										发表者:<a id=\"poster");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\" href=\"");
	templateBuilder.Append(UserInfoAspxRewrite(positivepost.Posterid).ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(positivepost.Poster.ToString().Trim());
	templateBuilder.Append("</a>\r\n										</h3>\r\n										<div class=\"debatemessage\"  id=\"message");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\">\r\n										");
	templateBuilder.Append(positivepost.Message.ToString().Trim());
	templateBuilder.Append("\r\n										</div>\r\n										");
	if (!isenddebate  && positivepost.Posterid!=userid)
	{

	templateBuilder.Append("\r\n										<input name=\"hiddendpid");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\" type=\"hidden\" id=\"hiddendpid");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\" value=\"");
	templateBuilder.Append(positivepost.Ubbmessage.ToString().Trim());
	templateBuilder.Append("\" />\r\n										<p class=\"othertalk\"><a id=\"reply_btn_");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\" href=\"###\" onclick=\"showDebatReplyBox(");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(", ");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append(", 2, ");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append(", '");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("');this.style.display='none';\">我不同意</a><div id=\"reply_box_owner_");
	templateBuilder.Append(positivepost.Pid.ToString().Trim());
	templateBuilder.Append("\"></div>\r\n										</p>\r\n										");
	}	//end if

	templateBuilder.Append("\r\n									</td>\r\n									</tr>\r\n									<tr>\r\n									  	<td class=\"comment\">时间:");	templateBuilder.Append(Convert.ToDateTime(positivepost.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
	templateBuilder.Append("</td>\r\n									</tr>\r\n									</tbody>\r\n									</table>\r\n								</div>\r\n							");
	}	//end loop

	templateBuilder.Append("\r\n						</div>\r\n						<div id=\"positive_pagenumbers_buttom\" class=\"debatepages\">");
	templateBuilder.Append(positivepagenumbers.ToString());
	templateBuilder.Append("</div>\r\n						");
	if (!isenddebate)
	{

	templateBuilder.Append("\r\n						<div class=\"buttoncontrol\"><button onclick=\"$('positivepostform2').style.display='';this.style.display='none';\">加入正方</button></div>\r\n						<div id=\"positivepostform2\" style=\"display:none;\">\r\n						       <form method=\"post\" name=\"postform_");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" id=\"Form1\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\"	enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this);\" >\r\n							    <input type=\"hidden\" id=\"Hidden1\" name=\"title\" size=\"84\" tabindex=\"1\" value=\"\" />\r\n							    <input type=\"hidden\" id=\"Hidden2\" name=\"postid\" value=\"-1\" />\r\n							    <input type=\"hidden\" name=\"debateopinion\" value=\"1\" />\r\n							    <input type=\"hidden\" name=\"parseurloff\" value=\"");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append("\" />\r\n							    <input type=\"hidden\" name=\"smileyoff\" value=\"");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("\" />\r\n							    <input type=\"hidden\" name=\"bbcodeoff\" value=\"");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append("\" />\r\n							    <input type=\"hidden\" name=\"usesig\" value=\"0\" />\r\n							    <table cellspacing=\"0\" cellpadding=\"0\" summary=\"正方观点\">\r\n								    <tr><td>我的意见：</td></tr>\r\n								    <tr>\r\n									    <td>\r\n										    <textarea name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" cols=\"50\" rows=\"4\" class=\"autosave txtarea\" id=\"Textarea1\" tabindex=\"2\" onkeydown=\"debatequickreply(event, this.form);\" onfocus=\"textareachange(this.form.id)\";></textarea>\r\n									    </td>\r\n								    </tr>\r\n								    <tr>\r\n									    <td>\r\n									    ");
	if (isseccode)
	{

	templateBuilder.Append("<div id=\"Div1\" name=\"debate_vcode\"><span style=\"position: relative;\">验证码:");
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

	templateBuilder.Append("</span></div>");
	}	//end if

	templateBuilder.Append("\r\n									    <button type=\"submit\" name=\"replysubmit\">我要发表</button>\r\n									    </td>\r\n								    </tr>\r\n							    </table>\r\n						    </form>\r\n						</div>\r\n						");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n				<div class=\"oppositionbox right\">\r\n				");
	if (!isenddebate)
	{

	templateBuilder.Append("\r\n					<div class=\"buttoncontrol\"><button onclick=\"$('negativepostform').style.display='';this.style.display='none';\">加入反方</button></div>\r\n					<div id=\"negativepostform\" style=\"display: none;\" >\r\n						<form method=\"post\" name=\"postform_");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" id=\"postform_");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\"	enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this);\" >\r\n							<input type=\"hidden\" id=\"title\" name=\"title\" size=\"84\" tabindex=\"1\" value=\"\"/>\r\n							<input type=\"hidden\" id=\"postid\" name=\"postid\" value=\"-1\" />\r\n							<input type=\"hidden\" name=\"debateopinion\" value=\"2\" />\r\n							<input type=\"hidden\" name=\"parseurloff\" value=\"");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"smileyoff\" value=\"");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"bbcodeoff\" value=\"");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"usesig\" value=\"0\" />\r\n							<table cellspacing=\"0\" cellpadding=\"0\" summary=\"反方观点\">\r\n								<tr>\r\n									<td>我的意见：</td>\r\n								</tr>\r\n								<tr>\r\n									<td>\r\n										<textarea name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" cols=\"50\" rows=\"4\" class=\"autosave txtarea\" id=\"message\" tabindex=\"2\" onkeydown=\"debatequickreply(event, this.form);\" onfocus=\"textareachange(this.form.id);\"></textarea>\r\n									</td>\r\n								</tr>\r\n								<tr>\r\n									<td>\r\n										");
	if (isseccode)
	{

	templateBuilder.Append("<div id=\"debate_vcode\" name=\"debate_vcode\"><span style=\"position: relative;\">验证码:");
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

	templateBuilder.Append("</span></div>");
	}	//end if

	templateBuilder.Append("<button type=\"submit\" name=\"replysubmit\"/>我要发表</button>\r\n									</td>\r\n								</tr>\r\n							</table>\r\n						</form>\r\n					</div>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					<div class=\"buttoncontrol\"></div>\r\n					");
	}	//end if


	if (negativepostlist.Count>0)
	{

	templateBuilder.Append("\r\n						<div id=\"negative_pagenumbers_top\" class=\"debatepages\">");
	templateBuilder.Append(negativepagenumbers.ToString());
	templateBuilder.Append("</div>\r\n						<div id=\"negativepage_owner\">\r\n							");
	int negativepost__loop__id=0;
	foreach(ShowtopicPagePostInfo negativepost in negativepostlist)
	{
		negativepost__loop__id++;

	templateBuilder.Append("\r\n								<div class=\"square cl\">\r\n									<table cellspacing=\"0\" cellpadding=\"0\" summary=\"反方观点\">\r\n									<tbody>\r\n									<tr>\r\n									<td rowspan=\"2\" class=\"supportbox\">\r\n											<p>支持度\r\n											<span class=\"talknum\" id=\"diggs");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(negativepost.Diggs.ToString().Trim());
	templateBuilder.Append("</span>\r\n											");
	if (!isenddebate && negativepost.Posterid!=userid)
	{


	if (!negativepost.Digged)
	{

	templateBuilder.Append("\r\n											<span class=\"cliktalk\" id=\"cliktalk");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\"><a href=\"###\" onclick=\"digg(");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",2)\">支持</a></span>\r\n										");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n										</p>\r\n									</td>\r\n									<td class=\"comment\">\r\n										<h3>\r\n										<span class=\"y\" style=\"font-size:12px;\">\r\n											");
	if (ismoder==1)
	{

	templateBuilder.Append("\r\n												<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("&debate=1\">编辑</a><cite class=\"pipe\">|</cite>\r\n												<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("&opinion=2\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n											");
	}
	else
	{


	if (negativepost.Posterid!=-1 && userid==negativepost.Posterid)
	{

	templateBuilder.Append("\r\n												<a href=\"editpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("&debate=1\">编辑</a><cite class=\"pipe\">|</cite>\r\n												<a href=\"delpost.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&postid=");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("&opinion=2\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n												");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n										</span>发表者:<a id=\"poster");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\" href=\"");
	templateBuilder.Append(UserInfoAspxRewrite(negativepost.Posterid).ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(negativepost.Poster.ToString().Trim());
	templateBuilder.Append("</a>\r\n										</h3>\r\n										<div class=\"debatemessage\" id=\"message");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\">\r\n										");
	templateBuilder.Append(negativepost.Message.ToString().Trim());
	templateBuilder.Append("										</div>\r\n										");
	if (!isenddebate  && negativepost.Posterid!=userid)
	{

	templateBuilder.Append("\r\n											<input name=\"hiddendpid");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\" type=\"hidden\" id=\"hiddendpid");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\" value=\"");
	templateBuilder.Append(negativepost.Ubbmessage.ToString().Trim());
	templateBuilder.Append("\" />\r\n										<p class=\"othertalk\"><a href=\"###\" id=\"reply_btn_");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"showDebatReplyBox(");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(", ");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append(", 1, ");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append(", ");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append(", '");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("');this.style.display='none';\">我不同意</a><div id=\"reply_box_owner_");
	templateBuilder.Append(negativepost.Pid.ToString().Trim());
	templateBuilder.Append("\"></div>\r\n										</p>\r\n										");
	}	//end if

	templateBuilder.Append("									</td>\r\n									</tr>\r\n									<tr>\r\n									  	<td class=\"comment\">时间:");	templateBuilder.Append(Convert.ToDateTime(negativepost.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
	templateBuilder.Append("</td>\r\n									</tr>\r\n									</tbody>\r\n									</table>\r\n								</div>\r\n							");
	}	//end loop

	templateBuilder.Append("\r\n						</div>\r\n						<div id=\"negative_pagenumbers_buttom\" class=\"debatepages\">");
	templateBuilder.Append(negativepagenumbers.ToString());
	templateBuilder.Append("</div>\r\n						");
	if (!isenddebate)
	{

	templateBuilder.Append("\r\n						<div class=\"buttoncontrol\">\r\n						<button onclick=\"$('negativepostform2').style.display='';this.style.display='none';\">加入反方</button></div>\r\n						<div id=\"negativepostform2\" style=\"display:none;\">\r\n						    <form method=\"post\" name=\"postform_");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\" id=\"Form2\" action=\"postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("\"	enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this);\" >\r\n							<input type=\"hidden\" id=\"Hidden3\" name=\"title\" size=\"84\" tabindex=\"1\" value=\"\"/>\r\n							<input type=\"hidden\" id=\"Hidden4\" name=\"postid\" value=\"-1\" />\r\n							<input type=\"hidden\" name=\"debateopinion\" value=\"2\" />\r\n							<input type=\"hidden\" name=\"parseurloff\" value=\"");
	templateBuilder.Append(parseurloff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"smileyoff\" value=\"");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"bbcodeoff\" value=\"");
	templateBuilder.Append(bbcodeoff.ToString());
	templateBuilder.Append("\" />\r\n							<input type=\"hidden\" name=\"usesig\" value=\"0\" />\r\n							<table cellspacing=\"0\" cellpadding=\"0\" summary=\"反方观点\">\r\n								<tr>\r\n									<td>我的意见：</td>\r\n								</tr>\r\n								<tr>\r\n									<td>\r\n										<textarea name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" cols=\"50\" rows=\"4\" class=\"autosave txtarea\" id=\"Textarea2\" tabindex=\"2\" onkeydown=\"debatequickreply(event, this.form);\" onfocus=\"textareachange(this.form.id);\"></textarea>\r\n									</td>\r\n								</tr>\r\n								<tr>\r\n									<td>\r\n										");
	if (isseccode)
	{

	templateBuilder.Append("<div id=\"Div2\" name=\"debate_vcode\"><span style=\"position: relative;\">验证码:");
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

	templateBuilder.Append("</span></div>");
	}	//end if

	templateBuilder.Append("<button type=\"submit\" name=\"replysubmit\"/>我要发表</button>\r\n									</td>\r\n								</tr>\r\n							</table>\r\n						</form>\r\n						</div>\r\n						");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n			</div>\r\n		</div>\r\n	</div>\r\n</div>\r\n</div>\r\n</div>\r\n</div>\r\n");
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


	if (config.Forumjump==1)
	{

	templateBuilder.Append("\r\n	");
	templateBuilder.Append(navhomemenu.ToString());
	templateBuilder.Append("\r\n");
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
