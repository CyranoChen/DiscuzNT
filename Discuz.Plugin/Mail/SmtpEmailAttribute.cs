using System;
using System.Text;

namespace Discuz.Plugin.Mail
{

    #region 定制发送邮件的插件属性值

    /// <summary>
    /// 定制发送邮件的插件属性值
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SmtpEmailAttribute : System.Attribute
    {
        private string _plugInName;

        private string _version = null;

        private string _author = null;

        private string _dllFileName = null;


        public SmtpEmailAttribute(string Name) : base()
        {
            _plugInName = Name;
            return;
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string PlugInName
        {
            get { return _plugInName; }
            set { _plugInName = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// DLL 文件名称
        /// </summary>
        public string DllFileName
        {
            get { return _dllFileName; }
            set { _dllFileName = value; }
        }
    }
    #endregion

}
