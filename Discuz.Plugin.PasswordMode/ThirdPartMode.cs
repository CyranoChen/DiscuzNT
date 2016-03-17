using System;

using Discuz.Data;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Plugin;
using Discuz.Forum;

namespace Discuz.Plugin.PasswordMode
{
    /// <summary>
    /// 第三方认识模式
    /// </summary>
    public class ThirdPartMode : IPasswordMode
    {
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="userInfo">通过username获取的用户信息，用于进行密码验证</param>
        /// <param name="postpassword">用户提交的密码</param>
        /// <returns>返回当前用户信息与提交密码的验证结果</returns>
        public bool CheckPassword(UserInfo userInfo, string postpassword)
        {
            if (userInfo == null)
            {
                return false;
            }
            string doubleMd5 = GetEncryptedPassword(userInfo, postpassword);

            return doubleMd5 == userInfo.Password;//比较
        }

        /// <summary>
        /// 创建用户信息(用于用户注册等行为)
        /// </summary>
        /// <param name="userInfo">要创建的用户信息(密码为明文)</param>
        /// <returns></returns>
        public int CreateUserInfo(UserInfo userInfo)
        {
            userInfo.Salt = Forum.ForumUtils.CreateAuthStr(6, false);
            userInfo.Password = GetEncryptedPassword(userInfo, userInfo.Password);
            return Discuz.Data.Users.CreateUser(userInfo);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userInfo">要保存的用户信息(密码为明文)</param>
        /// <returns>返回经过处理之后的实际用户信息</returns>
        public UserInfo SaveUserInfo(UserInfo userInfo)
        {
            if (Utils.StrIsNullOrEmpty(userInfo.Salt))
                userInfo.Salt = Forum.ForumUtils.CreateAuthStr(6, false);

            userInfo.Password = GetEncryptedPassword(userInfo, userInfo.Password);

            return Discuz.Data.Users.UpdateUser(userInfo) ? userInfo : null;
        }

        /// <summary>
        /// 获取加密后的密码字符串
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="unEncryptPassword">未加密的密码字符串</param>
        /// <returns></returns>
        private string GetEncryptedPassword(UserInfo userInfo, string unEncryptPassword)
        {
            return Utils.MD5(Utils.MD5(unEncryptPassword) + userInfo.Salt);//两遍MD5
        }
    }
}
