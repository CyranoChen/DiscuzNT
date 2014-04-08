using System;
using System.Text;

namespace Discuz.Entity
{
    public class MySqlInfo
    {
        private string _tablename;
        private string _tabletype;
        private string _rowcount;
        private string _tabledata;
        private string _index;
        private string _datafree;

        public string tablename

        { set { _tablename=value;}
        
         get{return _tablename;}
        }

        public string tabletype
        {
            set { _tabletype = value; }
            get { return _tabletype; }
        
        }

        public string rowcount
        {

            set { _rowcount = value; }
            get { return _rowcount; }
        }

        public string index
        {
            set { _index = value; }
            get { return _index; }
        }

        public string datafree
        {
            set { _datafree = value; }
            get { return _datafree; }
        
        }
        public string tabledata
        {
            set { _tabledata = value; }
            get { return _tabledata; }
        
        }
    }
}
