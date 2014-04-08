#if NET1

using System;
using System.Data;

using System.Text;

namespace Discuz.Data
{
    public interface IDbProviderFactory
    {
        IDbConnection CreateConnection();
        IDbCommand CreateCommand();
        IDbDataAdapter CreateDataAdapter();
       // void Dispose(bool disposing);
    }
}

#endif