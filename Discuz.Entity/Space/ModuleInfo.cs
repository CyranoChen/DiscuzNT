using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ModuleInfo ��ժҪ˵����
	/// </summary>
	public class ModuleInfo
	{
		public ModuleInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

        /// <summary>
        /// �ռ�ģ��ID
        /// </summary>
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

        /// <summary>
        /// ��ǩID
        /// </summary>
		private int _tabID;
		public int TabID
		{
			get
			{
				return _tabID;
			}
			set
			{
				_tabID = value;
			}
		}

        /// <summary>
        /// �û�ID
        /// </summary>
		private int _uid;
		public int Uid
		{
			get
			{
				return _uid;
			}
			set
			{
				_uid = value;
			}
		}

		private int _moduleDefID;
		public int ModuleDefID
		{
			get
			{
				return _moduleDefID;
			}
			set
			{
				_moduleDefID = value;
			}
		}

		private string _paneName;
		public string PaneName
		{
			get
			{
				return _paneName;
			}
			set
			{
				_paneName = value;
			}
		}

		private int _displayOrder;
		public int DisplayOrder
		{
			get
			{
				return _displayOrder;
			}
			set
			{
				_displayOrder = value;
			}
		}

		private string _modulePref;
		public string UserPref
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

		private int _val = 0;
		public int Val
		{
			get { return _val; }
			set { _val = value; }
		}

		private string _moduleurl = string.Empty;
		public string ModuleUrl
		{
			get { return _moduleurl; }
			set { _moduleurl = value; }
		}

		private ModuleType _moduletype;
		public ModuleType ModuleType
		{
			get
			{
				return _moduletype;
			}
			set
			{
				_moduletype = value;
			}
		}


	}
}
