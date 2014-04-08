using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 空间开通配置类
    /// </summary>
    public class SpaceActiveConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static SpaceActiveConfigInfo GetConfig()
        {
            return SpaceActiveConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(SpaceActiveConfigInfo spaceconfiginfo)
        {
            SpaceActiveConfigFileManager sfm = new SpaceActiveConfigFileManager();
            SpaceActiveConfigFileManager.ConfigInfo = spaceconfiginfo;
            return sfm.SaveConfig();
        }
    }
}
