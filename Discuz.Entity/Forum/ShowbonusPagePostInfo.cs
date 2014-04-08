using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// showbonus页面所使用的postinfo实体类
    /// </summary>
    public class ShowbonusPagePostInfo : ShowtopicPagePostInfo
    {
        #region 私有字段
        private int m_bonus;//本帖得分        
        private int m_isbest;//是否是最佳答案，0=不是，1=是有价值的答案，2=最佳答案        
        private int m_bonusextid;//获奖的扩展积分类型
        #endregion

        #region 属性
        /// <summary>
        /// 本帖得分
        /// </summary>
        public int Bonus
        {
            get { return m_bonus; }
            set { m_bonus = value; }
        }
        /// <summary>
        /// 是否是最佳答案，0=不是，1=是有价值的答案，2=最佳答案    
        /// </summary>
        public int Isbest
        {
            get { return m_isbest; }
            set { m_isbest = value; }
        }
        /// <summary>
        /// 获奖的扩展积分类型
        /// </summary>
        public int Bonusextid
        {
            get { return m_bonusextid; }
            set { m_bonusextid = value; }
        }
        #endregion
    }
}
