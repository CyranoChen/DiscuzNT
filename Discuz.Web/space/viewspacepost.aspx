<%@ Page language="c#" Codebehind="viewspacepost.aspx.cs" AutoEventWireup="false" Inherits="Discuz.Space.viewspacepost" %>
<%@ Register TagPrefix="uc1" TagName="ajaxviewuserpost" Src="manage/usercontrols/ajaxviewuserpost.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxuserspacecommentlist" Src="manage/usercontrols/ajaxuserspacecommentlist.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxsubmitcomment" Src="manage/usercontrols/ajaxsubmitcomment.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxspaceconfigstatic" Src="manage/usercontrols/ajaxspaceconfigstatic.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxspacelink" Src="manage/usercontrols/ajaxspacelink.ascx" %>
<%@ Register TagPrefix="uc1" TagName="spacecalendar" Src="manage/usercontrols/spacecalendar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxtopnewpost" Src="manage/usercontrols/ajaxtopnewpost.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ajaxtopnewcomment" Src="manage/usercontrols/ajaxtopnewcomment.ascx" %>
<%@ Register TagPrefix="uc1" TagName="frontleftnavmenu" Src="manage/usercontrols/frontleftnavmenu.ascx" %>

<%@ Register TagPrefix="uc1" TagName="fronttop" Src="manage/usercontrols/fronttop.ascx" %>
<%@ Register TagPrefix="uc1" TagName="frontbottom" Src="manage/usercontrols/frontbottom.ascx" %>


<html>
	<head>
	    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="keywords" content="ASP.net,论坛,space,blog" />
		<meta name="description" content="Discuz!NT <% = config.Spacename%>" />
		<title><%=spacepostinfo.Title %> - <%=spaceconfiginfo.Spacetitle%> - <%=config.Forumtitle%><% = config.Spacename%> - Powered by Discuz!NT</title>
		
		<link href="skins/themes/space.css" rel="stylesheet" type="text/css" id="css" />
		<link rel="stylesheet" type="text/css" href="skins/themes/blog.css" />
		<link rel="stylesheet" type="text/css" href="skins/themes/<%=spaceconfiginfo.ThemePath%>/style.css" />
		<script type="text/javascript">var forumpath="<%=forumpath %>space/";var aspxrewrite="<%=config.Aspxrewrite %>";</script>
		<script type="text/javascript" src="javascript/space.js"></script>
		<script src="manage/js/AjaxHelper.js" type="text/javascript"></script>
		<script type="text/javascript" src="manage/js/common.js"></script>
		<style>
		.modbox {margin:5px 5px;}
		.col {vertical-align:top;height:100px;}
		#category{
				line-height:240%;
		}
		#category li{
				border-bottom:1px solid #eee;
				text-align:center;
		}
		#list h3{
				font-weight:bold;
		}
		.AddModule{
				line-height:30px;
		}
		.AddModule span{
						padding:2px;
						border:1px solid #333;
		}
		.PageList strong{
						color:#333;
		}
		</style>
	</head>
    <body>

<uc1:fronttop id="fronttop1" runat="server"></uc1:fronttop>

<div id="modules">
<table width="100%" align="center" cellpadding="0" cellspacing="0" id="t_1">
	<tr>
		<td valign="top" class="viewcol" id="col_2" style="width: 100%;">
		<div id = "bodyrightcontent">
			<div class="modbox" >
			        <div id="blognavigator">
			        <a href="<% = forumurl%>"><%=config.Forumtitle%></a>
			        >
			        <a href="<% = configspaceurl%>"><% = config.Spacename%>首页</a>
			        >
			        <a href="<% = spaceurl %>"><% = spaceconfiginfo.Spacetitle %></a>
			        >
			        <a href="viewspacepostlist.aspx?spaceid=<% = spaceid %>">日志</a>
			        </div>
					<div id="viewuserpost" >
						<uc1:ajaxviewuserpost id="ajaxviewuserpost1" runat="server" Showasajax="false"></uc1:ajaxviewuserpost>
					</div>
			</div>

            <%
                if (spacepostinfo.Postid != 0)  
                {  
            %>
			<div class="modbox">
			        <a name="comments"></a>
					<div id="usercommentlist">
						<uc1:ajaxuserspacecommentlist id="ajaxuserspacecommentlist1" runat="server"></uc1:ajaxuserspacecommentlist>
					</div>
			</div>
	
			<div class="modbox" style="position: relative;">
					<div id="submitcomment">
						<uc1:ajaxsubmitcomment id="ajaxsumbitcomment1" runat="server"></uc1:ajaxsubmitcomment>
					</div>
					<div id="completesubmitcomment" style="position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;display:none;">
						<div id="completesubmitcomment2" style="position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;">
							<div id="completesubmitcomment3" style="height:26px;background:#D3DBDE;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder; color:#000; "><img id="close" style="float:right;margin:6px; " border="0"  src="images/close.gif" onclick="javascript:document.getElementById('completesubmitcomment').style.display ='none';" alt="点击关闭" />操作提示 </div> 
							<div id="returnmsg" style="height:64px;line-height:150%;padding:0px 3px 0px 3px;vertical-align:middle;"></div>
						</div>
					    <div id="completesubmitcomment4" style="position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: transparent; color:#000;"></div>
			        </div>
			</div>
			<%
			    }
			%>
			
			
		</div>
		</td>
	</tr>
</table>

</div>
</div>


<div id="footer">
	<uc1:frontbottom id="frontbottom1" runat="server"></uc1:frontbottom>
</div>
		
		
	
    </body>
	
</html>


<script type="text/javascript">	


var urlparam="load=true&spaceid=" + <%=spaceid%> + "&postid=" + <%=postid%>;

//当前日志的评论数
var commentcount = <%=commentcount%> ; 

  
AjaxProxyUrl = new String("manage/ajax.aspx");
  
AjaxHelper.Updater('usercontrols/ajaxviewuserpost.ascx','viewuserpost',urlparam, parsetag);
  
//加载用户日志评论列表
AjaxHelper.Updater('usercontrols/ajaxuserspacecommentlist.ascx','usercommentlist',urlparam);
    
//加载提交评论页面
AjaxHelper.Updater('usercontrols/ajaxsubmitcomment.ascx','submitcomment',urlparam);

//加载数据统计页面
AjaxHelper.Updater('usercontrols/ajaxspaceconfigstatic.ascx','infomation',urlparam);

//加载友情链接页面
AjaxHelper.Updater('usercontrols/ajaxspacelink.ascx','userlink',urlparam);

//加载最新日志列表
AjaxHelper.Updater('usercontrols/ajaxtopnewpost.ascx','ajaxtopnewpost',urlparam);

//最新评论
AjaxHelper.Updater('usercontrols/ajaxtopnewcomment.ascx','ajaxtopnewcomment',urlparam);


function submitcommentinf(postid, userid)
{
   	//回复信息
	var commentcontent = document.getElementById('commentcontent').value;
	var commentauthor = document.getElementById('commentauthor').value;
	var commentemail =  document.getElementById('commentemail').value;
	var commenturl =  document.getElementById('commenturl').value;
	var vcode =  document.getElementById('vcode').value;
	var notice =  document.getElementById('notice').checked;

	document.getElementById('replyuserid').value = 0;//恢复初始值
		
	if(commentcontent=="")
	{
	    alert('请输入评论内容!');
	    return ;
	}
	document.getElementById('completesubmitcomment').style.display = "none";
	AjaxHelper.Updater('usercontrols/ajaxsubmitcomment.ascx','','load=true&submit=true&postid=' + postid + '&userid=' + userid + '&commentcontent=' + commentcontent + '&commentauthor='+commentauthor+'&commentemail='+commentemail+'&commenturl='+commenturl+'&vcode='+vcode + '&notice=' + notice,PostCommentComplete);
} 

function RefreshCommentCount(number)
{
	//评论数加1
	commentcount = commentcount + number;
	try{
	document.getElementById('commentcount').innerHTML=commentcount;  
	}
	catch (e){}
}

function PostCommentComplete(result)
{
    if(result.indexOf('completeinfo')<0)
    {
		RefreshCommentCount(1);
     
 		document.getElementById('commentcontent').value= ''; 
		AjaxHelper.Updater('usercontrols/ajaxuserspacecommentlist.ascx','usercommentlist','load=true&postid=<%=postid%>&currentpage=1');
	}
    else
    {
		document.getElementById('returnmsg').innerHTML='<table height=\"100%\" width=\"100%\"><tr><td><img border=\"0\" src=\"manage/images/hint.gif\"  /></td><td style=\"font-size: 14px;color:#000;\" >'+result.split('|')[1]+'</td></tr></table>';
	    document.getElementById('completesubmitcomment').style.display = "block"; 
	}
	
	count();
	bar=0;
}


var bar=0;
function count()
{ 
	bar=bar+1;
	if (bar<5)
	{
	    setTimeout("count()",50);
	} 
	else
	{ 
		document.getElementById('vcodeimg').src='<% = forumurlnopage%>/tools/VerifyImagePage.aspx?id=<%=olid.ToString()%>&time=' + Math.random();
	    document.getElementById('vcode').value = ''; 
	} 
}

function quicksubmit(event,postid,userid)
{
   if(event.ctrlKey && event.keyCode == 13)
   {
      submitcommentinf(postid,userid)
   }
}

</script>

