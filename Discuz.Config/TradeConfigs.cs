using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 交易配置操作类
    /// </summary>
    public class TradeConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static TradeConfigInfo GetConfig()
        {
            return TradeConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(TradeConfigInfo apiconfiginfo)
        {
            TradeConfigFileManager acfm = new TradeConfigFileManager();
            TradeConfigFileManager.ConfigInfo = apiconfiginfo;
            return acfm.SaveConfig();
        }
    }
}

