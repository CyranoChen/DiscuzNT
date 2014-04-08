var waitinghint = '<div style="width: 200px;">加载中...</div>';
function _DS_popup(event, id)
{	
	var popupmenu = _gel("DD_" + id), sender = _gel("DD_tg_" + id), owner = _gel("DS_DD_div");
	if(popupmenu.style.display == "none")
	{
		var pos = _findPos(sender);
		popupmenu.style.display = "block";
		var width = popupmenu.offsetWidth;
		popupmenu.style.left = pos[0] + sender.offsetWidth / 2 - width / 2;
		popupmenu.style.top = pos[1] + sender.offsetHeight;
		ds_p = id;
	}	
}

function createDIV(divname){
	var div = document.createElement("DIV");
	div.id = divname;
	div.style.display = "none";
	div.className = "NavBig";
	div.innerHTML = waitinghint;
	_gel("DS_DD_div").appendChild(div);
}

function initThemes(){
	createDIV("DD___theme");
}
function _toggleById(id) {
	a = _gel(id);
	if (a.style.display == "" || a.style.display == "block") {
		a.style.display = "none"
	}else if (a.style.display=="none") {
		a.style.display = "block"
	}
}

function loadThemes(){
	closeAll();

	var url = window.location.protocol + "//" + window.location.host + forumpath + "ss.aspx?type=theme";
	var div = _gel("DD___theme");
	div.style.display = "block";
	_DS_FetchLocalXmlContent(url, function (response) {
		if (response == null || typeof(response) != "object" || response.firstChild == null)
		{
			div.innerHTML = "无效的数据";
			return;
		}
		var html = '<div class="NavTopic">';
		html += '		<div class="NavTitle">';
		html += '			<div class="Nright"><img src="' + forumpath + 'images/clear.gif" onclick="closeAll()" alt="关闭" title="关闭" style="margin-top:6px;"/></div>';
		html += '			<div class="Nleft"><span>设置主题</span></div>';
		html += '		</div>';
	
		var categorys = response.getElementsByTagName("Category");
		if (categorys.length == 0)
		{
			div.innerHTML = "暂时没有数据";
			return;
		}
		
		for (var i = 0; i < categorys.length; i++)
		{
			var category = categorys.item(i);
			var themes = category.getElementsByTagName("Theme");
			html += '<div class="NavContent" onclick="_toggleById(\'themecategory_' + i + '\')"><span>' + category.getAttribute("name") + '</span></div>';
			if (i > 0)
			{
				html += '	<div class="NavPhoto" style="display:none;" id="themecategory_' + i + '"><ul>';
			}
			else
			{
				html += '	<div class="NavPhoto" style="display:block;" id="themecategory_' + i + '"><ul>';
			}
			
			for (var c = 0; c < themes.length; c++)
			{
				var theme = themes[c];
				if (theme.getAttribute("directory") == themepath)
				{
					html += '	<li><div class="Selected"><img title="' + theme.getAttribute("name") + '" alt="' + theme.getAttribute("name") + '" src="' + forumpath + 'skins/themes/' + theme.getAttribute("directory") + '/about.png" /></div></li>';
				}
				else
				{
					html += '	<li><div style="cursor: pointer;" onclick="_dlsetp(\'action=changetheme&themeid=' + theme.getAttribute("themeid") + '&themepath=' + theme.getAttribute("directory") + '\')" class="MouseOut" onmouseover="this.className=\'MouseOver\'" onmouseout="this.className=\'MouseOut\'"><img title="' + theme.getAttribute("name") + '" alt="' + theme.getAttribute("name") + '" src="' + forumpath + 'skins/themes/' + theme.getAttribute("directory") + '/about.png" /></div></li>';
				}
			}

			html += '	</ul></div>';
			
		}
		html += '</div>';
		div.innerHTML = html;
		var x = document.documentElement.offsetWidth;
		var w = div.offsetWidth;
		div.style.left = x - w - 20;
		
	});
}

function initTemplates(){	
	createDIV("DD___template");
}
function loadTemplates(){
		closeAll();
		var url = window.location.protocol + "//" + window.location.host + forumpath + "ss.aspx?type=template";
		var div = _gel("DD___template");
		div.style.display = "block";
		_DS_FetchLocalXmlContent(url, function (response) {
			if (response == null || typeof(response) != "object" || response.firstChild == null)
			{
				div.innerHTML = "无效的数据";
				return;
			}
			var html = '<div class="NavShape">';
			html += '		<div class="NavTitle2">';
			html += '		<div class="Nright2"><img src="' + forumpath + 'images/clear.gif" onclick="closeAll()"  alt="关闭" title="关闭" style="margin-top:6px;"/></div>';
			html += '		<div class="Nleft2"><span>选择版式</span></div>'
			html += '		</div>';
			
	
			html += '<ul class="shape">';
			
			var itemList = response.getElementsByTagName("Template");

			if (itemList.length == 0)
			{
				div.innerHTML = "暂时没有数据";
				return;
			}

			for (var i = 0; i < itemList.length; i++) { 
				var nodeList = itemList.item(i);
				var filename = nodeList.getAttribute("filename");
				var thumbnail = nodeList.getAttribute("thumbnail");
				var title = filename.replace(/\.htm/, "").replace(/template_/,"").replace(/_/g,":");
				if (filename == currenttabTemplate)
				{
					html += '<li><div class="Selected"><img src="' + forumpath + 'skins/templates/' + thumbnail + '" alt="'+ title +'" title="'+ title +'"/></div></li>';
				}
				else
				{
					html += '<li><div style="cursor: pointer;" onclick="_dlsetp(\'action=changetemplate&t=' + tid + '&template=' + filename + '\');" class="MouseOut" onmouseover="this.className=\'MouseOver\'" onmouseout="this.className=\'MouseOut\'"><img src="' + forumpath + 'skins/templates/' + thumbnail + '" alt="'+ title +'" title="'+ title +'"/></div></li>';
				}
			}
			html += '</ul></div>';
			div.innerHTML = html;		
		});

}
function initModules(){
	createDIV("DD___gadget");
}

function closeAll(){
	_gel("DD___theme").style.display = "none";
	_gel("DD___template").style.display = "none";
	_gel("DD___gadget").style.display = "none";
}
function _DS_FetchLocalXmlContent(url, callback)
{
	_sendx(url, callback, true);
}
function loadModules(){
	closeAll();
	var url = window.location.protocol + "//" + window.location.host + forumpath + "modules/list_gadget.xml";
	var div = _gel("DD___gadget");
	div.style.display = "block";
	_DS_FetchLocalXmlContent(url, function(response) {
		if (response == null || typeof(response) != "object" || response.firstChild == null)
		{
			div.innerHTML = "无效的数据";
			return;
		}
	
		var html = '<div class="NavTopic3">';
		html += '		<div class="NavTitle3">';
		html += '		<div class="Nright3"><img src="' + forumpath + 'images/clear.gif" onclick="closeAll()" alt="关闭" title="关闭" style="margin-top:6px;"/></div>';
		html += '		<div class="Nleft3"><span>添加模块</span></div>';
		html += '		</div>';

		var categorys = response.getElementsByTagName("Category");

		if (categorys.length == 0)
		{
			div.innerHTML = "暂时没有数据";
			return;
		}
		
		for (var i = 0; i < categorys.length; i++)
		{
			var category = categorys.item(i);
			var gadgets = category.getElementsByTagName("Gadget");
			html += '	<div class="NavContent3" onclick="_toggleById(\'gadgetcategory_' + i + '\')"><span>' + category.getAttribute("name") + '</span></div>';
			if (i > 0)
			{
				html += '	<div class="NavPhoto3" style="display:none;" id="gadgetcategory_' + i + '"><ul>';
			}
			else
			{
				html += '	<div class="NavPhoto3" style="display:block;" id="gadgetcategory_' + i + '"><ul>';
			}

			for (var c = 0; c < gadgets.length; c++)
			{
				var gadget = gadgets[c];
				html += '	<li><div style="cursor: pointer;" onclick="isexistmodule(\'' + gadget.getAttribute("url") + '\');" class="MouseOut" onmouseover="this.className=\'MouseOver\'" onmouseout="this.className=\'MouseOut\'">' + gadget.getAttribute("name") + '</div></li>';
			}
			html += '	</ul></div>';
		}
		html += '	<div class="NavContent3" onclick="_toggleById(\'gadgetcategory_feed\')"><span>Feed</span></div>';
		html += '	<div class="NavPhoto3" style="display:none" id="gadgetcategory_feed"><ul><li>Url : <input id="feed" type="text" style="width:160px" /><input type="button" id="addfeed" onclick="_add_remote_module(_gel(\'feed\').value, afteraddmodule, ' + tid + ')" value="添加" /></li></ul></div>';
		html += '</div>';
		div.innerHTML = html;
		var x = document.documentElement.offsetWidth;
		var w = div.offsetWidth;
		div.style.left = x - w - 20;
	});
}

function isexistmodule(module_url)
{
	var url = window.location.protocol + "//" + window.location.host + forumpath + "modules/" + module_url;
	
	_DS_FetchXmlContent(url, function(response) {
		if (response == null || typeof(response) != "object" || response.firstChild == null)
		{
			alert("无效的数据");
		}

		var modulepref = response.getElementsByTagName("ModulePrefs")[0];

		if (modulepref.getAttribute("singleton") == "true")
		{
			//要求只能加一次,验证本Tab是否已有此模块
			for (var i = 0; i < currentTabModule.length; i++)
			{
				if (currentTabModule[i] == module_url)
				{
					alert('此模块仅允许每个页面中存在一个');
					return;
				}
			}
			
			_add_remote_module(module_url, afteraddmodule, tid);
		}
		else
		{
			_add_remote_module(module_url, afteraddmodule, tid);
		}

	});
}

function afteraddmodule()
{
}
initThemes();
initTemplates();
initModules();
