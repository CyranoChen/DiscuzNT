using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Entity;

namespace Discuz.Plugin.Preview
{
    /// <summary>
    /// 预览信息接口
    /// </summary>
    public interface IPreview
    {
        /// <summary>
        /// 系统是否使用ftp
        /// </summary>
        bool UseFTP
        { 
            get; set; 
        }
        /// <summary>
        /// 获得预览信息
        /// </summary>
        /// <param name="fileName">文件的物理路径</param>
        /// <param name="attachment">附件对象</param>
        /// <returns>展现内容的html</returns>
        string GetPreview(string fileName, ShowtopicPageAttachmentInfo attachment);

        /// <summary>
        /// 文件生成后的操作
        /// </summary>
        /// <param name="fileName">文件的物理路径</param>
        void OnSaved(string fileName);
    }
}
