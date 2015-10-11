using System;
using System.Web;
using System.Data;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Plugin.Mall;

namespace Discuz.Web
{
    /// <summary>
    /// attachment页的类派生于BasePage类
    /// </summary>
    public class attachment : PageBase
    {
        #region 变量声明
        /// <summary>
        /// 附件所属主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 附件信息
        /// </summary>
        public AttachmentInfo attachmentinfo;
        /// <summary>
        /// 附件Id
        /// </summary>
        public int attachmentid = DNTRequest.GetInt("attachmentid", -1);
        /// <summary>
        /// 是否需要登录后进行下载
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// 附件商品信息
        /// </summary>
        public Goodsinfo goodsinfo;
        /// <summary>
        /// 商品附件信息
        /// </summary>
        public Goodsattachmentinfo goodsattachmentinfo;
        /// <summary>
        /// 版块信息
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 错误提示信息
        /// </summary>
        private string msg = "";

        private bool ismoder = false;
        #endregion 变量声明

        protected override void ShowPage()
        {
            pagetitle = "附件下载";

            if (attachmentid == -1)
            {
                AddErrLine("无效的附件ID");
                return;
            }

            //　如果当前用户非管理员并且论坛设定了禁止下载附件时间段，当前时间如果在其中的一个时间段内，则不允许用户下载附件
            if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visitTime = "";
                if (Scoresets.BetweenTime(config.Attachbanperiods, out visitTime))
                {
                    AddErrLine("在此时间段( " + visitTime + " )内用户不可以下载附件");
                    return;
                }
            }

            if (DNTRequest.GetString("goodsattach").ToLower() == "yes")
                GetGoodsAttachInfo(attachmentid);
            else
            {
                // 获取该附件的信息
                attachmentinfo = Attachments.GetAttachmentInfo(attachmentid);
                if (attachmentinfo == null)
                {
                    AddErrLine("不存在的附件ID");
                    return;
                }
                //当前用户已上传但是还没有绑定到帖子的附件需要特殊处理，直接输出图片
                if ((userid > 0 || userid == -1) && userid == attachmentinfo.Uid && attachmentinfo.Tid == 0 && attachmentinfo.Filetype.StartsWith("image/"))
                {
                    HttpContext.Current.Response.Clear();
                    if(attachmentinfo.Filename.IndexOf("http") < 0 )
                       HttpContext.Current.Response.TransmitFile(BaseConfigs.GetForumPath + "upload/" + attachmentinfo.Filename.Trim());
                    else
                       HttpContext.Current.Response.Redirect(attachmentinfo.Filename.Trim());
                    
                    HttpContext.Current.Response.End();
                    return;
                }
                // 获取该主题的信息
                topic = Topics.GetTopicInfo(attachmentinfo.Tid);
                if (topic == null)
                {
                    AddErrLine("不存在的主题ID");
                    return;
                }

                ForumInfo forum = Forums.GetForumInfo(topic.Fid);
                pagetitle = Utils.RemoveHtml(forum.Name);
                if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
                {
                    AddErrLine(msg);
                    if (userid == -1)
                        needlogin = true;
                    return;
                }

                //添加判断特殊用户的代码
                if (!UserAuthority.CheckUsertAttachAuthority(forum, usergroupinfo, userid, ref msg))
                {
                    AddErrLine(msg);
                    if (userid == -1)
                        needlogin = true;
                    return;
                }

                ismoder = Moderators.IsModer(useradminid, userid, forum.Fid);
                // 检查用户是否拥有足够的阅读权限
                if ((attachmentinfo.Readperm > usergroupinfo.Readaccess) && (attachmentinfo.Uid != userid) && (!ismoder))
                {
                    AddErrLine("您的阅读权限不够");
                    if (userid == -1)
                        needlogin = true;
                    return;
                }

                //检查附件是否存在
                if (attachmentinfo.Filename.IndexOf("http") < 0 && !File.Exists(Utils.GetMapPath(string.Format(@"{0}upload/{1}", BaseConfigs.GetForumPath, attachmentinfo.Filename))))
                {
                    AddErrLine("该附件文件不存在或已被删除");
                    return;
                }

                //(!Utils.IsImgFilename(attachmentinfo.Filename.Trim()) || config.Showimages != 1)：判断文件是否是图片和图片是否允许在帖子中直接显示
                //userid != attachmentinfo.Uid && !ismoder：判断当前下载用户是否是该附件的发布者和当前用户是否具有管理权限
                //Utils.StrIsNullOrEmpty(Utils.GetCookie("dnt_attachment_" + attachmentid))当前用户是否已经下载过该附件
                if ((!Utils.IsImgFilename(attachmentinfo.Filename.Trim()) || config.Showimages != 1) &&
                    (userid != attachmentinfo.Uid && !ismoder && Utils.StrIsNullOrEmpty(Utils.GetCookie("dnt_attachment_" + attachmentid))))
                {
                    if (Scoresets.IsSetDownLoadAttachScore() && UserCredits.UpdateUserExtCreditsByDownloadAttachment(userid, 1) == -1)
                    {
                        string addExtCreditsTip = "";
                        if (EPayments.IsOpenEPayments())
                            addExtCreditsTip = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";
                        AddErrLine("您的积分不足" + addExtCreditsTip);
                        return;
                    }
                    //设置该附件已经下载的cookie
                    Utils.WriteCookie("dnt_attachment_" + attachmentid, "true", 5);
                }

                //检查附件是否存在
                if (AttachPaymentLogs.HasBoughtAttach(userid, usergroupinfo.Radminid, attachmentinfo))
                {
                    AddErrLine("该附件为交易附件, 请先行购买!");
                    return;
                }
                //如果是图片就不更新下载次数
                if (!Utils.IsImgFilename(attachmentinfo.Filename.Trim()))
                {
                    Attachments.UpdateAttachmentDownloads(attachmentid);
                }

                EntLibConfigInfo entLibConfigInfo = EntLibConfigs.GetConfig();
                //当使用企业版squid静态文件加速时
                if (attachmentinfo.Filename.IndexOf("http") < 0 && entLibConfigInfo != null && !Utils.StrIsNullOrEmpty(entLibConfigInfo.Attachmentdir))
                    attachmentinfo.Filename = EntLibConfigs.GetConfig().Attachmentdir.TrimEnd('/') + "/" + attachmentinfo.Filename;

                if (attachmentinfo.Filename.IndexOf("http") < 0)
                {   //当使用mongodb数据库存储附件及相关信息时
                    if (entLibConfigInfo != null && entLibConfigInfo.Cacheattachfiles.Enable && entLibConfigInfo.Cacheattachfiles.Attachpostid > 0 && entLibConfigInfo.Cacheattachfiles.Attachpostid < attachmentinfo.Pid)
                        Discuz.Cache.Data.DBCacheService.GetAttachFilesService().ResponseFile(attachmentinfo.Filename, Path.GetFileName(attachmentinfo.Attachment), attachmentinfo.Filetype);
                    else
                        Utils.ResponseFile(Utils.GetMapPath(BaseConfigs.GetForumPath + @"upload/" + attachmentinfo.Filename), Path.GetFileName(attachmentinfo.Attachment), attachmentinfo.Filetype);
                }
                else
                {
                    try //添加try语法, 以防止在并发访问情况下, 服务器端远程链接被关闭后出现应用程序 '警告'(事件查看器)
                    {
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Redirect(attachmentinfo.Filename.Trim());
                        HttpContext.Current.Response.End();
                    }
                    catch { }
                }
            }
        }

        public void GetGoodsAttachInfo(int attachmentid)
        {
            MallPluginBase mpb = MallPluginProvider.GetInstance();
            if (mpb == null)
            {
                AddErrLine("未安装商城插件");
                return;
            }
            goodsattachmentinfo = mpb.GetGoodsAttachmentsByAid(attachmentid);
            if (goodsattachmentinfo == null)
            {
                AddErrLine("不存在的附件ID");
                return;
            }
            // 获取该商品的信息
            goodsinfo = mpb.GetGoodsInfo(goodsattachmentinfo.Goodsid);
            if (goodsinfo == null)
            {
                AddErrLine("不存在的商品ID");
                return;
            }

            forum = Forums.GetForumInfo(mpb.GetCategoriesFid(goodsinfo.Categoryid));
            pagetitle = Utils.RemoveHtml(forum.Name);

            //添加判断特殊用户的代码
            if (!Forums.AllowViewByUserId(forum.Permuserlist, userid) && !Forums.AllowView(forum.Viewperm, usergroupid))
            {
                AddErrLine("您没有浏览该版块的权限");
                if (userid == -1)
                    needlogin = true;
                return;
            }

            //添加判断特殊用户的代码
            if (!UserAuthority.CheckUsertAttachAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                if (userid == -1)
                    needlogin = true;
                return;
            }
            // 检查用户是否拥有足够的阅读权限
            if (goodsattachmentinfo.Readperm > usergroupinfo.Readaccess && goodsattachmentinfo.Uid != userid && !Moderators.IsModer(useradminid, userid, forum.Fid))
            {
                AddErrLine("您的阅读权限不够");
                if (userid == -1)
                    needlogin = true;
                return;
            }
            if (goodsattachmentinfo.Filename.IndexOf("http") < 0 && !File.Exists(Utils.GetMapPath(string.Format(@"{0}upload/{1}", BaseConfigs.GetForumPath, goodsattachmentinfo.Filename))))
            {
                AddErrLine("该附件文件不存在或已被删除");
                return;
            }

            if (goodsattachmentinfo.Filename.IndexOf("http") < 0)
                Utils.ResponseFile(Utils.GetMapPath(string.Format(@"{0}upload/{1}", BaseConfigs.GetForumPath, goodsattachmentinfo.Filename)), Path.GetFileName(goodsattachmentinfo.Attachment), goodsattachmentinfo.Filetype);
            else
                HttpContext.Current.Response.Redirect(goodsattachmentinfo.Filename.Trim());
        }
    }
}