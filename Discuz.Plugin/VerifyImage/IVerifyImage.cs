using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Discuz.Entity;

namespace Discuz.Plugin.VerifyImage
{
    /// <summary>
    /// 验证码图片接口
    /// </summary>
    public interface IVerifyImage
    {        
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="code">要显示的验证码</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="bgcolor">背景色</param>
        /// <param name="textcolor">文字颜色</param>
        VerifyImageInfo GenerateImage(string code, int width, int height, Color bgcolor, int textcolor);
    }
}
