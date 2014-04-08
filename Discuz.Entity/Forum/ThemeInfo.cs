using System;

namespace Discuz.Entity
{
	/// <summary>
	/// SpaceThemeInfo ��ժҪ˵����
	/// </summary>
    [Serializable]
	public class ThemeInfo
	{
		public ThemeInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		private int _themeId;
        /// <summary>
        /// �ռ�Ƥ��ID
        /// </summary>
		public int ThemeId
		{
			get { return _themeId; }
			set { _themeId = value; }
		}

		private string _directory;
        /// <summary>
        /// �����ļ���
        /// </summary>
		public string Directory
		{
			get { return _directory; }
			set { _directory = value; }
		}

		private string _name;
        /// <summary>
        /// ����
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
        /// ����
        /// </summary>
		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		private string _createDate;
        /// <summary>
        /// ����ʱ��
        /// </summary>
		public string CreateDate
		{
			get { return _createDate; }
			set { _createDate = value; }
		}

		private string _copyRight;
        /// <summary>
        /// ��Ȩ
        /// </summary>
		public string CopyRight
		{
			get { return _copyRight; }
			set { _copyRight = value; }
		}
	}
}
