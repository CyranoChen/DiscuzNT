using System;
using System.Collections;
using Discuz.Space.Utilities;

namespace Discuz.Space.Provider
{
	/// <summary>
	/// SkinProvider 的摘要说明。
	/// </summary>
	public class StaticFileProvider
	{
		private StaticFileProvider()
		{}

		private static Hashtable skinfiles = new Hashtable();
		private static Hashtable lastchanges = new Hashtable();

		public static string GetContent(string filename)
		{
			string changetime = System.IO.File.GetLastWriteTime(filename).ToString();
			if (skinfiles[filename] != null)
			{
				if (lastchanges[filename].ToString() != changetime)
				{
					skinfiles[filename] = Globals.GetFileContent(filename);
					lastchanges[filename] = changetime;
				}
			}
			else
			{
				skinfiles[filename] = Globals.GetFileContent(filename);
				lastchanges[filename] = changetime;
			}
			changetime = null;
			return skinfiles[filename].ToString();
		}
	}
}
