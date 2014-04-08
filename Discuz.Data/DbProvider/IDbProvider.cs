using System;
using System.Data;
using System.Data.Common;

namespace Discuz.Data
{
    public interface IDbProvider
    {
        /// <summary>
        /// ����DbProviderFactoryʵ��
        /// </summary>
        /// <returns></returns>
        DbProviderFactory Instance();
        /// <summary>
        /// ����SQL������Ϣ�����
        /// </summary>
        /// <param name="cmd"></param>
        void DeriveParameters(IDbCommand cmd);

        /// <summary>
        /// ����SQL����
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size);
        /// <summary>
        /// �Ƿ�֧��ȫ������
        /// </summary>
        /// <returns></returns>
        bool IsFullTextSearchEnabled();

        /// <summary>
        /// �Ƿ�֧��ѹ�����ݿ�
        /// </summary>
        /// <returns></returns>
        bool IsCompactDatabase();

        /// <summary>
        /// �Ƿ�֧�ֱ������ݿ�
        /// </summary>
        /// <returns></returns>
        bool IsBackupDatabase();

        /// <summary>
        /// ���ظղ����¼������IDֵ, �粻֧����Ϊ""
        /// </summary>
        /// <returns></returns>
        string GetLastIdSql();
        /// <summary>
        /// �Ƿ�֧�����ݿ��Ż�
        /// </summary>
        /// <returns></returns>
        bool IsDbOptimize();
        /// <summary>
        /// �Ƿ�֧�����ݿ�����
        /// </summary>
        /// <returns></returns>
        bool IsShrinkData();
        /// <summary>
        /// �Ƿ�֧�ִ洢����
        /// </summary>
        /// <returns></returns>
        bool IsStoreProc();
    }
}
