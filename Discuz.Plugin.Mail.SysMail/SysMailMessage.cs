using System;
using System.Text;

namespace Discuz.Plugin.Mail
{
    
    #region .net邮件发送程
    [SmtpEmail(".net邮件发送程序", Version = "2.0", Author = "Discuz!NT ", DllFileName = "Discuz.Plugin.Mail.SysMail.dll")]
    public class SysMailMessage : ISmtpMail
    {
        private string _subject;
        private string _body;
        private string _from;
        private string _fromName;
        private string _recipientName;
        private string _mailDomain;
        private int _mailserverport;
        private string _username;
        private string _password;
        private bool _html;
        private string _recipient;

        public SysMailMessage()
        { }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject
        {
            get
            {
                return this._subject;
            }
            set
            {
                this._subject = value;
            }
        }

        /// <summary>
        /// 邮件正文
        /// </summary>
        public string Body
        {
            get
            {
                return this._body;
            }
            set
            {
                this._body = value;
            }
        }


        /// <summary>
        /// 发件人地址
        /// </summary>
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                this._from = value;
            }
        }


        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string FromName
        {
            get
            {
                return this._fromName;
            }
            set
            {
                this._fromName = value;
            }
        }


        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string RecipientName
        {
            get
            {
                return this._recipientName;
            }
            set
            {
                this._recipientName = value;
            }
        }

        /// <summary>
        /// 邮箱域
        /// </summary>
        public string MailDomain
        {
            get
            {
                return this._mailDomain;
            }
            set
            {
                this._mailDomain = value;
            }
        }

        /// <summary>
        /// 邮件服务器端口号
        /// </summary>	
        public int MailDomainPort
        {
            set
            {
                this._mailserverport = value;
            }
            get
            {
                return this._mailserverport;
            }
        }


        /// <summary>
        /// SMTP认证时使用的用户名
        /// </summary>
        public string MailServerUserName
        {
            set
            {
                if (value.Trim() != "")
                {
                    this._username = value.Trim();
                }
                else
                {
                    this._username = "";
                }
            }
            get
            {
                return _username;
            }
        }

        /// <summary>
        /// SMTP认证时使用的密码
        /// </summary>
        public string MailServerPassWord
        {
            set
            {
                this._password = value;
            }
            get
            {
                return _password;
            }
        }

        /// <summary>
        ///  是否Html邮件
        /// </summary>
        public bool Html
        {
            get
            {
                return this._html;
            }
            set
            {
                this._html = value;
            }
        }



        //收件人的邮箱地址
        public bool AddRecipient(params string[] username)
        {
            //this._recipient= null;
            this._recipient = username[0].Trim();

            return true;
        }

        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        private string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <returns></returns>
        public bool Send()
        {
#if NET1
            System.Web.Mail.MailMessage myEmail = new System.Web.Mail.MailMessage(); 
            myEmail.BodyEncoding = Encoding.GetEncoding("utf-8");
			myEmail.From = this.From;
			myEmail.To = this._recipient;
			myEmail.Subject = this.Subject;
			myEmail.Body = this.Body;
			myEmail.Priority = System.Web.Mail.MailPriority.Normal; 
			myEmail.BodyFormat = this.Html?System.Web.Mail.MailFormat.Html:System.Web.Mail.MailFormat.Text; //邮件形式，.Text、.Html 

			// 通过SMTP服务器验证
			myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
			myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", this.MailServerUserName);
			myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpaccountname",this.MailServerUserName);
			myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", this.MailServerPassWord);
			myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/postusername",this.RecipientName);
            myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport",this.MailDomainPort);	

            //当不是25端口(gmail:587)
            if(this.MailDomainPort != 25)
            {
                myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
            }
     
			System.Web.Mail.SmtpMail.SmtpServer = this.MailDomain;

			try
			{
				System.Web.Mail.SmtpMail.Send(myEmail);
			}
			catch
			{
			}
			return true;
#else

            System.Net.Mail.MailMessage myEmail = new System.Net.Mail.MailMessage();
            Encoding eEncod = Encoding.GetEncoding("utf-8");
            myEmail.From = new System.Net.Mail.MailAddress(this.From, this.Subject, eEncod);
            myEmail.To.Add(this._recipient);
            myEmail.Subject = this.Subject;
            myEmail.IsBodyHtml = true;
            myEmail.Body =  this.Body;
            myEmail.Priority = System.Net.Mail.MailPriority.Normal;
            myEmail.BodyEncoding = Encoding.GetEncoding("utf-8");
            //myEmail.BodyFormat = this.Html?MailFormat.Html:MailFormat.Text; //邮件形式，.Text、.Html 

           
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = this.MailDomain;
            smtp.Port = this.MailDomainPort;
            smtp.Credentials = new System.Net.NetworkCredential(this.MailServerUserName, this.MailServerPassWord);
            //smtp.UseDefaultCredentials = true;
            //smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            //当不是25端口(gmail:587)
            if (this.MailDomainPort != 25)
            {
                smtp.EnableSsl = true;
            }
            //System.Web.Mail.SmtpMail.SmtpServer = this.MailDomain;

            try
            {
                smtp.Send(myEmail);
            }
            catch (System.Net.Mail.SmtpException e)
            {
                string result = e.Message;
                return false;
            }
          
            return true;
#endif
        }
    }
    #endregion

}
