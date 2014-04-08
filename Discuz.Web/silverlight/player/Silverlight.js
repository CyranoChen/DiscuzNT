if (!window.Silverlight)
{
    window.Silverlight = { };
}

// Silverlight control instance counter for memory mgt
Silverlight._silverlightCount = 0;
Silverlight.ua = null;
Silverlight.available = false;
Silverlight.fwlinkRoot='http://go.microsoft.com/fwlink/?LinkID=';   

///////////////////////////////////////////////////////////////////////////////
// detectUserAgent Parses UA string and stores relevant data in Silverlight.ua.
///////////////////////////////////////////////////////////////////////////////
Silverlight.detectUserAgent = function()
{
    var ua = window.navigator.userAgent;
    
    Silverlight.ua = {OS:'Unsupported',Browser:'Unsupported'};
    
    //Silverlight does not support pre-Windows NT platforms
    if (ua.indexOf('Windows NT') >= 0) {
        Silverlight.ua.OS = 'Windows';
    }
    else if (ua.indexOf('PPC Mac OS X') >= 0) {
        Silverlight.ua.OS = 'MacPPC';
    }
    else if (ua.indexOf('Intel Mac OS X') >= 0) {
        Silverlight.ua.OS = 'MacIntel';
    }
    
    if ( Silverlight.ua.OS != 'Unsupported' )
    {
        if (ua.indexOf('MSIE') >= 0) {
            if (navigator.userAgent.indexOf('Win64') == -1)
            {
                if (parseInt(ua.split('MSIE')[1]) >= 6) {
                    Silverlight.ua.Browser  = 'MSIE';
                }
                
            }
        }
        else if (ua.indexOf('Firefox') >= 0) {
            var version = ua.split('Firefox/')[1].split('.');
            var major = parseInt(version[0]);
            if (major >= 2) {
                Silverlight.ua.Browser = 'Firefox';
            }
            else {
                var minor = parseInt(version[1]);
                if ((major == 1) && (minor >= 5)) {
                    Silverlight.ua.Browser  = 'Firefox';
                }
            }
        }
        
        else if (ua.indexOf('Safari') >= 0) {
            Silverlight.ua.Browser = 'Safari';
        }            
    }
}

// Detect the user agent at script load time
Silverlight.detectUserAgent();

//////////////////////////////////////////////////////////////////
// isInstalled, checks to see if the correct version is installed
//////////////////////////////////////////////////////////////////
Silverlight.isInstalled = function(version)
{
    var isVersionSupported=false;
    var container = null;
    
    try {
        var control = null;
        
        if (Silverlight.ua.Browser == 'MSIE') 
        {
            control = new ActiveXObject('AgControl.AgControl');
        }
        else 
        {
            if ( navigator.plugins["Silverlight Plug-In"] )
            {
                container = document.createElement('div');
                document.body.appendChild(container);
                if ( Silverlight.ua.Browser == "Safari" )
                {
                    container.innerHTML= '<embed type="application/x-silverlight" />';
                }
                else
                {
                    container.innerHTML= '<object type="application/x-silverlight"  data="data:," />';                    
                }    
                control = container.childNodes[0];
            }
        }
        
        document.body.innerHTML;
        
        if ( control.IsVersionSupported(version) )
        {
            isVersionSupported = true;
        }
        
        control = null;
        
        Silverlight.available = true;
    }
    catch (e) {
        isVersionSupported = false;
    }
    if (container) {
        document.body.removeChild(container);
    }
    
    return isVersionSupported;
}


///////////////////////////////////////////////////////////////////////////////
// createObject();  Params:
// parentElement of type Element, the parent element of the Silverlight Control
// source of type String
// id of type string
// properties of type String, object literal notation { name:value, name:value, name:value},
//     current properties are: width, height, background, framerate, isWindowless, enableHtmlAccess, inplaceInstallPrompt:  all are of type string
// events of type String, object literal notation { name:value, name:value, name:value},
//     current events are onLoad onError, both are type string
// initParams of type Object or object literal notation { name:value, name:value, name:value}
// userContext of type Object
/////////////////////////////////////////////////////////////////////////////////

Silverlight.createObject = function(source, parentElement, id, properties, events, initParams, userContext)
{
    var slPluginHelper = new Object();
    var slProperties = properties;
    var slEvents = events;
    
    slProperties.source = source;    
    slPluginHelper.parentElement = parentElement;
    slPluginHelper.id = Silverlight.HtmlAttributeEncode(id);
    slPluginHelper.width = Silverlight.HtmlAttributeEncode(slProperties.width);
    slPluginHelper.height = Silverlight.HtmlAttributeEncode(slProperties.height);
    slPluginHelper.ignoreBrowserVer = Boolean(slProperties.ignoreBrowserVer);
    slPluginHelper.inplaceInstallPrompt = Boolean(slProperties.inplaceInstallPrompt);
    var reqVerArray = slProperties.version.split(".");
    slPluginHelper.shortVer = reqVerArray[0]+'.'+reqVerArray[1];
    slPluginHelper.version = slProperties.version;
    
    //rename properties to their tag property names
    slProperties.initParams = initParams;
    slProperties.windowless = slProperties.isWindowless;
    slProperties.maxFramerate = slProperties.framerate;
    
    //move unknown events to the slProperties array
    for (var name in slEvents)
    {
        if (slEvents[name] && name != "onLoad" && name != "onError")
        {
            slProperties[name] = slEvents[name];
            slEvents[name] = null;
        }
    }
    
    // remove elements which are not to be added to the instantiation tag
    delete slProperties.width;              
    delete slProperties.height;
    delete slProperties.id;
    delete slProperties.onLoad;
    delete slProperties.onError;
    delete slProperties.ignoreBrowserVer;
    delete slProperties.inplaceInstallPrompt;
    delete slProperties.version;
    delete slProperties.isWindowless;
    delete slProperties.framerate;
    delete slProperties.data;
    delete slProperties.src;

    // detect that the correct version of Silverlight is installed, else display install

    if (Silverlight.isInstalled(slPluginHelper.version))
    {
        // initialize unload event one time
        if (Silverlight._silverlightCount == 0)
        {
            if (window.addEventListener) {
                window.addEventListener('onunload', Silverlight.__cleanup , false);
            }
            else {
                window.attachEvent('onunload', Silverlight.__cleanup );
            }
        }
        
        var count = Silverlight._silverlightCount++;
        
        slProperties.onLoad = '__slLoad' + count;
        slProperties.onError = '__slError' + count;
        
        //add the onLoad handler if one exists
        window[slProperties.onLoad] = function(sender)
        {
            if ( slEvents.onLoad)
            {
                slEvents.onLoad(document.getElementById(slPluginHelper.id), userContext, sender);
            }
        };
        
        //add the error handler if one exists. Otherwise, add the default error handler.
        window[slProperties.onError] = function(sender, e)
        {
            if (slEvents.onError)
            {
                slEvents.onError(sender, e);
            }
            else
            {
                Silverlight.default_error_handler(sender, e);
            }
        }
        slPluginHTML = Silverlight.buildHTML(slPluginHelper, slProperties);
    }
    //The control could not be instantiated. Show the installation prompt
    else 
    {
        slPluginHTML = Silverlight.buildPromptHTML(slPluginHelper);
    }

    // insert or return the HTML
    if(slPluginHelper.parentElement)
    {
        slPluginHelper.parentElement.innerHTML = slPluginHTML;
    }
    else
    {
        return slPluginHTML;
    }

}

///////////////////////////////////////////////////////////////////////////////
//  detect to see if this is a supported user agent
///////////////////////////////////////////////////////////////////////////////
Silverlight.supportedUserAgent = function(version)
{        
    var ua = Silverlight.ua;
    //detect all unsupported platform combinations (IE on Mac, Safari on Win)
    var noSupport = (   ua.OS == 'Unsupported' ||                           //Unsupported OS
                        ua.Browser == 'Unsupported' ||                      //Unsupported Browser
                        (ua.OS == 'Windows' && ua.Browser == 'Safari') ||   //Safari is not supported on Windows
                        (ua.OS.indexOf('Mac') >= 0 && ua.Browser == 'IE')   //IE is not supported on Mac
                            );

//<parsingTag><noSilverlight.Debug.js>
    if (version=='1.1')
    {
        //add PPC to unsupported list
        return (!(noSupport || ua.OS == 'MacPPC' ));
    }
    else
    {
//<parsingTag></noSilverlight.Debug.js>
        return (!noSupport);  
//<parsingTag><noSilverlight.Debug.js>  
    }    
//<parsingTag></noSilverlight.Debug.js>
}

///////////////////////////////////////////////////////////////////////////////
//
//  create HTML that instantiates the control
//
///////////////////////////////////////////////////////////////////////////////
Silverlight.buildHTML = function(slPluginHelper, slProperties)
{
    var htmlBuilder = [];
    var start ;
    var pre ;
    var dur ;
    var post ;
    var end ;
    
    if (Silverlight.ua.Browser=='Safari')
    {
        htmlBuilder.push('<embed ');
        start = '';
        pre = ' ';
        dur = '="';
        post = '"';
        end = ' type="application/x-silverlight"/>' + "<iframe style='visibility:hidden;height:0;width:0'/>";
    }
    else
    {
        htmlBuilder.push('<object type=\"application/x-silverlight\" data="data:,"');
        start = '>';
        pre = ' <param name="';
        dur = '" value="';
        post = '" />';
        end = '<\/object>';        
    }
    htmlBuilder.push(' id="' + slPluginHelper.id + '" width="' + slPluginHelper.width + '" height="' +slPluginHelper.height + '" '+start);
    
    for (var name in slProperties)
    {
        if (slProperties[name])
        {
            htmlBuilder.push(pre+Silverlight.HtmlAttributeEncode(name)+dur+Silverlight.HtmlAttributeEncode(slProperties[name])+post);
        }
    }

    htmlBuilder.push(end);
    return htmlBuilder.join('');
}

///////////////////////////////////////////////////////////////////////////////
//
//  Default error handling function to be used when a custom error handler is
//  not present
//
///////////////////////////////////////////////////////////////////////////////

Silverlight.default_error_handler = function (sender, args)
{
    var iErrorCode;
    var errorType = args.ErrorType;

    iErrorCode = args.ErrorCode;

    var errMsg = "\nSilverlight error message     \n" ;

    errMsg += "ErrorCode: "+ iErrorCode + "\n";


    errMsg += "ErrorType: " + errorType + "       \n";
    errMsg += "Message: " + args.ErrorMessage + "     \n";

    if (errorType == "ParserError")
    {
        errMsg += "XamlFile: " + args.xamlFile + "     \n";
        errMsg += "Line: " + args.lineNumber + "     \n";
        errMsg += "Position: " + args.charPosition + "     \n";
    }
    else if (errorType == "RuntimeError")
    {
        if (args.lineNumber != 0)
        {
            errMsg += "Line: " + args.lineNumber + "     \n";
            errMsg += "Position: " +  args.charPosition + "     \n";
        }
        errMsg += "MethodName: " + args.methodName + "     \n";
    }
    alert (errMsg);
}


// createObjectEx, takes a single parameter of all createObject parameters enclosed in {}
Silverlight.createObjectEx = function(params)
{
    var parameters = params;
    var html = Silverlight.createObject(parameters.source, parameters.parentElement, parameters.id, parameters.properties, parameters.events, parameters.initParams, parameters.context);
    if (parameters.parentElement == null)
    {
        return html;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////
// Builds the HTML to prompt the user to download and install Silverlight
///////////////////////////////////////////////////////////////////////////////////////////////
Silverlight.buildPromptHTML = function(slPluginHelper)
{
    var slPluginHTML = null;
    var urlRoot=Silverlight.fwlinkRoot;
    var OS = Silverlight.ua.OS;
    var target = '92822';  // 92822 is the 'unsupported platform' linkid
    var image;
    var L_ImageTitle_Text = '拥有 Microsoft Silverlight';
    var L_CLCID_Text = '0x804';  
    
    //<parsingTag><noSilverlight.Debug.js> 
    if (slPluginHelper.shortVer == '1.1')
    {
        slPluginHelper.inplaceInstallPrompt = false;
    }
    //<parsingTag></noSilverlight.Debug.js> 
    
    if (slPluginHelper.inplaceInstallPrompt)
    {
        var center = '98109';
        var bottom;
        //upgrade image, when available
        if ( Silverlight.available )
        {
            image = '96189';
            bottom = '96422';
        }
        else
        {
            image = '96188';
            bottom = '96422';
        }
        var EULA = '93481';     // unsupported platform EULA linkid
        var Privacy = '93483';  // unsupported platform Privacy LinkID
        
        if (OS=='Windows')
        {
            target = '92799';
            EULA = '92803';
            Privacy = '92805'; 
        }
        else if (OS=='MacIntel')
        {
            target = '92808';
            EULA = '92804';
            Privacy = '92806';
        }
        else if (OS=='MacPPC')
        {
            target = '92807';
            EULA = '92815';
            Privacy = '92816';
        }

	    var L_DirectPromptEULA_Text = '单击<b></b>“拥有 Microsoft Silverlight”进入更佳的用户体验，并表示您接受<br /><a title="Silverlight 许可协议" href="{2}" target="_top" style="text-decoration: underline; color: #96C5E1"><b>Silverlight 许可协议</b></a>';
	    var L_DirectPromptPrivacy_Text = 'Silverlight 会自动进行更新。<a title="Silverlight 隐私声明" href="{3}" target="_top" style="text-decoration: underline; color: #96C5E1"><b>了解更多信息</b></a>';
	    slPluginHTML =  '<table border="0" cellpadding="0" cellspacing="0" width="206px"><tr><td><img style="display: block; cursor: pointer; border= 0;" title="' + L_ImageTitle_Text + '" alt="' + L_ImageTitle_Text + '" onclick="javascript:Silverlight.followFWLink({0});" src="{1}" /></td></tr><tr><td style="width: 206px; margin: 0px; background: #FFFFFF; color: #C7C7C7; text-align: left; border-left-style: solid; border-right-style: solid; padding-left: 6px; padding-right: 6px; padding-top: 3px; padding-bottom: 0px; border-width: 2px; border-color: #c7c7bd; font-family: Verdana; font-size: 55%">' + L_DirectPromptEULA_Text + '</td></tr><tr><td><img src="{5}" style="border: 0; display: block;margin-top:0;" /></td></tr><tr><td style="width: 206px; margin: 0px; background: #D8EFF9; color: #C7C7C7; text-align: left; border-left-style: solid; border-right-style: solid; padding-left: 6px; padding-right: 6px; padding-top: 0px; padding-bottom: 2px; border-width: 2px; border-color: #c7c7bd; font-family: Verdana; font-size: 55%">' +L_DirectPromptPrivacy_Text + '</td></tr><tr><td><img alt="" src="{4}" style="margin-top:0;"/></td></tr></table>';
        slPluginHTML = slPluginHTML.replace('{2}', urlRoot+EULA);
        slPluginHTML = slPluginHTML.replace('{3}', urlRoot+Privacy);
        slPluginHTML = slPluginHTML.replace('{4}', urlRoot+bottom);
        slPluginHTML = slPluginHTML.replace('{5}', urlRoot+center);
    }
    else
    {       
//<parsingTag><noSilverlight.Debug.js> 
        if (slPluginHelper.shortVer == '1.1')
        {
            target = '92821'; //this is the unsupported platform target for 1.1
                    //upgrade image, when available
            if ( Silverlight.available )
            {
                image = '94378';
            }
            else
            {
                image = '92810';
            }
            
            if (OS=='Windows')
            {
                target = '92809';
            }
            else if (OS=='MacIntel')
            {
                target = '92813';
            }
            // PPC Mac is not supported. Thus, the unsupported platform links will be used.
        }
        else
        {
//<parsingTag></noSilverlight.Debug.js>
            if ( Silverlight.available )
            {
                image = '94377';
            }
            else
            {
                image = '92801';
            }
            if (OS=='Windows')
            {
                target = '92800';
            }
            else if (OS=='MacIntel')
            {
                target = '92812';
            }
            else if (OS=='MacPPC')
            {
                target = '92811';
            }
//<parsingTag><noSilverlight.Debug.js>
        }
//<parsingTag></noSilverlight.Debug.js>
        slPluginHTML = '<div style="display:block; width: 205px; height: 67px;"><img onclick="javascript:Silverlight.followFWLink({0});" style="border:0; cursor:pointer" src="{1}" title="' + L_ImageTitle_Text + '" alt="' + L_ImageTitle_Text + '"/></div>';
    }
    slPluginHTML = slPluginHTML.replace('{0}', target );
    slPluginHTML = slPluginHTML.replace('{1}', urlRoot+image+'&amp;clcid='+L_CLCID_Text);
    return slPluginHTML;
}

///////////////////////////////////////////////////////////////////////////////////////////////
/// Releases event handler resources when the page is unloaded
///////////////////////////////////////////////////////////////////////////////////////////////
Silverlight.__cleanup = function ()
{
    for (var i = Silverlight._silverlightCount - 1; i >= 0; i--) {
        window['__slLoad' + i] = null;
        window['__slError' + i] = null;
    }
    if (window.removeEventListener) { 
       window.removeEventListener('unload', Silverlight.__cleanup , false);
    }
    else { 
        window.detachEvent('onunload', Silverlight.__cleanup );
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////
/// Navigates to a url based on fwlinkid
///////////////////////////////////////////////////////////////////////////////////////////////
Silverlight.followFWLink = function(linkid)
{
    top.location=Silverlight.fwlinkRoot+String(linkid);
}
///////////////////////////////////////////////////////////////////////////////////////////////
/// Encodes special characters in input strings as charcodes
///////////////////////////////////////////////////////////////////////////////////////////////
Silverlight.HtmlAttributeEncode = function( strInput )
{
	var c;
	var retVal = '';

    if(strInput == null)
	{
	    return null;
    }
	
	for(var cnt = 0; cnt < strInput.length; cnt++)
	{
		c = strInput.charCodeAt(cnt);

		if (( ( c > 96 ) && ( c < 123 ) ) ||
			( ( c > 64 ) && ( c < 91 ) ) ||
			( ( c > 43 ) && ( c < 58 ) && (c!=47)) ||
			( c == 95 ))
		{
			retVal = retVal + String.fromCharCode(c);
		}
		else
		{
			retVal = retVal + '&#' + c + ';';
		}
	}
	
	return retVal;
}

