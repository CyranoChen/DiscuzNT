using System;
using System.Text;

namespace Discuz.Album.Config
{
    /// <summary>
    /// 相册配置操作类
    /// </summary>
    public class AlbumConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static AlbumConfigInfo GetConfig()
        {
            return AlbumConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(AlbumConfigInfo albumconfiginfo)
        {
            AlbumConfigFileManager acfm = new AlbumConfigFileManager();
            AlbumConfigFileManager.ConfigInfo = albumconfiginfo;
            return acfm.SaveConfig();
        }
    }
}