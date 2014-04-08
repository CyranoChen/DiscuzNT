using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ��Ʒ��Ϣ����,�̳��� <see cref="Discuz.Entity.PostpramsInfo"/> PostpramsInfo
    /// </summary>
    public class GoodspramsInfo : PostpramsInfo
    {
        public int __goodsid;

        /// <summary>
        /// ��Ʒid
        /// </summary>
        public int Goodsid
        {
            set { __goodsid = value; }
            get { return __goodsid; }
        }
    }

    /// <summary>
    /// ��Ʒ��Ϣ��ժҪ˵����
    /// </summary>
    public class Goodsinfo
    {
        private int _goodsid;//��ƷID
        /// <summary> 
        /// ��ƷID
        /// </summary>
        public int Goodsid
        {
            get { return _goodsid; }
            set { _goodsid = value; }
        }

        private int _shopid;//����ID
        /// <summary> 
        /// ����ID
        /// </summary>
        public int Shopid
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        private int _categoryid;//��Ʒ����
        /// <summary> 
        /// ��Ʒ����
        /// </summary>
        public int Categoryid
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        private string _parentcategorylist;//��Ʒ���������б�,����categories���е�parentidlist�ֶ�
        /// <summary> 
        /// ��Ʒ���������б�,����categories���е�parentidlist�ֶ�
        /// </summary>
        public string Parentcategorylist
        {
            get { return _parentcategorylist; }
            set { _parentcategorylist = value; }
        }

        private string _shopcategorylist = "";// ������Ʒ����. ��ʽ(1,3,4)
        /// <summary> 
        /// ������Ʒ����. ��ʽ(1,3,4)
        /// </summary>
        public string Shopcategorylist
        {
            get { return _shopcategorylist; }
            set { _shopcategorylist = value; }
        }

        private int _recommend;//�Ƿ����Ƽ���Ʒ(0Ϊ���Ƽ�,)
        /// <summary> 
        /// �Ƿ����Ƽ���Ʒ(0Ϊ���Ƽ�,)
        /// </summary>
        public int Recommend
        {
            get { return _recommend; }
            set { _recommend = value; }
        }

        private int _discount;//�ۿ�
        /// <summary> 
        /// �ۿ�
        /// </summary>
        public int Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        private int _selleruid;//����uid
        /// <summary> 
        /// ����uid
        /// </summary>
        public int Selleruid
        {
            get { return _selleruid; }
            set { _selleruid = value; }
        }

        private string _seller;//�����ʺ�(��Ӧ֧����)
        /// <summary> 
        /// �����ʺ�(��Ӧ֧����)
        /// </summary>
        public string Seller
        {
            get { return _seller; }
            set { _seller = value; }
        }

        private string _account;//�����ʺ�(��Ӧ֧����)
        /// <summary> 
        /// �����ʺ�(��Ӧ֧����)
        /// </summary>
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private string _title = "";//����(��Ӧ֧����:subject)
        /// <summary> 
        /// ����(��Ӧ֧����:subject)
        /// </summary>
        public string Title
        {
            get { return _title.Trim(); }
            set { _title = value.Trim(); }
        }

        private int _magic;//
        /// <summary> 
        /// 
        /// </summary>
        public int Magic
        {
            get { return _magic; }
            set { _magic = value; }
        }

        private decimal _price;//�۸�(�ּ�)
        /// <summary> 
        /// �۸�(�ּ�)
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _amount;//����
        /// <summary> 
        /// ����
        /// </summary>
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private int _quality;//��ɫ(����)(1: ȫ��, 2:����)
        /// <summary> 
        /// ��ɫ(����)(1: ȫ��, 2:����)
        /// </summary>
        public int Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private int _lid;//����locations���е�id
        /// <summary> 
        /// ����locations���е�id
        /// </summary>
        public int Lid
        {
            get { return _lid; }
            set { _lid = value; }
        }

        private string _locus;//���ڵ�,��ʽΪ"(����),ʡ(��),��"
        /// <summary> 
        /// ���ڵ�,��ʽΪ"(����),ʡ(��),��"
        /// </summary>
        public string Locus
        {
            get { return _locus; }
            set { _locus = value; }
        }

        private int _transport;//������ʽ
        /// <summary> 
        /// ������ʽ
        /// ����: 0 ������Ʒ�������ʵ�
        ///       1 ���ҳе��˷�
        ///       2 ��ҳе��˷�  
        /// 3 ֧����������˾
        /// </summary>
        public int Transport
        {
            get { return _transport; }
            set { _transport = value; }
        }

        private decimal _ordinaryfee;//ƽ�ʸ��ӷ�
        /// <summary> 
        /// ƽ�ʸ��ӷ�
        /// </summary>
        public decimal Ordinaryfee
        {
            get { return _ordinaryfee; }
            set { _ordinaryfee = value; }
        }

        private decimal _expressfee;//��ݸ��ӷ�
        /// <summary> 
        /// ��ݸ��ӷ�
        /// </summary>
        public decimal Expressfee
        {
            get { return _expressfee; }
            set { _expressfee = value; }
        }

        private decimal _emsfee;//EMS���ӷ�
        /// <summary> 
        /// EMS���ӷ�
        /// </summary>
        public decimal Emsfee
        {
            get { return _emsfee; }
            set { _emsfee = value; }
        }

        private int _itemtype;//��Ʒ����(��Ʒ,����,����,����,�ʷ�,����)
        /// <summary> 
        /// ��Ʒ����(��Ʒ,����,����,����,�ʷ�,����)
        /// </summary>
        public int Itemtype
        {
            get { return _itemtype; }
            set { _itemtype = value; }
        }

        private DateTime _dateline;//��ʼʱ��
        /// <summary> 
        /// ��ʼʱ��
        /// </summary>
        public DateTime Dateline
        {
            get { return _dateline; }
            set { _dateline = value; }
        }

        private DateTime _expiration;//����ʱ��
        /// <summary> 
        /// ����ʱ��
        /// </summary>
        public DateTime Expiration
        {
            get { return _expiration; }
            set { _expiration = value; }
        }

        private string _lastbuyer = "";//�������û���
        /// <summary> 
        /// �������û���
        /// </summary>
        public string Lastbuyer
        {
            get { return _lastbuyer; }
            set { _lastbuyer = value.Trim(); }
        }

        private DateTime _lasttrade;//�����ʱ��
        /// <summary> 
        /// �����ʱ��
        /// </summary>
        public DateTime Lasttrade
        {
            get { return _lasttrade; }
            set { _lasttrade = value; }
        }

        private DateTime _lastupdate;//������ʱ��
        /// <summary> 
        /// ������ʱ��
        /// </summary>
        public DateTime Lastupdate
        {
            get { return _lastupdate; }
            set { _lastupdate = value; }
        }

        private int _totalitems;//�ܽ�����
        /// <summary> 
        /// �ܽ�����
        /// </summary>
        public int Totalitems
        {
            get { return _totalitems; }
            set { _totalitems = value; }
        }

        private decimal _tradesum;//�ܽ��׶�
        /// <summary> 
        /// �ܽ��׶�
        /// </summary>
        public decimal Tradesum
        {
            get { return _tradesum; }
            set { _tradesum = value; }
        }

        private int _closed;//�Ƿ�ر�
        /// <summary> 
        /// �Ƿ�ر� 1:Ϊ�ر� 0: ���ر�
        /// </summary>
        public int Closed
        {
            get { return _closed; }
            set { _closed = value; }
        }

        private int _aid;//��ƷͼƬ��aid
        /// <summary> 
        /// ��ƷͼƬ��aid
        /// </summary>
        public int Aid
        {
            get { return _aid; }
            set { _aid = value; }
        }

        private string _goodspic = "";//��ƷͼƬ·��(����aid)
        /// <summary> 
        /// ��ƷͼƬ·��(����aid)
        /// </summary>
        public string Goodspic
        {
            get { return _goodspic; }
            set { _goodspic = value; }
        }

        private int _displayorder;//��ʾ˳�� 0Ϊ���� <0����ʾ   -1Ϊ����վ   -2�����  -3δ�ϼ�(���ɼ�)
        /// <summary> 
        /// ��ʾ˳�� 0Ϊ���� <0����ʾ   -1Ϊ����վ   -2�����  -3δ�ϼ�(���ɼ�)
        /// </summary>
        public int Displayorder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

        private decimal _costprice;//��Ʒԭ��
        /// <summary> 
        /// ��Ʒԭ��
        /// </summary>
        public decimal Costprice
        {
            get { return _costprice; }
            set { _costprice = value; }
        }

        private int _invoice;//���޷�Ʊ(0:��,1:��)
        /// <summary> 
        /// ���޷�Ʊ(0:��,1:��)
        /// </summary>
        public int Invoice
        {
            get { return _invoice; }
            set { _invoice = value; }
        }

        private int _repair;//����(��λ:�� 0Ϊ������)
        /// <summary> 
        /// ����(��λ:�� 0Ϊ������)
        /// </summary>
        public int Repair
        {
            get { return _repair; }
            set { _repair = value; }
        }

        private string _message;//��ϸ��Ϣ
        /// <summary> 
        /// ��ϸ��Ϣ
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _otherlink;//��������(�����ӵ���������TAOBAO��)
        /// <summary> 
        /// ��������(�����ӵ���������TAOBAO��)
        /// </summary>
        public string Otherlink
        {
            get { return _otherlink; }
            set { _otherlink = value; }
        }

        private int _readperm;//�Ķ�Ȩ��
        /// <summary> 
        /// �Ķ�Ȩ��
        /// </summary>
        public int Readperm
        {
            get { return _readperm; }
            set { _readperm = value; }
        }

        private int _tradetype;//���׷�ʽ( 1:֧�������߽���  0:���½��� �������)
        /// <summary> 
        /// ���׷�ʽ( ֧�������߽���  ���½��� �������)
        /// </summary>
        public int Tradetype
        {
            get { return _tradetype; }
            set { _tradetype = value; }
        }

        private int _tagid;//tags��ǩ
        /// <summary> 
        /// tags��ǩ
        /// </summary>
        public int Tagid
        {
            get { return _tagid; }
            set { _tagid = value; }
        }

        private int _viewcount;//�������
        /// <summary> 
        /// �������
        /// </summary>
        public int Viewcount
        {
            get { return _viewcount; }
            set { _viewcount = value; }
        }

        //private int _invisible;//�Ƿ��ϼ� 0:�ϼ�(�ɼ�) 1:
        ///// <summary> 
        ///// �Ƿ��ϼ� 0:�ϼ�(�ɼ�) 1:δ�ϼ�(���ɼ�)
        ///// </summary>
        //public int Invisible
        //{
        //    get { return _invisible; }
        //    set { _invisible = value; }
        //}

        private int _smileyoff;//
        ///<summary>
        ///�Ƿ�ر�smile����
        ///</summary>
        public int Smileyoff
        {
            get { return _smileyoff; }
            set { _smileyoff = value; }
        }

        private int _bbcodeoff;//�Ƿ�֧��UBB
        /// <summary> 
        /// �Ƿ�֧��UBB
        /// </summary>
        public int Bbcodeoff
        {
            get { return _bbcodeoff; }
            set { _bbcodeoff = value; }
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


        private string _highlight = "";//������ʽ����
        /// <summary> 
        /// ������ʽ����
        /// </summary>
        public string Highlight
        {
            get { return _highlight; }
            set { _highlight = value; }
        }

        private string _htmltitle = "";//����(�ɺ�html����,�����ݿ��ֶ�)
        /// <summary> 
        /// ��ʾ����(�ɺ�html����,�����ݿ��ֶ�)
        /// </summary>
        public string Htmltitle
        {
            get { return _htmltitle; }
            set { _htmltitle = value; }
        }
    }

    /// <summary>
    /// ��Ʒ��Ϣ��ʾ�б���
    /// </summary>
    //public class ShowGoodsinfoList : GoodsInfo
    //{

    //}
}


