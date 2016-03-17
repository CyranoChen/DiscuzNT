<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.website" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/6/2 16:12:41.
		本页面代码由Discuz!NT模板引擎生成于 2011/6/2 16:12:41. 
	*/

	base.OnInit(e);

	templateBuilder.Capacity = 220000;

	/*
	聚合首面方法说明
	
	///////////////////////////////////////////////////////////////////////////////////////////////
	
	方法名称: GetForumTopicList(count, views, forumid, timetype, ordertype, isdigest, onlyimg)
	方法说明: 返回指定条件的主题列表信息
	参数说明:
	          count : 返回的主题数 
	          views : 浏览量 [返回等于或大于当前浏览量的主题]
	          forumid : 版块ID [默认值 0 为所有版块]
	          timetype : 指定时间段内的主题 [ TopicTimeType.Day(一天内)  , TopicTimeType.Week(一周内),   TopicTimeType.Month(一个月内),   TopicTimeType.SixMonth(六个月内),  TopicTimeType.Year(一年内),  TopicTimeType.All(默认 从1754-1-1至今的所有主题)
	          ordertype : 排序字段(降序) [TopicOrderType.ID(默认 主题ID) , TopicOrderType.Views(浏览量),   TopicOrderType.LastPost(最后回复),    TopicOrderType.PostDateTime(按最新主题查),    TopicOrderType.Digest(按精华主题查),    TopicOrderType.Replies(按回复数)]  
	          isdigest : 是否精化 [true(仅返回精华主题)   false(不加限制)]
	          onlyimg : 是否包含附件 [true(仅返回包括图片附件的主题)   false(不加限制)]
	      
	//////////////////////////////////////////////////////////////////////////////////////////////    
	
	方法名称: GetHotForumList(count)   
	方法说明: 返回指定数量的热门版块列表
	参数说明:
	          count : 返回的版块数
	    
	//////////////////////////////////////////////////////////////////////////////////////////////      
	
	方法名称: GetForumList(forumid)   
	方法说明: 返回指定版块下的所有子段块列表
	参数说明:
	          forumid : 指定的版块id
	      
	//////////////////////////////////////////////////////////////////////////////////////////////  
	
	方法名称: GetLastPostList(forumid, count)   
	方法说明: 返回指定版块下的最新回帖列表
	参数说明:
	          forumid : 指定的版块id     
	          count : 返回的回帖数
	 
	//////////////////////////////////////////////////////////////////////////////////////////////  
	
	方法名称: GetAlbumList(photoconfig.Focusalbumshowtype, count, days)   
	方法说明: 返回指定条件的相册列表
	参数说明:
	          photoconfig.Focusalbumshowtype : 排序字段(降序) [1(浏览量), 2(照片数), 3(创建时间)]    注:管理后台聚合设置项
	          count : 返回的相册数
	          days :有效天数 [指定天数内的相册]
	      
	//////////////////////////////////////////////////////////////////////////////////////////////  
	
	方法名称: GetWeekHotPhotoList(photoconfig.Weekhot)
	方法说明: 返回指定数量的热门图片
	参数说明:
	          photoconfig.Weekhot : 返回的热图数量  注:管理后台聚合设置项
	          
	//////////////////////////////////////////////////////////////////////////////////////////////  
	
	方法名称: GetSpaceTopComments(count)
	方法说明: 返回指定数量的空间最新评论
	参数说明:
	          count : 返回的评论数
	          
	//////////////////////////////////////////////////////////////////////////////////////////////  
	
	方法名称: GetRecentUpdateSpaceList(count)
	方法说明: 返回指定数量的最新更新空间列表
	参数说明:
	          count : 返回的空间信息数
	
	
	//////////////////////////////////////////////////////////////////////////////////////////////  
	
	方法名称: GetGoodsList(condition, orderby, categoryid, count)
	方法说明: 返回指定数量的最新更新空间列表
	参数说明:
	          condition : 条件 [recommend(仅返回推荐商品, 商城模式下可用) , quality_new(仅返回全新(状态)商品),    quality_old(仅返回二手(状态)商品)]  
	          orderby: 排序字段(降序) [viewcount(按浏览量排序),    hotgoods(按商品交易量排序),  newgoods(按发布商品先后顺序排序) ]
	          categoryid : 商品所属分类id [默认值 0 为不加限制]
	          count : 返回的商品数
	          
	 
	//////////////////////////////////////////////////////////////////////////////////////////////  
	
	方法名称: GetUserList(count, orderby)
	方法说明: 返回指定数量及排序方式的用户列表
	参数说明:
	          count : 返回的用户数       
	          orderby: 排序字段(降序) [credits(用户积分), posts(用户发帖数), lastactivity(最后活动时间), joindate(注册时间), oltime(在线时间)]
	*/
	
	templateBuilder.Append(" <!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n");
	templateBuilder.Append(meta.ToString());
	templateBuilder.Append("\r\n<title>");
	templateBuilder.Append(pagetitle.ToString());
	templateBuilder.Append(" ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - ");
	templateBuilder.Append(config.Webtitle.ToString().Trim());
	templateBuilder.Append(" - Powered by Discuz!NT</title>\r\n<meta name=\"generator\" content=\"Discuz!NT 3.6.601\" />\r\n<meta name=\"author\" content=\"Discuz!NT Team and Comsenz UI Team\" />\r\n<meta name=\"copyright\" content=\"2001-2010 Comsenz Inc.\" />\r\n<meta http-equiv=\"x-ua-compatible\" content=\"ie=7\" />\r\n<link rel=\"icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n<link rel=\"shortcut icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n<link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/float.css\" type=\"text/css\" media=\"all\"  />\r\n<link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/website.css\" type=\"text/css\" media=\"all\"  />\r\n");
	templateBuilder.Append(link.ToString());
	templateBuilder.Append("\r\n<script type=\"text/javascript\">\r\n    var forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\n</");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_report.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_utils.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_website.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/common.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append(script.ToString());
	templateBuilder.Append("\r\n<script type=\"text/javascript\">\r\n	var aspxrewrite = ");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append(";\r\n	var IMGDIR = '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("';\r\n    var disallowfloat = '");
	templateBuilder.Append(config.Disallowfloatwin.ToString().Trim());
	templateBuilder.Append("';\r\n	var rooturl=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("\";\r\n</");
	templateBuilder.Append("script>\r\n</head>\r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("\r\n<body onkeydown=\"if(event.keyCode==27) return false;\">\r\n<div id=\"append_parent\"></div><div id=\"ajaxwaitid\"></div>\r\n");
	if (headerad!="")
	{

	templateBuilder.Append("\r\n	<div id=\"ad_headerbanner\">");
	templateBuilder.Append(headerad.ToString());
	templateBuilder.Append("</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<div id=\"hd\">\r\n	<div class=\"wrap\">\r\n		<div class=\"head cl\">\r\n			<h2><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\" title=\"Discuz!NT|BBS|论坛\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/logo.png\" alt=\"Discuz!NT|BBS|论坛\"/></a></h2>\r\n			");
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

	templateBuilder.Append("\r\n			    <div id=\"floatlayout_login\" class=\"pbm\">\r\n					<select style=\"width:156px;margin-bottom:8px;\" id=\"question\" name=\"question\" selecti=\"5\" name=\"question\" onchange=\"displayAnswer();\" tabindex=\"904\">\r\n						<option id=\"question\" value=\"0\" selected=\"selected\">安全提问(未设置请忽略)</option>\r\n						<option id=\"question\" value=\"1\">母亲的名字</option>\r\n						<option id=\"question\" value=\"2\">爷爷的名字</option>\r\n						<option id=\"question\" value=\"3\">父亲出生的城市</option>\r\n						<option id=\"question\" value=\"4\">您其中一位老师的名字</option>\r\n						<option id=\"question\" value=\"5\">您个人计算机的型号</option>\r\n						<option id=\"question\" value=\"6\">您最喜欢的餐馆名称</option>\r\n						<option id=\"question\" value=\"7\">驾驶执照的最后四位数字</option>\r\n					</select>\r\n					<input type=\"text\" tabindex=\"905\" class=\"txt\" size=\"20\" autocomplete=\"off\" style=\"width:140px;display:none;\"  id=\"answer\" name=\"answer\"/>\r\n		        </div>\r\n                ");
	}	//end if

	templateBuilder.Append("\r\n                <script type=\"text/javascript\">\r\n                    function closeIsMore() {\r\n                        $('ls_more').style.display = 'none';\r\n                    }\r\n                    function displayAnswer() {\r\n                        $('answer').style.display = '';\r\n						$('answer').focus();\r\n                    }\r\n                </");
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


	if (config.Isframeshow!=0)
	{

	templateBuilder.Append("\r\n		<script type=\"text/javascript\">\r\n		if(top == self) {\r\n			document.write('<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("frame.aspx?f=1\" target=\"_top\" class=\"frameswitch\">分栏模式<\\/a>');\r\n		}\r\n		</");
	templateBuilder.Append("script>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n		<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("forumindex.aspx\" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a>\r\n	</div>\r\n</div>\r\n<div class=\"wrap cl contentbox\">\r\n");
	if (page_err==0)
	{

	templateBuilder.Append("\r\n<div id=\"websiteheaderad\" class=\"ad cl\"></div>\r\n<div id=\"forum_global\" class=\"forum_global cl\">\r\n	<div class=\"forum_main\">\r\n		<div class=\"forumtopic\">\r\n            ");
	int toppost__loop__id=0;
	foreach(PostInfo toppost in postlist)
	{
		toppost__loop__id++;


	if (toppost__loop__id==3)
	{

	templateBuilder.Append("\r\n                </div>\r\n		        <div class=\"topic_list i_topic cl\">\r\n		            <ul>		\r\n                ");
	}	//end if


	if (toppost__loop__id==11)
	{

	templateBuilder.Append("\r\n			        </ul><ul>\r\n                ");
	}	//end if


	if (toppost__loop__id<=2)
	{

	templateBuilder.Append("		\r\n				<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(ShowTopicAspxRewrite(toppost.Tid,0).ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">\r\n				<h2><a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(ShowTopicAspxRewrite(toppost.Tid,0).ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(toppost.Title.ToString().Trim());
	templateBuilder.Append("</a></h2>\r\n				<p>");
	templateBuilder.Append(toppost.Message.ToString().Trim());
	templateBuilder.Append("</p>\r\n				");
	}
	else
	{

	templateBuilder.Append("\r\n                <li>\r\n                    <strong>\r\n                        ");	 aspxrewriteurl = this.ShowForumAspxRewrite(toppost.Fid,0,toppost.ForumRewriteName);
	
	templateBuilder.Append("\r\n                        [<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(toppost.Forumname.ToString().Trim());
	templateBuilder.Append("</a>]\r\n                    </strong>\r\n                    ");	 aspxrewriteurl = this.ShowTopicAspxRewrite(toppost.Tid,0);
	
	templateBuilder.Append("\r\n                    <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" title=\"");
	templateBuilder.Append(toppost.Title.ToString().Trim());
	templateBuilder.Append("\">");	templateBuilder.Append(Utils.GetUnicodeSubString(toppost.Title,43,"..."));
	templateBuilder.Append("</a>\r\n               </li>            \r\n				");
	}	//end if


	if (toppost__loop__id==18)
	{

	break;


	}	//end if


	}	//end loop


	if (postlist.Length>=11||postlist.Length>2&&postlist.Length<11)
	{

	templateBuilder.Append("\r\n				</ul>\r\n		    ");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n		");
	if (postlist.Length<3)
	{

	templateBuilder.Append("\r\n		<div class=\"topic_list i_topic cl\">\r\n		<ul>		\r\n		");	 topiclist = forumagg.GetForumTopicList(16, 0, 0, TopicTimeType.Year, topicordertype, false, false);
	

	int toptopicinfo__loop__id=0;
	foreach(DataRow toptopicinfo in topiclist.Rows)
	{
		toptopicinfo__loop__id++;


	if (toptopicinfo__loop__id==9)
	{

	templateBuilder.Append("	\r\n			</ul><ul>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n            <li>\r\n                <strong>\r\n                    ");	 aspxrewriteurl = this.ShowForumAspxRewrite(Utils.StrToInt(toptopicinfo["fid"].ToString().Trim(), 0),0,toptopicinfo["rewritename"].ToString().Trim());
	
	templateBuilder.Append("\r\n                    [<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + toptopicinfo["name"].ToString().Trim() + "</a>]\r\n                </strong>\r\n                ");	 aspxrewriteurl = this.ShowTopicAspxRewrite(toptopicinfo["tid"].ToString().Trim(),0);
	
	templateBuilder.Append("\r\n                <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" title=\"" + toptopicinfo["title"].ToString().Trim() + "\">");	templateBuilder.Append(Utils.GetUnicodeSubString(toptopicinfo["title"].ToString().Trim(),43,"..."));
	templateBuilder.Append("</a>\r\n           </li>            \r\n        ");
	}	//end loop

	templateBuilder.Append("\r\n		</ul>\r\n		</div>\r\n		");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n	<div class=\"forum_exta\">\r\n		<div class=\"flash cl\">\r\n			");
	if (rotatepicdata!=null && rotatepicdata!="")
	{

	templateBuilder.Append("			\r\n			<div class=\"slideflash cl\">\r\n				<script type='text/javascript'>\r\n				var imgwidth = 312;\r\n				var imgheight = 200;\r\n				</");
	templateBuilder.Append("script>			\r\n				<!--图片轮换代码开始-->\r\n				<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_rotatepic.js\"></");
	templateBuilder.Append("script>\r\n				<script type=\"text/javascript\">\r\n				var data = { };\r\n				\r\n				");
	templateBuilder.Append(rotatepicdata.ToString());
	templateBuilder.Append("\r\n				\r\n				var ri = new MzRotateImage();\r\n				ri.dataSource = data;\r\n				ri.width = 312;\r\n				ri.height = 200;\r\n				ri.interval = 3000;\r\n				ri.duration = 2000;\r\n				document.write(ri.render());				\r\n				</");
	templateBuilder.Append("script>\r\n				<!--图片轮换代码结束-->\r\n			</div>\r\n        ");
	}	//end if

	templateBuilder.Append("	    \r\n		</div>\r\n		<div class=\"hot_topic cl\">\r\n			<h2>论坛热帖</h2>\r\n			<ul>\r\n				");	int hottopicscount = Discuz.Aggregation.TopicAggregationData.GetForumAggerationHotTopics().Rows.Count;
	

	if (hottopicscount==0)
	{

	templateBuilder.Append("\r\n				<script language=\"JavaScript\" type=\"text/javascript\" src=\"tools/showtopics.aspx?aggregation=1&count=9&time=2&order=1\"></");
	templateBuilder.Append("script>\r\n				");
	}
	else
	{

	DataTable hottopicslist = Discuz.Aggregation.TopicAggregationData.GetForumAggerationHotTopics();
	

	int newhottopicinfo__loop__id=0;
	foreach(DataRow newhottopicinfo in hottopicslist.Rows)
	{
		newhottopicinfo__loop__id++;

	templateBuilder.Append("\r\n				<li>\r\n				<span>\r\n					");	 aspxrewriteurl = this.ShowForumAspxRewrite(Utils.StrToInt(newhottopicinfo["fid"].ToString().Trim(), 0),0,newhottopicinfo["forumnamerewritename"].ToString().Trim());
	
	templateBuilder.Append("\r\n					<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">[" + newhottopicinfo["forumname"].ToString().Trim() + "]</a>\r\n				</span>\r\n				");	 aspxrewriteurl = this.ShowTopicAspxRewrite(newhottopicinfo["tid"].ToString().Trim(),0);
	
	templateBuilder.Append("\r\n				<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" title=\"" + newhottopicinfo["title"].ToString().Trim() + "\">" + newhottopicinfo["title"].ToString().Trim() + "</a>\r\n				</li>\r\n				");
	}	//end loop


	}	//end if

	templateBuilder.Append("\r\n			</ul>			\r\n			<div class=\"ad_pic\" style=\"padding-left:4px;\"><div id=\"websitehottopicad\"></div></div>\r\n		</div>\r\n	</div>\r\n	<div class=\"forum_slide\">\r\n		<div class=\"hot_forum\">\r\n			<div class=\"titlebar\">\r\n			<ul>\r\n				<li id=\"li_hotforum\" class=\"current\"><a onclick=\"javascript:tabselect($('li_hotforum'));\" href=\"javascript:;\">热门版块</a></li>\r\n				<li id=\"li_bbsmessage\"><a onclick=\"javascript:tabselect($('li_bbsmessage'));\" href=\"javascript:;\">论坛信息</a></li>\r\n			</ul>\r\n			</div>\r\n			<div class=\"content\">\r\n				<ul id=\"hotforum\">\r\n				");	string orderby = "posts";
	

	int __foruminfo__loop__id=0;
	foreach(DataRow __foruminfo in forumagg.GetHotForumList(10,orderby,0).Rows)
	{
		__foruminfo__loop__id++;

	 aspxrewriteurl = ShowForumAspxRewrite(Utils.StrToInt(__foruminfo["fid"].ToString().Trim(), 0),0, __foruminfo["rewritename"].ToString().Trim());
	
	templateBuilder.Append("				\r\n					<li><em>" + __foruminfo["posts"].ToString().Trim() + "帖</em><cite ");
	if (__foruminfo__loop__id==1)
	{

	templateBuilder.Append("class=\"first\"");
	}	//end if


	if (__foruminfo__loop__id==2)
	{

	templateBuilder.Append("class=\"second\"");
	}	//end if


	if (__foruminfo__loop__id==3)
	{

	templateBuilder.Append("class=\"third\"");
	}	//end if

	templateBuilder.Append(" > " + __foruminfo__loop__id.ToString() + "</cite><a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + __foruminfo["name"].ToString().Trim() + "</a></li>\r\n				");
	}	//end loop

	templateBuilder.Append("\r\n				</ul>\r\n				<ul id=\"bbsmessage\" style=\"display:none;\">\r\n					<li>会员总数: <i>");
	templateBuilder.Append(totalusers.ToString());
	templateBuilder.Append("</i>人</li>\r\n					<li>最新注册会员:<i>");	 aspxrewriteurl = this.UserInfoAspxRewrite(lastuserid);
	
	templateBuilder.Append("<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(lastusername.ToString());
	templateBuilder.Append("</a></i></li>\r\n					<li>主题数:<i>");
	templateBuilder.Append(totaltopic.ToString());
	templateBuilder.Append("</i>主题</li>\r\n					<li>帖子数:<i>");
	templateBuilder.Append(totalpost.ToString());
	templateBuilder.Append("</i> 个(含回帖) </li>\r\n					<li>今  日:<i>");
	templateBuilder.Append(todayposts.ToString());
	templateBuilder.Append("</i>帖  昨 日: <i>");
	templateBuilder.Append(yesterdayposts.ToString());
	templateBuilder.Append("</i> 帖</li>\r\n					");
	if (highestpostsdate!="")
	{

	templateBuilder.Append("\r\n						<li>	\r\n							最高日:<i>");
	templateBuilder.Append(highestposts.ToString());
	templateBuilder.Append("</i>帖\r\n						</li>\r\n						<li>	\r\n							最高发帖日:<i>");
	templateBuilder.Append(highestpostsdate.ToString());
	templateBuilder.Append("</i>\r\n						</li>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<li>在线总数:<i>");
	templateBuilder.Append(totalonline.ToString());
	templateBuilder.Append("</i>人</li>\r\n					<li>最高在线:<i>");
	templateBuilder.Append(highestonlineusercount.ToString());
	templateBuilder.Append("</i> 人 </li>\r\n					<li>发生于:<i>");
	templateBuilder.Append(highestonlineusertime.ToString());
	templateBuilder.Append("</i></li>\r\n				</ul>\r\n			</div>\r\n		</div>		\r\n        <div id=\"websiteuserposttopad\" class=\"ad_side\"></div>\r\n		<div class=\"forum_info\">\r\n			<div class=\"titlebar\">\r\n			<strong>发帖排行</strong>\r\n			<ul class=\"posttime\">\r\n			    <li id=\"li_postcount_day\" class=\"current\"><a onclick=\"javascript:tabselect($('li_postcount_day'));\" href=\"javascript:;\">日</a></li>\r\n				<li id=\"li_postcount_week\"><a onclick=\"javascript:tabselect($('li_postcount_week'));\" href=\"javascript:;\">周</a></li>\r\n				<li id=\"li_postcount_month\"><a onclick=\"javascript:tabselect($('li_postcount_month'));\" href=\"javascript:;\">月</a></li>\r\n			</ul>\r\n			</div>\r\n			<div class=\"content\">\r\n				<div id=\"postcount_month\" class=\"postcount\" style=\"display:none;\">\r\n					");	 userPostCountInfoList = forumagg.GetUserPostCountList(4, DateType.Month, 1);
	

	int userMonthPostCountInfo__loop__id=0;
	foreach(UserPostCountInfo userMonthPostCountInfo in userPostCountInfoList)
	{
		userMonthPostCountInfo__loop__id++;

	templateBuilder.Append("\r\n					<dl>\r\n						");	string avatarurl = Avatars.GetAvatarUrl(userMonthPostCountInfo.Uid, AvatarSize.Small);
	
	templateBuilder.Append("\r\n						<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" \" style=\"border:1px solid #E8E8E8;padding:1px;\" />\r\n						<dt>");	 aspxrewriteurl = this.UserInfoAspxRewrite(userMonthPostCountInfo.Uid);
	
	templateBuilder.Append("\r\n					      <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(userMonthPostCountInfo.Username,20,"..."));
	templateBuilder.Append("</a></dt>\r\n						<dd>发帖<em>");
	templateBuilder.Append(userMonthPostCountInfo.PostCount.ToString().Trim());
	templateBuilder.Append("</em>篇</dd>\r\n					</dl>\r\n					");
	}	//end loop

	templateBuilder.Append(" \r\n				</div>\r\n				<div id=\"postcount_week\" class=\"postcount\" style=\"display:none;\">\r\n					");	 userPostCountInfoList = forumagg.GetUserPostCountList(4, DateType.Week, 1);
	

	int userWeekPostCountInfo__loop__id=0;
	foreach(UserPostCountInfo userWeekPostCountInfo in userPostCountInfoList)
	{
		userWeekPostCountInfo__loop__id++;

	templateBuilder.Append("\r\n					<dl>\r\n						");	string avatarurl = Avatars.GetAvatarUrl(userWeekPostCountInfo.Uid, AvatarSize.Small);
	
	templateBuilder.Append("\r\n						<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" \" style=\"border:1px solid #E8E8E8;padding:1px;\" />\r\n						<dt>");	 aspxrewriteurl = this.UserInfoAspxRewrite(userWeekPostCountInfo.Uid);
	
	templateBuilder.Append("\r\n						<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(userWeekPostCountInfo.Username,20,"..."));
	templateBuilder.Append("</a></dt>\r\n						<dd>发帖<em>");
	templateBuilder.Append(userWeekPostCountInfo.PostCount.ToString().Trim());
	templateBuilder.Append("</em>篇</dd>\r\n					</dl>\r\n					");
	}	//end loop

	templateBuilder.Append("\r\n				</div>\r\n				<div id=\"postcount_day\" class=\"postcount\">\r\n					");	 userPostCountInfoList = forumagg.GetUserPostCountList(4, DateType.Day, 1);
	

	int userDayPostCountInfo__loop__id=0;
	foreach(UserPostCountInfo userDayPostCountInfo in userPostCountInfoList)
	{
		userDayPostCountInfo__loop__id++;

	templateBuilder.Append("\r\n					<dl>\r\n						");	string avatarurl = Avatars.GetAvatarUrl(userDayPostCountInfo.Uid, AvatarSize.Small);
	
	templateBuilder.Append("\r\n						<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" \" style=\"border:1px solid #E8E8E8;padding:1px;\" />\r\n						<dt>");	 aspxrewriteurl = this.UserInfoAspxRewrite(userDayPostCountInfo.Uid);
	
	templateBuilder.Append("\r\n						<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(userDayPostCountInfo.Username,20,"..."));
	templateBuilder.Append("</a></dt>\r\n						<dd>发帖<em>");
	templateBuilder.Append(userDayPostCountInfo.PostCount.ToString().Trim());
	templateBuilder.Append("</em>篇</dd>\r\n					</dl> \r\n					");
	}	//end loop

	templateBuilder.Append("\r\n				</div>							\r\n			</div>\r\n		</div>\r\n	</div>\r\n</div>\r\n<div id=\"websiterecforumtopad\" class=\"ad cl\"></div>\r\n<script type=\"text/javascript\">\r\nvar reco_topic = ");
	templateBuilder.Append(forumagg.GetTopicJsonFromFile().ToString().Trim());
	templateBuilder.Append(";\r\nvar templatepath = \"");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("\";\r\nvar aspxrewrite = ");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append(";\r\n</");
	templateBuilder.Append("script>\r\n");
	int forumid__loop__id=0;
	foreach(int forumid in Discuz.Aggregation.AggregationFacade.ForumAggregation.GetRecommendForumID())
	{
		forumid__loop__id++;

	ForumInfo foruminfo = Forums.GetForumInfo(forumid);
	

	if (foruminfo!=null)
	{

	int tcount = Forums.GetForumList(forumid).Rows.Count!=0?12:8;
	
	templateBuilder.Append("\r\n<div class=\"forum_list cl forum ");
	if (forumid__loop__id%2==0)
	{

	templateBuilder.Append("other");
	}	//end if

	templateBuilder.Append("\">\r\n	<h3>\r\n		<cite>\r\n		");	 aspxrewriteurl = this.ShowForumAspxRewrite(forumid,0,foruminfo.Rewritename);
	
	templateBuilder.Append("\r\n		<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" tabindex=\"_blank\">更多内容</a>\r\n		</cite>\r\n		<strong><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" tabindex=\"_blank\">");
	templateBuilder.Append(foruminfo.Name.ToString().Trim());
	templateBuilder.Append("</a></strong>\r\n		");
	int sub_forum__loop__id=0;
	foreach(DataRow sub_forum in Forums.GetForumList(forumid).Rows)
	{
		sub_forum__loop__id++;

	 aspxrewriteurl = this.ShowForumAspxRewrite(Utils.StrToInt(sub_forum["fid"].ToString().Trim(), 0),0,sub_forum["rewritename"].ToString().Trim());
	
	templateBuilder.Append("\r\n			<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" tabindex=\"_blank\">" + sub_forum["name"].ToString().Trim() + "</a><span class=\"pipe\">|</span>\r\n			");
	if (sub_forum__loop__id>4)
	{

	break;


	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n	</h3>\r\n	<div class=\"list_main\">\r\n		<div class=\"forum_topic cl\">\r\n			<div class=\"showpic\">\r\n				<script type=\"text/javascript\">document.write(showtopicinfo(");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(", " + forumid__loop__id.ToString() + "-1));</");
	templateBuilder.Append("script>\r\n			</div>\r\n			 ");	int listcount = Discuz.Aggregation.TopicAggregationData.GetForumAggregationTopic(forumid).Rows.Count;
	
	 topiclist = listcount==0?forumagg.GetForumTopicList(8, 0, forumid, TopicTimeType.All, TopicOrderType.PostDateTime, false, false):Discuz.Aggregation.TopicAggregationData.GetForumAggregationTopic(forumid);
	
	int listsize = listcount==0?8 :4;
	

	int newtopicinfo__loop__id=0;
	foreach(DataRow newtopicinfo in topiclist.Rows)
	{
		newtopicinfo__loop__id++;


	if (newtopicinfo__loop__id<=listsize)
	{


	if (listcount!=0 && newtopicinfo__loop__id==1)
	{

	 aspxrewriteurl = this.ShowTopicAspxRewrite(newtopicinfo["topicid"].ToString().Trim(),0);
	
	templateBuilder.Append("\r\n						<div class=\"topic\">\r\n							<h2><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + newtopicinfo["title"].ToString().Trim() + "</a></h2>\r\n							<p><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + newtopicinfo["shortdescription"].ToString().Trim() + "</a></p>\r\n						</div>\r\n						");	continue;


	}	//end if


	if (listcount!=0)
	{


	if (newtopicinfo__loop__id==2)
	{

	templateBuilder.Append("\r\n						<div class=\"topic_list forumthread\">\r\n						<ul id=\"topiclist_ul_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">						\r\n						");
	}	//end if


	}
	else
	{


	if (newtopicinfo__loop__id==1)
	{

	templateBuilder.Append("\r\n						<div class=\"topic_list forumthread\">\r\n						<ul>\r\n						");
	}	//end if


	}	//end if

	templateBuilder.Append("										\r\n						<li>\r\n							<strong>\r\n								");
	if (listcount==0)
	{

	 aspxrewriteurl = this.ShowForumAspxRewrite(Utils.StrToInt(newtopicinfo["fid"].ToString().Trim(), 0),0);
	
	templateBuilder.Append("\r\n								<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">[" + newtopicinfo["name"].ToString().Trim() + "]</a>\r\n								");
	}
	else
	{

	 aspxrewriteurl = ShowForumAspxRewrite(Utils.StrToInt(newtopicinfo["fid"].ToString().Trim(), 0),0);
	
	string name = Forums.GetForumInfo(Utils.StrToInt(newtopicinfo["fid"].ToString().Trim(), 0)).Name;
	
	templateBuilder.Append("\r\n								<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">[");
	templateBuilder.Append(name.ToString());
	templateBuilder.Append("]</a>\r\n								");
	}	//end if

	templateBuilder.Append("\r\n							</strong>\r\n							");
	if (listcount==0)
	{

	 aspxrewriteurl = this.ShowTopicAspxRewrite(newtopicinfo["tid"].ToString().Trim(),0);
	

	}
	else
	{

	 aspxrewriteurl = this.ShowTopicAspxRewrite(newtopicinfo["topicid"].ToString().Trim(),0);
	

	}	//end if

	templateBuilder.Append("\r\n							<a title=\"" + newtopicinfo["title"].ToString().Trim() + "\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(newtopicinfo["title"].ToString().Trim(),43,"..."));
	templateBuilder.Append("</a>\r\n						</li> \r\n					");
	if (newtopicinfo__loop__id==listsize)
	{

	templateBuilder.Append("\r\n					</ul>\r\n					</div> \r\n					");
	}	//end if


	}	//end if


	}	//end loop


	if (listcount!=0 && listcount<4)
	{


	if (listcount==1)
	{

	templateBuilder.Append("	\r\n			<div class=\"topic_list forumthread\">\r\n			<ul id=\"topiclist_ul_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\">\r\n			 ");
	}	//end if


	if (listcount<8)
	{

	templateBuilder.Append("\r\n			</ul>\r\n				</div>\r\n			<script>\r\n			function addhtmltoul()\r\n			{\r\n			var lihtml=\"\";\r\n			");	 topiclist = forumagg.GetForumTopicList(4-listcount, 0, forumid, TopicTimeType.All, TopicOrderType.PostDateTime, false, false);
	

	int newtopicinfos__loop__id=0;
	foreach(DataRow newtopicinfos in topiclist.Rows)
	{
		newtopicinfos__loop__id++;


	if (newtopicinfos__loop__id<=4-listcount)
	{

	templateBuilder.Append("\r\n					lihtml+='<li>';\r\n						lihtml+='<strong>';\r\n						");	 aspxrewriteurl = this.ShowForumAspxRewrite(Utils.StrToInt(newtopicinfos["fid"].ToString().Trim(), 0),0);
	
	templateBuilder.Append("\r\n						lihtml+='<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">[" + newtopicinfos["name"].ToString().Trim() + "]</a>';\r\n						lihtml+='</strong>';\r\n					");	 aspxrewriteurl = this.ShowTopicAspxRewrite(newtopicinfos["tid"].ToString().Trim(),0);
	
	templateBuilder.Append("\r\n					lihtml+=' <a title=\"" + newtopicinfos["title"].ToString().Trim() + "\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + newtopicinfos["title"].ToString().Trim() + "</a>';	\r\n					lihtml+='</li>';\r\n				");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n			$('topiclist_ul_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("').innerHTML+=(lihtml);\r\n			}\r\n			addhtmltoul();\r\n			</");
	templateBuilder.Append("script>\r\n			");
	}	//end if


	}	//end if


	if (Forums.GetForumList(forumid).Rows.Count!=0)
	{

	templateBuilder.Append("\r\n			<div class=\"topic_box\">		\r\n			");
	int subforum__loop__id=0;
	foreach(DataRow subforum in Forums.GetForumList(forumid).Rows)
	{
		subforum__loop__id++;

	templateBuilder.Append("\r\n				<dl>	\r\n				");	 aspxrewriteurl = this.ShowForumAspxRewrite(Utils.StrToInt(subforum["fid"].ToString().Trim(), 0),0,subforum["rewritename"].ToString().Trim());
	
	templateBuilder.Append("\r\n					<dt><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" tabindex=\"_blank\">" + subforum["name"].ToString().Trim() + "</a></dt>		\r\n					<dd>" + subforum["description"].ToString().Trim() + "</dd>\r\n				</dl>\r\n				");
	if (subforum__loop__id>2)
	{

	break;


	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n			</div>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n	</div>\r\n	<div class=\"list_side\">\r\n		<div class=\"forum_side cl\">\r\n			<div class=\"titlebar\">\r\n				<ul>\r\n					<li class=\"current\" id=\"li_forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_topic\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_topic'),");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(");\">最热主题</a></li>\r\n					<li id=\"li_forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_reply\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_reply'), ");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(");\">最新回复</a></li>\r\n					<li id=\"li_forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_digest\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_digest'), ");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(");\">精华</a></li>\r\n				</ul>\r\n			</div>\r\n			<div class=\"content cl\" ");
	if (tcount==8)
	{

	templateBuilder.Append("style=\"height:180px\"");
	}	//end if

	templateBuilder.Append(">				\r\n				<ul id=\"forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_topic\" class=\"forum_hot_topic\">\r\n				");	 topiclist = forumagg.GetForumTopicList(tcount, 0, forumid, TopicTimeType.All, TopicOrderType.Replies, false, false);
	

	if (topiclist.Rows.Count>0)
	{


	int hottopicinfo__loop__id=0;
	foreach(DataRow hottopicinfo in topiclist.Rows)
	{
		hottopicinfo__loop__id++;

	 aspxrewriteurl = this.ShowTopicAspxRewrite(hottopicinfo["tid"].ToString().Trim(),0);
	
	templateBuilder.Append("\r\n					<li><em>" + hottopicinfo["replies"].ToString().Trim() + "</em><a title=\"" + hottopicinfo["title"].ToString().Trim() + "\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(hottopicinfo["title"].ToString().Trim(),28,"..."));
	templateBuilder.Append("</a></li> \r\n				");
	}	//end loop


	}
	else
	{

	templateBuilder.Append("\r\n				暂无数据!\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</ul>\r\n				<ul id=\"forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_reply\" class=\"forum_hot_topic\" style=\"display:none;\">\r\n				");	 topiclist = forumagg.GetForumTopicList(tcount, 0, forumid, TopicTimeType.All, TopicOrderType.LastPost, false, false);
	

	if (topiclist.Rows.Count>0)
	{


	int replytopic__loop__id=0;
	foreach(DataRow replytopic in topiclist.Rows)
	{
		replytopic__loop__id++;

	templateBuilder.Append(" \r\n					<li><a title=\"" + replytopic["title"].ToString().Trim() + "\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=" + replytopic["tid"].ToString().Trim() + "&page=end#lastpost\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(replytopic["title"].ToString().Trim(),28,"..."));
	templateBuilder.Append("</a></li> \r\n				");
	}	//end loop


	}
	else
	{

	templateBuilder.Append("\r\n				暂无数据!\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</ul>\r\n				<ul id=\"forum_");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("_digest\" class=\"forum_hot_topic\" style=\"display:none;\">\r\n				");	 topiclist = forumagg.GetForumTopicList(tcount, 0, forumid, TopicTimeType.All, TopicOrderType.LastPost, true, false);
	

	if (topiclist.Rows.Count>0)
	{


	int replytopic__loop__id=0;
	foreach(DataRow replytopic in topiclist.Rows)
	{
		replytopic__loop__id++;

	templateBuilder.Append(" \r\n					<li><a title=\"" + replytopic["title"].ToString().Trim() + "\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=" + replytopic["tid"].ToString().Trim() + "&page=end#lastpost\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(replytopic["title"].ToString().Trim(),28,"..."));
	templateBuilder.Append("</a></li> \r\n				");
	}	//end loop


	}
	else
	{

	templateBuilder.Append("\r\n				暂无数据!\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</ul>\r\n			</div>\r\n		</div>\r\n	</div>\r\n</div>\r\n");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n<div id=\"websiterecforumbottomad\" class=\"ad cl\"></div>\r\n");
	if (Discuz.Plugin.Space.SpacePluginProvider.GetInstance()!=null)
	{


	if (config.Enablespace==1)
	{

	templateBuilder.Append("\r\n<div class=\"forum_list cl space\">\r\n	<h3><cite><a href=\"spaceindex.aspx\" target=\"_blank\">空间首页</a></cite><strong><a href=\"spaceindex.aspx\" target=\"_blank\">个人空间</a></strong></h3>\r\n	<div class=\"list_main\">\r\n		<div class=\"forum_topic cl\">\r\n			<div class=\"showpic\" style=\"position:relative;\">\r\n				<script type='text/javascript'>\r\n					var imgwidth = 300;\r\n					var imgheight = 170;\r\n				</");
	templateBuilder.Append("script>\r\n				<script type='text/javascript'>\r\n					var data = {};\r\n					");
	templateBuilder.Append(spacerotatepicdata.ToString());
	templateBuilder.Append("\r\n					var ri = new MzRotateImage();\r\n					ri.dataSource = data;\r\n					ri.width = 300;\r\n					ri.height = 170;\r\n					ri.interval = 3000;\r\n					ri.duration = 2000;\r\n					document.write(ri.render());\r\n				</");
	templateBuilder.Append("script>\r\n			</div>\r\n			<div class=\"topic_list i_space\">				\r\n				<ul class=\"spacepost\">\r\n				");
	int __spacepostinfo__loop__id=0;
	foreach(SpaceShortPostInfo __spacepostinfo in spacepostlist)
	{
		__spacepostinfo__loop__id++;

	templateBuilder.Append(" \r\n					<li><cite><a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/?uid=");
	templateBuilder.Append(__spacepostinfo.Uid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(__spacepostinfo.Author.ToString().Trim());
	templateBuilder.Append("</a></cite><a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/viewspacepost.aspx?postid=");
	templateBuilder.Append(__spacepostinfo.Postid.ToString().Trim());
	templateBuilder.Append("\">");	templateBuilder.Append(Utils.GetUnicodeSubString(__spacepostinfo.Title,56,"..."));
	templateBuilder.Append("</a> </li>\r\n				");
	}	//end loop

	templateBuilder.Append("\r\n				</ul>\r\n			</div>\r\n			<div class=\"space_list\">\r\n				");
	int __spaceconfig__loop__id=0;
	foreach(SpaceConfigInfoExt __spaceconfig in spaceconfigs)
	{
		__spaceconfig__loop__id++;


	if (__spaceconfig__loop__id<=3)
	{

	string avatarurl = Avatars.GetAvatarUrl(__spaceconfig.Userid,AvatarSize.Small);
	
	templateBuilder.Append("\r\n				<dl>\r\n					<a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/?uid=");
	templateBuilder.Append(__spaceconfig.Userid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\"><img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" alt=\"blogphoto\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"/></a>\r\n					<dt><a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/?uid=");
	templateBuilder.Append(__spaceconfig.Userid.ToString().Trim());
	templateBuilder.Append("\">");	templateBuilder.Append(Utils.GetUnicodeSubString(__spaceconfig.Spacetitle,20,"..."));
	templateBuilder.Append("</a></dt>\r\n					<dd><a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/viewspacepost.aspx?postid=");
	templateBuilder.Append(__spaceconfig.Postid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(__spaceconfig.Posttitle,30,"..."));
	templateBuilder.Append("</a></dd>\r\n				</dl>\r\n				");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n			</div>\r\n		</div>\r\n	</div>\r\n	<div class=\"list_side\">\r\n		<div class=\"forum_side cl\">\r\n			<div class=\"titlebar\">\r\n				<ul>\r\n					<li id=\"li_space\" class=\"current\" ><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_space'));\">最新更新空间</a></li>\r\n					<li id=\"li_spacecomment\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_spacecomment'));\">最新评论</a></li>\r\n				</ul>\r\n			</div>\r\n			<div class=\"content cl\">\r\n				<ul id=\"spacecommentlist\" class=\"topicdot\" style=\"display:none;\">\r\n				");
	int comment__loop__id=0;
	foreach(DataRow comment in spaceagg.GetSpaceTopComments(13).Rows)
	{
		comment__loop__id++;

	templateBuilder.Append("\r\n					<li><a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/viewspacepost.aspx?postid=" + comment["postid"].ToString().Trim() + "\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(Discuz.Common.Utils.HtmlEncode(comment["content"].ToString().Trim()),30,"..."));
	templateBuilder.Append("</a></li>\r\n				");
	}	//end loop

	templateBuilder.Append("\r\n				</ul>				\r\n				<div id=\"spacelist\" class=\"postcount\" >\r\n				");
	int space__loop__id=0;
	foreach(DataRow space in spaceagg.GetRecentUpdateSpaceList(5).Rows)
	{
		space__loop__id++;


	if (space__loop__id<=5)
	{

	templateBuilder.Append("				\r\n					<dl>\r\n						");	string avatarurl = Avatars.GetAvatarUrl(space["userid"].ToString().Trim(), AvatarSize.Small);
	
	templateBuilder.Append("\r\n						<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" \" style=\"border:1px solid #E8E8E8;padding:1px;\" />\r\n						<dt>\r\n							<a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/?uid=" + space["userid"].ToString().Trim() + "\" target=\"_blank\">");	templateBuilder.Append(Utils.GetUnicodeSubString(space["spacetitle"].ToString().Trim(),30,"..."));
	templateBuilder.Append("</a>\r\n						</dt>\r\n						<dd>日志<em>" + space["PostCount"].ToString().Trim() + "</em>篇</dd>\r\n					</dl>\r\n					");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n				</div>\r\n			</div>\r\n		</div>\r\n	</div>\r\n</div>\r\n");
	}	//end if


	}	//end if


	if (Discuz.Plugin.Album.AlbumPluginProvider.GetInstance()!=null)
	{


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n<div class=\"forum_list cl album other\">\r\n	<h3>\r\n	<cite><a href=\"albumindex.aspx\" target=\"_blank\">更多相册</a></cite><strong><a href=\"albumindex.aspx\" target=\"_blank\">论坛相册</a></strong>\r\n	");
	int ac__loop__id=0;
	foreach(AlbumCategoryInfo ac in albumcategorylist)
	{
		ac__loop__id++;

	templateBuilder.Append("\r\n		<a href=\"showalbumlist.aspx?cate=");
	templateBuilder.Append(ac.Albumcateid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(ac.Title.ToString().Trim());
	templateBuilder.Append("</a><span class=\"pipe\">|</span>\r\n		");
	if (ac__loop__id>=4)
	{

	break;


	}	//end if


	}	//end loop

	templateBuilder.Append("	\r\n	</h3>\r\n	<div class=\"list_main\">\r\n		<div class=\"forum_topic cl\">\r\n			<div class=\"photo\">\r\n				<ul>\r\n					");
	int photo__loop__id=0;
	foreach(PhotoInfo photo in recommendphotolist)
	{
		photo__loop__id++;


	if (photo__loop__id<=4)
	{

	templateBuilder.Append("\r\n						<li>\r\n						<a href=\"showphoto.aspx?photoid=");
	templateBuilder.Append(photo.Photoid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\"><img src=\"");
	templateBuilder.Append(photo.Filename.ToString().Trim());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/album/errorphoto.gif';\" alt=\"");
	templateBuilder.Append(photo.Title.ToString().Trim());
	templateBuilder.Append("\" title=\"");
	templateBuilder.Append(photo.Title.ToString().Trim());
	templateBuilder.Append("\"  height=\"100\"/></a>\r\n						<p><a href=\"showphoto.aspx?photoid=");
	templateBuilder.Append(photo.Photoid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	if (photo.Title=="")
	{

	templateBuilder.Append("暂无标题");
	}
	else
	{
	templateBuilder.Append(photo.Title.ToString().Trim());
	}	//end if

	templateBuilder.Append("</a></p>\r\n						</li>\r\n						");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n				</ul>\r\n			</div>\r\n			<div class=\"album_list\">					\r\n				");
	int __albuminfo__loop__id=0;
	foreach(AlbumInfo __albuminfo in recommendalbumlist)
	{
		__albuminfo__loop__id++;


	if (__albuminfo__loop__id<=4)
	{

	templateBuilder.Append("\r\n				<dl>\r\n					<dd>\r\n					<a href=\"");
	templateBuilder.Append(albumurl.ToString());
	templateBuilder.Append("showalbum.aspx?albumid=");
	templateBuilder.Append(__albuminfo.Albumid.ToString().Trim());
	templateBuilder.Append("\">\r\n					");
	if (__albuminfo.Logo!="")
	{

	templateBuilder.Append("\r\n					<img src=\"");
	templateBuilder.Append(__albuminfo.Logo.ToString().Trim());
	templateBuilder.Append("\" alt=\"");
	templateBuilder.Append(__albuminfo.Title.ToString().Trim());
	templateBuilder.Append("\" style=\"height: 100px; width: 120px\"/>\r\n					");
	}
	else
	{

	templateBuilder.Append("\r\n					<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/NoPhoto.jpg\" alt=\"");
	templateBuilder.Append(__albuminfo.Albumid.ToString().Trim());
	templateBuilder.Append("\"  style=\"height: 75px; width: 115px\"/>\r\n					");
	}	//end if

	templateBuilder.Append("	\r\n					</a>\r\n					</dd>\r\n					<dt><a href=\"");
	templateBuilder.Append(albumurl.ToString());
	templateBuilder.Append("showalbum.aspx?albumid=");
	templateBuilder.Append(__albuminfo.Albumid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(__albuminfo.Title.ToString().Trim());
	templateBuilder.Append("</a></dt>\r\n				</dl>\r\n				");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n			</div>\r\n		</div>\r\n	</div>\r\n	<div class=\"list_side\">\r\n		<div class=\"forum_side cl\">\r\n			<div class=\"titlebar\">\r\n				<ul>\r\n					<li class=\"current\" id=\"li_album\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_album'));\">热门相册</a></li>\r\n					<li id=\"li_photo\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_photo'));\">热门相片</a></li>\r\n				</ul>\r\n			</div>\r\n			<div class=\"content cl\">\r\n				<ul id=\"albumlist\" class=\"forum_hot_topic\">\r\n				");	 albumlist = albumagg.GetAlbumList(photoconfig.Focusalbumshowtype, 7, 180);
	

	if (albumlist.Count>0)
	{


	int hotalbuminfo__loop__id=0;
	foreach(AlbumInfo hotalbuminfo in albumlist)
	{
		hotalbuminfo__loop__id++;

	templateBuilder.Append("\r\n					<li><a href=\"showalbum.aspx?albumid=");
	templateBuilder.Append(hotalbuminfo.Albumid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(hotalbuminfo.Title.ToString().Trim());
	templateBuilder.Append("</a> (<a href=\"showalbumlist.aspx?uid=");
	templateBuilder.Append(hotalbuminfo.Userid.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(hotalbuminfo.Username.ToString().Trim());
	templateBuilder.Append("</a>)</li>\r\n				");
	}	//end loop


	}
	else
	{

	templateBuilder.Append("\r\n					暂无数据!\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</ul>\r\n				<ul id=\"photolist\" class=\"forum_hot_topic\" style=\"display:none;\">\r\n				  <!--一周热图总排行-->\r\n				");	 photolist = albumagg.GetWeekHotPhotoList(photoconfig.Weekhot);
	

	if (photolist.Count>0)
	{


	int __photolist__loop__id=0;
	foreach(PhotoInfo __photolist in photolist)
	{
		__photolist__loop__id++;

	templateBuilder.Append("\r\n					<li><em>");
	templateBuilder.Append(__photolist.Views.ToString().Trim());
	templateBuilder.Append("</em><a href=\"showphoto.aspx?photoid=");
	templateBuilder.Append(__photolist.Photoid.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(__photolist.Title.ToString().Trim());
	templateBuilder.Append("</a> (<a href=\"showalbumlist.aspx?uid=");
	templateBuilder.Append(__photolist.Userid.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(__photolist.Username.ToString().Trim());
	templateBuilder.Append("</a>)</li>\r\n				");
	}	//end loop


	}
	else
	{

	templateBuilder.Append("\r\n					暂无数据!\r\n				");
	}	//end if

	templateBuilder.Append("\r\n				</ul>\r\n			</div>\r\n		</div>\r\n	</div>\r\n</div>\r\n");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n<div id=\"websiterecalbumAd\" class=\"ad cl\"></div>\r\n");
	if (forumlinkcount>0)
	{

	templateBuilder.Append("\r\n<div class=\"bm cl\" id=\"forumlink\">\r\n<div class=\"bm_inner\">\r\n	<div id=\"forumlinks\">	\r\n	");	bool forumlinkend = false;
	

	int forumlink__loop__id=0;
	foreach(DataRow forumlink in forumlinklist.Rows)
	{
		forumlink__loop__id++;


	if (forumlink__loop__id==1)
	{

	templateBuilder.Append("\r\n		<ul class=\"forumlinks\">	\r\n		");
	}	//end if


	if (forumlink["logo"].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n		<li>\r\n			<a href=\"" + forumlink["url"].ToString().Trim() + "\" target=\"_blank\"><img src=\"" + forumlink["logo"].ToString().Trim() + "\" alt=\"" + forumlink["name"].ToString().Trim() + "\"  class=\"forumlink_logo\"/></a>\r\n			<h5><a href=\"" + forumlink["url"].ToString().Trim() + "\" target=\"_blank\">" + forumlink["name"].ToString().Trim() + "</a></h5>\r\n			<p>" + forumlink["note"].ToString().Trim() + "</p>\r\n		</li>\r\n		");
	}
	else if (forumlink["name"].ToString().Trim()!="$$otherlink$$")
	{

	templateBuilder.Append("\r\n		<li>\r\n			<h5>\r\n				<a href=\"" + forumlink["url"].ToString().Trim() + "\" target=\"_blank\">" + forumlink["name"].ToString().Trim() + "</a>\r\n			</h5>\r\n			<p>" + forumlink["note"].ToString().Trim() + "</p>\r\n		</li>\r\n	");
	}
	else
	{


	if (forumlinkend==false)
	{

	templateBuilder.Append("\r\n		</ul>\r\n		");	 forumlinkend = true;
	

	}	//end if

	templateBuilder.Append("\r\n	<div class=\"" + forumlink["url"].ToString().Trim() + "\">\r\n	    <ul>\r\n		" + forumlink["note"].ToString().Trim() + "\r\n		</ul>\r\n	</div>\r\n	");
	}	//end if


	}	//end loop


	if (forumlinkend==false)
	{

	templateBuilder.Append("\r\n		</ul>\r\n	</div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n</div>\r\n</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n</div>\r\n");
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

	templateBuilder.Append("\r\n</div>\r\n");
	}	//end if

	string webSiteBottomAd = Advertisements.GetWebSiteAd(AdType.WebSiteBottomAd);
	


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



	string webSiteHeaderAd = Advertisements.GetWebSiteAd(AdType.WebSiteHeaderAd);
	
	string webSiteHotTopicAd = Advertisements.GetWebSiteAd(AdType.WebSiteHotTopicAd);
	
	string webSiteUserPostTopAd = Advertisements.GetWebSiteAd(AdType.WebSiteUserPostTopAd);
	
	string webSiteRecForumTopAd = Advertisements.GetWebSiteAd(AdType.WebSiteRecForumTopAd);
	
	string webSiteRecForumBottomAd = Advertisements.GetWebSiteAd(AdType.WebSiteRecForumBottomAd);
	
	string webSiteRecAlbumAd = Advertisements.GetWebSiteAd(AdType.WebSiteRecAlbumAd);
	
	templateBuilder.Append("\r\n<div id=\"websiteheaderad_nodisplay\" style=\"display:none\">");
	templateBuilder.Append(webSiteHeaderAd.ToString());
	templateBuilder.Append("</div>\r\n<div id=\"websitehottopicad_nodisplay\" style=\"display:none\">");
	templateBuilder.Append(webSiteHotTopicAd.ToString());
	templateBuilder.Append("</div>\r\n<div id=\"websiteuserposttopad_nodisplay\" style=\"display:none\">");
	templateBuilder.Append(webSiteUserPostTopAd.ToString());
	templateBuilder.Append("</div>\r\n<div id=\"websiterecforumtopad_nodisplay\" style=\"display:none\">");
	templateBuilder.Append(webSiteRecForumTopAd.ToString());
	templateBuilder.Append("</div>\r\n<div id=\"websiterecforumbottomad_nodisplay\" style=\"display:none\">");
	templateBuilder.Append(webSiteRecForumBottomAd.ToString());
	templateBuilder.Append("</div>\r\n<div id=\"websiterecalbumAd_nodisplay\" style=\"display:none\">");
	templateBuilder.Append(webSiteRecAlbumAd.ToString());
	templateBuilder.Append("</div>\r\n<div id=\"websitebottomad_nodisplay\" style=\"display:none\">");
	templateBuilder.Append(webSiteBottomAd.ToString());
	templateBuilder.Append("</div>\r\n<script type=\"text/javascript\">\r\nvar adDiv = [\"websiteheaderad\", \"websitehottopicad\", \"websiteuserposttopad\", \"websiterecforumtopad\", \"websiterecforumbottomad\", \"websiterecalbumAd\", \"websitebottomad\"];\r\nfor (var i = 0; i < adDiv.length;  i++) {\r\n    if ($(adDiv[i] + '_nodisplay').innerHTML == '')\r\n        $(adDiv[i]).style.display = 'none';\r\n    else {\r\n        $(adDiv[i]).innerHTML = $(adDiv[i] + '_nodisplay').innerHTML;\r\n        $(adDiv[i] + '_nodisplay').parentNode.removeChild($(adDiv[i] + '_nodisplay'));\r\n    }\r\n}\r\n</");
	templateBuilder.Append("script>\r\n");

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
