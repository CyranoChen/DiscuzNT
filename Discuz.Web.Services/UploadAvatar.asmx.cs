using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Web.services
{

    /// <summary>
    /// Summary description for UploadAvatar
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class UploadAvatar : System.Web.Services.WebService
    {

        #region 证书信息类
        /// <summary>
        /// 证书信息类
        /// </summary>
        public class CredentialInfo
        {
            private string m_authToken;
            /// <summary>
            /// 安全令牌
            /// </summary>
            public string AuthToken
            {
                get { return m_authToken; }
                set { m_authToken = value; }
            }

            private int m_userID;
            /// <summary>
            /// 当前用户id
            /// </summary>
            public int UserID
            {
                get { return m_userID; }
                set { m_userID = value; }
            }

            private string m_passWord;
            /// <summary>
            /// 当前用户口令（密文）
            /// </summary>
            public string Password
            {
                get { return m_passWord; }
                set { m_passWord = value; }
            }

            private int m_forumid;
            /// <summary>
            /// 当前所属版块id
            /// </summary>
            public int ForumID
            {
                get { return m_forumid; }
                set { m_forumid = value; }
            }
        }
        #endregion

        #region 上传设置信息类
        /// <summary>
        /// 上传设置信息类
        /// </summary>
        public class UploadSetInfo
        {
            public UploadSetInfo()
            { }

            public UploadSetInfo(string attachExtensions, string attachExtensionsNoSize, int maxTodaySize, int attachSize, bool canPostAttach, int maxAttachSize, string errMessage)
            {
                m_attachExtensions = attachExtensions;
                m_attachExtensionsNoSize = attachExtensionsNoSize;
                m_maxTodaySize = maxTodaySize;
                m_attachSize = attachSize;
                m_canPostAttach = canPostAttach;
                m_maxAttachSize = maxAttachSize;
                m_errMessage = errMessage;
                m_maxAttachments = GeneralConfigs.GetConfig().Maxattachments;
            }

            private string m_attachExtensions;
            /// <summary>
            /// 用户可以上传的文件类型
            /// </summary>
            public string AttachExtensions
            {
                get { return m_attachExtensions; }
                set { m_attachExtensions = value; }
            }

            private string m_attachExtensionsNoSize;
            /// <summary>
            /// 用户可以上传的文件类型(不带上传数据大小)
            /// </summary>
            public string AttachExtensionsNoSize
            {
                get { return m_attachExtensionsNoSize; }
                set { m_attachExtensionsNoSize = value; }
            }

            private int m_maxTodaySize;
            /// <summary>
            /// 得到今天允许用户上传的附件总大小(字节)
            /// </summary>
            public int MaxTodaySize
            {
                get { return m_maxTodaySize; }
                set { m_maxTodaySize = value; }
            }

            private int m_attachSize;
            /// <summary>
            /// 今天可上传的大小
            /// </summary>
            public int AttachSize
            {
                get { return m_attachSize; }
                set { m_attachSize = value; }
            }

            private bool m_canPostAttach;
            /// <summary>
            /// 是否允许上传附件
            /// </summary>
            public bool CanPostAttach
            {
                get { return m_canPostAttach; }
                set { m_canPostAttach = value; }
            }

            private int m_maxAttachSize;
            /// <summary>
            /// 单个附件大小
            /// </summary>
            public int MaxAttachSize
            {
                get { return m_maxAttachSize; }
                set { m_maxAttachSize = value; }
            }

            private string m_errMessage;
            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrMessage
            {
                get { return m_errMessage; }
                set { m_errMessage = value; }
            }

            private int m_maxAttachments;
            /// <summary>
            /// 最大允许上传的附件数
            /// </summary>
            public int Maxattachments
            {
                get { return m_maxAttachments; }
                set { m_maxAttachments = value; }
            }
        }
        #endregion

        #region 上传辅助方法
        //上传临时文件后缀
        private string _tempExtension = "_temp";

        /// <summary>
        /// 获得上传路径
        /// </summary>
        /// <param name="用户">文件名</param>
        /// <returns>返回上传路径</returns>
        private string GetAvatarUploadFolder(string uid)
        {
            uid = Avatars.FormatUid(uid);
            string avatarDir = string.Format("{0}avatars/upload/{1}/{2}/{3}",
                BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2));
            if (!Directory.Exists(Utils.GetMapPath(avatarDir)))
                Directory.CreateDirectory(Utils.GetMapPath(avatarDir));

            return avatarDir;
        }

        private GeneralConfigInfo config = GeneralConfigs.GetConfig();

        #endregion

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        //[WebMethod]
        //public string GetAvatarUrl()
        //{ 
        //    //当支持FTP上传头像时,使用FTP上传远程头像
        //    if (FTPs.GetForumAvatarInfo.Allowupload == 1)
        //        return FTPs.GetForumAvatarInfo.Remoteurl;
        //    else
        //        return "";
        //}
        /// <summary>
        /// WEB权限认证
        /// </summary>
        /// <param name="creinfo">认证信息</param>
        /// <returns>是否通过验正</returns>
        private bool AuthenticateUser(CredentialInfo creinfo)
        {
            if (creinfo.UserID > 0)
            {
                int olid = Discuz.Forum.OnlineUsers.GetOlidByUid(creinfo.UserID);
                if (olid > 0)
                {
                    OnlineUserInfo oluserinfo = Discuz.Forum.OnlineUsers.GetOnlineUser(olid);
                    //if (oluserinfo.Userid == creinfo.UserID && Utils.UrlEncode(Discuz.Forum.ForumUtils.SetCookiePassword(oluserinfo.Password, GeneralConfigs.GetConfig().Passwordkey)) == creinfo.Password &&//检测用户id和口令
                    //   creinfo.AuthToken == Discuz.Common.DES.Encode(oluserinfo.Olid.ToString() + "," + oluserinfo.Username.ToString(), oluserinfo.Password.Substring(0, 10)).Replace("+", "["))//检查认证信息
                    if (oluserinfo.Userid == creinfo.UserID && Utils.UrlEncode(Discuz.Forum.ForumUtils.SetCookiePassword(oluserinfo.Password, GeneralConfigs.GetConfig().Passwordkey)) == creinfo.Password)//检测用户id和口令                  
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static object lockHelper = new object();


        [WebMethod]
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="data">上传的字节数据</param>
        /// <param name="dataLength">数据长度</param>
        /// <param name="parameters">上传参数</param>
        /// <param name="firstChunk">是否第一块数据</param>
        /// <param name="lastChunk">是否最后一块数据</param>
        public bool SaveAvatar(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, CredentialInfo creinfo, Point point, Size size)
        {
            if (AuthenticateUser(creinfo))
            {
                string fileextname = Utils.CutString(fileName, fileName.LastIndexOf(".") + 1).ToLower();
                string uploadFolder = GetAvatarUploadFolder(creinfo.UserID.ToString());
                string tempFileName = fileName + _tempExtension;

                if (firstChunk)
                {
                    //删除临时文件
                    if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath +  BaseConfigs.GetForumPath + "upload/temp/" + tempFileName))
                        File.Delete(@HostingEnvironment.ApplicationPhysicalPath + BaseConfigs.GetForumPath + "upload/temp/" + tempFileName);

                    //删除目录文件
                    if (File.Exists(uploadFolder + "/" + fileName))
                        File.Delete(uploadFolder + "/" + fileName);
                }

                FileStream fs = File.Open(@HostingEnvironment.ApplicationPhysicalPath + BaseConfigs.GetForumPath + "upload/temp/" + tempFileName, FileMode.Append);
                fs.Write(data, 0, dataLength);
                fs.Close();

                if (lastChunk)
                {
                    lock (lockHelper)
                    {
                        string newfilename = (Environment.TickCount & int.MaxValue).ToString() + new Random().Next(1000, 9999) + "." + fileextname;
                        File.Move(HostingEnvironment.ApplicationPhysicalPath + BaseConfigs.GetForumPath + "upload/temp/" + tempFileName, Utils.GetMapPath(uploadFolder + "/" + newfilename));

                        GrabImage(creinfo, Utils.GetMapPath(uploadFolder + "/" + newfilename), point, size);
                        return true;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// 截取指定位置的图片信息
        /// </summary>
        /// <param name="filePathName"></param>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool GrabImage(CredentialInfo creinfo, string filePathName, Point point, Size size)
        {
            if(!System.IO.File.Exists(filePathName))  return false;

            Image oldImage = System.Drawing.Image.FromFile(filePathName);

            if (size.Width > oldImage.Width || size.Height > oldImage.Height)
            {
                oldImage.Dispose();
                return false;
            }

            //用指定的大小和格式初始化 Bitmap 类的新实例
            Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Color backColor = Color.Black;// bitmap.GetPixel(1, 1);
            bitmap.MakeTransparent(backColor);

            //从指定的 Image 对象创建新 Graphics 对象
            Graphics graphics = Graphics.FromImage(bitmap);
            // 0,0 是graphics的起始位置,也就是从原点开始画
            // new Rectangle是截取源图片的目标区域,用户只需要改变其中四个值即可
            graphics.DrawImage(oldImage /*是原图片*/, 0, 0, new Rectangle(point.X, point.Y, size.Width, size.Height), GraphicsUnit.Pixel);
            // 将Bitmap转化成Image
            Image image = Image.FromHbitmap(bitmap.GetHbitmap());

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            qualityParam[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;


            string uid = Avatars.FormatUid(creinfo.UserID.ToString());
            string avatarFileName = string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_",
               BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2));
            avatarFileName = Utils.GetMapPath(avatarFileName);
            image.Save(avatarFileName + "large.jpg", ici, encoderParams);

            if (size.Width * 0.8 <= 130)//前台模版宽度
            {
                Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "medium.jpg",
                          (int)(size.Width * 0.8),
                          (int)(size.Height * 0.8));
                Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "small.jpg",
                          (int)(size.Width * 0.6),
                          (int)(size.Height * 0.6));
            }
            else
            {
                 Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "medium.jpg",
                          (int)(size.Width * 0.5),
                          (int)(size.Height * 0.5));
                 Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "small.jpg",
                          (int)(size.Width * 0.3),
                          (int)(size.Height * 0.3));
            }

            try
            {
                oldImage.Dispose();
                System.IO.File.Delete(filePathName);
            }
            catch { }

            //当支持FTP上传头像时,使用FTP上传远程头像
            if (FTPs.GetForumAvatarInfo.Allowupload == 1)
            {
                FTPs ftps = new FTPs();
                string ftpAvatarFileName = string.Format("/avatars/upload/{0}/{1}/{2}/",
                       uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2));
                ftps.UpLoadFile(ftpAvatarFileName, avatarFileName + "large.jpg", FTPs.FTPUploadEnum.ForumAvatar);
                ftps.UpLoadFile(ftpAvatarFileName, avatarFileName + "medium.jpg", FTPs.FTPUploadEnum.ForumAvatar);
                ftps.UpLoadFile(ftpAvatarFileName, avatarFileName + "small.jpg", FTPs.FTPUploadEnum.ForumAvatar);
            }
            return true;
        }
    }
}
