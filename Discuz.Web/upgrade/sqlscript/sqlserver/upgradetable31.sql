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
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_templates') AND NAME='templateurl')
	ALTER TABLE [dnt_templates] ADD [templateurl] [nvarchar] (100) NOT NULL CONSTRAINT [DF_dnt_templates_templateurl] DEFAULT('')
GO

--[dnt_polls]表中添加字段[allowview]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_polls') AND NAME='allowview')
	ALTER TABLE [dnt_polls] ADD [allowview] [tinyint] NOT NULL CONSTRAINT [DF_dnt_polls_allowview] DEFAULT(0)
GO

--[dnt_notices]表中添加字段[fromid]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_notices') AND NAME='fromid')
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

ALTER TABLE [dnt_attachments] ALTER COLUMN [attachment] nchar(255)  NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='allowhtmltitle')
	ALTER TABLE [dnt_usergroups] ADD [allowhtmltitle] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowhtmltitle] DEFAULT (0)
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='ignoreseccode')
	ALTER TABLE [dnt_usergroups] ADD [ignoreseccode] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_ignoreseccode] DEFAULT (0)
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