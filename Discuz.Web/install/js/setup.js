function disableOtherSubmit()
{           var objs = document.getElementsByTagName('INPUT');
            for(var i=0; i<objs.length; i++)
            {
                if(objs[i].type.toLowerCase() == 'submit')
                {
                    objs[i].disabled = true;
                }
            } 
}
            
      
var n = 0;

	
function HighlightAll(obj) {
	obj.focus();
	obj.select();
	if (document.getElementById("TextBox1")) {
		obj.createTextRange().execCommand("Copy");
		window.status = "将模板内容复制到剪贴板";
		setTimeout("window.status=''", 1800);
	}
}




var PasswordStrength ={
Level : ["极佳","一般","较弱","太短"],
LevelValue : [15,10,5,0],//强度值
Factor : [1,2,5],//字符加数,分别为字母,数字,其它
KindFactor : [0,0,10,20],//密码含几种组成的加数 
Regex : [/[a-zA-Z]/g,/\d/g,/[^a-zA-Z0-9]/g] //字符正则数字正则其它正则
}
            
PasswordStrength.StrengthValue = function(pwd)
{
    var strengthValue = 0;
    var ComposedKind = 0;
    for(var i = 0 ; i < this.Regex.length;i++)
    {
        var chars = pwd.match(this.Regex[i]);
        if(chars != null)
        {
            strengthValue += chars.length * this.Factor[i];
            ComposedKind ++;
        }
    }
    strengthValue += this.KindFactor[ComposedKind];
    return strengthValue;
} 
        
PasswordStrength.StrengthLevel = function(pwd)
{
    var value = this.StrengthValue(pwd);
    for(var i = 0 ; i < this.LevelValue.length ; i ++)
    {
        if(value >= this.LevelValue[i] )
             return this.Level[i];
    }
}
     
function loadinputcontext(o)
{
    var showmsg=PasswordStrength.StrengthLevel(o.value);
    switch(showmsg)
    {
         case "太短": showmsg+=" <img src=../images/level/1.gif width=88 height=10>";break;
         case "较弱": showmsg+=" <img src=../images/level/2.gif width=88 height=10>";break;
         case "一般": showmsg+=" <img src=../images/level/3.gif width=88 height=10>";break;
         case "极佳": showmsg+=" <img src=../images/level/4.gif width=88 height=10>";break;
    }
           
    document.getElementById('showmsg').innerHTML =showmsg;
}