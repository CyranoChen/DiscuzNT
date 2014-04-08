using System;

namespace Discuz.Entity
{
	/// <summary>
	/// SpaceStatusType 的摘要说明。
	/// </summary>
	public enum SpaceStatusType
	{
		Natural = 0, //个人空间状态正常
		AdminClose = 1, //个人空间被管理员关闭
		OwnerClose = 2 //个人空间被所有人关闭
	}
}