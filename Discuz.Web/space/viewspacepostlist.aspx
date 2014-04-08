<%@ Page language="c#" Codebehind="viewspacepostlist.aspx.cs" AutoEventWireup="false" Inherits="Discuz.Space.viewpostlist" %>
<%@ Register TagPrefix="uc1" TagName="ajaxspaceconfigstatic" Src="manage/usercontrols/ajaxspaceconfigstatic.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxspacelink" Src="manage/usercontrols/ajaxspacelink.ascx" %>
<%@ Register TagPrefix="uc1" TagName="spacecalendar" Src="manage/usercontrols/spacecalendar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxtopnewpost" Src="manage/usercontrols/ajaxtopnewpost.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxtopnewcomment" Src="manage/usercontrols/ajaxtopnewcomment.ascx" %>
<%@ Register TagPrefix="uc1" TagName="frontleftnavmenu" Src="manage/usercontrols/frontleftnavmenu.ascx" %>

<%@ Register TagPrefix="uc1" TagName="ajaxuserbloglist" Src="manage/usercontrols/ajaxuserbloglist.ascx" %>
<%@ Register TagPrefix="uc1" TagName="fronttop" Src="manage/usercontrols/fronttop.ascx" %>
<%@ Register TagPrefix="uc1" TagName="frontbottom" Src="manage/usercontrols/frontbottom.ascx" %>


<html>
	<head>
	    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="keywords" content="ASP.net,论坛,space,blog" />
		<meta name="description" content="Discuz!NT <% = config.Spacename%>" />
		<title>日志列表 - <%=spaceconfiginfo.Spacetitle%> - <%=config.Forumtitle%><% = config.Spacename%> - Powered by Discuz!NT</title>
		
		<link href="skins/themes/space.css" rel="stylesheet" type="text/css" id="css" />
		<link rel="stylesheet" type="text/css" href="skins/themes/blog.css" />
		<link rel="stylesheet" type="text/css" href="skins/themes/<%=spaceconfiginfo.ThemePath%>/style.css" />
	
	    <script type="text/javascript">var forumpath="<%=forumpath %>space/";var aspxrewrite="<%=config.Aspxrewrite %>";</script>
		<script type="text/javascript" src="javascript/space.js"></script>
		<script language="javascript" src="manage/js/AjaxHelper.js" type="text/javascript"></script>
		<script language="javascript" src="manage/js/common.js"></script>
		<style>
		.modbox {margin:5px 5px;}
		.col {vertical-align:top;height:100px;}
		#category{
				line-height:240%;
		}
		#category li{
				border-bottom:1px solid #eee;
				text-align:center;
		}
		#list h3{
				font-weight:bold;
		}
		.AddModule{
				line-height:30px;
		}
		.AddModule span{
						padding:2px;
						border:1px solid #333;
		}
		.PageList strong{
						color:#333;
		}
		</style>
	</head>
<body>

<uc1:fronttop id="fronttop1" runat="server"></uc1:fronttop>

<div id="modules">
<table width="100%" align="center" cellpadding="0" cellspacing="0" id="t_1">
	<tr>
		<td valign="top" class="viewcol" id="col_1" style="width: 33%;">
		<div id="m_6" class="modbox">
					<div id="managenavmenu">
						<uc1:frontleftnavmenu id="frontleftnavmenu1" runat="server"></uc1:frontleftnavmenu>
					</div>
			</div>

			<div id="m_4" class="modbox">
				<uc1:spacecalendar id="spacecalendar1" runat="server"></uc1:spacecalendar>
			</div>


			<div id="m_1" class="modbox">
				<div id="userlink">
					<uc1:ajaxspacelink id="ajaxspacelink1" runat="server"></uc1:ajaxspacelink>
				</div>
			</div>

			<div id="m_5" class="modbox">
				<div id="infomation">
					<uc1:ajaxspaceconfigstatic id="ajaxspaceconfigstatic1" runat="server"></uc1:ajaxspaceconfigstatic>
				</div>
			</div>

			<div id="m_2" class="modbox">
				<div id="ajaxtopnewcomment">
					<uc1:ajaxtopnewcomment id="ajaxtopnewcomment1" runat="server"></uc1:ajaxtopnewcomment>
				</div>
			</div>

			<div id="m_3" class="modbox">
				<div id="ajaxtopnewpost">
					<uc1:ajaxtopnewpost id="ajaxtopnewpost1" runat="server"></uc1:ajaxtopnewpost>
				</div>
			</div>
		</td>
		<td valign="top" class="viewcol" id="col_2" style="width: 67%;">
			<div id = "bodyrightcontent">
				<!--UserBlogListStart-->
					<div id="ajaxspaceuserbloglist" class="modbox">
					
							<uc1:ajaxuserbloglist id="ajaxuserbloglist1" runat="server"></uc1:ajaxuserbloglist>
					</div>
				<!--UserBlogListEnd-->
			</div>
		</td>
	</tr>
</table>

</div>
</div>


<div id="footer">
	<uc1:frontbottom id="frontbottom1" runat="server"></uc1:frontbottom>
</div>
		
		
	
</body>
</html>


<script language="javascript1.2" type="text/javascript">	
function parsetag(){eval(_gel('posttagscript').innerHTML);}
var urlparam="load=true&spaceid="+<%=spaceid%>;

AjaxProxyUrl = new String("manage/ajax.aspx");
  
//加载数据统计页面
AjaxHelper.Updater('usercontrols/ajaxspaceconfigstatic.ascx','infomation',urlparam);

//加载友情链接页面
AjaxHelper.Updater('usercontrols/ajaxspacelink.ascx','userlink',urlparam);

//加载最新日志列表
AjaxHelper.Updater('usercontrols/ajaxtopnewpost.ascx','ajaxtopnewpost',urlparam);

//最新评论
AjaxHelper.Updater('usercontrols/ajaxtopnewcomment.ascx','ajaxtopnewcomment',urlparam);

//日志列表
AjaxHelper.Updater('usercontrols/ajaxuserbloglist.ascx','ajaxspaceuserbloglist',urlparam+'&currentpage=1', parsetag);

</script>

