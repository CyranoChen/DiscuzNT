using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Space.Utilities
{
	/// <summary>
	/// Globals 的摘要说明。
	/// </summary>
	public class Globals
	{

        private static Regex regexGB2312 = new Regex("([\u4e00-\u9fa5]+)");

        /// <summary>
        /// 检查RewriteName是否可用
        /// </summary>
        /// <param name="rewriteName"></param>
        /// <returns></returns>
        public static int CheckSpaceRewriteNameAvailable(string rewriteName)
        {
            if (rewriteName != string.Empty)
            {
                rewriteName = rewriteName.ToLower().Trim();
                if (rewriteName.IndexOfAny(new char[] { '　', ' ', ':' }) != -1)
                {
                    return 1;
                }
                else if (rewriteName == PrivateMessages.SystemUserName || ForumUtils.InBanWordArray(rewriteName) || ForumUtils.IsBanUsername(rewriteName, GeneralConfigs.GetConfig().Censoruser))
                {
                    return 1;
                }
                else if (Regex.IsMatch(rewriteName, "([^a-z0-9_-]+?)"))
                {
                    return 2;
                }
                else if (Space.Data.DbProvider.GetInstance().IsRewritenameExist(rewriteName))
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        
        }

    

		/// <summary>
		/// 获得指定目录下的文件列表
		/// </summary>
		/// <param name="path">路径</param>
		/// <returns></returns>
		public static string[] GetDirectoryFileList(string path, string searchPattern)
		{
			if (!Directory.Exists(path))
				return new string[0];

			DirectoryInfo dirInfo = new DirectoryInfo(path);
			FileInfo[] fileInfos = dirInfo.GetFiles(searchPattern);
			string[] result = new string[fileInfos.Length];
			for (int i = 0; i < fileInfos.Length; i++)
			{
				result[i] = fileInfos[i].Name;
			}

			return result;
		}

        /// <summary>
        /// 获取指定目录的所有文件列表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		public static string[] GetDirectoryFileList(string path)
		{
			return GetDirectoryFileList(path, "*.*");
		}

		/// <summary>
		/// 读取文件内容
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		public static string GetFileContent(string filename)
		{
			string result = string.Empty;

			if (!File.Exists(filename))
				return string.Format("文件 {0} 不存在", filename);
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
				{
					result = sr.ReadToEnd();
				}
			}

			return result;
		}
		/// <summary>
		/// 转换SQL执行错误时信息
		/// </summary>
		/// <param name="sqlerrorinfo"></param>
		/// <returns></returns>
		public static string TransferSqlErrorInfo(string sqlerrorinfo)
		{
			sqlerrorinfo = sqlerrorinfo.Replace("'", " ");
			sqlerrorinfo = sqlerrorinfo.Replace("\\", "/");
			sqlerrorinfo = sqlerrorinfo.Replace("\r\n", "\\r\\n");
			sqlerrorinfo = sqlerrorinfo.Replace("\r", "\\r");
			sqlerrorinfo = sqlerrorinfo.Replace("\n", "\\n");
			return sqlerrorinfo;
		}

		/// <summary>
		/// 根据Url获得源文件内容
		/// </summary>
		/// <param name="url">合法的Url地址</param>
		/// <returns></returns>
		public static string GetSourceTextByUrl(string url)
		{
			WebRequest request = WebRequest.Create(url);
			request.Timeout = 20000;//20秒超时
			WebResponse response = request.GetResponse();
			
			Stream resStream = response.GetResponseStream();
			StreamReader sr = new StreamReader(resStream);
			return sr.ReadToEnd();
		}

		/// <summary>
		///	上传Space文件
		/// </summary>
		/// <param name="uploadFile">上传文件对象</param>
		/// <param name="saveDir">保存地址</param>
        /// <param name="createThumbnailImage">是否生成缩略图</param>
		/// <returns></returns>
		public static string UploadSpaceFile(HttpPostedFile uploadFile,string saveDir,bool createThumbnailImage)
		{
			string sSavePath = saveDir;
			int nFileLen = uploadFile.ContentLength;
			byte[] myData = new Byte[nFileLen];
			uploadFile.InputStream.Read(myData, 0, nFileLen);
			//string sFilename = Path.GetFileName(uploadFile.FileName).ToLower();
            string fileextname = Path.GetExtension(uploadFile.FileName).ToLower();
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            string sFilename = Environment.TickCount.ToString() + random.Next(1000, 9999).ToString() + fileextname;
			while (File.Exists(sSavePath + sFilename))
			{
				//sFilename = Path.GetFileNameWithoutExtension(uploadFile.FileName) + file_append.ToString() + Path.GetExtension(uploadFile.FileName).ToLower();
                sFilename = Environment.TickCount.ToString() + random.Next(1000, 9999).ToString() + fileextname;
			}

		
            if (Common.Utils.InArray(Path.GetExtension(uploadFile.FileName).ToLower(), ".jpg,.jpeg,.gif,.png") && createThumbnailImage)
			{ //上传图片文件jpg,gif

				//Bitmap myBitmap;
				try
				{
                    FileStream newFile = new FileStream(sSavePath + sFilename, FileMode.Create);
                    newFile.Write(myData, 0, myData.Length);
                    newFile.Close();


					//myBitmap = new Bitmap(sSavePath + sFilename);
					//myBitmap.Dispose();
					//if ((Path.GetExtension(uploadFile.FileName).ToLower() == ".jpg"))
					//GetThumbnailImage(150,150,sSavePath + sFilename);

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

        /// <summary>
        /// 获取页面响应
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="err">错误信息</param>
        /// <returns></returns>
		public static HttpWebResponse GetPageResponse(string url, out string err)
		{
			err = "";
			HttpWebResponse response = null;
			HttpWebRequest request = null;
			//			byte[] data = encoding.GetBytes(postData);
			// 准备请求... 
			try
			{
				// 设置参数 
				request = WebRequest.Create(url) as HttpWebRequest;
				CookieContainer cookieContainer = new CookieContainer();
				request.CookieContainer = cookieContainer;
				request.AllowAutoRedirect = true;
				request.Method = "GET";
				request.Timeout = 10000;
				
				//发送请求并获取相应回应数据 
				response = request.GetResponse() as HttpWebResponse;
				return response;
			}
			catch (Exception ex)
			{
				err = ex.Message;
				return null;
			}
		}

		/// <summary>
		/// 将字符串按照Gb2312格式进行Encode
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string EncodeStringAsGB2312(string str)
		{
			
			MatchCollection mc = regexGB2312.Matches(str);

			foreach (Match m in mc)
			{
				if (m.Success)
				{
					str = str.Replace(m.Groups[1].Value, GetGBCharCode(m.Groups[1].Value));
				}
			}
			return str;
		}

		/// <summary>
		/// 获得汉字的gb2312码,如 北,返回 %B1%B1, 北京 返回 %B1%B1%BE%A9
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static string GetGBCharCode(string str)
		{
			StringBuilder sb = new StringBuilder();
			byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(str);

			foreach(byte b in bytes)
				sb.AppendFormat("%{0}", b.ToString("X"));
			return sb.ToString();
		}
    }
}
