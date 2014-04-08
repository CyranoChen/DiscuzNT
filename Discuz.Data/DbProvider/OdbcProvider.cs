using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;

using Discuz.Common;

namespace Discuz.Data
{
    public class OdbcProvider : IDbProvider
    {
        public DbProviderFactory Instance()
        {
            return OdbcFactory.Instance;
        }

        public void DeriveParameters(IDbCommand cmd)
        {
            if ((cmd as OdbcCommand) != null)
            {
                OdbcCommandBuilder.DeriveParameters(cmd as OdbcCommand);
            }
        }

        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size)
        {
            OdbcParameter param;

            if (Size > 0)
                param = new OdbcParameter(ParamName, (OdbcType)DbType, Size);
            else
                param = new OdbcParameter(ParamName, (OdbcType)DbType);

            return param;
        }

        public bool IsFullTextSearchEnabled()
        {
            return false;
        }

        public bool IsCompactDatabase()
        {
            return false;
        }

        public bool IsBackupDatabase()
        {
            return false;
        }
    }
}
