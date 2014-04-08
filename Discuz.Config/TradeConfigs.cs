using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// �������ò�����
    /// </summary>
    public class TradeConfigs
    {
        /// <summary>
        /// ��ȡ������ʵ��
        /// </summary>
        /// <returns></returns>
        public static TradeConfigInfo GetConfig()
        {
            return TradeConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(TradeConfigInfo apiconfiginfo)
        {
            TradeConfigFileManager acfm = new TradeConfigFileManager();
            TradeConfigFileManager.ConfigInfo = apiconfiginfo;
            return acfm.SaveConfig();
        }
    }
}

