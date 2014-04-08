<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Space.Admin.SpaceThemeManage" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>主题管理</title>		
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/default.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function validate(form)
	{
		if(document.getElementById("themeName").value == "")
		{
				Message("主题分类名称不能为空！");
				document.getElementById("themeName").focus();
				return false;
		}
		return true;
	}
	
	function Message(m)
	{
		document.getElementById("success").style.display = 'none';
		document.getElementById("SubmitButton").disabled = false;
		alert(m);
	}
	
	function isNaNEx(str)
	{
		return !(/^\d+$/.test(str));
	}
	
	function checkFileList(form)
	{
		var i = 1;
		while(true)
		{
			if(form.elements["id" + i] == null)
				break;
			form.elements["id" + i].checked = form.cfile.checked;
			i++;
		}
	}
	
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'id','DelRec');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server" >
<cc1:datagrid id="themegrid" runat="server" IsFixConlumnControls="true">
<Columns>
	<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
		<HeaderStyle Width="20px" />
		<ItemTemplate>
			<input id="id" type="checkbox" onclick="checkedEnabledButton(this.form,'id','DelRec')" value="<%# DataBinder.Eval(Container, "DataItem.themeid").ToString() %>"	name="id"/>
			<%# themegrid.LoadSelectedCheckBox(DataBinder.Eval(Container, "DataItem.themeid").ToString())%>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="themeid" HeaderText="Id" Visible="false"></asp:BoundColumn>
	<asp:BoundColumn DataField="name" HeaderText="主题分类名称"></asp:BoundColumn>
	<asp:TemplateColumn HeaderText="操作">
		<ItemTemplate>
			<a href="space_spacethemegrid.aspx?themeid=<%# DataBinder.Eval(Container, "DataItem.themeid").ToString() %>">管理此主题分类</a>
		</ItemTemplate>
	</asp:TemplateColumn>		
</Columns>
</cc1:datagrid><br />
<p style="text-align:right;">
<cc1:Button id="SaveTheme" runat="server" Text=" 保存主题分类修改 "></cc1:Button>&nbsp;&nbsp;
<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" OnClick="DelRec_Click" Enabled="false"></cc1:Button>
</p>
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">新增主题分类</legend>
	<table class="datagridStyles" cellspacing="0" cellpadding="3" rules="rows" border="0" width="100%" id="smilesgrid">
		<tr>
		  <td style="width: 100px">主题分类名称:</td>
		  <td style="width: 200px"><cc1:TextBox id="themeName" runat="server" maxLength="50" /></td>
		  <td><cc1:Button id="SubmitButton" runat="server" Text=" 提 交 " ValidateForm="true"></cc1:Button></td>
		</tr>
	  </table>
</fieldset>
</div>
</form>	
<%=footer%>
</body>
</html>