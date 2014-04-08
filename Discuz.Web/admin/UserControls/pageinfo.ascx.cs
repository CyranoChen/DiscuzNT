using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace Discuz.Web.Admin
{
    public partial class pageinfo : UserControl
    {
        private IconType _icon = IconType.Information;
        private string _text = "";

        public pageinfo()
        {
            this.Visible = ReadAdminUserConfig();
        }

        /// <summary>
        /// 读取/设置图标
        /// </summary>
        public IconType Icon
        {
            get 
            {
                return _icon;
            }
            set 
            {
                switch (value)
                {
                    case IconType.Information:
                        _icon = IconType.Information;
                        break;
                    case IconType.Warning:
                        _icon = IconType.Warning;
                        break;
                }
            }
        }

        /// <summary>
        /// 读取/设置提示文字
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// 获取图标图片
        /// </summary>
        /// <returns></returns>
        protected string GetInfoImg()
        {
            switch (_icon)
            {
                case IconType.Information:
                    return "../images/hint.gif";
                case IconType.Warning:
                    return "../images/warning.gif";
                default:
                    return "../images/hint.gif";
            }
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public enum IconType
        {
            Information, Warning
        }

        private bool ReadAdminUserConfig()
        {
            string userid = HttpContext.Current.Request.Cookies["dntadmin"]["userid"].ToString();
            string configPath = HttpContext.Current.Server.MapPath("../xml/user_" + userid + ".config");
            if (System.IO.File.Exists(configPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);
                XmlNode showinfo = doc.SelectSingleNode("/UserConfig/ShowInfo");
                if (showinfo != null)
                {
                    return showinfo.InnerText == "1";
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }
}