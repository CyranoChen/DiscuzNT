
var DNTeditor = function(instanceName, width, height, value) 
{
    // Properties
    this.InstanceName = instanceName;
    this.Width = width || '100%';
    this.Height = height || '200';
    this.Value = value || '';
    this.BasePath = '/';
    this.RedundancyHeight = undefined;
    this.Style = '';
    this.CheckBrowser = true;
    this.DisplayErrors = true;
    this.EnableSafari = false; 	// This is a temporary property, while Safari support is under development.
    this.EnableOpera = false; 	// This is a temporary property, while Opera support is under development.
    this.Basic = false;
    this.IsAutoSave = true;
    //this.Config			= new Object() ;

    // Events
    this.OnError = null; // function( source, errorNumber, errorDescription )
    this.OnChange = null;
}

DNTeditor.prototype.Create = function()
{
    document.write(this.CreateHtml());
    this._SetValue();
}

DNTeditor.prototype._SetValue = function() 
{
    var iframe = document.getElementById(this.InstanceName + "___Frame");
    var value = this.Value;
    var style = this.Style;
    var width = this.Width;
    var height = this.Height;
    var basic = this.Basic;
    var isAutoSave = this.IsAutoSave;
    var onchange = this.OnChange;
    var redundancyheight = this.RedundancyHeight;
    iframe.onload = function() {
        InitializeEditor(iframe, value, style, width, height, basic, isAutoSave, onchange, redundancyheight);
    };
    iframe.onreadystatechange = function() {
    if (this.readyState == 'complete') {
            InitializeEditor(iframe, value, style, width, height, basic, isAutoSave, onchange, redundancyheight);
        }
    };

}

function InitializeEditor(iframe, value, style, width, height, basic, isAutoSave, onchange, redundancyheight) 
{
    if (redundancyheight == undefined)
        redundancyheight = 170;
    else if (is_moz)
        redundancyheight += 100;
    if (style != '') {
	    iframe.contentWindow.document.getElementById('editorcss').href = style;
	}
	iframe.contentWindow.document.getElementById('e_iframe').contentWindow.document.body.innerHTML = value;
	iframe.contentWindow.document.getElementById('e_textarea').value = value;
	
	//iframe.contentWindow.document.getElementById('bbcodemode').style.top = "1px";
	//iframe.contentWindow.document.getElementById('wysiwygmode').style.top = "1px";
	
	iframe.contentWindow.document.getElementById('e_textarea').style.width = width;
	iframe.contentWindow.document.getElementById('e_textarea').style.height = height;
	//iframe.contentWindow.document.getElementById('e_iframe').style.width = width;
	iframe.contentWindow.document.getElementById('e_iframe').style.height = height;
	if(is_moz || is_opera) {
	    iframe.contentWindow.document.getElementById('e_iframe').contentWindow.addEventListener('keydown', function(e) {ctlentParent(e);}, true);
	    iframe.contentWindow.document.getElementById('e_iframe').contentWindow.addEventListener('keyup', function(e) {if (onchange){onchange();}}, true);
	}
	else {
	    iframe.contentWindow.document.getElementById('e_iframe').contentWindow.document.body.attachEvent("onkeydown", ctlentParent);
	    iframe.contentWindow.document.getElementById('e_iframe').contentWindow.document.body.attachEvent("onkeyup", function(e){if (onchange){onchange();}});
	}
	var newheight = parseInt(iframe.height, 10);
	iframe.style.height = (newheight + redundancyheight) + 'px';
	if (basic && basic===true)
	{
		iframe.contentWindow.document.getElementById('e_morebuttons').style.display = "none";
		//iframe.contentWindow.document.getElementById('e_bottom').style.display = "none";
		iframe.style.height = (newheight + 90) + 'px';
	}
	if (isAutoSave == false)
	{
		iframe.contentWindow.onbeforeunload = function(){};
	}

}

DNTeditor.prototype.CreateHtml = function()
{
    	// Check for errors
	if ( !this.InstanceName || this.InstanceName.length == 0 )
	{
		this._ThrowError( 701, 'You must specify an instance name.' ) ;
		return '' ;
	}

	var sHtml = '<div>' ;

	if ( !this.CheckBrowser || this._IsCompatibleBrowser() )
	{
		sHtml += '<input type="hidden" id="' + this.InstanceName + '" name="' + this.InstanceName + '" value="' + this._HTMLEncode( this.Value ) + '" style="display:none" />' ;
		//sHtml += this._GetConfigHtml() ;
		sHtml += this._GetIFrameHtml() ;
	}
	else
	{
		var sWidth  = this.Width.toString().indexOf('%')  > 0 ? this.Width  : this.Width  + 'px' ;
		var sHeight = this.Height.toString().indexOf('%') > 0 ? this.Height : this.Height + 'px' ;
		sHtml += '<textarea name="' + this.InstanceName + '" rows="4" cols="40" style="width:' + sWidth + ';height:' + sHeight + '">' + this._HTMLEncode( this.Value ) + '<\/textarea>' ;
	}

	sHtml += '</div>' ;

	return sHtml ;

}

DNTeditor.prototype.ReplaceTextarea = function()
{
	if ( !this.CheckBrowser || this._IsCompatibleBrowser() )
	{
		// We must check the elements firstly using the Id and then the name.
		var oTextarea = document.getElementById( this.InstanceName ) ;
		var colElementsByName = document.getElementsByName( this.InstanceName ) ;
		var i = 0;
		while ( oTextarea || i == 0 )
		{
			if ( oTextarea && oTextarea.tagName.toLowerCase() == 'textarea' )
				break ;
			oTextarea = colElementsByName[i++] ;
		}

		if ( !oTextarea )
		{
			alert( 'Error: The TEXTAREA with id or name set to "' + this.InstanceName + '" was not found' ) ;
			return ;
		}

		oTextarea.style.display = 'none' ;
		//this._InsertHtmlBefore( this._GetConfigHtml(), oTextarea ) ;
		this._InsertHtmlBefore( this._GetIFrameHtml(), oTextarea ) ;
		this._SetValue();
		
	}
}

DNTeditor.prototype.InsertHtml = function( html )
{
    
}

DNTeditor.prototype._InsertHtmlBefore = function( html, element )
{
	if ( element.insertAdjacentHTML )	// IE
		element.insertAdjacentHTML( 'beforeBegin', html ) ;
	else								// Gecko
	{
		var oRange = document.createRange() ;
		oRange.setStartBefore( element ) ;
		var oFragment = oRange.createContextualFragment( html );
		element.parentNode.insertBefore( oFragment, element ) ;
	}
}

DNTeditor.prototype._GetIFrameHtml = function()
{
	var sFile = 'cp_editor.htm?style=' + this.Style ;


	var sLink = this.BasePath + 'editor/' + sFile ;

	return '<iframe id="' + this.InstanceName + '___Frame" src="' + sLink + '" allowTransparency="true" width="' + this.Width + '" height="' + this.Height + '" frameborder="0" scrolling="no"></iframe>' ;
}

DNTeditor.prototype._ThrowError = function( errorNumber, errorDescription )
{
	this.ErrorNumber		= errorNumber ;
	this.ErrorDescription	= errorDescription ;

	if ( this.DisplayErrors )
	{
		document.write( '<div style="COLOR: #ff0000">' ) ;
		document.write( '[ DNTeditor Error ' + this.ErrorNumber + ': ' + this.ErrorDescription + ' ]' ) ;
		document.write( '</div>' ) ;
	}

	if ( typeof( this.OnError ) == 'function' )
		this.OnError( this, errorNumber, errorDescription ) ;
}

DNTeditor.prototype._HTMLEncode = function( text )
{
	if ( typeof( text ) != "string" )
		text = text.toString() ;

	text = text.replace(
		/&/g, "&amp;").replace(
		/"/g, "&quot;").replace(
		/</g, "&lt;").replace(
		/>/g, "&gt;") ;

	return text ;
}

DNTeditor.prototype._IsCompatibleBrowser = function()
{
	return DNTeditor_IsCompatibleBrowser( this.EnableSafari, this.EnableOpera ) ;
}

function DNTeditor_IsCompatibleBrowser( enableSafari, enableOpera )
{
	var sAgent = navigator.userAgent.toLowerCase() ;

	// Internet Explorer
	if ( sAgent.indexOf("msie") != -1 && sAgent.indexOf("mac") == -1 && sAgent.indexOf("opera") == -1 )
	{
		var sBrowserVersion = navigator.appVersion.match(/MSIE (.\..)/)[1] ;
		return ( sBrowserVersion >= 5.5 ) ;
	}

	// Gecko (Opera 9 tries to behave like Gecko at this point).
	if ( navigator.product == "Gecko" && navigator.productSub >= 20030210 && !( typeof(opera) == 'object' && opera.postError ) )
		return true ;

	// Opera
	if ( enableOpera && navigator.appName == 'Opera' && parseInt( navigator.appVersion, 10 ) >= 9 )
			return true ;

	// Safari
	if ( enableSafari && sAgent.indexOf( 'safari' ) != -1 )
		return ( sAgent.match( /safari\/(\d+)/ )[1] >= 312 ) ;	// Build must be at least 312 (1.3)

	return false ;
}

DNTeditor.prototype.GetHtml = function()
{
    var iframe = document.getElementById(this.InstanceName + "___Frame");
	var doc = iframe.contentWindow.document;
	try {
	    doc.getElementById("wysiwygmode").click();
	}
	catch(e) {
	}
	var value;
	if (doc.getElementById('e_textarea').style.display == 'none')
	{
		value = doc.getElementById('e_iframe').contentWindow.document.body.innerHTML;
	}
	else
	{
		value = doc.getElementById('e_textarea').value;
	}
	return value;
}

DNTeditor.prototype.GetText = function()
{
	return this.GetHtml().replace(/<[^>]*>/ig, '');
}

DNTeditor.prototype._InsertNodeAtSelection = function(text) {

	this._CheckFocus();

    var editwin = document.getElementById(this.InstanceName + "___Frame").contentWindow.document.getElementById('e_iframe').contentWindow;
    var editdoc = editwin.document;
	var sel = editwin.getSelection();
	var range = sel ? sel.getRangeAt(0) : editdoc.createRange();
	sel.removeAllRanges();
	range.deleteContents();
	var node = range.startContainer;
	var pos = range.startOffset;

	switch(node.nodeType) {
		case Node.ELEMENT_NODE:
			if(text.nodeType == Node.DOCUMENT_FRAGMENT_NODE) {
				selNode = text.firstChild;
			} else {
				selNode = text;
			}
			node.insertBefore(text, node.childNodes[pos]);
			this._AddRange(selNode);
			break;
		case Node.TEXT_NODE:
			if(text.nodeType == Node.TEXT_NODE) {
				var text_length = pos + text.length;
				node.insertData(pos, text.data);
				range = editdoc.createRange();
				range.setEnd(node, text_length);
				range.setStart(node, text_length);
				sel.addRange(range);
			} else {
				node = node.splitText(pos);
				var selNode;
				if(text.nodeType == Node.DOCUMENT_FRAGMENT_NODE) {
					selNode = text.firstChild;
				} else {
					selNode = text;
				}
				node.parentNode.insertBefore(text, node);
				this._AddRange(selNode);
			}
			break;
	}
}

DNTeditor.prototype._AddRange = function (node) {
	this._CheckFocus();
    var editwin = document.getElementById(this.InstanceName + "___Frame").contentWindow.document.getElementById('e_iframe').contentWindow;
    var editdoc = editwin.document;

	var sel = editwin.getSelection();
	var range = editdoc.createRange();
	range.selectNodeContents(node);
	sel.removeAllRanges();
	sel.addRange(range);
}

DNTeditor.prototype.InsertText = function(text, movestart, moveend, select) {
    var iframe = document.getElementById(this.InstanceName + "___Frame");
	var doc = iframe.contentWindow.document;
	var editdoc = doc.getElementById('e_iframe').contentWindow.document;

	if(doc.getElementById('e_textarea').style.display == 'none') {
		if(is_moz || is_opera) {
			//applyFormat('removeformat');
			editdoc.execCommand('removeformat', false, true);

			var fragment = editdoc.createDocumentFragment();
			var holder = editdoc.createElement('span');
			holder.innerHTML = text;

			while(holder.firstChild) {
				fragment.appendChild(holder.firstChild);
			}
			this._InsertNodeAtSelection(fragment);
		} else {
		    this._CheckFocus();
			if(typeof(editdoc.selection) != 'undefined' && editdoc.selection.type != 'Text' && editdoc.selection.type != 'None') {
				movestart = false;
				editdoc.selection.clear();
			}

			var sel = editdoc.selection.createRange();

			sel.pasteHTML(text);

			if(text.indexOf('\n') == -1) {
				if(typeof(movestart) != 'undefined') {
					sel.moveStart('character', -strlen(text) + movestart);
					sel.moveEnd('character', -moveend);
				} else if(movestart != false) {
					sel.moveStart('character', -strlen(text));
				}
				if(typeof(select) != 'undefined' && select) {
					sel.select();
				}
			}
		}
	} else {
		this._CheckFocus();
		editdoc = doc.getElementById('e_textarea');
		if(typeof(editdoc.selectionStart) != 'undefined') {
			var opn = editdoc.selectionStart + 0;
			editdoc.value = editdoc.value.substr(0, editdoc.selectionStart) + text + editdoc.value.substr(editdoc.selectionEnd);

			if(typeof(movestart) != 'undefined') {
				editdoc.selectionStart = opn + movestart;
				editdoc.selectionEnd = opn + strlen(text) - moveend;
			} else if(movestart !== false) {
				editdoc.selectionStart = opn;
				editdoc.selectionEnd = opn + strlen(text);
			}
		} else if(document.selection && document.selection.createRange) {
			var sel = document.selection.createRange();
			sel.text = text.replace(/\r?\n/g, '\r\n');
			if(typeof(movestart) != 'undefined') {
				sel.moveStart('character', -strlen(text) +movestart);
				sel.moveEnd('character', -moveend);
			} else if(movestart !== false) {
				sel.moveStart('character', -strlen(text));
			}
			sel.select();
		} else {
			editdoc.value += text;
		}
	}
}

DNTeditor.prototype._CheckFocus = function() {
    try {
            var iframe = document.getElementById(this.InstanceName + "___Frame");
	        iframe.contentWindow.document.getElementById('e_iframe').contentWindow.focus();
	        iframe.contentWindow.document.getElementById('e_textarea').focus();
    }	
    catch(e) {
    }
	

}


