using System;
using System.Data;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;


namespace Discuz.Web
{
    public class showratelist : PageBase
    {
        public int postid = DNTRequest.GetInt("pid", 0);

        public List<RateLogInfo> rateloglist = new List<RateLogInfo>();

        public string[] scorename = Scoresets.GetValidScoreName();

        public string[] scoreunit = Scoresets.GetValidScoreUnit();

        public int ratecount = 0;

        public int pagecount = 0;

        public int pagesize = 10;

        public int pageid = DNTRequest.GetInt("page", 0);

        public string pagenumbers = "";

        protected override void ShowPage()
        {
            if (postid > 0)
            {
                pagetitle = "帖子ID为" + postid + "的评分列表";

                ratecount = Posts.GetPostRateLogCount(postid);
                rateloglist = Posts.GetPostRateLogList(postid, pageid, pagesize);

                //获取总页数
                pagecount = ratecount % pagesize == 0 ? ratecount / pagesize : ratecount / pagesize + 1;
                pagecount = pagecount == 0 ? 1 : pagecount;

                //修正请求页数中可能的错误
                pageid = pageid < 1 ? 1 : pageid;
                pageid = pageid > pagecount ? pagecount : pageid;

                //得到页码链接
                pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "showratelist.aspx?pid=" + postid, 8);
            }
        }

        public string GetExtCreditName(int extCredit)
        {
            return scorename[extCredit];
        }

        public string GetExtCreditUnit(int extCredit)
        {
            return scoreunit[extCredit];
        }

        public string GetAvatarUrl(int uid)
        {
            return Urls.UserInfoAspxRewrite(uid);
        }

        public string GetScoreMark(int value)
        {
            return value > 0 ? "+" : "";
        }
    }
}
