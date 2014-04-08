using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 脚本任务计划类
    /// </summary>
    public class ScriptEventConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static ScriptEventConfigInfo GetConfig()
        {
            return ScriptEventConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(ScriptEventConfigInfo scripteventconfiginfo)
        {
            ScriptEventConfigFileManager scfm = new ScriptEventConfigFileManager();
            ScriptEventConfigFileManager.ConfigInfo = scripteventconfiginfo;
            return scfm.SaveConfig();
        }
    }
}
