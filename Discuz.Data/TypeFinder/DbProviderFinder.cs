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
        /// ��÷���Discuz.Common.Database.IDbProvider�ӿں������淶��ȫ�������б�
        /// </summary>
        /// <returns>�����б�</returns>
        public override IList<Assembly> GetFilteredAssembliyList()
        {
            this.SearchDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            this.SearchPattern = "discuz.dbproviders.*.dll";
            return base.GetFilteredAssembliyList();
        }


        /// <summary>
        /// ����ָ���ķ���Discuz.Common.Database.IDbProvider�ӿڱ�׼�ĳ��򼯲�������DbFactoryʵ��
        /// </summary>
        /// <param name="proviername">����</param>
        /// <returns>DbFactoryʵ��</returns>
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
            //����޷��ҵ�ָ���ķ���Discuz.Common.Database.IDbProvider�ӿڱ�׼�ĳ���, �򷵻�SqlClientFactoryʵ��
            return new SqlServerProvider();
        }

        
    }
}

#endif
