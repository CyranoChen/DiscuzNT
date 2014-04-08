using System;
using System.Text;

namespace Discuz.Plugin.Spread.Config
{
    public class SpreadConfigs
    {
        /// <summary>
        /// ªÒ»°≈‰÷√
        /// </summary>
        /// <returns></returns>
        public static SpreadConfigInfo GetConfig()
        {
            //return (new SpreadConfigFileManager()).LoadConfig() as SpreadConfigInfo;
            return SpreadConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// ±£¥Ê≈‰÷√
        /// </summary>
        /// <param name="spreadconfiginfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(SpreadConfigInfo spreadconfiginfo)
        {
            SpreadConfigFileManager ecfm = new SpreadConfigFileManager();
            SpreadConfigFileManager.ConfigInfo = spreadconfiginfo;
            return ecfm.SaveConfig();
        }
    }
}
