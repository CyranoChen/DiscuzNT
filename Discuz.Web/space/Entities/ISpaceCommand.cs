using System;
using System.Web;

namespace Discuz.Space.Entities
{
	/// <summary>
	/// 空间模块的操作接口
	/// </summary>
	public interface ISpaceCommand
	{
        /// <summary>
        /// 获得模块提交数据
        /// </summary>
        /// <param name="httpContext">当前httpContext对象</param>
        /// <returns>返回重新加载的内容(保留功能，下个版本改进)</returns>
        string GetModulePost(HttpContext httpContext);        
        /// <summary>
        /// 删除模块时的行为，主要是删除数据库中冗余数据
        /// </summary>
        void RemoveModule();
	}
}
