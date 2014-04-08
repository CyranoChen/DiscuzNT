using System;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
	/// <summary>
	/// ���»���
	/// </summary>
    public class global_refreshallcache : Page
	{
		private void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int opnumber = DNTRequest.GetInt("opnumber", 0);
				int result = -1;

				#region ���ݻ������ѡ�������Ӧ�Ļ�������

				switch (opnumber)
				{
					case 1:
					{
                        //�����������Ϣ
                        Caches.ReSetAdminGroupList();
						result = 2;
						break;
					}
					case 2:
					{
                        //�����û�����Ϣ
                        Caches.ReSetUserGroupList();
						result = 3;
						break;
					}
					case 3:
					{
                        //���������Ϣ
                        Caches.ReSetModeratorList();
						result = 4;
						break;
					}

					case 4:
					{
                        //����ָ��ʱ���ڵĹ����б�
                        Caches.ReSetAnnouncementList();
                        Caches.ReSetSimplifiedAnnouncementList();
						result = 5;
						break;
					}
					case 5:
					{
                        //�����һ������
                        Caches.ReSetSimplifiedAnnouncementList();
						result = 6;
						break;
					}
					case 6:
					{
                        //�����������б�
                        Caches.ReSetForumListBoxOptions();
						result = 7;
						break;
					}
					case 7:
					{
                        //�������
                        Caches.ReSetSmiliesList();
						result = 8;
						break;
					}
					case 8:
					{
                        //��������ͼ��
                        Caches.ReSetIconsList();
						result = 9;
						break;
					}
					case 9:
					{
                        //�����Զ����ǩ
                        Caches.ReSetCustomEditButtonList();
						result = 10;
						break;
					}
					case 10:
					{
                        //������̳��������
						//Caches.ReSetConfig();
						result = 11;
						break;
					}
					case 11:
					{
                        //������̳����
                        Caches.ReSetScoreset();
						result = 12;
						break;
					}
					case 12:
					{
                        //�����ַ���ձ�
                        Caches.ReSetSiteUrls();
						result = 13;
						break;
					}
					case 13:
					{
                        //������̳ͳ����Ϣ
                        Caches.ReSetStatistics();
						result = 14;
						break;
					}
					case 14:
					{
                        //����ϵͳ����ĸ������ͺʹ�С
                        Caches.ReSetAttachmentTypeArray();
						result = 15;
						break;
					}
					case 15:
					{
                        //����ģ���б��������html
                        Caches.ReSetTemplateListBoxOptionsCache();
						result = 16;
						break;
					}
					case 16:
					{
                        //���������û��б�ͼ��
                        Caches.ReSetOnlineGroupIconList();
						result = 17;
						break;
					}
					case 17:
					{
                        //�������������б�
                        Caches.ReSetForumLinkList();
						result = 18;
						break;
					}
					case 18:
					{
                        //�������ֹ����б�
                        Caches.ReSetBanWordList();
						result = 19;
						break;
					}
					case 19:
					{
                        //������̳�б�
                        Caches.ReSetForumList();
						result = 20;
						break;
					}
					case 20:
					{
                        //���������û���Ϣ
                        Caches.ReSetOnlineUserTable();
						result = 21;
						break;
					}
					case 21:
					{
                        //������̳����RSS��ָ�����RSS
                        Caches.ReSetRss();
						result = 22;
						break;
					}
					case 22:
					{
                        //������̳����RSS
                        Caches.ReSetRssXml();
						result = 23;
						break;
					}
					case 23:
					{
                        //����ģ��ID�б�
                        Caches.ReSetValidTemplateIDList();
						result = 24;
						break;
					}
					case 24:
					{
                        //������Ч�û�����չ�ֶ�
                        Caches.ReSetValidScoreName();
						result = 25;
						break;
					}
					case 25:
					{
                        //����ѫ���б�
                        Caches.ReSetMedalsList();
						result = 26;
						break;
					}
					case 26:
					{
                        //�����������Ӵ��ͱ�ǰ׺
                        Caches.ReSetDBlinkAndTablePrefix();
						result = 27;
						break;
					}
					case 27:
					{
                        //���������б�
                        Caches.ReSetAllPostTableName();
						result = 28;
						break;
					}
					case 28:
					{
                        //����������ӱ�
                        Caches.ReSetLastPostTableName();
						result = 29;
						break;
					}
					case 29:
					{
                        //�������б�
                        Caches.ReSetAdsList();
						result = 30;
						break;
					}
					case 30:
					{
                        //�����û���һ��ִ����������ʱ��
                        Caches.ReSetStatisticsSearchtime();
						result = 31;
						break;
					}
					case 31:
					{
                        //�����û�һ��������������
                        Caches.ReSetStatisticsSearchcount();
						result = 32;
						break;
					}
					case 32:
					{
                        //�����û�ͷ���б�
                        Caches.ReSetCommonAvatarList();
						result = 33;
						break;
					}
					case 33:
					{
                        //����������ַ���
						Caches.ReSetJammer();
						result = 34;
						break;
					}
					case 34:
					{
                        //����ħ���б�
						Caches.ReSetMagicList();
						result = 35;
						break;
					}
					case 35:
					{
                        //����һ����ʿɽ��׻��ֲ���
						Caches.ReSetScorePaySet();
						result = 36;
						break;
					}
					case 36:
					{
                        //���赱ǰ���ӱ������Ϣ
						Caches.ReSetPostTableInfo();
						result = 37;
						break;
					}
					case 37:
					{
                        //����ȫ����龫�������б�
						Caches.ReSetDigestTopicList(16);
						result = 38;
						break;
					}
					case 38:
					{
                        //����ȫ��������������б�
						Caches.ReSetHotTopicList(16, 30);
						result = 39;
						break;
					}
					case 39:
					{
                        //������������б�
						Caches.ReSetRecentTopicList(16);
						result = 40;
						break;
					}
					case 40:
					{
                        //����BaseConfig
						Caches.EditDntConfig();
						result = 41;
						break;
					}
					case 41:
					{
                        //���������û���
						OnlineUsers.InitOnlineList();
						result = 42;
						break;
					}
                    case 42:
			        {
                        //���赼�������˵�
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
