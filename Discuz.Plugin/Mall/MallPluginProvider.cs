using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Plugin.Mall
{
    public class MallPluginProvider
    {
        private static MallPluginBase _sp;

        private MallPluginProvider() { }

        static MallPluginProvider()
        {
            try
            {
                _sp = (MallPluginBase)Activator.CreateInstance(Type.GetType("Discuz.Mall.MallPlugin, Discuz.Mall", false, true));
            }
            catch
            {
                _sp = null;
            }
        }

        public static MallPluginBase GetInstance()
        {
            return _sp;
        }
    }
}
