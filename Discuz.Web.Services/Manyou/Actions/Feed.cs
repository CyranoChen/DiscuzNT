using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Entity;
using Discuz.Forum;

using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Feed : ActionBase
    {
        public string PublishTemplatizedAction()
        {
            try
            {
                //JsonParams = "{\"uId\":\"1\",\"appId\":1048438,\"titleTemplate\":\"{actor} \u6dfb\u52a0\u4e86&nbsp;{app}&nbsp;\u5e94\u7528\",\"titleData\":{},\"bodyTemplate\":null,\"bodyData\":[],\"bodyGeneral\":null,\"image1\":null,\"image1Link\":null,\"image2\":null,\"image2Link\":null,\"image3\":null,\"image3Link\":null,\"image4\":null,\"image4Link\":null,\"targetIds\":null,\"privacy\":\"public\",\"hashTemplate\":null,\"hashData\":null}";
                PublishTemplatizedActionParams actionParams = JavaScriptConvert.DeserializeObject<PublishTemplatizedActionParams>(UnicodeToString(JsonParams).Replace("[]", "{}"));//将PHP的空keyvalue数组格式json转换为.net的空keyvalue数组格式

                MiniFeedInfo feedInfo = new MiniFeedInfo();
                feedInfo.Uid = actionParams.Uid;
                feedInfo.UserName = Forum.Users.GetShortUserInfo(actionParams.Uid).Username;
                feedInfo.AppId = actionParams.AppId;
                feedInfo.FeedType = FeedTypeEnum.Application;
                feedInfo.BodyGeneral = actionParams.BodyGeneral ?? string.Empty;
                feedInfo.BodyTemplate = actionParams.BodyTemplate ?? string.Empty;
                feedInfo.TitleTemplate = actionParams.TitleTemplate ?? string.Empty;
                feedInfo.BodyData = actionParams.BodyData.Count == 0 ? "" : JavaScriptConvert.SerializeObject(actionParams.BodyData);
                feedInfo.TitleData = actionParams.TitleData.Count == 0 ? "" : JavaScriptConvert.SerializeObject(actionParams.TitleData);
                feedInfo.Image1Link = actionParams.Image1Link ?? string.Empty;
                feedInfo.Image1Url = actionParams.Image1Url ?? string.Empty;
                feedInfo.Image2Link = actionParams.Image2Link ?? string.Empty;
                feedInfo.Image2Url = actionParams.Image2Url ?? string.Empty;
                feedInfo.Image3Link = actionParams.Image3Link ?? string.Empty;
                feedInfo.Image3Url = actionParams.Image3Url ?? string.Empty;
                feedInfo.Image4Link = actionParams.Image4Link ?? string.Empty;
                feedInfo.Image4Url = actionParams.Image4Url ?? string.Empty;
                return GetResult(MiniFeeds.PublishFeed(feedInfo) > 0);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
