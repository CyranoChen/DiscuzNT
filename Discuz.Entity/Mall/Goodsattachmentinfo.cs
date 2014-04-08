using System;

namespace  Discuz.Entity
{
　　/// <summary>
	/// 商品附件的摘要说明。
	/// </summary>
	public class Goodsattachmentinfo : AttachmentInfo
　　{
　　　　private int _goodsid;//商品id
		/// <summary> 
		/// 商品id
		/// </summary>
		public int Goodsid
		{
			get { return _goodsid; }
			set { _goodsid = value; }
		}
		
　　　　private int _categoryid;//商品分类id
		/// <summary> 
		/// 商品分类id
		/// </summary>
		public int Categoryid
		{
			get { return _categoryid; }
			set { _categoryid = value; }
		}

        private int _goodscount;//分类的商品数
		/// <summary> 
        /// 分类的商品数
		/// </summary>
        public int Goodscount
		{
            get { return _goodscount; }
            set { _goodscount = value; }
		}
        
	}
}
