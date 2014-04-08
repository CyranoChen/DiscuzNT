using System;
using Discuz.Common;


namespace Discuz.Config.Provider
{
	/// <summary>
	/// ConfigProvider 的摘要说明。
	/// </summary>
	public class ConfigProvider 
	{
		private ConfigProvider()
		{
		}

		private static object lockHelper = new object();

		private static string path = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/general.config");
		
		//程序刚加载时general.config文件修改时间
		private static string fileoldchange = null;

		//最新general.config文件修改时间
		private static string filenewchange = null;

		static ConfigProvider()
		{
			fileoldchange = System.IO.File.GetLastWriteTime(path).ToString();
		
			config = (GeneralConfigInfo)Common.SerializationHelper.Load(typeof(GeneralConfigInfo), path);
		}

		private static GeneralConfigInfo config = null;

		/// <summary>
		/// 获取配置对象实例
		/// </summary>
		/// <returns></returns>
		public static GeneralConfigInfo Instance()
		{
			filenewchange = System.IO.File.GetLastWriteTime(path).ToString();
			
			//当程序运行中general.config发生变化时则对config重新赋值
			if(fileoldchange != filenewchange)
			{
				fileoldchange = filenewchange;
				lock (lockHelper)
				{
					config = (GeneralConfigInfo)Common.SerializationHelper.Load(typeof(GeneralConfigInfo), path);
				}
			}

			return config;
		}

		/// <summary>
		/// 设置配置对象实例
		/// </summary>
		/// <param name="anConfig"></param>
		public static void SetInstance(GeneralConfigInfo anConfig)
		{
			if (anConfig == null)
				return;
			config = anConfig;
		}

	}
}
