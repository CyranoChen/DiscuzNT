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
        /// ����ռ���ʱconfig�ļ��޸�ʱ��
        /// </summary>
        private static DateTime m_fileoldchange;

        static SpreadConfigFileManager()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (SpreadConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(SpreadConfigInfo));
        }

        /// <summary>
        /// ���ö���
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (SpreadConfigInfo)value; }
        }

        /// <summary>
        /// �����ļ�·��
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                return Utils.GetMapPath(BaseConfigs.GetForumPath + "config/spread.config");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public static SpreadConfigInfo LoadConfig()
        {
            //return base.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            return ConfigInfo as SpreadConfigInfo;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
