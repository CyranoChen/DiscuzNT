using System;

namespace Discuz.Entity
{
    /// <summary>
    /// �ҵĸ�������
    /// </summary>

    public class MyAttachmentInfo : AttachmentInfo
    {
        private string simplename=string.Empty;
        /// <summary>
        /// ������������
        /// </summary>
        public string SimpleName
        {
            set { simplename = value; }
            get { return simplename; }
        
        }
     }


}
