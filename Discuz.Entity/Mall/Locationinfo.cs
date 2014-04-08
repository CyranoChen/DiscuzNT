using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 所在地信息类
    /// </summary>
    public class Locationinfo
    {
        private int _lid;//
        /// <summary> 
        /// 所在地id
        /// </summary>
        public int Lid
        {
            get { return _lid; }
            set { _lid = value; }
        }

        private string _city;//城市
        /// <summary> 
        /// 城市
        /// </summary>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _state;//省,州
        /// <summary> 
        /// 省,州
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        private string _country;//国家
        /// <summary> 
        /// 国家
        /// </summary>
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        private string _zipcode;//邮编
        /// <summary> 
        /// 邮编
        /// </summary>
        public string Zipcode
        {
            get { return _zipcode; }
            set { _zipcode = value; }
        }
    }
}