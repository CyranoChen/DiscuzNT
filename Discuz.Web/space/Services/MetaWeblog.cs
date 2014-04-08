using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.IO;
using CookComputing.XmlRpc;
using System.Web;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Space.Data; 

namespace Discuz.Space.Services
{
    public class MetaWeblog : XmlRpcService, IMetaWeblog
    { 

        #region IMetaWeblog 成员 

        public string newPost(string blogid, string username, string password, Post post, bool publish)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");
            Entity.SpaceConfigInfo spaceconfig = Space.Spaces.GetSpaceConfigByUserId(uid);

            Discuz.Entity.SpacePostInfo spacepostsinfo = new Discuz.Entity.SpacePostInfo();
            spacepostsinfo.Title = Discuz.Forum.ForumUtils.BanWordFilter(post.title);
            spacepostsinfo.Content = Utils.RemoveUnsafeHtml(ForumUtils.BanWordFilter(post.description));
            spacepostsinfo.Category = Space.Spaces.GetCategoryIds(post.categories, uid);
            spacepostsinfo.PostStatus = publish ? 1 : 0;
            spacepostsinfo.CommentStatus = spaceconfig.Commentpref;
            spacepostsinfo.Postdatetime = DateTime.Now;
            spacepostsinfo.Author = username;
            spacepostsinfo.Uid = uid;
            spacepostsinfo.PostUpDateTime = DateTime.Now;
            spacepostsinfo.Commentcount = 0;

            int postid = DbProvider.GetInstance().AddSpacePost(spacepostsinfo);
            if (postid < 1)
                throw new XmlRpcFaultException(0, "发表文章不成功");
            return postid.ToString();
        } 

        public bool editPost(string postid, string username, string password, Post post, bool publish)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");
            Entity.SpacePostInfo spacepostinfo = Discuz.Space.Provider.BlogProvider.GetSpacepostsInfo(DbProvider.GetInstance().GetSpacePost(int.Parse(postid)));

            spacepostinfo.Title = ForumUtils.BanWordFilter(post.title);
            spacepostinfo.Content = ForumUtils.BanWordFilter(post.description);
            spacepostinfo.Category = Space.Spaces.GetCategoryIds(post.categories, uid);
            spacepostinfo.PostUpDateTime = DateTime.Now;

            bool success;
            try
            {
                success = DbProvider.GetInstance().SaveSpacePost(spacepostinfo);
            }
            catch (Exception ex)
            {
                throw new XmlRpcFaultException(0, ex.ToString());
            }
            return success;

        } 

        public Post getPost(string postid, string username, string password)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");
            Entity.SpacePostInfo spacepostinfo = Discuz.Space.Provider.BlogProvider.GetSpacepostsInfo(DbProvider.GetInstance().GetSpacePost(int.Parse(postid)));

            Post post = ConvertPost(spacepostinfo, uid);
            return post;
        } 

        public CategoryInfo[] getCategories(string blogid, string username, string password)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");

            DataTable dt = DbProvider.GetInstance().GetSpaceCategoryListByUserId(uid);
            DataRowCollection drs = dt.Rows;
#if NET1
            ArrayList list = new ArrayList(drs.Count); 
#else
            List<CategoryInfo> list = new List<CategoryInfo>(drs.Count);
#endif
            foreach (DataRow dr in drs)
            {
                CategoryInfo catInfo = new CategoryInfo();
                catInfo.categoryid = dr["categoryid"].ToString();
                catInfo.title = dr["title"].ToString();
                catInfo.description = dr["description"].ToString(); ;
                catInfo.htmlUrl = "";
                catInfo.rssUrl = "";
                list.Add(catInfo);
            }
#if NET1
            return (Post[])list.ToArray(typeof(Post));
#else
            return list.ToArray();
#endif
        } 

        public Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");
            int blogId = int.Parse(blogid);

            DataTable dt = DbProvider.GetInstance().SpacePostsList(numberOfPosts, 1, uid);

            DataRowCollection drs = dt.Rows; 
#if NET1
            ArrayList list = new ArrayList(drs.Count); 
#else
            List<Post> list = new List<Post>(drs.Count);
#endif

            foreach (DataRow dr in drs)
            {
                Post post = new Post();
                post.postid = dr["postid"].ToString();
                post.title = dr["title"].ToString();
                post.description = dr["content"].ToString();
                post.categories = Space.Spaces.GetCategories(dr["category"].ToString(), uid);
                list.Add(post);
            } 
#if NET1
            return (Post[])list.ToArray(typeof(Post));
#else
            return list.ToArray();
#endif
        } 

//实现文件上传

        public MediaObjectUrl newMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");

            string[] currentdate = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
            string fileDatePath = currentdate[0] + "/" + currentdate[1] + "/" + currentdate[2] + "/";
            string sSavePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/upload/" + ((FTPs.GetSpaceAttachInfo.Allowupload == 1 && FTPs.GetSpaceAttachInfo.Reservelocalattach == 0) ? "temp/" : fileDatePath));
            if (!Directory.Exists(sSavePath))
            {
                Utils.CreateDir(sSavePath);
            }

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            string sFilename = (Environment.TickCount & int.MaxValue).ToString() + random.Next(1000, 9999).ToString() + System.IO.Path.GetExtension(mediaObject.name).ToLower();
            //判断sFilename的文件名称是否已存在于服务器上. 如存在, 则添加文件递增标识
            int file_append = 0;
            while (File.Exists(sSavePath + sFilename))
            {
                file_append++;
                sFilename = Path.GetFileNameWithoutExtension(mediaObject.name) + file_append.ToString() + Path.GetExtension(mediaObject.name).ToLower();
            }
            string fileExtName = Path.GetExtension(mediaObject.name).ToLower();
            fileExtName = fileExtName !="" ? fileExtName: ".invalidExtName";
            int groupid = Users.GetShortUserInfo(uid).Groupid;
            string attachextensions = GetAllowedExtensions(groupid);

            if ((attachextensions == null) || (attachextensions.ToLower().IndexOf(fileExtName.Remove(0, 1)) >= 0))
            {
                
                //上传图片文件
                if ((fileExtName == ".jpg") || (fileExtName == ".gif") || (fileExtName == ".png") || (fileExtName == ".jpeg"))
                {
                    try
                    {
                        int maxspacesize = UserGroups.GetUserGroupInfo(groupid).Maxspaceattachsize;
                        int currentspaceattachmentsize = DbProvider.GetInstance().GetSpaceAttachmentSizeByUserid(uid);
                        if ((maxspacesize - currentspaceattachmentsize - mediaObject.bits.Length) <= 0)  //个人空间的存储空间不足
                        {
                            throw new XmlRpcFaultException(101, "存储空间不足, 上传失败!");
                        }
                        else
                        {
                            FileStream newFile = new FileStream(sSavePath + sFilename, FileMode.Create);
                            newFile.Write(mediaObject.bits, 0, mediaObject.bits.Length);
                            newFile.Close();
                        }

                        string filename = "";
                        filename = GetAttachRootPath(fileDatePath);

                        //当支持FTP上传附件时,使用FTP上传远程附件,并在上传完成之后删除本地tempfilename文件
                        if (FTPs.GetSpaceAttachInfo.Allowupload == 1)
                        {
                            FTPs ftps = new FTPs();
                            ftps.UpLoadFile(filename.Replace(FTPs.GetSpaceAttachInfo.Remoteurl, ""), sSavePath + sFilename, FTPs.FTPUploadEnum.SpaceAttach);
                        }


                        filename = filename + sFilename;
                        //保存文件信息
                        SaveSpaceAttachment(mediaObject, uid, filename);

                        string permalink = filename;
                        if (!(FTPs.GetSpaceAttachInfo.Allowupload == 1))
                        {
                            permalink = "http://" + DNTRequest.GetCurrentFullHost() + filename;
                        }

                        MediaObjectUrl mediaObjectUrl = new MediaObjectUrl();
                        mediaObjectUrl.url = permalink;
                        return mediaObjectUrl;
                    }
                    catch 
                    {
                        File.Delete(sSavePath + sFilename);
                        throw new XmlRpcFaultException(102, "上传文件发生异常");
                    }
                }
                else //其它类型文件
                {

                    int maxspacesize = UserGroups.GetUserGroupInfo(groupid).Maxspaceattachsize;
                    int currentspaceattachmentsize = DbProvider.GetInstance().GetSpaceAttachmentSizeByUserid(uid);
                    if ((maxspacesize - currentspaceattachmentsize - mediaObject.bits.Length) <= 0)  //个人空间的存储空间不足
                    {
                        throw new XmlRpcFaultException(101, "存储空间不足, 上传失败!");
                    }
                    else
                    {
                        try
                        {
                            FileStream newFile = new FileStream(sSavePath + sFilename, FileMode.Create);
                            newFile.Write(mediaObject.bits, 0, mediaObject.bits.Length);
                            newFile.Close();

                            string filename = "";

                            filename = GetAttachRootPath(fileDatePath);
                            filename = filename + sFilename;
                            //保存文件信息
                            SaveSpaceAttachment(mediaObject, uid, filename);

                            //当支持FTP上传附件时,使用FTP上传远程附件,并在上传完成之后删除本地tempfilename文件
                            if (FTPs.GetSpaceAttachInfo.Allowupload == 1)
                            {
                                FTPs ftps = new FTPs();
                                ftps.UpLoadFile(GetAttachRootPath(fileDatePath).Replace(FTPs.GetSpaceAttachInfo.Remoteurl, ""), sSavePath + sFilename, FTPs.FTPUploadEnum.SpaceAttach);
                            }

                            string permalink = filename;
                            if (!(FTPs.GetSpaceAttachInfo.Allowupload == 1))
                            {
                                permalink = "http://" + DNTRequest.GetCurrentFullHost() + filename;
                            }
                            MediaObjectUrl mediaObjectUrl = new MediaObjectUrl();
                            mediaObjectUrl.url = permalink;
                            return mediaObjectUrl;
                        }
                        catch
                        {
                            File.Delete(sSavePath + sFilename);
                            throw new XmlRpcFaultException(102, "上传文件发生异常");
                        }
                    }
                }
            }
            return new MediaObjectUrl();
        }

        public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");
            return Discuz.Space.Spaces.DeleteSpacePost(postid, uid);
        } 

        public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
        {
            int uid = ValidateUser(username, password);
            if (uid < 1)
                throw new XmlRpcFaultException(0, "用户不存在");
            Entity.SpaceConfigInfo s = Space.Spaces.GetSpaceConfigByUserId(uid);

            BlogInfo bloginfo = new BlogInfo();
            bloginfo.blogid = s.SpaceID.ToString();
            bloginfo.blogName = s.Spacetitle;
            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();

            bloginfo.url = forumurl + "space/?uid=" + uid;
            return new BlogInfo[] { bloginfo };
        } 

        public UserInfo getUserInfo(string appKey, string username, string password)
        {
            int uid = ValidateUser(username,password);
            if(uid < 1) 
                throw new XmlRpcFaultException(0, "用户不存在");
            Entity.ShortUserInfo u = Forum.Users.GetShortUserInfo(uid);
            UserInfo userInfo = new UserInfo();
            userInfo.nickname = u.Username;
            userInfo.email = u.Email;
            userInfo.userid = uid.ToString(); 

            return userInfo;
        }

        #endregion
        #region Private
        /// <summary>
        /// 转换日志
        /// </summary>
        /// <param name="postinfo"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        private Post ConvertPost(Entity.SpacePostInfo postinfo, int uid)
        {
            Post post = new Post();
            post.postid = postinfo.Postid;
            post.title = postinfo.Title;
            post.description = postinfo.Content;
            post.categories = Space.Spaces.GetCategories(postinfo.Category, uid);
            return post;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private int ValidateUser(string username, string password)
        {
            return Forum.Users.CheckPassword(username, password, true);
        }

        /// <summary>
        /// 获得允许的附件后缀
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        private static string GetAllowedExtensions(int groupid)
        {
            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(groupid);
            StringBuilder sbAttachmentTypeSelect = new StringBuilder();
            if (!usergroupinfo.Attachextensions.Trim().Equals(""))
            {

                sbAttachmentTypeSelect.Append("[id] in (");
                sbAttachmentTypeSelect.Append(usergroupinfo.Attachextensions);
                sbAttachmentTypeSelect.Append(")");
            }

            return Attachments.GetAttachmentTypeArray(sbAttachmentTypeSelect.ToString());
        }

        /// <summary>
        /// 获得附件根路径
        /// </summary>
        /// <returns></returns>
        private static string GetAttachRootPath(string fileDatePath)
        {
            string filename;
            if (FTPs.GetSpaceAttachInfo.Allowupload == 1)
            {
                filename = FTPs.GetSpaceAttachInfo.Remoteurl + "/" + fileDatePath;
            }
            else
            {
                filename = BaseConfigs.GetForumPath + "space/upload/" + fileDatePath;
            }
            return filename;
        }

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <param name="mediaObject"></param>
        /// <param name="uid"></param>
        /// <param name="filename"></param>
        private static void SaveSpaceAttachment(MediaObject mediaObject, int uid, string filename)
        {
            SpaceAttachmentInfo spaceattachmentinfo = new SpaceAttachmentInfo();

            spaceattachmentinfo.UID = uid;
            spaceattachmentinfo.SpacePostID = 0;
            spaceattachmentinfo.PostDateTime = DateTime.Now;
            spaceattachmentinfo.FileName = filename;
            spaceattachmentinfo.FileType = mediaObject.type;
            spaceattachmentinfo.FileSize = mediaObject.bits.Length;
            spaceattachmentinfo.Attachment = mediaObject.name;
            spaceattachmentinfo.Downloads = 0;
            DbProvider.GetInstance().AddSpaceAttachment(spaceattachmentinfo);
        }
        #endregion
    }
} 

