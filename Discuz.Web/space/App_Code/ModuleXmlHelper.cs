using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Entity;

namespace Discuz.Space
{
	/// <summary>
	/// ModuleXmlHelper 的摘要说明。
	/// </summary>
	public class ModuleXmlHelper
	{
		#region 常量
		private const string SCRIPT_NO_SET_TITLE = "function _DS_SetTitle(title){throw new Error(\"To use this feature, you must add \"+\"<Require feature='settitle'/> to your <ModulePref> tag.\");};";
		private const string SCRIPT_NO_DYNAMIC_HEIGHT = "function _DS_AdjustIFrameHeight(){throw new Error(\"To use this feature, you must add \"+\"<Require feature='dynamic-height'/> to your "+"<ModulePref> tag.\");};";
		private const string SCRIPT_SET_TITLE = @"function _IFPC_SetTitle(title) {sendRequest(""remote_iframe___MODULE_ID__"",""set_title"",[""remote_iframe___MODULE_ID__"", title],""ifpc_relay.aspx"",null,"""");}
function _DS_SetTitle(a)
{
	if(_args()[""synd""]==""open"")
	{
		return; 
	}
	_IFPC_SetTitle(a)
};";
		private const string SCRIPT_SET_TITLE_INLINE = @"		function _setModTitle(title,module_id)
{
	var title_element=_gel(""m_""+module_id+""_title"");
	if(title_element)
	{
		title_element.innerHTML=_hesc(title);																		
	}
}

function _DS_SetTitle(title,specified_module_id)
{
	if(typeof (specified_module_id)==""undefined""||!specified_module_id||specified_module_id==""undefined"")
	{
		throw new Error(""Inline modules must specify their ""+""__MODULE_ID__ when using _DS_SetTitle"");
		
	}
	else 
	{
		_setModTitle(title,specified_module_id);																			
	}
}";
		
		private const string SCRIPT_DYNAMIC_HEIGHT_INLINE =	@"		var igRDH_=0;
function _IFPC_AdjustIFrameHeight(a)
{
	var b=igRDH_,c=document.body;
	if(a!==undefined)
	{
		a=parseInt(a,10);
		a=isNaN(a)?-1:a
	}
	else 
	{
		a=-1
	}
	if(a>=0)
	{
		b=a
	}
	else if(c)
	{
		if(document.compatMode==""CSS1Compat""&&document.documentElement.scrollHeight)
		{
			b=document.documentElement.scrollHeight
		}
		else 
		{
			var d=c.scrollHeight,e=c.offsetHeight;
			b=d>e?d:e
		}
	}
	if(igRDH_!=b)
	{
		_IFPC.call(_IFPC_SUPPORT.iframeId,""resize_iframe"",[_IFPC_SUPPORT.iframeId,b],_IFPC_SUPPORT.relayUrl,null,"""");
		igRDH_=b
	}
}
function _DS_AdjustIFrameHeight(a)
{
	if(_args()[""synd""]==""open"")
	{
		return;
	}
	setTimeout(function (){_IFPC_AdjustIFrameHeight(a)},10);
}
_DS_AddEventHandler(""resize"",_DS_AdjustIFrameHeight);";

		private const string SCRIPT_DYNAMIC_HEIGHT = @"var ifpc_height = 0;function _IFPC_AdjustIFrameHeight(opt_height) {var h = ifpc_height;var el = document.getElementById(""remote___MODULE_ID__"");if (opt_height !== undefined) {opt_height = parseInt(opt_height, 10);opt_height = isNaN(opt_height) ? -1 : opt_height;} else {opt_height = -1;}if (opt_height >= 0) {h = opt_height;} else if (el) {var sh = el.scrollHeight;var oh = el.offsetHeight;h = sh > oh ? sh : oh;}if (ifpc_height != h) {sendRequest(""remote_iframe___MODULE_ID__"",""resize_iframe"",[""remote_iframe___MODULE_ID__"", h],""ifpc_relay.aspx"",null,"""");ifpc_height = h;}}
function _DS_AdjustIFrameHeight(opt_height) {if (_args()[""synd""] == ""open"") {return;}setTimeout(function() { _IFPC_AdjustIFrameHeight(opt_height); }, 10);}
_DS_AddEventHandler(""resize"", _DS_AdjustIFrameHeight);";
		#endregion
		public static string GetModuleRequireScript(ModulePref modulePref, bool isInline)
		{
			Hashtable ht = new Hashtable();
			ht.Add(FeatureType.SetTitle, SCRIPT_NO_SET_TITLE);
			ht.Add(FeatureType.Dynamic_Height, SCRIPT_NO_DYNAMIC_HEIGHT);

			if (modulePref.Requires != null)
			{
				foreach (ModuleRequire mr in modulePref.Requires)
				{
					string script = string.Empty;
					switch (mr.Feature)
					{
						case FeatureType.Analytics:
							break;
						case FeatureType.Drag:
							break;
						case FeatureType.Dynamic_Height:
							script = isInline ? SCRIPT_DYNAMIC_HEIGHT_INLINE : SCRIPT_DYNAMIC_HEIGHT;
							break;
						case FeatureType.Flash:
							break;
						case FeatureType.Grid:
							break;
						case FeatureType.MiniMessage:
							break;
						case FeatureType.SetPrefs:
							break;
						case FeatureType.SetTitle:
							if (isInline)
								script = SCRIPT_SET_TITLE_INLINE;
							else
								script = SCRIPT_SET_TITLE;
							break;
						case FeatureType.Tabs:
							break;
					}

					ht[mr.Feature] = script;
				}
			}
		
			StringBuilder sb = new StringBuilder();

			foreach (DictionaryEntry de in ht)
			{
				sb.Append(de.Value.ToString());
			}
			return sb.ToString();
		}

		/// <summary>
		/// 加载ModulePref信息
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		public static ModulePref LoadModulePref(string filename)
		{
			XmlDocument xmlfile = LoadXmlFile(filename);

			if (xmlfile == null)
				return null;
			XmlNode xmlnode = xmlfile.SelectSingleNode("/Module/ModulePrefs");

			if (xmlnode == null)
				return null;

			ModulePref module = new ModulePref();

			module.Author = xmlnode.Attributes["author"] == null ? "" : xmlnode.Attributes["author"].Value;
			module.AuthorAffiliation = xmlnode.Attributes["author_affiliation"] == null ? "" : xmlnode.Attributes["author_affiliation"].Value;
			module.AuthorEmail = xmlnode.Attributes["author_email"] == null ? "" : xmlnode.Attributes["author_email"].Value;
			module.AuthorLocation = xmlnode.Attributes["author_location"] == null ? "" : xmlnode.Attributes["author_location"].Value;
			module.Category = xmlnode.Attributes["category"]== null ? "" : xmlnode.Attributes["category"].Value;
			module.Category2 = xmlnode.Attributes["category2"] == null ? "" : xmlnode.Attributes["category2"].Value;
			module.Description = xmlnode.Attributes["description"] == null ? "" : xmlnode.Attributes["description"].Value;
			module.RenderInline = xmlnode.Attributes["render_inline"] == null ? "" : xmlnode.Attributes["render_inline"].Value;
			module.Screenshot = xmlnode.Attributes["screenshot"] == null ? "" : xmlnode.Attributes["screenshot"].Value;
			module.Thumbnail = xmlnode.Attributes["thumbnail"] == null ? "" : xmlnode.Attributes["thumbnail"].Value;
			module.Title = xmlnode.Attributes["title"] == null ? "" : xmlnode.Attributes["title"].Value;
			module.TitleUrl = xmlnode.Attributes["title_url"] == null ? "" : xmlnode.Attributes["title_url"].Value;
			module.DirectoryTitle = xmlnode.Attributes["directory_title"] == null ? "" : xmlnode.Attributes["directory_title"].Value;
			module.Height = xmlnode.Attributes["height "] == null ? 200 : Utils.StrToInt(xmlnode.Attributes["height"].Value, 200);
			module.Width = xmlnode.Attributes["width"] == null ? 320 : Utils.StrToInt(xmlnode.Attributes["width"].Value, 320);
			module.Scaling = xmlnode.Attributes["scaling"] == null ? true : Utils.StrToBool(xmlnode.Attributes["scaling"].Value, true);
			module.Scrolling = xmlnode.Attributes["scrolling"] == null ? false : Utils.StrToBool(xmlnode.Attributes["scrolling"].Value, false);
			module.Singleton = xmlnode.Attributes["singleton"] == null ? true : Utils.StrToBool(xmlnode.Attributes["singleton"].Value, true);
			module.AuthorPhoto = xmlnode.Attributes["author_photo"] == null ? "" : xmlnode.Attributes["author_photo"].Value;
			module.AuthorAboutMe = xmlnode.Attributes["author_aboutme"] == null ? "" : xmlnode.Attributes["author_aboutme"].Value;
			module.AuthorLink = xmlnode.Attributes["author_link"] == null ? "" : xmlnode.Attributes["author_link"].Value;
			module.AuthorQuote = xmlnode.Attributes["author_quote"] == null ? "" : xmlnode.Attributes["author_quote"].Value;
            module.Controller = xmlnode.Attributes["controller"] == null ? "" : xmlnode.Attributes["controller"].Value;

			XmlNodeList xnl = xmlfile.SelectNodes("/Module/ModulePrefs/Require");
			if (xnl.Count > 0)
			{
                module.Requires = new Discuz.Common.Generic.List<ModuleRequire>();
				foreach (XmlNode xn in xnl)
				{
					module.Requires.Add(new ModuleRequire(xn.Attributes["feature"] == null ? "" : xn.Attributes["feature"].Value));
				}
			}
			return module;
		}

		/// <summary>
		/// 返回指定文件中的UserPref集合
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
        public static UserPrefCollection<UserPref> LoadUserPrefs(string filename)
    	{
			XmlDocument xmlfile = LoadXmlFile(filename);

			if (xmlfile == null)
				return null;
			XmlNodeList xmlnodelist = xmlfile.SelectNodes("/Module/UserPref");

			if (xmlnodelist == null)
				return null;

            UserPrefCollection<UserPref> upc = new UserPrefCollection<UserPref>();

			foreach (XmlNode xmlnode in xmlnodelist)
			{
				UserPref up = new UserPref();
				up.Name = xmlnode.Attributes["name"] == null ? "" : xmlnode.Attributes["name"].Value;
				up.DisplayName = xmlnode.Attributes["display_name"] == null ? "" : xmlnode.Attributes["display_name"].Value;
				up.UrlParam = xmlnode.Attributes["urlparam"] == null ? "" : xmlnode.Attributes["urlparam"].Value;
				up.DataType = xmlnode.Attributes["datatype"] == null ? UserPrefDataType.StringType : ParseUserPrefDataType(xmlnode.Attributes["datatype"].Value);

				if (up.DataType == UserPrefDataType.EnumType)
				{
                    up.EnumValues = new Discuz.Common.Generic.List<EnumValue>();
					XmlNodeList enumlist = xmlnode.SelectNodes("EnumValue");
					foreach(XmlNode enumnode in enumlist)
					{
						EnumValue ev = new EnumValue();
						ev.Value = enumnode.Attributes["value"] == null ? "" : enumnode.Attributes["value"].Value;
						ev.DisplayValue = enumnode.Attributes["display_value"] == null ? "" : enumnode.Attributes["display_value"].Value;
						up.EnumValues.Add(ev);
					}
				}

				up.Required = xmlnode.Attributes["required"] == null ? false : Utils.StrToBool(xmlnode.Attributes["required"].Value, false);
				up.DefaultValue = xmlnode.Attributes["default_value"] == null ? "" : xmlnode.Attributes["default_value"].Value;
				upc.Add(up);
			}
			return upc;
		}

		/// <summary>
		/// 将UserPrefs组织成Table表现形式
		/// </summary>
		/// <param name="defaultUserPrefs">默认设置</param>
		/// <param name="personalUserPrefs">用户的个性化设置</param>
		/// <returns></returns>
        public static string GetUserPrefsTable(UserPrefCollection<UserPref> defaultUserPrefs, UserPrefsSaved personalUserPrefs)
		{
			StringBuilder sb = new StringBuilder("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" align=\"center\">");
			
			bool scriptAdded = false;

			for (int i = 0; i < defaultUserPrefs.Count; i++)
			{
				string value = personalUserPrefs.GetValueByName(defaultUserPrefs[i].Name);

				//此处不需要进行判断value的差异是因为当value为string.empty时ToHtml方法会自动按默认值输出
				sb.Append(defaultUserPrefs[i].ToHtml(value).Replace("__ITEM_INDEX__", i.ToString()));

				if (!scriptAdded && defaultUserPrefs[i].DataType == UserPrefDataType.ListType)
				{
					scriptAdded = true;
					sb.Insert(0,"<script>var listcontrol___MODULE_ID__ = new Array();function checklist___MODULE_ID__() {for (var i = 0; i < listcontrol___MODULE_ID__.length; i ++) {var inputFld = listcontrol___MODULE_ID__[i][0];var checkFunc = listcontrol___MODULE_ID__[i][1];if (inputFld.value &&inputFld.value.length > 0) {if(!checkFunc || checkFunc()) {inputFld.listApp.add();} else {inputFld.focus();inputFld.select();return false;}}}return true;};var checklist_submit___MODULE_ID__ = function(event) {if (!checklist___MODULE_ID__()) {return false;}return this.prev_submit(event);};var reset_list___MODULE_ID__ = function() {this.prev_reset();for (var i = 0; i < this.elements.length; i ++) {if (this.elements[i].listApp) {this.elements[i].listApp.reset();}}};_gel(\"m___MODULE_ID___form\").prev_reset = _gel(\"m___MODULE_ID___form\").reset;_gel(\"m___MODULE_ID___form\").reset = reset_list___MODULE_ID__;if (!_gel(\"m___MODULE_ID___form\").prev_submit) {_gel(\"m___MODULE_ID___form\").prev_submit = _gel(\"m___MODULE_ID___form\").onsubmit;_gel(\"m___MODULE_ID___form\").onsubmit = checklist_submit___MODULE_ID__;}</script>");
				}

			}
			if (defaultUserPrefs.ShowRequired)
				sb.Append("<tr><td colspan=\"3\" align=\"left\"><font color=\"red\">* 为必填项</font></td></tr>");
			sb.Append("</table>");
			return sb.ToString();
		}

		/// <summary>
		/// 根据Hashtable组织成用户个性化设置的Xml数据
		/// </summary>
		/// <param name="hashtable"></param>
		/// <returns></returns>
		public static string GetXmlFromHashTable(Hashtable hashtable)
		{
			StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Module>");
			foreach (DictionaryEntry de in hashtable)
			{
				sb.AppendFormat("<UserPref name=\"{0}\" value=\"{1}\" />", de.Key, de.Value);
			}
			sb.Append("</Module>");

			return sb.ToString();
		}
		/// <summary>
		/// 返回指定Xml文件中的Content元素所对应的对象
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		public static ModuleContent LoadContent(string filename)
		{
			XmlDocument xmlfile = LoadXmlFile(filename);

			if (xmlfile == null)
				return null;
			XmlNode xmlnode = xmlfile.SelectSingleNode("/Module/Content");
			
			if (xmlnode == null)
				return null;
			
			ModuleContent ct = new ModuleContent();
			ct.Type = xmlnode.Attributes["type"] == null ? ModuleContentType.HtmlInline : ParseContentType(xmlnode.Attributes["type"].Value);
			switch (ct.Type)
			{
				case ModuleContentType.Html:
					//ct.ContentHtml = "<script><!--\r\nremote_modules.push(new RemoteModule(\"{0}\",\"__MODULE_ID__\",\"{1}\",\"ifr.aspx?url={0}&nocache=1&mid=__MODULE_ID__&parent={2}\",false));// -->\r\n</script><div id=remote___MODULE_ID__ style=\"border:0px;padding:0px;margin:0px;width:100%\"><iframe id=remote_iframe___MODULE_ID__ style=\"border:0px;padding:0px;margin:0px;width:100%;height:200px;overflow:hidden;\" frameborder=0 scrolling=no></iframe>";
					ct.ContentHtml = xmlnode.InnerText;
					break;
				case ModuleContentType.HtmlInline:
					ct.ContentHtml = xmlnode.InnerText;
					break;
				case ModuleContentType.Url:
					//<div id="remote_2" style="border: 0px none ; margin: 0px; padding: 0px; width: 100%;"><iframe src="http://offtype.net/google_gadget/gallery_widget.html?lang=en&amp;country=us&amp;.lang=en&amp;.country=us&amp;synd=ig&amp;mid=2&amp;parent=http://www.google.com&amp;libs=xu4k3HB9Ud0/lib/libcore.js" id="remote_iframe_2" style="border: 0px none ; margin: 0px; padding: 0px; overflow: hidden; width: 100%; height: 300px;" frameborder="0" scrolling="no"></iframe></div>
					ct.Href = xmlnode.Attributes["href"] == null ? "" : xmlnode.Attributes["href"].Value;
					ct.ContentHtml = string.Format("<div id=\"remote___MODULE_ID__\" style=\"border: 0px none ; margin: 0px; padding: 0px; width: 100%;\"><iframe src=\"{0}\"  id=\"remote_iframe___MODULE_ID__\" style=\"border: 0px none ; margin: 0px; padding: 0px; overflow: hidden; width: 100%; height: 300px; background: transparent;\" allowtransparency=\"yes\" frameborder=\"0\" scrolling=\"no\"></iframe></div>", ct.Href + "?{0}");
					break;
				default:
					break;
			}

			return ct;
		}

		public static Hashtable LoadUserPrefsSaved(string xmlContent)
		{
			if (xmlContent == string.Empty)
				return new Hashtable();

			XmlDocument xml = new XmlDocument();
			xml.LoadXml(xmlContent);

			Hashtable result = new Hashtable();
			XmlNodeList xmlNodeList = xml.SelectNodes("/Module/UserPref");

			foreach (XmlNode node in xmlNodeList)
			{
				string name = node.Attributes["name"] == null ? string.Empty : node.Attributes["name"].Value;
				string value = node.Attributes["value"] == null ? string.Empty : node.Attributes["value"].Value;
				result.Add(name, value);
			}
			return result;
		}

		/// <summary>
		/// 根据字符串返回ContentType枚举类型
		/// </summary>
		/// <param name="contentType">字符串</param>
		/// <returns></returns>
		private static ModuleContentType ParseContentType(string contentType)
		{
			switch (contentType.ToLower())
			{
				case "html":
					return ModuleContentType.Html;
				case "html-inline":
					return ModuleContentType.HtmlInline;
				case "url":
					return ModuleContentType.Url;
				default:
					return ModuleContentType.Html;
			}
		}

		/// <summary>
		/// 根据字符串返回UserPrefDataType枚举类型
		/// </summary>
		/// <param name="userprefdatatype">字符串</param>
		/// <returns></returns>
		private static UserPrefDataType ParseUserPrefDataType(string userprefdatatype)
		{
			switch (userprefdatatype.ToLower())
			{
				case "string":
					return UserPrefDataType.StringType;
				case "bool":
					return UserPrefDataType.BoolType;
				case "enum":
					return UserPrefDataType.EnumType;
				case "hidden":
					return UserPrefDataType.HiddenType;
				case "list":
					return UserPrefDataType.ListType;
				case "location":
					return UserPrefDataType.LoactionType;
				default:
					return UserPrefDataType.StringType;
			}
		}

		/// <summary>
		/// 加载Xml文件
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		private static XmlDocument LoadXmlFile(string filename)
		{
			if (!File.Exists(filename))
				return null;
			XmlDocument xmlfile = new XmlDocument();
			xmlfile.Load(filename);
			return xmlfile;
		}
	}
}
