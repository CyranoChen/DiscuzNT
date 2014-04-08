using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{

    /// <summary>
    ///	ajax 读取帖子信息
    /// </summary>
    public class ajaxpostinfo : UserControl
    {
        public string title = "";
        public string message = "";
        public bool isexist = false;

        protected internal GeneralConfigInfo config = GeneralConfigs.GetConfig();

        public ajaxpostinfo()
        {
            if (DNTRequest.GetInt("tid", 0) == 0)
                return;
            DataTable dt = Posts.GetPostInfo(DNTRequest.GetString("istopic") == "true", DNTRequest.GetInt("tid", 0), DNTRequest.GetInt("pid", 0));
            GetPostInfo(dt);
            dt.Dispose();

            //    dt.Dispose();
            //int tid = DNTRequest.GetInt("tid", 0);
            //string posttablename = string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, Posts.GetPostTableId(tid));
            ////是否帖子
            //if (DNTRequest.GetString("istopic") == "false")
            //{
            //    int pid = DNTRequest.GetInt("pid", 0);
            //    DataTable dt = Posts.GetPostInfoByPid(posttablename, pid);
            //    GetPostInfo(dt);
            //    dt.Dispose();
            //}
            ////是否是主题
            //if (DNTRequest.GetString("istopic") == "true")
            //{
            //    DataTable dt = Posts.GetMainPostInfo(posttablename, tid);
            //    GetPostInfo(dt);
            //    dt.Dispose();
            //}
        }

        /// <summary>
        /// 获取帖子信息
        /// </summary>
        /// <param name="dt"></param>
        public void GetPostInfo(DataTable dt)
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    isexist = true;
                    PostpramsInfo postPramsInfo = new PostpramsInfo();
                    postPramsInfo.Fid = Convert.ToInt32(dt.Rows[0]["fid"].ToString());
                    postPramsInfo.Tid = Convert.ToInt32(dt.Rows[0]["tid"].ToString());
                    postPramsInfo.Pid = Convert.ToInt32(dt.Rows[0]["pid"].ToString());
                    postPramsInfo.Jammer = 0;
                    postPramsInfo.Attachimgpost = config.Attachimgpost;
                    postPramsInfo.Showattachmentpath = 1;
                    postPramsInfo.Showimages = 1;
                    postPramsInfo.Smiliesinfo = Discuz.Forum.Smilies.GetSmiliesListWithInfo();
                    postPramsInfo.Customeditorbuttoninfo = Discuz.Forum.Editors.GetCustomEditButtonListWithInfo();
                    postPramsInfo.Smiliesmax = config.Smiliesmax;
                    postPramsInfo.Bbcodemode = config.Bbcodemode;

                    postPramsInfo.Smileyoff = Utils.StrToInt(dt.Rows[0]["smileyoff"], 0);
                    postPramsInfo.Bbcodeoff = Utils.StrToInt(dt.Rows[0]["bbcodeoff"], 0);
                    postPramsInfo.Parseurloff = Utils.StrToInt(dt.Rows[0]["parseurloff"], 0);
                    postPramsInfo.Allowhtml = Utils.StrToInt(dt.Rows[0]["htmlon"], 0);
                    postPramsInfo.Sdetail = dt.Rows[0]["message"].ToString();

                    message = dt.Rows[0]["message"].ToString();

                    //是不是加干扰码
                    if (postPramsInfo.Jammer == 1)
                    {
                        message = ForumUtils.AddJammer(message);
                    }

                    postPramsInfo.Sdetail = message;

                    if (!postPramsInfo.Ubbmode)
                    {
                        message = UBB.UBBToHTML(postPramsInfo);
                    }
                    else
                    {
                        message = Utils.HtmlEncode(message);
                    }

                    #region 附件

                    // 处理上传图片图文混排问题
                    if (dt.Rows[0]["attachment"].ToString().Equals("1") || new Regex(@"\[attach\](\d+?)\[\/attach\]", RegexOptions.IgnoreCase).IsMatch(message))
                    {
                        DataTable dtAttachment = Attachments.GetAttachmentListByPid(Utils.StrToInt(dt.Rows[0]["pid"], 0));
                        dtAttachment.Columns.Add("attachimgpost", Type.GetType("System.Int16"));

                        string replacement = "";
                        string filesize = "";

                        foreach (DataRow drAt in dtAttachment.Rows)
                        {
                            if (message.IndexOf("[attach]" + drAt["aid"].ToString() + "[/attach]") != -1)
                            {
                                if (Convert.ToInt64(drAt["filesize"]) > 1024)
                                {
                                    filesize = Convert.ToString(Math.Round(Convert.ToDecimal(drAt["filesize"]) / 1024, 2)) + " K";
                                }
                                else
                                {
                                    filesize = drAt["filesize"].ToString() + " B";
                                }

                                if (Utils.IsImgFilename(drAt["attachment"].ToString().Trim()))
                                {
                                    drAt["attachimgpost"] = 1;

                                    if (postPramsInfo.Showattachmentpath == 1)
                                    {
                                        replacement = "<img src=\"../../upload/" + drAt["filename"].ToString() + "\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt='点击在新窗口浏览图片 CTRL+鼠标滚轮可放大/缩小';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor='hand'; this.alt='点击在新窗口浏览图片 CTRL+Mouse 滚轮可放大/缩小';}\" onclick=\"if(!this.resized) { return true; } else { window.open(this.src); }\" onmousewheel=\"return imgzoom(this);\" />";
                                    }
                                    else
                                    {
                                        replacement = "<img src=\"../../attachment.aspx?attachmentid=" + drAt["aid"].ToString() + "\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt='点击在新窗口浏览图片 CTRL+鼠标滚轮可放大/缩小';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor='hand'; this.alt='点击在新窗口浏览图片 CTRL+Mouse 滚轮可放大/缩小';}\" onclick=\"if(!this.resized) { return true; } else { window.open(this.src); }\" onmousewheel=\"return imgzoom(this);\" />";
                                    }
                                }
                                else
                                {
                                    drAt["attachimgpost"] = 0;
                                    replacement = "<p><img alt=\"\" src=\"../../images/attachicons/attachment.gif\" border=\"0\" /><span class=\"bold\">附件</span>: <a href=\"../../attachment.aspx?attachmentid=" + drAt["aid"].ToString() + "\" target=\"_blank\">" + drAt["attachment"].ToString().Trim() + "</a> (" + drAt["postdatetime"].ToString() + ", " + filesize + ")<br />该附件被下载次数 " + drAt["downloads"].ToString() + "</p>";
                                }

                                message = message.Replace("[attach]" + drAt["aid"].ToString() + "[/attach]", replacement);
                            }
                        }
                        dtAttachment.Dispose();
                    }

                    #endregion

                    title = Utils.RemoveHtml(dt.Rows[0]["title"].ToString().Trim());
                }
            }
        }
    }
}