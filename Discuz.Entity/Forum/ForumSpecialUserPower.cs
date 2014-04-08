using System;
using System.Text;

namespace Discuz.Entity
{
    public enum ForumSpecialUserPower
    {
        ViewByUser = 1,
        PostByUser = 2,
        ReplyByUser = 4,
        DownloadAttachByUser = 8,
        PostAttachByUser = 16
    }
}
