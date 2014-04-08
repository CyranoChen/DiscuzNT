using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    ///  Email配置类
    /// </summary>
    public class EmailConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static EmailConfigInfo GetConfig()
        {
            return EmailConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="emailconfiginfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(EmailConfigInfo emailconfiginfo)
        {
            EmailConfigFileManager ecfm = new EmailConfigFileManager();
            EmailConfigFileManager.ConfigInfo = emailconfiginfo;
            return ecfm.SaveConfig();
        }
    }
}
