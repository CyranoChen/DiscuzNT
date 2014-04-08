#if NET1
#else
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Discuz.Data
{
    public class DbProviderFinder : Discuz.Common.TypeFinder.DefaultTypeFinder
    {
        /// <summary>
        /// 获得符合Discuz.Common.Database.IDbProvider接口和命名规范的全部程序集列表
        /// </summary>
        /// <returns>程序集列表</returns>
        public override IList<Assembly> GetFilteredAssembliyList()
        {
            this.SearchDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            this.SearchPattern = "discuz.dbproviders.*.dll";
            return base.GetFilteredAssembliyList();
        }


        /// <summary>
        /// 查找指定的符合Discuz.Common.Database.IDbProvider接口标准的程序集并返回其DbFactory实例
        /// </summary>
        /// <param name="proviername">名称</param>
        /// <returns>DbFactory实例</returns>
        public IDbProvider GetDbProvider(string proviername)
        {
            proviername = proviername.ToLower();
            //switch (proviername)
            //{
            //    case "oledb":
            //        return OleDbFactory as IDbProvider;
            //    case "sqlserver":
            //        return SqlClientFactory;
            //    case "odbc":
            //        return OdbcFactory;
            //}
            
            foreach (Assembly a in this.GetFilteredAssembliyList())
            {

                if (a.FullName.ToLower().StartsWith("discuz.dbproviders"))
                {
                    foreach (Type t in a.GetExportedTypes())
                    {

                        if (t.IsClass && typeof(IDbProvider).IsAssignableFrom(t))
                        {

                            IDbProvider DbProvider = (IDbProvider)Activator.CreateInstance(t);
                            if (DbProvider != null)
                            {
                                return DbProvider;
                            }
                        }
                    }
                    
                }

                
            }
            //如果无法找到指定的符合Discuz.Common.Database.IDbProvider接口标准的程序集, 则返回SqlClientFactory实例
            return new SqlServerProvider();
        }

        
    }
}

#endif
