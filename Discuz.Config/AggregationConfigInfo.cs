using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 聚合数据提取设置信息类
    /// </summary>
    [Serializable]
    public class AggregationConfigInfo : IConfigInfo
    {
        #region 私有字段

        private int m_spacetopnewcommentscount = 10; //最新空间评论条数
        private int m_spacetopnewcommentstimeout = 20;  //最新空间评论条数超时分钟数

        private int m_topcommentcountpostlistcount = 10;  //最多评论日志数
        private int m_topcommentcountpostlisttimeout = 20;   //最多评论日志数超时分钟数

        private int m_topviewspostlistcount = 10; //最多访问日志条数
        private int m_topviewspostlisttimeout = 20;  //最多访问日志条数超时分钟数

        private int m_topcommentcountspacelistcount = 10; //最多评论空间条数
        private int m_topcommentcountspacelisttimeout = 20;  //最多评论空间条数超时分钟数

        private int m_toppostcountspacelistcount = 10;    //最多发帖空间数据条数
        private int m_toppostcountspacelisttimeout = 20; //最多发帖空间数据条数超时分钟数

        private int m_topvisitedtimesspacelistcount = 10;   //最多访问空间数据
        private int m_topvisitedtimesspacelisttimeout = 20;    //最多访问空间数据超时分钟数

        private int m_recentupdatespaceaggregationlistcount = 10;   //最近更新的空间数据条数
        private int m_recentupdatespaceaggregationlisttimeout = 20; //最近更新的空间数据条数超时分钟数

        #endregion

        #region 属性

        /// <summary>
        /// 最新空间评论条数
        /// </summary>
        public int SpaceTopNewCommentsCount
        {
            get { return m_spacetopnewcommentscount; }
            set { m_spacetopnewcommentscount = value; }
        }
        /// <summary>
        /// 最新空间评论条数超时分钟数
        /// </summary>
        public int SpaceTopNewCommentsTimeout
        {
            get { return m_spacetopnewcommentstimeout; }
            set { m_spacetopnewcommentstimeout = value; }
        }


        /// <summary>
        /// 最多评论日志数
        /// </summary>
        public int TopcommentcountPostListCount
        {
            get { return m_topcommentcountpostlistcount; }
            set { m_topcommentcountpostlistcount = value; }
        }
        /// <summary>
        /// 最多评论日志数超时分钟数
        /// </summary>
        public int TopcommentcountPostListTimeout
        {
            get { return m_topcommentcountpostlisttimeout; }
            set { m_topcommentcountpostlisttimeout = value; }
        }

        
        /// <summary>
        /// 最多访问日志条数
        /// </summary>
        public int TopviewsPostListCount
        {
            get { return m_topviewspostlistcount; }
            set { m_topviewspostlistcount = value; }
        }
        /// <summary>
        /// 最多访问日志条数超时分钟数
        /// </summary>
        public int TopviewsPostListTimeout
        {
            get { return m_topviewspostlisttimeout; }
            set { m_topviewspostlisttimeout = value; }
        }

       
        /// <summary>
        /// 最多评论空间条数
        /// </summary>
        public int TopcommentcountSpaceListCount
        {
            get { return m_topcommentcountspacelistcount; }
            set { m_topcommentcountspacelistcount = value; }
        }
        /// <summary>
        /// 最多评论空间条数超时分钟数
        /// </summary>
        public int TopcommentcountSpaceListTimeout
        {
            get { return m_topcommentcountspacelisttimeout; }
            set { m_topcommentcountspacelisttimeout = value; }
        }

     
        /// <summary>
        /// 最多访问空间数据
        /// </summary>
        public int TopvisitedtimesSpaceListCount
        {
            get { return m_topvisitedtimesspacelistcount; }
            set { m_topvisitedtimesspacelistcount = value; }
        }
        /// <summary>
        /// 最多访问空间数据超时分钟数
        /// </summary>
        public int TopvisitedtimesSpaceListTimeout
        {
            get { return m_topvisitedtimesspacelisttimeout; }
            set { m_topvisitedtimesspacelisttimeout = value; }
        }

     
        /// <summary>
        /// 最多发帖空间数据条数
        /// </summary>
        public int ToppostcountSpaceListCount
        {
            get { return m_toppostcountspacelistcount; }
            set { m_toppostcountspacelistcount = value; }
        }
        /// <summary>
        /// 最多发帖空间数据条数超时分钟数
        /// </summary>
        public int ToppostcountSpaceListTimeout
        {
            get { return m_toppostcountspacelisttimeout; }
            set { m_toppostcountspacelisttimeout = value; }
        }

      
        /// <summary>
        /// 最近更新的空间数据条数
        /// </summary>
        public int RecentUpdateSpaceAggregationListCount
        {
            get { return m_recentupdatespaceaggregationlistcount; }
            set { m_recentupdatespaceaggregationlistcount = value; }
        }
        /// <summary>
        /// 最近更新的空间数据条数超时分钟数
        /// </summary>
        public int RecentUpdateSpaceAggregationListTimeout
        {
            get { return m_recentupdatespaceaggregationlisttimeout; }
            set { m_recentupdatespaceaggregationlisttimeout = value; }
        }

        #endregion
    }
}
