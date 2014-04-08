<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Page language="c#" Inherits="Discuz.Web.Admin.addiconfile" Codebehind="forum_addiconfile.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>添加表情</title>		
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">文件添加</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">显示顺序</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="displayorder" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Text="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">代码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="code" runat="server" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">显示顺序</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="displayorder" runat="server" RequiredFieldType="数据校验" CanBeNull="必填" Text="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button id="AddIncoInfo" runat="server" Text=" 提 交 "></cc1:Button></div></div>
</fieldset>
</form>
<%=footer%>
</body>
</html>