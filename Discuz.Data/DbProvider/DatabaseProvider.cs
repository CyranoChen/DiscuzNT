using System;
using System.Text;
using System.Reflection;
using Discuz.Config;

namespace Discuz.Data
{
	public class DatabaseProvider
	{
		private DatabaseProvider()
		{ }

		private static IDataProvider _instance = null;
		private static object lockHelper = new object();

		static DatabaseProvider()
		{
			GetProvider();
		}

		private static void GetProvider()
		{
			try
			{
				_instance = (IDataProvider)Activator.CreateInstance(Type.GetType(string.Format("Discuz.Data.{0}.DataProvider, Discuz.Data.{0}", BaseConfigs.GetDbType), false, true));
			}
			catch
			{
				throw new Exception("请检查DNT.config中Dbtype节点数据库类型是否正确，例如：SqlServer、Access、MySql");
			}
		}

		public static IDataProvider GetInstance()
		{
			if (_instance == null)
			{
				lock (lockHelper)
				{
					if (_instance == null)
					{
						GetProvider();
					}
				}
			}
			return _instance;
		}

        public static void ResetDbProvider()
        {
            _instance = null; 
        }
	}
}
