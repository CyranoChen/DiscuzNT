<%@ Page language="c#" Inherits="Discuz.Mall.Admin.mall_goodscategoriesmanage"%>
<%@ Register TagPrefix="uc1" TagName="PageInfo" Src="../UserControls/PageInfo.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
    <head id="Head1">
		<title>商品分类管理</title>
		<script type="text/javascript" src="../js/common.js"></script>
		<link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
        <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
        <script type="text/javascript" src="../js/modalpopup.js"></script>
	    <style type="text/css">
	    body {
		    overflow-x:auto;
	    }
	    .contral{
		    font-size:12px;
		    cursor:default;
		    margin:0 auto;
	    }
	    .contral input{
		    font-size:12px;
		    cursor:hand;
		    valign:middle;
	    }

	    .treenode_move {
		    position:absolute;
		    width:200px;
		    line-height:18px;
		    filter:alpha(opacity=50);
		    opacity:0.5;
		    z-index:110;
		    display:none;
		    padding-left:20px;
		    background:url('../images/folder.gif') no-repeat 0px 0px;
	    }

	    .treenode_0_noselected{
		    height:4px;
		    line-height:4px;
		    overflow:hidden;
		    z-index:100;
		    border: 0px #ffffff solid;
	    }

	    .treenode_0_over{
		    background-color: #c0ddfc;
		    height:6px;
		    line-height:6px;
		    overflow:hidden;
		    z-index:100;
		    border: 0px #933456 solid;
    		
	    }
    	
    	
	    .treenode_selected{
		    line-height:15px;
		    z-index:100;
		    background-color: #c0ddfc;
		    color:#ffffff;	
		    border: 0px #0A246A solid;
    		
		    }
    	
	    .treenode_noselected{
		    line-height:15px;
		    z-index:100;
		    border: 0px #ffffff solid;
    		
	    }
    	
	    .treenode_over{
		    line-height:15px;
		    background-color: #c0ddfc;
		    color:#ffffff;
		    cursor:poiter;		
		    z-index:100;
		    border: 0px #4963A9 solid;
    		
	    }
    	
        </style>
        <script type="text/javascript">	
	    var treeID = null;
	    function tree(treeID,treeNodes,funName){
		var fid = 0;
		var treeType = 0;
		var layer = 0;
		var parentidlist = "";
		var title = "";
		var lastDiv = null;

		this.id = treeID;
		this.drag = false;
		this.dragObj = null;
		this.oldClass = "";
		this.fid = 0;
		this.treeType = 0;
		this.layer = 0;
		this.parentidlist = "";
		this.title = ""
		this.lastDiv = null
		
		this.getPos=function(el,sProp)
		{
			var iPos = 0;
			while (el!=null)
			{
				iPos+=el["offset" + sProp];
				el = el.offsetParent;
			}
			return iPos;
		}
		
		this.getETarget = function(e)
		{
			if (!e){return null;}
			if (!e.srcElement && !e.target){return null;}
			var obj = e.srcElement ? e.srcElement : e.target;
			if (obj == null){return null};
			while (obj.getAttribute("treetype") == null && obj.tagName != "BODY" && obj.tagName != "HTML")
			{
				obj = obj.parentNode;
				if(obj == null){break;}
			}
			return obj;
		}		
		
		//鼠标按下
		this.onmousedown = function(e){
		
			if (!e){return false;}
			
			if (this.dragObj){
				this.dragObj.className = "noselected";
			}
			
			this.dragObj = this.getETarget(e);
			if (!this.dragObj){return;}
			
			if (!this.dragObj.getAttribute("treetype")){return;}
			
			var mX = e.x ? e.x : e.pageX;
			var mY = e.y ? e.y : e.pageY;
			
			this.drag = true;
			this.dragObj.className = "treenode_selected";
			this.oldClass = "treenode_selected";
			
			var textContent = this.dragObj.textContent ? this.dragObj.textContent : this.dragObj.innerText; 
			textContent = textContent.replace("添加","");
			textContent = textContent.replace("编辑","");
			textContent = textContent.replace("删除","");
			textContent = textContent.replace("移动","");

			this.ShowMove(mX,mY,textContent );
			treeID = this;
			document.onmousedown = function(){return false};
			document.onmousemove = function(e)
			{
				if (tree==null)
				{
					return;
				}
				treeID.document_onmousemove(e);
			}
		    alignElWithMouse(mX,mY);
		};
		
		this.onmouseup = function(e){this.document_onmouseup(e)};
		this.onmouseover = function(e)
		{
			if (this.drag)
			{
				this.oldClass = this.getETarget(e).className;
				if (this.getETarget(e).getAttribute("treeType") == 1)
				{
					this.getETarget(e).className = "treenode_over";
				}
				else
				{
					this.getETarget(e).className = "treenode_0_over";				
				}
			}
			var mX = e.x ? e.x : e.pageX;
			var mY = e.y ? e.y : e.pageY;
			alignElWithMouse(mX,mY);
			
		};
		this.onmouseout = function(e)
		{
			if (this.drag)
			{
				this.getETarget(e).className = this.oldClass;
				if (this.dragObj.id == this.getETarget(e).id)
				{
					this.findObj(this.id + "_treenode_move").style.display = "block";
				}
			}
			
		};
		this.init = function()
		{
			document.write("\n<div id=\"" + this.id + "_control\" class=\"contral\">");
			document.write("\n<div id = \"" + this.id + "_treenode_move\" class=\"treenode_move\"></div>")
			document.write("\n<div id = \"" + this.id + "_line\"></div>")
			document.write("\n<div class=\"treenode_0\" style=\"display:none;\" id = \"" + this.id + "_treenode_0\" onmouseup=\"" + this.id + ".onmouseup(event);\" onmouseover=\"" + this.id + ".onmouseover(event)\" onmouseout=\"" + this.id + ".onmouseout(event)\"></div>")
			document.write("\n<div class=\"treenode_1\" style=\"display:none;\" id = \"" + this.id + "_treenode\" onmousedown=\"" + this.id + ".onmousedown(event);\" onmouseup=\"" + this.id + ".onmouseup(event);\" onmouseover=\"" + this.id + ".onmouseover(event)\" onmouseout=\"" + this.id + ".onmouseout(event)\"></div>");
			for(i=0;i<treeNodes.length;i++){
				var nodeStr = "";
				for(treeNode in treeNodes[i]){
					nodeStr += treeNode + "=\"" + escape(treeNodes[i][treeNode]) + "\" ";
				}
				document.write("\n<div class=\"treenode_0_noselected\" id = \"" + this.id + "_treenode" + i + "_0\" index=\"" + i + "\" treetype=\"0\" ");
				document.write(nodeStr);
				document.write("onmouseup=\"" + this.id + ".onmouseup(event);\" onmouseover=\"" + this.id + ".onmouseover(event)\" onmouseout=\"" + this.id + ".onmouseout(event)\">");
				if (i>0){
					document.write(treeNodes[i].linetitle);
				}
				document.write("</div>");
				document.write("\n<div class=\"treenode_noselected\" id = \"" + this.id + "_treenode" + i + "\" index=\"" + i + "\" treetype=\"1\" ");
				document.write(nodeStr);
				if(treeNodes[i].layer =='0')
				{
				     str = "onmousedown=\"" + this.id + ".onmousedown(event);\" onmouseup=\"" + this.id + ".onmouseup(event);\" onmouseover=\"" + this.id;
				     str += ".onmouseover(event)\" onmouseout=\"" + this.id + ".onmouseout(event)\">" + treeNodes[i].subject;
				     str += " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src=../images/cal_nextMonth.gif> &nbsp;<a class=\"TopicButton\"";
				     str += " type=\"button\" onclick=\"javascript:addNode('" + treeNodes[i].fid + "'," + (treeNodes[i].layer == 0 ? "true" : "false") + "," + treeNodes[i].addfid + ",event);\">";
				     str += "<img src=\"../images/add.gif\"/>添加</a><a class=\"TopicButton\" type=\"button\"";
				     str += " onclick=\"javascript:editNode('" + treeNodes[i].fid + "','" + treeNodes[i].name + "',";
				     str += ((treeNodes[i].layer == 0 || treeNodes[i].layer == 1) ? "true" : "false") + "," + treeNodes[i].editfid + "," + treeNodes[i].cfid + ",event);\"><img src=\"../images/submit.gif\"/>编辑</a>";
				     if(treeNodes[i].subforumcount == '0')
				     {
				        str += "<a class=\"TopicButton\" type=\"button\" onclick=\"javascript:if(confirm('您要删除该项吗?'))";
				        str += "{success.style.display = 'block';HideOverSels('success');window.location.href='mall_goodscategoriesmanage.aspx?method=del&id=";
				        str += treeNodes[i].fid + "';}\"><img src=\"../images/del.gif\" />删除</a>";
                     }
                     str += "<a class=\"TopicButton\" onclick=\"javascript:success.style.display = 'block';HideOverSels('success');window.location='mall_goodscategoriesmanage.aspx?method=update&id=";
                     str += treeNodes[i].fid + "';\" type=\"button\"><img src=\"../images/cache_resetall.gif\"/> 更新分类商品数 </a>"
				     str += "</div>";
				     document.write(str);
				}
				else
				{
				     str = "onmousedown=\"" + this.id + ".onmousedown(event);\" onmouseup=\"" + this.id + ".onmouseup(event);\" onmouseover=\"" + this.id;
				     str += ".onmouseover(event)\" onmouseout=\"" + this.id + ".onmouseout(event)\">" + treeNodes[i].subject;
				     str += " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src=../images/cal_nextMonth.gif>&nbsp;<a class=\"TopicButton\"";
				     str += "type=\"button\" onclick=\"javascript:addNode('" + treeNodes[i].fid + "'," + (treeNodes[i].layer == 0 ? "true" : "false") + "," + treeNodes[i].addfid + ",event);\">";
				     str += "<img src=\"../images/add.gif\"/>添加</a><a class=\"TopicButton\" type=\"button\"";
				     str += " onclick=\"javascript:editNode('" + treeNodes[i].fid + "','" + treeNodes[i].name + "',";
				     str += ((treeNodes[i].layer == 0 || treeNodes[i].layer == 1) ? "true" : "false") + "," + treeNodes[i].editfid + "," + treeNodes[i].cfid + ",event);\"><img src=\"../images/submit.gif\"/>编辑</a>";
				     if(treeNodes[i].subforumcount == '0')
				     {
				        str += "<a class=\"TopicButton\" type=\"button\" onclick=\"javascript:if(confirm('您要删除该项吗?'))";
				        str += "{success.style.display = 'block';HideOverSels('success');window.location.href='mall_goodscategoriesmanage.aspx?method=del&id=";
				        str += treeNodes[i].fid + "';}\"><img src=\"../images/del.gif\" />删除</a>";
				     }
                     str += "<a class=\"TopicButton\" onclick=\"javascript:success.style.display = 'block';HideOverSels('success');window.location='mall_goodscategoriesmanage.aspx?method=update&id=";
                     str += treeNodes[i].fid + "';\" type=\"button\"><img src=\"../images/cache_resetall.gif\"/> 更新分类商品数 </a>"
				     str += "</div>";
				     document.write(str);
				}
			}			
		}
		
		document.write("\n</div>");
			
		
		this.findObj = function(objname)
		{
			if (document.getElementById(objname))
			{
				return document.getElementById(objname);
			}
			else if(document.getElementsByName(objname))
			{
				return document.getElementsByName(objname)
			}
			else
			{
				return null;
			}
		}
		
		this.HideMove = function()
		{
			if (this.findObj(this.id + "_treenode_move").style.display != "none")
			{
				this.findObj(this.id + "_treenode_move").style.display = "none";
			}
		}
		
		this.ShowMove = function(mX ,mY,innerHTML)
		{
			this.findObj(this.id + "_treenode_move").innerHTML = innerHTML;
			//this.findObj(this.id + "_treenode_move").style.left = (mX + 10) + "px";
			this.findObj(this.id + "_treenode_move").style.left = (mX) + "px";
			this.findObj(this.id + "_treenode_move").style.top = (mY) + "px";
		}
		
		this.SetMove = function(e,mX,mY)
		{
			//this.findObj(this.id + "_treenode_move").style.left = (mX + 10) + "px";
			this.findObj(this.id + "_treenode_move").style.left = (mX) + "px";
			this.findObj(this.id + "_treenode_move").style.top = (mY) + "px";			
		}
		
		
		//document事件
		this.document_onmouseup = function(e)
		{
			if (this.drag)
			{
				this.drag = false;
				try
				{
					this.getETarget(e).className = this.oldClass;
					
					index = this.getETarget(e).getAttribute("index");
					eval(funName + "(treeNodes[index],treeNodes[this.dragObj.getAttribute(\"index\")],this.getETarget(e).getAttribute(\"treetype\"))");
				}
				catch(e){}
				
				this.HideMove();
				treeID = null;
				
				document.onmousedown = function(){ return true; };
				document.onmousemove = function(){ null; };
			}

		}
		document.onmouseup = new Function(this.id + ".document_onmouseup();");
		
		this.document_onmousemove = function(e)
		{
			e= e ? e : window.event;
			var mX = e.x ? e.x : e.pageX;
			var mY = e.y ? e.y : e.pageY;
			this.SetMove(e,mX,mY);
			
		}
		
		this.document_onselectstart = function()
		{
			return !this.drag;
		}
		document.onselectstart = new Function("return " + this.id + ".document_onselectstart ();");
		
		this.init();		
	}	
        </script>
        <script type="text/javascript">
	    function reSetTree(objN,objO,treetype)
	    {
		    if(objN.fid != objO.fid)
		    { 
		       var message = '您是否要将分类\"' + objO.name + '\"移动到分类\"' + objN.name + '\"';
		       if(treetype == 1)
		       {
		           message += '子分类下吗 ?';
		       }
		       else
		       {
			       message += '之前吗 ?';
		       }
    		  
		       if(confirm(message))
		       {
		          var objnparentidlist = ',' + objN.parentidlist + ',';
		          if(objnparentidlist.indexOf(',' + objO.fid + ',') < 0)
		          {
		             success.style.display = 'block';
		             HideOverSels('success');
//		             alert('mall_goodscategoriesmanage.aspx?currentfid=' + objO.fid + '&targetfid=' + objN.fid + '&isaschildnode=' + treetype);
//		             return;
		             window.location.href='mall_goodscategoriesmanage.aspx?currentfid=' + objO.fid + '&targetfid=' + objN.fid + '&isaschildnode=' + treetype;
	              }
	              else
	              {
	                 alert('不能将当前分类作为其子分类的版块');
	              }
	           }
	        }
	    }

        function netscapeMouseMove(e)
        {
            if(e.screenY>(document.body.offsetHeight-10))
            {
                window.scrollTo(e.screenX, e.screenY+1000);
            }
            if(e.screenY<10)
            {
                window.scrollTo(e.screenX, e.screenY-1000);
            }         
        }

        function microSoftMouseMove()
        {
             if(window.event.y>(document.body.offsetHeight-10))
             {
                 window.scrollTo(window.event.x, window.event.y+1000);
             }
             if(window.event.y<10)
             {
                 window.scrollTo(window.event.x, window.event.y-1000);
             }
        }

        var countdown=1;
        var countup=1;
        function alignElWithMouse(x,y)
        {
        }
        
        function saveNode()
        {
            if(document.getElementById("categoryname").value == "")
            {
                document.getElementById("categoryname").focus();
                alert("分类名称没有填写!");
                return false;
            }
            document.forms[0].submit();
        }
        
        function addNode(pid,showforumtree,fid,e)
        {
            Locate(e);
            document.getElementById("parentcategoryid").value = pid;
            document.getElementById("categoryname").value = "";
            document.getElementById("method").value = "new";
            var forums = document.getElementById("forumtreelist");
            cleanSelect();
            if(showforumtree)
            {
                document.getElementById("forumtreelistlayer").style.display = 'block';
                for(var i = 0 ; i < forumstree.length ; i++)
                {
                    if(forumstree[i]["fid"] == fid)
                    {
                        var item = new Option(forumstree[i]["name"],forumstree[i]["fid"]);
                        forums.options.add(item);
                    }
                }
                //addForumTree(fid,0,true);
                addForumTree(fid,1,0);
            }
            else
            {
                document.getElementById("forumtreelistlayer").style.display = 'none';
            }
        }
        
        function editNode(id,categoryname,showforumtree,fid,selectfid,e)
        {
            if(fid == 0)
                fid = selectfid;
            if(selectfid == 0)
                selectfid = fid;
            Locate(e);
            document.getElementById("categoryid").value = id;
            document.getElementById("categoryname").value = categoryname;
            document.getElementById("method").value = "edit";
            var forums = document.getElementById("forumtreelist");
            cleanSelect();
            if(showforumtree)
            {
                document.getElementById("forumtreelistlayer").style.display = 'block';
                for(var i = 0 ; i < forumstree.length ; i++)
                {
                    if(forumstree[i]["fid"] == fid)
                    {
                        var item = new Option(forumstree[i]["name"],forumstree[i]["fid"]);
                        forums.options.add(item);
                    }
                }
                //addForumTree(fid,0,true);
                addForumTree(fid,1,selectfid);
                for(var i = 0 ; i < forums.options.length ; i++)
                {
                    if(forums.options[i].value == selectfid)
                    {
                        forums.options[i].selected = true;
                        break;
                    }
                }
            }
            else
            {
                document.getElementById("forumtreelistlayer").style.display = 'none';
            }
        }
        
        function newRootNode(e)
        {
            Locate(e);
            document.getElementById("method").value = "newrootnode";
            document.getElementById("categoryname").value = "";
            document.getElementById("forumtreelistlayer").style.display = 'block';
            cleanSelect();
            //addForumTree(0,0,true);
            addForumTree(0,0,0);
        }
        
        function cleanSelect()
        {
            var forums = document.getElementById("forumtreelist");
            var blank = document.createTextNode('');        
            while (forums.childNodes.length > 0) 
            { 
                forums.replaceChild(blank, forums.childNodes[0]); 
                forums.removeChild(blank); 
            }
            var item = new Option("请选择","0");
            forums.options.add(item);
            return;
        }
        
        function Locate(e)
        {
            var posx = 0,posy = 0;
            if(e == null) 
                e = window.event;
            if(e.pageX || e.pageY)
            {
                posx = e.pageX; 
                posy = e.pageY;
            }
            else if(e.clientX || e.clientY)
            {
                if(document.documentElement.scrollTop)
                {
                    posx = e.clientX + document.documentElement.scrollLeft;
                    posy = e.clientY + document.documentElement.scrollTop;
                }
                else
                {
                    posx = e.clientX + document.body.scrollLeft;
                    posy = e.clientY + document.body.scrollTop;
                }
            }
            BOX_show('neworedit');
            document.getElementById("neworedit").style.position = "absolute";
            document.getElementById("neworedit").style.top = posy + "px";
            document.getElementById("neworedit").style.left= posx + "px";
            document.getElementById("newnode").style.display = 'block';
        }
        
        //function addForumTree(parentid,spacecount,isitem)
        function addForumTree(parentid,spacecount,selectfid)
        {
            var issub = false;
            var forums = document.getElementById("forumtreelist");
            for(var i = 0 ; i < forumstree.length ; i++)
            {
                if(forumstree[i]["parentid"] == parentid)
                {
                    space = getSpace(spacecount);
                    if(forumstree[i]["parentid"] == "0")
                    {
                        var systemGroup  = document.createElement("OPTGROUP");
                        systemGroup.label = space + forumstree[i]["name"];
                        forums.appendChild(systemGroup);
                    }
                    else if(findSelect(forumstree[i]["fid"]) && forumstree[i]["fid"] != selectfid)
                    {
                        var systemGroup  = document.createElement("OPTGROUP");
                        systemGroup.label = space + forumstree[i]["name"];
                        forums.appendChild(systemGroup);
                    }
                    else
                    {
                        var item = new Option(space + forumstree[i]["name"],forumstree[i]["fid"]);
                        forums.options.add(item);
                    }
                    issub = true;
                    spacecount++;
                    //addForumTree(forumstree[i]["fid"],spacecount,isitem);
                    addForumTree(forumstree[i]["fid"],spacecount,selectfid);
                    spacecount--;
                }
            }
            if(!issub)
            {
                //isoptgroup = false;
                return;
            }
        }
        
        function findSelect(fid)
        {
            for(var i = 0 ; i < obj.length ; i++)
            {
                if(fid == obj[i]["cfid"])
                    return true;
            }
            return false;
        }
        
        function getSpace(count)
        {
            var space = "";
            for(var i = 0; i < count; i++)
                space += "　　";
            return space;
        }
        
        function sethighlevel()
        {
            document.getElementById("highlevel").value = "set";
            document.forms[0].submit();
        }
        </script>
<meta http-equiv="X-UA-Compatible" content="IE=7" />
    </head>
    <body style="overflow:scroll;">
        <form id="Form1" runat="server" >
            <div style="clear:both;position:relative">
                <uc1:PageInfo id="info1" runat="server" Icon="Information" Text="<li>支持鼠标拖拽更改商品分类</li>"></uc1:PageInfo>
                <a class="TopicButton" onclick="javascript:newRootNode(event);" type="button"><img src="../images/add.gif"/> 添加顶级分类 </a>&nbsp;&nbsp;
                <a class="TopicButton" onclick="javascript:sethighlevel();" type="button"><img src="../images/del.gif"/> 取消版块绑定 </a>&nbsp;&nbsp;
                <a class="TopicButton" onclick="javascript:window.location='mall_goodscategoriesmanage.aspx?method=updateall';" type="button"><img src="../images/cache_resetall.gif"/> 更新所有分类商品数 </a>
                <input type="hidden" name="highlevel" id="highlevel" />
                <div class="Navbutton" style=" margin:0 auto; text-align:center;">
	                <table width="100%">
		                <tr>
			                <td width="100%">
				                <asp:Label id="ShowTreeLabel" runat="server"></asp:Label>
				                <asp:Label ID="ForumsTreeLabel" runat="server"></asp:Label>
			                </td>
		                </tr>
	                </table>
                </div>
            </div>
            <div id="BOX_overlay" style="background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;"></div>
            <div id="neworedit" style="display: none; background :#fff; padding:10px; border:1px solid #999; width:450px;">
            <div id="newnode" style="border: 1px dotted rgb(219, 221, 211); padding: 10px; background: rgb(253, 255, 242); clear: both; margin-top: 10px; margin-bottom: 10px; text-align: left; font-size: 12px;display:none">
               <p>分类名称:&nbsp;<input type="text" id="categoryname" name="categoryname" /></p>
               <p id="forumtreelistlayer" style="height:30px;margin-top:5px;">绑定版块:&nbsp;<select id="forumtreelist" name="forumtreelist"></select></p>
               <input type="hidden" name="parentcategoryid" id="parentcategoryid" />
               <input type="hidden" name="categoryid" id="categoryid" />
               <input type="hidden" name="method" id="method" />
               <p style="text-align:center;height:20px;">
                   <a class="TopicButton" onclick="javascript:saveNode();" type="button"><img src="../images/submit.gif"/> 保存</a>&nbsp;
                   <a class="TopicButton" onclick="javascript:BOX_remove('neworedit');" type="button"><img src="../images/state1.gif"/> 取消</a>
               </p>
            </div>
            </div>
        </form>
        <div id="setting" />
        <%=footer%>
    </body>
</html>
