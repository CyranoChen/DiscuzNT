<%@ Page language="c#" Inherits="Discuz.Web.Admin.forumaggset" Codebehind="aggregation_forumaggset.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxTopicInfo" Src="../UserControls/AjaxTopicInfo.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>websitesetting</title>
<link href="../styles/gridStyle.css" type="text/css" rel="stylesheet" />
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />		
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/AjaxHelper.js"></script>
<script type="text/javascript" src="../js/calendar.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/draglist.js"></script>
<link href="../styles/draglist.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
	function validate(theform)
	{
		var idColl = $("dom0").getElementsByTagName("input");
		var idlist = "";
		for(i = 0 ; i < idColl.length ; i++)
		{
			if(idlist=="")
			{
			   idlist = idColl[i].value;
			}
			else
			{
			   idlist = idlist + "," + idColl[i].value;
			}
		}
		$("forumtopicstatus").value = idlist;
		return true;
	} 
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body >
<form id="Form1" runat="server">
<div class="ManagerForm">
<fieldset>		
<legend style="background:url(../images/legendimg.jpg) no-repeat 6px 50%;">聚合首页自动主题推荐设置</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">主题推荐条件</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:DropDownList ID="showtype" runat="server">
				<asp:ListItem Value="1">按浏览量显示</asp:ListItem>
				<asp:ListItem Value="2">按最后回复显示</asp:ListItem>
				<asp:ListItem Value="3">按最新主题显示</asp:ListItem>
				<asp:ListItem Value="4">按精华主题显示</asp:ListItem>
				<asp:ListItem Value="5">按回复数显示</asp:ListItem>
				<asp:ListItem Value="6">按评分数显示</asp:ListItem>
			</cc1:DropDownList>
		</td>
		<td class="vtop">当下方手工推荐主题数不超过2条时在聚合首页主题推荐区域会根据此条件自动显示主题列表</td>
	</tr>
	<tr style=" display:none;"><td class="item_title" colspan="2">显示主题条数</td></tr>
	<tr style=" display:none;">
		<td class="vtop rowform">
			 <cc1:TextBox id="topnumber" runat="server" Width="40" MaxLength="2" RequiredFieldType="数据校验"></cc1:TextBox>
		</td>
		<td class="vtop">当显示方式为"按版块列表显示"时, 则当前文本框数据无效!</td>
	</tr>
 </table>
<div class="Navbutton"><cc1:Button ID="SaveTopicDisplay" runat="server" Text=" 保存 " OnClick="SaveTopicDisplay_Click" /></div>
</fieldset>
<fieldset>
<legend style="background:url(../images/legendimg.jpg) no-repeat 6px 50%;">搜索帖子</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">所在表</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:DropDownList ID="tablelist" runat="server" />
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">所在论坛:</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:dropdowntreelist id="forumid" runat="server"></cc1:dropdowntreelist>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">原帖作者</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="poster" runat="server" Width="150px" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">多个用户名之间请用半角逗号"," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">标题关键字</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="title" runat="server" Width="150px" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">多关键字之间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">帖子发表时间范围</td></tr>
	<tr>
		<td class="vtop rowform">
			开始日期:<cc1:Calendar id="postdatetimeStart" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js" HintLeftOffSet="-20" HintHeight="0" ></cc1:Calendar><br />
            结束日期:<cc1:Calendar ID="postdatetimeEnd" runat="server" ReadOnly="True" ScriptPath="../js/calendar.js" HintLeftOffSet="-20" HintHeight="0"/>
		</td>
		<td class="vtop">格式 yyyy-mm-dd, 不限制请留空</td>
	</tr>
</table>
<div class="Navbutton">
    <cc1:Button id="SearchTopicAudit" runat="server" Text=" 搜索符合条件的帖子 "></cc1:Button>&nbsp;&nbsp;
    <button class="ManagerButton" type="reset"><img src="../images/submit.gif"> 重置条件 </button>
</div>
</fieldset>
</div>
<div id="topiclistgrid" style="width:98%;margin:0 auto;"><uc1:AjaxTopicInfo id="AjaxTopicInfo1" runat="server"></uc1:AjaxTopicInfo></div>
<br />
<span>&nbsp;&nbsp;<b>已选择主题列表</b></span>
<div class="content">
		<div class="left" id="dom0">
			<asp:Literal id="forumlist" runat="server"></asp:Literal>
		</div>
 </div>
 <br />
<div class="Navbutton">
	<cc1:Button id="SaveTopic" runat="server" Text=" 保存 "></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" onclick="window.location='aggregation_editforumaggset.aspx';"><img src="../images/submit.gif"> 维护上次保存列表 </button>
</div>
  <cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
  <input id="forumtopicstatus" type="hidden" runat="server" />
</form>
<%=footer%>
</body>
</html>