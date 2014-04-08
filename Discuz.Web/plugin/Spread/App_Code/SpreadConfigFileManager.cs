using System;
using System.Text;

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Plugin.Spread.Config
{
    class SpreadConfigFileManager : Discuz.Config.DefaultConfigFileManager
    {
         private static SpreadConfigInfo m_configinfo ;
        
        /// <summary>
        /// 程序刚加载时config文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        static SpreadConfigFileManager()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (SpreadConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(SpreadConfigInfo));
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (SpreadConfigInfo)value; }
        }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                return Utils.GetMapPath(BaseConfigs.GetForumPath + "config/spread.config");
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public static SpreadConfigInfo LoadConfig()
        {
            //return base.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            return ConfigInfo as SpreadConfigInfo;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
