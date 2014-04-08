using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Discuz.Web.Admin.AutoUpdateManager
{
    /// <summary>
    /// 此类为引用webservice时系统自动生成
    /// </summary>
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [WebServiceBinding(Name = "CatchSoftInfoSoap", Namespace = "http://tempuri.org/")]

    public class AutoUpdate : SoapHttpClientProtocol
    {
        public AutoUpdate()
        {
            this.Url = "http://service.nt.discuz.net/AutoUpdate.asmx";
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetFile", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] GetFile(string dbtype, bool isrequired, string version, string filename)
        {
            object[] results = this.Invoke("GetFile", new object[] {
                        dbtype,
                        isrequired,
                        version,
                        filename});
            return ((byte[])(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetVersionList", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetVersionList()
        {
            object[] results = this.Invoke("GetVersionList", new object[0]);
            return ((string)(results[0]));
        }
    }
}
