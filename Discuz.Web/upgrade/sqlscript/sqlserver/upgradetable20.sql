--并添加allowpostspecial,allowspecialonly，两字段均为整型
ALTER TABLE [dnt_forums] ADD [allowpostspecial] [int] NOT NULL DEFAULT ('0')
GO

ALTER TABLE [dnt_forums] ADD [allowspecialonly] [int] NOT NULL DEFAULT ('0')
GO

ALTER TABLE [dnt_forums] ADD [allowtag] [int] NOT NULL DEFAULT ('0')
GO

EXEC sp_rename '[dnt_forums].[colcount]',  'colcount2',  'COLUMN'
GO

ALTER TABLE [dnt_forums] DROP CONSTRAINT [DF_dnt_forums_colcount]
GO

ALTER TABLE [dnt_forums] ADD [colcount] [smallint]  NULL
GO

ALTER TABLE [dnt_forums] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_forums_colcount]  DEFAULT (1) FOR [colcount]
GO

UPDATE [dnt_forums] SET [colcount]=[colcount2]
GO

ALTER TABLE [dnt_forums] ALTER COLUMN [colcount] [smallint]  NOT NULL
GO

ALTER TABLE [dnt_forums] DROP COLUMN [colcount2]
GO

ALTER TABLE [dnt_forums] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_forums_colcount] DEFAULT (1) FOR [colcount]
GO

UPDATE  [dnt_forums] SET [allowpostspecial]=0,[allowspecialonly]=0,[allowtag]=0
GO

ALTER TABLE [dnt_usergroups] ADD [allowdebate] [int] NOT NULL DEFAULT (0)
GO

ALTER TABLE [dnt_usergroups] ADD [allowbonus] [int] NOT NULL DEFAULT (0)
GO

ALTER TABLE [dnt_usergroups] ADD [minbonusprice] [smallint] NOT NULL DEFAULT (0)
GO

ALTER TABLE [dnt_usergroups] ADD [maxbonusprice] [smallint] NOT NULL DEFAULT (0)
GO

ALTER TABLE [dnt_usergroups] ADD [allowtrade] [int] NOT NULL DEFAULT (0)
GO

ALTER TABLE [dnt_usergroups] ADD [allowdiggs] [int] NOT NULL DEFAULT (0)
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=100,[allowtrade]=1,[allowdiggs]=1 where groupid=1
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=90,[allowtrade]=1,[allowdiggs]=1 where groupid=2
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=80,[allowtrade]=1,[allowdiggs]=1 where groupid=3
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=0,[allowbonus]=0,[minbonusprice]=0,[maxbonusprice]=0,[allowtrade]=0,[allowdiggs]=1 where groupid=4
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=0,[allowbonus]=0,[minbonusprice]=0,[maxbonusprice]=0,[allowtrade]=0,[allowdiggs]=0 where groupid=5
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=0,[allowbonus]=0,[minbonusprice]=0,[maxbonusprice]=0,[allowtrade]=0,[allowdiggs]=0 where groupid=6
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=0,[allowbonus]=0,[minbonusprice]=0,[maxbonusprice]=0,[allowtrade]=0,[allowdiggs]=0 where groupid=7
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=0,[allowbonus]=0,[minbonusprice]=0,[maxbonusprice]=0,[allowtrade]=0,[allowdiggs]=0 where groupid=8
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=0,[allowbonus]=0,[minbonusprice]=0,[maxbonusprice]=0,[allowtrade]=0,[allowdiggs]=0 where groupid=9
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=0,[allowbonus]=0,[minbonusprice]=0,[maxbonusprice]=0,[allowtrade]=0,[allowdiggs]=1 where groupid=10
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=20,[allowtrade]=0,[allowdiggs]=1 where groupid=11
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=30,[allowtrade]=1,[allowdiggs]=1 where groupid=12
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=50,[allowtrade]=1,[allowdiggs]=1 where groupid=13
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=60,[allowtrade]=1,[allowdiggs]=1 where groupid=14
GO

UPDATE  [dnt_usergroups] SET [allowdebate]=1,[allowbonus]=1,[minbonusprice]=1,[maxbonusprice]=70,[allowtrade]=1,[allowdiggs]=1 where groupid=15
GO

ALTER TABLE [dnt_topictags] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_topictags_tagid] DEFAULT (0) FOR [tagid],
	CONSTRAINT [DF_dnt_topictags_tid] DEFAULT (0) FOR [tid]
GO

ALTER TABLE [dnt_statistics] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_topictags_tagid] DEFAULT (0) FOR [tagid],
	CONSTRAINT [DF_dnt_topictags_tid] DEFAULT (0) FOR [tid]
GO

ALTER TABLE [dnt_statistics] ADD [yesterdayposts] [int] NOT NULL DEFAULT (0)
GO

ALTER TABLE [dnt_statistics] ADD [highestposts] [int] NOT NULL DEFAULT (0)
GO

ALTER TABLE [dnt_statistics] ADD [highestpostsdate] [char] (10) NOT NULL DEFAULT ('')
GO

--同时运行下面语句将转换typeid数据类型为int
EXEC sp_rename '[dnt_topics].[typeid]',  'typeid2',  'COLUMN'
GO

ALTER TABLE [dnt_topics] DROP CONSTRAINT [DF_dnt_topics_typeid]
GO

ALTER TABLE [dnt_topics] ADD [typeid] [int]  NULL
GO

ALTER TABLE [dnt_topics] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_topics_typeid]  DEFAULT (0) FOR [typeid]
GO

UPDATE [dnt_topics] SET [typeid]=[typeid2]
GO

ALTER TABLE [dnt_topics] ALTER COLUMN [typeid] [int]  NOT NULL
GO

ALTER TABLE [dnt_topics] DROP COLUMN [typeid2]
GO

--同时运行下面语句将转换rate数据类型为int
EXEC sp_rename '[dnt_topics].[rate]',  'rate2',  'COLUMN'
GO

ALTER TABLE [dnt_topics] DROP CONSTRAINT [DF_dnt_topics_rate]
GO

ALTER TABLE [dnt_topics] ADD [rate] [int] NULL
GO

ALTER TABLE [dnt_topics] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_topics_rate] DEFAULT (0) FOR [rate]
GO

UPDATE [dnt_topics] SET [rate]=[rate2]
GO

ALTER TABLE [dnt_topics] ALTER COLUMN [rate] [int]  NOT NULL
GO

ALTER TABLE [dnt_topics] DROP COLUMN [rate2]
GO

--同时运行下面语句将原有投票贴字段poll倒入的special字段中并去掉topics表中的poll字段
ALTER TABLE [dnt_topics] ADD [special] [tinyint] NULL
GO

ALTER TABLE [dnt_topics] WITH NOCHECK ADD 
  CONSTRAINT [DF_dnt_topics_special] DEFAULT (0) FOR [special]
GO

UPDATE [dnt_topics] SET [special] = 0
GO

UPDATE [dnt_topics] SET [special] = [poll]
GO

ALTER TABLE [dnt_topics] ALTER COLUMN [special] [tinyint] NOT NULL
GO

ALTER TABLE [dnt_topics] DROP CONSTRAINT [DF_dnt_topics_poll]
GO

ALTER TABLE [dnt_topics] DROP COLUMN [poll]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_online]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_online]
GO

CREATE TABLE [dnt_online] (
	[olid] [int] IDENTITY (1, 1) NOT NULL ,
	[userid] [int] NOT NULL ,
	[ip] [varchar] (15) NOT NULL ,
	[username] [nvarchar] (20) NOT NULL ,
	[nickname] [nvarchar] (20) NOT NULL ,
	[password] [char] (32) NOT NULL ,
	[groupid] [smallint] NOT NULL ,
	[olimg] [varchar] (80) NOT NULL ,
	[adminid] [smallint] NOT NULL ,
	[invisible] [smallint] NOT NULL ,
	[action] [smallint] NOT NULL ,
	[lastactivity] [smallint] NOT NULL ,
	[lastposttime] [datetime] NOT NULL ,
	[lastpostpmtime] [datetime] NOT NULL ,
	[lastsearchtime] [datetime] NOT NULL ,
	[lastupdatetime] [datetime] NOT NULL ,
	[forumid] [int] NOT NULL ,
	[forumname] [nvarchar] (50) NOT NULL ,
	[titleid] [int] NOT NULL ,
	[title] [nvarchar] (80) NOT NULL ,
	[verifycode] [varchar] (10) NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_online] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnt_online] PRIMARY KEY  CLUSTERED 
	(
		[olid]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dnt_online] ADD 
	CONSTRAINT [DF_dnt_online_userid] DEFAULT ((-1)) FOR [userid],
	CONSTRAINT [DF_dnt_online_ip] DEFAULT ('0.0.0.0') FOR [ip],
	CONSTRAINT [DF_dnt_online_username] DEFAULT ('') FOR [username],
	CONSTRAINT [DF_dnt_online_nickname] DEFAULT ('') FOR [nickname],
	CONSTRAINT [DF_dnt_online_password] DEFAULT ('') FOR [password],
	CONSTRAINT [DF_dnt_online_groupid] DEFAULT (0) FOR [groupid],
	CONSTRAINT [DF_dnt_online_olimg] DEFAULT ('') FOR [olimg],
	CONSTRAINT [DF_dnt_online_adminid] DEFAULT (0) FOR [adminid],
	CONSTRAINT [DF_dnt_online_invisible] DEFAULT (0) FOR [invisible],
	CONSTRAINT [DF_dnt_online_action] DEFAULT (0) FOR [action],
	CONSTRAINT [DF_dnt_online_lastactivity] DEFAULT (0) FOR [lastactivity],
	CONSTRAINT [DF_dnt_online_lastposttime] DEFAULT ('1900-1-1 00:00:00') FOR [lastposttime],
	CONSTRAINT [DF_dnt_online_lastpostpmtime] DEFAULT ('1900-1-1 00:00:00') FOR [lastpostpmtime],
	CONSTRAINT [DF_dnt_online_lastsearchtime] DEFAULT ('1900-1-1 00:00:00') FOR [lastsearchtime],
	CONSTRAINT [DF_dnt_online_lastupdatetime] DEFAULT (getdate()) FOR [lastupdatetime],
	CONSTRAINT [DF_dnt_online_forumid] DEFAULT (0) FOR [forumid],
	CONSTRAINT [DF_dnt_online_forumname] DEFAULT ('') FOR [forumname],
	CONSTRAINT [DF_dnt_online_titleid] DEFAULT (0) FOR [titleid],
	CONSTRAINT [DF_dnt_online_title] DEFAULT ('') FOR [title],
	CONSTRAINT [DF_dnt_online_verifycode] DEFAULT ('') FOR [verifycode]
GO

 CREATE  INDEX [forum] ON [dnt_online]([userid], [forumid], [invisible]) ON [PRIMARY]
GO

 CREATE  INDEX [invisible] ON [dnt_online]([userid], [invisible]) ON [PRIMARY]
GO

 CREATE  INDEX [forumid] ON [dnt_online]([forumid]) ON [PRIMARY]
GO

 CREATE  INDEX [password] ON [dnt_online]([userid], [password]) ON [PRIMARY]
GO

 CREATE  INDEX [ip] ON [dnt_online]([userid], [ip]) ON [PRIMARY]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_tags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
ALTER TABLE [dnt_tags] ADD [gcount] [int] NULL DEFAULT ('0')
end
GO

if not exists (select * from sysobjects where id = object_id(N'[dnt_tags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
CREATE TABLE [dnt_tags] (
	[tagid] [int] IDENTITY (1, 1) NOT NULL ,
	[tagname] [nchar] (10) NOT NULL ,
	[userid] [int] NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[orderid] [int] NOT NULL ,
	[color] [char] (6) NOT NULL ,
	[count] [int] NOT NULL ,
	[fcount] [int] NOT NULL ,
	[pcount] [int] NOT NULL ,
	[scount] [int] NOT NULL ,
	[vcount] [int] NOT NULL ,
	[gcount] [int] NOT NULL 
) ON [PRIMARY]
end
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_tags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
ALTER TABLE [dnt_tags] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnt_tags] PRIMARY KEY  CLUSTERED 
	(
		[tagid]
	)  ON [PRIMARY] 
end
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_tags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
ALTER TABLE [dnt_tags] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_tags_userid] DEFAULT (0) FOR [userid],
	CONSTRAINT [DF_dnt_tags_orderid] DEFAULT (0) FOR [orderid],
	CONSTRAINT [DF_dnt_tags_count] DEFAULT (0) FOR [count],
	CONSTRAINT [DF_dnt_tags_fcount] DEFAULT (0) FOR [fcount],
	CONSTRAINT [DF_dnt_tags_pcount] DEFAULT (0) FOR [pcount],
	CONSTRAINT [DF_dnt_tags_scount] DEFAULT (0) FOR [scount],
	CONSTRAINT [DF_dnt_tags_vcount] DEFAULT (0) FOR [vcount],
	CONSTRAINT [DF_dnt_tags_gcount] DEFAULT (0) FOR [gcount]
end
GO

ALTER TABLE [dnt_tags] WITH NOCHECK ADD 
	CONSTRAINT [DF_dnt_tags_userid] DEFAULT (0) FOR [userid]
GO

--将版块表中的allowtrade字段改名为istrade
EXEC sp_rename '[dnt_forums].[allowtrade]',  'istrade',  'COLUMN'
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_topictags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_topictags]
GO

CREATE TABLE [dnt_topictags] (
	[tagid] [int] NOT NULL ,
	[tid] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_topictags] ADD 
	CONSTRAINT [DF_dnt_topictags_tagid] DEFAULT (0) FOR [tagid],
	CONSTRAINT [DF_dnt_topictags_tid] DEFAULT (0) FOR [tid]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_stats]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_stats]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_statvars]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_statvars]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_bonuslog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_bonuslog]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_debatediggs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_debatediggs]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_debates]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_debates]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_myattachments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_myattachments]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_onlinetime]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_onlinetime]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_postdebatefields]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_postdebatefields]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_scheduledevents]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_scheduledevents]
GO

CREATE TABLE [dnt_stats] (
	[type] [char] (10) NOT NULL ,
	[variable] [char] (20) NOT NULL ,
	[count] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE UNIQUE CLUSTERED INDEX [IX_dnt_stats] ON [dnt_stats]([type], [variable]) WITH IGNORE_DUP_KEY ON [PRIMARY]
GO

ALTER TABLE [dnt_stats] ADD 
	CONSTRAINT [DF_dnt_stats_count] DEFAULT (0) FOR [count]
GO

CREATE TABLE [dnt_statvars] (
	[type] [char] (20) NOT NULL ,
	[variable] [char] (20) NOT NULL ,
	[value] [text] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE CLUSTERED INDEX [IX_dnt_statvars] ON [dnt_statvars]([type], [variable]) WITH IGNORE_DUP_KEY ON [PRIMARY]
GO

CREATE TABLE [dnt_bonuslog] (
	[tid] [int] NOT NULL ,
	[authorid] [int] NOT NULL ,
	[answerid] [int] NOT NULL ,
	[answername] [nchar] (20) NOT NULL ,
	[pid] [int] NOT NULL ,
	[dateline] [datetime] NOT NULL ,
	[bonus] [int] NOT NULL ,
	[extid] [tinyint] NOT NULL ,
	[isbest] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_debatediggs] (
	[tid] [int] NOT NULL ,
	[pid] [int] NOT NULL ,
	[digger] [nchar] (20) NOT NULL ,
	[diggerid] [int] NOT NULL ,
	[diggerip] [nchar] (15) NOT NULL ,
	[diggdatetime] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_debates] (
	[tid] [int] NOT NULL ,
	[positiveopinion] [nvarchar] (200) NOT NULL ,
	[negativeopinion] [nvarchar] (200) NOT NULL ,
	[terminaltime] [datetime] NOT NULL ,
	[positivediggs] [int] NOT NULL ,
	[negativediggs] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_debates] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnt_debate] PRIMARY KEY  CLUSTERED 
	(
		[tid]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dnt_debates] ADD 
	CONSTRAINT [DF_dnt_debates_positidiggs] DEFAULT (0) FOR [positivediggs],
	CONSTRAINT [DF_dnt_debates_negatidiggs] DEFAULT (0) FOR [negativediggs]
GO

CREATE TABLE [dnt_myattachments] (
	[aid] [int] NOT NULL ,
	[uid] [int] NOT NULL ,
	[attachment] [nchar] (100) NOT NULL ,
	[description] [nchar] (100) NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[downloads] [int] NOT NULL ,
	[filename] [nchar] (100) NOT NULL ,
	[pid] [int] NOT NULL ,
	[tid] [int] NOT NULL ,
	[extname] [nvarchar] (50) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_onlinetime] (
	[uid] [int] NOT NULL ,
	[thismonth] [smallint] NOT NULL ,
	[total] [int] NOT NULL ,
	[lastupdate] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_postdebatefields] (
	[tid] [int] NOT NULL ,
	[pid] [int] NOT NULL ,
	[opinion] [int] NOT NULL ,
	[diggs] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_postdebatefields] ADD 
	CONSTRAINT [DF_dnt_postdebatefields_tid] DEFAULT (0) FOR [tid],
	CONSTRAINT [DF_dnt_postdebatefields_pid] DEFAULT (0) FOR [pid],
	CONSTRAINT [DF_dnt_postdebatefields_opinion] DEFAULT (0) FOR [opinion],
	CONSTRAINT [DF_dnt_postdebatefields_diggs] DEFAULT (0) FOR [diggs]
GO

CREATE TABLE [dnt_scheduledevents] (
	[scheduleID] [int] IDENTITY (1, 1) NOT NULL ,
	[key] [varchar] (50) NOT NULL ,
	[lastexecuted] [datetime] NOT NULL ,
	[servername] [varchar] (100) NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_templates] ADD [templateid] [smallint] IDENTITY (1, 1) NOT NULL 
GO

ALTER TABLE [dnt_spacethemes] ADD [themeid] [int] IDENTITY (1, 1) NOT NULL
GO

ALTER TABLE [dnt_spacemoduledefs] ADD [moduledefid] [int] IDENTITY (1, 1) NOT NULL 
GO

ALTER TABLE [dnt_albumcategories] ADD [albumcateid] [int] IDENTITY (1, 1) NOT NULL
GO

ALTER TABLE [dnt_forumlinks] ADD [id] [smallint] IDENTITY (1, 1) NOT NULL
GO

ALTER TABLE [dnt_creditslog] ALTER COLUMN [sendcredits] [tinyint] NOT NULL
GO

ALTER TABLE [dnt_creditslog] ALTER COLUMN [receivecredits] [tinyint] NOT NULL
GO

ALTER TABLE [dnt_creditslog] ALTER COLUMN [send] [float] NOT NULL
GO

ALTER TABLE [dnt_creditslog] ALTER COLUMN [receive] [float] NOT NULL
GO

ALTER TABLE [dnt_help] ALTER COLUMN [orderby] [int] DEFAULT(0) NOT NULL
GO

ALTER TABLE [dnt_myattachments] ADD 
	CONSTRAINT [DF_dnt_myattachments_pid] DEFAULT (0) FOR [pid],
	CONSTRAINT [DF_dnt_myattachments_tid] DEFAULT (0) FOR [tid],
	CONSTRAINT [DF_dnt_myattachments_extname] DEFAULT ('') FOR [extname]
GO

ALTER TABLE [dbo].[dnt_help] ADD 
	CONSTRAINT [DF_dnt_help_orderby] DEFAULT (0) FOR [orderby]
GO

if exists (select * from sysobjects where id = object_id(N'[dnt_onlinetime]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_onlinetime]
GO

CREATE TABLE [dnt_onlinetime] (
	[uid] [int] NOT NULL ,
	[thismonth] [smallint] NOT NULL ,
	[total] [int] NOT NULL ,
	[lastupdate] [datetime] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_onlinetime] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnt_onlinetime] PRIMARY KEY  CLUSTERED 
	(
		[uid]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dnt_onlinetime] ADD 
	CONSTRAINT [DF_dnt_onlinetime_thismonth] DEFAULT (0) FOR [thismonth],
	CONSTRAINT [DF_dnt_onlinetime_total] DEFAULT (0) FOR [total],
	CONSTRAINT [DF_dnt_onlinetime_lastupdate] DEFAULT (getdate()) FOR [lastupdate]
GO

ALTER TABLE [dnt_paymentlog] ADD 
	CONSTRAINT [DF_dnt_paymentlog_buydate] DEFAULT (getdate()) FOR [buydate]GO

ALTER TABLE [dbo].[dnt_ratelog] ADD 
	CONSTRAINT [DF_dnt_ratelog_postdatetime] DEFAULT (getdate()) FOR [postdatetime]
GO

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

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='allowhtmltitle')
	ALTER TABLE [dnt_usergroups] ADD [allowhtmltitle] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowhtmltitle] DEFAULT (0)
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='ignoreseccode')
	ALTER TABLE [dnt_usergroups] ADD [ignoreseccode] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_ignoreseccode] DEFAULT (0)
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