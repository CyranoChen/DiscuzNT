<%@ Page Language="c#" Inherits="Discuz.Install.InstallStep4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Discuz!NT安装</title>
<meta name="keywords" content="Discuz!NT安装" />
<meta name="description" content="Discuz!NT安装" />
<meta name="generator" content="Discuz!NT 3.0.0" />
<meta http-equiv="x-ua-compatible" content="ie=7" />
<link rel="icon" href="/favicon.ico" type="image/x-icon" />
<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
<link rel="stylesheet" href="main.css" type="text/css" media="all" />
</head>
<script type="text/javascript" src="js/jquery.js"></script>
<link rel="stylesheet" href="js/jquery_boxy/css/common.css" type="text/css" />
<link rel="stylesheet" href="js/jquery_boxy/css/boxy.css" type="text/css" />
<script type="text/javascript" src="js/jquery_boxy/js/jquery.boxy.js"></script>
<script type="text/javascript">
    var flag = true;
    var checkValue = function() {
        $(".txt").each(function() {
            if (this.value == "") {
                var textStr = $(this).parent().prev().text();
                textStr = textStr.substring(0, textStr.length - 1);
                Boxy.alert(textStr + '不能为空！',false, { width: 400 });
                return false;
            }
        });
        if ($("#adminPassword").val() != $("#confirmPassword").val()) {
            $("#s_adminPassword").show();
            $("#s_confirmPassword").show();
            $("#isnull_confirmPassword").hide();
            return false;
        }
        checkForumPath();
        if (!flag) {
            Boxy.alert("论坛路径填写错误！请重新填写",false, { width: 400 });
            return false;
        }
        return true;
    }
    var checkForumPath = function() {
        $.ajax(
        {   
            type: "GET",
            async: false,
            url: window.location.href,
            dataType: "json",
            data: "checkForumPath=exists&forumpath=" + $("#ntPath").val(),
            success: function(data) {
                flag = data.Result;
            }
        });
    }
function checkid(obj,id)
{
    var v = obj.value;
    if(v.length == 0)
        $(id).show();
    else
        $(id).hide();
}

function checkPassword() {
    if ($("#adminPassword").val() != $("#confirmPassword").val()) {
        $("#s_adminPassword").show();
        $("#s_confirmPassword").show();
        $("#isnull_confirmPassword").hide();
    }
    else {
        $("#s_adminPassword").hide();
        $("#s_confirmPassword").hide();
    }
}
</script>

<body>
<div class="wrap">
	<div class="side s_clear">
		<div class="side_bar">
		<h1><%=Discuz.Common.Utils.ASSEMBLY_VERSION%></h1>
			<ul>
				<li class="currentitem">欢迎</li>
				<li class="currentitem">环境检测</li>
				<li class="currentitem">数据库信息</li>
				<li class="currentitem">论坛设置信息</li>
				<li>安装</li>
			</ul>
			<div class="copy">北京康盛新创科技有限责任公司</div>
		</div>
	</div>
	<div class="main s_clear">
		<div class="content">
			<h1>论坛设置信息</h1>
			<div class="info">请认真填写下面的论坛设置信息：</div>
			<div class="inner">
				<div class="hint_info">论坛路径：如果要安装在站点的虚拟目录，则填写/bbs/(bbs为虚拟目录的名字)；否则就填写“/”</div>
				<form id="form_step4" action="step4.aspx" method="post" runat="server">
					<table width="488" cellspacing="0" cellpadding="0" summary="setup" class="setup">
					<tbody>
						<tr>
							<td class="title">管理员名称:</td>
							<td>
							    <input name="adminName" id="adminName" class="txt" type="text" onblur="checkid(this,'#isnull_adminName')" value="admin"><span id='isnull_adminName' style="color:#ff0000;display:none">此处不能为空！</span>
							</td>
						</tr>
						<tr>
							<td class="title">管理员密码:</td>
							<td>
							    <input name="adminPassword" id="adminPassword" class="txt" type="password" onblur="checkid(this,'#isnull_adminPassword')">
							    <span id='isnull_adminPassword' style="color:#ff0000;display:none">此处不能为空！</span>
							    <span id='s_adminPassword' style="color:#ff0000;display:none">两次密码不一致！</span>
							</td>
						</tr>
						<tr>
							<td class="title">管理员密码确认:</td>
							<td>
							    <input name="confirmPassword" id="confirmPassword" class="txt" type="password" onblur="checkid(this,'#isnull_confirmPassword');checkPassword()">
							    <span id='isnull_confirmPassword' style="color:#ff0000;display:none">此处不能为空！</span>
							    <span id='s_confirmPassword' style="color:#ff0000;display:none">两次密码不一致！</span>
							</td>
						</tr>
						<tr>
							<td class="title">论坛路径:</td>
							<td>
							    <input name="ntPath" id="ntPath" class="txt" type="text" onblur="checkid(this,'#isnull_ntPath')" value="<%=forumPath %>" disabled="disabled"><span id="isnull_ntPath" style="color:#ff0000;display:none">此处不能为空！</span>
							</td>
						</tr>
					</tbody>
					</table>
				</form>
			</div>
		</div>
		<div class="btn_box">
		    <button type="submit" name="back" id="back" value="true" onclick="location.href='step3.aspx'">上一步</button>
		    <button type="submit" name="next" id="next"  value="true" onserverclick="Setup" runat="server" onclick="if(!checkValue()) return false;" >下一步</button>
		    
		</div>
	</div>
</div>
</body>

</html>