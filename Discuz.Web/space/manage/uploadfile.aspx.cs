using System;
using Discuz.Common;

namespace Discuz.Space.Manage
{
	/// <summary>
	/// �ϴ��ļ�ҳ��
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