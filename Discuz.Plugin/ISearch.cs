using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Discuz.Plugin
{
    public interface ISearch
    {
        DataTable GetResult(int pagesize, string idstr);
    }
}
