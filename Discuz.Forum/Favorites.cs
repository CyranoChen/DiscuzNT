using System;
using System.Data;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// �ղؼв�����
	/// </summary>
	public class Favorites
	{
		/// <summary>
		/// �����ղ���Ϣ
		/// </summary>
		/// <param name="uid">�û�ID</param>
		/// <param name="tid">����ID</param>
		/// <returns>�����ɹ����� 1 ���򷵻� 0</returns>	
		public static int CreateFavorites(int uid,int tid)
		{
            if (uid < 0)
                return 0;

            return CreateFavorites(uid, tid, FavoriteType.ForumTopic);
		}

        /// <summary>
        /// �����ղ���Ϣ
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="tid">����ID</param>
        /// <param name="type">�ղ�����</param>
        /// <returns>�����ɹ����� 1 ���򷵻� 0</returns>	
        public static int CreateFavorites(int uid, int tid, FavoriteType type)
        {
            return Discuz.Data.Favorites.CreateFavorites(uid, tid, (byte)type);
        }
	
		/// <summary>
		/// ɾ��ָ���û����ղ���Ϣ
		/// </summary>
		/// <param name="uid">�û�id</param>
		/// <param name="fitemid">Ҫɾ�����ղ���Ϣid�б�</param>
        /// <param name="type">�ղ�����</param>
		/// <returns>ɾ��������������ʱ���� -1</returns>
        public static int DeleteFavorites(int uid, string[] fitemid, FavoriteType type)
		{
			foreach (string id in fitemid)
			{
				if (!Utils.IsNumeric(id))
					return -1;
			}

            return Discuz.Data.Favorites.DeleteFavorites(uid, String.Join(",",fitemid), (byte)type);
		}

		       
		/// <summary>
		/// �õ��û��ղ���Ϣ�б�
		/// </summary>
		/// <param name="uid">�û�id</param>
		/// <param name="pagesize">��ҳʱÿҳ�ļ�¼��</param>
		/// <param name="pageindex">��ǰҳ��</param>
		/// <param name="type">�ղ�����id</param>
		/// <returns>�û���Ϣ�б�</returns>
        public static DataTable GetFavoritesList(int uid, int pagesize, int pageindex, FavoriteType type)
        {
            return Discuz.Data.Favorites.GetFavoritesList(uid, pagesize, pageindex, (int)type);
        }


        /// <summary>
        /// �õ��û����������ղص�����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>�ղ�����</returns>
        public static int GetFavoritesCount(int uid, FavoriteType type)
        {
            return uid > 0 ? Discuz.Data.Favorites.GetFavoritesCount(uid, (int)type) : 0;
        }


		/// <summary>
		/// �ղؼ����Ƿ������ָ������
		/// </summary>
		/// <param name="uid">�û�id</param>
		/// <param name="tid">��Id</param>
        /// <param name="type">����: ���, ��־, ����</param>
		/// <returns></returns>
		public static int CheckFavoritesIsIN(int uid,int tid, FavoriteType type)
		{
            return Discuz.Data.Favorites.CheckFavoritesIsIN(uid, tid, (byte)type);	
        }

        /// <summary>
        /// �����û��ղ���Ŀ�Ĳ鿴ʱ��
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="tid">����id</param>
        /// <returns></returns>
        public static int UpdateUserFavoriteViewTime(int uid, int tid)
        {
            return Discuz.Data.Favorites.UpdateUserFavoriteViewTime(uid, tid);
        }
	}//class end
}
