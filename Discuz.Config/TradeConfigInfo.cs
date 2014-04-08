using System;
using System.Text;

namespace Discuz.Config
{
    
    /// <summary>
    /// ����������Ϣ��
    /// </summary>
    [Serializable]
    public class TradeConfigInfo : IConfigInfo
    {
        /// <summary>
        /// ֧����������Ϣ
        /// </summary>
        private AliPayConfigInfo _alipayconfiginfo;
        /// <summary>
        /// ֧����������Ϣ
        /// </summary>
        public AliPayConfigInfo Alipayconfiginfo
        {
            get {return _alipayconfiginfo;}
            set { _alipayconfiginfo = value; }
        }
    }

     
    /// <summary>
    /// ֧����������Ϣ��
    /// </summary>
    [Serializable]
    public class AliPayConfigInfo 
    {
        #region ˽���ֶ�
        private string _inputCharset = "utf-8";
        private string _partner = "2088002052150939"; //Discuz Partner ID
        private string _sign = "gh0bis45h89m5mwcoe85us4qrwispes0"; //Discuz ���װ�ȫУ����(key)
        private string _acount = ""; //֧�����ʺ�(���ڶһ����׹���)
        #endregion

        #region ����
        /// <summary>
        /// ���������ַ���,Ĭ��Ϊ"utf-8"(Alipay�ĵ�Ĭ��ֵΪ"GBK")
        /// </summary>
        public string Inputcharset
        {
            get
            {
                if (_inputCharset == null)
                {
                    return "utf-8";
                }
                else
                {
                    return _inputCharset;
                }
            }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("��Ч�� Input_Charset(���������ַ���)", value, value.ToString());
                }
                _inputCharset = value;
            }
        }

        #region ע�͵Ĵ���
        //private string _agent = "";

        ///// <summary>
        ///// ���һЩ������վ�Ľ��ף���һ���ġ�����������ϵ�������̿����ڽ����д��ݸò������������������ݡ����ﴫ�͵�ֵ����ʹ�ô���������֧�����˻���PartnerID
        ///// </summary>
        //public string Agent
        //{
        //    get { return _agent; }
        //    set
        //    {
        //        if (value != null && value.Length > 16)
        //            throw new ArgumentOutOfRangeException("��Ч�� Agent(����)", value, value.ToString());
        //        _agent = value;
        //    }
        //}
        #endregion

        /// <summary>
        /// ���������֧�������û�ID
        /// Alipay�ĵ�����:string(16)
        /// </summary>
        public string Partner
        {
            get
            {
                if (_partner == null)
                {
                    throw new ArgumentNullException(_partner);
                }
                return _partner;
            }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("��Ч�� Partner(�������ID)", value, value.ToString());
                _partner = value;
            }
        }


        /// <summary>
        /// ǩ��,�ڴ����뽻�װ�ȫУ���루key������ͬ��֧�����ĵ���Sign
        /// </summary>
        public string Sign
        {
            get
            {
                if (_sign == null)
                {
                    throw new ArgumentNullException(_sign);
                }
                return _sign;
            }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("��Ч�� Sign(ǩ��)", value, value.ToString());
                _sign = value;
            }
        }

        /// <summary>
        /// ֧�����ʺ�(���ڶһ����׹���)
        /// </summary>
        public string Acount
        {
            get
            {
                if (_acount == null)
                {
                    throw new ArgumentNullException(_acount);
                }
                return _acount;
            }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("��Ч��֧�����ʺ�", value, value.ToString());
                _acount = value;
            }
        }
        #endregion
    }
}
