using System;

namespace Discuz.Entity
{
	public enum ModuleType
	{
		/// <summary>
		/// Rss模块
		/// </summary>
		Rss = 1,
		/// <summary>
		/// 远程模块
		/// </summary>
		Remote = 3,
		/// <summary>
		/// 本地模块
		/// </summary>
		Local = 2,
		/// <summary>
		/// 在加载模块Xml文件时发生错误
		/// </summary>
		Error = 4
	}
}
