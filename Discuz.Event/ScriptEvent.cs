using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Config;
using Discuz.Data;
using System.Reflection;
using Discuz.Common;
using Discuz.Forum.ScheduledEvents;

namespace Discuz.Event
{
    /// <summary>
    /// 执行脚本的任务
    /// </summary>
    public class ScriptEvent : IEvent
    {
        private static ScriptEventConfigInfo scriptevents = ScriptEventConfigs.GetConfig();

        private static IConfigInfo[] configs = { GeneralConfigs.GetConfig(), BaseConfigs.GetBaseConfig() };

        #region IEvent 成员

        public void Execute(object state)
        {
            //ScriptEventConfigFileManager.filename = EventManager.RootPath + "config\\scriptevent.config";
            foreach (ScriptEventInfo sei in scriptevents.ScriptEvents)
            {
                if (sei.Enabled && sei.ShouldExecute)
                {
                    StringBuilder script = new StringBuilder(sei.Script.Replace("dnt_", BaseConfigs.GetTablePrefix));

                    #region 内置关键字和config
                    foreach (IConfigInfo config in configs)
                    {
                        foreach (PropertyInfo p in config.GetType().GetProperties())
                        {
                            script.Replace(string.Format("{{{0}.{1}}}", config.GetType().Name.ToLower().Replace("info", ""), p.Name.ToLower()), p.GetValue(config, null).ToString());
                        }
                    }

                    script.Replace("{nowdate}", Utils.GetDate());
                    #endregion

                    DatabaseProvider.GetInstance().RunSql(script.ToString());
                }
            }
        }

        #endregion
    }
}
