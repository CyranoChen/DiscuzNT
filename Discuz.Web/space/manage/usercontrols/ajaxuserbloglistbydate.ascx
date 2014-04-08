<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxuserbloglistbydate.ascx.cs" Inherits="Discuz.Space.Manage.ajaxuserbloglistbydate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Common" %>
<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
   Response.Write(base.WriteLoadingDiv("ajaxuserbloglistbydate","正在加载日志列表"));
}
else
{
    if(hidetitle == 0)
    {
        Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">我的日志</span></h2>");
	}
    if (errorinfo == "") 
	{ 
		if(__spacepostinfos !=null)
		{
			foreach(SpacePostInfo __spacepostinfo in __spacepostinfos) 
			{
                if (__spacepostinfo.Postid > 0)
				{%>
			    <div class="SpaceArticle">
				    <div class="SpaceArticleTitle">
					    <div class="SpaceTime"><span class="TimeYear"><% = __spacepostinfo.Postdatetime.Year%></span><br><span class="TimeMonth"><% = __spacepostinfo.Postdatetime.ToString("MM-dd")%></span></div>
					    <div class="SpaceTitle"><a href="viewspacepost.aspx?postid=<%=__spacepostinfo.Postid%>&spaceid=<%=spaceid%>"><%=__spacepostinfo.Title%></a>  查看: <strong><a href="viewspacepost.aspx?postid=<%=__spacepostinfo.Postid%>&spaceid=<%=spaceid%>"><%=__spacepostinfo.Views%></a></strong></span></div>
				    </div>
				    <div class="SpaceNumberBack"><%=__spacepostinfo.Commentcount%></div>
			    </div>
			    <div class="SpaceArticleContent">
				    <p><%=Utils.RemoveHtml(__spacepostinfo.Content.Length>1000?__spacepostinfo.Content.Substring(0,1000)+"...":__spacepostinfo.Content)%></p>
			    </div><br /><br />
				<%}
			}
			Response.Write(pagelink);
		}
	}
	else
	{
		Response.Write(errorinfo);
	}	
}%>
