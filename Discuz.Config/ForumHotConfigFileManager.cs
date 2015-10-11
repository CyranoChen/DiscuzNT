using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;

namespace Discuz.Config
{
    class ForumHotConfigFileManager : Discuz.Config.DefaultConfigFileManager
    {
        private static ForumHotConfigInfo m_configinfo;

        private static DateTime m_fileoldchange;

        static ForumHotConfigFileManager()
        {
            if (!Utils.FileExists(ConfigFilePath))
                SerializationHelper.Save(ForumHotConfigInfo.CreateInstance(), ConfigFilePath);

            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (ForumHotConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(ForumHotConfigInfo));
        }

        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (ForumHotConfigInfo)value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/forumhotsetting.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static ForumHotConfigInfo LoadConfig()
        {
            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            return ConfigInfo as ForumHotConfigInfo;
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
