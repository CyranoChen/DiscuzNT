<%@ Page language="c#" Inherits="Discuz.Space.Admin.SpaceAggset"%>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="AjaxSpaceInfo" Src="../UserControls/ajaxspaceinfo.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>websitesetting</title>		
<link href="../styles/calendar.css" type="text/css" rel="stylesheet" />
<link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />	
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />	
<link href="../styles/draglist.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript" src="../js/AjaxHelper.js"></script>
<script type="text/javascript" src="../js/draglist.js"></script>
<script type="text/javascript">
function validate(theform)
{
	var sidColl = $("dom0").getElementsByTagName("input");
	var sidlist = "";
	for(i = 0 ; i < sidColl.length ; i++)
	{
		if(sidlist=="")
		{
		   sidlist = sidColl[i].value;
		}
		else
		{
		   sidlist = sidlist + "," + sidColl[i].value;
		}
	}
			   
	$("spacestatus").value = sidlist;
	return true;
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" runat="server">
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/legendimg.jpg) no-repeat 6px 50%;">搜索个人空间</legend>
<table width="100%">
	<tr><td class="item_title" colspan="2">空间所有者</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="poster" runat="server" Width="150px" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">多个用户名之间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">日志关键字</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="title" runat="server" HintTitle="提示" Width="150px" RequiredFieldType="暂无校验"></cc1:TextBox>
		</td>
		<td class="vtop">多关键字之间请用半角逗号 "," 分割</td>
	</tr>
	<tr><td class="item_title" colspan="2">日志发表时间范围</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:Calendar id="postdatetimeStart" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar>&nbsp;-
            <cc1:Calendar id="postdatetimeEnd" runat="server" ReadOnly="False" ScriptPath="../js/calendar.js"></cc1:Calendar>
		</td>
		<td class="vtop">格式 yyyy-mm-dd, 不限制请留空</td>
	</tr>
</table>
<div class="Navbutton">
	<cc1:Button id="SearchTopicAudit" runat="server" Text=" 搜索符合条件的个人空间 "></cc1:Button>
</div>	
</fieldset>
<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
</div>
<div id="spacelistgrid"><uc1:AjaxSpaceInfo id="AjaxSpaceInfo1" runat="server"></uc1:AjaxSpaceInfo></div>		
<br />
<div class="content">
	<div class="left" id="dom0">
		<asp:Literal id="spacelist" runat="server"></asp:Literal>
	</div>
</div><br />
<table class="table1" cellspacing="0" cellpadding="4" width="100%">
 <tr>
 <td align="center">
 <input id="spacestatus" type="hidden" runat="server" />
 <cc1:Button id="SaveTopic" runat="server" Text=" 保存 " ValidateForm="false"></cc1:Button>
 </td></tr> 
</table>		  
</form>

<%=footer%>
</body>
</html>