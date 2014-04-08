using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using Discuz.Common;

namespace Discuz.Config
{
	/// <summary>
	/// 论坛基本设置类
	/// </summary>
    public class MyAttachmentsTypeConfigs
	{
		private static object lockHelper = new object();

        private static System.Timers.Timer generalConfigTimer = new System.Timers.Timer(15000);

        private static MyAttachmentsTypeConfigInfo m_configinfo;

        /// <summary>
        /// 静态构造函数初始化相应实例和定时器
        /// </summary>
        static MyAttachmentsTypeConfigs()
        {
            m_configinfo = MyAttachmentsTypeConfigFileManager.LoadConfig();

            generalConfigTimer.AutoReset = true;
            generalConfigTimer.Enabled = true;
            generalConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            generalConfigTimer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = MyAttachmentsTypeConfigFileManager.LoadConfig();
        }

        public static MyAttachmentsTypeConfigInfo GetConfig()
		{
            return m_configinfo;
  		}

        

		/// <summary>
		/// 获取默认模板id
		/// </summary>
		/// <returns></returns>
        //public static int GetDefaultTemplateID()
        //{
        //     return GetConfig().Templateid;
        //}



		/// <summary>
		/// 获得设置项信息
		/// </summary>
		/// <returns>设置项</returns>
		public static bool SetIpDenyAccess(string denyipaccess)
		{
			bool result;
			
			lock(lockHelper) 
			{
				try
				{
                    MyAttachmentsTypeConfigInfo configInfo = MyAttachmentsTypeConfigs.GetConfig();
                    //configInfo.Ipdenyaccess = configInfo.Ipdenyaccess + "\n" + denyipaccess;
                    MyAttachmentsTypeConfigs.Serialiaze(configInfo, Utils.GetMapPath(BaseConfigs.GetForumPath + "config/MyAttachmentsTypeConfiginfo.config"));
					result = true;
				}
				catch
				{
					return false;
				}
	
			}
			return result;

		}




		#region Helper

		/// <summary>
		/// 序列化配置信息为XML
		/// </summary>
		/// <param name="configinfo">配置信息</param>
		/// <param name="configFilePath">配置文件完整路径</param>
        public static MyAttachmentsTypeConfigInfo Serialiaze(MyAttachmentsTypeConfigInfo configinfo, string configFilePath)
		{
			lock(lockHelper) 
			{
				SerializationHelper.Save(configinfo, configFilePath);
			}
			return configinfo;
		}


        public static MyAttachmentsTypeConfigInfo Deserialize(string configFilePath)
		{
            return (MyAttachmentsTypeConfigInfo)SerializationHelper.Load(typeof(MyAttachmentsTypeConfigInfo), configFilePath);
		}

		#endregion



	}
}
