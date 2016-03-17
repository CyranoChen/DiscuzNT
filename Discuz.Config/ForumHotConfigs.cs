using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{
    public class ForumHotConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static ForumHotConfigInfo GetConfig()
        {
            return ForumHotConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(ForumHotConfigInfo forumHotConfigInfo)
        {
            ForumHotConfigFileManager fhcfm = new ForumHotConfigFileManager();
            ForumHotConfigFileManager.ConfigInfo = forumHotConfigInfo;
            return fhcfm.SaveConfig();
        }
    }
}
