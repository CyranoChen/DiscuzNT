<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ajaxsubmitcomment.ascx.cs" Inherits="Discuz.Space.Manage.ajaxsubmitcomment" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%if(Discuz.Common.DNTRequest.GetString("load")!="true")
{
    Response.Write(base.WriteLoadingDiv("loadajaxsubmitcomment", "正在加载评论页面<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />"));
}
else
{ 
	if(completeinfo!="")
	{
		Response.Write("<div id=\"completeinfo\" style=\"display:none\">|"+completeinfo+"|</div>");
	}
%>

    <div id="loadajaxsubmitcomment" style="display:none;position:relative;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin:4px;width:99%;valign:middle">
        <table>
	        <tr>
		        <td>
			        <img border="0" src="manage/images/ajax_loading.gif" /></td><td valign=middle>正在载入中,请稍等.....
		        </td>
	        </tr>
        </table>
    </div>
    
    <h2 class="modtitle"><span class="modtitle_text">我来说两句</span></h2>

    <div class="submitComment">
		<p>昵称 <input type="text" size="20" id="commentauthor" name="commentauthor" value="<%=commentauthor%>" onkeydown="javascript:quicksubmit(event,<%=postid%>,<%=userid%>)"></p>
		<div id="regoptions" style="display: none">
    		<p>主页 <input type="text" size="20" id="commenturl" name="commenturl" value="<%=commenturl%>" onkeydown="javascript:quicksubmit(event,<%=postid%>,<%=userid%>)"></p>
    		<p>邮箱 <input type="text" size="20" id="commentemail" name="commentemail" value="<%=commentemail%>" onkeydown="javascript:quicksubmit(event,<%=postid%>,<%=userid%>)"></p>
		</div>
		<p>内容 <textarea id="commentcontent" name="commentcontent" cols="45" rows="6"  maxlength="2000" onkeydown="javascript:quicksubmit(event,<%=postid%>,<%=userid%>)"></textarea></p>
		<p id="NtSpace-seccodeline">
			验证 <input class="colorblur" id="vcode" type="text" size="10" name="vcode" value="<%=vcode%>" onkeydown="javascript:quicksubmit(event,<%=postid%>,<%=userid%>)">
			<img id="vcodeimg" height="50" src="<% = forumurlnopage%>tools/VerifyImagePage.aspx?id=<%=olid.ToString()%>&time=' + Math.random()" width="90" align="absMiddle">&nbsp;
													<input class=colorccc id=reloadvcade onclick="document.getElementById('vcodeimg').src='<% = forumurlnopage%>tools/VerifyImagePage.aspx?id=<%=olid.ToString()%>&amp;time=' + Math.random()" type=button value=刷新验证码 name=reloadvcade>
		</p>
		<p>
		    <input id="replyuserid" name="replyuserid" type="hidden" />
			<input class=colorccc id="submitinfo" name="submitinfo" onclick="javascript:submitcommentinf(<%=postid%>,document.getElementById('replyuserid').value);" type="button" value="提交评论" >
		    &nbsp;<a href="###" onclick="expandoptions('regoptions');" >高级选项</a>
		    &nbsp;<input type="checkbox"  id="notice" name="notice" />发送评论通知<br><br>
		    <LABEL class=CtrlEnter>[使用Ctrl+Enter键可以直接提交]</LABEL>
		</p>
    </div>
    
    <script type="text/javascript">
    function isMaxLen(o)
    {
	    var nMaxLen=o.getAttribute? parseInt(o.getAttribute("maxlength")):"";
	    if(o.getAttribute && o.value.length>nMaxLen)
	    {
		    o.value=o.value.substring(0,nMaxLen);
	    }	 
    }
    </script>
<%}%>