<%@ Control Language="c#" Inherits="Discuz.Album.Admin.AjaxPhotoInfo" AutoEventWireup="false"%>
<%@ Import Namespace="Discuz.Config" %>
<br />
<div style="width:100%" align=center>
    <table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
        <tr>
		<td colspan="2">
		<table class="ntcplist" >
            <tr class="head">
              <td>&nbsp;&nbsp;候选照片列表</td>
            </tr>
            <tr>
            <td>
            <table class="datalist" cellspacing="0" id="Table2">
                  <%
                  int i = 1;
                  foreach (System.Data.DataRow dr in dt.Rows)
                  {
                     if (i % 4 == 1)
                     {
                  %>
                  <tr>
                  <%
                      }
                  %>
                    <td align="center" nowrap="nowrap">
                       <a href="../../showphoto.aspx?photoid=<%=dr["photoid"] %>" target="_blank"><img src="<%=GetThumbnail(dr["filename"].ToString().Trim())%>" alt="<%=dr["description"].ToString().Trim() %>" onerror="this.src='../../space/images/nopic.gif'" /></a><br />
			        <input id="tid<%=dr["photoid"] %>" onclick="javascript:insertElement('pid',this.value,$('phototitle'+this.value).innerHTML,this.checked)" type="checkbox" value="<%=dr["photoid"] %>" name="photoid" />
			        <span id="phototitle<%=dr["photoid"] %>"><a href="../../showphoto.aspx?photoid=<%=dr["photoid"] %>" target="_blank"><%=dr["title"].ToString().Trim()%></a></span>
			        <br />所有者:<a href="../../userinfo.aspx?userid=<%=dr["userid"] %>" target="_blank"><%=dr["username"] %></a>
			        </td>
                    <%
                      if (i % 4 == 0)
                      {
                     %>
                  </tr>
                  <%
                      }
                      i++;
                  } %>
                  <tr>
	                <td align="left" valign="bottom" colspan="4" style="border-width:0px;"><%=pagelink %></td>
                  </tr>
              </table>
              </td>
            </tr>
          </table>
          </td>
          </tr>
          </table>
</div>
