using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using Discuz.Common;

namespace Discuz.Config
{
    public class InvitationConfigs
    {
        private static object lockHelper = new object();

        private static System.Timers.Timer invitationConfigTimer = new System.Timers.Timer(15000);

        private static InvitationConfigInfo m_configinfo;

        /// <summary>
        /// 静态构造函数初始化相应实例和定时器
        /// </summary>
        static InvitationConfigs()
        {
            m_configinfo = InvitationConfigFileManager.LoadConfig();

            invitationConfigTimer.AutoReset = true;
            invitationConfigTimer.Enabled = true;
            invitationConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            invitationConfigTimer.Start();
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
            m_configinfo = InvitationConfigFileManager.LoadConfig();
        }

        public static InvitationConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        #region Helper

        /// <summary>
        /// 序列化配置信息为XML
        /// </summary>
        /// <param name="configinfo">配置信息</param>
        /// <param name="configFilePath">配置文件完整路径</param>
        public static InvitationConfigInfo Serialiaze(InvitationConfigInfo configinfo, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(configinfo, configFilePath);
            }
            return configinfo;
        }


        public static InvitationConfigInfo Deserialize(string configFilePath)
        {
            return (InvitationConfigInfo)SerializationHelper.Load(typeof(InvitationConfigInfo), configFilePath);
        }

        #endregion


    }
}
