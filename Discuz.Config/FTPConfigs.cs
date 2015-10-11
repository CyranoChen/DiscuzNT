using System;
using System.Text;

using Discuz.Common;

namespace Discuz.Config
{
    #region 该类目前暂被放弃

    /// <summary>
    ///  FTP配置类
    /// </summary>
    public class FTPConfigs
    {
        private static object lockhelper = new object();


        #region 声明上传信息(静态)对象

        private static FTPConfigInfo m_forumattach = null;

        private static FTPConfigInfo m_spaceattach = null;

        private static FTPConfigInfo m_albumattach = null;

        private static FTPConfigInfo m_mallattach = null;

        private static FTPConfigInfo m_forumavatar = new FTPConfigInfo();
        #endregion

        private static string m_configfilepath = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/ftp.config");

        /// <summary>
        /// 程序刚加载时ftp.config文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;
        /// <summary>
        /// 最近ftp.config文件修改时间
        /// </summary>
        private static DateTime m_filenewchange;


        static FTPConfigs()
        {
            if (Utils.FileExists(m_configfilepath))
            {
                SetFtpConfigInfo();
                m_fileoldchange = System.IO.File.GetLastWriteTime(m_configfilepath);
            }
        }

        /// <summary>
        /// 设置FTP对象信息
        /// </summary>
        private static void SetFtpConfigInfo()
        {
            FTPConfigInfoCollection ftpconfiginfocollection = (FTPConfigInfoCollection) SerializationHelper.Load(typeof(FTPConfigInfoCollection), m_configfilepath);

            FTPConfigInfoCollection.FTPConfigInfoCollectionEnumerator fcice = ftpconfiginfocollection.GetEnumerator();

            //遍历集合并设置相应的FTP信息(静态)对象
            while (fcice.MoveNext())
            {
                if (fcice.Current.Name == "ForumAttach")
                {
                    m_forumattach = fcice.Current;
                    continue;
                }

                if (fcice.Current.Name == "SpaceAttach")
                {
                    m_spaceattach = fcice.Current;
                    continue;
                }

                if (fcice.Current.Name == "AlbumAttach")
                {
                    m_albumattach = fcice.Current;
                    continue;
                }

                if (fcice.Current.Name == "MallAttach")
                {
                    m_mallattach = fcice.Current;
                    continue;
                }

                if (fcice.Current.Name == "ForumAvatar")
                {
                    m_forumavatar = fcice.Current;
                    continue;
                }
            }
        }


        /// <summary>
        /// FTP配置文件监视方法
        /// </summary>
        private static void FtpFileMonitor()
        {
            if (Utils.FileExists(m_configfilepath))
            {
                //获取文件最近修改时间 
                m_filenewchange = System.IO.File.GetLastWriteTime(m_configfilepath);

                //当ftp.config修改时间发生变化时
                if (m_fileoldchange != m_filenewchange)
                {
                    lock (lockhelper)
                    {
                        if (m_fileoldchange != m_filenewchange)
                        {
                            //当文件发生修改(时间变化)则重新设置相关FTP信息对象
                            SetFtpConfigInfo();
                            m_fileoldchange = m_filenewchange;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 论坛附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetForumAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_forumattach;
            }
        }

        /// <summary>
        /// 空间附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetSpaceAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_spaceattach;
            }
        }

        /// <summary>
        /// 相册附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetAlbumAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_albumattach;
            }
        }

        /// <summary>
        /// 相册附件FTP信息
        /// </summary>
        public static FTPConfigInfo GetMallAttachInfo
        {
            get
            {
                FtpFileMonitor();
                return m_mallattach;
            }
        }

        /// <summary>
        /// 论坛头像FTP信息
        /// </summary>
        public static FTPConfigInfo GetForumAvatarInfo
        {
            get
            {
                FtpFileMonitor();
                return m_forumavatar;
            }
        }
        

        ///// <summary>
        ///// 获取配置类实例
        ///// </summary>
        ///// <returns></returns>
        //public static FTPConfigInfo GetConfig()
        //{
        //    return FTPConfigFileManager.LoadConfig();
        //}

        ///// <summary>
        ///// 保存配置类实例
        ///// </summary>
        ///// <param name="emailconfiginfo"></param>
        ///// <returns></returns>
        //public static bool SaveConfig(FTPConfigInfo ftpconfiginfo)
        //{
        //    FTPConfigFileManager fcfm = new FTPConfigFileManager();
        //    FTPConfigFileManager.ConfigInfo = ftpconfiginfo;
        //    return fcfm.SaveConfig();
        //}
    }

    #endregion
}
