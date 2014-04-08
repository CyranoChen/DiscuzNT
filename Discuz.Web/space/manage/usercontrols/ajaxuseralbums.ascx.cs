using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Data;

namespace Discuz.Space.Manage
{
	/// <summary>
	///	用户相册控件
	/// </summary>
	public class ajaxuseralbum : DiscuzSpaceUCBase
	{

		public int currentpage = 0;

		public string pagelink = "";

		public bool ispostauthor = false;

		public AlbumInfo[] __albums;
		public string forumPath;

		public ajaxuseralbum()
		{
		
			forumPath = BaseConfigs.GetForumPath;
			if(spaceid > 0)
			{
				if(Discuz.Common.DNTRequest.GetString("load") == "true")
				{
					currentpage = DNTRequest.GetInt("currentpage",1);
			
		
					//得到当前相册列表
                    __albums = BlogProvider.GetSpaceAlbumsInfo(Space.Data.DbProvider.GetInstance().SpaceAlbumsList(10, currentpage, spaceconfiginfo.UserID));
                    
					if(__albums.Length > 0)
					{
						if(__albums[0].Userid == userid)
						{
							ispostauthor = true;
						}
					}

					//得到页码链接
                    pagelink = AjaxPagination(Space.Data.DbProvider.GetInstance().GetSpaceAlbumsCount(spaceconfiginfo.UserID), pagesize, currentpage, "");
				}
			}
			else
			{
				errorinfo = "当前请求日志图片信息无效!";
			}
		}


		//// <summary>
		/// 分页函数
		/// </summary>
		/// <param name="recordcount">总记录数</param>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>
		/// <param name="url">Url参数</param>
		public string AjaxPagination(int recordcount, int pagesize, int currentpage, string url )
		{
			return base.AjaxPagination(recordcount,pagesize,currentpage,"usercontrols/ajaxuseralbums.ascx","spaceid="+spaceid,"useralbums");
		}

	}
}
