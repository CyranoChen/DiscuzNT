<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Mall.Admin.mall_tradelog" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>订单管理</title>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server">    
    订单状态:<cc1:DropDownList ID="drpstatus" AutoPostBack="true" runat="server">
        <asp:ListItem Text="全部" Value="" />
        <asp:ListItem Text="未生效的交易" Value="0" />
        <asp:ListItem Text="等待买家付款" Value="1" />
        <asp:ListItem Text="交易已创建,等待卖家确认" Value="2" />
        <asp:ListItem Text="确认买家付款中，暂勿发货" Value="3" />
        <asp:ListItem Text="买家已付款(或支付宝收到买家付款),请卖家发货" Value="4" />
        <asp:ListItem Text="卖家已发货，买家确认中" Value="5" />
        <asp:ListItem Text="买家确认收到货，等待支付宝打款给卖家" Value="6" />
        <asp:ListItem Text="交易成功结束" Value="7" />
        <asp:ListItem Text="交易中途关闭(未完成)" Value="8" />
        <asp:ListItem Text="等待卖家同意退款" Value="10" />
        <asp:ListItem Text="卖家拒绝买家条件，等待买家修改条件" Value="11" />
        <asp:ListItem Text="卖家同意退款，等待买家退货" Value="12" />
        <asp:ListItem Text="等待卖家收货" Value="13" />
        <asp:ListItem Text="双方已经一致，等待支付宝退款" Value="14" />
        <asp:ListItem Text="支付宝处理中" Value="15" />
        <asp:ListItem Text="结束的退款" Value="16" />
        <asp:ListItem Text="退款成功" Value="17" />
        <asp:ListItem Text="退款关闭" Value="18" />
    </cc1:DropDownList>
    <cc1:DataGrid ID="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged">
        <Columns>
            <asp:TemplateColumn HeaderText="交易单号">
                <ItemTemplate>
                    <%#GetOrderNo(DataBinder.Eval(Container, "DataItem.offline").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="商品名称">
                <ItemTemplate>
                    <%#GetProductInfo(DataBinder.Eval(Container, "DataItem.goodsid").ToString(),DataBinder.Eval(Container, "DataItem.subject").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="买家">
                <ItemTemplate>
                    <%#GetUserInfo(DataBinder.Eval(Container, "DataItem.sellerid").ToString(),DataBinder.Eval(Container, "DataItem.seller").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="卖家">
                <ItemTemplate>
                    <%#GetUserInfo(DataBinder.Eval(Container, "DataItem.buyerid").ToString(),DataBinder.Eval(Container, "DataItem.buyer").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="金额（元）">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container, "DataItem.tradesum").ToString()%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="交易数量">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container, "DataItem.number").ToString()%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="订单状态">
                <ItemTemplate>
                    <%#GetOrderStatus(DataBinder.Eval(Container, "DataItem.offline").ToString(), DataBinder.Eval(Container, "DataItem.id").ToString(), DataBinder.Eval(Container, "DataItem.status").ToString(), DataBinder.Eval(Container, "DataItem.lastupdate").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </cc1:DataGrid>
    </form>
    <%=footer%>
</body>
</html>
