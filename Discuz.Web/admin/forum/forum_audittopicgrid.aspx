<%@ Page language="c#" Inherits="Discuz.Web.Admin.audittopicgrid" Codebehind="forum_audittopicgrid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxPostInfo" Src="../UserControls/AjaxPostInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>主题列表</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript">
function check(browser)
{ 
   document.forms[0].operation.value=browser;
}
</script>
<script type="text/javascript" src="../js/AjaxHelper.js" ></script>
<script type="text/javascript">	
function  LoadInfo(istopic,pid,tid)
{            	 
	 AjaxHelper.Updater('../UserControls/AjaxPostInfo','AjaxPostInfo','istopic='+istopic+'&pid='+pid+'&tid='+tid);
	 if(navigator.appName.indexOf("Explorer") > -1)
	 {
		 document.getElementById('PostInfo').style.display = "block";
	 }
	 else
	 {
		 document.getElementById('PostInfo').style.display = "block";
	 }
}

function Check(form)
{
	CheckAll(form);
	checkedEnabledButton(form,'tid','AudioSelectTopic','DeleteSelectTopic');
}
</script>	
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<cc1:datagrid id="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid">
<Columns>
   <asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
		<HeaderStyle Width="20px" />
		<ItemTemplate>
			<input id="tid" onclick="checkedEnabledButton(this.form,'tid','AudioSelectTopic','DeleteSelectTopic')" type="checkbox" value="<%# DataBinder.Eval(Container, "DataItem.tid").ToString() %>" name="tid" />
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="tid" SortExpression="tid"  HeaderText="帖子ID" Visible="false"></asp:BoundColumn>
	<asp:TemplateColumn HeaderText="标题">
		<ItemTemplate>
			 <a href="javascript:void(0);" onclick="javascript:LoadInfo('true','0','<%# DataBinder.Eval(Container, "DataItem.tid").ToString() %>');">
				<%# DataBinder.Eval(Container, "DataItem.title").ToString() %>
			 </a>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:TemplateColumn HeaderText="发帖人">
		<itemtemplate>
			<%# (DataBinder.Eval(Container, "DataItem.posterid").ToString() != "-1") ? "<a href='../../userinfo-" + DataBinder.Eval(Container, "DataItem.posterid").ToString() + ".aspx' target='_blank'>" + DataBinder.Eval(Container, "DataItem.poster").ToString() + "</a>" : DataBinder.Eval(Container, "DataItem.poster").ToString()%>
		</itemtemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="postdatetime" SortExpression="postdatetime" HeaderText="发布日期"></asp:BoundColumn>
	<asp:TemplateColumn HeaderText="最后回复人">
		<itemtemplate>
			<%# (DataBinder.Eval(Container, "DataItem.lastposterid").ToString() != "0") ? "<a href='../../userinfo-" + DataBinder.Eval(Container, "DataItem.lastposterid").ToString() + ".aspx' target='_blank'>" + DataBinder.Eval(Container, "DataItem.lastposter").ToString() + "</a>" : DataBinder.Eval(Container, "DataItem.lastposter").ToString()%>
		</itemtemplate>
	</asp:TemplateColumn>					
	<asp:TemplateColumn HeaderText="回帖数">
		<ItemTemplate>
			<%# GetPostLink(DataBinder.Eval(Container, "DataItem.tid").ToString(),DataBinder.Eval(Container, "DataItem.replies").ToString()) %>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn DataField="views" SortExpression="views" HeaderText="查看数"></asp:BoundColumn>
	<asp:BoundColumn DataField="digest" SortExpression="digest" HeaderText="精华帖"></asp:BoundColumn>
	<asp:BoundColumn DataField="displayorder" SortExpression="displayorder" HeaderText="显示顺序"></asp:BoundColumn>
	<asp:BoundColumn DataField="price" SortExpression="price" HeaderText="价格"></asp:BoundColumn>					
	<asp:TemplateColumn HeaderText="关闭">
		<ItemTemplate>
			<%# BoolStr(DataBinder.Eval(Container, "DataItem.closed").ToString()) %>
		</ItemTemplate>
	</asp:TemplateColumn>								
</Columns>
</cc1:datagrid>
<p style="text-align:right;">
<cc1:Button id="AudioSelectTopic" runat="server" Text="恢复选中的帖子" Enabled="false"></cc1:Button>&nbsp;&nbsp;
<cc1:Button id="AllAudioPass" runat="server" Text="全部恢复"></cc1:Button>&nbsp;&nbsp;
<cc1:Button id="DeleteSelectTopic" runat="server" Text="删除选中的主题" ButtonImgUrl="../images/del.gif" Enabled="false" OnClientClick="if(!confirm('你确认要删除所选主题吗？')) return false;"></cc1:Button>&nbsp;&nbsp;
<cc1:Button id="AllDelete" runat="server" Text="全部删除" ButtonImgUrl="../images/del.gif" OnClientClick="if(!confirm('你确认要删除所有主题吗？')) return false;"></cc1:Button>
</p>			
<div id="AjaxPostInfo" style="OVERFLOW-Y: auto;" valign="top">
<uc1:AjaxPostInfo id="AjaxPostInfo1" runat="server"></uc1:AjaxPostInfo>
</div>	
</form>
<%=footer%>
</body>
</html>