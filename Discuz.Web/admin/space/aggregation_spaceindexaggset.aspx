<%@ Page language="c#" Inherits="Discuz.Space.Admin.SpaceIndexAggset"%>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>CacheManage</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<meta http-equiv="X-UA-Compatible" content="IE=7" /> 
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/legendimg.jpg) no-repeat 6px 50%;">自动提取数据</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">提取最新空间评论条数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="newcommentcount" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最新空间评论时间间隔(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="newcommentcounttimeout" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="10" MaximumValue="300"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多评论日志条数</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox id="maxarticlecommentcount" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多评论日志时间间隔(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxarticlecommentcounttimeout" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="10" MaximumValue="300"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多访问日志条数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxarticleviewcount" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多访问日志时间间隔(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxarticleviewcounttimeout" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="10" MaximumValue="300"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多评论空间条数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxcommentcount" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多评论空间时间间隔(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxcommentcounttimeout" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="10" MaximumValue="300"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多访问空间条数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxspaceviewcount" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多访问空间时间间隔(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxspaceviewcounttimeout" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="10" MaximumValue="300"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最近更新的空间条数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="updatespacecount" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最近更新的空间时间间隔(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="updatespacetimeout" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="10" MaximumValue="300"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
<tbody style="display:none">		
<!--因前台显示已去掉所以在此隐藏-->
	<tr><td class="item_title" colspan="2">提取最多发帖数空间条数</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxpostarticlespacecount" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="0"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">提取最多发帖数空间时间间隔(分钟)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="maxpostarticlespacecounttimeout" runat="server" size="3" MaxLength="4" CanBeNull="必填" MinimumValue="10" MaximumValue="300"></cc1:TextBox>
		</td>
		<td class="vtop"></td>
	</tr>
</tbody>
</table>
<div class="Navbutton"><cc1:Button id="Btn_SaveInfo" runat="server" Text="  保存  " ButtonImgUrl="../images/submit.gif"></cc1:Button></div>					
</fieldset>
</div>
</form>
<%=footer%>
</body>
</html>