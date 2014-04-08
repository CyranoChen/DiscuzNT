var color = "" ;
var SelRGB = color;
var DrRGB = '';
var SelGRAY = '120';

var hexch = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F');

function ToHex(n) {	
	var h, l;

	n = Math.round(n);
	l = n % 16;
	h = Math.floor((n / 16)) % 16;
	return (hexch[h] + hexch[l]);
}

function DoColor(c, l){
	var r=1, g=2, b=3;

     
 		r = '0x' + c.substring(1, 3);
		g = '0x' + c.substring(3, 5);
		b = '0x' + c.substring(5, 7);

		if(l > 120)
		{
			l = l - 120;
			r = (r * (120 - l) + 255 * l) / 120;
			g = (g * (120 - l) + 255 * l) / 120;
			b = (b * (120 - l) + 255 * l) / 120;
		}
		else
		{
			r = (r * l) / 120;
			g = (g * l) / 120;
			b = (b * l) / 120;
		}
    var aaa='#' + ToHex(r) + ToHex(g) + ToHex(b);
    if(aaa=='#NaNNaNNaN')
    {
		return '#FFFFFF';
    }
    else
    {
        return '#' + ToHex(r) + ToHex(g) + ToHex(b);
    }
}


function ColorTableMouseDown(e)
{
	SelRGB = e.title;
	//document.getElementById('s_bgcolor').style.background=e.title;
	EndColor();
}

function ColorTableMouseOver(e)
{
 	document.getElementById('RGB').innerHTML = e.title;
	EndColor();
}

function ColorTableMouseOut(e)
{
	document.getElementById('RGB').innerHTML = SelRGB;
	EndColor();
}


function GrayTableMouseDown(e)
{
	SelGRAY = e.title;
	EndColor();
}

function GrayTableMouseOver(e)
{
	document.getElementById('GRAY').innerHTML = e.title;
	EndColor();
}

function GrayTableMouseOut(e)
{
	document.getElementById('GRAY').innerHTML = SelGRAY;
	EndColor();
}



function HideColorPanel()
{  
  document.getElementById('ColorPicker').style.display='none';
}


/*

function EndColor(){
	var i;

	if(DrRGB != SelRGB)
	{
		DrRGB = SelRGB;
		for(i = 0; i <= 30; i ++)
		{
		   document.getElementById("GrayTable").rows[i].bgColor = DoColor(SelRGB, 240 - i * 8);
		}
	}

	document.getElementById('SelColor').value = DoColor(RGB.innerHTML, GRAY.innerHTML);
	document.getElementById('ShowColor').bgColor = document.getElementById('SelColor').value;
}

function ColorPickerOK()
{
    obj=document.getElementById('d_bgcolor');
	obj.focus();
	obj.select();
	if(navigator.appName.indexOf("Explorer") > -1)
    {
		obj.createTextRange().execCommand("Copy");
		window.status = '将模板内容复制到剪贴板';
		setTimeout("window.status=''", 1800);
	}
	HideColorPanel();
}


function ShowColorPanel()
{
   	var p = getposition(document.getElementById('ColorPicker1_ColorPicker1'));
  	//alert(p['x']);
	document.getElementById('ColorPicker').style.display = 'block';
	document.getElementById('ColorPicker').style.left = p['x']+'px';
	document.getElementById('ColorPicker').style.top = (p['y'] + 20)+'px';
}


	
function getposition(obj) {
	var r = new Array();
	r['x'] = obj.offsetLeft;
	r['y'] = obj.offsetTop;
	while(obj = obj.offsetParent) {
		r['x'] += obj.offsetLeft-3.6;
		r['y'] += obj.offsetTop-38.7;
	}
	return r;
}
*/