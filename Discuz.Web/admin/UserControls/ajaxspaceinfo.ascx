<%@ Control Language="c#" Inherits="Discuz.Space.Admin.AjaxSpaceInfo" AutoEventWireup="false"%>
<br />

<div style="width:100%" align=center>
        <table class="table1" cellspacing="0" cellpadding="4" width="100%" align="center">
        <tr>
		<td colspan="2">
		<table class="ntcplist" >
            <tr class="head">
              <td>&nbsp;&nbsp;候选空间列表</td>
            </tr>
            <tr>
            <td>
	          <table class="datalist" cellspacing="0" rules="all" border="1" id="DataGrid1" style="border-collapse:collapse;">
                  <tr class="category">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">选择</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">空间标题</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">空间描述</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">所有者</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">建立日期</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">日志数</td>
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">相册数</td>
                  </tr>
                  <%foreach(System.Data.DataRow dr in dt.Rows){ %>
                  <tr class="mouseoutstyle" onmouseover="this.className='mouseoverstyle'" onmouseout="this.className='mouseoutstyle'" style="cursor:hand;">
                    <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">
			        <input id="tid<%=dr["spaceid"] %>" onclick="javascript:insertElement('sid',this.value,$('title'+this.value).innerHTML,this.checked)" 
			        type="checkbox" value="<%=dr["spaceid"] %>" name="spaceid" /></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;" align="left">
                    <a href="../../space/?uid=<%=dr["userid"] %>" target="_blank"><span id="title<%=dr["spaceid"] %>"><%=dr["spacetitle"].ToString().Trim() %></span></a>
                    </td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["description"] %></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><a href="../../userinfo.aspx?userid=<%=dr["userid"] %>" target="_blank"><%=dr["username"] %></a></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["createdatetime"] %></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["postcount"] %></td>
                    <td style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><%=dr["albumcount"]%></td>
                  </tr>
                  <%} %>
                  <tr>
	                <td align="left" valign="bottom" colspan="7" style="border-width:0px;"><%=pagelink %></td>
                  </tr>
              </table>
              </td>
            </tr>
          </table>
          </td>
          </tr>
          </table>
</div>
