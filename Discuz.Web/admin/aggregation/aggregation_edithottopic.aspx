<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aggregation_edithottopic.aspx.cs" Inherits="Discuz.Web.Admin.edithottopic" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>论坛数据</title>
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />  
<script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" runat="server">
<div>
	<cc1:datagrid id="websiteconfig" runat="server" ColumnSpan="5">
		<Columns>
			<asp:TemplateColumn HeaderText=""><itemstyle width="8%" />
				<ItemTemplate>
					<a href="?tid=<%# DataBinder.Eval(Container, "DataItem.tid").ToString()%>">编辑</a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="title" HeaderText="标题"><itemstyle width="20%" /></asp:BoundColumn>
			<asp:BoundColumn DataField="poster" HeaderText="发帖人"><itemstyle width="20%" /></asp:BoundColumn>
			<asp:BoundColumn DataField="postdatetime" HeaderText="发帖时间"><itemstyle width="30%" /></asp:BoundColumn>						
		</Columns>
	</cc1:datagrid>
</div>
<p style="text-align:right;">
	<button class="ManagerButton" type="button" onclick="javascript:window.location.href='aggregation_forumhottopic.aspx';"><img src="../images/submit.gif" />重新推荐热帖</button>
</p>
<asp:Panel ID="panel1" runat="server" Visible="false">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">编辑帖子</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">标题</td></tr>
	<tr>
		<td class="vtop rowform">
			<input type="hidden" id="topicid" runat="server" />
			<cc1:TextBox id="title" runat="server" RequiredFieldType="暂无校验" width="200"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr style="display:none"><td class="item_title" colspan="2">发帖人</td></tr>
	<tr style="display:none">
		<td class="vtop rowform">
			<cc1:TextBox id="poster" runat="server" RequiredFieldType="暂无校验" width="200"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr style="display:none"><td class="item_title" colspan="2">发帖时间</td></tr>
	<tr style="display:none">
		<td class="vtop rowform">
			<cc1:TextBox id="postdatetime" runat="server" RequiredFieldType="暂无校验" width="200"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr style="display:none"><td class="item_title" colspan="2">内容</td></tr>
	<tr style="display:none">
		<td class="vtop rowform">
			<uc1:TextareaResize id="shortdescription" runat="server" controlname="shortdescription" Cols="60" Rows="5"></uc1:TextareaResize>
			<button class="TopicButton" type="button" onclick="document.getElementById('shortdescription_posttextarea').value=document.getElementById('fulldescription').value" style="width:250px;"><img src="../images/submit.gif" />读入帖子完整内容</button>
			<input type="hidden" id="fulldescription" runat="server" />
		</td>
		<td class="vtop">将帖子完整内容读入文本框, 您可自行修改内容长度</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button ID="savetopic" runat="server" Text="保存" designtimedragdrop="247" OnClick="savetopic_Click"></cc1:Button></div>
</fieldset>
</div>
</asp:Panel>
<cc1:Hint id="hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</form>
 <%=footer%>
</body>
</html>