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

		#region 构造方法
		/// <summary>Creates a new instance of the DefaultTypeFinder.</summary>
		public DefaultTypeFinder()
		{
		}
		#endregion

		#region 属性
		/// <summary>程序集</summary>
		public IList AssembyNames
		{
            get { return m_Assemblies; }
		}

		/// <summary>跳过的程序集</summary>
		public string AssemblySkipLoadingPattern
		{
			get { return m_AssemblySkipLoadingPattern; }
			set { m_AssemblySkipLoadingPattern = value; }
		}

        /// <summary>
        /// 设置搜索程序集的文件目录
        /// </summary>
        public string SearchDirectoryPath
		{
            get { return m_SearchDirectoryPath; }
            set { m_SearchDirectoryPath = value; }
		}

        /// <summary>
        /// 设置搜索程序集的文件名范围
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
        /// 匹配程序集名称
        /// </summary>
        /// <param name="assemblyFullName">程序集名称</param>
        /// <returns>是否匹配成功</returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return Matches(assemblyFullName, this.AssemblySkipLoadingPattern);
        }

        /// <summary>
        /// 匹配程序集名称
        /// </summary>
        /// <param name="assemblyFullName">程序集名称</param>
        /// <param name="pattern">匹配的正则条件</param>
        /// <returns>是否匹配成功</returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(assemblyFullName, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}