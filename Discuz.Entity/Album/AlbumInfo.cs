using System;

namespace Discuz.Entity
{
	/// <summary>
	/// AlbumInfo 的摘要说明。
	/// </summary>
	public class AlbumInfo
	{

		private int m_albumid;	//相册ID
		private int m_albumcateid; //相册分类ID
		private int m_userid;	//用户ID
		private string m_username; //用户名称
		private string m_title;	//相册名称
		private string m_description;	//相册说明
		private string m_logo;	//相册封面图片
		private string m_password;	//相册密码
		private int m_imgcount;	//图片总数
		private int m_views;	//相册查看次数
		private int m_type;	//相册类型(0=公开,1=私人)
		private string m_createdatetime; //相册创建日期

		///<summary>
		///相册ID
		///</summary>
		public int Albumid
		{
			get { return m_albumid;}
			set { m_albumid = value;}
		}

		///<summary>
		///相册分类ID
		///</summary>
		public int Albumcateid
		{
			get { return m_albumcateid;}
			set { m_albumcateid = value;}
		}
		///<summary>
		///用户ID
		///</summary>
		public int Userid
		{
			get { return m_userid;}
			set { m_userid = value;}
		}

		/// <summary>
		/// 用户名称
		/// </summary>
		public string Username
		{
			get { return m_username; }
			set { m_username = value; }
		}

		///<summary>
		///相册名称
		///</summary>
		public string Title
		{
			get { return m_title;}
			set { m_title = value.Trim();}
		}
		///<summary>
		///相册说明
		///</summary>
		public string Description
		{
			get { return m_description;}
			set { m_description = value.Trim();}
		}
		///<summary>
		///相册封面图片
		///</summary>
		public string Logo
		{
			get { return m_logo;}
			set { m_logo = value.Trim();}
		}
		///<summary>
		///相册密码
		///</summary>
		public string Password
		{
			get { return m_password;}
			set { m_password = value.Trim();}
		}
		///<summary>
		///图片总数
		///</summary>
		public int Imgcount
		{
			get { return m_imgcount;}
			set { m_imgcount = value;}
		}
		///<summary>
		///相册查看次数
		///</summary>
		public int Views
		{
			get { return m_views;}
			set { m_views = value;}
		}
		///<summary>
		///相册类型(0=公开,1=私人)
		///</summary>
		public int Type
		{
			get { return m_type;}
			set { m_type = value;}
		}

		/// <summary>
		/// 相册创建日期
		/// </summary>
		public string Createdatetime
		{
			get { return m_createdatetime; }
			set { m_createdatetime = value.Trim(); }
		}

	}
}
