<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Space.Admin.EditModuleDef" %>
<%@ Register TagPrefix="cc2" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>编辑模块</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/tabstrip.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">模块编辑</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">模块名称</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc2:TextBox id="modulename" runat="server" CanBeNull="必填"  IsReplaceInvertedComma="false" size="20"  MaxLength="20"></cc2:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">模块类型</td></tr>
	<tr>
		<td class="vtop rowform">
			 <asp:literal id="moduletype" runat="server"></asp:literal>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">所属分类</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc2:DropDownList ID="category" runat="server"></cc2:DropDownList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">配置文件</td></tr>
	<tr>
		<td class="vtop rowform">
			  <asp:literal id="configfile" runat="server"></asp:literal>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton">
	<cc2:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />&nbsp;&nbsp;
	<button type="button" class="ManagerButton" id="Button3" onclick="window.location='space_moduledefmanage.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</div>
</fieldset>
</div>
</form>	
<%=footer%>
</body>
</html>