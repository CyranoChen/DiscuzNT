<%@ Page language="c#" Inherits="Discuz.Web.Admin.addmedal" Codebehind="global_addmedal.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>ѫ�����</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">ѫ�����</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">����</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="name" runat="server" RequiredFieldType="����У��" CanBeNull="����" Width="80%"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">�Ƿ���Ч</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="available" runat="server">
				<asp:ListItem Value="1" Selected="True">��Ч</asp:ListItem>
				<asp:ListItem Value="0">��Ч</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">ѫ��ͼƬ�ϴ�</td></tr>
	<tr>
		<td class="vtop" colspan="2">
			<cc1:UpFile id="image" runat="server" UpFilePath="../../images/medals" FileType=".jpg|.gif|.png" ShowPostDiv="false"></cc1:UpFile>
		</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button id="AddMedalInfo" runat="server" Text=" �� �� " OnClick="AddMedalInfo_Click"></cc1:Button></div>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>