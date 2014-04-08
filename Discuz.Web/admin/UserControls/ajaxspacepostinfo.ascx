<%@ Control Language="c#" Inherits="Discuz.Space.Admin.AjaxSpacePostInfo" AutoEventWireup="false"%>
<br />

<div style="width:100%" align=center>
    <table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
        <tr>
		<td colspan="2">
		<table class="ntcplist" >
            <tr class="head">
              <td>&nbsp;&nbsp;候选日志列表
            </tr>
            <tr>
            <td>
            <table class="datalist" cellspacing="0" rules="all" border="1" id="Table1" style="border-collapse:collapse;">
                  <tr class="category">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">选择</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">标题</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">作者</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">发表日期</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">查看次数</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">评论次数</td>
                  </tr>
                  <%foreach(System.Data.DataRow dr in dt.Rows){ %>
                  <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'" style="cursor:hand;">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
			        <INPUT id="tid<%=dr["postid"] %>" onclick="javascript:insertElement('pid',this.value,$('title'+this.value).innerHTML,this.checked)" type="checkbox" value="<%=dr["postid"] %>" name="postid"></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;" align="left">
                    <a href="../../space/viewspacepost.aspx?postid=<%=dr["postid"] %>" target="_blank"><span id="title<%=dr["postid"] %>"><%=dr["title"].ToString().Trim() %></span></a>
                    </td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
                    <%if (dr["uid"].ToString() == "-1"){ %>
                        <%=dr["author"] %>
                    <%}else{ %>
                        <a href="../../userinfo.aspx?userid=<%=dr["uid"] %>" target="_blank"><%=dr["author"] %></a>
                    <%} %>
                    </td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["postdatetime"]%></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["views"]%></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["commentcount"]%></td>
                  </tr>
                  <%} %>
                  <tr>
	                <td align="left" valign="bottom" colspan="6" style="border-width:0px;"><%=pagelink %></td>
                  </tr>
              </table>
              </td>
            </tr>
          </table>
          </td>
          </tr>
          </table>
</div>
