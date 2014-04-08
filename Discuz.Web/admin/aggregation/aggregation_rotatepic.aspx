<%@ Page language="c#" Codebehind="aggregation_rotatepic.aspx.cs" Inherits="Discuz.Web.Admin.aggregation_rotatepic" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>聚合轮换图片管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'rowid','DelRec');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">		
<cc1:datagrid id="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="rowid" onclick="checkedEnabledButton(this.form,'rowid','DelRec')" type="checkbox" value="<%# DataBinder.Eval(Container, "DataItem.rowid").ToString() %>" name="rowid" />
				<%# DataGrid1.LoadSelectedCheckBox(DataBinder.Eval(Container, "DataItem.rowid").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="rotatepicid" HeaderText="序号[递增]"></asp:BoundColumn>
		<asp:BoundColumn DataField="img"  HeaderText="图片路径"></asp:BoundColumn>
		<asp:BoundColumn DataField="url" HeaderText="点击链接"></asp:BoundColumn>
		<asp:BoundColumn DataField="titlecontent"  HeaderText="说明文字"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="图片"><HeaderStyle Width="10%" />
			<ItemTemplate>
				<img src ="<%# DataBinder.Eval(Container, "DataItem.img").ToString() %>" height="25" width="25"  border="0px" />
			</ItemTemplate>
		</asp:TemplateColumn>
	
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
    <a target="_blank" href="../global/thumbnailremoteimage.aspx?w=320&h=240">缩略图生成工具</a>
	<cc1:Button id="SaveRotatepic" runat="server" Text="保存轮换图片修改"></cc1:Button>&nbsp;&nbsp;
	<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选图片吗？')) return false;"></cc1:Button>
</p>
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/icon4.jpg) no-repeat 6px 50%;">添加聚合轮换图片</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">图片路径</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox runat="server" ID="rotaimg" />
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">点击链接</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox runat="server" ID="url" />
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">说明文字</td></tr>
	<tr>
		<td class="vtop rowform">
			  <cc1:TextBox runat="server" ID="titlecontent" />
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton">
	<cc1:Button id="addrota" runat="server" Text=" 增 加 " ButtonImgUrl="../images/add.gif"></cc1:Button>
</div>
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>