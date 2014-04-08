using System;
using System.Text;

namespace Discuz.Plugin.Mail
{

    #region 邮件收发代码接口

    public interface ISmtpMail
    {
        /// <summary>
        /// 端口
        /// </summary>
        int MailDomainPort { set;}

        /// <summary>
        /// 发件人地址
        /// </summary>
        string From { set;get;}

        /// <summary>
        /// 发件人姓名
        /// </summary>
        string FromName { set;get;}

        /// <summary>
        /// 是否支持Html
        /// </summary>
        bool Html { set;get;}

        /// <summary>
        /// 邮件标题
        /// </summary>
        string Subject { set;get;}

        /// <summary>
        /// 邮件内容
        /// </summary>
        string Body { set;get;}

        /// <summary>
        ///  邮件服务器地址
        /// </summary>
        string MailDomain { set;}

        /// <summary>
        /// 用户名
        /// </summary>
        string MailServerUserName { set;}

        /// <summary>
        /// 口令
        /// </summary>
        string MailServerPassWord { set;}

        /// <summary>
        /// 收件人姓名
        /// </summary>
        string RecipientName { set;get;}

        /// <summary>
        /// 收件人列表
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool AddRecipient(params string[] username);

        /// <summary>
        /// 发送
        /// </summary>
        /// <returns></returns>
        bool Send();

    }

    #endregion

}
