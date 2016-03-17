using System;
using System.Text;
using System.Data;
using System.IO;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Cache.Data;

namespace Discuz.Data
{
    /// <summary>
    /// 附件数据操作类
    /// </summary>
    public class Attachments
    {
        /// <summary>
        /// 是否启用TokyoTyrantCache缓存用户表
        /// </summary>
        public static bool appDBCache = (EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Cacheattachments.Enable);

        public static ICacheAttachments IAttachmentService = appDBCache ? DBCacheService.GetAttachmentsService() : null;



        /// <summary>
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类</param>
        /// <returns>附件id</returns>
        public static int CreateAttachments(AttachmentInfo attachmentinfo)
        {
            attachmentinfo.Aid = DatabaseProvider.GetInstance().CreateAttachment(attachmentinfo);

            if (appDBCache)
                IAttachmentService.CreateAttachments(attachmentinfo);

            return attachmentinfo.Aid;
        }

        /// <summary>
        /// 获得指定附件的描述信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns>描述信息</returns>
        public static AttachmentInfo GetAttachmentInfo(int aid)
        {
            if (appDBCache)
            {
                AttachmentInfo attachmentInfo = IAttachmentService.GetAttachmentInfo(aid);
                if (attachmentInfo != null)
                    return attachmentInfo;
            }

            return LoadSingleAttachmentInfo(DatabaseProvider.GetInstance().GetAttachmentInfo(aid), true);
        }

        /// <summary>
        /// 将单个附件DataRow转换为AttachmentInfo类
        /// </summary>
        /// <param name="drAttach">单个附件DataRow</param>
        /// <param name="drAttach">是否返回原始路径</param>
        /// <returns>AttachmentInfo类</returns>
        private static AttachmentInfo LoadSingleAttachmentInfo(IDataReader drAttach, bool isOriginalFilename)
        {
            AttachmentInfo attach = new AttachmentInfo();
            if (drAttach.Read())
            {
                attach.Aid = TypeConverter.ObjectToInt(drAttach["aid"]);
                attach.Uid = TypeConverter.ObjectToInt(drAttach["uid"]);
                attach.Tid = TypeConverter.ObjectToInt(drAttach["tid"]);
                attach.Pid = TypeConverter.ObjectToInt(drAttach["pid"]);
                attach.Postdatetime = drAttach["postdatetime"].ToString();
                attach.Readperm = TypeConverter.ObjectToInt(drAttach["readperm"]);

                if (isOriginalFilename)
                {
                    attach.Filename = drAttach["filename"].ToString();
                }
                else if (drAttach["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                {
                    attach.Filename = BaseConfigs.GetForumPath + "upload/" + drAttach["filename"].ToString().Trim().Replace("\\", "/");
                }
                else
                {
                    attach.Filename = drAttach["filename"].ToString().Trim().Replace("\\", "/");
                }
                attach.Description = drAttach["description"].ToString().Trim();
                attach.Filetype = drAttach["filetype"].ToString().Trim();
                attach.Attachment = drAttach["attachment"].ToString().Trim();
                attach.Filesize = TypeConverter.ObjectToInt(drAttach["filesize"]);
                attach.Downloads = TypeConverter.ObjectToInt(drAttach["downloads"]);
                attach.Attachprice = TypeConverter.ObjectToInt(drAttach["attachprice"], 0);
                attach.Height = TypeConverter.ObjectToInt(drAttach["height"]);
                attach.Width = TypeConverter.ObjectToInt(drAttach["width"], 0);
                attach.Isimage = TypeConverter.ObjectToInt(drAttach["isimage"], 0);
            }
            drAttach.Close();
            return attach;
        }

        /// <summary>
        /// 获得指定帖子的附件个数
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>附件个数</returns>
        public static int GetAttachmentCountByPid(int pid)
        {
            if (appDBCache)
                return IAttachmentService.GetAttachmentCountByPid(pid);

            return DatabaseProvider.GetInstance().GetAttachmentCountByPid(pid);
        }

        /// <summary>
        /// 获得指定主题的附件个数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>附件个数</returns>
        public static int GetAttachmentCountByTid(int tid)
        {
            if (appDBCache)
                return IAttachmentService.GetAttachmentCountByTid(tid);

            return DatabaseProvider.GetInstance().GetAttachmentCountByTid(tid);
        }

        /// <summary>
        /// 获得指定帖子的附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>帖子信息</returns>
        public static DataTable GetAttachmentListByPid(int pid)
        {
            return DatabaseProvider.GetInstance().GetAttachmentListByPid(pid);
        }

        public static List<ShowtopicPageAttachmentInfo> GetAttachmentListByPidList(string pidList)
        {
            List<ShowtopicPageAttachmentInfo> list = new List<ShowtopicPageAttachmentInfo>();

            if (appDBCache)
            {
                list = IAttachmentService.GetAttachmentListByPidList(pidList);
                if (list.Count > 0)
                    return list;
            }
           
            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentListByPid(pidList.ToString());
            while (reader.Read())
            {
                list.Add(LoadSingleAttachmentInfo(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 获得已购买的附件
        /// </summary>
        /// <param name="attachidList">当前全部附件id list</param>
        /// <param name="userid">当前用户id</param>
        /// <returns></returns>
        public static string GetPurchasedAttachmentIdList(string attachidList, int userId)
        {
            if (!Utils.IsNumericList(attachidList))
                return string.Empty;

            //获取当前用户所购买的指定附件列表
            IDataReader reader = DatabaseProvider.GetInstance().GetAttachPaymentLogByUid(attachidList, userId);
            attachidList = ",";
            while (reader.Read())
            {
                attachidList += reader["aid"].ToString() + ",";
            }
            reader.Close();
            return attachidList.Remove(0, 1);
        }

        /// <summary>
		/// 将系统设置的附件类型以DataTable的方式存入缓存
		/// </summary>
		/// <returns>系统设置的附件类型</returns>
        public static DataTable GetAttachmentType()
        {
            return DatabaseProvider.GetInstance().GetAttachmentType();
        }

        /// <summary>
        /// 更新附件下载次数
        /// </summary>
        /// <param name="aid">附件id</param>
        public static void UpdateAttachmentDownloads(int aid)
        {
            if (appDBCache)
                IAttachmentService.UpdateAttachmentDownloads(aid);

            DatabaseProvider.GetInstance().UpdateAttachmentDownloads(aid);
        }

        /// <summary>
        /// 更新主题中的附件标志
        /// </summary>
        /// <param name="tid">主题id</param>
        public static void UpdateTopicAttachment(int tid)
        {
            int attachmentCount = 0;
            if (appDBCache)
                attachmentCount = IAttachmentService.GetAttachmentCountByTid(tid);                
            else
                attachmentCount =  DatabaseProvider.GetInstance().GetAttachmentCountByTid(tid);

            if (Topics.appDBCache)
            {
                TopicInfo topicInfo = Topics.GetTopicInfo(tid, 0, 0);
                topicInfo.Attachment = attachmentCount;
                Topics.ITopicService.UpdateTopic(topicInfo);
            }
            DatabaseProvider.GetInstance().UpdateTopicAttachment(tid, attachmentCount > 0 ? 1 : 0);
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题tid</param>
        /// <returns>删除个数</returns>
        public static int DeleteAttachmentByTid(int tid)
        {
            if (appDBCache)
                IAttachmentService.DeleteAttachmentByTid(tid);         

            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentListByTid(tid);
            if (reader != null)
            {
                while (reader.Read())
                {
                    DeleteFile(reader["filename"].ToString());
                    new FTPs().DeleFtpFile(reader["filename"].ToString());
                }
                reader.Close();
            }

            DatabaseProvider.GetInstance().DelMyAttachmentByTid(tid.ToString());
            return DatabaseProvider.GetInstance().DeleteAttachmentByTid(tid);
        }

        /// <summary>
        /// 删除指定用户的附件
        /// </summary>
        /// <param name="uid">用户ID</param>
        public static void DeleteAttachmentByUid(int uid)
        {
            DeleteAttachmentByUid(uid, 0);
        }

        public static void DeleteAttachmentByUid(int uid, int days)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentListByUid(uid, days);
            if (reader != null)
            {
                while (reader.Read())
                {
                    DeleteFile(reader["filename"].ToString());
                    new FTPs().DeleFtpFile(reader["filename"].ToString());
                }
                reader.Close();
            }
            DatabaseProvider.GetInstance().DeleteAttachmentByUid(uid, days);

            if (appDBCache)
                IAttachmentService.DeleteAttachmentByUid(uid);
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">版块tid列表</param>
        /// <returns>删除个数</returns>
        public static int DeleteAttachmentByTid(string tidlist)
        {
            if (appDBCache)
                IAttachmentService.DeleteAttachmentByTid(tidlist);      

            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentListByTid(tidlist);

            if (reader != null)
            {
                while (reader.Read())
                {
                    DeleteFile(reader["filename"].ToString());
                    new FTPs().DeleFtpFile(reader["filename"].ToString());
                }
                reader.Close();
            }

            DatabaseProvider.GetInstance().DelMyAttachmentByTid(tidlist);
            return DatabaseProvider.GetInstance().DeleteAttachmentByTid(tidlist);
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="fileName">要删除的文件名（含路径）</param>
        private static void DeleteFile(string fileName)
        {
            if (fileName.Trim().ToLower().IndexOf("http") < 0)
            {
                try
                {
                    File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + fileName.Trim()));
                }
                catch { }
            }
        }

        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aid">附件aid</param>
        /// <returns>删除个数</returns>
        public static int DeleteAttachment(int aid)
        {
            if (appDBCache)
                IAttachmentService.DeleteAttachment(aid);      

            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentInfo(aid);
            int tid = 0;
            int pid = 0;
            if (reader != null)
            {
                if (reader.Read())
                {
                    DeleteFile(reader["filename"].ToString());
                    new FTPs().DeleFtpFile(reader["filename"].ToString());
                    tid = TypeConverter.ObjectToInt(reader["tid"], 0);
                    pid = TypeConverter.ObjectToInt(reader["pid"], 0);
                }
                reader.Close();
            }

            int reval = DatabaseProvider.GetInstance().DeleteAttachment(aid);
            DeleteAttachment(aid.ToString(), pid, tid);

            return reval;
        }

        /// <summary>
        /// 删除指定附件id的附件同时更新主题和帖子中的附件个数
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <param name="pid">附件所属帖子id</param>
        /// <param name="tid">附件所属主题id</param>
        private static void DeleteAttachment(string aidlist, int pid, int tid)
        {
            if (appDBCache)
                IAttachmentService.DeleteAttachment(aidlist);      

            DatabaseProvider.GetInstance().DelMyAttachmentByAid(aidlist);
            if (tid > 0)
            {
                if (DatabaseProvider.GetInstance().GetAttachmentCountByPid(pid) <= 0)
                {
                    string postTableId = PostTables.GetPostTableId(tid);
                    DatabaseProvider.GetInstance().UpdatePostAttachment(pid, postTableId, 0);

                    if (Posts.appDBCache)
                    {
                        PostInfo postInfo = Posts.GetPostInfo(postTableId, pid);
                        postInfo.Attachment = 0;
                        Posts.IPostService.UpdatePost(postInfo, postTableId);
                    }
                }

                if (DatabaseProvider.GetInstance().GetAttachmentCountByTid(tid) <= 0)
                {
                    DatabaseProvider.GetInstance().UpdateTopicAttachment(tid, 0);

                    if (Topics.appDBCache)
                    {
                        TopicInfo topicInfo = Topics.GetTopicInfo(tid, 0, 0);
                        topicInfo.Attachment = 0;
                        Topics.ITopicService.UpdateTopic(topicInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="attachmentInfo">附件对象</param>
        /// <returns>返回被更新的数量</returns>
        public static int UpdateAttachment(AttachmentInfo attachmentInfo)
        {
            if (appDBCache)
                IAttachmentService.UpdateAttachment(attachmentInfo);      

            return DatabaseProvider.GetInstance().UpdateAttachment(attachmentInfo);
        }


        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="aidList">附件Id，以英文逗号分割</param>
        /// <returns>返回被删除的个数</returns>
        public static int DeleteAttachment(string aidList)
        {
            if (appDBCache)
                IAttachmentService.DeleteAttachment(aidList);      

            IDataReader reader = DatabaseProvider.GetInstance().GetAttachmentList(aidList);
            int tid = 0;
            int pid = 0;
            while (reader.Read())
            {
                if (reader["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                {
                    string attachmentFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + reader["filename"].ToString());
                    if (Utils.FileExists(attachmentFilePath))
                    {
                        try
                        {
                            FileInfo fileInfo = new FileInfo(attachmentFilePath);
                            fileInfo.Attributes = FileAttributes.Normal;
                            fileInfo.Delete();
                        }
                        catch { }
                    }
                }
                else if (FTPConfigs.GetForumAttachInfo.Reserveremoteattach == 0) //删除远程附件
                    new FTPs().DeleFtpFile(reader["filename"].ToString());

                tid = TypeConverter.ObjectToInt(reader["tid"], 0);
                pid = TypeConverter.ObjectToInt(reader["pid"], 0);
            }
            reader.Close();

            int reval = DatabaseProvider.GetInstance().DeleteAttachment(aidList);
            DeleteAttachment(aidList, pid, tid);

            return reval;
        }

        /// <summary>
        /// 获得上传附件文件的大小
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetUploadFileSizeByUserId(int uid)
        {
            return DatabaseProvider.GetInstance().GetUploadFileSizeByUserId(uid);
        }

        /// <summary>
        /// 取得主题帖的第一个图片附件
        /// </summary>
        /// <param name="tid">主题id</param>
        public static AttachmentInfo GetFirstImageAttachByTid(int tid)
        {
            if (appDBCache)
                return IAttachmentService.GetFirstImageAttachByTid(tid);      

            return LoadSingleAttachmentInfo(DatabaseProvider.GetInstance().GetFirstImageAttachByTid(tid),false);
        }


        /// <summary>
        /// 根据帖子ID删除附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        public static void DeleteAttachmentByPid(int pid)
        {
            if (appDBCache)
                IAttachmentService.DeleteAttachmentByPid(pid);      

            DatabaseProvider.GetInstance().DelMyAttachmentByPid(pid.ToString());
            DatabaseProvider.GetInstance().DeleteAttachmentByPid(pid);
        }

        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>附件数量</returns>
        public static int GetUserAttachmentCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetUserAttachmentCount(uid);
        }

        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="typeid">附件类型id</param>
        /// <returns>附件数量</returns>
        public static int GetUserAttachmentCount(int uid, string extNamelist)
        {
            return DatabaseProvider.GetInstance().GetUserAttachmentCount(uid, extNamelist);
        }


        /// <summary>
        /// 获取用户附件列表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="typeid">附件类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>用户附件列表</returns>
        public static List<MyAttachmentInfo> GetAttachmentByUid(int uid, int typeid, int pageIndex, int pageSize, string extNamelist)
        {
            List<MyAttachmentInfo> myattachment = new List<MyAttachmentInfo>();

            IDataReader reader;
            if (typeid == 0)
                reader = DatabaseProvider.GetInstance().GetAttachmentByUid(uid, pageIndex, pageSize);
            else
                reader = DatabaseProvider.GetInstance().GetAttachmentByUid(uid, extNamelist, pageIndex, pageSize);

            while (reader.Read())
            {
                MyAttachmentInfo info = new MyAttachmentInfo();

                info.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                info.Aid = TypeConverter.ObjectToInt(reader["aid"]);
                info.Postdatetime = reader["postdatetime"].ToString();
                info.Filename = reader["filename"].ToString().StartsWith("http://") ? reader["filename"].ToString() : "upload/" + reader["filename"].ToString().Replace("\\", "/");
                info.Description = reader["description"].ToString();
                info.Attachment = reader["attachment"].ToString();
                info.SimpleName = Utils.ConvertSimpleFileName(reader["attachment"].ToString(), "...", 6, 3, 15);
                info.Downloads = TypeConverter.ObjectToInt(reader["downloads"]);

                myattachment.Add(info);
            }
            reader.Close();
            return myattachment;
        }

        //public static List<AttachmentInfo> GetAttachmentListByUid(int uid)
        //{
        //    List<AttachmentInfo> attachmentlist = new List<AttachmentInfo>();

        //    IDataReader reader = DatabaseProvider.GetInstance().GetUnusedAttachmentListByUid(uid);
             
        //    while (reader.Read())
        //    {
        //        AttachmentInfo info = new AttachmentInfo();

        //        info.Attachment = reader["attachment"].ToString().Trim();
        //        info.Aid = TypeConverter.ObjectToInt(reader["aid"]);
        //        info.Filetype = reader["filetype"].ToString();
        //        info.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
        //        info.Attachprice = TypeConverter.ObjectToInt(reader["attachprice"]);
        //        info.Width = TypeConverter.ObjectToInt(reader["width"]);
        //        info.Height = TypeConverter.ObjectToInt(reader["height"]);
        //        attachmentlist.Add(info);
        //    }
        //    reader.Close();
        //    return attachmentlist;
        //}

        /// <summary>
        /// 获取附件类型列表
        /// </summary>
        /// <returns>附件类型列表</returns>
        public static List<AttachmentType> AttachTypeList()
        {
            List<AttachmentType> list = new List<AttachmentType>();

            foreach (AttachmentType act in MyAttachmentsTypeConfigs.GetConfig().AttachmentType)
            {
                AttachmentType MyAttachmentType = new AttachmentType();
                MyAttachmentType.TypeId = act.TypeId;
                MyAttachmentType.TypeName = act.TypeName;
                MyAttachmentType.ExtName = act.ExtName;
                list.Add(MyAttachmentType);
            }

            return list;
        }


        /// <summary>
        /// 获取当前编辑帖子的附件id列表的附件信息列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="aidList"></param>
        /// <returns></returns>
        public static List<AttachmentInfo> GetEditPostAttachList(int userid, string aidList)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetEditPostAttachList(userid,aidList);
            List<AttachmentInfo> attachmentList = new List<AttachmentInfo>();
            while (reader.Read())
            {
                AttachmentInfo attach = new AttachmentInfo();
                attach.Aid = TypeConverter.ObjectToInt(reader["aid"]);
                attach.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                attach.Tid = TypeConverter.ObjectToInt(reader["tid"]);
                attach.Pid = TypeConverter.ObjectToInt(reader["pid"]);
                attach.Postdatetime = reader["postdatetime"].ToString();
                attach.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
                attach.Filename = reader["filename"].ToString();
                attach.Description = reader["description"].ToString().Trim();
                attach.Filetype = reader["filetype"].ToString().Trim();
                attach.Attachment = reader["attachment"].ToString().Trim();
                attach.Filesize = TypeConverter.ObjectToInt(reader["filesize"]);
                attach.Downloads = TypeConverter.ObjectToInt(reader["downloads"]);
                attach.Attachprice = TypeConverter.ObjectToInt(reader["attachprice"], 0);
                attach.Height = TypeConverter.ObjectToInt(reader["height"]);
                attach.Width = TypeConverter.ObjectToInt(reader["width"], 0);
                attach.Isimage = TypeConverter.ObjectToInt(reader["isimage"], 0);
                attachmentList.Add(attach);
            }
            reader.Close();
            return attachmentList;
        }
        
        /// <summary>
        /// 获取指定用户未使用的附件的附件列表
        /// </summary>
        /// <param name="userid">指定用户id</param>
        /// <param name="posttime">获取指定时间后的新附件,空为不限时间</param>
        /// <param name="attachmentType">附件的类型</param>
        /// <returns>AttachmentInfo列表</returns>
        public static List<AttachmentInfo> GetNoUsedAttachmentList(int userid, string posttime, AttachmentFileType attachmentType)
        {
            if (appDBCache)
            {
                if (attachmentType == AttachmentFileType.All)
                    return IAttachmentService.GetNoUsedAttachmentJson(userid, posttime, 2);
                else if (attachmentType == AttachmentFileType.FileAttachment)
                    return IAttachmentService.GetNoUsedAttachmentJson(userid, posttime, 0);
                else
                    return IAttachmentService.GetNoUsedAttachmentJson(userid, posttime, 1);
            }

            IDataReader reader;
            if (attachmentType == AttachmentFileType.All)
                reader = DatabaseProvider.GetInstance().GetNoUsedAttachmentListByUid(userid, posttime, 2);
            else if (attachmentType == AttachmentFileType.FileAttachment)
                reader = DatabaseProvider.GetInstance().GetNoUsedAttachmentListByUid(userid, posttime, 0);
            else
                reader = DatabaseProvider.GetInstance().GetNoUsedAttachmentListByUid(userid, posttime, 1);
            List<AttachmentInfo> attachmentList = new List<AttachmentInfo>();
            while (reader.Read())
            {
                AttachmentInfo attach = LoadAttachmentInfo(reader);
                attachmentList.Add(attach);
            }
            reader.Close();
            return attachmentList;
        }

        /// <summary>
        /// 获取指定用户未使用的附件
        /// </summary>
        /// <param name="userid">指定用户id</param>
        /// <param name="count">获取指定时间后的新附件,空为不限时间</param>
        /// <returns>AttachmentInfo列表</returns>
        public static List<AttachmentInfo> GetNoUsedAttachmentList(int userid, string posttime)
        {
            return GetNoUsedAttachmentList(userid, posttime, AttachmentFileType.All);
        }

        private static AttachmentInfo LoadAttachmentInfo(IDataReader reader)
        {
            AttachmentInfo attach = new AttachmentInfo();
            attach.Aid = TypeConverter.ObjectToInt(reader["aid"]);
            attach.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            attach.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            attach.Pid = TypeConverter.ObjectToInt(reader["pid"]);
            attach.Postdatetime = reader["postdatetime"].ToString();
            attach.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
            attach.Filename = reader["filename"].ToString();
            attach.Description = reader["description"].ToString().Trim();
            attach.Filetype = reader["filetype"].ToString().Trim();
            attach.Attachment = reader["attachment"].ToString().Trim();
            attach.Filesize = TypeConverter.ObjectToInt(reader["filesize"]);
            attach.Downloads = TypeConverter.ObjectToInt(reader["downloads"]);
            attach.Attachprice = TypeConverter.ObjectToInt(reader["attachprice"], 0);
            attach.Height = TypeConverter.ObjectToInt(reader["height"]);
            attach.Width = TypeConverter.ObjectToInt(reader["width"], 0);
            attach.Isimage = TypeConverter.ObjectToInt(reader["isimage"], 0);
            return attach;
        }


        ///// <summary>
        ///// 获取指定用户未使用的附件的JSON字符串
        ///// </summary>
        ///// <param name="userid">指定用户id</param>
        ///// <returns>JSON字符串</returns>
        //public static string GetNoUsedAttachmentJson(int userid)
        //{
        //    if (appDBCache)
        //        return IAttachmentService.GetNoUsedAttachmentJson(userid);      

        //    StringBuilder attachmentStringBuilder = new StringBuilder();
        //    attachmentStringBuilder.Append("[");
        //    IDataReader reader = DatabaseProvider.GetInstance().GetNoUsedAttachmentListByUid(userid);

        //    if (reader != null)
        //    {
        //        while (reader.Read())
        //        {
        //            if (!Utils.StrIsNullOrEmpty(reader["aid"].ToString()))
        //            {
        //                attachmentStringBuilder.Append(string.Format("{{'aid' : {0}, 'attachment' : '{1}'}},",
        //                    reader["aid"].ToString().Trim(),
        //                    reader["attachment"].ToString().Trim()
        //                   ));
        //            }
        //        }
        //        reader.Close();
        //    }
        //    if (attachmentStringBuilder.ToString().EndsWith(","))
        //    {
        //        attachmentStringBuilder.Remove(attachmentStringBuilder.Length - 1, 1);
        //    }
        //    attachmentStringBuilder.Append("]");

        //    return attachmentStringBuilder.ToString();
        //}

        /// <summary>
        /// 删除未被使用的论坛附件
        /// </summary>
        public static void DeleteNoUsedForumAttachment()
        {
            if (appDBCache)
                IAttachmentService.DeleteNoUsedForumAttachment();      

            IDataReader reader = DatabaseProvider.GetInstance().GetNoUsedForumAttachment();
            if (reader != null)
            {
                string UploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/");
                while (reader.Read())
                {
                    if (!Utils.StrIsNullOrEmpty(reader["filename"].ToString().Trim()))
                    {
                        //如果物理文件存在,则删除
                        if (Utils.FileExists(UploadDir + reader["filename"].ToString().Trim()))
                        {
                            try
                            {
                                File.Delete(UploadDir + reader["filename"].ToString().Trim());
                            }
                            finally
                            { ; }
                        }
                    }
                    if (reader["filename"].ToString().Trim().ToLower().IndexOf("http") >= 0 && FTPConfigs.GetForumAttachInfo.Reserveremoteattach == 0) //删除远程附件
                        new FTPs().DeleFtpFile(reader["filename"].ToString());                    
                }
                reader.Close();
            }
            DatabaseProvider.GetInstance().DeleteNoUsedForumAttachment();
        }


        /// <summary>
        /// 加载单个附件实体对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ShowtopicPageAttachmentInfo LoadSingleAttachmentInfo(IDataReader reader)
        {
            ShowtopicPageAttachmentInfo attInfo = new ShowtopicPageAttachmentInfo();
            attInfo.Aid = TypeConverter.ObjectToInt(reader["aid"]);
            attInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            attInfo.Pid = TypeConverter.ObjectToInt(reader["pid"]);
            attInfo.Postdatetime = reader["postdatetime"].ToString();
            attInfo.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
            attInfo.Filename = reader["filename"].ToString();
            attInfo.Description = reader["description"].ToString();
            attInfo.Filetype = reader["filetype"].ToString();
            attInfo.Filesize = TypeConverter.ObjectToInt(reader["filesize"]);
            attInfo.Attachment = reader["attachment"].ToString();
            attInfo.Downloads = TypeConverter.ObjectToInt(reader["downloads"]);
            attInfo.Attachprice = TypeConverter.ObjectToInt(reader["attachprice"]);
            attInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            attInfo.Width = TypeConverter.ObjectToInt(reader["width"]);
            attInfo.Height = TypeConverter.ObjectToInt(reader["height"]);
            return attInfo;
        }


        /// <summary>
        /// 删除附件类型
        /// </summary>
        /// <param name="attchtypeidlist">附件类型Id列表</param>
        public static void DeleteAttchType(string idList)
        {
            DatabaseProvider.GetInstance().DeleteAttchType(idList);
        }

        /// <summary>
        /// 添加附件类型
        /// </summary>
        /// <param name="extension">附件类型扩展名</param>
        /// <param name="maxsize"></param>
        public static void AddAttchType(string extension, string maxSize)
        {
            DatabaseProvider.GetInstance().AddAttchType(extension, maxSize);
        }

        /// <summary>
        /// 更新允许的附件类型
        /// </summary>
        /// <param name="extension">附件类型扩展名</param>
        /// <param name="maxsize">大小</param>
        /// <param name="id">附件类型ID</param>
        public static void UpdateAttchType(string extension, string maxSize, int id)
        {
            DatabaseProvider.GetInstance().UpdateAttchType(extension, maxSize, id);
        }

        /// <summary>
        /// 按条件获取特定分表中的附件列表
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="postName">分表名称</param>
        /// <returns></returns>
        public static DataTable GetAttachList(string condition, string postName)
        {
            return DatabaseProvider.GetInstance().GetAttachDataTable(condition, postName);
        }

        /// <summary>
        /// 生成搜索附件的条件
        /// </summary>
        /// <param name="forumid">板块ID</param>
        /// <param name="posttablename">分表名称</param>
        /// <param name="filesizemin">最小</param>
        /// <param name="filesizemax">最大</param>
        /// <param name="downloadsmin"></param>
        /// <param name="downloadsmax"></param>
        /// <param name="postdatetime">提交时间</param>
        /// <param name="filename"></param>
        /// <param name="description"></param>
        /// <param name="poster"></param>
        /// <returns></returns>
        public static string SearchAttachment(int forumid, string posttablename, string filesizemin, string filesizemax,
                                string downloadsmin, string downloadsmax, string postdatetime, string filename,
                                string description, string poster)
        {
            return DatabaseProvider.GetInstance().SearchAttachment(forumid, posttablename, filesizemin, filesizemax, downloadsmin, downloadsmax, postdatetime, filename, description, poster);
        }

        #region ftp类文件
        /// <summary>
        /// FTP操作类
        /// </summary>
        public class FTPs
        {
            #region 声明上传信息(静态)对象

            private static FTPConfigInfo m_forumattach = null;        

            #endregion

            private static string m_configfilepath = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/ftp.config");

            /// <summary>
            /// 程序刚加载时ftp.config文件修改时间
            /// </summary>
            private static DateTime m_fileoldchange;
            /// <summary>
            /// 最近ftp.config文件修改时间
            /// </summary>
            private static DateTime m_filenewchange;

            private static object lockhelper = new object();

        
            /// <summary>
            /// 静态构造函数(用于初始化对象和变量)
            /// </summary>
            static FTPs()
            {
                if (Utils.FileExists(m_configfilepath))
                {
                    SetFtpConfigInfo();
                    m_fileoldchange = System.IO.File.GetLastWriteTime(m_configfilepath);
                }
            }


            /// <summary>
            /// FTP配置文件监视方法
            /// </summary>
            private static void FtpFileMonitor()
            {
                if (Utils.FileExists(m_configfilepath))
                {
                    //获取文件最近修改时间 
                    m_filenewchange = System.IO.File.GetLastWriteTime(m_configfilepath);

                    //当ftp.config修改时间发生变化时
                    if (m_fileoldchange != m_filenewchange)
                    {
                        lock (lockhelper)
                        {
                            if (m_fileoldchange != m_filenewchange)
                            {
                                //当文件发生修改(时间变化)则重新设置相关FTP信息对象
                                SetFtpConfigInfo();
                                m_fileoldchange = m_filenewchange;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// 设置FTP对象信息
            /// </summary>
            private static void SetFtpConfigInfo()
            {
                FTPConfigInfoCollection ftpconfiginfocollection = (FTPConfigInfoCollection)SerializationHelper.Load(typeof(FTPConfigInfoCollection), m_configfilepath);

                FTPConfigInfoCollection.FTPConfigInfoCollectionEnumerator fcice = ftpconfiginfocollection.GetEnumerator();

                //遍历集合并设置相应的FTP信息(静态)对象
                while (fcice.MoveNext())
                {
                    if (fcice.Current.Name == "ForumAttach")
                    {
                        m_forumattach = fcice.Current;
                        break;
                    }
                }
            }

            /// <summary>
            /// 论坛附件FTP信息
            /// </summary>
            public static FTPConfigInfo GetForumAttachInfo
            {
                get
                {
                    FtpFileMonitor();
                    return m_forumattach;
                }
            }
        
            public FTPs() { }

            #region 异步删除FTP文件

            private delegate bool delegateDeleteFile(string fileUrl);

            //异步FTP删除文件代理
            private delegateDeleteFile delete_aysncallback;

            public void AsyncDeleteFile(string fileUrl)
            {
                delete_aysncallback = new delegateDeleteFile(DeleFtpFile);
                delete_aysncallback.BeginInvoke(fileUrl, null, null);
            }

            #endregion

            /// <summary>
            /// 删除远程附件
            /// </summary>
            /// <param name="fileUrl">要删除的远程附件链接</param>
            /// <param name="ftpuploadname">远程FTP类型(论坛,空间或相册)</param>
            /// <returns></returns>
            public bool DeleFtpFile(string fileUrl)
            {
                fileUrl = fileUrl.Replace("\\", "/");
                //远程附件必须以HTTP打头且不能以"/"结尾
                if (fileUrl.ToLower().StartsWith("http://") && !fileUrl.ToLower().EndsWith("/"))
                {
                    try
                    {
                        FTP ftpupload = new FTP();

                        string path = fileUrl.Substring(0, fileUrl.LastIndexOf("/") + 1);
                        string file = fileUrl.Replace(path, "");

                        ftpupload = new FTP(m_forumattach.Serveraddress, m_forumattach.Serverport, m_forumattach.Username, m_forumattach.Password, m_forumattach.Timeout);
                        path = path.Replace(m_forumattach.Remoteurl, m_forumattach.Uploadpath);

                        //当不以HTTP打头，则表示附件是远程附件，则切换到指定路径下
                        if (!path.StartsWith("http://") && ftpupload.ChangeDir(path))
                        {
                            ftpupload.RemoveFile(file);
                            return true;
                        }
                    }
                    catch { return false; }
                }
                return false;
            }      
        }
        #endregion ftp类文件结束
    }


}