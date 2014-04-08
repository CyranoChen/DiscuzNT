using System;

using Discuz.Common.Generic;

namespace Discuz.Config
{
    /// <summary>
    /// 交易配置信息类
    /// </summary>
    [Serializable]
    public class DbSnapInfo 
    {
        /// <summary>
        /// 源ID,用于唯一标识快照在数据库负载均衡中的信息
        /// </summary>
        private int _souceID;
        /// <summary>
        /// 源ID,用于唯一标识快照在数据库负载均衡中的信息
        /// </summary>
        public int SouceID
        {
            get { return _souceID; }
            set { _souceID = value; }
        }

        /// <summary>
        /// 快照是否有效
        /// </summary>
        private bool _enable;    
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 快照链接
        /// </summary>
        private string _dbConnectString;
        /// <summary>
        /// 快照链接
        /// </summary>
        public string DbconnectString
        {
            get { return _dbConnectString; }
            set { _dbConnectString = value; }
        }

        /// <summary>
        /// 权重信息，该值越高则意味着被轮循到的次数越多
        /// </summary>
        private int _weight;
        /// <summary>
        /// 权重信息，该值越高则意味着被轮循到的次数越多
        /// </summary>
        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
    }

    [Serializable]
    public class DbSnapAppConfig : Discuz.Config.IConfigInfo
    {
        private bool _appDbSnap;
        /// <summary>
        /// 是否启用快照，如不使用，则即使DbSnapInfoList已设置有效快照信息也不会使用。
        /// </summary>
        public bool AppDbSnap
        {
            get { return _appDbSnap; }
            set { _appDbSnap = value; }
        }

        private int _writeWaitTime = 6;
        /// <summary>
        /// 写操作等待时间(单位:秒), 说明:在执行完写操作之后，在该时间内的sql请求依旧会被发往master数据库
        /// </summary>
        public int WriteWaitTime
        {
            get { return _writeWaitTime; }
            set { _writeWaitTime = value; }
        }

        private string _loadBalanceScheduling = "WeightedRoundRobinScheduling";
        /// <summary>
        /// 负载均衡调度算法，默认为权重轮询调度算法 http://www.pcjx.com/Cisco/zhong/209068.html
        /// </summary>
        public string LoadBalanceScheduling
        {
            get { return _loadBalanceScheduling; }
            set { _loadBalanceScheduling = value; }
        }

        private bool _recordeLog = false;
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool RecordeLog
        {
            get { return _recordeLog; }
            set { _recordeLog = value; }
        }
        

        private  List<DbSnapInfo> _dbSnapInfoList;
        /// <summary>
        /// 快照轮循列表
        /// </summary>
        public  List<DbSnapInfo> DbSnapInfoList
        {
            get { return _dbSnapInfoList; }
            set { _dbSnapInfoList = value; }
        }


                

    }

    /// <summary>
    ///  负载均衡调度接口
    /// </summary>
    public interface ILoadBalanceScheduling
    {
        /// <summary>
        /// 获取应用当前负载均衡调度算法下的快照链接信息
        /// </summary>
        /// <returns></returns>
        DbSnapInfo GetConnectDbSnap();
    }

   //测试脚本
   //for (int count = 1; count < 25; count++)
   //     {
   //         string current = Discuz.Data.DbHelper.GetRealConnectionString("select");
   //         if (current != null)
   //             Response.Write(current + "请求：" + count + "<br/>");
   //         else
   //             Response.Write("server= null <br/>");
   //     }

}