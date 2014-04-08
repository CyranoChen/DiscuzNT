using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    [Serializable]
    public class IpInfo
    {
        private int _id=0;
        private int _ip1=0;
        private int _ip2=0;
        private int _ip3=0;
        private int _ip4=0;
        private string _username="";
        private string _dateline="";
        private string _expiration="";
        private string _location = "";

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Ip1
        {
            get { return _ip1; }
            set { _ip1=value;}
        }

        public int Ip2
        {
            get { return _ip2; }
            set { _ip2 = value; }
        }

        public int Ip3
        {
            get { return _ip3; }
            set { _ip3 = value; }
        }

        public int Ip4
        {
            get { return _ip4; }
            set { _ip4 = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Dateline
        {
            get { return _dateline; }
            set{_dateline=value;}
        }

        public string Expiration
        {
            get { return _expiration; }
            set { _expiration = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
    }
}
