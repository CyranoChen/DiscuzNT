using System;
using System.Text;
using System.Data;
using System.Data.Common;

using Discuz.Config;
using Discuz.Common;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        public IDataReader GetHelpList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}help] ORDER BY [orderby] ASC , [id] ASC", DbFields.HELP, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader ShowHelp(int id)
        {
            string commandText = string.Format("SELECT [title],[message],[pid],[orderby] FROM [{0}help] WHERE [id]={1}", 
                                                BaseConfigs.GetTablePrefix, 
                                                id);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void AddHelp(string title, string message, int pid, int orderBy)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.Char, 100, title),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText,0,message),
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int,4, pid),
                                        DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Int, 4, orderBy)                                        
                                    };
            string commandText = string.Format("INSERT INTO [{0}help]([title],[message],[pid],[orderby]) VALUES(@title,@message,@pid,@orderby)", 
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void DelHelp(string idList)
        {
            string commandText = string.Format("DELETE FROM [{0}help] WHERE [id] IN ({1}) OR [pid] IN ({1})", 
                                                BaseConfigs.GetTablePrefix, 
                                                idList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void UpdateHelp(int id, string title, string message, int pid, int orderBy)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.Char, 100, title),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText,0,message),
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int,4, pid),
                                        DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Int, 4, orderBy),
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id)                                        
                                    };

            string commandText = string.Format("UPDATE [{0}help] SET [title]=@title,[message]=@message,[pid]=@pid,[orderby]=@orderby WHERE [id]=@id", 
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public int HelpCount()
        {
            return TypeConverter.ObjectToInt(
                                DbHelper.ExecuteScalar(CommandType.Text, 
                                                       string.Format("SELECT COUNT(id) FROM [{0}help]", BaseConfigs.GetTablePrefix)));
        }

        public DataTable GetHelpTypes()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}help] WHERE [pid]=0 ORDER BY [orderby] ASC", 
                                                DbFields.HELP, 
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void UpdateOrder(string orderBy, string id)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Char, 100, orderBy),
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.VarChar, 100,id)
                                    };
            string commandText = string.Format("UPDATE [{0}help] SET [ORDERBY]=@orderby  Where id=@id", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }
    }
}
