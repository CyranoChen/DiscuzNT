using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    ///  聚合数据提取操作类
    /// </summary>
    public class AggregationConfig
    {
        /// <summary>
        /// 锁变量
        /// </summary>
        private static object lockHelper = new object();

        /// <summary>
        /// 更新定时器
        /// </summary>
        private static System.Timers.Timer aggregationConfigTimer = new System.Timers.Timer(15000);


        /// <summary>
        /// 聚合数据提取设置类实例
        /// </summary>
        private static AggregationConfigInfo m_configinfo;

        /// <summary>
        /// 静态构造函数初始化相应实例和定时器
        /// </summary>
        static AggregationConfig()
        {
            m_configinfo = AggregationConfigFileManager.LoadConfig();

            aggregationConfigTimer.AutoReset = true;
            aggregationConfigTimer.Enabled = true;
            aggregationConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            aggregationConfigTimer.Start();
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
            m_configinfo = AggregationConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取聚合配置类实例
        /// </summary>
        /// <returns></returns>
        public static AggregationConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(AggregationConfigInfo aggregationconfiginfo)
        {
            AggregationConfigFileManager acfm = new AggregationConfigFileManager();
            AggregationConfigFileManager.ConfigInfo = aggregationconfiginfo;
            return acfm.SaveConfig();
        }
    }
}
