using System;

using Discuz.Common;

namespace Discuz.Config
{
    public class RabbitMQConfigs
    {
        private static System.Timers.Timer rabbitMQConfigTimer = new System.Timers.Timer(600000);//间隔为10分钟

        private static RabbitMQConfigInfo m_configinfo;

        static RabbitMQConfigs()
        {
            m_configinfo = RabbitMQConfigFileManager.LoadConfig();
            rabbitMQConfigTimer.AutoReset = true;
            rabbitMQConfigTimer.Enabled = true;
            rabbitMQConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            rabbitMQConfigTimer.Start();
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
            m_configinfo = RabbitMQConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static RabbitMQConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(RabbitMQConfigInfo configinfo)
        {
            RabbitMQConfigFileManager rmcfm = new RabbitMQConfigFileManager();
            RabbitMQConfigFileManager.ConfigInfo = configinfo;
            return rmcfm.SaveConfig();
        }
    }
}
