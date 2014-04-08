using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Plugin.PasswordMode
{
    public class PasswordModeProvider
    {
        private static IPasswordMode _passwordMode;

        private PasswordModeProvider() { }

        static PasswordModeProvider()
        {
            try
            {
                _passwordMode = (IPasswordMode)Activator.CreateInstance(Type.GetType("Discuz.Plugin.PasswordMode.ThirdPartMode, Discuz.Plugin.PasswordMode", false, true));
            }
            catch
            {
                _passwordMode = null;
            }
        }

        public static IPasswordMode GetInstance()
        {
            //_passwordMode = (IPasswordMode)Activator.CreateInstance(Type.GetType("Discuz.Plugin.PasswordMode.ThirdPartMode, Discuz.Plugin.PasswordMode", false, true));
            return _passwordMode;
        }
    }
}
