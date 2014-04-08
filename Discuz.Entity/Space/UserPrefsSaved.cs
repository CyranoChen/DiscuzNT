using System;
using System.Collections;
using System.Xml;

namespace Discuz.Entity
{
	/// <summary>
	/// UserPrefSaved ��ժҪ˵����
	/// </summary>
	public class UserPrefsSaved
	{
		/// <summary>
		/// �û����ü���
		/// </summary>
		/// <param name="xmlcontent">xml����</param>
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
		/// ����name���value
		/// </summary>
		/// <param name="name">ָ����name</param>
		/// <returns></returns>
		public string GetValueByName(string name)
		{
			return this._userPrefs[name] == null ? "" : this._userPrefs[name].ToString() ;
		}
	}
}
