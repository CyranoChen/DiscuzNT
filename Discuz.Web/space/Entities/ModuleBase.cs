using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Provider;
using Discuz.Space.Utilities;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Space.Entities
{
	/// <summary>
	///  模块抽象
	/// </summary>
	public class ModuleBase : ISpaceCommand
	{
		public ModuleBase()
		{
		}

		#region Properties
		private int _moduleID;
		public int ModuleID
		{
			get
			{
				return _moduleID;
			}
			set
			{
				_moduleID = value;
			}
		}

		private ModuleDefInfo _moduleDef;
		public ModuleDefInfo ModuleDef
		{
			get
			{
				return _moduleDef;
			}	
			set
			{
				_moduleDef = value;
			}
		}

		private ModuleInfo _module;
		public ModuleInfo Module
		{
			get
			{
				return _module;
			}	
			set
			{
				_module = value;
			}
		}

		private ModulePref _modulePref;
		public ModulePref ModulePref
		{
			get
			{
				return _modulePref;
			}	
			set
			{
				_modulePref = value;
			}
		}

		private ModuleContent _moduleContent;
		public ModuleContent ModuleContent
		{
			get
			{
				return _moduleContent;
			}	
			set
			{
				_moduleContent = value;
			}
		}


        private UserPrefCollection<UserPref> _userPrefCollection;
        public UserPrefCollection<UserPref> UserPrefCollection
		{
			get
			{
				return _userPrefCollection;
			}	
			set
			{
				_userPrefCollection = value;
			}
		}

		private int _userid;
		public int UserID
		{
			get
			{
				return _userid;
			}
			set
			{
				_userid = value;
			}
		}

		private SpaceConfigInfo _spaceconfig;
		public SpaceConfigInfo SpaceConfig
		{
			get
			{
				return _spaceconfig;
			}
			set
			{
				_spaceconfig = value;
			}
		}

		private bool _editable = false;
		public bool Editable
		{
			get
			{
				return _editable;
			}
			set
			{
				_editable = value;	
			}
		}

		private bool _scalable = true;
		public bool Scalable
		{
			get
			{
				return _scalable;
			}
			set
			{
				_scalable = value;	
			}
		}
		#endregion

        /// <summary>
        /// 获得模块提交数据
        /// </summary>
        /// <param name="httpContext">当前httpContext对象</param>
        /// <returns>返回重新加载的内容(保留功能，下个版本改进)</returns>
		public virtual string GetModulePost(HttpContext httpContext)
		{
			string upPrefix = "m_" + Module.ModuleID + "_up_";
			string valKey = "m_" + Module.ModuleID + "_val";
			string val = "";

			Hashtable ht = new Hashtable();
			foreach (DictionaryEntry de in ModuleXmlHelper.LoadUserPrefsSaved(_module.UserPref))
			{
				ht[de.Key] = de.Value;
			}

			NameValueCollection nvc = httpContext.Request.QueryString;
			foreach (string s in nvc.Keys)
			{
                //需要重构，只有当跟默认值不同并且存在当前s的键才可以插入。
				if (s.StartsWith(upPrefix))
					ht[s.Replace(upPrefix, string.Empty)] = Utils.RemoveHtml(nvc[s].Replace("\"", "&quot;"));

				if (s == valKey)
					val = nvc[s];
			}
			_module.UserPref = ModuleXmlHelper.GetXmlFromHashTable(ht);
			if (Utils.IsNumeric(val))
				_module.Val = Utils.StrToInt(val, 3);
			
			Spaces.UpdateModule(_module);
			return "";
		}

        /// <summary>
        /// 模块加载的操作
        /// </summary>
        /// <param name="content"></param>
        /// <returns>返回要显示的内容</returns>
		public virtual string OnMouduleLoad(string content)
		{
			return content;
		}

        /// <summary>
        /// 当模块删除时的操作，主要是删除数据库中冗余数据
        /// </summary>
        protected virtual void OnRemove()
        {
            return;
        }

        /// <summary>
        /// 编辑器内容加载时的操作
        /// </summary>
        /// <param name="editbox">待加工html内容</param>
        /// <returns>返回要显示的内容</returns>
		public virtual string OnEditBoxLoad(string editbox)
		{
			return editbox;
		}

		/// <summary>
		/// Get the content area's html 内建模块专有功能
		/// </summary>
		/// <returns>模块内容区的html</returns>
		public string GetModuleHtml()
		{
			string prefScriptFormat = "_DS_Prefs._add(\"{0}\",\"up_{1}\",\"{2}\");";
			UserPrefsSaved ups = new UserPrefsSaved(_module.UserPref);
			StringBuilder registScript = new StringBuilder("<script type=\"text/javascript\">");
			string root = BaseConfigs.GetForumPath;
			Hashtable ht = new Hashtable();

			if (_module.ModuleDefID == 0)
				_editable = _userPrefCollection == null ? false : _userPrefCollection.VisibleItemCount > 0;
			if (_module.ModuleType == ModuleType.Rss)
				_editable = true;

			string boxin = OnMouduleLoad(GetModuleBoxin());
			string editbox = OnEditBoxLoad(GetModuleEditBox());
			string moduleTitle = "<span id=\"m___MODULE_ID___title\" class=\"modtitle_text\"></span>";
			if (_module.ModuleType == ModuleType.Local)
				moduleTitle = _modulePref.TitleUrl == string.Empty ? "<span id=\"m___MODULE_ID___title\" class=\"modtitle_text\">" + _modulePref.Title + "</span>" : "<a class=\"mtlink\" id=\"m___MODULE_ID___url\" href=\"" + _modulePref.TitleUrl + "\" target=\"_blank\"><span id=\"m___MODULE_ID___title\" class=\"modtitle_text\">" + _modulePref.Title + "</span></a>";

			if (_moduleContent != null && _moduleContent.Type != ModuleContentType.Url)
			{
				//在内容区用js加载UserPref设置，并且将UserPref值加载到TemplateEngine的需要替换的集合里
				foreach (UserPref up in _userPrefCollection)
				{
					string userprefvalue = ups.GetValueByName(up.Name);
					userprefvalue = userprefvalue == string.Empty ? up.DefaultValue : userprefvalue;
					registScript.AppendFormat(prefScriptFormat, Module.ModuleID, up.Name, userprefvalue);

					string upName = "__UP_" + up.Name + "__";
					boxin = boxin.Replace(upName, userprefvalue);
					editbox = editbox.Replace(upName, userprefvalue);
					moduleTitle = moduleTitle.Replace(upName, userprefvalue);
				}
			}
			ht.Add("modboxin", registScript.Append("</script>").Append(boxin).ToString());
			ht.Add("meditbox", editbox);
			ht.Add("title", moduleTitle);
			ht.Add("editable", _editable ? _module.Uid == _userid : false);//还需要加权限判断
			ht.Add("scalable", _scalable);
			ht.Add("deletable", _module.Uid == _userid && _userid > 0);
			
			string html = ModuleTemplate.Instance.GetHtml(ht);//te.MergeTemplate();
			html = html.Replace("__MODULE_ID__", _moduleID.ToString());
			html = html.Replace("__TAB_ID__", _module.TabID.ToString());
			if (_userPrefCollection != null)
			{
				html = html.Replace("__USERPREF_COUNT__", _userPrefCollection.Count.ToString());
			}
            return html;
		}

        /// <summary>
        /// 获取编辑区内容，根据不同类型来做处理，然后交给子类再加工
        /// </summary>
        /// <returns>编辑区html</returns>
		private string GetModuleEditBox()
		{
			switch(_module.ModuleType)
			{
				case ModuleType.Remote ://do nothing
					return "";
				case ModuleType.Rss :
					return GetRssEditBox();
				case ModuleType.Error ://do nothing
					return "";
			}
			return ModuleXmlHelper.GetUserPrefsTable(_userPrefCollection, new UserPrefsSaved(_module.UserPref));
		}

        /// <summary>
        /// 获取RSS的编辑区HTML
        /// </summary>
        /// <returns>编辑区HTML</returns>
		private string GetRssEditBox()
		{
			StringBuilder options = new StringBuilder();
			for (int i = 1; i < 16; i++)
			{
				options.AppendFormat("<option value='{0}'{1}>{0}</option>", i, i == this._module.Val ? " selected" : "");
			}
			return "显示 <select onchange=\"_uhc('__MODULE_ID__','val',this.value)\">" + options + "</select> 条 ";
		}

        /// <summary>
        /// 获取模块内容区html，不包括内置模块
        /// </summary>
        /// <returns>内容区html</returns>
		private string GetModuleBoxin()
		{
			switch(_module.ModuleType)
			{
				case ModuleType.Remote :
					return "此类模块暂不支持";
				case ModuleType.Rss :
					return GetRssBoxin();
				case ModuleType.Error :
					return "此类模块暂不支持";
			}

			//如果是内建类型则进行其他处理
			UserPrefsSaved ups = new UserPrefsSaved(_module.UserPref);
			string parms = "", returnStr = "";
			
			switch (_moduleContent.Type)
			{
				case ModuleContentType.Html :
					foreach (UserPref up in _userPrefCollection)
					{
						string userprefvalue = ups.GetValueByName(up.Name);
						userprefvalue = userprefvalue == string.Empty ? up.DefaultValue : userprefvalue;
						parms += string.Format("&up_{0}={1}", up.Name, Utils.UrlEncode(userprefvalue));
					}
					//{0}代表模块url,{1}代表render_inline,{2}代表当前站点路径
					returnStr = "<script><!--\r\nremote_modules.push(new RemoteModule(\"{0}\",\"__MODULE_ID__\",\"{1}\",\"{4}space/ifr.aspx?url={0}&nocache=1&uid={2}&mid=__MODULE_ID__&parent={3}\",false));// -->\r\n</script><div id=remote___MODULE_ID__ style=\"border:0px;padding:0px;margin:0px;width:100%\"><iframe id=remote_iframe___MODULE_ID__ style=\"border:0px;padding:0px;margin:0px;width:100%;height:200px;overflow:hidden;background:transparent;\" allowtransparency=\"yes\" frameborder=0 scrolling=no></iframe></div>";
					returnStr = string.Format(returnStr, _module.ModuleUrl, _modulePref.RenderInline, _module.Uid, GeneralConfigs.GetConfig().Forumurl + parms, BaseConfigs.GetForumPath);
					break;
				case ModuleContentType.HtmlInline :
					returnStr = _moduleContent.ContentHtml;
					break;
				case ModuleContentType.Url :
					//这里要对ContentHtml(这里一开始它的值与Href相同)进行进一步的处理，因为需要传递参数，参数是UserPref
					foreach (UserPref up in _userPrefCollection)
					{
						string userprefvalue = ups.GetValueByName(up.Name);
						userprefvalue = userprefvalue == string.Empty ? up.DefaultValue : userprefvalue;
						parms += string.Format("&up_{0}={1}", up.Name, Utils.UrlEncode(userprefvalue));
					}					
					if (parms.StartsWith("&"))
						parms = parms.TrimStart('&');

					returnStr = string.Format(_moduleContent.ContentHtml, parms);
					break;
			}
			return returnStr;
		}

        /// <summary>
        /// 获取RSS模块内容区html
        /// </summary>
        /// <returns>内容区html</returns>
		private string GetRssBoxin()
		{
            Hashtable ht = new Hashtable();
		    ht["forumpath"] = BaseConfigs.GetForumPath;
			return RssTemplate.Instance.GetHtml(ht).Replace("__MODULE_URL__", _module.ModuleUrl).Replace("__MODULE_VAL__", _module.Val.ToString());
		}

        /// <summary>
        /// 删除模块
        /// </summary>
        public void RemoveModule()
        {
            OnRemove();
            Spaces.DeleteModuleById(this.ModuleID, this.Module.Uid);
        }
    }
}
