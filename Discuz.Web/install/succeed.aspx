<%@ Page Language="c#" Inherits="Discuz.Install.Succeed" %>
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
</head>
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
				<li class="currentitem">安装</li>
			</ul>
			<div class="copy">北京康盛新创科技有限责任公司</div>
		</div>
	</div>
	<div class="main s_clear">
		<div class="content">
			<h1>安装完成</h1>
			<div class="info"></div>
			<div class="inner">
			    <div class="ad">
			        <ul>
			            <li><a href="http://idc.comsenz.com/store/webwin.php?source=install" target="_blank"><img src="images/ad.jpg" width="500"/ ></a></li>
			        </ul>
			    </div>
			    <div class="msgsinfo">
				    <p>恭喜! 您已经成功安装，为了安全，请及时删除install目录下的aspx文件</p>
				    <p>用管理员登录后，您可以在首页中点击“系统设置”进入论坛后台进行站点设置</p>
				</div>
				<div class="userinfo">
				    <p>论坛创始人帐号：<%=userID %></p>
				    <p>论坛创始人密码：<%=password %></p>
				</div>
				<div class="more_info">
				    <p>为了保障安全，强烈建议到Discuz!NT官方论坛下载安装程序</p>
				    <ul class="links">
				        <li><a href="http://nt.disucz.net" style="color:#0083B9;text-decoration:underline;padding:0 2px;">Discuz!NT官方论坛</a></li>
				        <li><a href="http://www.comsenz.com/purchase/nt" style="color:#0083B9;text-decoration:underline;padding:0 2px;">Discuz!NT服务购买</a></li>
				        <li><a href="http://nt.discuz.net/support/" style="color:#0083B9;text-decoration:underline;padding:0 2px;">Discuz!NT问题解答</a></li>
				        <li><a href="http://nt.discuz.net/bugs/" style="color:#0083B9;text-decoration:underline;padding:0 2px;">Discuz!NT反馈BUG</a></li>
				    </ul>
				</div>
			</div>
		</div>
		<div class="btn_box"><button type="submit" name="submit" id="submit" value="true" onclick="location.href='../index.aspx'">进入论坛</button></div>
	</div>
</div>
</body>
</html>