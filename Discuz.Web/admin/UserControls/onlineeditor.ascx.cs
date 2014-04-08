using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Discuz.Control;

namespace Discuz.Web.Admin
{
    /// <summary>
    ///	ÔÚÏß±à¼­¿Ø¼þ
    /// </summary>
    public partial class OnlineEditor : UserControl
    {
        public int postminchars = 0;
        public int postmaxchars = 200;     
        public string text = "";
        public string Text
        {
            set { text = value; }
            get { return Discuz.Common.DNTRequest.GetString(ID + "message_hidden").Replace("'", "''"); }
        }
    }
}