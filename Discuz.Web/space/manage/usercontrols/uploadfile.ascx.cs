using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Plugin.Space;
using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Config;
using Discuz.Entity;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Discuz.Plugin.Album;

namespace Discuz.Space.Manage
{
    /// <summary>
    ///	上传文件控件
    /// </summary>
    public class uploadfile : DiscuzSpaceUCBase
    {
        #region 控件声明

        protected Discuz.Control.TextBox UpfileList;
        protected System.Web.UI.WebControls.Button startup;
        protected HtmlInputFile filefield1;
        protected Discuz.Control.DropDownList albums;

        #endregion

        public string httplink = "";
        public int freePhotoSize = 0;
        public bool allowScript = false;
        public string attachextensions = null;

        private void Page_Load(object sender, EventArgs e)
        {
            //当用户在线信息不正确时，则重新返回登陆页
            if (userid <= 0 || Utils.StrToInt(ForumUtils.GetCookie("userid"), -1) != userid)
            {
                Context.Response.Redirect("../../login.aspx");
                return;
            }
        
            //已登录
            ShortUserInfo _user = Users.GetShortUserInfo(userid);
            if (_user == null || _user.Spaceid <= 0) //用户还未开通个人空间
            {
                Context.Response.Write("<script type='text/javascript'>alert('您还未开通" + config.Spacename + "！');window.location='../../';</script>");
                Context.Response.End();
                return;
            }
                    
            string[] currentdate = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
            if (config.Forumurl == "")
            {
                Response.Write("请正确配置论坛URL地址");
                Response.End();
                return;
            }

            string uploaddir = "";
            string fileDatePath = currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/";

            //当支持FTP上传附件
            if (FTPs.GetSpaceAttachInfo.Allowupload == 1)
            {
                //不保留本地附件模式时
                if (FTPs.GetSpaceAttachInfo.Reservelocalattach == 0)
                    uploaddir = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/upload/temp/");
                else
                    uploaddir = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/upload/" + fileDatePath);

                httplink = FTPs.GetSpaceAttachInfo.Remoteurl + "/" + fileDatePath;
                ViewState["RelativeFilePath"] = FTPs.GetSpaceAttachInfo.Remoteurl + "/" + fileDatePath;
            }
            else
            {
                httplink = BaseConfigs.GetForumPath + "space/upload/" + fileDatePath;
                uploaddir = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/upload/" + fileDatePath);
                ViewState["RelativeFilePath"] = BaseConfigs.GetForumPath + "space/upload/" + fileDatePath;
             }

            if (!Directory.Exists(uploaddir))
                Utils.CreateDir(uploaddir);

            ViewState["UploadDir"] = uploaddir;
            ViewState["postid"] = DNTRequest.GetInt("postid", 0);

            //载入相册列表
            if (this.spaceconfiginfo.Status == SpaceStatusType.Natural)
            {
                UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(userid);
                //得到用户可以上传的文件类型
                StringBuilder sbAttachmentTypeSelect = new StringBuilder();
                if (!usergroupinfo.Attachextensions.Trim().Equals(""))
                {
                    sbAttachmentTypeSelect.Append("[id] in (");
                    sbAttachmentTypeSelect.Append(usergroupinfo.Attachextensions);
                    sbAttachmentTypeSelect.Append(")");
                }
                attachextensions = Attachments.GetAttachmentTypeArray(sbAttachmentTypeSelect.ToString());
            }
            else
                albums.Visible = false;
        }


        private string StartUploadFile()
        {
            string sSavePath = "";

            if (ViewState["UploadDir"] != null)
                sSavePath = ViewState["UploadDir"].ToString();
            else
                sSavePath = Server.MapPath(BaseConfigs.GetForumPath + "space/upload/");

            if (filefield1.PostedFile != null)
            {
                HttpPostedFile myFile = filefield1.PostedFile;
                int nFileLen = myFile.ContentLength;
                if (nFileLen == 0)
                    return "";

                byte[] myData = new Byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                string sFilename = (Environment.TickCount & int.MaxValue).ToString() + random.Next(1000, 9999).ToString() + System.IO.Path.GetExtension(myFile.FileName).ToLower();

                //判断sFilename的文件名称是否已存在于服务器上. 如存在, 则添加文件递增标识
                int file_append = 0;
                while (File.Exists(sSavePath + sFilename))
                {
                    file_append++;
                    sFilename = Path.GetFileNameWithoutExtension(myFile.FileName) + file_append.ToString() + Path.GetExtension(myFile.FileName).ToLower();
                }

                string fileExtName = Path.GetExtension(myFile.FileName).ToLower();
                string relativeFilePath = ViewState["RelativeFilePath"].ToString().Trim();

                fileExtName = fileExtName !="" ? fileExtName: ".invalidExtName";

                if ((attachextensions == null) || (attachextensions.ToLower().IndexOf(fileExtName.Remove(0,1)) >= 0))
                {
                    //上传图片文件
                    if ((fileExtName == ".jpg") || (fileExtName == ".gif") || (fileExtName == ".png") || (fileExtName == ".jpeg"))
                    { 
                        try
                        {
                            AlbumPluginBase apb = AlbumPluginProvider.GetInstance();                        
                            //上传附件同时加入相册
                            if (albums.SelectedValue != "" && apb != null)
                            {
                                int maxphotosize = UserGroups.GetUserGroupInfo(_userinfo.Groupid).Maxspacephotosize;
                                int currentphotisize = apb.GetPhotoSizeByUserid(userid);
                                if ((maxphotosize - currentphotisize - nFileLen) <= 0)  //相册的存储空间不足
                                {
                                    HttpContext.Current.Response.Write("<script>alert('" + config.Albumname + "空间不足, 上传至相册失败!');</script>");
                                    HttpContext.Current.Response.End();
                                    return "";
                                }
                                else
                                {
                                    FileStream newFile = new FileStream(sSavePath + sFilename, FileMode.Create);
                                    newFile.Write(myData, 0, myData.Length);
                                    newFile.Close();

                                    string extension = Path.GetExtension(sSavePath + sFilename);
                                    Common.Thumbnail.MakeThumbnailImage(sSavePath + sFilename, (sSavePath + sFilename).Replace(extension, "_thumbnail" + extension), 150, 150);
                                    Common.Thumbnail.MakeSquareImage(sSavePath + sFilename, (sSavePath + sFilename).Replace(extension, "_square" + extension), 100);
                                    string sPath = relativeFilePath;
                                    if (sPath.StartsWith("/"))
                                        sPath = sPath.Substring(1, sPath.Length - 1);

                                    PhotoInfo photoinfo = new PhotoInfo();
                                    photoinfo.Filename = sPath + sFilename;
                                    photoinfo.Attachment = Path.GetFileName(filefield1.PostedFile.FileName);
                                    photoinfo.Filesize = nFileLen;
                                    photoinfo.Title = sFilename.Remove(sFilename.IndexOf("."), 1);
                                    photoinfo.Description = "";
                                    photoinfo.Albumid = Utils.StrToInt(albums.SelectedValue, 0);
                                    photoinfo.Userid = userid;
                                    photoinfo.Username = username;
                                    photoinfo.Views = 0;
                                    photoinfo.Commentstatus = 0;
                                    photoinfo.Tagstatus = 0;
                                    photoinfo.Comments = 0;
                                    photoinfo.IsAttachment = 1;
                                    Space.Data.DbProvider.GetInstance().AddSpacePhoto(photoinfo);
                                    AlbumInfo albumInfo = apb.GetAlbumInfo( Utils.StrToInt((albums.SelectedValue),0));
                                    albumInfo.Imgcount = Space.Data.DbProvider.GetInstance().GetSpacePhotoCountByAlbumId(Utils.StrToInt(albums.SelectedValue, 0));
                                    Space.Data.DbProvider.GetInstance().SaveSpaceAlbum(albumInfo);

                                    //当支持FTP上传附件时,使用FTP上传远程附件,并在上传完成之后删除本地tempfilename文件
                                    if (FTPs.GetSpaceAttachInfo.Allowupload == 1)
                                    {
                                        FTPs ftps = new FTPs();
                                        relativeFilePath = relativeFilePath.Replace(FTPs.GetSpaceAttachInfo.Remoteurl, "");
                                        ftps.UpLoadFile(relativeFilePath, sSavePath + sFilename, FTPs.FTPUploadEnum.SpaceAttach);
                                        ftps = new FTPs();
                                        ftps.UpLoadFile(relativeFilePath, (sSavePath + sFilename).Replace(extension, "_thumbnail" + extension), FTPs.FTPUploadEnum.SpaceAttach);
                                        ftps = new FTPs();
                                        ftps.UpLoadFile(relativeFilePath, (sSavePath + sFilename).Replace(extension, "_square" + extension), FTPs.FTPUploadEnum.SpaceAttach);
                                    }
                                }
                            }
                            else
                            {
                                int maxspacesize = UserGroups.GetUserGroupInfo(_userinfo.Groupid).Maxspaceattachsize;
                                int currentspaceattachmentsize = Space.Data.DbProvider.GetInstance().GetSpaceAttachmentSizeByUserid(userid);
                                if ((maxspacesize - currentspaceattachmentsize - nFileLen) <= 0)  //个人空间的存储空间不足
                                {
                                    HttpContext.Current.Response.Write("<script>alert('" + config.Spacename + "存储空间不足, 上传失败!');</script>");
                                    HttpContext.Current.Response.End();
                                    return "";
                                }
                                else
                                {
                                    FileStream newFile = new FileStream(sSavePath + sFilename, FileMode.Create);
                                    newFile.Write(myData, 0, myData.Length);
                                    newFile.Close();
                                }

                                //当支持FTP上传附件时,使用FTP上传远程附件,并在上传完成之后删除本地tempfilename文件
                                if (FTPs.GetSpaceAttachInfo.Allowupload == 1)
                                {
                                    FTPs ftps = new FTPs();
                                    ftps.UpLoadFile(relativeFilePath.Replace(FTPs.GetSpaceAttachInfo.Remoteurl, ""), sSavePath + sFilename, FTPs.FTPUploadEnum.SpaceAttach);
                                }     
                            }

                            InsertSapceAttachment(relativeFilePath + sFilename, myFile.ContentType, myData.Length, Path.GetFileName(myFile.FileName).ToLower());

                            return sFilename;
                        }
                        catch (ArgumentException errArgument)
                        {
                            File.Delete(sSavePath + sFilename);
                            HttpContext.Current.Response.Write("<script>alert('" + errArgument.Message + "!');</script>");
                            HttpContext.Current.Response.End();
                            return "";
                        }
                    }
                    else //其它类型文件
                    {  
                        int maxspacesize = UserGroups.GetUserGroupInfo(_userinfo.Groupid).Maxspaceattachsize;
                        int currentspaceattachmentsize = Space.Data.DbProvider.GetInstance().GetSpaceAttachmentSizeByUserid(userid);
                        if ((maxspacesize - currentspaceattachmentsize - nFileLen) <= 0)  //个人空间的存储空间不足
                        {
                            HttpContext.Current.Response.Write("<script>alert('" + config.Spacename + "存储空间不足, 上传失败!');</script>");
                            HttpContext.Current.Response.End();
                            return "";
                        }
                        else
                        {
                            try
                            {
                                myFile.SaveAs(sSavePath + sFilename);
                                InsertSapceAttachment(relativeFilePath + sFilename, myFile.ContentType, myData.Length, Path.GetFileName(myFile.FileName).ToLower());

                                //当支持FTP上传附件时,使用FTP上传远程附件,并在上传完成之后删除本地tempfilename文件
                                if (FTPs.GetSpaceAttachInfo.Allowupload == 1)
                                {
                                    FTPs ftps = new FTPs();
                                    ftps.UpLoadFile(relativeFilePath.Replace(FTPs.GetSpaceAttachInfo.Remoteurl, ""), sSavePath + sFilename, FTPs.FTPUploadEnum.SpaceAttach);
                                } 
                                return sFilename;
                            }
                            catch (ArgumentException errArgument)
                            {
                                File.Delete(sSavePath + sFilename);
                                HttpContext.Current.Response.Write("<script>alert('" + errArgument.Message + "!');</script>");
                                HttpContext.Current.Response.End();
                                return "";
                            }
                        }
                    }
                }
                else //当上传的附件类型无效时
                {
                    return "invalid_file";
                }
            }
            return "";
        }


        private void InsertSapceAttachment(string filename, string filetype, int filesize, string attachment)
        {
            SpaceAttachmentInfo spaceattachmentinfo = new SpaceAttachmentInfo();
            spaceattachmentinfo.UID = userid;
            spaceattachmentinfo.SpacePostID = Convert.ToInt32(ViewState["postid"].ToString());
            spaceattachmentinfo.PostDateTime = DateTime.Now;
            spaceattachmentinfo.FileName = filename;
            spaceattachmentinfo.FileType = filetype;
            spaceattachmentinfo.FileSize = filesize;
            spaceattachmentinfo.Attachment = attachment;
            spaceattachmentinfo.Downloads = 0;
            Space.Data.DbProvider.GetInstance().AddSpaceAttachment(spaceattachmentinfo);
        }

        private void startup_Click(object sender, EventArgs e)
        {
            string filename = StartUploadFile();

            if (filename != "")
            {
                if (filename != "invalid_file")
                {
                    if (UpfileList.Text == "")
                        UpfileList.Text = filename;
                    else
                        UpfileList.Text += "|" + filename;

                    if (DNTRequest.GetString("upload") == "attachment")
                        RegisterStartupScript("Form1", "<script>parentreload();</script>");
                    else
                        RegisterStartupScript("Form1", "<script>loadfilelist('" + httplink + "');</script>");
                }
                else 
                    RegisterStartupScript("Form1", "<script>alert('上传文件的格式无效!');</script>");
            }
            else
                RegisterStartupScript("Form1", "<script>noselectfile();</script>");
        }

        public void RegisterStartupScript(string key, string scriptstr)
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), key, scriptstr);
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.startup.Click += new EventHandler(this.startup_Click);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion
    }
}