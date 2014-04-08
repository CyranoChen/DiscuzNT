using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Album.Data;

namespace Discuz.Album.Pages
{
    /// <summary>
    /// 编辑图片信息
    /// </summary>
    public class usercpphotoedit : PageBase
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user;
        /// <summary>
        /// 图片Id
        /// </summary>
        public int photoid = DNTRequest.GetInt("photoid", 0);
        /// <summary>
        /// 图片信息
        /// </summary>
        public PhotoInfo photo;
        /// <summary>
        /// 评论设置
        /// </summary>
        public int commentstatus = DNTRequest.GetInt("commentstatus", 2);
        /// <summary>
        /// 标签设置(预留)
        /// </summary>
        public int tagstatus;
        public string tags;

        protected override void ShowPage()
        {
            pagetitle = "编辑图片信息";

            #region 验证
            if (userid == -1)
            {
                AddErrLine("请先登录");
                return;
            }
            user = Users.GetUserInfo(userid);
            if (config.Enablealbum != 1)
            {
                AddErrLine("相册功能已被关闭");
                return;
            }
            if (photoid < 1)
            {
                AddErrLine("不存在的图片Id");
                return;
            }
            photo = DTOProvider.GetPhotoInfo(photoid, 0, 0);
            if (photo == null)
            {
                AddErrLine("图片不存在");
                return;
            }
            if (photo.Userid != userid)
            {
                AddErrLine("您没有编辑此图片的权限");
                return;
            }
            #endregion

            if (!DNTRequest.IsPost())
            {
                photo.Filename = Globals.GetThumbnailImage(photo.Filename);
                commentstatus = (int) photo.Commentstatus;
                tagstatus = (int) photo.Tagstatus;
                tags = AlbumTags.GetTagsByPhotoId(photoid);
            }
            else
            {
                photo.Title = DNTRequest.GetString("title");
                photo.Description = DNTRequest.GetString("description");

                if (commentstatus < 0 || commentstatus > 2)
                    commentstatus = 2;

                photo.Commentstatus = (PhotoStatus) commentstatus;

                string newtags = DNTRequest.GetString("phototag").Trim();
                string[] tagsArray = null;
                if (config.Enabletag == 1 && newtags != string.Empty && newtags != tags )
                {
                    tagsArray = Utils.SplitString(newtags, " ", true, 10);
                    if (tagsArray.Length > 0 && string.Join(" ", tagsArray) != tags)
                    {
                        DbProvider.GetInstance().DeletePhotoTags(photoid);
                        DbProvider.GetInstance().CreatePhotoTags(string.Join(" ", tagsArray), photoid, userid, Utils.GetDateTime());
                        AlbumTags.WritePhotoTagsCacheFile(photoid);
                    }
                }

                DTOProvider.UpdatePhotoInfo(photo);
                
                //生成json数据
                Albums.CreateAlbumJsonData(photo.Albumid);
                //生成图片标题
                Albums.CreatePhotoTitleImage(photo.Photoid, photo.Title);
                //生成用户名图片
                Albums.CreateUserImage(photo.Userid, photo.Username);

                AddMsgLine("图片信息修改成功, 将返回图片列表");
                SetUrl("usercpspacemanagephoto.aspx?albumid=" + photo.Albumid);
                SetMetaRefresh();
            }
        }
    }
}