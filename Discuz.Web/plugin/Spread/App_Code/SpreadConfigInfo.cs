using System;
using Discuz.Config;

namespace Discuz.Plugin.Spread.Config
{
	/// <summary>
	/// spreadinfo ��ժҪ˵����
	/// </summary>
	[Serializable]
    public class SpreadConfigInfo : IConfigInfo
	{


        private string _transferUrl;
		/// <summary>
		/// ת���ַ
		/// </summary>
		public string TransferUrl
		{
            get { return _transferUrl; }
            set { _transferUrl = value; }
		}

        private string _spreadCredits;
		/// <summary>
		/// �ƹ㽱��
		/// </summary>
		public string SpreadCredits
		{
            get { return _spreadCredits; }
            set { _spreadCredits = value; }
		}
	
	}
}
