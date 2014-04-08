using System;
using Discuz.Common;


namespace Discuz.Config.Provider
{
	/// <summary>
	/// ConfigProvider ��ժҪ˵����
	/// </summary>
	public class ConfigProvider 
	{
		private ConfigProvider()
		{
		}

		private static object lockHelper = new object();

		private static string path = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/general.config");
		
		//����ռ���ʱgeneral.config�ļ��޸�ʱ��
		private static string fileoldchange = null;

		//����general.config�ļ��޸�ʱ��
		private static string filenewchange = null;

		static ConfigProvider()
		{
			fileoldchange = System.IO.File.GetLastWriteTime(path).ToString();
		
			config = (GeneralConfigInfo)Common.SerializationHelper.Load(typeof(GeneralConfigInfo), path);
		}

		private static GeneralConfigInfo config = null;

		/// <summary>
		/// ��ȡ���ö���ʵ��
		/// </summary>
		/// <returns></returns>
		public static GeneralConfigInfo Instance()
		{
			filenewchange = System.IO.File.GetLastWriteTime(path).ToString();
			
			//������������general.config�����仯ʱ���config���¸�ֵ
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
		/// �������ö���ʵ��
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
