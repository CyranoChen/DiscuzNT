<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false" Inherits="Discuz.Mall.Admin.mall_locationsmanage" %>
<%@ Register Src="../UserControls/PageInfo.ascx" TagName="PageInfo" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="Discuz.Control" Assembly="Discuz.Control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>区域管理</title>
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
            checkedEnabledButton(form,'delid','DelRec');
        }
        
        
        function combox(obj,select)
        {
            this.obj = obj;
            this.name = select;
            this.select = document.getElementsByName(select)[0];
            /*要转换的下拉框*/
        }
        /*初始化对象*/
        combox.prototype.init=function()
        {
            var isIE = document.all ? true : false;
            var inputbox = "<input name='combox_" + this.name + "' onchange='" + this.obj + ".find()' ";
            inputbox += "style='position:absolute;width:" + (this.select.offsetWidth - 20) + "px;height:" + (this.select.offsetHeight - 4) + "px;z-index:1;'>";
            document.getElementById(this.obj + '_input').innerHTML = inputbox;
            with(this.select.style)
            {
                //position = "absolute";
                clip = "rect(0 " + (this.select.offsetWidth) + " " + this.select.offsetHeight + " " + (this.select.offsetWidth - 18) + ")";
                /*切割下拉框*/
            }
            this.select.onchange = new Function(this.obj + ".change()");
            this.change();
        }
        /*初始化结束*/
        ////////对象事件定义///////
        combox.prototype.find=function()
        {
            /*当搜索到输入框的值时,下拉框自动定位*/
            var inputbox = document.getElementsByName("combox_" + this.name)[0];
            with(this.select)
            {
                for(i = 0 ; i < options.length ; i++)
                    if(options[i].text.indexOf(inputbox.value) == 0)
                    {
                        selectedIndex = i;
                        this.change();
                        break;
                    }
            }
        }
        combox.prototype.change=function()
        {
            /*定义下拉框的onchange事件*/
            var inputbox = document.getElementsByName("combox_" + this.name)[0];
            inputbox.value = this.select.options[this.select.selectedIndex].value;
            if(this.select.name == "country_select")
            {
                document.getElementById("state_select").options.length = 1;
                for(var i = 0 ; i < locations.length ; i++)
                {
                    if(locations[i]["country"] == this.select.options[this.select.selectedIndex].text)
                    {
                        isExist = false;
                        for(var j = 0 ; j < document.getElementById("state_select").options.length ; j++)
                        {
                            if(document.getElementById("state_select").options[j].value == locations[i]["state"])
                            {
                                isExist = true;
                                break;
                            }
                        } 
                        if(!isExist)
                        {                            
                            var item = new Option(locations[i]["state"],locations[i]["state"]);
                            document.getElementById("state_select").options.add(item);
                        }                 
                    }
                }
                var isIE = document.all ? true : false;
                if(isIE)
                {
                    if(document.getElementById("state_select").parentNode.childNodes[0].childNodes[0] != null)
                    {
                        document.getElementById("state_select").parentNode.childNodes[0].childNodes[0].value = "";
                    }
                }
                else
                {
                    if(document.getElementById("state_input").childNodes.item(0) != null)
                    {
                        document.getElementById("state_input").childNodes.item(0).value = "";
                    }
                }
            }
        }
        ////////对象事件结束///////
        /*公用定位函数(获取控件绝对坐标)*/
        function getL(e)
        {
            var l = e.offsetLeft;
            while(e = e.offsetParent)
                l += e.offsetLeft;
            return l;
        }
        function getT(e)
        {
            var t = e.offsetTop;
            while(e = e.offsetParent)
                t += e.offsetTop;
            return t;
        }

        
        var country;
        var state;
        function showDialog()
        {
            BOX_show('neworedit');
            country = new combox("country","country_select");
            country.init();
            state = new combox("state","state_select");
            state.init();
        }
        
        function validate()
        {
            document.getElementById("country_select_info").innerHTML = "";
            document.getElementById("state_select_info").innerHTML = "";
            document.getElementById("city_info").innerHTML = "";
            var country = document.getElementById("country_input").childNodes[0].value;
            var state = document.getElementById("state_input").childNodes[0].value;
            var city = document.getElementById("city").value;
            var error = false;
            if(country == "")
            {
                document.getElementById("country_select_info").innerHTML = "<img src='../images/false.gif' title='请输入国家名称!'>";
                error = true;
            }
            if(state == "")
            {
                document.getElementById("state_select_info").innerHTML = "<img src='../images/false.gif' title='请输入省份名称!'>";
                error = true;
            }
            if(city == "")
            {
                document.getElementById("city_info").innerHTML = "<img src='../images/false.gif' title='请输入城市名称!'>";
                error = true;
            }
            var url = "../global/global_ajaxcall.aspx?opname=location&country=" + escape(country) + "&state=" + escape(state) + "&city=" + escape(city);
            var result = getReturn(url);
            if(result != "ok")
            {
                document.getElementById("city_info").innerHTML = result;
                document.getElementById("city").value = "";
                error = true;
            }
            if(error)
            {                
                document.getElementById('success').style.display = 'none'
	            document.getElementById("AddNewRec").disabled = false;
            }
            return !error;
        }
    </script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:PageInfo ID="info1" runat="server" Icon="information" Text="省份列也可以是其它国家的州或郡." />
    <cc1:DataGrid ID="DataGrid1" runat="server" IsFixConlumnControls="true" OnPageIndexChanged="DataGrid_PageIndexChanged">
        <Columns>
            <asp:TemplateColumn HeaderText="<input title='选中/取消' onclick='Check(this.form)' type='checkbox' name='chkall' id='chkall' />">
                <HeaderStyle Width="20px" />
                <ItemTemplate>
                    <input id="delid" onclick="checkedEnabledButton(this.form,'delid','DelRec')" type="checkbox" value="<%#DataBinder.Eval(Container, "DataItem.lid").ToString()%>" name="delid" />                        
					<%# DataGrid1.LoadSelectedCheckBox(DataBinder.Eval(Container, "DataItem.lid").ToString())%>
				</ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="city" HeaderText="城市" />
            <asp:BoundColumn DataField="state" HeaderText="省份" />
            <asp:BoundColumn DataField="country"  HeaderText="国家" />
            <asp:BoundColumn DataField="zipcode" HeaderText="邮编" />
        </Columns>
    </cc1:DataGrid>
    <p style="text-align:right;">
        <cc1:Button ID="SaveLocation" runat="server" Text="保存区域" ValidateForm="false" OnClick="SaveLocation_Click"></cc1:Button>&nbsp;&nbsp;
        <button type="button" class="ManagerButton" id="Button2" onclick="showDialog()"><img src="../images/add.gif"/> 新建区域 </button>&nbsp;&nbsp;
        <cc1:Button ID="DelRec" runat="server" Text=" 删 除 " ButtonImgUrl="../images/del.gif" Enabled="false" OnClick="DelRec_Click"></cc1:Button>
    </p>
    <div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
    <div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:350px;">
    <div class="ManagerForm">
        <fieldset>
            <legend style="background: url(../images/icons/icon4.jpg) no-repeat 6px 50%;">新建区域</legend>
            <table cellspacing="0" cellpadding="4" cellspacing="4" width="100%" align="center">
                <tr>
                    <td style="width: 60px;height:35px;vertical-align:top;">国家:</td>
                    <td style="vertical-align:top;">
                        <span id="country_input"></span>
                        <asp:DropDownList ID="country_select" runat="server" />
                        <span id="country_select_info" />
                    </td>
                </tr>
                <tr>
                    <td style="height:35px;vertical-align:top;">省份:</td>
                    <td style="vertical-align:top;">
                        <span id="state_input"></span>
                        <asp:DropDownList ID="state_select" runat="server" />
                        <span id="state_select_info" />
                    </td>
                </tr>
                <tr>
                    <td style="height:35px;vertical-align:top;">城市:</td>
                    <td style="vertical-align:top;">
                        <input type="text" name="city" id="city"/>
                        <span id="city_info" />
                    </td>
                </tr>
                <tr>
                    <td style="height:35px;vertical-align:top;">邮编:</td>
                    <td style="vertical-align:top;">
                        <input type="text" name="zipcode" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="height:35px;">
                        <cc1:Button ID="AddNewRec" runat="server" Text=" 提 交 " ValidateForm="true" OnClick="AddNewRec_Click"></cc1:Button>&nbsp;&nbsp;
			            <button type="button" class="ManagerButton" id="Button1" onclick="BOX_remove('neworedit');"><img src="../images/state1.gif"/> 取 消 </button>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </div>
    </form>
    <div id="setting" />
    <%=footer%>
</body>
</html>
