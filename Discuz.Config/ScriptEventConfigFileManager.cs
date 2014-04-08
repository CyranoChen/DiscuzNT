using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;
using System.IO;

namespace Discuz.Config
{
    /// <summary>
    /// 脚本类计划任务配置文件管理类
    /// </summary>
    public class ScriptEventConfigFileManager : DefaultConfigFileManager
    {
        private static ScriptEventConfigInfo m_configinfo = new ScriptEventConfigInfo();

      
        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static ScriptEventConfigFileManager()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);

            //m_configinfo = (ScriptEventConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(ScriptEventConfigInfo));
        }

        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (ScriptEventConfigInfo)value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/scriptevent_" + BaseConfigs.GetDbType + ".config").ToLower();
                }

                return filename;
            }

        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static ScriptEventConfigInfo LoadConfig()
        {
            try
            {
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, false);
            }
            catch {
                if (!File.Exists(ConfigFilePath))
                {
                    ScriptEventConfigFileManager secf = new ScriptEventConfigFileManager();
                    secf.SaveConfig(ConfigFilePath, ConfigInfo);
                }
            }

            return ConfigInfo as ScriptEventConfigInfo;
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
