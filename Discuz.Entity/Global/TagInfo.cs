using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Discuz.Entity
{
    /// <summary>
    /// 标签实体类
    /// </summary>
    [Serializable]
    public class TagInfo : IComparable<TagInfo>
    {
        public TagInfo()
        { }

        #region 私有字段
        private int _tagid;
        private string _tagname;
        private int _userid;
        private DateTime _postdatetime;
        private int _orderid;
        private string _color;
        private int _count;
        private int _fcount;
        private int _pcount;
        private int _scount;
        private int _vcount;
        private int _gcount;
        #endregion

        #region 属性
        /// <summary>
        /// TagID
        /// </summary>   
        [JsonPropertyAttribute("tagid")]
        public int Tagid
        {
            set { _tagid = value; }
            get { return _tagid; }
        }
        /// <summary>
        /// Tag名称
        /// </summary>
        [JsonPropertyAttribute("tagname")]
        public string Tagname
        {
            set { _tagname = value.Trim(); }
            get { return _tagname; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Userid
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime Postdatetime
        {
            set { _postdatetime = value; }
            get { return _postdatetime; }
        }
        /// <summary>
        /// 顺序ID
        /// </summary>
        public int Orderid
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 颜色值(6位html格式)
        /// </summary>
        public string Color
        {
            set { _color = value.Trim(); }
            get { return _color; }
        }
        /// <summary>
        /// 该Tag相关内容总数
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// 该Tag相关主题内容总数
        /// </summary>
        public int Fcount
        {
            set { _fcount = value; }
            get { return _fcount; }
        }
        /// <summary>
        /// 该Tag相关像册图片总数
        /// </summary>
        public int Pcount
        {
            set { _pcount = value; }
            get { return _pcount; }
        }
        /// <summary>
        /// 该Tag相关个人空间文章总数
        /// </summary>
        public int Scount
        {
            set { _scount = value; }
            get { return _scount; }
        }
        /// <summary>
        /// 该Tag相关视频总数
        /// </summary>
        public int Vcount
        {
            set { _vcount = value; }
            get { return _vcount; }
        }
         /// <summary>
        /// 该Tag相关商品总数
        /// </summary>
        public int Gcount
        {
            set { _gcount = value; }
            get { return _gcount; }
        }
        #endregion

        #region IComparable<TagInfo> 成员,调用Sort方法时使用

        public int CompareTo(TagInfo tag)
        {
            return this.Tagid.CompareTo(tag.Tagid);
        }

        #endregion
    }
}
