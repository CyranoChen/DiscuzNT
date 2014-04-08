using System;
using Discuz.Common;

namespace Discuz.Space.Manage
{
	/// <summary>
	/// 上传文件页面
	/// </summary>
	public class UploadFile : SpaceManageBasePage
	{
		public string postid = "0";

        public UploadFile()
		{
			postid = DNTRequest.GetInt("postid", 0).ToString();
		}
	}
}