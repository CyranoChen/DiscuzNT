<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxuseralbums.ascx.cs" Inherits="Discuz.Space.Manage.ajaxuseralbum" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import namespace="Discuz.Entity" %>
<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
   Response.Write(WriteLoadingDiv("loadajaxuseralbums","正在加载图片列表"));
}
else
{
    if(hidetitle == 0)
    {
		Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">我的" + config.Albumname +"</span></h2>");
	}
      
	if(__albums!=null)
	{
	    Response.Write("<div  class=\"modboxin\">");
		foreach(AlbumInfo __spacealbuminfo in __albums)
		{%>				
	    <div class="SpacePhotoList">
	        <div class="SpacePhotoImg"><a href="javascript:browsealbums(<%=__spacealbuminfo.Albumid%>);" ><img src="<%=forumPath%><%=__spacealbuminfo.Logo%>" alt="<%=__spacealbuminfo.Description%>" width="100" border="0" onerror="javascript:this.src='images/nopic.gif';"  /></a></div>
	        <div class="SpacePhotoIntro">
		        <h2><a href="javascript:browsealbums(<%=__spacealbuminfo.Albumid%>);"><%=__spacealbuminfo.Title%></a><strong>(<%=__spacealbuminfo.Imgcount%>张)</strong></h2>
		        <p><%=__spacealbuminfo.Description%></p>
		        <em><%=__spacealbuminfo.Createdatetime%> / <%=__spacealbuminfo.Views%>人查看</em>
		        <span><a href="javascript:browsealbums(<%=__spacealbuminfo.Albumid%>);">浏览</a> <a title="幻灯片播放" style="CURSOR: hand" href="javascript:void(0);" onclick="window.open('slidealbum.aspx?albumid=<%=__spacealbuminfo.Albumid%>','','fullscreen=yes')" >幻灯片</a>  
			    <%if(ispostauthor){
				    Response.Write("<a href=\"" + forumurlnopage +"/usercpspacemanagephoto.aspx?albumid="+ __spacealbuminfo.Albumid +"\" target=\"_blank\">管理</a>");
			    }%>
		        </span>
	        </div>
        </div>
        <%} //end foreach
		Response.Write("<br /><br />"+pagelink);
        Response.Write("</div>");
	}
	else
	{
		Response.Write(errorinfo);
	}
    Response.Write("</div>");
}%>
<script>
function browsealbums(albumid)
{
    window.open('showalbum.aspx?albumid='+albumid, '','');
}
</script>