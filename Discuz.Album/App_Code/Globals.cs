using System;
using System.Text;
using System.IO;
using System.Web;

using Discuz.Common;

namespace Discuz.Album
{
    public class Globals
    {
        /// <summary>
        /// 获取缩略图地址
        /// </summary>
        /// <param name="fileName">图片地址</param>
        /// <returns></returns>
        public static string GetThumbnailImage(string fileName)
        {
            string extname = Path.GetExtension(fileName);

            if (Utils.StrIsNullOrEmpty(extname))
                return string.Empty;

            return fileName.Replace(extname, "_thumbnail" + extname);
        }

        /// <summary>
        /// 获取方图缩略图文件名
        /// </summary>
        /// <param name="fileName">原图文件名</param>
        /// <returns></returns>
        public static string GetSquareImage(string fileName)
        {
            string extname = Path.GetExtension(fileName);

            if (Utils.StrIsNullOrEmpty(extname))
                return string.Empty;

            return fileName.Replace(extname, "_square" + extname);
        }

        /// <summary>
        ///	上传Space文件
        /// </summary>
        /// <param name="uploadFile">上传文件对象</param>
        /// <param name="saveDir">保存地址</param>
        /// <param name="createThumbnailImage">是否生成缩略图</param>
        /// <returns></returns>
        public static string UploadSpaceFile(HttpPostedFile uploadFile, string saveDir, bool createThumbnailImage)
        {
            string sSavePath = saveDir;
            int nFileLen = uploadFile.ContentLength;
            byte[] myData = new Byte[nFileLen];
            uploadFile.InputStream.Read(myData, 0, nFileLen);
            string fileextname = Path.GetExtension(uploadFile.FileName).ToLower();
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            string sFilename = Environment.TickCount + random.Next(1000, 9999) + fileextname;
            while (File.Exists(sSavePath + sFilename))
            {
                sFilename = Environment.TickCount + random.Next(1000, 9999) + fileextname;
            }

            if (Common.Utils.InArray(Path.GetExtension(uploadFile.FileName).ToLower(), ".jpg,.jpeg,.gif,.png") && createThumbnailImage)
            { 
                //上传图片文件jpg,gif
                try
                {
                    FileStream newFile = new FileStream(sSavePath + sFilename, FileMode.Create);
                    newFile.Write(myData, 0, myData.Length);
                    newFile.Close();

                    string extension = Path.GetExtension(sSavePath + sFilename);
                    Common.Thumbnail.MakeThumbnailImage(sSavePath + sFilename, (sSavePath + sFilename).Replace(extension, "_thumbnail" + extension), 150, 150);
                    Common.Thumbnail.MakeSquareImage(sSavePath + sFilename, (sSavePath + sFilename).Replace(extension, "_square" + extension), 100);
                    return sFilename;
                }
                catch (ArgumentException errArgument)
                {
                    File.Delete(sSavePath + sFilename);
                    string errinfo = errArgument.Message;
                    return sFilename;
                }
            }
            else //上传除图片文件以外的全部文件
            {
                try
                {
                    uploadFile.SaveAs(sSavePath + sFilename);
                    return sFilename;
                }
                catch (ArgumentException errArgument)
                {
                    File.Delete(sSavePath + sFilename);
                    string errinfo = errArgument.Message;
                    return sFilename;
                }
            }
        }
    }
}
