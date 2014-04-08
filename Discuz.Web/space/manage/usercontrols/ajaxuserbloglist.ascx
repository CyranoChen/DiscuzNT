<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxuserbloglist.ascx.cs" Inherits="Discuz.Space.Manage.ajaxbloglist" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Common" %>

<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
   Response.Write(base.WriteLoadingDiv("ajaxuserbloglist","正在加载日志列表"));
}
else
{   if(hidetitle == 0)
    {
        Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">我的日志</span></h2>");
	}
	
	if (errorinfo == "") 
	{ 
        Response.Write("<div class=\"ArticleList\">");
		if(__spacepostinfos !=null)
		{
            %>
                <script id="posttagscript" type="text/javascript">
                var postids = _gel('postsidlist').value.split(',');
                var cachefilepath = window.location.protocol + "//" + window.location.host + '<%=forumpath %>cache/spaceposttag/';
                try
                {
                    for (var i = 0; i < postids.length; i++)
                    {
                        eval(_gel("tags_" +postids[i]+"_script").innerHTML);
                    }
                }catch(e){};
                </script>
                <input type="hidden" value="<%=postsidlist %>" id="postsidlist" />
            <%
			foreach(SpacePostInfo __spacepostinfo in __spacepostinfos) 
			{
				if (__spacepostinfo.Postid>0)
				{%>					
			        <div class="SpaceArticle">
				        <div class="SpaceArticleTitle">
					        <div class="SpaceTime"><span class="TimeYear"><% = __spacepostinfo.Postdatetime.Year%></span><br><span class="TimeMonth"><% = __spacepostinfo.Postdatetime.ToString("MM-dd")%></span></div>
					        <div class="SpaceTitle"><a target="_blank" href="<%=configspaceurlnopage %>space/viewspacepost.aspx?postid=<%=__spacepostinfo.Postid%>&spaceid=<%=spaceid%>"><%=__spacepostinfo.Title%></a>  查看: <strong><a href="<%=configspaceurlnopage %>space/viewspacepost.aspx?postid=<%=__spacepostinfo.Postid%>&spaceid=<%=spaceid%>"><%=__spacepostinfo.Views%></a></strong> <a href="<%=configspaceurlnopage %>space/viewspacepost.aspx?postid=<%=__spacepostinfo.Postid%>&spaceid=<%=spaceid%>#comments"><span style="color:#">评论(<%=__spacepostinfo.Commentcount%>)</span></a>  <%= isAdmin ? "<strong><a href='viewspacepostlist.aspx?postid=" + __spacepostinfo.Postid + "&spaceid=" + spaceid + "&act=del'>删除</a></strong>" : ""%></div>
                        </div>
			        </div>
			        <div class="tagbox" id="tags_<%=__spacepostinfo.Postid %>"><script type="text/javascript" id="tags_<%=__spacepostinfo.Postid %>_script">
			            _DS_FetchContent(cachefilepath + Math.ceil(postids[i]/1000) + "/" + postids[i] + "_tags.txt", function showtag(json){  
                            var jsonArray;
                            try
                            {         
                               jsonArray = eval(json);
                            }catch(e){};
                            if (jsonArray)
                            {
                                var html = "标签: ";
                                for (var x = 0; x < jsonArray.length; x++)
                                {
                                    if (aspxrewrite == 1)
                                    {
                                        html += '<a href="<%=configspaceurlnopage %>spacetag-' + jsonArray[x].tagid + '.aspx">' + jsonArray[x].tagname + '</a>&nbsp;';
                                    }
                                    else
                                    {
                                        html += '<a href="<%=configspaceurlnopage %>tags.aspx?t=spacepost&tagid=' + jsonArray[x].tagid + '">' + jsonArray[x].tagname + '</a>&nbsp;';
                                    }
                                }
                                _gel("tags_<%=__spacepostinfo.Postid %>").innerHTML = _trim(html);
                            }
                        });</script>
			        </div>
			        <%
			        Response.Write("<div class=\"SpaceArticleContent\">");
			        switch (this.spaceconfiginfo.BlogDispMode)
                    {
                        case 0:
			            %>
				        <p><%=Utils.GetTextFromHTML(__spacepostinfo.Content).Length > 1000 ? Utils.GetTextFromHTML(__spacepostinfo.Content).Substring(0, 1000) + "..." : Utils.GetTextFromHTML(__spacepostinfo.Content)%></p>
				        <p><a href="<%=configspaceurlnopage %>space/viewspacepost.aspx?postid=<%=__spacepostinfo.Postid%>&spaceid=<%=spaceid%>">点击此处查看原文</a></p>
			            <%
                            break;
                        case 1:
			                 Response.Write("<p>"+ __spacepostinfo.Content +"</p>");
			                 break;
                        default:
                            break;
                    }
			        Response.Write("</div><br />");
		        }
			} //endforeach
			Response.Write(pagelink);
		}
        Response.Write("</div>");
	}
	else
	{
		Response.Write(errorinfo);
	}
}%>
