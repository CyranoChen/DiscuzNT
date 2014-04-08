using System;

using Discuz.Common;

namespace Discuz.Config
{
    /// <summary>
    ///  负载均衡配置类
    /// </summary>
    public class LoadBalanceConfigs
    {
        private static System.Timers.Timer loadBalanceConfigTimer = new System.Timers.Timer(600000);//间隔为10分钟

        private static LoadBalanceConfigInfo m_configinfo;

        static LoadBalanceConfigs()
        {
            m_configinfo = LoadBalanceConfigFileManager.LoadConfig();
            loadBalanceConfigTimer.AutoReset = true;
            loadBalanceConfigTimer.Enabled = true;
            loadBalanceConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            loadBalanceConfigTimer.Start();
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
            m_configinfo = LoadBalanceConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static LoadBalanceConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(LoadBalanceConfigInfo configinfo)
        {
            LoadBalanceConfigFileManager lbcfm = new LoadBalanceConfigFileManager();
            LoadBalanceConfigFileManager.ConfigInfo = configinfo;
            return lbcfm.SaveConfig();
        }
    }
}
