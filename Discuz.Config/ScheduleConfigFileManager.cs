using System;
using System.Text;
using System.Web;
using System.IO;

using Discuz.Common;
using System.Xml.Serialization;
using System.Xml;


namespace Discuz.Config
{
    /// <summary>
    /// 计划任务设置管理类
    /// </summary>
    class ScheduleConfigFileManager : DefaultConfigFileManager
    {
        private static ScheduleConfigInfo m_configinfo;

      
        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static ScheduleConfigFileManager()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (ScheduleConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(ScheduleConfigInfo));
        }

        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (ScheduleConfigInfo) value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/schedule.config");
                    if (!File.Exists(filename))
                    {
                        filename = Utils.GetMapPath("~/config/schedule.config");
                    }
                }

                return filename;
            }

        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static ScheduleConfigInfo LoadConfig()
        {

            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, true);
            return  ConfigInfo as ScheduleConfigInfo;
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

