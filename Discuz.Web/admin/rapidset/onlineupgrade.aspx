<%@ Page language="c#" Inherits="Discuz.Web.Admin.onlineupgrade" Codebehind="onlineupgrade.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>在线升级</title>
		<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
		<script type="text/javascript" src="../js/common.js"></script>
		<script type="text/javascript">
		    function createDivElement(innerHTML,idName,idValue)
		    {
		        var divElement = document.createElement("div");
		        divElement.innerHTML = innerHTML;
		        if(idName != undefined)
		        {
		            divElement.setAttribute(idName,idValue);
		        }
		        return divElement;
		    }
		    function updateversion()
		    {
		        var print = $("updateinfo");
		        print.innerHTML = "";
		        var error = false;
		        print.appendChild(createDivElement("<b>正在准备升级......</b>"));
		        for(var i = 0; i < versionList.length; i++)
		        {
		            var result = "";
		            var link = "<br /><a href='" + versionList[i]["link"] + "' target='_blank' title='下载升级包'>请下载" + versionList[i]["versiondescription"] + "升级包手动进行升级</a>";
		            print.appendChild(createDivElement("<b>正在下载升级" + versionList[i]["versiondescription"] + "所需的文件......</b><img id='downupgradefile" + i + "' src='../images/busy.gif' />"));
		            result = getReturn('ajaxupgrade.aspx?op=downupgradefile&upgradetype=required&ver=' + versionList[i]["version"]);
		            if(result != "")
		            {
		                $("downupgradefile" + i).src = "../images/state1.gif";
		                print.appendChild(createDivElement("<p style='padding:0px 0px 0px 20px;'>升级错误:" + result + link + "</p>"));
		                error = true;
		                break;
		            }
		            else
		            {
		                $("downupgradefile" + i).src = "../images/state2.gif";
		            }
		            print.appendChild(createDivElement("<b>正在下载" + versionList[i]["versiondescription"] + "升级包......</b><img id='downzip" + i + "' src='../images/busy.gif' />"));
		            result = getReturn('ajaxupgrade.aspx?op=downzip&upgradetype=required&ver=' + versionList[i]["version"]);
		            if(result != "")
		            {
		                $("downzip" + i).src = "../images/state1.gif";
		                print.appendChild(createDivElement("<p style='padding:0px 0px 0px 20px;'>升级错误:" + result + link + "</p>"));
		                error = true;
		                break;
		            }
		            else
		            {
		                $("downzip" + i).src = "../images/state2.gif";
		            }
		            print.appendChild(createDivElement("<b>正在解压缩" + versionList[i]["versiondescription"] + "升级包......</b><img id='unzip" + i + "' src='../images/busy.gif' />"));
		            result = getReturn('ajaxupgrade.aspx?op=unzip&ver=' + versionList[i]["version"]);
		            if(result != "")
		            {
		                $("unzip" + i).src = "../images/state1.gif";
		                print.appendChild(createDivElement("<p style='padding:0px 0px 0px 20px;'>升级错误:" + result + link + "</p>"));
		                error = true;
		                break;
		            }
		            else
		            {
		                $("unzip" + i).src = "../images/state2.gif";
		            }
		            print.appendChild(createDivElement("<b>正在布署" + versionList[i]["versiondescription"] + "升级包文件......</b><img id='dispose" + i + "' src='../images/busy.gif' />"));
		            result = getReturn('ajaxupgrade.aspx?op=dispose&ver=' + versionList[i]["version"]);
		            if(result != "")
		            {
		                $("dispose" + i).src = "../images/state1.gif";
		                print.appendChild(createDivElement("<p style='padding:0px 0px 0px 20px;'>升级错误:" + result + link + "</p>"));
		                error = true;
		                break;
		            }
		            else
		            {
		                $("dispose" + i).src = "../images/state2.gif";
		            }
		            print.appendChild(createDivElement("<b>正在升级" + versionList[i]["versiondescription"] + "版数据库......</b><img id='runsql" + i + "' src='../images/busy.gif' />"));
		            result = getReturn('ajaxupgrade.aspx?op=runsql&step=1&ver=' + versionList[i]["version"]);
		            if(result != "")
		            {
		                $("runsql" + i).src = "../images/state1.gif";
		                print.appendChild(createDivElement("<p style='padding:0px 0px 0px 20px;'>升级错误:" + result + link + "</p>"));
		                error = true;
		                break;
		            }
		            else
		            {
		                $("runsql" + i).src = "../images/state2.gif";
		            }
		        }
		        if(error)
		        {
		            print.appendChild(createDivElement("<b style='color:red'>升级失败!</b>"));
		        }
		        else
		        {
		            print.appendChild(createDivElement("<b style='color:green'>升级完毕!</b>"));
		            document.getElementById("update").style.display = "none";
		        }
		    }
		</script>
	<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
	<body>
	    <form id="Form1" runat="server">
	    <uc1:PageInfo id="info1" runat="server" Icon="Information"
        Text="您在进行升级之前，&lt;font color='red'&gt;建议备份您的数据库和程序&lt;/font&gt;。升级程序也会为您备份升级前的文件，其文件放置在论坛根目录下cache/upgradebackup目录中。"></uc1:PageInfo>
	        <table width="100%">
	            <tr>
	                <td style="background:url(../images/update.gif) no-repeat 20px 50%; font-weight:bold; text-indent:80px; height:60px;border-bottom:1px dashed #CCC;">你可以随时检查并更新到最新版本！</td>
	            </tr>
	            <tr><td style="padding:10px 20px;"><asp:Label ID="info" runat="server" /></td></tr>
	            <tr>
	                <td style="padding:10px 20px;">
	                    <div id="updateinfo" style="width:800px;height:230px;border: 1px solid rgb(219, 221, 211);padding:5px;overflow:auto;"></div>
	                </td>
	            </tr>
	            <tr>
	                <td style="padding:10px 20px;">
	                    <button<%= isNew?"":" disabled=\"true\"" %> id="update" class="ManagerButton" type="button" onclick="updateversion()"><img src="../images/submit.gif" />立即升级</button>&nbsp;&nbsp;
	                    <button id="hotfix" class="ManagerButton" type="button" onclick="window.location='hotfix.aspx'"><img src="../images/submit.gif" />补丁升级</button>
	                </td>
	            </tr>
	        </table>
        </form>
	</body>
</html>
