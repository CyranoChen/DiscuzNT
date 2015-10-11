﻿using System;

using Discuz.Common;

namespace Discuz.Config
{
    class DbSnapConfigFileManager : Discuz.Config.DefaultConfigFileManager
    {
        private static DbSnapAppConfig m_dbSnapAppConfig;
        
        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static DbSnapConfigFileManager()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                m_dbSnapAppConfig = (DbSnapAppConfig)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(DbSnapAppConfig));
            }
        }

        /// <summary>
        /// 当前的配置实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_dbSnapAppConfig; }
            set { m_dbSnapAppConfig = (DbSnapAppConfig) value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/dbsnap.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static DbSnapAppConfig LoadConfig()
        {
            if (Utils.FileExists(ConfigFilePath))
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);

            return ConfigInfo as DbSnapAppConfig;
               
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
