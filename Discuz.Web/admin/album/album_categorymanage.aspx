<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Album.Admin.CategoryManage"%>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>相册分类管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />  
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function validate()
	{
		if(document.getElementById("albumcateTitle").value == "")
		{
			Message("相册分类名称不能为空！");
			document.getElementById("albumcateTitle").focus();
			return false;
		}
		
		if(document.getElementById("displayorder").value == "")
		{
			Message("显示顺序不能为空！");
			document.getElementById("displayorder").focus();
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
	
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'delid','DelRec')
	}		
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="form1" runat="server">
<cc1:DataGrid ID="DataGrid1" runat="server" PageSize="10" DataKeyField="Albumcateid"  IsFixConlumnControls="true"
TableHeaderName="相册分类管理" OnItemDataBound="DataGrid1_ItemDataBound" OnPageIndexChanged="DataGrid_PageIndexChanged">
<Columns>
<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
	<HeaderStyle Width="20px" />
	<ItemTemplate>
		<input id="delid" type="checkbox" onclick="checkedEnabledButton(this.form,'delid','DelRec')" value="<%#DataBinder.Eval(Container, "DataItem.albumcateid").ToString()%>" name="delid" />
		<%# DataGrid1.LoadSelectedCheckBox(DataBinder.Eval(Container, "DataItem.albumcateid").ToString())%>
	</ItemTemplate>
</asp:TemplateColumn>
<asp:BoundColumn DataField="Title" HeaderText="相册分类名称"></asp:BoundColumn>
<asp:BoundColumn DataField="displayorder" HeaderText="序号"></asp:BoundColumn>
<asp:BoundColumn DataField="description" HeaderText="描述"></asp:BoundColumn>
</Columns>
</cc1:DataGrid>
<p style="text-align:right;">
<cc1:Button ID="SubmitButton" runat="server" Text=" 保 存 " OnClick="SubmitButton_Click"></cc1:Button>&nbsp;&nbsp;
<cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif"  OnClick="DelRec_Click" Enabled="false" />&nbsp;&nbsp;
<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 新建相册分类 </button>
</p>
<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:450px;">
<div class="ManagerForm">
<fieldset>
<legend style="background: url(../images/icons/icon54.jpg) no-repeat 6px 50%;"><asp:Label ID="prompt" runat="server" Text="新增相册分类" /></legend>
<table cellspacing="0" cellpadding="4" width="100%" align="center">
	<tr>
		<td style="width: 80px;height:35px;">相册分类名称:</td>
		<td>
			<cc1:TextBox ID="albumcateTitle" runat="server" Size="44" RequiredFieldType="暂无校验" CanBeNull="可为空"></cc1:TextBox>
		</td>
	</tr>
	<tr>
		<td style="height:35px;">显示顺序:</td>
		<td>
			<cc1:TextBox ID="displayorder" runat="server" Size="10" Text="0" RequiredFieldType="数据校验" />
		</td>
	</tr>
	<tr>
		<td style="height:35px;">相册分类描述:</td>
		<td>
			<cc1:TextBox ID="description" runat="server" Rows="6" Cols="50" TextMode="MultiLine" RequiredFieldType="暂无校验" CanBeNull="可为空" />
		</td>
	</tr>
	<tr>
		<td align="center" colspan="2" style="height:35px;">
			<cc1:Button ID="Save" runat="server" Text=" 提 交 " OnClick="Save_Click"></cc1:Button>&nbsp;&nbsp;
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