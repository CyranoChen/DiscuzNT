using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Data;

namespace Discuz.Space.Manage
{
	/// <summary>
	///	论坛功能统计控件
	/// </summary>
	public class ajaxspaceconfigstatic : DiscuzSpaceUCBase
	{

		public SpaceConfigInfo __spaceconfiginfo ;

		public ajaxspaceconfigstatic()
		{
			if(Discuz.Common.DNTRequest.GetString("load") =="true")
			{
                __spaceconfiginfo = BlogProvider.GetSpaceConfigInfo(spaceconfiginfo.UserID);
			}
		}
	}
}
