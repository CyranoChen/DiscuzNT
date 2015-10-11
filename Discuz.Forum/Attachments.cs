using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Plugin.Preview;
using Discuz.Data;

namespace Discuz.Forum
{
    /// <summary>
    /// 附件操作类
    /// </summary>
    public class Attachments
    {
        private const string ATTACH_TIP_IMAGE = "<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_{0}\"><img border=\"0\" src=\"{1}images/attachicons/attachimg.gif\" /></span>";
        
        private const string IMAGE_ATTACH = "{0}<img imageid=\"{2}\" src=\"{1}\" onload=\"{8}attachimg(this, 'load');\" onmouseover=\"attachimginfo(this, 'attach_{2}', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_{2}', 0, event)\" {7} /><div id=\"attach_{2}_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"{3}images/attachicons/image.gif\" /><a target=\"_blank\"  href=\"{1}\"><strong>{4}</strong></a>({5})<br/><div class=\"t_smallfont\">{6}</div></div>";
        private const string IMAGE_ATTACH_MTDOWNLOAD = "{0}<img alt=\"点击加载图片\" imageid=\"{2}\" inpost=\"1\" src=\"/images/common/imgloading.png\" newsrc=\"{1}\" onload=\"{8}attachimg(this, 'load');\" onclick=\"loadImg(this);\" onmouseout=\"attachimginfo(this, 'attach_{2}', 0, event)\" {7} /><div id=\"attach_{2}_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"{3}images/attachicons/image.gif\" /><a target=\"_blank\" onclick=\"return ShowDownloadTip({9});\"  href=\"{1}\"><strong>{4}</strong></a>({5})<br/><div class=\"t_smallfont\">{6}</div></div>";

        private const string PAID_ATTACH = "{0}<strong>收费附件:{1}</strong>";
        private const string PAID_ATTACH_LINK = "<p>售价({0}):<strong>{1} </strong>[<a onclick=\"loadattachpaymentlog({2});\" href=\"javascript:void(0);\">记录</a>] [<a onclick=\"loadbuyattach({2});\" href=\"javascript:void(0);\">购买</a>]</p>";

        private const string ATTACH_INFO_DL = "<dl class=\"t_attachlist attachimg cl\">{0}</dl>";
        private const string ATTACH_INFO_DTIMG = "<dt><img src=\"{0}images/attachicons/{1}\" alt=\"\"></dt>";
        private const string ATTACH_INFO_DD = "<dd>{0}</dd>";
        private const string ATTACH_INFO_EM = "<em class=\"xg1\">(<script type=\"text/javascript\">ShowFormatBytesStr({0});</script>, 下载次数:{1})</em>";


        /// <summary>
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类数组</param>
        /// <returns>附件id数组</returns>
        public static int[] CreateAttachments(AttachmentInfo[] attachmentinfo)
        {
            int acount = attachmentinfo.Length;
            int icount = 0;
            int tid = 0;
            int pid = 0;
            int[] aid = new int[acount];
            int attType = 1;//普通附件,2为图片附件

            for (int i = 0; i < acount; i++)
            {
                if (attachmentinfo[i] != null && attachmentinfo[i].Sys_noupload.Equals(""))
                {
                    aid[i] = Discuz.Data.Attachments.CreateAttachments(attachmentinfo[i]);
                    icount++;
                    tid = attachmentinfo[i].Tid;
                    pid = attachmentinfo[i].Pid;
                    attachmentinfo[i].Aid = aid[i];
                    if (attachmentinfo[i].Filetype.ToLower().StartsWith("image"))
                        attType = 2;
                }
            }

            if (icount > 0)
                UpdateTopicAndPostAttachmentType(tid, pid, attType);

            return aid;
        }

        /// <summary>
        /// 更新指定主题和帖子的附件类型
        /// </summary>
        /// <param name="tid">指定主题</param>
        /// <param name="pid">指定帖子</param>
        /// <param name="attType">附件类型</param>
        public static void UpdateTopicAndPostAttachmentType(int tid, int pid, int attType)
        {
            Discuz.Data.Topics.UpdateTopicAttachmentType(tid, attType);
            Discuz.Data.Posts.UpdatePostAttachmentType(tid, pid, attType);
        }

        /// <summary>
        /// 获得指定附件的描述信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns>描述信息</returns>
        public static AttachmentInfo GetAttachmentInfo(int aid)
        {
            return aid > 0 ? Discuz.Data.Attachments.GetAttachmentInfo(aid) : null;
        }


        /// <summary>
        /// 获得指定帖子的附件个数
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>附件个数</returns>
        public static int GetAttachmentCountByPid(int pid)
        {
            return Discuz.Data.Attachments.GetAttachmentCountByPid(pid);
        }


        /// <summary>
        /// 获得指定帖子的附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>帖子信息</returns>
        public static DataTable GetAttachmentListByPid(int pid)
        {
            return pid > 0 ? Discuz.Data.Attachments.GetAttachmentListByPid(pid) : null;
        }

        //public static List<AttachmentInfo> GetAttachmentListByUid(int uid)
        //{
        //    return Discuz.Data.Attachments.GetAttachmentListByUid(uid);
        //}
        /// <summary>
        /// 将系统设置的附件类型以DataTable的方式存入缓存
        /// </summary>
        /// <returns>系统设置的附件类型</returns>
        public static DataTable GetAttachmentType()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/ForumSetting/AttachmentType") as DataTable;
            if (dt == null)
            {
                dt = Discuz.Data.Attachments.GetAttachmentType();

                cache.AddObject("/Forum/ForumSetting/AttachmentType", dt);
            }
            return dt;
        }

        /// <summary>
        /// 获得系统允许的附件类型和大小, 格式为: 扩展名,最大允许大小\r\n扩展名,最大允许大小\r\n.......
        /// </summary>
        /// <returns></returns>
        public static string GetAttachmentTypeArray(string filterexpression)
        {
            DataTable dt = GetAttachmentType();
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Select(filterexpression))
            {
                sb.Append(dr["extension"]);
                sb.Append(",");
                sb.Append(dr["maxsize"]);
                sb.Append("|");
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// 获得当前设置允许的附件类型
        /// </summary>
        /// <returns>逗号间隔的附件类型字符串</returns>
        public static string GetAttachmentTypeString(string filterexpression)
        {
            DataTable dt = GetAttachmentType();
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Select(filterexpression))
            {
                if (!Utils.StrIsNullOrEmpty(sb.ToString()))
                    sb.Append(",");

                sb.Append(dr["extension"]);
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// 得到用户可以上传的文件类型
        /// </summary>
        /// <param name="usergroupinfo">当前用户所属用户组信息</param>
        /// <param name="forum">所在版块</param>
        /// <returns></returns>
        public static string GetAllowAttachmentType(UserGroupInfo usergroupinfo, ForumInfo forum)
        {
            //得到用户可以上传的文件类型
            StringBuilder sbAttachmentTypeSelect = new StringBuilder();
            if (!usergroupinfo.Attachextensions.Trim().Equals(""))
            {
                sbAttachmentTypeSelect.Append("[id] in (");
                sbAttachmentTypeSelect.Append(usergroupinfo.Attachextensions);
                sbAttachmentTypeSelect.Append(")");
            }

            if (!forum.Attachextensions.Equals(""))
            {
                if (sbAttachmentTypeSelect.Length > 0)
                    sbAttachmentTypeSelect.Append(" AND ");

                sbAttachmentTypeSelect.Append("[id] in (");
                sbAttachmentTypeSelect.Append(forum.Attachextensions);
                sbAttachmentTypeSelect.Append(")");
            }
            return sbAttachmentTypeSelect.ToString();
        }

        /// <summary>
        /// 更新附件下载次数
        /// </summary>
        /// <param name="aid">附件id</param>
        public static void UpdateAttachmentDownloads(int aid)
        {
            if (aid > 0)
                Discuz.Data.Attachments.UpdateAttachmentDownloads(aid);
        }

        /// <summary>
        /// 更新主题中的附件标志
        /// </summary>
        /// <param name="tidlist">以逗号分割的主题id列表</param>
        public static void UpdateTopicAttachment(string tidlist)
        {
            string[] tid = Utils.SplitString(tidlist, ",");
            for (int i = 0; i < tid.Length; i++)
            {
                Discuz.Data.Attachments.UpdateTopicAttachment(Utils.StrToInt(tid[i].Trim(), -1));
            }
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题tid</param>
        /// <returns>删除个数</returns>
        public static int DeleteAttachmentByTid(int tid)
        {
            if (tid <= 0)
                return 0;

            return Discuz.Data.Attachments.DeleteAttachmentByTid(tid);
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">版块tid列表</param>
        /// <returns>删除个数</returns>
        public static int DeleteAttachmentByTid(string tidlist)
        {
            if (!Utils.IsNumericList(tidlist))
                return -1;

            return Discuz.Data.Attachments.DeleteAttachmentByTid(tidlist);
        }


        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aid">附件aid</param>
        /// <returns>删除个数</returns>
        public static int DeleteAttachment(int aid)
        {
            if (aid <= 0)
                return 0;

            return Discuz.Data.Attachments.DeleteAttachment(aid);
        }

        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="attachmentInfo">附件对象</param>
        /// <returns>返回被更新的数量</returns>
        public static int UpdateAttachment(AttachmentInfo attachmentInfo)
        {
            if (attachmentInfo == null || attachmentInfo.Aid <= 0)
                return 0;

            return Discuz.Data.Attachments.UpdateAttachment(attachmentInfo);
        }

        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="aidList">附件Id，以英文逗号分割</param>
        /// <returns>返回被删除的个数</returns>
        public static int DeleteAttachment(string aidList)
        {
            if (!Utils.IsNumericArray(aidList.Split(',')))
                return -1;

            return Discuz.Data.Attachments.DeleteAttachment(aidList);
        }


        /// <summary>
        /// 获得上传附件文件的大小
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetUploadFileSizeByuserid(int uid)
        {
            return Discuz.Data.Attachments.GetUploadFileSizeByUserId(uid);
        }

        /// <summary>
        /// 过滤临时内容中的本地临时标签
        /// </summary>
        /// <param name="aid">广告id</param>
        /// <param name="attachmentinfo">附件信息列表</param>
        /// <param name="tempMessage">临时信息内容</param>
        /// <returns>过滤结果</returns>
        public static string FilterLocalTags(int[] aid, AttachmentInfo[] attachmentinfo, string tempMessage)
        {
            Match m;
            Regex r;
            for (int i = 0; i < aid.Length; i++)
            {
                if (aid[i] > 0)
                {
                    r = new Regex(@"\[localimg=(\d{1,}),(\d{1,})\]" + attachmentinfo[i].Sys_index + @"\[\/localimg\]", RegexOptions.IgnoreCase);
                    for (m = r.Match(tempMessage); m.Success; m = m.NextMatch())
                    {
                        tempMessage = tempMessage.Replace(m.Groups[0].ToString(), "[attachimg]" + aid[i] + "[/attachimg]");
                    }

                    r = new Regex(@"\[local\]" + attachmentinfo[i].Sys_index + @"\[\/local\]", RegexOptions.IgnoreCase);
                    for (m = r.Match(tempMessage); m.Success; m = m.NextMatch())
                    {
                        tempMessage = tempMessage.Replace(m.Groups[0].ToString(), "[attach]" + aid[i] + "[/attach]");
                    }
                }
            }

            tempMessage = Regex.Replace(tempMessage, @"\[localimg=(\d{1,}),\s*(\d{1,})\][\s\S]+?\[/localimg\]", string.Empty, RegexOptions.IgnoreCase);
            tempMessage = Regex.Replace(tempMessage, @"\[local\][\s\S]+?\[/local\]", string.Empty, RegexOptions.IgnoreCase);
            return tempMessage;
        }

        /// <summary>
        /// 绑定附件数组中的参数，返回新上传的附件个数
        /// </summary>
        /// <param name="attachmentInfo">提交的附件列表</param>
        /// <param name="topicId">当前主题id</param>
        /// <param name="postId">当前帖子id</param>
        /// <param name="userId">当前用户id</param>
        /// <param name="userGroupInfo">当前用户用户组</param>
        /// <returns></returns>
        public static int BindAttachment(AttachmentInfo[] attachmentInfo, int topicId, int postId, int userId, UserGroupInfo userGroupInfo)
        {
            //附件阅读权限
            //string[] readperm = String.IsNullOrEmpty(DNTRequest.GetString("readperm")) ? null : DNTRequest.GetString("readperm").Split(',');
            //string[] attachdesc = DNTRequest.GetString("attachdesc") == null ? null : DNTRequest.GetString("attachdesc").Split(',');
            //string[] localid = DNTRequest.GetString("localid") == null ? null : DNTRequest.GetString("localid").Split(',');
            ////附件价格
            //string[] attachprice = DNTRequest.GetString("attachprice") == null ? null : DNTRequest.GetString("attachprice").Split(',');

            int newAttachCount = 0;
            int i_readperm = 0;

            for (int i = 0; i < attachmentInfo.Length; i++)
            {
                if (attachmentInfo[i] == null)
                    continue;

                if (attachmentInfo[i].Pid == 0)//如果附件pid=0，就代表它是新上传的附件
                    newAttachCount++;
                string aid = attachmentInfo[i].Aid.ToString();
                attachmentInfo[i].Uid = userId;
                attachmentInfo[i].Tid = topicId;
                attachmentInfo[i].Pid = postId;
                attachmentInfo[i].Postdatetime = Utils.GetDateTime();
                attachmentInfo[i].Readperm = 0;
                //attachmentInfo[i].Attachprice = attachprice != null ? UserGroups.CheckUserGroupMaxPrice(userGroupInfo, Utils.StrToInt(attachprice[i], 0)) : 0;
                int attachprice = Utils.StrToInt(DNTRequest.GetString("attachprice_" + aid),0);
                attachmentInfo[i].Attachprice = attachprice == 0 ? 0 : UserGroups.CheckUserGroupMaxPrice(userGroupInfo, attachprice);
                int readperm = Utils.StrToInt(DNTRequest.GetString("readperm_" + aid), 0);
                if (readperm != 0)
                {
                    i_readperm = readperm;
                    //当为最大阅读仅限(255)时
                    i_readperm = i_readperm > 255 ? 255 : i_readperm;
                    attachmentInfo[i].Readperm = i_readperm;
                }
                //if (attachdesc != null && !attachdesc[i].Equals(""))
                    attachmentInfo[i].Description = Utils.HtmlEncode(DNTRequest.GetString("attachdesc_" + aid));
            }
            return newAttachCount;
        }

        /// <summary>
        /// 绑定附件数组中的参数，返回新上传的附件个数
        /// </summary>
        /// <param name="attachmentInfo">附件类型</param>
        /// <param name="postId">帖子id</param>
        /// <param name="msg">原有提示信息</param>
        /// <param name="topicId">主题id</param>
        /// <param name="userId">用户id</param>
        /// <returns>无效附件个数</returns>
        //public static int BindAttachment(AttachmentInfo[] attachmentInfo, int postId, StringBuilder msg, int topicId, int userId, UserGroupInfo userGroupInfo, out int newAttachmentCount)
        //{
        //    //int acount = attachmentInfo.Length;
        //    // 附件查看权限
        //    string[] readperm = String.IsNullOrEmpty(DNTRequest.GetString("readperm")) ? null : DNTRequest.GetString("readperm").Split(',');
        //    string[] attachdesc = DNTRequest.GetString("attachdesc") == null ? null : DNTRequest.GetString("attachdesc").Split(',');
        //    string[] localid = DNTRequest.GetString("localid") == null ? null : DNTRequest.GetString("localid").Split(',');
        //    string[] attachprice = DNTRequest.GetString("attachprice") == null ? null : DNTRequest.GetString("attachprice").Split(',');

        //    //设置无效附件计数器，将在下面UserCreditsFactory.UpdateUserCreditsByUploadAttachment方法中减去该计数器的值
        //    int errorAttachment = 0;
        //    int i_readperm = 0;
        //    newAttachmentCount = 0;

        //    for (int i = 0; i < attachmentInfo.Length; i++)
        //    {
        //        if (attachmentInfo[i] != null)
        //        {
        //            if (attachmentInfo[i].Pid == 0)
        //                newAttachmentCount++;

        //            attachmentInfo[i].Uid = userId;
        //            attachmentInfo[i].Tid = topicId;
        //            attachmentInfo[i].Pid = postId;
        //            attachmentInfo[i].Postdatetime = Utils.GetDateTime();
        //            attachmentInfo[i].Readperm = 0;
        //            attachmentInfo[i].Attachprice = attachprice != null ? UserGroups.CheckUserGroupMaxPrice(userGroupInfo, Utils.StrToInt(attachprice[i], 0)) : 0;

        //            if (readperm != null)
        //            {
        //                i_readperm = Utils.StrToInt(readperm[i], 0);
        //                //当为最大阅读仅限(255)时
        //                i_readperm = i_readperm > 255 ? 255 : i_readperm;
        //                attachmentInfo[i].Readperm = i_readperm;
        //            }

        //            if (attachdesc != null && !attachdesc[i].Equals(""))
        //                attachmentInfo[i].Description = Utils.HtmlEncode(attachdesc[i]);

        //            //if (!attachmentInfo[i].Sys_noupload.Equals(""))
        //            //{
        //            //    msg.AppendFormat("<tr><td align=\"left\">{0}</td>", attachmentInfo[i].Attachment);
        //            //    msg.AppendFormat("<td align=\"left\">{0}</td></tr>", attachmentInfo[i].Sys_noupload);
        //            //    errorAttachment++;
        //            //}
        //        }
        //    }
        //    //如果有上传错误的，则减去错误的个数
        //    newAttachmentCount -= errorAttachment;
        //    return errorAttachment;
        //}



        /// <summary>
        /// 根据主题id得到缩略图附件对象
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="maxsize">最大图片尺寸</param>
        /// <param name="type">缩略图类型</param>
        /// <returns>缩略图附件对象</returns>
        public static string GetThumbnailByTid(int tid, int maxsize, ThumbnailType type)
        {
            int theMaxsize = maxsize < 300 ? maxsize : 300;
            string attCachePath = string.Format("{0}cache/thumbnail/t_{1}_{2}_{3}.jpg", BaseConfigs.GetForumPath, tid, theMaxsize, (int)type);

            AttachmentInfo attach = Discuz.Data.Attachments.GetFirstImageAttachByTid(tid);
            if (attach == null)
                return null;

            attach.Attachment = attCachePath;
            string attPhyCachePath = Utils.GetMapPath(attCachePath);
            if (!File.Exists(attPhyCachePath))
            {
                CreateTopicAttThumbnail(attPhyCachePath, type, attach.Filename, theMaxsize);
            }
            return attCachePath;
        }


        /// <summary>
        /// 创建主题附件缩略图
        /// </summary>
        /// <param name="attPhyCachePath">缓存文件路径</param>
        /// <param name="type">缩略图类型</param>
        /// <param name="attPhyPath">附件物理路径</param>
        /// <param name="theMaxsize">最大尺寸</param>
        private static void CreateTopicAttThumbnail(string attPhyCachePath, ThumbnailType type, string attPhyPath, int theMaxsize)
        {
            if (!attPhyPath.StartsWith("http://"))
                attPhyPath = Utils.GetMapPath(attPhyPath);
            if (!attPhyPath.StartsWith("http://") && !File.Exists(attPhyPath))
                return;

            string cachedir = Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/thumbnail/");

            if (!Directory.Exists(cachedir))
            {
                try
                {
                    Utils.CreateDir(cachedir);
                }
                catch
                {
                    throw new Exception("请检查程序目录下cache文件夹的用户权限！");
                }
            }

            FileInfo[] files = new DirectoryInfo(cachedir).GetFiles();

            if (files.Length > 1500)
            {
                QuickSort(files, 0, files.Length - 1);

                for (int i = files.Length - 1; i >= 1400; i--)
                {
                    try
                    {
                        files[i].Delete();
                    }
                    catch
                    { }
                }
            }
            try
            {
                switch (type)
                {
                    case ThumbnailType.Square:
                        if (attPhyPath.StartsWith("http://"))
                            Thumbnail.MakeRemoteSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        else
                            Thumbnail.MakeSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        break;
                    case ThumbnailType.Thumbnail:
                        if (attPhyPath.StartsWith("http://"))
                            Thumbnail.MakeRemoteThumbnailImage(attPhyPath, attPhyCachePath, theMaxsize, theMaxsize);
                        else
                            Thumbnail.MakeThumbnailImage(attPhyPath, attPhyCachePath, theMaxsize, theMaxsize);
                        break;
                    default:
                        if (attPhyPath.StartsWith("http://"))
                            Thumbnail.MakeRemoteSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        else
                            Thumbnail.MakeSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        break;
                }
            }
            catch
            { }
        }

        #region Helper
        /// <summary>
        /// 快速排序算法
        /// </summary>
        /// 快速排序为不稳定排序,时间复杂度O(nlog2n),为同数量级中最快的排序方法
        /// <param name="arr">划分的数组</param>
        /// <param name="low">数组低端上标</param>
        /// <param name="high">数组高端下标</param>
        /// <returns></returns>
        private static int Partition(FileInfo[] arr, int low, int high)
        {
            //进行一趟快速排序,返回中心轴记录位置
            // arr[0] = arr[low];
            FileInfo pivot = arr[low];//把中心轴置于arr[0]
            while (low < high)
            {
                while (low < high && arr[high].CreationTime <= pivot.CreationTime)
                    --high;
                //将比中心轴记录小的移到低端
                Swap(ref arr[high], ref arr[low]);
                while (low < high && arr[low].CreationTime >= pivot.CreationTime)
                    ++low;
                //将比中心轴记录大的移到高端
                Swap(ref arr[high], ref arr[low]);
            }
            arr[low] = pivot; //中心轴移到正确位置
            return low;  //返回中心轴位置
        }

        private static void Swap(ref FileInfo i, ref FileInfo j)
        {
            FileInfo t;
            t = i;
            i = j;
            j = t;
        }

        /// <summary>
        /// 快速排序算法
        /// </summary>
        /// 快速排序为不稳定排序,时间复杂度O(nlog2n),为同数量级中最快的排序方法
        /// <param name="arr">划分的数组</param>
        /// <param name="low">数组低端上标</param>
        /// <param name="high">数组高端下标</param>
        public static void QuickSort(FileInfo[] arr, int low, int high)
        {
            if (low <= high - 1)//当 arr[low,high]为空或只一个记录无需排序
            {
                int pivot = Partition(arr, low, high);
                QuickSort(arr, low, pivot - 1);
                QuickSort(arr, pivot + 1, high);
            }
        }

        #endregion

        /// <summary>
        /// 根据帖子ID删除附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        public static void DeleteAttachmentByPid(int pid)
        {
            DataTable dtAttach = GetAttachmentListByPid(pid);
            if (dtAttach != null || dtAttach.Rows.Count != 0)
            {
                foreach (DataRow dr in dtAttach.Rows)
                {
                    string path = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + dr["filename"]);

                    if (dr["filename"].ToString().Trim().ToLower().IndexOf("http") < 0 && File.Exists(path))
                        File.Delete(path);
                }
            }
            Discuz.Data.Attachments.DeleteAttachmentByPid(pid);
        }

        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>附件数量</returns>
        public static int GetUserAttachmentCount(int uid)
        {
            return (uid < 0) ? 0 : Discuz.Data.Attachments.GetUserAttachmentCount(uid);
            //return Discuz.Data.Attachments.GetUserAttachmentCount(uid);
        }

        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="typeid">附件类型id</param>
        /// <returns>附件数量</returns>
        public static int GetUserAttachmentCount(int uid, int typeid)
        {
            return Discuz.Data.Attachments.GetUserAttachmentCount(uid, SetExtNamelist(typeid));
        }

        /// <summary>
        /// 按照附件分类返回用户的附件
        /// </summary>
        /// </summary>
        /// <param name="typeid">附件类型id</param>
        /// <returns>返回用户附件</returns>
        public static string SetExtNamelist(int typeid)
        {
            string newstring = "";
            foreach (string s in GetExtName(typeid).Split(','))
            {
                newstring += "'" + s + "',";
            }
            return newstring.Remove(newstring.Length - 1, 1);
        }


        /// <summary>
        /// 获取用户附件列表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="typeid">附件类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>用户附件列表</returns>
        public static List<MyAttachmentInfo> GetAttachmentByUid(int uid, int typeid, int pageIndex, int pageSize)
        {
            List<MyAttachmentInfo> myattachment = new List<MyAttachmentInfo>();

            if (pageIndex <= 0)
                return myattachment;

            return Discuz.Data.Attachments.GetAttachmentByUid(uid, typeid, pageIndex, pageSize, SetExtNamelist(typeid));
        }

        /// <summary>
        /// 获取附件类型列表
        /// </summary>
        /// <returns>附件类型列表</returns>
        public static List<AttachmentType> AttachTypeList()
        {
            return Discuz.Data.Attachments.AttachTypeList();
        }


        /// <summary>
        /// 获取我的附件指定类型的扩展名
        /// </summary>
        /// <param name="typeid">附件类型id</param>
        /// <returns>扩展名称</returns>
        public static string GetExtName(int typeid)
        {
            foreach (AttachmentType act in MyAttachmentsTypeConfigs.GetConfig().AttachmentType)
            {
                if (act.TypeId == typeid)
                    return "." + act.ExtName.Replace(",", ",.");
            }
            return "";
        }

        /// <summary>
        /// 更新Silverlight上传的指定主题和帖子的附件信息
        /// </summary>
        /// <param name="topicid">主题id</param>
        /// <param name="postid">帖子id</param>
        public static void UpdateSLUploadAttachInfo(int topicid, int postid, UserGroupInfo usergroupinfo)
        {
            //如何启用了silvelight并且使用它上传了相关附件时
            if (GeneralConfigs.GetConfig().Silverlight != 1 || Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("attachid")))
                return;

            //更新silverlight上传的附件信息
            //编辑帖子时如果进行了更新附件操作
            string[] attachidArray = DNTRequest.GetFormString("attachid").Split(',');//所有已上传的附件Id列表
            if (attachidArray[0] == "")
                attachidArray[0] = "0";

            if (Utils.IsNumericArray(attachidArray))
            {
                string[] attachdescArray = DNTRequest.GetFormString("sl_attachdesc").Split(',');//所有已上传的附件的描述
                string[] readpermArray = DNTRequest.GetFormString("sl_readperm").Split(',');//所有已上传得附件的阅读权限
                string[] attachprice = DNTRequest.GetString("sl_attachprice") == null ? null : DNTRequest.GetString("sl_attachprice").Split(',');
                int attType = 0;//普通附件,2为图片附件

                for (int i = 0; (i < attachidArray.Length && i <= GeneralConfigs.GetConfig().Maxattachments); i++)
                {
                    int aid = Utils.StrToInt(attachidArray[i], 0);
                    if (aid > 0 && Utils.IsSafeSqlString(attachdescArray[i]))
                    {
                        AttachmentInfo attachmentInfo = Attachments.GetAttachmentInfo(aid);
                        attachmentInfo.Description = attachdescArray[i];
                        attachmentInfo.Readperm = readpermArray.Length == attachidArray.Length ? Utils.StrToInt(readpermArray[i], 0) : 0;
                        attachmentInfo.Tid = topicid;
                        attachmentInfo.Pid = postid;
                        attachmentInfo.Attachprice = attachprice.Length == attachidArray.Length ? UserGroups.CheckUserGroupMaxPrice(usergroupinfo, Utils.StrToInt(attachprice[i], 0)) : 0;
                        Attachments.UpdateAttachment(attachmentInfo);

                        attType = attachmentInfo.Filetype.ToLower().StartsWith("image") ? 2 : 1;
                    }
                }

                if (attType > 0)
                    UpdateTopicAndPostAttachmentType(topicid, postid, attType);
            }
        }

        /// <summary>
        /// 获取指定用户未使用的附件的JSON字符串(posttopic模版页调用)
        /// </summary>
        /// <param name="userid">指定用户id</param>
        /// <returns>JSON字符串</returns>
        //public static string GetNoUsedAttachmentJson(int userid)
        //{
        //    return Discuz.Data.Attachments.GetNoUsedAttachmentJson(userid);
        //}

        /// <summary>
        /// 删除未被使用的论坛附件
        /// </summary>
        public static void DeleteNoUsedForumAttachment()
        {
            Discuz.Data.Attachments.DeleteNoUsedForumAttachment();
        }

        /// <summary>
        /// 根据pidlist获得附件列表，并查询是否被购买过
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="pidList">帖子id列表字符串</param>
        /// <returns></returns>
        public static List<ShowtopicPageAttachmentInfo> GetAttachmentList(PostpramsInfo postpramsInfo, string pidList)
        {
            List<ShowtopicPageAttachmentInfo> attachList = new List<ShowtopicPageAttachmentInfo>();

            if (!Utils.StrIsNullOrEmpty(pidList))
            {
                attachList = Data.Attachments.GetAttachmentListByPidList(pidList.ToString());
                CheckPurchasedAttachments(attachList, postpramsInfo.CurrentUserid);
            }

            return attachList;
        }

        /// <summary>
        /// 是否启用squid附件加速功能
        /// </summary>
        private static bool appSquidAttachment = EntLibConfigs.GetConfig() != null && !Utils.StrIsNullOrEmpty(EntLibConfigs.GetConfig().Attachmentdir);

        /// <summary>
        /// 检查当前用户查看的附件是否已经付费
        /// </summary>
        /// <param name="attachList">附件列表</param>
        /// <param name="uid">当前用户Id</param>
        public static void CheckPurchasedAttachments(List<ShowtopicPageAttachmentInfo> attachList, int uid)
        {
            string attachIdList = "";//附件id列表
            foreach (ShowtopicPageAttachmentInfo attach in attachList)
            {
                //当使用企业版squid静态附件加速时,则对本地附件修改成squid服务前缀地址
                if (attach.Filename.IndexOf("http") < 0 && appSquidAttachment)
                    attach.Filename = EntLibConfigs.GetConfig().Attachmentdir.TrimEnd('/') + "/" + attach.Filename;

                attachIdList = Utils.MergeString(attach.Aid.ToString(), attachIdList);
            }
            if (!string.IsNullOrEmpty(attachIdList) && uid > 0)
            {
                //获取当前用户所购买的指定附件列表
                attachIdList = Data.Attachments.GetPurchasedAttachmentIdList(attachIdList, uid);

                //更新当前附件是否被购买
                foreach (ShowtopicPageAttachmentInfo attachmentInfo in attachList)
                {
                    if (attachmentInfo.Attachprice > 0 && Utils.InArray(attachmentInfo.Aid.ToString(), attachIdList))
                    {
                        attachmentInfo.Isbought = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 获取图片显示宽度
        /// </summary>
        /// <param name="templateWidth">模版宽度</param>
        /// <param name="attachWidth">附件宽度</param>
        /// <returns></returns>
        private static string GetImageShowWidth(int templateWidth, int attachWidth)
        {
            if (attachWidth < 0 || templateWidth < attachWidth)
                return string.Format("width={0}", templateWidth);

            return "";
        }

        /// <summary>
        /// 生成改变图片宽度脚本
        /// </summary>
        /// <param name="templateWidth">模版宽度</param>
        /// <param name="attachWidth">附件宽度</param>
        /// <param name="isOnLoad">是否需要生成装载前辍</param>
        /// <returns></returns>
        private static string GetAutoImageWidthScript(int templateWidth, int attachWidth, bool needOnLoad)
        {
            string imgScript = string.Empty;
            if (attachWidth == 0)
                imgScript = string.Format("if(this.width > {0}) this.width = {0};", templateWidth);
            if (needOnLoad)
                imgScript = string.Format("onload=\"{0}\"", imgScript);
            return imgScript;
        }

        /// <summary>
        /// 解析图片附件内容
        /// </summary>
        /// <param name="postPramsInfo">参数对象</param>
        /// <param name="attachInfo">附件对象</param>
        /// <param name="fileSize">文件大小字符串</param>
        /// <returns>解析内容</returns>
        private static string ParseImageAttachContent(PostpramsInfo postPramsInfo, ShowtopicPageAttachmentInfo attachInfo, string fileSize)
        {
            string content = string.Empty;
            string forumPath = BaseConfigs.GetBaseConfig().Forumpath;
            string tipImg = string.Format(ATTACH_TIP_IMAGE, attachInfo.Aid, forumPath);
            string attachInfoDtimg = string.Format(ATTACH_INFO_DTIMG, forumPath, "image.gif");
            string attachInfoEm = string.Format(ATTACH_INFO_EM, attachInfo.Filesize, attachInfo.Downloads);

            bool loadImageMode = GeneralConfigs.GetConfig().Showimgattachmode == 0;
            string imageAttachTemplate = loadImageMode ? IMAGE_ATTACH : IMAGE_ATTACH_MTDOWNLOAD;
            string imageAttachWidth = loadImageMode ? GetImageShowWidth(postPramsInfo.TemplateWidth, attachInfo.Width) : "";

            attachInfo.Attachimgpost = 1;

            if (postPramsInfo.Showattachmentpath == 1)
            {
                if (postPramsInfo.Isforspace == 1)
                {
                    if (attachInfo.Filename.ToLower().IndexOf("http") == 0)
                    {
                        return string.Format("<img src=\"{0}\" {1} {2}/>", attachInfo.Filename, GetImageShowWidth(postPramsInfo.TemplateWidth, attachInfo.Width), GetAutoImageWidthScript(postPramsInfo.TemplateWidth, attachInfo.Width, true));
                    }
                    return string.Format("<img src=\"{0}upload/{1}\" {2} {3}/>", forumPath, attachInfo.Filename, GetImageShowWidth(postPramsInfo.TemplateWidth, attachInfo.Width), GetAutoImageWidthScript(postPramsInfo.TemplateWidth, attachInfo.Width, true));
                }
                if (postPramsInfo.CurrentUserGroup.Radminid == 1 || postPramsInfo.CurrentUserid == attachInfo.Uid || attachInfo.Attachprice <= 0 || attachInfo.Isbought == 1)
                {
                    if (attachInfo.Filename.ToLower().IndexOf("http") == 0)
                    {
                        content = string.Format(imageAttachTemplate, tipImg, attachInfo.Filename, attachInfo.Aid, forumPath, attachInfo.Attachment.Trim(), fileSize, attachInfo.Postdatetime, imageAttachWidth, GetAutoImageWidthScript(postPramsInfo.TemplateWidth, attachInfo.Width, false), attachInfo.Uid);
                        return content;
                    }
                    content = string.Format(imageAttachTemplate, tipImg, forumPath + "upload/" + attachInfo.Filename, attachInfo.Aid, forumPath, attachInfo.Attachment.Trim(), fileSize, attachInfo.Postdatetime, imageAttachWidth, GetAutoImageWidthScript(postPramsInfo.TemplateWidth, attachInfo.Width, false), attachInfo.Uid);
                    return content;
                }
                string.Format(ATTACH_INFO_DL, attachInfoDtimg + string.Format(ATTACH_INFO_DD, attachInfo.Attachment + attachInfoEm +
                            string.Format(PAID_ATTACH_LINK, Scoresets.GetTopicAttachCreditsTransName(), attachInfo.Attachprice, attachInfo.Aid)));
            }
            if (postPramsInfo.Isforspace == 1)
            {
                return string.Format("<img src=\"{0}attachment.aspx?attachmentid={1}\" {2} {3}/>", forumPath, attachInfo.Aid, GetImageShowWidth(postPramsInfo.TemplateWidth, attachInfo.Width), GetAutoImageWidthScript(postPramsInfo.TemplateWidth, attachInfo.Width, true));
            }
            if (postPramsInfo.CurrentUserGroup.Radminid == 1 || postPramsInfo.CurrentUserid == attachInfo.Uid || attachInfo.Attachprice <= 0 || attachInfo.Isbought == 1)
            {
                return string.Format(imageAttachTemplate, tipImg, forumPath + "attachment.aspx?attachmentid=" + attachInfo.Aid, attachInfo.Aid, forumPath, attachInfo.Attachment.Trim(), fileSize, attachInfo.Postdatetime, imageAttachWidth, GetAutoImageWidthScript(postPramsInfo.TemplateWidth, attachInfo.Width, false), attachInfo.Uid);
            }

            return string.Format(ATTACH_INFO_DL, attachInfoDtimg + string.Format(ATTACH_INFO_DD, attachInfo.Attachment + attachInfoEm +
                            string.Format(PAID_ATTACH_LINK, Scoresets.GetTopicAttachCreditsTransName(), attachInfo.Attachprice, attachInfo.Aid)));
        }

        /// <summary>
        /// 附件内容正则替换字符串
        /// </summary>
        /// <param name="postPramsInfo">帖子参数</param>
        /// <param name="allowGetAttach">是否允许返回附件</param>
        /// <param name="attachInfo">附件对象</param>
        /// <returns>返回附件内容正则替换字符串</returns>
        private static string GetAttachReplacement(PostpramsInfo postPramsInfo, int allowGetAttach, ShowtopicPageAttachmentInfo attachInfo)
        {
            string fileSize = "";
            string replacement = "";
            string forumPath = BaseConfigs.GetBaseConfig().Forumpath;
            string attachIcon = "attachment.gif";

            //string needPayAttachTemplateRow1 = "<p><img alt=\"\" src=\"{0}images/attachicons/{2}\" border=\"0\"/><span class=\"bold\">收费附件</span>: <strong>{1}</strong><em class=\"xg1\">(<script type=\"text/javascript\">ShowFormatBytesStr({3});</script>, 下载次数:{4})</em></p>";
            //string needPayAttachTemplateRow2 = "<p>售价(" + Scoresets.GetTopicAttachCreditsTransName() + "):<strong>{5} </strong>[<a onclick=\"loadattachpaymentlog({6});\" href=\"javascript:void(0);\">记录</a>] [<a onclick=\"loadbuyattach({6});\" href=\"javascript:void(0);\">购买</a>]</p>";


            if (Utils.InArray(Utils.GetFileExtName(attachInfo.Filename), ".rar,.zip"))
                attachIcon = "rar.gif";

            string attachInfoDtimg = string.Format(ATTACH_INFO_DTIMG, forumPath, attachIcon);

            if (allowGetAttach != 1)
                replacement = string.Format(ATTACH_INFO_DL, attachInfoDtimg + string.Format(ATTACH_INFO_DD, "<span class=\"attachnotdown\">您所在的用户组无法下载或查看附件</span>"));
            //replacement = string.Format("<br /><img src=\"{0}images/attachicons/{1}\" alt=\"\">&nbsp;附件: <span class=\"attachnotdown\">您所在的用户组无法下载或查看附件</span>", forumPath, attachIcon);
            else if (attachInfo.Allowread == 1)
            {
                if (attachInfo.Filesize > 1024)
                    fileSize = Convert.ToString(Math.Round(Convert.ToDecimal(attachInfo.Filesize) / 1024, 2)) + " K";
                else
                    fileSize = attachInfo.Filesize + " B";

                string attachInfoEm = string.Format(ATTACH_INFO_EM, attachInfo.Filesize, attachInfo.Downloads);
                //string attachInfoA = string.Format("<a href=\"{0}attachment.aspx?attachmentid={1}\" onclick=\"return ShowDownloadTip({2});\" target=\"_blank\">{3}</a>", forumPath, attachInfo.Aid, attachInfo.Uid, attachInfo.Attachment);

                if (Utils.IsImgFilename(attachInfo.Attachment))
                    replacement = Attachments.ParseImageAttachContent(postPramsInfo, attachInfo, fileSize);
                else
                {
                    attachInfo.Attachimgpost = 0;
                    if (postPramsInfo.CurrentUserGroup.Radminid == 1 || postPramsInfo.CurrentUserid == attachInfo.Uid || attachInfo.Attachprice <= 0 || attachInfo.Isbought == 1)
                        replacement = string.Format(ATTACH_INFO_DL, attachInfoDtimg + string.Format(ATTACH_INFO_DD,
                            string.Format("<a href=\"{0}attachment.aspx?attachmentid={1}\" onclick=\"return ShowDownloadTip({2});\" target=\"_blank\">{3}</a>",
                            forumPath, attachInfo.Aid, attachInfo.Uid, attachInfo.Attachment) + attachInfoEm));

                    //replacement = string.Format("<p><img alt=\"\" src=\"{0}images/attachicons/{6}\" border=\"0\"/><span class=\"bold\">附件</span>: <a href=\"{0}attachment.aspx?attachmentid={1}\" onclick=\"return ShowDownloadTip({7});\" target=\"_blank\">{2}</a> ({3}, {4})<br />该附件被下载次数 {5}</p>", forumPath, attachInfo.Aid, attachInfo.Attachment.Trim(), attachInfo.Postdatetime, fileSize, attachInfo.Downloads, attachIcon, attachInfo.Uid);
                    else
                        replacement = string.Format(ATTACH_INFO_DL, attachInfoDtimg + string.Format(ATTACH_INFO_DD, attachInfo.Attachment + attachInfoEm +
                            string.Format(PAID_ATTACH_LINK, Scoresets.GetTopicAttachCreditsTransName(), attachInfo.Attachprice, attachInfo.Aid)));
                    //replacement = string.Format(needPayAttachTemplateRow1 + needPayAttachTemplateRow2, forumPath, attachInfo.Attachment, attachIcon, attachInfo.Filesize, attachInfo.Downloads, attachInfo.Attachprice, attachInfo.Aid);
                }
            }
            else
            {
                if (postPramsInfo.CurrentUserid > 0)
                    replacement = string.Format("<br /><span class=\"notdown\">你的下载权限 {0} 低于此附件所需权限 {1}, 你无权查看此附件</span><br />", postPramsInfo.Usergroupreadaccess, attachInfo.Readperm);
                else
                    replacement = string.Format("<div class=\"hide\">附件: <em><span class=\"attachnotdown\">你需要<a href=\"{0}login.aspx\" onclick=\"hideWindow('register');showWindow('login', this.href);\">登录</a>才可以下载或查看附件。没有帐号? <a href=\"{0}register.aspx\" onclick=\"hideWindow('login');showWindow('register', this.href);\" title=\"注册帐号\">注册</a></span></em></div>", BaseConfigs.GetForumPath);
            }

            return replacement;
        }

        /// <summary>
        /// 获取加载附件信息的帖子内容
        /// </summary>
        /// <param name="postPramsInfo">参数列表</param>
        /// <param name="allowGetAttach">是否允许获取附件</param>
        /// <param name="attHidArray">隐藏在hide标签中的附件数组</param>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="attInfo">附件信息</param>
        /// <param name="message">帖子内容</param>
        /// <returns>帖子内容</returns>
        public static string GetMessageWithAttachInfo(PostpramsInfo postPramsInfo, int allowGetAttach, string[] hideAttachIdArray, ShowtopicPagePostInfo postInfo, ShowtopicPageAttachmentInfo attachInfo, string message)
        {
            string replacement;
            if (Utils.InArray(attachInfo.Aid.ToString(), hideAttachIdArray))
                return message;
            if ((attachInfo.Readperm <= postPramsInfo.Usergroupreadaccess || postInfo.Posterid == postPramsInfo.CurrentUserid) && allowGetAttach == 1)
                attachInfo.Allowread = 1;
            else
                attachInfo.Allowread = 0;

            attachInfo.Getattachperm = allowGetAttach;
            attachInfo.Filename = attachInfo.Filename.ToString().Replace("\\", "/");

            if (message.IndexOf("[attach]" + attachInfo.Aid + "[/attach]") != -1 || message.IndexOf("[attachimg]" + attachInfo.Aid + "[/attachimg]") != -1)
            {
                replacement = GetAttachReplacement(postPramsInfo, allowGetAttach, attachInfo);

                Regex r = new Regex(string.Format(@"\[attach\]{0}\[/attach\]|\[attachimg\]{0}\[/attachimg\]", attachInfo.Aid));
                message = r.Replace(message, replacement, 1);

                message = message.Replace("[attach]" + attachInfo.Aid + "[/attach]", string.Empty);
                message = message.Replace("[attachimg]" + attachInfo.Aid + "[/attachimg]", string.Empty);

                if (attachInfo.Pid == postInfo.Pid)
                    attachInfo.Inserted = 1;
            }
            else
            {
                if (attachInfo.Pid == postInfo.Pid)
                {
                    attachInfo.Attachimgpost = Utils.IsImgFilename(attachInfo.Attachment) ? 1 : 0;

                    //加载文件预览类指定方法
                    IPreview preview = PreviewProvider.GetInstance(Path.GetExtension(attachInfo.Filename).Remove(0, 1).Trim());

                    if (preview != null)
                    {
                        //当支持FTP上传附件时
                        if (attachInfo.Filename.StartsWith("http://") || attachInfo.Filename.StartsWith("ftp://"))
                        {
                            preview.UseFTP = true;
                            attachInfo.Preview = preview.GetPreview(attachInfo.Filename, attachInfo);
                        }
                        else
                        {
                            preview.UseFTP = false;
                            string filename = "";
                            if (!attachInfo.Filename.Contains("://"))
                                filename = Utils.GetMapPath(BaseConfigs.GetForumPath + @"upload/" + attachInfo.Filename);

                            attachInfo.Preview = preview.GetPreview(filename, attachInfo);
                        }
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// 删除附件类型
        /// </summary>
        /// <param name="attchtypeidlist">附件类型Id列表</param>
        public static void DeleteAttchType(string idList)
        {
            if (Utils.IsNumericList(idList))
            {
                Data.Attachments.DeleteAttchType(idList);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumSetting/AttachmentType");
            }
        }

        /// <summary>
        /// 添加附件类型
        /// </summary>
        /// <param name="extension">附件类型扩展名</param>
        /// <param name="maxsize"></param>
        public static void AddAttchType(string extension, string maxSize)
        {
            Data.Attachments.AddAttchType(extension, maxSize);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumSetting/AttachmentType");
            Discuz.Forum.Attachments.GetAttachmentType();
        }

        /// <summary>
        /// 更新允许的附件类型
        /// </summary>
        /// <param name="extension">附件类型扩展名</param>
        /// <param name="maxsize">大小</param>
        /// <param name="id">附件类型ID</param>
        public static void UpdateAttchType(string extension, string maxSize, int id)
        {
            if (id > 0)
                Data.Attachments.UpdateAttchType(extension, maxSize, id);
        }

        /// <summary>
        /// 按条件获取特定分表中的附件列表
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="postName">分表名称</param>
        /// <returns></returns>
        public static DataTable GetAttachList(string condition, string postName)
        {
            return Data.Attachments.GetAttachList(condition, postName);
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
                             string downloadsmin, string downloadsmax, string postdatetime, string filename, string description, string poster)
        {
            return Data.Attachments.SearchAttachment(forumid, posttablename, filesizemin, filesizemax, downloadsmin, downloadsmax, postdatetime, filename, description, poster);
        }


        /// <summary>
        /// 附件操作
        /// </summary>
        /// <param name="attachmentinfo">附件信息</param>
        /// <param name="topicId">主题id</param>
        /// <param name="postId">帖子id</param>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="returnMsg">返回信息</param>
        /// <param name="userId">当前用户id</param>
        /// <param name="config">配置信息</param>
        /// <param name="userGroupInfo">当前用户组信息</param>
        /// <returns></returns>
        public static bool UpdateAttachment(AttachmentInfo[] attachmentArray, int topicId, int postId, PostInfo postInfo, ref StringBuilder returnMsg, int userId, GeneralConfigInfo config, UserGroupInfo userGroupInfo)
        {
            if (attachmentArray == null)
                return false;

            if (attachmentArray.Length > config.Maxattachments)
            {
                //returnMsg = new StringBuilder("系统设置为每个帖子附件不得多于" + config.Maxattachments + "个");
                returnMsg = new StringBuilder();
                returnMsg.AppendFormat("您添加了{0}个图片/附件,多于系统设置的{1}个.<br/>请重新编辑该帖并删除多余图片/附件.", attachmentArray.Length, config.Maxattachments);
                return false;
            }
            int newAttachCount = Attachments.BindAttachment(attachmentArray, topicId, postId, userId, userGroupInfo);
            //int errorAttachment = Attachments.BindAttachment(attachmentinfoarray, postid, sb, topicid, userid, usergroupinfo, out newAttachCount);
            int[] aid = new int[attachmentArray.Length];
            int attType = 0;//普通附件,2为图片附件
            for (int i = 0; i < attachmentArray.Length; i++)
            {
                //attachmentinfoarray[i].Tid = topicid;
                //attachmentinfoarray[i].Pid = postid;
                Attachments.UpdateAttachment(attachmentArray[i]);
                aid[i] = attachmentArray[i].Aid;
                attType = attachmentArray[i].Filetype.ToLower().StartsWith("image") ? 2 : 1;
            }

            string tempMessage = Attachments.FilterLocalTags(aid, attachmentArray, postInfo.Message);

            if (tempMessage != postInfo.Message)
            {
                postInfo.Message = tempMessage;
                postInfo.Pid = postId;
                Posts.UpdatePost(postInfo);
            }
            if (newAttachCount > 0)
                UserCredits.UpdateUserExtCreditsByUploadAttachment(userId, newAttachCount);

            UpdateTopicAndPostAttachmentType(topicId, postId, attType);
            return true;
        }

        /// <summary>
        /// 获取指定用户未使用的附件列表
        /// </summary>
        /// <param name="userid">指定用户id</param>
        /// <param name="aidList">指定过滤的附件id</param>
        /// <returns>AttachmentInfo数组</returns>
        public static AttachmentInfo[] GetNoUsedAttachmentArray(int userid, string aidList)
        {
            List<AttachmentInfo> attachmentList = Data.Attachments.GetNoUsedAttachmentList(userid, "");
            if (attachmentList.Count > 0)
            {
                List<AttachmentInfo> filterAttachmentList = new List<AttachmentInfo>();
                foreach (AttachmentInfo attachmentInfo in attachmentList)
                {
                    if (("," + aidList + ",").Contains("," + attachmentInfo.Aid + ","))
                        filterAttachmentList.Add(attachmentInfo);
                }
                return filterAttachmentList.ToArray();
            }
            return null;
        }

        /// <summary>
        /// 获取当前编辑帖子的附件id列表的附件信息列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="attachId"></param>
        /// <returns></returns>
        public static AttachmentInfo[] GetEditPostAttachArray(int userid, string attachId)
        {
            return Data.Attachments.GetEditPostAttachList(userid, attachId).ToArray();
        }

        /// <summary>
        /// 允许上传的图片扩展名
        /// </summary>
        /// <param name="allowFormats">用户允许上传的所有扩展名</param>
        /// <returns></returns>
        public static string GetImageAttachmentTypeString(string allowFormats)
        {
            if (allowFormats == "")
                allowFormats = "jpg,gif,png,jpeg";
            else
            {
                string[] tempExt = allowFormats.Split(',');
                allowFormats = "";
                foreach (string ext in tempExt)
                {
                    if (Utils.InArray(ext, "jpg,gif,png,jpeg"))
                    {
                        allowFormats += ext + ",";
                    }
                }
            }
            return allowFormats.TrimEnd(',');
        }
    }// class end
}
