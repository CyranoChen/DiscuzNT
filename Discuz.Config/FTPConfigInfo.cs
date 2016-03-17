using System;
using System.Xml.Serialization;

namespace Discuz.Config
{
	/// <summary>
	/// FTP配置信息类
	/// </summary>
	[Serializable]
    public class FTPConfigInfo : IConfigInfo
    {

       
        #region FTP私有字段

        private string m_name = ""; //名称,如forumattach:论坛附件,spaceattach:空间附件,album:相册等

        private string m_serveraddress = ""; //服务器地址

        private int m_serverport = 25; //服务器端口号

        private string m_username = "";  //登陆帐号

		private string m_password = "";  //登陆密码

        private int m_mode = 1; //链接模式 1:被动 2:主动

        private int m_allowupload = 0; //允许FTP上传附件 0:不允许 1:允许   

        private string m_uploadpath = ""; //上传路径

        private int m_timeout = 0; //无响应时间（FTP在指定时间内无响应）,单位:秒

        private string m_remoteurl = ""; //远程访问 URL

        private int m_reservelocalattach = 0; //是否保留本地附件.  0:不保留  1:保留

        private int m_reserveremoteattach = 1; //删除帖子时是否保留远程附件    0为不保留  1为保留

        #endregion


       
        public FTPConfigInfo()
		{
        }

        #region 属性


        /// <summary>
        /// 名称
		/// </summary>
        public string Name
		{
            get { return m_name; }
            set { m_name = value; }
		}

        /// <summary>
        /// FTP服务器名称
		/// </summary>
        public string Serveraddress
		{
            get { return m_serveraddress; }
            set { m_serveraddress = value; }
		}

		/// <summary>
        /// FTP端口号
		/// </summary>
        public int Serverport
		{
            get { return m_serverport; }
            set { m_serverport = value; }
		}
		

		/// <summary>
        /// 登陆帐号
		/// </summary>
        public string Username
		{
            get { return m_username; }
            set { m_username = value; }
		}


		/// <summary>
        /// 登陆密码
		/// </summary>
        public string Password
		{
            get { return m_password; }
            set { m_password = value; }
		}

		/// <summary>
        /// 链接模式 1:被动 2:主动
		/// </summary>
        public int Mode
		{
            get { return m_mode <= 0 ? 1 : m_mode; }
            set { m_mode = value <= 0 ? 1 : m_mode; }
		}

     

        /// <summary>
        /// 允许FTP上传附件
		/// </summary>
        public int Allowupload
		{
            get { return m_allowupload; }
            set { m_allowupload = value; }
		}


        /// <summary>
        /// 上传路径
        /// </summary>
        public string Uploadpath
        {
            get { return m_uploadpath; }
            set { m_uploadpath = value; }
        }

        /// <summary>
        /// 无响应时间（FTP在指定时间内无响应）,单位:秒
        /// </summary>
        public int Timeout
        {
            get { return m_timeout <= 0 ? 10 : m_timeout; }
            set { m_timeout = value <= 0 ? 10 : m_timeout; }
        }

        /// <summary>
        /// 远程访问 URL
        /// </summary>
        public string Remoteurl
        {
            get { return m_remoteurl; }
            set { m_remoteurl = value; }
        }

        /// <summary>
        /// 保留本地附件:0为不保留  1为保留
        /// </summary>
        public int Reservelocalattach
        {
            get { return m_reservelocalattach; }
            set { m_reservelocalattach = value; }
        }

        /// <summary>
        /// 删除帖子时是否保留远程附件:0为不保留  1为保留
        /// </summary>
        public int Reserveremoteattach
        {
            get { return m_reserveremoteattach; }
            set { m_reserveremoteattach = value; }
        }
        
        #endregion

    }
}
