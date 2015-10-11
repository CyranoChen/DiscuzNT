<%@ Page Language="c#" Inherits="Discuz.Install.install" %>
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
    <link rel="stylesheet" href="js/jquery_boxy/css/common.css" type="text/css" />
    <link rel="stylesheet" href="js/jquery_boxy/css/boxy.css" type="text/css" />
    <script type="text/javascript" src="js/jquery_boxy/js/jquery.boxy.js"></script>
</head>
<body>
<div class="wrap cl">
	<h2><img alt="Discuz!NT|BBS|论坛" src="images/logo.png" /><cite><b><%=Discuz.Common.Utils.ASSEMBLY_VERSION%></b>安装程序</cite></h2>
	<div class="nav">
        <ul>
		    <li <%if(stepNum==0){ %> class="cur"<%}else{ %> class="finish"<%} %>><span id="index">欢迎</span></li>
		    <li <%if(stepNum==1){ %> class="cur"<%}else if(stepNum>1){ %> class="finish"<%} %>><span>环境检测</span></li>
		    <li <%if(stepNum==2){ %> class="cur"<%}else if(stepNum>2){ %> class="finish"<%} %>><span>数据库配置</span></li>
		    <li <%if(stepNum==3){ %> class="cur"<%}else if(stepNum>3){ %> class="finish"<%} %>><span>论坛配置</span></li>
		    <li <%if(stepNum==4){ %> class="cur"<%}else if(stepNum>4){ %> class="finish"<%} %>><span>安装</span></li>
        </ul>
	</div>
    <%if(stepNum==0){ %>
	<div class="main cl">
		<h1>欢迎使用Discuz!NT 论坛系统</h1>
        <div class="inner">
            <h2>Discuz!NT 代码使用协议</h2>
            <strong>版权所有 (c) 2001-2009，北京康盛新创科技有限责任公司,保留所有权利</strong>。 
            <br>
            <br>
            <p>感谢您选择 Discuz!NT 社区产品，希望我们的努力能为您提供一个高效快速和强大的社区论坛解决方案。 </p>
            <p>北京康盛新创科技有限责任公司（Comsenz Technology Ltd）为 Discuz!NT 产品的开发商，依法独立拥有 Discuz!NT 产品著作权（中国国家版权局著作权登记号 2003SR6623）。北京康盛新创科技有限责任公司网址为 <a href="http://www.comsenz.com" target="_blank">http://www.comsenz.com</a>， Discuz!NT 官方讨论区网址为 <a href="http://nt.discuz.net" target="_blank">http://nt.discuz.net</a>。 </p>
            <p>Discuz!NT 著作权已在中华人民共和国国家版权局注册，著作权受到法律和国际公约保护。使用者：无论个人或组织、盈利与否、用途如何（包括以学习和研究为目的），均需仔细阅读本协议，在理解、同意、并遵守本协议的全部条款后，方可开始使用 Discuz!NT 软件。 </p>
            <p>本授权协议适用且仅适用于 Discuz!NT 3.x 版本，北京康盛新创科技有限责任公司拥有对本授权协议的最终解释权。 <br></p>
            <p></p>
            <h2>协议许可的权利</h2>
                1) 您可以在完全遵守本最终用户授权协议的基础上，将本软件应用于非商业用途，而不必支付费用。 <br>
            <br>
            2) 您可以在协议规定的约束和限制范围内修改 Discuz!NT 源代码(如果被提供的话)或界面风格以适应您的网站要求。 <br>
            <br>
            3) 您拥有使用本软件构建的论坛中全部会员资料、文章及相关信息的所有权，并独立承担与文章内容的相关法律义务。 <br>
            <br>
            4) 获得商业授权之后，您可以将本软件应用于商业用途，同时依据所购买的授权类型中确定的技术支持期限、技术支持方式和技术支持内容，自购买时刻起，在技术支持期限内拥有通过指定的方式获得指定范围内的技术支持服务。商业授权用户享有反映和提出意见的权力，相关意见将被作为首要考虑，但没有一定被采纳的承诺或保证。 
            <p></p>
            <h2>协议规定的约束和限制</h2>
            1) 未获商业授权之前，不得将本软件用于商业用途（包括但不限于企业网站、经营性网站、以营利为目或实现盈利的网站）。 <br>
            <br>
            2) 不得对本软件或与之关联的商业授权进行出租、出售、抵押或发放子许可证。 <br>
            <br>
            3) 无论如何，即无论用途如何、是否经过修改或美化、修改程度如何，只要使用 Discuz!NT 的整体或任何部分，未经书面许可，论坛页面页脚处的 Discuz!NT 名称和北京康盛新创科技有限责任公司下属网站（http://www.comsenz.com、http://www.discuznt.com 或 http://nt.discuz.net） 的链接都必须保留，而不能清除或修改。<br> 
            <br>
            4) 禁止在 Discuz!NT 的整体或任何部分基础上以发展任何派生版本、修改版本或第三方版本用于重新分发。 与其它条款无抵触的前提下，允许以自用为目的的进行进行二次开发或整合，但同样受前文第3项约束和限制，即保留Discuz!NT名称与链接。<br>
            <br>
            5) 如果您未能遵守本协议的条款，您的授权将被终止，所被许可的权利将被收回，并承担相应法律责任。 
            <p></p>
            <h2>有限担保和免责声明</h2>
            1) 本软件及所附带的文件是作为不提供任何明确的或隐含的赔偿或担保的形式提供的。<br> 
            <br>
            2) 用户出于自愿而使用本软件，您必须了解使用本软件的风险，在尚未购买产品技术服务之前，我们不承诺提供任何形式的技术支持、使用担保，也不承担任何因使用本软件而产生问题的相关责任。<br> 
            <br>
            3) 北京康盛新创科技有限责任公司不对使用本软件构建的论坛中的文章或信息承担责任。<br>
            <br>
            有关 Discuz!NT 最终用户授权协议、商业授权与技术服务的详细内容，均由 Discuz!NT 官方网站独家提供。北京康盛新创科技有限责任公司拥有在不事先通知的情况下，修改授权协议和服务价目表的权力，修改后的协议或价目表对自改变之日起的新授权用户生效。 
            <p>电子文本形式的授权协议如同双方书面签署的协议一样，具有完全的和等同的法律效力。您一旦开始安装 Discuz!NT，即被视为完全理解并接受本协议的各项条款，在享有上述条款授予的权力的同时，受到相关的约束和限制。协议许可范围以外的行为，将直接违反本授权协议并构成侵权，我们有权随时终止授权，责令停止损害，并保留追究相关责任的权力。 <br>
            <br>
            </p>
            <h2>附：</h2>
                北京康盛新创科技有限责任公司网址：<a href="http://www.comsenz.com" target="_blank">http://www.comsenz.com</a><br>
                Discuz!NT产品网站：<a href="http://www.comsenz.com/products/nt" target="_blank">http://www.comsenz.com/products/nt</a><br>
                Discuz!NT官方论坛：<a href="http://nt.discuz.net" target="_blank">http://nt.discuz.net</a><br>
        </div>
	</div>
	<div class="btn cl">
		<a href="javascript:window.close();" class="back">退出安装</a>
		<a href="install.aspx?step=servertest" class="next">接受协议</a>
	</div>
    <%}else if(stepNum==1){ %>
	<div class="main cl">
		<h1>环境检测</h1>
		<div class="inner">
			<ul id="resultlist" class="list">
			</ul>
		</div>
	</div>
	<div class="btn cl">
		<a href="javascript:history.back();" class="back">上一步</a>
		<a id="nextlink" href="install.aspx?step=dbset" class="next" disabled="true">下一步</a>		
	</div>
    <script type="text/javascript">
        var resultlist = <%=testResult%>;
        var errorcount = 0;
        for (i = 0; i < resultlist.length; i++) {
            var icon, iconname;
            if (resultlist[i].result == 'true') {
                icon = 'ok';
                iconname = '成功';
            }
            else {
                icon = 'error';
                iconname = '失败';
                errorcount++;
            }
            $('#resultlist').append('<li><img src="images/' + icon + '.gif" alt="' + iconname + '"/><a href="#">' + resultlist[i].msg + '</a></li>');
        }
        if(errorcount <= 0){
            $('#nextlink').attr("disabled","");
        }
    </script>
    <%}else if(stepNum==2){ %>
    <div class="main cl">
		<h1>数据库配置</h1>
		<div class="inner">
			<form action="" method="post">
			    <table width="100%" cellspacing="0" cellpadding="0" summary="数据库配置">
			        <tbody>
			            <tr>
				            <td class="title">数据库地址:</td><td><input id="sql_ip" name="sql_ip" class="txt" type="text" value="<%=sqlServerIP %>"/></td>
			            </tr>
			            <tr>
				            <td class="title">数据库名称:</td><td><input id="sql_name" name="sql_name" class="txt" type="text" value="<%=dataBaseName %>"/><span>填写不存在的数据库名称会尝试自动创建</span></td>
			            </tr>
			            <tr>
				            <td class="title">数据库用户名:</td><td><input id="sql_username" name="sql_username" class="txt" type="text" value="<%=sqlUID %>"/></td>
			            </tr>
			            <tr>
				            <td class="title">数据库密码:</td><td><input id="sql_password" name="sql_password" class="txt" type="password" value="<%=sqlPassword %>"/></td>
			            </tr>
			            <tr>
				            <td class="title">表前辍:</td><td><input id="table_prefix" name="table_prefix" class="txt" type="text" value="<%=tablePrefix %>"/></td>
			            </tr>
                    </tbody>
			    </table>
			</form>
		</div>
	</div>
	<div class="btn cl">
		<a href="javascript:history.back();" class="back">上一步</a>
		<a href="###" onclick="checkDbset();" class="next">下一步</a>		
	</div>
    <script type="text/javascript">
        var showbox;
        var alerthead = "<p style=\"width:300px;font-size:14px;\"><img src=\"../images/common/loading.gif\" alt=\"loading\" />";
        var createdDb = 0;
        var runstep = 0;
        var steptime = 700;

        function checkDbset() {
            var sqlip = $('#sql_ip').val();
            var sqlname = $('#sql_name').val();
            var loginname = $('#sql_username').val();
            var password = $('#sql_password').val();
            var tableprefix = $('#table_prefix').val();

            if (sqlip == "") {
                Boxy.alert('数据库地址不能为空', false, { width: 400 });
                return;
            }
            if (sqlname == "") {
                Boxy.alert('数据库名称不能为空', false, { width: 400 });
                return;
            }
            if (loginname == "") {
                Boxy.alert('数据库登录名不能为空', false, { width: 400 });
                return;
            }
            if (password == "") {
                Boxy.alert('数据库登录密码不能为空', false, { width: 400 });
                return;
            }
            if (tableprefix == "") {
                Boxy.alert('表前缀不能为空', false, { width: 400 });
                return;
            }

            showbox = new Boxy(alerthead + "正在检测数据库连接,该检测可能比较耗时,请耐心等待......</p>", { closeable: false, modal: true, center: true });

            checkDbAjax(sqlip, sqlname, loginname, password, tableprefix);
        }

        function checkDbAjax(sqlip, sqlname, loginname, password, tableprefix) {
            showbox.setContent(alerthead + "正在检测数据库连接,该检测可能比较耗时,请耐心等待......</p>");
            showbox.show();
            jQuery.get('ajax.aspx', { 't': 'checkdbconnection', 'ip': sqlip, 'name': sqlname, 'loginname': loginname, 'loginpwd': password, 'time': Math.random() },
            function (data) {
                var callback = eval("(" + data + ")");
                if (!callback.result) {
                    if (createdDb == 0 && callback.code == "4060") {
                        showbox.hide();
                        Boxy.confirm("您填写的数据库\""+sqlname+"\"不存在,是否尝试在数据库自动创建该名称的数据库?",
                        function on() {
                            runstep = 0;
                            createDbAjax(sqlip, sqlname, loginname, password, tableprefix);
                        }, { title: "是否创建数据库" });
                    }
                    else if (callback.code == "53") {
                        setTimeout(function on() {
                            showbox.hide();
                            Boxy.alert("数据库连接超时,请检查数据库地址是否正确", null, { width: 400 });
                            runstep = 0;
                        }, ++runstep * steptime);
                    }
                    else {
                        setTimeout(function on() {
                            showbox.hide();
                            Boxy.alert(callback.message, null, { width: 400 });
                            runstep = 0;
                        }, ++runstep * steptime);
                    }
                } else {
                    setTimeout(function on() {
                        checkDBCollation(sqlip, sqlname, loginname, password, tableprefix);
                    }, ++runstep * steptime);
                }
            });
        }

        function DBSourceExist(sqlip, sqlname, loginname, password, tableprefix) {
            showbox.show();
            showbox.setContent(alerthead + "正在检测数据库已有数据......</p>");
            jQuery.get('ajax.aspx', { 't': 'dbsourceexist', 'ip': sqlip, 'name': sqlname, 'loginname': loginname, 'loginpwd': password, 'prefix': tableprefix, 'time': Math.random() },
                function (data) {
                    var callback = eval("(" + data + ")");
                    if (callback.result) {
                        showbox.hide();
                        runstep = 0;
                        Boxy.confirm('系统检测到数据库"' + sqlname + '"已经包含论坛所需的数据表,继续安装会清空之前数据,是否继续?',
                        function on() {
                            saveDbSet(sqlip, sqlname, loginname, password, tableprefix);
                        }
                        , { title: "是否继续安装?" });
                    }
                    else {
                        if (callback.code = '208') {
                            setTimeout(function on() {
                                saveDbSet(sqlip, sqlname, loginname, password, tableprefix);
                            }, ++runstep * steptime);
                        } else {
                            showbox.hide();
                            Boxy.alert(callback.message, null, { width: 400 });
                            runstep = 0;
                        }
                    }
                });
        }

        function checkDBCollation(sqlip, sqlname, loginname, password, tableprefix) {
            showbox.show();
            showbox.setContent(alerthead + "正在检测数据库排序规则......</p>");
            jQuery.get('ajax.aspx', { 't': 'checkdbcollation', 'ip': sqlip, 'name': sqlname, 'loginname': loginname, 'loginpwd': password, 'time': Math.random() },
                function (data) {
                    var callback = eval("(" + data + ")");
                    if (callback.result) {
                        setTimeout(function on() {
                            DBSourceExist(sqlip, sqlname, loginname, password, tableprefix);
                        }, ++runstep * steptime);
                    }
                    else {
                        showbox.hide();
                        Boxy.alert(callback.message, null, { width: 400 });
                        runstep = 0;
                    }
            });
        }

        function createDbAjax(sqlip, sqlname, loginname, password, tableprefix) {
            showbox.show();
            showbox.setContent(alerthead + "正在创建数据库......</p>");
            jQuery.get('ajax.aspx', { 't': 'createdb', 'ip': sqlip, 'name': sqlname, 'loginname': loginname, 'loginpwd': password, 'time': Math.random() },
                function (data) {
                    var callback = eval("(" + data + ")");
                    if (callback.result) {
                        createdDb = 1;
                        setTimeout(function on() {
                            checkDbAjax(sqlip, sqlname, loginname, password, tableprefix);
                        }, ++runstep * steptime);
                    } else if (callback.code = "262") {
                        createdDb = 0;
                        setTimeout(function on() {
                            showbox.hide();
                            Boxy.alert('数据库用户 \'' + loginname + '\' 没有创建数据库的权限,创建新数据库失败,请填写已有的数据库 ', null, { width: 400 });
                            runstep = 0;
                        }, ++runstep * steptime);
                    } else {
                        createdDb = 0;
                        setTimeout(function on() {
                            showbox.hide();
                            Boxy.alert(callback.message, null, { width: 400 });
                            runstep = 0;
                        }, ++runstep * steptime);
                    }
                }
            );
        }

        function saveDbSet(sqlip, sqlname, loginname, password, tableprefix) {
            showbox.show();
            showbox.setContent(alerthead + "正在保存数据库配置......</p>");
            jQuery.get('ajax.aspx', { 't': 'savedbset', 'ip': sqlip, 'name': sqlname, 'loginname': loginname, 'loginpwd': password, 'prefix': tableprefix, 'time': Math.random() },
                function (data) {
                    var callback = eval("(" + data + ")");
                    if (callback.result) {
                        setTimeout(function on() {
                            showbox.hide();
                            Boxy.alert(callback.message, function on() { location.href = 'install.aspx?step=forumset'; }, { width: 400 });
                            runstep = 0;
                        }, ++runstep * steptime);
                    }
                }
            );
        }
    </script>
    <%} else if (stepNum == 3) {%>
	<div class="main cl">
		<h1>论坛配置</h1>
		<div class="inner">
			<form id="forumset" action="" method="post">
				<table width="100%" cellspacing="0" cellpadding="0" summary="论坛配置">
				<tbody>
					<tr>
						<td class="title">管理员名称:</td><td><input id="adminname" name="adminname" class="txt" type="text"/></td>
					</tr>
					<tr>
						<td class="title">管理员密码:</td><td><input id="adminpassword" name="adminpassword" class="txt" type="password"/></td>
					</tr>
					<tr>
						<td class="title">管理员密码确认:</td><td><input id="confirmpassword" name="confirmpassword" class="txt" type="password"/></td>
					</tr>
				</tbody>
				</table>
			</form>
		</div>
	</div>
	<div class="btn cl">
		<a href="javascript:history.back();" class="back">上一步</a>
		<a href="###" onclick="checkforumset();" class="next">下一步</a>		
	</div>
    <script type="text/javascript">
        function checkforumset() {
            var adminname = $('#adminname').val();
            var adminpassword = $('#adminpassword').val();
            var confirmpassword = $('#confirmpassword').val();

            if (adminname == "") {
                Boxy.alert('管理员名称不能为空', false, { width: 400 });
                return;
            }
            if (adminpassword == "") {
                Boxy.alert('管理员密码不能为空', false, { width: 400 });
                return;
            }
            if (adminpassword != confirmpassword) {
                Boxy.alert('两次输入的密码不一致', false, { width: 400 });
                return;
            }

            $('#forumset').attr("action", "install.aspx?step=initial");
            $('#forumset').submit();
        }
    </script>
    <%} else if (stepNum == 4) {%>
    <div class="main cl">
		<h1>安装</h1>
		<div class="inner">
			<ul id="processlist" class="list">
            </ul>
		</div>
        <input type="hidden" id="adminname" name="adminname" value="<%=adminName %>" />
        <input type="hidden" id="adminpassword" name="adminpassword" value="<%=adminPassword %>" />

	</div>
	<div class="btn cl">
		<a href="###" class="back">上一步</a>
		<a id="successlink" href="###" class="back">完成</a>
	</div>
    <script type="text/javascript">
        var adminname = $('#adminname').val();
        var adminpassword = $('#adminpassword').val();

        var runstep = 0;
        var steptime = 500;
        function runinstall() {
            $('#processlist').append('<li><img alt="成功" src="images/ok.gif"/>资源准备中......</li>');

            if (adminname == "" || adminpassword == "") {
                $('#processlist').append('<li><img alt="失败" src="images/error.gif"/>管理员帐号数据异常,安装失败......</li>');
                return;
            }
            createTable();
        }

        function createTable() {
            jQuery.get('ajax.aspx', { 't': 'createtable', 'time': Math.random() },
                function (data) {
                    var callback = eval("(" + data + ")");
                    if (callback.result) {
                        setTimeout(function on() {
                            $('#processlist').append('<li><img alt="成功" src="images/ok.gif"/>数据表,约束和索引创建成功......</li>');
                        }, ++runstep * steptime);
                        createSP();
                    }
                    else {
                        setTimeout(function on() {
                            $('#processlist').append('<li><img alt="失败" src="images/error.gif"/>内部异常,数据表创建失败......</li>');
                        }, ++runstep * steptime);
                    }
                }
            );
        }

        function createSP() {
            jQuery.get('ajax.aspx', { 't': 'createsp', 'time': Math.random() },
                function (data) {
                    var callback = eval("(" + data + ")");
                    if (callback.result) {
                        setTimeout(function on() {
                            $('#processlist').append('<li><img alt="成功" src="images/ok.gif"/>选择创建SqlServer ' + callback.message + ' 版本存储过程......</li>');
                        }, ++runstep * steptime);

                        setTimeout(function on() {
                            $('#processlist').append('<li><img alt="成功" src="images/ok.gif"/>存储过程创建成功......</li>');
                        }, ++runstep * steptime);

                        initSource();
                    }
                    else {
                        setTimeout(function on() {
                            $('#processlist').append('<li><img alt="失败" src="images/error.gif"/>内部异常,存储过程创建失败......</li>');
                        }, ++runstep * steptime);
                    }
                }
            );
        }

        function initSource() {
            jQuery.get('ajax.aspx', { 't': 'initsource', 'admin': adminname, 'pwd': adminpassword, 'time': Math.random() },
                function (data) {
                    var callback = eval("(" + data + ")");
                    if (callback.result) {
                        setTimeout(function on() {
                            $('#processlist').append('<li><img alt="成功" src="images/ok.gif"/>初始数据添加成功......</li>');
                        }, ++runstep * steptime);
                        showSuccessLink();
                    }
                    else {
                        setTimeout(function on() {
                            $('#processlist').append('<li><img alt="失败" src="images/error.gif"/>内部异常,' + callback.message + ',请检查......</li>');
                        }, ++runstep * steptime);
                    }
                }
            );
        }

        function showSuccessLink() {
            setTimeout(function on(){
                $('#successlink').attr('class', 'next');
                $('#successlink').attr('href', '../index.aspx');
                $('#processlist').append('<li><img alt="成功" src="images/ok.gif"/>安装成功,点击"完成"进入首页......</li>');
            }, ++runstep * steptime);
        }

        runinstall();
    </script>
    <%} %>
	<div class="copy">
		北京康盛新创科技有限责任公司 &copy; 2001 - 2011 Comsenz Inc. 
	</div>
</div>
</body>
</html>