using System;
using System.IO;
using System.Collections;
using System.Text;

using Discuz.Common;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 聚合数据主体类
    /// </summary>
    public class AggregationDataSubject
    {
        /// <summary>
        /// 程序刚加载时aggregation.config文件修改时间
        /// </summary>
        public static DateTime fileoldchange;
        /// <summary>
        /// 最新aggregation.config文件修改时间
        /// </summary>
        public static DateTime filenewchange;
        /// <summary>
        /// 设置定时器时间为15秒
        /// </summary>
        private static System.Timers.Timer aggregationConfigTimer = new System.Timers.Timer(15000);

        private static object lockHelper = new object();

        static AggregationDataSubject()
        {
            if (!Utils.FileExists(AggregationData.DataFilePath))
            {
                string content = "<?xml version=\"1.0\" standalone=\"yes\"?>\r\n";
                content += "<remove>\r\n<table1 xpath=\"example\" removedatetime=\""+DateTime.Now.ToShortDateString()+"\" />\r\n</remove>";

                using (FileStream fs = new FileStream(AggregationData.DataFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes(content);
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
            }
            fileoldchange = System.IO.File.GetLastWriteTime(AggregationData.DataFilePath);

            //初始化定时器
            aggregationConfigTimer.AutoReset = true;
            aggregationConfigTimer.Enabled = true;
            aggregationConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            aggregationConfigTimer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsFileHadRewrite())
            {
                ReSetFileChangeTime();

                AggregationData.ReadAggregationConfig();

                NotifyClearDataBind();
            }
        }

        /// <summary>
        /// 聚合数据文件是否已被重写
        /// </summary>
        /// <returns></returns>
        public static bool IsFileHadRewrite()
        {
            //当程序运行中aggregation.config发生变化时
            filenewchange = System.IO.File.GetLastWriteTime(AggregationData.DataFilePath);
            if (fileoldchange != filenewchange)
            {
                lock (lockHelper)
                {
                    if (fileoldchange != filenewchange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 重新设置文件加载时间
        /// </summary>
        private static void ReSetFileChangeTime()
        {
            fileoldchange = filenewchange;
        }


        #region 采用Observer模式清空当前进程中的聚合数据

        private static ArrayList __aggregationDataArrayList = new ArrayList();


        /// <summary>
        /// 附加对象 (注:调用下面函数的操作在AggregationFacade类的静态构造函数)
        /// </summary>
        /// <param name="__aggregationData"></param>
        public static void Attach(AggregationData __aggregationData)
        {
            __aggregationDataArrayList.Add(__aggregationData);
        }

        public static void Detach(AggregationData __aggregationData)
        {
            __aggregationDataArrayList.Remove(__aggregationData);
        }

        public static void NotifyClearDataBind()
        {
            foreach (AggregationData __aggregationData in __aggregationDataArrayList)
            {
                __aggregationData.ClearDataBind();
            }
        }

        #endregion
    }
}
