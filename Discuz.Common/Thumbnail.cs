using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace Discuz.Common
{
	/// <summary>
	/// Thumbnail 的摘要说明。
	/// </summary>
	public class Thumbnail
	{
		private Image srcImage;
		private string srcFileName;		
		
		/// <summary>
		/// 创建
		/// </summary>
		/// <param name="FileName">原始图片路径</param>
		public bool SetImage(string FileName)
		{
			srcFileName = Utils.GetMapPath(FileName);
			try
			{
				srcImage = Image.FromFile(srcFileName);
			}
			catch
			{
				return false;
			}
			return true;

		}

		/// <summary>
		/// 回调
		/// </summary>
		/// <returns></returns>
		public bool ThumbnailCallback()
		{
			return false;
		}

		/// <summary>
		/// 生成缩略图,返回缩略图的Image对象
		/// </summary>
		/// <param name="Width">缩略图宽度</param>
		/// <param name="Height">缩略图高度</param>
		/// <returns>缩略图的Image对象</returns>
		public Image GetImage(int Width,int Height)
		{
			Image img;
			Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback); 
 			img = srcImage.GetThumbnailImage(Width,Height,callb, IntPtr.Zero);
 			return img;
		}

		/// <summary>
		/// 保存缩略图
		/// </summary>
		/// <param name="Width"></param>
		/// <param name="Height"></param>
		public void SaveThumbnailImage(int Width,int Height)
		{
			switch(Path.GetExtension(srcFileName).ToLower())
			{
				case ".png":
					SaveImage(Width, Height, ImageFormat.Png);
					break;
				case ".gif":
					SaveImage(Width, Height, ImageFormat.Gif);
					break;
				default:
					SaveImage(Width, Height, ImageFormat.Jpeg);
					break;
			}
		}

		/// <summary>
		/// 生成缩略图并保存
		/// </summary>
		/// <param name="Width">缩略图的宽度</param>
		/// <param name="Height">缩略图的高度</param>
		/// <param name="imgformat">保存的图像格式</param>
		/// <returns>缩略图的Image对象</returns>
		public void SaveImage(int Width,int Height, ImageFormat imgformat)
		{
            if (imgformat != ImageFormat.Gif && (srcImage.Width > Width) || (srcImage.Height > Height))
            {
                Image img;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                img = srcImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);
                srcImage.Dispose();
                img.Save(srcFileName, imgformat);
                img.Dispose();
            }
		}

		#region Helper

		/// <summary>
		/// 保存图片
		/// </summary>
		/// <param name="image">Image 对象</param>
		/// <param name="savePath">保存路径</param>
		/// <param name="ici">指定格式的编解码参数</param>
		private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
		{
			//设置 原图片 对象的 EncoderParameters 对象
			EncoderParameters parameters = new EncoderParameters(1);
			parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long) 100));
			image.Save(savePath, ici, parameters);
			parameters.Dispose();
		}

		/// <summary>
		/// 获取图像编码解码器的所有相关信息
		/// </summary>
		/// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
		/// <returns>返回图像编码解码器的所有相关信息</returns>
		private static ImageCodecInfo GetCodecInfo(string mimeType)
		{
			ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
			foreach(ImageCodecInfo ici in CodecInfo)
			{
				if(ici.MimeType == mimeType)
                    return ici;
			}
			return null;
		}

		/// <summary>
		/// 计算新尺寸
		/// </summary>
		/// <param name="width">原始宽度</param>
		/// <param name="height">原始高度</param>
		/// <param name="maxWidth">最大新宽度</param>
		/// <param name="maxHeight">最大新高度</param>
		/// <returns></returns>
		private static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
		{
			decimal MAX_WIDTH = (decimal)maxWidth;
			decimal MAX_HEIGHT = (decimal)maxHeight;
			decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

			int newWidth, newHeight;
			decimal originalWidth = (decimal)width;
			decimal originalHeight = (decimal)height;
			
			if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT) 
			{
				decimal factor;
				// determine the largest factor 
				if (originalWidth / originalHeight > ASPECT_RATIO) 
				{
					factor = originalWidth / MAX_WIDTH;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				} 
				else 
				{
					factor = originalHeight / MAX_HEIGHT;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				}	  
			} 
			else 
			{
				newWidth = width;
				newHeight = height;
			}
			return new Size(newWidth,newHeight);			
		}

        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        public static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
		#endregion

		/// <summary>
		/// 制作小正方形
		/// </summary>
		/// <param name="image">图片对象</param>
		/// <param name="newFileName">新地址</param>
		/// <param name="newSize">长度或宽度</param>
		public static void MakeSquareImage(Image image, string newFileName, int newSize)
		{	
			int i = 0;
			int width = image.Width;
			int height = image.Height;
			if (width > height)
				i = height;
			else
				i = width;

            Bitmap b = new Bitmap(newSize, newSize);

			try
			{
				Graphics g = Graphics.FromImage(b);
				g.InterpolationMode = InterpolationMode.High;
				g.SmoothingMode = SmoothingMode.HighQuality;

				//清除整个绘图面并以透明背景色填充
				g.Clear(Color.Transparent);
				if (width < height)
					g.DrawImage(image,  new Rectangle(0, 0, newSize, newSize), new Rectangle(0, (height-width)/2, width, width), GraphicsUnit.Pixel);
				else
					g.DrawImage(image, new Rectangle(0, 0, newSize, newSize), new Rectangle((width-height)/2, 0, height, height), GraphicsUnit.Pixel);

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(newFileName).ToString().ToLower()));
			}
			finally
			{
				image.Dispose();
				b.Dispose();
			}
		}

        /// <summary>
        /// 制作小正方形
        /// </summary>
        /// <param name="fileName">图片文件名</param>
        /// <param name="newFileName">新地址</param>
        /// <param name="newSize">长度或宽度</param>
        public static void MakeSquareImage(string fileName, string newFileName, int newSize)
        {
            MakeSquareImage(Image.FromFile(fileName), newFileName, newSize);
        }

        /// <summary>
        /// 制作远程小正方形
		/// </summary>
		/// <param name="url">图片url</param>
		/// <param name="newFileName">新地址</param>
		/// <param name="newSize">长度或宽度</param>
        public static void MakeRemoteSquareImage(string url, string newFileName, int newSize)
        {
            Stream stream = GetRemoteImage(url);
            if (stream == null)
                return;
            Image original = Image.FromStream(stream);
            stream.Close();
            MakeSquareImage(original, newFileName, newSize);
        }

		/// <summary>
		/// 制作缩略图
		/// </summary>
		/// <param name="original">图片对象</param>
		/// <param name="newFileName">新图路径</param>
		/// <param name="maxWidth">最大宽度</param>
		/// <param name="maxHeight">最大高度</param>
        public static void MakeThumbnailImage(Image original, string newFileName, int maxWidth, int maxHeight)
		{
			Size _newSize = ResizeImage(original.Width,original.Height,maxWidth, maxHeight);

            using (Image displayImage = new Bitmap(original, _newSize))
            {
                try
                {
                    displayImage.Save(newFileName, original.RawFormat);
                }
                finally
                {
                    original.Dispose();
                }
            }
		}

        /// <summary>
        /// 制作缩略图
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        public static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight)
        {
            MakeThumbnailImage(Image.FromFile(fileName), newFileName, maxWidth, maxHeight);
        }

        /// <summary>
        /// 制作远程缩略图
        /// </summary>
        /// <param name="url">图片URL</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        public static void MakeRemoteThumbnailImage(string url, string newFileName, int maxWidth, int maxHeight)
        {
            Stream stream = GetRemoteImage(url);
            if(stream == null)
                return;
            Image original = Image.FromStream(stream);
            stream.Close();
            MakeThumbnailImage(original, newFileName, maxWidth, maxHeight);
        }

        /// <summary>
        /// 获取图片流
        /// </summary>
        /// <param name="url">图片URL</param>
        /// <returns></returns>
        private static Stream GetRemoteImage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentLength = 0;
            request.Timeout = 20000;
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取图片缩略图时用到的KEY
        /// </summary>
        /// <param name="aid">附件ID</param>
        /// <param name="width">缩略后的宽度</param>
        /// <param name="height">缩略后的高度</param>
        /// <returns></returns>
        public static string GetKey(int aid, int width, int height)
        {
            return Discuz.Common.DES.Encode(aid.ToString() + ",300,300", Utils.MD5(aid.ToString())).Replace("+", "[");
        }

        public static string GetKey(int aid)
        {
            return GetKey(aid, 300, 300);
        }
	}
}
