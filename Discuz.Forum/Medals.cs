using System.Data;

using Discuz.Common;

namespace Discuz.Forum
{
    public class Medals
    {
        /// <summary>
        /// 返回勋章列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMedal()
        {
            return Discuz.Data.Medals.GetMedal();
        }
        
        /// <summary>
        /// 返回指定勋章
        /// </summary>
        /// <param name="medalId">勋章Id</param>
        /// <returns></returns>
        public static DataTable GetMedal(int medalId)
        {
            DataTable dt = GetMedal().Clone();
            foreach (DataRow dr in GetMedal().Rows)
            {
                if (dr["medalid"].ToString() == medalId.ToString())
                    dt.ImportRow(dr);
            }
            return dt;
        }

        /// <summary>
        /// 获取可用勋章列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAvailableMedal()
        {
            DataTable medal = GetMedal();
            DataTable availableMedal = medal.Clone();
            foreach (DataRow dr in medal.Select("available=1"))
            {
                availableMedal.ImportRow(dr);
            }
            return availableMedal;
        }

        /// <summary>
        /// 创建勋章
        /// </summary>
        /// <param name="medalName">勋章名称</param>
        /// <param name="available">是否可用</param>
        /// <param name="image">图片名称</param>
        public static void CreateMedal(string medalName, int available,string image)
        {
            Discuz.Data.Medals.CreateMedal(medalName, available, image);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/MedalsList");
        }

        /// <summary>
        /// 更新勋章
        /// </summary>
        /// <param name="medalid">勋章ID</param>
        /// <param name="name">名称</param>
        /// <param name="image">图片</param>
        public static void UpdateMedal(int medalid, string name, string image)
        {
            if(medalid > 0)
                Data.Medals.UpdateMedal(medalid, name, image);
        }

        /// <summary>
        /// 设置勋章为可用
        /// </summary>
        /// <param name="available"></param>
        /// <param name="medailidlist"></param>
        public static void SetAvailableForMedal(int available, string medailIdList)
        {
            if (Utils.IsNumericList(medailIdList))
            {
                Data.Medals.SetAvailableForMedal(available, medailIdList);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/MedalsList");
            }
        }

        /// <summary>
        /// 获取存在的勋章
        /// </summary>
        /// <returns></returns>
        public static void UpdateMedalList(System.Collections.ArrayList medalFiles)
        {
            medalFiles.Remove("thumbs.db");
            DataTable dt = Data.Medals.GetExistMedalList();
            foreach (DataRow dr in dt.Rows)
            {
                medalFiles.Remove(dr["image"].ToString().ToLower()); //移除已经存在于勋章库中的勋章文件名
            }
            int newMedalBaseId = TypeConverter.ObjectToInt(dt.Rows[dt.Rows.Count - 1]["medalid"]) + 1; //获取新的Medalid的基数
            for (int i = 0; i < medalFiles.Count; i++) //将未入库的勋章入库
            {
                int newMedalId = newMedalBaseId + i;
                Medals.UpdateMedal(newMedalId, "Medal No." + newMedalId, medalFiles[i].ToString());
            }
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/MedalsList");
        }
    }
}
