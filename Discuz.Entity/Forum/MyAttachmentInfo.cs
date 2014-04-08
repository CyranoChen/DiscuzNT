using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 我的附件属性
    /// </summary>

    public class MyAttachmentInfo : AttachmentInfo
    {
        private string simplename=string.Empty;
        /// <summary>
        /// 附件简练略名
        /// </summary>
        public string SimpleName
        {
            set { simplename = value; }
            get { return simplename; }
        
        }
     }


}
