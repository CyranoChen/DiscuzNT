using System;
using System.Data;
using System.Text;

using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Cache.Data
{
    public interface ICacheAttachments
    {
        /// <summary>
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类</param>
        /// <returns>附件id</returns>
        int CreateAttachments(AttachmentInfo attachmentinfo);

        /// <summary>
        /// 获得指定附件的描述信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns>描述信息</returns>
        AttachmentInfo GetAttachmentInfo(int aid);
      
        /// <summary>
        /// 获得指定帖子的附件个数
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>附件个数</returns>
        int GetAttachmentCountByPid(int pid);

        /// <summary>
        /// 获得指定主题的附件个数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>附件个数</returns>
        int GetAttachmentCountByTid(int tid);

        /// <summary>
        /// 获取指定PID列表的相应附件信息列表
        /// </summary>
        /// <param name="pidList">指定PID列表</param>
        /// <returns>相应附件信息列表</returns>
        List<ShowtopicPageAttachmentInfo> GetAttachmentListByPidList(string pidList);

        /// <summary>
        /// 更新附件下载次数
        /// </summary>
        /// <param name="aid">附件id</param>
        void UpdateAttachmentDownloads(int aid);

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题tid</param>
        /// <returns>删除个数</returns>
        void DeleteAttachmentByTid(int tid);

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">版块tid列表</param>
        /// <returns>删除个数</returns>
        void DeleteAttachmentByTid(string tidlist);

        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aid">附件aid</param>
        /// <returns>删除个数</returns>
        void DeleteAttachment(int aid);

        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="attachmentInfo">附件对象</param>
        /// <returns>返回被更新的数量</returns>
        void UpdateAttachment(AttachmentInfo attachmentInfo);

        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="aidList">附件Id，以英文逗号分割</param>
        /// <returns>返回被删除的个数</returns>
        void DeleteAttachment(string aidList);
        
        /// <summary>
        /// 取得主题帖的第一个图片附件
        /// </summary>
        /// <param name="tid">主题id</param>
        AttachmentInfo GetFirstImageAttachByTid(int tid);

        /// <summary>
        /// 根据帖子ID删除附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        void DeleteAttachmentByPid(int pid);

        /// <summary>
        /// 获取指定用户未使用的附件的JSON字符串
        /// </summary>
        /// <param name="userid">指定用户id</param>
        /// <param name="posttime">获取指定时间后的新附件,空为不限时间</param>
        /// <param name="attachmentType">附件的类型</param>
        /// <returns>JSON字符串</returns>
        Discuz.Common.Generic.List<AttachmentInfo> GetNoUsedAttachmentJson(int userid, string posttime, int isimage);

        /// <summary>
        /// 删除未被使用的论坛附件
        /// </summary>
        void DeleteNoUsedForumAttachment();

        /// <summary>
        /// 删除指定用户的附件
        /// </summary>
        /// <param name="uid">用户ID</param>
        void DeleteAttachmentByUid(int uid);
    }
}