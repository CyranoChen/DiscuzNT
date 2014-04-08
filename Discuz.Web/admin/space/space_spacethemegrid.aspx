<%@ Page Language="C#" AutoEventWireup="false" Inherits="Discuz.Space.Admin.SpaceThemeGrid" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Import NameSpace="Discuz.Common"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<HEAD>
<title>论坛图标文件列表</title>		
<LINK href="../styles/datagrid.css" type="text/css" rel="stylesheet">
<LINK href="../styles/dntmanager.css" type="text/css" rel="stylesheet">
<script language="javascript" src="../js/common.js"></script>
<script language="javascript" src="../../javascript/menu.js"></script>
<script language="javascript" src="../../javascript/common.js"></script>
<script type="text/javascript">
function validate()
{
	var count = 2;
	while(true)
	{
		if(	document.getElementById("id" + count) != null)
		{
			for(var i = count - 1; i >= 1; i--)
			{
				if(!document.getElementById("id" + i).checked) continue;
				if(!document.getElementById("id" + count).checked) continue;
				if(document.getElementById("themename" + i).value == document.getElementById("themename" + count).value)
				{
					Message("第" + i + "行的主题名称与第" + count + "行相同");
					return false;
				}
			}
			count++;
		}
		else
			break;
	}
	for(var i = 1;;i++)
	{
		if(document.getElementById("id" + i) == null)
		{
			Message("没有要提交的数据!");
			return false;
		}				
		if(document.getElementById("id" + i).checked && document.getElementById("themename" + i).value == "")
		{
			Message("第" + i + "行的主题名称不能为空！");
			return false;
		}
		if(document.getElementById("id" + i).checked) break;
	}
	return true;
}

function Message(m)
{
	document.getElementById("success").style.display = 'none';
	document.getElementById("SubmitButton").disabled = false;
	alert(m);
}

function isNaNEx(str)
{
	return !(/^\d+$/.test(str));
}

function CheckSelect(form)
{
	for (var i=0;i<form.elements.length;i++)
	{
		var e = form.elements[i];
		if (e.name != 'chkall' && e.name =='id')
		e.checked = form.chkall.checked;
	}
}

function checkFileList(form)
{
	var i = 1;
	while(true)
	{
		if(form.elements["id" + i] == null)
			break;
		form.elements["id" + i].checked = form.cfile.checked;
		i++;
	}
}

function Check(form)
{
	CheckSelect(form);
	checkedEnabledButton(form,'id','DelRec');
}
</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
<div id="append_parent"></div>
<div id="view" style="border-color:#000000;z-index:100"></div>
<form id="Form1" method="post" runat="server"><br />
<cc1:datagrid id="themesgrid" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged" OnSortCommand="Sort_Grid" IsFixConlumnControls="true" pagesize="10">
	<Columns>				
		<asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
			<HeaderStyle Width="20px" />
			<ItemTemplate>
				<input id="id" type="checkbox" onclick="checkedEnabledButton(this.form,'id','DelRec')" value="<%# DataBinder.Eval(Container, "DataItem.themeid").ToString()%>"	name="id" />
				<%# themesgrid.LoadSelectedCheckBox(DataBinder.Eval(Container, "DataItem.themeid").ToString())%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="themeID" HeaderText="图标id" Visible="false" ></asp:BoundColumn>
		<asp:BoundColumn DataField="type" SortExpression="type" HeaderText="" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="name" SortExpression="name" HeaderText="主题名称" ><headerstyle width="20%"/><ItemStyle width="100px"/></asp:BoundColumn>
		<asp:BoundColumn DataField="directory" HeaderText="文件夹" readonly="true"><headerstyle width="20%"/><ItemStyle width="100px"/></asp:BoundColumn>
		<asp:BoundColumn DataField="author" SortExpression="author" HeaderText="作者"><headerstyle width="20%"/><ItemStyle width="100px"/></asp:BoundColumn>
		<asp:BoundColumn DataField="createdate" SortExpression="createdate" HeaderText="创建日期" readonly="true"><headerstyle width="40%"/><ItemStyle width="100px"/></asp:BoundColumn>
		<asp:BoundColumn DataField="copyright" SortExpression="copyright" HeaderText="版权"><headerstyle width="40%"/><ItemStyle width="200px"/></asp:BoundColumn>
		<asp:TemplateColumn HeaderText="预览图">
			<ItemTemplate>
				<span id="layer<%# DataBinder.Eval(Container, "DataItem.themeid").ToString()%>" onmouseover="showMenu(this.id, 0, 0, 1, 0);">预览</span>
				<div id="layer<%# DataBinder.Eval(Container, "DataItem.themeid").ToString()%>_menu" style="display:none">
				<img src="../../space/skins/themes/<%# DataBinder.Eval(Container, "DataItem.directory").ToString()%>/about.png" onerror="this.src='../../images/common/none.gif'" />
				</div>
			</ItemTemplate>
		</asp:TemplateColumn>			
	</Columns>
</cc1:datagrid>
<p style="text-align:right;">
<cc1:Button id="EditTheme" runat="server" Text=" 编辑提交 "></cc1:Button>&nbsp;&nbsp;
<cc1:Button id="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false"></cc1:Button>&nbsp;&nbsp;
<button type="button" class="ManagerButton" id="Button3" onclick="window.location='space_spacethememanage.aspx';"><img src="../images/arrow_undo.gif"/> 返 回 </button>
</p>
<div class="ManagerForm">
<fieldset>		
<legend style="background:url(../images/icons/icon43.jpg) no-repeat 6px 50%;">增加现有主题</legend>	
<table class="ntcplist" >
<tr>
<td>
	<table class="datalist" cellspacing="0" rules="all" border="1" width="100%" id="Table1" style="border-collapse:collapse;">
		<tr class="category">
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;"><input type="checkbox" name="cfile" onclick="checkFileList(this.form)"></td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">主题名称</td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">文件夹名称</td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">作者</td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">创建日期</td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">版权信息</td>
		  <td nowrap="nowrap" style="border-color:#EAE9E1;border-width:1px;border-style:solid;">预览图</td>
		</tr>
		<asp:Literal ID="themeInfoList" Runat="server" />
	  </table>
  </td>
</tr>
</table>
<div class="Navbutton"><cc1:Button id="SubmitButton" runat="server" Text=" 提 交 " ValidateForm="true"></cc1:Button></div>
</fieldset>
</div>	
</form>
<%=footer%>
</body>
</HTML>