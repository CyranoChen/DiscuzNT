using System;
using System.Collections;

namespace Discuz.Common
{
    #region userless code
  
    //public class CalUtility
    //{
    //    System.Text.StringBuilder StrB;
    //    private int iCurr = 0;
    //    private int iCount = 0;
    //    /// <summary>
    //    /// ���췽��
    //    /// </summary>
    //    public CalUtility(string calStr)
    //    {
    //        StrB = new System.Text.StringBuilder(calStr.Trim());
    //        iCount = System.Text.Encoding.Default.GetByteCount(calStr.Trim());
    //    }

    //    /// <summary>
    //    /// ȡ��,�Զ�������ֵ������
    //    /// </summary>
    //    /// <returns></returns>\
    //    public string getItem()
    //    {
    //        //������
    //        if(iCurr == iCount)
    //            return "";
    //        char ChTmp = StrB[iCurr];
    //        bool b = IsNum(ChTmp);
    //        if(!b)
    //        {
    //            iCurr++;
    //            return ChTmp.ToString();
    //        }
    //        string strTmp = "";
    //        while(IsNum(ChTmp) == b && iCurr < iCount)
    //        {
    //            ChTmp = StrB[iCurr];
    //            if(IsNum(ChTmp) == b)
    //                strTmp += ChTmp;
    //            else
    //                break;
    //            iCurr++;
    //        }
    //        return strTmp;
    //    }
		
    //    /// <summary>
    //    /// �Ƿ�������
    //    /// </summary>
    //    /// <param name="c">����</param>
    //    /// <returns></returns>
    //    public bool IsNum(char c)
    //    {
    //        if((c>='0' && c<='9')|| c=='.')
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
		

    //    /// <summary>
    //    /// �Ƿ�������
    //    /// </summary>
    //    /// <param name="c">����</param>
    //    /// <returns></returns>
    //    public bool IsNum(string c)
    //    {
    //        if(c.Equals(""))
    //            return false;
    //        if((c[0]>='0' && c[0]<='9')|| c[0]=='.')
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
		
    //    /// <summary>
    //    /// �Ƚ�str1��str2��������������ȼ�,ture��ʾstr1����str2,false��ʾstr1����str2
    //    /// </summary>
    //    /// <param name="str1">�����1</param>
    //    /// <param name="str2">�����2</param>
    //    /// <returns></returns>
    //    public bool Compare(string str1,string str2)
    //    {
    //        return getPriority(str1) >= getPriority(str2);
    //    }


    ///// <summary>
    ///// ȡ�ü�����ŵ����ȼ�
    ///// </summary>
    ///// <param name="str">�����</param>
    ///// <returns></returns>	
    //    public int getPriority(string str)
    //    {
    //        if(str.Equals(""))
    //        {
    //            return -1;
    //        }
    //        if(str.Equals("("))
    //        {
    //            return 0;
    //        }
    //        if(str.Equals("+") || str.Equals("-"))
    //        {
    //            return 1;
    //        }
    //        if(str.Equals("*") || str.Equals("/"))
    //        {
    //            return 2;
    //        }
    //        if(str.Equals(")"))
    //        {
    //            return 0;
    //        }
    //        return 0;
    //    }
    //}



    ///// <summary>
    ///// IOper ��ժҪ˵��
    ///// ������ӿ�
    ///// </summary>	
    //public interface IOper
    //{
    //    /// <summary>
    //    /// ���������ӿڼ��㷽��
    //    /// </summary>
    //    /// <param name="o1">����1</param>
    //    /// <param name="o2">����2</param>
    //    /// <returns></returns>
    //    object Oper(object o1,object o2);
    //}


    ///// <summary>
    ///// Opers ��ժҪ˵��
    ///// ���������Ľӿ�ʵ��,�Ӽ��˳�
    ///// </summary>
    //public class OperAdd:IOper
    //{
    //    public OperAdd()
    //    {
    //    }
    //    #region IOper ��Ա
    //    public object Oper(object o1, object o2)
    //    {
    //        Decimal d1 = Decimal.Parse(o1.ToString());
    //        Decimal d2 = Decimal.Parse(o2.ToString());
    //        return d1 + d2;
    //    }
    //    #endregion
    //}
	

    //public class OperDec:IOper
    //{
    //    public OperDec()
    //    {
    //    }
    //    #region IOper ��Ա
		
    //    public object Oper(object o1, object o2)
    //    {
    //        Decimal d1 = Decimal.Parse(o1.ToString());
    //        Decimal d2 = Decimal.Parse(o2.ToString());
    //        return d1 - d2;
    //    }
    //    #endregion
    //}
	
	
    //public class OperRide:IOper
    //{
    //    public OperRide()
    //    {
    //        //
    //        // TODO: �ڴ˴���ӹ��캯���߼�
    //        //
    //    }
    //    #region IOper ��Ա
		
    //    public object Oper(object o1, object o2)
    //    {
    //        Decimal d1 = Decimal.Parse(o1.ToString());
    //        Decimal d2 = Decimal.Parse(o2.ToString());
    //        return d1 * d2;
    //    }
    //    #endregion
    //}
	
	
    //public class OperDiv:IOper
    //{
    //    public OperDiv()
    //    {
    //    }
    //    #region IOper ��Ա
		
    //    public object Oper(object o1, object o2)
    //    {
    //        Decimal d1 = Decimal.Parse(o1.ToString());
    //        Decimal d2 = Decimal.Parse(o2.ToString());
    //        return d1 / d2;
    //    }
		
    //    #endregion
    //}



    ///// <summary>
    ///// OperFactory ��ժҪ˵����
    ///// ������ӿڹ���
    ///// </summary>

    //public class OperFactory
    //{
    //    public OperFactory()
    //    {
    //    }
    //    public IOper CreateOper(string Oper)
    //    {
    //        if(Oper.Equals("+"))
    //        {
    //            IOper p = new OperAdd();
    //            return p;
    //        }
    //        if(Oper.Equals("-"))
    //        {
    //            IOper p = new OperDec();
    //            return p;
    //        }
    //        if(Oper.Equals("*"))
    //        {
    //            IOper p = new OperRide();
    //            return p;
    //        }
    //        if(Oper.Equals("/"))
    //        {
    //            IOper p = new OperDiv();
    //            return p;
    //        }
    //        return null;
    //    }
    //}

	
    ///// <summary>
    ///// Arithmetic ��ժҪ˵��
    ///// ����ʵ������
    ///// </summary>

    //public class Arithmetic
    //{
    ///// <summary>
    ///// ������ջ
    ///// </summary>
    //private ArrayList HList;
    ///// <summary>
    ///// ��ֵջ
    ///// </summary>
    //public ArrayList Vlist;
    ///// <summary>
    ///// �����Թ���
    ///// </summary>
    //private CalUtility cu;
    ///// <summary>
    ///// �������������
    ///// </summary>
    //private OperFactory of;
    ///// <summary>
    ///// ���췽��
    ///// </summary>
    ///// <param name="str">��ʽ</param>
		
    //    public Arithmetic(string str)
    //    {
    //        //
    //        // TODO: �ڴ˴���ӹ��캯���߼�
    //        //
    //        HList = new ArrayList();
    //        Vlist = new ArrayList();
    //        of = new OperFactory();
    //        cu = new CalUtility(str);
    //    }


    //    /// <summary>
    //    /// ��ʼ����
    //    /// </summary>
    //    /// 
		
    //    public object DoCal()
    //    {
    //        string strTmp = cu.getItem();
    //        while(true)
    //        {
    //            if(cu.IsNum(strTmp))
    //            {
    //                //�������ֵ,��д������ջ
    //                Vlist.Add(strTmp);
    //            }
    //            else
    //            {
    //                //��ֵ
    //                Cal(strTmp);
    //            }
    //            if(strTmp.Equals(""))
    //                break;
    //            strTmp = cu.getItem();
    //        }
    //        return Vlist[0];
    //    }
		
		
    //    /// <summary>
    //    /// ����
    //    /// </summary>
    //    /// <param name="str">�����</param>
    //    /// 
    //    private void Cal(string str)
    //    {
    //        //���ű�Ϊ��,���ҵ�ǰ����Ϊ"",����Ϊ�Ѿ��������
    //        if(str.Equals("")&&HList.Count == 0)
    //            return;
    //        if(HList.Count > 0 && Vlist.Count > 1)
    //        {
    //            //�����Ƿ���Զ�����
    //            if(HList[HList.Count-1].ToString().Equals("(") && str.Equals(")"))
    //            {
    //                HList.RemoveAt(HList.Count-1);
    //                if(HList.Count > 0)
    //                {
    //                    str = HList[HList.Count-1].ToString();
    //                    //HList.RemoveAt(HList.Count-1);
    //                    Cal(str);
    //                }
    //                return;
    //            }
    //            //�Ƚ����ȼ�
    //            if(cu.Compare(HList[HList.Count-1].ToString(),str))
    //            {
    //                //�������,�����
    //                IOper p = of.CreateOper(HList[HList.Count -1].ToString());
    //                if(p != null)
    //                {
    //                    Vlist[Vlist.Count -2] = p.Oper(Vlist[Vlist.Count-2],Vlist[Vlist.Count-1]);
    //                    HList.RemoveAt(HList.Count -1);
    //                    Vlist.RemoveAt(Vlist.Count -1);
    //                    Cal(str);
    //                }
    //                return;
    //            }
    //            if(!str.Equals(""))
    //                HList.Add(str);
    //        }
    //        else
    //        {
    //            if(!str.Equals(""))
    //                HList.Add(str);
    //        }
    //    }
    //}
    #endregion

    /// <summary>
    /// CalUtility ��ժҪ˵��
    /// ����ʽ��������
    /// </summary>
    public class Arithmetic
    {
        private Arithmetic() { }

        #region No01.���ʽ�ָ�ΪArrayList��ʽ
        /// <summary>
        /// Ҫ����ʽ�Կո�\t��Ϊ�ָ���
        /// ת�����ʽ�۷�Ϊ��
        /// ��������ֵ ,����������Ϊ@
        /// �ַ�������
        /// �������{+��-��*��/��++��+=��--��-=��*=��/=��!��!=��>��>=��>>��<��<=��<>��|��|=��||��&��&=��&&}
        /// ����{����(��)}
        /// </summary>
        /// <param name="sExpression"></param>
        /// <returns></returns>
        public static ArrayList ConvertExpression(string sExpression)
        {
            ArrayList alist = new ArrayList();

            string word = null;
            int i = 0;
            string c = "";

            while (i < sExpression.Length)
            {
                #region "
                if (word != null && word != "")
                    if (word.Substring(0, 1) == "\"")
                    {
                        do
                        {
                            c = sExpression[i++].ToString();
                            if (c == "\"") { alist.Add(word + c); word = c = null; break; }
                            else { word += c; c = null; }
                        } while (i < sExpression.Length);
                    }
                if (i > sExpression.Length - 1)
                { alist.Add(word); alist.Add(c); word = c = null; break; }
                #endregion

                #region �ַ��б�
                switch (c = sExpression[i++].ToString())
                {
                    #region ( )
                    case "\"":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; break; }
                        else
                        {
                            word = c; c = null;
                            do
                            {
                                c = sExpression[i++].ToString();
                                if (c == "\"") { alist.Add(word + c); word = c = null; break; }
                                else { word += c; c = null; }
                            } while (i < sExpression.Length);
                            break;
                        }

                    case "(": alist.Add(word); alist.Add(c); word = c = null; break;
                    case ")": alist.Add(word); alist.Add(c); word = c = null; break;
                    case " ": alist.Add(word); word = c = null; break;
                    #endregion

                    #region + - * / %
                    case "+":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "+": alist.Add(word); alist.Add("++"); word = c = null; break;
                                case "=": alist.Add(word); alist.Add("+="); word = c = null; break;
                                default: alist.Add(word); alist.Add("+"); word = c = null; i--; break;
                            }
                        break;
                    case "-":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "-": alist.Add(word); alist.Add("--"); word = c = null; break;
                                case "=": alist.Add(word); alist.Add("-="); word = c = null; break;
                                default: alist.Add(word); alist.Add("-"); word = c = null; i--; break;
                            }
                        break;
                    case "*":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "=": alist.Add(word); alist.Add("*="); word = c = null; break;
                                default: alist.Add(word); alist.Add("*"); word = c = null; i--; break;
                            }
                        break;
                    case "/":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "=": alist.Add(word); alist.Add("/="); word = c = null; break;
                                default: alist.Add(word); alist.Add("/"); word = c = null; i--; break;
                            }
                        break;
                    case "%":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "=": alist.Add(word); alist.Add("%="); word = c = null; break;
                                default: alist.Add(word); alist.Add("%"); word = c = null; i--; break;
                            }
                        break;
                    #endregion

                    #region > < =
                    case ">":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case ">": alist.Add(word); alist.Add(">>"); word = c = null; break;
                                case "=": alist.Add(word); alist.Add(">="); word = c = null; break;
                                default: alist.Add(word); alist.Add(">"); word = c = null; i--; break;
                            }
                        break;
                    case "<":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "<": alist.Add(word); alist.Add("<<"); word = c = null; break;
                                case ">": alist.Add(word); alist.Add("<>"); word = c = null; break;
                                case "=": alist.Add(word); alist.Add("<="); word = c = null; break;
                                default: alist.Add(word); alist.Add("<"); word = c = null; i--; break;
                            }
                        break;
                    case "=":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "=": alist.Add(word); alist.Add("=="); word = c = null; break;
                                default: alist.Add(word); alist.Add("="); word = c = null; i--; break;
                            }
                        break;
                    #endregion

                    #region ! | &
                    case "!":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "=": alist.Add(word); alist.Add("!="); word = c = null; break;
                                default: alist.Add(word); alist.Add("!"); word = c = null; i--; break;
                            }
                        break;
                    case "|":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "=": alist.Add(word); alist.Add("|="); word = c = null; break;
                                case "|": alist.Add(word); alist.Add("||"); word = c = null; break;
                                default: alist.Add(word); alist.Add("|"); word = c = null; i--; break;
                            }
                        break;
                    case "&":
                        if (i > sExpression.Length - 1)
                        { alist.Add(word); alist.Add(c); word = c = null; }
                        else
                            switch (c = sExpression[i++].ToString())
                            {
                                case "=": alist.Add(word); alist.Add("&="); word = c = null; break;
                                case "&": alist.Add(word); alist.Add("&&"); word = c = null; break;
                                default: alist.Add(word); alist.Add("&"); word = c = null; i--; break;
                            }
                        break;
                    #endregion
                    default:
                        word += c;
                        break;
                }
                if (i == sExpression.Length) alist.Add(word);
                #endregion
            }

            ArrayList alresult = new ArrayList();
            foreach (object a in alist)
            {
                if (a == null) continue;
                if (a.ToString().Trim() == "") continue;
                alresult.Add(a);
            }
            return alresult;
        }

        /// <summary>
        /// �Է��صı��ʽ���Ѿ��ֺ÷���ArrayList�еı��������滻Ϊʵ�ʳ���
        /// </summary>
        /// <param name="alExpression"></param>
        /// <param name="mapVar"></param>
        /// <param name="mapValue"></param>
        /// <returns></returns>
        public static ArrayList ConvertExpression(ArrayList alExpression, string mapVar, string mapValue)
        {
            for (int i = 0; i < alExpression.Count; i++)
            {
                if (alExpression[i].ToString() == mapVar) { alExpression[i] = mapValue; break; }
            }
            return alExpression;
        }
        /// <summary>
        /// �Է��صı��ʽ���Ѿ��ֺ÷���ArrayList�еı��������滻Ϊʵ�ʳ���
        /// </summary>
        /// <param name="alExpression"></param>
        /// <param name="name"></param>
        /// <param name="mapvalue"></param>
        /// <returns></returns>
        public static ArrayList ConvertExpression(ArrayList alExpression, string[] mapVar, string[] mapValue)
        {
            for (int i = 0; i < alExpression.Count; i++)
            {
                for (int j = 0; j < mapVar.Length; j++)
                {
                    if (alExpression[i].ToString() == mapVar[j])
                    {
                        alExpression[i] = mapValue[j];
                        break;
                    }
                    //     System.Console.WriteLine("Expression: {0}  >>>  {1}",mapVar[j], mapValue[j]);
                }
            }
            return alExpression;
        }

        #endregion

        #region No02.��׺���ʽ��ʽ �������ʽ
        /// <summary>
        /// �ҳ���һ��������
        /// </summary>
        /// <param name="alExpression"></param>
        /// <returns></returns>
        public static int Find_First_RightBracket(ArrayList alExpression)
        {
            for (int i = 0; i < alExpression.Count; i++)
            { if (OperatorMap.CheckRightBracket(alExpression[i].ToString())) return i; }
            return 0;
        }
        /// <summary>
        /// �ҳ�ƥ��Ŀ�����
        /// </summary>
        /// <param name="alExpression"></param>
        /// <param name="iRightBracket"></param>
        /// <returns></returns>
        public static int Find_Near_LeftBracket(ArrayList alExpression, int iRightBracket)
        {
            int i = iRightBracket - 2;
            while (i >= 0)
            {
                if (OperatorMap.CheckLeftBracket(alExpression[i].ToString())) return i;
                i--;
            }
            return 0;
        }
        /// <summary>
        /// ��׺���ʽת��Ϊ��׺���ʽ
        /// </summary>
        /// <param name="alexpression"></param>
        /// <returns></returns>
        public static ArrayList ConvertToPostfix(ArrayList alexpression)
        {
            ArrayList alOutput = new ArrayList();
            Stack sOperator = new Stack();
            string word = null;
            int count = alexpression.Count;
            int i = 0;
            while (i < count)
            {
                word = alexpression[i++].ToString();

                //������������ʱ���ǽ���ѹ��ջ��
                if (OperatorMap.CheckLeftBracket(word))
                { sOperator.Push(word); }
                else

                    //������������ʱ����*��ջ���ĵ�һ������������������ȫ�����ε���������������к��ٶ��������š�
                    if (OperatorMap.CheckRightBracket(word))
                    {
                        while (true)
                        {
                            if (sOperator.Count == 0) break;
                            string sTop = sOperator.Peek().ToString();
                            if (sTop == "(") { sOperator.Pop(); break; }
                            else alOutput.Add(sOperator.Pop());
                        }
                    }
                    else

                        //������������ֱ���������������
                        if (OperatorMap.IsVar(word))
                        { alOutput.Add(word); }
                        else

                            //�������������tʱ��
                            //����������a.��ջ���������ȼ����ڻ����t��������������͵���������У� 
                            //����������b.t��ջ
                            if (OperatorMap.CheckOperator(word))
                            {
                                while (sOperator.Count > 0)
                                {
                                    string sPop = sOperator.Peek().ToString();

                                    if (sPop == "(") break;

                                    if (OperatorMap.GetMaxprior(word, sPop) >= 0)
                                    {
                                        //       sPop = sOperator.Pop().ToString();
                                        alOutput.Add(sOperator.Pop().ToString());
                                    }
                                    else
                                        break;
                                    //      System.Console.WriteLine("XH{0}",sPop);

                                }
                                sOperator.Push(word);
                            }

                //    System.Console.WriteLine("{0}",word.ToString());
            }

            //��׺���ʽȫ���������ջ������������������͵����������
            while (sOperator.Count > 0)
            {
                string s = sOperator.Pop().ToString();
                alOutput.Add(s);
                //    System.Console.WriteLine("{0}:{1}",sOperator.Count,s.ToString());
            }

            return alOutput;
        }


        /// <summary>
        /// �����׺���ʽ
        /// </summary>
        /// <param name="alexpression"></param>
        /// <returns></returns>
        public static object ComputePostfix(ArrayList alexpression)
        {
            try
            {
                //������һ��ջS
                Stack s = new Stack();
                int count = alexpression.Count;
                int i = 0;
                while (i < count)
                {
                    //�������Ҷ���׺���ʽ���������־ͽ���ת��Ϊ��ֵѹ��ջS�У�
                    string word = alexpression[i++].ToString();
                    if (OperatorMap.IsVar(word))
                    {
                        s.Push(word);
                        //      System.Console.WriteLine("Push:{0}",word);
                    }
                    else//������������ջ�����ε����������ֱ�Y��X��
                        if (OperatorMap.CheckOperator(word))
                        {
                            string y, x, sResult;
                            if (!CheckOneOperator(word))
                            {
                                y = s.Pop().ToString();
                                x = s.Pop().ToString();
                                //Ȼ���ԡ�X ����� Y������ʽ��������������ѹ��ջS�� 
                                sResult = ComputeTwo(x, y, word).ToString();
                                s.Push(sResult);
                            }
                            else
                            {
                                x = s.Pop().ToString();
                                sResult = ComputeOne(x, word).ToString();
                                s.Push(sResult);
                            }
                        }
                }
                string spop = s.Pop().ToString();
                //    System.Console.WriteLine("Result:{0}",spop);
                return spop;
            }
            catch
            {
                System.Console.WriteLine("Result:���ʽ�������������!Sorry!");
                return "Sorry!Error!";
            }

        }

        public static object ComputeExpression(string sExpression)
        {
            return Arithmetic.ComputePostfix(Arithmetic.ConvertToPostfix(Arithmetic.ConvertExpression(sExpression)));
        }

        public static object ComputeExpression(string sExpression, string mapVar, string mapValue)
        {
            return Arithmetic.ComputePostfix(Arithmetic.ConvertToPostfix(Arithmetic.ConvertExpression(Arithmetic.ConvertExpression(sExpression), mapVar, mapValue)));
        }

        public static object ComputeExpression(string sExpression, string[] mapVar, string[] mapValue)
        {

            return Arithmetic.ComputePostfix(Arithmetic.ConvertToPostfix(Arithmetic.ConvertExpression(Arithmetic.ConvertExpression(sExpression), mapVar, mapValue)));
        }

        #endregion

        #region No03. �������ű��ʽ�ļ���

        #region ����ַ�����ת��������
        public static bool CheckNumber(string str)
        {
            try { Convert.ToDouble(str); return true; }
            catch { return false; }
        }

        public static bool CheckBoolean(string str)
        {
            try { Convert.ToBoolean(str); return true; }
            catch { return false; }
        }

        public static bool CheckString(string str)
        {
            try
            {
                str = str.Replace("\"", "");
                char c = (char)(str[0]);
                if ((c >= 'a') && (c <= 'z') || (c >= 'A') && (c <= 'Z'))
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
        public static bool CheckOneOperator(string sOperator)
        {
            if (sOperator == "++" || sOperator == "--" || sOperator == "!")
                return true;
            else
                return false;
        }

        #endregion

        #region ˫Ŀ����
        public static object ComputeTwoNumber(double dL, double dR, string sO)
        {
            switch (sO)
            {
                case "+": return (dL + dR);
                case "-": return (dL - dR);
                case "*": return (dL * dR);
                case "%": return (dL % dR);
                case "/": try { return (dL / dR); }
                    catch
                    {
                        return false;
                        //return "ComputeTwoNumber ["+sO+"] Sorry!";
                    }

                case "+=": return (dL += dR);
                case "-=": return (dL -= dR);
                case "*=": return (dL *= dR);
                case "/=": try { return (dL /= dR); }
                    catch
                    {
                        return false;
                        //return "ComputeTwoNumber ["+sO+"] Sorry!";
                    }

                case "=": return (dL == dR);
                case "==": return (dL == dR);
                case "!=": return (dL != dR);
                case "<>": return (dL != dR);
                case ">": return (dL.CompareTo(dR) > 0);
                case ">=": return (dL.CompareTo(dR) >= 0);
                case "<": return (dL.CompareTo(dR) < 0);
                case "<=": return (dL.CompareTo(dR) <= 0);

                case ">>": return (int)dL >> (int)dR;
                case "<<": return (int)dL << (int)dR;
                case "|": return (int)dL | (int)dR;
                case "&": return (int)dL & (int)dR;
                case "|=":
                    {
                        int iL = (int)dL;
                        int iR = (int)dR;
                        return iL |= iR;
                    }
                case "&=":
                    {
                        int iL = (int)dL;
                        int iR = (int)dR;
                        return iL &= iR;
                    }
                default:
                    return false;
                //return "ComputeTwoNumber ["+sO+"] Sorry!";
            }
        }

        public static object ComputeTwoBoolean(bool bL, bool bR, string sO)
        {
            switch (sO)
            {
                case ">": return bL.CompareTo(bR) > 0;
                case ">=": return bL.CompareTo(bR) >= 0;
                case "<": return bL.CompareTo(bR) < 0;
                case "<=": return bL.CompareTo(bR) <= 0;
                case "=": return bL == bR;
                case "==": return bL == bR;
                case "!=": return bL != bR;
                case "<>": return bL != bR;

                case "||": return bL || bR;
                case "&&": return bL && bR;
                default: return false;
                //return "ComputeTwoBoolean ["+sO+"] Sorry!";
            }
        }

        public static object ComputeTwoString(string sL, string sR, string sO)
        {
            switch (sO)
            {
                case "+": return sL + sR;
                case "=": return (sL == sR);
                case "==": return (sL == sR);
                case "!=": return (sL != sR);
                case "<>": return (sL != sR);
                case ">": return (sL.CompareTo(sR) > 0);
                case ">=": return (sL.CompareTo(sR) >= 0);
                case "<": return (sL.CompareTo(sR) < 0);
                case "<=": return (sL.CompareTo(sR) <= 0);
                default: return false;
                //return "ComputeTwoString ["+sO+"] Sorry!";
            }
        }

        public static object ComputeTwo(string sL, string sR, string sO)
        {
            if (CheckNumber(sL))
            {
                if (CheckNumber(sR))
                    return ComputeTwoNumber(Convert.ToDouble(sL), Convert.ToDouble(sR), sO);
                else
                    if (CheckString(sR)) return ComputeTwoString(sL, sR, sO);
            }
            else if (CheckBoolean(sL))
            {
                if (CheckBoolean(sR))
                    return ComputeTwoBoolean(Convert.ToBoolean(sL), Convert.ToBoolean(sR), sO);
                else
                    if (CheckString(sR)) return ComputeTwoString(sL, sR, sO);
            }
            else if (CheckString(sL)) return ComputeTwoString(sL, sR, sO);

            return "ComputeTwo [" + sL + "][" + sO + "][" + sR + "] Sorry!";
        }

        #endregion

        #region ��Ŀ����
        public static object ComputeOneNumber(double dou, string sO)
        {
            switch (sO)
            {
                case "++": return (dou + 1);
                case "--": return (dou - 1);
                default: return false;
                //return "ComputeOneNumber ["+sO+"] Sorry!";
            }
        }

        public static object ComputeOneString(string str, string sO)
        {
            switch (sO)
            {
                case "++": return (str + str);
                default: return false;
                //return "ComputeOneString ["+sO+"] Sorry!";
            }
        }

        public static object ComputeOneBoolean(bool bo, string sO)
        {
            switch (sO)
            {
                case "!": return (!bo);
                default: return false;
                //   return "ComputeOneBoolean ["+sO+"] Sorry!";
            }
        }

        public static object ComputeOne(string str, string sO)
        {
            if (CheckNumber(str))
                return ComputeOneNumber(Convert.ToDouble(str), sO);
            if (CheckBoolean(str))
                return ComputeOneBoolean(Convert.ToBoolean(str), sO);
            if (CheckString(str))
                return ComputeOneString(str, sO);
            return "ComputerOne [" + str + "][" + sO + "] Sorry!";
        }


        #endregion

        #endregion

        #region No04. ʵ�ù�����
        /// <summary>
        /// ArrayList�Ӽ�����
        /// </summary>
        public class ArrayListCopy
        {
            private ArrayListCopy() { }
            /// <summary>
            /// ����ArrayList�Ӽ�{L--R}����
            /// </summary>
            /// <param name="alist"></param>
            /// <param name="iLeft"></param>
            /// <param name="iRight"></param>
            /// <returns></returns>
            public static ArrayList CopyBewteenTo(ArrayList alist, int iLeft, int iRight)
            {
                ArrayList alResult = new ArrayList();
                bool b = false;
                for (int i = iLeft; i < iRight; i++)
                {
                    alResult.Add(alist[i]);
                    b = true;
                }
                if (b) return alResult;
                else return null;
            }

            /// <summary>
            /// ����ArrayList�Ӽ�{L--R}�Ĳ�������
            /// </summary>
            /// <param name="alist"></param>
            /// <param name="iLeft"></param>
            /// <param name="iRight"></param>
            /// <returns></returns>
            public static ArrayList CopyNotBetweenTo(ArrayList alist, int iLeft, int iRight)
            {
                ArrayList alResult = new ArrayList();
                bool b = false;
                for (int i = 0; i < iLeft - 1; i++)
                {
                    alResult.Add(alist[i]);
                    b = true;
                }

                if (b)
                {
                    alResult.Add("@");

                    for (int i = iRight + 1; i < alist.Count; i++)
                    {
                        alResult.Add(alist[i]);
                        b = true;
                    }
                }
                if (b) return alResult;
                else return null;
            }

            /// <summary>
            /// ͳ���ַ���sin��str�г��ֵĴ���
            /// </summary>
            /// <param name="str"></param>
            /// <param name="sin"></param>
            /// <returns></returns>
            public static int GetSubStringCount(string str, string sin)
            {
                int i = 0;
                int ibit = 0;
                while (true)
                {
                    ibit = str.IndexOf(sin, ibit);
                    if (ibit > 0)
                    {
                        ibit += sin.Length;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                return i;
            }
        }


        /// <summary>
        /// ��������ȼ�ʵ��
        /// </summary>
        public class OperatorMap
        {
            public struct Map
            {
                public int Priority;
                public string Operator;
                public Map(int iPrior, string sOperator)
                {
                    Priority = iPrior;
                    Operator = sOperator;
                }
            }
            private OperatorMap() { }
            public static Map[] map()
            {
                Map[] om;
                om = new Map[30];

                om[0] = new Map(5, "*");
                om[1] = new Map(5, "/");
                om[29] = new Map(5, "%");

                om[2] = new Map(10, "+");
                om[3] = new Map(10, "-");

                om[4] = new Map(20, ">");
                om[5] = new Map(20, ">=");
                om[6] = new Map(20, "<");
                om[7] = new Map(20, "<=");
                om[8] = new Map(20, "<>");
                om[9] = new Map(20, "!=");
                om[10] = new Map(20, "==");
                om[11] = new Map(20, "=");

                om[12] = new Map(41, "!");
                om[13] = new Map(42, "||");
                om[14] = new Map(43, "&&");

                om[15] = new Map(40, "++");
                om[16] = new Map(40, "--");
                om[17] = new Map(40, "+=");
                om[18] = new Map(40, "-=");
                om[19] = new Map(40, "*=");
                om[20] = new Map(40, "/=");
                om[21] = new Map(40, "&");
                om[22] = new Map(40, "|");
                om[23] = new Map(40, "&=");
                om[24] = new Map(40, "|=");
                om[25] = new Map(40, ">>");
                om[26] = new Map(40, "<<");

                om[27] = new Map(3, "(");
                om[28] = new Map(3, ")");
                return om;
            }
            public static bool CheckLeftBracket(string str) { return (str == "("); }
            public static bool CheckRightBracket(string str) { return (str == ")"); }

            public static bool CheckBracket(string str) { return (str == "(" || str == ")"); }
            public static bool CheckOperator(string scheck)
            {
                string[] Operator = {"+", "-", "*", "/", "%",
										">", ">=", "<", "<=", "<>", "!=", "==", "=",
										"!", "||", "&&",
										"++", "--", "+=", "-=", "*=", "/=", 
										"&", "|", "&=", "|=", 
										">>", "<<",
										")", "("
									};
                bool bl = false;
                for (int i = 0; i < Operator.Length - 1; i++) { if (Operator[i] == scheck) { bl = true; break; } }
                return bl;
            }

            public static Map GetMap(string Operator)
            {
                if (CheckOperator(Operator)) foreach (Map tmp in map()) { if (tmp.Operator == Operator) return tmp; }
                return new Map(99, Operator);
            }

            public static int Getprior(string Operator) { return GetMap(Operator).Priority; }

            public static int GetMaxprior(string Loperator, string Roperator)
            { return GetMap(Loperator).Priority - GetMap(Roperator).Priority; }

            public static bool IsVar(string svar)
            {
                if ((svar[0] >= '0' && svar[0] <= '9') || (svar[0] >= 'a' && svar[0] <= 'z') || (svar[0] >= 'A' && svar[0] <= 'Z'))
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}
