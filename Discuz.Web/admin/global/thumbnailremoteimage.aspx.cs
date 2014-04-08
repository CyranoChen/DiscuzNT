using System;
using System.Web.UI;
using System.Collections;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using System.Net;
using System.Web;
using System.IO;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 时间段设置
    /// </summary>
    public partial class ThumbnailRemoteImage : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AllowShowNavigation = false;
            if (!Page.IsPostBack)
            {
                int w = TypeConverter.StrToInt(DNTRequest.GetString("w"));
                int h = TypeConverter.StrToInt(DNTRequest.GetString("h"));
                if (w > 0)
                {
                    maxWidth.Text = w.ToString();
                }
                if (h > 0)
                {
                    maxHeight.Text = h.ToString();
                }
            }
        }


        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                int width = TypeConverter.StrToInt(maxWidth.Text);
                int height = TypeConverter.StrToInt(maxHeight.Text);
                if (width <= 0 || height <= 0 || !IsImage(imageUrl.Text.Trim()))
                {
                    this.reImgUrl.InnerHtml = "<font color='red'>请填入正确的图片地址和宽度高度</font>";
                    return;
                }
                string imgUrl = GetRemoteThumbnail(imageUrl.Text.Trim(), width, height);

                if (imgUrl == string.Empty)
                {
                    this.reImgUrl.InnerHtml = "<font color='red'>生成缩略图失败，请确认图片地址正确后重试</font>";
                }

                this.reImgUrl.InnerHtml = "<img src='" + imgUrl + "' /><br /><br />生成缩略图成功，地址如下:<br /><br /><textarea cols='50' rows='2'>" + imgUrl + "</textarea>";
                //base.RegisterStartupScript( "PAGE", "window.location.href='global_timespan.aspx';");
            }

            #endregion
        }

        /// <summary>
        /// 获取远程图片
        /// </summary>
        /// <returns></returns>
        public string GetRemoteThumbnail(string url, int maxWidth, int maxHeight)
        {
            string fileName = Utils.CutString(url, url.LastIndexOf("/") + 1).ToLower();
            string fileExtName = Utils.CutString(fileName, fileName .LastIndexOf(".") + 1).ToLower();
            if (fileExtName == string.Empty || !Utils.InArray(fileExtName, "jpg,jpeg,gif"))
                fileExtName = "jpg";


            //Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            string tempImageFileName = string.Format("{0}_{1}_{2}.{3}",
                Utils.MD5(url.ToLower()),
                maxWidth,
                maxHeight,
                fileExtName);
                
                /*string.Format("{0}{1}.{2}",
                                (Environment.TickCount & int.MaxValue).ToString(),
                                random.Next(1000, 9999).ToString(),
                                fileExtName);*/
            string tempFilePath = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "upload/temp/");
            string tempFileName = tempFilePath + tempImageFileName;
            string thumbnailPath = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "cache/thumbnail/");
            if (!Directory.Exists(tempFilePath))
                Utils.CreateDir(tempFilePath);
            if (!Directory.Exists(thumbnailPath))
                Utils.CreateDir(thumbnailPath);
            string realFileName = thumbnailPath + tempImageFileName;
            try
            {
                DownloadImage(url, tempFileName);

                //获得图片的根路径并确保文件夹存在

                //获得图片的基本文件名

                //保存图片相关属性

                //生成缩略图
                Thumbnail.MakeThumbnailImage(tempFileName, realFileName, maxWidth, maxHeight);

                //删除原图
                DeleteImage(tempFileName);
            }
            catch
            {
                return string.Empty;
            }

            return Utils.GetRootUrl(BaseConfigs.GetForumPath) + "cache/thumbnail/" + tempImageFileName;

        }

        /// <summary>
        /// 将远程图片保存到服务器
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="filePath"></param>
        public void DownloadImage(string remotePath, string filePath)
        {
            WebClient w = new WebClient();
            try
            {
                w.DownloadFile(remotePath, filePath);
            }
            finally
            {
                w.Dispose();
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="fileName">删除图片</param>
        public void DeleteImage(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 判断远程地址是否是image
        /// </summary>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public bool IsImage(string remotePath)
        {
            if (!Uri.IsWellFormedUriString(remotePath, UriKind.RelativeOrAbsolute))
                return false;
            WebRequest i = WebRequest.Create(remotePath);
            WebResponse a = i.GetResponse();
            if (a.ContentType.IndexOf("image/") > -1)
            {
                a.Close();
                return true;
            }
            a.Close();
            return false;
        }




        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }

        #endregion
    }
}