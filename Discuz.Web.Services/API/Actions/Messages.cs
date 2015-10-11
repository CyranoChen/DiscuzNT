using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.API.Actions
{
    public class Messages : ActionBase
    {

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <returns></returns>
        public string Send()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("subject,message,to_ids"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }
            string ids = GetParam("to_ids").ToString();

            if (!CheckRequiredParams("to_ids") || !Utils.IsNumericArray(ids.Split(',')))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if ((!CheckRequiredParams("from_id") || !Utils.IsNumeric(GetParam("from_id"))) && Uid < 1)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }
            int fromId = TypeConverter.ObjectToInt(GetParam("from_id"), Uid);

            string message = UBB.ParseUrl(Utils.EncodeHtml(GetParam("message").ToString()));

            string[] to_ids = ids.Split(',');

            string successfulIds = string.Empty;
            foreach (string id in to_ids)
            {
                if (TypeConverter.StrToInt(id) < 1)
                    continue;
                PrivateMessageInfo pm = new PrivateMessageInfo();
                pm.Folder = 0;
                pm.Message = message;
                pm.Msgfrom = Discuz.Forum.Users.GetShortUserInfo(fromId).Username;
                pm.Msgfromid = fromId;
                pm.Msgto = "";//可能需要查询具体的收件人姓名
                pm.Msgtoid = TypeConverter.StrToInt(id);
                pm.New = 1;
                pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                pm.Subject = GetParam("subject").ToString();

                successfulIds += (PrivateMessages.CreatePrivateMessage(pm, 0) > 0) ? (id + ",") : "";
            }
            successfulIds = successfulIds.Length > 0 ? successfulIds.Remove(successfulIds.Length - 1) : successfulIds;

            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", successfulIds);

            MessageSendResponse nsr = new MessageSendResponse();
            nsr.Result = successfulIds;
            return SerializationHelper.Serialize(nsr);
        }

        /// <summary>
        /// 获取短消息列表
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP && Uid < 1)
            {
                //if (Uid < 1)
                //{
                ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                return "";
                //}
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("uid,page_size,page_index"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }
            int uid = GetIntParam("uid");
            int pageSize = GetIntParam("page_size", 10);
            int pageIndex = GetIntParam("page_index", 1);

            List<PrivateMessageInfo> list = PrivateMessages.GetPrivateMessageCollection(uid, 0, pageSize, pageIndex, 1);

            List<Message> newList = new List<Message>();
            foreach (PrivateMessageInfo pm in list)
            {
                Message m = new Message();
                m.MessageId = pm.Pmid;
                m.From = pm.Msgfrom;
                m.FromId = pm.Msgfromid;
                m.MessageContent = pm.Message;
                m.PostDateTime = pm.Postdatetime;
                m.Subject = pm.Subject;

                newList.Add(m);
            }

            MessageGetResponse mgr = new MessageGetResponse();
            mgr.Count = PrivateMessages.GetPrivateMessageCount(uid, 0, 1);
            mgr.List = true;
            mgr.Messages = newList.ToArray();

            if (Format == FormatType.JSON)
                return JavaScriptConvert.SerializeObject(mgr);

            return Util.AddMessageCDATA(SerializationHelper.Serialize(mgr));

        }
    }
}
