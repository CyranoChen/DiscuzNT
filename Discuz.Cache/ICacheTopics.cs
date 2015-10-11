using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Discuz.Entity;

namespace Discuz.Cache.Data
{
    public interface ICacheTopics
    {
        /// <summary>
        /// 创建新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>返回主题ID</returns>
        int CreateTopic(TopicInfo topicInfo);
        /// <summary>
        /// 获得主题信息
        /// </summary>
        /// <param name="tid">要获得的主题ID</param>
        /// <param name="fid">版块ID</param>
        /// <param name="mode">模式选择, 0=当前主题, 1=上一主题, 2=下一主题</param>
        TopicInfo GetTopicInfo(int tid, int fid, byte mode);
        ///// <summary>
        ///// 获取置顶主题列表
        ///// </summary>
        ///// <param name="fid">版块id</param>
        ///// <param name="pageIndex">页号</param>
        ///// <param name="pageSize">每页条数</param>
        ///// <param name="tids">置顶主题Id列表</param>
        ///// <returns></returns>
        Discuz.Common.Generic.List<TopicInfo> GetTopTopicList(int fid, int pageSize, int pageIndex, string tidList);
        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>成功返回1，否则返回0</returns>
        int UpdateTopic(TopicInfo topicInfo);
        /// <summary>
        /// 列新主题的回复数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableId">当前帖子分表Id</param>
        void UpdateTopicReplyCount(int tid);
        /// <summary>
        /// 更新主题为已被管理
        /// </summary>
        /// <param name="tidList">主题id列表</param>
        /// <param name="moderated">管理操作id</param>
        /// <returns>成功返回1，否则返回0</returns>
        int UpdateTopicModerated(string tidList, int moderated);
        /// <summary>
        /// 将主题设置为隐藏主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns></returns>
        int UpdateTopicHide(int tid);
        /// <summary>
        /// 获得一般主题信息列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="pageSize">每页显示主题数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="startNumber">置顶帖数量</param>
        /// <param name="condition">条件</param>
        /// <returns>主题信息列表</returns>
        Discuz.Common.Generic.List<TopicInfo> GetTopicList(int fid, int pageSize, int pageIndex, int startNumber);
        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        void PassAuditNewTopic(int tid);
        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        void PassAuditNewTopic(string tidList);
        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="ignoreTidList">忽略的主题列表</param>
        /// <param name="validateTidList">需要验证的主题列表</param>
        /// <param name="deleteTidList">删除的主题列表</param>
        /// <param name="fidList">版块列表</param>
        void PassAuditNewTopic(string ignoreTidList, string validateTidList, string deleteTidList, string fidList);
        /// <summary>
        /// 批量更新关注列表
        /// </summary>
        /// <param name="tidList">主题列表</param>
        /// <param name="attention">关注/取消关注(1/0)</param>
        void UpdateTopicAttentionByTidList(string tidList, int attention);
        /// <summary>
        /// 批量更新关注列表
        /// </summary>
        /// <param name="fidList">版块列表</param>
        /// <param name="attention">关注/取消关注(1/0)</param>
        /// <param name="datetime">时间</param>
        void UpdateTopicAttentionByFidList(string fidList, int attention, string datetime);
        /// <summary>
        /// 更新主题附件类型
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
        /// <returns></returns>
        int UpdateTopicAttachmentType(int tid, int attType);
        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <param name="fid">版块id</param>
        /// <param name="topicType">要绑定的主题类型</param>
        /// <returns></returns>
        int UpdateTopic(string topicList, int fid, int topicType);
        /// <summary>
        /// 删除关闭的主题
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <returns></returns>
        int UpdateTopic(int fid, string topicList);
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int DeleteTopic(int tid);
        /// <summary>
        /// 更新主题最后发帖人Id
        /// </summary>
        /// <param name="tid"></param>
        void UpdateTopicLastPosterId(int tid);


        void DeleteTopicByPosterid(int uid);
        /// <summary>
        /// 清除主题分类
        /// </summary>
        /// <param name="typeid">主题分类Id</param>
        void ClearTopicType(int typeid);
        /// <summary>
        /// 设置主题分类
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="value">分类ID</param>
        /// <returns></returns>
        bool SetTypeid(string topiclist, int value);
        /// <summary>
        /// 设置主题属性
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="value">主题属性</param>
        /// <returns></returns>
        bool SetDisplayorder(string topiclist, int value);
        /// <summary>
        /// 更新主题最后回复人
        /// </summary>
        /// <param name="uid">最后回复人的Uid</param>
        /// <param name="newUserName">最后回复人的新用户名</param>
        void UpdateTopicLastPoster(int uid, string newUserName);
        /// <summary>
        /// 更新主题作者
        /// </summary>
        /// <param name="posterid">作者Id</param>
        /// <param name="poster">作者的新名称</param>
        void UpdateTopicPoster(int posterid, string poster);
        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postcount">帖子数</param>
        /// <param name="lastpostid">最后发帖人</param>
        /// <param name="lastpost">最后发帖时间</param>
        /// <param name="lastposterid">最后发帖人ID</param>
        /// <param name="poster">最后发帖人</param>
        void UpdateTopic(int tid, int postcount, int lastpostid, string lastpost, int lastposterid, string poster);
        /// <summary>
        /// 得到论坛中主题总数;
        /// </summary>
        /// <returns>主题总数</returns>
        int GetTopicCount();

        #region 注:下面方法来自TopicAdmins
        /// <summary>
        /// 设置主题指定字段的属性值(字符型)
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        int SetTopicStatus(string topiclist, string field, string intValue);
        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topicList">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        int SetTopicClose(string topicList, short intValue);
        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topiclist">要删除的主题ID列表</param>
        /// <returns></returns>
        int DeleteTopicByTidList(string topicList);
        /// <summary>
        /// 复制主题链接,因为该操作无效获取新增主题ID信息，所以只能变通，使用获取新增主题之前的最大TID来批量添加TTCACHE信息
        /// </summary>
        /// <param name="oldfid"></param>
        /// <param name="topicList"></param>
        /// <returns></returns>
        int CopyTopicLink(int maxtid);
        /// <summary>
        /// 修复主题
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="posttable"></param>
        /// <returns></returns>
        int RepairTopics(string topicList);
        /// <summary>
        /// 重设主题类型
        /// </summary>
        /// <param name="topictypeid">主题类型</param>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <returns></returns>
        int ResetTopicTypes(int topictypeid, string topiclist);
        /// <summary>
        /// 设置主题鉴定信息
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="identify"></param>
        void IdentifyTopic(string topiclist, int identify);
        /// <summary>
        /// 设置主题的下沉和提升
        /// </summary>
        /// <param name="tidList"></param>
        /// <param name="lastpostid"></param>
        void SetTopicsBump(string tidList, int lastpostid);
        /// <summary>
        /// 删除关闭主题
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="tidList"></param>
        void DeleteClosedTopics(int fid, string tidList);
        /// <summary>
        /// 重置缓存数据（从数据库中获取主题信息并更新到TTCACHE中）
        /// </summary>
        /// <param name="tid"></param>
        void ResetTopicByTid(int tid);
        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        void ReSetClearMove();
        /// <summary>
        /// 删除版块
        /// </summary>
        void DeleteForumTopic(int fid);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="delPosts">是否删除帖子</param>
        void DeleteUserTopic(int uid, bool delPosts);
        /// <summary>
        /// 通过未审核的帖子
        /// </summary>
        /// <param name="currentPostTableId">当前表ID</param>
        /// <param name="pidlist">帖子ID列表</param>
        void PassPostTopic(int currentPostTableId, string pidlist);
        /// <summary>
        /// 根据分表名更新主题的最后回复等信息
        /// </summary>
        /// <param name="postTableID">当前表ID</param>
        void ResetLastRepliesInfoOfTopics(int postTableID);
        
        #endregion
    }
}
