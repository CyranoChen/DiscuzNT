using System;

namespace Discuz.Entity
{
	/// <summary>
	/// Content 的摘要说明。
	/// </summary>
	public class ModuleContent
	{
		public ModuleContent()
		{
			//
			// TODO: 在此处添加构造函数逻辑
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
