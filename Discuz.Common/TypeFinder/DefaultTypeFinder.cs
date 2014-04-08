using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.IO;

namespace Discuz.Common.TypeFinder
{
	
    public class DefaultTypeFinder : Discuz.Common.TypeFinder.ITypeFinder
    {
		#region Private Fields
		private string m_AssemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft";
		private string m_SearchDirectoryPath = "";
        private string m_SearchPattern = "*.dll";
        private List<Assembly> m_Assemblies = new List<Assembly>();
		#endregion

		#region ���췽��
		/// <summary>Creates a new instance of the DefaultTypeFinder.</summary>
		public DefaultTypeFinder()
		{
		}
		#endregion

		#region ����
		/// <summary>����</summary>
		public IList AssembyNames
		{
            get { return m_Assemblies; }
		}

		/// <summary>�����ĳ���</summary>
		public string AssemblySkipLoadingPattern
		{
			get { return m_AssemblySkipLoadingPattern; }
			set { m_AssemblySkipLoadingPattern = value; }
		}

        /// <summary>
        /// �����������򼯵��ļ�Ŀ¼
        /// </summary>
        public string SearchDirectoryPath
		{
            get { return m_SearchDirectoryPath; }
            set { m_SearchDirectoryPath = value; }
		}

        /// <summary>
        /// �����������򼯵��ļ�����Χ
        /// </summary>
        public string SearchPattern
		{
            get { return m_SearchPattern; }
            set { m_SearchPattern = value; }
		}
        #endregion

        public virtual IList<Assembly> GetFilteredAssemblyList()
        {

			foreach (string dllPath in Directory.GetFiles(m_SearchDirectoryPath, m_SearchPattern))
			{
				try
				{
                    Assembly a = Assembly.LoadFrom(dllPath);
                    if (!Matches(a.FullName))
                        m_Assemblies.Add(a);
				}
				catch (BadImageFormatException ex)
				{
					Trace.TraceError(ex.ToString());
				}
			}
		
			return m_Assemblies;
        }



        /// <summary>
        /// ƥ���������
        /// </summary>
        /// <param name="assemblyFullName">��������</param>
        /// <returns>�Ƿ�ƥ��ɹ�</returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return Matches(assemblyFullName, this.AssemblySkipLoadingPattern);
        }

        /// <summary>
        /// ƥ���������
        /// </summary>
        /// <param name="assemblyFullName">��������</param>
        /// <param name="pattern">ƥ�����������</param>
        /// <returns>�Ƿ�ƥ��ɹ�</returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(assemblyFullName, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}