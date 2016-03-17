<%@ Page language="c#" Inherits="Discuz.Web.Admin.shortcut" Codebehind="shortcut.aspx.cs"%>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<%@ Import NameSpace="Discuz.Common"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>shortcut</title>
		<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
		<link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
		<script type="text/javascript" src="../js/common.js"></script>
        <script type="text/javascript" src="../js/modalpopup.js"></script>
		<script type="text/javascript">       
            var currentid=0;
            var bar=0;
            var filenameliststr='<%=filenamelist%>';
            var filenamelist=new Array();            
            filenamelist= filenameliststr.split('|');
            
            function runstatic()
            {  
                 if(filenamelist[currentid]!="")
                 {
		             document.getElementById('Layer5').innerHTML =  '<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在更新'+filenamelist[currentid]+'.htm模板, 请稍等...<BR /></td></tr></table><BR />';
                     document.getElementById('Layer5').style.witdh='350';
                     document.getElementById('success').style.witdh='400';
                     document.getElementById('success').style.display ="block";
                     getReturn('createtemplate.aspx?type=single&path=' + document.getElementById('Templatepath').value + '&filename=' + filenamelist[currentid]);
                     currentid++;
                 }
                 else
                 {
                    document.getElementById('Layer5').innerHTML="<br />模板更新成功, 请稍等...";
                    document.getElementById('success').style.display = "block";
		            count(); 
		            document.getElementById('Form1').submit();
                 }
           }
           
           function count()
           { 
		        bar=bar+2;
		        if (bar<99) {setTimeout("count()",100);} 
		        else { document.getElementById('success').style.display ="none"; } 
           }
           
           function run()
           {
              bar=0;
              document.getElementById('Layer5').innerHTML="<BR /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在提交数据, 请稍等...<BR /></td></tr></table><BR />";
              document.getElementById('success').style.display = "block";
              setInterval('runstatic()',5000); //每次提交时间为6秒
           }
           
           function validateform(theform)
           {
              document.getElementById('Form1').submit();
 	          return true;
           }
           
           function validate(theform)
           {
              if(document.getElementById('createtype').checked)
              {
                  run();
                  return false;
              }
              else
              {
 	              return true;
	          }
           }
           
            function showInfo()
            { 
		        BOX_show('upgradelayer'); 
            }
    
   </script>
		
	<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
	<body>
		<form id="Form1" method="post" runat="server">
		<table cellspacing="0" cellpadding="4" width="100%" align="center">
	        <tr>
                <td class="panelbox">
                    <table width="100%">
                        <tr>
                            <td style="width:120px">编辑用户:</td>
                            <td style="width: 120px"><cc1:textbox ID="Username" runat="server" RequiredFieldType="暂无校验" width="200"></cc1:textbox></td>
                            <td><cc1:Button ID="EditUser" runat="server" Text="提 交"></cc1:Button></td>
                        </tr>                        
                        <tr>
                            <td>编辑论坛:</td>
                            <td><cc1:dropdowntreelist ID="forumid" runat="server"></cc1:dropdowntreelist></td>
                            <td><cc1:Button ID="EditForum" runat="server" Text="提 交"></cc1:Button></td>
                        </tr>                        
                        <tr>
                            <td>编辑用户组:</td>
                            <td><cc1:dropdownlist id="Usergroupid" runat="server"></cc1:dropdownlist></td>
                            <td><cc1:Button id="EditUserGroup" runat="server" Text="提 交"></cc1:Button></td>
                        </tr>
                        <tr>
                            <td>生成模板:</td>
                            <td><cc1:dropdownlist id="Templatepath" runat="server"></cc1:dropdownlist> <input type="checkbox" id="createtype" name="createtype" >降低CPU占用</td>
                            <td><cc1:Button id="CreateTemplate" runat="server" Text="提 交" ValidateForm="true"></cc1:Button></td>
                        </tr>
                    </table>
                </td>
            </tr>
			<tr>
				<td colspan="3">
				<a class="ManagerButton" onclick="javascript:window.open('http://nt.discuz.net/doc/','','');"><strong>访问官方文档中心</strong></a>
				<a class="ManagerButton" onclick="javascript:window.open('http://nt.discuz.net/','','');" ><strong>访问官方论坛</strong></a>
				<asp:LinkButton id="UpdateCache" CssClass="ManagerButton" runat="server" Text="<span>更新缓存</span>"></asp:LinkButton>
				<asp:LinkButton id="UpdateForumStatistics" CssClass="ManagerButton" runat="server" Text="<span>更新论坛统计</span>"></asp:LinkButton>
				</td>
			</tr>
        </table>
	    <fieldset id="info" style="overflow:hidden;zoom:1;margin-top:10px;background-color:#f5f7f8;">
            <legend style="padding: 0pt 10px; margin-left: 25px;font-size:13px;color:#000;">官方消息</legend>
	        <iframe  width="100%" height="110px" id="checkveriframe" framespacing="0px" style="border-width: 0px;"  frameborder="0px"  noResize ></iframe>
	    </fieldset>
	    <fieldset style="overflow:hidden;zoom:1;margin-top:10px;background-color:#f5f7f8;padding:10px">
	    <legend style="padding: 0pt 10px; margin-left: 25px;font-size:13px;color:#000;">系统信息</legend>
	    <table width="100%">
	        <tr>
	            <td>服务器名称:</td><td align="left"><%=Server.MachineName%></td>
	            <td>服务器操作系统:</td><td align="left"><%=Environment.OSVersion.ToString()%></td>
	        </tr>
	        <tr>
	            <td>服务器IIS版本:</td><td align="left"><%=Request.ServerVariables["SERVER_SOFTWARE"] %></td>
	            <td>.NET解释引擎版本:</td><td align="left">.NET CLR  <%=Environment.Version.Major %>.<%=Environment.Version.Minor %>.<%=Environment.Version.Build %>.<%=Environment.Version.Revision %></td>
	        </tr>
	    </table>
	    </fieldset>
	    <fieldset style="overflow:hidden;zoom:1;margin-top:10px;background-color:#f5f7f8;padding:10px;">
	    <legend style="padding: 0pt 10px; margin-left: 25px;font-size:13px;color:#000;">开发团队</legend>
	    <table width="100%">
	        <tr>
	            <td>
	            <a href="http://nt.discuz.net/space/?uid=53152" target="_blank">doopcl</a>,
	            <a href="http://nt.discuz.net/space/?uid=2" target="_blank">daizhj</a>,
	            <a href="http://nt.discuz.net/space/?uid=9216" target="_blank">fanzjgw</a>,
	            <a href="http://nt.discuz.net/space/?uid=2692" target="_blank">like</a>,
	            <a href="http://nt.discuz.net/space/?uid=7899" target="_blank">sunshili</a>,
	            <a href="http://nt.discuz.net/space/?uid=19841" target="_blank">旭雨</a>,
                <a href="http://nt.discuz.net/space/?uid=7133" target="_blank">wysky</a>,
                <a href="#">Scorpion</a>
	            </td>
	        </tr>
	    </table>
	    </fieldset>	
        <fieldset style="overflow:hidden;zoom:1;margin-top:10px;background-color:#f5f7f8;padding:10px;">
	    <legend style="padding: 0pt 10px; margin-left: 25px;font-size:13px;color:#000;">特别感谢</legend>
	    <table width="100%">
	        <tr>
	            <td>
	            <a href="http://nt.discuz.net/space/?uid=12" target="_blank">雪人</a>,
	            <a href="http://nt.discuz.net/space/?uid=5691" target="_blank">大川</a>,
                <a href="http://nt.discuz.net/space/?uid=1180" target="_blank">bighead</a>,
	            <a href="http://nt.discuz.net/space/?uid=6233" target="_blank">戏水</a>,
                <a href="http://nt.discuz.net/space/?uid=36838" target="_blank">coolpest</a>
	            </td>
	        </tr>
	    </table>
	    </fieldset>	
	     <fieldset style="overflow:hidden;zoom:1;margin-top:10px;background-color:#f5f7f8;padding:10px;">
	    <legend style="padding: 0pt 10px; margin-left: 25px;font-size:13px;color:#000;">相关链接</legend>
	    <table width="100%">
	        <tr>
	            <td>
	            <a href="http://www.comsenz.com" target="_blank">公司网站</a>, 
	            <a href="http://www.comsenz.com/purchase/nt" target="_blank">购买授权</a>, 
	            <a href="http://www.comsenz.com/products/nt" target="_blank">Discuz!NT 产品</a>, 
	            <a href="http://nt.discuz.net/showforum-17.html" target="_blank">模板</a>, 
	            <a href="http://nt.discuz.net/showforum-22.html" target="_blank">插件</a>, 
	            <a href="http://nt.discuz.net/doc/" target="_blank">文档</a>, 
	            <a href="http://nt.discuz.net/" target="_blank">讨论区</a>
	            </td>
	        </tr>
	    </table>
	    </fieldset>
	    <div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
            <div id="upgradelayer" style="display:none;background :#fff; padding:10px; border:1px solid #999; width:500px;">
            <div class="ManagerForm">
			    <fieldset>
		        <legend style="background:url(../images/icons/icon60.jpg) no-repeat 6px 50%;">在线更新</legend>
		        <span id="upgradeMessage" style="float:left;text-align:left;"><%= upgradeInfo%></span>
		        </fieldset>
		        <span style="float:left;text-align:right;width:400px;">
		        <button<%= isNew?"":" disabled=\"true\"" %> id="update" class="ManagerButton" type="button" onclick="window.location='onlineupgrade.aspx'"><img src="../images/submit.gif" />版本升级</button>&nbsp;&nbsp;
		        <button<%= isHotFix?"":" disabled=\"true\"" %> id="hotfix" class="ManagerButton" type="button" onclick="window.location='hotfix.aspx'"><img src="../images/submit.gif" />补丁升级</button>&nbsp;&nbsp;
		        <button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('upgradelayer');"><img src="../images/state1.gif"/> 取 消 </button>
		        </span>
		    </div>
            </div>
		    <div id="setting" />
		</form>
		<script type="text/javascript">
		    document.getElementById("info").style.height = "130px";
		</script>
		<%=footer%>
	</body>
</html>
