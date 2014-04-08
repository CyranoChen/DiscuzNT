<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Space.Admin.ModuleDefManage" %>
<%@ Register Assembly="Discuz.Control" Namespace="Discuz.Control" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>模块管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function submitAddApply()
	{
		if (typeof(Page_ClientValidate) == 'function') 
		{ 
			if (Page_ClientValidate() == false) 
				{ return false; }
		}
		this.disabled=true;
		document.getElementById('success').style.display = 'block';
		HideOverSels('success');
		__doPostBack('AddApply','');
	}
	function submitDeleteApply()
	{
		if (typeof(Page_ClientValidate) == 'function') 
		{ 
			if (Page_ClientValidate() == false) 
				{ return false; }
		}
		if (confirm('将模块置为无效将导致用户正在使用的模块无法正常工作,确定要继续吗?'))
		{
			this.disabled=true;    			    
			document.getElementById('success').style.display = 'block';
			HideOverSels('success');
			__doPostBack('DeleteApply','');
		}
	}
	
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'url','AddApply','DeleteApply');
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<ul><li>如果要使用新的模块, 请将模块的xml配置文件上传至space/modules文件夹</li></ul>" />        
<cc1:datagrid id="DataGrid1" runat="server" PageSize="20">
<Columns>					
<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(form)' type='checkbox' name='chkall' id='chkall' />">
	<HeaderStyle Width="20px" />
	<ItemTemplate>
	   <input id="url<%#DataBinder.Eval(Container, "DataItem.url").ToString()%>" onclick="checkedEnabledButton(this.form,'url','AddApply','DeleteApply')" type="checkbox" value="<%#DataBinder.Eval(Container, "DataItem.url").ToString()%>" name="url" />						
	</ItemTemplate>
</asp:TemplateColumn>
	<asp:BoundColumn DataField="modulestatus" HeaderText="模块状态"></asp:BoundColumn>
	<asp:BoundColumn DataField="moduledefname" HeaderText="模块名称"></asp:BoundColumn>
	<asp:BoundColumn DataField="moduledefcatelog" HeaderText="所属分类"></asp:BoundColumn>
	<asp:TemplateColumn HeaderText="模块类型">
		<ItemTemplate>
			<%#DataBinder.Eval(Container, "DataItem.moduledeftype")%>	
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="moduledefop"></asp:BoundColumn>
</Columns>
<PagerStyle Mode="NumericPages" />
<SelectedItemStyle CssClass="datagridSelectedItem" />
<HeaderStyle CssClass="category" />
</cc1:datagrid>
<div class="imageexample">
	图例: <img title='模块当前无法正常工作' alt='模块当前无法正常工作' src='../images/state1.gif' />模块当前无法正常工作
		  <img title='模块可以正常工作' alt='模块可以正常工作' src='../images/state2.gif' />模块可以正常工作
		  <img title='尚未启用的模块' alt='尚未启用的模块' src='../images/state3.gif' />尚未启用的模块
</div>
<p style="text-align:right;">
	<cc1:Button ID="AddApply" runat="server" Text="置为有效" OnClick="AddApply_Click" Enabled="false"></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DeleteApply" runat="server" Text="置为无效" ButtonImgUrl="../images/invalidation.gif" OnClick="DeleteApply_Click" Enabled="false"></cc1:Button>
</p>
</form>
<%=footer%>
</body>
</html>