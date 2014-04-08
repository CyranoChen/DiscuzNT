using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Plugin.Album
{
    public class AlbumPluginProvider
    {
        private static AlbumPluginBase _sp;

        private AlbumPluginProvider() { }

        static AlbumPluginProvider()
        {
            try
            {
                _sp = (AlbumPluginBase)Activator.CreateInstance(Type.GetType("Discuz.Album.AlbumPlugin, Discuz.Album", false, true));
            }
            catch
            {
                _sp = null;
            }
        }

        public static AlbumPluginBase GetInstance()
        {
            return _sp;
        }
    }
}
