using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Web.Hosting;

using System.Data;
using System.Text;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;

namespace Discuz.Web.Services
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

    [WebService(Namespace = "Discuz.Web.Services")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MixObjects : System.Web.Services.WebService
    {

        public MixObjects()
        { }

        [WebMethod]
        public string ADMedia(string pagename, int forumid)
        {
            string[] parameters = Forum.Advertisements.GetMediaAdParams(pagename, forumid);
            return @"<?xml version='1.0'?>" +
                  " <AD>" +
                  "  <play>" +
                  "     <playmodel>3</playmodel>" +
                  "     <backgrounduri>" + parameters[2] + "</backgrounduri>" +
                  "     <mediauri>" + parameters[1] + "</mediauri>" +
                  "  </play>" +
                  "  <speed>" +
                  "     <group>" +
                  "        <speedbackground>" + parameters[4] + "</speedbackground>" +
                  "        <speedphoto>assets/images/heros03.png</speedphoto>" +
                  "        <speedtext>assets/images/heros02.png</speedtext>" +
                  "     </group>" +
                  "  </speed>" +
                  "  <screen>" +
                  "     <group>" +
                  "        <text>" + parameters[5] + "</text>" +
                  "        <textwidth>320</textwidth>" +
                  "        <textheight>18</textheight>" +
                  "        <image>assets/images/smallmsnbc.png</image>" +
                  "        <imagewidth>15</imagewidth>" +
                  "        <imageheight>9</imageheight>" +
                  "        <text>" + parameters[6] + "</text>" +
                  "        <textwidth>320</textwidth>" +
                  "        <textheight>18</textheight>" +
                  "        <image>assets/images/smallmsnbc.png</image>" +
                  "        <imagewidth>15</imagewidth>" +
                  "        <imageheight>9</imageheight>" +
                  "        <text>" + parameters[7] + "</text>" +
                  "        <textwidth>320</textwidth>" +
                  "        <textheight>18</textheight>" +
                  "        <image>assets/images/smallmsnbc.png</image>" +
                  "        <imagewidth>15</imagewidth>" +
                  "        <imageheight>9</imageheight>" +
                  "    </group>" +
                  "  </screen>" +
                  " </AD>";
        }

        //[WebMethod]
        //public string ChartData(int topicid)
        //{
        //    DataTable polllist = Discuz.Forum.Polls.GetPollOptionList(topicid);
        //    StringBuilder builder = new StringBuilder("{CreateDate:'");
        //    builder.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
        //    builder.Append("',Sectors:[");
        //    int i = 0;
        //    foreach (DataRow dr in polllist.Rows)
        //    {
        //        builder.Append("{");
        //        builder.AppendFormat("ID:{0},", i);
        //        builder.AppendFormat("Value:{0},", (Convert.ToDouble(dr["percent"].ToString().Replace("%", string.Empty))/100.00).ToString());
        //        builder.AppendFormat("Title:'{0}',", dr["name"]);
        //        builder.AppendFormat("Comment:'{0}'", dr["value"]);
        //        builder.Append("},");
        //        i++;
        //    }
        //    if (polllist.Rows.Count > 0)
        //        builder.Remove(builder.Length - 1, 1);

        //    builder.Append("]}");
        //    return builder.ToString();
        //}

        [WebMethod]
        public string PollPieData(int topicid)
        {
            List<PollOptionInfo> pollOptionInfoList = Discuz.Data.Polls.GetPollOptionInfoCollection(topicid);
            StringBuilder builder = new StringBuilder("{CreateDate:'");
            builder.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
            builder.Append("',Sectors:[");
            int i = 0;
            foreach (PollOptionInfo pollOptionInfo in pollOptionInfoList)
            {
                builder.Append("{");
                builder.AppendFormat("ID:{0},", i);
                builder.AppendFormat("Value:{0},", pollOptionInfo.Votes);
                builder.AppendFormat("Title:'{0}. {1}',", i+1, pollOptionInfo.Polloption);
                builder.AppendFormat("Comment:'{0}'", pollOptionInfo.Polloptionid);
                builder.Append("},");
                i++;
            }
            if (pollOptionInfoList.Count > 0)
                builder.Remove(builder.Length - 1, 1);

            builder.Append("]}");
            return builder.ToString();
        }

        [WebMethod]
        public string AlbumData(int albumid)
        {
            string jsonpath = BaseConfigs.GetForumPath + "cache/album/" + (albumid / 1000 + 1).ToString() + "/" + albumid + "_json.txt";
            if (File.Exists(jsonpath))
            {
                using (StreamReader sr = new StreamReader(jsonpath, Encoding.UTF8))
                {
                    string Content = sr.ReadToEnd();
                    sr.Close();
                    if (Content.Trim() != string.Empty)
                        return Content.Trim();
                }
            }
            try
            {
                return Plugin.Album.AlbumPluginProvider.GetInstance().GetAlbumJsonData(albumid);
            }
            catch
            {
                return "";
            }
        }



        #region 文件上传
        /// <summary>
        /// WEB权限认证
        /// </summary>
        /// <param name="creinfo">认证信息</param>
        /// <returns>是否通过验正</returns>
        private bool AuthenticateUser(CredentialInfo creinfo)
        {
            if (creinfo.ForumID > 0)
            {
                int olid = Discuz.Forum.OnlineUsers.GetOlidByUid(creinfo.UserID);
                if (olid > 0)
                {
                    OnlineUserInfo oluserinfo = Discuz.Forum.OnlineUsers.GetOnlineUser(olid);
                    if (oluserinfo.Userid == creinfo.UserID && Utils.UrlEncode(Discuz.Forum.ForumUtils.SetCookiePassword(oluserinfo.Password.Trim(), GeneralConfigs.GetConfig().Passwordkey)) == creinfo.Password &&//检测用户id和口令
                        creinfo.AuthToken == DES.Encode(string.Format("{0},{1}", oluserinfo.Olid.ToString(), oluserinfo.Username.ToString()), oluserinfo.Password.Substring(0, 10)).Replace("+", "["))//检查认证信息
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 通过指定用户认证信息来获得该用户的上传设置信息
        /// </summary>
        /// <param name="creinfo">用户认证信息</param>
        /// <returns></returns>
        [WebMethod]
        public UploadSetInfo GetAttachmentUploadSet(CredentialInfo creinfo)
        {
            if (AuthenticateUser(creinfo))
            {
                UserInfo userinfo = Discuz.Forum.Users.GetUserInfo(creinfo.UserID);
                if (userinfo == null)
                    return new UploadSetInfo("", "", 0, 0, false, 0, "当前用户信息无效,请尝试刷新");

                UserGroupInfo usergroupinfo = Discuz.Forum.UserGroups.GetUserGroupInfo(userinfo.Groupid);
                if (usergroupinfo == null)
                    return new UploadSetInfo("", "", 0, 0, false, 0, "当前用户所属用户组信息无效");


                ForumInfo forum = Discuz.Forum.Forums.GetForumInfo(creinfo.ForumID);
                if (forum == null)
                    return new UploadSetInfo(null, null, 0, 0, false, 0, "当前版块信息无效,请尝试刷新");

                //得到用户可以上传的文件类型
                StringBuilder sbAttachmentTypeSelect = new StringBuilder();
                if (!usergroupinfo.Attachextensions.Trim().Equals(""))
                {
                    sbAttachmentTypeSelect.Append("[id] in (");
                    sbAttachmentTypeSelect.Append(usergroupinfo.Attachextensions);
                    sbAttachmentTypeSelect.Append(")");
                }

                if (!forum.Attachextensions.Equals(""))
                {
                    if (sbAttachmentTypeSelect.Length > 0)
                        sbAttachmentTypeSelect.Append(" AND ");

                    sbAttachmentTypeSelect.Append("[id] in (");
                    sbAttachmentTypeSelect.Append(forum.Attachextensions);
                    sbAttachmentTypeSelect.Append(")");
                }
                string attachextensions = Discuz.Forum.Attachments.GetAttachmentTypeArray(sbAttachmentTypeSelect.ToString());
                string attachextensionsnosize = Discuz.Forum.Attachments.GetAttachmentTypeString(sbAttachmentTypeSelect.ToString());

                //得到今天允许用户上传的附件总大小(字节)
                int MaxTodaySize = 0;
                if (creinfo.UserID > 0)
                    MaxTodaySize = Discuz.Forum.Attachments.GetUploadFileSizeByuserid(creinfo.UserID);

                int attachsize = usergroupinfo.Maxsizeperday - MaxTodaySize;//今天可上传大小

                bool canpostattach = false; //是否允许上传附件
                //是否有上传附件的权限
                if (Discuz.Forum.Forums.AllowPostAttachByUserID(forum.Permuserlist, creinfo.UserID))
                    canpostattach = true;
                else
                {
                    if (forum.Postattachperm == "")
                    {
                        if (usergroupinfo.Allowpostattach == 1)
                            canpostattach = true;
                    }
                    else
                    {
                        if (Discuz.Forum.Forums.AllowPostAttach(forum.Postattachperm, usergroupinfo.Groupid))
                            canpostattach = true;
                    }
                }
                return new UploadSetInfo(attachextensions, attachextensionsnosize, MaxTodaySize, attachsize, canpostattach, usergroupinfo.Maxattachsize, "");
            }

            return new UploadSetInfo("", "", 0, 0, false, 0, "当前用户信息无效,请尝试刷新");

        }

        //上传临时文件后缀
        private string _tempExtension = "_temp";
        private static object lockHelper = new object();

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="data">上传的字节数据</param>
        /// <param name="dataLength">数据长度</param>
        /// <param name="parameters">上传参数</param>
        /// <param name="firstChunk">是否第一块数据</param>
        /// <param name="lastChunk">是否最后一块数据</param>
        [WebMethod]
        public AttachmentInfo StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, CredentialInfo creinfo)
        {
            if (AuthenticateUser(creinfo))
            {
                UploadSetInfo uploadSetInfo = GetAttachmentUploadSet(creinfo);
                string fileextname = Utils.CutString(fileName, fileName.LastIndexOf(".") + 1).ToLower();

                if (uploadSetInfo.CanPostAttach && uploadSetInfo.AttachExtensionsNoSize.IndexOf(fileextname) >= 0 && uploadSetInfo.AttachSize > dataLength &&
                    Utils.StrIsNullOrEmpty(uploadSetInfo.ErrMessage))
                {
                    string uploadFolder = GetUploadFolder(fileName, creinfo.ForumID.ToString());
                    string tempFileName = fileName + _tempExtension;

                    if (firstChunk)
                    {
                        //删除临时文件
                        if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/upload/temp/" + tempFileName))
                            File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/upload/temp/" + tempFileName);

                        //删除目录文件
                        if (File.Exists(uploadFolder + "/" + fileName))
                            File.Delete(uploadFolder + "/" + fileName);
                    }


                    FileStream fs = File.Open(@HostingEnvironment.ApplicationPhysicalPath + "/upload/temp/" + tempFileName, FileMode.Append);
                    fs.Write(data, 0, dataLength);
                    fs.Close();
                    fs.Dispose();
                    if (lastChunk)
                    {
                        lock (lockHelper)
                        {
                            string newfilename = (Environment.TickCount & int.MaxValue).ToString() + new Random().Next(1000, 9999) + "." + fileextname;
                            File.Move(HostingEnvironment.ApplicationPhysicalPath + "/upload/temp/" + tempFileName, uploadFolder + "/" + newfilename);
                         
                            try
                            {
                                //当支持FTP上传附件时,使用FTP上传远程附件
                                if (FTPs.GetForumAttachInfo != null && FTPs.GetForumAttachInfo.Allowupload == 1)
                                {
                                    FTPs ftps = new FTPs();
                                    //当不保留本地附件模式时,在上传完成之后删除本地tempfilename文件
                                    ftps.UpLoadFile(newfilename, uploadFolder + "/" + newfilename, FTPs.FTPUploadEnum.ForumAttach);
                                }
                            }
                            catch
                            {
                                ;
                            }
                            return Attachments.GetAttachmentInfo(AddAttachment(newfilename, fileName, creinfo));
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 取消上传
        /// </summary>
        /// <param name="creinfo">认证信息</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="forumid">版块id</param>
        [WebMethod]
        public void CancelUpload(CredentialInfo creinfo, string fileName, string forumid)
        {
            if (AuthenticateUser(creinfo))
            {
                string uploadFolder = GetUploadFolder(fileName, forumid);
                string tempFileName = fileName + _tempExtension;

                if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);
            }
        }

        /// <summary>
        /// 删除指定的上传文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="forumid">版块id</param>
        private void DeleteUploadedFile(string fileName, string forumid)
        {
            string uploadFolder = GetUploadFolder(fileName, forumid);

            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);
        }

        private GeneralConfigInfo config = GeneralConfigs.GetConfig();

        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="savedfileName">上传之后保存的文件名称</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="creinfo">认证信息</param>
        /// <returns>返回当前插入的附件id</returns>
        private int AddAttachment(string savedFileName, string fileName,  CredentialInfo creinfo)
        {
            string UploadDir = GetUploadFolder(savedFileName, creinfo.ForumID.ToString());
            AttachmentInfo attachmentinfo = new AttachmentInfo();
            string fileextname = Utils.CutString(savedFileName, savedFileName.LastIndexOf(".") + 1).ToLower();
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            string newfilename = string.Format("{0}{1}{2}.{3}",
                              (Environment.TickCount & int.MaxValue).ToString(),
                              random.Next(1000, 99999), random.Next(1000, 99999), fileextname);
            try
            {
                // 如果是bmp jpg png图片类型
                if ((fileextname == "bmp" || fileextname == "jpg" || fileextname == "jpeg" || fileextname == "png"))
                {
                    if (Discuz.Common.Utils.FileExists(UploadDir + savedFileName))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(UploadDir + savedFileName);
                        //System.IO.File.Copy(UploadDir + savedFileName, UploadDir + newfilename, true);
                        if (config.Attachimgmaxwidth > 0 && img.Width > config.Attachimgmaxwidth)
                            attachmentinfo.Sys_noupload = "图片宽度为" + img.Width.ToString() + ", 系统允许的最大宽度为" + config.Attachimgmaxwidth.ToString();

                        if (config.Attachimgmaxheight > 0 && img.Height > config.Attachimgmaxheight)
                            attachmentinfo.Sys_noupload = "图片高度为" + img.Width.ToString() + ", 系统允许的最大高度为" + config.Attachimgmaxheight.ToString();

                        attachmentinfo.Width = img.Width;
                        attachmentinfo.Height = img.Height;

                        if (config.Watermarkstatus == 0)
                        {
                            img.Dispose();
                            File.Move(UploadDir + savedFileName, UploadDir + newfilename);
                        }
                        else
                        {
                            if (config.Watermarktype == 1 && File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic)))
                                Discuz.Forum.ForumUtils.AddImageSignPic(img, UploadDir + newfilename, Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic), config.Watermarkstatus, config.Attachimgquality, config.Watermarktransparency);
                            else
                            {
                                string watermarkText;
                                watermarkText = config.Watermarktext.Replace("{1}", config.Forumtitle);
                                watermarkText = watermarkText.Replace("{2}", "http://" + DNTRequest.GetCurrentFullHost() + "/");
                                watermarkText = watermarkText.Replace("{3}", Utils.GetDate());
                                watermarkText = watermarkText.Replace("{4}", Utils.GetTime());

                                Discuz.Forum.ForumUtils.AddImageSignText(img, UploadDir + newfilename, watermarkText, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                            }
                            System.IO.File.Delete(UploadDir + savedFileName);
                        }
                        // 获得文件长度
                        attachmentinfo.Filesize = new FileInfo(UploadDir + newfilename).Length;
                    }
                }
                else
                {
                    System.IO.File.Move(UploadDir + savedFileName, UploadDir + newfilename);
                    attachmentinfo.Filesize = new FileInfo(UploadDir + newfilename).Length;
                }
            }
            catch {}

            if (Discuz.Common.Utils.FileExists(UploadDir + savedFileName))
            {
                attachmentinfo.Filesize = new FileInfo(UploadDir + savedFileName).Length;
                attachmentinfo.Filename = GetDirInfo(savedFileName, creinfo.ForumID.ToString()) + savedFileName;
            }

            if (Discuz.Common.Utils.FileExists(UploadDir + newfilename))
            {
                attachmentinfo.Filesize = new FileInfo(UploadDir + newfilename).Length;
                attachmentinfo.Filename = GetDirInfo(newfilename, creinfo.ForumID.ToString()) + newfilename;
            }

            //当支持FTP上传附件时
            if (FTPs.GetForumAttachInfo != null && FTPs.GetForumAttachInfo.Allowupload == 1)
                attachmentinfo.Filename = FTPs.GetForumAttachInfo.Remoteurl + "/" + newfilename.Replace("\\", "/");
        
            attachmentinfo.Uid = creinfo.UserID;
            attachmentinfo.Description = fileextname;
            attachmentinfo.Filetype = GetContentType(fileextname);
            attachmentinfo.Attachment = fileName;
            attachmentinfo.Downloads = 0;
            attachmentinfo.Postdatetime = DateTime.Now.ToString();
            attachmentinfo.Sys_index = 0;

            //return Discuz.Data.DatabaseProvider.GetInstance().CreateAttachment(attachmentinfo);
            return Discuz.Data.Attachments.CreateAttachments(attachmentinfo);
        }

        #region 获取相应扩展名的ContentType类型
        private string GetContentType(string fileextname)
        {
            switch (fileextname)
            {
                #region 常用文件类型
                case "jpeg": return "image/jpeg";
                case "jpg": return "image/jpeg";
                case "js": return "application/x-javascript";
                case "jsp": return "text/html";
                case "gif": return "image/gif";
                case "htm": return "text/html";
                case "html": return "text/html";
                case "asf": return "video/x-ms-asf";
                case "avi": return "video/avi";
                case "bmp": return "application/x-bmp";
                case "asp": return "text/asp";
                case "wma": return "audio/x-ms-wma";
                case "wav": return "audio/wav";
                case "wmv": return "video/x-ms-wmv";
                case "ra": return "audio/vnd.rn-realaudio";
                case "ram": return "audio/x-pn-realaudio";
                case "rm": return "application/vnd.rn-realmedia";
                case "rmvb": return "application/vnd.rn-realmedia-vbr";
                case "xhtml": return "text/html";
                case "png": return "image/png";
                case "ppt": return "application/x-ppt";
                case "tif": return "image/tiff";
                case "tiff": return "image/tiff";
                case "xls": return "application/x-xls";
                case "xlw": return "application/x-xlw";
                case "xml": return "text/xml";
                case "xpl": return "audio/scpls";
                case "swf": return "application/x-shockwave-flash";
                case "torrent": return "application/x-bittorrent";
                case "dll": return "application/x-msdownload";
                case "asa": return "text/asa";
                case "asx": return "video/x-ms-asf";
                case "au": return "audio/basic";
                case "css": return "text/css";
                case "doc": return "application/msword";
                case "exe": return "application/x-msdownload";
                case "mp1": return "audio/mp1";
                case "mp2": return "audio/mp2";
                case "mp2v": return "video/mpeg";
                case "mp3": return "audio/mp3";
                case "mp4": return "video/mpeg4";
                case "mpa": return "video/x-mpg";
                case "mpd": return "application/vnd.ms-project";
                case "mpe": return "video/x-mpeg";
                case "mpeg": return "video/mpg";
                case "mpg": return "video/mpg";
                case "mpga": return "audio/rn-mpeg";
                case "mpp": return "application/vnd.ms-project";
                case "mps": return "video/x-mpeg";
                case "mpt": return "application/vnd.ms-project";
                case "mpv": return "video/mpg";
                case "mpv2": return "video/mpeg";
                case "wml": return "text/vnd.wap.wml";
                case "wsdl": return "text/xml";
                case "xsd": return "text/xml";
                case "xsl": return "text/xml";
                case "xslt": return "text/xml";
                case "htc": return "text/x-component";
                case "mdb": return "application/msaccess";
                #endregion

                case "*": return "application/octet-stream";
                case "001": return "application/x-001";
                case "301": return "application/x-301";
                case "323": return "text/h323";
                case "906": return "application/x-906";
                case "907": return "drawing/907";
                case "a11": return "application/x-a11";
                case "acp": return "audio/x-mei-aac";
                case "ai": return "application/postscript";
                case "aif": return "audio/aiff";
                case "aifc": return "audio/aiff";
                case "aiff": return "audio/aiff";
                case "anv": return "application/x-anv";
                case "awf": return "application/vnd.adobe.workflow";
                case "biz": return "text/xml";
                case "bot": return "application/x-bot";
                case "c4t": return "application/x-c4t";
                case "c90": return "application/x-c90";
                case "cal": return "application/x-cals";
                case "cat": return "application/vnd.ms-pki.seccat";
                case "cdf": return "application/x-netcdf";
                case "cdr": return "application/x-cdr";
                case "cel": return "application/x-cel";
                case "cer": return "application/x-x509-ca-cert";
                case "cg4": return "application/x-g4";
                case "cgm": return "application/x-cgm";
                case "cit": return "application/x-cit";
                case "class": return "java/*";
                case "cml": return "text/xml";
                case "cmp": return "application/x-cmp";
                case "cmx": return "application/x-cmx";
                case "cot": return "application/x-cot";
                case "crl": return "application/pkix-crl";
                case "crt": return "application/x-x509-ca-cert";
                case "csi": return "application/x-csi";
                case "cut": return "application/x-cut";
                case "dbf": return "application/x-dbf";
                case "dbm": return "application/x-dbm";
                case "dbx": return "application/x-dbx";
                case "dcd": return "text/xml";
                case "dcx": return "application/x-dcx";
                case "der": return "application/x-x509-ca-cert";
                case "dgn": return "application/x-dgn";
                case "dib": return "application/x-dib";
                case "dot": return "application/msword";
                case "drw": return "application/x-drw";
                case "dtd": return "text/xml";
                case "dwf": return "application/x-dwf";
                case "dwg": return "application/x-dwg";
                case "dxb": return "application/x-dxb";
                case "dxf": return "application/x-dxf";
                case "edn": return "application/vnd.adobe.edn";
                case "emf": return "application/x-emf";
                case "eml": return "message/rfc822";
                case "ent": return "text/xml";
                case "epi": return "application/x-epi";
                case "eps": return "application/x-ps";
                case "etd": return "application/x-ebx";
                case "fax": return "image/fax";
                case "fdf": return "application/vnd.fdf";
                case "fif": return "application/fractals";
                case "fo": return "text/xml";
                case "frm": return "application/x-frm";
                case "g4": return "application/x-g4";
                case "gbr": return "application/x-gbr";
                case "gcd": return "application/x-gcd";

                case "gl2": return "application/x-gl2";
                case "gp4": return "application/x-gp4";
                case "hgl": return "application/x-hgl";
                case "hmr": return "application/x-hmr";
                case "hpg": return "application/x-hpgl";
                case "hpl": return "application/x-hpl";
                case "hqx": return "application/mac-binhex40";
                case "hrf": return "application/x-hrf";
                case "hta": return "application/hta";
                case "htt": return "text/webviewhtml";
                case "htx": return "text/html";
                case "icb": return "application/x-icb";
                case "ico": return "application/x-ico";
                case "iff": return "application/x-iff";
                case "ig4": return "application/x-g4";
                case "igs": return "application/x-igs";
                case "iii": return "application/x-iphone";
                case "img": return "application/x-img";
                case "ins": return "application/x-internet-signup";
                case "isp": return "application/x-internet-signup";
                case "IVF": return "video/x-ivf";
                case "java": return "java/*";
                case "jfif": return "image/jpeg";
                case "jpe": return "application/x-jpe";
                case "la1": return "audio/x-liquid-file";
                case "lar": return "application/x-laplayer-reg";
                case "latex": return "application/x-latex";
                case "lavs": return "audio/x-liquid-secure";
                case "lbm": return "application/x-lbm";
                case "lmsff": return "audio/x-la-lms";
                case "ls": return "application/x-javascript";
                case "ltr": return "application/x-ltr";
                case "m1v": return "video/x-mpeg";
                case "m2v": return "video/x-mpeg";
                case "m3u": return "audio/mpegurl";
                case "m4e": return "video/mpeg4";
                case "mac": return "application/x-mac";
                case "man": return "application/x-troff-man";
                case "math": return "text/xml";
                case "mfp": return "application/x-shockwave-flash";
                case "mht": return "message/rfc822";
                case "mhtml": return "message/rfc822";
                case "mi": return "application/x-mi";
                case "mid": return "audio/mid";
                case "midi": return "audio/mid";
                case "mil": return "application/x-mil";
                case "mml": return "text/xml";
                case "mnd": return "audio/x-musicnet-download";
                case "mns": return "audio/x-musicnet-stream";
                case "mocha": return "application/x-javascript";
                case "movie": return "video/x-sgi-movie";
                case "mpw": return "application/vnd.ms-project";
                case "mpx": return "application/vnd.ms-project";
                case "mtx": return "text/xml";
                case "mxp": return "application/x-mmxp";
                case "net": return "image/pnetvue";
                case "nrf": return "application/x-nrf";
                case "nws": return "message/rfc822";
                case "odc": return "text/x-ms-odc";
                case "out": return "application/x-out";
                case "p10": return "application/pkcs10";
                case "p12": return "application/x-pkcs12";
                case "p7b": return "application/x-pkcs7-certificates";
                case "p7c": return "application/pkcs7-mime";
                case "p7m": return "application/pkcs7-mime";
                case "p7r": return "application/x-pkcs7-certreqresp";
                case "p7s": return "application/pkcs7-signature";
                case "pc5": return "application/x-pc5";
                case "pci": return "application/x-pci";
                case "pcl": return "application/x-pcl";
                case "pcx": return "application/x-pcx";
                case "pdf": return "application/pdf";
                case "pdx": return "application/vnd.adobe.pdx";
                case "pfx": return "application/x-pkcs12";
                case "pgl": return "application/x-pgl";
                case "pic": return "application/x-pic";
                case "pko": return "application/vnd.ms-pki.pko";
                case "pl": return "application/x-perl";
                case "plg": return "text/html";
                case "pls": return "audio/scpls";
                case "plt": return "application/x-plt";
                case "pot": return "application/vnd.ms-powerpoint";
                case "ppa": return "application/vnd.ms-powerpoint";
                case "ppm": return "application/x-ppm";
                case "pps": return "application/vnd.ms-powerpoint";
                case "pr": return "application/x-pr";
                case "prf": return "application/pics-rules";
                case "prn": return "application/x-prn";
                case "prt": return "application/x-prt";
                case "ps": return "application/x-ps";
                case "ptn": return "application/x-ptn";
                case "pwz": return "application/vnd.ms-powerpoint";
                case "r3t": return "text/vnd.rn-realtext3d";
                case "ras": return "application/x-ras";
                case "rat": return "application/rat-file";
                case "rdf": return "text/xml";
                case "rec": return "application/vnd.rn-recording";
                case "red": return "application/x-red";
                case "rgb": return "application/x-rgb";
                case "rjs": return "application/vnd.rn-realsystem-rjs";
                case "rjt": return "application/vnd.rn-realsystem-rjt";
                case "rlc": return "application/x-rlc";
                case "rle": return "application/x-rle";
                case "rmf": return "application/vnd.adobe.rmf";
                case "rmi": return "audio/mid";
                case "rmj": return "application/vnd.rn-realsystem-rmj";
                case "rmm": return "audio/x-pn-realaudio";
                case "rmp": return "application/vnd.rn-rn_music_package";
                case "rms": return "application/vnd.rn-realmedia-secure";
                case "rmx": return "application/vnd.rn-realsystem-rmx";
                case "rnx": return "application/vnd.rn-realplayer";
                case "rp": return "image/vnd.rn-realpix";
                case "rpm": return "audio/x-pn-realaudio-plugin";
                case "rsml": return "application/vnd.rn-rsml";
                case "rt": return "text/vnd.rn-realtext";
                case "rtf": return "application/msword";
                case "rv": return "video/vnd.rn-realvideo";
                case "sam": return "application/x-sam";
                case "sat": return "application/x-sat";
                case "sdp": return "application/sdp";
                case "sdw": return "application/x-sdw";
                case "sit": return "application/x-stuffit";
                case "slb": return "application/x-slb";
                case "sld": return "application/x-sld";
                case "slk": return "drawing/x-slk";
                case "smi": return "application/smil";
                case "smil": return "application/smil";
                case "smk": return "application/x-smk";
                case "snd": return "audio/basic";
                case "sol": return "text/plain";
                case "sor": return "text/plain";
                case "spc": return "application/x-pkcs7-certificates";
                case "spl": return "application/futuresplash";
                case "spp": return "text/xml";
                case "ssm": return "application/streamingmedia";
                case "sst": return "application/vnd.ms-pki.certstore";
                case "stl": return "application/vnd.ms-pki.stl";
                case "stm": return "text/html";
                case "sty": return "application/x-sty";
                case "svg": return "text/xml";
                case "tdf": return "application/x-tdf";
                case "tg4": return "application/x-tg4";
                case "tga": return "application/x-tga";
                case "tld": return "text/xml";
                case "top": return "drawing/x-top";
                case "tsd": return "text/xml";
                case "txt": return "text/plain";
                case "uin": return "application/x-icq";
                case "uls": return "text/iuls";
                case "vcf": return "text/x-vcard";
                case "vda": return "application/x-vda";
                case "vdx": return "application/vnd.visio";
                case "vml": return "text/xml";
                case "vpg": return "application/x-vpeg005";
                case "vsd": return "application/vnd.visio";
                case "vss": return "application/vnd.visio";
                case "vst": return "application/vnd.visio";
                case "vsw": return "application/vnd.visio";
                case "vsx": return "application/vnd.visio";
                case "vtx": return "application/vnd.visio";
                case "vxml": return "text/xml";
                case "wax": return "audio/x-ms-wax";
                case "wb1": return "application/x-wb1";
                case "wb2": return "application/x-wb2";
                case "wb3": return "application/x-wb3";
                case "wbmp": return "image/vnd.wap.wbmp";
                case "wiz": return "application/msword";
                case "wk3": return "application/x-wk3";
                case "wk4": return "application/x-wk4";
                case "wkq": return "application/x-wkq";
                case "wks": return "application/x-wks";
                case "wm": return "video/x-ms-wm";
                case "wmd": return "application/x-ms-wmd";
                case "wmf": return "application/x-wmf";
                case "wmx": return "video/x-ms-wmx";
                case "wmz": return "application/x-ms-wmz";
                case "wp6": return "application/x-wp6";
                case "wpd": return "application/x-wpd";
                case "wpg": return "application/x-wpg";
                case "wpl": return "application/vnd.ms-wpl";
                case "wq1": return "application/x-wq1";
                case "wr1": return "application/x-wr1";
                case "wri": return "application/x-wri";
                case "wrk": return "application/x-wrk";
                case "ws": return "application/x-ws";
                case "ws2": return "application/x-ws";
                case "wsc": return "text/scriptlet";
                case "wvx": return "video/x-ms-wvx";
                case "xdp": return "application/vnd.adobe.xdp";
                case "xdr": return "text/xml";
                case "xfd": return "application/vnd.adobe.xfd";
                case "xfdf": return "application/vnd.adobe.xfdf";
                case "xq": return "text/xml";
                case "xql": return "text/xml";
                case "xquery": return "text/xml";
                case "xwd": return "application/x-xwd";
                case "x_b": return "application/x-x_b";
                case "x_t": return "application/x-x_t";
            }
            return "";
        }
        #endregion

        /// <summary>
        /// 获得上传路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="forumid">论坛版块id</param>
        /// <returns>返回上传路径</returns>
        private string GetUploadFolder(string filename, string forumid)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            string UploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/");

            string savedir = GetDirInfo(filename, forumid);
            // 如果指定目录不存在则建立
            if (!Directory.Exists(UploadDir + savedir))
                Utils.CreateDir(UploadDir + savedir);

            // 如果临时目录不存在则建立
            if (!Directory.Exists(UploadDir + "\\temp\\"))
                Utils.CreateDir(UploadDir + "\\temp\\");

            return UploadDir + savedir;
        }

        /// <summary>
        /// 获得附件保存方式路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="forumid">论坛版块id</param>
        /// <returns>返回附件保存方式路径</returns>
        private string GetDirInfo(string filename, string forumid)
        {
            string fileextname = Utils.CutString(filename, filename.LastIndexOf(".") + 1).ToLower();
            StringBuilder savedir = new StringBuilder("");
            //附件保存方式 0=按年/月/日存入不同目录 1=按年/月/日/论坛存入不同目录 2=按论坛存入不同目录 3=按文件类型存入不同目录
            if (config.Attachsave == 1)
            {
                savedir.Append(DateTime.Now.ToString("yyyy"));
                savedir.Append(Path.DirectorySeparatorChar);
                savedir.Append(DateTime.Now.ToString("MM"));
                savedir.Append(Path.DirectorySeparatorChar);
                savedir.Append(DateTime.Now.ToString("dd"));
                savedir.Append(Path.DirectorySeparatorChar);
                savedir.Append(forumid.ToString());
                savedir.Append(Path.DirectorySeparatorChar);
            }
            else if (config.Attachsave == 2)
            {
                savedir.Append(forumid);
                savedir.Append(Path.DirectorySeparatorChar);
            }
            else if (config.Attachsave == 3)
            {
                savedir.Append(fileextname);
                savedir.Append(Path.DirectorySeparatorChar);
            }
            else
            {
                savedir.Append(DateTime.Now.ToString("yyyy"));
                savedir.Append(Path.DirectorySeparatorChar);
                savedir.Append(DateTime.Now.ToString("MM"));
                savedir.Append(Path.DirectorySeparatorChar);
                savedir.Append(DateTime.Now.ToString("dd"));
                savedir.Append(Path.DirectorySeparatorChar);
            }
            return savedir.ToString();
        }
        #endregion
    }
}
