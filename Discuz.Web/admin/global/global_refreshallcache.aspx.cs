using System;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
	/// <summary>
	/// 更新缓存
	/// </summary>
    public class global_refreshallcache : Page
	{
		private void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int opnumber = DNTRequest.GetInt("opnumber", 0);
				int result = -1;

				#region 根据缓存更新选项更新相应的缓存数据

				switch (opnumber)
				{
					case 1:
					{
                        //重设管理组信息
                        Caches.ReSetAdminGroupList();
						result = 2;
						break;
					}
					case 2:
					{
                        //重设用户组信息
                        Caches.ReSetUserGroupList();
						result = 3;
						break;
					}
					case 3:
					{
                        //重设版主信息
                        Caches.ReSetModeratorList();
						result = 4;
						break;
					}

					case 4:
					{
                        //重设指定时间内的公告列表
                        Caches.ReSetAnnouncementList();
                        Caches.ReSetSimplifiedAnnouncementList();
						result = 5;
						break;
					}
					case 5:
					{
                        //重设第一条公告
                        Caches.ReSetSimplifiedAnnouncementList();
						result = 6;
						break;
					}
					case 6:
					{
                        //重设版块下拉列表
                        Caches.ReSetForumListBoxOptions();
						result = 7;
						break;
					}
					case 7:
					{
                        //重设表情
                        Caches.ReSetSmiliesList();
						result = 8;
						break;
					}
					case 8:
					{
                        //重设主题图标
                        Caches.ReSetIconsList();
						result = 9;
						break;
					}
					case 9:
					{
                        //重设自定义标签
                        Caches.ReSetCustomEditButtonList();
						result = 10;
						break;
					}
					case 10:
					{
                        //重设论坛基本设置
						//Caches.ReSetConfig();
						result = 11;
						break;
					}
					case 11:
					{
                        //重设论坛积分
                        Caches.ReSetScoreset();
						result = 12;
						break;
					}
					case 12:
					{
                        //重设地址对照表
                        Caches.ReSetSiteUrls();
						result = 13;
						break;
					}
					case 13:
					{
                        //重设论坛统计信息
                        Caches.ReSetStatistics();
						result = 14;
						break;
					}
					case 14:
					{
                        //重设系统允许的附件类型和大小
                        Caches.ReSetAttachmentTypeArray();
						result = 15;
						break;
					}
					case 15:
					{
                        //重设模板列表的下拉框html
                        Caches.ReSetTemplateListBoxOptionsCache();
						result = 16;
						break;
					}
					case 16:
					{
                        //重设在线用户列表图例
                        Caches.ReSetOnlineGroupIconList();
						result = 17;
						break;
					}
					case 17:
					{
                        //重设友情链接列表
                        Caches.ReSetForumLinkList();
						result = 18;
						break;
					}
					case 18:
					{
                        //重设脏字过滤列表
                        Caches.ReSetBanWordList();
						result = 19;
						break;
					}
					case 19:
					{
                        //重设论坛列表
                        Caches.ReSetForumList();
						result = 20;
						break;
					}
					case 20:
					{
                        //重设在线用户信息
                        Caches.ReSetOnlineUserTable();
						result = 21;
						break;
					}
					case 21:
					{
                        //重设论坛整体RSS及指定版块RSS
                        Caches.ReSetRss();
						result = 22;
						break;
					}
					case 22:
					{
                        //重设论坛整体RSS
                        Caches.ReSetRssXml();
						result = 23;
						break;
					}
					case 23:
					{
                        //重设模板ID列表
                        Caches.ReSetValidTemplateIDList();
						result = 24;
						break;
					}
					case 24:
					{
                        //重设有效用户表扩展字段
                        Caches.ReSetValidScoreName();
						result = 25;
						break;
					}
					case 25:
					{
                        //重设勋章列表
                        Caches.ReSetMedalsList();
						result = 26;
						break;
					}
					case 26:
					{
                        //重设数据链接串和表前缀
                        Caches.ReSetDBlinkAndTablePrefix();
						result = 27;
						break;
					}
					case 27:
					{
                        //重设帖子列表
                        Caches.ReSetAllPostTableName();
						result = 28;
						break;
					}
					case 28:
					{
                        //重设最后帖子表
                        Caches.ReSetLastPostTableName();
						result = 29;
						break;
					}
					case 29:
					{
                        //重设广告列表
                        Caches.ReSetAdsList();
						result = 30;
						break;
					}
					case 30:
					{
                        //重设用户上一次执行搜索操作时间
                        Caches.ReSetStatisticsSearchtime();
						result = 31;
						break;
					}
					case 31:
					{
                        //重设用户一分钟内搜索次数
                        Caches.ReSetStatisticsSearchcount();
						result = 32;
						break;
					}
					case 32:
					{
                        //重设用户头象列表
                        Caches.ReSetCommonAvatarList();
						result = 33;
						break;
					}
					case 33:
					{
                        //重设干扰码字符串
						Caches.ReSetJammer();
						result = 34;
						break;
					}
					case 34:
					{
                        //重设魔力列表
						Caches.ReSetMagicList();
						result = 35;
						break;
					}
					case 35:
					{
                        //重设兑换比率可交易积分策略
						Caches.ReSetScorePaySet();
						result = 36;
						break;
					}
					case 36:
					{
                        //重设当前帖子表相关信息
						Caches.ReSetPostTableInfo();
						result = 37;
						break;
					}
					case 37:
					{
                        //重设全部版块精华主题列表
						Caches.ReSetDigestTopicList(16);
						result = 38;
						break;
					}
					case 38:
					{
                        //重设全部版块热帖主题列表
						Caches.ReSetHotTopicList(16, 30);
						result = 39;
						break;
					}
					case 39:
					{
                        //重设最近主题列表
						Caches.ReSetRecentTopicList(16);
						result = 40;
						break;
					}
					case 40:
					{
                        //重设BaseConfig
						Caches.EditDntConfig();
						result = 41;
						break;
					}
					case 41:
					{
                        //重设在线用户表
						OnlineUsers.InitOnlineList();
						result = 42;
						break;
					}
                    case 42:
			        {
                        //重设导航弹出菜单
			            Caches.ReSetNavPopupMenu();
			            result = -1;
			            break;
			        }
				}

				#endregion
				Response.Write(result);
				Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
				Response.Expires = -1;
				Response.End();
			}
		}
	}
}
