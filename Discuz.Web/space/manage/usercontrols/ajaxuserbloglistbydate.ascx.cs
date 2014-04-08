using System;
	
using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Manage;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Data;

namespace Discuz.Space.Manage
{
	/// <summary>
    ///	按日期显示日志列表控件
	/// </summary>
	public class ajaxuserbloglistbydate :DiscuzSpaceUCBase
	{
        public int currentpage = DNTRequest.GetInt("currentpage", 1);

		public string postdate = null;

		public string pagelink = "";

		public bool ispostauthor = false;

		public SpacePostInfo[] __spacepostinfos ;
		
		public ajaxuserbloglistbydate()
		{		
			if(spaceid > 0)
			{
				if(Discuz.Common.DNTRequest.GetString("load") =="true")
				{					
					if(DNTRequest.GetString("postdate")!="")
					{
						try
						{
							Convert.ToDateTime(DNTRequest.GetString("postdate"));
							postdate = DNTRequest.GetString("postdate");
						}
						catch
						{
							postdate = null;	
						}
					}	

					if(postdate != null)
					{
						if(DNTRequest.GetInt("postnumber",0)>0)
							pagesize = DNTRequest.GetInt("postnumber",0);
					
						//得到页码链接
                        int postcount = Space.Data.DbProvider.GetInstance().GetSpacePostsCount(spaceconfiginfo.UserID, 1, postdate);
						if(postcount>0)
							pagelink = AjaxPagination(postcount ,pagesize,currentpage);
						else
						{
							errorinfo = "暂无数据!";
							return ;
						}
		
						//得到当前日志列表
						__spacepostinfos  = GetCurrentUserSpacePostList(currentpage,spaceconfiginfo.UserID, postdate);
					}
				}
			}
			else
			{
				errorinfo = "当前请求日志列表信息无效!";
			}
		}

		private SpacePostInfo[] GetCurrentUserSpacePostList(int currentpage,int userid,string postdate)
		{
            return BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().SpacePostsList(pagesize, currentpage, userid,Convert.ToDateTime(postdate)));
		}


		//// <summary>
		/// 分页函数
		/// </summary>
		/// <param name="recordcount">总记录数</param>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>
		public string AjaxPagination(int recordcount, int pagesize, int currentpage)
		{
			if(DNTRequest.GetInt("postnumber",0)>0)
			{
				return base.AjaxPagination(recordcount,pagesize,currentpage,"usercontrols/ajaxuserbloglistbydate.ascx","hidetitle="+hidetitle+"&spaceid="+spaceid+"&postdate="+postdate+"&postnumber="+DNTRequest.GetInt("postnumber",0),"bodyrightcontent");
			}
			else
			{
				return base.AjaxPagination(recordcount,pagesize,currentpage,"usercontrols/ajaxuserbloglistbydate.ascx","hidetitle="+hidetitle+"&spaceid="+spaceid+"&postdate="+postdate,"bodyrightcontent");
			}
		}
	}
}
