using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ����״̬ö��
    /// </summary>
    public enum TradeStatusEnum
    {
        /// <summary>
        /// δ��Ч�Ľ���
        /// </summary>
        UnStart = 0,
        /// <summary>
        /// �ȴ���Ҹ���
        /// </summary>
        WAIT_BUYER_PAY = 1,
        /// <summary>
        /// �����Ѵ���,�ȴ�����ȷ��
        /// </summary>
        WAIT_SELLER_CONFIRM_TRADE = 2,
        /// <summary>
        /// ȷ����Ҹ����У����𷢻�
        /// </summary>
        WAIT_SYS_CONFIRM_PAY = 3,
        /// <summary>
        /// ����Ѹ���(��֧�����յ���Ҹ���),�����ҷ���
        /// </summary>
        WAIT_SELLER_SEND_GOODS = 4,
        /// <summary>
        /// �����ѷ��������ȷ����
        /// </summary>
        WAIT_BUYER_CONFIRM_GOODS = 5,
        /// <summary>
        /// ���ȷ���յ������ȴ�֧������������
        /// </summary>
        WAIT_SYS_PAY_SELLER = 6,
        /// <summary>
        /// ���׳ɹ�����
        /// </summary>
        TRADE_FINISHED = 7,
        /// <summary>
        /// ������;�ر�(δ���)
        /// </summary>
        TRADE_CLOSED = 8,
        /// <summary>
        /// �ȴ�����ͬ���˿�
        /// </summary>
        WAIT_SELLER_AGREE = 10,
        /// <summary>
        /// ���Ҿܾ�����������ȴ�����޸�����
        /// </summary>
        SELLER_REFUSE_BUYER = 11,
        /// <summary>
        /// ����ͬ���˿�ȴ�����˻�
        /// </summary>
        WAIT_BUYER_RETURN_GOODS = 12,
        /// <summary>
        /// �ȴ������ջ�
        /// </summary>
        WAIT_SELLER_CONFIRM_GOODS = 13,
        /// <summary>
        /// ˫���Ѿ�һ�£��ȴ�֧�����˿�
        /// </summary>
        WAIT_ALIPAY_REFUND = 14,
        /// <summary>
        /// ֧����������
        /// </summary>
        ALIPAY_CHECK = 15,
        /// <summary>
        /// �������˿�
        /// </summary>
        OVERED_REFUND = 16,
        /// <summary>
        /// �˿�ɹ�(�������յ��˻�)
        /// </summary>
        REFUND_SUCCESS = 17,
        /// <summary>
        /// �˿�ر�
        /// </summary>
        REFUND_CLOSED = 18
    }

    /// <summary>
    /// ��������ͳ����
    /// </summary>
    public class Goosdstradestatisticinfo
    {
        private int __userid;//�û�id
        /// <summary>
        /// �û�id
        /// </summary>
        public int Userid
        {
            get { return __userid; }
            set { __userid = value; }
        }

        private int __sellerattention;//���ҹ�ע������
        /// <summary>
        /// ���ҹ�ע������
        /// </summary>
        public int Sellerattention
        {
            get { return __sellerattention;}
            set { __sellerattention = value;}
        }

        private int __sellertrading;//���ҽ��׽����еĽ�����
        /// <summary>
        /// ���ҽ��׽����еĽ�����
        /// </summary>
        public int Sellertrading 
        {
            get { return __sellertrading;}
            set { __sellertrading = value; }
        }

        private int __sellerrate;//���������۵Ľ�����
        /// <summary>
        /// ���������۵Ľ�����
        /// </summary>
        public int Sellerrate
        {
            get { return __sellerrate;}
            set { __sellerrate = value; }
        }

        private decimal __sellnumbersum;//�����۳���Ʒ����
        /// <summary>
        /// �����۳���Ʒ����
        /// </summary>
        public decimal Sellnumbersum
        {
            get { return __sellnumbersum; }
            set { __sellnumbersum = value; }
        }

        private decimal __selltradesum;//�������۳ɽ��ܶ�
        /// <summary>
        /// �������۳ɽ��ܶ�
        /// </summary>
        public decimal Selltradesum
        {
            get { return __selltradesum; }
            set { __selltradesum = value; }
        }


        private int __buyerattention;//��ҹ�ע������
        /// <summary>
        /// ��ҹ�ע������
        /// </summary>
        public int Buyerattention
        {
            get { return __buyerattention;}
            set { __buyerattention = value; }
        }

        private int __buyertradeing;//��ҽ��׽����еĽ�����
        /// <summary>
        /// ��ҽ��׽����еĽ�����
        /// </summary>
        public int Buyertrading   
        {
            get { return __buyertradeing;}
            set { __buyertradeing = value; }
        }

        private int __buyerrate;//��������۵Ľ�����
        /// <summary>
        /// ��������۵Ľ�����
        /// </summary>
        public int Buyerrate
        {
            get { return __buyerrate;}
            set { __buyerrate = value; }
        }

        private decimal __buynumbersum;//������Ʒ����
        /// <summary>
        /// ������Ʒ����
        /// </summary>
        public decimal Buynumbersum
        {
            get { return __buynumbersum; }
            set { __buynumbersum = value; }
        }

        private decimal __buytradesum;//����ɽ��ܶ�
        /// <summary>
        /// ����ɽ��ܶ�
        /// </summary>
        public decimal Buytradesum
        {
            get { return __buytradesum; }
            set { __buytradesum = value; }
        }

    }

    /// <summary>
    /// ��Ʒ������Ϣ��ժҪ˵����
    /// </summary>
    public class Goodstradeloginfo
    {
        private int _id;//������־ID
        /// <summary> 
        /// ������־ID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _goodsid;//��ƷID
        /// <summary> 
        /// ��ƷID
        /// </summary>
        public int Goodsid
        {
            get { return _goodsid; }
            set { _goodsid = value; }
        }

        private string _orderid = "";//������ID
        /// <summary> 
        /// ������ID
        /// </summary>
        public string Orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }

        private string _tradeno = "";//֧����������
        /// <summary> 
        /// ֧����������
        /// </summary>
        public string Tradeno 
        {
            get { return _tradeno; }
            set { _tradeno = value; }
        }

        private string _subject = "";//��Ʒ����(��Ӧ֧�ֱ�)
        /// <summary> 
        /// ��Ʒ����(��Ӧ֧�ֱ�)
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private decimal _price;//�۸�
        /// <summary> 
        /// �۸�
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _quality;//��ɫ
        /// <summary> 
        /// ��ɫ
        /// </summary>
        public int Quality
        {
            get { return _quality; }
            set { _quality = value; }
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

        private int _number;//����
        /// <summary> 
        /// ����
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        private decimal _tax;//����������
        /// <summary> 
        /// ����������
        /// </summary>
        public decimal Tax
        {
            get { return _tax; }
            set { _tax = value; }
        }

        private string _locus = "";//��Ʒ���ڵ�
        /// <summary> 
        /// ��Ʒ���ڵ�
        /// </summary>
        public string Locus
        {
            get { return _locus; }
            set { _locus = value; }
        }

        private int _sellerid;//
        /// <summary> 
        /// 
        /// </summary>
        public int Sellerid
        {
            get { return _sellerid; }
            set { _sellerid = value; }
        }

        private string _seller = "";//������
        /// <summary> 
        /// ������
        /// </summary>
        public string Seller
        {
            get { return _seller; }
            set { _seller = value; }
        }

        private string _selleraccount = "";//���ҽ����ʺ�
        /// <summary> 
        /// ���ҽ����ʺ�
        /// </summary>
        public string Selleraccount
        {
            get { return _selleraccount; }
            set { _selleraccount = value; }
        }

        private int _buyerid;//���ID
        /// <summary> 
        /// ���ID
        /// </summary>
        public int Buyerid
        {
            get { return _buyerid; }
            set { _buyerid = value; }
        }

        private string _buyer = "";//�����
        /// <summary> 
        /// �����
        /// </summary>
        public string Buyer
        {
            get { return _buyer; }
            set { _buyer = value; }
        }

        private string _buyercontact = "";//�����ϵ��ʽ
        /// <summary> 
        /// �����ϵ��ʽ
        /// </summary>
        public string Buyercontact
        {
            get { return _buyercontact; }
            set { _buyercontact = value; }
        }

        private int _buyercredits;//����ݿۻ���
        /// <summary> 
        /// ����ݿۻ���
        /// </summary>
        public int Buyercredit
        {
            get { return _buyercredits; }
            set { _buyercredits = value; }
        }

        private string _buyermsg = "";//�������
        /// <summary> 
        /// �������
        /// </summary>
        public string Buyermsg
        {
            get { return _buyermsg; }
            set { _buyermsg = value; }
        }



        private int _status;//״̬,��ϸ���òμ�ö������:TradeStatus
        /// <summary> 
        /// ״̬,��ϸ���òμ�ö������:TradeStatus
        /// </summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private DateTime _lastupdate;//״̬������
        /// <summary> 
        /// ״̬������
        /// </summary>
        public DateTime Lastupdate
        {
            get { return _lastupdate; }
            set { _lastupdate = value; }
        }

        private int _offline;//�Ƿ����߽���
        /// <summary> 
        /// �Ƿ����߽���
        /// </summary>
        public int Offline
        {
            get { return _offline; }
            set { _offline = value; }
        }

        private string _buyername = "";//�������
        /// <summary> 
        /// �������
        /// </summary>
        public string Buyername
        {
            get { return _buyername; }
            set { _buyername = value; }
        }

        private string _buyerzip = "";//����ʱ�
        /// <summary> 
        /// ����ʱ�
        /// </summary>
        public string Buyerzip
        {
            get { return _buyerzip; }
            set { _buyerzip = value; }
        }

        private string _buyerphone = "";//��ҵ绰
        /// <summary> 
        /// ��ҵ绰
        /// </summary>
        public string Buyerphone
        {
            get { return _buyerphone; }
            set { _buyerphone = value; }
        }

        private string _buyermobile = "";//����ֻ�
        /// <summary> 
        /// ����ֻ�
        /// </summary>
        public string Buyermobile
        {
            get { return _buyermobile; }
            set { _buyermobile = value; }
        }

        private int _transport;//��������: 0: VIRTUAL(������Ʒ), 1:POST(ƽ��),  2: EMS(EMS), 3:EXPRESS(������ݹ�˾)
        /// <summary> 
        /// ��������: 0: VIRTUAL(������Ʒ), 1:POST(ƽ��),  2: EMS(EMS), 3:EXPRESS(������ݹ�˾)
        /// </summary>
        public int Transport
        {
            get { return _transport; }
            set { _transport = value; }
        }

        private int _transportpay;//����֧������: SELLER_PAY(����֧����������,���ò��Ƶ��ܼ���),BUYER_PAY(���֧����������,������Ҫ�Ƶ��ܼ���),BUYER_PAY_AFTER_RECEIVE(����յ�����ֱ��֧����������˾�����ò��üƵ��ܼ���)
        /// <summary> 
        /// ����֧������: SELLER_PAY(����֧����������,���ò��Ƶ��ܼ���),BUYER_PAY(���֧����������,������Ҫ�Ƶ��ܼ���),BUYER_PAY_AFTER_RECEIVE(����յ�����ֱ��֧����������˾�����ò��üƵ��ܼ���)
        /// </summary>
        public int Transportpay
        {
            get { return _transportpay; }
            set { _transportpay = value; }
        }

        private decimal _transportfee;//�������
        /// <summary> 
        /// �������
        /// </summary>
        public decimal Transportfee
        {
            get { return _transportfee; }
            set { _transportfee = value; }
        }

        private decimal _tradesum;//�����ܶ�
        /// <summary> 
        /// �����ܶ�
        /// </summary>
        public decimal Tradesum
        {
            get { return _tradesum; }
            set { _tradesum = value; }
        }


        private decimal _baseprice;//��Ʒԭ��
        /// <summary> 
        /// ��Ʒԭ��
        /// </summary>
        public decimal Baseprice
        {
            get { return _baseprice; }
            set { _baseprice = value; }
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

        private int _ratestatus;//����״̬(0:˫��δ�� 1:�ȴ��Է����� 2:���۶Է� 3:˫������)
        /// <summary> 
        /// ����״̬(0:˫��δ�� 1:�ȴ��Է����� 2:���۶Է� 3:˫������)
        /// </summary>
        public int Ratestatus
        {
            get { return _ratestatus; }
            set { _ratestatus = value; }
        }

        private string _message = "";//��������
        /// <summary> 
        /// ��������
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
