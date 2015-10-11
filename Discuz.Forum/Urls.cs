using System;
using System.Text;

using Discuz.Config;
using Discuz.Common;

namespace Discuz.Forum
{
    public class Urls
    {
        #region aspxrewrite 配置

        public static GeneralConfigInfo config
        {
            get { return GeneralConfigs.GetConfig(); }
        }

        /// <summary>
        /// 设置关于showforum页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowForumAspxRewrite(int forumid, int pageid)
        {
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 1)
                    return "showforum-" + forumid + "-" + pageid + config.Extname;
                else
                    return "showforum-" + forumid + config.Extname;
            }
            else
            {
                if (pageid > 1)
                    return "showforum.aspx?forumid=" + forumid + "&page=" + pageid;
                else
                    return "showforum.aspx?forumid=" + forumid;
            }
        }

        /// <summary>
        /// 设置关于showtopic页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowTopicAspxRewrite(int topicid, int pageid)
        {
            return ShowTopicAspxRewrite(topicid, pageid, -1);
        }

        public static string ShowTopicAspxRewrite(int topicid, int pageid, int typeid)
        {
            //当使用伪aspx
            if (config.Aspxrewrite == 1 && typeid <0)
            {
                if (pageid > 1)
                    return "showtopic-" + topicid + "-" + pageid + config.Extname;
                else
                    return "showtopic-" + topicid + config.Extname;
            }
            else
            {
                if (pageid > 1)
                    return "showtopic.aspx?topicid=" + topicid + "&page=" + pageid + (typeid>0 ? "&typeid=" + typeid : "");
                else
                    return "showtopic.aspx?topicid=" + topicid + (typeid >= 0 ? "&typeid=" + typeid : "");
            }
        }

        public static string ShowDebateAspxRewrite(int topicid)
        {
            return ShowDebateAspxRewrite(topicid, 0);
        }

        public static string ShowDebateAspxRewrite(int topicid, int typeid)
        {
            if (config.Aspxrewrite == 1 && typeid < 0)
                return string.Format("showdebate-{0}{1}", topicid, config.Extname);
            else
                return string.Format("showdebate.aspx?topicid={0}{1}", topicid, (typeid >= 0 ? "&typeid=" + typeid : ""));
        }

        /// <summary>
        /// 设置关于showbonus页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowBonusAspxRewrite(int topicid, int pageid)
        {
            return ShowBonusAspxRewrite(topicid, pageid, -1);
        }

        public static string ShowBonusAspxRewrite(int topicid, int pageid, int typeid)
        {
            //当使用伪aspx
            if (config.Aspxrewrite == 1 && typeid < 0)
            {
                if (pageid > 1)
                    return "showbonus-" + topicid + "-" + pageid + config.Extname;
                else
                    return "showbonus-" + topicid + config.Extname;
            }
            else
            {
                if (pageid > 1)
                    return "showbonus.aspx?topicid=" + topicid + "&page=" + pageid + (typeid > 0 ? "&typeid=" + typeid : "");
                else
                    return "showbonus.aspx?topicid=" + topicid + (typeid >= 0 ? "&typeid=" + typeid : "");
            }
        }


        public static string UserInfoAspxRewrite(int userid)
        {
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
                return "userinfo-" + userid + config.Extname;
            else
                return "userinfo.aspx?userid=" + userid;
        }

        /// <summary>
        /// 设置关于userinfo页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string RssAspxRewrite(int forumid)
        {
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
                return "rss-" + forumid + config.Extname;
            else
                return "rss.aspx?forumid=" + forumid;
        }


        /// <summary>
        /// 设置关于showgoods页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowGoodsAspxRewrite(int goodsid)
        {
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
                return "showgoods-" + goodsid + config.Extname;
            else
                return "showgoods.aspx?goodsid=" + goodsid;
        }


        /// <summary>
        /// 设置关于showgoods页面链接的显示样式
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static string ShowGoodsListAspxRewrite(int categoryid, int pageid)
        {
            //当使用伪aspx
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 1)
                    return "showgoodslist-" + categoryid + "-" + pageid + config.Extname;
                else
                    return "showgoodslist-" + categoryid + config.Extname;
            }
            else
            {
                if (pageid > 1)
                    return "showgoodslist.aspx?categoryid=" + categoryid + "&page=" + pageid;
                else
                    return "showgoodslist.aspx?categoryid=" + categoryid;
            }
        }

        /// <summary>
        /// 获取当前版块链接信息
        /// </summary>
        /// <param name="pathlist">版块路径串</param>
        /// <param name="forumid">当前所属版块id</param>
        /// <param name="forumpageid">当前分类id</param>
        /// <returns></returns>
        public static string ShowForumAspxRewrite(string pathlist, int forumid, int pageid)
        {
            //当页面链接参数形式为"showforum-1.aspx"时
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 1)
                    pathlist = pathlist.Replace("showforum-" + forumid + config.Extname, "showforum-" + forumid + "-" + pageid + config.Extname);
                else
                    pathlist = pathlist.Replace("showforum-" + forumid + config.Extname, "showforum-" + forumid + config.Extname);
            }
            else
            {
                if (pageid > 1)
                    pathlist = pathlist.Replace(config.Extname + "?forumid=" + forumid + "\"", config.Extname + "?forumid=" + forumid + "&page=" + pageid + "\"");
                else
                    pathlist = pathlist.Replace(config.Extname + "?forumid=" + forumid + "\"", config.Extname + "?forumid=" + forumid + "\"");
            }
            return pathlist;
        }

        /// <summary>
        /// 获取当前版块链接信息
        /// </summary>
        /// <param name="pathlist">版块路径串</param>
        /// <param name="forumid">当前所属版块id</param>
        /// <param name="forumpageid">当前分类id</param>
        /// <returns></returns>
        public static string ShowForumAspxRewrite(int forumid, int pageid, string rewritename)
        {
            if (!Utils.StrIsNullOrEmpty(rewritename))
            {
                //当为IIS Rewrite
                if (config.Iisurlrewrite == 1)
                    return rewritename = rewritename + (pageid > 1 ? "/" + pageid : "") + "/";

                //当页面链接参数形式为"showforum-1.aspx"时
                if (config.Aspxrewrite == 1)
                    return rewritename = rewritename + (pageid > 1 ? "/" + pageid : "") + "/list.aspx";
            }
            return ShowForumAspxRewrite(forumid, pageid);
        }
        #endregion

    }
}
