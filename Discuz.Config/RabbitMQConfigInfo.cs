using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{

    public class RabbitMQConfigInfo : IConfigInfo
    {
        public HttpModuleErrLogInfo HttpModuleErrLog = new HttpModuleErrLogInfo();

        public SendShortMsgInfo SendShortMsg = new SendShortMsgInfo(); 
    }

    /// <summary>
    /// RabbitMQ基类配置信息类
    /// </summary>
    public class RabbitMQBaseInfo
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable = false;
        /// <summary>
        /// rabbitmq服务地址及端口
        /// </summary>
        public string RabbitMQAddress = "";
        /// <summary>
        /// rabbitmq服务的用户名
        /// </summary>
        public string UserName = "";
        /// <summary>
        /// rabbitmq服务的口令
        /// </summary>
        public string PassWord = "";
        /// <summary>
        /// rabbitmq队列名称
        /// </summary>
        public string QueueName = "";
    }



    /// <summary>
    /// HttpModule错误日志信息类
    /// </summary>
    public class HttpModuleErrLogInfo : RabbitMQBaseInfo
    {
        /// <summary>
        /// mongo服务地址及相关配置信息
        /// </summary>
        public string MongoDB = "";
    }

    /// <summary>
    /// 发送短消息信息类
    /// </summary>
    public class SendShortMsgInfo : RabbitMQBaseInfo
    {
        /// <summary>
        /// sqlserver服务地址及相关配置信息
        /// </summary>
        public string SqlConn = "";
    }
}
