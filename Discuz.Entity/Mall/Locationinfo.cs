using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ���ڵ���Ϣ��
    /// </summary>
    public class Locationinfo
    {
        private int _lid;//
        /// <summary> 
        /// ���ڵ�id
        /// </summary>
        public int Lid
        {
            get { return _lid; }
            set { _lid = value; }
        }

        private string _city;//����
        /// <summary> 
        /// ����
        /// </summary>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _state;//ʡ,��
        /// <summary> 
        /// ʡ,��
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        private string _country;//����
        /// <summary> 
        /// ����
        /// </summary>
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        private string _zipcode;//�ʱ�
        /// <summary> 
        /// �ʱ�
        /// </summary>
        public string Zipcode
        {
            get { return _zipcode; }
            set { _zipcode = value; }
        }
    }
}