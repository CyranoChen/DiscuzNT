using System;
using System.Text;
using System.Collections;

namespace Discuz.Space.Entities
{
    /// <summary>
    /// 空间模板接口
    /// </summary>
    public interface ISpaceTemplate
    {
        /// <summary>
        /// 获得相应的模板内容
        /// </summary>
        /// <param name="ht">模板变量的数组</param>
        /// <returns>模板内容的html代码</returns>
        string GetHtml(Hashtable ht);
    }
}
