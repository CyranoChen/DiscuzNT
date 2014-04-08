<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="uploadfile.ascx.cs" Inherits="Discuz.Space.Manage.uploadfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" style="BACKGROUND: transparent">
<tr>
<td>
<input id="filefield1" type="file" runat="server" size="40" style="BACKGROUND: white" name="filefield1" >&nbsp;&nbsp;<asp:Button id="startup" runat="server" Text="上传" CssClass="uploadbutton"></asp:Button>
</td>
<td>
<span id="albumlayer" style="display:none;">
<cc1:DropDownList id="albums" runat="server"></cc1:DropDownList>
<span id="freePhotoSize" align="center"></span>
</span>
</td>
</tr>
</table>
<div style="display:none">
	<cc1:TextBox id="UpfileList" runat="server" TextMode="MultiLine" RequiredFieldType="暂无校验"></cc1:TextBox>
</div>
<%if(allowScript) {%>
<script type="text/javascript">
    var is_ie = document.all ? true : false; 
	var currentFreePhotoSize = <%=freePhotoSize %>;
	var heavyImage;
	SetMessage(currentFreePhotoSize);
	function PhotoStatus(file)
	{
		if(file != "")
		{
			var patn = /\.jpg$|\.jpeg$|\.gif$|\.png$/i;
			if(patn.test(file))
			{
				document.getElementById("albumlayer").style.display = "block";
				heavyImage = new Image(); 
				heavyImage.src = "file:///" + document.getElementById("uploadfile1_filefield1").value;
				setTimeout("CalSize()",500);
			}
			else
			{
			    document.getElementById("albumlayer").style.display = "none";
			}		
		}
	}
	
	function CalSize()
	{
		filesize = parseInt(heavyImage.fileSize);
		SetMessage(currentFreePhotoSize - filesize);
	}
	
	function SetMessage(bytesize)
	{
		if (is_ie)
		{
			if(bytesize >= 0)
				document.getElementById("freePhotoSize").innerHTML = "<span style='font-size:12px'>您还有<font color='#009900'>" + parseInt(bytesize / 1024) + "</font>K的<% = config.Albumname%>空间</span>";
			else
				document.getElementById("freePhotoSize").innerHTML = "<span style='color:#ff0000;font-size:12px'>警告：您已经超过最大<% = config.Albumname%>空间" + parseInt(-bytesize / 1024) + "K</span>";
		}
		
	}
</script>
<%} %>