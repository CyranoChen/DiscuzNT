<%@ Page language="c#" AutoEventWireup="false" Inherits="Discuz.Space.Admin.SpaceManage" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>用户列表</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
function Check(form,checked)
{
CheckByName(form,'spaceid',checked);
checkedEnabledButton(form,'spaceid','CloseSpace','OpenSpace','DeleteSpace');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<asp:Panel id="searchtable" runat="server"  Visible="true">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">搜索个人空间</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Username" runat="server" RequiredFieldType="暂无校验" width="200"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">注册日期</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:Calendar id="joindateStart" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar>&nbsp;-&nbsp;
			<cc1:Calendar id="joindateEnd" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar>
		</td>
		<td class="vtop"></td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button ID="Search" runat="server" Text="开始搜索" designtimedragdrop="247"></cc1:Button></div></div>
</fieldset>
</div>
</asp:Panel>
<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged">
<Columns>
<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form,this.checked)' type='checkbox' name='chkall' id='chkall' />">
	<HeaderStyle Width="20px" />
	<ItemTemplate>
	   <input id="spaceid" type="checkbox" onclick="checkedEnabledButton(this.form,'spaceid','CloseSpace','OpenSpace','DeleteSpace')" value="<%#DataBinder.Eval(Container, "DataItem.spaceid").ToString()%>" name="spaceid" />							
	</ItemTemplate>
</asp:TemplateColumn>
	<asp:TemplateColumn HeaderText="空间名称">
		<ItemTemplate>
		<a href="../../space/?uid=<%#DataBinder.Eval(Container, "DataItem.userid").ToString()%>" target="_blank"><%#DataBinder.Eval(Container, "DataItem.spacetitle").ToString()%></a>
		</ItemTemplate>
	 </asp:TemplateColumn>
	<asp:TemplateColumn HeaderText="作者">
		<itemtemplate>
			<%# (DataBinder.Eval(Container, "DataItem.userid").ToString() != "-1") ? "<a href='../../userinfo.aspx?userid=" + DataBinder.Eval(Container, "DataItem.userid").ToString() + "' target='_blank'>" + DataBinder.Eval(Container, "DataItem.username").ToString() + "</a>" : DataBinder.Eval(Container, "DataItem.username").ToString()%>
		</itemtemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="grouptitle" HeaderText="所属组"></asp:BoundColumn>
	<asp:BoundColumn DataField="postcount" HeaderText="日志数"></asp:BoundColumn>
	<asp:BoundColumn DataField="commentcount" HeaderText="评论数"></asp:BoundColumn>
	<asp:BoundColumn DataField="createdatetime" HeaderText="注册时间" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
	<asp:BoundColumn DataField="tstatus" HeaderText="状态"></asp:BoundColumn>
</Columns>
</cc1:datagrid>
<p style="text-align:right;">
<cc1:Button ID="CloseSpace" runat="server" Text=" 关 闭 " Enabled="false"></cc1:Button>&nbsp;&nbsp;
<cc1:Button ID="OpenSpace" runat="server" Text=" 开 启 " Enabled="false"></cc1:Button>&nbsp;&nbsp;
<cc1:Button id="DeleteSpace" runat="server" Text=" 删 除 "  ButtonImgUrl="../images/del.gif" Enabled="false"></cc1:Button>
</p>
</form>
<%=footer%>
</body>
</html>