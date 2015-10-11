<%@ Page Language="c#" Inherits="Discuz.Install.install" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Discuz!NT安装</title>
<meta name="keywords" content="Discuz!NT安装" />
<meta name="description" content="Discuz!NT安装" />
<meta name="generator" content="Discuz!NT 3.1.0" />
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
    var message = "";
    var urlStr = "";
    var dbExists = false;
    var ischecked = function() {

        if ($("#cb_newDatabase").attr("checked")) {
            urlStr = "type=checked" + postData();
            ajaxCheck(urlStr);
            if (!flag && !dbExists) {
                Boxy.alert(message, false, { width: 400 });
                return false;
            }
            else if (dbExists) {
            Boxy.confirm(message, function() { __doPostBack('next', ''); })
                return false;
            }
        }
        else {
            urlStr = "type=unchecked" + postData();
            ajaxCheck(urlStr);
            if (!flag) {
                Boxy.alert(message, false, { width: 400 });
                return false;
            }
        }
        return true;
    }
    var ajaxCheck = function(dataStr) {

        $.ajax(
        {
            type: "GET",
            async: false,
            url: window.location.href,
            dataType: "json",
            data: dataStr,
            success: function(data) {
                flag = data.Result;
                message = data.Message;
                dbExists = data.Exists;
            }
        });
    }

    function postData() {
        var post = "";
        $(":text").each(function() {
            post += "&" + this.name + "=" + this.value;
        });
        $(":password").each(function() {
        post += "&" + this.name + "=" + this.value;
        });
        return post;
    }
    
function checkid(obj,id)
{
    var v = obj.value;
    if(v.length == 0)
        $('#isnull_' + id).show();
    else
        $('#isnull_' + id).hide();
}
function isSelected() {
    if ($("#cb_newDatabase").attr("checked")) {
        $('#newDatabase').show();
        $('#confirmPassword').show();
    }
    else {
        $('#newDatabase').hide();
        $('#confirmPassword').hide();
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
				<li>论坛设置信息</li>
				<li>安装</li>
			</ul>
			<div class="copy">北京康盛新创科技有限责任公司</div>
		</div>
	</div>
	<div class="main s_clear">
		<div class="content">
			<h1>数据库信息</h1>
			<div class="info">请认真填写下面的数据库信息：</div>
			<div class="inner">
				<div class="hint_info">自动创建数据库选项适用于独立主机或本地安装;选择自动创建数据库时,如果数据库用户名不存在,系统将自动创建;虚拟主机的用户安装时,请不要勾选"自动创建数据库"
				</div>
				<form id="form_sql" action="step3.aspx"  method="post" runat="server">
					<table cellspacing="0" cellpadding="0" summary="setup" class="setup">
					<tr>
						<td class="title">数据库名称:</td>
						<td>
						    <input name="sql_name" id="sql_name" class="txt" type="text" onblur="checkid(this,'sql_name')"  value="<%=dataBaseName %>"><span id='isnull_sql_name' style="color:#ff0000;display:none">此处不能为空！</span>
						    <input name="cb_newDatabase" id="cb_newDatabase" type="checkbox" onclick="isSelected()" runat="server">自动创建数据库
						</td>
					</tr>	
					<tbody id="newDatabase" class="other_item" style="display:none">
						<tr>
							<td class="title">数据库管理帐号:</td>
							<td>
							    <input name="sql_manager" id="sql_manager" class="txt" type="text" value="<%=Request.Form["sql_manager"]%>" onblur="checkid(this,'sql_manager')">
							    <span id='isnull_sql_manager' style="color:#ff0000;display:none">此处不能为空！</span>
							    <span style="color:#CCCCCC">例如：sa(创建数据库时使用)</span>
							</td>
						</tr>
						<tr>
							<td class="title">数据库管理帐号密码:</td>
							<td>
							    <input name="sql_managerpassword" id="sql_managerpassword" class="txt" type="password" value="<%=Request.Form["sql_managerpassword"]%>" onblur="checkid(this,'sql_managerpassword')"><span id='isnull_sql_managerpassword' style="color:#ff0000;display:none">此处不能为空！</span>
							</td>
						</tr>
						<tr>
						    <td colspan="2"><div style="border-top:1px dashed #cccccc;height: 1px;overflow:hidden"></div></td>
						</tr>
					</tbody>
					<tbody>
											
						<tr>
							<td class="title">数据库用户名:</td>
							<td>
							    <input name="sql_username" id="sql_username" class="txt" type="text" onblur="checkid(this,'sql_username')"  value="<%=sqlUID %>">
							    <span id='isnull_sql_username' style="color:#ff0000;display:none">此处不能为空！</span>
							    <span style="color:#CCCCCC">连接数据库时使用</span>
							</td>
						</tr>
						<tr>
							<td class="title">数据库密码:</td>
							<td>
							    <input name="sql_password" id="sql_password" class="txt" type="password" onblur="checkid(this,'sql_password')"  value="<%=sqlPassword %>">
							    <span id='isnull_sql_password' style="color:#ff0000;display:none">此处不能为空！</span>
							</td>
						</tr>
						<tbody id="confirmPassword" style="display:none">
						    <tr >
							    <td class="title">数据库密码确认:</td>
							    <td>
							        <input name="sql_confirmPassword" id="sql_confirmPassword" class="txt" type="password" onblur="checkid(this,'sql_password_second')" value="<%=sqlPasswordConfirm %>">
							        <span id='isnull_sql_password_second' style="color:#ff0000;display:none">此处不能为空！</span>
							        <span style="color:#CCCCCC">密码不支持复制粘贴</span>
							    </td>
						    </tr>
						</tbody>
						<tr>
						    <td colspan="2"><div style="border-top:1px dashed #cccccc;height: 1px;overflow:hidden"></div></td>
						</tr>
						<tr>
							<td class="title">表前辍:</td>
							<td>
							    <input name="table_prefix" id="table_prefix" class="txt" type="text" onblur="checkid(this,'table_prefix')" value="<%=tablePrefix %>"><span id='isnull_table_prefix' style="color:#ff0000;display:none">此处不能为空！</span>
							</td>
						</tr>
						<tr>
							<td class="title">数据库服务器:</td>
							<td >
							    <input name="sql_ip" id="sql_ip" class="txt" type="text" onblur="checkid(this,'sql_ip')"  value="<%=sqlServerIP %>">
							    <span id='isnull_sql_ip' style="color:#ff0000;display:none">此处不能为空！</span>
							    <span style="color:#CCCCCC">例如：127.0.0.1,(local),"."</span>
							</td>
						</tr>
					</tbody>
					</table>
					<input type="hidden" id="Hidden1" name="Hidden1" value="0" />
					<input type="hidden" id="secondSubmit" name="secondSubmit" value="0" />
				</form>
			</div>
		</div>
		<div class="btn_box">
		    <button type="submit" name="next" id="next" value="true" onserverclick="CheckConnection" runat="server" onclick="if(!ischecked()) return false;">下一步</button>
		</div>
	</div>
</div>
<asp:Literal id="MessageInfo" runat="server" />
</body>
</html>