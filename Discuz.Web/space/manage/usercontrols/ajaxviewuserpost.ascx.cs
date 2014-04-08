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
    ///	��ʾ��־���ݿؼ�
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
                    //���Ƿ���״̬��ǰ���ߵ���־ʱ
                    if (__spacepostsinfo.PostStatus == 1 || __spacepostsinfo.Uid == userid)
                    {
                        categorylink = GetCategoryLink(__spacepostsinfo.Category);
                        ForumUtils.WriteCookie("referer", string.Format("space/viewspacepost.aspx?postid={0}&spaceid={1}", postid, sid));
                    }
                    else
                        errorinfo = "��ǰ�����������Ч!";
                }
                else
                    errorinfo = "�������־������";
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
