using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ��Ʒ�û�������Ϣ��
    /// </summary>
    public class Goodsusercreditinfo
    {
        private int _id;
        /// <summary> 
        /// 
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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

      
        private int _oneweek;//һ����
        /// <summary> 
        /// һ����
        /// </summary>
        public int Oneweek
        {
            get { return _oneweek; }
            set { _oneweek = value; }
        }

        private int _onemonth;//һ������
        /// <summary> 
        /// һ������
        /// </summary>
        public int Onemonth
        {
            get { return _onemonth; }
            set { _onemonth = value; }
        }

        private int _sixmonth;//��������
        /// <summary> 
        /// ��������
        /// </summary>
        public int Sixmonth
        {
            get { return _sixmonth; }
            set { _sixmonth = value; }
        }

        private int _sixmonthago;//��������ǰ
        /// <summary> 
        /// ��������ǰ
        /// </summary>
        public int Sixmonthago
        {
            get { return _sixmonthago; }
            set { _sixmonthago = value; }
        }

        private int _ratefrom;//�������� 1:���� 2:���
        /// <summary> 
        /// �������� 1:���� 2:���
        /// </summary>
        public int Ratefrom
        {
            get { return _ratefrom; }
            set { _ratefrom = value; }
        }

        private int _ratetype;//�������� 1:���� 2:���� 3:���� 
        /// <summary> 
        ///�������� 1:���� 2:���� 3:���� 
        /// </summary>
        public int Ratetype
        {
            get { return _ratetype; }
            set { _ratetype = value; }
        }


    }
}
