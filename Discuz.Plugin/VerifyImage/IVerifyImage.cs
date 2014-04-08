using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Discuz.Entity;

namespace Discuz.Plugin.VerifyImage
{
    /// <summary>
    /// ��֤��ͼƬ�ӿ�
    /// </summary>
    public interface IVerifyImage
    {        
        /// <summary>
        /// ������֤��ͼƬ
        /// </summary>
        /// <param name="code">Ҫ��ʾ����֤��</param>
        /// <param name="width">���</param>
        /// <param name="height">�߶�</param>
        /// <param name="bgcolor">����ɫ</param>
        /// <param name="textcolor">������ɫ</param>
        VerifyImageInfo GenerateImage(string code, int width, int height, Color bgcolor, int textcolor);
    }
}
