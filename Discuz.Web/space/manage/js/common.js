function showhint(iconid, str)
{
	var imgUrl='images/hint.gif';
	if (iconid != 0)
	{
		imgUrl = 'images/warning.gif';
	}
	document.write('<div style="background:url(' + imgUrl + ') no-repeat 20px 10px;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin:10px; padding:10px 10px 10px 56px;">');
	document.write(str + '</div><div style="clear:both;"></div>');
}

function showloadinghint(divid, str)
{
	if (divid=='')
	{
		divid='PostInfo';
	}
	document.write('<div id="' + divid + ' " style="display:none;position:relative;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin:auto;padding:10px" width="90%"  ><img border="0" src="../images/ajax_loading.gif" /> ' + str + '</div>');
}


function CheckByName(form,name,noname)
{
  for (var i=0;i<form.elements.length;i++)
    {
	    var e = form.elements[i];
	   
	    if(e.name.indexOf(name)>=0)
		{
		   if(noname!="")
           {
              if(e.name.indexOf(noname)>=0) ;
              else
              {
                 e.checked = form.chkall.checked;
                // alert(e.name+' '+form.chkall.checked);
              }
             
		   }	  
		   else
		   {
		      e.checked = form.chkall.checked;   
		   }
	    }
	}
}


function CheckAll(form)
  {
  for (var i=0;i<form.elements.length;i++)
    {
    var e = form.elements[i];
    if (e.name != 'chkall' && e.name !='deleteMode')
       e.checked = form.chkall.checked;
    }
}

//function SH_SelectOne()
//{
//	var obj = window.event.srcElement;
//	if( obj.checked == false)
//	{
//		document.getElementById('chkall').checked = obj.chcked;
//		
//	}
//}

function SH_SelectOne(obj)
{
	//var obj = window.event.srcElement;
	if( obj.checked == false)
	{
		document.getElementById('chkall').checked = obj.chcked;
	}
}



var xmlhttp;
   
function getReturn(Url)  //提交为aspx,aspx页面路径, 返回页面的值
{
    try { xmlhttp=new ActiveXObject("Msxml2.XMLHTTP") } 
    catch (e) 
    {
         try {
                   xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
             }
         catch (E) 
             {
                   //alert("请安装Microsofts XML parsers")
             }
    }
        
    if ( !xmlhttp && typeof XMLHttpRequest != "undefined" ) 
	{   
		xmlhttp=new XMLHttpRequest() 
	} 
        
    try 
    {
        xmlhttp.open('GET',Url,false);   
        xmlhttp.setRequestHeader('Content-Type','application/x-www-form-urlencoded')
        xmlhttp.send(null);    
        
        if((xmlhttp.readyState == 4)&&(xmlhttp.status ==200)){
             return xmlhttp.responseText;
        }
        else{
           return null;
        }
    }
    catch (e) 
    {  
         alert("你的浏览器不支持XMLHttpRequest对象, 请升级"); 
    }

    return null;
}
       

function isMaxLen(o)
{
	var nMaxLen=o.getAttribute? parseInt(o.getAttribute("maxlength")):"";
	if(o.getAttribute && o.value.length>nMaxLen)
	{
		o.value=o.value.substring(0,nMaxLen)
	}
}
    
/*
function Pause(obj,iMinSecond){ 
 if (window.eventList==null) window.eventList=new Array(); 
 var ind=-1; 
 for (var i=0;i<window.eventList.length;i++){ 
  if (window.eventList[i]==null) { 
   window.eventList[i]=obj; 
   ind=i; 
   break; 
  } 
 } 
  
 if (ind==-1){ 
  ind=window.eventList.length; 
  window.eventList[ind]=obj; 
 } 
 setTimeout("GoOn(" + ind + ")",iMinSecond); 
} 


function GoOn(ind){ 
 var obj=window.eventList[ind]; 
 window.eventList[ind]=null; 
 if (obj.NextStep) obj.NextStep(); 
 else obj(); 
} 


function Test(name){ 
 alert(name); 
 Pause(this,10000);//调用暂停函数 
 this.NextStep=function hello(name){ 
  alert('hello'+name); 
} 
} 

Test('dai');
*/

//权限按行选函数
function selectRow(rowId,check)
{
	document.getElementById("viewperm" + rowId).checked = check;
	document.getElementById("postperm" + rowId).checked = check;
	document.getElementById("replyperm" + rowId).checked = check;
	document.getElementById("getattachperm" + rowId).checked = check;
	document.getElementById("postattachperm" + rowId).checked = check;
}
//权限按列选函数
function seleCol(colPerfix,check)
{
	var obj;
	var i = 1;
	while(true)
	{
		obj = document.getElementById(colPerfix + i);
		if(obj == null) break;
		obj.checked = check;
		i++;
	}
}
   

function changeDeleteModeState(item,form)
{
	switch(item)
	{
		case 1:
			document.getElementById("chkall").disabled = false;
			document.getElementById("deleteNum").disabled = document.getElementById("deleteFrom_deleteFrom").disabled = true;
			enableCheckBox(false,form);
			document.getElementById("deleteNum").value = "";
			document.getElementById("deleteFrom_deleteFrom").value = "";
			break;
		case 2:
			document.getElementById("deleteNum").disabled = false;
			document.getElementById("chkall").disabled = document.getElementById("deleteFrom_deleteFrom").disabled = true;
			enableCheckBox(true,form);
			document.getElementById("chkall").checked = false;			
			document.getElementById("deleteFrom_deleteFrom").value = "";
			break;
		case 3:
			document.getElementById("deleteFrom_deleteFrom").disabled = false;
			document.getElementById("chkall").disabled = document.getElementById("deleteNum").disabled = true;
			enableCheckBox(true,form);
			document.getElementById("chkall").checked = false;			
			document.getElementById("deleteNum").value = "";
			break;
	}
}  

function enableCheckBox(b,form)
{
	for (var i=0;i<form.elements.length;i++)
	{
		var e = form.elements[i];
		if (e.type == "checkbox")
		{
			e.disabled = b;
			e.checked = false;
		}
	}
} 


/**  
* 一些常用的javascript函数(方法)  
*  
* 为便于使用，均书写成String对象的方法  
* 把他保存为.js文件，可方便的扩展字符串对象的功能  
*  
* 方法名 功 能  
* ----------- --------------------------------  
* Trim 删除首位空格  
* Occurs 统计指定字符出现的次数  
* isDigit 检查是否由数字组成  
* isAlpha 检查是否由数字字母和下划线组成  
* isNumber 检查是否为数  
* lenb 返回字节数  
* isInChinese 检查是否包含汉字  
* isEmail 简单的email检查  
* isDate 简单的日期检查，成功返回日期对象  
* isInList 检查是否有列表中的字符字符  
* isInList 检查是否有列表中的字符字符  
*/  
/*** 删除首尾空格 ***/  
String.prototype.Trim = function() {  
return this.replace(/(^\s*)|(\s*$)/g, "");  
}  
/*** 统计指定字符出现的次数 ***/  
String.prototype.Occurs = function(ch) {  
// var re = eval("/[^"+ch+"]/g");  
// return this.replace(re, "").length;  
return this.split(ch).length-1;  
}  
/*** 检查是否由数字组成 ***/  
String.prototype.isDigit = function() {  
var s = this.Trim();  
return (s.replace(/\d/g, "").length == 0);  
}  
/*** 检查是否由数字字母和下划线组成 ***/  
String.prototype.isAlpha = function() {  
return (this.replace(/\w/g, "").length == 0);  
}  
/*** 检查是否为数 ***/  
String.prototype.isNumber = function() {  
var s = this.Trim();  
return (s.search(/^[+-]?[0-9.]*$/) >= 0);  
}  

/*** 返回字节数 ***/  
String.prototype.lenb = function() {  
return this.replace(/[^\x00-\xff]/g,"**").length;  
}  
/*** 检查是否包含汉字 ***/  
String.prototype.isInChinese = function() {  
return (this.length != this.replace(/[^\x00-\xff]/g,"**").length);  
}  
/*** 简单的email检查 ***/  
String.prototype.isEmail = function() {  
　var strr;  
var mail = this;  
　var re = /(\w+@\w+\.\w+)(\.{0,1}\w*)(\.{0,1}\w*)/i;  
　re.exec(mail);  
　if(RegExp.$3!="" && RegExp.$3!="." && RegExp.$2!=".")  
strr = RegExp.$1+RegExp.$2+RegExp.$3;  
　else  
　　if(RegExp.$2!="" && RegExp.$2!=".")  
strr = RegExp.$1+RegExp.$2;  
　　else  
　strr = RegExp.$1;  
　return (strr==mail);  
}  
/*** 简单的日期检查，成功返回日期对象 ***/  
String.prototype.isDate = function() {  
var p;  
var re1 = /(\d{4})[年./-](\d{1,2})[月./-](\d{1,2})[日]?$/;  
var re2 = /(\d{1,2})[月./-](\d{1,2})[日./-](\d{2})[年]?$/;  
var re3 = /(\d{1,2})[月./-](\d{1,2})[日./-](\d{4})[年]?$/;  
if(re1.test(this)) {  
p = re1.exec(this);  
return new Date(p[1],p[2],p[3]);  
}  
if(re2.test(this)) {  
p = re2.exec(this);  
return new Date(p[3],p[1],p[2]);  
}  
if(re3.test(this)) {  
p = re3.exec(this);  
return new Date(p[3],p[1],p[2]);  
}  
return false;  
}  
/*** 检查是否有列表中的字符字符 ***/  
String.prototype.isInList = function(list) {  
var re = eval("/["+list+"]/");  
return re.test(this);  
}  


function quicksubmit(event)
{
   if(event.ctrlKey && event.keyCode == 13)
   {
      return true;
   }
   else
   {
      return false;
   }
}


//加入收藏
function addBookmark(site, url){
	if(navigator.userAgent.toLowerCase().indexOf('ie') > -1) {
		window.external.addFavorite(url,site)
	} else if (navigator.userAgent.toLowerCase().indexOf('opera') > -1) {
		alert ("请使用Ctrl+T将本页加入收藏夹");
	} else {
		alert ("请使用Ctrl+D将本页加入收藏夹");
	}
}

//复制URL地址
function setCopy(_sTxt){
	if(navigator.userAgent.toLowerCase().indexOf('ie') > -1) {
		clipboardData.setData('Text',_sTxt);
		alert ("网址“"+_sTxt+"”\n已经复制到您的剪贴板中\n您可以使用Ctrl+V快捷键粘贴到需要的地方");
	} else {
		prompt("请复制网站地址:",_sTxt); 
	}
}


function expandoptions(id)
{
	var a = document.getElementById(id);
	if(a.style.display=='')
	{
		a.style.display='none';
	}
	else
	{
		a.style.display='';
	}
}
