using System;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Collections;

using Discuz.Common.Generic;
using Discuz.Entity;

namespace Discuz.Data
{
    public partial interface IDataProvider
    {
        /// <summary>
        /// 添加广告信息
        /// </summary>
        /// <param name="available">广告是否有效</param>
        /// <param name="type">广告类型</param>
        /// <param name="displayOrder">显示顺序</param>
        /// <param name="title">广告标题</param>
        /// <param name="targets">投放位置</param>
        /// <param name="parameters">相关参数</param>
        /// <param name="code">广告代码</param>
        /// <param name="startDateTime">起始日期</param>
        /// <param name="endDateTime">结束日期</param>
        void AddAdInfo(int available, string type, int displayOrder, string title, string targets, string parameters, string code, string startTime, string endTime);
        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="announcementInfo">公告对象</param>
        /// <returns></returns>
        int CreateAnnouncement(AnnouncementInfo announcementInfo);
        /// <summary>
        /// 添加附件类型
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="maxSize"></param>
        void AddAttchType(string extension, string maxSize);
        /// <summary>
        /// 添加自定义Discuz!NT代码
        /// </summary>
        /// <param name="available">是否启用</param>
        /// <param name="tag">标签</param>
        /// <param name="icon">图标</param>
        /// <param name="replacement">替换内容</param>
        /// <param name="example">示例</param>
        /// <param name="explanation">说明</param>
        /// <param name="param">参数</param>
        /// <param name="nest">嵌套次数</param>
        /// <param name="paramsDescription">参数描述</param>
        /// <param name="paramsDefaultValue">参数默认值</param>
        void AddBBCCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescript, string paramsDefvalue);
        /// <summary>
        /// 添加积分日志
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fromTo">来自/到</param>
        /// <param name="sendCredits">付出积分类型</param>
        /// <param name="receiveCredits">得到积分类型</param>
        /// <param name="send">付出积分数额</param>
        /// <param name="receive">得到积分数额</param>
        /// <param name="payDate">时间</param>
        /// <param name="operation">积分操作(1=兑换, 2=转帐)</param>
        /// <returns>执行影响的行</returns>
        int AddCreditsLog(int uid, int fromTo, int sendCredits, int receiveCredits, float send, float receive, string payDate, int operation);
        /// <summary>
        /// 添加错误登录次数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        int AddErrLoginCount(string ip);
        /// <summary>
        /// 添加错误登录记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        int AddErrLoginRecord(string ip);
        /// <summary>
        /// 添加友情链接
        /// </summary>
        /// <param name="displayOrder">序号</param>
        /// <param name="name">链接名称</param>
        /// <param name="url">链接地址</param>
        /// <param name="note">注释</param>
        /// <param name="logo">图片地址</param>
        /// <returns></returns>
        int AddForumLink(int displayOrder, string name, string url, string note, string logo);
        /// <summary>
        /// 添加勋章
        /// </summary>
        /// <param name="name">勋章名称</param>
        /// <param name="available">是否可用</param>
        /// <param name="image">图片名称</param>
        void AddMedal(string name, int available, string image);
        /// <summary>
        /// 添加勋章日志
        /// </summary>
        /// <param name="adminId">管理员Id</param>
        /// <param name="adminName">管理员名称</param>
        /// <param name="ip">IP</param>
        /// <param name="userName">授予人名称</param>
        /// <param name="uid">授予人Id</param>
        /// <param name="action">授予方式说明</param>
        /// <param name="medalId">勋章Id</param>
        /// <param name="reason">理由</param>
        void AddMedalslog(int adminId, string adminName, string ip, string userName, int uid, string actions, int medals, string reason);
        /// <summary>
        /// 添加版主
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="fid">板块ID</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="inherited"></param>
        void AddModerator(int uid, int fid, int displayOrder, int inherited);
        /// <summary>
        /// 添加在线用户组图例
        /// </summary>
        /// <param name="grouptitle"></param>
        void AddOnlineList(string grouptitle);
        /// <summary>
        /// 添加在线用户
        /// </summary>
        /// <param name="onlineuserinfo">在组用户信息内容</param>
        /// <param name="timeout">过期时间</param>
        /// <returns></returns>
        int AddOnlineUser(OnlineUserInfo onlineUserInfo, int timeOut, int deletingFrequency);
        /// <summary>
        /// 增加父版块主题数
        /// </summary>
        /// <param name="fpIdList">父板块id列表</param>
        /// <param name="topics">主题数</param>     
        void AddParentForumTopics(string fpIdList, int topics);
        /// <summary>
        /// 添加积分交易日志
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="posterId">发帖人ID</param>
        /// <param name="price">售价</param>
        /// <param name="netAmount">净收入</param>
        /// <returns></returns>
        int CreatePaymentLog(int uid, int tid, int posterId, int price, float netAmount);
        /// <summary>
        /// 添加帖子分表
        /// </summary>
        /// <param name="description"></param>
        /// <param name="mintid"></param>
        /// <param name="maxtid"></param>
        void AddPostTableToTableList(string description, int minTid, int maxTid);
        /// <summary>
        /// 添加表情
        /// </summary>
        /// <param name="id">表情Id</param>
        /// <param name="displayOrder">显示顺序</param>
        /// <param name="type">分类</param>
        /// <param name="code">快捷编码</param>
        /// <param name="url">图片地址</param>
        void AddSmiles(int id, int displayOrder, int type, string code, string url);
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <param name="directory">模板文件所在目录</param>
        /// <param name="copyright">模板版权文字</param>
        /// <returns></returns>
        int AddTemplate(string templateName, string directory, string copyRight);
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="name">模版名称</param>
        /// <param name="directory">模版目录</param>
        /// <param name="copyRight">版权信息</param>
        /// <param name="author">作者</param>
        /// <param name="createDate">创建日期</param>
        /// <param name="ver">版本</param>
        /// <param name="forDntVer">适用论坛版本</param>
        void AddTemplate(string name, string directory, string copyRight, string author, string createDate, string ver, string forDntVer);
        /// <summary>
        /// 创建用户组信息
        /// </summary>
        /// <param name="userGroupInfo">用户组信息</param>
        void AddUserGroup(UserGroupInfo userGroupInfo);
        /// <summary>
        /// 添加访问日志
        /// </summary>
        /// <param name="uid">用户UID</param>
        /// <param name="userName">用户名</param>
        /// <param name="groupId">所属组ID</param>
        /// <param name="groupTitle">所属组名称</param>
        /// <param name="ip">IP地址</param>
        /// <param name="actions">动作</param>
        /// <param name="others"></param>
        void AddVisitLog(int uid, string userName, int groupId, string groupTitle, string ip, string actions, string others);
        /// <summary>
        /// 添加词语过滤
        /// </summary>
        /// <param name="userName">创建管理员用户名</param>
        /// <param name="find">查找词</param>
        /// <param name="replacement">替换内容</param>
        /// <returns></returns>
        int AddWord(string userName, string find, string replacement);
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="backUpPath">备份文件路径</param>
        /// <param name="serverName">服务器名称</param>
        /// <param name="userName">数据库用户名</param>
        /// <param name="passWord">数据库密码</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="strFileName">备份文件名</param>
        /// <returns></returns>
        string BackUpDatabase(string backUpPath, string serverName, string userName, string passWord, string strDbName, string strFileName);
        /// <summary>
        /// 批量设置版块信息
        /// </summary>
        /// <param name="forumInfo">复制的论坛信息</param>
        /// <param name="bsp">是否要批量设置的信息字段</param>
        /// <param name="fidList">目标论坛(fid)串</param>
        /// <returns></returns>
        bool BatchSetForumInf(ForumInfo forumInfo, BatchSetParams bsp, string fidList);
        /// <summary>
        /// 购买主题
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="posterId">发帖人ID</param>
        /// <param name="price">售价</param>
        /// <param name="netAmount">净收入</param>
        /// <param name="creditsTrans">要更新的扩展积分ID</param>
        void BuyTopic(int uid, int tid, int posterId, int price, float netAmount, int creditsTrans);
        /// <summary>
        /// 更改用户管理权限Id
        /// </summary>
        /// <param name="adminId">管理组Id</param>
        /// <param name="groupId">用户组Id</param>
        void UpdateUserAdminIdByGroupId(int adminId, int groupId);
        /// <summary>
        /// 更改用户组
        /// </summary>
        /// <param name="sourceUserGroupId">源组Id</param>
        /// <param name="targetUserGroupId">目标组Id</param>
        void ChangeUsergroup(int sourceUserGroupId, int targetUserGroupId);
        /// <summary>
        /// 更改用户组
        /// </summary>
        /// <param name="groupId">目标组</param>
        /// <param name="uidList">用户列表</param>
        void ChangeUserGroupByUid(int groupId, string uidList);
        /// <summary>
        /// 检查Email和安全问题
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">email</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        IDataReader CheckEmailAndSecques(string userName, string email, string secques);
        /// <summary>
        /// 检查收藏是否已经存在
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="tid">项Id</param>
        /// <param name="type">类型: 相册, 日志, 主题</param>
        /// <returns></returns>
        int CheckFavoritesIsIN(int uid, int tid, byte type);
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="originalPassWord">是否为未MD5密码</param>
        /// <returns>如果正确则返回uid</returns>
        IDataReader CheckPassword(string userName, string passWord, bool originalPassWord);
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="passWord">密码</param>
        /// <param name="originalPassword">是否非MD5密码</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        IDataReader CheckPassword(int uid, string passWord, bool originalPassword);
        /// <summary>
        /// 检查密码和安全问题
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="originalPassword">是否非MD5密码</param>
        /// <param name="secques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        IDataReader CheckPasswordAndSecques(string userName, string passWord, bool originalPassword, string secques);
        /// <summary>
        /// 检查评分状态
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="pid">帖子id</param>      
        /// <returns></returns>
        string CheckRateState(int userId, string pid);
        /// <summary>
        /// 检查用户积分是否足够
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="values">扩展积分</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <returns></returns>
        bool CheckUserCreditsIsEnough(int uid, float[] values, int pos, int mount);
        /// <summary>
        /// 检查用户积分是否足够
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="values">扩展积分</param>
        /// <returns></returns>
        bool CheckUserCreditsIsEnough(int uid, float[] values);
        /// <summary>
        /// 检查用户验证码
        /// </summary>
        /// <param name="olId">在组用户ID</param>
        /// <param name="verifyCode">验证码</param>
        /// <param name="newverifyCode">新验证码</param>
        /// <returns></returns>
        bool CheckUserVerifyCode(int olId, string verifyCode, string newverifyCode);
        /// <summary>
        /// 清空指定用户的认证串
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        void ClearAuthstrByUidlist(string uidList);
        /// <summary>
        /// 清空数据库日志
        /// </summary>
        /// <param name="dbName"></param>
        void ClearDBLog(string dbName);
        /// <summary>
        /// 清除用户所发的帖子
        /// </summary>
        /// <param name="uid"></param>
        void ClearPosts(int uid);
        /// <summary>
        /// 合并版块
        /// </summary>
        /// <param name="sourceFid"></param>
        /// <param name="targetFid"></param>
        /// <param name="fidList"></param>
        void CombinationForums(string sourceFid, string targetFid, string fidList);
        /// <summary>
        /// 合并用户
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="targetUserInfo">目标用户</param>
        /// <param name="srcUserInfo">要合并用户</param>
        void CombinationUser(string postTableName, UserInfo targetUserInfo, UserInfo srcUserInfo);
        /// <summary>
        /// 确认全文索引是否启用
        /// </summary>
        void ConfirmFullTextEnable();
        /// <summary>
        /// 复制主题链接
        /// </summary>
        /// <param name="oldFid"></param>
        /// <param name="topicList"></param>
        /// <returns></returns>
        int CopyTopicLink(int oldFid, string topicList);
        /// <summary>
        /// 创建一个新的管理组信息
        /// </summary>
        /// <param name="adminGroupsInfo">要添加的管理组信息</param>
        /// <returns>更改记录数</returns>
        int CreateAdminGroupInfo(AdminGroupInfo adminGroupsInfo);
        /// <summary>
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类</param>
        /// <returns>附件id</returns>
        int CreateAttachment(AttachmentInfo attachmentInfo);
        /// <summary>
        /// 创建收藏
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="type">收藏类型</param>
        /// <returns>创建成功返回 1 否则返回 0</returns>	
        int CreateFavorites(int uid, int tid, byte type);
        /// <summary>
        /// 建立全文索引
        /// </summary>
        /// <param name="dbName">数据库名</param>
        int CreateFullTextIndex(string dbName);
        /// <summary>
        /// 创建在线表记录(本方法在应用程序初始化时被调用)
        /// </summary>
        /// <returns></returns>
        int CreateOnlineTable();
        /// <summary>
        /// 创建或填充索引
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="postsId"></param>
        /// <returns></returns>
        bool CreateORFillIndex(string dbName, string postsId);
        /// <summary>
        /// 创建一个投票
        /// </summary>
        /// <param name="pollInfo">投票信息</param>
        /// <returns></returns>
        int CreatePoll(PollInfo pollInfo);
        /// <summary>
        /// 创建投票项
        /// </summary>
        /// <param name="pollOptionInfo">投票项</param>
        /// <returns></returns>
        int CreatePollOption(PollOptionInfo pollOptionInfo);
        /// <summary>
        /// 更新投票项
        /// </summary>
        /// <param name="pollOptionInfo">投票项</param>
        /// <returns></returns>
        bool UpdatePollOption(PollOptionInfo pollOptionInfo);
        /// <summary>
        /// 删除指定的投票项
        /// </summary>
        /// <param name="pollOptionInfo">投票项</param>
        bool DeletePollOption(PollOptionInfo pollOptionInfo);
        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="postTableId">分表ID</param>
        /// <returns></returns>
        int CreatePost(PostInfo postInfo, string postTableId);
        /// <summary>
        /// 创建帖子存储过程
        /// </summary>
        /// <param name="sqlTemplate"></param>
        void CreatePostProcedure(string sqlTemplate);
        /// <summary>
        /// 构建相应表及全文索引
        /// </summary>
        /// <param name="tableName"></param>
        void CreatePostTableAndIndex(string tableName);
        /// <summary>
        /// 创建短消息
        /// </summary>
        /// <param name="privateMessageInfo">短消息内容</param>
        /// <param name="saveToSentBox">设置短消息是否在发件箱保留(0为不保留, 1为保留)</param>
        /// <returns>短消息在数据库中的pmid</returns>
        int CreatePrivateMessage(PrivateMessageInfo privateMessageInfo, int saveToSentBox);
        /// <summary>
        /// 创建搜索缓存
        /// </summary>
        /// <param name="cacheInfo">搜索缓存信息</param>
        /// <returns>搜索缓存id</returns>
        int CreateSearchCache(SearchCacheInfo cacheInfo);
        /// <summary>
        /// 创建新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>返回主题ID</returns>
        int CreateTopic(TopicInfo topicInfo);
        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        int CreateUser(UserInfo userinfo);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>是否更新成功</returns>
        bool UpdateUser(UserInfo userInfo);
        /// <summary>
        /// 将用户的未读短信息数量减小一个指定的值
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="subval">短消息将要减小的值,负数为加</param>
        /// <returns>更新记录个数</returns>
        int DecreaseNewPMCount(int uid, int subVal);
        /// <summary>
        /// 删除指定的管理组信息
        /// </summary>
        /// <param name="adminGid">管理组ID</param>
        /// <returns>更改记录数</returns>
        int DeleteAdminGroupInfo(short adminGid);
        /// <summary>
        /// 删除广告列表            
        /// </summary>
        /// <param name="aidList">广告列表Id</param>
        void DeleteAdvertisement(string aidList);
        /// <summary>
        /// 删除通告
        /// </summary>
        /// <param name="idList">逗号分隔的id列表字符串</param>
        int DeleteAnnouncements(string idList);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="aidList"></param>
        /// <returns></returns>
        int DeleteAttachment(string aidList);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="aId"></param>
        /// <returns></returns>
        int DeleteAttachment(int aId);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int DeleteAttachmentByTid(int tid);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="tidList"></param>
        /// <returns></returns>
        int DeleteAttachmentByTid(string tidList);
        /// <summary>
        /// 删除附件类型
        /// </summary>
        /// <param name="attchTypeIdList">附件类型Id列表</param>
        void DeleteAttchType(string attchTypeIdList);
        /// <summary>
        /// 删除自定义Discuz!NT代码
        /// </summary>
        /// <param name="idlist"></param>
        void DeleteBBCode(string idList);
        /// <summary>
        /// 删除关闭的主题
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <returns></returns>
        int DeleteClosedTopics(int fid, string topicList);
        /// <summary>
        /// 删除指定ip地址的登录错误日志
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>int</returns>
        int DeleteErrLoginRecord(string ip);
        /// <summary>
        /// 删除过期的搜索缓存
        /// </summary>
        void DeleteExpriedSearchCache();
        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fidList">要删除的收藏信息id列表</param>
        /// <param name="type">收藏类型</param>
        /// <returns>删除的条数．出错时返回 -1</returns>
        /// <returns></returns>
        int DeleteFavorites(int uid, string fidList, byte type);
        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="forumLinkIdList">链接ID列表</param>
        /// <returns></returns>
        int DeleteForumLink(string forumLinkIdList);
        /// <summary>
        /// 删除板块
        /// </summary>
        /// <param name="postName">分表名称</param>
        /// <param name="fid">版块Id</param>
        void DeleteForumsByFid(string postName, string fid);
        /// <summary>
        /// 删除勋章日志
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool DeleteMedalLog(string condition);
        /// <summary>
        /// 删除勋章日志
        /// </summary>
        /// <returns></returns>
        bool DeleteMedalLog();
        /// <summary>
        /// 删除版主
        /// </summary>
        /// <param name="uid"></param>
        void DeleteModerator(int uid);
        /// <summary>
        /// 删除版主
        /// </summary>
        /// <param name="fid">版块Id</param>
        void DeleteModeratorByFid(int fid);
        /// <summary>
        /// 删除版主日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        bool DeleteModeratorLog(string condition);
        /// <summary>
        /// 删除在线图例
        /// </summary>
        /// <param name="groupId">用户组Id</param>
        void DeleteOnlineList(int groupId);
        /// <summary>
        /// 删除交易日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        bool DeletePaymentLog(string condition);
        /// <summary>
        /// 删除交易日志
        /// </summary>
        /// <returns></returns>
        bool DeletePaymentLog();
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="postTableId">帖子所在分表Id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="chanagePostStatistic">是否更新帖子数量统计</param>
        /// <returns>删除数量</returns>
        int DeletePost(string postTableId, int pid, bool changePosts);
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="posterId"></param>
        void DeletePostByPosterid(int tableId, int posterId);
        /// <summary>
        /// 删除指定用户指定天数的附件
        /// </summary>
        /// <param name="uid"></param>
        void DeleteAttachmentByUid(int uid, int days);
        /// <summary>
        /// 获取指定用户指定天数的附件
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="days">指定天数</param>
        /// <returns></returns>
        IDataReader GetAttachmentListByUid(int uid, int days);
        /// <summary>
        /// 删除用户指定天数的帖子
        /// </summary>
        /// <param name="uid">发帖人ID</param>
        /// <param name="days">天数</param>
        void DeletePostByUidAndDays(int uid, int days);
        /// <summary>
        /// 删除短消息
        /// </summary>
        /// <param name="isNew">是否删除新短消息</param>
        /// <param name="postDateTime">发送日期</param>
        /// <param name="msgFromList">发送者列表</param>
        /// <param name="lowerUpper">是否区分大小写</param>
        /// <param name="subject">主题</param>
        /// <param name="message">内容</param>
        /// <param name="isUpdateUserNewPm">是否更新用户短消息数</param>
        /// <returns></returns>
        int DeletePrivateMessages(bool isnew, string postDateTime, string msgFromList, bool lowerUpper, string subject, string message, bool isUpdateUserNewPm);
        /// <summary>
        /// 删除短消息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pmitemidList">要删除的短信息列表</param>
        /// <returns>删除记录数</returns>
        int DeletePrivateMessages(int userId, string pmIdList);
        /// <summary>
        /// 删除评分日志
        /// </summary>
        /// <param name="pid">帖子Id</param>
        void DeleteRateLog(int pid);
        /// <summary>
        /// 删除评分日志
        /// </summary>
        /// <returns></returns>
        bool DeleteRateLog();
        /// <summary>
        /// 删除评分日志
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        bool DeleteRateLog(string condition);
        /// <summary>
        /// 删除在线表中的一行
        /// </summary>
        /// <param name="olId">在线id</param>
        /// <returns></returns>
        int DeleteRows(int olId);
        /// <summary>
        /// 删除在线表中的一行
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>删除行数</returns>
        int DeleteRowsByIP(string ip);
        /// <summary>
        /// 删除表情
        /// </summary>
        /// <param name="idList">表情Id</param>
        /// <returns></returns>
        int DeleteSmilies(string idList);
        /// <summary>
        /// 删除模板项
        /// </summary>
        /// <param name="templateIdList">格式为： 1,2,3</param>
        void DeleteTemplateItem(string templateIdList);
        /// <summary>
        /// 删除模板项
        /// </summary>
        /// <param name="templateId">模板id</param>
        void DeleteTemplateItem(int templateId);
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int DeleteTopic(int tid);
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="posterId"></param>
        void DeleteTopicByPosterid(int posterId);
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        bool DeleteTopicByTid(int tid, string postTableName);
        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topicList">要删除的主题ID列表</param>
        /// <param name="postTableId">所以分表的ID</param>
        /// <param name="changePosts">删除帖时是否要减版块帖数</param>
        /// <returns></returns>
        int DeleteTopicByTidList(string topicList, string postTableId, bool changePosts);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        void DeleteUserByUidlist(string uidList);
        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupId"></param>
        void DeleteUserGroupInfo(int groupId);
        /// <summary>
        /// 删除访问日志
        /// </summary>
        void DeleteVisitLogs();
        /// <summary>
        /// 删除访问日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        void DeleteVisitLogs(string condition);
        /// <summary>
        /// 删除词语过滤
        /// </summary>
        /// <param name="idList">过滤词条Id列表</param>
        /// <returns></returns>
        int DeleteWords(string idList);
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="delPosts"></param>
        /// <param name="delPms"></param>
        /// <returns></returns>
        bool DelUserAllInf(int uid, bool delPosts, bool delPms);
        /// <summary>
        /// 查找用户Email
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        IDataReader FindUserEmail(string email);
        /// <summary>
        /// 获取管理组列表
        /// </summary>
        /// <returns>管理组信息</returns>
        DataTable GetAdminGroupList();
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <returns>广告列表</returns>
        DataTable GetAdsTable();
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="aId">广告Id</param>
        /// <returns></returns>
        DataTable GetAdvertisement(int aid);
        /// <summary>
        /// 获取广告
        /// </summary>
        /// <returns></returns>
        DataTable GetAdvertisements(int type);
        DataTable GetAdvertisements();
        /// <summary>
        /// 获取所有板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetShortForumList();
        /// <summary>
        /// 获取板块统计信息
        /// </summary>
        /// <returns></returns>
        IDataReader GetAllForumStatistics();
        /// <summary>
        /// 获取所有帖子分表名
        /// </summary>
        /// <returns>分表记录集</returns>
        DataSet GetAllPostTableName();
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        DataTable GetAllTemplateList();
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="subfidList">子版块列表</param>
        /// <returns>主题总数</returns>
        int GetTopicCountOfForumWithSub(string subfidList);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="includeClosedTopic">是否包含关闭的主题</param>
        /// <param name="condition">查询条件</param>
        /// <returns>主题总数</returns>
        int GetTopicCount(int fid, int state, string condition);
        /// <summary>
        /// 获取通告
        /// </summary>
        /// <param name="id">公告id</param>
        /// <returns></returns>
        IDataReader GetAnnouncement(int id);
        /// <summary>
        /// 获取通告
        /// </summary>
        /// <returns></returns>
        DataTable GetAnnouncements();
        /// <summary>
        /// 获得指定数量公告
        /// </summary>
        /// <param name="num"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        DataTable GetAnnouncements(int num, int pageId);
        /// <summary>
        /// 获取简洁版首页版块列表
        /// </summary>
        /// <returns>板块列表的DataTable</returns>
        DataTable GetArchiverForumIndexList();
        /// <summary>
        /// 获取附件数
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        int GetAttachmentCountByPid(int pid);
        /// <summary>
        /// 获取附件数
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int GetAttachmentCountByTid(int tid);
        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        IDataReader GetAttachmentInfo(int aid);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="aidList">附件Id，以英文逗号分割</param>
        /// <returns>返回被删除的个数</returns>
        IDataReader GetAttachmentList(string aidList);
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="pidList"></param>
        /// <returns></returns>
        IDataReader GetAttachmentListByPid(string pidList);
        /// <summary>
        /// 获得指定帖子的附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>帖子信息</returns>
        DataTable GetAttachmentListByPid(int pid);
        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">版块tid列表</param>
        /// <returns>删除个数</returns>
        IDataReader GetAttachmentListByTid(string tidList);
        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题tid</param>
        /// <returns>删除个数</returns>
        IDataReader GetAttachmentListByTid(int tid);
        /// <summary>
        /// 将系统设置的附件类型以DataTable的方式存入缓存
        /// </summary>
        /// <returns>系统设置的附件类型</returns>
        DataTable GetAttachmentType();
        /// <summary>
        /// 获取屏蔽词列表
        /// </summary>
        /// <returns></returns>
        DataTable GetBanWordList();
        /// <summary>
        /// 按Id获取Discuz!NT代码
        /// </summary>
        /// <param name="id">Discuz!NT代码Id</param>
        /// <returns></returns>
        DataTable GetBBCode(int id);
        /// <summary>
        /// 获取Discuz!NT代码
        /// </summary>
        /// <returns></returns>
        DataTable GetBBCode();
        /// <summary>
        /// 获取积分日志
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <returns>积分日志</returns>
        DataTable GetCreditsLogList(int pageSize, int currentPage, int uid);
        /// <summary>
        /// 获得指定用户的积分交易历史记录总条数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>历史记录总条数</returns>
        int GetCreditsLogRecordCount(int uid);
        /// <summary>
        /// 获取自定义按钮列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetCustomEditButtonList();
        /// <summary>
        /// 获取帖子分表Id
        /// </summary>
        /// <returns></returns>
        DataRowCollection GetDatechTableIds();
        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        string GetDbName();
        /// <summary>
        /// 返加登录错误日志列表
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        DataTable GetErrLoginRecordByIP(string ip);
        /// <summary>
        /// 得到用户单个类型收藏的总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="typeid"></param>
        /// <returns>收藏总数</returns>
        int GetFavoritesCount(int uid, int typeId);
        /// <summary>
        /// 得到用户收藏信息列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pageSize">分页时每页的记录数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="typeId">收藏类型id</param>
        /// <returns>用户信息列表</returns>
        DataTable GetFavoritesList(int uid, int pageSize, int pageIndex, int typeId);
        /// <summary>
        /// 取得主题帖的第一个图片附件
        /// </summary>
        /// <param name="tid">主题id</param>
        IDataReader GetFirstImageAttachByTid(int tid);
        /// <summary>
        /// 获得指定主题的第一个帖子的id
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="postTableId"></param>
        /// <returns>帖子id</returns>
        int GetFirstPostId(int tid, string postTableId);
        /// <summary>
        /// 获取关注主题列表
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="views">最小浏览量</param>
        /// <param name="fid">板块ID</param>
        /// <param name="typeIdList">主题分类ID</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="orderFieldName">排序字段</param>
        /// <param name="visibleForum">板块范围(逗号分隔)</param>
        /// <param name="isDigest">是否精华</param>
        /// <param name="onlyImg">是否仅取带有图片附件的帖子</param>
        /// <returns></returns>
        DataTable GetFocusTopicList(int count, int views, int fid, string typeIdList, string startTime, string orderFieldName, string visibleForum, bool isDigest, bool onlyImg);
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        DataTable GetForumByParentid(int parentId);
        /// <summary>
        /// 获取麽一个字段
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        DataTable GetForumField(int fid, string fieldName);
        /// <summary>
        /// 获取首页板块列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetForumIndexList();
        /// <summary>
        /// 获取首页板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumIndexListTable();
        /// <summary>
        /// 获取板块最后一帖
        /// </summary>
        /// <param name="fid">板块id</param>
        /// <param name="posttablename">分表名称</param>
        /// <param name="topiccount">主题数</param>
        /// <param name="postcount">回帖数</param>
        /// <param name="lasttid">最后发表主题ID</param>
        /// <param name="lasttitle">最后发表标题</param>
        /// <param name="lastpost">最后发表时间</param>
        /// <param name="lastposterid">最后发表人ID</param>
        /// <param name="lastposter">最后发表人</param>
        /// <param name="todaypostcount">今日发帖数</param>
        /// <returns></returns>
        IDataReader GetForumLastPost(int fid, string postTableName, int topicCount, int postCount, int lastTid, string lastTitle, string lastPost, int lastPosterId, string lastPoster, int todayPostCount);
        /// <summary>
        /// 获取友情链接列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumLinkList();
        /// <summary>
        /// 获取友情链接
        /// </summary>
        /// <returns></returns>
        DataTable GetForumLinks();
        /// <summary>
        /// 获取板块在线用户列表
        /// </summary>
        /// <param name="forumId"></param>
        /// <returns></returns>
        IDataReader GetForumOnlineUserList(int forumId);
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <param name="startFid"></param>
        /// <param name="endFid"></param>
        /// <returns></returns>
        IDataReader GetForums(int startFid, int endFid);
        /// <summary>
        /// 获得forum的最大排序号
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetForumsMaxDisplayOrder(int parentId);
        /// <summary>
        /// 获得forum的最大排序号
        /// </summary>
        /// <returns></returns>
        int GetForumsMaxDisplayOrder();
        /// <summary>
        /// 获取板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetForumsTable();
        /// <summary>
        /// 获取第一个版块ID(非版块分类)
        /// </summary>
        /// <returns></returns>
        int GetFirstFourmID();
        /// <summary>
        /// 获取板块统计信息
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetForumStatistics(int fid);
        /// <summary>
        /// 获取用户组数
        /// </summary>
        /// <param name="creditsHigher"></param>
        /// <returns></returns>
        int GetGroupCountByCreditsLower(int creditsHigher);
        /// <summary>
        /// 获取板块最后帖子
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        IDataReader GetLastPostByFid(int fid, string postTableName);
        /// <summary>
        /// 获取最后帖子
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        DataTable GetLastPostByTid(int tid, string postTableName);
        /// <summary>
        /// 获取最后帖子列表
        /// </summary>
        /// <param name="postParmsInfo"></param>
        /// <param name="string"></param>
        /// <returns></returns>
        DataTable GetLastPostList(PostpramsInfo postParmsInfo, string postTableId);
        /// <summary>
        /// 得到论坛中最后注册的用户ID和用户名
        /// </summary>
        /// <param name="lastuserid">输出参数：最后注册的用户ID</param>
        /// <param name="lastusername">输出参数：最后注册的用户名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        bool GetLastUserInfo(out string lastUserId, out string lastUserName);
        /// <summary>
        /// 获取顶级板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetMainForum();
        /// <summary>
        /// 获取主帖子信息
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        DataTable GetMainPostByTid(string postTableName, int tid);
        /// <summary>
        /// 获取板块最大和最小主题Id
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetMaxAndMinTid(int fid);
        /// <summary>
        /// 获取最大积分下限
        /// </summary>
        /// <returns></returns>
        DataTable GetMaxCreditLower();
        /// <summary>
        /// 获取最大板块Id
        /// </summary>
        /// <returns></returns>
        int GetMaxForumId();
        /// <summary>
        /// 获取最大帖子分表Id
        /// </summary>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        int GetMaxPostTableTid(string postTableName);
        /// <summary>
        /// 获取最大表情Id
        /// </summary>
        /// <returns></returns>
        int GetMaxSmiliesId();
        /// <summary>
        /// 获取最大帖子分表Id
        /// </summary>
        /// <returns></returns>
        int GetMaxTableListId();
        /// <summary>
        /// 获取最大主题Id
        /// </summary>
        /// <returns></returns>
        DataTable GetMaxTid();
        /// <summary>
        /// 获取最大用户组Id
        /// </summary>
        /// <returns></returns>
        int GetMaxUserGroupId();
        /// <summary>
        /// 获取勋章
        /// </summary>
        /// <returns></returns>
        DataTable GetMedal();
        /// <summary>
        /// 获取勋章Sql语句
        /// </summary>
        /// <returns></returns>
        string GetMedalSql();
        /// <summary>
        /// 得到当前指定条件和页数的勋章日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        DataTable GetMedalLogList(int pageSize, int currentPage, string condition);
        /// <summary>
        /// 得到当前指定页数的勋章日志记录(表)
        /// </summary>
        /// <param name="pageSize">当前分页的尺寸大小</param>
        /// <param name="currentPage">当前页码</param>
        /// <returns></returns>
        DataTable GetMedalLogList(int pageSize, int currentPage);
        /// <summary>
        /// 获取勋章日志数
        /// </summary>
        /// <returns></returns>
        int GetMedalLogListCount();
        /// <summary>
        /// 获取勋章日志数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetMedalLogListCount(string condition);
        /// <summary>
        /// 获取勋章列表
        /// </summary>
        /// <returns></returns>
        DataTable GetMedalsList();
        /// <summary>
        /// 获取最小的积分上限
        /// </summary>
        /// <returns></returns>
        DataTable GetMinCreditHigher();
        /// <summary>
        /// 获取最小的帖子分表Id
        /// </summary>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        int GetMinPostTableTid(string postTableName);
        /// <summary>
        /// 获取版主信息
        /// </summary>
        /// <param name="moderator"></param>
        /// <returns></returns>
        DataTable GetModeratorInfo(string moderator);
        /// <summary>
        /// 获取版主列表
        /// </summary>
        /// <returns>所有版主信息</returns>
        DataTable GetModeratorList();
        /// <summary>
        /// 按条件得到勋章日志列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable GetModeratorLogList(int pageSize, int currentPage, string condition);
        /// <summary>
        /// 得到指定查询条件下的前台管理日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        int GetModeratorLogListCount(string condition);
        /// <summary>
        /// 获取管理日志数
        /// </summary>
        /// <returns></returns>
        int GetModeratorLogListCount();
        /// <summary>
        /// 获取版主列表中包含用户名的版块列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        //DataTable GetModerators(string oldUserName);
        /// <summary>
        /// 获取版块版主
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <returns></returns>
        DataTable GetModerators(int fid);
        /// <summary>
        /// 获取新短消息数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        int GetNewPMCount(int userId);
        /// <summary>
        /// 获取新主题
        /// </summary>
        /// <param name="forumIdList">版块id列表</param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        IDataReader GetNewTopics(string forumIdList, string postTableId);
        /// <summary>
        /// 获得在线用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        int GetOnlineAllUserCount();
        /// <summary>
        /// 获取在线图例
        /// </summary>
        /// <returns></returns>
        IDataReader GetOnlineGroupIconList();
        /// <summary>
        /// 获取在线图例
        /// </summary>
        /// <returns></returns>
        DataTable GetOnlineGroupIconTable();
        /// <summary>
        /// 获取在线列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOnlineList();
        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        DataTable GetOnlineUser(int userId, string passWord);
        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ip"></param>
        /// <returns>用户的详细信息</returns>
        DataTable GetOnlineUserByIP(int userId, string ip);
        /// <summary>
        /// 获取在线用户数
        /// </summary>
        /// <returns></returns>
        int GetOnlineUserCount();
        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetOnlineUserList();
        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOnlineUserListTable();
        /// <summary>
        /// 获取板块父Id
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetParentIdByFid(int fid);
        /// <summary>
        /// 获取指定用户的交易日志
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        DataTable GetPayLogInList(int pageSize, int currentPage, int uid);
        /// <summary>
        /// 返回指定用户的支出日志记录数
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        DataTable GetPayLogOutList(int pageSize, int currentPage, int uid);
        /// <summary>
        /// 获取指定主题的购买记录
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        DataTable GetPaymentLogByTid(int pageSize, int currentPage, int tid);
        /// <summary>
        /// 获取主题购买记录数
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        int GetPaymentLogByTidCount(int tid);
        /// <summary>
        /// 获取指定用户的收入日志记录数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        int GetPaymentLogInRecordCount(int uid);
        /// <summary>
        /// 分页获取日志
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="currentPage">当前页码</param>
        /// <returns></returns>
        DataTable GetPaymentLogList(int pageSize, int currentPage);
        /// <summary>
        /// 分页获取日志
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable GetPaymentLogList(int pageSize, int currentPage, string condition);
        /// <summary>
        /// 获取交易记录数
        /// </summary>
        /// <returns></returns>
        int GetPaymentLogListCount();
        /// <summary>
        /// 得到指定查询条件下的积分交易日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        int GetPaymentLogListCount(string condition);
        /// <summary>
        /// 返回指定用户支出日志总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        int GetPaymentLogOutRecordCount(int uid);
        /// <summary>
        /// 获取投票截止时间
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        string GetPollEnddatetime(int tid);
        /// <summary>
        /// 获取投票列表
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetPollList(int tid);
        /// <summary>
        /// 通过主题ID获取相应的投票信息
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>投票选项集合</returns>
        IDataReader GetPollOptionList(int tid);
        /// <summary>
        /// 得到投票帖的投票类型
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>投票类型</returns>
        int GetPollType(int tid);
        /// <summary>
        /// 获取指定tid投票人列表
        /// </summary>
        /// <param name="tid">投票tid</param>
        /// <returns></returns>
        string GetPollUserNameList(int tid);
        /// <summary>
        /// 获取帖子信息
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="pid">帖子Id</param>
        /// <returns></returns>
        DataTable GetPost(string postTableName, int pid);
        /// <summary>
        /// 获取版块帖数
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <param name="postTableName">分表名称</param>
        /// <returns></returns>
        int GetPostCount(int fid, string postTableName);
        /// <summary>
        /// 获取指定tid的帖子总数
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        int GetPostCountByTid(int tid, string postTableName);
        /// <summary>
        /// 获得指定用户回复指定主题次数
        /// </summary>
        /// <param name="postTableId">帖子分表Id</param>
        /// <param name="topicId">主题Id</param>
        /// <param name="posterId">用户Id</param>
        /// <returns>回复次数</returns>
        int GetPostCount(string postTableId, int tid, int posterId);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        int GetPostCount(string postTableName);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        int GetPostCountByUid(int uid, string postTableName);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="postsid"></param>
        /// <returns></returns>
        DataTable GetPostCountFromIndex(string postsId);
        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="postsId"></param>
        /// <returns></returns>
        DataTable GetPostCountTable(string postsId);
        /// <summary>
        /// 获取帖子信息
        /// </summary>
        /// <param name="postTableId"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        IDataReader GetPostInfo(string postTableId, int pid);
        /// <summary>
        /// 获取帖子登记
        /// </summary>
        /// <param name="currentPostTableId">分表ID</param>
        /// <param name="postid">帖子ID</param>
        /// <returns></returns>
        DataTable GetPostLayer(int currentPostTableId, int postId);
        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <param name="postTableId"></param>
        /// <returns>指定条件的帖子DataSet</returns>
        IDataReader GetPostList(PostpramsInfo postParmsInfo, string postTableId);
        /// <summary>
        /// 获得指定主题的帖子列表
        /// </summary>
        /// <param name="topicList">主题ID列表</param>
        /// <param name="postTableId">分表Id列表</param>
        /// <returns></returns>
        DataTable GetPostList(string topicList, string[] postTableId);
        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="postParmsInfo">参数列表</param>
        /// <param name="postTableName"></param>
        /// <returns>指定条件的帖子DataSet</returns>
        IDataReader GetPostListByCondition(PostpramsInfo postParmsInfo, string postTableName);
        /// <summary>
        /// 获取帖子标题列表
        /// </summary>
        /// <param name="Tid"></param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        DataTable GetPostListTitle(int tid, string postTableName);
        /// <summary>
        /// 获取帖子评分列表
        /// </summary>
        /// <param name="pid">帖子列表</param>
        /// <param name="displayRateCount"></param>
        /// <returns>帖子评分列表</returns>
        IDataReader GetPostRateLogs(int pid, int displayRateCount);
        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="postTableId">分表id</param>
        /// <returns></returns>
        DataSet GetPosts(int pid, int pageSize, int pageIndex, string postTableId);
        /// <summary>
        /// 获取分表帖数
        /// </summary>
        /// <param name="postTableid">分表Id</param>
        /// <returns></returns>
        int GetPostsCount(string postTableId);
        /// <summary>
        /// 获取分表列表
        /// </summary>
        /// <returns></returns>
        DataTable GetPostTableList();
        /// <summary>
        /// 获取指定tid的帖子DataTable
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <param name="postTableId"></param>
        /// <returns>指定tid的帖子DataTable</returns>
        DataTable GetPostTree(int tid, string postTableId);
        /// <summary>
        /// 得到当用户的短消息数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="state">短消息状态(0:已读短消息、1:未读短消息、2:最近消息（7天内）、-1:全部短消息)</param>
        /// <returns>短消息数量</returns>
        int GetPrivateMessageCount(int userId, int folder, int state);
        /// <summary>
        /// 获得指定ID的短消息的内容
        /// </summary>
        /// <param name="pmid">短消息pmid</param>
        /// <returns>短消息内容</returns>
        IDataReader GetPrivateMessageInfo(int pmId);
        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="pageSize">每页显示短信息数</param>
        /// <param name="pageIndex">当前要显示的页数</param>
        /// <param name="intType">筛选条件1为未读</param>
        /// <returns>短信息列表</returns>
        IDataReader GetPrivateMessageList(int userId, int folder, int pageSize, int pageIndex, int intType);
        /// <summary>
        /// 获取评分日志数
        /// </summary>
        /// <returns></returns>
        int GetRateLogCount();
        /// <summary>
        /// 按条件获取评分日志的条数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int GetRateLogCount(string condition);
        /// <summary>
        /// 获取评分范围
        /// </summary>
        /// <param name="scoreId"></param>
        /// <returns></returns>
        DataTable GetRateRange(int scoreId);
        /// <summary>
        /// 获指定的搜索缓存的DataTable
        /// </summary>
        /// <param name="searchid">搜索缓存的searchid</param>
        /// <returns></returns>
        DataTable GetSearchCache(int searchId);
        /// <summary>
        /// 获取精华主题列表
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="strTids">全部Tid列表</param>
        /// <returns></returns>
        DataTable GetSearchDigestTopicsList(int pageSize, string strTids);
        /// <summary>
        /// 获取精华主题帖列表
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="strTids">全部Tid列表</param>
        /// <param name="posTableName">当前分表名称</param>
        /// <returns></returns>
        DataTable GetSearchPostsTopicsList(int pageSize, string strTids, string posTableName);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="strTids">全部Tid列表</param>
        /// <returns></returns>
        DataTable GetSearchTopicsList(int pageSize, string strTids);
        /// <summary>
        /// 获取简要板块信息
        /// </summary>
        /// <returns></returns>
        DataTable GetShortForums();
        /// <summary>
        /// 返回指定用户的简短信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        IDataReader GetShortUserInfoToReader(int uid);
        /// <summary>
        /// 获得全部指定时间段内的前n条公告列表
        /// </summary>
        /// <param name="maxCount">最大记录数,小于0返回全部</param>
        /// <returns>公告列表</returns>
        DataTable GetAnnouncementList(int maxCount);
        /// <summary>
        /// 获取单个帖子
        /// </summary>
        /// <param name="attachments"></param>
        /// <param name="postParmsInfo"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        IDataReader GetSinglePost(out IDataReader attachments, PostpramsInfo postParmsInfo, string postTableId);
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        DataTable GetSmilies();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        IDataReader GetSmiliesList();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        DataTable GetSmiliesListDataTable();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        DataTable GetSmiliesListWithoutType();
        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        DataTable GetSmilieTypes();
        /// <summary>
        /// 获取特殊组信息
        /// </summary>
        /// <returns></returns>
        DataTable GetSpecialUserGroup();
        /// <summary>
        /// 得到指定帖子分表的全文索引建立(填充)语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        string GetSpecialTableFullIndexSQL(string tableName, string dbName);
        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns></returns>
        DataTable GetStatisticsRow();
        /// <summary>
        /// 获取子版块列表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetSubForumReader(int fid);
        /// <summary>
        /// 获取子版块列表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetSubForumTable(int fid);
        /// <summary>
        /// 获取系统组信息SQL语句
        /// </summary>
        /// <returns></returns>
        string GetSystemGroupInfoSql();
        /// <summary>
        /// 获取管理日志被操作的Tid字符串
        /// </summary>
        /// <param name="postDateTime"></param>
        /// <returns></returns>
        DataTable GetModeratorLogByPostDate(DateTime postDateTime);
        /// <summary>
        /// 根据时间获取管理日志标题
        /// </summary>
        /// <param name="moderatorName"></param>
        /// <returns></returns>
        DataTable GetModeratorLogByName(string moderatorName);
        /// <summary>
        /// 根据时间获取管理日志标题
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        DataTable GetModeratorLogByPostDate(DateTime startDateTime, DateTime endDateTime);
        /// <summary>
        /// 获取今日帖数
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        int GetTodayPostCount(int fid, string postTableName);
        /// <summary>
        /// 获取今日帖数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        int GetTodayPostCountByUid(int uid, string postTableName);
        /// <summary>
        /// 获取顶级板块Id列表
        /// </summary>
        /// <param name="lastFid"></param>
        /// <param name="statCount"></param>
        /// <returns></returns>
        IDataReader GetTopForumFids(int lastFid, int statCount);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        int GetTopicCount(int fid);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetTopicCount(string condition);
        /// <summary>
        /// 获取主题所属板块Id
        /// </summary>
        /// <param name="tidList"></param>
        /// <returns></returns>
        DataTable GetTopicFidByTid(string tidList);
        /// <summary>
        /// 获取主题信息
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="fid"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IDataReader GetTopicInfo(int tid, int fid, byte mode);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="displayOrder"></param>
        /// <returns></returns>
        DataTable GetTopicList(string topicList, int displayOrder);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="forumId"></param>
        /// <param name="pageId"></param>
        /// <param name="tpp"></param>
        /// <returns></returns>
        DataTable GetTopicList(int forumId, int pageId, int tpp);
        /// <summary>
        /// 获取主题管理日志列表
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        IDataReader GetTopicListModeratorLog(int tid);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="startTid"></param>
        /// <param name="endTid"></param>
        /// <returns></returns>
        IDataReader GetTopics(int startTid, int endTid);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="startNum"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IDataReader GetTopics(int fid, int pageSize, int pageIndex, int startNum, string condition);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="startNumber"></param>
        /// <param name="condition"></param>
        /// <param name="orderFields"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        IDataReader GetTopicsByDate(int fid, int pageSize, int pageIndex, int startNumber, string condition, string orderFields, int sortType);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IDataReader GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="startNum"></param>
        /// <param name="condition"></param>
        /// <param name="ascDesc"></param>
        /// <returns></returns>
        IDataReader GetTopicsByType(int pageSize, int pageIndex, int startNum, string condition, int ascDesc);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="startNum"></param>
        /// <param name="condition"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascDesc"></param>
        /// <returns></returns>
        IDataReader GetTopicsByTypeDate(int pageSize, int pageIndex, int startNum, string condition, string orderBy, int ascDesc);
        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IDataReader GetTopicsByUserId(int userId, int pageIndex, int pageSize);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <returns></returns>
        int GetTopicCount();
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetTopicReplyCountByUserId(int userId);
        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetTopicCountByUserId(int userId);
        /// <summary>
        /// 获取主题状态
        /// </summary>
        /// <param name="topicList"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        int GetTopicStatus(string topicList, string field);
        /// <summary>
        /// 获取主题Id列表
        /// </summary>
        /// <param name="tidList"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataTable GetTopicTidByFid(string tidList, int fid);
        /// <summary>
        /// 获取主题Tid列表
        /// </summary>
        /// <param name="statCount"></param>
        /// <param name="lastTid"></param>
        /// <returns></returns>
        IDataReader GetTopicTids(int statCount, int lastTid);
        /// <summary>
        /// 获取主题分类列表
        /// </summary>
        /// <returns></returns>
        DataTable GetTopicTypeList();
        /// <summary>
        /// 获取置顶主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        DataSet GetTopTopicList();
        /// <summary>
        /// 获取置顶主题列表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="tids"></param>
        /// <returns></returns>
        IDataReader GetTopTopics(int fid, int pageSize, int pageIndex, string tids);
        /// <summary>
        /// 获取指定数量的用户
        /// </summary>
        /// <param name="statCount"></param>
        /// <param name="lastUid"></param>
        /// <returns></returns>
        IDataReader GetTopUsers(int statCount, int lastUid);
        /// <summary>
        /// 获取Uid和AdminId
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DataTable GetUidAdminIdByUsername(string userName);
        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        int GetUidByUserName(string userName);
        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <param name="currentfid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetUidInModeratorsByUid(int currentFid, int uid);
        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <param name="fidList"></param>
        /// <returns></returns>
        DataTable GetUidModeratorByFid(string fidList);
        /// <summary>
        /// 获取未审核的主题SQL语句
        /// </summary>
        /// <returns></returns>
        //DataTable GetUnauditNewTopic();
        /// <summary>
        /// 获取未审核的帖子SQL语句
        /// </summary>
        /// <param name="currentPostTableId"></param>
        /// <returns></returns>
        //DataTable GetUnauditPost(int currentPostTableId);
        /// <summary>
        /// 获取用户上传的文件大小
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetUploadFileSizeByUserId(int uid);
        /// <summary>
        /// 获取用户数
        /// </summary>
        /// <returns></returns>
        int GetUserCount();
        /// <summary>
        /// 获取有管理权限的用户数
        /// </summary>
        /// <returns></returns>
        int GetUserCountByAdmin();
        /// <summary>
        /// 获取指定组的用户Email地址
        /// </summary>
        /// <param name="groupIdList"></param>
        /// <returns></returns>
        DataTable GetUserEmailByGroupid(string groupIdList);
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="uidList"></param>
        /// <returns></returns>
        DataTable GetUsersByUidlLst(string uidList);
        /// <summary>
        /// 获取用户扩展积分
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="extid"></param>
        /// <returns></returns>
        float GetUserExtCredits(int uid, int exTid);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        DataTable GetUserGroup(int groupId);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroup();
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="creditsHigher"></param>
        /// <returns></returns>
        DataTable GetUserGroupByCreditshigher(int creditsHigher);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="creditsHigher"></param>
        /// <param name="creditsLower"></param>
        /// <returns></returns>
        DataTable GetUserGroupByCreditsHigherAndLower(int creditsHigher, int creditsLower);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        DataTable GetUserGroupCreditsLowerAndHigher(int groupId);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        DataTable GetUserGroupExceptGroupid(int groupId);
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        DataTable GetUserGroupInfoByGroupid(int groupId);
        /// <summary>
        /// 获取用户组的RadminId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        string GetUserGroupRAdminId(int groupId);
        /// <summary>
        /// 获取用户组评分权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        DataTable GetUserGroupRateRange(int groupId);
        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroups();
        /// <summary>
        /// 获取用户组列表字符串
        /// </summary>
        /// <returns></returns>
        string GetUserGroupsStr();
        /// <summary>
        /// 获取用户组名列表
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupsTitle();
        /// <summary>
        /// 获取用户组名称
        /// </summary>
        /// <returns></returns>
        string GetUserGroupTitle();
        /// <summary>
        /// 获取除游客组之外的用户组名称
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupWithOutGuestTitle();
        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IDataReader GetShortUserInfoByName(string userName);
        /// <summary>
        /// 根据验证字串获取用户Id
        /// </summary>
        /// <param name="authStr"></param>
        /// <returns></returns>
        DataTable GetUserIdByAuthStr(string authStr);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        DataTable GetUserInfo(string userName, string passWord);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable GetUserInfo(int userId);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        IDataReader GetUserInfoByIP(string ip);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserInfoToReader(int uid);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IDataReader GetUserInfoToReader(string userName);
        /// <summary>
        /// 获取用户注册日期
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserJoinDate(int uid);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="column"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        DataTable GetUserList(int pageSize, int pageIndex, string column, string orderType);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        DataTable GetUserList(int pageSize, int currentPage);
        /// <summary>
        /// 获取用户列表和帖子列表
        /// </summary>
        /// <param name="postList"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        IDataReader GetUserListWithPostList(string postList, string postTableId);
        /// <summary>
        /// 获取用户列表和主题列表
        /// </summary>
        /// <param name="topicList"></param>
        /// <param name="lossLessDel"></param>
        /// <returns></returns>
        IDataReader GetUserListWithTopicList(string topicList, int lossLessDel);
        /// <summary>
        /// 获取用户列表和主题列表
        /// </summary>
        /// <param name="topicList"></param>
        /// <returns></returns>
        IDataReader GetUserListWithTopicList(string topicList);
        /// <summary>
        /// 获取设置主题精华操作的用户列表
        /// </summary>
        /// <param name="digestTopicList"></param>
        /// <param name="digestType"></param>
        /// <returns></returns>
        IDataReader GetUserListWithDigestTopicList(string digestTopicList, int digestType);
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserName(int uid);
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetUserNameByUid(int uid);
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="groupIdList"></param>
        /// <returns></returns>
        DataTable GetUserListByGroupid(string groupIdList);
        /// <summary>
        /// 获取指定用户组的用户信息
        /// </summary>
        /// <param name="groupIdList">用户组</param>
        /// <param name="topNumber">获取前N条记录</param>
        /// <param name="start_uid">大于该uid的用户记录</param>
        /// <returns></returns>
        DataTable GetUserListByGroupid(string groupIdList, int topNumber, int start_uid);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="startUid"></param>
        /// <param name="endUid"></param>
        /// <returns></returns>
        IDataReader GetUsers(int startUid, int endUid);
        /// <summary>
        /// 获取今日用户评分
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IDataReader GetUserTodayRate(int uid);
        /// <summary>
        /// 获取可用模板Id列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetValidTemplateIDList();
        /// <summary>
        /// 获取可用模板列表
        /// </summary>
        /// <returns></returns>
        DataTable GetValidTemplateList();
        /// <summary>
        /// 获取可见板块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetVisibleForumList();
        /// <summary>
        /// 获取访问日志数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetVisitLogCount(string condition);
        /// <summary>
        /// 获取访问日志数
        /// </summary>
        /// <returns></returns>
        int GetVisitLogCount();
        /// <summary>
        /// 获取访问日志列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetVisitLogList(int pageSize, int currentPage, string condition);
        /// <summary>
        /// 获取访问日志列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        DataTable GetVisitLogList(int pageSize, int currentPage);
        /// <summary>
        /// 判断帖子列表是否都在当前板块
        /// </summary>
        /// <param name="topicIdList"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        int GetTopicCountInForumAndTopicIdList(string topicIdList, int fid);
        /// <summary>
        /// 插入板块信息
        /// </summary>
        /// <param name="forumInfo"></param>
        /// <returns></returns>
        int InsertForumsInf(ForumInfo forumInfo);
        /// <summary>
        /// 插入管理信息
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="moderators"></param>
        /// <param name="displayOrder"></param>
        /// <param name="inherited"></param>
        void InsertForumsModerators(string fid, string moderators, int displayOrder, int inherited);
        /// <summary>
        /// 插入管理日志
        /// </summary>
        /// <param name="moderatorUid"></param>
        /// <param name="moderatorName"></param>
        /// <param name="groupId"></param>
        /// <param name="groupTitle"></param>
        /// <param name="ip"></param>
        /// <param name="postDateTime"></param>
        /// <param name="fid"></param>
        /// <param name="fName"></param>
        /// <param name="tid"></param>
        /// <param name="title"></param>
        /// <param name="actions"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        bool InsertModeratorLog(string moderatorUid, string moderatorName, string groupId, string groupTitle, string ip, string postDateTime, string fid, string fName, string tid, string title, string actions, string reason);
        /// <summary>
        /// 创建评分记录
        /// </summary>
        /// <param name="pid">被评分帖子pid</param>
        /// <param name="userId">评分者uid</param>
        /// <param name="userName">评分者用户名</param>
        /// <param name="extId">分的积分类型</param>
        /// <param name="score">积分数值</param>
        /// <param name="reason">评分理由</param>
        /// <returns>更新数据行数</returns>
        int InsertRateLog(int pid, int userId, string userName, int extId, float score, string reason);
        /// <summary>
        /// 判断用户是否已购买主题
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        bool IsBuyer(int tid, int uid);
        /// <summary>
        /// 是否存在勋章授予记录
        /// </summary>
        /// <param name="medalId">勋章Id</param>
        /// <param name="uid">用户Id</param>
        /// <returns></returns>
        bool IsExistMedalAwardRecord(int medalId, int userId);
        /// <summary>
        /// 是否存在子版块
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        bool IsExistSubForum(int fid);
        /// <summary>
        /// 判断指定用户是否是指定主题的回复者
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        bool IsReplier(int tid, int uid, string postTableId);
        /// <summary>
        /// 是否是系统组
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <returns></returns>
        bool IsSystemOrTemplateUserGroup(int groupId);
        /// <summary>
        /// 移动版块位置
        /// </summary>
        /// <param name="currentFid">当前板块ID</param>
        /// <param name="targetFid"></param>
        /// <param name="isAsChildNode"></param>
        /// <param name="extName"></param>
        void MovingForumsPos(string currentFid, string targetFid, bool isAsChildNode, string extName);
        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableName">当前帖子分表Id</param>
        /// <param name="tidList">主题Id</param>
        void PassAuditNewTopic(string postTableName, string tidList);
        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableName">回复表ID</param>
        /// <param name="tidList">忽略的主题列表</param>
        /// <param name="validate">需要验证的主题列表</param>
        /// <param name="delete">删除的主题列表</param>
        /// <param name="fidList">版块列表</param>
        void PassAuditNewTopic(string postTableName, string tidList, string validate, string delete, string fidList);
        /// <summary>
        /// 获取版主有权限管理的主题数
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="tidList">主题ID列表</param>
        /// <returns></returns>
        int GetModTopicCountByTidList(string fidList, string tidList);
        /// <summary>
        /// 通过未审核的帖子
        /// </summary>
        /// <param name="currentPostTableId">当前表ID</param>
        /// <param name="pidList">帖子ID列表</param>
        void PassPost(int currentPostTableId, string pidList);
        /// <summary>
        /// 通过待验证的帖子
        /// </summary>
        /// <param name="tableId">帖子分表Id</param>
        /// <param name="validate">需要验证的帖子Id列表</param>
        /// <param name="delete">需要删除的帖子Id列表</param>
        /// <param name="ignore">需要忽略的帖子ID列表</param>
        /// <param name="fidList">版块Id列表</param>
        void AuditPost(int tableId, string validate, string delete, string ignore, string fidList);
        /// <summary>
        /// 获取版主有权限管理的帖子数
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="postTableId">分表ID</param>
        /// <param name="pidList">帖子ID列表</param>
        /// <returns></returns>
        int GetModPostCountByPidList(string fidList, string postTableId, string pidList);
        /// <summary>
        /// 按条件获取评分日志列表
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="postTableName">分表名称</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable RateLogList(int pageSize, int currentPage, string postTableName, string condition);
        /// <summary>
        /// 得到当前指定页数的评分日志记录(表)
        /// </summary>
        /// <param name="pageSize">当前分页的尺寸大小</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="postTableName"></param>
        /// <returns></returns>
        DataTable RateLogList(int pageSize, int currentPage, string postTableName);
        /// <summary>
        /// 修复主题
        /// </summary>
        /// <param name="topicList"></param>
        /// <param name="postTable"></param>
        /// <returns></returns>
        int RepairTopics(string topicId, string postTable);
        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        void ReSetClearMove();
        /// <summary>
        /// 重置登录错误次数
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        int ResetErrLoginCount(string ip);
        /// <summary>
        /// 重设统计信息
        /// </summary>
        /// <param name="userCount">用户数</param>
        /// <param name="topicsCount">主题数</param>
        /// <param name="postCount">帖子数</param>
        /// <param name="lastUserId">最后注册用户ID</param>
        /// <param name="lastUserName">最后注册用户名称</param>
        void ReSetStatistic(int userCount, int topicsCount, int postCount, string lastUserId, string lastUserName);
        /// <summary>
        /// 重置主题类型
        /// </summary>
        /// <param name="topicTypeId"></param>
        /// <param name="topicList"></param>
        /// <returns></returns>
        int ResetTopicTypes(int topicTypeId, string topicList);
        /// <summary>
        /// 重置所有用户的精华帖数
        /// </summary>
        void ResetUserDigestPosts();
        /// <summary>
        /// 恢复备份数据库          
        /// </summary>
        /// <param name="backupPath">备份文件路径</param>
        /// <param name="serverName">服务器名称</param>
        /// <param name="userName">数据库用户名</param>
        /// <param name="password">数据库密码</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="fileName">备份文件名</param>
        /// <returns></returns>
        string RestoreDatabase(string backUpPath, string serverName, string userName, string passWord, string strDbName, string strFileName);
        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        string RunSql(string sql);
        /// <summary>
        /// 保存板块信息
        /// </summary>
        /// <param name="forumInfo">版块信息</param>
        void SaveForumsInfo(ForumInfo forumInfo);
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="spaceenabled">空间是否开启</param>
        /// <param name="albumenable">相册是否开启</param>
        /// <param name="posttableid">帖子表id</param>
        /// <param name="userid">用户id</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="posterid">发帖者id</param>
        /// <param name="searchType">搜索类型</param>
        /// <param name="searchforumid">搜索版块id</param>
        /// <param name="searchtime">搜索时间</param>
        /// <param name="searchtimetype">搜索时间类型</param>
        /// <param name="resultorder">结果排序方式</param>
        /// <param name="resultordertype">结果类型类型</param>
        /// <returns>如果成功则返回searchid, 否则返回-1</returns>
        int Search(bool spaceEnabled, bool albumEnabled, int postTableId, int userId, int userGroupId, string keyWord, int posterId, SearchType searchType, string searchForumId, int searchTime, int searchTimeType, int resultOrder, int resultOrderType);
        /// <summary>
        /// 生成搜索附件的条件
        /// </summary>
        /// <param name="forumid">板块ID</param>
        /// <param name="posttablename">分表名称</param>
        /// <param name="filesizemin">最小</param>
        /// <param name="filesizemax">最大</param>
        /// <param name="downloadsmin"></param>
        /// <param name="downloadsmax"></param>
        /// <param name="postdatetime">提交时间</param>
        /// <param name="filename"></param>
        /// <param name="description"></param>
        /// <param name="poster"></param>
        /// <returns></returns>
        string SearchAttachment(int forumId, string postTableName, string fileSizeMin, string fileSizeMax, string downLoadsMin, string downLoadsMax, string postDateTime, string fileName, string description, string poster);
        /// <summary>
        /// 获取搜索勋章列表条件
        /// </summary>
        /// <param name="postDateTimeStart">授予开始日期</param>
        /// <param name="postDateTimeEnd">授予结束日期</param>
        /// <param name="userName">授予人</param>
        /// <param name="reason">理由</param>
        /// <returns></returns>
        string SearchMedalLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string reason);
        /// <summary>
        /// 获取管理日志搜索条件
        /// </summary>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <param name="userName">用户名</param>
        /// <param name="others">其它</param>
        /// <returns></returns>
        string SearchModeratorManageLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others);
        /// <summary>
        /// 搜索管理日志
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        string SearchModeratorManageLog(string keyWord);
        /// <summary>
        /// 获取交易日志搜索条件
        /// </summary>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        string SearchPaymentLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName);
        /// <summary>
        /// 搜索帖子
        /// </summary>
        /// <param name="forumId"></param>
        /// <param name="posttableiId"></param>
        /// <param name="postDateTimeStart"></param>
        /// <param name="postDateTimeEnd"></param>
        /// <param name="poster"></param>
        /// <param name="lowerUpper"></param>
        /// <param name="ip"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        string SearchPost(int forumId, string posttableiId, DateTime postDateTimeStart, DateTime postDateTimeEnd, string poster, bool lowerUpper, string ip, string message);
        /// <summary>
        /// 获取评分日志搜索条件
        /// </summary>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <param name="userName">用户名</param>
        /// <param name="others">其它</param>
        /// <returns></returns>
        string SearchRateLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others);
        /// <summary>
        /// 获取帖子审核的条件
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <param name="poster">帖子作者</param>
        /// <param name="title">标题</param>
        /// <param name="moderatorName">版主名称</param>
        /// <param name="postDateTimeStart">主题发布起始日期</param>
        /// <param name="postDateTimeEnd">主题发布结束日期</param>
        /// <param name="delDateTimeStart">删除起始日期</param>
        /// <param name="delDateTimeEnd">删除结束日期</param>
        /// <returns></returns>
        string SearchTopicAudit(int fid, string poster, string title, string moderatorName, DateTime postDateTimeStart, DateTime postDateTimeEnd, DateTime delDateTimeStart, DateTime delDateTimeEnd);
        /// <summary>
        /// 获取搜索主题条件
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="displayOrder">显示序号</param>
        /// <param name="digest">精华</param>
        /// <param name="attachment">附件</param>
        /// <param name="poster">作者</param>
        /// <param name="lowerUpper">是否区分大小写</param>
        /// <param name="viewsMin">最小查看数</param>
        /// <param name="viewsMax">最大查看数</param>
        /// <param name="repliesMax">最大回复数</param>
        /// <param name="repliesMin">最小回复数</param>
        /// <param name="rate">售价</param>
        /// <param name="lastPost">最后回复</param>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <returns></returns>
        string SearchTopics(int forumId, string keyWord, string display0rder, string digest, string attachment, string poster, bool lowerUpper, string viewsMin, string viewsMax, string repliesMax, string repliesMin, string rate, string lastPost, DateTime postDateTimeStart, DateTime postDateTimeEnd);
        /// <summary>
        /// 获取管理日志条件
        /// </summary>
        /// <param name="postDateTimeStart">访问起始日期</param>
        /// <param name="postDateTimeEnd">访问结束日期</param>
        /// <param name="userName">用户名</param>
        /// <param name="others">其它</param>
        /// <returns></returns>
        string SearchVisitLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others);
        /// <summary>
        /// 设置管理组信息
        /// </summary>
        /// <param name="adminGroupsInfo">管理组信息</param>
        /// <returns></returns>
        int SetAdminGroupInfo(AdminGroupInfo adminGroupsInfo);
        /// <summary>
        /// 设置勋章为可用
        /// </summary>
        /// <param name="available"></param>
        /// <param name="medalIdList"></param>
        void SetAvailableForMedal(int available, string medalIdList);
        /// <summary>
        /// 批量更新BBCode的可用性
        /// </summary>
        /// <param name="idList">可用性状态</param>
        /// <param name="status">BBCodeId列表</param>
        void SetBBCodeAvailableStatus(string idList, int status);
        /// <summary>
        /// 设置主题属性
        /// </summary>
        /// <param name="topicList">主题ID列表</param>
        /// <param name="value">主题属性</param>
        /// <returns></returns>
        bool SetDisplayorder(string topicList, int value);
        /// <summary>
        /// 设置版主
        /// </summary>
        /// <param name="moderator">用户名</param>
        void SetModerator(string moderator);
        /// <summary>
        /// 设置主帖
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="tid"></param>
        /// <param name="postId"></param>
        /// <param name="postTableId"></param>
        void SetPrimaryPost(string subject, int tid, string[] postId, string postTableId);
        /// <summary>
        /// 设置短信息状态
        /// </summary>
        /// <param name="pmId">短信息ID</param>
        /// <param name="state">状态值</param>
        /// <returns>更新记录数</returns>
        int SetPrivateMessageState(int pmId, byte state);
        /// <summary>
        /// 设置当前版块主题数(不含子版块)
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns>主题数</returns>
        int SetRealCurrentTopics(int fid);
        /// <summary>
        /// 设置版块状态
        /// </summary>
        /// <param name="status">板块状态</param>
        /// <param name="fid">板块ID</param>
        void SetStatusInForum(int status, int fid);
        /// <summary>
        /// 禁言用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        void SetStopTalkUser(string uidList);
        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topicList">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        int SetTopicClose(string topicList, short intValue);
        /// <summary>
        /// 设置主题状态
        /// </summary>
        /// <param name="topicList"></param>
        /// <param name="field"></param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        int SetTopicStatus(string topicList, string field, string intValue);
        /// <summary>
        /// 设置主题分类
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="value">分类ID</param>
        /// <returns></returns>
        bool SetTypeid(string topicList, int value);
        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmNum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        int SetUserNewPMCount(int uid, int pmNum);
        /// <summary>
        /// 设置用户在线状态
        /// </summary>
        /// <param name="uid">在线用户id</param>
        /// <param name="onlineState">在线用户状态</param>
        /// <returns></returns>
        int SetUserOnlineState(int uid, int onlineState);
        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <param name="shrinkSize">收缩大小</param>
        /// <param name="dbName">数据库名</param>
        void ShrinkDataBase(string shrinkSize, string dbName);
        /// <summary>
        /// 开始填充全文索引
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        int StartFullIndex(string dbName);
        /// <summary>
        /// 测试全文索引
        /// </summary>
        /// <param name="postTableId">分表ID</param>
        void TestFullTextIndex(int postTableId);
        /// <summary>
        /// 更新用户动作
        /// </summary>
        /// <param name="olid">在线用户id</param>
        /// <param name="action">用户操作</param>
        /// <param name="fid">版块id</param>
        /// <param name="forumName">版块名称</param>
        /// <param name="tid">主题id</param>
        /// <param name="topicTitle">主题标题</param>
        void UpdateAction(int olId, int action, int fid, string forumName, int tid, string topicTitle);
        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        void UpdateAction(int olId, int action, int inid);
        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="adId">广告Id</param>
        /// <param name="available">是否生效</param>
        /// <param name="type">广告类型</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="title">广告标题</param>
        /// <param name="targets">广告投放范围</param>
        /// <param name="parameters">展现方式</param>
        /// <param name="code">广告内容</param>
        /// <param name="startTime">生效时间</param>
        /// <param name="endTime">结束时间</param>
        int UpdateAdvertisement(int aid, int available, string type, int displayOrder, string title, string targets, string parameters, string code, string startTime, string endTime);
        /// <summary>
        /// 更新广告可用状态
        /// </summary>
        /// <param name="aidList">广告Id</param>
        /// <param name="available"></param>
        /// <returns></returns>
        int UpdateAdvertisementAvailable(string aidList, int available);
        /// <summary>
        /// 更新通告
        /// </summary>
        /// <param name="announcementInfo">公告对象</param>
        /// <returns></returns>
        int UpdateAnnouncement(AnnouncementInfo announcementInfo);
        /// <summary>
        /// 更新公告的创建者用户名
        /// </summary>
        /// <param name="posterId">posterId</param>
        /// <param name="poster">新用户名</param>
        void UpdateAnnouncementPoster(int posterId, string poster);
        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="attachmentInfo">附件对象</param>
        /// <returns>返回被更新的数量</returns>
        int UpdateAttachment(AttachmentInfo attachmentInfo);
        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="aid">附件id</param>
        void UpdateAttachmentDownloads(int aid);
        /// <summary>
        /// 更新附件到另一主题
        /// </summary>
        /// <param name="oldTid"></param>
        /// <param name="newTid"></param>
        /// <returns></returns>
        int UpdateAttachmentTidToAnotherTopic(int oldTid, int newTid);
        /// <summary>
        /// 更新允许的附件类型
        /// </summary>
        /// <param name="extension">附件类型扩展名</param>
        /// <param name="maxSize">大小</param>
        /// <param name="id">附件类型ID</param>
        void UpdateAttchType(string extension, string maxSize, int id);
        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authStr">验证串</param>
        /// <param name="authFlag">验证标志</param>
        void UpdateAuthStr(int uid, string authStr, int authFlag);
        /// <summary>
        /// 更新Discuz!NT代码
        /// </summary>
        /// <param name="available">是否启用</param>
        /// <param name="tag">标签</param>
        /// <param name="icon">图标</param>
        /// <param name="replacement">替换内容</param>
        /// <param name="example">示例</param>
        /// <param name="explanation">说明</param>
        /// <param name="param">参数</param>
        /// <param name="nest">嵌套次数</param>
        /// <param name="paramsDescription">参数描述</param>
        /// <param name="paramsDefaultValue">参数默认值</param>
        /// <param name="id">Id</param>
        void UpdateBBCCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescript, string paramsDefvalue, int id);
        /// <summary>
        /// 更新帖子分表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        int UpdateDetachTable(int fid, string description);
        /// <summary>
        /// 更新Email验证信息
        /// </summary>
        /// <param name="authStr"></param>
        /// <param name="authTime"></param>
        /// <param name="uid"></param>
        void UpdateEmailValidateInfo(string authStr, DateTime authTime, int uid);
        /// <summary>
        /// 更新版块信息
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="topicCount">主题数</param>
        /// <param name="postCount">帖子数</param>
        /// <param name="lastTid">最后回复主题</param>
        /// <param name="lastTitle">最后回复标题</param>
        /// <param name="lastPost">最后回复时间</param>
        /// <param name="lastPosterId">最后回复人ID</param>
        /// <param name="lastPoster">最后回复人</param>
        /// <param name="todayPostCount">当天发的帖子数</param>
        void UpdateForum(int fid, int topicCount, int postCount, int lastTid, string lastTitle, string lastPost, int lastPosterId, string lastPoster, int todayPostCount);
        /// <summary>
        /// 更新版块和用户模板Id
        /// </summary>
        /// <param name="templateIdList">模板Id列表</param>
        void UpdateForumAndUserTemplateId(string templateIdList);
        /// <summary>
        /// 更新板块的字段
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        int UpdateForumField(int fid, string fieldName, string fieldValue);
        /// <summary>
        /// 更新版块版主的名字
        /// </summary>
        /// <param name="oldName">旧版主名字</param>
        /// <param name="newName">新版主名字，为空则删除该版主</param>
        void UpdateModeratorName(string oldName, string newName);
        /// <summary>
        /// 更新论坛友情链接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayOrder"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="note"></param>
        /// <param name="logo"></param>
        /// <returns></returns>
        int UpdateForumLink(int id, int displayOrder, string name, string url, string note, string logo);
        /// <summary>
        /// 更新版块显示顺序
        /// </summary>
        /// <param name="minDisplayOrder"></param>
        void UpdateForumsDisplayOrder(int minDisplayOrder);
        /// <summary>
        /// 更新用户的用户组
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">组名</param>
        void UpdateGroupid(int userId, int groupId);
        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        void UpdateInvisible(int olId, int invisible);
        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olId">在线id</param>
        /// <param name="ip">ip地址</param>
        void UpdateIP(int olId, string ip);
        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olId">在线id</param>
        void UpdateLastTime(int olId);
        /// <summary>
        /// 更新勋章
        /// </summary>
        /// <param name="medalId">勋章ID</param>
        /// <param name="name">名称</param>
        /// <param name="image">图片</param>
        void UpdateMedal(int medalId, string name, string image);
        /// <summary>
        /// 更新用户勋章信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="medals">勋章信息</param>
        void UpdateMedals(int uid, string medals);
        /// <summary>
        /// 更新勋章授予记录
        /// </summary>
        /// <param name="newAction">新授予方式说明</param>
        /// <param name="postDateTime">更新日期</param>
        /// <param name="reason">理由</param>
        /// <param name="oldAction">原授予方式说明</param>
        /// <param name="medalId">勋章Id</param>
        /// <param name="uid">授予人Id</param>
        void UpdateMedalslog(string newActions, DateTime postDateTime, string reason, string oldActions, int medals, int uid);
        /// <summary>
        /// 更新勋章授予记录
        /// </summary>
        /// <param name="newAction">授予方式说明</param>
        /// <param name="postDateTime">更新日期</param>
        /// <param name="reason">理由</param>
        /// <param name="uid">授予人Id</param>
        void UpdateMedalslog(string actions, DateTime postDateTime, string reason, int uid);
        /// <summary>
        /// 更新帖子分表最大最小主题Id
        /// </summary>
        /// <param name="postTableName"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        int UpdateMinMaxField(string postTableName, int postTableId);
        /// <summary>
        /// 更新在线表
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <param name="displayOrder">序号</param>
        /// <param name="img">图片</param>
        /// <param name="title">名称</param>
        /// <returns></returns>
        int UpdateOnlineList(int groupId, int displayOrder, string img, string title);
        /// <summary>
        /// 更新在线表
        /// </summary>
        /// <param name="userGroupInfo"></param>
        void UpdateOnlineList(UserGroupInfo userGroupInfo);
        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olId">在线id</param>
        /// <param name="passWord">用户密码</param>
        void UpdatePassword(int olId, string passWord);
        /// <summary>
        /// 更新短信发送和接收者的用户名
        /// </summary>
        /// <param name="uid">Uid</param>
        /// <param name="newUserName">新用户名</param>
        void UpdatePMSenderAndReceiver(int uid, string newUserName);
        /// <summary>
        /// 更新投票
        /// </summary>
        /// <param name="pollInfo">更新投票</param>
        /// <returns></returns>
        bool UpdatePoll(PollInfo pollInfo);
        /// <summary>
        /// 更新帖子信息
        /// </summary>
        /// <param name="postsInfo">帖子信息</param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        int UpdatePost(PostInfo postsInfo, string postTableId);
        /// <summary>
        /// 更新帖子信息
        /// </summary>
        /// <param name="topicList">要移动的主题列表</param>
        /// <param name="fid">转到的版块ID</param>
        /// <param name="postTable"></param>
        void UpdatePost(string topicList, int fid, string postTable);
        /// <summary>
        /// 更新帖子是否包含附件
        /// </summary>
        /// <param name="pid">帖子id</param>
        /// <param name="postTableId">附件所属帖子id</param>
        /// <param name="hasAttachment"></param>
        /// <returns></returns>
        int UpdatePostAttachment(int pid, string postTableId, int hasAttachment);
        /// <summary>
        /// 更新帖子附件类型
        /// </summary>
        /// <param name="pid">帖子id</param>
        /// <param name="postTableId"></param>
        /// <param name="attType"></param>
        /// <returns></returns>
        int UpdatePostAttachmentType(int pid, string postTableId, int attType);
        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olId">在线id</param>
        void UpdatePostPMTime(int olId);
        /// <summary>
        /// 更新帖子作者名称
        /// </summary>
        /// <param name="uid">要更新的帖子作者的Uid</param>
        /// <param name="poster">作者的新用户名</param>
        void UpdatePostPoster(int posterId, string poster);
        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="postIdList">帖子ID列表</param>
        /// <param name="postTableId"></param>
        /// <returns>更新的帖子数量</returns>
        int UpdatePostRateTimes(string postIdList, string postTableId);
        /// <summary>
        /// 更新帖子评分
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="rate"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        int UpdatePostRate(int pid, float rate, string postTableId);
        /// <summary>
        /// 取消帖子评分
        /// </summary>
        /// <param name="postIdList"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        int CancelPostRate(string postIdList, string postTableId);
        /// <summary>
        /// 更新帖子所属主题
        /// </summary>
        /// <param name="postIdList"></param>
        /// <param name="tid"></param>
        /// <param name="postTableId"></param>
        void UpdatePostTid(string postIdList, int tid, string postTableId);
        /// <summary>
        /// 更新帖子到另一主题
        /// </summary>
        /// <param name="oldTid"></param>
        /// <param name="newTid"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        int UpdatePostTidToAnotherTopic(int oldTid, int newTid, string postTableId);
        /// <summary>
        /// 更新用户最后发帖时间
        /// </summary>
        /// <param name="olId">在线id</param>
        void UpdatePostTime(int olId);
        /// <summary>
        /// 更新评分范围
        /// </summary>
        /// <param name="rateRange"></param>
        /// <param name="groupId"></param>
        void UpdateRateRange(string rateRange, int groupId);
        /// <summary>
        /// 更新用户组积分设置信息
        /// </summary>
        /// <param name="rateRange">积分设置信息</param>
        /// <param name="groupId">用户组Id</param>
        void UpdateRaterangeByGroupid(string rateRange, int groupId);
        /// <summary>
        /// 更新最后搜索时间
        /// </summary>
        /// <param name="olId">在线id</param>
        void UpdateSearchTime(int olId);
        /// <summary>
        /// 更新表情
        /// </summary>
        /// <param name="id">表情ID</param>
        /// <param name="displayOrder">排序</param>
        /// <param name="type">类型</param>
        /// <param name="code">代码</param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        int UpdateSmilies(int id, int displayOrder, int type, string code);
        /// <summary>
        /// 更新指定名称的统计项
        /// </summary>
        /// <param name="param">项目名称</param>
        /// <param name="Value">指定项的值</param>
        /// <returns>更新数</returns>
        int UpdateStatistics(string param, string strValue);
        /// <summary>
        /// 更新最后回复人用户名
        /// </summary>
        /// <param name="lastUserId">Uid</param>
        /// <param name="lastUserName">新用户名</param>
        /// <returns></returns>
        int UpdateStatisticsLastUserName(int lastUserId, string lastUserName);
        /// <summary>
        /// 更新版块状态
        /// </summary>
        /// <param name="fidList">板块ID列表</param>
        void UpdateStatusByFidlist(string fidList);
        /// <summary>
        /// 更新版块状态
        /// </summary>
        /// <param name="fidList">板块ID列表</param>
        void UpdateStatusByFidlistOther(string fidList);
        /// <summary>
        /// 更新子版块数量
        /// </summary>
        /// <param name="subForumCount">子板块数</param>
        /// <param name="fid">板块ID</param>
        void UpdateSubForumCount(int subForumCount, int fid);
        /// <summary>
        /// 更新子版块数
        /// </summary>
        /// <param name="fid"></param>
        void UpdateSubForumCount(int fid);
        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postCount">帖子数</param>
        /// <param name="lastPostId">最后发帖人</param>
        /// <param name="lastPost">最后发帖时间</param>
        /// <param name="lastPosterId">最后发帖人ID</param>
        /// <param name="poster">最后发帖人</param>
        void UpdateTopic(int tid, int postCount, int lastPostId, string lastPost, int lastPosterId, string poster);
        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <param name="fid">版块id</param>
        /// <returns></returns>
        int UpdateTopic(string topicList, int fid, int topicType);
        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>成功返回1，否则返回0</returns>
        int UpdateTopic(TopicInfo topicInfo);
        /// <summary>
        /// 更新主题附件
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="hasAttachment"></param>
        /// <returns></returns>
        int UpdateTopicAttachment(int tid, int hasAttachment);
        /// <summary>
        /// 更新主题附件类型
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
        /// <returns></returns>
        int UpdateTopicAttachmentType(int tid, int attType);
        /// <summary>
        /// 更新主题是否需要回复可见
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        int UpdateTopicHide(int tid);
        /// <summary>
        /// 更新主题最后回复人
        /// </summary>
        /// <param name="lastPosterId">最后回复人的Uid</param>
        /// <param name="lastPoster">最后回复人的新用户名</param>
        void UpdateTopicLastPoster(int lastPosterId, string lastPoster);
        /// <summary>
        /// 更新主题最后发帖人Id
        /// </summary>
        /// <param name="tid"></param>
        void UpdateTopicLastPosterId(int tid);
        /// <summary>
        /// 更新主题为已被管理
        /// </summary>
        /// <param name="tidList">主题id列表</param>
        /// <param name="moderated">管理操作id</param>
        /// <returns>成功返回1，否则返回0</returns>
        int UpdateTopicModerated(string topicList, int moderated);
        /// <summary>
        /// 更新主题作者
        /// </summary>
        /// <param name="posterId">作者Id</param>
        /// <param name="poster">作者的新名称</param>
        void UpdateTopicPoster(int posterId, string poster);
        /// <summary>
        /// 列新主题的回复数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableId">当前帖子分表Id</param>
        int UpdateTopicReplyCount(int tid, string postTableId);
        /// <summary>
        /// 更新主题浏览量
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="viewCount">浏览量</param>
        /// <returns>成功返回1，否则返回0</returns>
        int UpdateTopicViewCount(int tid, int viewCount);
        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatar">头像</param>
        /// <param name="avatarWidth">头像宽度</param>
        /// <param name="avatarHeight">头像高度</param>
        /// <param name="templateId">模板Id</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        void UpdateUserPreference(int uid, string avatar, int avatarWidth, int avatarHeight, int templateId);
        /// <summary>
        /// 根据积分公式更新用户积分
        /// <param name="uid">用户ID</param>
        /// </summary>
        void UpdateUserCredits(int uid);
        /// <summary>
        /// 更新用户扩展积分
        /// <param name="uid">用户ID</param>
        /// <param name="values">扩展积分</param>
        /// </summary>
        void UpdateUserCredits(int uid, float[] values);
        /// <summary>
        /// 更新用户扩展积分
        /// <summary>
        /// <param name="uid">用户ID</param>
        /// <param name="values">扩展积分</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        void UpdateUserCredits(int uid, float[] values, int pos, int mount);
        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="userIdList">uid列表</param>
        /// <returns></returns>
        int UpdateUserDigest(string userIdList);
        /// <summary>
        /// 修改用户自定义积分字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extId">扩展字段序号(1-8)</param>
        /// <param name="pos">增加的数值(可以是负数)</param>
        /// <returns>执行是否成功</returns>
        void UpdateUserExtCredits(int uid, int extId, float pos);
        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        void UpdateUserForumSetting(UserInfo userInfo);
        /// <summary>
        /// 更新用户组信息
        /// </summary>
        /// <param name="userGroupInfo">用户组信息</param>
        void UpdateUserGroup(UserGroupInfo userGroupInfo);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="currentCreditsHigher"></param>
        /// <param name="creditsHigher"></param>
        void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int creditsHigher);
        /// <summary>
        /// 更新用户组积分上下限
        /// </summary>
        /// <param name="groupid"></param>
        void UpdateUserGroupLowerAndHigherToLimit(int groupId);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="creditsHigher"></param>
        /// <param name="creditsLower"></param>
        void UpdateUserGroupsCreditsHigherByCreditsHigher(int creditsHigher, int creditsLower);
        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="creditsLower"></param>
        /// <param name="creditsHigher"></param>
        void UpdateUserGroupsCreditsLowerByCreditsLower(int creditsLower, int creditsHigher);
        /// <summary>
        /// 更新用户最后访问时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="ip"></param>
        void UpdateUserLastvisit(int uid, string ip);
        /// <summary>
        /// 更新在线表用户信息
        /// </summary>
        /// <param name="groupId">用户组id</param>
        /// <param name="userId">用户ID</param>
        void UpdateUserOnlineInfo(int groupId, int userId);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uidList">用户uid列表</param>
        /// <param name="onlineState">当前在线状态(0:离线,1:在线)</param>
        /// <param name="activityTime"></param>
        void UpdateUserOnlineStateAndLastActivity(string uidList, int onlineState, string activityTime);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uid">用户uid列表</param>
        /// <param name="onlineState">当前在线状态(0:离线,1:在线)</param>
        /// <param name="activityTime"></param>
        void UpdateUserOnlineStateAndLastActivity(int uid, int onlineState, string activityTime);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uid">用户uid列表</param>
        /// <param name="onlineState">当前在线状态(0:离线,1:在线)</param>
        /// <param name="activityTime"></param>
        void UpdateUserOnlineStateAndLastVisit(int uid, int onlineState, string activityTime);
        /// <summary>
        /// 更新用户在线信息
        /// </summary>
        /// <param name="uidList">用户uid列表</param>
        /// <param name="onlineState">当前在线状态(0:离线,1:在线)</param>
        /// <param name="activityTime"></param>
        void UpdateUserOnlineStateAndLastVisit(string uidList, int onlineState, string activityTime);
        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        /// <param name="activityTime"></param>
        void UpdateUserLastActivity(int uid, string activityTime);
        /// <summary>
        /// 更新用户其他信息
        /// </summary>
        /// <param name="groupId">用户组id</param>
        /// <param name="userId">用户ID</param>
        void UpdateUserOtherInfo(int groupId, int userId);
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="passWord">密码</param>
        /// <param name="originalPassWord">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        void UpdateUserPassword(int uid, string passWord, bool originalPassWord);
        /// <summary>
        /// 更新用户帖数
        /// </summary>
        /// <param name="postCount">帖子数</param>
        /// <param name="userId">用户ID</param>
        void UpdateUserPostCount(int postCount, int userId);
        /// <summary>
        /// 更新所有用户的
        /// </summary>
        /// <param name="postTableId"></param>
        void UpdateAllUserPostCount(int postTableId);
        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        void UpdateUserProfile(UserInfo userInfo);
        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="secques">用户安全问题答案的存储数据</param>
        /// <returns>成功返回true否则false</returns>
        void UpdateUserSecques(int uid, string secques);
        /// <summary>
        /// 更新用户空间Id
        /// </summary>
        /// <param name="userId">要更新的SpaceId</param>
        void UpdateUserSpaceId(int userId);
        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceId">要更新的SpaceId</param>
        /// <param name="userId">要更新的UserId</param>
        /// <returns>是否更新成功</returns>
        void UpdateUserSpaceId(int spaceId, int userId);
        /// <summary>
        /// 更新过滤词条
        /// </summary>
        /// <param name="id">词条Id</param>
        /// <param name="find">查找词</param>
        /// <param name="replacement">替换内容</param>
        /// <returns></returns>
        int UpdateWord(int id, string find, string replacement);
        /// <summary>
        /// 更新过滤词
        /// </summary>
        /// <param name="find">要替换的词</param>
        /// <param name="replacement">被替换的词</param>
        void UpdateBadWords(string find, string replacement);
        /// <summary>
        /// 获取热门版块
        /// </summary>
        /// <param name="topNumber">获取数量</param>
        /// <param name="orderby">排序方式,todayposts:今日发帖数,topics:主题数,posts:帖子总数</param>
        /// <param name="fid">板块ID</param>
        /// <returns></returns>
        DataTable GetWebSiteAggHotForumList(int topNumber, string orderby, int fid);
        /// <summary>
        /// 获取热门图片
        /// </summary>
        /// <param name="count">热门图片数量</param> 
        /// <param name="orderby">排序方式,aid:最新图片,downloads:查看数</param>
        /// <param name="fid">板块ID</param>
        /// <param name="continuous">是否是可以从一个帖子内连续取图片</param>
        /// <returns></returns>
        DataTable GetWebSiteAggHotImages(int count, string orderby, string forumlist, int continuous);
        /// <summary>
        /// 创建主题标签(已存在的标签不会被创建)
        /// </summary>
        /// <param name="tags">标签, 以半角空格分隔</param>
        /// <param name="topicId">主题Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="curdateTime">提交时间</param>
        void CreateTopicTags(string tags, int topicId, int userId, string curdateTime);
        /// <summary>
        /// 获取主题所包含的Tag
        /// </summary>
        /// <param name="topicId">主题Id</param>
        /// <returns></returns>
        IDataReader GetTagsListByTopic(int topicId);
        /// <summary>
        /// 获取论坛热门标签
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        IDataReader GetHotTagsListForForum(int count);
        /// <summary>
        /// 获取Tag信息
        /// </summary>
        /// <param name="tagId">标签id</param>
        /// <returns></returns>
        IDataReader GetTagInfo(int tagId);
        /// <summary>
        /// 获取使用同一tag的主题列表
        /// </summary>
        /// <param name="tagId">TagId</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        IDataReader GetTopicListByTag(int tagId, int pageIndex, int pageSize);
        /// <summary>
        /// 获取相关主题
        /// </summary>
        /// <param name="topicId">主题Id</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        IDataReader GetRelatedTopics(int topicId, int count);
        /// <summary>
        /// 获取使用指定Tag的主题数
        /// </summary>
        /// <param name="tagId">TagId</param>
        /// <returns></returns>
        int GetTopicsCountByTag(int tagId);
        /// <summary>
        /// 设置上次任务计划的执行时间
        /// </summary>
        /// <param name="key">任务的标识</param>
        /// <param name="serverName">主机名</param>
        /// <param name="lastExecuted">最后执行时间</param>
        void SetLastExecuteScheduledEventDateTime(string key, string serverName, DateTime lastExecuted);
        /// <summary>
        /// 获取上次任务计划的执行时间
        /// </summary>
        /// <param name="key">任务的标识</param>
        /// <param name="serverName">主机名</param>
        /// <returns></returns>
        DateTime GetLastExecuteScheduledEventDateTime(string key, string serverName);
        /// <summary>
        /// 整理相关主题表
        /// </summary>
        void NeatenRelateTopics();
        /// <summary>
        /// 删除主题的相关标签
        /// </summary>
        /// <param name="topicId">主题ID</param>
        void DeleteTopicTags(int topicId);
        /// <summary>
        /// 删除主题的相关主题记录
        /// </summary>
        /// <param name="topicId">主题ID</param>
        void DeleteRelatedTopics(int topicId);
        /// <summary>
        /// 更新昨日发帖数
        /// </summary>
        void UpdateYesterdayPosts(string postTableId);
        /// <summary>
        /// 返回论坛Tag列表
        /// </summary>
        /// <param name="tagKey">查询关键字</param>
        /// <param name="type">全部0 锁定1 开放2</param>
        /// <returns></returns>
        DataTable GetForumTags(string tagKey, int type);
        /// <summary>
        /// 获取一定范围内的主题
        /// </summary>
        /// <param name="tagName">标签名称</param>
        /// <param name="from">板块</param>
        /// <param name="end"></param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        DataTable GetTopicNumber(string tagName, int from, int end, int type);
        /// <summary>
        /// 更新TAG
        /// </summary>
        /// <param name="tagId">标签ID</param>
        /// <param name="orderId">排序</param>
        /// <param name="color">颜色</param>
        void UpdateForumTags(int tagId, int orderId, string color);
        /// <summary>
        /// 获取开放论坛的列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOpenForumList();
        /// <summary>
        /// 增加悬赏日志
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="authorId">悬赏者Id</param>
        /// <param name="winerId">获奖者Id</param>
        /// <param name="winnerName">获奖者用户名</param>
        /// <param name="postId">帖子Id</param>
        /// <param name="bonus">奖励积分</param>
        /// <param name="extId">进行悬赏时的交易积分</param>
        /// <param name="isBest">是否是最佳答案</param>
        void AddBonusLog(int tid, int authorId, int winerId, string winnerName, int postId, int bonus, int extId, int isBest);
        /// <summary>
        /// 获取指定主题的给分记录
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableId">主题所在的分表</param>
        /// <returns>悬赏日志集合</returns>
        IDataReader GetTopicBonusLogs(int tid, string postTableId);
        /// <summary>
        /// 更新统计表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="count"></param>
        void UpdateStats(string type, string variable, int count);
        /// <summary>
        /// 更新统计变量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        void UpdateStatVars(string type, string variable, string value);
        /// <summary>
        /// 获得所有统计信息
        /// </summary>
        /// <returns></returns>
        IDataReader GetAllStats();
        /// <summary>
        /// 获得所有统计
        /// </summary>
        /// <returns></returns>
        IDataReader GetAllStatVars();
        /// <summary>
        /// 删除旧的帖数统计
        /// </summary>
        void DeleteOldDayposts();
        /// <summary>
        /// 统计板块数量
        /// </summary>
        /// <returns></returns>
        int GetForumCount();
        /// <summary>
        /// 获得今日发帖数
        /// </summary>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        int GetTodayPostCount(string postTableId);
        /// <summary>
        /// 获得今日新用户数
        /// </summary>
        /// <returns></returns>
        int GetTodayNewMemberCount();
        /// <summary>
        /// 获得管理员数量
        /// </summary>
        /// <returns></returns>
        int GetAdminCount();
        /// <summary>
        /// 获得未发帖的会员数
        /// </summary>
        /// <returns></returns>
        int GetNonPostMemCount();
        /// <summary>
        /// 获得本日最佳会员
        /// </summary>
        /// <param name="postTableId"></param>
        IDataReader GetBestMember(string postTableId);
        /// <summary>
        /// 获得每月帖数统计
        /// </summary>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        IDataReader GetMonthPostsStats(string postTableId);
        /// <summary>
        /// 获得30天内的每日发帖统计
        /// </summary>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        IDataReader GetDayPostsStats(string postTableId);
        /// <summary>
        /// 获得热门主题
        /// </summary>
        /// <returns></returns>
        IDataReader GetHotTopics(int count);
        /// <summary>
        /// 获得热门回复主题
        /// </summary>
        /// <returns></returns>
        IDataReader GetHotReplyTopics(int count);
        /// <summary>
        /// 获得主题数板块排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IDataReader GetForumsByTopicCount(int count);
        /// <summary>
        /// 获得发帖数板块排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IDataReader GetForumsByPostCount(int count);
        /// <summary>
        /// 获得30天发帖数排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        IDataReader GetForumsByMonthPostCount(int count, string postTableId);
        /// <summary>
        /// 获得当日发帖板块排行榜
        /// </summary>
        /// <param name="count"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        IDataReader GetForumsByDayPostCount(int count, string postTableId);
        /// <summary>
        /// 获得用户排行
        /// </summary>
        /// <param name="count"></param>
        /// <param name="postTableId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IDataReader GetUsersRank(int count, string postTableId, string type);
        /// <summary>
        /// 获得用户排行
        /// </summary>
        /// <param name="filed">当月还是总在线时间</param>
        /// <returns></returns>
        IDataReader GetUserByOnlineTime(string filed);
        /// <summary>
        /// 获取趋势图形信息
        /// </summary>
        /// <param name="field">读取字段</param>
        /// <param name="begin">起始日期</param>
        /// <param name="end">结束日期</param>
        /// <returns></returns>
        IDataReader GetTrendGraph(string field, string begin, string end);
        /// <summary>
        /// 获取主题中每条获奖帖子的得分记录
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        IDataReader GetTopicBonusLogsByPost(int tid);
        /// <summary>
        /// 更新统计数据
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="os"></param>
        /// <param name="visitorsAdd"></param>
        void UpdateStatCount(string browser, string os, string visitorsAdd);
        /// <summary>
        /// 增加辩论主题扩展信息
        /// </summary>
        /// <param name="debateTopic"></param>
        void CreateDebateTopic(DebateInfo debateTopic);
        /// <summary>
        /// 获取所有开放版块，即获取条件为autoclose=0 and password='' and redirect=''的版块
        /// </summary>
        /// <returns></returns>
        DataTable GetAllOpenForum();
        /// <summary>
        /// 获取辩论的扩展信息
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>辩论主题扩展信息</returns>
        IDataReader GetDebateTopic(int tid);
        /// <summary>
        /// 更新辩论信息
        /// </summary>
        /// <param name="debateInfo">辩论信息</param>
        /// <returns></returns>
        bool UpdateDebateTopic(DebateInfo debateInfo);
        /// <summary>
        /// 获取最热辩论数据
        /// </summary>
        /// <param name="hotField">按照用户指定方式来获取热帖，</param>
        /// <param name="defHotCount">按照用户指定方式，并且要大于等于用户指定的数量</param>
        /// <param name="getCount">获取热帖的条数</param>
        /// <returns></returns>
        IDataReader GetHotDebatesList(string hotField, int defHotCount, int getCount);
        /// <summary>
        /// 获取推荐辩论帖数据
        /// </summary>
        /// <param name="tidList">主题ID列表</param>
        /// <returns></returns>
        IDataReader GetRecommendDebates(string tidList);
        /// <summary>
        /// 创建参加辩论帖子的扩展信息
        /// </summary>
        /// <param name="dpei">参与辩论的帖子的辩论相关扩展字段实体</param>
        void CreateDebatePostExpand(DebatePostExpandInfo dpei);
        /// <summary>
        /// 增加点评信息
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="tableId"></param>
        /// <param name="commentMsg">点评内容</param>
        void AddCommentDabetas(int tid, int tableId, string commentMsg);
        /// <summary>
        /// 增加顶
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="field"></param>
        /// <param name="ip"></param>
        /// <param name="userInfo">用户信息</param>
        void AddDebateDigg(int tid, int pid, int field, string ip, UserInfo userInfo);
        /// <summary>
        /// 判断是否可以顶
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>判断是否顶过</returns>
        bool AllowDiggs(int pid, int userId);
        /// <summary>
        /// 获取辩论中某一方的帖子列表
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="opinion"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="postTableId"></param>
        /// <param name="postOrderType"></param>
        /// <returns></returns>
        IDataReader GetDebatePostList(int tid, int opinion, int pageSize, int pageIndex, string postTableId, PostOrderType postOrderType);
        /// <summary>
        /// 获取指定版块下的最新回复
        /// </summary>
        /// <param name="fid">指定的版块</param>
        /// <param name="count">返回记录数</param>
        /// <param name="postTableName">当前分表名称</param>
        /// <param name="visibleForum">可见版块列表</param>
        /// <returns></returns>
        DataTable GetLastPostList(int fid, int count, string postTableName, string visibleForum);
        /// <summary>
        /// 得到用户顶过的记录
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>用户已顶过帖子PID</returns>
        IDataReader GetUserDiggs(int tid, int uid);
        /// <summary>
        /// 修复辩论主题的支持数
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="debateOpinion">辩论所持观点</param>
        int ReviseDebateTopicDiggs(int tid, int debateOpinion);
        /// <summary>
        /// 获取辩论帖的支持数
        /// </summary>
        /// <param name="pidList">帖子ID数组</param>
        /// <returns></returns>
        IDataReader GetDebatePostDiggs(string pidList);
        /// <summary>
        /// 获取指定版块的最新回复主题id
        /// </summary>
        /// <param name="forumInfo">指定的版块信息</param>
        /// <param name="visibleForum">可见版块列表</param>
        /// <returns></returns>
        int GetLastPostTid(ForumInfo forumInfo, string visibleForum);
        /// <summary>
        /// 更新指定版块的最新发帖信息
        /// </summary>
        /// <param name="foruminfo">当前版块信息</param>
        /// <param name="postinfo">要更新的帖子信息</param>
        /// <returns></returns>
        void UpdateLastPost(ForumInfo forumInfo, PostInfo postInfo);
        /// <summary>
        /// 更新在线时间
        /// </summary>
        /// <param name="olTimeSpan">在线时间间隔</param>
        /// <param name="uid">当前用户id</param>
        void UpdateOnlineTime(int olTimeSpan, int uid);
        /// <summary>
        /// 重置每月在线时间(清零)
        /// </summary>
        void ResetThismonthOnlineTime();
        /// <summary>
        /// 同步在线时间
        /// </summary>
        /// <param name="uid">用户id</param>
        void SynchronizeOnlineTime(int uid);
        /// <summary>
        /// 返回帖子列表
        /// </summary>
        /// <param name="postList">帖子ID</param>
        /// <param name="tableId">帖子分表ID</param>
        /// <returns>帖子列表</returns>
        DataTable GetPostList(string postList, string tableId);
        /// <summary>
        /// 获取指定的主题过滤的条件
        /// </summary>
        /// <param name="filter">过滤类型</param>
        /// <returns></returns>
        string GetTopicFilterCondition(string filter);
        /// <summary>
        /// 获取用户组中设置的图片空间最大尺寸
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupMaxspacephotosize();
        /// <summary>
        /// 获取用户组中设置的空间附件最大尺寸
        /// </summary>
        /// <returns></returns>
        DataTable GetUserGroupMaxspaceattachsize();
        /// <summary>
        /// 清除用户的SpaceId
        /// </summary>
        /// <param name="uid">要清除的用户</param>
        void ClearUserSpace(int uid);
        /// <summary>
        /// 获取指定用户和通知类型的通知信息
        /// </summary>
        /// <param name="uid">指定的用户id</param>
        /// <param name="noticeType">通知类型</param>
        /// <returns></returns>
        IDataReader GetNoticeByUid(int uid, NoticeType noticeType);
        /// <summary>
        /// 获取指定通知id的信息
        /// </summary>
        /// <param name="nid">通知id</param>
        /// <returns>通知信息</returns>
        IDataReader GetNoticeByNid(int nid);
        /// <summary>
        /// 获取指定通知id和类型的通知
        /// </summary>
        /// <param name="uid">指定通知id</param>
        /// <param name="noticeType"><see cref="Noticetype"/>通知类型</param>
        /// <param name="pageId">分页id</param>
        /// <param name="pageSize">页面尽寸</param>
        /// <returns></returns>
        IDataReader GetNoticeByUid(int uid, NoticeType noticeType, int pageId, int pageSize);
        /// <summary>
        /// 添加指定的通知信息
        /// </summary>
        /// <param name="noticeInfo">要添加的通知信息</param>
        /// <returns></returns>
        int CreateNoticeInfo(NoticeInfo noticeInfo);
        /// <summary>
        /// 更新指定的通知信息(注释无用方法 2011-04-12)
        /// </summary>
        /// <param name="noticeInfo">要更新的通知信息</param>
        /// <returns></returns>
        //bool UpdateNoticeInfo(NoticeInfo noticeInfo);
        /// <summary>
        /// 删除指定通知id的信息
        /// </summary>
        /// <param name="nid">指定的通知id</param>
        /// <returns></returns>
        bool DeleteNoticeByNid(int nid);
        /// <summary>
        /// 删除指定用户id的通知信息
        /// </summary>
        /// <param name="uid">指定的通知id</param>
        /// <returns></returns>
        bool DeleteNoticeByUid(int uid);
        /// <summary>
        /// 获取指定用户id及通知类型的通知数
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="noticeType">通知类型</param>
        /// <returns></returns>
        int GetNoticeCountByUid(int uid, NoticeType noticeType);
        /// <summary>
        /// 获取需要审核的主题
        /// </summary>
        /// <param name="forumIdList">版块ID列表</param>
        /// <param name="tpp">每页主题数</param>
        /// <param name="pageId">页数</param>
        /// <param name="filter">displayorder过滤器</param>
        /// <returns></returns>
        IDataReader GetUnauditNewTopic(string forumIdList, int tpp, int pageId, int filter);
        /// <summary>
        /// 获取指定用户和分页下的通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        int GetNewNoticeCountByUid(int uid);
        /// <summary>
        /// 更新指定用户的通知新旧状态
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="newType">通知新旧状态(1:新通知 0:旧通知)</param>
        void UpdateNoticeNewByUid(int uid, int newType);
        /// <summary>
        /// 删除指定通知类型和天数内的通知
        /// </summary>
        /// <param name="noticeType">删除的通知类型</param>
        /// <param name="days">指定天数</param>
        void DeleteNotice(NoticeType noticeType, int days);
        /// <summary>
        /// 得到公共消息数量
        /// </summary>
        /// <returns>公共消息数量</returns>
        int GetAnnouncePrivateMessageCount();
        /// <summary>
        /// 获得公共消息列表
        /// </summary>
        /// <param name="pageSize">每页显示短信息数</param>
        /// <param name="pageIndex">当前要显示的页数</param>
        /// <returns>公共消息列表</returns>
        IDataReader GetAnnouncePrivateMessageList(int pageSize, int pageIndex);
        /// <summary>
        /// 得到自定义菜单
        /// </summary>
        /// <returns></returns>
        IDataReader GetNavigationData(bool getAllNavigation);
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="nav">导航菜单</param>
        void InsertNavigation(NavInfo nav);
        /// <summary>
        /// 更校菜单
        /// </summary>
        /// <param name="nav">导航菜单</param>
        void UpdateNavigation(NavInfo nav);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        void DeleteNavigation(int id);
        /// <summary>
        /// 得到拥有子菜单的主菜单ID
        /// </summary>
        /// <returns></returns>
        IDataReader GetNavigationHasSub();
        /// <summary>
        /// 更新signature签名 location来自 bio个人介绍
        /// </summary>
        /// <param name="location"></param>
        /// <param name="bio"></param>
        /// <param name="signature"></param>
        /// <param name="uid"></param>
        void UpdateUserShortInfo(string location, string bio, string signature, int uid);
        /// <summary>
        /// 添加被住址的ip
        /// </summary>
        /// <param name="info"></param>
        void AddBannedIp(IpInfo info);
        /// <summary>
        /// 显示被住址的ip列表
        /// </summary>
        /// <returns></returns>
        int GetBannedIpCount();
        /// <summary>
        /// 指定的被deny的ip数量
        /// </summary>
        /// <returns></returns>
        IDataReader GetBannedIpList();
        /// <summary>
        /// 获取指定分页的禁止IP列表
        /// </summary>
        /// <param name="num"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        IDataReader GetBannedIpList(int num, int pageId);
        /// <summary>
        /// 更新被禁止的用户
        /// </summary>
        /// <param name="groupId">用户组id</param>
        /// <param name="groupExpiry">过期时间</param>
        /// <param name="uid">用户id</param>
        void UpdateBanUser(int groupId, string groupExpiry, int uid);
        /// <summary>
        /// 删除选中的ip地址段
        /// </summary>
        /// <param name="bannedIdList"></param>
        int DeleteBanIp(string bannedIdList);
        /// <summary>
        /// 编辑banip结束时间
        /// </summary>
        /// <param name="iplist"></param>
        /// <param name="endTime"></param>
        int UpdateBanIpExpiration(int id, string endTime);
        /// <summary>
        /// 更新文件夹显示顺序
        /// </summary>
        /// <param name="displayOrder">排序序号</param>
        /// <param name="aId">公告id</param>
        int UpdateAnnouncementDisplayOrder(int displayOrder, int aId);
        /// <summary>
        /// 获取包含该特殊用户的版块
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        DataTable GetForumTableBySpecialUser(string userName);
        /// <summary>
        /// 联合查询 获得perlist和fname，fid
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <returns></returns>
        DataTable GetForumTableWithSpecialUser(int fid);
        /// <summary>
        /// 获取指定论坛的特殊用户
        /// </summary>
        /// <param name="fid">板块id</param>
        /// <returns></returns>
        DataTable SearchSpecialUser(int fid);
        /// <summary>
        /// 更新特定板块特殊用户
        /// </summary>
        /// <param name="permUserList">特殊用户列表</param>
        /// <param name="fid">板块id</param>
        void UpdateSpecialUser(string permUserList, int fid);
        /// <summary>
        /// 更新用户新短消息数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="pluscount">增加量</param>
        /// <returns></returns>
        int UpdateNewPms(int olId, int plusCount);
        /// <summary>
        /// 更新用户新通知数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="plusCount">增加量</param>
        /// <returns></returns>
        int UpdateNewNotices(int olId, int plusCount);
        /// <summary>
        /// 获取需要被关注的主题列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="tpp">分页数</param>
        /// <param name="pageId">当前页数</param>
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        IDataReader GetAttentionTopics(string fidList, int tpp, int pageId, string keyWord);
        /// <summary>
        /// 获取需要关注的列表的数量
        /// </summary>
        /// <param name="fidList">版块ID列表g</param>
        /// <param name="keyWord">搜索关键字</param>
        /// <returns></returns>
        int GetAttentionTopicCount(string fidList, string keyWord);
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
        void UpdateTopicAttentionByFidList(string fidList, int attention, string dateTime);
        /// <summary>
        /// 获取指定用户id下未使用的附件
        /// </summary>
        /// <param name="userid">指定用户id</param>
        /// <param name="posttime">获取指定时间后的新附件,空为不限时间</param>
        /// <param name="attachmentType">附件的类型</param>
        /// <returns></returns>
        IDataReader GetNoUsedAttachmentListByUid(int userId, string posttime, int isimage);
        /// <summary>
        /// 获取用户附件及未使用附件列表
        /// </summary>
        /// <param name="userid">指定用户id</param>
        /// <param name="aidList">附件ID列表</param>
        /// <returns></returns>
        IDataReader GetEditPostAttachList(int userid, string aidList);
        /// <summary>
        /// 删除未使用的论坛附件
        /// </summary>
        int DeleteNoUsedForumAttachment();
        /// <summary>
        /// 获取未使用的论坛附件
        /// </summary>
        IDataReader GetNoUsedForumAttachment();
        /// <summary>
        /// 查询相同的通知数
        /// </summary>
        /// <param name="type">通知类型</param>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        int ReNewNotice(int type, int uid);
        /// <summary>
        /// 创建附件交易信息
        /// </summary>
        /// <param name="attachPaymentLogInfo">要创建的附件交易信息</param>
        /// <returns>创建的交易id</returns>
        int CreateAttachPaymetLog(AttachPaymentlogInfo attachPaymentLogInfo);
        /// <summary>
        /// 更新的附件交易信息
        /// </summary>
        /// <param name="attachPaymentLogInfo">要更新的附件交易信息</param>
        /// <returns></returns>
        int UpdateAttachPaymetLog(AttachPaymentlogInfo attachPaymentLogInfo);
        /// <summary>
        /// 获取指定符件id的附件交易日志
        /// </summary>
        /// <param name="aid">指定附件id</param>
        /// <returns>附件交易日志</returns>
        IDataReader GetAttachPaymentLogByAid(int aid);
        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="extNumber">指定扩展字段</param>
        /// <returns>扩展字展积分值</returns>
        int GetUserExtCreditsByUserid(int uid, int extNumber);
        /// <summary>
        /// 查看用户是否购买过指定附件
        /// </summary>
        /// <param name="userId">购买用户id</param>
        /// <param name="aId">附件id</param>
        /// <returns>交易日志</returns>
        bool HasBoughtAttach(int userId, int aid);
        /// <summary>
        /// 获取指定附件id和用户的交易附件列表
        /// </summary>
        /// <param name="attachidlist">指定的附件id列表</param>
        /// <param name="uid">指定的附件id</param>
        /// <returns></returns>
        IDataReader GetAttachPaymentLogByUid(string attachIdList, int uid);
        /// <summary>
        /// 获得指定用户的新通知
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IDataReader GetNewNotices(int userId);
        /// <summary>
        /// 按条件获取主题
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable GetTopicsByCondition(string condition);
        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetHelpList();
        /// <summary>
        /// 获取指定ID的帮助信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IDataReader ShowHelp(int id);
        /// <summary>
        /// 添加帮助信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="pid"></param>
        /// <param name="orderBy"></param>
        void AddHelp(string title, string message, int pid, int orderBy);
        /// <summary>
        /// 删除帮助信息
        /// </summary>
        /// <param name="idList"></param>
        void DelHelp(string idList);
        /// <summary>
        /// 更新帮助信息
        /// </summary>
        /// <param name="id">帮助ID</param>
        /// <param name="title">帮助标题</param>
        /// <param name="message">帮助内容</param>
        /// <param name="pid">帮助</param>
        /// <param name="ordeorderByrby">排序方式</param>
        void UpdateHelp(int id, string title, string message, int pid, int orderBy);
        /// <summary>
        /// 获取帮助信息条数
        /// </summary>
        /// <returns></returns>
        int HelpCount();
        /// <summary>
        /// 获取帮助信息类型
        /// </summary>
        /// <returns></returns>
        DataTable GetHelpTypes();
        /// <summary>
        /// 更新帮助信息
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="id"></param>
        void UpdateOrder(string orderBy, string id);
        /// <summary>
        /// 获取管理组信息
        /// </summary>
        /// <returns></returns>
        DataTable GetAdminGroups();
        /// <summary>
        /// 获取指定关键字的主题类型
        /// </summary>
        /// <param name="searthKeyWord"></param>
        /// <returns></returns>
        DataTable GetTopicTypes(string searthKeyWord);
        /// <summary>
        /// 获取已绑定主题类型的版块列表
        /// </summary>
        /// <returns></returns>
        DataTable GetExistTopicTypeOfForum();
        /// <summary>
        /// 更新主题分类
        /// </summary>
        /// <param name="topicTypes">板块的主题分类</param>
        /// <param name="fid">版块ID</param>
        void UpdateTopicTypeForForum(string topicTypes, int fid);
        /// <summary>
        /// 更新主题分类
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <param name="displayOrder">排序号</param>
        /// <param name="description">介绍</param>
        /// <param name="typeId">分类ID</param>
        void UpdateTopicTypes(string name, int displayOrder, string description, int typeId);
        /// <summary>
        /// 添加主题类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="displayOrder"></param>
        /// <param name="description"></param>
        void AddTopicTypes(string typeName, int displayOrder, string description);
        /// <summary>
        /// 删除指定类型ID列表的主题信息
        /// </summary>
        /// <param name="typeIdList">主题分类Id列表</param>
        void DeleteTopicTypesByTypeidlist(string typeIdList);
        /// <summary>
        /// 获取版块的主题类型
        /// </summary>
        /// <returns></returns>
        DataTable GetForumNameIncludeTopicType();
        /// <summary>
        /// 更校指定类型的主题类型信息
        /// </summary>
        /// <param name="typeId"></param>
        void ClearTopicTopicType(int typeId);
        /// <summary>
        /// 更新表情
        /// </summary>
        /// <param name="id">表情ID</param>
        /// <param name="displayOrder">排序</param>
        /// <param name="code">代码</param>
        /// <returns></returns>
        int UpdateSmiliesPart(string code, int displayOrder, int id);
        /// <summary>
        /// 获取指定type的smilies信息
        /// </summary>
        /// <param name="typeId">分类Id</param>
        /// <returns></returns>
        DataTable GetSmiliesInfoByType(int typeId);
        /// <summary>
        /// 获取主题鉴定列表信息
        /// </summary>
        /// <returns></returns>
        IDataReader GetTopicsIdentifyItem();
        /// <summary>
        /// 根据主题ID列表取出主题帖子
        /// </summary>
        /// <param name="postTableId">分表ID</param>
        /// <param name="tidList">主题ID列表</param>
        /// <returns></returns>
        DataTable GetTopicListByTidlist(string postTableId, string tidList);
        /// <summary>
        /// 更新用户短消息设置
        /// </summary>
        /// <param name="user">用户信息</param>
        void UpdateUserPMSetting(UserInfo user);
        /// <summary>
        /// 更新指定主题的鉴定类型
        /// </summary>
        /// <param name="topicList"></param>
        /// <param name="identify"></param>
        void IdentifyTopic(string topicList, int identify);
        /// <summary>
        /// 获取指定用户ID列表的邮件信息
        /// </summary>
        /// <param name="uids">用户id列表</param>
        /// <returns></returns>
        DataTable GetMailTable(string uids);
        /// <summary>
        /// 获取用户名列表指定的Email列表
        /// </summary>
        /// <param name="userNameList">用户名列表</param>
        /// <returns></returns>
        DataTable MailListTable(string userNameList);
        /// <summary>
        /// 获取指定条件的主题列表
        /// </summary>
        /// <param name="postName">分表名称</param>
        /// <param name="forumId">版块Id</param>
        /// <param name="posterList">作者列表</param>
        /// <param name="keyList">关键字列表</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="currentPage">当前页号</param>
        /// <returns></returns>
        DataTable GetTopicListByCondition(string postName, int forumId, string posterList, string keyList, string startDate, string endDate, int pageSize, int currentPage);
        /// <summary>
        /// 获取聚合首页热列表
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页号</param>
        /// <returns></returns>
        DataTable GetHotTopicsList(int pageSize, int pageIndex, int fid, string showType, int timeBetween);
        /// <summary>
        /// 获取指定条件的主题数
        /// </summary>
        /// <param name="postName">分表名称</param>
        /// <param name="forumId">版块Id</param>
        /// <param name="posterList">作者列表</param>
        /// <param name="keyList">关键字列表</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        int GetTopicListCountByCondition(string postName, int forumId, string posterList, string keyList, string startDate, string endDate);
        /// <summary>
        /// 获取聚合首页热帖数
        /// </summary>
        /// <returns></returns>
        int GetHotTopicsCount(int fid, int timeBetween);
        /// <summary>
        /// 获取指定空间配置rewriteName的UserId信息
        /// </summary>
        /// <param name="rewriteName"></param>
        /// <returns></returns>
        int GetUserIdByRewriteName(string rewriteName);
        /// <summary>
        /// 设置指定Id的帖子是否隐藏
        /// </summary>
        /// <param name="tableId">分表Id</param>
        /// <param name="postListId">帖子Id列表</param>
        /// <param name="invisible">屏蔽还是解除屏蔽</param>
        void SetPostsBanned(string tableId, string postListId, int invisible);
        /// <summary>
        /// 获取指定用户名的用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        IDataReader GetUserInfoByName(string userName);
        /// <summary>
        /// 获取置顶主题列表的条件查询语句
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="fid"></param>
        /// <param name="parentIdList"></param>
        /// <returns></returns>
        string ResetTopTopicListSql(int layer, string fid, string parentIdList);
        /// <summary>
        /// 设置当前版块主题列表的搜索条件
        /// </summary>
        /// <param name="sqlId"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        string ShowForumCondition(int sqlId, int cond);
        /// <summary>
        /// 获取指定条件和分页下的用户列表信息
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable UserList(int pageSize, int currentPage, string condition);
        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="credits">积分</param>
        /// <param name="startuid">更新的用户uid起始值</param>
        int UpdateUserCredits(string credits, int startuid);
        /// <summary>
        /// 删除指定条件的访问日志
        /// </summary>
        /// <param name="deleteMod">删除方式</param>
        /// <param name="visitId">管理日志Id</param>
        /// <param name="deleteNum">删除条数</param>
        /// <param name="deleteFrom">删除从何时起</param>
        /// <returns></returns>
        string DelVisitLogCondition(string deleteMod, string visitId, string deleteNum, string deleteFrom);
        /// <summary>
        /// 获取指定条件的附件信息
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="postName">分表名称</param>
        /// <returns></returns>
        DataTable GetAttachDataTable(string condition, string postName);
        /// <summary>
        /// 需要审核的帖子数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        bool AuditTopicCount(string condition);
        /// <summary>
        /// 获取审核帖子列表的查询语句
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string AuditTopicBindStr(string condition);
        /// <summary>
        /// 需要审核的帖子列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable AuditTopicBind(string condition);
        /// <summary>
        /// 搜索未审核用户              
        /// </summary>
        /// <param name="searchUser">用户名</param>
        /// <param name="regBefore">注册时间</param>
        /// <param name="regIp">注册IP</param>
        /// <returns></returns>
        DataTable AuditNewUserClear(string searchUser, string regBefore, string regIp);
        /// <summary>
        /// 获取删除勋章日志条件
        /// </summary>
        /// <param name="deleteMode">删除方式</param>
        /// <param name="id">Id</param>
        /// <param name="deleteNum">删除条数</param>
        /// <param name="deleteFrom">删除到</param>
        /// <returns></returns>
        string DelMedalLogCondition(string deleteMode, string id, string deleteNum, string deleteFrom);
        /// <summary>
        /// 获取删除管理日志条件
        /// </summary>
        /// <param name="deleteMode">删除方式</param>
        /// <param name="id">Id</param>
        /// <param name="deleteNum">删除条数</param>
        /// <param name="deleteFrom">删除到</param>
        /// <returns></returns>
        string DelModeratorManageCondition(string deleteMode, string id, string deleteNum, string deleteFrom);
        /// <summary>
        /// 按条件获取帖子列表
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable PostGridBind(string postTableName, string condition);
        /// <summary>
        /// 获取指定条件的主题数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="getType"></param>
        /// <param name="getNewTopic"></param>
        /// <returns></returns>
        string GetTopicCountCondition(out string type, string getType, int getNewTopic);
        /// <summary>
        /// 更新分表存储过程
        /// </summary>
        void UpdatePostSP();
        /// <summary>
        /// 获取数据库版本
        /// </summary>
        /// <returns></returns>
        string GetDataBaseVersion();
        /// <summary>
        /// 创建分表存储过程
        /// </summary>
        /// <param name="tableListMaxId"></param>
        void CreateStoreProc(int tableListMaxId);
        /// <summary>
        /// 按类型删除表情
        /// </summary>
        /// <param name="type">类型</param>
        void DeleteSmilyByType(int type);
        /// <summary>
        /// 更新我的主题
        /// </summary>
        void UpdateMyTopic();
        /// <summary>
        /// 更新我的帖子
        /// </summary>
        void UpdateMyPost();
        /// <summary>
        /// 获取全部鉴定图片
        /// </summary>
        /// <returns></returns>
        DataTable GetAllIdentify();
        /// <summary>
        /// 更新鉴定
        /// </summary>
        /// <param name="id">鉴定ID</param>
        /// <param name="name">鉴定名称</param>
        /// <returns></returns>
        bool UpdateIdentifyById(int id, string name);
        /// <summary>
        /// 添加鉴定
        /// </summary>
        /// <param name="name">鉴定名称</param>
        /// <param name="fileName">图片名称</param>
        bool AddIdentify(string name, string fileName);
        /// <summary>
        /// 删除图片鉴定
        /// </summary>
        /// <param name="idlist"></param>
        void DeleteIdentify(string idList);
        /// <summary>
        /// 获取存在的勋章
        /// </summary>
        /// <returns></returns>
        DataTable GetExistMedalList();
        /// <summary>
        /// 获得百度论坛收录协议xml
        /// </summary>
        /// <param name="ttl">TTL数值</param>
        /// <returns></returns>
        IDataReader GetSitemapNewTopics(string p);
        /// <summary>
        /// 获取评分日志条件
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="postidList">帖子Id列表</param>
        /// <returns></returns>
        string GetRateLogCountCondition(int userId, string postIdList);
        /// <summary>
        /// 根据帖子ID删除附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        int DeleteAttachmentByPid(int pid);
        /// <summary>
        /// 获取指定olId的在线用户信息
        /// </summary>
        /// <param name="olId"></param>
        /// <returns></returns>
        IDataReader GetOnlineUser(int olId);
        /// <summary>
        /// 根据Uid获得Olid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetOlidByUid(int uid);
        /// <summary>
        /// 获取指定组的用户列表
        /// </summary>
        /// <param name="groupIdList"></param>
        /// <returns></returns>
        DataTable GetUsers(string groupIdList);
        /// <summary>
        /// 获取指定用户的附件列表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="extList">附件类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>用户附件列表</returns>
        IDataReader GetAttachmentByUid(int uid, string extList, int pageIndex, int pageSize);
        //IDataReader GetUnusedAttachmentListByUid(int uid);
        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>附件数量</returns>
        int GetUserAttachmentCount(int uid);
        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="typeid">附件类型id</param>
        /// <returns>附件数量</returns>
        int GetUserAttachmentCount(int uid, string extList);
        /// <summary>
        /// 获取指定用户id的附件数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>附件数量</returns>
        IDataReader GetAttachmentByUid(int uid, int pageIndex, int pageSize);
        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">版块tid列表</param>
        /// <returns>删除个数</returns>
        void DelMyAttachmentByTid(string tidList);
        /// <summary>
        /// 删除指定帖子的所有附件
        /// </summary>
        /// <param name="pidList">帖子id列表</param>
        /// <returns>删除个数</returns>
        void DelMyAttachmentByPid(string pidList);
        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aidList">附件aid列表</param>
        /// <returns>删除个数</returns>
        void DelMyAttachmentByAid(string aidList);
        /// <summary>
        /// 获取帖子观点
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>Dictionary泛型</returns>
        IDataReader GetPostDebate(int tid);
        /// <summary>
        /// 获取需要审核的主题数量
        /// </summary>
        /// <param name="fidlist">版块ID</param>
        /// <returns></returns>
        int GetUnauditNewTopicCount(string fidList, int filter);
        /// <summary>
        /// 获取需要审核的回复
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="ppp">每页帖子书</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="postTableId">分表</param>
        /// <param name="filter">可见性过滤器</param>
        /// <returns></returns>
        IDataReader GetUnauditNewPost(string fidList, int ppp, int pageId, int tableId, int filter);
        /// <summary>
        /// 获取需要审核的回复数
        /// </summary>
        // <param name="fidList">版块ID</param>
        /// <param name="postTableId">分表ID</param>
        /// <param name="filter">可见性过滤器</param>
        /// <returns></returns>
        int GetUnauditNewPostCount(string fidList, int tableId, int filter);
        /// <summary>
        /// 获得最后回复的帖子列表，支持分页
        /// </summary>
        /// <param name="postParmsInfo">参数对象</param>
        /// <returns></returns>
        DataTable GetPagedLastPostList(PostpramsInfo postParmsInfo, string postTableName);
        /// <summary>
        /// 按条件获取公告
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetAnnouncementsByCondition(string condition);
        /// <summary>
        /// 得到通知数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="state">通知状态(0为已读，1为未读)</param>
        /// <returns></returns>
        int GetNoticeCount(int userId, int state);
        /// <summary>
        /// 检查rewritename是否存在或非法
        /// </summary>
        /// <param name="rewriteName"></param>
        /// <returns>如果存在或者非法的Rewritename则返回true,否则为false</returns>
        bool CheckForumRewriteNameExists(string rewriteName);
        /// <summary>
        /// 设置主题的下沉和提升
        /// </summary>
        /// <param name="tidList"></param>
        /// <param name="lastpostid"></param>
        void SetTopicsBump(string tidList, int type);
        /// <summary>
        /// 获取聚合页面主题列表
        /// </summary>
        /// <param name="showType"></param>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        DataTable GetWebSiteAggForumTopicList(string showType, int topNumber);
        /// <summary>
        /// 返回辩论主题的帖子一方的帖子数
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="debateOpinion">帖子观点</param>
        /// <returns>帖子数</returns>
        int GetDebatesPostCount(int tid, int debateOpinion);
        /// <summary>
        /// 删除辩论帖子信息
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="opiniontext">正反方字段，positivediggs：正文 negativediggs：反方</param>
        /// <param name="pid">帖子Id</param>
        void DeleteDebatePost(int tid, string opinion, int pid);
        /// <summary>
        /// 通过主题ID得到主帖内容,此方法可继续扩展
        /// </summary>
        /// <param name="tid"></param>
        /// <returns>ShowtopicPagePostInfo</returns>
        IDataReader GetSinglePost(int tid, string postTableId);

        string Global_UserGrid_GetCondition(string getString);
        int Global_UserGrid_RecordCount(string condition);
        string Global_UserGrid_SearchCondition(bool isLike, bool isPostDateTime, string userName, string nickName, string userGroup, string email, string creditsStart, string creditsEnd, string lastIp, string posts, string digestPosts, string uid, string joindateStart, string joindateEnd);
        DataTable Global_UserGrid(string searchCondition);

        DataTable GetLastPostNotInPidList(string postIdList, int topicId, int postId);
        int GetPostId();

        /// <summary>
        /// 创建邀请码信息
        /// </summary>
        /// <param name="inviteCode">新的邀请码信息</param>
        /// <returns></returns>
        int CreateInviteCode(InviteCodeInfo inviteCode);

        /// <summary>
        /// 检查邀请码code是否已存在
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        bool IsInviteCodeExist(string code);

        /// <summary>
        /// 通过id获取邀请码信息
        /// </summary>
        /// <param name="inviteId">邀请码id</param>
        /// <returns></returns>
        IDataReader GetInviteCodeById(int inviteId);

        /// <summary>
        /// 通过用户id获取邀请码信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        IDataReader GetInviteCodeByUid(int userId);

        /// <summary>
        /// 通过code获取邀请码信息
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        IDataReader GetInviteCodeByCode(string code);

        /// <summary>
        /// 删除邀请码信息
        /// </summary>
        /// <param name="inviteId">邀请码id</param>
        void DeleteInviteCode(int inviteId);

        /// <summary>
        /// 更新邀请码信息的成功使用次数（+1）
        /// </summary>
        /// <param name="inviteId">邀请码id</param>
        void UpdateInviteCodeSuccessCount(int inviteId);

        /// <summary>
        /// 获取用户邀请码信息列表
        /// </summary>
        /// <param name="creatorId">用户id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        IDataReader GetUserInviteCodeList(int creatorId, int pageIndex);

        /// <summary>
        /// 获取用户邀请码数量
        /// </summary>
        /// <param name="creatorId">用户id</param>
        /// <returns></returns>
        int GetUserInviteCodeCount(int creatorId);

        /// <summary>
        /// 清理数据库中过期或者失效的邀请码
        /// </summary>
        /// <returns></returns>
        int ClearExpireInviteCode();

        /// <summary>
        /// 获取用户当日获取的邀请码数量
        /// </summary>
        /// <param name="creatorId">用户id</param>
        /// <returns></returns>
        int GetTodayUserCreatedInviteCode(int creatorId);
        /// <summary>
        /// 重置整个论坛所有版块的帖子数（topics and posts）
        /// </summary>
        void ResetForumsPosts();
        /// <summary>
        /// 更新所有版块的最后发帖人等信息
        /// </summary>
        void ResetLastPostInfo();
        /// <summary>
        /// 根据分表名更新主题的最后回复等信息
        /// </summary>
        /// <param name="postTableName">分表ID</param>
        void ResetLastRepliesInfoOfTopics(int postTableID);
        /// <summary>
        /// 更新我的帖子
        /// </summary>
        /// <param name="lasttableid">分表ID</param>
        void UpdateMyPost(int lasttableid);
        /// <summary>
        /// 更新所有版块的主题数
        /// </summary>
        void ResetForumsTopics();

        /// <summary>
        /// 更新所有版块的今日发帖数
        /// </summary>
        void ResetTodayPosts();

        int CreateCreditOrder(CreditOrderInfo creditOrderInfo);

        IDataReader GetCreditOrderList(int pageIndex, int status, int orderId, string tradeNo, string buyer, string submitStartTime, string submitLastTime, string confirmStartTime, string confirmLastTime);

        int GetCreditOrderCount(int status, int orderId, string tradeNo, string buyer, string submitStartTime, string submitLastTime, string confirmStartTime, string confirmLastTime);

        IDataReader GetCreditOrderByOrderCode(string orderCode);

        int UpdateCreditOrderInfo(int orderId, string tradeNo, int orderStatus, string confirmedTime);

        /// <summary>
        /// 获取用户单位时间内的发帖数
        /// </summary>
        /// <param name="topNumber">Top条数</param>
        /// <param name="dateType">时间类型</param>
        /// <param name="dateNum">时间数</param>
        /// <param name="postTableName">当前帖子分表名</param>
        /// <returns></returns>
        IDataReader GetUserPostCountList(int topNumber, DateType dateType, int dateNum, string postTableName);
        /// <summary>
        /// 建立更新用户积分存储过程的方法
        /// </summary>
        /// <param name="creditExpression">总积分计算公式</param>
        /// <param name="testCreditExpression">是否需要测试总积分计算公式是否正确</param>
        /// <returns></returns>
        bool CreateUpdateUserCreditsProcedure(string creditExpression, bool testCreditExpression);
        /// <summary>
        /// 按浏览数和回帖数排序读取版块列表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="startNumber"></param>
        /// <param name="condition"></param>
        /// <param name="orderFields"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        IDataReader GetTopicsByViewsOrReplies(int fid, int pageSize, int pageIndex, int startNumber, string condition, string orderFields, int sortType);

        IDataReader GetPostRateLogList(int pid, int pageIndex, int pageSize);

        int GetPostRateLogCount(int pid);

        /// <summary>
        /// 获取指定版块id下的最后发帖主题id
        /// </summary>
        /// <param name="fidList">版块id列表</param>
        /// <returns></returns>
        int GetForumsLastPostTid(string fidList);



        IDataReader GetUserListByEmail(string email);
        /// <summary>
        /// 通过email获取用户列表
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        DataTable GetUserInfoByEmail(string email);

        /// <summary>
        /// 更新用户收藏条目的查看时间
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        int UpdateUserFavoriteViewTime(int uid, int tid);
        /// <summary>
        /// 论坛每日信息统计
        /// </summary>
        /// <param name="trendType">统计项名称</param>
        void UpdateTrendStat(TrendType trendType);

        /// <summary>
        /// 更新指定版块或分类的displayorder信息
        /// </summary>
        /// <param name="displayorder">要更新的displayorder信息</param>
        /// <param name="fid">版块id</param>
        void UpdateDisplayorderInForumByFid(int displayorder, int fid);

        /// <summary>
        /// 获取指定主题下小于pid的有效帖子数
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        int GetPostsCountBeforePid(int pid, int tid);

        /// <summary>
        /// 批理设置版块模板信息
        /// </summary>
        /// <param name="templateID">新的模板id</param>
        /// <param name="fidlist">要更新的版块id列表</param>
        /// <returns></returns>
        int UpdateForumTemplateID(int templateID, string fidlist);        
        
    }
}
