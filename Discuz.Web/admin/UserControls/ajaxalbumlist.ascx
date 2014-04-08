<%@ Control Language="C#" AutoEventWireup="false" Inherits="Discuz.Album.Admin.AjaxAlbumList" %>
<br />
<div style="width:100%" align=center>
        <table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
        <tr>
		<td colspan="2">
		<table class="ntcplist" >
            <tr class="head">
              <td>&nbsp;&nbsp;候选相册列表</td>
            </tr>
            <tr>
            <td>
            <table class="datalist" cellspacing="0" id="Table1" style="border-collapse:collapse;">
                 <%
                 int i = 1;
                 foreach(System.Data.DataRow dr in dt.Rows)
                 {
                     if (i % 4 == 1)
                     {
                  %>
                        <tr style="cursor:hand;height:180px;">
                  <%
                     }
                  %>
                    <td nowrap="nowrap" align="center">
                    <%if (isShowPrivateAlbum)
                      { %>
                              <a href="#" onclick="managephoto(<%=dr["albumid"] %>)"><img src="<%=GetLogo(dr["logo"].ToString().Trim())%>" title="<%=dr["description"].ToString().Trim().Replace("\"","&quot;") %>" onerror="this.src='../../space/images/nopic.gif'" border="0"/></a><br />
                    <%}else{ %>
                              <a href="../../showalbum.aspx?albumid=<%=dr["albumid"] %>" target="_blank"><img src="<%=GetLogo(dr["logo"].ToString().Trim())%>" title="<%=dr["description"].ToString().Trim().Replace("\"","&quot;") %>" onerror="this.src='../../space/images/nopic.gif'" border="0"/></a><br />
                    <%} %>
			        <input id="tid<%=dr["albumid"] %>" onclick="javascript:insertElement('aid',this.value,$('albumtitle'+this.value).innerHTML,this.checked)" type="checkbox" value="<%=dr["albumid"] %>" name="albumid" />
			         <span id="albumtitle<%=dr["albumid"] %>">
			         <a href="../../showalbum.aspx?albumid=<%=dr["albumid"] %>" title="<%=dr["title"].ToString().Trim().Replace("\"","&quot;") %>" target="_blank">
			            <%=dr["title"].ToString().Trim().Length > 15 ? dr["title"].ToString().Trim().Substring(0,15) + "..." : dr["title"].ToString().Trim() %>
			        </a>
			        </span>(<%=dr["imgcount"]%>)<br />
                     <span> 创建日期:<%=Convert.ToDateTime(dr["createdatetime"].ToString()).ToShortDateString()%></span><br />
                    所有者:<a href="../../userinfo.aspx?userid=<%=dr["userid"] %>" target="_blank"><%=dr["username"] %></a><%=dr["type"].ToString()=="1"?"(<font color='red'>私有</font>)":""%>
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