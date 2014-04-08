using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Windows.Interop;
using System.Windows.Browser.Net;
using System.Windows.Browser;

namespace Discuz.Silverlight
{
    public delegate void ResponseHandle(object sender,StreamReader stream);
    public abstract class RemoteBase
    {
        public RemoteBase()
        {
            //Connectremote();
        }
        protected virtual void ConnectRemote()
        {
            try
            {
                
                string serverUri = HtmlPage.DocumentUri.ToString();
                int thisApp = serverUri.IndexOf(SubSite);

                serverUri = serverUri.Substring(0, thisApp) + WebService;
               
                System.Uri webServiceUri = new System.Uri(serverUri + Function + Parameter + Value);

                _request = new BrowserHttpWebRequest(webServiceUri);
                
                IAsyncResult iar = _request.BeginGetResponse(new AsyncCallback(OnResponseDownload),
                    _request);
                this.Caption = serverUri + Function + Parameter + Value;
            }

            catch (Exception ex)
            {
                _caption = ex.ToString();
                
                
            }
        }
        protected void OnResponseDownload(IAsyncResult iar)
        {
            try
            {
                this.Caption = "OnResponseDownload";
                HttpWebResponse response =
                    ((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ApplicationException("HttpStatusCode " +
                        response.StatusCode.ToString() + " was returned.");
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                if (OnResponseHandle != null)
                    OnResponseHandle(this, responseReader);
                responseReader.Close();
                response.Close();
            }

            catch (Exception ex)
            {
                _caption = ex.ToString();
                //this.msg.Text = ex.ToString();
            }
            finally
            {
            }
        }

        public string SubSite
        {
            get { return _subsiteurl; }
            set { _subsiteurl = prefixBias(value); }
        }
        public string WebService
        {
            get { return _webserviceurl; }
            set { _webserviceurl = prefixBias(value); }
        }
        public string Function
        {
            get { return _function; }
            set { _function = prefixBias(value); }
        }
        public string Parameter
        {
            get { return _parameter; }
            set { _parameter = prefixinterrogation(value); }
        }
        public string Value
        {
            get { return _value; }
            set { _value = prefixequal(value); }
        }
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }
        
        public ResponseHandle OnResponseHandle;
        private string _subsiteurl = "/silverlight";
        private string _webserviceurl = "/services/MixObjects.asmx";
        private string _function = "";
        private string _parameter = "";
        private string _value = "";
        private string _caption = "";
        BrowserHttpWebRequest _request;
        private string prefixBias(string value)
        {
            if (value == null || value.Trim().Length == 0)
                return "";
            string _value = "";
            if (!value.StartsWith("/"))
            {
                return _value = "/" + value;
            }
            return value;
        }
        private string prefixinterrogation(string value)
        {
            if (value == null || value.Trim().Length == 0)
                return "";
            string _value = "";
            if (!value.StartsWith("?"))
            {
                return _value = "?" + value;
            }
            return value;
        }
        private string prefixequal(string value)
        {
            if (value == null || value.Trim().Length == 0)
                return "";
            string _value = "";
            if (!value.StartsWith("="))
            {
                return _value = "=" + value;
            }
            return value;
        }
    }
}
