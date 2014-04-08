namespace Discuz.Config
{
    /// <summary>
    ///  MemCached配置类
    /// </summary>
    public class MemCachedConfigs
    {
        private static System.Timers.Timer memCachedConfigTimer = new System.Timers.Timer(600000);//间隔为10分钟

        private static MemCachedConfigInfo m_configinfo;

        static MemCachedConfigs()
        {
            m_configinfo = MemCachedConfigFileManager.LoadConfig();
            memCachedConfigTimer.AutoReset = true;
            memCachedConfigTimer.Enabled = true;
            memCachedConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            memCachedConfigTimer.Start();
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
            m_configinfo = MemCachedConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static MemCachedConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="emailconfiginfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(MemCachedConfigInfo configinfo)
        {
            MemCachedConfigFileManager mccfm = new MemCachedConfigFileManager();
            MemCachedConfigFileManager.ConfigInfo = configinfo;
            return mccfm.SaveConfig();
        }
    }
}
