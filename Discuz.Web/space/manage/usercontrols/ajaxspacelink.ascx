<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxspacelink.ascx.cs" Inherits="Discuz.Space.Manage.ajaxspacelink" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import namespace="Discuz.Entity" %>

<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
   Response.Write(WriteLoadingDiv("loadajaxspacelink","正在加载友情链接"));
}
else
{
	if(hidetitle == 0)
	{
        Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">友情链接</span></h2>");
	}
	Response.Write("<div class=\"dnt-statistic\">");
	if(__spacelinkinfos!=null)
	{
		foreach(SpaceLinkInfo __spacelinkinfo in __spacelinkinfos)
		{%>				
				<ul class="ItemContent">
					<li><a href="<%=__spacelinkinfo.LinkUrl%>" title="<%=__spacelinkinfo.Description%>" target="_blank"><% = __spacelinkinfo.LinkTitle.Length>20?__spacelinkinfo.LinkTitle.Substring(0,15)+"..":__spacelinkinfo.LinkTitle%></a></li>
				</ul>
		<%}
	}			
	Response.Write("</div>");
}%>	
