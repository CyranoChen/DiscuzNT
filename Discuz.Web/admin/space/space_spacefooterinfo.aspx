<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Space.Admin.SpaceFooterInfoPage" %>
<%@ Register TagPrefix="uc1" TagName="TextareaResize" Src="../UserControls/TextareaResize.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>spacefooterinfo</title>
<script language="javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet">
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet">
<script type="text/javascript" src="../js/modalpopup.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server" name="Form1">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">其它信息设置</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">空间页面底部相关显示信息</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="spacefooterinfo" runat="server" controlname="spacefooterinfo" cols="30" rows="10"></uc1:TextareaResize>
		</td>
		<td class="vtop">该内容将会显示在所有空间底部</td>
	</tr>
	<tr><td class="item_title" colspan="2">个人空间欢迎辞</td></tr>
	<tr>
		<td class="vtop rowform">
			<uc1:TextareaResize id="greeting" runat="server" controlname="greeting" cols="30" rows="10"></uc1:TextareaResize>
		</td>
		<td class="vtop">该内容将会在用户开通空间时作为第一篇日志发表,不发表请留空</td>
	</tr>
	<tr><td class="item_title" colspan="2">自动加入日志数量</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="topictoblog" runat="server" width="60" MinimumValue="0" MaximumValue="10"></cc1:TextBox>
		</td>
		<td class="vtop">开通空间后的自动加入日志论坛主题数量</td>
	</tr>
	<tr><td class="item_title" colspan="2">启用个人空间个性域名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList id="enablerewrite" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1">启用</asp:ListItem>
				<asp:ListItem Value="0" Selected="True">不启用</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop">允许用户以 http://论坛地址/space/个性域名 的方式访问个人空间.本功能需要 IIS 级别的URL Rewirte组件支持</td>
	</tr>
</table>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div class="Navbutton"><cc1:Button ID="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button></div></div>
</fieldset>
</form>
<%=footer%>
</body>
</html>