using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Windows.Interop;
using System.Windows.Browser.Net;
using System.Windows.Browser;

namespace Discuz.Silverlight
{
    public delegate void ADCompleted(object sender, ADMediaHelper admedia);
    public sealed class ADRemote : RemoteBase
    {
        public event ADCompleted OnCompleted;
        public ADRemote()
            : base()
        {
            init();
            this.OnResponseHandle += new ResponseHandle(OnResponse);
            this.ConnectRemote();

        }
        public ADRemote(ADCompleted OnCompleted)
            : base()
        {
            this.OnCompleted += new ADCompleted(OnCompleted);
            this.OnResponseHandle += new ResponseHandle(OnResponse);
            this.ConnectRemote();

        }
        void OnResponse(object sender, StreamReader stream)
        {
            string rawResponse = stream.ReadToEnd();
            XmlReader xr = XmlReader.Create(new StringReader(rawResponse));
            xr.ReadToFollowing("string");
            xr.Read();
            ADMediaHelper _helper = new ADMediaHelper(xr.Value);
            xr.Close();
            if (OnCompleted != null)
                OnCompleted(this, _helper);
        }
        void init()
        {
            this.Function = "ADMedia";
            string pagename = HtmlPage.QueryString["pagename"];
            this.Parameter = "pagename=" + pagename + "&forumid";
            this.Value = HtmlPage.QueryString["forumid"];
        }
    }
    
}
