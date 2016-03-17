using System;
using System.Data;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using System.Data.Common;
using Discuz.Config;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 用户管理页面(目前只做禁言功能使用)
    /// </summary>
    public class useradmin : PageBase 
    {
        #region 页面变量
        /// <summary>
        /// 操作标题
        /// </summary>
        public string operationtitle = "操作提示";
        /// <summary>
        /// 被操作用户Id
        /// </summary>
        public int operateduid = DNTRequest.GetInt("uid", -1);
        /// <summary>
        /// 被操作用户名
        /// </summary>
        public string operatedusername;
        /// <summary>
        /// 禁止用户类型
        /// </summary>
        public int bantype;
        /// <summary>
        /// 操作类型
        /// </summary>
        public string action = DNTRequest.GetQueryString("action");
        /// <summary>
        /// 
        /// </summary>
        private AdminGroupInfo admininfo;
        /// <summary>
        /// 
        /// </summary>
        private ShortUserInfo operateduser;
        /// <summary>
        /// 信息是否充满整个弹出窗
        /// </summary>
        public bool titlemessage = false;
        /// <summary>
        /// 禁止用户的时间期限
        /// </summary>
        public string groupexpiry = "";
        #endregion

        
        protected override void ShowPage()
        {
            pagetitle = "用户管理";

            if (userid == -1)
            {
                AddErrLine("请先登录");
                return;
            }
            if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()) || Utils.StrIsNullOrEmpty(action))
            {
                AddErrLine("非法提交");
                return;
            }
            if (action == "")
            {
                AddErrLine("操作类型参数为空");
                return;
            }
            // 如果拥有管理组身份
            admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            // 如果所属管理组不存在
            if (admininfo == null)
            {
                AddErrLine("你没有管理权限");
                return;
            }
            if (operateduid == -1)
            {
                AddErrLine("没有选择要操作的用户");
                return;
            }
            operateduser = Users.GetShortUserInfo(operateduid);
            if (operateduser == null)
            {
                AddErrLine("选择的用户不存在");
                return;
            }
            if (operateduser.Adminid > 0)
            {
                AddErrLine("无法对拥有管理权限的用户进行操作, 请管理员登录后台进行操作");
                return;
            }
            operatedusername = operateduser.Username;
            

            if (!ispost)
            {
                Utils.WriteCookie("reurl", DNTRequest.GetUrlReferrer());
                if (action == "banuser")
                {
                    operationtitle = "禁止用户";
                    switch (operateduser.Groupid)
                    {
                        case 4:
                            bantype = 1;
                            groupexpiry = "(" + Utils.FormatDate(operateduser.Groupexpiry) + ")";
                            break;
                        case 5: 
                            bantype = 2;
                            groupexpiry = "(" + Utils.FormatDate(operateduser.Groupexpiry) + ")";
                            break;
                        case 6: 
                            bantype = 3;
                            groupexpiry = "(" + Utils.FormatDate(operateduser.Groupexpiry) + ")";
                            break;
                        default:
                            bantype = 0; 
                            break;
                    }
                    if (admininfo.Allowbanuser != 1)
                    {
                        AddErrLine("您没有禁止用户的权限");
                        return;
                    }
                }
            }
            else if (action == "banuser")
            {
                operationtitle = "禁止用户";
                DoBanUserOperation();
            }
        }

        private void DoBanUserOperation()
        {
            ispost = false;
            string actions = "";
            string title = "";
            //判断后台是否设置必须输入理由， 0-不需要 1-必须
            if (usergroupinfo.Reasonpm == 1 && Utils.StrIsNullOrEmpty(DNTRequest.GetString("reason")))
            {
                titlemessage = true;
                AddErrLine("请填写操作原因");
                return;
            }
            int banexpirynew = DNTRequest.GetFormInt("banexpirynew", -1);
            string expday = (banexpirynew == 0) ? "29990101" : string.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(banexpirynew));
            switch (DNTRequest.GetInt("bantype", -1))
            {
                case 0://正常状态
                    //Users.UpdateUserGroup(operateduid, UserCredits.GetCreditsUserGroupId(operateduser.Credits).Groupid);
                    Users.UpdateBanUser(UserCredits.GetCreditsUserGroupId(operateduser.Credits).Groupid, "0", operateduid);
                    title = string.Format("取消对 <a href=\"../../userinfo-{1}.aspx\" target=\"_blank\">{0}</a> 的禁止", operatedusername, operateduid);
                    actions = "取消禁止";
                    break;
                case 1://禁止发言
                    //Users.UpdateUserGroup(operateduid, 4);
                    Users.UpdateBanUser(4, expday, operateduid);
                    title = string.Format("禁止 <a href=\"../../userinfo-{1}.aspx\" target=\"_blank\">{0}</a> 发言", operatedusername, operateduid);
                    actions = "禁止发言";
                    break;
                case 2://禁止访问
                    //Users.UpdateUserGroup(operateduid, 5);
                    Users.UpdateBanUser(5, expday, operateduid);
                    title = string.Format("禁止 <a href=\"../../userinfo-{1}.aspx\" target=\"_blank\">{0}</a> 访问", operatedusername, operateduid);
                    actions = "禁止访问";
                    break;
                default:
                    titlemessage = true;
                    actions = "错误的禁止类型";
                    AddErrLine("错误的禁止类型");
                    return;
            }

            AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(), usergroupinfo.Grouptitle, DNTRequest.GetIP(),
                                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "0", "", "0", title, actions, DNTRequest.GetString("reason").Trim());
            // 收件箱
            //if (DNTRequest.GetFormInt("sendmessage", 0) == 1)
            //{
            //    PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();
            //    privatemessageinfo.Message = Utils.HtmlEncode(string.Format("这是由论坛系统自动发送的通知短消息。操作理由: {0}\r\n\r\n如果您对本管理操作有异议，请与我取得联系。", DNTRequest.GetString("reason").Trim()));
            //    privatemessageinfo.Subject = Utils.HtmlEncode("您被执行 " + actions + " 操作");
            //    privatemessageinfo.Msgto = operateduser.Username;
            //    privatemessageinfo.Msgtoid = operateduid;
            //    privatemessageinfo.Msgfrom = username;
            //    privatemessageinfo.Msgfromid = userid;
            //    privatemessageinfo.New = 1;
            //    privatemessageinfo.Postdatetime = Utils.GetDateTime();
            //    privatemessageinfo.Folder = 0;
            //    PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);
            //}

            ispost = true;
            SetShowBackLink(false);
            SetUrl(Utils.UrlDecode(ForumUtils.GetReUrl()));
            SetMetaRefresh();
            MsgForward("useradmin_succeed",true);
        }
    }
}