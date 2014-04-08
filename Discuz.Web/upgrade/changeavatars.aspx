<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changeavatars.aspx.cs" Inherits="Discuz.Install.chageavatars" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>	    
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	    <title>Discuz!NT 头像转换程序</title>
	    <link rel="stylesheet" href="css/styles.css" type="text/css" media="all"  />
	</head>
	<body>
		<table width="700" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#666666">
			<tr>
				<td bgcolor="#ffffff"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
						<tr>
							<td colspan="2" bgcolor="#333333"><table width="100%" border="0" cellspacing="0" cellpadding="8">
									<tr>
										<td><font color="#ffffff">欢迎使用Discuz!NT 头像转换程序</font></td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td width="180" valign="top"><img src="images/logo.jpg" /></td>
							<td width="520" valign="top"><br />
								<br />
								<table id="table2" cellspacing="1" cellpadding="1" width="90%" align="center" border="0">
									<tr>
										<td>
											<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 在 <%=productname%> 中采用了新的头像机制，原有的头像机制将取消。本头像转换程序是将 <%=preproduct%> 中原有的头像转换为 <%=productname%> 可用的头像机制。</p>
										</td>
									</tr>
									<tr>
									    <td><asp:Label ID="info" runat="server" /></td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td>&nbsp;</td>
							<td><table width="90%" border="0" cellspacing="0" cellpadding="8">
									<tr>
										<td align="right">
										<form id="forum1" runat="server"><asp:Button ID="change" Text="开始转换" runat="server" onclick="ChangeAvatars_Click"/></form>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</body>
</html>
