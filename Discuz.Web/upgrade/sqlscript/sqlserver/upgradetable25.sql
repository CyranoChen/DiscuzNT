--表dnt_forumfields中添加两个字段，seokeywords和seodescription，类型：ntext
if not exists(select * from syscolumns where id=object_id('dnt_forumfields') and name='seokeywords')
	ALTER TABLE [dnt_forumfields] ADD [seokeywords] [nvarchar] (500) NULL
GO

if not exists(select * from syscolumns where id=object_id('dnt_forumfields') and name='seodescription')
	ALTER TABLE [dnt_forumfields] ADD [seodescription] [nvarchar] (500) NULL
GO

if not exists(select * from syscolumns where id=object_id('dnt_forumfields') and name='rewritename')
	ALTER TABLE [dnt_forumfields] ADD [rewritename] [nvarchar] (20)  NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='allowhtmltitle')
	ALTER TABLE [dnt_usergroups] ADD [allowhtmltitle] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowhtmltitle] DEFAULT (0)
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='ignoreseccode')
	ALTER TABLE [dnt_usergroups] ADD [ignoreseccode] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_ignoreseccode] DEFAULT (0)
GO

if not exists(select * from syscolumns where id=object_id('dnt_userfields') and name='ignorepm')
	ALTER TABLE [dnt_userfields] ADD [ignorepm] [nvarchar] (1000) NOT NULL CONSTRAINT [DF_dnt_userfields_ignorepm] DEFAULT('')
GO

if not exists(select * from syscolumns where id=object_id('dnt_online') and name='newpms')
	ALTER TABLE [dnt_online] ADD [newpms] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_newpms] DEFAULT(0)
GO

if not exists(select * from syscolumns where id=object_id('dnt_online') and name='newnotices')
	ALTER TABLE [dnt_online] ADD [newnotices] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_newnotices] DEFAULT(0)
GO

if not exists(select * from syscolumns where id=object_id('dnt_topics') and name='attention')
	ALTER TABLE [dnt_topics] ADD [attention] [int] NOT NULL CONSTRAINT [DF_dnt_topics_attention] DEFAULT(0)
GO

if not exists(select * from syscolumns where id=object_id('dnt_attachments') and name='attachprice')
	ALTER TABLE [dnt_attachments] ADD [attachprice] [int] NOT NULL CONSTRAINT [DF_dnt_attachments_attachprice] DEFAULT(0)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dnt_attachpaymentlog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_attachpaymentlog]
GO

CREATE TABLE [dnt_attachpaymentlog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL ,
	[username] [nchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[aid] [int] NOT NULL ,
	[authorid] [int] NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[amount] [int] NOT NULL ,
	[netamount] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_attachpaymentlog] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnt_attachpaymentlog] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dnt_attachpaymentlog] ADD 
	CONSTRAINT [DF_dnt_attachpaymentlog_postdatetime] DEFAULT (getdate()) FOR [postdatetime]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_navs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_navs]
GO

CREATE TABLE [dnt_navs]
(
	[id] [int] IDENTITY(1,1) not null CONSTRAINT PK_id primary key(id),
	[parentid] [int] not null CONSTRAINT [DF_dnt_navs_parentid] DEFAULT(0),
	[name] [char](50) not null,
	[title] [char](255) not null,
	[url] [char](255) not null,
	[target] [tinyint] not null CONSTRAINT [DF_dnt_navs_target] DEFAULT(0),
	[type] [tinyint] not null CONSTRAINT [DF_dnt_navs_type] DEFAULT(0),
	[available] [tinyint] not null CONSTRAINT [DF_dnt_navs_available] DEFAULT(0),
	[displayorder] [int] not null,
	[highlight] [tinyint] not null CONSTRAINT [DF_dnt_navs_highlight] DEFAULT(0),
	[level] [tinyint] not null CONSTRAINT [DF_dnt_navs_level] DEFAULT(0) 
)
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_banned]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_banned]
GO

CREATE TABLE [dnt_banned](

	[id] [smallint]  NOT NULL,
	[ip1] [smallint]  NOT NULL,
	[ip2] [smallint]  NOT NULL,
	[ip3] [smallint]  NOT NULL,
	[ip4] [smallint]  NOT NULL,
	[admin] [nvarchar] (50) NOT NULL,
	[dataline] [datetime]  NOT NULL,
	[expiration] [datetime]  NOT NULL,			
	CONSTRAINT [PK_dnt_banned] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY]
)
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_notices]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_notices]
GO

CREATE TABLE [dnt_notices]
(
[nid] [int] NOT NULL IDENTITY(1, 1),
[uid] [int] NOT NULL,
[type] [smallint] NOT NULL,
[new] [tinyint] NOT NULL,
[posterid] [int] NOT NULL,
[poster] [nchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL,
[note] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
[postdatetime] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE  INDEX [uid] ON [dnt_notices]([uid] DESC ) ON [PRIMARY]
GO

-- 约束和索引
ALTER TABLE [dnt_notices] ADD CONSTRAINT [PK_dnt_notices_nid] PRIMARY KEY CLUSTERED  ([nid]) ON [PRIMARY]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_orders]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_orders]
GO

CREATE TABLE [dnt_orders]
(
[orderid] [nchar] (10) COLLATE Chinese_PRC_CI_AS NOT NULL,
[status] [nchar] (10) COLLATE Chinese_PRC_CI_AS NULL,
[buyer] [nchar] (10) COLLATE Chinese_PRC_CI_AS NULL,
[admin] [nchar] (10) COLLATE Chinese_PRC_CI_AS NULL,
[uid] [int] NULL,
[amount] [int] NULL,
[price] [float] NULL,
[submitdate] [int] NULL,
[confirmdate] [int] NULL
) ON [PRIMARY]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_tradeoptionvars]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_tradeoptionvars]
GO

CREATE TABLE [dnt_tradeoptionvars]
(
[typeid] [smallint] NOT NULL,
[pid] [int] NULL,
[optionid] [smallint] NULL,
[value] [ntext] COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_trendstat') IS NULL
BEGIN
	CREATE TABLE [dnt_trendstat](
		[daytime]	[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_daytime]	DEFAULT (0),
		[login]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_login]		DEFAULT (0),
		[register]	[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_register]	DEFAULT (0),
		[topic]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_topic]		DEFAULT (0),
		[post]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_post]		DEFAULT (0),
		[poll]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_poll]		DEFAULT (0),
		[debate]	[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_debate]		DEFAULT (0),
		[bonus]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_bonus]		DEFAULT (0),
	 CONSTRAINT [PK_dnt_trendstat] PRIMARY KEY CLUSTERED 
	(
		[daytime]
	) ON [PRIMARY]
	)ON [PRIMARY]
END
GO

UPDATE [dnt_usergroups] SET [allowhtml]=1 WHERE [groupid]=1
GO

-- 修改字段--

ALTER TABLE [dnt_words] ALTER COLUMN [id] [int]
GO

IF EXISTS (select * from sysobjects where name='DF_dnt_forums_parentid')
ALTER TABLE [dnt_forums] DROP CONSTRAINT [DF_dnt_forums_parentid]
GO

ALTER TABLE [dnt_forums] ALTER COLUMN [parentid] [int] NOT NULL
GO

ALTER TABLE [dnt_forums] ADD CONSTRAINT [DF_dnt_forums_parentid]  DEFAULT (0) FOR [parentid]
GO

ALTER TABLE [dnt_forumfields] ALTER COLUMN [fid] [int] NOT NULL
GO

ALTER TABLE [dnt_usergroups] ALTER COLUMN [creditslower] [int] NOT NULL
GO

ALTER TABLE [dnt_polloptions] ALTER COLUMN [polloption] [nvarchar] (80) NOT NULL
GO

ALTER TABLE [dnt_words] ADD CONSTRAINT [PK_dnt_words] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO

UPDATE [dnt_templates] SET [createdate]='2008-12-1',[ver]=2.6,[fordntver]=2.6
GO

-- 建立索引--
CREATE INDEX [msgtoid] ON [dnt_pms] ([msgtoid])	ON [PRIMARY]
GO

CREATE INDEX [getsearchid] ON [dnt_searchcaches] ([searchstring], [groupid]) ON [PRIMARY]
GO

 CREATE  INDEX [displayorder] ON [dnt_topics]([displayorder]) ON [PRIMARY]
GO

 CREATE  INDEX [fid] ON [dnt_topics]([fid]) ON [PRIMARY]
GO

 CREATE  INDEX [list_date] ON [dnt_topics]([fid], [displayorder], [postdatetime], [lastpostid] DESC ) ON [PRIMARY]
GO

 CREATE  INDEX [list_tid] ON [dnt_topics]([fid], [displayorder], [tid]) ON [PRIMARY]
GO

 CREATE  INDEX [list_replies] ON [dnt_topics]([fid], [displayorder], [postdatetime], [replies]) ON [PRIMARY]
GO

 CREATE  INDEX [list_views] ON [dnt_topics]([fid], [displayorder], [postdatetime], [views]) ON [PRIMARY]
GO

 CREATE  INDEX [displayorder_fid] ON [dnt_topics]([displayorder], [fid]) ON [PRIMARY]
GO

 CREATE  INDEX [fid_displayorder] ON [dnt_topics]([fid], [displayorder]) ON [PRIMARY]
GO

 CREATE  INDEX [tid] ON [dnt_topics]([tid] DESC ) ON [PRIMARY]
GO

 CREATE  INDEX [parentid] ON [dnt_posts1]([parentid]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [showtopic] ON [dnt_posts1]([tid], [invisible], [pid]) ON [PRIMARY]
GO

 CREATE  INDEX [treelist] ON [dnt_posts1]([tid], [invisible], [parentid]) ON [PRIMARY]
GO

 CREATE  INDEX [tid] ON [dnt_attachments]([tid]) ON [PRIMARY]
GO

 CREATE  INDEX [pid] ON [dnt_attachments]([pid]) ON [PRIMARY]
GO

 CREATE  INDEX [uid] ON [dnt_attachments]([uid]) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [email] ON [dnt_users] 
(
	[email] ASC
)
GO

CREATE NONCLUSTERED INDEX [regip] ON [dnt_users] 
(
	[regip] ASC
)


-- 初始化表数据--
INSERT INTO [dnt_navs] VALUES(0,'短消息','短消息','usercpinbox.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(0,'用户中心','用户中心','usercp.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(0,'系统设置','系统设置','admin/index.aspx',1,0,1,0,0,3)
GO

INSERT INTO [dnt_navs] VALUES(0,'我的','我的','#',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(0,'标签','标签','tags.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(0,'统计','统计','stats.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(0,'会员','会员','showuser.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(0,'搜索','搜索','search.aspx',1,0,0,0,0,2)
GO

INSERT INTO [dnt_navs] VALUES(0,'帮助','帮助','help.aspx',1,0,0,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'基本状况','基本状况','usercpinbox.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'流量统计','流量统计','stats.aspx?type=views',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'客户软件','客户软件','stats.aspx?type=client',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'发帖量记录','发帖量记录','stats.aspx?type=posts',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'版块排行','版块排行','stats.aspx?type=forumsrank',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'主题排行','主题排行','stats.aspx?type=topicsrank',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'发帖排行','发帖排行','stats.aspx?type=postsrank',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'积分排行','积分排行','stats.aspx?type=creditsrank',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(6,'在线时间','在线时间','stats.aspx?type=onlinetime',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的主题','我的主题','mytopics.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的回复','我的回复','myposts.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的精华','我的精华','search.aspx?posterid=current&type=digest',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的附件','我的附件','myattachment.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的收藏','我的收藏','usercpsubscribe.aspx',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的空间','我的空间','space/',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的相册','我的相册','showalbumlist.aspx?uid=-1',0,0,1,0,0,0)
GO

INSERT INTO [dnt_navs] VALUES(4,'我的商品','我的商品','usercpmygoods.aspx',0,0,1,0,0,0)
GO

-------------添加字段--------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_attachments') AND name='width')
BEGIN
ALTER TABLE [dnt_attachments] ADD [width] INT NOT NULL DEFAULT (0)
END
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_attachments') AND name='height')
BEGIN
ALTER TABLE [dnt_attachments] ADD [height] INT NOT NULL DEFAULT (0)
END
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_users') AND name='salt')
BEGIN
ALTER TABLE [dnt_users] ADD [salt] NCHAR(6) NOT NULL CONSTRAINT [DF_dnt_users_salt] DEFAULT ('')
END
GO

-------------修改初始值--------------------------------------------
UPDATE [dnt_bbcodes] 
SET replacement='<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0" width="550" height="400"><param name="allowScriptAccess" value="sameDomain"/><param name="wmode" value="opaque"/><param name="movie" value="{1}"/><param name="quality" value="high"/><param name="bgcolor" value="#ffffff"/><embed src="{1}" quality="high" bgcolor="#ffffff" width="550" height="400" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" wmode="transparent"/></object>'
WHERE id=1
GO

DELETE FROM [dnt_navs] 
WHERE [name]='短消息' OR [name]='用户中心' OR [name]='系统设置' OR [name]='我的' OR [name]='统计'
GO

-------------重建表--------------------------------------------
if exists (select * from sysobjects where id = object_id(N'[dnt_banned]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_banned]
GO

CREATE TABLE [dnt_banned](

	[id] [smallint]  IDENTITY(1,1) not null CONSTRAINT PK_dnt_bannedid primary key(id),
	[ip1] [smallint]  NOT NULL,
	[ip2] [smallint]  NOT NULL,
	[ip3] [smallint]  NOT NULL,
	[ip4] [smallint]  NOT NULL,
	[admin] [nvarchar] (50) NOT NULL,
	[dateline] [datetime]  NOT NULL,
	[expiration] [datetime]  NOT NULL,			
)
GO

-------------删除表--------------------------------------------
IF OBJECT_ID('catchsoftstatics') IS NOT NULL
DROP TABLE catchsoftstatics
GO

-------------创建新表及索引等--------------------------------------------

if exists (select * from sysobjects where id = object_id(N'[dnt_invitation]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_invitation]
GO

CREATE TABLE [dnt_invitation](
	[inviteid] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_dnt_invitation_inviteid] PRIMARY KEY([inviteid]),
	[invitecode] [nchar](7) NOT NULL,
	[creatorid] [int] NOT NULL,
	[creator] [nchar](20) NOT NULL,
	[successcount] [int] NOT NULL,
	[createdtime] [smalldatetime] NOT NULL,
	[expiretime] [smalldatetime] NOT NULL,
	[maxcount] [int] NOT NULL,
	[invitetype] [int] NOT NULL,
	[isdeleted] [int] NOT NULL
)
GO

ALTER TABLE [dnt_invitation] ADD  CONSTRAINT [DF_dnt_invitation_usecount]  DEFAULT ((0)) FOR [successcount]
GO

ALTER TABLE [dnt_invitation] ADD  CONSTRAINT [DF_dnt_invitation_isdelete]  DEFAULT ((0)) FOR [isdeleted]
GO

CREATE NONCLUSTERED INDEX [code] ON [dnt_invitation]
(
	[invitecode] ASC
)
GO

CREATE NONCLUSTERED INDEX [creatorid] ON [dnt_invitation]
(
	[creatorid] ASC
)
GO

CREATE NONCLUSTERED INDEX [invitetype] ON [dnt_invitation]
(
	[invitetype] ASC
)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dnt_orders]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_orders]
GO

CREATE TABLE [dnt_orders](
	[orderid] [int] IDENTITY(10000,1) NOT NULL CONSTRAINT [PK_dnt_orders_orderid] PRIMARY KEY([orderid]),
	[ordercode] [char](32) NOT NULL,
	[uid] [int] NOT NULL,
	[buyer] [char](20) NOT NULL,
	[paytype] [tinyint] NOT NULL,
	[tradeno] [char](32) NULL,
	[price] [decimal](18, 2) NOT NULL,
	[orderstatus] [tinyint] NOT NULL,
	[createdtime] [smalldatetime] NOT NULL,
	[confirmedtime] [smalldatetime] NULL,
	[credit] [tinyint] NOT NULL,
	[amount] [int] NOT NULL
)
GO

ALTER TABLE [dnt_orders] ADD  CONSTRAINT [DF_dnt_orders_createdtime]  DEFAULT (getdate()) FOR [createdtime]
GO

CREATE NONCLUSTERED INDEX [dnt_orders_ordercode] ON [dnt_orders] 
(
	[ordercode] ASC
)
GO

CREATE INDEX [IX_dnt_topictagcaches_tid] ON dnt_topictagcaches(tid) INCLUDE (linktid, linktitle);
GO

CREATE INDEX [dnt_polls_tid] ON dnt_polls(tid)
GO

CREATE INDEX [IX_dnt_attachpaymentlog_uid] ON dnt_attachpaymentlog([uid],[aid])
GO

CREATE INDEX [IX_dnt_forums_status] ON dnt_forums([status],[layer],[parentid])
GO

CREATE INDEX [dnt_forumfields_fid] ON dnt_forumfields(fid)
GO

CREATE INDEX [IX_dnt_smilies_type] ON dnt_smilies ([type], [displayorder],[id]) include(code,url)
GO

CREATE INDEX [dnt_bbcodes_available] ON dnt_bbcodes(available)
GO

CREATE INDEX [dnt_moderatormanagelog_tid] ON dnt_moderatormanagelog(tid)
GO

--[dnt_forums]表中添加字段[modnewtopics]
IF NOT EXISTS(select * from syscolumns where id=object_id('dnt_forums') and name='modnewtopics')
	ALTER TABLE [dnt_forums] ADD [modnewtopics] [smallint] NOT NULL CONSTRAINT [DF_dnt_forums_modnewtopics] DEFAULT (0)
GO

--[dnt_usergroups]表中添加字段[modnewtopics]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='modnewtopics')
	ALTER TABLE [dnt_usergroups] ADD [modnewtopics] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_modnewtopics] DEFAULT (0)
GO

--[dnt_usergroups]表中添加字段[modnewposts]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='modnewposts')
	ALTER TABLE [dnt_usergroups] ADD [modnewposts] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_modnewposts] DEFAULT (0)
GO

--[dnt_templates]表中添加字段[templateurl]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[dnt_templates]') AND NAME='templateurl')
	ALTER TABLE [dnt_templates] ADD [templateurl] [nvarchar] (100) NOT NULL CONSTRAINT [DF_dnt_templates_templateurl] DEFAULT('')
GO

--[dnt_polls]表中添加字段[allowview]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[dnt_polls]') AND NAME='allowview')
	ALTER TABLE [dnt_polls] ADD [allowview] [tinyint] NOT NULL CONSTRAINT [DF_dnt_polls_allowview] DEFAULT(0)
GO

--[dnt_notices]表中添加字段[fromid]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[dnt_notices]') AND NAME='fromid')
	ALTER TABLE [dnt_notices] ADD [fromid] [int] NOT NULL CONSTRAINT [DF_dnt_notices_fromid] DEFAULT(0)
GO

--[dnt_favorites]表中添加字段[favtime]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_favorites') AND NAME='favtime')
	ALTER TABLE [dnt_favorites] ADD [favtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_favorites_favtime] DEFAULT(getdate())
GO

--[dnt_favorites]表中添加字段[viewtime]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_favorites') AND NAME='viewtime')
	ALTER TABLE [dnt_favorites] ADD [viewtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_favorites_viewtime] DEFAULT(getdate())
GO

--[dnt_attachments]表中添加字段[isimage]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_attachments') AND NAME='isimage')
	ALTER TABLE [dnt_attachments] ADD [isimage] [tinyint] NOT NULL CONSTRAINT [DF_dnt_attachments_isimage] DEFAULT (0)