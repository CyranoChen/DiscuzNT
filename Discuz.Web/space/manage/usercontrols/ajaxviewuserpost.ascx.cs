using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Space.Manage
{
    /// <summary>
    ///	显示日志内容控件
    /// </summary>
    public class ajaxviewuserpost : DiscuzSpaceUCBase
    {
        public SpacePostInfo __spacepostsinfo = new SpacePostInfo();

        public string categorylink = "";

        public int postid = DNTRequest.GetInt("postid", 0);

        public int sid = DNTRequest.GetInt("spaceid", 0);

        public bool isAdmin = false;

        private bool _showasajax = true;

        public string forumpath = BaseConfigs.GetForumPath;

        public bool Showasajax
        {
            set { _showasajax = value; }
            get { return _showasajax; }
        }

        public ajaxviewuserpost()
        {
            if (postid == 0)
                return;
            else
            {
                __spacepostsinfo = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().GetSpacePost(postid));
                if (_userinfo != null && Forum.AdminGroups.GetAdminGroupInfo(_userinfo.Groupid) != null)
                    isAdmin = true;

                if (__spacepostsinfo != null)
                {
                    //当是发布状态或当前作者的日志时
                    if (__spacepostsinfo.PostStatus == 1 || __spacepostsinfo.Uid == userid)
                    {
                        categorylink = GetCategoryLink(__spacepostsinfo.Category);
                        ForumUtils.WriteCookie("referer", string.Format("space/viewspacepost.aspx?postid={0}&spaceid={1}", postid, sid));
                    }
                    else
                        errorinfo = "当前请求的内容无效!";
                }
                else
                    errorinfo = "请求的日志不存在";
            }
        }

        private string GetCategoryLink(string categoryidlist)
        {
            System.Data.IDataReader __idatareader = Space.Data.DbProvider.GetInstance().GetCategoryIDAndName(categoryidlist);
            string categorylinkinfo = "";
            if (__idatareader != null)
            {
                while (__idatareader.Read())
                {
                    if (categorylinkinfo == "")
                        categorylinkinfo = __idatareader["title"].ToString();
                    else
                        categorylinkinfo = categorylinkinfo + " , " + __idatareader["title"].ToString();
                }
                __idatareader.Close();
            }
            return categorylinkinfo;
        }
    }
}
