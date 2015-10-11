using System;
using System.Data;

using Discuz.Entity;

namespace Discuz.Data
{
    /// <summary>
    /// 广告数据操作类
    /// </summary>
    public class Advertisenments
    {
        /// <summary>
        /// 得到广告列表
        /// </summary>
        /// <returns>广告列表</returns>
        public static DataTable GetAdsTable()
        {
            return DatabaseProvider.GetInstance().GetAdsTable();
        }

        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="available">是否生效</param>
        /// <param name="type">广告类型</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="title">广告标题</param>
        /// <param name="targets">广告投放范围</param>
        /// <param name="parameters">展现方式</param>
        /// <param name="code">广告内容</param>
        /// <param name="startTime">生效时间</param>
        /// <param name="endTime">结束时间</param>
        public static void CreateAd(int available, string type, int displayorder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            DatabaseProvider.GetInstance().AddAdInfo(available, type, displayorder, title, targets, parameters, code, startTime, endTime);
        }

        /// <summary>
        /// 获取全部广告列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdvertisements(int type)
        {
            return DatabaseProvider.GetInstance().GetAdvertisements(type);
        }

        public static DataTable GetAdvertisements()
        {
            return DatabaseProvider.GetInstance().GetAdvertisements();
        }

        /// <summary>
        /// 删除广告列表            
        /// </summary>
        /// <param name="advIdList">广告列表Id</param>
        public static void DeleteAdvertisementList(string advIdList)
        {
            DatabaseProvider.GetInstance().DeleteAdvertisement(advIdList);
        }

        /// <summary>
        /// 更新广告可用状态
        /// </summary>
        /// <param name="aidList">广告Id</param>
        /// <param name="available"></param>
        /// <returns></returns>
        public static int UpdateAdvertisementAvailable(string aidList, int available)
        {
            return DatabaseProvider.GetInstance().UpdateAdvertisementAvailable(aidList,available);
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="adId">广告Id</param>
        /// <param name="available">是否生效</param>
        /// <param name="type">广告类型</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="title">广告标题</param>
        /// <param name="targets">广告投放范围</param>
        /// <param name="parameters">展现方式</param>
        /// <param name="code">广告内容</param>
        /// <param name="startTime">生效时间</param>
        /// <param name="endTime">结束时间</param>
        public static void UpdateAdvertisement(int adId, int available, string type, int displayorder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            DatabaseProvider.GetInstance().UpdateAdvertisement(adId, available, type, displayorder, title, targets, parameters, code, startTime, endTime);
        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="aid">广告Id</param>
        /// <returns></returns>
        public static DataTable GetAdvertisement(int aid)
        {
            return DatabaseProvider.GetInstance().GetAdvertisement(aid);
        }
    }
}
