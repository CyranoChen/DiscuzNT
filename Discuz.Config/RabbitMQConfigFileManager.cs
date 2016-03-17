using System;

using Discuz.Common;

namespace Discuz.Config
{
    public class RabbitMQConfigFileManager: Discuz.Config.DefaultConfigFileManager
    {

        private static RabbitMQConfigInfo m_configinfo;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static RabbitMQConfigFileManager()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                m_configinfo = (RabbitMQConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(RabbitMQConfigInfo));
            }
        }

        /// <summary>
        /// 当前的配置类实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (RabbitMQConfigInfo)value; }
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
                    try
                    {
                        filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/rabbitmq.config");
                    }
                    catch
                    {
                        filename = "../../config/rabbitmq.config";
                    }
                }
                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static RabbitMQConfigInfo LoadConfig()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
                return ConfigInfo as RabbitMQConfigInfo;
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
