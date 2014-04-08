<%@ Page Language="C#" AutoEventWireup="true" Inherits="Discuz.Mall.Admin.mall_goodsmanage" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>商品管理</title>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../js/AjaxHelper.js"></script>
    <script type="text/javascript">
        function Check(form)
        {
            CheckAll(form);
            setStatus(form);
        }
        
        function setStatus(form)
        {
            if(document.getElementById("resume"))
            {
                checkedEnabledButton(form,'goodsid','resume');
            }
            if(document.getElementById("pass"))
            {
                checkedEnabledButton(form,'goodsid','pass');
            }
            checkedEnabledButton(form,'goodsid','delete');
        }
        
        function ShowGoodsInfo(goodsid)
        {
            var url = "../global/global_ajaxcall.aspx?opname=goodsinfo&goodsid=" + goodsid;
            var result = getReturn(url);
            result = result.replace(" onload=\"attachimg(this, 'load');\"","");
            document.getElementById("goodsinfolayer").innerHTML = result;
        }
    </script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server">
    <cc1:DataGrid ID="DataGrid1" runat="server" OnPageIndexChanged="DataGrid_PageIndexChanged">
        <Columns>
            <asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
                    <HeaderStyle Width="20px" />
                    <ItemTemplate>
							<input id="goodsid" onclick="setStatus(this.form);" type="checkbox" value="<%# DataBinder.Eval(Container, "DataItem.goodsid").ToString() %>" name="goodsid" />
					</ItemTemplate>
                </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="商品名称">
                <ItemTemplate>
                    <%#GetProductInfo(DataBinder.Eval(Container, "DataItem.goodsid").ToString(),DataBinder.Eval(Container, "DataItem.title").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="卖家">
                <ItemTemplate>
                    <%#GetUserInfo(DataBinder.Eval(Container, "DataItem.selleruid").ToString(), DataBinder.Eval(Container, "DataItem.seller").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="price" HeaderText="售价" />
            <asp:BoundColumn DataField="categoryname" HeaderText="商品分类" />
            <asp:TemplateColumn HeaderText="商品状态">
                <ItemTemplate>
                    <%#GetStatus(DataBinder.Eval(Container, "DataItem.displayorder").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="物流方式">
                <ItemTemplate>
                    <%#GetTransport(DataBinder.Eval(Container, "DataItem.transport").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="付宝帐号">
                <ItemTemplate>
                    <%#GetAccount(DataBinder.Eval(Container, "DataItem.account").ToString())%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="expiration" HeaderText="有效期" />
        </Columns>
    </cc1:DataGrid>
    <p style="text-align:right;">
        <cc1:Button ID="resume" runat="server" Text=" 恢 复 " ButtonImgUrl="../images/cache_reset.gif" Enabled="false" OnClick="resume_Click"></cc1:Button>
        <cc1:Button ID="pass" runat="server" Text=" 通 过 " ButtonImgUrl="../images/submit.gif" Enabled="false" OnClick="pass_Click"></cc1:Button>
        <cc1:Button ID="delete" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClick="delete_Click"></cc1:Button>
    </p>
    <div id="goodsinfolayer"></div>
    </form>
    <div id="setting" />
    <%=footer%>
</body>
</html>
