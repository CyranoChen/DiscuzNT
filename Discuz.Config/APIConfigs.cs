using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 相册配置操作类
    /// </summary>
    public class APIConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static APIConfigInfo GetConfig()
        {
            return APIConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(APIConfigInfo apiconfiginfo)
        {
            APIConfigFileManager acfm = new APIConfigFileManager();
            APIConfigFileManager.ConfigInfo = apiconfiginfo;
            return acfm.SaveConfig();
        }
    }
}