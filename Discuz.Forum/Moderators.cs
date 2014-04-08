using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
	/// <summary>
	/// ����������
	/// </summary>
	public class Moderators
	{
		/// <summary>
		/// ������а�����Ϣ
		/// </summary>
		/// <returns>���а�����Ϣ</returns>
		public static List<ModeratorInfo> GetModeratorList()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            List<ModeratorInfo> morderatorList = cache.RetrieveObject("/Forum/ModeratorList") as List<ModeratorInfo>;
            if (morderatorList == null)
			{
                morderatorList = Discuz.Data.Moderators.GetModeratorList();
                cache.AddObject("/Forum/ModeratorList", morderatorList);
			}
            return morderatorList;
		}

		/// <summary>
		/// �ж�ָ���û��Ƿ���ָ�����İ���
		/// </summary>
        /// <param name="adminId">�û�����(1Ϊ����Ա��2Ϊ���棬3Ϊ������0Ϊ��ͨ�û�)</param>
		/// <param name="uid">�û�id</param>
		/// <param name="fid">��̳id</param>
		/// <returns>����ǰ�������true, ��������򷵻�false</returns>
		public static bool IsModer(int adminId, int uid, int fid)
		{
			if (adminId == 0)
				return false;

            // �û�Ϊ����Ա���ܰ���ֱ�ӷ�����
			if (adminId == 1 || adminId == 2)
				return true;

            if (adminId == 3)
			{
				// ����ǹ���Ա���ܰ���, ��������ͨ�������ڸð���а���Ȩ��
				foreach(ModeratorInfo moderInfo in GetModeratorList())
				{
					// ��̳�������д���,�򷵻���
                    if (moderInfo.Uid == uid && moderInfo.Fid == fid)
						return true;
				}
			}
			return false;
		}

        /// <summary>
        /// ͨ�������û�����ȡ�����İ���б�
        /// </summary>
        /// <param name="moderatorUserName"></param>
        /// <returns></returns>
        public static string GetFidListByModerator(string moderatorUserName)
        {
            string fidList = "";
            foreach (ForumInfo forumInfo in Forums.GetForumList())
            {
                if (("," + forumInfo.Moderators + ",").Contains("," + moderatorUserName + ","))
                    fidList += forumInfo.Fid + ",";
            }
            return fidList.TrimEnd(',');
        }
	}
}
