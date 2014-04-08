using System;
using System.Text;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 聚合类对象Facade类
    /// </summary>
    public class AggregationFacade
    {
        private static AggregationData baseAggregationData;

        private static ForumAggregationData forumAggregationData;

        private static SpaceAggregationData spaceAggregationData;

        private static AlbumAggregationData albumAggregationData;

        private static PhotoAggregationData photoAggregationData;

        private static GoodsAggregationData goodsAggregationData;

        static AggregationFacade()
        {
            baseAggregationData = new AggregationData();
            forumAggregationData = new ForumAggregationData();
            spaceAggregationData = new SpaceAggregationData();
            albumAggregationData = new AlbumAggregationData();
            photoAggregationData = new PhotoAggregationData();
            goodsAggregationData = new GoodsAggregationData();

            //加载要通知的聚合数据对象
            AggregationDataSubject.Attach(baseAggregationData);
            AggregationDataSubject.Attach(forumAggregationData);
            AggregationDataSubject.Attach(spaceAggregationData);
            AggregationDataSubject.Attach(albumAggregationData);
            AggregationDataSubject.Attach(photoAggregationData);
        }

        public static AggregationData BaseAggregation
        {
            get
            {
                return baseAggregationData;
            }
        }

        public static ForumAggregationData ForumAggregation
        {
              get
              {
                  return forumAggregationData;
              }
        }

        public static SpaceAggregationData SpaceAggregation
        {
            get
            {
                return spaceAggregationData;
            }
        }

        public static AlbumAggregationData AlbumAggregation
        {
            get
            {
                return albumAggregationData;
            }
        }

        public static PhotoAggregationData PhotoAggregation
        {
            get
            {
                return photoAggregationData;
            }
        }

        public static GoodsAggregationData GoodsAggregation
        {
            get
            {
                return goodsAggregationData;
            }
        }
    }
}
