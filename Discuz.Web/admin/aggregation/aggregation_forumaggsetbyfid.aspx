<%@ Page language="c#" Inherits="Discuz.Web.Admin.forumaggsetbyfid" Codebehind="aggregation_forumaggsetbyfid.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxTopicInfo" Src="../UserControls/AjaxTopicInfo.ascx" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
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
<legend style="background:url(../images/legendimg.jpg) no-repeat 6px 50%;">搜索帖子</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">所在表</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:DropDownList ID="tablelist" runat="server" />
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
			开始日期:<cc1:Calendar id="postdatetimeStart" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js" HintLeftOffSet="-20" HintHeight="0"></cc1:Calendar><br />
            结束日期:<cc1:Calendar ID="postdatetimeEnd" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js" HintLeftOffSet="-20" HintHeight="0"/>
		</td>
		<td class="vtop">格式 yyyy-mm-dd, 不限制请留空</td>
	</tr>
</table>
<div class="Navbutton"><cc1:Button id="SearchTopicAudit" runat="server" Text=" 搜索符合条件的帖子 "></cc1:Button></div>
</fieldset>
</div>
<div id="topiclistgrid" style="width:98%;margin:0 auto;"><uc1:AjaxTopicInfo id="AjaxTopicInfo1" runat="server"></uc1:AjaxTopicInfo></div>
<br />
<uc1:PageInfo ID="info1" runat="server" Icon="information" Text="<ul><li>上下拖动主题可以对主题进行排序 </li><li>将主题拖离已选择主题列表区，则将主题从列表中删除</li><li>点击保存按钮确认保存</li></ul>" />
<span>&nbsp;&nbsp;<b>已选择主题列表</b></span>
<div class="content">
		<div class="left" id="dom0">
			<asp:Literal id="forumlist" runat="server"></asp:Literal>
		</div>
 </div>
 <br />
<div class="Navbutton">
	<cc1:Button id="SaveTopic" runat="server" Text=" 保存 "></cc1:Button>&nbsp;&nbsp;
	<button type="button" class="ManagerButton" onclick="window.location='aggregation_editforumaggset.aspx?fid=<%=fid%>';"><img src="../images/submit.gif"> 维护上次保存列表 </button>
	<button class="ManagerButton" type="button" onclick="javascript:window.location.href='aggregation_recommendtopic.aspx';"><img src="../images/submit.gif" />返回</button>
</div>
  <cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
  <input id="forumtopicstatus" type="hidden" runat="server" />
</form>
<%=footer%>
</body>
</html>