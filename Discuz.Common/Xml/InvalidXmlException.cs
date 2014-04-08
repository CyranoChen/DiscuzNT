using System;
using System.Text;

namespace Discuz.Common.Xml
{
    public class InvalidXmlException : DNTException
    {
        public InvalidXmlException(string message)
            : base(message)
        {
        }
    }
}
