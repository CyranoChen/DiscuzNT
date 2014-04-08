<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Space.Admin.SpaceApplySetting" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>spaceapplysetting</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript">
function ChanageUserGroupStatus(status)
{
	var i = 0;
	while(true)
	{
		var obj = $("UserGroup_" + i);
		if(obj == null) break;
		obj.disabled = !status;
		obj.checked = status;
		i++;
	} 
}
function ShowHiddenOption(status)
{
	$("ShowSpaceOption").style.display = status ? "block" : "none";
	$("ShowUserGroup").style.display = status ? "block" : "none";
}		    
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="只有选择开通个人空间并选中选项名称前的复选框，该项才会发挥作用" />
<asp:Panel id="searchtable" runat="server" Visible="true">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">申请设置</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">是否启用个人空间服务</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="EnableSpace" runat="server">
				<asp:ListItem Value="1" Selected="True">是</asp:ListItem>
				<asp:ListItem Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tbody id="ShowSpaceOption" runat="server">
	<tr><td class="item_title" colspan="2"><asp:CheckBox id="allowPostcount" runat="server" />论坛发帖数超过</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Postcount" runat="server" width="60" RequiredFieldType="数据校验"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2"><asp:CheckBox id="allowUserGroup" runat="server" />属于用户组</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:CheckBoxList id="UserGroup" Runat="server"  RepeatColumns="2"/>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2"><asp:CheckBox id="allowDigestcount" runat="server" />论坛精华帖数超过</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Digestcount" runat="server" width="60" RequiredFieldType="数据校验"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr> 
	<tr><td class="item_title" colspan="2"><asp:CheckBox id="allowScore" runat="server" />论坛用户积分超过</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Score" runat="server" width="60" RequiredFieldType="数据校验"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">达到以上条件后申请是否自动开通?</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="ActiveType" runat="server">
				<asp:ListItem Value="1" Selected="True">自动开通</asp:ListItem>
				<asp:ListItem Value="0">手动审核</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	</tbody>
</table>
</fieldset>
<div id="ShowUserGroup" runat="server">
<fieldset>
	<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">空间附件最大空间配置</legend>
	<table cellspacing="0" cellpadding="4" width="100%" align="center"  id="groupattachsize" runat="server"></table>
</fieldset>
</div>
<div class="Navbutton"><cc1:Button id="Submit" runat="server" designtimedragdrop="247" Text="提 交" OnClick="Submit_Click"></cc1:Button></div>
</div>
</asp:Panel>
</form>
<script type="text/javascript">
if(document.getElementById("<%=allowUserGroup.ClientID%>").checked==false)
	ChanageUserGroupStatus(false);
</script>
<%=footer%>
</body>
</html>