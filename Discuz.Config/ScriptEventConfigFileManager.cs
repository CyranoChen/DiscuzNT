using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;
using System.IO;

namespace Discuz.Config
{
    /// <summary>
    /// �ű���ƻ����������ļ�������
    /// </summary>
    public class ScriptEventConfigFileManager : DefaultConfigFileManager
    {
        private static ScriptEventConfigInfo m_configinfo = new ScriptEventConfigInfo();

      
        /// <summary>
        /// �ļ��޸�ʱ��
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// ��ʼ���ļ��޸�ʱ��Ͷ���ʵ��
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
        /// �����ļ�����·��
        /// </summary>
        public static string filename = null;


        /// <summary>
        /// ��ȡ�����ļ�����·��
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
        /// ����������ʵ��
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
        /// ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
