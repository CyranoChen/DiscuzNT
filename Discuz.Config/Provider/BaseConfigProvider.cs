using System;
using System.IO;
using System.Web;
using Discuz.Common;

namespace Discuz.Config.Provider
{
	public class BaseConfigProvider
	{
		private BaseConfigProvider()
		{
		}

		static BaseConfigProvider()
		{
			config = GetRealBaseConfig();
		}

		private static BaseConfigInfo config = null;

		/// <summary>
		/// 获取基础配置实例
		/// </summary>
		/// <returns></returns>
		public static BaseConfigInfo Instance()
		{
			return config;
		}

		/// <summary>
		/// 设置对象实例
		/// </summary>
		/// <param name="anConfig"></param>
		public static void SetInstance(BaseConfigInfo anConfig)
		{
			if (anConfig == null)
				return;
			config = anConfig;
		}

		/// <summary>
		/// 获取真实基础配置对象
		/// </summary>
		/// <returns></returns>
		public static BaseConfigInfo GetRealBaseConfig()
		{
			BaseConfigInfo newBaseConfig = null;
            string filename = BaseConfigFileManager.ConfigFilePath;
			//string filename = null;            
            //HttpContext context = HttpContext.Current;
            //if(context != null)
            //    filename = context.Server.MapPath("/DNT.config");
            //else
            //    filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "DNT.config");

			try
			{
				newBaseConfig = (BaseConfigInfo)SerializationHelper.Load(typeof(BaseConfigInfo), filename);
			}
			catch
			{
				newBaseConfig = null;
			}
			
			if (newBaseConfig == null)
			{
				try
				{
					BaseConfigInfoCollection bcc = (BaseConfigInfoCollection)SerializationHelper.Load(typeof(BaseConfigInfoCollection), filename);
					foreach (BaseConfigInfo bc in bcc)
					{
						if (Utils.GetTrueForumPath() == bc.Forumpath)
						{
							newBaseConfig = bc;
							break;
						}
					}
					if (newBaseConfig == null)
					{
						BaseConfigInfo rootConfig = null;
						foreach (BaseConfigInfo bc in bcc)
						{
							if (Utils.GetTrueForumPath().StartsWith(bc.Forumpath) && bc.Forumpath != "/")
							{
								newBaseConfig = bc;
								break;
							}
							if (("/").Equals(bc.Forumpath))
							{
								rootConfig = bc;
							}
						}
						if (newBaseConfig == null)
						{
							newBaseConfig = rootConfig;
						}
					}

				}
				catch
				{
					newBaseConfig = null;
				}
			}
			if (newBaseConfig == null) 
			{
                throw new Exception("发生错误: 虚拟目录或网站根目录下没有正确的DNT.config文件，或者没有序列化权限");
			}
			return newBaseConfig;

		}


	}
}
