using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ��������������Ϣ��
    /// </summary>
    public class Shoplinkinfo
    {
        private int _id;//������������id
        /// <summary> 
        /// ������������id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _shopid;//����id
        /// <summary> 
        /// ����id
        /// </summary>
        public int Shopid
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        private int _displayorder;//��ʾ˳��
        /// <summary> 
        /// ��ʾ˳��
        /// </summary>
        public int Displayorder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

        private string _name = "";//����
        /// <summary> 
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name.Trim(); }
            set { _name = value.Trim(); }
        }

        private int _linkshopid;//���ӵ���shopid��Ϣ
        /// <summary> 
        /// ���ӵ���shopid��Ϣ
        /// </summary>
        public int Linkshopid
        {
            get { return _linkshopid; }
            set { _linkshopid = value; }
        }

       
    }
}
