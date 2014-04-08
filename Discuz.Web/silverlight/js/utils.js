/******************************************************************************
 * WPF/E UTILITY CODE
 *****************************************************************************/

/******************************************************************************
 * Helper method for creating per-object callbacks
 * @param target object to invoke the callback on
 * @param callback method to be called on the object
 *****************************************************************************/
function delegate(target, callback) {
	var func = function() {
		callback.apply(target, arguments);
	}
	return func;
}

function createGlobalMethod(delegate) {
      if (!window.methodID)
            window.methodID = 0;
      
      var callbackName = "uniqueCallback" + (window.methodID++);
      eval(callbackName + " = delegate;");
      
      return " " + callbackName;
}

function hookEvent(control, eventName, delegate)
{
    // no such control? ignore..
    if (control==null)
        return;

    var callbackName = get_UniqueName("uniqueCallback");
    eval(callbackName + " = delegate;");
    control.addEventListener(eventName, "" + callbackName);
}

function get_UniqueName(strBase)
{
    if (!window.uniqueID)
    window.uniqueID = 1;
    return strBase + (++window.uniqueID);
}



/******************************************************************************
 * Creates an XML Object checking for browser type
 * @return the xml object if successful, false if not
 *****************************************************************************/
function createXmlObj(){
    var httprequest = false;
    if (window.XMLHttpRequest) { // if Mozilla, Safari etc
        httprequest = new XMLHttpRequest();
        if (httprequest.overrideMimeType)
            httprequest.overrideMimeType('text/xml');
    }
    else if (window.ActiveXObject){ // if IE
        try {
            httprequest=new ActiveXObject("Msxml2.XMLHTTP");
        } 
        catch (e){
            try{
                httprequest=new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e){}
        }
    }
    return httprequest;
}

/******************************************************************************
 * Replaces all instances of the given substring
 *
 * @param strTarget The substring you want to replace
 * @param strSubString The string you want to replace in.
 * @return The updated string with replacements
 *****************************************************************************/
String.prototype.replaceAll = function( strTarget, strSubString ){
    var strText = this;
    var intIndexOfMatch = strText.indexOf( strTarget );
     

    // Keep looping while an instance of the target string
    // still exists in the string.
    while (intIndexOfMatch != -1){
        // Relace out the current instance.
        strText = strText.replace( strTarget, strSubString );
         

        // Get the index of any next matching substring.
        intIndexOfMatch = strText.indexOf( strTarget );
    }
 

    // Return the updated string with ALL the target strings
    // replaced out with the new substring.
    return( strText );
}


String.prototype.cleanAndKeepNames = function(list, id)
{
    var keepNames = list;
    var strText = this;
    var name = 'x:Name="';
    var indexOfMatch = strText.indexOf( name );
    
    // Keep looping while an instance of the target string
    // still exists in the string.
    while (indexOfMatch != -1){
        var indexNextQuote = strText.indexOf( '"', indexOfMatch+8 );
        
        var thisName = strText.substr(indexOfMatch+8, (indexNextQuote-indexOfMatch)-8);
        var keep = false;
        for (var i=0; i<keepNames.length; i++)
        {
            if (thisName == keepNames[i]) {
                keep = true;
            }
        }       

        if (!keep)
        {
            var newText = strText.substr(0, indexOfMatch);
            newText += strText.substr(indexNextQuote+1);
            strText = newText;
            indexOfMatch = strText.indexOf( name, indexOfMatch );
        } else {
            var newText = strText.substr(0, indexOfMatch);
            newText += 'x:Name="'+thisName+id+'"';
            newText += strText.substr(indexNextQuote+1);
            strText = newText;
            indexOfMatch = strText.indexOf( name, indexNextQuote );
        }
        
    }
    
    return( strText );
}

String.prototype.cleanAndKeepSources = function()
{
    var strText = this;
    var imagePath = new Array();
    
    var name = 'Source="';
    var indexOfMatch = strText.indexOf( name );
    
    // Keep looping while an instance of the target string
    // still exists in the string.
    count = 1;
    while (indexOfMatch != -1){
        var indexNextQuote = strText.indexOf( '"', indexOfMatch+8 );
        var indexName = strText.lastIndexOf( 'x:Name="', indexOfMatch );
        var indexNextQuoteName = strText.indexOf( '"', indexName+8 );
        var thisName = strText.substr(indexName+8, (indexNextQuoteName-indexName)-8);
        var thisSource = strText.substr(indexOfMatch+8, (indexNextQuote-indexOfMatch)-8);

        imagePath[count] = new Object();
        imagePath[count].name = thisName;
        imagePath[count].source = thisSource;
        count++;
        
        var newText = strText.substr(0, indexOfMatch);
        newText += 'Source=""';
        newText += strText.substr(indexNextQuote+1);
        strText = newText;

        indexOfMatch = strText.indexOf( name, indexNextQuote );
    }
    imagePath[0] = strText;
    return( imagePath );
}

String.prototype.cleanAndKeepTargets = function(list, id)
{
    var keepNames = list;
    var strText = this;
    var name = 'TargetName="';
    var indexOfMatch = strText.indexOf( name );
    
    // Keep looping while an instance of the target string
    // still exists in the string.
    while (indexOfMatch != -1){
        var indexNextQuote = strText.indexOf( '"', indexOfMatch+12 );
        
        var thisName = strText.substr(indexOfMatch+12, (indexNextQuote-indexOfMatch)-12);
        var keep = false;
        for (var i=0; i<keepNames.length; i++)
        {
            if (thisName == keepNames[i]) {
                keep = true;
            }
        }       

        if (!keep)
        {
            var newText = strText.substr(0, indexOfMatch);
            newText += strText.substr(indexNextQuote+1);
            strText = newText;
            indexOfMatch = strText.indexOf( name, indexOfMatch );
        } else {
            var newText = strText.substr(0, indexOfMatch);
            newText += 'TargetName="'+thisName+id+'"';
            newText += strText.substr(indexNextQuote+1);
            strText = newText;
            indexOfMatch = strText.indexOf( name, indexNextQuote );
        }
        
    }
    
    return( strText );
}

function bringToFront(parent, element) {
	if (element == null) return;
	parent.Children.Remove(element);
	parent.Children.Add(element);
}
