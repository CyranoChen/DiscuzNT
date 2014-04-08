<%@ Page Language="C#" AutoEventWireup="true"  Inherits="Discuz.Mall.Admin.mall_creditrule" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>诚信规则</title>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="您可以修改 /templates/模板名称/images 目录下相应的图片,设计适合自己站点风格的图标" />
    <cc1:DataGrid ID="DataGrid1" runat="server" IsFixConlumnControls="true">
            <Columns>
                <asp:TemplateColumn HeaderText="信用等级">
                    <HeaderStyle Width="80px" />
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container, "DataItem.id").ToString()%>
						<%# DataGrid1.LoadSelectedCheckBox(DataBinder.Eval(Container, "DataItem.id").ToString())%>
					</ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="lowerlimit" HeaderText="信用度大于" />
                <asp:BoundColumn DataField="upperlimit" HeaderText="信用度小于" />
                <asp:TemplateColumn HeaderText="卖家图标">
                    <headerstyle width="10%" />
                    <itemstyle horizontalalign="Left" />
                    <ItemTemplate>
                        <img src="../../templates/default/images/<%#DataBinder.Eval(Container, "DataItem.sellericon").ToString()%>" />
					</ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="买家图标">
                    <headerstyle width="10%" />
                    <ItemStyle horizontalalign="Left" />
                    <ItemTemplate>
                        <img src="../../templates/default/images/<%#DataBinder.Eval(Container, "DataItem.buyericon").ToString()%>" />
					</ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </cc1:DataGrid>
        <p style="text-align:right;">
            <cc1:Button ID="SaveCreditRule" runat="server" Text="保存信用等级" OnClick="SaveCreditRule_Click"></cc1:Button>
        </p>
    </form>
    <%=footer%>
</body>
</html>
