using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// �����
    /// </summary>
    public class AdShowInfo
    {
        private int _advid = 0;
        private int _displayorder = -1;
        private string _code = "��������";
        //�������|��Դ��ַ|���|�߶�|���ӵ�ַ|˵������|���Ͷ��λ��|�����ʾλ��
        private string _parameters = "|||||||";


        /// <summary>
        /// ���id
        /// </summary>
        public int Advid
        {
            get { return _advid; }
            set { _advid = value; }
        }

        /// <summary>
        /// ��ʾ˳��
        /// </summary>
        public int Displayorder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

        /// <summary>
        /// ������ݴ���
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
    }
}
