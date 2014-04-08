if (typeof playerscriptexist == "undefined")
{
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/player/Silverlight.js'></scr" + "ipt>");
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/player/MicrosoftAjax.js'></scr" + "ipt>");
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/player/BasePlayer.js'></scr" + "ipt>");
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/player/PlayerStrings.js'></scr" + "ipt>");
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/player/player.js'></scr" + "ipt>");	
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/player/StartPlayer.js'></scr" + "ipt>");
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/js/InstallCreateSilverlight.js'></scr" + "ipt>");
	document.write("<scr" + "ipt type='text/javascript' src='silverlight/js/InstallExperience.js'></scr" + "ipt>");
	var playerscriptexist = true;
}