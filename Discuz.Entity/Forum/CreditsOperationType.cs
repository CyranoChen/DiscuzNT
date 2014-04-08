using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 积分操作类型，如发表主题等
    /// </summary>
    public enum CreditsOperationType
    {
        /// <summary>
        /// 作者发新主题增加的积分数, 如果该主题被删除, 作者积分也会按此标准相应减少
        /// </summary>
        PostTopic = 4,
        /// <summary>
        /// 作者发新回复增加的积分数, 如果该回复被删除, 作者积分也会按此标准相应减少
        /// </summary>
        PostReply = 5,
        /// <summary>
        /// 主题被加入精华时单位级别作者增加的积分数(根据精华级别乘以1～3), 如果该主题被移除精华, 作者积分也会按此标准相应减少
        /// </summary>
        Digest = 6,
        /// <summary>
        /// 用户每上传一个附件增加的积分数, 如果该附件被删除, 发布者积分也会按此标准相应减少
        /// </summary>
        UploadAttachment = 7,
        /// <summary>
        /// 用户每下载一个附件扣除的积分数. 注意: 如果允许游客组下载附件, 本策略将可能被绕过
        /// </summary>
        DownloadAttachment = 8,
        /// <summary>
        /// 用户每发送一条短消息扣除的积分数
        /// </summary>
        SendMessage = 9,
        /// <summary>
        /// 用户每进行一次帖子搜索或短消息搜索扣除的积分数
        /// </summary>
        Search = 10,
        /// <summary>
        /// 用户每成功进行一次交易后增加的积分数
        /// </summary>
        TradeSucceed = 11,
        /// <summary>
        /// 用户每参与一次投票后增加的积分数
        /// </summary>
        Vote = 12,
        /// <summary>
        /// 用户每邀请成功一个用户注册后增加的积分数
        /// </summary>
        Invite = 13,
        /// <summary>
        /// 管理员删除用户帖子时(扣减用户积分数)
        /// </summary>
        DeletePost = 14,
    }
}
