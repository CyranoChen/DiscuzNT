using System;

namespace Discuz.Entity
{
	/// <summary>
	/// EnumValue ��ժҪ˵����
	/// </summary>
	public class EnumValue
	{
		public EnumValue()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
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
