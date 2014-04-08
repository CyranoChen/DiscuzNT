<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxviewuserpost.ascx.cs" Inherits="Discuz.Space.Manage.ajaxviewuserpost" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="Discuz.Space" %>

<div class="dnt-userpost">
<%
if (this.Showasajax)
{    
    if (Discuz.Common.DNTRequest.GetString("load") != "true")
    {
        Response.Write(WriteLoadingDiv("loadajaxviewuserpost", "正在加载帖子内容信息"));
    }
    else
    {
        if (errorinfo == "")
        {%>
        <h2 class="modtitle">
		    <div id="m_3_h" class="moduletitle">
            <span id="m_3_title" class="modtitle_text">&nbsp;</span>
            </div>
        </h2>
		<div  class="modboxin">
			<div class="SpaceArticle">
				<div class="SpaceArticleTitle">
					<div class="SpaceTime"><span class="TimeYear"><% = __spacepostsinfo.Postdatetime.Year%></span><br><span class="TimeMonth"><% = __spacepostsinfo.Postdatetime.ToString("MM-dd")%></span></div>
					<div class="SpaceTitle"><% = __spacepostsinfo.Title%><br/><span><a href="../favorites.aspx?postid=<%=postid%>&spaceid=<%=spaceid%>">收藏</a> | 分类: <% = categorylink%> |  查看: <strong><% = __spacepostsinfo.Views%></strong> | <a href="#comments">评论(<strong><% = __spacepostsinfo.Commentcount%></strong>)</a> <%= isAdmin ? "| <a href='viewspacepost.aspx?postid=" + postid + "&spaceid=" + spaceid + "&act=del'>删除</a>" : ""%></span>
					</div>					
				</div>
			</div>
			<div class="tagbox" id="tags"><script type="text/javascript" id="tags_script">
	            _DS_FetchContent(cachefilepath + Math.ceil(<%=postid%>/1000) + "/<%=postid%>_tags.txt", function showtag(json){  
                    var jsonArray;
                    try
                    {         
                       jsonArray = eval(json);
                    }catch(e){
                        _DS_FetchContent(window.location.protocol + "//" + window.location.host + '<%=forumpath %>tools/ajax.aspx?t=getspaceposttags&postid=<%=postid%>', function showtag(json){
                            try{
                                jsonArray = eval(json);
                            }catch(e){}                                    
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
                                _gel("tags").innerHTML = _trim(html);
                            }                                
                        });
                    };
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
                        _gel("tags").innerHTML = _trim(html);
                    }
                });</script>
	        </div>
			<div class="SpaceArticleContent"><p><% = __spacepostsinfo.Content%></p></div>
        </div>			
	    <%}
        else
        {
            Response.Write(errorinfo);
	    }
    }
}
else
{ 
    if (errorinfo == "")
    {%>
		<h2 class="modtitle">
		    <div id="m_3_h" class="moduletitle">
                <span id="m_3_title" class="modtitle_text">&nbsp;</span>
            </div>
        </h2>
		<div  class="modboxin">
			<div class="SpaceArticle">
				<div class="SpaceArticleTitle">
					<div class="SpaceTime"><span class="TimeYear"><%=__spacepostsinfo.Postdatetime.Year%></span><br><span class="TimeMonth"><% = __spacepostsinfo.Postdatetime.ToString("MM-dd")%></span></div>
					<div class="SpaceTitle"><% = __spacepostsinfo.Title%><br><span>分类: <% = categorylink%> |  查看: <strong><% = __spacepostsinfo.Views%></strong> | <a href="#comments">评论(<strong><% = __spacepostsinfo.Commentcount%></strong>)</a> <%= isAdmin ? "| <a href='viewspacepost.aspx?postid=" + postid + "&spaceid=" + spaceid + "&act=del'>删除</a>" : ""%></span>
					    <script type="text/javascript" src="<%=SpaceTags.GetSpacePostTagCacheFilePath(postid) %>"></script>
					</div>
				</div>
			</div>
			<div class="SpaceArticleContent"><p><%=__spacepostsinfo.Content%></p></div>
        </div>			
     <%}
     else
     {
		   Response.Write(errorinfo);
	 }
}        
%>
</div>
<script id="posttagscript" type="text/javascript">
function parsetag()
{
var cachefilepath = window.location.protocol + "//" + window.location.host + '<%=forumpath %>cache/spaceposttag/';
try
{
    eval(_gel("tags_script").innerHTML);
}catch(e){};
}
</script>