using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Discuz.Entity;
using System.Text;
using Discuz.Forum;
using Discuz.Plugin.Album;
using Discuz.Common;

namespace Discuz.Web.UI
{
    public class AttachUploadPage : PageBase
    {
        AlbumPluginBase apb = AlbumPluginProvider.GetInstance();
        public int forumid = DNTRequest.GetInt("forumid", 0);
        public DataTable albumlist;
        public bool caninsertalbum = false;

        public AttachUploadPage()
        {
            if (!DNTRequest.GetRawUrl().Contains("action=swfupload") && ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost())) //如果是跨站提交...
                return;

            //处理flash批量上传无法获取userid的问题
            bool canpostattach = true;
            if (DNTRequest.GetString("operation") == "upload")
            {
                string uploadUserid = DNTRequest.GetString("uid");
                int olid = Discuz.Forum.OnlineUsers.GetOlidByUid(TypeConverter.StrToInt(uploadUserid));
                if (olid > 0)
                {
                    OnlineUserInfo oluserinfo = Discuz.Forum.OnlineUsers.GetOnlineUser(olid);
                    string hash = Discuz.Common.DES.Encode(oluserinfo.Olid.ToString() + "," + oluserinfo.Username.ToString(), oluserinfo.Password.Substring(0, 10)).Replace("+", "[");
                    if (DNTRequest.GetString("hash") == hash)
                    {
                        userid = oluserinfo.Userid;
                        usergroupinfo = UserGroups.GetUserGroupInfo(oluserinfo.Groupid);
                    }
                    else
                        canpostattach = false;
                }
                else
                    canpostattach = false;
            }

            UserInfo userinfo = Users.GetUserInfo(userid);
            ForumInfo forum = Forums.GetForumInfo(forumid);
            int MaxTodaySize = (userid > 0 ? Attachments.GetUploadFileSizeByuserid(userid) : 0);
            //今天可上传得大小
            int attachsize = usergroupinfo.Maxsizeperday - MaxTodaySize;
            //得到用户可以上传的文件类型
            string attachmentTypeSelect = Attachments.GetAllowAttachmentType(usergroupinfo, forum);
            string attachextensions = Attachments.GetAttachmentTypeArray(attachmentTypeSelect);
            string attachextensionsnosize = Attachments.GetAttachmentTypeString(attachmentTypeSelect);

            if (DNTRequest.GetString("action") == "swfupload" && DNTRequest.GetString("operation") == "config")
            {
                GetConfig(userid, attachextensionsnosize, attachsize, DNTRequest.GetString("type").Trim() == "image");
            }
            else
            {
                //处理附件
                string msg = "";
                StringBuilder sb = new StringBuilder();
                canpostattach &= UserAuthority.PostAttachAuthority(forum, usergroupinfo, userid, ref msg);                
                if (!canpostattach)
                {
                    ResponseXML(sb.Append("DISCUZUPLOAD|11|0|-1").ToString());//11,上传权限
                    return;
                }
                if (attachsize <= 0)
                {
                    ResponseXML(sb.Append("DISCUZUPLOAD|3|0|-1").ToString());//3,附件大小超限
                    return;
                }

                //得到今天允许用户上传的附件总大小(字节)
                AttachmentInfo[] attachmentinfoarray = ForumUtils.SaveRequestFiles(forumid, config.Maxattachments, usergroupinfo.Maxsizeperday, usergroupinfo.Maxattachsize, MaxTodaySize, 
                    attachextensions, forum.Disablewatermark == 1 ? 0 : config.Watermarkstatus, config, "Filedata", DNTRequest.GetString("type") == "image");
                if (attachmentinfoarray.Length > 0)//已有上传文件
                {
                    int aId = DNTRequest.GetInt("aid", 0);
                    string noUpload = "";

                    foreach (AttachmentInfo attachmentinfo in attachmentinfoarray)
                    {
                        noUpload = string.IsNullOrEmpty(attachmentinfo.Sys_noupload) ? noUpload : attachmentinfo.Sys_noupload;
                        attachmentinfo.Uid = userid;
                    }

                    if (aId <= 0)
                        Attachments.CreateAttachments(attachmentinfoarray);
                    else if (string.IsNullOrEmpty(noUpload))
                    {
                        AttachmentInfo attchmentInfo = Attachments.GetAttachmentInfo(aId);
                        //判断当前用户是否为附件所有者
                        if (attchmentInfo == null || (userinfo.Adminid <= 0 && attchmentInfo.Uid != userid))
                            return;
                        attchmentInfo.Postdatetime = attachmentinfoarray[0].Postdatetime;
                        attchmentInfo.Filename = attachmentinfoarray[0].Filename;
                        attchmentInfo.Description = attachmentinfoarray[0].Description;
                        attchmentInfo.Filetype = attachmentinfoarray[0].Filetype;
                        attchmentInfo.Filesize = attachmentinfoarray[0].Filesize;
                        attchmentInfo.Attachment = attachmentinfoarray[0].Attachment;
                        attchmentInfo.Width = attachmentinfoarray[0].Width;
                        attchmentInfo.Height = attachmentinfoarray[0].Height;
                        attchmentInfo.Isimage = attachmentinfoarray[0].Isimage;
                        Attachments.UpdateAttachment(attchmentInfo);
                    }
                    StringBuilder text = new StringBuilder();
                    int type = attachmentinfoarray[0].Filetype.StartsWith("image") ? 0 : -1;

                    int resultCode = GetNoUploadCode(noUpload);

                    if (aId <= 0)
                        if(DNTRequest.GetString("action") != "swfupload")
                            text.AppendFormat("DISCUZUPLOAD|{0}|{1}|{2}", resultCode, attachmentinfoarray[0].Aid, type);
                        else
                            text.AppendFormat(resultCode != 0 ? "error" : attachmentinfoarray[0].Aid.ToString());
                    else
                        text.AppendFormat("DISCUZUPDATE|{0}|{1}|{2}|{3}", resultCode, attachmentinfoarray[0].Attachment, aId, type);
                    ResponseXML(text.ToString());
                }
            }
        }

        private void GetConfig(int uid, string allowFormats, int maxupload, bool isImage)
        {
            if (isImage)
            {
                if (allowFormats == "")
                    allowFormats = "*.jpg,*.gif,*.png,*.jpeg";
                else
                {
                    string[] tempExt = allowFormats.Split(',');
                    allowFormats = "";
                    foreach (string ext in tempExt)
                    {
                        if (Utils.InArray(ext, "jpg,gif,png,jpeg"))
                        {
                            allowFormats += "*." + ext + ",";
                        }
                    }
                    allowFormats = allowFormats.TrimEnd(',');
                }
            }
            else
            {
                if (allowFormats == "")
                    allowFormats = "*.*";
                else
                    allowFormats = "*." + allowFormats.Replace(",", ",*.");
            }
            string hash = Discuz.Common.DES.Encode(oluserinfo.Olid.ToString() + "," + oluserinfo.Username.ToString(), oluserinfo.Password.Substring(0, 10)).Replace("+", "[");
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<parameter>
<allowsExtend><extend depict=""{0}"">{1}</extend></allowsExtend>
<language>
<okbtn>确定</okbtn>
<ctnbtn>继续</ctnbtn>
<fileName>文件名</fileName>
<size>文件大小</size>
<stat>上传进度</stat>
<browser>浏览</browser>
<delete>删除</delete>
<return>返回</return>
<upload>上传</upload>
<okTitle>上传完成</okTitle>
<okMsg>文件上传完成</okMsg>
<uploadTitle>正在上传</uploadTitle>
<uploadMsg1>总共有</uploadMsg1>
<uploadMsg2>个文件等待上传,正在上传第</uploadMsg2>
<uploadMsg3>个文件</uploadMsg3>
<bigFile>文件过大</bigFile>
<uploaderror>上传失败</uploaderror>
</language>
<config><userid>{2}</userid><hash>{3}</hash><maxupload>{4}</maxupload></config>
</parameter>";
            ResponseXML(string.Format(xml, isImage ? "All Image File" : "All Support Formats", allowFormats, uid, hash, maxupload));
        }

        private int GetNoUploadCode(string message)
        {
            if (message == "")
                return 0;
            if (message == "文件格式无效")
                return 1;
            if (message == "文件大于今天允许上传的字节数" || message == "文件大于该类型附件允许的字节数" || message == "文件大于单个文件允许上传的字节数")
                return 3;
            if (message.IndexOf("图片宽度") > 0 || message.IndexOf("图片高度") > 0)
                return 7;
            return -1;
        }

        private void ResponseXML(string xmlnode)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "text/html";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(xmlnode);
            System.Web.HttpContext.Current.Response.End();
        }
    }

}
