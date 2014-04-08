<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxtopnewpost.ascx.cs" Inherits="Discuz.Space.Manage.ajaxtopnewpost" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import namespace="Discuz.Entity" %>
<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
    Response.Write(WriteLoadingDiv("loadajaxtopnewpost", "正在加载最新日志列表"));
}
else
{
    if(hidetitle == 0)
    {
        Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">最新日志列表</span></h2>");
	}
    Response.Write("<div class=\"dnt-newpost\"><ul class=\"ItemContent\">");
	if(__spacepostinfos!=null)
	{
		foreach(SpacePostInfo __spacepostinfo in __spacepostinfos)
		{%>				
			<li><a href="viewspacepost.aspx?postid=<%=__spacepostinfo.Postid%>&spaceid=<%=spaceid%>" title="<%=__spacepostinfo.Title%>" target="_blank"><% = __spacepostinfo.Title.Length>20?__spacepostinfo.Title.Substring(0,15)+"..":__spacepostinfo.Title%></a></li>
		<%}
	}
    Response.Write("</ul></div>");
}%>	
