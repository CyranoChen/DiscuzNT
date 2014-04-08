using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 店铺分类信息类
    /// </summary>
    public class Shopcategoryinfo
    {
        private int _categoryid;//分类id
        /// <summary> 
        /// 分类id
        /// </summary>
        public int Categoryid
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        private int _parentid;//父分类id
        /// <summary> 
        /// 父分类id
        /// </summary>
        public int Parentid
        {
            get { return _parentid; }
            set { _parentid = value; }
        }

        private string _parentidlist = "";//父分类id列表
        /// <summary> 
        /// 父分类id列表
        /// </summary>
        public string Parentidlist
        {
            get { return _parentidlist.Trim(); }
            set { _parentidlist = value.Trim(); }
        }

        private int _layer;//层数
        /// <summary> 
        /// 层数
        /// </summary>
        public int Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        private int _childcount;//子分类数
        /// <summary> 
        /// 子分类数
        /// </summary>
        public int Childcount
        {
            get { return _childcount; }
            set { _childcount = value; }
        }
        

        private int _syscategoryid;//系统分类id
        /// <summary> 
        /// 系统分类id
        /// </summary>
        public int Syscategoryid
        {
            get { return _syscategoryid; }
            set { _syscategoryid = value; }
        }

        private string _name = "";//分类名称
        /// <summary> 
        /// 分类名称
        /// </summary>
        public string Name
        {
            get { return _name.Trim(); }
            set { _name = value.Trim(); }
        }

        private string _categorypic = "";//
        /// <summary> 
        /// 
        /// </summary>
        public string Categorypic
        {
            get { return _categorypic; }
            set { _categorypic = value; }
        }

        private int _shopid;//所属店铺id
        /// <summary> 
        /// 所属店铺id
        /// </summary>
        public int Shopid
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        private int _displayorder;//显示顺序
        /// <summary> 
        /// 显示顺序
        /// </summary>
        public int Displayorder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

    }
}
