<%@ Page language="c#" Inherits="Discuz.Mall.Admin.mall_mallsetting" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
	<title>registerandvisit</title>		
	<script type="text/javascript" src="../js/common.js"></script>			    
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />        
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
	<script type="text/javascript" src="../js/modalpopup.js"></script>
	<script type="text/javascript">
	    function LoadImage(index)
	    {
	        document.getElementById("preview").src = images[index];
	    }
	</script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
	<div class="ManagerForm">
		<form id="Form1" method="post" runat="server">
			<cc1:Hint id="Hint1" runat="server" HintImageUrl="../images"></cc1:Hint>
		    <fieldset>
		        <legend style="background:url(../images/icons/icon55.jpg) no-repeat 6px 50%;">商城设置</legend>
			    <table cellspacing="0" cellpadding="4" width="100%" align="center">
			    <tr>
			        <td  class="panelbox" width="50%" align="left">
			            <table width="100%">
			                <tr>
			                    <td style="width: 100px">是否启用交易<br />帖或商城服务:</td>
		                        <td>
                                    <cc1:DropDownList ID="EnableMall" runat="server">
			                            <asp:ListItem Value="0" Selected="True">不开启</asp:ListItem>
			                            <asp:ListItem Value="1">启用交易帖</asp:ListItem>
		                            </cc1:DropDownList>
		                        </td>
				            </tr>
			            </table>			        
			        </td>
			        <td  class="panelbox" width="50%" align="right">
			            <table width="100%">
                            <tr>
                              <td style="width: 100px">每页商品数:</td>
                              <td><cc1:TextBox ID="gpp" runat="server" Size="3" /></td>
                            </tr>
			            </table>
			        </td>
			    </tr>
			    </table>
		    </fieldset>
		    <div class="Navbutton">
		        <cc1:Button id="SaveInfo" runat="server" Text=" 提 交 "></cc1:Button>
		    </div>
		</form>
	</div>			
	<%=footer%>
</body>
</html>
