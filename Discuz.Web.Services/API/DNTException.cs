using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;

namespace Discuz.Web.Services.API
{
    public class DNTException : Exception
    {
        private int error_code;
        private string error_message;

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode
        {
            get { return error_code; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get { return error_message; }
        }

        public DNTException(int error_code, string error_message)
            : base(CreateMessage(error_code, error_message))
        {
            this.error_code = error_code;
            this.error_message = error_message;
        }

        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="error_code"></param>
        /// <param name="error_message"></param>
        /// <returns></returns>
        private static string CreateMessage(int error_code, string error_message)
        {
            return string.Format("Code: {0}, Message: {1}", error_code, error_message);
        }
    }
}
