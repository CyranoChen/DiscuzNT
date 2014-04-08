using System;

namespace Discuz.Config
{
	/// <summary>
	/// 基本设置描述类, 加[Serializable]标记为可序列化
	/// </summary>
	[Serializable]
	public class BaseConfigInfo : IConfigInfo
    {
        #region 私有字段

        private string m_dbconnectstring = "Data Source=;User ID=dntuser;Password=;Initial Catalog=;Pooling=true";		// 数据库连接串-格式(中文为用户修改的内容)：Data Source=数据库服务器地址;User ID=您的数据库用户名;Password=您的数据库用户密码;Initial Catalog=数据库名称;Pooling=true
		private string m_tableprefix = "dnt_";		// 数据库中表的前缀
		private string m_forumpath = "/";			// 论坛在站点内的路径
        private string m_dbtype = "";
        private int m_founderuid = 0;				// 创始人
        
        #endregion

        #region 属性

        /// <summary>
		/// 数据库连接串
		/// 格式(中文为用户修改的内容)：
		///    Data Source=数据库服务器地址;
		///    User ID=您的数据库用户名;
		///    Password=您的数据库用户密码;
		///    Initial Catalog=数据库名称;Pooling=true
		/// </summary>
		public string Dbconnectstring
		{
			get { return m_dbconnectstring;}
			set { m_dbconnectstring = value;}
		}

		/// <summary>
		/// 数据库中表的前缀
		/// </summary>
		public string Tableprefix
		{
			get { return m_tableprefix;}
			set { m_tableprefix = value;}
		}

		/// <summary>
		/// 论坛在站点内的路径
		/// </summary>
		public string Forumpath
		{
			get { return m_forumpath;}
			set { m_forumpath = value;}
		}

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Dbtype
        {
            get { return m_dbtype; }
            set { m_dbtype = value; }
        }

        /// <summary>
        /// 创始人ID
        /// </summary>
		public int Founderuid
		{
			get { return m_founderuid;}
			set { m_founderuid = value;}
        }

        #endregion
    }
}
