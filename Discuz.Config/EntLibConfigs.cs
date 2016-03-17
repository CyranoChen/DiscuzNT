using System;

using Discuz.Common;

namespace Discuz.Config
{
    /// <summary>
    ///  企业版配置类
    /// </summary>
    public class EntLibConfigs
    {
        private static System.Timers.Timer loadBalanceConfigTimer = new System.Timers.Timer(600000);//间隔为10分钟

        private static EntLibConfigInfo m_configinfo;

        static EntLibConfigs()
        {
            m_configinfo = EntLibConfigFileManager.LoadConfig();
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
            m_configinfo = EntLibConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static EntLibConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(EntLibConfigInfo configinfo)
        {
            EntLibConfigFileManager lbcfm = new EntLibConfigFileManager();
            EntLibConfigFileManager.ConfigInfo = configinfo;
            return lbcfm.SaveConfig();
        }
    }
}
