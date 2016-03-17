using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Cache.Data
{
    public interface  ICacheAttachFiles
    {
         /// <summary>
        /// 上传文件到mongodb
        /// </summary>
        /// <param name="uploadDir">要上传的路径</param>
        /// <param name="fileName">要上传的文件名</param>
        /// <returns></returns>
        bool UploadFile(string uploadDir, string fileName);

        /// <summary>
        /// Response附件
        /// </summary>
        /// <param name="fileName">要获取附件的存储文件名</param>
        /// <param name="originfilename">附件原来名称，即response的名称</param>
        /// <param name="filetype">要获取附件的类型</param>
        /// <returns></returns>
        bool ResponseFile(string fileName, string originfilename, string filetype);
    }
}
