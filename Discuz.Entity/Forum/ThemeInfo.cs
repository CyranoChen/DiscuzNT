using System;

namespace Discuz.Entity
{
	/// <summary>
	/// SpaceThemeInfo 的摘要说明。
	/// </summary>
    [Serializable]
	public class ThemeInfo
	{
		public ThemeInfo()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		private int _themeId;
        /// <summary>
        /// 空间皮肤ID
        /// </summary>
		public int ThemeId
		{
			get { return _themeId; }
			set { _themeId = value; }
		}

		private string _directory;
        /// <summary>
        /// 所在文件夹
        /// </summary>
		public string Directory
		{
			get { return _directory; }
			set { _directory = value; }
		}

		private string _name;
        /// <summary>
        /// 名称
        /// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private int _type;
		public int Type
		{
			get { return _type; }
			set { _type = value; }
		}

		private string _author;
        /// <summary>
        /// 作者
        /// </summary>
		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		private string _createDate;
        /// <summary>
        /// 创建时间
        /// </summary>
		public string CreateDate
		{
			get { return _createDate; }
			set { _createDate = value; }
		}

		private string _copyRight;
        /// <summary>
        /// 版权
        /// </summary>
		public string CopyRight
		{
			get { return _copyRight; }
			set { _copyRight = value; }
		}
	}
}
