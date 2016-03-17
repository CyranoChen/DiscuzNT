<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Install.pluginsetup" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<%@import namespace="Discuz.Install"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<%=SetupPage.header%>

<body>
  <form id="Form1" method="post" runat="server">
    <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#666666">
        <tr>
            <td bgcolor="#ffffff">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" bgcolor="#333333">
                            <table width="100%" border="0" cellspacing="0" cellpadding="8">
                                <tr>
                                    <td>
                                        <span style="color: #ffffff">插件安装向导</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <asp:Panel ID="Panel1" runat="server" Visible="true">
                    <tr>
                        <td valign="top">
                            <br />
                            
                            <br />
                            <br />
                            <table id="Table2" cellspacing="1" cellpadding="1" width="90%" align="center" border="0">
                                <tr>
                                    <td>
                                        <font color="red">注意: 此操作会删除以前安装的插件数据，请务必备份数据库!</font><br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        请从插件列表中选择将安装的插件：<br />
                                        <br />
                                        <cc1:CheckBoxList ID="PlugIn" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">                                                          
                                        </cc1:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table width="90%" border="0" cellspacing="0" cellpadding="8">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="SetupPlugIn" runat="server" Text="开始安装插件" OnClick="SetupPlugIn_Click" ></asp:Button>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </asp:Panel>
                    
                    <asp:Panel ID="Panel2" runat="server" Visible="false">
                     <tr>
                        <td width="180" valign="top">
                            <%=SetupPage.logo%>
                        </td>
                        <td width="520" valign="top">
                            <br />
                            <br />
                            <table id="Table1" cellspacing="1" cellpadding="1" width="90%" align="center" border="0">
                                <tr>
                                    <td>
                                        下列插件已安装成功：<br /><br />
                                        <% = installedPlugIn %><br /><br />
                                        
                                        <input type="button" onclick="javascript:window.location.href='../index.aspx';" value="进入首页"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </asp:Panel>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <%=SetupPage.footer%>
</body>
</html>
