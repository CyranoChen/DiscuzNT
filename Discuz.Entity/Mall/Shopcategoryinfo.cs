using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ���̷�����Ϣ��
    /// </summary>
    public class Shopcategoryinfo
    {
        private int _categoryid;//����id
        /// <summary> 
        /// ����id
        /// </summary>
        public int Categoryid
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        private int _parentid;//������id
        /// <summary> 
        /// ������id
        /// </summary>
        public int Parentid
        {
            get { return _parentid; }
            set { _parentid = value; }
        }

        private string _parentidlist = "";//������id�б�
        /// <summary> 
        /// ������id�б�
        /// </summary>
        public string Parentidlist
        {
            get { return _parentidlist.Trim(); }
            set { _parentidlist = value.Trim(); }
        }

        private int _layer;//����
        /// <summary> 
        /// ����
        /// </summary>
        public int Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        private int _childcount;//�ӷ�����
        /// <summary> 
        /// �ӷ�����
        /// </summary>
        public int Childcount
        {
            get { return _childcount; }
            set { _childcount = value; }
        }
        

        private int _syscategoryid;//ϵͳ����id
        /// <summary> 
        /// ϵͳ����id
        /// </summary>
        public int Syscategoryid
        {
            get { return _syscategoryid; }
            set { _syscategoryid = value; }
        }

        private string _name = "";//��������
        /// <summary> 
        /// ��������
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

        private int _shopid;//��������id
        /// <summary> 
        /// ��������id
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

    }
}
