<%@ Page language="c#" Inherits="Discuz.Web.Admin.bbcodegrid" Codebehind="forum_bbcodegrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>Discuz!NT代码</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'id','DelRec','SetAvailable','SetUnAvailable')
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<uc1:PageInfo id="info1" runat="server" Icon="Information"
	 Text="<ul><li>自定义 Discuz!NT 代码只在帖子和签名中生效, 其他位置(如短消息内容)中无效. </ul><ul><li>只有在用户组权限设定中, 将&quot;允许使用自定义 Discuz!NT 代码&quot;打开时, 该组用户所发表的包含自定义 Discuz!NT 代码的内容才会被解析. </ul><ul><li>出于安全考虑, 如果您启用了 [flash]、[rm]、[wmv] 等不安全代码, 请务必在用户组中设定只有内部人员组可以使用, 以免带来可能的安全问题. </ul><ul><li>只有在自定义 Discuz!NT 代码可用并设置了图标文件时, 具有&quot;允许使用自定义 Discuz!NT 代码&quot;权限的用户组在发帖时才会看到相应自定义 Discuz!NT 代码按钮. </ul>"></uc1:PageInfo>
<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" >
	<Columns>
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" type="checkbox" onclick="checkedEnabledButton(this.form,'id','DelRec','SetAvailable','SetUnAvailable')" value="<%# DataBinder.Eval(Container, "DataItem.id").ToString() %>"	name="id" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="">
			<ItemTemplate>
				<a href="forum_editbbcode.aspx?id=<%# DataBinder.Eval(Container, "DataItem.id").ToString()%>">编辑</a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="ID" SortExpression="id" HeaderText="id [递增]" Visible="false" ></asp:BoundColumn>
		<asp:BoundColumn DataField="tag"  SortExpression="tag" HeaderText="标签" Visible="true"></asp:BoundColumn>
		<asp:BoundColumn DataField="icon" SortExpression="icon"  HeaderText="图标文件" ></asp:BoundColumn>
		<asp:BoundColumn DataField="icon" HeaderText="图标"></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="图片">
			<ItemTemplate>
			   <img src="../../editor/images/<%# DataBinder.Eval(Container, "DataItem.icon").ToString() %>" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="可用">
			<ItemTemplate>
			<%# BoolStr(DataBinder.Eval(Container, "DataItem.available").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>	
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
	<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选的Discuz!NT代码吗？')) return false;"></cc1:Button> &nbsp;&nbsp; 
	<cc1:Button id="SetAvailable" runat="server" Text="置为有效" Enabled="false"></cc1:Button> &nbsp;&nbsp; 
	<cc1:Button id="SetUnAvailable" runat="server" Text="置为无效" ButtonImgUrl="../images/invalidation.gif" Enabled="false"></cc1:Button> &nbsp;&nbsp; 
	<button type="button" class="ManagerButton" onclick="javascript:window.location.href='forum_addbbcode.aspx';"><img src="../images/add.gif" /> 添加Discuz!NT代码</button>
</p>
</form>
<%=footer%>
</body>
</html>