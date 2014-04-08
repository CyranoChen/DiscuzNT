<%@ Page language="c#" Inherits="Discuz.Web.Admin.forumhottopic" Codebehind="aggregation_forumhottopic.aspx.cs" %>
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
<script language="JavaScript" type="text/javascript" src="../../javascript/ajax.js"></script>
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
	
	
	function gettopicinfo()
	{
		var tid=$('ajaxtid').value;
		if(!isNumber(tid))
		{
		  $('adderror').innerHTML='ID必须是整数'
		  return;
		}
		_sendRequest('../global/global_ajaxcall.aspx?opname=gettopicinfo', gettopicinfo_callback, false, 'tid='+tid);
	}
	function gettopicinfo_callback(doc)
	{
	
	  var data=eval(doc);
	  if(data[0]==undefined)
	  {
	  $('adderror').innerHTML='主题不存在'
	  }
	  else
	  {
	  addElement('tid',data[0].tid,data[0].title);
	  $('adderror').innerHTML='添加主题成功'
	  }
	  $('ajaxtid').value='';
	}

   function checkformid()
   {
		if($('forumid').value!='' && !isNumber($('forumid').value))
		{
		alert("版块ID必须为数字");
		return false;
		}
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body >

<form id="Form1" runat="server">
<div class="ManagerForm" id="searchcondition">
<fieldset>
<legend style="background: url(&quot;../images/icons/icon32.jpg&quot;) no-repeat scroll 6px 50% transparent;">搜索论坛主题帖</legend>
<div id="searchtable">
	<table width="100%">
	<tbody>
		<tr>
			<td><span style="padding-right:4px;">排序方式</span>
			<select name="showtype" id="showtype" style="margin-right:8px;">
				<option value="replies"  <%if (showtype=="replies"){%>selected<%}%>>按回复数排序</option>
				<option value="views"  <%if (showtype=="views"){%>selected<%}%>>按浏览量排序</option>
			</select><span style="padding-right:4px;">时间范围</span>
			<select name="timebetween" id="timebetween" style="margin-right:8px;">
				<option value="1" <%if (timebetween==1){%>selected<%}%>>一天</option>
				<option value="7" <%if (timebetween==7){%>selected<%}%>>一周</option>
				<option value="30" <%if (timebetween==30){%>selected<%}%>>一月</option>
				<option value="0" <%if (timebetween==0){%>selected<%}%>>全部</option>
			</select><span style="padding-right:4px;">版块Fid</span>
			<input name="forumid" type="text"  id="forumid" style="margin-right:8px;" value="<%if (forumid!=0){%><%=forumid%><%}%>" size="4"/>
			<input name="search" type="submit" value="开始搜索" class="ManagerButton" onClick="return  checkformid();"/>
			</td>
		</tr>
	</tbody>
	</table>
</div>
</fieldset>
</div>
<div id="topiclistgrid" style="width:98%;margin:0 auto;"><uc1:AjaxTopicInfo id="AjaxTopicInfo1" runat="server"></uc1:AjaxTopicInfo></div>

<br />
<div class="ManagerForm">
<fieldset>
<legend style="background: url(&quot;../images/icons/icon32.jpg&quot;) no-repeat scroll 6px 50% transparent;">直接添加热帖</legend>
<div id="addhottopics">
	<table width="100%">
	<tbody>
		<tr>
			<td><span style="padding-right:4px;">主题ID:</span>
			<input name="ajaxtid" type="text" id="ajaxtid" style="margin-right:8px;"><input name="button" type="button" id="dsfsdafsa" onClick="gettopicinfo()" value="添加帖子"  class="ManagerButton"><span style="color:#FF0000;padding-left:8px;" id="adderror"></span>
			</td>
		</tr>
	</tbody>
	</table>
</div>
</fieldset>
</div>
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
</div>
  <cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
  <input id="forumtopicstatus" type="hidden" runat="server" />
</form>
<%=footer%>
</body>
</html>