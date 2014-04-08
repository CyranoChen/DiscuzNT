using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 模板说明结构, 每个模板目录下均可使用指定结构的xml文件来说明该模板的基本信息
    /// </summary>
    public class TemplateAboutInfo
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string name = "";
        /// <summary>
        /// 作者
        /// </summary>
        public string author = "";
        /// <summary>
        /// 创建日期
        /// </summary>
        public string createdate = "";
        /// <summary>
        /// 模板版本
        /// </summary>
        public string ver = "";
        /// <summary>
        /// 模板适用的论坛版本
        /// </summary>
        public string fordntver = "";
        /// <summary>
        /// 版权文字
        /// </summary>
        public string copyright = "";
        /// <summary>
        /// 模板宽度
        /// </summary>
        public string width = "600";
    }

}
