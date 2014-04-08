using System;
using System.Text;
using Discuz.Common;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Drawing.Imaging;

namespace Discuz.Web.UI
{
    public class ImageConverterPage : Page
    {
        public ImageConverterPage()
        {
            string url = DNTRequest.GetString("u").ToLower();

            HttpContext.Current.Response.ContentType = "image/jpeg";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(5));

            try
            {
                System.Drawing.Image img = GetComputedImage(url);
                img.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
            }
            catch
            { }

            HttpContext.Current.Response.End();
        }

        private System.Drawing.Image GetComputedImage(string url)
        {
            return new System.Drawing.Bitmap(Utils.GetMapPath(url));
        }

        private string DecodeFrom64(string toDecode)
        {
            byte[] byteArray = Convert.FromBase64String(toDecode);
            return System.Text.ASCIIEncoding.ASCII.GetString(byteArray);
        }
    }
}
