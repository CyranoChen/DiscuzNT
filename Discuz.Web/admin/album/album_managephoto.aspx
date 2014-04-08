<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Album.Admin.ManagePhoto" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxAlbumList" Src="../UserControls/ajaxalbumlist.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>照片列表</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/AjaxHelper.js"></script>
<script type="text/javascript" src="../js/calendar.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<div style="width:100%" align="center">
<table cellspacing="0" cellPadding="0" class="tabledatagrid">
	<tr>
	  <td colspan="12" class="tdheader">&nbsp;&nbsp;<b>照片列表</b></td>
	</tr>
	<tr>
	  <td class="tdcontent" >
	  <table class="datagridStyles" cellspacing="0" cellpadding="3" border="0" id="DataGrid1">
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
				<img src="<%=GetThumbnail(dr["filename"].ToString().Trim())%>" alt="<%=dr["description"].ToString().Trim() %>" onerror="this.src='../../space/images/nopic.gif'" /><br />
				<input id="photoid<%=dr["photoid"] %>" type="checkbox" value="<%=dr["photoid"] %>" name="photoid" />
				<span id="phototitle<%=dr["photoid"] %>"><%=dr["title"].ToString().Trim()%></span>
				<br />所有者:<%=dr["username"] %>
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
	  </table></td>
	</tr>
  </table>
</div>	
<p style="text-align:right;">
<cc1:Button id="DeleteApply" runat="server" Text=" 删 除 "  ButtonImgUrl="../images/del.gif" OnClick="DeleteApply_Click"></cc1:Button>&nbsp;
<button type="button" class="ManagerButton" onclick="history.back()"><img src="../images/arrow_undo.gif" /> 返 回 </button>
</p>
</form>
<%=footer%>
</body>
</html>