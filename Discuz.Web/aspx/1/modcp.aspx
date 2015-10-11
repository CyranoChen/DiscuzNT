<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.modcp" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/6/2 16:12:50.
		本页面代码由Discuz!NT模板引擎生成于 2011/6/2 16:12:50. 
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
	templateBuilder.Append(config.Jqueryurl.ToString().Trim());
	templateBuilder.Append("\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\n    jQuery.noConflict();\r\n</");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_modcp.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_quickreply.js\"></");
	templateBuilder.Append("script>\r\n<div class=\"wrap cl pageinfo\">\r\n	<div id=\"nav\">\r\n		");
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
	templateBuilder.Append("</a> &raquo; <strong>管理面板</strong>\r\n	</div>\r\n</div>\r\n");
	if (page_err==0)
	{


	if (ispost && editusersubmit||banusersubmit||editannouncement||deleteannoucement)
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

	templateBuilder.Append("\r\n<div class=\"wrap uc cl\">\r\n	<div class=\"uc_app\">\r\n		<h2>管理面板</h2>\r\n		<ul>\r\n			");
	if (admingroupinfo.Allowpostannounce==1)
	{

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=list&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">公告</a></li>\r\n			");
	}	//end if


	if (admingroupinfo.Allowedituser==1)
	{

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=edituser&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">编辑用户</a></li>\r\n			");
	}	//end if


	if (admingroupinfo.Allowbanuser==1)
	{

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=banusersearch&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">禁止用户</a></li>					\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=forumaccesslist&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">用户权限</a></li>\r\n			");
	if (admingroupinfo.Allowbanip==1)
	{

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=showbannedlist&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">禁止 IP</a></li>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=editforum&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">版块编辑</a></li>\r\n			");
	if (admingroupinfo.Allowmodpost==1)
	{

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=audittopic&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=-2\"><span id=\"audittopiccount\" class=\"y numbg\">");
	templateBuilder.Append(auditTopicCount.ToString());
	templateBuilder.Append("</span>审核主题</a></li>\r\n			<li><a href=\"modcp.aspx?operation=auditpost&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&tablelist=");
	templateBuilder.Append(tableid.ToString());
	templateBuilder.Append("&filter=1\"><span id=\"auditpostcount\" class=\"y numbg\">");
	templateBuilder.Append(auditPostCount.ToString());
	templateBuilder.Append("</span>审核回复</a></li>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=attention&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">主题关注</a></li>\r\n			");
	if (admingroupinfo.Allowviewlog==1)
	{

	templateBuilder.Append("<li><a href=\"modcp.aspx?operation=logs&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">管理日志</a></li>");
	}	//end if

	templateBuilder.Append("\r\n			<li><a href=\"modcp.aspx?operation=userout&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">返回论坛</a></li>\r\n		</ul>\r\n	</div>\r\n	<div class=\"uc_main\">\r\n	<div class=\"uc_content\">\r\n	");
	if (operation=="login")
	{

	templateBuilder.Append("\r\n		<h1>管理登录</h1>\r\n		<form action=\"\" method=\"post\" class=\"exfm\">\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" name=\"operation\"/>\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"管理登录\">\r\n		<tbody>\r\n			<tr>\r\n				<th width=\"60\"><label for=\"cpname\">用户名</label></th><td><input type=\"text\" name=\"cpname\" class=\"txt\" value=\"");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("\" readonly=\"readonly\"/></td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"cppwd\">密码</label></th><td><input type=\"password\" name=\"cppwd\" class=\"txt\"/></td>\r\n			</tr>\r\n			<tr>\r\n				<th></th><td><button type=\"submit\" id=\"Submit1\" name=\"Submit\" class=\"pn\"><span>提交</span></button></td>\r\n			</tr>\r\n		</table>\r\n		</form>\r\n		<div class=\"hintinfo\">当您首次进入管理面板或者长时间没有管理动作时，您需要输入密码才能进入。如果密码输入错误超过 3 次，管理面板将会锁定。30 分钟或者更长时间后，管理面板才能解除锁定。\r\n		</div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<!--批量删帖-->\r\n	");
	if (operation=="deleteuserpost")
	{

	templateBuilder.Append("\r\n		<h1>批量删帖</h1>\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"批量删帖\">\r\n		<tbody>\r\n			<tr>\r\n				<td>\r\n				<form action=\"modcp.aspx?operation=deleteuserpost\" method=\"post\">\r\n					<input type=\"hidden\" value=\"topics\" name=\"deletetype\"/>\r\n					删除该用户\r\n					<select name=\"delTopicDate\">\r\n						<option value=\"1\">1</option>\r\n						<option value=\"2\">2</option>\r\n						<option value=\"3\">3</option>\r\n						<option value=\"4\">4</option>\r\n						<option value=\"5\">5</option>\r\n						<option value=\"6\">6</option>\r\n						<option value=\"7\">7</option>\r\n						<option value=\"8\">8</option>\r\n						<option value=\"9\">9</option>\r\n						<option value=\"10\">10</option>\r\n					 </select>天内的主题<button type=\"submit\" id=\"Submit1\" name=\"Submit\">提交</button>\r\n				</form>\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n				<td>\r\n			    <form action=\"modcp.aspx?operation=deleteuserpost\" method=\"post\">\r\n					<input type=\"hidden\" value=\"posts\" name=\"deletetype\"/>\r\n					删除该用户\r\n					<select name=\"delTopicDate\">\r\n						<option value=\"1\">1</option>\r\n						<option value=\"2\">2</option>\r\n						<option value=\"3\">3</option>\r\n						<option value=\"4\">4</option>\r\n						<option value=\"5\">5</option>\r\n						<option value=\"6\">6</option>\r\n						<option value=\"7\">7</option>\r\n						<option value=\"8\">8</option>\r\n						<option value=\"9\">9</option>\r\n						<option value=\"10\">10</option>\r\n					 </select>天内的回帖<button type=\"submit\" id=\"Submit1\" name=\"Submit\" class=\"pn\"><span>提交</span></button>\r\n				</form>			 \r\n				</td>\r\n			</tr>\r\n		</tbody>\r\n	</table>\r\n	</form>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<!--添加公告-->\r\n	");
	if (operation=="addannouncements" && admingroupinfo.Allowpostannounce==1)
	{

	templateBuilder.Append("\r\n		<h1>添加公告</h1>\r\n		<form action=\"modcp.aspx?operation=add\" method=\"post\">\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" name=\"operation\"/>\r\n		<input type=\"hidden\" value=\"0\" name=\"displayorder\"/>\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"添加公告\">\r\n		<tbody>\r\n			<tr>\r\n				<th>标题</th><td><input type=\"text\" size=\"80\" id=\"subject\" name=\"subject\" value=\"\" class=\"txt\"/></td>\r\n			</tr>\r\n			<tr>\r\n				<th>起始时间</th><td><input type=\"text\" style=\"cursor:default\" onClick=\"showcalendar(event, 'starttime', 'cal_startdate1', 'cal_enddate1', '");
	templateBuilder.Append(nowdatetime.ToString());
	templateBuilder.Append("');\" name=\"starttime\" value='");
	templateBuilder.Append(starttime.ToString());
	templateBuilder.Append("' id=\"starttime\" class=\"txt\"/>\r\n					<input type=\"hidden\" name=\"cal_startdate1\" id=\"cal_startdate1\" size=\"10\"  value=\"1900-01-01\" />\r\n					<input type=\"hidden\" name=\"cal_enddate1\" id=\"cal_enddate1\" size=\"10\"  value=\"2099-01-01\" />\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n				<th>结束时间</th><td><input type=\"text\" style=\"cursor:default\" onClick=\"showcalendar(event, 'endtime', 'cal_startdate2', 'cal_enddate2', 'nowdatetime');\" name=\"endtime\" value='");
	templateBuilder.Append(endtime.ToString());
	templateBuilder.Append("' id=\"endtime\" class=\"txt\"/>\r\n				<input type=\"hidden\" name=\"cal_startdate2\" id=\"cal_startdate2\" size=\"10\"  value=\"1900-01-01\" />\r\n				<input type=\"hidden\" name=\"cal_enddate2\" id=\"cal_enddate2\" size=\"10\"  value=\"2099-01-01\" />\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n				<th>内容</th><td><textarea cols=\"80\" rows=\"10\" id=\"message\" name=\"message\" class=\"txtarea\" ></textarea></td>\r\n			</tr>\r\n			<tr>\r\n				<th> </th><td><button type=\"submit\" id=\"addannouncement_Submit\" name=\"Submit\" class=\"pn\"><span>提交</span></button></td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_calendar.js\"></");
	templateBuilder.Append("script>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<!--编辑公告-->\r\n	");
	if (operation=="editannouncements")
	{

	templateBuilder.Append("\r\n		<h1>编辑公告</h1>\r\n		<form action=\"modcp.aspx?operation=updateannouncements\" method=\"post\">\r\n		<input type=\"hidden\" value=");
	templateBuilder.Append(id.ToString());
	templateBuilder.Append(" name=\"id\"/>\r\n		<input type=\"hidden\" value=");
	templateBuilder.Append(displayorder.ToString());
	templateBuilder.Append(" name=\"displayorder\"/>\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"编辑公告\">\r\n		<tbody>\r\n			<tr>\r\n				<th>标题</th><td><input type=\"text\" size=\"80\" id=\"subject\" name=\"subject\" value=");
	templateBuilder.Append(subject.ToString());
	templateBuilder.Append(" class=\"txt\"/></td>\r\n			</tr>\r\n			<tr>\r\n			<th>起始时间</th>\r\n			<td>\r\n				<input type=\"text\" style=\"cursor:default\" onClick=\"showcalendar(event, 'starttime', 'cal_startdate1', 'cal_enddate1', '");
	templateBuilder.Append(nowdatetime.ToString());
	templateBuilder.Append("');\" name=\"starttime\" value=\"");
	templateBuilder.Append(starttime.ToString());
	templateBuilder.Append("\" id=\"starttime\" class=\"txt\"/>\r\n				<input type=\"hidden\" name=\"cal_startdate1\" id=\"cal_startdate1\" size=\"10\"  value=\"1900-01-01\" />\r\n				<input type=\"hidden\" name=\"cal_enddate1\" id=\"cal_enddate1\" size=\"10\"  value=\"2099-01-01\" />\r\n			</td>\r\n			</tr>\r\n			<tr>\r\n				<th>结束时间</th>\r\n				<td>\r\n				<input type=\"text\" style=\"cursor:default\" onClick=\"showcalendar(event, 'endtime', 'cal_startdate2', 'cal_enddate2', 'nowdatetime');\" name=\"endtime\" value=\"");
	templateBuilder.Append(endtime.ToString());
	templateBuilder.Append("\" id=\"endtime\" class=\"txt\"/>\r\n				<input type=\"hidden\" name=\"cal_startdate2\" id=\"cal_startdate2\" size=\"10\"  value=\"1900-01-01\" />\r\n				<input type=\"hidden\" name=\"cal_enddate2\" id=\"cal_enddate2\" size=\"10\"  value=\"2099-01-01\" />\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n				<th>内容</th><td><textarea cols=\"80\" rows=\"10\" id=\"message\" name=\"message\" class=\"txtarea\" >");
	templateBuilder.Append(message.ToString());
	templateBuilder.Append("</textarea></td>\r\n			</tr>\r\n			<tr>\r\n				<th></th><td><button type=\"submit\" id=\"editannoucementsubmit\" name=\"editannoucementsubmit\" class=\"pn\"><span>提交</span></button></td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_calendar.js\"></");
	templateBuilder.Append("script>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<!--公告列表-->\r\n	");
	if (operation=="list")
	{

	templateBuilder.Append("\r\n		<form action=\"modcp.aspx?operation=manage&op=delannouncements\" method=\"post\"  class=\"datalist\">\r\n		<h1>公告列表</h1>\r\n		<input type=\"hidden\" value=");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append(" name=\"operation\"/>\r\n		<input type=\"hidden\" value=");
	templateBuilder.Append(op.ToString());
	templateBuilder.Append(" name=\"op\"/>\r\n		<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"datatable\">\r\n		<thead>\r\n		<tr class=\"colplural\">\r\n			<th width=\"8%\"><input type=\"checkbox\" onclick=\"checkall(this.form)\" name=\"chkall\" id=\"chkall\"/>删除</th>\r\n			<th width=\"5%\">顺序</th>\r\n			<th width=\"12%\">作者</th>\r\n			<th>标题</th>\r\n			<th width=\"15%\">起始时间</th>\r\n			<th width=\"15%\">结束时间</th>\r\n			<th width=\"8%\">操作</th>\r\n		</tr>\r\n		</thead>\r\n		");
	if (counts<=0)
	{

	templateBuilder.Append("\r\n		<tbody>\r\n			<tr>\r\n				<td colspan=\"7\">暂无公告</td>\r\n			</tr>\r\n		</tbody>\r\n		");
	}
	else
	{


	int announcement__loop__id=0;
	foreach(DataRow announcement in announcementlist.Rows)
	{
		announcement__loop__id++;

	templateBuilder.Append("\r\n		<tbody>\r\n			<tr>\r\n				<td>");
	if (username=="admin"||username==announcement["poster"].ToString().Trim())
	{

	templateBuilder.Append("\r\n				<input type=\"checkbox\" name='aidlist' id='aidlist' value=\"" + announcement["id"].ToString().Trim() + "\" class=\"checkbox\"/>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n				<input type=\"checkbox\" name='aidlist' id='aidlist' disabled=\"disabled\" value=\"" + announcement["id"].ToString().Trim() + "\" class=\"checkbox\"/>	\r\n				");
	}	//end if

	templateBuilder.Append("</td>	\r\n				<td>\r\n				");
	if (username=="admin"||username==announcement["poster"].ToString().Trim())
	{

	templateBuilder.Append("\r\n				<input type=\"text\" size=\"3\" id=\"displayorder\" name=\"displayorder\" value=" + announcement["displayorder"].ToString().Trim() + "  class=\"txt\"/>");
	}
	else
	{

	templateBuilder.Append("<input type=\"text\" size=\"3\" readonly=\"readonly\" disabled id=\"displayorder\" name=\"displayorder\" value=" + announcement["displayorder"].ToString().Trim() + "  class=\"txt\"/>");
	}	//end if

	templateBuilder.Append("\r\n				<input type=\"hidden\" name=\"hid\" id=\"hid\" size=\"10\"  value=\"" + announcement["id"].ToString().Trim() + "\" />\r\n				</td>						\r\n				<td>" + announcement["poster"].ToString().Trim() + "</td>\r\n				<td class=\"datatitle\">" + announcement["title"].ToString().Trim() + "</td>\r\n				<td class=\"time\">" + announcement["starttime"].ToString().Trim() + " </td>\r\n				<td class=\"time\">" + announcement["endtime"].ToString().Trim() + "</td>\r\n				<td>\r\n				");
	if (username=="admin"||(username==announcement["poster"].ToString().Trim()))
	{

	templateBuilder.Append("\r\n				<a href=\"modcp.aspx?operation=editannouncements&id=" + announcement["id"].ToString().Trim() + "\">编辑\r\n				");
	}	//end if

	templateBuilder.Append("</td>\r\n			</tr>\r\n		</tbody>\r\n		");
	}	//end loop


	}	//end if

	templateBuilder.Append("\r\n		</table>\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">\r\n			");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n			</div>\r\n			<span class=\"z\">\r\n				<button type=\"submit\" name=\"operationsubmit\" id=\"operationsubmit\"  name=\"Submit\" class=\"pn\"/><span>提交</span></button>\r\n			</span>\r\n		</div>\r\n		");
	if (tip=="add")
	{

	templateBuilder.Append("<div class=\"hintinfo\">公告添加完毕，请继续操作</div>");
	}	//end if


	if (tip=="delsuccessful")
	{

	templateBuilder.Append("<div class=\"hintinfo\">选定公告删除完毕，请继续操作</div>");
	}	//end if

	templateBuilder.Append("\r\n		</form>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<!--编辑用户-->\r\n	");
	if (operation=="edituser")
	{

	templateBuilder.Append("\r\n		<h1>编辑用户 - 搜索</h1>\r\n		<form method=\"post\" action=\"modcp.aspx?operation=edituser&op=edit&forumid=");
	templateBuilder.Append(fid.ToString());
	templateBuilder.Append("\" class=\"exfm\">\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" name=\"operation\"/>\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(op.ToString());
	templateBuilder.Append("\" name=\"op\"/>\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"编辑用户 - 搜索\">\r\n		<tbody>\r\n			<tr>\r\n				<th width=\"60\"><label for=\"username\">用户名</label></th><td><input name=\"username\" value=\"\" size=\"20\" type=\"text\" class=\"txt\" ></td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"uid\">UID</label></th><td><input name=\"uid\" value=\"\" size=\"20\" type=\"text\" class=\"txt\"> [可选]</td>\r\n			</tr>\r\n			<tr>\r\n				<th> </th><td><button type=\"submit\" class=\"pn\" name=\"editusersubmit\" id=\"editusersubmit\"><span>提交</span></button></td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		");
	if (op!="edit")
	{

	templateBuilder.Append("\r\n		<div class=\"hintinfo\">\r\n			请首先输入用户名或者 UID 搜索用户，然后进行下一步。搜索 UID 比搜索用户名速度更快且准确\r\n		</div>\r\n	");
	}
	else
	{


	if (op=="edit" &&uid>0)
	{

	templateBuilder.Append("\r\n			<h1>编辑用户</h1>\r\n			<form method=\"post\" action=\"modcp.aspx?operation=updateuser&forumid=");
	templateBuilder.Append(fid.ToString());
	templateBuilder.Append("\"  class=\"exfm\">\r\n			<input type=\"hidden\" value=");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append(" name=\"operation\"/>\r\n			<input type=\"hidden\" value=\"updateuser\" name=\"op\"/>\r\n			<input type=\"hidden\" name=\"username\" value=");
	templateBuilder.Append(uname.ToString());
	templateBuilder.Append(">\r\n			<input type=\"hidden\" name=\"uid\" value=");
	templateBuilder.Append(uid.ToString());
	templateBuilder.Append(">\r\n			<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"编辑用户 - ");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("\">\r\n			    <tr>\r\n			        <th></th>\r\n			        <td>\r\n			        ");	string avatarurl = Avatars.GetAvatarUrl(uid);
	
	templateBuilder.Append("\r\n			            <img src=\"t");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_medium.gif';\" />\r\n			            ");
	if (allowdeleteavatar)
	{

	templateBuilder.Append("\r\n			            <input type=\"checkbox\" name=\"delavatar\" value=\"1\" onclick=\"if(this.checked) this.checked=confirm('您确认要在提交后删除该用户的头像吗?');\" />删除头像\r\n			            ");
	}	//end if

	templateBuilder.Append("\r\n			        </td>\r\n			    </tr>\r\n				<tr>\r\n					<th width=\"60\"><label for=\"username\">用户名</label></th>\r\n					<td>");
	templateBuilder.Append(uname.ToString());
	templateBuilder.Append("</td>\r\n				</tr>\r\n				<tr>\r\n					<th><label for=\"username\">Uid</label></th><td>");
	templateBuilder.Append(uid.ToString());
	templateBuilder.Append("</td>\r\n				</tr>\r\n				<tr>\r\n					<th><label for=\"locationnew\">来自</label></th><td><input type=\"text\" name=\"locationnew\" value=\"");
	templateBuilder.Append(location.ToString());
	templateBuilder.Append("\" size=\"40\"></td>\r\n				</tr>\r\n				<tr>\r\n					<th><label for=\"bionew\">自我介绍</label></th><td><textarea name=\"bionew\" rows=\"4\" cols=\"80\">");
	templateBuilder.Append(bio.ToString());
	templateBuilder.Append("</textarea></td>\r\n				</tr>\r\n				<tr>\r\n					<th><label for=\"bionew\">签名</label></th><td><textarea name=\"signaturenew\" rows=\"4\" cols=\"80\">");
	templateBuilder.Append(signature.ToString());
	templateBuilder.Append("</textarea></td>\r\n				</tr>\r\n				<tr>\r\n					<th></th><td><button type=\"submit\" class=\"pn\" name=\"editsubmit\" id=\"editsubmit\"><span>提交</span></button></td>\r\n				</tr>\r\n			</table>\r\n			</form>\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n			<div class=\"hintinfo\">\r\n			该用户不存在，请重新输入 \r\n			</div>\r\n		");
	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	<!--禁止用户-->\r\n	");
	if (operation=="banusersearch")
	{

	templateBuilder.Append("\r\n		<h1>禁止用户 - 搜索</h1>\r\n		<form method=\"post\" action=\"modcp.aspx?operation=banusersearch&op=ban\"  class=\"exfm\">\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" name=\"operation\"/>\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(op.ToString());
	templateBuilder.Append("\" name=\"op\"/>\r\n			<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"禁止用户 - 搜索\">\r\n			<tbody>\r\n			<tr>\r\n				<th width=\"60\"><label for=\"username\">用户名</label></th><td><input name=\"username\" value=\"\" size=\"20\" type=\"text\" class=\"txt\"></td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"Uid\">UID</label></th><td><input name=\"uid\" value=\"\" size=\"20\" type=\"text\" class=\"txt\"> [可选]</td>\r\n			</tr>\r\n			<tr>\r\n				<th> </th><td><button type=\"submit\" class=\"pn\" name=\"Submit\" id=\"Submit\"><span>提交</span></button></td>\r\n			</tr>\r\n			</tbody>\r\n		</table>\r\n		</form>\r\n	");
	if (op!="ban")
	{

	templateBuilder.Append("\r\n		<div class=\"hintinfo\">\r\n			请首先输入用户名或者 UID 搜索用户，然后进行下一步。搜索 UID 比搜索用户名速度更快且准确\r\n		</div>\r\n	");
	}
	else
	{


	if (uid>0)
	{

	templateBuilder.Append("\r\n		<h3>禁止用户 - ");
	templateBuilder.Append(uname.ToString());
	templateBuilder.Append("</h3>\r\n		<form method=\"post\" action=\"modcp.aspx?operation=banuser\">\r\n		<input type=\"hidden\" value=");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append(" name=\"operation\"/>\r\n		<input type=\"hidden\" value=");
	templateBuilder.Append(uname.ToString());
	templateBuilder.Append(" name=\"username\" >\r\n		<input type=\"hidden\" value=");
	templateBuilder.Append(uid.ToString());
	templateBuilder.Append(" name=\"uid\"/>\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"禁止用户 - ");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("\">\r\n		<tbody>\r\n			<tr>\r\n				<th><label for=\"uname\">用户名</label></th><td>");
	templateBuilder.Append(uname.ToString());
	templateBuilder.Append("</td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"Uid\">UID</label></th><td>");
	templateBuilder.Append(uid.ToString());
	templateBuilder.Append("</td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"curstatus\">当前状态</label></th>\r\n				<td>");
	if (curstatus=="禁止发言"||curstatus=="禁止访问")
	{
	templateBuilder.Append(grouptitle.ToString());
	templateBuilder.Append(" (有效期至 ");
	templateBuilder.Append(groupexpiry.ToString());
	templateBuilder.Append(")");
	}
	else
	{

	templateBuilder.Append("正常状态");
	}	//end if

	templateBuilder.Append("</td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"bannew\">变更为</label></th><td>");
	if (grouptitle=="禁止发言")
	{

	templateBuilder.Append(" <input name=\"bannew\" value=\"4\" type=\"radio\" checked/> 禁止发言");
	}
	else
	{

	templateBuilder.Append("<input name=\"bannew\" value=\"4\" type=\"radio\"/>禁止发言");
	}	//end if


	if (grouptitle=="禁止访问")
	{

	templateBuilder.Append("<input name=\"bannew\" value=\"5\" type=\"radio\" checked/>禁止访问");
	}
	else
	{

	templateBuilder.Append("<input name=\"bannew\" value=\"5\" type=\"radio\"/>禁止访问");
	}	//end if

	templateBuilder.Append("</td>\r\n			</tr>\r\n			<tr>\r\n			<th><label for=\"banexpirynew\">期限</label></th>\r\n			<td>	\r\n			<div class=\"ftid\">\r\n				<select name=\"banexpirynew\" id=\"banexpirynew\">\r\n					<option value=\"0\">永 久</option>\r\n					<option value=\"1\">1天</option>\r\n					<option value=\"3\">3天</option>\r\n					<option value=\"5\">5天</option>\r\n					<option value=\"7\">7天</option>\r\n					<option value=\"14\">14天</option>\r\n					<option value=\"30\">1个月</option>\r\n					<option value=\"90\">3个月</option>\r\n					<option value=\"180\">半年</option>\r\n					<option value=\"365\">1年</option>\r\n				</select>\r\n			</div>\r\n			<script type=\"text/javascript\">simulateSelect('banexpirynew');</");
	templateBuilder.Append("script>\r\n			期限设置仅对禁止发言和禁止访问的操作有效\r\n			</td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"reason\">理由</label></th><td><textarea name=\"reason\" rows=\"4\" cols=\"80\" class=\"txtarea\">");
	templateBuilder.Append(reason.ToString());
	templateBuilder.Append("</textarea></td>\r\n			</tr>\r\n			<tr>\r\n				<th></th><td><button type=\"submit\" class=\"pn\" name=\"banusersubmit\" id=\"banusersubmit\"><span>提交</span></button></td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n		<div class=\"hintinfo\">\r\n		该用户不存在，请重新输入 \r\n		</div>\r\n		");
	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	<!--用户权限-->\r\n	");
	if (operation=="forumaccesslist")
	{

	templateBuilder.Append("\r\n		<h1>特殊用户</h1>\r\n		<div class=\"datalist\">\r\n		<table summary=\"\" cellpadding=\"0\" cellspacing=\"0\" class=\"datatable\">\r\n		<thead>\r\n		<tr class=\"colplural\">\r\n			<td width=\"15%\">会员</td>\r\n			<td width=\"15%\">版块</td>\r\n			<th width=\"8%\">浏览主题</th>\r\n			<th width=\"8%\">发表主题</th>\r\n			<th width=\"8%\">回复主题</th>\r\n			<th width=\"8%\">上传附件</th>\r\n			<th width=\"8%\">下载附件</th>\r\n			<td width=\"15%\">版主</td>\r\n		</tr>\r\n		</thead>\r\n		");
	if (foruminfolist.Count>0)
	{


	int filist__loop__id=0;
	foreach(ForumInfo filist in foruminfolist)
	{
		filist__loop__id++;


	int t__loop__id=0;
	foreach(string t in filist.Permuserlist.Split('|'))
	{
		t__loop__id++;

	templateBuilder.Append("\r\n			<tr>\r\n			 ");	 aspxrewriteurl = this.UserInfoAspxRewrite(t.Split(',')[1].ToString().Trim());
	
	templateBuilder.Append("\r\n			<td><a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + t.Split(',')[0].ToString().Trim() + "</a></td>\r\n			");	string aspxforumrewriteurl = this.ShowForumAspxRewrite(filist.Fid,0,filist.Rewritename);
	
	templateBuilder.Append("\r\n			<td><a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxforumrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(filist.Name.ToString().Trim());
	templateBuilder.Append("</a></td>\r\n			");	int power = Convert.ToInt32(t.Split(',')[2].ToString().Trim());
	
	string viewpowerimg = GetPowerImg(power,Discuz.Entity.ForumSpecialUserPower.ViewByUser);
	
	templateBuilder.Append("\r\n			<td><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(viewpowerimg.ToString());
	templateBuilder.Append("\" /></td>\r\n			");	string postpowerimg = GetPowerImg(power,Discuz.Entity.ForumSpecialUserPower.PostByUser);
	
	templateBuilder.Append("\r\n			<td><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(postpowerimg.ToString());
	templateBuilder.Append("\" /></td>\r\n			");	string replypowerimg = GetPowerImg(power,Discuz.Entity.ForumSpecialUserPower.ReplyByUser);
	
	templateBuilder.Append("\r\n			<td><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(replypowerimg.ToString());
	templateBuilder.Append("\" /></td>\r\n			");	string downloadpowerimg = GetPowerImg(power,Discuz.Entity.ForumSpecialUserPower.DownloadAttachByUser);
	
	templateBuilder.Append("\r\n			<td><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(downloadpowerimg.ToString());
	templateBuilder.Append("\" /></td>\r\n			");	string postattachpowerimg = GetPowerImg(power,Discuz.Entity.ForumSpecialUserPower.PostAttachByUser);
	
	templateBuilder.Append("\r\n			<td><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(postattachpowerimg.ToString());
	templateBuilder.Append("\" /></td>\r\n			<td>\r\n				<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(filist.Moderators.ToString().Trim());
	templateBuilder.Append("</a>\r\n			</td>\r\n			</tr>\r\n			");
	}	//end loop


	}	//end loop


	}
	else
	{

	templateBuilder.Append("\r\n			<tr>\r\n			<td colspan=\"9\" align=\"left\">\r\n			当前没有特殊权限用户\r\n			</td>\r\n			</tr>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n			<tbody>\r\n			<tr>\r\n				<td colspan=\"9\">\r\n					<form method=\"post\" action=\"modcp.aspx?operation=forumaccesslist\" class=\"y\">\r\n					<input name=\"formhash\" value=\"5ff4e742\" type=\"hidden\">\r\n					<div class=\"z\" style=\"margin-left:2px;\">用户名: <input name=\"suser\" value=\"\" onclick=\"this.value='';\" type=\"text\" class=\"txt\"></div>\r\n					<div class=\"ftid\" style=\"margin-left:2px;\">\r\n						<select name=\"forumid2\" id=\"forumid2\">\r\n							<option value=\"0\">全部</option>\r\n							");
	templateBuilder.Append(forumliststr.ToString());
	templateBuilder.Append("\r\n						</select>\r\n						<script type=\"text/javascript\">simulateSelect('forumid2','160');</");
	templateBuilder.Append("script>\r\n					</div>\r\n					<button type=\"submit\" id=\"Submit\" name=\"Submit\" class=\"z pn\" style=\"margin-left:2px;\"><span>搜索</span></button>\r\n					</form>\r\n				</td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</div>\r\n		<script type=\"text/javascript\">\r\n		function chkallaccess(obj) {\r\n			$('new_post').checked\r\n				= $('new_post').disabled\r\n				= $('new_reply').checked\r\n				= $('new_reply').disabled\r\n				= $('new_postattach').checked\r\n				= $('new_postattach').disabled\r\n				= $('new_getattach').checked\r\n				= $('new_getattach').disabled\r\n				= obj.checked;\r\n		}\r\n\r\n		function disallaccess(obj) {\r\n			$('new_view').checked\r\n				= $('new_post').checked\r\n				= $('new_post').checked\r\n				= $('new_reply').checked\r\n				= $('new_postattach').checked\r\n				= $('new_getattach').checked\r\n				= false;\r\n			$('customaccess').disabled\r\n				= $('new_view').disabled\r\n				= $('new_view').disabled\r\n				= $('new_post').disabled\r\n				= $('new_post').disabled\r\n				= $('new_reply').disabled\r\n				= $('new_postattach').disabled\r\n				= $('new_getattach').disabled\r\n				= obj.checked;\r\n		}\r\n\r\n		</");
	templateBuilder.Append("script>\r\n		<h1>编辑用户权限</h1>\r\n		<form method=\"post\" action=\"modcp.aspx?operation=forumaccessupdate\" class=\"exfm\">\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" name=\"operation\"/>\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"编辑用户权限\">\r\n		<tbody>\r\n			<tr>\r\n				<th width=\"60\"><label for=\"forumid\">版块选择</label></th>\r\n				<td>\r\n				<div class=\"ftid\">\r\n					<select name=\"forumid\" id=\"forumid\">\r\n						<option value=\"0\">全部</option>\r\n						");
	templateBuilder.Append(forumliststr.ToString());
	templateBuilder.Append("\r\n					</select>\r\n					<script type=\"text/javascript\">simulateSelect('forumid','160');</");
	templateBuilder.Append("script>\r\n				</div>\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"new_user\">用户名</label></th>\r\n				<td><input size=\"25\" value=\"\" id=\"new_user\" name=\"new_user\" type=\"text\" class=\"txt\"></td>\r\n			</tr>\r\n			<tr>\r\n				<th><label for=\"deleteaccess\">权限变更</label></th>\r\n				<td>\r\n					<input value=\"0\" name=\"deleteaccess\" id=\"deleteaccess\" onclick=\"disallaccess(this)\" type=\"checkbox\"> 恢复默认\r\n					<span id=\"customaccess\" style=\"border: 0px solid rgb(221, 221, 221); padding: 0px;\">\r\n					<input value=\"1\" name=\"new_view\" id=\"new_view\" onclick=\"chkallaccess(this)\" type=\"checkbox\"> 禁止查看主题 \r\n					<input value=\"2\" name=\"new_post\" id=\"new_post\" type=\"checkbox\"> 禁止发表主题\r\n					<input value=\"4\" name=\"new_reply\" id=\"new_reply\" type=\"checkbox\"> 禁止发表回复\r\n					<input value=\"8\" name=\"new_postattach\" id=\"new_postattach\" type=\"checkbox\"> 禁止上传附件\r\n					<input value=\"16\" name=\"new_getattach\" id=\"new_getattach\" type=\"checkbox\"> 禁止下载附件\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n				<th></th><td><button type=\"submit\" id=\"Submit\" name=\"Submit\" class=\"pn\"><span>提交</span></button></td>\r\n			</tr>\r\n			<tr>\r\n				<th></th><td>");
	if (tip=="access_successful")
	{

	templateBuilder.Append("用户权限更新成功, 请继续操作");
	}	//end if

	templateBuilder.Append("</td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		<div class=\"hintinfo\">\r\n			通常情况下，用户在版块的权限是根据他的用户组决定的，此处您可以限制某个用户在某版块的权限。<br>注意: 看帖是基本权限，一旦禁止, 其他权限会同时进行禁止。<br>图例说明: <img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/access_normal.gif\"> &nbsp; 默认权限&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/access_allow.gif\"> &nbsp;强制允许\r\n		</div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<!--禁止IP-->	\r\n	");
	if (operation=="showbannedlist")
	{

	templateBuilder.Append("\r\n		<form method=\"post\" action=\"modcp.aspx?operation=ipban\" class=\"datalist\">\r\n		<h1>禁止 IP</h1>\r\n		<input type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" name=\"operation\"/>\r\n		<table cellpadding=\"0\" cellspacing=\"0\" class=\"datatable\">\r\n		<thead>\r\n			<tr class=\"colplural\">\r\n				<th width=\"8%\"><input class=\"checkbox\" id=\"chkall\" name=\"chkall\" onclick=\"checkall(this.form)\" type=\"checkbox\">删除</th>\r\n				<th>IP 地址</th>\r\n				<th width=\"20%\">地理位置</th>\r\n				<th width=\"10%\">操作者</th>\r\n				<th width=\"15%\">起始时间</th>\r\n				<th width=\"15%\">结束时间</th>\r\n			</tr>\r\n		</thead>\r\n		");
	if (counts>0)
	{

	templateBuilder.Append("\r\n		<tbody>\r\n		");
	int showbannedip__loop__id=0;
	foreach(IpInfo showbannedip in showbannediplist)
	{
		showbannedip__loop__id++;

	templateBuilder.Append("\r\n		<tr>\r\n			<td>");
	if (showbannedip.Username!=username)
	{

	templateBuilder.Append("<input class=\"checkbox\" id=\"chkbanip\" name=\"chkbanip\" disabled=\"disabled\" value=");
	templateBuilder.Append(showbannedip.Id.ToString().Trim());
	templateBuilder.Append(" type=\"checkbox\">");
	}
	else
	{

	templateBuilder.Append("<input class=\"checkbox\" id=\"chkbanip\" name=\"chkbanip\" value=");
	templateBuilder.Append(showbannedip.Id.ToString().Trim());
	templateBuilder.Append(" type=\"checkbox\">");
	}	//end if

	templateBuilder.Append("</td>\r\n			<td>");
	templateBuilder.Append(showbannedip.Ip1.ToString().Trim());
	templateBuilder.Append(".");
	templateBuilder.Append(showbannedip.Ip2.ToString().Trim());
	templateBuilder.Append(".");
	templateBuilder.Append(showbannedip.Ip3.ToString().Trim());
	templateBuilder.Append(".");
	templateBuilder.Append(showbannedip.Ip4.ToString().Trim());
	templateBuilder.Append("</th>\r\n			<td>");
	templateBuilder.Append(showbannedip.Location.ToString().Trim());
	templateBuilder.Append("</td>\r\n			<td>");
	templateBuilder.Append(showbannedip.Username.ToString().Trim());
	templateBuilder.Append("</td>\r\n			<td class=\"time\">");
	templateBuilder.Append(showbannedip.Dateline.ToString().Trim());
	templateBuilder.Append("</td>\r\n			<td><input type=\"text\" name=\"expiration\" id=\"expiration\" size=\"15\" value=");
	templateBuilder.Append(showbannedip.Expiration.ToString().Trim());
	templateBuilder.Append("></input><input type=\"hidden\" name =\"hiddenexpiration\" id =\"hiddenexpiration\" value=");
	templateBuilder.Append(showbannedip.Expiration.ToString().Trim());
	templateBuilder.Append("><input type=\"hidden\" name =\"hiddenid\" id =\"hiddenid\" value=");
	templateBuilder.Append(showbannedip.Id.ToString().Trim());
	templateBuilder.Append("></td>\r\n		</tr>\r\n		");
	}	//end loop

	templateBuilder.Append("\r\n		</tbody>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		</table>\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("</div>\r\n		</div>\r\n		<hr class=\"solidline\"/>\r\n		<h1>新增要禁止IP</h1>\r\n		<div class=\"exfm\">\r\n			<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"新增要禁止IP\">\r\n			<tbody>\r\n			<tr>\r\n				<td width=\"300\">ip地址：<input name=\"ip1new\" value=\"\" size=\"1\" maxlength=\"10\" type=\"text\" class=\"txt\"> .\r\n					<input name=\"ip2new\" value=\"\" size=\"1\" maxlength=\"10\" type=\"text\" class=\"txt\"> .\r\n					<input name=\"ip3new\" value=\"\" size=\"1\" maxlength=\"10\" type=\"text\" class=\"txt\"> .\r\n					<input name=\"ip4new\" value=\"\" size=\"1\" maxlength=\"10\" type=\"text\" class=\"txt\">\r\n				</td>\r\n				<td>期限：<input name=\"validitynew\" value=\"30\" size=\"3\" type=\"text\" class=\"txt\"> 天</td>\r\n				<td><button type=\"submit\" id=\"Submit\" name=\"Submit\" class=\"pn\"><span>提交</span></button></td>\r\n			</tr>\r\n			</tbody>\r\n			</table>\r\n		</div>\r\n		</form>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	<!--审核回复-->	\r\n	");
	if (operation=="auditpost" && admingroupinfo.Allowmodpost==1)
	{

	templateBuilder.Append("\r\n		<h1>审核回复范围选择</h1>\r\n		<form id=\"form_post\" action=\"\" method=\"get\" class=\"exfm\">\r\n		<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"审核回复范围选择\">\r\n		<tbody>\r\n		<tr>\r\n			<th width=\"60\"><label for=\"forumid\">版块选择</label></th>\r\n			<td>\r\n				<div class=\"ftid\">\r\n					<select name=\"forumid\" id=\"forumid\" onchange=\"location.href='modcp.aspx?operation=auditpost&forumid='+jQuery('#forumid option:selected').val()+'&filter='+jQuery('#filter option:selected').val()\">\r\n						<option value=\"0\">全部</option>\r\n						");
	templateBuilder.Append(forumliststr.ToString());
	templateBuilder.Append("\r\n					</select>\r\n				</div>\r\n				<div class=\"ftid\">\r\n					<select name=\"tablelist\" id=\"tablelist\" onchange=\"location.href='modcp.aspx?operation=auditpost&forumid='+jQuery('#forumid option:selected').val()+'&filter='+jQuery('#filter option:selected').val()+'&tablelist='+jQuery('#tablelist option:selected').val()\" ");
	if (counts!=0)
	{

	templateBuilder.Append("style=\"display:none\"");
	}	//end if

	templateBuilder.Append(">\r\n						");
	int table__loop__id=0;
	foreach(DataRow table in posttablelist.Rows)
	{
		table__loop__id++;

	templateBuilder.Append("							\r\n						<option value=\"" + table["id"].ToString().Trim() + "\" ");
	if (tableid.ToString()==table["id"].ToString().Trim())
	{

	templateBuilder.Append("selected=\"selected\"");
	}	//end if

	templateBuilder.Append(">帖子分表" + table["id"].ToString().Trim() + "</option>\r\n						");
	}	//end loop

	templateBuilder.Append("\r\n					</select>\r\n				</div>\r\n			</td>\r\n		</tr>\r\n		<tr>\r\n			<th><label for=\"filter\">帖子范围</label></th>\r\n			<td>\r\n                <input id=\"auditpost_radio\" type=\"radio\" ");
	if (filter==1)
	{

	templateBuilder.Append("checked=\"checked\"");
	}	//end if

	templateBuilder.Append("  onclick=\"jQuery('#ignorepost_radio').attr('checked', false);location.href='modcp.aspx?operation=auditpost&forumid='+jQuery('#forumid option:selected').val()+'&filter=1&tablelist='+jQuery('#tablelist option:selected').val();\">未审核的回复</input>\r\n                <input id=\"ignorepost_radio\" type=\"radio\" ");
	if (filter==-3)
	{

	templateBuilder.Append("checked=\"checked\"");
	}	//end if

	templateBuilder.Append(" onclick=\"jQuery('#auditpost_radio').attr('checked', false);location.href='modcp.aspx?operation=auditpost&forumid='+jQuery('#forumid option:selected').val()+'&filter=-3&tablelist='+jQuery('#tablelist option:selected').val()\">已忽略的回复</input>\r\n			</td>\r\n		</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		");
	if (counts!=0)
	{

	templateBuilder.Append("\r\n		<form action=\"\" method=\"post\" name=\"auditpost_form\" id=\"auditpost_form\">\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("</div>\r\n			<input id=\"checkedall\" name=\"checkedall\" type=\"checkbox\" value=\"\" onclick=\"newCheckAll(this.form.id,this.checked)\"/>全选 \r\n			操作理由&nbsp; <input type=\"text\" style=\"margin: 0px;\" id=\"pm_top\" name=\"pm_top\" class=\"txt\"/> &nbsp; \r\n			<select onchange=\"$('pm_top').value=this.value;\" style=\"margin: 0px;\">\r\n				<option value=\"\">无</option>\r\n				<option value=\"\">--------</option>\r\n				<option value=\"广告/SPAM\">广告/SPAM</option>\r\n				<option value=\"恶意灌水\">恶意灌水</option>\r\n				<option value=\"违规内容\">违规内容</option>\r\n				<option value=\"文不对题\">文不对题</option>\r\n				<option value=\"重复发帖\">重复发帖</option>\r\n				<option></option>\r\n				<option value=\"我很赞同\">我很赞同</option>\r\n				<option value=\"精品文章\">精品文章</option>\r\n				<option value=\"原创内容\">原创内容</option>\r\n			</select>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"通过\" onclick=\"optionCheckedPosts('pass',$('pm_top').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"删除\" onclick=\"optionCheckedPosts('delete',$('pm_top').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"忽略\" onclick=\"optionCheckedPosts('ignore',$('pm_top').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"submit\" value=\"展开\" style=\"display:none\"/></span>\r\n		</div>\r\n		<input name=\"tableid\" type=\"hidden\" value=\"");
	templateBuilder.Append(tableid.ToString());
	templateBuilder.Append("\" />\r\n		<div class=\"audit_list\">\r\n		<table id=\"table_post\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">\r\n			");
	int post__loop__id=0;
	foreach(PostInfo post in postlist)
	{
		post__loop__id++;

	templateBuilder.Append("\r\n			<tbody id=\"postbody_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" ");
	if (post__loop__id==last&&inajax==1&&about=="post")
	{

	templateBuilder.Append("name=\"lastpost\"");
	}	//end if

	templateBuilder.Append(" onmousemove=\"chageclass('postover',");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append(")\" onmouseout=\"chageclass('postout',");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append(")\">\r\n				<tr>\r\n				<td>\r\n					<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />	\r\n					<input name=\"pidlist\" type=\"hidden\" value=\"");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" />\r\n					<input name=\"tidlist\" type=\"hidden\" value=\"");
	templateBuilder.Append(post.Tid.ToString().Trim());
	templateBuilder.Append("\" />\r\n					<input name=\"fidlist\" type=\"hidden\" value=\"");
	templateBuilder.Append(post.Fid.ToString().Trim());
	templateBuilder.Append("\" />\r\n					<dl>\r\n						<dt>\r\n							<cite id=\"postcite_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"y\" style=\"display:none\">\r\n							操作理由&nbsp; <input type=\"text\" style=\"margin: 0px;\" id=\"pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" name=\"pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"txt\"> &nbsp; \r\n			                <select onchange=\"$('pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("').value=this.value;\" style=\"margin: 0px;\">\r\n			                <option value=\"\">无</option>\r\n			                <option value=\"\">--------</option>\r\n			                <option value=\"广告/SPAM\">广告/SPAM</option>\r\n			                <option value=\"恶意灌水\">恶意灌水</option>\r\n			                <option value=\"违规内容\">违规内容</option>\r\n			                <option value=\"文不对题\">文不对题</option>\r\n			                <option value=\"重复发帖\">重复发帖</option>\r\n			                <option></option>\r\n			                <option value=\"我很赞同\">我很赞同</option>\r\n			                <option value=\"精品文章\">精品文章</option>\r\n			                <option value=\"原创内容\">原创内容</option>\r\n			                </select>\r\n							<a href=\"###\" id=\"passpost_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" name=\"mod_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"_auditpost('passpost','");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("|");
	templateBuilder.Append(post.Tid.ToString().Trim());
	templateBuilder.Append("',$('pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("').value)\">通过</a>\r\n							<a href=\"###\" id=\"mod_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" name=\"mod_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"_auditpost('ignorepost','");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("|");
	templateBuilder.Append(post.Tid.ToString().Trim());
	templateBuilder.Append("',$('pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("').value)\">忽略</a>\r\n							<a href=\"###\"   id=\"mod_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" name=\"mod_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"_auditpost('deletepost','");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("|");
	templateBuilder.Append(post.Tid.ToString().Trim());
	templateBuilder.Append("',$('pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("').value)\">删除</a>\r\n							</cite>\r\n							<input id=\"auditpostid\" name=\"auditpostid\" type=\"checkbox\" value=\"");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("|");
	templateBuilder.Append(post.Tid.ToString().Trim());
	templateBuilder.Append("\" />\r\n							");	 aspxrewriteurl = this.ShowTopicAspxRewrite(post.Tid,0);
	
	string aspxuserrewriteurl = this.UserInfoAspxRewrite(post.Posterid);
	
	templateBuilder.Append("\r\n							");
	templateBuilder.Append(forumnav.ToString());
	templateBuilder.Append(" » ");
	templateBuilder.Append(post.Title.ToString().Trim());
	templateBuilder.Append("\r\n						</dt>\r\n						<dd>\r\n							<a id=\"post");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" href=\"");
	templateBuilder.Append(aspxuserrewriteurl.ToString());
	templateBuilder.Append("\" title=\"");
	templateBuilder.Append(post.Forumname.ToString().Trim());
	templateBuilder.Append("\" class=\"lightlink\" onmouseout=\"showMenu(this.id);\" onmousemove=\"showMenu(this.id);\">");
	templateBuilder.Append(post.Poster.ToString().Trim());
	templateBuilder.Append("</a> 发表于 <a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" title=\"");
	templateBuilder.Append(post.Forumname.ToString().Trim());
	templateBuilder.Append("\"  class=\"audit_time\">");
	templateBuilder.Append(post.Postdatetime.ToString().Trim());
	templateBuilder.Append("</a>\r\n						</dd>\r\n						<dd>\r\n							<div style=\"overflow-y: auto; overflow-x: hidden; height: 120px; width: 100%;\">");
	templateBuilder.Append(post.Message.ToString().Trim());
	templateBuilder.Append("</div>\r\n						</dd>\r\n					</dl>					\r\n					<div id=\"auditreason_menu\" class=\"popupmenu_popup\" style=\"display:none;\">\r\n						<textarea type=\"textarea\" size=\"80\" name=\"pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" id=\"pm_");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("\" class=\"txtarea\"></textarea>\r\n					</div>\r\n                    <ul class=\"p_pop\" id=\"post");
	templateBuilder.Append(post.Pid.ToString().Trim());
	templateBuilder.Append("_menu\" style=\"display:none\">\r\n                    <li><a href=\"useradmin.aspx?action=banuser&uid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"showWindow('mods', this.href);doane(event);\" class=\"forbid_user\">禁言用户</a></li>\r\n                    <li><a href=\"###\" onclick=\"if(confirm('您确定要删除吗？')) _auditpost('deletepostsbyuidanddays',");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append(")\">删除7天内的帖子</a></li>\r\n                    ");	 aspxrewriteurl = this.UserInfoAspxRewrite(post.Posterid);
	
	templateBuilder.Append("\r\n					<li><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"public_info\">查看公共资料</a></li>\r\n                    <li><a href=\"search.aspx?posterid=");
	templateBuilder.Append(post.Posterid.ToString().Trim());
	templateBuilder.Append("&searchsubmit=1\" class=\"all_topic\" target=\"_blank\">查看所有帖子</a></li>\r\n                </ul>\r\n				</td>\r\n				</tr>\r\n			</tbody>\r\n			");
	}	//end loop

	templateBuilder.Append("\r\n			</table>\r\n		</div>\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("</div>\r\n			<input id=\"lastcheckedall\" name=\"lastcheckedall\" type=\"checkbox\" value=\"\" onclick=\"newCheckAll(this.form.id,this.checked)\"/>全选 \r\n			操作理由&nbsp; <input type=\"text\" style=\"margin: 0px;\" id=\"pm_bottom\" name=\"pm_bottom\" class=\"txt\"> &nbsp; \r\n			<select onchange=\"$('pm_bottom').value=this.value;\" style=\"margin: 0px;\">\r\n			<option value=\"\">无</option>\r\n			<option value=\"\">--------</option>\r\n			<option value=\"广告/SPAM\">广告/SPAM</option>\r\n			<option value=\"恶意灌水\">恶意灌水</option>\r\n			<option value=\"违规内容\">违规内容</option>\r\n			<option value=\"文不对题\">文不对题</option>\r\n			<option value=\"重复发帖\">重复发帖</option>\r\n			<option></option>\r\n			<option value=\"我很赞同\">我很赞同</option>\r\n			<option value=\"精品文章\">精品文章</option>\r\n			<option value=\"原创内容\">原创内容</option>\r\n			</select>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"通过\" onclick=\"optionCheckedPosts('pass',$('pm_bottom').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"删除\" onclick=\"optionCheckedPosts('delete',$('pm_bottom').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"忽略\" onclick=\"optionCheckedPosts('ignore',$('pm_bottom').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"submit\" value=\"展开\" style=\"display:none\"/></span>\r\n		</div>\r\n		</form>		\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n			<div class=\"hintinfo\">对不起，没有找到需要审核的回复。</div>\r\n            <input id=\"postcount\" type=\"hidden\" value=\"0\" />\r\n		");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n        \r\n<!--审核主题-->\r\n		<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />\r\n        <input id=\"count\" type=\"hidden\" value=\"");
	templateBuilder.Append(counts.ToString());
	templateBuilder.Append("\" />\r\n        <input id=\"fid\" type=\"hidden\" value=\"");
	templateBuilder.Append(fid.ToString());
	templateBuilder.Append("\" />\r\n        <input id=\"pagecount\" type=\"hidden\" value=\"");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("\" />\r\n        <input id=\"pageid\" type=\"hidden\" value=\"");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" />\r\n        <table id=\"temp\" style=\"display:none\"></table>\r\n");
	if (operation=="audittopic" && admingroupinfo.Allowmodpost==1)
	{

	templateBuilder.Append("\r\n		<h1>审核主题范围选择</h1>\r\n		<form id=\"forum_topic\" action=\"\" method=\"get\" class=\"exfm\">\r\n		<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"审核主题范围选择\">\r\n		<tbody>\r\n		<tr>\r\n			<th width=\"60\"><label for=\"forumid\">版块选择</label></th>\r\n			<td>\r\n				<div class=\"ftid\">\r\n					<select name=\"forumid\" id=\"forumid\" onchange=\"location.href='modcp.aspx?operation=audittopic&forumid='+jQuery('#forumid option:selected').val()+'&filter='+jQuery('#filter option:selected').val()\">\r\n						<option value=\"0\">全部</option>\r\n						");
	templateBuilder.Append(forumliststr.ToString());
	templateBuilder.Append("\r\n					</select>\r\n				</div>\r\n				<div class=\"ftid\">\r\n					<select name=\"tablelist\" id=\"tablelist\" onchange=\"location.href='modcp.aspx?operation=audittopic&forumid='+jQuery('#forumid option:selected').val()+'&filter='+jQuery('#filter option:selected').val()+'&tablelist='+jQuery('#tablelist option:selected').val()\" ");
	if (counts!=0)
	{

	templateBuilder.Append("style=\"display:none\"");
	}	//end if

	templateBuilder.Append(">\r\n						");
	int table__loop__id=0;
	foreach(DataRow table in posttablelist.Rows)
	{
		table__loop__id++;

	templateBuilder.Append("							\r\n						<option value=\"" + table["id"].ToString().Trim() + "\" ");
	if (tableid.ToString()==table["id"].ToString().Trim())
	{

	templateBuilder.Append("selected=\"selected\"");
	}	//end if

	templateBuilder.Append(">帖子分表" + table["id"].ToString().Trim() + "</option>\r\n						");
	}	//end loop

	templateBuilder.Append("\r\n					</select>\r\n				</div>\r\n			</td>\r\n		</tr>\r\n		<tr>\r\n			<th><label for=\"filter\">帖子范围</label></th>\r\n			<td>\r\n			<div>\r\n                <input id=\"audittopic_radio\" type=\"radio\" ");
	if (filter==-2)
	{

	templateBuilder.Append("checked=\"checked\"");
	}	//end if

	templateBuilder.Append(" onclick=\"jQuery('#ignoretopic_radio').attr('checked', false);location.href='modcp.aspx?operation=audittopic&forumid='+jQuery('#forumid option:selected').val()+'&filter=-2&tablelist='+jQuery('#tablelist option:selected').val()\">未审核的主题</input>\r\n                <input id=\"ignoretopic_radio\" type=\"radio\" ");
	if (filter==-3)
	{

	templateBuilder.Append("checked=\"checked\"");
	}	//end if

	templateBuilder.Append(" onclick=\"jQuery('#audittopic_radio').attr('checked', false);location.href='modcp.aspx?operation=audittopic&forumid='+jQuery('#forumid option:selected').val()+'&filter=-3&tablelist='+jQuery('#tablelist option:selected').val()\">已忽略的主题</input>\r\n			</div>\r\n			</td>\r\n		</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		");
	if (counts!=0)
	{

	templateBuilder.Append("\r\n		<form action=\"#\" method=\"post\" id=\"audittopic_form\" name=\"audittopic_form\" id=\"audittopic_form\">\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("</div>\r\n			<input id=\"chkall\" name=\"chkall\" type=\"checkbox\" value=\"\" onclick=\"newCheckAll(this.form.id,this.checked)\"/>全选 \r\n			操作理由&nbsp; <input type=\"text\" style=\"margin: 0px;\" id=\"pm_top\" name=\"pm_top\" class=\"txt\"> &nbsp; \r\n			<select onchange=\"$('pm_top').value=this.value;\" style=\"margin: 0px;\">\r\n			<option value=\"\">无</option>\r\n			<option value=\"\">--------</option>\r\n			<option value=\"广告/SPAM\">广告/SPAM</option>\r\n			<option value=\"恶意灌水\">恶意灌水</option>\r\n			<option value=\"违规内容\">违规内容</option>\r\n			<option value=\"文不对题\">文不对题</option>\r\n			<option value=\"重复发帖\">重复发帖</option>\r\n			<option></option>\r\n			<option value=\"我很赞同\">我很赞同</option>\r\n			<option value=\"精品文章\">精品文章</option>\r\n			<option value=\"原创内容\">原创内容</option>\r\n			</select>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"通过\" onclick=\"optionCheckedTopics('pass',$('pm_top').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"删除\" onclick=\"optionCheckedTopics('delete',$('pm_top').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"忽略\" onclick=\"optionCheckedTopics('ignore',$('pm_top').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"submit\" value=\"展开\" style=\"display:none\"/></span>\r\n		</div>\r\n\r\n		<div class=\"audit_list\">\r\n			<table id=\"table_topic\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">\r\n				");
	int topic__loop__id=0;
	foreach(TopicInfo topic in topiclist)
	{
		topic__loop__id++;

	templateBuilder.Append("\r\n				<tbody id=\"tbody_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" ");
	if (topic__loop__id==last&&inajax==1&&about=="topic")
	{

	templateBuilder.Append("name=\"lasttopic\"");
	}	//end if

	templateBuilder.Append(" onmousemove=\"chageclass('topicover',");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(")\" onmouseout=\"chageclass('topicout',");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(")\">\r\n				<tr>\r\n				<td>\r\n					<dl>\r\n						<dt>\r\n							<cite id=\"cite_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" class=\"y\" style=\"display:none\">\r\n							操作理由&nbsp; <input type=\"text\" style=\"margin: 0px;\" id=\"pm_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" name=\"pm_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" class=\"txt\"> &nbsp; \r\n							<select onchange=\"$('pm_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("').value=this.value;\" style=\"margin: 0px;\">\r\n							<option value=\"\">无</option>\r\n							<option value=\"\">--------</option>\r\n							<option value=\"广告/SPAM\">广告/SPAM</option>\r\n							<option value=\"恶意灌水\">恶意灌水</option>\r\n							<option value=\"违规内容\">违规内容</option>\r\n							<option value=\"文不对题\">文不对题</option>\r\n							<option value=\"重复发帖\">重复发帖</option>\r\n							<option></option>\r\n							<option value=\"我很赞同\">我很赞同</option>\r\n							<option value=\"精品文章\">精品文章</option>\r\n							<option value=\"原创内容\">原创内容</option>\r\n							</select>\r\n							<a href=\"###\" id=\"pass_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" name=\"pass_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"_auditpost('passtopic',");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",$('pm_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("').value)\">通过</a>\r\n							<a href=\"###\"  id=\"ignore_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" name=\"mod_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"_auditpost('ignoretopic',");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",$('pm_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("').value)\">忽略</a>\r\n							<a href=\"###\"  name=\"delete_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" id=\"mod_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"_auditpost('deletetopic',");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",$('pm_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("').value)\">删除</a>\r\n							</cite>\r\n							<input id=\"audittopicid\" name=\"audittopicid\" type=\"checkbox\" value=\"");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" />\r\n							<input name=\"topicidlist\" type=\"hidden\" value=\"");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" />\r\n							<input name=\"fidlist\" type=\"hidden\" value=\"");
	templateBuilder.Append(topic.Fid.ToString().Trim());
	templateBuilder.Append("\" />\r\n							");	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	
	templateBuilder.Append("\r\n							<a href=\"javascript:void(0);\" onclick=\"getpostinfo(");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(")\" title=\"");
	templateBuilder.Append(topic.Forumname.ToString().Trim());
	templateBuilder.Append("\" class=\"f_bold\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n						</dt>\r\n						<dd>\r\n							");	 aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);
	
	templateBuilder.Append("\r\n							<a id=\"topic");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" class=\"lightlink\" onmouseout=\"showMenu(this.id);\" onmousemove=\"showMenu(this.id);\">");
	templateBuilder.Append(topic.Poster.ToString().Trim());
	templateBuilder.Append("</a><span class=\"audit_time\">");
	templateBuilder.Append(topic.Postdatetime.ToString().Trim());
	templateBuilder.Append("</span>\r\n						</dd>\r\n						<dd id=\"msgtbody_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" style=\"display:none\">\r\n							<div id=\"msg_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" style=\"overflow-y:auto;overflow-x:hidden;height:120px;\"></div>\r\n						</dd>\r\n					</dl>\r\n					<div id=\"auditreason_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" class=\"popupmenu_popup\" style=\"display:none;\">\r\n						<textarea type=\"textarea\" id=\"reason_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" size=\"80\" id=\"pm_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" class=\"txtarea\"></textarea>\r\n                        <input type=\"button\" onclick=\"_auditpost('ignore',");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(")\" />\r\n					</div>\r\n                    <ul class=\"p_pop audit_menu\" id=\"topic");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("_menu\" style=\"display:none;\">\r\n                        <li><a href=\"useradmin.aspx?action=banuser&uid=");
	templateBuilder.Append(topic.Posterid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"showWindow('mods', this.href);doane(event);\" class=\"forbid_user\">禁言用户</a></li>\r\n                        <li><a href=\"###\" onclick=\"if(confirm('您确定要删除吗？')) _auditpost('deletepostsbyuidanddays',");
	templateBuilder.Append(topic.Posterid.ToString().Trim());
	templateBuilder.Append(");\">删除7天内的帖子</a></li>\r\n                        ");	 aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);
	
	templateBuilder.Append("\r\n						<li><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"public_info\">查看公共资料</a></li>\r\n                        <li><a href=\"search.aspx?posterid=");
	templateBuilder.Append(topic.Posterid.ToString().Trim());
	templateBuilder.Append("&searchsubmit=1\" class=\"all_topic\" target=\"_blank\">查看所有帖子</a></li>\r\n                 </ul>\r\n				</td>\r\n				</tr>\r\n				</tbody>\r\n				");
	}	//end loop

	templateBuilder.Append("					\r\n			</table>\r\n		</div>\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("</div>\r\n			<input id=\"lastchkall\" name=\"lastchkall\" type=\"checkbox\" value=\"\" onclick=\"newCheckAll(this.form.id,this.checked)\"/>全选 \r\n			操作理由&nbsp; <input type=\"text\" style=\"margin: 0px;\" id=\"pm_bottom\" name=\"pm_bottom\" class=\"txt\"> &nbsp; \r\n			<select onchange=\"$('pm_bottom').value=this.value;\" style=\"margin: 0px;\">\r\n			<option value=\"\">无</option>\r\n			<option value=\"\">--------</option>\r\n			<option value=\"广告/SPAM\">广告/SPAM</option>\r\n			<option value=\"恶意灌水\">恶意灌水</option>\r\n			<option value=\"违规内容\">违规内容</option>\r\n			<option value=\"文不对题\">文不对题</option>\r\n			<option value=\"重复发帖\">重复发帖</option>\r\n			<option></option>\r\n			<option value=\"我很赞同\">我很赞同</option>\r\n			<option value=\"精品文章\">精品文章</option>\r\n			<option value=\"原创内容\">原创内容</option>\r\n			</select>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"通过\" onclick=\"optionCheckedTopics('pass',$('pm_bottom').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"删除\" onclick=\"optionCheckedTopics('delete',$('pm_bottom').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"button\" value=\"忽略\" onclick=\"optionCheckedTopics('ignore',$('pm_bottom').value)\" /></span>\r\n			<span class=\"postbtn\"><input type=\"submit\" value=\"展开\" style=\"display:none\"/></span>\r\n		</div>\r\n		</form>\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n		<div class=\"hintinfo\">对不起，没有找到需要审核的主题。</div>\r\n		");
	}	//end if


	}	//end if


	if (operation=="editforum")
	{

	templateBuilder.Append("\r\n		<h1>版块编辑</h1>\r\n		<div class=\"exfm\">\r\n		<form method=\"get\" action=\"\">\r\n			<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />\r\n			<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"论坛版块选择\">\r\n			<tbody>\r\n			<tr>\r\n				<th width=\"60\">版块</th>\r\n				<td>\r\n					<div class=\"ftid\">\r\n					<select name=\"forumid\" id=\"forumid\" onchange=\"location.href='modcp.aspx?operation=editforum&forumid='+jQuery('#forumid option:selected').val()\">\r\n						<option value=\"0\" disabled=\"true\">全部</option>\r\n						");
	templateBuilder.Append(forumliststr.ToString());
	templateBuilder.Append("\r\n					</select>\r\n					</div>\r\n				</td>\r\n			</tr>\r\n			</tbody>\r\n			</table>\r\n		</form>\r\n		");
	if (forumid!=0)
	{

	templateBuilder.Append("\r\n		<form method=\"post\" action=\"?operation=updateforum\">\r\n		<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(foruminfo.Fid.ToString().Trim());
	templateBuilder.Append("\" />\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"版块编辑\">\r\n		<tbody>\r\n			<tr>\r\n                <th width=\"60\">版块介绍</th>\r\n				<td width=\"\"><div id=\"descpreview\" style=\"display:none\" class=\"rulespreview\"></div>\r\n                    ");	string coloroptions = "Black,Sienna,DarkOliveGreen,DarkGreen,DarkSlateBlue,Navy,Indigo,DarkSlateGray,DarkRed,DarkOrange,Olive,Green,Teal,Blue,SlateGray,DimGray,Red,SandyBrown,YellowGreen,SeaGreen,MediumTurquoise,RoyalBlue,Purple,Gray,Magenta,Orange,Yellow,Lime,Cyan,DeepSkyBlue,DarkOrchid,Silver,Pink,Wheat,LemonChiffon,PaleGreen,PaleTurquoise,LightBlue,Plum,White";
	
	string seditorid = "description";
	
	char comma = ',';
	

	if (alloweditrules)
	{

	templateBuilder.Append("\r\n                    <link href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/seditor.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n                    <div class=\"editor_tb\" style=\"width:70%\">\r\n                        <span class=\"y\">\r\n                            <a id=\"viewdescription\" href=\"###\" onclick=\"preview('descpreview','descriptionmessage')\">预览</a>		\r\n	                    </span>\r\n                        <div>\r\n		                    <a href=\"javascript:;\" title=\"粗体\" class=\"tb_bold\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[b]', '[/b]')\">B</a>\r\n		                    <a href=\"javascript:;\" title=\"颜色\" class=\"tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor\" onclick=\"showMenu(this.id, true, 0, 2)\">Color</a>\r\n		                    <div class=\"popupmenu_popup tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor_menu\" style=\"display: none\">\r\n			                    ");
	int colornamedes__loop__id=0;
	foreach(string colornamedes in coloroptions.Split(comma))
	{
		colornamedes__loop__id++;

	templateBuilder.Append("\r\n				                    <input type=\"button\" style=\"background-color: ");
	templateBuilder.Append(colornamedes.ToString());
	templateBuilder.Append("\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[color=");
	templateBuilder.Append(colornamedes.ToString());
	templateBuilder.Append("]', '[/color]')\" />");
	if (colornamedes__loop__id%8==0)
	{

	templateBuilder.Append("<br />");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n		                    </div>\r\n		                    <a href=\"javascript:;\" title=\"图片\" class=\"tb_img\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("img\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'img')\">Image</a>\r\n		                    <a href=\"javascript:;\" title=\"链接\" class=\"tb_link\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("url\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'url')\">Link</a>\r\n                            </div>\r\n                    </div>\r\n					<textarea id=\"descriptionmessage\" name=\"descriptionmessage\" rows=\"8\"class=\"txtarea\" style=\"width:70%;padding:0\" >");
	templateBuilder.Append(foruminfo.Description.ToString().Trim());
	templateBuilder.Append("</textarea>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					<textarea id=\"descriptionmessage\" name=\"descriptionmessage\" rows=\"8\" cols=\"80\" disabled readonly class=\"txtarea\" style=\"width:70%;padding:0\">\r\n					");
	templateBuilder.Append(foruminfo.Description.ToString().Trim());
	templateBuilder.Append("</textarea>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n                <th valign=\"top\">版块规则</th>\r\n				<td>\r\n					<div id=\"rulespreview\" style=\"display:none\" class=\"rulespreview\"></div>\r\n				");
	if (alloweditrules)
	{

	 seditorid = "rules";
	
	templateBuilder.Append("\r\n                    <link href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/seditor.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n					<div class=\"editor_tb\" style=\"width: 70%;\"> \r\n						<span class=\"y\">\r\n							<a id=\"viewrule\" href=\"###\" onclick=\"preview('rulespreview','rulesmessage')\">预览</a>		\r\n						</span>\r\n						<div>\r\n							<a href=\"javascript:;\" title=\"粗体\" class=\"tb_bold\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[b]', '[/b]')\">B</a>\r\n							<a href=\"javascript:;\" title=\"颜色\" class=\"tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor\" onclick=\"showMenu(this.id, true, 0, 2)\">Color</a>\r\n							<div class=\"popupmenu_popup tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor_menu\" style=\"display: none\">\r\n								");
	int colornamerules__loop__id=0;
	foreach(string colornamerules in coloroptions.Split(comma))
	{
		colornamerules__loop__id++;

	templateBuilder.Append("\r\n									<input type=\"button\" style=\"background-color: ");
	templateBuilder.Append(colornamerules.ToString());
	templateBuilder.Append("\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[color=");
	templateBuilder.Append(colornamerules.ToString());
	templateBuilder.Append("]', '[/color]')\" />");
	if (colornamerules__loop__id%8==0)
	{

	templateBuilder.Append("<br />");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n							</div>\r\n							<a href=\"javascript:;\" title=\"图片\" class=\"tb_img\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("img\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'img')\">Image</a>\r\n							<a href=\"javascript:;\" title=\"链接\" class=\"tb_link\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("url\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'url')\">Link</a>\r\n							</div>\r\n					</div>\r\n					<textarea id=\"rulesmessage\" name=\"rulesmessage\" rows=\"8\" cols=\"80\" class=\"txtarea\" style=\"width:70%;padding:0\">\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					<textarea id=\"rulesmessage\" name=\"rulesmessage\" rows=\"8\" cols=\"80\" disabled readonly class=\"txtarea\" style=\"width:70%;padding:0\">\r\n					");
	}	//end if
	templateBuilder.Append(foruminfo.Rules.ToString().Trim());
	templateBuilder.Append("</textarea>\r\n				</td>\r\n			</tr>\r\n			<tr>\r\n				<th></th><td><button type=\"submit\" id=\"Submit1\" class=\"pn\"><span>提交</span></button></td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		</div>\r\n		");
	}	//end if


	}	//end if


	if (operation=="logs" && admingroupinfo.Allowviewlog==1)
	{

	templateBuilder.Append("\r\n		<h1>管理日志搜索</h1>\r\n		<form method=\"get\" action=\"\" class=\"exfm\">\r\n		<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\" summary=\"版块编辑\">\r\n		<tbody>\r\n			<tr>\r\n			<th>关键字</label>\r\n			<td>\r\n				<input name=\"keyword\" value=\"\" size=\"60\" type=\"text\" class=\"txt\">每页显示条数\r\n				<input name=\"lpp\" value=\"20\" size=\"4\" type=\"text\" class=\"txt\">\r\n				<button type=\"submit\" id=\"searchsubmit\" value=\"true\">提交</button>\r\n			</td>\r\n			</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		<h1>管理日志列表</h1>\r\n		<div class=\"datalist\">\r\n		    <table summary=\"\" cellpadding=\"0\" cellspacing=\"0\" class=\"datatable\">\r\n		        <thead>\r\n		            <tr class=\"colplural\">\r\n                        <td>操作人</td>\r\n                        <td>用户组</td>\r\n                        <td>IP</td>\r\n                        <td>操作类型</td>\r\n                        <td>操作版块</td>\r\n                        <td width=\"15%\">操作主题</td>\r\n                        <td>操作原因</td>\r\n                        <td>操作时间</td>\r\n		            </tr>\r\n		        </thead>\r\n		        <tbody>\r\n			        ");
	int logs__loop__id=0;
	foreach(DataRow logs in moderatorLogs.Rows)
	{
		logs__loop__id++;

	templateBuilder.Append("\r\n			        <tr>\r\n				        ");	 aspxrewriteurl = this.UserInfoAspxRewrite(logs["moderatoruid"].ToString().Trim());
	
	templateBuilder.Append("\r\n				        <td><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\"><b>" + logs["moderatorname"].ToString().Trim() + "</b></a></td>\r\n				        <td>" + logs["grouptitle"].ToString().Trim() + "</td>\r\n				        <td>" + logs["ip"].ToString().Trim() + "</td>\r\n				        <td>" + logs["actions"].ToString().Trim() + "</td>\r\n				        ");	 aspxrewriteurl = this.ShowForumAspxRewrite(logs["fid"].ToString().Trim(),0);
	
	templateBuilder.Append("\r\n				        <td><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + logs["fname"].ToString().Trim() + "</a></td>\r\n                        ");	 aspxrewriteurl = this.ShowTopicAspxRewrite(logs["tid"].ToString().Trim(),0);
	
	templateBuilder.Append("\r\n                        <td><div style=\"overflow:hidden; height:20px;\"><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + logs["title"].ToString().Trim() + "</a></div></td>\r\n				        <td>\r\n                            <a href=\"###\" ");
	if (logs["reason"].ToString().Trim()!="")
	{

	templateBuilder.Append("onmouseover=\"showMenu(this.id)\"");
	}	//end if

	templateBuilder.Append(" id=\"logreason" + logs__loop__id.ToString() + "\">\r\n                                ");
	if (logs["reason"].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n                                    查看\r\n                                ");
	}
	else
	{

	templateBuilder.Append("\r\n                                    无操作原因\r\n                                ");
	}	//end if

	templateBuilder.Append("\r\n                            </a>\r\n                            <div id=\"logreason" + logs__loop__id.ToString() + "_menu\" class=\"popupmenu_popup cl\" style=\" width:150px;display: none;\">\r\n				                " + logs["reason"].ToString().Trim() + "\r\n		                    </div>\r\n                        </td>\r\n                        <td class=\"time\">" + logs["postdatetime"].ToString().Trim() + "</td>\r\n			        </tr>\r\n			        ");
	}	//end loop

	templateBuilder.Append("\r\n		        </tbody>\r\n		    </table>\r\n		</div>\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">\r\n			");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n			</div>\r\n		</div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n		<!--主题关注-->\r\n		");
	if (operation=="attention")
	{

	templateBuilder.Append("\r\n		<h1>主题关注</h1>\r\n		<form action=\"\" method=\"get\" class=\"exfm\">\r\n		<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />\r\n		<table cellspacing=\"0\" cellpadding=\"0\" class=\"formtable\">\r\n		<tbody>\r\n		<tr>\r\n			<th width=\"60\"><label for=\"keyword\">关键字</label></th><td><input name=\"keyword\" value=\"\" size=\"45\" type=\"text\" class=\"txt\"></td>\r\n		</tr>\r\n		<tr>\r\n			<th><label for=\"forumid\">版块选择</label></th>\r\n			<td>\r\n				<div class=\"ftid\">\r\n					<select name=\"forumid\" id=\"forumid\">\r\n						<option value=\"0\">全部</option>\r\n						");
	templateBuilder.Append(forumliststr.ToString());
	templateBuilder.Append("\r\n					</select>\r\n					<script type=\"text/javascript\">simulateSelect('forumid','160');</");
	templateBuilder.Append("script>\r\n				</div>\r\n			</td>\r\n		</tr>\r\n		<tr>\r\n			<th></th><td><button type=\"submit\" class=\"pn\"><span>提交</span></button></td>\r\n		</tr>\r\n		</tbody>\r\n		</table>\r\n		</form>\r\n		");
	if (counts>0)
	{

	templateBuilder.Append("\r\n		<form action=\"\" method=\"post\">\r\n		<h1>主题关注列表</h1>\r\n		<input name=\"forumid\" type=\"hidden\" value=\"");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" />\r\n		<input name=\"operation\" type=\"hidden\" value=\"");
	templateBuilder.Append(operation.ToString());
	templateBuilder.Append("\" />\r\n		<div class=\"datalist\">\r\n		<table summary=\"\" cellpadding=\"0\" cellspacing=\"0\" class=\"datatable\">\r\n		<thead>\r\n		<tr class=\"colplural\">\r\n			<th style=\"text-align:left;\"><input type=\"checkbox\" name=\"chkall\" onclick=\"checkall(this.form, 'topicid')\"/>标题</th>\r\n			<th width=\"10%\">作者</th>\r\n			<th width=\"20%\">发布时间</th>\r\n			<td width=\"20%\">最后发表</td>\r\n		</tr>\r\n		</thead>\r\n		<tbody>\r\n		");
	int topic__loop__id=0;
	foreach(TopicInfo topic in topiclist)
	{
		topic__loop__id++;

	templateBuilder.Append("\r\n			<tr>\r\n				<td class=\"datatitle\">\r\n				");	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	
	templateBuilder.Append("\r\n				<input type=\"checkbox\" id=\"topicid\" name=\"topicid\" value=\"");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\"/><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a></td>\r\n				");	 aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);
	
	templateBuilder.Append("\r\n				<td><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(topic.Poster.ToString().Trim());
	templateBuilder.Append("</a></td>\r\n				<td class=\"time\">");
	templateBuilder.Append(topic.Postdatetime.ToString().Trim());
	templateBuilder.Append("</td>\r\n				<td class=\"time\">");
	templateBuilder.Append(topic.Lastpost.ToString().Trim());
	templateBuilder.Append("</td>\r\n			</tr>\r\n			");
	}	//end loop

	templateBuilder.Append("\r\n		</tbody>\r\n		</table>\r\n		</div>\r\n		<div class=\"pages_btns\">\r\n			<div class=\"pages\">\r\n				");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n			</div>\r\n			<div class=\"ftid\">\r\n				<select name=\"disattentiontype\" id=\"disattentiontype\">\r\n					<option value=\"0\" selected=\"selected\">所选主题</option>\r\n					<option value=\"7\">一星期前</option>\r\n					<option value=\"30\">一月前</option>\r\n					<option value=\"365\">一年前</option>\r\n				</select>\r\n			</div>\r\n			<script type=\"text/javascript\">simulateSelect('disattentiontype');</");
	templateBuilder.Append("script>\r\n			<button type=\"submit\" value=\"取消关注\" class=\"pn\"/><span>取消关注</span></button>\r\n		</div>\r\n		</form>	\r\n		");
	}
	else
	{

	templateBuilder.Append("\r\n		<div class=\"hintinfo\">对不起，没有找到需要关注的主题。</div>\r\n		");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n	</div>\r\n</div>\r\n");
	}	//end if


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
