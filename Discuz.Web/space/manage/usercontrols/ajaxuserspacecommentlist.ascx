<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxuserspacecommentlist.ascx.cs" Inherits="Discuz.Space.Manage.ajaxuserspacecommentlist" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import namespace="Discuz.Entity" %>

<%
if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
      Response.Write(base.WriteLoadingDiv("loadajaxuserpostcommentlist","正在加载评论列表"));
}
else
{
      Response.Write("<h2 class=\"modtitle\"><span class=\"modtitle_text\">相关评论</span></h2>\r\n");
      Response.Write("<div class=\"dnt-commentlist\">");
      string forumurl = "http://" + Discuz.Common.DNTRequest.GetCurrentFullHost() + Discuz.Config.BaseConfigs.GetForumPath.ToLower();

      if (errorinfo == "") 
      {
          if (__spacecommentinfos != null)
          {
              foreach (SpaceCommentInfo __spacecommentinfo in __spacecommentinfos)
              {
                    if (__spacecommentinfo.CommentID > 0)
                    {
                        Response.Write("<dt><a href=\"###\" onclick=\"document.getElementById('commentcontent').value='" + (__spacecommentinfo.Author != "" ? "@" + __spacecommentinfo.Author : "") + "';document.getElementById('replyuserid').value='" + __spacecommentinfo.Uid + "';\">回复</a>\r\n");
    			
			            if (ispostauthor || isadmin)
                        {
                            Response.Write("<a href=\"javascript:void(0);\" onclick=\"RefreshCommentCount(-1);AjaxHelper.Updater('usercontrols/ajaxuserspacecommentlist.ascx','usercommentlist', 'load=true&postid=" + postid + "&currentpage=" + currentpage + "&delcommentid=" + __spacecommentinfo.CommentID + "');\">删除</a>\r\n");
			            }
                            
                        if (__spacecommentinfo.Uid != -1)
                        {
                            if (config.Forumurl.StartsWith("http://"))
                            {
                                if (config.Aspxrewrite == 1)
                                {
                                    Response.Write("<a href=\"" + forumurl + "userinfo-" + __spacecommentinfo.Uid + config.Extname + "\" target=\"_blank\">" + __spacecommentinfo.Author + "</a>\r\n");
                                }
                                else
                                {
                                    Response.Write("<a href=\"" + forumurl + "userinfo.aspx?userid=" + __spacecommentinfo.Uid + "\" target=\"_blank\">" + __spacecommentinfo.Author + "</a>\r\n");
                                }
                            }
                            else
                            {
                                if (config.Aspxrewrite == 1)
                                {
                                    Response.Write("<a href=\"../userinfo-" + __spacecommentinfo.Uid + config.Extname + "\" target=\"_blank\">" + __spacecommentinfo.Author + "</a>\r\n");
                                }
                                else
                                {
                                    Response.Write("<a href=\"../userinfo.aspx?userid=" + __spacecommentinfo.Uid + "\" target=\"_blank\">" + __spacecommentinfo.Author + "</a>\r\n");
                                }
                            }
                        }
                        else if (__spacecommentinfo.Url != string.Empty && __spacecommentinfo.Url.StartsWith("http://"))
                        {
                            Response.Write("<a href=\"" + __spacecommentinfo.Url + "\" target=\"_blank\">" + __spacecommentinfo.Author + "</a>\r\n");
                        }
                        else
                        {
                            Response.Write(__spacecommentinfo.Author);
                        }

                        Response.Write("<span class=\"NtSpace-smalltxt\"> &nbsp; / &nbsp; " + __spacecommentinfo.PostDateTime + "</span></dt>\r\n");

                        Response.Write(Discuz.Common.Utils.HtmlEncode(__spacecommentinfo.Content).Replace("\r\n", "<BR />").Replace("\n", "<BR />") + "<br /><br />\r\n"); 
		            }
               } //foreach end
               Response.Write(pagelink);
         }
         else
         {
             Response.Write("<dt style='padding-left: 5px;margin-left : 5px;'>(暂无评论)</dt>\r\n");
         }
     }
	 else
	 {
		 Response.Write(errorinfo);
	 }
    
     Response.Write("</div>\r\n");	
}
%>

