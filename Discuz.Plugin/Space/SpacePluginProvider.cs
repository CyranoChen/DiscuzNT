using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Plugin.Space
{
    public class SpacePluginProvider
    {
        private static SpacePluginBase _sp;

        private SpacePluginProvider() { }

        static SpacePluginProvider()
        {
            try
            {
                _sp = (SpacePluginBase)Activator.CreateInstance(Type.GetType("Discuz.Space.SpacePlugin, Discuz.Space", false, true));
            }
            catch
            {
                _sp = null;
            }
        }

        public static SpacePluginBase GetInstance()
        {
            return _sp;
        }


    }
}
