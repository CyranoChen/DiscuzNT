using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Web.UI
{

    public class UserCpPage : PageBase
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 分页数
        /// </summary>
        public int pagecount = 0;
        /// <summary>
        /// 页码
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 公共消息数
        /// </summary>
        public int announcepmcount = PrivateMessages.GetAnnouncePrivateMessageCount();
        /// <summary>
        /// 当前用户的新通知数
        /// </summary>
        public int newnoticecount = 0;
        /// <summary>
        /// 短消息数
        /// </summary>
        public int pmcount = 0;
        /// <summary>
        /// 可用的积分名称
        /// </summary>
        public string[] score = Scoresets.GetValidScoreName();
        /// <summary>
        /// 用户最大短消息数
        /// </summary>
        public int maxmsg;
        /// <summary>
        /// 已使用的短消息数
        /// </summary>
        public int usedmsgcount;
        /// <summary>
        /// 已使用的短消息条宽度
        /// </summary>
        public int usedmsgbarwidth = 0;
        /// <summary>
        /// 未使用的短消息条宽度
        /// </summary>
        public int unusedmsgbarwidth = 0;

        protected bool IsLogin()
        {
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return false;
            }

            user = Users.GetUserInfo(userid);
            if (user == null)
            {
                AddErrLine("用户不存在");
                return false;
            }
            return true;
        }

        protected void BindItems(int recordCount, string pageName)
        {
            user = Users.GetUserInfo(userid);
            //获取总页数
            pagecount = recordCount % 16 == 0 ? recordCount / 16 : recordCount / 16 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;

            //修正请求页数中可能的错误
            pageid = pageid < 1 ? 1 : pageid;
            pageid = pageid > pagecount ? pagecount : pageid;
            
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, pageName, 8);
        }

        protected void BindItems(int recordCount)
        {
            BindItems(recordCount, pagename.ToLower());
        }

        /// <summary>
        /// 绑定短消息
        /// </summary>
        /// <param name="folder">文件箱,所属文件夹(0:收件箱,1:发件箱,2:草稿箱),-1为获取全部短消息条数</param>
        protected void BindPrivateMessage(int folder)
        {
            //获取主题总数
            pmcount = PrivateMessages.GetPrivateMessageCount(userid, folder);
            usedmsgcount = PrivateMessages.GetPrivateMessageCount(userid, folder);
            maxmsg = usergroupinfo.Maxpmnum;
            if (maxmsg > 0)
            {
                usedmsgbarwidth = usedmsgcount * 100 / maxmsg;
                unusedmsgbarwidth = 100 - usedmsgbarwidth;
            }
            BindItems(pmcount);
        }
    }
}
