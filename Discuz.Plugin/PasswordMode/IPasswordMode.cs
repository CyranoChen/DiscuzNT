using System;
using System.Text;

using Discuz.Entity;

namespace Discuz.Plugin.PasswordMode
{
    public interface IPasswordMode
    {
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="userInfo">通过username获取的用户信息，用于进行密码验证</param>
        /// <param name="postpassword">用户提交的密码(明文)</param>
        /// <returns>返回当前用户信息与提交密码的验证结果</returns>
        bool CheckPassword(UserInfo userInfo, string postpassword);
        /// <summary>
        /// 创建用户信息(用于用户注册等行为)
        /// </summary>
        /// <param name="userInfo">要创建的用户信息(密码为明文)</param>
        /// <returns></returns>
        int CreateUserInfo(UserInfo userInfo);
        /// <summary>
        /// 更新用户信息(该方法用于修改用户密码等操作)
        /// </summary>
        /// <param name="userInfo">要修改的用户信息(密码为明文)</param>
        /// <returns></returns>
        UserInfo SaveUserInfo(UserInfo userInfo);
    }
}
