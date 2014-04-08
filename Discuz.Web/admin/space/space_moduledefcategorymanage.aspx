<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Space.Admin.ModuleDefCategoryManage" %>
<%@ Register Assembly="Discuz.Control" Namespace="Discuz.Control" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>模块分类管理</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script> 
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<cc1:datagrid id="DataGrid1" runat="server" PageSize="20" OnCancelCommand="DataGrid1_CancelCommand" OnDeleteCommand="DataGrid1_DeleteCommand" OnEditCommand="DataGrid1_EditCommand" OnUpdateCommand="DataGrid1_UpdateCommand">
	<Columns>			    
		<asp:BoundColumn DataField="categoryname" HeaderText="分类名称"></asp:BoundColumn>
	</Columns>
	<PagerStyle Mode="NumericPages" />
	<SelectedItemStyle CssClass="datagridSelectedItem" />
	<HeaderStyle CssClass="category" />
	</cc1:datagrid><br />
<p style="text-align:right;">
<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 新建分类 </button>
</p>
<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:350px;">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">新建分类</legend>
<table width="100%">
	<tr>
		<td style="width: 90px;height:35px">分类名称:</td>
		<td>
			<cc1:TextBox ID="newcategoryname" runat="server"></cc1:TextBox>
		 </td>
	</tr>
	<tr>
		<td align="center" colspan="2" style="height:35px">
			<cc1:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" />&nbsp;&nbsp;
			<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
		</td>
	</tr>
</table>
</fieldset>
</div>
</div>
</form>
<div id="setting" />
<%=footer%>
</body>
</html>