using System;
using System.IO;
using System.Data;
using System.Text;
using System.Xml;

using Discuz.Config;
using Discuz.Common.Xml;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 聚合数据基类
    /// </summary>
    public class  AggregationData
    {        
        /// <summary>
        /// 聚合数据页面路径
        /// </summary>
        private static string filepath = System.Web.HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
        /// <summary>
        /// 图片轮换字符串
        /// </summary>
        private static StringBuilder picRotateData = null;
        /// <summary>
        /// xmldoc对象,用于数据文件的信息操作
        /// </summary>
        protected static XmlDocumentExtender xmlDoc = new XmlDocumentExtender();


        static AggregationData()
		{
            xmlDoc.Load(filepath);
		}


        /// <summary>
        /// 读取聚合页面数据信息
        /// </summary>
        public static void ReadAggregationConfig()
        {
            xmlDoc = new XmlDocumentExtender();
            xmlDoc.Load(filepath);
        }
      
        /// <summary>
        /// 获取聚合页面数据文件路径
        /// </summary>
        public static string DataFilePath
        {
            get
            { 
                return filepath; 
            }
        }
		

        
        /// <summary>
        /// 从XML中检索出指定的轮换广告信息
        /// </summary>
        /// <returns></returns>
        public string GetRotatePicData()
        {
            //当文件未被修改时将直接返回相关记录
            if (picRotateData != null)
            {
                return picRotateData.ToString();
            }

            picRotateData = new StringBuilder();
            picRotateData.Append(this.GetRotatePicStr("Website"));

            return picRotateData.ToString();
        }

        /// <summary>
        /// 从相应的节点下检索轮显数据
        /// </summary>
        /// <param name="nodename">节点名称,如:Website,Spaceindex,Albumindex(区分大小写)</param>
        /// <returns></returns>
        protected string GetRotatePicStr(string nodeName)
        {
            xmlDoc.Load(filepath);
            XmlNodeList xmlnodelist = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_rotatepiclist/" + nodeName + "_rotatepic");

            StringBuilder picRotate = new StringBuilder();
            for (int i = 0; i < xmlnodelist.Count; i++)
            {
                picRotate.Append("data[\"-1_" + (i + 1) + "\"] = \"img: " + xmlDoc.GetSingleNodeValue(xmlnodelist[i], "img").Replace("\"", "\\\"") + "; url: " + xmlDoc.GetSingleNodeValue(xmlnodelist[i], "url").Replace("\"", "\\\"") + "; target: _blank; alt:" + xmlDoc.GetSingleNodeValue(xmlnodelist[i], "titlecontent").Replace("\"", "\\\"") + " ; titlecontent: " + xmlDoc.GetSingleNodeValue(xmlnodelist[i], "titlecontent").Replace("\"", "\\\"") + ";\"\r\n");
            }
            return picRotate.ToString().Trim();
        }


        /// <summary>
        /// 清空数据绑定
        /// </summary>
        public virtual void ClearDataBind()
        {
            picRotateData = null;
        }


        /// <summary>
        /// 清空内存及缓存中所有聚合数据绑定
        /// </summary>
        public void ClearAllDataBind()
        {
            ClearDataBind();
            AggregationFacade.ForumAggregation.ClearDataBind();
            AggregationFacade.AlbumAggregation.ClearDataBind();
            AggregationFacade.PhotoAggregation.ClearDataBind();
            AggregationFacade.SpaceAggregation.ClearDataBind();
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/HotForumList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumNewTopicList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumHotTopicList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/RecentUpdateSpaceAggregationList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/ToppostcountSpaceList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/TopcommentcountSpaceList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/TopvisitedtimesSpaceList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/TopcommentcountPostList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/TopviewsPostList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/SpaceTopNewComments");

            //更新文件的最后修改时间
            FileInfo fi = new FileInfo(Discuz.Common.Utils.GetMapPath(BaseConfigs.GetForumPath + "config/aggregation.config"));
            fi.LastWriteTime = DateTime.Now;

            ReadAggregationConfig();
        }
    }
}
