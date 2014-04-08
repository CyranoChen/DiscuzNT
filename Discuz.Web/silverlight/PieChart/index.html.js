// JScript source code

//contains calls to silverlight.js, example below loads Page.xaml
function createSilverlight()
{
	Silverlight.createObjectEx({
		source: "Page.xaml",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "100%",
			height: "100%",
			version: "1.1",
			enableHtmlAccess: "true",
			inplaceInstallPrompt: "true",
			background: getQueryString('bg') == '' ? 'white' : '#' + getQueryString('bg')
			},
		events: {
		    //onLoad : OnLoaded
		}
	});
	   
	// Give the keyboard focus to the Silverlight control by default
    
    document.body.onload = function() {
      var silverlightControl = document.getElementById('SilverlightControl');
      if (silverlightControl)
      silverlightControl.focus();
    }
    

}

function getQueryString(queryname) {
    var qKeys = {};
    var re = /[?&]([^=]+)(?:=([^&]*))?/g;
    var matchInfo;
    while(matchInfo = re.exec(location.search)){
	    qKeys[matchInfo[1]] = matchInfo[2];
    }
    return typeof(qKeys[queryname])=='undefined'?'':qKeys[queryname];
}

function HighlightCaption(sender, args)
{
    // args can be resolved :(
}

function OnLoaded(sender, args)
{
    sender.Content.PiePage.CallJs = HighlightCaption;
}