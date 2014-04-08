<%@ Page language="c#" Inherits="Discuz.Web.Admin.aggregation_recommendforums" Codebehind="aggregation_recommendforums.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
<title>推荐版块选择</title>
<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript">  
<!-- Begin  
sortitems = 0;   // Automatically sort items within lists? (1 or 0)  
function move(fbox,tbox,movemod) 
{  
for(var i = 0; i < fbox.options.length; i++) 
{  
	if((fbox.options[i].selected || movemod) && fbox.options[i].value != "") 
	{  
		for(var j = 0 ; j < tbox.options.length ; j++)
		{
			if(tbox.options[j].value == fbox.options[i].value)
				return false;
		}
		var no = new Option();
		no.value = fbox.options[i].value;  
		no.text = unescape(escape(fbox.options[i].text).replace(/%A0/ig,""));  
		tbox.options[tbox.options.length] = no;  
		//fbox.options[i].value = "";  
		//fbox.options[i].text = "";  
	}  
}  
BumpUp(fbox);  
if (sortitems)
{
	SortD(tbox);
} 
}  

function BumpUp(box)
{  
for(var i=0; i<box.options.length; i++) 
{  
	if(box.options[i].value == "")
	{  
		for(var j=i; j<box.options.length-1; j++)
		{  
			box.options[j].value = box.options[j+1].value;  
			box.options[j].text = box.options[j+1].text;  
		}  
		var ln = i;  
		break;  
	}  
}  
if(ln < box.options.length)
{  
	box.options.length -= 1;  
	BumpUp(box);  
}  
}  
function SortD(box)
{  
var temp_opts = new Array();  
var temp = new Object();  
for(var i=0; i<box.options.length; i++)
{  
	temp_opts[i] = box.options[i];  
}  
for(var x=0; x<temp_opts.length-1; x++)
{  
	for(var y=(x+1); y<temp_opts.length; y++)
	{  
		if(temp_opts[x].text > temp_opts[y].text)
		{  
			temp = temp_opts[x].text;  
			temp_opts[x].text = temp_opts[y].text;  
			temp_opts[y].text = temp;  
		}  
	}  
}  
for(var i=0; i<box.options.length; i++)
{  
	box.options[i].value = temp_opts[i].value;  
	box.options[i].text = temp_opts[i].text;  
}  
}  
function restr(rbox,tbox,str)
{  
if(tbox.options.length)
{  
	rbox.value = tbox.options[0].value;  
	for(var i=1; i<tbox.options.length; i++) 
	{  
		rbox.value = rbox.value+str+tbox.options[i].value;  
	}  
}
else
{
	rbox.value = "";
}
} 

//   排序：向上移动  
function Up(sel)
{
var nIndex = sel.selectedIndex;  
var nLen = sel.length;  
if((nLen < 1) || (nIndex == 0))
{
	return; 
} 
if(nIndex < 0)
{  
	alert("请选择一个要移动的已选按钮！");  
	return;  
}  
var sValue = sel.options[nIndex].value;  
var sHTML = sel.options[nIndex].innerHTML;  
sel.options[nIndex].value = sel.options[nIndex - 1].value;  
sel.options[nIndex].innerHTML = sel.options[nIndex - 1].innerHTML;  
sel.options[nIndex - 1].value = sValue;  
sel.options[nIndex - 1].innerHTML = sHTML;  
sel.selectedIndex = nIndex - 1;  
}  

//   排序：向下移动  
function Down(sel)
{  
var nIndex = sel.selectedIndex;  
var nLen = sel.length;  
if((nLen < 1) || (nIndex == nLen - 1))
{
	return; 
} 
if(nIndex < 0)
{  
	alert("请选择一个要移动的已选按钮！");  
	return;  
}  
var sValue = sel.options[nIndex].value;  
var sHTML = sel.options[nIndex].innerHTML;  
sel.options[nIndex].value = sel.options[nIndex + 1].value;  
sel.options[nIndex].innerHTML = sel.options[nIndex + 1].innerHTML;  
sel.options[nIndex + 1].value = sValue;  
sel.options[nIndex + 1].innerHTML = sHTML;  
sel.selectedIndex = nIndex + 1;  
}

function DelteItem(sel)
{
var nIndex = sel.selectedIndex;
var nLen = sel.length; 
sel.remove(nIndex);
if(nIndex < (nLen - 1))
{
	sel.selectedIndex = nIndex;
}
else
{
	sel.selectedIndex = sel.length - 1;
}
}

//移动到最后
function lastOnClick(sel)
{ 
var nIndex = sel.selectedIndex;  
var nLen = sel.length;  
if((nLen < 1) || (nIndex == nLen - 1))
{
	return;  
}
if(nIndex < 0)
{  
	alert("请选择一个要移动的已选按钮！");  
	return;  
}  
var sValue = sel.options[nIndex].value;  
var sHTML = sel.options[nIndex].innerHTML;
var nOption = document.createElement('OPTION');  
sel.options.add(nOption);  
nOption.innerHTML = sHTML;  
nOption.Value = sValue;  
sel.remove(nIndex);  
sel.selectedIndex = nLen - 1;  
}

//移动到最前
function firstOnClick(sel)
{  
var nIndex = sel.selectedIndex;  
var nLen = sel.length;  
if((nLen < 1) || (nIndex == 0))
{
	return;
}
if(nIndex < 0)
{  
	alert("请选择一个要移动的已选按钮！");  
	return;  
}  
var sValue = sel.options[nIndex].value;  
var sHTML = sel.options[nIndex].innerHTML;  
sel.remove(nIndex);  
var nOption = document.createElement('OPTION');  
sel.options.add(nOption,0);  
nOption.innerHTML = sHTML;  
nOption.Value = sValue;  
sel.selectedIndex = 0;  
}

function validate(form)
{
restr(form.rst,form.list2,',');
return true;
}
// End -->  
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<uc1:PageInfo id="info1" runat="server" Icon="Information" Text="<li>第一步:选择要推荐的版块</li><li>第二步:为推荐版块设置推荐主题及图片链接</li>"></uc1:PageInfo>
<div class="ManagerForm">
<fieldset>
<legend style="background:url(../images/legendimg.jpg) no-repeat 6px 50%;">推荐版块选择</legend>
<table border="0" align="center">  
	<tr>  
		<td>可选版块：<br /><cc1:ListBoxTreeList id="list1" runat="server"></cc1:ListBoxTreeList></td>  
		<td>  
			<input type="button" value=">" onclick="move(this.form.<%=list1.TypeID.ClientID %>,this.form.list2,0)" style="font-weight:bold;width:32" /> 
		</td>  
		<td>已选版块：<br /><select multiple="multiple" size="8" id="list2" name="list2"  style="width:260px;height:290px;"></select></td>
		<td>
			<input type="button" value="移至最前" onclick="firstOnClick(this.form.list2)" /><br />
			<input type="button" value='上移一位' onclick="Up(this.form.list2)" /><br /><br />
			<input type="button" value='移除项目' onclick="DelteItem(this.form.list2)" /><br /><br />
			<input type="button" value='下移一位' onclick="Down(this.form.list2)" /><br />
			<input type="button" value="移至最后" onclick="lastOnClick(this.form.list2)" />
		</td>
	</tr>  
</table>  
<input type="hidden" name="rst" value="" />  
</fieldset><br />
<div align="center">
<cc1:Button id="Btn_SaveInfo" runat="server" Text="  保存  " ButtonImgUrl="../images/submit.gif" ValidateForm="true"></cc1:Button>
<button class="ManagerButton" type="button" onclick="javascript:window.location.href='aggregation_recommendtopic.aspx';"><img src="../images/submit.gif" />返回</button>
</div>
</div>
</form>
<%=footer%>
</body>
</html>
