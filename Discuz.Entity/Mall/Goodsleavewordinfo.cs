using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// ��Ʒ������
    /// </summary>
    public class Goodsleavewordinfo
    {
        private int _id;//
        /// <summary> 
        /// ����id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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

        private int _tradelogid;//������־id
        /// <summary> 
        /// ������־id
        /// </summary>
        public int Tradelogid
        {
            get { return _tradelogid; }
            set { _tradelogid = value; }
        }

        private int _isbuyer;//���
        /// <summary> 
        /// ���
        /// </summary>
        public int Isbuyer
        {
            get { return _isbuyer; }
            set { _isbuyer = value; }
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

        private string _username;//�û���
        /// <summary> 
        /// �û���
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _message;//����
        /// <summary> 
        /// ����
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private int _invisible;//�Ƿ�ɼ�
        /// <summary> 
        /// �Ƿ�ɼ�
        /// </summary>
        public int Invisible
        {
            get { return _invisible; }
            set { _invisible = value; }
        }

        private string _ip;//IP��ַ
        /// <summary> 
        /// IP��ַ
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private int _usesig;//�Ƿ�����ǩ��
        /// <summary> 
        /// �Ƿ�����ǩ��
        /// </summary>
        public int Usesig
        {
            get { return _usesig; }
            set { _usesig = value; }
        }

        private int _htmlon;//�Ƿ�֧��html
        /// <summary> 
        /// �Ƿ�֧��html
        /// </summary>
        public int Htmlon
        {
            get { return _htmlon; }
            set { _htmlon = value; }
        }

        private int _smileyoff;//�Ƿ�ر�smaile����
        /// <summary> 
        /// �Ƿ�ر�smaile����
        /// </summary>
        public int Smileyoff
        {
            get { return _smileyoff; }
            set { _smileyoff = value; }
        }

        private int _parseurloff;//�Ƿ�ر�url�Զ�����
        /// <summary> 
        /// �Ƿ�ر�url�Զ�����
        /// </summary>
        public int Parseurloff
        {
            get { return _parseurloff; }
            set { _parseurloff = value; }
        }

        private int _bbcodeoff;//�Ƿ�����bbcode
        /// <summary> 
        /// �Ƿ�����bbcode
        /// </summary>
        public int Bbcodeoff
        {
            get { return _bbcodeoff; }
            set { _bbcodeoff = value; }
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
		
    }
}
