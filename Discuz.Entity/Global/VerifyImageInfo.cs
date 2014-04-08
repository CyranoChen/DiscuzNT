using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace Discuz.Entity
{
    /// <summary>
    /// ��֤��ͼƬ��Ϣ
    /// </summary>
    public class VerifyImageInfo
    {
        private Bitmap image;
        private string contentType = "image/pjpeg";
        private ImageFormat imageFormat = ImageFormat.Jpeg;

        /// <summary>
        /// ���ɳ���ͼƬ
        /// </summary>
        public Bitmap Image
        {
            get { return image;}
            set { image = value; }
        }

        /// <summary>
        /// �����ͼƬ���ͣ��� image/pjpeg
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// ͼƬ�ĸ�ʽ
        /// </summary>
        public ImageFormat ImageFormat
        {
            get { return imageFormat;}
            set { imageFormat = value;}
        }
    }
}
