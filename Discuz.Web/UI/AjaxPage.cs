using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Cache;
using Discuz.Config;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;
using Discuz.Common.Generic;
using Newtonsoft.Json;
using System.Web.UI;

namespace Discuz.Web.UI
{
    /// <summary>
    /// Ajax相关功能操作类
    /// </summary>
    public class AjaxPage : Page
    {
        GeneralConfigInfo config;
        public AjaxPage()
        {
            config = GeneralConfigs.GetConfig();
            //如果是Flash提交
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetUrlReferrer()))
            {
                string[] input = DecodeUid(DNTRequest.GetString("input")).Split(','); //下标0为Uid，1为Olid
                UserInfo userInfo = Users.GetUserInfo(TypeConverter.StrToInt((input[0])));
                if (userInfo == null || DNTRequest.GetString("appid") != Utils.MD5(userInfo.Username + userInfo.Password + userInfo.Uid + input[1]))
                    return;
            }
            else if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost())) //如果是跨站提交...
                return;


            string type = DNTRequest.GetString("t");
            if (Utils.InArray(type, "deleteattach,getattachlist,deletepostsbyuidanddays,deletepost,ignorepost,passpost,deletetopic,ignoretopic,passtopic,getimagelist,getblocklist,getpagelist,forumtree,topictree,quickreply,report,getdebatepostpage,confirmbuyattach,getnewpms,getnewnotifications,getajaxforums,checkuserextcredit,diggdebates,imagelist"))
            {
                //如果需要验证用户身份，跳转至继承了PageBase的页面
                try
                {
                    HttpContext.Current.Server.Transfer("sessionajax.aspx?t=" + type + "&reason=" + DNTRequest.GetString("reason"));
                }
                catch //子页面请求错误，期待更好方案
                { }
                return;
            }
            switch (type)
            {
                case "checkusername":
                    CheckUserName();    //检查用户名是否存在
                    break;
                case "album":  //相册
                    GetAlbum();
                    break;
                case "checkrewritename":
                    CheckRewriteName();
                    break;
                case "ratelist":
                    GetRateLogList();	//帖子评分记录
                    break;
                case "smilies":
                    GetSmilies();
                    break;
                case "relatekw":
                    GetRelateKeyword();
                    break;
                case "gettopictags":
                    GetTopicTags();
                    break;
                case "topicswithsametag":
                    GetTopicsWithSameTag();
                    break;
                case "getforumhottags":
                    GetForumHotTags();
                    break;
                case "getspaceposttags":
                    GetSpacePostTags();
                    break;
                case "getspacehottags":
                    GetSpaceHotTags();
                    break;
                case "getphototags":
                    GetPhotoTags();
                    break;
                case "getphotohottags":
                    GetPhotoHotTags();
                    break;
                case "getgoodstradelog":
                    GetGoodsTradeLog(DNTRequest.GetInt("goodsid", 0), DNTRequest.GetInt("pagesize", 0), DNTRequest.GetInt("pageindex", 0), DNTRequest.GetString("orderby", true), DNTRequest.GetInt("ascdesc", 1));
                    break;
                case "getgoodsleavewordbyid":
                    GetGoodsLeaveWordById(DNTRequest.GetInt("leavewordid", 0));
                    break;
                case "getgoodsleaveword":
                    GetGoodsLeaveWord(DNTRequest.GetInt("goodsid", 0), DNTRequest.GetInt("pagesize", 0), DNTRequest.GetInt("pageindex", 0));
                    break;
                case "ajaxgetgoodsratelist":
                    GetGoodsRatesList(DNTRequest.GetInt("uid", 0), DNTRequest.GetInt("uidtype", 0), DNTRequest.GetInt("ratetype", 0), DNTRequest.GetString("filter", true));
                    break;
                case "getmallhottags":
                    GetMallHotTags();
                    break;
                case "gethotgoods":
                    GetHotGoods(DNTRequest.GetInt("days", 0), DNTRequest.GetInt("categoryid", 0), DNTRequest.GetInt("count", 0));
                    break;
                case "getshopinfo": //获取热门或新开的店铺信息
                    GetShopInfoJson(DNTRequest.GetInt("shoptype", 0));
                    break;
                case "getgoodslist":
                    GetGoodsList(DNTRequest.GetInt("categoryid", 0), DNTRequest.GetInt("order", 0), DNTRequest.GetInt("topnumber", 0));
                    break;
                case "gethotdebatetopic":
                    Getdebatesjsonlist("gethotdebatetopic", DNTRequest.GetString("tidlist", true));
                    break;
                case "recommenddebates":
                    Getdebatesjsonlist("recommenddebates", DNTRequest.GetString("tidlist", true));
                    break;
                case "addcommentdebates":
                    ResponseXML(Debates.CommentDabetas(DNTRequest.GetInt("tid", 0), DNTRequest.GetString("commentdebates", true), DNTRequest.IsPost()));
                    break;
                case "getpostinfo":
                    GetPostInfo();
                    break;
                case "getattachpaymentlog"://获取指定符件id的附件交易日志
                    GetAttachPaymentLogByAid(DNTRequest.GetInt("aid", 0));
                    break;
                case "getiplist":
                    GetIpList();
                    break;
                case "getforumtopictypelist":
                    GetForumTopicTypeList();
                    break;
                case "image":
                    GetImage();
                    break;
                case "resetemail":
                    ResetEmail();
                    break;

            }
            if (DNTRequest.GetString("Filename") != "" && DNTRequest.GetString("Upload") != "")
            {
                string uid = DecodeUid(DNTRequest.GetString("input")).Split(',')[0];
                ResponseText(UploadTempAvatar(uid));
                return;
            }
            if (DNTRequest.GetString("avatar1") != "" && DNTRequest.GetString("avatar2") != "" && DNTRequest.GetString("avatar3") != "")
            {
                string uid = DecodeUid(DNTRequest.GetString("input")).Split(',')[0];
                CreateDir(uid);
                if (!(SaveAvatar("avatar1", uid) && SaveAvatar("avatar2", uid) && SaveAvatar("avatar3", uid)))
                {
                    File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                    ResponseText("<?xml version=\"1.0\" ?><root><face success=\"0\"/></root>");
                    return;
                }
                File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                ResponseText("<?xml version=\"1.0\" ?><root><face success=\"1\"/></root>");
                return;
            }
        }

        #region 头像
        /// <summary>
        /// 解码Uid
        /// </summary>
        /// <param name="encodeUid"></param>
        /// <returns></returns>
        private string DecodeUid(string encodeUid)
        {
            return DES.Decode(encodeUid.Replace(' ', '+'), config.Passwordkey);
        }

        /// <summary>
        /// 获取游客的所在地
        /// </summary>
        private void GetIpList()
        {
            try
            {
                string[] ipList = Utils.SplitString(DNTRequest.GetString("iplist"), ",");
                string[] pidList = Utils.SplitString(DNTRequest.GetString("pidlist"), ",");
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                for (int i = 0; i < ipList.Length; i++)
                {
                    sb.Append("'");
                    sb.Append(pidList[i]);
                    sb.Append("|");
                    sb.Append(IpSearch.GetAddressWithIP(ipList[i].Replace("*", "1")));
                    sb.Append("'");
                    sb.Append(",");
                }
                ResponseJSON(sb.ToString().TrimEnd(',') + "]");
            }
            catch //添加try语法, 以防止在并发情况下, 服务器端远程链接被关闭后出现应用程序 '警告'(事件查看器)
            {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Expires = 0;
                System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                System.Web.HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="uid"></param>
        private void CreateDir(string uid)
        {
            uid = Avatars.FormatUid(uid);
            string avatarDir = string.Format("{0}avatars/upload/{1}/{2}/{3}",
                BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2));
            if (!Directory.Exists(Utils.GetMapPath(avatarDir)))
                Directory.CreateDirectory(Utils.GetMapPath(avatarDir));
        }

        /// <summary>
        /// 保存头像文件
        /// </summary>
        /// <param name="avatar"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        private bool SaveAvatar(string avatar, string uid)
        {
            byte[] b = FlashDataDecode(DNTRequest.GetString(avatar));
            if (b.Length == 0)
                return false;
            uid = Avatars.FormatUid(uid);
            string size = "";
            if (avatar == "avatar1")
                size = "large";
            else if (avatar == "avatar2")
                size = "medium";
            else
                size = "small";
            string avatarFileName = string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_{5}.jpg",
                BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), size);
            FileStream fs = new FileStream(Utils.GetMapPath(avatarFileName), FileMode.Create);
            fs.Write(b, 0, b.Length);
            fs.Close();

            //当支持FTP上传头像时,使用FTP上传远程头像
            if (FTPs.GetForumAvatarInfo.Allowupload == 1)
            {
                FTPs ftps = new FTPs();
                string ftpAvatarFileName = string.Format("/avatars/upload/{0}/{1}/{2}/",
                       uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2));
                ftps.UpLoadFile(ftpAvatarFileName, Utils.GetMapPath(avatarFileName), FTPs.FTPUploadEnum.ForumAvatar);
            }
            return true;
        }

        /// <summary>
        /// 解码Flash头像传送的数据
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private byte[] FlashDataDecode(string s)
        {
            byte[] r = new byte[s.Length / 2];
            int l = s.Length;
            for (int i = 0; i < l; i = i + 2)
            {
                int k1 = ((int)s[i]) - 48;
                k1 -= k1 > 9 ? 7 : 0;
                int k2 = ((int)s[i + 1]) - 48;
                k2 -= k2 > 9 ? 7 : 0;
                r[i / 2] = (byte)(k1 << 4 | k2);
            }
            return r;
        }

        /// <summary>
        /// 上传临时头像文件
        /// </summary>
        /// <returns></returns>
        private string UploadTempAvatar(string uid)
        {
            string filename = "avatar_" + uid + ".jpg";
            string uploadUrl = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "upload/";
            string uploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\");
            if (!Directory.Exists(uploadDir + "temp\\"))
                Utils.CreateDir(uploadDir + "temp\\");

            filename = "temp/" + filename;
            HttpContext.Current.Request.Files[0].SaveAs(uploadDir + filename);
            return uploadUrl + filename;
        }
        #endregion


        //private void GetAjaxForumsJsonList()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    List<ForumInfo> forumlist = Forums.GetSubForumList(DNTRequest.GetInt("fid", 0));
        //    sb.Append("[");
        //    if (forumlist != null && forumlist.Count > 0)
        //    {
        //        foreach (ForumInfo info in forumlist)
        //        {
        //            if (config.Hideprivate == 1 && info.Viewperm != "" && !Utils.InArray(usergroupid.ToString(), info.Viewperm))
        //                continue;
        //            sb.Append(string.Format("{{'forumname':'{0}','fid':{1},'parentid':{2},'applytopictype':{3},'topictypeselectoptions':'{4}','postbytopictype':{5}}},", info.Name.Trim(), info.Fid.ToString(), info.Parentid.ToString(), info.Applytopictype.ToString(), Forums.GetCurrentTopicTypesOption(info.Fid, info.Topictypes), info.Postbytopictype.ToString()));
        //        }
        //        if (sb.ToString() != "")
        //            ResponseJSON(sb.ToString().Remove(sb.ToString().Length - 1) + "]");
        //    }
        //    ResponseJSON(sb.Append("]").ToString());
        //}

        private void GetPostInfo()
        {
            PostInfo info = Posts.GetPostInfo(DNTRequest.GetInt("tid", 0), Posts.GetTopicPostInfo(DNTRequest.GetInt("tid", 0)).Pid);
            StringBuilder xmlnode = IsValidGetPostInfo(info);
            if (!xmlnode.ToString().Contains("<error>"))
            {
                xmlnode.Append("<post>\r\n\t");
                xmlnode.AppendFormat("<message>{0}</message>\r\n", info.Message);
                xmlnode.AppendFormat("<tid>{0}</tid>\r\n", info.Tid);
                xmlnode.Append("</post>\r\n\t");
            }
            ResponseXML(xmlnode);
        }

        private StringBuilder IsValidGetPostInfo(PostInfo info)
        {
            StringBuilder xmlnode = new StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return xmlnode;
            }
            if (info == null)
            {
                xmlnode.Append("<error>读取帖子失败</error>");
                return xmlnode;
            }
            return xmlnode;
        }

        private void ResponseText(string text)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }

        private void ResponseText(StringBuilder builder)
        {
            ResponseText(builder.ToString());
        }

        private string ValidatePurview()
        {
            return string.Empty;
        }

        private void GetForumTopicTypeList()
        {
            int fid = DNTRequest.GetInt("fid", 0);
            if (fid <= 0)
                ResponseText("[]");

            ForumInfo forumInfo = Forums.GetForumInfo(fid);
            if (forumInfo == null)
                ResponseText("[]");
            if (string.IsNullOrEmpty(forumInfo.Topictypes))
                ResponseText("[]");

            StringBuilder sb = new StringBuilder("[{'typeid':'0','typename':'分类'}");

            foreach (string topictype in forumInfo.Topictypes.Split('|'))
            {
                if (!Utils.StrIsNullOrEmpty(topictype.Trim()))
                {
                    sb.Append(",{");
                    sb.AppendFormat("'typeid':'{0}','typename':'{1}'", topictype.Split(',')[0], topictype.Split(',')[1]);
                    sb.Append("}");
                }
            }
            sb.Append("]");

            ResponseText(sb);
        }


        /// <summary>
        /// 获取图片标签
        /// </summary>
        private void GetPhotoTags()
        {
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                Response.Write("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return;
            }
            if (DNTRequest.GetInt("photoid", 0) <= 0) return;

            string filename = Utils.GetMapPath(string.Format("{0}cache/photo/{1}/{2}_tags.txt", BaseConfigs.GetForumPath, DNTRequest.GetInt("photoid", 0) / 1000 + 1, DNTRequest.GetInt("photoid", 0)));
            if (!File.Exists(filename))
                AlbumPluginProvider.GetInstance().WritePhotoTagsCacheFile(DNTRequest.GetInt("photoid", 0));

            WriteFile(filename);
        }

        /// <summary>
        /// 获取指定路径下的文件内容并输出
        /// </summary>
        /// <param name="filename">文件所在路径</param>
        private void WriteFile(string filename)
        {
            string tags = "";

            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        tags = sr.ReadToEnd();
                    }
                }
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(tags);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            //HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取图片热门标签
        /// </summary>
        private void GetPhotoHotTags()
        {
            string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + AlbumPluginProvider.GetInstance().PHOTO_HOT_TAG_CACHE_FILENAME);
            if (!File.Exists(filename))
                AlbumPluginProvider.GetInstance().WriteHotTagsListForPhotoJSONPCacheFile(60);

            WriteFile(filename);
        }

        /// <summary>
        /// 获取空间热门标签
        /// </summary>
        private void GetSpaceHotTags()
        {
            string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + SpacePluginProvider.GetInstance().SpaceHotTagJSONPCacheFileName);
            if (!File.Exists(filename))
                SpacePluginProvider.GetInstance().WriteHotTagsListForSpaceJSONPCacheFile(60);

            WriteFile(filename);
        }

        /// <summary>
        /// 获取论坛热门标签
        /// </summary>
        private void GetForumHotTags()
        {
            string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + ForumTags.ForumHotTagJSONPCacheFileName);
            if (!File.Exists(filename))
                ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);

            WriteFile(filename);
        }


        /// <summary>
        /// 空间日志标签缓存文件
        /// </summary>
        private void GetSpacePostTags()
        {
            SpacePluginProvider.GetInstance().GetSpacePostTagsCacheFile(DNTRequest.GetInt("postid", 0));
        }



        /// <summary>
        /// 获取根据Tag的相关主题
        /// </summary>
        private void GetTopicsWithSameTag()
        {
            if (DNTRequest.GetInt("tagid", 0) > 0)
            {
                TagInfo tag = Tags.GetTagInfo(DNTRequest.GetInt("tagid", 0));
                if (tag != null)
                {
                    List<TopicInfo> topics = Topics.GetTopicsWithSameTag(DNTRequest.GetInt("tagid", 0), config.Tpp);
                    StringBuilder builder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                    builder.Append("<root><![CDATA[ \r\n");
                    builder.Append(@"<div class=""tagthread"" style=""width:300px"">
                                <a class=""close"" href=""javascript:;hideMenu()"" title=""关闭""><img src=""images/common/close.gif"" alt=""关闭"" /></a>
                                <h4>标签: ");
                    builder.Append(string.Format("<font color='{1}'>{0}</font>", tag.Tagname, tag.Color));
                    builder.Append("</h4>\r\n<ul>\r\n");
                    foreach (TopicInfo topic in topics)
                    {
                        builder.Append(string.Format(@"<li><a href=""{0}"" target=""_blank"">{1}</a></li>", Urls.ShowTopicAspxRewrite(topic.Tid, 1), topic.Title));
                    }
                    builder.Append(string.Format(@"<li class=""more""><a href=""tags.aspx?tagid={0}"" target=""_blank"">查看更多</a></li>", tag.Tagid));
                    builder.Append("</ul>\r\n");
                    builder.Append(@"</div>
                                ]]></root>");

                    ResponseXML(builder);
                }
            }
        }

        /// <summary>
        /// 读取主题标签缓存文件
        /// </summary>
        private void GetTopicTags()
        {
            if (DNTRequest.GetInt("topicid", 0) > 0)
            {
                StringBuilder dir = new StringBuilder();
                dir.Append(BaseConfigs.GetForumPath);
                dir.Append("cache/topic/magic/");
                dir.Append((DNTRequest.GetInt("topicid", 0) / 1000 + 1).ToString());
                dir.Append("/");
                string filename = Utils.GetMapPath(dir.ToString() + DNTRequest.GetInt("topicid", 0) + "_tags.config");
                if (!File.Exists(filename))
                {
                    ForumTags.WriteTopicTagsCacheFile(DNTRequest.GetInt("topicid", 0));
                }

                WriteFile(filename);
            }
        }

        /// <summary>
        /// 获取关键字分词
        /// </summary>
        private void GetRelateKeyword()
        {
            string title = Utils.UrlEncode(Utils.RemoveHtml(Utils.ClearUBB(DNTRequest.GetString("titleenc").Trim())));
            string content = Utils.RemoveHtml(Utils.ClearUBB(DNTRequest.GetString("contentenc").Trim()));
            content = content.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("　", "");
            content = Utils.GetUnicodeSubString(content, 500, string.Empty);
            content = Utils.UrlEncode(content);

            string xmlContent = Utils.GetSourceTextByUrl(string.Format("http://keyword.discuz.com/related_kw.html?title={0}&content={1}&ics=utf-8&ocs=utf-8", title, content));

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlContent);

            XmlNodeList xnl = xmldoc.GetElementsByTagName("kw");
            StringBuilder builder = new StringBuilder();
            foreach (XmlNode node in xnl)
            {
                builder.AppendFormat("{0} ", node.InnerText);
            }

            StringBuilder xmlBuilder = new StringBuilder(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                            <root><![CDATA[
                                            <script type=""text/javascript"">
                                            var tagsplit = $('tags').value.split(' ');
                                            var inssplit = '{0}';
                                            var returnsplit = inssplit.split(' ');
                                            var result = '';
                                            for(i in tagsplit) {{
                                                for(j in returnsplit) {{
                                                    if(tagsplit[i] == returnsplit[j]) {{
                                                        tagsplit[i] = '';break;
                                                    }}
                                                }}
                                            }}

                                            for(i in tagsplit) {{
                                                if(tagsplit[i] != '') {{
                                                    result += tagsplit[i] + ' ';
                                                }}
                                            }}
                                            $('tags').value = result + '{0}';
                                            </script>
                                            ]]></root>", builder.ToString()));

            ResponseXML(xmlBuilder);
        }

        /// <summary>
        /// 输出表情字符串
        /// </summary>
        private void GetSmilies()
        {
            //如果不是提交...
            if (ForumUtils.IsCrossSitePost()) return;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("{" + Caches.GetSmiliesCache() + "}");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            //HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 检查Rewritename是否存在
        /// </summary>
        private void CheckRewriteName()
        {
            //if (userid == -1) return;

            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder();

            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xmlnode.Append("<result>");
            xmlnode.Append(SpacePluginProvider.GetInstance().CheckSpaceRewriteNameAvailable(DNTRequest.GetString("rewritename").Trim()).ToString());
            xmlnode.Append("</result>");
            ResponseXML(xmlnode);
        }

        /// <summary>
        /// 查询用户名是否存在
        /// </summary>
        public void CheckUserName()
        {
            if (DNTRequest.GetString("username").Trim() == "")
                return;
            string result = "0";
            string tmpUsername = DNTRequest.GetString("username").Trim();
            if (tmpUsername.IndexOf("　") != -1)//用户名中不允许包含全空格符
                result = "1";
            else if (tmpUsername.IndexOf(" ") != -1) //用户名中不允许包含空格
                result = "1";
            else if (tmpUsername.IndexOf(":") != -1) //用户名中不允许包含冒号
                result = "1";
            else if (Users.GetUserId(tmpUsername) > 0) //该用户名已存在  
                result = "1";
            else if ((!Utils.IsSafeSqlString(tmpUsername)) || (!Utils.IsSafeUserInfoString(tmpUsername))) //用户名中存在非法字符
                result = "1";
            else if (tmpUsername.Trim() == PrivateMessages.SystemUserName || ForumUtils.IsBanUsername(tmpUsername, config.Censoruser)) //如果用户名属于禁止名单, 或者与负责发送新用户注册欢迎信件的用户名称相同
                result = "1";

            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            ResponseXML(xmlnode.AppendFormat("<result>{0}</result>", result));
        }

        /// <summary>
        /// 获得帖子评分列表
        /// </summary>
        public void GetRateLogList()
        {
            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");

            //如果不是提交...
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                ResponseXML(xmlnode);
                return;
            }

            try
            {
                List<RateLogInfo> rateList = Posts.GetPostRateLogList(DNTRequest.GetFormInt("pid", 0));
                if (rateList == null || rateList.Count == 0)
                {
                    xmlnode.Append("<error>该帖没有评分记录</error>");
                    ResponseXML(xmlnode);
                    return;
                }
                xmlnode.Append("<data>\r\n");

                List<RateLogInfo> finalRateList = new List<RateLogInfo>();

                //该循环用于将评分列表中多次评分用户的分值聚合成一条数据
                foreach (RateLogInfo info in rateList)
                {
                    Predicate<RateLogInfo> match = new Predicate<RateLogInfo>(delegate(RateLogInfo rateLog) { return rateLog.Uid == info.Uid && rateLog.ExtCredits == info.ExtCredits; });
                    RateLogInfo finalRateInfo = finalRateList.Find(match);
                    if (finalRateInfo == null)
                        finalRateList.Add(info);
                    else
                    {
                        finalRateInfo.Score += info.Score;
                        finalRateInfo.Reason = string.IsNullOrEmpty(finalRateInfo.Reason) ? info.Reason : finalRateInfo.Reason;
                    }
                }

                string[] scorename = Scoresets.GetValidScoreName();
                string[] scoreunit = Scoresets.GetValidScoreUnit();

                int uidCount = 0;
                int previoursUid = 0;
                foreach (RateLogInfo rate in finalRateList)
                {
                    if (previoursUid != rate.Uid)
                        uidCount++;
                    xmlnode.Append("<ratelog>");
                    xmlnode.AppendFormat("\r\n\t<rateid>{0}</rateid>", rate.Id);
                    xmlnode.AppendFormat("\r\n\t<uid>{0}</uid>", rate.Uid);
                    xmlnode.AppendFormat("\r\n\t<username>{0}</username>", rate.UserName.Trim());
                    xmlnode.AppendFormat("\r\n\t<extcredits>{0}</extcredits>", rate.ExtCredits);
                    xmlnode.AppendFormat("\r\n\t<extcreditsname>{0}</extcreditsname>", scorename[rate.ExtCredits]);
                    xmlnode.AppendFormat("\r\n\t<extcreditsunit>{0}</extcreditsunit>", scoreunit[rate.ExtCredits]);
                    xmlnode.AppendFormat("\r\n\t<postdatetime>{0}</postdatetime>", ForumUtils.ConvertDateTime(rate.PostDateTime));
                    xmlnode.AppendFormat("\r\n\t<score>{0}</score>", rate.Score > 0 ? ("+" + rate.Score.ToString()) : rate.Score.ToString());
                    xmlnode.AppendFormat("\r\n\t<reason>{0}</reason>", rate.Reason.Trim());
                    xmlnode.Append("\r\n</ratelog>\r\n");
                    previoursUid = rate.Uid;
                }
                xmlnode.Append("</data>");

                ResponseXML(xmlnode);
                if (DNTRequest.GetFormInt("ratetimes", 0) != uidCount)
                    Posts.UpdatePostRateTimes(DNTRequest.GetFormInt("tid", 0), DNTRequest.GetFormInt("pid", 0).ToString());
            }
            catch //添加try语法, 以防止在并发情况下, 服务器端远程链接被关闭后出现应用程序 '警告'(事件查看器)
            {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Expires = 0;
                System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                System.Web.HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 获取相册导航
        /// </summary>
        public void GetAlbum()
        {
            //如果不是提交...
            if (ForumUtils.IsCrossSitePost())
                return;

            if (DNTRequest.GetQueryInt("albumid", 0) < 1)
                return;

            AlbumPluginProvider.GetInstance().CreateAlbumJsonData(DNTRequest.GetQueryInt("albumid", 0));
            string builder = AlbumPluginProvider.GetInstance().GetAlbumJsonData(DNTRequest.GetQueryInt("albumid", 0));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(builder);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 通过留言id获取留言信息
        /// </summary>
        /// <param name="leavewordid">留言id</param>
        private void GetGoodsLeaveWordById(int leavewordid)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(MallPluginProvider.GetInstance().GetLeaveWordJson(leavewordid));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取指定商品下的留言
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        private void GetGoodsLeaveWord(int goodsid, int pagesize, int pageindex)
        {
            pageindex = (pageindex < 0) ? 1 : pageindex;
            pagesize = (pagesize < 0 || pagesize > 25) ? 25 : pagesize;

            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(MallPluginProvider.GetInstance().GetLeaveWordJson(goodsid, pagesize, pageindex, "id", 0).ToString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取交易日志
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方法</param>
        private void GetGoodsTradeLog(int goodsid, int pagesize, int pageindex, string orderby, int ascdesc)
        {
            pageindex = (pageindex < 0) ? 1 : pageindex;
            pagesize = (pagesize < 0 || pagesize > 25) ? 25 : pagesize;

            HttpContext.Current.Response.Clear();
            if (Utils.InArray(orderby, "lastupdate"))
                HttpContext.Current.Response.Write(MallPluginProvider.GetInstance().GetTradeLogJson(goodsid, pagesize, pageindex, orderby, ascdesc).ToString());

            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取指定用户id的商品评价(信用)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="uidtype">用户类型(1:卖方 2:买方)</param>
        /// <param name="ratetype">评价类型(1:好评 2:中评 3:差评)</param>
        /// <param name="filter">过滤方式(或字段)</param>
        private void GetGoodsRatesList(int uid, int uidtype, int ratetype, string filter)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(MallPluginProvider.GetInstance().GetGoodsRatesJson(uid, uidtype, ratetype, filter));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取论坛热门标签
        /// </summary>
        private void GetMallHotTags()
        {
            string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + MallPluginBase.GoodsHotTagJSONPCacheFileName);
            if (!File.Exists(filename))
                MallPluginProvider.GetInstance().WriteHotTagsListForGoodsJSONPCacheFile(60);

            WriteFile(filename);
        }

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="days">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        private void GetHotGoods(int days, int categroyid, int count)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(MallPluginProvider.GetInstance().GetHotGoodsJsonData(days, categroyid, count));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取热门或新开的店铺信息
        /// </summary>
        /// <param name="shoptype">热门店铺(1:热门店铺, 2 :新开店铺)</param>
        /// <returns></returns>
        private void GetShopInfoJson(int shoptype)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(MallPluginProvider.GetInstance().GetShopInfoJson(shoptype));
            HttpContext.Current.Response.End();
        }


        private void GetGoodsList(int categroyid, int order, int topnumber)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(MallPluginProvider.GetInstance().GetGoodsListJsonData(categroyid, order, topnumber));
            HttpContext.Current.Response.End();
        }


        private void Getdebatesjsonlist(string callback, string tidllist)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(Debates.GetDebatesJsonList(callback, tidllist));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取指定符件id的附件交易日志
        /// </summary>
        /// <param name="aId">指定的附件id</param>
        private void GetAttachPaymentLogByAid(int aid)
        {
            if (aid > 0)
            {
                HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                HttpContext.Current.Response.Expires = -1;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(AttachPaymentLogs.GetAttachPaymentLogJsonByAid(aid));
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 图片缓存方法
        /// </summary>
        private void GetImage()
        {
            //检查参数是否合法
            if (DNTRequest.GetString("aid") == "" || DNTRequest.GetString("size") == "" || DNTRequest.GetString("key") == "")
            {
                HttpContext.Current.Response.Redirect("images/common/none.gif");
                return;
            }
            string forumPath = BaseConfigs.GetBaseConfig().Forumpath;
            //是否在服务器上缓存图片
            bool nocache = DNTRequest.GetString("nocache") == "yes";
            int aid = DNTRequest.GetInt("aid", 0);
            //缩略图缩略方式
            string type = DNTRequest.GetString("type") != "" ? DNTRequest.GetString("type") : "fixwr";
            string[] wxh = DNTRequest.GetString("size").Split('x');
            //宽
            int w = TypeConverter.StrToInt(wxh[0]);
            //高
            int h = TypeConverter.StrToInt(wxh[1]);
            string thumbfile = string.Format("{0}_{1}_{2}.jpg'", aid, w, h);
            string attachurl = forumPath + "cache/thumbnail/";
            //获取缓存图片
            if (!nocache)
            {
                if (File.Exists(Utils.GetMapPath(attachurl + thumbfile)))
                {
                    HttpContext.Current.Response.Redirect(attachurl + thumbfile);
                    return;
                }
            }
            //校验参数正确性
            string hash = Discuz.Common.DES.Encode(aid.ToString() + "," + w.ToString() + "," + h.ToString(), Utils.MD5(aid.ToString())).Replace("+", "[");
            if (hash != DNTRequest.GetString("key"))
            {
                HttpContext.Current.Response.Redirect("images/common/none.gif");
                return;
            }
            AttachmentInfo attInfo = Attachments.GetAttachmentInfo(aid);
            //客户端缓存60分，在firefox下调试状态失效
            HttpContext.Current.Response.Expires = 60;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddMinutes(60);

            if (!Directory.Exists(Utils.GetMapPath(attachurl)))
                Directory.CreateDirectory(Utils.GetMapPath(attachurl));
            //生成缩略图
            if (attInfo.Filename.StartsWith("http://")) //远程附件
            {
                Thumbnail.MakeRemoteThumbnailImage(attInfo.Filename, Utils.GetMapPath(attachurl + thumbfile), w, h);
            }
            else
                Thumbnail.MakeThumbnailImage(Utils.GetMapPath(forumPath + "upload/" + attInfo.Filename), Utils.GetMapPath(attachurl + thumbfile), w, h);
            if (nocache)
            {
                HttpContext.Current.Response.ContentType = "image/jpg";
                HttpContext.Current.Response.BinaryWrite(File.ReadAllBytes(Utils.GetMapPath(attachurl + thumbfile)));
                try
                {
                    File.Delete(Utils.GetMapPath(attachurl + thumbfile));
                }
                catch { }
            }
            else
            {
                HttpContext.Current.Response.Redirect(attachurl + thumbfile);
            }
        }

        private void ResetEmail()
        {
            int uid = DNTRequest.GetInt("uid", -1);
            if (uid <= 0)
            {
                ResponseText("{'text':'非法请求','code':0}");
                return;
            }

            string newEmail = DNTRequest.GetString("newemail");
            if (!Utils.IsValidEmail(newEmail))
            {
                ResponseText("{'text':'E-mail格式不正确','code':0}");
                return;
            }

            UserInfo userInfo = Users.GetUserInfo(uid);
            if (Utils.MD5(string.Concat(userInfo.Password, config.Passwordkey, DNTRequest.GetString("ts"))) != DNTRequest.GetString("auth"))
            {
                ResponseText("{'text':'非法请求','code':0}");
                return;
            }

            //如果接收到的时间戳是在两分钟之前的，则证明该操作已经超过了操作时限
            if (long.Parse(DNTRequest.GetString("ts")) < DateTime.Now.AddMinutes(-2).Ticks)
            {
                ResponseText("{'text':'该操作已经超过了时限,无法执行','code':0}");
                return;
            }

            if (userInfo.Groupid != 8)
            {
                ResponseText("{'text':'该用户不是等待验证的用户','code':0}");
                return;
            }
            if (userInfo.Email != newEmail)
            {
                if (!Users.ValidateEmail(newEmail, uid))
                {
                    ResponseText("{'text':'Email: \"" + newEmail + "\" 已经被其它用户注册使用','code':0}");
                    return;
                }
                userInfo.Email = newEmail;
                Users.UpdateUserProfile(userInfo);
            }
            Emails.DiscuzSmtpMail(userInfo.Username, newEmail, string.Empty, userInfo.Authstr);
            ResponseText("{'text':'验证邮件已经重新发送到您指定的E-mail地址当中','code':1}");
        }


        #region Helper
        /// <summary>
        /// 向页面输出xml内容
        /// </summary>
        /// <param name="xmlnode">xml内容</param>
        private void ResponseXML(System.Text.StringBuilder xmlnode)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "Text/XML";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(xmlnode.ToString());
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            //System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 输出json内容
        /// </summary>
        /// <param name="json"></param>
        private void ResponseJSON(string json)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/json";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(json);
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            //System.Web.HttpContext.Current.Response.End();
        }

        private void ResponseJSON<T>(T jsonobj)
        {
            ResponseJSON(JavaScriptConvert.SerializeObject(jsonobj));
        }
        #endregion

    } //class
} //namespace
