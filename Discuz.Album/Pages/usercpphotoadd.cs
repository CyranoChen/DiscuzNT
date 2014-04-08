using System;
using System.IO;
using System.Web;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Album.Data;

namespace Discuz.Album.Pages
{
    /// <summary>
    /// 个人中心添加照片
    /// </summary>
    public class usercpphotoadd : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 空余图片空间大小
        /// </summary>
        public int freephotosize = 0;
        /// <summary>
        /// 相册名称
        /// </summary>
        public string albumname = "";
        /// <summary>
        /// 相册Id
        /// </summary>
        public int albumid = DNTRequest.GetQueryInt("albumid", 0);
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 当前是否启用了Tag
        /// </summary>
        public bool enabletag = false;
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            user = Users.GetUserInfo(userid);

            if (config.Enablealbum != 1)
            {
                AddErrLine("相册功能已被关闭");
                return;
            }
            if (albumid < 1)
            {
                AddErrLine("指定的相册不存在");
                return;
            }
            AlbumInfo albuminfo = DTOProvider.GetAlbumInfo(albumid);
            if (this.userid != albuminfo.Userid)
            {
                AddErrLine("您无权限在该相册内添加照片");
                return;
            }           

            enabletag = config.Enabletag == 1;
            freephotosize = UserGroups.GetUserGroupInfo(usergroupid).Maxspacephotosize - Data.DbProvider.GetInstance().GetPhotoSizeByUserid(userid);            
            albumname = albuminfo.Title;
           
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                HttpFileCollection files = HttpContext.Current.Request.Files;
                int imgcount = 0;

                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    HttpPostedFile postedFile = files[iFile];
                    if (postedFile == null || postedFile.FileName == "")
                        continue;
                    string fileName, fileExtension;
                    fileName = Path.GetFileName(postedFile.FileName);
                    fileExtension = Path.GetExtension(fileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".gif" && fileExtension != ".png" && fileExtension != ".jpeg")
                        continue;
                    
                    //判断用户是否达到了照片最大上传数
                    int filesize = postedFile.ContentLength;
                    if (freephotosize < filesize)
                    {
                        AddErrLine("照片上传空间数已满，某些照片不能再上传！<br />如果想继续上传，请删除以前旧照片！");
                        return;
                    }
                    string phototitle = DNTRequest.GetFormString("phototitle" + (iFile + 1));
                    PhotoInfo spacephotoinfo = new PhotoInfo();
                    spacephotoinfo.Title = Utils.StrIsNullOrEmpty(phototitle) ? fileName.Remove(fileName.IndexOf("."), 1) : phototitle;
                    spacephotoinfo.Albumid = albumid;
                    spacephotoinfo.Userid = userid;
                    string[] currentdate = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
                    string uploaddir = "";

                    //当支持FTP上传远程照片
                    if (FTPs.GetAlbumAttachInfo.Allowupload == 1)
                    {
                        //当不保留本地附件模式时
                        if (FTPs.GetAlbumAttachInfo.Reservelocalattach == 0)
                            uploaddir = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/upload/temp/");
                        else
                            uploaddir = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/upload/" + currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/");

                        if (!Directory.Exists(uploaddir))
                            Utils.CreateDir(uploaddir);

                        string ftpfilename = Globals.UploadSpaceFile(postedFile, uploaddir, true);
                        spacephotoinfo.Filename = FTPs.GetAlbumAttachInfo.Remoteurl+ "/" + currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/" + ftpfilename;

                        FTPs ftps = new FTPs();
                        ftps.UpLoadFile("/" + currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/" ,
                                        uploaddir + ftpfilename,
                                        FTPs.FTPUploadEnum.AlbumAttach);
                        ftps = new FTPs();
                        ftps.UpLoadFile("/" + currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/" ,
                                        uploaddir + Globals.GetThumbnailImage(ftpfilename),
                                        FTPs.FTPUploadEnum.AlbumAttach);
                        ftps = new FTPs();
                        ftps.UpLoadFile("/" + currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/" ,
                                        uploaddir + Globals.GetSquareImage(ftpfilename),
                                        FTPs.FTPUploadEnum.AlbumAttach);
                    }
                    else 
                    {
                        uploaddir = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/upload/" + currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/");
                        if (!Directory.Exists(uploaddir))
                            Utils.CreateDir(uploaddir);

                        spacephotoinfo.Filename = "space/upload/" + currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/" + Globals.UploadSpaceFile(postedFile, uploaddir, true);
                    }
                    
                    spacephotoinfo.Attachment = fileName;
                    spacephotoinfo.Description = DNTRequest.GetFormString("description" + (iFile + 1));
                    spacephotoinfo.Filesize = filesize;
                    spacephotoinfo.Username = username;
                    spacephotoinfo.IsAttachment = 0;
                    freephotosize -= filesize;
                    int photoid = Data.DbProvider.GetInstance().AddSpacePhoto(spacephotoinfo);
                    string tags = DNTRequest.GetString("phototag" + (iFile + 1)).Trim();
                    string[] tagsArray = null;
                    if (enabletag && tags != string.Empty)
                    {
                        tagsArray = Utils.SplitString(tags, " ", true, 10);
                        if (tagsArray.Length > 0)
                        {
                            Data.DbProvider.GetInstance().CreatePhotoTags(string.Join(" ", tagsArray), photoid, userid, Utils.GetDateTime());
                            AlbumTags.WritePhotoTagsCacheFile(photoid);
                        }
                    }
                    imgcount++;
                }
                if (imgcount != 0)
                {
                    AlbumInfo albumInfo = DTOProvider.GetAlbumInfo(albumid);
                    albumInfo.Imgcount = Data.DbProvider.GetInstance().GetSpacePhotoCountByAlbumId(albumid);
                    Data.DbProvider.GetInstance().SaveSpaceAlbum(albumInfo);
                }
                else
                {
                    AddErrLine("没有符合要求的图片，请上传jpg,jpeg,gif,png格式的图片");
                    return;
                }

                //生成json数据
                Albums.CreateAlbumJsonData(albumid);                
                Albums.CreatePhotoImageByAlbum(albumid);

                SetUrl(string.Format("usercpspacemanagephoto.aspx?albumid={0}", albumid));
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("照片增加完毕");
            }
        }
    }
}