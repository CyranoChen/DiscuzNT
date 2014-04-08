using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 商品分类信息类
    /// </summary>
    public class Goodscategoryinfo
    {
		private int _categoryid;
		public int Categoryid
		{
			get { return _categoryid; }
			set { _categoryid = value; }
		}
		
　　　　private int _parentid;//父id
		/// <summary> 
		/// 父id
		/// </summary>
		public int Parentid
		{
			get { return _parentid; }
			set { _parentid = value; }
		}
		
　　　　private int _layer;//所在层数
		/// <summary> 
		/// 所在层数
		/// </summary>
		public int Layer
		{
			get { return _layer; }
			set { _layer = value; }
		}
		
　　　　private string _parentidlist;//父结点字符串
		/// <summary> 
		/// 父结点字符串
		/// </summary>
		public string Parentidlist
		{
			get { return _parentidlist; }
			set { _parentidlist = value; }
		}

        private int m_displayorder;//显示顺序
        ///<summary>
        ///显示顺序
        ///</summary>
        public int Displayorder
        {
            get { return m_displayorder; }
            set { m_displayorder = value; }
        }
		
　　　　private string _categoryname = "";//分类名称
		/// <summary> 
		/// 分类名称
		/// </summary>
		public string Categoryname
		{
			get { return _categoryname; }
			set { _categoryname = value.Trim(); }
		}
		
　　　　private int _haschild;//是否有子结点
		/// <summary> 
		/// 是否有子结点
		/// </summary>
		public int Haschild
		{
			get { return _haschild; }
			set { _haschild = value; }
		}
		
　　　　private int _fid;//版块ID
		/// <summary> 
		/// 版块ID
		/// </summary>
		public int Fid
		{
			get { return _fid; }
			set { _fid = value; }
		}

        private string _pathlist;//链接路径
		/// <summary> 
		/// 链接路径
		/// </summary>
		public string Pathlist
		{
			get { return _pathlist; }
			set { _pathlist = value; }
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
