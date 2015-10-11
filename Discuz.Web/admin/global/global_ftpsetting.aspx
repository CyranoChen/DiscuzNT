<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="global_ftpsetting.aspx.cs" Inherits="Discuz.Web.Admin.ftpsetting" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>远程附件设置</title>
<link href="../styles/tab.css" type="text/css" rel="stylesheet" />
<link href="../styles/colorpicker.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/common.js"></script>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
<script type="text/javascript" src="../js/AjaxHelper.js"></script>
<script type="text/javascript" src="../js/modalpopup.js"></script>
<script type="text/javascript">
	function ShowFtpLayout(ischecked)
	{
		document.getElementById("FtpLayout").style.display = ischecked ? "block" : "none";
	}
	function validate(thisform)
	{
		document.getElementById("success").style.display = "block";
		var remoteurl = document.getElementById("Remoteurl").value;
		var uploadpath = document.getElementById("Uploadpath").value;
		if(uploadpath.lastIndexOf("/") == uploadpath.length - 1)
		{
			resetpage();
			alert("附件保存路径非法，不能以“/”结尾");
			return false;
		}
		if(remoteurl.substring(0,7).toLowerCase() != "http://" ||
		   remoteurl.lastIndexOf("/") == remoteurl.length - 1)
		{
			resetpage();
			alert("远程访问 URL 非法！不是以“http://”开头或是以“/”结尾");
			return false;
		}
//            if(remoteurl.indexOf(uploadpath) == -1)
//            {
//                resetpage();
//                alert("远程访问 URL 未以“附件保存路径”结尾");
//                return false;
//            }
		if(document.getElementById("Allowupload_0").checked)
		{
			var result = TestFtp();
			if(result != "ok")
			{
				resetpage();
				ShowFtpLayout(false);
				document.getElementById("Allowupload_1").checked = true;
				alert("无法链接FTP，不能保存设置！");
				return false;
			}
		}
		return true;
	}
	function resetpage()
	{
		document.getElementById("success").style.display = "none";
		document.getElementById("SaveFtpInfo").disabled = false;
	}
	function TestFtp()
	{
		var serveraddress = document.getElementById("Serveraddress").value;
		var serverport = document.getElementById("Serverport").value;
		var username = document.getElementById("Username").value;
		var password = document.getElementById("Password").value;
		var timeout = document.getElementById("Timeout").value;
		var uploadpath = document.getElementById("Uploadpath").value;
		var url = "serveraddress="+serveraddress+"&serverport="+serverport+"&username="+username+"&password="+password.replace(/\+/g,"%2B");
		url += "&timeout="+timeout+"&uploadpath="+uploadpath;
		var result = getReturn('global_ajaxcall.aspx?opname=ftptest&' + url);
		return result;
	}
	function TestFtp_Click()
	{
		document.getElementById("TestFtpButton").disabled = true;
		var result = TestFtp();
		if(result == "ok")
		{
			alert("远程附件设置测试通过！");
		}
		else
		{
			alert(result);
		}
		document.getElementById("TestFtpButton").disabled = false;
	}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div class="ManagerForm">
<form id="Form1" method="post" runat="server">
<div class="ManagerForm">
<div id="TabControl1_Tab" class="tabs"><input type="hidden" value="TabControl1:tabPage51" id="TabControl1" name="TabControl1">
	<ul>		
		<li class="TabSelect" ><a href="global_attach.aspx">附件设置</a></li>
		<li class="CurrentTabSelect"><a class="current" href="global_ftpsetting.aspx?ftptype=forumattach">远程附件</a></li>
		<li class="TabSelect" ><a href="../forum/forum_attchemnttypes.aspx">附件类型</a></li>
		<li class="TabSelect" ><a href="../forum/forum_attachtypesgrid.aspx">附件尺寸</a></li>
	</ul>
</div>
<div class="tabarea" id="TabControl1tabarea" style="display: block;">
<table width="100%">
	<tr><td class="item_title" colspan="2">启用远程附件</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="Allowupload" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Value="0" Selected="true">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
</table>			    
<div id="FtpLayout" runat="server">
<table width="100%">
	<tr><td class="item_title" colspan="2">FTP服务器</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Serveraddress" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Width="170"></cc1:TextBox>
		</td>
		<td class="vtop">可以是 FTP 服务器的 IP 地址或域名</td>
	</tr>
	<tr><td class="item_title" colspan="2">FTP端口</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Serverport" runat="server" MinimumValue="1" CanBeNull="必填" size="7" maxlength="5" Text="21"></cc1:TextBox>
		</td>
		<td class="vtop">默认为 21</td>
	</tr>
	<tr><td class="item_title" colspan="2">用户名</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Username" runat="server" CanBeNull="必填" Width="170"></cc1:TextBox>
		</td>
		<td class="vtop">该帐号必需具有以下权限:读取文件 写入文件 删除文件 创建目录 子目录继承</td>
	</tr>
	<tr><td class="item_title" colspan="2">密  码</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox id="Password" TextMode="password" runat="server" RequiredFieldType="暂无校验" CanBeNull="必填" Width="170"></cc1:TextBox>
			<input type="hidden" id="hiddpassword" runat="server" />
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">超时时间(秒)</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="Timeout" runat="server" CanBeNull="必填" MinimumValue="0" Width="100" Text="10"></cc1:TextBox>
		</td>
		<td class="vtop">单位:秒,10 为服务器默认.0为不受超时时间限制.</td>
	</tr>
	<tr><td class="item_title" colspan="2">附件保存路径</td></tr>
	<tr>
		<td class="vtop rowform">
			 <cc1:TextBox ID="Uploadpath" runat="server" CanBeNull="必填" RequiredFieldType="暂无校验" Width="170" Text="forumattach/"></cc1:TextBox>
		</td>
		<td class="vtop">远程附件目录的绝对路径或相对于 FTP 主目录的相对路径,结尾不要加斜杠"/","."表示 FTP 主目录</td>
	</tr>
	<tr><td class="item_title" colspan="2">远程访问 URL</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:TextBox ID="Remoteurl" runat="server" CanBeNull="必填" Width="200"></cc1:TextBox>
	        <div stylle="margin:4px 0;"><button type="button" class="ManagerButton" id="TestFtpButton" onclick="TestFtp_Click()"><img src="../images/submit.gif" /> 测试远程附件设置 </button></div>
		</td>
		<td class="vtop">仅支持 HTTP 协议，结尾不要加斜杠"/";<br/>例如上传的文件是"1.jpg", 则最终远程链接为<br/>"http://远程访问URL/1.jpg"</td>
	</tr>
	<tr><td class="item_title" colspan="2">是否保留本地附件</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="Reservelocalattach" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Selected="true" Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
	<tr><td class="item_title" colspan="2">删帖时是否保留远程附件</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="Reserveremoteattach" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1">是</asp:ListItem>
				<asp:ListItem Selected="true" Value="0">否</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
</table>	
<cc1:Hint ID="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
<div style="display:none">
	<tr><td class="item_title" colspan="2">FTP模式</td></tr>
	<tr>
		<td class="vtop rowform">
			<cc1:RadioButtonList ID="Mode" runat="server" RepeatColumns="2">
				<asp:ListItem Value="1" Selected="true">被动模式</asp:ListItem>
				<asp:ListItem Value="2">主动模式</asp:ListItem>
			</cc1:RadioButtonList>
		</td>
		<td class="vtop"></td>
	</tr>
</div>
</div>   
<div class="Navbutton">
	<cc1:Button id="SaveFtpInfo" runat="server" Text=" 提 交 " OnClick="SaveFtpInfo_Click" ValidateForm="true"></cc1:Button>
</div>	   
</div>
</form>
</div>
<script type="text/javascript">document.getElementById("Password").value = document.getElementById("hiddpassword").value;</script>
<% =footer %>
</body>
</html>