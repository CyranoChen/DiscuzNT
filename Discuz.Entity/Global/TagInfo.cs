using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Entity
{
    /// <summary>
    /// ��ǩʵ����
    /// </summary>
    [Serializable]
    public class TagInfo : IComparable<TagInfo>
    {
        public TagInfo()
        { }

        #region ˽���ֶ�
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

        #region ����
        /// <summary>
        /// TagID
        /// </summary>   
        //[JsonPropertyAttribute("tagid")]
        public int Tagid
        {
            set { _tagid = value; }
            get { return _tagid; }
        }
        /// <summary>
        /// Tag����
        /// </summary>
        //[JsonPropertyAttribute("tagname")]
        public string Tagname
        {
            set { _tagname = value.Trim(); }
            get { return _tagname; }
        }
        /// <summary>
        /// �û�ID
        /// </summary>
        public int Userid
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// �ύʱ��
        /// </summary>
        public DateTime Postdatetime
        {
            set { _postdatetime = value; }
            get { return _postdatetime; }
        }
        /// <summary>
        /// ˳��ID
        /// </summary>
        public int Orderid
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// ��ɫֵ(6λhtml��ʽ)
        /// </summary>
        public string Color
        {
            set { _color = value.Trim(); }
            get { return _color; }
        }
        /// <summary>
        /// ��Tag�����������
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// ��Tag���������������
        /// </summary>
        public int Fcount
        {
            set { _fcount = value; }
            get { return _fcount; }
        }
        /// <summary>
        /// ��Tag������ͼƬ����
        /// </summary>
        public int Pcount
        {
            set { _pcount = value; }
            get { return _pcount; }
        }
        /// <summary>
        /// ��Tag��ظ��˿ռ���������
        /// </summary>
        public int Scount
        {
            set { _scount = value; }
            get { return _scount; }
        }
        /// <summary>
        /// ��Tag�����Ƶ����
        /// </summary>
        public int Vcount
        {
            set { _vcount = value; }
            get { return _vcount; }
        }
         /// <summary>
        /// ��Tag�����Ʒ����
        /// </summary>
        public int Gcount
        {
            set { _gcount = value; }
            get { return _gcount; }
        }
        #endregion

        #region IComparable<TagInfo> ��Ա,����Sort����ʱʹ��

        public int CompareTo(TagInfo tag)
        {
            return this.Tagid.CompareTo(tag.Tagid);
        }

        #endregion
    }
}
