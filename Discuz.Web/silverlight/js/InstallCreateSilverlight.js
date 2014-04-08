///////////////////////////////////////////////////////////////////////////////
//
//  installcreatesilverlight.js   			version 1.0
//
//  This file is provided by Microsoft as a helper file for websites that
//  incorporate Silverlight Objects. This file is provided under the Silverlight 
//  SDK 1.0 license available at http://go.microsoft.com/fwlink/?linkid=94240.  
//  You may not use or distribute this file or the code in this file except as 
//  expressly permitted under that license.
// 
//  Copyright (c) 2007 Microsoft Corporation. All rights reserved.
//
///////////////////////////////////////////////////////////////////////////////
if(!window.Silverlight)
    window.Silverlight={};

Silverlight.InstallAndCreateSilverlight = function(version, SilverlightDiv, installExperienceHTML, installPromptDivID, createSilverlightDelegate)
{
    var RetryTimeout=3000;	              //The interval at which Silverlight instantiation is attempted(ms)	
    if ( Silverlight.isInstalled(version) )
    {
	createSilverlightDelegate();
    }
    else
    {
		if ( installExperienceHTML && SilverlightDiv )
		{
			SilverlightDiv.innerHTML=installExperienceHTML;
			document.write('<scri' + 'pt src="silverlight/js/InstallGuidance.js"></scri' + 'pt>');
			if ( !Silverlight.available )
			{
				var menudiv = document.getElementById('menuDiv');
				menudiv.innerHTML= '<p>立刻<a href="http://go.microsoft.com/fwlink/?LinkID=92799&clcid=0x804">下载 Silverlight</a>, <font color="gray">精彩体验不容错过!</font></p>';
			}

			if ( !Silverlight.available && version.toString()=='1.1' )
			{
				document.getElementById('menuDiv').innerHTML= '<p>立刻下载 Silverlight, <font color="gray">精彩体验不容错过!</font></p>';
			}

			//document.body.innerHTML;
		}
		if (installPromptDivID)
		{
			var installPromptDiv = document.getElementById(installPromptDivID);
			if ( installPromptDiv )
				installPromptDiv.innerHTML = Silverlight.createObject(null, null, null, {version: version, inplaceInstallPrompt:true},{}, null);
		}
		if ( ! (Silverlight.available || Silverlight.ua.Browser != 'MSIE' ) )
		{
			TimeoutDelegate = function()
			{
				Silverlight.InstallAndCreateSilverlight(version, null, null, null, createSilverlightDelegate);
			}
			setTimeout(TimeoutDelegate, RetryTimeout);
		}
    }
}