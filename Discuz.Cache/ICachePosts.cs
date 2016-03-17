using System;
using System.Data;
using System.Text;

using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Cache.Data
{
    public interface ICachePosts
    {
        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="postTableId">分表ID</param>
        /// <returns>帖子ID</returns>
        void CreatePost(PostInfo postInfo, string postTableId);

        /// <summary>
        /// 更新指定帖子信息
        /// </summary>
        /// <param name="postsInfo">帖子信息</param>
        /// <param name="postTableId">帖子所在分表Id</param>
        /// <returns>更新数量</returns>
        void UpdatePost(PostInfo postsInfo, string postTableId);

        /// <summary>
        /// 删除指定ID的帖子
        /// </summary>
        /// <param name="postTableId">帖子所在分表Id</param>
        /// <param name="pid">帖子ID</param>
        /// <returns>删除数量</returns>
        void DeletePost(string postTableId, int pid);

        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="postTableId">当前分表ID</param>
        /// <param name="postidlist">帖子ID列表</param>
        /// <returns>更新的帖子数量</returns>
        void UpdatePostRateTimes(string postTableId, string postidlist);

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <param name="postTableId">当前分表ID</param>
        /// <returns>指定条件的帖子DataSet</returns>
        List<ShowtopicPagePostInfo> GetPostList(PostpramsInfo postpramsInfo, string postTableId);


        /// <summary>
        /// 更新帖子附件类型
        /// </summary>
        /// <param name="postTableId">当前分表ID</param>
        /// <param name="pid"></param>
        /// <param name="attType"></param>
        void UpdatePostAttachmentType(string postTableId, int pid, float attType);

        /// <summary>
        /// 更新帖子评分
        /// </summary>
        /// <param name="pid">帖子id</param>
        /// <param name="rate">评分</param>
        /// <param name="postTableId">当前分表ID</param>
        /// <returns></returns>
        void UpdatePostRate(int pid, float rate, string postTableId);

        /// <summary>
        /// 取消帖子评分
        /// </summary>
        /// <param name="pidIdList">帖子ID列表</param>
        /// <param name="postTableId">当前分表ID</param>
        void CancelPostRate(string pidIdList, string postTableId);

        /// <summary>
        /// 装载帖子列表
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="postTableId">当前分表ID</param>
        /// <returns></returns>
        List<ShowtopicPagePostInfo> LoadPostList(PostpramsInfo postpramsInfo, string postTableId);


        /// <summary>
        /// 屏蔽帖子内容
        /// </summary>
        /// <param name="tableId">分表Id</param>
        /// <param name="postList">帖子Id列表</param>
        /// <param name="invisible">屏蔽还是解除屏蔽</param>
        void SetPostsBanned(string postTableId, string postList, int invisible);

        /// <summary>
        /// 通过待验证的帖子
        /// </summary>
        /// <param name="postTableId">帖子分表Id</param>
        /// <param name="validatePidList">需要验证的帖子Id列表</param>
        /// <param name="deletePidList">需要删除的帖子Id列表</param>
        /// <param name="ignorePidList">需要忽略的帖子ID列表</param>
        /// <param name="fidList">版块Id列表</param>
        void PassPost(int postTableId, string validatePidList, string deletePidList, string ignorePidList, string fidList);

        /// <summary>
        /// 删除指定用户的帖子
        /// </summary>
        /// <param name="postTableId">帖子分表Id</param>
        /// <param name="uid">用户ID</param>
        void DeletePostByPosterid(int postTableId, int uid);
        
        /// <summary>
        /// 更新帖子作者名称
        /// </summary>
        /// <param name="uid">要更新的帖子作者的Uid</param>
        /// <param name="newUserName">作者的新用户名</param>
        void UpdatePostPoster(int uid, string newUserName);   

        /// <summary>
        /// 删除指定主题的帖子
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableName">帖子分表</param>
        /// <returns></returns>
        void DeleteTopicByTid(int tid, string postTableName);

        /// <summary>
        /// 通过未审核的帖子
        /// </summary>
        /// <param name="postTableId">当前表ID</param>
        /// <param name="pidlist">帖子ID列表</param>
        void PassPost(int postTableId, string pidlist);
    }
}
