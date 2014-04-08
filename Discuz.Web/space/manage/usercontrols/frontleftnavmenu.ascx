<%@ Control Language="c#" AutoEventWireup="false" Codebehind="frontleftnavmenu.ascx.cs" Inherits="Discuz.Space.Manage.frontleftnavmenu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

	<%if (hidetitle == 0){%>
				<h2 class="modtitle"><span class="modtitle_text">用户菜单</span></h2>

	<%}%>	
		<div class="dnt-leftmenu">	
		    <ul>			
					<li><a href="<% = forumpath %>showalbumlist.aspx?uid=<% = spaceuid%>">我的<% = config.Albumname%></a></li>
					<li><a href="<% = forumpath %>space/?uid=<% = spaceuid%>">个人首页</a></li>
					<li><a href="<% = forumpath %>space/viewspacepostlist.aspx?spaceid=<% = spaceid%>">我的日志</a></li>
					<% if (isshowuserpanel){%>
					<li><a href="<% = forumpath %>usercpspaceset.aspx"><% = config.Spacename%>管理</a></li>
					<% }%>
			</ul>
		</div>