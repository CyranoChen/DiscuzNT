using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 参与辩论的帖子的辩论相关扩展字段实体
    /// </summary>
    public class DebatePostExpandInfo
    {
        private int tid;        
        private int pid;        
        private int opinion;        
        private int diggs;


        /// <summary>
        /// 主题Id
        /// </summary>
        public int Tid
        {
            get { return tid; }
            set { tid = value; }
        }

        /// <summary>
        /// 帖子Id
        /// </summary>
        public int Pid
        {
            get { return pid; }
            set { pid = value; }
        }

        /// <summary>
        /// 所持观点，1=正方，2=反方
        /// </summary>
        public int Opinion
        {
            get { return opinion; }
            set { opinion = value; }
        }

        /// <summary>
        /// 支持数
        /// </summary>
        public int Diggs
        {
            get { return diggs; }
            set { diggs = value; }
        }


    }
    /// <summary>
    /// 辩论方观点
    /// </summary>
    public enum DebateType
    {
        positivediggs = 1,
        negativediggs

    }
}
