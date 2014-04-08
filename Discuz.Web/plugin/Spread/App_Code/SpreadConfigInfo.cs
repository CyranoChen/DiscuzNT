using System;
using Discuz.Config;

namespace Discuz.Plugin.Spread.Config
{
	/// <summary>
	/// spreadinfo 的摘要说明。
	/// </summary>
	[Serializable]
    public class SpreadConfigInfo : IConfigInfo
	{


        private string _transferUrl;
		/// <summary>
		/// 转向地址
		/// </summary>
		public string TransferUrl
		{
            get { return _transferUrl; }
            set { _transferUrl = value; }
		}

        private string _spreadCredits;
		/// <summary>
		/// 推广奖励
		/// </summary>
		public string SpreadCredits
		{
            get { return _spreadCredits; }
            set { _spreadCredits = value; }
		}
	
	}
}
