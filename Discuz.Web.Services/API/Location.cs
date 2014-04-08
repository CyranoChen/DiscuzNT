using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using System.Xml.Serialization;

namespace Discuz.Web.Services.API
{
    public class Location
    {
        [XmlElement("street")]
        public string Street;

        [XmlElement("city")]
        public string City;

        [XmlElement("state")]
        public string State;

        [XmlElement("country")]
        public string Country;

        [XmlElement("zip")]
        public string Zip;
    }
}
