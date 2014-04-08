using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ModulePref 的摘要说明。
	/// </summary>
	public class ModulePref
	{
		public ModulePref()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		private string _title = string.Empty;
		private string _description = string.Empty;
		private string _author = string.Empty;
		private string _authorEmail = string.Empty;
		private string _authorAffiliation = string.Empty;
		private string _authorLocation = string.Empty;
		private string _titleUrl = string.Empty;
		private string _renderInline = string.Empty;
		private string _screenshot = string.Empty;
		private string _thumbnail = string.Empty;
		private string _category = string.Empty;
		private string _category2 = string.Empty;
		private string _directoryTitle = string.Empty;
		private int _height = 200;
		private int _width = 320;
		private bool _scaling = true;
		private bool _scrolling = false;
		private bool _singleton = true;
		private string _authorPhoto = string.Empty;
		private string _authorAboutMe = string.Empty;
		private string _authorLink = string.Empty;
		private string _authorQuote = string.Empty;
        private string _controller = string.Empty;        

#if NET1
		private ModuleRequireCollection _requires = null;
#else
        private Discuz.Common.Generic.List<ModuleRequire> _requires = null;
#endif

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		public string AuthorEmail
		{
			get { return _authorEmail; }
			set { _authorEmail = value; }
		}

		public string AuthorAffiliation
		{
			get { return _authorAffiliation; }
			set { _authorAffiliation = value; }
		}

		public string AuthorLocation
		{
			get { return _authorLocation; }
			set { _authorLocation = value; }
		}

		public string TitleUrl
		{
			get { return _titleUrl; }
			set { _titleUrl = value; }
		}

		public string RenderInline
		{
			get { return _renderInline; }
			set { _renderInline = value; }
		}

		public string Screenshot
		{
			get { return _screenshot; }
			set { _screenshot = value; }
		}

		public string Thumbnail
		{
			get { return _thumbnail; }
			set { _thumbnail = value; }
		}

		public string Category
		{
			get { return _category; }
			set { _category = value; }
		}

		public string Category2
		{
			get { return _category2; }
			set { _category2 = value; }
		}

		public string DirectoryTitle
		{
			get { return _directoryTitle; }
			set { _directoryTitle = value; }
		}

		public int Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public int Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public bool Scaling
		{
			get { return _scaling; }
			set { _scaling = value; }
		}

		public bool Scrolling
		{
			get { return _scrolling; }
			set { _scrolling = value; }
		}

		public bool Singleton
		{
			get { return _singleton; }
			set { _singleton = value; }
		}

		public string AuthorPhoto
		{
			get { return _authorPhoto; }
			set { _authorPhoto = value; }
		}

		public string AuthorAboutMe
		{
			get { return _authorAboutMe; }
			set { _authorAboutMe = value; }
		}

		public string AuthorLink
		{
			get { return _authorLink; }
			set { _authorLink = value; }
		}

		public string AuthorQuote
		{
			get { return _authorQuote; }
			set { _authorQuote = value; }
		}

        public string Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }
#if NET1
		public ModuleRequireCollection Requires
        {
			get { return _requires; }
			set { _requires = value; }
		}
#else
        public Discuz.Common.Generic.List<ModuleRequire> Requires
		{
			get { return _requires; }
			set { _requires = value; }
		}
#endif

	}
}
