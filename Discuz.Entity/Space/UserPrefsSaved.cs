using System;
using System.Collections;
using System.Xml;

namespace Discuz.Entity
{
	/// <summary>
	/// UserPrefSaved 的摘要说明。
	/// </summary>
	public class UserPrefsSaved
	{
		/// <summary>
		/// 用户设置集合
		/// </summary>
		/// <param name="xmlcontent">xml内容</param>
		public UserPrefsSaved(string xmlcontent)
		{
			if (xmlcontent == null || xmlcontent == string.Empty)
			{
				return;
			}
			XmlDocument xml = new XmlDocument();
			try
			{
				xml.LoadXml(xmlcontent);
			}
			catch
			{
				return;
			}
			XmlNodeList xmlnodelist = xml.SelectNodes("/Module/UserPref");

			foreach (XmlNode xmlnode in xmlnodelist)
			{
				this._userPrefs.Add(xmlnode.Attributes["name"].Value, xmlnode.Attributes["value"].Value);
			}
		}

		private Hashtable _userPrefs = new Hashtable();

		/// <summary>
		/// 根据name获得value
		/// </summary>
		/// <param name="name">指定的name</param>
		/// <returns></returns>
		public string GetValueByName(string name)
		{
			return this._userPrefs[name] == null ? "" : this._userPrefs[name].ToString() ;
		}
	}
}
