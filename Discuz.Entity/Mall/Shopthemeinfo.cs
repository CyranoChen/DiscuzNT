using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ����������Ϣ��
    /// </summary>
    public class Shopthemeinfo
    {
        private int _themeid;//����id
        /// <summary> 
        /// ����id
        /// </summary>
        public int Themeid
        {
            get { return _themeid; }
            set { _themeid = value; }
        }

        private string _directory;//·��
        /// <summary> 
        /// ·��
        /// </summary>
        public string Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        private string _name;//����
        /// <summary> 
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _author;//����
        /// <summary> 
        /// ����
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        private string _createdate;//����ʱ��
        /// <summary> 
        /// ����ʱ��
        /// </summary>
        public string Createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

        private string _copyright;//��Ȩ��Ϣ
        /// <summary> 
        /// ��Ȩ��Ϣ
        /// </summary>
        public string Copyright
        {
            get { return _copyright; }
            set { _copyright = value; }
        }
		
    }
}
