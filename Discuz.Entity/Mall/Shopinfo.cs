using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ������Ϣ��
    /// </summary>
    public class Shopinfo
    {
        private int _shopid;//����id
        /// <summary> 
        /// ����id
        /// </summary>
        public int Shopid
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        private string _logo= "";//���
        /// <summary> 
        /// ���
        /// </summary>
        public string Logo
        {
            get { return _logo.Trim(); }
            set { _logo = value.Trim(); }
        }

        private string _shopname;//��������
        /// <summary> 
        /// ��������
        /// </summary>
        public string Shopname
        {
            get { return _shopname; }
            set { _shopname = value; }
        }

        private int _themeid;//����
        /// <summary> 
        /// ����
        /// </summary>
        public int Themeid
        {
            get { return _themeid; }
            set { _themeid = value; }
        }

        private string _themepath;//����·��
        /// <summary> 
        /// ����·��
        /// </summary>
        public string Themepath
        {
            get { return _themepath; }
            set { _themepath = value; }
        }

        private int _uid;//�û�(�ƹ�)id
        /// <summary> 
        /// �û�(�ƹ�)id
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private string _username;//�û�(�ƹ�)��
        /// <summary> 
        /// �û�(�ƹ�)��
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _introduce;//���̽���
        /// <summary> 
        /// ���̽���
        /// </summary>
        public string Introduce
        {
            get { return _introduce; }
            set { _introduce = value; }
        }

        private int _lid;//���ڵ�id
        /// <summary> 
        /// ���ڵ�id
        /// </summary>
        public int Lid
        {
            get { return _lid; }
            set { _lid = value; }
        }

        private string _locus;//���ڵ�
        /// <summary> 
        /// ���ڵ�
        /// </summary>
        public string Locus
        {
            get { return _locus; }
            set { _locus = value; }
        }

        private string _bulletin;//����
        /// <summary> 
        /// ����
        /// </summary>
        public string Bulletin
        {
            get { return _bulletin; }
            set { _bulletin = value; }
        }

        private DateTime _createdatetime;//����ʱ��
        /// <summary> 
        /// ����ʱ��
        /// </summary>
        public DateTime Createdatetime
        {
            get { return _createdatetime; }
            set { _createdatetime = value; }
        }

        private int _invisible;//�Ƿ�ɼ�(0:�ɼ�, 1:���ɼ�)
        /// <summary> 
        /// �Ƿ�ɼ�(0:�ɼ�, 1:���ɼ�)
        /// </summary>
        public int Invisible
        {
            get { return _invisible; }
            set { _invisible = value; }
        }

        private int _viewcount;//������
        /// <summary> 
        /// ������
        /// </summary>
        public int Viewcount
        {
            get { return _viewcount; }
            set { _viewcount = value; }
        }
    }
}
