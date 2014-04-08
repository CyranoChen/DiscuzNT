 //  工具类，使用DragUtil的命名空间，方便管理
var DragUtil = new Object();
// 获取http header里面的UserAgent，浏览器信息
DragUtil.getUserAgent = navigator.userAgent;
// 是否是Gecko核心的Browser，比如Mozila、Firefox
DragUtil.isGecko = DragUtil.getUserAgent.indexOf("Gecko") != -1;//ds_.aa
// 是否是Opera
DragUtil.isOpera = DragUtil.getUserAgent.indexOf("Opera") != -1;//ds_.ba
DragUtil.isSafari = DragUtil.getUserAgent.indexOf("Safari") != -1;//ds_.Ea
DragUtil.isIE = DragUtil.getUserAgent.indexOf("MSIE") != -1;
DragUtil.qa = "DS_pageDivMaskId";
DragUtil.oa = "DS_moduleDivMaskId";

// 获取一个element的offset信息，其实就是相对于Body的padding以内的绝对坐标
// 后面一个参数如果是true则获取offsetLeft，false则是offsetTop
//ds_.g
DragUtil.getOffset = function (el, isLeft) {
    var retValue = 0,d=0;
	if (el && el.offsetParent && el.offsetParent.id) {
		if (isLeft) {
			el = el.offsetParent
		}else {
			var e = el.offsetParent.getElementsByTagName(el.tagName);
			if (e && e[0]) {
				d = e[0].offsetTop
			}
		}
	}
    while (el) {
       retValue  +=  el["offset"  + (isLeft ? "Left" : "Top")];
       el = el.offsetParent;
	}
	return retValue-d;
};
/*
DragUtil.Va = function() {
	DragUtil.z.style.display = "none"
};
DragUtil.Wa = function() {
	DragUtil.z.style.display = ""
};
*/
DragUtil.ghostElement = null; // ds_p
//ds_.u
DragUtil.getGhostElement = function() {
	if (!DragUtil.ghostElement) {
		DragUtil.ghostElement = document.createElement("DIV");
		DragUtil.ghostElement.className = "modbox";
		DragUtil.ghostElement.backgroundColor = "";
		DragUtil.ghostElement.style.border = "2px dashed #aaa";
		DragUtil.ghostElement.innerHTML = "&nbsp;"
	}
	return DragUtil.ghostElement
};
// 将一个function（参数中的funcName是这个fuction的名字）绑定到一个element上，并且以这个element的上下文运行，其实是一种继承，这个可以google些文章看看
DragUtil.bindFunction = function (el, fucName) {
     return function () {
         return  el[fucName].apply(el, arguments);
    };
};

//绑定tabs
DragUtil.Qa = function() {
	if (DragUtil.tabs) {
		var a = DragUtil.tabs.getElementsByTagName("LI");
		if (false/*_old_html*/) {
			a = DragUtil.tabs.tBodies[0].rows[0].cells
		}
		for (var b = 0; b < a.length; b++) {
			var c = a[b];
			if (c.className.indexOf("unselectedtab") < 0)
				continue;
			if (c.style.display == "none")
				continue;
			c.isDraggableTo = true;
			c.pagePosLeft = DragUtil.getOffset(c,true);
			c.pagePosRight = c.pagePosLeft + c.offsetWidth;
			c.pagePosTop = DragUtil.getOffset(c,false);
			c.pagePosBottom = c.pagePosTop + c.offsetHeight
		}
	}
};

// 重新计算所有的可以拖拽的element的坐标，对同一个column下面的可拖拽图层重新计算它们的高度而得出新的坐标，防止遮叠
// 计算出来的坐标记录在pagePosLeft和pagePosTop两个属性里面  ds_.Pa
DragUtil.re_calcOff = function (el) {
     for (var i = 0; i < DragUtil.dragArray.length; i ++) {
         var ele = DragUtil.dragArray[i];
        ele.elm.pagePosLeft = DragUtil.getOffset(ele.elm, true);
        ele.elm.pagePosTop = DragUtil.getOffset(ele.elm, false);
    }
     var nextSib = el.elm.nextSibling;
     while (nextSib) {
        nextSib.pagePosTop -= el.elm.offsetHeight;
        nextSib = nextSib.nextSibling;
    }
};

// 隐藏Google Ig中间那个table，也就是拖拽的容器，配合show一般就是刷新用，解决一些浏览器的怪癖
DragUtil.hide = function () {
    DragUtil.rootElement.style.display ="none";
};
// 显示Google Ig中间那个table，解释同上
DragUtil.show = function () {
    DragUtil.rootElement.style.display = "";
};
DragUtil.getSortIndex = function(){
	var col_array = ['col_1', 'col_2', 'col_3', 'col_4'];
	//alert(col_array.toString());
	var sortIndex = '';
	for(var i = 0; i < col_array.length ; i++){
		sortIndex += col_array[i] + ":";
		var col = _gel(col_array[i]);
		if (col == null)
			continue;
		sortIndex += getColElements;
		sortIndex += "\n";
	}
	return sortIndex;
};
DragUtil.getColElements = function(col) {
	var elements = "";
	var childs = col.childNodes;
	for(var j = 0 ; j < childs.length ; j++){
		var s = childs[j].getAttribute("id");
		if (s != null && s.indexOf("m_")!=-1 && s.lastIndexOf("_")==1)
		{
			var id = s.substring(2);
			if( j != 0 )
				elements += ",";
			elements += id;
			
		 }
	}
	return elements;
}
DragUtil.sa = function(a) {
	DragUtil.ha();
	var b = document.createElement("DIV");
	b.id = DragUtil.qa;
	b.innerHTML = "&nbsp;";
	b.style.position = "absolute";
	b.style.width = "100%";
	b.style.height = document.body.offsetHeight + "px";
	b.style.left = "0px";
	b.style.top = "0px";
	b.style.backgroundImage = "url(images/cleardot.gif)";
	b.style.zIndex = "9999";
	document.body.appendChild(b);
	if(a.ab) {
		b = b.cloneNode(true);
		b.id = DragUtil.oa;
		b.style.height = a.elm.offsetHeight - a.header.offsetHeight + "px";
		b.style.top = a.header.offsetHeight + "px";
		a.elm.appendChild(b)
	}
};
DragUtil.ha = function() {
	var a = [DragUtil.oa,DragUtil.qa];
	for (var b = 0; b < a.length; b++) {
		var c = _gel(a[b]);
		if(c) {
			c.parentNode.removeChild(c);
			c = null
		}
	}
};
DragUtil.gb=function(){
	var a="";
	for(var b=0;b<DragUtil.column.length;b++){
		var c=DragUtil.column[b];
		for(var d=0;d<c.childNodes.length-1;d++){
			var e=c.childNodes[d];
			if(e.tagName=="DIV"){
				a+=a!=""?":":"";
				a+=e.id.substring(2)+"_"+c.id.substring(2)
			}
		}
	}
	_xsetp("mp="+_esc(a))
};
DragUtil.ia=function(col, a) {
	/*
	var b="",c=_gel("mobile_screen");
	for(var d=0;d<c.childNodes.length;d++){
		var e=c.childNodes[d];
		if(e.style.display!="none"){
			b+=b!=""?":":"";b+=e.id.substring(9)
		}
	}
	*/
	var b = "pane" + col.id.substring(4);
	var c = DragUtil.getColElements(col);

	_xsetp(/*"mobile_mp="+_esc(b)+"&*/"panename=" + _esc(b) + "&modules_mp=" + c + "&action="+_esc(a))
};

function undraggable(el)
{
	this.E = function(){this.elm = null;};
	this.elm = el;
	this.elm.innerHTML = "<br />";
}

// 初始化可以拖拽的Element的函数，与拖拽无关的我去掉了ds_T
function draggable(el) {
 	this._urlMouseUp=ds__;
	this._urlMouseDown=ds_Z;
	this._urlClick=ds_Y;
	// 公用的开始拖拽的函数
	this._dragStart = start_Drag;
	// 公用的正在拖拽的函数
	this._drag = when_Drag;
	// 公用的拖拽结束的函数
	this._dragEnd = end_Drag;
	// 这个函数主要用来进行拖拽结束后的dom处理
	this._afterDrag = after_Drag; //this.V=ds_w
	this.M=ds_X;
	this.E=ds_v;

	// 是否正在被拖动，一开始当然没有被拖动
	this.isDragging = false;
	// 将这个Element的this指针注册在elm这个变量里面，方便在自己的上下文以外调用自己的函数等，很常用的方法
	this.elm = el;
	// 触发拖拽的Element，在这里就是这个div上显示标题的那个div
	this.header = _gel(el.id + "_h");
	this.url = _gel(el.id + "_url");
	// 对于有iframe的element拖拽不同，这里检测一下并记录
	this.hasIFrame = this.elm.getElementsByTagName("IFRAME").length > 0;//ds_.ab
	this.elm.DND_Module = this;
	// 如果找到了header就绑定drag相关的event
	if (this.header) {
		 // 拖拽时的叉子鼠标指针
		 this.header.style.cursor = "move";
		 // 将函数绑定到header和element的this上，参照那个函数的说明
		 Drag.init(this.header, this.elm);
		 // 下面三个语句将写好的三个函数绑定给这个elemnt的三个函数钩子上，也就实现了element从draggable继承可拖拽的函数
		 this.elm.onDragStart = DragUtil.bindFunction(this, "_dragStart");
		 this.elm.onDrag = DragUtil.bindFunction(this, "_drag");
		 this.elm.onDragEnd = DragUtil.bindFunction(this, "_dragEnd");
		 if (this.url)
		 {
			this.url.style.cursor = "pointer";
			if(DragUtil.isGecko){
				this.url.onmousedown=DragUtil.bindFunction(this,"_urlMouseDown");
				this.url.onclick=DragUtil.bindFunction(this,"_urlClick")
			}else{
				this.url.onmouseup=DragUtil.bindFunction(this,"_urlMouseUp")
			}
		 }
	}
};
function ds_v(){
	if(this.header){
		if(this.url){
			this.url.onclick=null;
			this.url.onmouseup=null;
			this.url=null
		}
		Drag.uninit(this.header,this.elm);
		this.elm.onDragStart=null;
		this.elm.onDrag=null;
		this.elm.onDragEnd=null;
		this.header=null
	}
	this.elm=null
}
function ds__(a){
	a=Drag.fixE(a);
	if(this.i||!this.url||!this.url.href||a.which!=1){
		return true
	}
	this.M("titleclick");
	if(this.url.target||a.shiftKey){
		window.open(this.url.href,this.url.target)
	}else{
		document.location=this.url.href
	}
	return false
}
function ds_Z(a){
	a=Drag.fixE(a);
	a.stopPropagation();
	return true
}
function ds_Y(a){
	if(!this.isDragging&&this.url&&this.url.href){
		this.M("titleclick");
		return true
	}
	return false
}

// 下面就是draggable里面用到的那4个function
// 公用的开始拖拽的函数ds_W
function start_Drag() {
     // 重置坐标，实现拖拽以后自己的位置马上会被填充的效果
     DragUtil.re_calcOff(this);
	 DragUtil.Qa();
     // 记录原先的邻居节点，用来对比是否被移动到新的位置
     this.origNextSibling = this.elm.nextSibling;
     // 获取移动的时候那个灰色的虚线框
     var _ghostElement = DragUtil.getGhostElement();
     // 获取正在移动的这个对象的高度
     var offH = this.elm.offsetHeight;
    // if (DragUtil.isGecko) {
         // 修正gecko引擎的怪癖吧
        offH -= parseInt(_ghostElement.style.borderTopWidth) * 2;
   // }
     // 获取正在移动的这个对象的宽度
     var offW = this.elm.offsetWidth;
     // 获取left和top的坐标
     var offLeft = DragUtil.getOffset(this.elm, true);
     var offTop = DragUtil.getOffset(this.elm, false);
	 if (DragUtil.isIE)
	 {
		offTop = offTop - _gel('nhdrwrap').offsetHeight - _gel('topbar').offsetHeight;
		offLeft = offLeft - _gel('col_1').offsetLeft;
	 }
     // 防止闪烁，现隐藏
    DragUtil.hide();
     // 将自己的宽度记录在style属性里面
     this.elm.style.width = offW + "px";
     // 将那个灰框设定得与正在拖动的对象一样高，比较形象
	 _ghostElement.style.height = offH + "px";
     // 把灰框放到这个对象原先的位置上
     this.elm.parentNode.insertBefore(_ghostElement, this.elm.nextSibling);
     // 由于要拖动必须将被拖动的对象从原先的盒子模型里面抽出来，所以设定position为absolute，这个可以参考一下css布局方面的知识
     this.elm.style.position = "absolute";
     // 设置zIndex，让它处在最前面一层，当然其实zIndex=100是让它很靠前，如果页面里有zIndex>100的，那……
     this.elm.style.zIndex = 10000;
     // 由于position=absolute了，所以left和top实现绝对坐标定位，这就是先前计算坐标的作用，不让这个模型乱跑，要从开始拖动的地方开始移动
     this.elm.style.left = offLeft + "px";
     this.elm.style.top = offTop + "px";
     // 坐标设定完毕，可以显示了，这样就不会闪烁了
     DragUtil.show();
     // 这里本来有个ds_d.G，没搞明白干什么用的，不过没有也可以用，谁知道麻烦告诉我一声，不好意思
     DragUtil.sa(this);
	// 还没有开始拖拽，这里做个记号
     this.isDragging = false;
     return false;
};
// 在拖拽时的相应函数，由于绑定到鼠标的move这个event上，所以会传入鼠标的坐标clientX, clientY  -- ds_Q(a,b)
function when_Drag(clientX, clientY) {
     // 刚开始拖拽的时候将图层变透明，并标记为正在被拖拽
     if (! this.isDragging) {
         this.elm.style.filter = "alpha(opacity=50)";
         this.elm.style.opacity = 0.5;
         this.isDragging = true;
    }
     // 被拖拽到的新的column（当然也可以是原来那个）
     var found = null;
     // 最大的距离，可能是防止溢出或者什么bug
     var max_distance = 100000000;
     // 遍历所有的可拖拽的element，寻找离当前鼠标坐标最近的那个可拖拽元素，以便后面插入
	for (var i = 0; i < DragUtil.dragArray.length; i ++) {
		var ele = DragUtil.dragArray[i];
		// 利用勾股定理计算鼠标到遍历到的这个元素的距离
		var distance = Math.sqrt(Math.pow(clientX - ele.elm.pagePosLeft, 2) + Math.pow(clientY - ele.elm.pagePosTop, 2));
		// 自己已经浮动了，所以不计算自己的
		if (ele == this) {
			continue;
		}
		// 如果计算失败继续循环
		if (isNaN(distance)) {
			continue;
		}
		// 如果更小，记录下这个距离，并将它作为found
		if (distance < max_distance) {
			max_distance = distance;
			found = ele;
		}
	}
	
	this.n = null;
	if(DragUtil.tabs){
		var h=DragUtil.tabs.getElementsByTagName("LI");
		/*if(_old_html){
			h=ig_.A.tBodies[0].rows[0].cells
		}*/
		for(var e=0;e<h.length;e++){
			var f=h[e];
			if(!f.isDraggableTo)continue;
			var i=ds_getSrollXY();
			if(this.elm.lastMouseX>=f.pagePosLeft&&this.elm.lastMouseX<=f.pagePosRight&&this.elm.lastMouseY+i[1]>=f.pagePosTop&&this.elm.lastMouseY+i[1]<=f.pagePosBottom){
				this.n=f;var j=DragUtil.getGhostElement();
				if(j.parentNode!=null){
					j.parentNode.removeChild(j)
				}
				break
			}
		}
		for(var e=0;e<h.length;e++){
			var f=h[e];
			if(f.id.indexOf("view")>=0){
				if(f==this.n){
					if(f.className.indexOf(" tab_hover")<0){
						f.className+=" tab_hover"
					}
				}else{
					f.className=f.className.replace(/ tab_hover/g,"")
				}
			}
		}
	}

     // 准备让灰框落脚
	var _ghostElement = DragUtil.getGhostElement();
	//_ghostElement.className = "";
	// 如果找到了另外的落脚点
	if (this.n == null && found != null && _ghostElement.nextSibling != found.elm) {
		// 找到落脚点就先把灰框插进去，这就是我们看到的那个灰框停靠的特效，有点像吸附的感觉，哈哈
		found.elm.parentNode.insertBefore(_ghostElement, found.elm);

		//修正元素宽度
		this.elm.style.width = _ghostElement.offsetWidth + "px";
		if (DragUtil.isOpera) {
			// Opera的现实问题，要隐藏/显示后才能刷新出变化
			document.body.style.display = "none";
			document.body.style.display = "";
		}
	}
};
// 拖拽完毕ds_R
function end_Drag() {
	DragUtil.ha();

     // 拖拽完毕后执行后面的钩子，执行after_Drag()，如果布局发生了变动了就记录到远程服务器，保存你拖拽后新的布局顺序
	if(this.n){
		var a=this.n.id.match(/tab(\d+)_/)[1],b=this.elm.id.match(/m_(\d+)/)[1];
		_xsetp("mt="+b+":"+a);
		this.elm.style.display="none";
		this.n.className=this.n.className.replace(/ tab_hover/g,"")
	}
	else
	{
		if (this._afterDrag()) {
			var col = _gel(this.elm.parentNode.id);
			DragUtil.ia(col, "move")
			 // remote call to save the change
		}
	}
	if(!_uli){
		var c=_gel("new_user_tip");
		if(c){
			c.style.display="none"
		}
		var d=_gel("new_user_save");
		if(d){
			d.style.display="block"
		}
	}
	if(this.isDragging){
		this.M("dragend")
	}

	return true;
};
// 拖拽后的执行钩子ds_w
function after_Drag() {
	var returnValue = false;
	// 防止闪烁
	// DragUtil.hide();
	// 把拖拽时的position=absolute和相关的那些style都消除
	this.elm.style.position = "";
	this.elm.style.width = "";
	this.elm.style.zIndex = "";
	this.elm.style.filter = "";
	this.elm.style.opacity = "";
	window.scrollBy(0,0);
	// 获取灰框
	var ele = DragUtil.getGhostElement();
	if (ele.parentNode != null) {

		// 如果现在的邻居不是原来的邻居了
		if (ele.nextSibling != this.origNextSibling) {
			// 把被拖拽的这个节点插到灰框的前面
			ele.parentNode.insertBefore(this.elm, ele.nextSibling);
			// 标明被拖拽了新的地方
			returnValue = true;
		}
		ele.parentNode.removeChild(ele)
	}
	// 移除灰框，这是这个灰框的生命周期应该就结束了
	// ele.parentNode.removeChild(ele);
	// 修改完毕，显示
	//DragUtil.show();
	if (DragUtil.isOpera) {
		// Opera的现实问题，要隐藏/显示后才能刷新出变化
		document.body.style.display = "none";
		document.body.style.display = "";
	}
	return returnValue;
};

// 可拖拽Element的原形，用来将event绑定到各个钩子，这部分市比较通用的，netvibes也是基本完全相同的实现
// 这部分推荐看dindin的这个，也会帮助理解，http://www.jroller.com/page/dindin/?anchor=pro_javascript_12
var Drag = {
     // 对这个element的引用，一次只能拖拽一个Element
    obj: null , 
     // element是被拖拽的对象的引用，elementHeader就是鼠标可以拖拽的区域
    init: function (elementHeader, element) {
         // 将start绑定到onmousedown事件，按下鼠标触发start
        elementHeader.onmousedown = Drag.start;
         // 将element存到header的obj里面，方便header拖拽的时候引用
        elementHeader.obj = element;
         // 初始化绝对的坐标，因为不是position=absolute所以不会起什么作用，但是防止后面onDrag的时候parse出错了
         if (isNaN(parseInt(element.style.left))) {
            element.style.left = "0px";
        }
         if (isNaN(parseInt(element.style.top))) {
            element.style.top = "0px";
        }
         // 挂上空Function，初始化这几个成员，在Drag.init被调用后才帮定到实际的函数，可以参照draggable里面的内容
        element.onDragStart = new Function();
        element.onDragEnd = new Function();
        element.onDrag = new Function();
    },
	uninit: function(elementHeader, element) {
		window.clearInterval(element.Ja);
		elementHeader.onmousedown = null;
		elementHeader.obj = null;
		element.onDragStart = null;
		element.onDragEnd = null;
		element.onDrag = null
	},
     // 开始拖拽的绑定，绑定到鼠标的移动的event上
    start: function (event) {
	var element = Drag.obj = this.obj;
	// 解决不同浏览器的event模型不同的问题
	event = Drag.fixE(event);
	// 看看是不是左键点击
	if (event.which != 1) {
	 // 除了左键都不起作用
		return true;
	}
	// 参照这个函数的解释，挂上开始拖拽的钩子
	element.onDragStart();
	// 记录鼠标坐标
	element.lastMouseX = event.clientX;
	element.lastMouseY = event.clientY;


	if(DragUtil.isSafari) {
		element.lastMouseY -= document.body.scrollTop
	}
	element.Ja = window.setInterval(ds_windowScroll(element,ds_getScrollHeight()),10);


	// 将Global的event绑定到被拖动的element上面来
	document.onmouseup = Drag.end;
	document.onmousemove = Drag.drag;
	return false;
    }, 
     // Element正在被拖动的函数
    drag: function (event) {
		// 解决不同浏览器的event模型不同的问题
		event = Drag.fixE(event);
		// 看看是不是左键点击
		if (event.which == 0) {
			// 除了左键都不起作用
			return Drag.end();
		}
		// 正在被拖动的Element
		var element = Drag.obj;
		// 鼠标坐标
		var _clientY = event.clientY;

		if(DragUtil.isSafari) {
			_clientY -= document.body.scrollTop
		}

		var _clientX = event.clientX;
		// 如果鼠标没动就什么都不作
		if (element.lastMouseX == _clientX && element.lastMouseY == _clientY) {
			return false;
		}
		// 刚才Element的坐标
		var _lastY = parseInt(element.style.top);
		var _lastX = parseInt(element.style.left);
		// 新的坐标
		var newX, newY;
		// 计算新的坐标：原先的坐标+鼠标移动的值差
		newX = _lastX + _clientX - element.lastMouseX;
		newY = _lastY + _clientY - element.lastMouseY;
		// 修改element的显示坐标
		element.style.left = newX + "px";
		element.style.top = newY + "px";
		// 记录element现在的坐标供下一次移动使用
		element.lastMouseX = _clientX;
		element.lastMouseY = _clientY;
		// 参照这个函数的解释，挂接上Drag时的钩子
		element.onDrag(newX, newY);
		return false;
    },
     // Element正在被释放的函数，停止拖拽
    end: function (event) {
		// 解决不同浏览器的event模型不同的问题
		event = Drag.fixE(event);
		// 解除对Global的event的绑定
		document.onmousemove = null;
		document.onmouseup = null;
		window.clearInterval(Drag.obj.Ja);
		// 先记录下onDragEnd的钩子，好移除obj
		var _onDragEndFuc = Drag.obj.onDragEnd();
		// 拖拽完毕，obj清空
		Drag.obj = null;
		return _onDragEndFuc;
    }, 
     // 解决不同浏览器的event模型不同的问题
    fixE: function (ds_) {
         if (typeof ds_ == "undefined") {
            ds_ = window.event;
        }
         if (typeof ds_.layerX == "undefined") {
            ds_.layerX = ds_.offsetX;
        }
         if (typeof ds_.layerY == "undefined") {
            ds_.layerY = ds_.offsetY;
        }
         if (typeof ds_.which == "undefined") {
            ds_.which = ds_.button;
        }
         return ds_;
    }
};
function ds_X(a){
	switch(a){
		case "titleclick":
			_DS_TriggerModuleEvent(this.elm.id,a,this.url.href);
			_DS_TriggerEvent("module"+a,this.elm.id,this.url.href);
			break;
		case "dragstart":case "dragend":
			_DS_TriggerDelayedModuleEvent(this.elm.id,a);
			_DS_TriggerDelayedEvent("module"+a,this.elm.id);
			break
	}
}
//ds_9a(a,b)
function ds_windowScroll(ele, scrollHeight) {
	return function() {
		var c=ds_getClientHeight(),d=ds_getSrollXY(),e=d[1],f=4,g=0.05*c,h=e,i=ele.offsetTop;
		if(ele.lastMouseY <= g) {
			i = ele.offsetTop - f;
			h = e - f
		}else if(ele.lastMouseY >= c-g) {
			i = Math.min(scrollHeight - ele.offsetHeight,ele.offsetTop + f);
			h = Math.min(scrollHeight - c,e + f)
		}
		var j = h - e;
		if (j != 0) {
			window.scrollBy(0,j);
			ele.style.top = i + "px"
		}
	}
}
//ds_I
function ds_getSrollXY(){
	var a=0,b=0;
	if(typeof window.pageYOffset=="number"){
		a=window.pageXOffset;
		b=window.pageYOffset
	}else if(document.body&&(document.body.scrollLeft||document.body.scrollTop)){
		a=document.body.scrollLeft;
		b=document.body.scrollTop
	}else if(document.documentElement&&(document.documentElement.scrollLeft||document.documentElement.scrollTop)){
		a=document.documentElement.scrollLeft;
		b=document.documentElement.scrollTop
	}
	return[a,b]
}
//ds_fb
function ds_getClientHeight(){
	var a;
	if(window.innerHeight){
		a=window.innerHeight
	}else if(document.documentElement&&document.documentElement.clientHeight){
		a=document.documentElement.clientHeight
	}else{
		a=document.body.offsetHeight
	}if(a<document.body.clientHeight){
		return a
	}
	return document.body.clientHeight
}
//ds_ib
function ds_getScrollHeight() {
	if (document.body.scrollHeight > document.documentElement.clientHeight) {
		return document.body.scrollHeight
	}else {
		return document.documentElement.clientHeight
	}
};


function ds_gb(a,b,c)
{
	var d=b=="*"&&a.all?a.all:a.getElementsByTagName(b),e=[];
	c=c.replace(/\-/g,"\\-");
	var f=new RegExp("(^|\\s)"+c+"(\\s|$)"),g;
	for(var h=0;h<d.length;h++)
	{
		g=d[h];
		if(f.test(g.className))
		{
		e.push(g)
		}
	}
	return e
}

// 下面是初始化的函数了，看看上面这些东西怎么被调用
var _DS_initDrag = function (el, tabs) {
     // column那个容器，在google里面就是那个table布局的tbody，netvibes用的<div>
    DragUtil.rootElement = el;
	DragUtil.tabs = tabs;
	DragUtil.column = ds_gb(DragUtil.rootElement, "div", "yui-u");
	//if (_old_html)
	//{
		 // 这个tbody的行
		DragUtil._rows = DragUtil.rootElement.tBodies[0].rows[0];
		 // 列，google是3列，其实也可以更多
		DragUtil.column = DragUtil._rows.cells;
		DragUtil.column2 = null;
		if (DragUtil.rootElement.tBodies[0].rows.length > 1)
		{
			DragUtil.column2 = DragUtil.rootElement.tBodies[0].rows[1].cells;
		}
	//}

     // 用来存取可拖拽的对象
    DragUtil.dragArray = [];//new Array();
     var counter = 0;
     for (var i = 0; i < DragUtil.column.length; i ++) {
         // 搜索所有的column
         var ele = DragUtil.column[i];
		 if (ele.className.indexOf("ds_dnd_fixex_col") != -1)continue;
         for (var j = 0; j < ele.childNodes.length; j ++) {
             // 搜索每一column里面的所有element
             var ele1 = ele.childNodes[j];
             // 如果是div就把它初始化为一个draggable对象
             if (ele1.tagName == "DIV") {
                DragUtil.dragArray[counter] = ele1.className != "dm" ? new draggable(ele1) : new undraggable(ele1);
                counter ++;
            }
        }
    }
	if (DragUtil.column2 == null)
		return;
    for (var i = 0; i < DragUtil.column2.length; i ++) {
         // 搜索所有的column
         var ele = DragUtil.column2[i];
		 if (ele.className.indexOf("ds_dnd_fixex_col") != -1)continue;
         for (var j = 0; j < ele.childNodes.length; j ++) {
             // 搜索每一column里面的所有element
             var ele1 = ele.childNodes[j];
             // 如果是div就把它初始化为一个draggable对象
             if (ele1.tagName == "DIV") {
                DragUtil.dragArray[counter] = ele1.className != "dm" ? new draggable(ele1) : new undraggable(ele1);
                counter ++;
            }
        }
    }
};

// google的页面里可以拖动的部分的id是"t_1"
// 挂载到onload，载入完毕执行。

/* window.onload = function(){
	  _table = document.getElementById("t_1");
	  _tabs = null;
	 _DS_initDrag(_table, _tabs);
 }*/







function ds_s(a,b) {
	var c = function(){};
	c.prototype = a.prototype;
	b.prototype = new c
}
function _gel(a) {
	return document.getElementById?document.getElementById(a):null
}
function _gelstn(a) {
	if (a == "*" && document.all)
		return document.all;
	return document.getElementsByTagName?document.getElementsByTagName(a):[]
}
function _uc(a) {
	return a.toUpperCase()
}
function _trim(a) {
	return a.replace(/^\s*|\s*$/g,"")
}
function _esc(a) {
	return window.encodeURIComponent?encodeURIComponent(a):escape(a)
}
var ds_rb = function(a) {
	return window.decodeURIComponent?decodeURIComponent(a):unescape(a)
},_unesc = ds_rb;
function _toggle(a) {
	if (a.style.display == "" || a.style.display == "block") {
		a.style.display = "none"
	}else if (a.style.display=="none") {
		a.style.display = "block"
	}
}
function _hesc(a) {
	a = a.replace(/</g,"&lt;").replace(/>/g,"&gt;");
	a = a.replace(/"/g,"&quot;").replace(/'/g,"&#39;");
	return a
}
function _striptags(a) {
	return a.replace(/<\/?[^>]+>/gi,"")
}
var ds_qb = 0;
function _uid() {
	return "obj" + ds_qb++
}
function _min(a,b) {
	return a < b ? a : b
}
function _max(a,b) {
	return a > b ? a : b
}
function _editedTabName() {
	return _gel("tab" + _cet + "_title").value
}


function _sendx(a,b,c,d){
	var e=ds_sb();
	if(!d)d=null;
	e.open(d?"POST":"GET",a,true);
	if (d)	
		e.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	if(b){
		e.onreadystatechange=function(){
			if(e.readyState==4){
				b(c&&e.responseXML?e.responseXML:e.responseText)
			}
		}
	}
	e.send(d)
}
function _uhc(a,b,c){
	var d="m_"+a+"_"+b,e=_gel(d);
	if(!e){
		e=document.createElement("INPUT");
		e.type="hidden";
		e.disabled=true;
		e.name=d;
		_gel("m_"+a+"_form").appendChild(e)
	}
	e.value=c;
	e.disabled=false
}
function ds_sb(){
	var a=null;
	if(window.ActiveXObject){
		a=new ActiveXObject("Msxml2.XMLHTTP");
		if(!a){
			a=new ActiveXObject("Microsoft.XMLHTTP")
		}
	}else if(window.XMLHttpRequest){
		a=new XMLHttpRequest
	}
	return a
}












var _et="",_source="",_pid="",_authpath="",_prefid="",_setp_url=forumpath+"setp.aspx",_old_html=true,ds_l=null,ds_m=null;

var ds_b={T:0,La:1800,da:null,ca:null,ka:function(){ds_b.T=(new Date).getTime()+ds_b.La*1000},eb:function(){if((new Date).getTime()>ds_b.T){_reload(ds_b.T)}},fa:function(a){ds_b.ka();
if(ds_b.da){ds_b.da(a)}},ea:function(a){ds_b.ka();if(ds_b.ca){ds_b.ca(a)}}};function _DS_start_refresh_cycle(a,b){ds_b.da=document.onmousedown;ds_b.ca=document.onkeyup;ds_b.La=a;if(document.addEventListener){document.addEventListener("keyup",ds_b.ea,false);document.addEventListener("mousedown",ds_b.fa,false)}else if(document.attachEvent){document.attachEvent("onkeyup",ds_b.ea);document.attachEvent("onmousedown",ds_b.fa)}else{document.onkeyup=ds_b.ea;document.onmousedown=ds_b.fa}ds_b.ka();setInterval(ds_b.eb,
b*1000)}function _DS_GetImageUrl(a){var b=forumpath+"proxy.aspx?url="+_esc(a);return _et!=""?b+"&et="+_et:b}function _DS_GetImage(a){var b=new Image;b.src=_DS_GetImageUrl(a);return b}

function _findPos(a){var b=0,c=0;while(a!=null){b+=a.offsetLeft;c+=a.offsetTop;a=a.offsetParent}return[b,c]}function _getGadgetContainer(a){var b=_gel("m_"+a+"_b");if(!b){b=_gel("remote_"+a);if(!b){b=document.body}}return b};



function _zm(a,b) {
	var c = _gel("m_" + a + "_b");
	if(c) {
		var d = c.style.display != "none";
		c.style.display=d?"none":"block";
		var e = _gel("m_" + a + "_zippy");
		if(e) {
			if(d) {
				e.className=e.className.replace(/minbox/,"maxbox")
			}else {
				e.className=e.className.replace(/maxbox/,"minbox")
			}
		}
		_xsetp("mz=" + a + ":" + (d?"1":"0") + "&t=" + b);
		ds_j(d?"zip":"unzip",a)
	}
	return false
}
function ds_O(){_DS_TriggerDelayedEvent("xsetpdone");ds_N()}
function _xsetp(a){ds_t.push(a);if(!ds_u){ds_N()}}
function _dlsetp(a,b){
	if(!b){
		b=_esc(document.location)
	}
	document.location=_setp_url+ds_i()+"et="+_et+"&source="+_source+"&pid="+_pid+"&ap="+_authpath+"&prefid="+_prefid+"&url="+b+"&"+a
}
function _ssbc(a,b,c){
	var d=_gelstn("*");
	for(var e=0;e<d.length;e++){
		if(d[e].className==a){
			d[e].style[b]=c
		}
	}
}
function ds_kb(a){
	var b=_gelstn("*");
	for(var c=0;c<b.length;c++){
		for(var d=0;d<a.length;d++){
			if(b[c].className==a[d][0]){
				b[c].style[a[d][1]]=a[d][2]
			}
		}
	}
}

function ds_d(form,n,v){
	var d=document.createElement("input");
	d.type="hidden";
	d.name=n;
	d.value=v;
	form.appendChild(d)
}

function _args(){
	var a={},b=document.location.search.substring(1),c=b.split("&");
	for(var d=0;d<c.length;d++){
		var e=c[d].indexOf("=");
		if(e==-1)
			continue;
		var f=c[d].substring(0,e),g=c[d].substring(e+1);
		g=g.replace(/\+/g," ");
		a[f]=_unesc(g)
	}
	return a
}

function ds_i(){
	var a={pid:1,host:1,hl:1,uid:1},b=_args(),c="?";
	for(var d in b){
		if (d == 'extend'){
			continue;
		}
		if(a[d]){
			c+=d+"="+_esc(b[d])+"&"
		}
	}
	return c
}

function _fsetp(form,mid,tab){
	form.action=_setp_url;
	form.method="get";
	ds_d(form,"url",document.location);
	ds_d(form,"et",_et);
	ds_d(form,"source",_source);
	ds_d(form,"pid",_pid);
	ds_d(form,"ap",_authpath);
	ds_d(form,"prefid",_prefid);
	ds_d(form,"m_" + mid + "_t",tab);
	ds_d(form,"mid",mid);
	var d=_args(),e=d["host"],f=d["hl"];
	if(typeof e!="undefined"){
		ds_d(form,"host",e)
	}
	if(typeof f!="undefined"){
		ds_d(form,"hl",f)
	}
	return true
}


var ds_t=[],ds_u=false;
function ds_N() {
	if(ds_t.length==0){
		ds_u=false;
		return
	}
	ds_u=true;
	var a=_setp_url+ds_i()+"et="+_et+"&pid="+_pid+"&ap="+_authpath+"&source="+_source+"&prefid="+_prefid,b=ds_t.shift(),c=a.length+b.length>=1800;
	if(c){
		_sendx(a,ds_O,false,b)
	}else{
		a+="&"+b;
		_sendx(a,ds_O,false,null)
	}
}



function _edit(a,b){_gel("m_"+a).className="modbox_e";if(b){b()}ds_j("edit","m_"+a);return false}function _cedit(a){_gel("m_"+a).className="modbox";var b=_gel("m_"+a+"_form");b.reset();ds_j("canceledit","m_"+a);return false}function _confirmDel(a,b,c,d){var e=
_gel("m_"+a+"_title").innerHTML;d=d.replace(/MODULE_TITLE/,'"'+e+'"');if(confirm(d)){_del(a,b,c)}}
function _del(a,b,c){
	_xsetp("action=delmodule&m="+a+"&m_"+a+"_t="+b);
	var d=_gel("undel_msg");
	if(d){
		_gel("undel_title").innerHTML=_striptags(_gel("m_"+a+"_h").innerHTML)+" ";
		d.style.display="block";
		var e=_gel("undo_msg");
		if(e){
			e.style.display="none"
		}
	}
	var f=_gel("m_"+a);
	if(f){
		f.style.display="none"
	}
	ds_l=a;
	ds_m=c;
	var g=_gel(c);
	if(g){
		g.style.display=""
	}
	_mod=true;
	ds_j("delete","m_"+a);
	return false
}
function _reload(a){var b=a-(new Date).getTime();if(b>1000){setTimeout("_reload("+a+")",b);return}document.cookie="IGREL=1";document.location.reload()}
function ds_j(a,b){switch(a){case "delete":case "undelete":case "edit":case "canceledit":case "zip":case "unzip":/*_DS_TriggerDelayedModuleEvent(b,a);_DS_TriggerDelayedEvent("module"+a,b);*/break}}


var _uli,
	_pnlo,
	_mpnlo,
	_pl,
	_mod,
	_cbp=false,
	ds_J=false,
	ds_K=false,
	_table=null,
	_tabs=null,
	_insert_to_col=1;
function _upc(){
	var a=[];
	if(!_cbp){
		a[a.length]=["medit","display",_uli?"":"none"]
	}
	a[a.length]=["panelo","display",_pnlo?"":"none"];
	a[a.length]=["panelc","display",_pnlo?"none":""];
	if(_mod){
		a[a.length]=["unmod","display","none"];
		a[a.length]=["mod","display",""]
	}else{
		a[a.length]=["mod","display","none"];
		a[a.length]=["unmod","display",""]
	}
	ds_kb(a);
	if(_pl){
		if(_cbp||_uli){
			if(!ds_J&&!_mpnlo){
				_DS_initDrag(_table,_tabs);
				ds_J=true
			}/*else if(!ds_K&&_mpnlo){
				_DS_initMobileDrag(_table);ds_K=true
			}*/
		}
	}
}

function _add_m(a,b){_dlsetp(a+"&col="+_insert_to_col,b)}
function _add_m_confirm(a,b,c){if(confirm(b)){_add_m(a,c)}}
var ds_D=/^_add_m(_confirm)?\(\"[^"]+\"(, *\"[^"]+\")?\)$/;
function _add_remote_module(a,b,c){
	_sendx(forumpath+"feeds.aspx"+ds_i()+"module=1&url="+_esc(a)+"&tab="+_esc(c),function(d){
		b();
		ds_5a(d)
		},false,null);
	return false
}
function ds_5a(a){var b=/^alert\(\"[^"]+\"\)$/;if(a.match(ds_D)!=null||a.match(b)!=null){eval(a)}}

var _cet=-1;
function _editTab(a){
	if(_cet!=-1)return;
	_renameTab();
	_cet=a;
	_gel("tab"+_cet+"_view").style.display="none";
	_gel("tab"+_cet+"_edit").style.display="";
	_gel("tab"+_cet+"_title").select();
	_gel("tab"+_cet+"_title").focus()
}
function _renameTab(){
	if(_cet==-1)return;
	var a=_editedTabName();
	_xsetp("action=renametab&t=" + _cet + "&rt_"+_cet+"="+_esc(a));
	_gel("tab"+_cet+"_view_title").innerHTML=_hesc(a);
	var b=_gel("tip_tabtitle");
	if(b){b.innerHTML=_hesc(a)}_gel("tab"+_cet+"_edit").style.display="none";
	_gel("tab"+_cet+"_view").style.display="";
	_cet=-1;
	return false
}



/*  popup  */
function _DS_FR_toggle(a,b){
	var c=_gel("ft_"+a+"_"+b),d=_gel("fb_"+a+"_"+b),e=_gel("ftl_"+a+"_"+b);
	if(d.style.display=="block"){
		ds_fmax(c,d,e)
	}else{
		var f=eval("FEED"+a);
		if(!f.has_entries&&!f.is_fetching){
			f.is_fetching=true;
			ds_showfdetail(a,b);
		}else{
			for(var g=0;g<f.items.length;g++){
				var h=_gel("ftl_"+a+"_"+g);
				if(h){
					ds_fmax(_gel("ft_"+a+"_"+g),_gel("fb_"+a+"_"+g),h)
				}
			}
			/*if(d.style.height!=300&&ds_Ia(e.offsetWidth,d)>300){
				d.style.height=300
			}*/
			ds_fmin(c,d,e)
		}
	}
}
function ds_showfdetail(a,b){
	var c = eval("FEED"+a);
	c.is_fetching=false;
	if(c&&c.items){
		for(var e=0;e<c.items.length;e++){
			var f=_gel("fb_"+a+"_"+e);
			if(f){
				f.innerHTML=c.items[e].description
			}
		}

		c.has_entries=true;
		_DS_FR_toggle(a,b)
	}
}
function ds_fgtp(a,b){
	var feed = eval('FEED' + a);
	var min = (b-1)*feed.num_items;
	var max = min + feed.num_items;
	for (var i = 0; i < feed.items.length; i++){
		var disp = _gel('ftl_' + a + '_' + i).style;
		if (i >= min && i < max){
			disp.display = 'block';
		}else{
			disp.display = 'none';
		}
	}
	var c = _gel("FEED_" + a + "pages");
	c.innerHTML = "";
	if (b > 1){
		c.innerHTML += "<a class='prev' onclick='ds_fgtp(" + a + ", " + (b - 1) + ");'>上一页</a>";
	}
	if (b < feed.page_count){
		c.innerHTML += "<a class='next' onclick='ds_fgtp(" + a + ", " + (b + 1) + ");'>下一页</a>";
	}
	feed.current_page = b
}
function ds_fmax(a,b,c){
	a.className="fmaxbox";
	b.style.display="none";
	c.className="uftl"
}
function ds_fmin(a,b,c){
	a.className="fminbox";
	b.style.display="block";
	c.className="sftl"
}
function ds_Ia(a,b){
	var c=document.createElement("div");
	c.style.left=-screen.width;
	c.style.top=-screen.height;
	c.style.width=a;
	c.innerHTML=b.innerHTML;
	document.body.appendChild(c);
	var d=c.clientHeight;
	document.body.removeChild(c);
	return d
};
function _DS_DD_init(){document.write("<div id=DS_DD_div></div>")}function _DS_DD_create(a,b){var c=document.createElement("div");c.setAttribute("id","DD_"+a);c.onblur=_DS_DD_hide;c.className="dd dd_border";c.style.display="none";var d="";for(var e=0;e<b.length;e++){d+=ds_ia(b[e][0],b[e][1],b[e][2])}c.innerHTML=d;_gel("DS_DD_div").appendChild(c)}function ds_ia(a,b,c){if(b==""){b="javascript:void(0)"}var d="<div class=dd_item onclick='_DS_DD_hide();"+c+";'><a href='"+b+"'>"+a+"</a></div>";
return d}function _DS_DD_open(a,b){var c=_gel("DD_"+b),d=_gel("DD_tg_"+b),e=_gel("DS_DD_div");if(c.style.display=="none"){var f=_findPos(d);c.style.display="block";var g=c.offsetWidth;c.style.left=f[0]+d.offsetWidth/2-g/2+"px";c.style.top=f[1]+d.offsetHeight+"px";ds_p=b;e.className="dd_layer";e.onclick=_DS_DD_hide;e.style.height=document.body.clientHeight+document.body.scrollTop+"px";e.style.width=document.body.clientWidth+document.body.scrollLeft+"px"}}function _DS_DD_hide(){if(ds_p){var a=_gel("DD_"+
ds_p);a.style.display="none"}var b=_gel("DS_DD_div");b.className="";b.style.height="";b.style.width="";b.onclick=""};

/*  popup end  */




/*  pref start  */
function ds__a(){
	var a=Math.floor(arguments.length/2),b=[];
	b.push("mid=" + this.Fa);
	for(var c=0;c<a;c++){
		var d=arguments[c*2],e=arguments[c*2+1];
		if(_DS_Prefs.$){
			b.push("m_"+this.Fa+"_up_"+d+"="+_esc(e))
		}else{
			b.push(d,e)
		}
		this.h[_DS_Prefs.D+d]=e
	}
	if(_DS_Prefs.$){
		_xsetp(b.join("&"))
	}else{
		if(_args()["synd"]=="open"){
			return
		}
		_IFPC_SetPref(b)
	}
}
function _DS_Prefs(a){
	this.getString=ds_Wa;
	this.getInt=ds_Pa;
	this.getBool=ds_Na;
	this.getArray=ds_Ma;
	this.setArray=ds_0a;
	this.getCountry=ds_Oa;
	this.getLang=ds_Qa;
	this.getMsg=ds_Ta;
	this.getMsgFormatted=ds_Ua;
	this.set=ds__a;
	this.getPreloadedString=ds_Va;
	this.dump=ds_La;
	if(typeof a=="undefined"){
		var b=_args()["mid"];
		if(b){a=b}else{a=0}
		_DS_Prefs._parseURL(a)
	}
	this.getModuleHeight=ds_Ra;
	this.getModuleWidth=ds_Sa;
	this.h=_DS_Prefs.h[_DS_Prefs.pa+a]||{};
	this.v=ds_Ya;
	this.X=ds_Xa;
	this.Fa=a;
	this.db=ds_Za
}

_DS_Prefs.Ma="h";
_DS_Prefs.Na="w";
_DS_Prefs.pa="m_";
_DS_Prefs.D="up_";
_DS_Prefs.N="msg_";
_DS_Prefs._parseURL=function(a){
	_DS_Prefs.$=false;
	var b=window.location.search.substring(1).split("&");
	for(var c=0;c<b.length;c++){
		var d=b[c].indexOf("=");
		if(d==-1)
			continue;
		var e=b[c].substring(0,d);
		e=e.replace(/\+/g," ");
		var f=b[c].substring(d+1);
		f=f.replace(/\+/g," ");
		f=_unesc(f);
		if(e.indexOf(_DS_Prefs.D)==0||e.indexOf(_DS_Prefs.N)==0){
			_DS_Prefs._add(a,e,f)
		}else if(e==".lang"||e==".country"){
			_DS_Prefs._add(a,_DS_Prefs.D+e,f)
		}
	}
};
_DS_Prefs._add=function(a,b,c){
	var d=_DS_Prefs.pa+a;
	if(typeof _DS_Prefs.h[d]!="object"){
		_DS_Prefs.h[d]={}
	}
	_DS_Prefs.h[d][b]=c
};
_DS_Prefs._arrayToStr=function(a){
	var b=a.length&&a.join;
	if(b){
		var c=[];
		for(var d=0;d<a.length;d++){
			c.push(a[d].replace(/\|/g,"%7C"))
		}
		return c.join("|")
	}else{
		return new String(a)
	}
};
_DS_Prefs._strToArray=function(a){
	var b=a.length>0?a.split("|"):[];
	for(var c=0;c<b.length;c++){
		b[c]=b[c].replace(/%7C/g,"|")
	}
	return b
};
_DS_Prefs.h={};
_DS_Prefs.$=true;

function _PrefListApp(a,b,c,d,e)
{
	var f=typeof c=="string"?[]:c,g=new _ListApp(f,d,e);
	for(var h in g)
	{
		this[h]=g[h]
	}
	if(typeof c=="string")
	{
		this.Ga(c)
	}
	this.prefidx=a;
	this.prefname=b;
	this.app_name="m_"+e+"_"+a+"_App";
	this.display_area=_gel("m_"+e+"_"+a+"_disp");
	this.value_input_field=_gel("m_"+e+"_"+a+"_val");
	this.name_input_field=this.value_input_field;
	this.default_name="";
	this.default_value="";
	this.Y=_PrefListApp_get_tail
}
function _PrefListApp_get_tail(a,b)
{
	var c="</table>";
	_gel("m_"+this.module_id+"_"+this.prefidx).value=this.Ka();
	return c
}
_PrefListApp.prototype.Ga=function (a)
{
	if(a.length==0)
	{
		return 
	}var b=_DS_Prefs._strToArray(a),c=[];
	for(var d=0;d<b.length;d++)
	{
		var e=new this.item_constructor(b[d],b[d],-1);
		c[c.length]=e
	}this.items=c;
	this.prev_items=[].concat(c)
};
_PrefListApp.prototype.Ka=function ()
{
	var a=[];
	for(var b=0;b<this.items.length;b++)
	{
		a[a.length]=this.items[b]._v
	}return _DS_Prefs._arrayToStr(a)
};

function _ListItem(a,b,c)
{
	this._n=a;
	this._v=
	b;
	this._uid=c
}
_ListItem.prototype.w=function ()
{
	return this._n!=""
};
_ListItem.prototype.t=function (a)
{
	return 0
};
_ListItem.prototype.C=function ()
{
	return _hesc(this._n)
};
_ListItem.prototype.m=function (a)
{
	return "&"+_esc(this._n)+"="+_esc(this._v)
};

function _ListApp(a,b,c)
{
	this.items=a;
	this.deleted=[];
	this.item_constructor=b;
	this.module_id=c;
	this.app_name="m_"+c+"_App";	
	this.display_area=_gel("m_"+c+"_disp");
	this.value_input_field=_gel("m_"+c+"_val");
	this.name_input_field=_gel("m_"+c+"_name");
	if(!this.name_input_field)
	{
		this.name_input_field=this.value_input_field
	}
	if(this.name_input_field)
	{
		this.default_name=this.name_input_field.value;
		this.default_value=this.value_input_field.value
	}
	this.prev_items=[].concat(this.items)
}
_ListApp.prototype.reset=function ()
{
	this.items=[].concat(this.prev_items);
	this.refresh()
};
_ListApp.prototype.sort=function (a,b)
{
	return a.t(b)
};
_ListApp.prototype.Ha=function ()
{
	var a="";
	if(_old_html)
	{
		a="<table cellspacing=0 cellpadding=0 border=0>"
	}
	var b="",c=this.items;
	for(var d=0;d<c.length;d++)
	{
		if(!c[d])
		{
			this.items.splice(d,1);
			d--
		}
		else 
		{
			if(_old_html)
			{
				a+='<tr><td><a href="###" onclick="'+this.app_name+'".del("'+d+')"><img src="images/x.gif" width=16 height=13 border=0></a></td><td nowrap><font size=-1>"'+c[d].C()+'</font></td></tr>'
			}else 
			{
				a+='<a href="###" onclick="'+this.app_name+".del("+d+')" class="delbox" style="float:left;margin:1px 3px 0px 0px;"></a>'+c[d].C()+
				"<br />"
			}if(parseInt(c[d]._uid)<0)
			{
				b+=c[d].m(d)
			}
		}
	}
	var e=this.deleted,f="";
	for(var d=0;d<e.length;d++)
	{
		if(parseInt(e[d]._uid)>=0)
		{
			f+=","+e[d]._uid
		}
	}
	a+=this.Y(b,f);
	return a
};
_ListApp.prototype.Y=function (a,b)
{
	var c="<input type=hidden name=m_"+this.module_id+'_add value="'+a+'"><input type=hidden name=m_'+this.module_id+'_del value="'+b+'">';
	if(_old_html)
	{
		c="</table><input type=hidden name=m_"+this.module_id+'_add value="'+a+'"><input type=hidden name=m_'+this.module_id+'_del value="'+b+'">'
	}return c
};
_ListApp.prototype.refresh=function ()
{
	this.items.sort(this.sort);
	this.display_area.innerHTML="<font size=-1>"+this.Ha()+"</font>"
};
_ListApp.prototype.add=function (a,b)
{
	if(!a)
	{
		a=_trim(this.name_input_field.value)
	}
	if(!b)
	{
		b=_trim(this.value_input_field.value)
	}
	var c=new this.item_constructor(a,b,-1);
	if(!c.w())return ;
	this.items[this.items.length]=c;
	this.refresh();
	this.name_input_field.value=this.default_name;
	this.value_input_field.value=this.default_value
};
_ListApp.prototype.del=function (a)
{
	this.deleted[this.deleted.length]=
	this.items[a];
	this.items.splice(a,1);
	this.refresh()
};

var _DS_time=
{
	times:
	{
		epoch:(new Date).getTime()
	},set_epoch:function (a)
	{
		_DS_time.times["epoch"]=a
	},epoch:function ()
	{
		return _DS_time.times["epoch"]
	},start:function (a)
	{
		_DS_time.times[a]=(new Date).getTime()
	},stop:function (a)
	{
		var b=_DS_time.times[a]?_DS_time.times[a]:_DS_time.epoch();
		_DS_time.times[a]=(new Date).getTime()-b
	},get:function (a)
	{
		return _DS_time.times[a]
	},print:function (a)
	{
		document.write("<div style='color:#999999;font-size:75%'>"+a+" : "+_DS_time.get(a)+"ms</div>")
	},stop_and_print:function (a)
	{
		_DS_time.stop(a);		
		_DS_time.print(a)
	}
};
if(window._DS_time_epoch)
{
	_DS_time.set_epoch(window._DS_time_epoch)
};

function ds_Wa(a){
	var b="",c=this.X(a,b);
	return c!=null?c+"":b
}
function ds_Pa(a){
	var b=0,c=parseInt(this.X(a,b));
	return isNaN(c)?b:c
}
function ds_Na(a){
	return this.getInt(a)?true:false
}
function ds_Ma(a){
	return _DS_Prefs._strToArray(this.X(a,""))
}
function ds_0a(a,b){
	this.set(a,_DS_Prefs._arrayToStr(b))
}
function ds_Oa(){
	return this.getString(".country")
}
function ds_Qa(){
	return this.getString(".lang")
}
function ds_Ta(a){
	return this.v(_DS_Prefs.N+a,"")
}
function ds_Ua(a,b){
	var c=this.v(_DS_Prefs.N+a,""),d=c.match(this.db);
	if(!d||!d[0]){
		return c
	}
	if(!b){
		var e=d[4]||"";
		return d[1]+e+d[5]
	}
	return d[1]+b+d[5]
}
function ds_Va(){
	var a=["__module_id__="+this.Fa];
	for(var b in this.h){
		a.push(encodeURIComponent(b)+"="+encodeURIComponent(this.h[b]))
	}
	return a.join("&")
}
function ds_La(){
	document.write("<pre>");
	for(var a in this.h){
		document.writeln(a+" = "+this.v(a,null))
	}
	document.write("</pre>")
}
function ds_Ra(){
	return parseInt(this.v(_DS_Prefs.Ma,"0"))
}
function ds_Sa(){
	return parseInt(this.v(_DS_Prefs.Na,"0"))
}
function ds_Ya(a,b){
	if(typeof b=="undefined"){
		b=null
	}
	var c=this.h[a];
	return typeof c!="undefined"?c:b
}
function ds_Xa(a,b){
	return this.v(_DS_Prefs.D+a,b)
}
var ds_Za=/(.*)(\<ph.*?\>\s*(\<ex\>(.*?)\<\/ex\>)?\s*%1\s*\<\/ph\>)(.*)/;
/*  pref end  */







function _DS_RegisterOnloadHandler(a)
{
	_DS_AddEventHandler("domload",a)
}


function ds_Ga(a,b)
{
	if(a in ds_a.b)
	{
		for(var c=0;c<ds_a.b[a].length;c++)
		{
			if(ds_a.b[a][c])
			{
				var d=[];
				for(var e=1;e<arguments.length;e++)
				{
					d[d.length]=arguments[e]
				}
				ds_a.b[a][c].apply(this,d)
			}
		}
	}
}
function ds_la(a,b)
{
	if(!(a in ds_a.jb))
	{
		throwError("Unsupported event type: "+a);
	}
	var c=ds_a.Q(a);
	if(!(c in ds_a.b))
	{
		ds_a.b[c]=[]
	}
	ds_a.b[c][ds_a.b[c].length]=b
}
function ds_ma(a,b,c)
{
	if(!(b in ds_a.kb))
	{
		throwError("Unsupported module event type: "+b);
		
	}
	var d=ds_a.R(a,b);
	if(!(d in ds_a.b))
	{
		ds_a.b[d]=[]
	}
	ds_a.b[d][ds_a.b[d].length]=c
}
function ds_ka(a,b,c)
{
	var d=ds_a.xa(a,b);
	if(!(d in ds_a.b))
	{
		ds_a.b[d]=[];
		var e=function (g)
		{
			if(!g)g=window.event;
			ds_a.L.apply(a,[d,g])
		};
		if(a.addEventListener)
		{
			a.addEventListener(b,e,false)
		}else if(a.attachEvent)
		{
			a.attachEvent("on"+b,e)
		}else 
		{
			throwError("Object {"+a+"} does not support DOM events.");
			
		}ds_a.F[d]=[a,b,e]
	}
	var f=ds_a.b[d].length;
	if(a===window&&b=="unload"&&f>0)
	{
		ds_a.b[d][f]=ds_a.b[d][f-1];
		ds_a.b[d][f-1]=c
	}
	else 
	{
		ds_a.b[d][f]=c
	}
}
function ds_ja(a,b)
{
	var c=ds_a.P(a);
	if(!(c in ds_a.b))
	{
		ds_a.b[c]=[]
	}
	ds_a.b[c][ds_a.b[c].length]=b
}
function ds_wa(a,b)
{
	var c=ds_a.Q(a);
	return ds_a.H(c,b)
}
function ds_ya(a,b,c)
{
	var d=ds_a.R(a,b);
	return ds_a.H(d,c)
}
function ds_va(a,b,c)
{
	var d=ds_a.xa(a,b);
	return ds_a.H(d,c)
}
function ds_ua(a,b)
{
	var c=ds_a.P(a);
	return ds_a.H(c,b)
}
function ds_Fa(a,b)
{
	var c=ds_a.x([ds_a.Q(a)],arguments,1);
	ds_a.L.apply(window,c)
}
function ds_Da(a,b)
{
	var c=ds_a.x([],arguments,0);
	setTimeout(function () {
		ds_a.triggerEvent.apply(window,c)
	},0)
}
function ds_Ha(a,b,c)
{
	var d=ds_a.x([ds_a.R(a,b)],arguments,2);
	ds_a.L.apply(window,d)
}
function ds_Ea(a,b,c)
{
	var d=ds_a.x([],arguments,0);
	setTimeout(function () {
		ds_a.triggerModuleEvent.apply(window,d)
	},0)
}
function ds_Ba(a,b)
{
	var c=ds_a.x([ds_a.P(a)],arguments,1);
	ds_a.L.apply(window,c)
}
function ds_Ca(a,b)
{
	var c=ds_a.x([],arguments,0);
	setTimeout(function () {
		ds_a.triggerCustomEvent.apply(window,c)
	},0)
}
var ds_za={domload:1,xsetpdone:1,moduledragstart:1,moduledragend:1,moduletitleclick:1,moduleedit:1,modulecanceledit:1,moduledelete:1,moduleundelete:1,modulezip:1,moduleunzip:1,load:1,unload:1,resize:1},ds_Aa={dragstart:1,dragend:1,titleclick:1,edit:1,canceledit:1,"delete":1,undelete:1,zip:1,unzip:1};
function ds_sa(a)
{
	var b="ds_event_hashcode_";
	if(a.hasOwnProperty &&a.hasOwnProperty (b))
	{
		return a[b]
	}
	if(!a[b])
	{
		a[b]=++ds_a.bb
	}
	return a[b]
}
function ds_pa(a)
{
	return "builtin_"+a
}
function ds_qa(a,b)
{
	if(a.indexOf&&
	a.indexOf("m_")==0)
	{
		a=a.substring(2)
	}
	return "builtin_m"+a+"_"+b
}
function ds_oa(a,b)
{
	return "builtin_"+ds_a.Ya(a)+"_"+b
}
function ds_na(a)
{
	return "custom_"+a
}
function ds_ra()
{
	for(var a in ds_a.b)
	{
		for(var b=0;b<ds_a.b[a].length;b++)
		{
			ds_a.b[a][b]=null
		}if(a in ds_a.F)
		{
			var c=ds_a.F[a],d=c[0],e=c[1],f=c[2];
			if(d.removeEventListener)
			{
				d.removeEventListener(e,f,false)
			}else if(d.detachEvent)
			{
				d.detachEvent("on"+e,f)
			}ds_a.F[a]=null
		}
	}
}
function ds_ta(a,b,c)
{
	for(var d=c;d<b.length;d++)
	{
		a[a.length]=b[d]
	}
	return a
}
function ds_xa(a,b)
{
	if(a in ds_a.b)
	{
		for(var c=0;c<ds_a.b[a].length;c++)
		{
			if(ds_a.b[a][c]===b)
			{
				ds_a.b[a][c]=null;
				return true
			}
		}
	}
	return false
}
var ds_a=
{
	bb:0,
	b:{},
	F:{},
	jb:ds_za,
	kb:ds_Aa,
	Ya:ds_sa,
	Q:ds_pa,
	R:ds_qa,
	xa:ds_oa,
	P:ds_na,
	Ua:ds_ra,
	x:ds_ta,
	H:ds_xa,
	L:ds_Ga,
	addEventHandler:ds_la,
	addModuleEventHandler:ds_ma,
	addDOMEventHandler:ds_ka,
	addCustomEventHandler:ds_ja,
	removeEventHandler:ds_wa,
	removeModuleEventHandler:ds_ya,
	removeDOMEventHandler:ds_va,
	removeCustomEventHandler:ds_ua,
	triggerEvent:ds_Fa,
	triggerModuleEvent:ds_Ha,
	triggerCustomEvent:ds_Ba,
	triggerDelayedEvent:ds_Da,
	triggerDelayedModuleEvent:ds_Ea,
	triggerDelayedCustomEvent:ds_Ca
},
_DS_AddEventHandler=ds_a.addEventHandler,
_DS_AddModuleEventHandler=ds_a.addModuleEventHandler,
_DS_AddDOMEventHandler=ds_a.addDOMEventHandler,
_DS_AddCustomEventHandler=ds_a.addCustomEventHandler,
_DS_RemoveEventHandler=ds_a.removeEventHandler,
_DS_RemoveModuleEventHandler=ds_a.removeModuleEventHandler,
_DS_RemoveDOMEventHandler=ds_a.removeDOMEventHandler,
_DS_RemoveCustomEventHandler=ds_a.removeCustomEventHandler,
_DS_TriggerEvent=ds_a.triggerEvent,
_DS_TriggerModuleEvent=ds_a.triggerModuleEvent,
_DS_TriggerCustomEvent=ds_a.triggerCustomEvent,
_DS_TriggerDelayedEvent=ds_a.triggerDelayedEvent,
_DS_TriggerDelayedModuleEvent=ds_a.triggerDelayedModuleEvent,
_DS_TriggerDelayedCustomEvent=ds_a.triggerDelayedCustomEvent;







function ds_y(a,b){
	var c=forumpath+"feedproxy.aspx?"+a;
	_sendx(c,function(d){var e={};try{e=eval("("+d+")")}catch(f){e={}}for(var g in b){var h=e?e:null;b[g](h)}e=null;b=null},false,null)
}
var ds_E=false,ds_cb=0,ds_g="",ds_q={};
function ds_C(a){var b={};for(var c in a){b[c]=a[c]}return b}
function _DS_FetchContent(a,b,c)
{
	var d;
	d=c!=null&&typeof c=="object"?ds_C(c):{};
	d.url=a;
	d.callback=b;
	d.asXml=false;
	ds_x(d)
}

function _DS_FetchXmlContent(a,b,c)
{
	var d;
	d=c!=null&&typeof c=="object"?ds_C(c):{};
	d.url=a;
	d.callback=b;
	d.asXml=true;
	ds_x(d)
}

function _DS_FetchFeedAsJSON(a,b,c,d)
{
	var e="fr_"+ds_cb++,f="url="+_esc(a);
	if(c)
	{
		f+="&val="+_esc(c)
	}
	if(d)
	{
		f+="&sum=1"
	}
	var g=f;//e+"="+_esc(f);
		var h={};
		h[e]=b;
		ds_y(g,h)
/*	if(ds_E)
	{
		var h={};
		h[e]=b;
		ds_y(g,h)
	}
	else 
	{
		if(ds_g!="")ds_g+="&";
		ds_g+=g;
		ds_q[e]=b
	}
	*/
}
/*
function ds_3a(){
	ds_E=true;
	if(ds_g!=""){
		ds_y(ds_g,ds_q)
	}
	ds_g="";
	ds_q=null
}
_DS_AddEventHandler("domload",ds_3a);
*/
function imgzoom(o)
{
	if(event.ctrlKey)
	{
		var zoom = parseInt(o.style.zoom, 10) || 100;
		zoom -= event.wheelDelta / 12;
		if(zoom > 0)
		{
			o.style.zoom = zoom + '%';
		}
		return false;
	}
	else
	{
		return true;
	}
}
function _DS_Callback(a,b,c,d,e,f)
{
	var g=arguments;
	return function ()
	{
		var h=[];
		for(var i=0;i<arguments.length;i++)
		{
			h[h.length]=arguments[i]
		}for(var i=1;i<g.length;i++)
		{
			h[h.length]=g[i]
		}a.apply(null,h)
	}
};

function ds_x(a)
{
	if(!a.post_data&&!a.headers&&window._pl_data&&_pl_data[a.url])
	{
		if(a.asXml)
		{
			var b;
			if(window.ActiveXObject)
			{
				b=new ActiveXObject("Microsoft.XMLDOM");
				b.async=false;
				b.loadXML(_pl_data[a.url])
			}else {
				var c=new DOMParser;
				b=c.parseFromString(_pl_data[a.url],"text/xml")
			}
			a.callback(b)
		}else {
			a.callback(_pl_data[a.url])
		}
	}
	if(!isNaN(a.refreshInterval)&&a.refreshInterval>=0)
	{
		a.url+=(a.url.lastIndexOf("?")>-1?"&":"?")+".cache=";
		if(a.refreshInterval==0){
			a.url+=_esc(Math.random())
		}else {
			var d=new Date,
			e=d.getTimezoneOffset()/60,f=Math.floor(d.getTime()/1000);
			f=Math.floor(f/a.refreshInterval).toString ();
			a.url+=_esc(e+f)
		}
	}
	var g=forumpath+"proxy.aspx?";
	if(_et!="")
	{
		g+="et="+_et+"&"
	}
	if(a.encoding)
	{
		g+="enc="+_esc(a.encoding)+"&"
	}
	if(a.headers)
	{
		var h=[];
		for(var i=0;i<a.headers.length;i++)
		{
			h.push(_esc(a.headers[i][0]+":"+a.headers[i][1]))
		}g+="hdrs="+_esc(h.join(","))+"&"
	}g+="url="+_esc(a.url);
	_sendx(g,a.callback,a.asXml,a.post_data)
}


var ds_B=(
	function(){
	var a={"\u0008":"\\b","\t":"\\t","\n":"\\n","\u000c":"\\f","\r":"\\r",'"':'\\"',"\\":"\\\\"},
		b={
			"boolean":function(c){return String(c)},
			number:function(c){return isFinite(c)?String(c):"null"},
			string:function(c){if(/["\\\x00-\x1f]/.test(c)){c=c.replace(/([\x00-\x1f\\"])/g,function(d,e){var f=a[e];if(f){return f}f=e.charCodeAt();return"\\u00"+Math.floor(f/16).toString(16)+(f%16).toString(16)})}return'"'+c+'"'},object:function(c){if(c){var d=[],e,f,g,h,i;if(c instanceof Array){d[0]="[";h=c.length;for(g=0;g<h;g+=1){i=c[g];f=b[typeof i];if(f){i=f(i);if(typeof i=="string"){if(e){d[d.length]=","}d[d.length]=i;e=true}}}d[d.length]="]"}else if(typeof c.hasOwnProperty==="function"){d[0]="{";for(g in c){if(c.hasOwnProperty(g)){i=c[g];f=b[typeof i];if(f){i=f(i);if(typeof i=="string"){if(e){d[d.length]=","}d.push(b.string(g),":",i);e=true}}}}d[d.length]="}"}else{return}return d.join("")}return"null"}};return{copyright:"(c)2005 JSON.org",license:"http://www.JSON.org/license.html",
			stringify:function(c){var d=b[typeof c];if(d){c=d(c);if(typeof c=="string"){return c}}return null},
			parse:function(c){try{return!/[^,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t]/.test(c.replace(/"(\\.|[^"\\])*"/g,""))&&eval("("+c+")")}catch(d){return false}}
		}
	}
)();

function ds_ga(a){var b=document.createElement("DIV");b.innerHTML="<iframe onload='this.pool_locked=false'></iframe>";var c=b.getElementsByTagName("IFRAME")[0];c.style.visibility="hidden";c.style.width=(c.style.height="0px");c.style.border="0px";c.style.position="absolute";c.pool_locked=a;this.f[this.f.length]=c;b.removeChild(c);b=null;return c}
function ds_ha(a){var b=this;window.setTimeout(function(){var c=null;for(var d=b.f.length-1;d>=0;d--){var e=b.f[d];if(e&&!e.pool_locked){e.parentNode.removeChild(e);if(window.ActiveXObject){e=null;b.f[d]=null;b.f.splice(d,1)}else{e.pool_locked=true;c=e;break}}}c=c?c:b.Ta(true);c.src=a;document.body.appendChild(c)},0)}
function ds_fa(){for(var a=0;a<this.f.length;a++){this.f[a].onload=null;this.f[a]=null}this.f.length=0;this.f=[]}
function ds_ea(){this.f=[];this.Ta=ds_ga;this.iframe=ds_ha;this.clear=ds_fa}
function ds_ca(a,b){_IFPC.J[a]=b}
function ds_2(a,b,c,d,e,f){c.unshift(_IFPC.fb(e));c.unshift(f);c.unshift(b);c.unshift(a);var g=4095-d.length;g=parseInt(g/3,10);
var h=_IFPC.ya(c),i=parseInt(h.length/g,10);if(h.length%g>0){i+=1}for(var j=0;j<i;j++){var k=h.substr(j*g,g),l=[a,_IFPC.ua,i,j,k];_IFPC.Ba.iframe(d+"#"+_IFPC.ya(l))}_IFPC.ua++}
function ds_4(){_IFPC.J={};_IFPC.r={};_IFPC.Ba.clear()}
function ds_z(a){if(window.parent===window.top){var b=_IFPC.S(a),c=b.shift(),d=window.parent.frames[c];d.setTimeout(function(){try{d._IFPC.handleRequest(a)}catch(e){throw new Error("Unable to relay request to iframe: "+c+", reason: "+e);}},0)}else{var d=window.top;d.setTimeout(function(){d._IFPC.handleRequest(a)},0)}}
function ds_$(a){var a=_IFPC.S(a),b=a.shift(),c=a.shift(),d=a.shift(),e=a.shift(),f=a.shift(),g=b+"_"+c;if(!_IFPC.o[g])_IFPC.o[g]=[];_IFPC.o[g].push([e,f]);if(_IFPC.o[g].length==d){_IFPC.o[g].sort(function(o,p){return parseInt(o[0],10)-parseInt(p[0],10)});f="";for(var h=0;h<d;h++){f+=_IFPC.o[g][h][1]}_IFPC.o[g]=null;var i=_IFPC.S(f),b=i.shift(),j=i.shift(),k=i.shift(),l=i.shift(),m=_IFPC.$a(j);if(m){var n=m.apply(null,i);if(_IFPC.cb(l)){n.unshift(l);_IFPC.call(b,_IFPC.na,n,k,null,"")}}else{throw new Error("Service "+j+" not registered.");}}}
function ds_9(a){if(_IFPC.J.hasOwnProperty(a)){return _IFPC.J[a]}else{return null}}
function ds_ba(a){var b="";if(a&&typeof a=="function"){b=_IFPC.Za();_IFPC.r[b]=a}return b}
function ds_da(a){if(_IFPC.r.hasOwnProperty(a)){_IFPC.r[a]=null}}
function ds_7(a){if(a&&_IFPC.r.hasOwnProperty(a)){return _IFPC.r[a]}return null}
function ds_8(){return _IFPC.ma+_IFPC.Ra++}
function ds_aa(a){return(a+"").indexOf(_IFPC.ma)==0}
function ds_5(a){var b=a.split("&");for(var c=0;c<b.length;c++){var d=decodeURIComponent(b[c]);try{d=ds_B.parse(d)}catch(e){}b[c]=d}return b}
function ds_6(a){var b=[];for(var c=0;c<a.length;c++){var d=ds_B.stringify(a[c]);b.push(encodeURIComponent(d))}return b.join("&")}
function ds_3(a){var b=_IFPC.Xa(a);if(b){var c=[];for(var d=1;d<arguments.length;d++){c[c.length]=arguments[d]}b.apply(null,c);_IFPC.mb(a)}else{throw new Error("Invalid callbackId");}}var _IFPC={registerService:ds_ca,call:ds_2,clear:ds_4,relayRequest:ds_z,processRequest:ds_z,handleRequest:ds_$,ma:"cbid",na:"ifpc_callback",Ba:new ds_ea,o:{},J:{},r:{},Ra:0,ua:0,$a:ds_9,fb:ds_ba,mb:ds_da,Xa:ds_7,Za:ds_8,cb:ds_aa,S:ds_5,ya:ds_6,Sa:ds_3};_IFPC.registerService(_IFPC.na,_IFPC.Sa);