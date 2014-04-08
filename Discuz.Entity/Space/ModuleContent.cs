using System;

namespace Discuz.Entity
{
	/// <summary>
	/// Content ��ժҪ˵����
	/// </summary>
	public class ModuleContent
	{
		public ModuleContent()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		private ModuleContentType _type = ModuleContentType.Html;
		private string _href = string.Empty;
		private string _cdata = string.Empty;
		private string _contentHtml = string.Empty;

		public ModuleContentType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public string Href
		{
			get { return _href; }
			set { _href = value; }
		}

		public string CData
		{
			get { return _cdata; }
			set { _cdata = value; }
		}

		public string ContentHtml
		{
			get { return _contentHtml; }
			set { _contentHtml = value; }
		}
	}
}
