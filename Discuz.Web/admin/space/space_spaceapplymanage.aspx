<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Space.Admin.SpaceApplyManage" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>用户列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
function submitPassApply()
{
	if (typeof(Page_ClientValidate) == 'function') 
	{ 
		if (Page_ClientValidate() == false) 
		{ return false; }
	}
	this.disabled=true;success.style.display = 'block';
	HideOverSels('success');__doPostBack('PassApply','');
}
function submitDeleteApply()
{
	if (typeof(Page_ClientValidate) == 'function') 
	{ 
		if (Page_ClientValidate() == false) 
			{ return false; }
	}
	this.disabled=true;
	success.style.display = 'block';
	HideOverSels('success');
	__doPostBack('DeleteApply','');
}

function Check(form)
{
	CheckByName(form,'uid','no');
	checkedEnabledButton(form,'uid','PassApply','DeleteApply')
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" PageSize="20">
	<Columns>					
	<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='CheckByName(this.form)' type='checkbox' name='chkall' id='chkall' />">
		<HeaderStyle Width="20px" />
		<ItemTemplate>
			<input id="uid<%#DataBinder.Eval(Container, "DataItem.uid").ToString()%>" type="checkbox" onclick="checkedEnabledButton(this.form,'uid','PassApply','DeleteApply')" value="<%#DataBinder.Eval(Container, "DataItem.uid").ToString()%>" name="uid" />
		</ItemTemplate>
	</asp:TemplateColumn>
		<asp:BoundColumn DataField="spacetitle" HeaderText="个人空间名称"></asp:BoundColumn>
		<asp:BoundColumn DataField="username" HeaderText="申请人"></asp:BoundColumn>
		<asp:BoundColumn DataField="createdatetime" HeaderText="申请日期"></asp:BoundColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<a href="#" onclick="document.getElementById('uid<%#DataBinder.Eval(Container, "DataItem.uid").ToString()%>').checked=true;submitPassApply();">批准</a>
				<a href="#" onclick="document.getElementById('uid<%#DataBinder.Eval(Container, "DataItem.uid").ToString()%>').checked=true;submitDeleteApply();">删除</a>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
	<cc1:Button ID="PassApply" runat="server" Text=" 批 准 " Enabled="false"></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DeleteApply" runat="server" Text=" 删 除 "  ButtonImgUrl="../images/del.gif" Enabled="false"></cc1:Button>
</p>
</form>
<%=footer%>
</body>
</html>
