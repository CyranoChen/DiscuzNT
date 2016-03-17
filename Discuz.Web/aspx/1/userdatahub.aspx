<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.userdatahub" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/6/2 16:12:44.
		本页面代码由Discuz!NT模板引擎生成于 2011/6/2 16:12:44. 
	*/

	base.OnInit(e);

	templateBuilder.Capacity = 220000;
	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>用户发帖信息保存功能页面</title>\r\n    <meta content=\"用户发帖信息保存功能页面，用来解决当论坛版块设置了重写目录的情况下，保存的userdata无法在posttopic等页面中被读取出来，因为userdata不能跨目录\" />\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/common.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_report.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_utils.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n</head>\r\n<body>\r\n    <script type=\"text/javascript\">\r\n        function saveDataInIFrame() {\r\n            var obj = window.parent.document.getElementById('");
	templateBuilder.Append(formname.ToString());
	templateBuilder.Append("');\r\n            if (!obj)\r\n                return;\r\n            var data = subject = message = '';\r\n            for (var i = 0; i < obj.elements.length; i++) {\r\n                var el = obj.elements[i];\r\n                if (el.name != '' && (el.tagName == 'SELECT' || el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && (el.type == 'text' || el.type == 'checkbox' || el.type == 'radio' || el.type == 'hidden' || el.type == 'select')) && el.name.substr(0, 6) != 'attach') {\r\n                    var elvalue = el.value;\r\n                    if (el.name == '");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("') {\r\n                        subject = trim(elvalue);\r\n                    } else if (el.name == '");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("') {\r\n                        if (typeof wysiwyg != 'undefined' && wysiwyg == 1) {\r\n                            elvalue = html2bbcode(editdoc.body.innerHTML);\r\n                        }\r\n                        message = trim(elvalue);\r\n                    }\r\n\r\n                    if ((el.type == 'checkbox' || el.type == 'radio') && !el.checked) {\r\n                        continue;\r\n                    } else if (el.tagName == 'SELECT') {\r\n                        elvalue = el.value;\r\n                    } else if (el.type == 'hidden') {\r\n                        if (el.id) {\r\n                            eval('var check = typeof ' + el.id + '_upload == \\'function\\'');\r\n                            if (check) {\r\n                                elvalue = elvalue;\r\n                                if ($(el.id + '_url')) {\r\n                                    elvalue += String.fromCharCode(1) + $(el.id + '_url').value;\r\n                                }\r\n                            } else {\r\n                                continue;\r\n                            }\r\n                        } else {\r\n                            continue;\r\n                        }\r\n                    }\r\n                    if (trim(elvalue)) {\r\n                        data += el.name + String.fromCharCode(9) + el.tagName + String.fromCharCode(9) + el.type + String.fromCharCode(9) + elvalue + String.fromCharCode(9, 9);\r\n                    }\r\n                }\r\n            }\r\n            if (!subject && !message && !ignoreempty) {\r\n                return;\r\n            }\r\n            saveUserdata('forum', data);\r\n        }\r\n        saveDataInIFrame();\r\n    </");
	templateBuilder.Append("script>\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
