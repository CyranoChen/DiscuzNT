<%@ Page language="c#" Inherits="Discuz.Web.Admin.iconfilegrid" Codebehind="forum_iconfilegrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>表情列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'id','DelRec');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server"><br />
	<cc1:datagrid id="DataGrid1" runat="server" OnCancelCommand="DataGrid_Cancel" OnEditCommand="DataGrid_Edit"
		OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" OnUpdateCommand="DataGrid_Update"
		OnDeleteCommand="DataGrid_Delete">
		<Columns>				
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
					<input id="id" onclick="checkedEnabledButton(this.form,'id','DelRec')" type="checkbox" value="<%# DataBinder.Eval(Container, "DataItem.id").ToString() %>"	name="id" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="ID" SortExpression="id" HeaderText="表情id" Visible="false" ></asp:BoundColumn>					
			<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序"></asp:BoundColumn>
			<asp:BoundColumn DataField="type" SortExpression="type" HeaderText="类型" Visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="code" SortExpression="code" HeaderText="代码" ></asp:BoundColumn>
			<asp:BoundColumn DataField="url" SortExpression="url" HeaderText="文件名"></asp:BoundColumn>
			<asp:BoundColumn DataField="url" HeaderText="图片" readonly="true"></asp:BoundColumn>								
			<asp:TemplateColumn HeaderText="图片">
				<ItemTemplate>
					<asp:Label id=Label4 runat="server" Text='<%# PicStr(DataBinder.Eval(Container, "DataItem.url").ToString()) %>' />
				</ItemTemplate>
			</asp:TemplateColumn>			
		</Columns>
	</cc1:datagrid>
	<p style="text-align:right;">
		<input title="选中/取消选中 本页所有Case" onclick="document.getElementById('chkall').click()" type="checkbox" name="Checkbox1" id="Checkbox1" />全选/取消全选&nbsp;&nbsp;
		<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false"></cc1:Button> &nbsp;&nbsp;
		<button type="button" class="ManagerButton" onclick="javascript:window.location.href='forum_addiconfile.aspx';">
			<img src="../images/add.gif" /> 添加表情
		</button>
	</p>
</form>			
<%=footer%>
</body>
</html>