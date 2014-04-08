<%@ Page language="c#" Codebehind="UploadFile.aspx.cs" AutoEventWireup="false" Inherits="Discuz.Space.Manage.UploadFile" %>
<%@ Register TagPrefix="uc1" TagName="uploadfile" Src="UserControls/uploadfile.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<head>
		<title>uploadfile</title>
		<%=this.GetPageHeadContent()%>
<style type=text/css>	
body {
	font-size: 12px;
	font-family: Tahoma, Verdana;
	margin: 0px;
	padding: 0px;
	scrollbar-darkshadow-color: #808080;
	scrollbar-face-color: #DDDDDD;
	scrollbar-highlight-color: #EEEEEE;
	scrollbar-shadow-color: #DEE3E7;
	scrollbar-3dlight-color: #D1D7DC;
	scrollbar-arrow-color:  #000000;
	scrollbar-track-color: #EDEDED;
	background-color: transparent;
	
}
</style>		
<script type="text/javascript">

function loadfilelist(httpdir)
{
	var result = '';	
	if(document.getElementById('uploadfile1_UpfileList').value!='')
    {
		var splitedfile = document.getElementById('uploadfile1_UpfileList').value.split('|');
		for(i=0;i<splitedfile.length;i++)
		{
			if(splitedfile[i].indexOf('.jpg')>0||splitedfile[i].indexOf('.gif')>0||splitedfile[i].indexOf('.png')>0||splitedfile[i].indexOf('.jpeg')>0)
			{
				 result  = result + " <input type='button' style='display:none;' onclick=\"javascript:InsertHTML('"+httpdir+splitedfile[i]+"');\" id='insertatt_" + i + "'><a href=\"javascript:void(0);\" onclick=\"javascript:InsertHTML('"+httpdir+splitedfile[i]+"');\">[插入]</a>  <span  style=\"float:none; width: 100%;text-align:left;line-height:100%;position:absolute;\" id=\"span"+splitedfile[i]+"\" onmouseover=\"javascript:showimage('"+splitedfile[i]+"');\" onmouseout=\"javascript:hideimage('"+splitedfile[i]+"');\">"+splitedfile[i]+"</span><div id=\""+splitedfile[i]+"\" style=\"display:none;width:50px;z-index:10000;position:absolute;border:1px solid #B0C4EA;\"><img src=\""+httpdir+splitedfile[i]+"\" width=\"48px\" /></div><br />";
			}
			else
			{
				 result  = result + " <input type='button' style='display:none;' onclick=\"javascript:InsertHTML('"+httpdir+splitedfile[i]+"');\" id='insertatt_" + i + "'><a href=\"javascript:void(0);\" onclick=\"javascript:InsertHTML('"+httpdir+splitedfile[i]+"');\">[插入]</a>  " + splitedfile[i] + "<br />";
			}
		}
		//alert(result);
		var container = window.parent.document.getElementById('postattachfile').innerHTML = result;
		try{
		window.parent.document.getElementById('insertatt_' + (splitedfile.length-1).toString()).click();
		} catch(e){}
		
    }
}    
function parentreload()
{
	parent.location.reload();
}
function noselectfile()
{
    alert('请您选择上传的文件');
}

</script>
	</head>
	<body margin="0px">
		<form id="Form1" method="post" runat="server" >
			<uc1:uploadfile id="uploadfile1" runat="server"></uc1:uploadfile>
		</form>
	</body>
</HTML>
