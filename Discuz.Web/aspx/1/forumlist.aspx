<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.forumlist" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/6/2 16:12:39.
		本页面代码由Discuz!NT模板引擎生成于 2011/6/2 16:12:39. 
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

	templateBuilder.Append("\r\n<style>\r\nbody{\r\ntext-align:left;background:#F5FAFD;overflow-y:hidden;\r\n}\r\n.collapse,.expand{position:absolute;top:0;left:0;background-image:url(");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapse.gif);background-repeat:no-repeat;background-position:50% 50%;width:6px; height:50px;}\r\n.expand {background-image:url(");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/expand.gif);}\r\n</style>\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\n		var NoUser = ");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append(" == -1 ? true : false;\r\n		var lastA = null;		\r\n		function window_load(){\r\n			documentbody = document.documentElement.clientHeight > document.body.clientHeight ? document.documentElement : document.body;\r\n			var leftbar = document.getElementById('leftbar')\r\n			leftbar.style.height = documentbody.clientHeight +'px';\r\n			leftbar.style.left = 0; //document.body.clientWidth - 6;\r\n			leftbar.style.top = documentbody.scrollTop + 'px'; //document.body.clientWidth - 6;\r\n			document.onscroll = function(){ \r\n											leftbar.style.height=documentbody.clientHeight +'px';\r\n											leftbar.style.top=documentbody.scrollTop + 'px'; \r\n										}\r\n										\r\n			document.onresize = function(){ \r\n											leftbar.style.height=documentbody.clientHeight +'px';\r\n											leftbar.style.top=documentbody.scrollTop + 'px'; \r\n										}\r\n			\r\n		}\r\n		function resizediv_onClick(){\r\n			if (document.getElementById('menubar').style.display != 'none'){\r\n				top.document.getElementsByTagName('FRAMESET')[0].cols = \"8,*\";\r\n				document.getElementById('menubar').style.display = 'none';\r\n				document.getElementById('leftbar').className = \"expand\";\r\n			}\r\n			else{\r\n				top.document.getElementsByTagName('FRAMESET')[0].cols = \"210,*\";\r\n				document.getElementById('leftbar').className = \"collapse\";\r\n				document.getElementById('menubar').style.display = '';\r\n			}\r\n		\r\n		}\r\n		\r\n		//↓----------获得版块的树形列表相关脚本-------------------------\r\n		function changeExtImg(objImg){\r\n			if (!objImg){ return; }	\r\n			var fileName = objImg.src.toLowerCase().substring(objImg.src.lastIndexOf(\"/\"));\r\n			switch(fileName){\r\n				case \"/p0.gif\":\r\n					objImg.src = \"images/tree/m0.gif\";\r\n					break;\r\n				case \"/p1.gif\":\r\n					objImg.src = \"images/tree/m1.gif\";\r\n					break;\r\n				case \"/p2.gif\":\r\n					objImg.src = \"images/tree/m2.gif\";\r\n					break;\r\n				case \"/p3.gif\":\r\n					objImg.src = \"images/tree/m3.gif\";\r\n					break;\r\n				case \"/m0.gif\":\r\n					objImg.src = \"images/tree/p0.gif\";\r\n					break;\r\n				case \"/m1.gif\":\r\n					objImg.src = \"images/tree/p1.gif\";\r\n					break;\r\n				case \"/m2.gif\":\r\n					objImg.src = \"images/tree/p2.gif\";\r\n					break;\r\n				case \"/m3.gif\":\r\n					objImg.src = \"images/tree/p3.gif\";\r\n					break;\r\n			}\r\n		}\r\n\r\n		function changeFolderImg(objImg){\r\n			if (!objImg){ return; }	\r\n			var fileName = objImg.src.toLowerCase().substring(objImg.src.lastIndexOf(\"/\"));\r\n			switch(fileName){\r\n				case \"/folder.gif\":\r\n					objImg.src = \"images/tree/folderopen.gif\";\r\n					break;\r\n				case \"/folderopen.gif\":\r\n					objImg.src = \"images/tree/folder.gif\";\r\n					break;\r\n			}\r\n		}\r\n		\r\n		\r\n		function a_click(objA){\r\n			if (lastA){\r\n				lastA.className=''; \r\n			}\r\n			objA.className='bold'; \r\n			lastA = objA; \r\n		}\r\n\r\n		function writesubforum(objreturn,fid,AtEnd){\r\n			var process = document.getElementById(\"process_\" + fid);\r\n			var forum = document.getElementById(\"forum_\" + fid);\r\n			var dataArray = objreturn.getElementsByTagName('forum');\r\n			var dataArrayLen = dataArray.length;\r\n			\r\n			changeExtImg(document.getElementById(\"forumExt_\" + fid));\r\n			changeFolderImg(document.getElementById(\"forumFolder_\" + fid));\r\n			\r\n			for (i=0;i<dataArrayLen;i++){\r\n				var thisfid = dataArray[i].getAttribute(\"fid\");\r\n				var subforumcount = dataArray[i].getAttribute(\"subforumcount\");\r\n				var thisEnd = i==dataArrayLen-1;\r\n				\r\n				var layer = dataArray[i].getAttribute(\"layer\");\r\n\r\n					//显示树型线\r\n					list = \"\";\r\n					\r\n					for (l=1;l<=layer;l++){\r\n						if (AtEnd && NoUser){\r\n							list += \"<nobr><img src = \\\"images/tree/L5.gif\\\" align=\\\"absmiddle\\\" />\";\r\n						}\r\n						else{\r\n							list += \"<img src = \\\"images/tree/L4.gif\\\" align=\\\"absmiddle\\\" />\";\r\n						}\r\n					}\r\n					if (subforumcount>0){\r\n						folder = \"folder.gif\";\r\n						if (layer==0 && thisEnd){\r\n							if (NoUser){\r\n								src = \"p2.gif\";\r\n							}\r\n							else{\r\n								src = \"p1.gif\";\r\n							}\r\n						}\r\n						else{\r\n							if (thisEnd && layer>0){\r\n								src = \"P2.gif\";\r\n							}\r\n							else{\r\n								//if (i==0 && layer==0){\r\n								//	src = \"P0.gif\";\r\n								//}\r\n								//else{\r\n									src = \"P1.gif\";\r\n								//}\r\n							}\r\n						}\r\n					}\r\n					else{\r\n						folder = \"file.gif\";\r\n						if (layer==0 && thisEnd){\r\n							if (NoUser){\r\n								src = \"m2.gif\";\r\n							}\r\n							else{\r\n								src = \"m1.gif\";\r\n							}\r\n						}\r\n						else{\r\n							if (thisEnd){\r\n								src = \"L2.gif\";\r\n							}\r\n							else{\r\n								//if (i==0 && layer==0){\r\n								//	src = \"L0.gif\";\r\n								//}\r\n								//else{\r\n									src = \"L1.gif\";\r\n								//}\r\n							}\r\n						}\r\n					}\r\n					\r\n					if(");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append(")\r\n					{\r\n						list += \"<img id=\\\"forumExt_\" + thisfid + \"\\\" src = \\\"images/tree/\" + src + \"\\\" align=\\\"absmiddle\\\" /><img id=\\\"forumFolder_\" + thisfid + \"\\\" src = \\\"images/tree/\" + folder + \"\\\" align=\\\"absmiddle\\\" /> <a href=\\\"showforum-\" + thisfid + \".aspx\\\" target=\\\"main\\\" title=\\\"\" + dataArray[i].getAttribute(\"name\") + \"\\\" onclick=\\\"a_click(this);\\\">\" + dataArray[i].getAttribute(\"name\") + \"</a></nobr>\";\r\n					}\r\n					else\r\n					{\r\n						list += \"<img id=\\\"forumExt_\" + thisfid + \"\\\" src = \\\"images/tree/\" + src + \"\\\" align=\\\"absmiddle\\\" /><img id=\\\"forumFolder_\" + thisfid + \"\\\" src = \\\"images/tree/\" + folder + \"\\\" align=\\\"absmiddle\\\" /> <a href=\\\"showforum.aspx?forumid=\" + thisfid + \"\\\" target=\\\"main\\\" title=\\\"\" + dataArray[i].getAttribute(\"name\") + \"\\\" onclick=\\\"a_click(this);\\\">\" + dataArray[i].getAttribute(\"name\") + \"</a></nobr>\";\r\n					}\r\n\r\n\r\n				var div_forumtitle =  document.createElement(\"DIV\");\r\n					div_forumtitle.id = \"forumtitle_\" + thisfid;\r\n					div_forumtitle.className = \"tree_forumtitle\";\r\n					if (subforumcount>0){\r\n						div_forumtitle.onclick = new Function(\"getsubforum(\" + thisfid + \",\" + thisEnd + \");\");\r\n					}\r\n					div_forumtitle.innerHTML = list;\r\n					forum.appendChild(div_forumtitle);\r\n					\r\n				var div_forum = document.createElement(\"DIV\");\r\n					div_forum.id = \"forum_\" + thisfid;\r\n					div_forum.className = \"tree_forum\";\r\n					forum.appendChild(div_forum);\r\n				\r\n				\r\n			}\r\n			process.style.display=\"none\";\r\n		}\r\n		\r\n		\r\n		\r\n		function getsubforum(fid,AtEnd){\r\n			if (!document.getElementById(\"forum_\" + fid)){\r\n				document.writeln(\"<div id=\\\"forum_\" + fid + \"\\\"></div>\");\r\n			}\r\n			if (!document.getElementById(\"process_\" + fid)){\r\n				var div = document.createElement(\"DIV\");\r\n				div.id = \"process_\" + fid;\r\n				div.className = \"tree_process\";\r\n				div.innerHTML = \"<img src='images/common/loading.gif' />载入中...\";\r\n				\r\n				document.getElementById(\"forum_\" + fid).appendChild(div);\r\n				\r\n				ajaxRead(\"tools/ajax.aspx?t=forumtree&fid=\" + fid, \"writesubforum(obj,\" + fid+ \",\" + AtEnd + \");\");\r\n			}\r\n			else{\r\n				changeExtImg(document.getElementById(\"forumExt_\" + fid));\r\n				changeFolderImg(document.getElementById(\"forumFolder_\" + fid));\r\n				if (document.getElementById(\"forum_\" + fid).style.display == \"none\"){\r\n					document.getElementById(\"forum_\" + fid).style.display = \"block\";\r\n				}\r\n				else{												\r\n					document.getElementById(\"forum_\" + fid).style.display = \"none\";\r\n				}\r\n			}\r\n\r\n		}\r\n		\r\n		//↑----------获得版块的树形列表相关脚本-------------------------\r\n		\r\n</");
	templateBuilder.Append("script>\r\n<div id=\"leftbar\" class=\"collapse\" onmouseover=\"this.style.backgroundColor='#A7E8F3';\" onmouseout=\"this.style.backgroundColor = '';\" onclick=\"resizediv_onClick()\" style=\"width:6px; cursor:pointer\" title=\"打开/关闭导航\"></div>\r\n<div id=\"menubar\" style=\"white-space:nowrap;\">\r\n	<div id=\"frameback\">\r\n		<A href=\"###\" onClick=\"resizediv_onClick()\" class=\"hideside\">隐藏侧栏</a><A href=\"forumindex.aspx?f=0\" target=\"_top\" class=\"back\">平板模式</a>\r\n	</div>\r\n</div>\r\n<div class=\"framemenu\">\r\n	<ul>\r\n	");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n		<li>欢迎访问");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</li>\r\n		<li>");	 aspxrewriteurl = this.UserInfoAspxRewrite(userinfo.Uid);
	
	templateBuilder.Append("				\r\n		<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"main\" class=\"lightlink\">");
	templateBuilder.Append(userinfo.Username.ToString().Trim());
	templateBuilder.Append("</a> [ <a href=\"logout.aspx?userkey=");
	templateBuilder.Append(userkey.ToString());
	templateBuilder.Append("&amp;reurl=index.aspx\" target=\"main\">退出</a> ]</li>\r\n		<li>积分: <span class=\"lightlink\">");
	templateBuilder.Append(userinfo.Credits.ToString().Trim());
	templateBuilder.Append("</span>  [<span id=\"creditlist\" onMouseOver=\"showMenu(this.id, false);\" style=\"CURSOR:pointer\">详细积分</span>]</li>\r\n		<li>头衔: ");
	templateBuilder.Append(usergroupinfo.Grouptitle.ToString().Trim());
	templateBuilder.Append("\r\n			");
	if (useradminid==1)
	{

	templateBuilder.Append("\r\n				| <a href=\"admin/index.aspx\" target=\"_blank\">系统设置</a>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</li>\r\n	");
	}
	else
	{

	templateBuilder.Append("\r\n		<li>头衔: 游客\r\n			[<a href=\"register.aspx\" target=\"main\">注册</a>] \r\n			[<a href=\"login.aspx?reurl=index.aspx\" target=\"main\">登录</a>]\r\n		</li>\r\n	");
	}	//end if


	if (oluserinfo.Newpms>0)
	{

	templateBuilder.Append("\r\n		<li>\r\n			新的短消息<a href=\"usercpinbox.aspx\" target=\"main\"><span id=\"newpmcount\" class=\"lightlink\">");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("</span></a>条\r\n		</li>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n		<li><hr class=\"solidline\"/></li>\r\n		<li>\r\n			<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/home.gif\">\r\n			<a href=\"forumindex.aspx\" target=\"main\">论坛首页</a>\r\n		</li>\r\n		<li>\r\n			<img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" /><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/folder_new.gif\" width=\"20\" height=\"20\" />\r\n			<a href=\"showtopiclist.aspx?type=newtopic&amp;newtopic=");
	templateBuilder.Append(newtopicminute.ToString());
	templateBuilder.Append("&amp;forums=all\" target=\"main\">查看新帖</a>\r\n		</li>\r\n		<li>\r\n			<img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/showdigest.gif\" width=\"20\" height=\"20\" />\r\n			<a href=\"showtopiclist.aspx?type=digest&amp;forums=all\" target=\"main\">精华帖区</a>\r\n		</li>\r\n	</ul>\r\n	<script type=\"text/javascript\">\r\n		//生成版块列表\r\n		getsubforum(0);\r\n	</");
	templateBuilder.Append("script>\r\n	");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n	<div onClick=\"getsubforum(-1,true);\">\r\n		<img id=\"forumExt_-1\" src=\"images/tree/M2.gif\" width=\"20\" height=\"20\" /><img id=\"forumFolder_-1\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/mytopic.gif\" /><span class=\"cursor\">用户功能区</span>\r\n	</div>\r\n	<div id=\"process_-1\"></div>\r\n	<div id=\"forum_-1\" style=\"display:block;\">\r\n		<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/folder_mytopic.gif\" width=\"16\" height=\"16\">\r\n			<a href=\"mytopics.aspx\" target=\"main\">我的主题</a></div>\r\n		<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/folder_s.gif\" width=\"16\" height=\"16\">\r\n			<a href=\"myposts.aspx\" target=\"main\">我的帖子</a></div>\r\n		<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/digest.gif\">\r\n			<a href=\"search.aspx?posterid=");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("&amp;type=digest&searchsubmit=1\" target=\"main\">我的精华</a></div>\r\n		<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/favorite.gif\">\r\n			<a href=\"usercpsubscribe.aspx\" target=\"main\">我的收藏</a></div>\r\n		<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/usericon.gif\">\r\n			<a href=\"usercp.aspx\" target=\"main\">用户中心</a></div>\r\n		<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L2.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/pm_1.gif\" width=\"16\" height=\"16\">\r\n			<a href=\"usercppostpm.aspx\" target=\"main\">撰写短消息</a></div>\r\n	</div>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n	</div>\r\n	<hr class=\"solidline\"/>\r\n	<div class=\"framemenu\">\r\n		<ul>\r\n			<li>在线用户: </li>\r\n			<li>");
	templateBuilder.Append(totalonline.ToString());
	templateBuilder.Append("人在线  (");
	templateBuilder.Append(totalonlineuser.ToString());
	templateBuilder.Append("位会员) </li>\r\n			");
	if (config.Rssstatus!=0)
	{

	templateBuilder.Append("\r\n			<li>\r\n			<a href=\"tools/rss.aspx\" target=\"_blank\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/rss2.gif\" alt=\"RSS订阅全部论坛\"></a>\r\n			</li>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		 </ul>\r\n	</div>\r\n</div>\r\n");
	templateBuilder.Append("</body>\r\n</html>\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n]]></root>\r\n");
	}	//end if



	templateBuilder.Append("\r\n<ul id=\"creditlist_menu\" class=\"popupmenu_popup\" style=\"display:none;\">\r\n	");
	if (score[1].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[1].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits1.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if


	if (score[2].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[2].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits2.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if


	if (score[3].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[3].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits3.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if


	if (score[4].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[4].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits4.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if


	if (score[5].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[5].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits5.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if


	if (score[6].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[6].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits6.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if


	if (score[7].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[7].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits7.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if


	if (score[8].ToString().Trim()!="")
	{

	templateBuilder.Append("\r\n	<li>" + score[8].ToString().Trim() + ": ");
	templateBuilder.Append(userinfo.Extcredits8.ToString().Trim());
	templateBuilder.Append("</li>\r\n	");
	}	//end if

	templateBuilder.Append("\r\n</ul>");
	Response.Write(templateBuilder.ToString());
}
</script>
