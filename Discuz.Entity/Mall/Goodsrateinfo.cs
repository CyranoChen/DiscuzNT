using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ��Ʒ������
    /// </summary>
    public class Goodsrateinfo
    {
        private int _id;
        /// <summary> 
        /// ����id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _goodstradelogid;//��Ʒ������־ID
        /// <summary> 
        /// ��Ʒ������־ID
        /// </summary>
        public int Goodstradelogid
        {
            get { return _goodstradelogid; }
            set { _goodstradelogid = value; }
        }

        private string _message;//��������
        /// <summary> 
        /// ��������
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _explain;//��������
        /// <summary> 
        /// ��������
        /// </summary>
        public string Explain
        {
            get { return _explain; }
            set { _explain = value; }
        }

        private string _ip;//ip
        /// <summary> 
        /// ip
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private int _uid;//�û�id
        /// <summary> 
        /// �û�id
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private int _uidtype;//�û�id���� 1:���� 2:���
        /// <summary> 
        /// �û�id���� 1:���� 2:���
        /// </summary>
        public int Uidtype
        {
            get { return _uidtype; }
            set { _uidtype = value; }
        }


        private int _ratetouid;//�������˵�uid
        /// <summary> 
        /// �������˵�uid
        /// </summary>
        public int Ratetouid
        {
            get { return _ratetouid; }
            set { _ratetouid = value; }
        }


        private string _ratetousername;//�������˵��û���
        /// <summary> 
        /// �������˵��û���
        /// </summary>
        public string Ratetousername
        {
            get { return _ratetousername; }
            set { _ratetousername = value; }
        }
        

        private string _username;//�û���
        /// <summary> 
        /// �û���
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private DateTime _postdatetime;//��������
        /// <summary> 
        /// ��������
        /// </summary>
        public DateTime Postdatetime
        {
            get { return _postdatetime; }
            set { _postdatetime = value; }
        }

        private int _goodsid;//��Ʒid
        /// <summary> 
        /// ��Ʒid
        /// </summary>
        public int Goodsid
        {
            get { return _goodsid; }
            set { _goodsid = value; }
        }

        private string _goodstitle;//��Ʒ����
        /// <summary> 
        /// ��Ʒ����
        /// </summary>
        public string Goodstitle
        {
            get { return _goodstitle; }
            set { _goodstitle = value; }
        }

        private decimal _price;//���׼۸�
        /// <summary> 
        /// ���׼۸�
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _ratetype;//�������� 1:���� 2:���� 3:���� 
        /// <summary> 
        /// �������� 1:���� 2:���� 3:���� 
        /// </summary>
        public int Ratetype
        {
            get { return _ratetype; }
            set { _ratetype = value; }
        }
    }
}
