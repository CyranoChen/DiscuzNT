using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// �ű�����ƻ���
    /// </summary>
    public class ScriptEventConfigs
    {
        /// <summary>
        /// ��ȡ������ʵ��
        /// </summary>
        /// <returns></returns>
        public static ScriptEventConfigInfo GetConfig()
        {
            return ScriptEventConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(ScriptEventConfigInfo scripteventconfiginfo)
        {
            ScriptEventConfigFileManager scfm = new ScriptEventConfigFileManager();
            ScriptEventConfigFileManager.ConfigInfo = scripteventconfiginfo;
            return scfm.SaveConfig();
        }
    }
}
