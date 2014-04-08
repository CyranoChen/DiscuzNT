using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Discuz.Plugin.VerifyImage
{
    /// <summary>
    /// 验证码图片创建类
    /// </summary>
    public class VerifyImageProvider
    {
        private static Hashtable _instance = new Hashtable();
        private static object lockHelper = new object();

        /// <summary>
        /// 获取验证码的类实例
        /// </summary>
        /// <param name="assemlyName">用于区分库文件的名称</param>
        /// <returns></returns>
        public static IVerifyImage GetInstance(string assemlyName)
        {
            if (!_instance.ContainsKey(assemlyName))
            {
                lock (lockHelper)
                {
                    if (!_instance.ContainsKey(assemlyName))
                    {
                        IVerifyImage p = null;
                        try
                        {
                            p = (IVerifyImage)Activator.CreateInstance(Type.GetType(string.Format("Discuz.Plugin.VerifyImage.{0}.VerifyImage, Discuz.Plugin.VerifyImage.{0}", assemlyName), false, true));
                        }
                        catch
                        {
                            p = new Discuz.Plugin.VerifyImage.JpegImage.VerifyImage();
                        }
                        _instance.Add(assemlyName, p);
                    }
                }
            }
            return (IVerifyImage)_instance[assemlyName];
        }
    }
}
