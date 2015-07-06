using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;

namespace Discuz.Web.Services.API
{
    public enum FormatType
    {
        XML,
        JSON
    }

    public enum ApplicationType
    {
        WEB = 1,
        DESKTOP = 2
    }

    public enum ErrorType
    {
        #region System Error

        API_EC_UNKNOWN = 1, //An unknown error occurred. Please resubmit the request. 
        API_EC_SERVICE = 2, //The service is not available at this time.
        API_EC_METHOD = 3, //Unknown method 
        API_EC_TOO_MANY_CALLS = 4, //The application has reached the maximum number of requests allowed. More requests are allowed once the time window has completed. 
        API_EC_PARAM = 100, //One of the parameters specified was missing or invalid.
        API_EC_CALLID = 103, //The submitted call_id was not greater than the previous call_id for this session. 
        //API_EC_HOST_API = 6, //This method must run on  
        #endregion

        #region Authrity Error

        API_EC_BAD_IP = 5, //The request came from a remote address not allowed by this application. 
        API_EC_PERMISSION_DENIED = 10, //Application does not have permission for this action 
        API_EC_APPLICATION = 101, //The API key submitted is not associated with any known application.
        API_EC_SESSIONKEY = 102, //The session key was improperly submitted or has reached its timeout. Direct the user to log in again to obtain another key.
        API_EC_SIGNATURE = 104, //Incorrect signature.
        API_EC_SPAM = 105, //Spam info.

        #endregion

        #region Application Error

        API_EC_REGISTER_NOT_ALLOW = 109,//不允许注册

        API_EC_USER_ALREADY_EXIST = 110,//the username already exist
        API_EC_USER_NONEXIST = 114, // the username doesn't exist // Edit By Cyrano
        API_EC_USERNAME_ILLEGAL = 112,//the username not allow
        API_EC_USER_ONLINE = 113,//this user already online

        API_EC_EMAIL = 111, //email exist or invalid

        API_EC_REWRITENAME = 121, //rewrite name exists

        API_EC_TOPIC_CLOSED = 131, //topic closed
        API_EC_TOPIC_READ_PERM = 132, //阅读权限不足
        API_EC_FORUM_PASSWORD = 133, //need password, cant use api
        API_EC_FORUM_PERM = 134, //没有访问版块权限
        API_EC_REPLY_PERM = 135, //没有回复权限
        API_EC_FRESH_USER = 136, //用户太新，还不能发帖
        API_EC_TITLE_INVALID = 137, //title too long
        API_EC_MESSAGE_LENGTH = 138, //message too long or too short

        #endregion
    }
}
