using System;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using System.Diagnostics;
namespace Discuz.Forum
{
    
    /// <summary>
    /// FTP操作类
    /// </summary>
    public class FTPs
    {   
  
        /// <summary>
        /// FTP信息枚举类型
        /// </summary>
        public enum FTPUploadEnum
        {
            ForumAttach = 1, //论坛附件
            SpaceAttach = 2, //空间附件
            AlbumAttach = 3,  //相册附件
            MallAttach = 4, //商场附件
            ForumAvatar = 5 //论坛头像
        }

        public FTPs()
        { }

       
        #region 异步FTP上传文件

        private delegate bool delegateUpLoadFile(string path, string file, FTPUploadEnum ftpuploadname);

        //异步FTP上传文件代理
        private delegateUpLoadFile upload_aysncallback;

        public void AsyncUpLoadFile(string path, string file, FTPUploadEnum ftpuploadname)
        {
            upload_aysncallback = new delegateUpLoadFile(UpLoadFile);
            upload_aysncallback.BeginInvoke(path, file, ftpuploadname, null, null);
        }

        #endregion

        /// <summary>
        /// 论坛附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetForumAttachInfo
        {
            get
            {
                return FTPConfigs.GetForumAttachInfo;
            }
        }

        /// <summary>
        /// 空间附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetSpaceAttachInfo
        {
            get
            {
                return FTPConfigs.GetSpaceAttachInfo;
            }
        }

        /// <summary>
        /// 相册附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetAlbumAttachInfo
        {
            get
            {
                return FTPConfigs.GetAlbumAttachInfo;
            }
        }

        /// <summary>
        /// 相册附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetMallAttachInfo
        {
            get
            {
                return FTPConfigs.GetMallAttachInfo;
            }
        }

        /// <summary>
        /// 论坛头像FTP信息
        /// </summary>
        public static FTPConfigInfo GetForumAvatarInfo
        {
            get
            {
                return FTPConfigs.GetForumAvatarInfo;
            }
        }

        /// <summary>
        /// 上传远程附件
        /// </summary>
        /// <param name="path">要上传的文件路径(网络地址)</param>
        /// <param name="file">要上传的文件(本地地址)</param>
        /// <param name="ftpuploadname">远程FTP类型(论坛,空间或相册)</param>
        /// <returns>是否成功</returns>
        public bool UpLoadFile(string path, string file, FTPUploadEnum ftpuploadname)
        {
            FTP ftpupload = new FTP();
            //转换路径分割符为"/"
            path = path.Replace("\\", "/");
            path = path.StartsWith("/") ? path : "/" +path ;

            //删除file参数文件
            bool delfile = true;
            //根据上传名称确定上传的FTP服务器
            switch (ftpuploadname)
            {
                //论坛附件
                case FTPUploadEnum.ForumAttach:
                    {
                        ftpupload = new FTP(GetForumAttachInfo.Serveraddress, GetForumAttachInfo.Serverport, GetForumAttachInfo.Username, GetForumAttachInfo.Password, 1, GetForumAttachInfo.Timeout);
                        path = GetForumAttachInfo.Uploadpath + path;
                        delfile = (GetForumAttachInfo.Reservelocalattach == 1) ? false : true;
                        break;
                    }

                //空间附件
                case FTPUploadEnum.SpaceAttach:
                    {
                        ftpupload = new FTP(GetSpaceAttachInfo.Serveraddress, GetSpaceAttachInfo.Serverport, GetSpaceAttachInfo.Username, GetSpaceAttachInfo.Password, 1, GetSpaceAttachInfo.Timeout);
                        path = GetSpaceAttachInfo.Uploadpath + path;
                        delfile = (GetSpaceAttachInfo.Reservelocalattach == 1) ? false : true;
                        break;
                    }

                //相册附件
                case FTPUploadEnum.AlbumAttach:
                    {
                        ftpupload = new FTP(GetAlbumAttachInfo.Serveraddress, GetAlbumAttachInfo.Serverport, GetAlbumAttachInfo.Username, GetAlbumAttachInfo.Password, 1, GetAlbumAttachInfo.Timeout);
                        path = GetAlbumAttachInfo.Uploadpath + path;
                        delfile = (GetAlbumAttachInfo.Reservelocalattach == 1) ? false : true;
                        break;
                    }
                //商城附件
                case FTPUploadEnum.MallAttach:
                    {
                        ftpupload = new FTP(GetMallAttachInfo.Serveraddress, GetMallAttachInfo.Serverport, GetMallAttachInfo.Username, GetMallAttachInfo.Password, 1, GetMallAttachInfo.Timeout);
                        path = GetMallAttachInfo.Uploadpath + path;
                        delfile = (GetMallAttachInfo.Reservelocalattach == 1) ? false : true;
                        break;
                    }
                //论坛头像
                case FTPUploadEnum.ForumAvatar:
                    {
                        ftpupload = new FTP(GetForumAvatarInfo.Serveraddress, GetForumAvatarInfo.Serverport, GetForumAvatarInfo.Username, GetForumAvatarInfo.Password, 1, GetForumAvatarInfo.Timeout);
                        path = GetForumAvatarInfo.Uploadpath + path;
                        delfile = (GetForumAvatarInfo.Reservelocalattach == 1) ? false : true;
                        break;
                    }
            }

            //切换到指定路径下,如果目录不存在,将创建
            if (!ftpupload.ChangeDir(path))
            {
                foreach (string pathstr in path.Split('/'))
                {
                    if (pathstr.Trim() != "")
                    {
                        ftpupload.MakeDir(pathstr);
                        ftpupload.ChangeDir(pathstr);
                    }
                }                
            }
            
            ftpupload.Connect();

            if (!ftpupload.IsConnected)
                return false;

            int perc = 0;

            //绑定要上传的文件
            if (!ftpupload.OpenUpload(file, System.IO.Path.GetFileName(file)))
            {
                ftpupload.Disconnect();
                return false;
            }
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
           
            //开始进行上传
            while (ftpupload.DoUpload() > 0)
                perc = (int)(((ftpupload.BytesTotal) * 100) / ftpupload.FileSize);

            ftpupload.Disconnect();

            //long elapse = sw.ElapsedMilliseconds;
            //sw.Stop();    

            //(如存在)删除指定目录下的文件
            if (delfile && Utils.FileExists(file))
                System.IO.File.Delete(file);          

            return (perc >= 100) ? true : false;
        }

        /// <summary>
        /// FTP连接测试
        /// </summary>
        /// <param name="Serveraddress">FTP服务器地址</param>
        /// <param name="Serverport">FTP端口</param>
        /// <param name="Username">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="Timeout">超时时间(秒)</param>
        /// <param name="uploadpath">附件保存路径</param>
        /// <param name="message">返回信息</param>
        /// <returns>是否可用</returns>
        public bool TestConnect(string Serveraddress, int Serverport, string Username, string Password, int Timeout, string uploadpath, ref string message)
        {
            FTP ftpupload = new FTP(Serveraddress, Serverport, Username, Password, 1,  Timeout);
            bool isvalid = ftpupload.Connect();
            if (!isvalid)
            {
                message = ftpupload.errormessage;
                return isvalid;
            }

            //切换到指定路径下,如果目录不存在,将创建
            if (!ftpupload.ChangeDir(uploadpath))
            {
                ftpupload.MakeDir(uploadpath);
                if (!ftpupload.ChangeDir(uploadpath))
                {
                    message += ftpupload.errormessage;
                    isvalid = false;
                }
            }            
            return isvalid;
        }
     }
}
