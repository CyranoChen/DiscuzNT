using System;
using System.Text;
using Discuz.Common;

namespace Discuz.Config
{
    /// <summary>
    /// 空间开通配置管理类
    /// </summary>
    class SpaceActiveConfigFileManager : Discuz.Config.DefaultConfigFileManager
    {
        private static SpaceActiveConfigInfo m_configinfo;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        public static DateTime m_fileoldchange;


        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static SpaceActiveConfigFileManager()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (SpaceActiveConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(SpaceActiveConfigInfo));
        }

        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (SpaceActiveConfigInfo)value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/space.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static SpaceActiveConfigInfo LoadConfig()
        {
            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            return ConfigInfo as SpaceActiveConfigInfo;
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
