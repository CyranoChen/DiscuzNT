using System;

using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Config;
using Discuz.Common.Generic;

namespace Discuz.Space.Manage
{
	/// <summary>
    ///	日志列表控件
	/// </summary>
	public class ajaxbloglist : DiscuzSpaceUCBase
	{
        public int currentpage = DNTRequest.GetInt("currentpage", 1);
	
		public string pagelink = "";

		public bool ispostauthor = false;

        public string forumpath = BaseConfigs.GetForumPath;

		public SpacePostInfo[] __spacepostinfos;

        public string postsidlist = "";

        public bool isAdmin = false;

		public ajaxbloglist()
		{
			if (spaceid > 0)
			{
				if (DNTRequest.GetString("load") == "true")
				{
                    if (Forum.AdminGroups.GetAdminGroupInfo(_userinfo.Groupid) != null)
                        isAdmin = true;

					if (DNTRequest.GetInt("postnumber", 0) > 0)
						pagesize = DNTRequest.GetInt("postnumber", 0);			
					
					//得到页码链接
                    int postcount = Space.Data.DbProvider.GetInstance().GetSpacePostsCount(spaceconfiginfo.UserID ,1);
					if (postcount > 0)
						pagelink = AjaxPagination(postcount ,pagesize,currentpage);
					else
					{
						errorinfo = "暂无数据!";
						return ;
					}

					//得到当前日志列表
					__spacepostinfos = GetCurrentUserSpacePostList(currentpage, spaceconfiginfo.UserID);

                    string[] postidArray = new string[__spacepostinfos.Length];
                    for (int i = 0; i < __spacepostinfos.Length; i++)
                    {
                        postidArray[i] = __spacepostinfos[i].Postid.ToString();
                    }
                    if (postidArray.Length > 0)
                        postsidlist = string.Join(",", postidArray);
				}
			}
			else
				errorinfo = "当前请求日志列表信息无效!";
		}

		private SpacePostInfo[] GetCurrentUserSpacePostList(int currentpage, int userid)
		{
            return BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().SpacePostsList(pagesize, currentpage, userid, 1));
		}

        /// <summary>
        /// 获取空间日志标签缓存文件路径
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public string GetSpacePostTagCacheFilePath(int postid)
        {
            return string.Format("{0}cache/spaceposttag/{1}/{2}_tags.txt", configspaceurlnopage, + postid / 1000 + 1, postid);
        }

		//// <summary>
		/// 分页函数
		/// </summary>
		/// <param name="recordcount">总记录数</param>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>
		public string AjaxPagination(int recordcount, int pagesize, int currentpage)
		{
			if (DNTRequest.GetInt("postnumber", 0) > 0)
			{
				return base.AjaxPagination(recordcount, pagesize, currentpage, "usercontrols/ajaxuserbloglist.ascx", "hidetitle=" + hidetitle + "&spaceid=" + spaceid + "&postnumber=" + DNTRequest.GetInt("postnumber", 0), "ajaxspaceuserbloglist");
			}
			else
			{
				return base.AjaxPagination(recordcount, pagesize, currentpage, "usercontrols/ajaxuserbloglist.ascx", "hidetitle=" + hidetitle + "&spaceid=" + spaceid, "ajaxspaceuserbloglist");
			}
		}
	}
}