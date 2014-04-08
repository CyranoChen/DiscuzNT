using System;
using System.Text;

using Discuz.Common;

namespace Discuz.Config
{
    /// <summary>
    /// �������ù�����
    /// </summary>
    public class TradeConfigFileManager : DefaultConfigFileManager
    {
        private static TradeConfigInfo m_configinfo;

        /// <summary>
        /// �ļ��޸�ʱ��
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// ��ʼ���ļ��޸�ʱ��Ͷ���ʵ��
        /// </summary>
        static TradeConfigFileManager()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (TradeConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(TradeConfigInfo));
        }

        /// <summary>
        /// ��ǰ��������ʵ��
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (TradeConfigInfo)value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/trade.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public static TradeConfigInfo LoadConfig()
        {
            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            return ConfigInfo as TradeConfigInfo;
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
