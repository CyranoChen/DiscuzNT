using System;
using System.IO;

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Forum
{
    public enum AvatarSize { Large, Medium, Small }
    public class Avatars
    {
        const string AVATAR_URL = "upload/{0}/{1}/{2}/{3}_avatar_{4}.jpg";
        private static string forumPath = BaseConfigs.GetForumPath;
        /// <summary>
        /// 获取头像地址
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="avatarSize">头像大小，1：大，2：中，3：小</param>
        /// <returns></returns>
        public static string GetAvatarUrl(string uid, AvatarSize avatarSize)
        {
            uid = FormatUid(uid);
            string size = "";
            switch (avatarSize)
            {
                case AvatarSize.Large:
                    size = "large";
                    break;
                case AvatarSize.Medium:
                    size = "medium";
                    break;
                case AvatarSize.Small:
                    size = "small";
                    break;
            }
            string physicsAvatarPath = string.Format(AVATAR_URL, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), size);
            //当支持FTP上传头像时,使用FTP上传远程头像
            if (FTPs.GetForumAvatarInfo.Allowupload == 1)
                return FTPs.GetForumAvatarInfo.Remoteurl.Trim('/') + "/avatars/" + physicsAvatarPath;         
            else
                return Utils.GetRootUrl(forumPath) + "avatars/" + physicsAvatarPath;          
        }

        /// <summary>
        /// 获取头像url
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string GetAvatarUrl(string uid)
        {
            return GetAvatarUrl(uid, AvatarSize.Medium);
        }

        /// <summary>
        /// 获取默认头像
        /// </summary>
        /// <param name="avatarSize"></param>
        /// <returns></returns>
        public static string GetDefaultAvatarUrl(AvatarSize avatarSize)
        {
            return Utils.GetRootUrl(forumPath) + "images/common/noavatar_" + avatarSize.ToString().ToLower() + ".gif";
        }

        /// <summary>
        /// 格式化Uid为9位标准格式
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string FormatUid(string uid)
        {
            return uid.PadLeft(9, '0');
        }

        /// <summary>
        /// 是否存在上传头像
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool ExistAvatar(string uid)
        {
            uid = FormatUid(uid);

            return File.Exists(GetPhysicsAvatarPath(uid, AvatarSize.Large)) && 
                   File.Exists(GetPhysicsAvatarPath(uid, AvatarSize.Medium)) && 
                   File.Exists(GetPhysicsAvatarPath(uid, AvatarSize.Small));
        }

        /// <summary>
        /// 获取头像物理路径
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetPhysicsAvatarPath(string uid, AvatarSize size)
        {
            return Utils.GetMapPath(forumPath + "avatars/" +
                string.Format(AVATAR_URL, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), size.ToString().ToLower()));
        }

        /// <summary>
        /// 删除头像
        /// </summary>
        /// <param name="uid"></param>
        public static void DeleteAvatar(string uid)
        {
            uid = FormatUid(uid);
            if (File.Exists(Avatars.GetPhysicsAvatarPath(uid, AvatarSize.Large)))
            {
                File.Delete(Avatars.GetPhysicsAvatarPath(uid, AvatarSize.Large));
                File.Delete(Avatars.GetPhysicsAvatarPath(uid, AvatarSize.Medium));
                File.Delete(Avatars.GetPhysicsAvatarPath(uid, AvatarSize.Small));
            }
        }

        /// <summary>
        /// 获取头像地址
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatarSize">图片尺寸</param>
        /// <returns>图片地址</returns>
        public static string GetAvatarUrl(int uid, AvatarSize avatarSize)
        {
            string size = "";
            switch (avatarSize)
            {
                case AvatarSize.Large:
                    size = "large";
                    break;
                case AvatarSize.Medium:
                    size = "medium";
                    break;
                case AvatarSize.Small:
                    size = "small";
                    break;
            }
            if (GeneralConfigs.GetConfig().AvatarMethod == 0)
                return string.Format("{0}tools/avatar.aspx?uid={1}&size={2}", Utils.GetRootUrl(Discuz.Config.BaseConfigs.GetForumPath), uid, size);
            else  
                return Avatars.GetAvatarUrl(uid.ToString(), avatarSize); 
        }

        /// <summary>
        /// 获取中等头像地址
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>图片地址</returns>
        public static string GetAvatarUrl(int uid)
        {
            return GetAvatarUrl(uid, AvatarSize.Medium);
        }
    }
}
