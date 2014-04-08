using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminUserGroupFactory ��ժҪ˵����
	/// ��̨�û�����������
	/// </summary>
	public class AdminUserGroups : UserGroups
	{
		public static string opresult = ""; //�洢��������򷵻ظ��û�����Ϣ

		/// <summary>
		/// ͨ��ָ�����û���id�õ���ص��û�����Ϣ
		/// </summary>
		/// <param name="groupid"></param>
		/// <returns></returns>
		public static UserGroupInfo AdminGetUserGroupInfo(int groupid)
		{
            return UserGroups.GetUserGroupInfo(groupid);
		}

		/// <summary>
		/// �õ��������ֶ���Ϣ
		/// </summary>
		/// <param name="groupid"></param>
		/// <returns></returns>
		public static AdminGroupInfo AdminGetAdminGroupInfo(int groupid)
		{
            return AdminGroups.GetAdminGroupInfo(groupid);
		}

		/// <summary>
		/// ����û�����Ϣ
		/// </summary>
        /// <param name="userGroupInfo"></param>
		/// <returns></returns>
		public static bool AddUserGroupInfo(UserGroupInfo userGroupInfo)
		{
			try
			{
				int Creditshigher = userGroupInfo.Creditshigher;
				int Creditslower = userGroupInfo.Creditslower;
                DataTable dt = Discuz.Data.UserGroups.GetUserGroupByCreditsHigherAndLower(Creditshigher, Creditslower);
                if (dt.Rows.Count > 0)
                    return false;

				if (userGroupInfo.Radminid == 0 && !SystemCheckCredits("add", ref Creditshigher, ref Creditslower, 0))
					return false;

                userGroupInfo.Creditshigher = Creditshigher;
                userGroupInfo.Creditslower = Creditslower;
                Data.UserGroups.CreateUserGroup(userGroupInfo);
                Data.OnlineUsers.AddOnlineList(userGroupInfo.Grouptitle);

                Caches.ReSetAdminGroupList();
                Caches.ReSetUserGroupList();
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// У��ָ�������û����������
		/// </summary>
		/// <param name="opname">��������</param>
		/// <param name="Creditshigher">��������</param>
		/// <param name="Creditslower">��������</param>
		/// <param name="groupid"></param>
		public static bool SystemCheckCredits(string opname, ref int creditsHigher, ref int creditsLower, int groupid)
		{
			opresult = "";

			switch (opname.ToLower())
			{
				case "add":
					{
						#region

                        DataTable dt = Data.UserGroups.GetMinCreditHigher();

						if (dt.Rows.Count > 0)
						{
							int systemMiniCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (creditsLower <= systemMiniCredits) //��ϵͳ��С���������黹���ڵ��ڵ�ǰ��ӵĻ����������ʱ
							{
								creditsLower = systemMiniCredits;
								opresult = "����������Ļ�������С�ڻ����ϵͳ��Сֵ,���ϵͳ�ѽ������Ϊ" + systemMiniCredits;
								break;
							}
						}

                        dt = Data.UserGroups.GetMaxCreditLower();
						if (dt.Rows.Count > 0)
						{
							int systemMaxCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (creditsHigher >= systemMaxCredits) //��ϵͳ�����������黹С�ڵ��ڵ�ǰ��ӵĻ����������ʱ
							{
								creditsHigher = systemMaxCredits;
								opresult = "����������Ļ������޴��ڻ����ϵͳ���ֵ,���ϵͳ�ѽ������Ϊ" + systemMaxCredits;
								break;
							}
						}

                        dt = Discuz.Data.UserGroups.GetUserGroupByCreditshigher(creditsHigher);
						if (dt.Rows.Count > 0)
						{
							int currentGroupID = Convert.ToInt32(dt.Rows[0][0].ToString());
							int currentCreditsHigher = Convert.ToInt32(dt.Rows[0][1].ToString());
							int currentCreditsLower = Convert.ToInt32(dt.Rows[0][2].ToString());

							if (creditsLower > currentCreditsLower)
								return false;
							else
							{
								if (creditsHigher == currentCreditsHigher)
								{
									if (creditsLower < currentCreditsLower)
									{
                                        UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(currentGroupID);
                                        userGroupInfo.Creditslower = creditsLower;
                                        UserGroups.UpdateUserGroup(userGroupInfo);
										break;
									}
									else
									{
										opresult = "����������Ļ������޴��ڻ����������Ч�������޵����ֵ" + currentCreditsLower + ",���ϵͳ��Ч�ύ��������!";
										return false;
									}
								}
								else
								{
									creditsLower = currentCreditsLower;
									//���µ�ǰ��ѯ������,Ҳ����Ҫ��ӵ���Ļ�������λ�ڵ�ǰ��ѯ��Ļ���������֮��
                                    Discuz.Data.UserGroups.UpdateUserGroupCreidtsLower(currentCreditsHigher, creditsHigher);
									break;
								}
							}
						}
						else
						{
							opresult = "ϵͳδ�ᵽ���ʵ�λ�ñ������ύ����Ϣ!";
							return false;
						}

						#endregion
					}
				case "delete":
					{
                        if (Discuz.Data.UserGroups.GetGroupCountByCreditsLower(creditsHigher) > 0)
                            Discuz.Data.UserGroups.UpdateUserGroupsCreditsLowerByCreditsLower(creditsLower, creditsHigher);	
						else
                            Discuz.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(creditsHigher, creditsLower);
						break;
					}
				case "update":
					{
						#region

                        UserGroupInfo oldInfo = UserGroups.GetUserGroupInfo(groupid);
						int currentGroupOldCreatesHigher =oldInfo.Creditshigher; //Ҫ���µĵ�ǰ�û�����ϵ����޻���
						int currentGroupOldCreatesLower = oldInfo.Creditslower; //Ҫ���µĵ�ǰ�û�����ϵ����޻���

                        DataTable dt = Discuz.Data.UserGroups.GetMinCreditHigher();
						if (dt.Rows.Count > 0)
						{
							int systemMiniCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (creditsLower <= systemMiniCredits) //��ϵͳ��С���������黹���ڵ��ڵ�ǰ��ӵĻ����������ʱ
							{
								creditsLower = systemMiniCredits;
								opresult = "����������Ļ�������С�ڻ����ϵͳ���ֵ,���ϵͳ�ѽ������Ϊ" + systemMiniCredits;
                                Discuz.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(currentGroupOldCreatesHigher, currentGroupOldCreatesLower);								
								break;
							}
						}

                        dt = Data.UserGroups.GetMaxCreditLower();
						if (dt.Rows.Count > 0)
						{
							int systemMaxCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (creditsHigher >= systemMaxCredits) //��ϵͳ�����������黹С�ڵ��ڵ�ǰ��ӵĻ����������ʱ
							{
								creditsHigher = systemMaxCredits;
								opresult = "����������Ļ������޴��ڻ����ϵͳ���ֵ,���ϵͳ�ѽ������Ϊ" + systemMaxCredits;
                                Discuz.Data.UserGroups.UpdateUserGroupsCreditsLowerByCreditsLower(currentGroupOldCreatesLower, currentGroupOldCreatesHigher);								
								break;
							}
						}

                        dt = Discuz.Data.UserGroups.GetUserGroupByCreditshigher(creditsHigher);
						if (dt.Rows.Count > 0)
						{
							int currentGroupID = Convert.ToInt32(dt.Rows[0][0].ToString());
							int currentCreditsHigher = Convert.ToInt32(dt.Rows[0][1].ToString());
							int currentCreditsLower = Convert.ToInt32(dt.Rows[0][2].ToString());

							if (creditsLower > currentCreditsLower)
							{
								opresult = "����������Ļ������޴��ڻ����������Ч�������޵����ֵ" + currentCreditsLower + ",���ϵͳ��Ч�ύ��������!";
								return false;
							}
							else
							{
								if (creditsHigher == currentCreditsHigher)
								{
									if (creditsLower < currentCreditsLower)
									{
										//�����Ե�ǰ�ϵĻ�������Ϊ���޵��û��������ֵΪ�ϵĻ�������
                                        Discuz.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(creditsLower, currentCreditsLower);
										break;
									}
								}
								else
								{
									opresult = "ϵͳ���Զ������ύ�Ļ������޵���Ϊ" + currentCreditsLower;

									//�����Ե�ǰ�ϵĻ�������Ϊ���޵��û��������ֵΪ�ϵĻ�������
                                    Discuz.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(creditsLower, currentCreditsLower);
                                    Discuz.Data.UserGroups.UpdateUserGroupsCreditsLowerByCreditsLower(creditsHigher, currentCreditsHigher);
    								break;
								}
							}
						}
						else
						{
							opresult = "ϵͳδ�ᵽ���ʵ�λ�ñ������ύ����Ϣ!";
							return false;
						}

						#endregion

						break;
					}
			}
			return true;
		}


		/// <summary>
		/// �����û�����Ϣ
		/// </summary>
        /// <param name="userGroupInfo">�û�����Ϣ</param>
		/// <returns></returns>
		public static bool UpdateUserGroupInfo(UserGroupInfo userGroupInfo)
		{
			int Creditshigher = userGroupInfo.Creditshigher;
			int Creditslower = userGroupInfo.Creditslower;

			if ((userGroupInfo.Groupid >= 9) && (userGroupInfo.Radminid == 0))
			{
				//���Ѵ��ڵ��û�����������޲��ǵ�ǰ���ʱ��,������༭
                DataTable dt = Discuz.Data.UserGroups.GetUserGroupByCreditsHigherAndLower(Creditshigher, Creditslower);
				if (dt.Rows.Count > 0 && userGroupInfo.Groupid.ToString() != dt.Rows[0][0].ToString())
    				return false;
			
				if (!SystemCheckCredits("update", ref Creditshigher, ref Creditslower, userGroupInfo.Groupid))
					return false;
			}

            UserGroups.UpdateUserGroup(userGroupInfo);
            Discuz.Data.UserGroups.UpdateOnlineList(userGroupInfo);

            Caches.ReSetAdminGroupList();
            Caches.ReSetUserGroupList();
			return true;
		}
 
		/// <summary>
		/// ɾ��ָ���û���
		/// </summary>
		/// <param name="groupid"></param>
		/// <returns></returns>
		new public static bool DeleteUserGroupInfo(int groupid)
		{
			try
			{
                if (Discuz.Data.UserGroups.IsSystemOrTemplateUserGroup(groupid))
				{
					//��Ϊϵͳ��ʼ���ģ����ʱ,������ɾ��
					return false;
				}

				//��Ϊ�û���ʱ
				if (groupid >= 9)
				{
                    DataTable dt = UserGroups.GetUserGroupExceptGroupid(groupid);
					if (dt.Rows.Count > 1)
					{
                        UserGroupInfo info = UserGroups.GetUserGroupInfo(groupid);
                        if (info.Radminid == 0)
						{
							int creditshigher =info.Creditshigher;
							int creditslower = info.Creditslower;
							SystemCheckCredits("delete", ref creditshigher, ref creditslower, groupid);
						}
					}
					else
					{
						if (dt.Rows.Count == 1)
						{
							//��ϵͳɾ����ǰ���ֻ��һ�������ʱ��ֱ������Ψһ������,�����޸�Ψһ�����޵�ֵ
                            Data.UserGroups.UpdateUserGroupLowerAndHigherToLimit(Utils.StrToInt(dt.Rows[0][0], 0));
                        }
						else
						{ //ϵͳ���û���ֻ��һ��ʱ
							opresult = "��ǰ�û���Ϊϵͳ��Ψһ���û���,���ϵͳ�޷�ɾ��";
							return false;
						}
					}
				}
                UserGroups.DeleteUserGroupInfo(groupid);
                AdminGroups.DeleteAdminGroupInfo(short.Parse(groupid.ToString()));
                Data.OnlineUsers.DeleteOnlineByUserGroup(groupid);
                Caches.ReSetAdminGroupList();
                Caches.ReSetUserGroupList();

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}