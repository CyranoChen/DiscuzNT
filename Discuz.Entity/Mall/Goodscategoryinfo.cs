using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// ��Ʒ������Ϣ��
    /// </summary>
    public class Goodscategoryinfo
    {
		private int _categoryid;
		public int Categoryid
		{
			get { return _categoryid; }
			set { _categoryid = value; }
		}
		
��������private int _parentid;//��id
		/// <summary> 
		/// ��id
		/// </summary>
		public int Parentid
		{
			get { return _parentid; }
			set { _parentid = value; }
		}
		
��������private int _layer;//���ڲ���
		/// <summary> 
		/// ���ڲ���
		/// </summary>
		public int Layer
		{
			get { return _layer; }
			set { _layer = value; }
		}
		
��������private string _parentidlist;//������ַ���
		/// <summary> 
		/// ������ַ���
		/// </summary>
		public string Parentidlist
		{
			get { return _parentidlist; }
			set { _parentidlist = value; }
		}

        private int m_displayorder;//��ʾ˳��
        ///<summary>
        ///��ʾ˳��
        ///</summary>
        public int Displayorder
        {
            get { return m_displayorder; }
            set { m_displayorder = value; }
        }
		
��������private string _categoryname = "";//��������
		/// <summary> 
		/// ��������
		/// </summary>
		public string Categoryname
		{
			get { return _categoryname; }
			set { _categoryname = value.Trim(); }
		}
		
��������private int _haschild;//�Ƿ����ӽ��
		/// <summary> 
		/// �Ƿ����ӽ��
		/// </summary>
		public int Haschild
		{
			get { return _haschild; }
			set { _haschild = value; }
		}
		
��������private int _fid;//���ID
		/// <summary> 
		/// ���ID
		/// </summary>
		public int Fid
		{
			get { return _fid; }
			set { _fid = value; }
		}

        private string _pathlist;//����·��
		/// <summary> 
		/// ����·��
		/// </summary>
		public string Pathlist
		{
			get { return _pathlist; }
			set { _pathlist = value; }
		}
		
��������private int _goodscount;//�������Ʒ��
		/// <summary> 
		/// �������Ʒ��
		/// </summary>
		public int Goodscount
		{
			get { return _goodscount; }
			set { _goodscount = value; }
		}
    }
}
