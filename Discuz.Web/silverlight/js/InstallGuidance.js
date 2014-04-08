//place text below the install medallion to inform the users what to do after installation
var PostInstallGuidance = document.getElementById('PostInstallGuidance');
if ( document.getElementById('PostInstallGuidance') )
{
	if ( Silverlight.ua.Browser == "MSIE" )
	{
		if ( Silverlight.available )
		{
			PostInstallGuidance.innerHTML="安装完成后请重启浏览器激活Silverlight内容.";
		}
		else
		{
			PostInstallGuidance.innerHTML= "";
		}
	}
	else if ( Silverlight.ua.Browser == "Firefox" || Silverlight.ua.Browser == "Safari")
	{
		PostInstallGuidance.innerHTML="您的浏览器是 "+ Silverlight.ua.Browser + ". 安装完成后,<br />重启浏览器激活Silverlight内容.";
	}
	else
	{
		PostInstallGuidance.innerHTML="微软 Silverlight 可能不支持您的浏览器.<br />请浏览 http://www.microsoft.com/silverlight/system-requirements.aspx 查阅更多信息.";
	}
	//if silverlight is NOT available insert html into the menudiv layer 
	/*
	if ( !Silverlight.available )
	{
		var menudiv = document.getElementById('menuDiv');
		menudiv.innerHTML= '<p>立刻<a href="http://go.microsoft.com/fwlink/?LinkID=92799&clcid=0x804">下载 Silverlight</a>, <font color="gray">精彩体验不容错过!</font></p>';
	}
	*/
}
