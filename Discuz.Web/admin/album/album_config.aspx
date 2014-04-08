<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Album.Admin.AlbumConfig" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>相册配置</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
	function ShowHiddenOption(status)
	{
		$("ShowAlbumOption").style.display = status ? "block" : "none";
		$("ShowUserGroup").style.display = status ? "block" : "none";
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="form1" runat="server">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">相册配置</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">是否启用相册服务</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="EnableAlbum" runat="server">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tbody id="ShowAlbumOption" runat="server">
	<tr><td class="item_title" colspan="2">允许每个用户建立最大相册数上限</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxalbumcount" runat="server" RequiredFieldType="数据校验" Size="5"  MaxLength="4"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	</tbody>
</table>
</fieldset>
<div id="ShowUserGroup" runat="server">
<fieldset>
<legend style="background:url(../images/icons/icon55.jpg) no-repeat 6px 50%;">相册大小配置</legend>
	<table width="100%" id="groupphotosize" align="center" class="table1" cellspacing="0" cellPadding="4"  bgcolor="#C3C7D1" runat="server"></table>
</fieldset>
</div>
<div class="Navbutton"><cc1:Button id="SaveCombinationInfo" runat="server" Text=" 提 交 " OnClick="SaveCombinationInfo_Click"></cc1:Button></div>
</form>
</div>
<%=footer%>
</body>
</html>