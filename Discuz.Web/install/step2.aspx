<%@ Page Language="c#" AutoEventWireup="false" Inherits="Discuz.Install.install" %>
<%@import namespace="Discuz.Install"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
<script type="text/javascript" src="js/jquery.js"></script>
</head>
<body>
<script type="text/javascript">
    function get_detaile() {
        $("#a_detailedMessage").hide();
        $("#detailedMessage").show();
        $("#a_hide").show();
    }
    function hide() {
        $("#a_detailedMessage").show();
        $("#detailedMessage").hide();
        $("#a_hide").hide();
    }

   
</script>
<div class="wrap">
	<div class="side s_clear">
		<div class="side_bar">
		<h1><%=Discuz.Common.Utils.ASSEMBLY_VERSION%></h1>
			<ul>
				<li class="currentitem">欢迎</li>
				<li class="currentitem">环境检测</li>
				<li>数据库信息</li>
				<li>论坛设置信息</li>
				<li>安装</li>
			</ul>
			<div class="copy">北京康盛新创科技有限责任公司</div>
		</div>
	</div>
	<div class="main s_clear">
		<div class="content">
			<h1>环境检测</h1>
			<div class="info">检测到你的系统环境：</div>
			<div class="inner">
				<div class="hint_info">以下目录需要NETWORK SERVICE的完全控制权限
				<a id="a_detailedMessage" href="#" onclick="get_detaile()">&gt;&gt;查看详细信息</a>
				    <div id="detailedMessage" style="display:none">
				        如果出现目录或文件没有写入和删除权限情况,请选择该目录或文件->右键属性->安全->添加,
						在"输入对象名称来选择"中输入"Network Service",点击"确定".选择"组或用户名称"中"Network Service"用户组,在下面
						"Network Service"的权限中勾选"修改"的"允许"复选框,点击"确定"后再次重新刷新本页面继续.
						<a id="a_hide" href="#" style="display:none" onclick="hide()">&lt;&lt;收起</a>
				    </div>
				</div>
				<ul class="list">
					<%
						 bool err = false;
					     Response.Write(SetupPage.InitialSystemValidCheck(ref err));
					%>
				</ul>
			</div>
		</div>
		
		<div class="btn_box">
		    <button type="submit" name="recheck" id="recheck" value="true" onclick="location.href='step2.aspx'">重新检测</button>
		    <button type="submit" name="next" id="next" value="true" onclick="location.href='step3.aspx'">下一步</button>
		</div>
	</div>
</div>
<script type="text/javascript">
		    if (<%=isError %> ==0)
		    $("#recheck").attr("disabled", "disabled");        
		    else
		        $("#recheck").removeAttr("disabled");	        
        </script>
</body>
</html>
