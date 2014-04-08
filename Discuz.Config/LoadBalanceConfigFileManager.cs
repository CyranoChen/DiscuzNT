using System;

using Discuz.Common;

namespace Discuz.Config
{

    /// <summary>
    /// 负载均衡配置管理类
    /// </summary>
    public class LoadBalanceConfigFileManager : DefaultConfigFileManager
    {
        private static LoadBalanceConfigInfo m_configinfo;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static LoadBalanceConfigFileManager()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                m_configinfo = (LoadBalanceConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(LoadBalanceConfigInfo));
            }
        }

        /// <summary>
        /// 当前的配置类实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (LoadBalanceConfigInfo)value; }
        }

        /// <summary>
        /// 配置文件所在路径
        /// </summary>
        public static string filename = null;


        /// <summary>
        /// 获取配置文件所在路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                if (filename == null)
                {
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/loadbalance.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static LoadBalanceConfigInfo LoadConfig()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
                return ConfigInfo as LoadBalanceConfigInfo;
            }
            else
                return null;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
