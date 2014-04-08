using System;
using Discuz.Common;

namespace Discuz.Entity
{
	/// <summary>
	/// 用户使用偏好
	/// </summary>
	public class UserPref
	{
		public UserPref()
		{

		}

		private string _name = string.Empty;
		private string _displayName = string.Empty;
		private UserPrefDataType _dataType = UserPrefDataType.StringType;
		private string _defaultValue = string.Empty;
		private string _urlParam = string.Empty;
		private bool _required = false;

#if NET1
		private EnumValueCollection _enumValues = null;
#else
        private Discuz.Common.Generic.List<EnumValue> _enumValues = null;
#endif
		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		public string DisplayName
		{
			get { return _displayName; }
			set { _displayName = value; }
		}

		/// <summary>
		/// 数据类型
		/// </summary>
		public UserPrefDataType DataType
		{
			get { return _dataType; }
			set	{ _dataType = value;}
		}

		/// <summary>
		/// 默认值
		/// </summary>
		public string DefaultValue
		{
			get { return _defaultValue; }
			set { _defaultValue = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string UrlParam
		{
			get { return _urlParam; }
			set { _urlParam = value; }
		}

		/// <summary>
		/// 此项偏好是否必须选择
		/// </summary>
		public bool Required
		{
			get { return _required; }
			set { _required = value; }
		}

		/// <summary>
		/// 偏好为枚举型时可供枚举的值
		/// </summary>
		//public EnumValueCollection EnumValues

#if NET1
        public EnumValueCollection EnumValues
        {
			get { return _enumValues; }
			set { _enumValues = value; }
		}
#else
        public Discuz.Common.Generic.List<EnumValue> EnumValues
		{
			get { return _enumValues; }
			set { _enumValues = value; }
		}
#endif

		public string ToHtml()
		{
			return this.ToHtml("");
		}
		public string ToHtml(string value)
		{
			string result = string.Empty;
			value = value == "" ? this._defaultValue : value;
			string displayname = this._displayName == "" ? this._name : this._displayName;
			string required = this._required == true ? "<font color=\"red\">*</font>" : "";
			switch (this._dataType)
			{
				case UserPrefDataType.BoolType:
					int chechedvalue = 0;
					if (value.ToLower() == "true" || value == "1")
					{
						value = "checked";
						chechedvalue = 1;
					}
					else
					{
						value = "";
					}
					//int chechedvalue = (value == "true" || value == "1") ? 1 : 0;
					result = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{4}</td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input id=\"m___MODULE_ID_____ITEM_INDEX__\" name=\"m___MODULE_ID___up_{3}\" value=\"{1}\" type=\"hidden\" /><input type=\"checkbox\" {2} onclick=\"_gel('m___MODULE_ID_____ITEM_INDEX__').value = this.checked ? '1' : '0';\" /></td></tr>", displayname, chechedvalue, value, this._name, required);
					break;
				case UserPrefDataType.EnumType:
					foreach (EnumValue ev in this._enumValues)
					{
						string displayvalue = ev.DisplayValue == "" ? ev.Value : ev.DisplayValue;
						if (ev.Value == value)
						{
							result += string.Format("<option value=\"{0}\" selected>{1}</option>\r\n", ev.Value, displayvalue);
						}
						else
						{
							result += string.Format("<option value=\"{0}\">{1}</option>\r\n", ev.Value, displayvalue);
						}
					}
					result = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{3}</td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<select id=\"m___MODULE_ID_____ITEM_INDEX__\" name=\"m___MODULE_ID___up_{1}\">{2}</select></td></tr>", displayname, this._name, result, required);
					break;
				case UserPrefDataType.HiddenType:
					result = "";
					break;
				case UserPrefDataType.StringType:
					result = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{3}</td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input type=\"text\" size=\"20\" maxlen=\"200\" name=\"m___MODULE_ID___up_{1}\" value=\"{2}\" /></td></tr>", displayname, this._name, value, required);
					break;
				case UserPrefDataType.ListType:
					result = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{1}</td><td width=\"65%\" nowrap=\"\" align=\"left\" colspan=\"2\"><script>check_ac___MODULE_ID_____ITEM_INDEX__ = null;</script><nobr><input type=\"text\" name=\"m___MODULE_ID___up_{3}_val\" value=\"\" id=\"m___MODULE_ID_____ITEM_INDEX___val\" maxlen=\"200\" size=\"20\"/><input id=\"m___MODULE_ID_____ITEM_INDEX___add\" type=\"button\" onclick=\"m___MODULE_ID_____ITEM_INDEX___App.add();\" value=\"Add\"/><input id=\"m___MODULE_ID_____ITEM_INDEX__\" type=\"hidden\" value=\"{3}\" name=\"m___MODULE_ID___up_{2}\"/></nobr></td></tr>", displayname, required, this._name, value);
					result += string.Format("<tr><td /><td><div id=\"m___MODULE_ID_____ITEM_INDEX___disp\" style=\"padding-top: 4px;\">");
//					<font size=\"-1\">int i = 0;
//					foreach (string v in value.Split('|'))
//					{
//						result += string.Format("<a class=\"delbox\" style=\"margin: 1px 3px 0px 0px; float: left;\" onclick=\"m___MODULE_ID_____ITEM_INDEX___App.del({0})\" href=\"###\"/>{1}</a><br/>", i.ToString(), v);
//						i++;
//					}</font>
					result += string.Format("</div><script type=\"text/javascript\"><!--//\r\nlistcontrol___MODULE_ID__.push([_gel(\"m___MODULE_ID_____ITEM_INDEX___val\"),check_ac___MODULE_ID_____ITEM_INDEX__]);var m___MODULE_ID_____ITEM_INDEX___TextVal = _gel('m___MODULE_ID_____ITEM_INDEX__').value;var m___MODULE_ID_____ITEM_INDEX___App = new _PrefListApp(\"__ITEM_INDEX__\",\"up_{0}\",m___MODULE_ID_____ITEM_INDEX___TextVal,_ListItem,\"__MODULE_ID__\");m___MODULE_ID_____ITEM_INDEX___App.refresh();_gel('m___MODULE_ID_____ITEM_INDEX__').listApp = m___MODULE_ID_____ITEM_INDEX___App;_gel('m___MODULE_ID_____ITEM_INDEX___val').listApp = m___MODULE_ID_____ITEM_INDEX___App;\r\n// --></script></td></tr>", this._name);
					break;
				case UserPrefDataType.LoactionType:
					result = "";
					break;
				default:
					break;
			}

			return result;
		}
	}
}
