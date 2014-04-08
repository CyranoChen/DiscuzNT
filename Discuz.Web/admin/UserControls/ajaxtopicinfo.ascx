<%@ Control Language="c#" Inherits="Discuz.Web.Admin.ajaxtopicinfo" Codebehind="ajaxtopicinfo.ascx.cs" AutoEventWireup="false"%>
<br />

<div style="width:100%" align=center>
        <table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
        <tr>
		<td colspan="2">
		<table class="ntcplist" >
            <tr class="head">
              <td>&nbsp;&nbsp;候选主题列表</td>
            </tr>
            <tr>
            <td>
	          <table class="datalist" cellspacing="0" rules="all" border="1" id="DataGrid1" style="border-collapse:collapse;">
                  <tr class="category">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">选择</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">帖子标题</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">作者</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">所属版块</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">发布日期</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">回复数</td>
                  </tr>
                  <%
                    foreach(System.Data.DataRow dr in dt.Rows)
                    {
                        if (dr["title"].ToString().Trim().Length > 30)
                        {
                            dr["title"] = dr["title"].ToString().Trim().Substring(0, 30) + "...";
                        }
                   %>
                  <tr class="datagridItem" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'" style="cursor:hand;">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
			        <input id="tid<%=dr["tid"] %>" onclick="javascript:insertElement('tid',this.value,$('title'+this.value).innerHTML,this.checked)" type="checkbox" value="<%=dr["tid"] %>" name="topicid"></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;" align="left"><a href="../../showtopic.aspx?topicid=<%=dr["tid"] %>" target="_blank"><span id="title<%=dr["tid"] %>"><%=dr["title"].ToString().Trim() %></span></a></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
                    <%if(dr["posterid"].ToString() == "-1"){ %>
                        <%=dr["poster"] %>
                    <%}else{ %>
                        <a href="../../userinfo.aspx?userid=<%=dr["posterid"] %>" target="_blank"><%=dr["poster"] %></a>
                    <%} %>
                    </td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><a href="../../showforum.aspx?forumid=<%=dr["fid"] %>" target="_blank"><%=dr["name"] %></a></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["postdatetime"] %></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["replies"] %></td>
                  </tr>
                  <%} %>
                  <tr class="datagridPager">
	                <td align="left" valign="bottom" colspan="6" style="border-width:0px;"><%=pagelink %></td>
                  </tr>
              </table></td>
            </tr>
          </TABLE>
        </td>
        </tr>
        </table>
</div>
