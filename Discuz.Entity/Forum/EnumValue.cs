using System;

namespace Discuz.Entity
{
	/// <summary>
	/// EnumValue 的摘要说明。
	/// </summary>
	public class EnumValue
	{
		public EnumValue()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		public EnumValue(string value)
		{
			this._value = value;
		}

		private string _value = string.Empty;
		private string _displayValue = string.Empty;

		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public string DisplayValue
		{
			get { return _displayValue; }
			set { _displayValue = value; }
		}
	}
}
