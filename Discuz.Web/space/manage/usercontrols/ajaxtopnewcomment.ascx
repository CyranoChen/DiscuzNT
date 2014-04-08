<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxtopnewcomment.ascx.cs" Inherits="Discuz.Space.Manage.ajaxtopnewcomment" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import namespace="Discuz.Entity" %>
<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
   Response.Write(WriteLoadingDiv("loadajaxtopnewcomment","正在加载最新评论"));
}
else
{
	if(hidetitle == 0)
	{
        Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">最新评论</span></h2>");
	}	
	Response.Write("<div class=\"dnt-newcomment\">");
	if(__spacecommentinfos!=null)
	{
		foreach(SpaceCommentInfo __spacecommmentinfo in __spacecommentinfos)
		{%>				
			<ul class="ItemContent">
				<li><a href="<%=forumpath %>space/viewspacepost.aspx?postid=<%=__spacecommmentinfo.PostID%>&spaceid=<%=spaceid%>" title="<%=__spacecommmentinfo.Author%>" target="_blank"><%=__spacecommmentinfo.Content.Length>20?__spacecommmentinfo.Content.Substring(0,15)+"..":__spacecommmentinfo.Content%></a></li>
			</ul>
		<%}
	}		
	Response.Write("</div>");
}%>	
