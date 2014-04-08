using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Entity;

namespace Discuz.Space.Utilities
{
	/// <summary>
	/// ModuleValidate ��ժҪ˵����
	/// </summary>
	public class ModuleValidate
	{
		public ModuleValidate()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ��֤ģ������
		/// </summary>
		/// <param name="url">ģ��Xml�ļ���ַ</param>
		/// <returns>����ģ������</returns>
		public static ModuleType ValidateModuleType(string url)
		{
			try
			{
				bool local = false;
				string content = string.Empty;
				XmlDocument xml = new XmlDocument();

				if (!url.StartsWith("http://"))//��Ե�ַ��ȡ���������ļ�����
				{
					string filename = Utils.GetMapPath("modules/" + url);
					content = StaticFileProvider.GetContent(filename);
					local = true;
					xml.LoadXml(content);
				}
				else
				{
					string err = "";

					HttpWebResponse response = Globals.GetPageResponse(url, out err);
					if (response == null)
					{
						return ModuleType.Error;
					}
					//Encoding encoding = Encoding.GetEncoding(enc);
					Stream instream = response.GetResponseStream();
					xml.Load(instream);

					//content = Globals.GetSourceTextByUrl(url);
				}
			
			
				if (xml.DocumentElement.Name.ToLower() == "rss")
					return ModuleType.Rss;

				if (local)
					return ModuleType.Local;
				else
					return ModuleType.Remote;
			}
			catch
			{
				return ModuleType.Error;
			}
		}
	}
}
