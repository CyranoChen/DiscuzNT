<%@ Page Language="c#" Inherits="Discuz.Web.Admin.detachtable" Codebehind="global_detachtable.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>分表管理</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function Check(form)
	{
		CheckAll(form);
		checkedEnabledButton(form,'id','StartFullIndex')
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
	<uc1:PageInfo id="info1" runat="server" Icon="information"
	Text="帖子分表可以提高查看帖子的速度,但频繁分表也会带来用户使用上的不便. 一般以30-50万条帖子后另建新的分表为宜. <br />添加新分表后, 新主题的帖子将添加到新分表"></uc1:PageInfo>
	<uc1:PageInfo id="info2" runat="server" Icon="information" Text=""></uc1:PageInfo>
	<cc1:DataGrid ID="DataGrid1" runat="server" OnCancelCommand="DataGrid_Cancel" OnEditCommand="DataGrid_Edit"
		OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" OnUpdateCommand="DataGrid_Update">
		<Columns>
			<asp:BoundColumn Visible="false" DataField="id" HeaderText="分表id"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
				<HeaderStyle Width="20px" />
				<ItemTemplate>
					<input id="id" type="checkbox" onclick="checkedEnabledButton(this.form,'id','StartFullIndex')" value="<%# DataBinder.Eval(Container, "DataItem.id").ToString() %>" name="id" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="分表名称">
				<ItemTemplate>
					<%=Discuz.Config.BaseConfigs.GetTablePrefix%>posts<%# DataBinder.Eval(Container, "DataItem.id").ToString()%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="description" HeaderText="分表描述"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="帖子表当前记录数">
				<ItemTemplate>
					<%# CurrentPostsCount(DataBinder.Eval(Container, "DataItem.id").ToString())%> 
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="主题TID">
				<ItemTemplate>
					<%# DisplayTid(DataBinder.Eval(Container, "DataItem.mintid").ToString(),DataBinder.Eval(Container, "DataItem.maxtid").ToString())%> 
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="createdatetime" ReadOnly="True" HeaderText="分表时间"></asp:BoundColumn>
		</Columns>
	</cc1:DataGrid>
	<p style="text-align:right;">
		<button type="button" class="ManagerButton" id="Button2" onclick="BOX_show('neworedit');"><img src="../images/add.gif"/> 新建分表 </button>&nbsp;&nbsp;
		<cc1:Button ID="StartFullIndex" runat="server" Text="为选择的分表填充全文索引" Enabled="false"></cc1:Button>
	</p>
	<div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
	<div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:350px;">
	<div class="ManagerForm">
		<fieldset>
			<legend style="background: url(../images/icons/icon10.jpg) no-repeat 6px 50%;">新建分表</legend>
			<table cellspacing="0" cellpadding="4" width="100%" align="center">
				<tr>
					<td style="width: 70px">分表描述:</td>
					<td>
						<cc1:TextBox ID="detachtabledescription" runat="server" HintInfo="长度少于50字" RequiredFieldType="暂无校验" Width="200px"></cc1:TextBox>
					</td>
				</tr>
				<tr>
					<td align="center" colspan="2"><br />
                        <input type="checkbox" id="createtype" name="createtype" />IIS是否为多进程(web园>1)的独立主机<br />
						<cc1:Button ID="SaveInfo" runat="server" Text=" 添 加 "></cc1:Button> &nbsp;&nbsp;
						<button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
					</td>
				</tr>
			</table>
		</fieldset>
	</div>
	</div>
</form>
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div id="setting" />
<%=footer%>
</body>
</html>