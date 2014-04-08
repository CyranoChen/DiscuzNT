using System;
using System.Text;
using System.Xml;

using Discuz.Common.Xml;
using Discuz.Entity;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 图片聚合数据类
    /// </summary>
    public class PhotoAggregationData : AggregationData
    {        
        /// <summary>
        /// 图片聚合信息
        /// </summary>
        private static PhotoAggregationInfo __photoAggregationInfo;

        /// <summary>
        /// 清空数据绑定
        /// </summary>
        public override void ClearDataBind()
        {
            __photoAggregationInfo = null;
        }

        /// <summary>
        /// 得到图片聚合信息对象
        /// </summary>
        /// <returns></returns>
        public PhotoAggregationInfo GetPhotoAggregationInfo()
        {
            return (__photoAggregationInfo != null) ? __photoAggregationInfo : GetPhotoAggregationInfoFromFile();
        }

        /// <summary>
        /// 从文件中获得数据并初始化图片聚合对象
        /// </summary>
        /// <returns></returns>
        public PhotoAggregationInfo GetPhotoAggregationInfoFromFile()
        {
            XmlNode xmlnode = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Albumindex/Albumconfig")[0];

            __photoAggregationInfo = new PhotoAggregationInfo();
            if (xmlnode != null)
            {
                __photoAggregationInfo.Focusphotoshowtype = (xmlDoc.GetSingleNodeValue(xmlnode, "Focusphotoshowtype") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Focusphotoshowtype"));
                __photoAggregationInfo.Focusphotodays = (xmlDoc.GetSingleNodeValue(xmlnode, "Focusphotodays") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Focusphotodays"));
                __photoAggregationInfo.Focusphotocount = (xmlDoc.GetSingleNodeValue(xmlnode, "Focusphotocount") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Focusphotocount"));
                __photoAggregationInfo.Focusalbumshowtype = (xmlDoc.GetSingleNodeValue(xmlnode, "Focusalbumshowtype") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Focusalbumshowtype"));
                __photoAggregationInfo.Focusalbumdays = (xmlDoc.GetSingleNodeValue(xmlnode, "Focusalbumdays") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Focusalbumdays"));
                __photoAggregationInfo.Focusalbumcount = (xmlDoc.GetSingleNodeValue(xmlnode, "Focusalbumcount") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Focusalbumcount"));
                __photoAggregationInfo.Weekhot = (xmlDoc.GetSingleNodeValue(xmlnode, "Weekhot") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Weekhot"));
            }

            return __photoAggregationInfo;
        }

        /// <summary>
        /// 保存图片聚合对象信息到聚合数据文件
        /// </summary>
        /// <param name="__photoaggregationinfo"></param>
        public void SaveAggregationData(PhotoAggregationInfo photoAggregationInfo)
        {
            XmlNode photoaggregationsetting = xmlDoc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Albumindex/Albumconfig");
            if (photoaggregationsetting != null)
                photoaggregationsetting.RemoveAll();
            else
                photoaggregationsetting = xmlDoc.CreateNode("/Aggregationinfo/Aggregationdata/Space");

            xmlDoc.AppendChildElementByNameValue(ref photoaggregationsetting, "Focusphotoshowtype", photoAggregationInfo.Focusphotoshowtype);
            xmlDoc.AppendChildElementByNameValue(ref photoaggregationsetting, "Focusphotodays", photoAggregationInfo.Focusphotodays);
            xmlDoc.AppendChildElementByNameValue(ref photoaggregationsetting, "Focusphotocount", photoAggregationInfo.Focusphotocount);
            xmlDoc.AppendChildElementByNameValue(ref photoaggregationsetting, "Focusalbumshowtype", photoAggregationInfo.Focusalbumshowtype);
            xmlDoc.AppendChildElementByNameValue(ref photoaggregationsetting, "Focusalbumdays", photoAggregationInfo.Focusalbumdays);
            xmlDoc.AppendChildElementByNameValue(ref photoaggregationsetting, "Focusalbumcount", photoAggregationInfo.Focusalbumcount);
            xmlDoc.AppendChildElementByNameValue(ref photoaggregationsetting, "Weekhot", photoAggregationInfo.Weekhot);

            xmlDoc.Save(DataFilePath);
        }
      
    }
}
