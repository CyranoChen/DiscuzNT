using System;
using System.Web;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Mall;
using System.Text;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 进行论坛统计
    /// </summary>
    
    public class ajaxcall : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int pertask = DNTRequest.GetInt("pertask", 0);
                int lastnumber = DNTRequest.GetInt("lastnumber", 0);
                int startvalue = DNTRequest.GetInt("startvalue", 0);
                int endvalue = DNTRequest.GetInt("endvalue", 0);
                string resultmessage = "";
                switch (Request.Params["opname"])
                {
                    case "UpdatePostSP":
                        AdminForumStats.UpdatePostSP(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "UpdateMyPost":
                        AdminForumStats.UpdateMyPost(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetFourmTopicAPost":
                        //AdminForumStats.ReSetFourmTopicAPost(pertask, ref lastnumber);
                        AdminForumStats.ReSetFourmTopicAPost();
                        resultmessage = "-1";
                        break;
                    case "ReSetUserDigestPosts":
                        //AdminForumStats.ReSetUserDigestPosts(pertask, ref lastnumber);
                        //resultmessage = lastnumber.ToString();
                        AdminForumStats.ReSetUserDigestPosts();
                        resultmessage = "-1";
                        break;
                    case "ReSetUserPosts":
                        AdminForumStats.ReSetUserPosts(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetTopicPosts":
                        AdminForumStats.ReSetTopicPosts(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetFourmTopicAPost_StartEnd":
                        AdminForumStats.ReSetFourmTopicAPost(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetUserDigestPosts_StartEnd":
                        AdminForumStats.ReSetUserDigestPosts(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetUserPosts_StartEnd":
                        AdminForumStats.ReSetUserPosts(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetTopicPosts_StartEnd":
                        AdminForumStats.ResetLastRepliesInfoOfTopics(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ftptest":
                        FTPs ftps = new FTPs();
                        string message = "";
                        bool ok = ftps.TestConnect(DNTRequest.GetString("serveraddress"), DNTRequest.GetInt("serverport", 0), DNTRequest.GetString("username"), 
                            DNTRequest.GetString("password"), DNTRequest.GetInt("timeout", 0), DNTRequest.GetString("uploadpath"), ref message);
                        resultmessage = ok ? "ok" : "远程附件设置测试出现错误！\n描述：" + message;
                        break;
                    case "setapp":
                        APIConfigInfo aci = APIConfigs.GetConfig();
                        aci.Enable = DNTRequest.GetString("allowpassport") == "1";
                        APIConfigs.SaveConfig(aci);
                        resultmessage = "ok";
                        break;
                    case "location":
                        string city = DNTRequest.GetString("city");
                        resultmessage = "ok";
                        DataTable dt = MallPluginProvider.GetInstance().GetLocationsTable();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["country"].ToString() == DNTRequest.GetString("country") && dr["state"].ToString() == DNTRequest.GetString("state") && dr["city"].ToString() == city)
                            {
                               resultmessage = "<img src='../images/false.gif' title='" + city + "已经存在!'>";
                                break;
                            }
                        }
                        break;
                    case "goodsinfo":
                        int goodsid = DNTRequest.GetInt("goodsid", 0);
                        Goodsinfo goodsinfo = MallPluginProvider.GetInstance().GetGoodsInfo(goodsid);
                        if (goodsinfo == null)
                        {
                            resultmessage = "商品不存在!";
                            break;
                        }
                        //GoodsattachmentinfoCollection attachmentinfos = GoodsAttachments.GetGoodsAttachmentsByGoodsid(goodsinfo.Goodsid);
                        //string img = "";
                        //if (attachmentinfos != null)
                        //{
                        //    img = attachmentinfos[0].Filename;
                        //}
                        PostpramsInfo param = new PostpramsInfo();
                        param.Allowhtml = 1;
                        param.Showimages = 1;
                        param.Sdetail = goodsinfo.Message;
                        resultmessage = "<table width='100%'><tr><td>" + UBB.UBBToHTML(param) + "</td></tr></table>";
                        break;
                    case "downloadword":
                        dt = BanWords.GetBanWordList();
                        string words = "";
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                words += dt.Rows[i][2].ToString() + "=" + dt.Rows[i][3].ToString() + "\r\n";
                            }
                        }

                        string filename = "words.txt";
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Buffer = false;
                        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(filename));
                        HttpContext.Current.Response.ContentType = "text/plain";
                        HttpContext.Current.Response.Write(words);
                        HttpContext.Current.Response.End();
                        break;

                    case "gettopicinfo":
                        StringBuilder sb = new StringBuilder();
                        TopicInfo info = Topics.GetTopicInfo(DNTRequest.GetInt("tid", 0));
                        sb.Append("[");
                        if (info != null)
                        {
                           sb.Append(string.Format("{{'tid':{0},'title':'{1}'}}", info.Tid, info.Title));
                        }

                        System.Web.HttpContext.Current.Response.Clear();
                        System.Web.HttpContext.Current.Response.ContentType = "application/json";
                        System.Web.HttpContext.Current.Response.Expires = 0;
                        System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                        System.Web.HttpContext.Current.Response.Write(sb.Append("]").ToString());
                        System.Web.HttpContext.Current.Response.End();
                        break;

                    case "DeletePrivateMessages"://批量删除短消息
                        {
                            resultmessage = PrivateMessages.DeletePrivateMessages(DNTRequest.GetString("isnew") == "true", DNTRequest.GetString("postdatetime"), DNTRequest.GetString("msgfromlist"), DNTRequest.GetString("lowerupper") == "true", DecodeChar(DNTRequest.GetString("subject")), DecodeChar(DNTRequest.GetString("message")), DNTRequest.GetString("isupdateusernewpm") == "true");
                            resultmessage = string.Format("[{{'count':'{0}'}}]", resultmessage);
                            System.Threading.Thread.Sleep(4000);/*暂停4秒，以减轻数据库压力*/
                            break;
                        }
                    case "sendsmtogroup"://批量发送短消息
                        {
                            int start_uid = DNTRequest.GetInt("start_uid", 0);
                            resultmessage = Users.SendPMByGroupidList(DNTRequest.GetString("groupidlist"), DNTRequest.GetInt("topnumber", 0),
                                ref start_uid,
                                DNTRequest.GetString("msgfrom"), DNTRequest.GetInt("msguid", 1), DNTRequest.GetInt("folder", 0),
                                DecodeChar(DNTRequest.GetString("subject")), DNTRequest.GetString("postdatetime"), DecodeChar(DNTRequest.GetString("message"))).ToString();
                            resultmessage = string.Format("[{{'startuid':{0},'count':'{1}'}}]", start_uid, resultmessage);
                            System.Threading.Thread.Sleep(4000);/*暂停4秒，以减轻数据库压力*/
                            break;
                        }
                    case "usergroupsendemail"://批量发送邮件
                        {
                            int start_uid = DNTRequest.GetInt("start_uid", 0);
                            resultmessage = Users.SendEmailByGroupidList(DNTRequest.GetString("groupidlist"), DNTRequest.GetInt("topnumber", 0),
                                ref start_uid, DecodeChar(DNTRequest.GetString("subject")), DecodeChar(DNTRequest.GetString("body"))).ToString();
                            resultmessage = string.Format("[{{'startuid':{0},'count':'{1}'}}]", start_uid, resultmessage);
                            System.Threading.Thread.Sleep(4000);/*暂停4秒，以减轻数据库压力*/
                            break;
                        }
                    case "updateusercreditbyformula"://根据积分公式批量更新用户积分
                        {
                            int start_uid = DNTRequest.GetInt("start_uid", 0);
                            resultmessage = Users.UpdateUserCredits(DecodeChar(DNTRequest.GetString("formula")), start_uid).ToString();
                            resultmessage = string.Format("[{{'startuid':{0},'count':'{1}'}}]", start_uid + 100 /*每次递增100条记录*/, resultmessage);
                            System.Threading.Thread.Sleep(4000);/*暂停4秒，以减轻数据库压力*/
                            break;
                        }
                }
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Expires = 0;
                System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                Response.Write(resultmessage);
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.Expires = -1;
                Response.End();
            }
        }

        public string DecodeChar(string str)
        {
           return str.Replace("_plus_", "+").Replace("_and_", "&").Replace("_equal_", "=");
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}