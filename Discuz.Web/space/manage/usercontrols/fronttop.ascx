<%@ Control Language="c#" AutoEventWireup="false" Codebehind="fronttop.ascx.cs" Inherits="Discuz.Space.Manage.fronttop" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div id="topbar" style="left: 0px; top: 0px; position: absolute">
</div>

<div align="right" class="menu">
	<div id="duser" width=100%><nobr>&nbsp;
	<% if (isLogged)
    {%>
		<a href="<% = forumurlnopage%>/usercp.aspx"><b><% = username%></b></a>&nbsp;|&nbsp;
		<a href="<% = spacehttplink %>">我的<% = config.Spacename%></a>(<a href="<% = forumurlnopage%>/usercpspacemanageblog.aspx">设置</a>)&nbsp;|&nbsp;
		<a href="<% = forumurl%>">论坛</a>&nbsp;|&nbsp;
		<a href="<% = configalbumurl%>"><% = config.Albumname%></a>&nbsp;|&nbsp;
		<a href="<% = configspaceurl%>"><% = config.Spacename%>首页</a>&nbsp;|&nbsp;
		<a href="<% = forumurlnopage%>logout.aspx?userkey=<% = userkey%>">退出</a></nobr>
	<%}else{%>
	<a href="#" onclick="window.location.href='<% =forumurlnopage%>/login.aspx?reurl=' + _esc(document.location);">登录</a>&nbsp;|&nbsp;
	    <a href="<% = forumurl%>">论坛</a>&nbsp;|&nbsp;
		<a href="<% = configalbumurl%>"><% = config.Albumname%></a>&nbsp;|&nbsp;
		<a href="<% = configspaceurl%>"><% = config.Spacename%>首页</a>

	<%}%>	
	</nobr>
	</div>
</div>

<div id="doc">
	<div id="nhdrwrap">
		<div id="nhdrwrapinner">
			<div id="nhdrwrapsizer">
				<div class="title"><h1><% = spaceconfiginfo.Spacetitle%></h1><span><a href="<% = spacehttplink %>"><% = spacehttplink %></a></span></div>
				<div class="subtitle"><% = spaceconfiginfo.Description%></div>
				<noscript><div id="noscript_msg" class="msg"><div id="noscript_box" class="msg_box">本功能需要JavaScript支持，开启以获得更多功能。</div></div></noscript>
			</div>
		
			<div id="tabs"><ul><% =tabs %></ul>
			</div>
		</div>
	</div>
	