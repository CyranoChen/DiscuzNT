<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upgrade.aspx.cs" Inherits="Discuz.Install.Upgrade" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Discuz!NT安装</title>
<meta name="keywords" content="Discuz!NT安装" />
<meta name="description" content="Discuz!NT安装" />
<meta name="generator" content="Discuz!NT <%=Discuz.Common.Utils.ASSEMBLY_VERSION%>" />
<meta http-equiv="x-ua-compatible" content="ie=7" />
<link rel="icon" href="favicon.ico" type="image/x-icon" />
<link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
<link rel="stylesheet" href="main.css" type="text/css" media="all" />
<script type="text/javascript" src="js/jquery.js"></script>
<link rel="stylesheet" href="../install/js/jquery_boxy/css/common.css" type="text/css" />
<link rel="stylesheet" href="../install/js/jquery_boxy/css/boxy.css" type="text/css" />
<script type="text/javascript" src="../install/js/jquery_boxy/js/jquery.boxy.js"></script>
<script type="text/javascript">
    var checkConnection = function () {
        $("#recheck,#upgrade").attr("disabled", "disabled");

        $("#loading").ajaxStart(function () {
            $("#loading").removeClass("hint_info").addClass("loading_info").html("正在检测数据库链接...");
        });

        $.getJSON("step2.aspx?connection=check&r=" + Math.random(), function (data) {
            if (!data.Result) {
                $("#loading").removeClass("loading_info").addClass("hint_info").html(data.Message);
                $("#recheck").removeAttr("disabled");
            }
            else {
                $("#loading").removeClass("loading_info").addClass("hint_info").html("<ul><li>升级前请做好数据库的备份</li><li>请确认将detachtable_数据版本.config放置到config文件夹中</li></ul>");
                $("#upgrade").removeAttr("disabled");
            }
        });

        $("#recheck").click(checkConnection);
    };

    $(function () {
        checkConnection();
    });

    function upgrade() {
        var showbox = new Boxy('<p style=\"width:300px;font-size:14px;\"><img src=\"../images/common/loading.gif\" alt=\"loading\" />正在升级,时间较长,请耐心等待，不要关闭页面...</p>', { closeable: false, modal: true, center: true });
        showbox.show();
        $.getJSON("step2.aspx?upgrade=true&r=" + Math.random(), $('#form1').serialize(), function (data) {
            if (data.Result) {
                window.location.href = 'succeed.aspx';
            }
        });
        return false;
    }
</script>
</head>

<body>
<div class="wrap cl">
	<h2><img alt="Discuz!NT|BBS|论坛" src="images/logo.png" /><cite><b><%=Discuz.Common.Utils.ASSEMBLY_VERSION%></b>安装程序</cite></h2>
	<div class="nav">
        <ul>
            <li class="finish"><span>欢迎</span></li>
            <li class="cur"><span>升级版本选择</span></li>
            <li><span>升级完成</span></li>
        </ul>
	</div>
    <form id="form1" runat="server" >
	    <div class="main cl">
		    <h1>欢迎</h1>
		    <div class="inner">
                <div id="loading" class="hint loading_info">正在检测数据库链接...</div>
			    <table width="100%" cellspacing="0" cellpadding="0" summary="setup" class="setup">
				    <tbody>
					    <tr>
						    <td>请选择您要从以下哪个版本升级到Discuz!NT <%=Discuz.Common.Utils.ASSEMBLY_VERSION%>
							    <asp:RadioButtonList ID="rblBBSVersion" runat="server">
                                    <asp:ListItem Value="20">Discuz!NT 2.0</asp:ListItem>
                                    <asp:ListItem Value="21">Discuz!NT 2.1</asp:ListItem>
                                    <asp:ListItem Value="25">Discuz!NT 2.5</asp:ListItem>
                                    <asp:ListItem Value="26">Discuz!NT 2.6</asp:ListItem>
                                    <asp:ListItem Value="30">Discuz!NT 3.0</asp:ListItem>
                                    <asp:ListItem Value="31" Selected="True">Discuz!NT 3.1 ~ 3.5.x</asp:ListItem>
                                </asp:RadioButtonList>
						    </td>
					    </tr>
				    </tbody>
			    </table>
            </div>
	    </div>
	    <div class="btn cl">
		    <a href="#" id="recheck" class="back" disabled="disabled">重新检测</a>
		    <a href="#" onclick="upgrade();" id="upgrade" class="next" disabled="disabled">升级</a>
	    </div>
    </form>
	<div class="copy">
		北京康盛新创科技有限责任公司 &copy; 2001 - 2011 Comsenz Inc. 
	</div>
</div>
</body>
</html>