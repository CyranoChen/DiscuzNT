IF OBJECT_ID('dnt_admingroups') IS NOT NULL
DROP TABLE [dnt_admingroups]
GO

CREATE TABLE [dnt_admingroups] (
	[admingid] [smallint] NOT NULL ,
	[alloweditpost] [tinyint] NOT NULL ,
	[alloweditpoll] [tinyint] NOT NULL ,
	[allowstickthread] [tinyint] NOT NULL ,
	[allowmodpost] [tinyint] NOT NULL ,
	[allowdelpost] [tinyint] NOT NULL ,
	[allowmassprune] [tinyint] NOT NULL ,
	[allowrefund] [tinyint] NOT NULL ,
	[allowcensorword] [tinyint] NOT NULL ,
	[allowviewip] [tinyint] NOT NULL ,
	[allowbanip] [tinyint] NOT NULL ,
	[allowedituser] [tinyint] NOT NULL ,
	[allowmoduser] [tinyint] NOT NULL ,
	[allowbanuser] [tinyint] NOT NULL ,
	[allowpostannounce] [tinyint] NOT NULL ,
	[allowviewlog] [tinyint] NOT NULL ,
	[disablepostctrl] [tinyint] NOT NULL ,
	[allowviewrealname] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_adminvisitlog') IS NOT NULL
DROP TABLE [dnt_adminvisitlog]
GO

CREATE TABLE [dnt_adminvisitlog] (
	[visitid] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NULL ,
	[username] [nvarchar] (20) NOT NULL ,
	[groupid] [int] NULL ,
	[grouptitle] [nvarchar] (50) NOT NULL ,
	[ip] [varchar] (15) NULL ,
	[postdatetime] [datetime] NULL CONSTRAINT [DF_dnt_adminvisitlog_postdatetime] DEFAULT (getdate()),
	[actions] [nvarchar] (100) NOT NULL ,
	[others] [nvarchar] (200) NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_advertisements') IS NOT NULL
DROP TABLE [dnt_advertisements]
GO

CREATE TABLE [dnt_advertisements] (
	[advid] [int] IDENTITY (1, 1) NOT NULL ,
	[available] [int] NOT NULL ,
	[type] [nvarchar] (50) NOT NULL ,
	[displayorder] [int] NOT NULL ,
	[title] [nvarchar] (50)  NOT NULL ,
	[targets] [nvarchar] (255) NOT NULL ,
	[starttime] [datetime] NOT NULL ,
	[endtime] [datetime] NOT NULL ,
	[code] [ntext] NOT NULL CONSTRAINT [DF_dnt_advertisements_code] DEFAULT (''),
	[parameters] [ntext] NOT NULL CONSTRAINT [DF_dnt_advertisements_parameters] DEFAULT ('')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_announcements') IS NOT NULL
DROP TABLE [dnt_announcements]
GO

CREATE TABLE [dnt_announcements] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[poster] [nvarchar] (20) NOT NULL ,
	[posterid] [int] NOT NULL ,
	[title] [nvarchar] (250) NOT NULL ,
	[displayorder] [int] NOT NULL ,
	[starttime] [datetime] NOT NULL ,
	[endtime] [datetime] NOT NULL ,
	[message] [ntext] NOT NULL ,
	CONSTRAINT [PK_dnt_announcements] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_attachments') IS NOT NULL
DROP TABLE [dnt_attachments]
GO

CREATE TABLE [dnt_attachments] (
	[aid] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_attachments_uid] DEFAULT (0),
	[tid] [int] NOT NULL ,
	[pid] [int] NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[readperm] [int] NOT NULL ,
	[filename] [nchar] (100) NOT NULL ,
	[description] [nchar] (100) NOT NULL ,
	[filetype] [nchar] (50) NOT NULL ,
	[filesize] [int] NOT NULL ,
	[attachment] [nchar] (255) NOT NULL ,
	[downloads] [int] NOT NULL ,
	[width] [int] NOT NULL DEFAULT (0),
	[height] [int] NOT NULL DEFAULT (0),
	[attachprice] [int] NOT NULL CONSTRAINT [DF_dnt_attachments_attachprice] DEFAULT (0),
	[isimage] [tinyint] NOT NULL CONSTRAINT [DF_dnt_attachments_isimage] DEFAULT (0)
	CONSTRAINT [PK_dnt_attachments] PRIMARY KEY  CLUSTERED 
	(
		[aid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_attachtypes') IS NOT NULL
DROP TABLE [dnt_attachtypes]
GO

CREATE TABLE [dnt_attachtypes] (
	[id] [smallint] IDENTITY (1, 1) NOT NULL ,
	[extension] [varchar] (256) NOT NULL ,
	[maxsize] [int] NOT NULL 
) ON [PRIMARY]
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

IF OBJECT_ID('dnt_bbcodes') IS NOT NULL
DROP TABLE [dnt_bbcodes]
GO

CREATE TABLE [dnt_bbcodes] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[available] [int] NOT NULL ,
	[tag] [varchar] (100)  NOT NULL ,
	[icon] [varchar] (50) NULL ,
	[example] [nvarchar] (255) NOT NULL ,
	[params] [int] NOT NULL ,
	[nest] [int] NOT NULL ,
	[explanation] [ntext] NULL ,
	[replacement] [ntext] NULL ,
	[paramsdescript] [ntext] NULL ,
	[paramsdefvalue] [ntext] NULL ,
	CONSTRAINT [PK_dnt_bbcodes] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_bonuslog') IS NOT NULL
DROP TABLE [dnt_bonuslog]
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

IF OBJECT_ID('dnt_creditslog') IS NOT NULL
DROP TABLE [dnt_creditslog]
GO

CREATE TABLE [dnt_creditslog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL ,
	[fromto] [int] NOT NULL ,
	[sendcredits] [tinyint] NOT NULL ,
	[receivecredits] [tinyint] NOT NULL ,
	[send] [float] NOT NULL ,
	[receive] [float] NOT NULL ,
	[paydate] [datetime] NOT NULL ,
	[operation] [tinyint] NOT NULL ,
	CONSTRAINT [PK_dnt_creditslog] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_debatediggs') IS NOT NULL
DROP TABLE [dnt_debatediggs]
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

IF OBJECT_ID('dnt_debates') IS NOT NULL
DROP TABLE [dnt_debates]
GO

CREATE TABLE [dnt_debates] (
	[tid] [int] NOT NULL ,
	[positiveopinion] [nvarchar] (200)  NOT NULL ,
	[negativeopinion] [nvarchar] (200) NOT NULL ,
	[terminaltime] [datetime] NOT NULL ,
	[positivediggs] [int] NOT NULL CONSTRAINT [DF_dnt_debates_positidiggs] DEFAULT (0),
	[negativediggs] [int] NOT NULL CONSTRAINT [DF_dnt_debates_negatidiggs] DEFAULT (0),
	CONSTRAINT [PK_dnt_debate] PRIMARY KEY  CLUSTERED 
	(
		[tid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_failedlogins') IS NOT NULL
DROP TABLE [dnt_failedlogins]
GO

CREATE TABLE [dnt_failedlogins] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[ip] [char] (15) NOT NULL ,
	[errcount] [smallint] NOT NULL CONSTRAINT [DF_dnt_failedlogins_errcount] DEFAULT (0),
	[lastupdate] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_failedlogins_lastupdate] DEFAULT (getdate()),
	CONSTRAINT [PK_dnt_failedlogins] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO
CREATE UNIQUE INDEX [ip] ON [dnt_failedlogins]([ip]) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_favorites') IS NOT NULL
DROP TABLE [dnt_favorites]
GO

CREATE TABLE [dnt_favorites] (
	[uid] [int] NOT NULL ,
	[tid] [int] NOT NULL ,
	[typeid] [tinyint] NOT NULL CONSTRAINT [DF_dnt_favorites_typeid] DEFAULT (0),
	[favtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_favorites_favtime] DEFAULT (getdate()),
	[viewtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_favorites_viewtime] DEFAULT (getdate())
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_help') IS NOT NULL
DROP TABLE [dnt_help]
GO

CREATE TABLE [dnt_help] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[title] [nvarchar] (100) NOT NULL ,
	[message] [ntext] NULL ,
	[pid] [int] NOT NULL ,
	[orderby] [int] NULL CONSTRAINT [DF_dnt_help_orderby] DEFAULT (0)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_locations') IS NOT NULL
DROP TABLE [dnt_locations]
GO

CREATE TABLE [dnt_locations] (
	[lid] [int] IDENTITY (1, 1) NOT NULL ,
	[city] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_locations_city] DEFAULT (''),
	[state] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_locations_state] DEFAULT (''),
	[country] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_locations_country] DEFAULT (''),
	[zipcode] [nvarchar] (20) NOT NULL CONSTRAINT [DF_dnt_locations_zipcode] DEFAULT ('')
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_medals') IS NOT NULL
DROP TABLE [dnt_medals]
GO

CREATE TABLE [dnt_medals] (
	[medalid] [smallint] NOT NULL ,
	[name] [nvarchar] (50)  NOT NULL ,
	[available] [int] NOT NULL CONSTRAINT [DF_dnt_medals_available] DEFAULT (0),
	[image] [varchar] (30) NOT NULL CONSTRAINT [DF_dnt_medals_image] DEFAULT ('')
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_medalslog') IS NOT NULL
DROP TABLE [dnt_medalslog]
GO

CREATE TABLE [dnt_medalslog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[adminname] [nvarchar] (50) NULL ,
	[adminid] [int] NULL ,
	[ip] [nvarchar] (15) NULL ,
	[postdatetime] [datetime] NULL CONSTRAINT [DF_dnt_medalslog_postdatetime] DEFAULT (getdate()),
	[username] [nvarchar] (50) NULL ,
	[uid] [int] NULL ,
	[actions] [nvarchar] (100) NULL ,
	[medals] [int] NULL ,
	[reason] [nvarchar] (100)  NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_moderatormanagelog') IS NOT NULL
DROP TABLE [dnt_moderatormanagelog]
GO

CREATE TABLE [dnt_moderatormanagelog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[moderatoruid] [int] NULL ,
	[moderatorname] [nvarchar] (50)  NULL ,
	[groupid] [int] NULL ,
	[grouptitle] [nvarchar] (50)  NULL ,
	[ip] [varchar] (15)  NULL ,
	[postdatetime] [datetime] NULL CONSTRAINT [DF_dnt_moderatormanagelog_postdatetime] DEFAULT (getdate()),
	[fid] [int] NULL ,
	[fname] [nvarchar] (100)  NULL ,
	[tid] [int] NULL ,
	[title] [varchar] (200) NULL ,
	[actions] [varchar] (50) NULL ,
	[reason] [nvarchar] (200) NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_moderators') IS NOT NULL
DROP TABLE [dnt_moderators]
GO

CREATE TABLE [dnt_moderators] (
	[uid] [int] NOT NULL ,
	[fid] [smallint] NOT NULL ,
	[displayorder] [smallint] NOT NULL ,
	[inherited] [smallint] NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_myattachments') IS NOT NULL
DROP TABLE [dnt_myattachments]
GO

CREATE TABLE [dnt_myattachments] (
	[aid] [int] NOT NULL ,
	[uid] [int] NOT NULL ,
	[attachment] [nchar] (100) NOT NULL ,
	[description] [nchar] (100) NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[downloads] [int] NOT NULL ,
	[filename] [nchar] (100) NOT NULL ,
	[pid] [int] NOT NULL CONSTRAINT [DF_dnt_myattachments_pid] DEFAULT (0),
	[tid] [int] NOT NULL CONSTRAINT [DF_dnt_myattachments_tid] DEFAULT (0),
	[extname] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_myattachments_extname] DEFAULT ('')
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_myposts') IS NOT NULL
DROP TABLE [dnt_myposts]
GO

CREATE TABLE [dnt_myposts] (
	[uid] [int] NOT NULL ,
	[tid] [int] NOT NULL ,
	[pid] [int] NOT NULL ,
	[dateline] [smalldatetime] NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_mytopics') IS NOT NULL
DROP TABLE [dnt_mytopics]
GO

CREATE TABLE [dnt_mytopics] (
	[uid] [int] NOT NULL ,
	[tid] [int] NOT NULL ,
	[dateline] [smalldatetime] NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_online') IS NOT NULL
DROP TABLE [dnt_online]
GO

CREATE TABLE [dnt_online] (
	[olid] [int] IDENTITY (1, 1) NOT NULL ,
	[userid] [int] NOT NULL CONSTRAINT [DF_dnt_online_userid] DEFAULT ((-1)),
	[ip] [varchar] (15) NOT NULL CONSTRAINT [DF_dnt_online_ip] DEFAULT ('0.0.0.0'),
	[username] [nvarchar] (20) NOT NULL CONSTRAINT [DF_dnt_online_username] DEFAULT (''),
	[nickname] [nvarchar] (20) NOT NULL CONSTRAINT [DF_dnt_online_nickname] DEFAULT (''),
	[password] [char] (32) NOT NULL CONSTRAINT [DF_dnt_online_password] DEFAULT (''),
	[groupid] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_groupid] DEFAULT (0),
	[olimg] [varchar] (80) NOT NULL CONSTRAINT [DF_dnt_online_olimg] DEFAULT (''),
	[adminid] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_adminid] DEFAULT (0),
	[invisible] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_invisible] DEFAULT (0),
	[action] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_action] DEFAULT (0),
	[lastactivity] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_lastactivity] DEFAULT (0),
	[lastposttime] [datetime] NOT NULL CONSTRAINT [DF_dnt_online_lastposttime] DEFAULT ('1900-1-1 00:00:00'),
	[lastpostpmtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_online_lastpostpmtime] DEFAULT ('1900-1-1 00:00:00'),
	[lastsearchtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_online_lastsearchtime] DEFAULT ('1900-1-1 00:00:00'),
	[lastupdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_online_lastupdatetime] DEFAULT (getdate()),
	[forumid] [int] NOT NULL CONSTRAINT [DF_dnt_online_forumid] DEFAULT (0),
	[forumname] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_online_forumname] DEFAULT (''),
	[titleid] [int] NOT NULL CONSTRAINT [DF_dnt_online_titleid] DEFAULT (0),
	[title] [nvarchar] (80) NOT NULL CONSTRAINT [DF_dnt_online_title] DEFAULT (''),
	[verifycode] [varchar] (10) NOT NULL CONSTRAINT [DF_dnt_online_verifycode] DEFAULT (''),
	[newpms] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_newpms] DEFAULT(0),
	[newnotices] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_newnotices] DEFAULT (0)
	
	CONSTRAINT [PK_dnt_online] PRIMARY KEY  CLUSTERED 
	(
		[olid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_onlinelist') IS NOT NULL
DROP TABLE [dnt_onlinelist]
GO

CREATE TABLE [dnt_onlinelist] (
	[groupid] [smallint] NOT NULL ,
	[displayorder] [int] NULL ,
	[title] [nvarchar] (50) NOT NULL ,
	[img] [varchar] (50) NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_onlinetime') IS NOT NULL
DROP TABLE [dnt_onlinetime]
GO

CREATE TABLE [dnt_onlinetime] (
	[uid] [int] NOT NULL ,
	[thismonth] [smallint] NOT NULL CONSTRAINT [DF_dnt_onlinetime_thismonth] DEFAULT (0),
	[total] [int] NOT NULL CONSTRAINT [DF_dnt_onlinetime_total] DEFAULT (0),
	[lastupdate] [datetime] NOT NULL CONSTRAINT [DF_dnt_onlinetime_lastupdate] DEFAULT (getdate()),
	CONSTRAINT [PK_dnt_onlinetime] PRIMARY KEY  CLUSTERED 
	(
		[uid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_paymentlog') IS NOT NULL
DROP TABLE [dnt_paymentlog]
GO

CREATE TABLE [dnt_paymentlog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL ,
	[tid] [int] NOT NULL ,
	[authorid] [int] NOT NULL ,
	[buydate] [datetime] NOT NULL CONSTRAINT [DF_dnt_paymentlog_buydate] DEFAULT (getdate()),
	[amount] [smallint] NOT NULL ,
	[netamount] [smallint] NOT NULL ,
	CONSTRAINT [PK_dnt_paymentlog] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_pms') IS NOT NULL
DROP TABLE [dnt_pms]
GO

CREATE TABLE [dnt_pms] (
	[pmid] [int] IDENTITY (1, 1) NOT NULL ,
	[msgfrom] [nvarchar] (50) NOT NULL ,
	[msgfromid] [int] NOT NULL ,
	[msgto] [nvarchar] (50) NOT NULL ,
	[msgtoid] [int] NOT NULL ,
	[folder] [smallint] NOT NULL ,
	[new] [int] NOT NULL ,
	[subject] [nvarchar] (60) NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[message] [ntext] NOT NULL ,
	CONSTRAINT [PK_dnt_pms] PRIMARY KEY  CLUSTERED 
	(
		[pmid]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_polloptions') IS NOT NULL
DROP TABLE [dnt_polloptions]
GO

CREATE TABLE [dnt_polloptions] (
	[polloptionid] [int] IDENTITY (1, 1) NOT NULL ,
	[tid] [int] NOT NULL CONSTRAINT [DF_dnt_polloptions_tid] DEFAULT (0),
	[pollid] [int] NOT NULL CONSTRAINT [DF_dnt_polloptions_pollid] DEFAULT (0),
	[votes] [int] NOT NULL CONSTRAINT [DF_dnt_polloptions_votes] DEFAULT (0),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_polloptions_displayorder] DEFAULT (0),
	[polloption] [nvarchar] (80) NOT NULL CONSTRAINT [DF_dnt_polloptions_polloption] DEFAULT (''),
	[voternames] [ntext] NOT NULL CONSTRAINT [DF_dnt_polloptions_voternames] DEFAULT (''),
	CONSTRAINT [PK_dnt_polloptions] PRIMARY KEY  CLUSTERED 
	(
		[polloptionid]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_polls') IS NOT NULL
DROP TABLE [dnt_polls]
GO

CREATE TABLE [dnt_polls] (
	[pollid] [int] IDENTITY (1, 1) NOT NULL ,
	[tid] [int] NOT NULL CONSTRAINT [DF_dnt_polls_tid] DEFAULT (0),
	[displayorder] [int] NOT NULL ,
	[multiple] [tinyint] NOT NULL CONSTRAINT [DF_dnt_polls_multiple] DEFAULT (0),
	[visible] [tinyint] NOT NULL CONSTRAINT [DF_dnt_polls_visible] DEFAULT (0),
	[allowview] [tinyint] NOT NULL CONSTRAINT [DF_dnt_polls_allowview] DEFAULT(0),
	[maxchoices] [smallint] NOT NULL CONSTRAINT [DF_dnt_polls_maxchoices] DEFAULT (0),
	[expiration] [datetime] NOT NULL ,
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_polls_uid] DEFAULT (0),
	[voternames] [ntext] NOT NULL CONSTRAINT [DF_dnt_polls_voternames] DEFAULT ('')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_postdebatefields') IS NOT NULL
DROP TABLE [dnt_postdebatefields]
GO

CREATE TABLE [dnt_postdebatefields] (
	[tid] [int] NOT NULL CONSTRAINT [DF_dnt_postdebatefields_tid] DEFAULT (0),
	[pid] [int] NOT NULL CONSTRAINT [DF_dnt_postdebatefields_pid] DEFAULT (0),
	[opinion] [int] NOT NULL CONSTRAINT [DF_dnt_postdebatefields_opinion] DEFAULT (0),
	[diggs] [int] NOT NULL CONSTRAINT [DF_dnt_postdebatefields_diggs] DEFAULT (0)
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_postid') IS NOT NULL
DROP TABLE [dnt_postid]
GO

CREATE TABLE [dnt_postid] (
	[pid] [int] IDENTITY (1, 1) NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	CONSTRAINT [PK_dnt_postid] PRIMARY KEY  CLUSTERED 
	(
		[pid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_posts1') IS NOT NULL
DROP TABLE [dnt_posts1]
GO

CREATE TABLE [dnt_posts1] (
	[pid] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_pid] DEFAULT (0),
	[fid] [int] NOT NULL ,
	[tid] [int] NOT NULL ,
	[parentid] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_parentid] DEFAULT (0),
	[layer] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_layer] DEFAULT (0),
	[poster] [nvarchar] (20) NOT NULL CONSTRAINT [DF_dnt_posts1_poster] DEFAULT (''),
	[posterid] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_posterid] DEFAULT (0),
	[title] [nvarchar] (60) NOT NULL ,
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_posts1_postdatetime] DEFAULT (getdate()),
	[message] [ntext] NOT NULL CONSTRAINT [DF_dnt_posts1_message] DEFAULT (''),
	[ip] [nvarchar] (15) NOT NULL CONSTRAINT [DF_dnt_posts1_ip] DEFAULT (''),
	[lastedit] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_posts1_lastedit] DEFAULT (''),
	[invisible] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_invisible] DEFAULT (0),
	[usesig] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_usesig] DEFAULT (0),
	[htmlon] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_htmlon] DEFAULT (0),
	[smileyoff] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_smileyoff] DEFAULT (0),
	[parseurloff] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_parseurloff] DEFAULT (0),
	[bbcodeoff] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_bbcodeoff] DEFAULT (0),
	[attachment] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_attachment] DEFAULT (0),
	[rate] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_rate] DEFAULT (0),
	[ratetimes] [int] NOT NULL CONSTRAINT [DF_dnt_posts1_ratetimes] DEFAULT (0),
	CONSTRAINT [PK_dnt_posts1] PRIMARY KEY  CLUSTERED 
	(
		[pid]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_ratelog') IS NOT NULL
DROP TABLE [dnt_ratelog]
GO

CREATE TABLE [dnt_ratelog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[pid] [int] NOT NULL ,
	[uid] [int] NOT NULL ,
	[username] [nchar] (20) NOT NULL ,
	[extcredits] [tinyint] NOT NULL ,
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_ratelog_postdatetime] DEFAULT (getdate()),
	[score] [smallint] NOT NULL ,
	[reason] [nvarchar] (50) NOT NULL ,
	CONSTRAINT [PK_dnt_ratelog] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_scheduledevents') IS NOT NULL
DROP TABLE [dnt_scheduledevents]
GO

CREATE TABLE [dnt_scheduledevents] (
	[scheduleID] [int] IDENTITY (1, 1) NOT NULL ,
	[key] [varchar] (50) NOT NULL ,
	[lastexecuted] [datetime] NOT NULL ,
	[servername] [varchar] (100) NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_searchcaches') IS NOT NULL
DROP TABLE [dnt_searchcaches]
GO

CREATE TABLE [dnt_searchcaches] (
	[searchid] [int] IDENTITY (1, 1) NOT NULL ,
	[keywords] [nvarchar] (255) NOT NULL ,
	[searchstring] [nvarchar] (255) NOT NULL ,
	[ip] [varchar] (15) NOT NULL ,
	[uid] [int] NOT NULL ,
	[groupid] [int] NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[expiration] [datetime] NOT NULL ,
	[topics] [int] NOT NULL ,
	[tids] [text]  NOT NULL ,
	CONSTRAINT [PK_dnt_searchindex] PRIMARY KEY  CLUSTERED 
	(
		[searchid]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_smilies') IS NOT NULL
DROP TABLE [dnt_smilies]
GO

CREATE TABLE [dnt_smilies] (
	[id] [int] NOT NULL ,
	[displayorder] [int] NOT NULL ,
	[type] [int] NOT NULL ,
	[code] [nvarchar] (30) NOT NULL ,
	[url] [varchar] (60) NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_statistics') IS NOT NULL
DROP TABLE [dnt_statistics]
GO

CREATE TABLE [dnt_statistics] (
	[totaltopic] [int] NOT NULL ,
	[totalpost] [int] NOT NULL ,
	[totalusers] [int] NOT NULL ,
	[lastusername] [nchar] (20) NOT NULL ,
	[lastuserid] [int] NOT NULL ,
	[highestonlineusercount] [int] NULL ,
	[highestonlineusertime] [smalldatetime] NULL ,
	[yesterdayposts] [int] NOT NULL CONSTRAINT [DF__dnt_statistics__yesterdayposts] DEFAULT (0),
	[highestposts] [int] NOT NULL CONSTRAINT [DF_dnt_statistics_highestposts] DEFAULT (0),
	[highestpostsdate] [char] (10) NOT NULL CONSTRAINT [DF_dnt_statistics_highestpostsdate] DEFAULT ('')
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_stats') IS NOT NULL
DROP TABLE [dnt_stats]
GO

CREATE TABLE [dnt_stats] (
	[type] [char] (10) NOT NULL ,
	[variable] [char] (20) NOT NULL ,
	[count] [int] NOT NULL CONSTRAINT [DF_dnt_stats_count] DEFAULT (0)
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_statvars') IS NOT NULL
DROP TABLE [dnt_statvars]
GO

CREATE TABLE [dnt_statvars] (
	[type] [char] (20) NOT NULL ,
	[variable] [char] (20) NOT NULL ,
	[value] [text] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_tablelist') IS NOT NULL
DROP TABLE [dnt_tablelist]
GO

CREATE TABLE [dnt_tablelist] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[createdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_tablelist_createdatetime] DEFAULT (getdate()),
	[description] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_tablelist_description] DEFAULT (''),
	[mintid] [int] NOT NULL CONSTRAINT [DF_dnt_tablelist_mintid] DEFAULT ((-1)),
	[maxtid] [int] NOT NULL CONSTRAINT [DF_dnt_tablelist_maxtid] DEFAULT ((-1)),
	CONSTRAINT [PK_dnt_tablelist] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_tags') IS NOT NULL
DROP TABLE [dnt_tags]
GO

CREATE TABLE [dnt_tags] (
	[tagid] [int] IDENTITY (1, 1) NOT NULL ,
	[tagname] [nchar] (10) NOT NULL ,
	[userid] [int] NOT NULL CONSTRAINT [DF_dnt_tags_userid] DEFAULT (0),
	[postdatetime] [datetime] NOT NULL ,
	[orderid] [int] NOT NULL CONSTRAINT [DF_dnt_tags_orderid] DEFAULT (0),
	[color] [char] (6) NOT NULL ,
	[count] [int] NOT NULL CONSTRAINT [DF_dnt_tags_count] DEFAULT (0),
	[fcount] [int] NOT NULL CONSTRAINT [DF_dnt_tags_fcount] DEFAULT (0),
	[pcount] [int] NOT NULL CONSTRAINT [DF_dnt_tags_pcount] DEFAULT (0),
	[scount] [int] NOT NULL CONSTRAINT [DF_dnt_tags_scount] DEFAULT (0),
	[vcount] [int] NOT NULL CONSTRAINT [DF_dnt_tags_vcount] DEFAULT (0),
	[gcount] [int] NOT NULL CONSTRAINT [DF_dnt_tags_gcount] DEFAULT (0),
	CONSTRAINT [PK_dnt_tags] PRIMARY KEY  CLUSTERED 
	(
		[tagid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_templates') IS NOT NULL
DROP TABLE [dnt_templates]
GO

CREATE TABLE [dnt_templates] (
	[templateid] [smallint] IDENTITY (1, 1) NOT NULL ,
	[directory] [varchar] (100) NOT NULL ,
	[name] [nvarchar] (50) NOT NULL ,
	[author] [nvarchar] (100) NOT NULL ,
	[createdate] [nvarchar] (50) NOT NULL ,
	[ver] [nvarchar] (100) NOT NULL ,
	[fordntver] [nvarchar] (100) NOT NULL ,
	[copyright] [nvarchar] (100) NOT NULL ,
	[templateurl] [nvarchar] (100) NOT NULL CONSTRAINT [DF_dnt_templates_templateurl] DEFAULT('')
	CONSTRAINT [PK_dnt_templates] PRIMARY KEY  CLUSTERED 
	(
		[templateid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_topicidentify') IS NOT NULL
DROP TABLE [dnt_topicidentify]
GO

CREATE TABLE [dnt_topicidentify] (
	[identifyid] [int] IDENTITY (1, 1) NOT NULL ,
	[name] [nvarchar] (50) NOT NULL ,
	[filename] [varchar] (50) NOT NULL ,
	CONSTRAINT [PK_dnt_topicidentify] PRIMARY KEY  CLUSTERED 
	(
		[identifyid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_topics') IS NOT NULL
DROP TABLE [dnt_topics]
GO

CREATE TABLE [dnt_topics] (
	[tid] [int] IDENTITY (1, 1) NOT NULL ,
	[fid] [smallint] NOT NULL ,
	[iconid] [tinyint] NOT NULL CONSTRAINT [DF_dnt_topics_iconid] DEFAULT (0),
	[typeid] [int] NOT NULL CONSTRAINT [DF_dnt_topics_typeid] DEFAULT (0),
	[readperm] [int] NOT NULL CONSTRAINT [DF_dnt_topics_readperm] DEFAULT (0),
	[price] [smallint] NOT NULL CONSTRAINT [DF_dnt_topics_price] DEFAULT (0),
	[poster] [nchar] (20) NOT NULL CONSTRAINT [DF_dnt_topics_poster] DEFAULT (''),
	[posterid] [int] NOT NULL CONSTRAINT [DF_dnt_topics_posterid] DEFAULT (0),
	[title] [nchar] (60) NOT NULL ,
	[attention] [int] NOT NULL CONSTRAINT [DF_dnt_topics_attention] DEFAULT (0),
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_topics_postdatetime] DEFAULT (getdate()),
	[lastpost] [datetime] NOT NULL CONSTRAINT [DF_dnt_topics_lastpost] DEFAULT (getdate()),
	[lastpostid] [int] NOT NULL CONSTRAINT [DF_dnt_topics_lastpostid] DEFAULT (0),
	[lastposter] [nchar] (20) NOT NULL CONSTRAINT [DF_dnt_topics_lastposter] DEFAULT (''),
	[lastposterid] [int] NOT NULL CONSTRAINT [DF_dnt_topics_lastposterid] DEFAULT (0),
	[views] [int] NOT NULL CONSTRAINT [DF_dnt_topics_views] DEFAULT (0),
	[replies] [int] NOT NULL CONSTRAINT [DF_dnt_topics_replies] DEFAULT (0),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_topics_displayorder] DEFAULT (0),
	[highlight] [varchar] (500) NOT NULL CONSTRAINT [DF_dnt_topics_highlight] DEFAULT (''),
	[digest] [tinyint] NOT NULL CONSTRAINT [DF_dnt_topics_digest] DEFAULT (0),
	[rate] [int] NOT NULL CONSTRAINT [DF_dnt_topics_rate] DEFAULT (0),
	[hide] [int] NOT NULL CONSTRAINT [DF_dnt_topics_blog] DEFAULT (0),
	[attachment] [int] NOT NULL CONSTRAINT [DF_dnt_topics_attachment] DEFAULT (0),
	[moderated] [tinyint] NOT NULL CONSTRAINT [DF_dnt_topics_moderated] DEFAULT (0),
	[closed] [int] NOT NULL CONSTRAINT [DF_dnt_topics_closed] DEFAULT (0),
	[magic] [int] NOT NULL CONSTRAINT [DF_dnt_topics_magic] DEFAULT (0),
	[identify] [int] NOT NULL CONSTRAINT [DF_dnt_topics_identify] DEFAULT ('0'),
	[special] [tinyint] NOT NULL CONSTRAINT [DF_dnt_topics_special] DEFAULT (0),
	CONSTRAINT [PK_dnt_topics] PRIMARY KEY  NONCLUSTERED 
	(
		[tid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_topictagcaches') IS NOT NULL
DROP TABLE [dnt_topictagcaches]
GO

CREATE TABLE [dnt_topictagcaches] (
	[tid] [int] NOT NULL CONSTRAINT [DF_dnt_topictagcaches_tid] DEFAULT (0),
	[linktid] [int] NOT NULL CONSTRAINT [DF_dnt_topictagcaches_linktid] DEFAULT (0),
	[linktitle] [nchar] (60) NOT NULL 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_topictags') IS NOT NULL
DROP TABLE [dnt_topictags]
GO

CREATE TABLE [dnt_topictags] (
	[tagid] [int] NOT NULL CONSTRAINT [DF_dnt_topictags_tagid] DEFAULT (0),
	[tid] [int] NOT NULL CONSTRAINT [DF_dnt_topictags_tid] DEFAULT (0)
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_topictypes') IS NOT NULL
DROP TABLE [dnt_topictypes]
GO

CREATE TABLE [dnt_topictypes] (
	[typeid] [int] IDENTITY (1, 1) NOT NULL ,
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_topictypes_displayorder] DEFAULT (0),
	[name] [nvarchar] (30) NOT NULL CONSTRAINT [DF_dnt_topictypes_name] DEFAULT (''),
	[description] [nvarchar] (500) NOT NULL CONSTRAINT [DF_dnt_topictypes_description] DEFAULT ('')
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_userfields') IS NOT NULL
DROP TABLE [dnt_userfields]
GO

CREATE TABLE [dnt_userfields] (
	[uid] [int] NOT NULL ,
	[website] [nvarchar] (80) NOT NULL CONSTRAINT [DF_dnt_userfields_website] DEFAULT (''),
	[icq] [varchar] (12) NOT NULL CONSTRAINT [DF_dnt_userfields_icq] DEFAULT (''),
	[qq] [varchar] (12) NOT NULL CONSTRAINT [DF_dnt_userfields_qq] DEFAULT (''),
	[yahoo] [varchar] (40) NOT NULL CONSTRAINT [DF_dnt_userfields_yahoo] DEFAULT (''),
	[msn] [varchar] (40) NOT NULL CONSTRAINT [DF_dnt_userfields_msn] DEFAULT (''),
	[skype] [varchar] (40) NOT NULL CONSTRAINT [DF_dnt_userfields_skype_] DEFAULT (''),
	[location] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_userfields_location] DEFAULT (''),
	[customstatus] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_userfields_customstatus] DEFAULT (''),
	[avatar] [nvarchar] (255) NOT NULL CONSTRAINT [DF_dnt_userfields_avatar] DEFAULT ('avatars\common\0.gif'),
	[avatarwidth] [int] NOT NULL CONSTRAINT [DF_dnt_userfields_avatarwidth] DEFAULT (60),
	[avatarheight] [int] NOT NULL CONSTRAINT [DF_dnt_userfields_avatarheight] DEFAULT (60),
	[medals] [varchar] (300) NOT NULL CONSTRAINT [DF_dnt_userfields_medals] DEFAULT (''),
	[bio] [nvarchar] (500) NOT NULL CONSTRAINT [DF_dnt_userfields_bio] DEFAULT (''),
	[signature] [nvarchar] (500) NOT NULL CONSTRAINT [DF_dnt_userfields_signature] DEFAULT (''),
	[sightml] [nvarchar] (1000) NOT NULL CONSTRAINT [DF_dnt_userfields_sightml] DEFAULT (''),
	[authstr] [varchar] (20) NOT NULL CONSTRAINT [DF_dnt_userfields_authstr] DEFAULT (''),
	[authtime] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_userfields_authtime] DEFAULT (getdate()),
	[authflag] [tinyint] NOT NULL CONSTRAINT [DF_dnt_userfields_authflag] DEFAULT (0),
	[realname] [nvarchar] (10) NOT NULL CONSTRAINT [DF_dnt_userfields_realname] DEFAULT (''),
	[idcard] [varchar] (20) NOT NULL CONSTRAINT [DF_dnt_userfields_idcard] DEFAULT (''),
	[mobile] [varchar] (20) NOT NULL CONSTRAINT [DF_dnt_userfields_mobile] DEFAULT (''),
	[phone] [varchar] (20) NOT NULL CONSTRAINT [DF_dnt_userfields_phone] DEFAULT (''),
	[ignorepm] [nvarchar] (1000) NOT NULL CONSTRAINT [DF_dnt_userfields_ignorepm] DEFAULT (''),
	CONSTRAINT [PK_dnt_userfields] PRIMARY KEY  CLUSTERED 
	(
		[uid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_usergroups') IS NOT NULL
DROP TABLE [dnt_usergroups]
GO

CREATE TABLE [dnt_usergroups] (
	[groupid] [smallint] IDENTITY (1, 1) NOT NULL ,
	[radminid] [int] NOT NULL ,
	[type] [smallint] NULL CONSTRAINT [DF_dnt_usergroups_type] DEFAULT (0),
	[system] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_system] DEFAULT ('0'),
	[grouptitle] [nvarchar] (50) NOT NULL ,
	[creditshigher] [int] NOT NULL ,
	[creditslower] [int] NOT NULL ,
	[stars] [int] NOT NULL ,
	[color] [char] (7) NOT NULL ,
	[groupavatar] [nvarchar] (60) NOT NULL ,
	[readaccess] [int] NOT NULL ,
	[allowvisit] [int] NOT NULL ,
	[allowpost] [int] NOT NULL ,
	[allowreply] [int] NOT NULL ,
	[allowpostpoll] [int] NOT NULL ,
	[allowdirectpost] [int] NOT NULL ,
	[allowgetattach] [int] NOT NULL ,
	[allowpostattach] [int] NOT NULL ,
	[allowvote] [int] NOT NULL ,
	[allowmultigroups] [int] NOT NULL ,
	[allowsearch] [int] NOT NULL ,
	[allowavatar] [int] NOT NULL ,
	[allowcstatus] [int] NOT NULL ,
	[allowuseblog] [int] NOT NULL ,
	[allowinvisible] [int] NOT NULL ,
	[allowtransfer] [int] NOT NULL ,
	[allowsetreadperm] [int] NOT NULL ,
	[allowsetattachperm] [int] NOT NULL ,
	[allowhidecode] [int] NOT NULL ,
	[allowhtml] [int] NOT NULL ,
	[allowhtmltitle] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowhtmltitle] DEFAULT (0),
	[allowcusbbcode] [int] NOT NULL ,
	[allownickname] [int] NOT NULL ,
	[allowsigbbcode] [int] NOT NULL ,
	[allowsigimgcode] [int] NOT NULL ,
	[allowviewpro] [int] NOT NULL ,
	[allowviewstats] [int] NOT NULL ,
	[disableperiodctrl] [int] NOT NULL ,
	[reasonpm] [int] NOT NULL ,
	[maxprice] [smallint] NOT NULL ,
	[maxpmnum] [smallint] NOT NULL ,
	[maxsigsize] [smallint] NOT NULL ,
	[maxattachsize] [int] NOT NULL ,
	[maxsizeperday] [int] NOT NULL ,
	[attachextensions] [char] (100) NOT NULL ,
	[raterange] [nchar] (500) NOT NULL ,
	[allowspace] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowspace] DEFAULT (0),
	[maxspaceattachsize] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_maxspaceattachsize] DEFAULT (0),
	[maxspacephotosize] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_maxspacephotosize] DEFAULT (0),
	[allowdebate] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowdebate] DEFAULT (0),
	[allowbonus] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowbonus] DEFAULT (0),
	[minbonusprice] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_minbonusprice] DEFAULT (0),
	[maxbonusprice] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_maxbonusprice] DEFAULT (0),
	[allowtrade] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowtrade] DEFAULT (0),
	[allowdiggs] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowdiggs] DEFAULT (0),
	[modnewtopics] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_modnewtopics] DEFAULT (0),
	[modnewposts] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_modnewposts] DEFAULT (0),
	[ignoreseccode] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_ignoreseccode] DEFAULT (0)
	CONSTRAINT [PK_dnt_usergroups] PRIMARY KEY  CLUSTERED 
	(
		[groupid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_users') IS NOT NULL
DROP TABLE [dnt_users]
GO

CREATE TABLE [dnt_users] (
	[uid] [int] IDENTITY (1, 1) NOT NULL ,
	[username] [nchar] (20) NOT NULL CONSTRAINT [DF_dnt_users_username] DEFAULT (''),
	[nickname] [nchar] (20) NOT NULL CONSTRAINT [DF_dnt_users_nickname] DEFAULT (''),
	[password] [char] (32) NOT NULL CONSTRAINT [DF__dnt_users_password] DEFAULT (''),
	[secques] [char] (8) NOT NULL CONSTRAINT [DF_dnt_users_secques] DEFAULT (''),
	[spaceid] [int] NOT NULL CONSTRAINT [DF_dnt_users_spaceid] DEFAULT (0),
	[gender] [int] NOT NULL CONSTRAINT [DF_dnt_users_gender] DEFAULT (0),
	[adminid] [int] NOT NULL CONSTRAINT [DF_dnt_users_adminid] DEFAULT (0),
	[groupid] [smallint] NOT NULL CONSTRAINT [DF_dnt_users_groupid] DEFAULT (0),
	[groupexpiry] [int] NOT NULL CONSTRAINT [DF_dnt_users_groupexpiry] DEFAULT (0),
	[extgroupids] [char] (60) NOT NULL CONSTRAINT [DF_dnt_users_extgroupids] DEFAULT (''),
	[regip] [char] (15) NOT NULL CONSTRAINT [DF_dnt_users_regip] DEFAULT (''),
	[joindate] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_users_joindate] DEFAULT (getdate()),
	[lastip] [char] (15) NOT NULL CONSTRAINT [DF_dnt_users_lastip] DEFAULT (''),
	[lastvisit] [datetime] NOT NULL CONSTRAINT [DF__dnt_users_lastvisit] DEFAULT (getdate()),
	[lastactivity] [datetime] NOT NULL CONSTRAINT [DF_dnt_users_lastactivity] DEFAULT (getdate()),
	[lastpost] [datetime] NOT NULL CONSTRAINT [DF_dnt_users_lastpost] DEFAULT (getdate()),
	[lastpostid] [int] NOT NULL CONSTRAINT [DF_dnt_users_lastpostid] DEFAULT (0),
	[lastposttitle] [nchar] (60) NOT NULL CONSTRAINT [DF_dnt_users_lastposttitle] DEFAULT (''),
	[posts] [int] NOT NULL CONSTRAINT [DF_dnt_users__posts_] DEFAULT ('0'),
	[digestposts] [smallint] NOT NULL CONSTRAINT [DF_dnt_users_digestposts] DEFAULT ('0'),
	[oltime] [int] NOT NULL CONSTRAINT [DF__dnt_users__oltim__14F1071C] DEFAULT ('0'),
	[pageviews] [int] NOT NULL CONSTRAINT [DF_dnt_users_pageviews] DEFAULT ('0'),
	[credits] [int] NOT NULL CONSTRAINT [DF_dnt_users_credits] DEFAULT ('0'),
	[extcredits1] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits1] DEFAULT ('0'),
	[extcredits2] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits2] DEFAULT ('0'),
	[extcredits3] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits3] DEFAULT ('0'),
	[extcredits4] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits4] DEFAULT ('0'),
	[extcredits5] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits5] DEFAULT ('0'),
	[extcredits6] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits6] DEFAULT ('0'),
	[extcredits7] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits7] DEFAULT ('0'),
	[extcredits8] [decimal](18, 2) NOT NULL CONSTRAINT [DF__dnt_users_extcredits8] DEFAULT ('0'),
	[avatarshowid] [int] NOT NULL CONSTRAINT [DF_dnt_users_avatarshowid] DEFAULT ('0'),
	[email] [char] (50) NOT NULL CONSTRAINT [DF_dnt_users_email] DEFAULT (''),
	[bday] [char] (10) NOT NULL CONSTRAINT [DF_dnt_users_bday_] DEFAULT (''),
	[sigstatus] [int] NOT NULL CONSTRAINT [DF_dnt_users_sigstatus] DEFAULT ('0'),
	[tpp] [int] NOT NULL CONSTRAINT [DF_dnt_users_tpp_] DEFAULT ('0'),
	[ppp] [int] NOT NULL CONSTRAINT [DF_dnt_users_ppp_] DEFAULT ('0'),
	[templateid] [smallint] NOT NULL CONSTRAINT [DF_dnt_users_templateid] DEFAULT ('0'),
	[pmsound] [int] NOT NULL CONSTRAINT [DF_dnt_users_pmsound] DEFAULT ('0'),
	[showemail] [int] NOT NULL CONSTRAINT [DF_dnt_users_showemail] DEFAULT ('0'),
	[invisible] [int] NOT NULL CONSTRAINT [DF__dnt_users_invisible] DEFAULT ('0'),
	[newpm] [int] NOT NULL CONSTRAINT [DF_dnt_users_newpm] DEFAULT ('0'),
	[newpmcount] [int] NOT NULL CONSTRAINT [DF_dnt_users_newpmcount] DEFAULT (0),
	[accessmasks] [int] NOT NULL CONSTRAINT [DF_dnt_users_accessmasks] DEFAULT ('0'),
	[onlinestate] [int] NOT NULL CONSTRAINT [DF_dnt_users_onlinestate] DEFAULT (0),
	[newsletter] [int] NOT NULL CONSTRAINT [DF_dnt_users_newsletter] DEFAULT ('7'),
	[salt] [nchar] (6) NOT NULL CONSTRAINT [DF_dnt_users_salt] DEFAULT ('')
	CONSTRAINT [PK_dnt_members] PRIMARY KEY  CLUSTERED 
	(
		[uid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_words') IS NOT NULL
DROP TABLE [dnt_words]
GO

CREATE TABLE [dnt_words] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[admin] [nvarchar] (20) NOT NULL ,
	[find] [nvarchar] (255) NOT NULL ,
	[replacement] [nvarchar] (255) NOT NULL ,
	CONSTRAINT [PK_dnt_words] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_forums') IS NOT NULL
DROP TABLE [dnt_forums]
GO

CREATE TABLE [dnt_forums] (
	[fid] [int] IDENTITY (1, 1) NOT NULL ,
	[parentid] [int] NOT NULL CONSTRAINT [DF_dnt_forums_parentid] DEFAULT ('0'),
	[layer] [smallint] NOT NULL CONSTRAINT [DF_dnt_forums_layer] DEFAULT ('0'),
	[pathlist] [nchar] (3000) NOT NULL CONSTRAINT [DF_dnt_forums_pathlist] DEFAULT (''),
	[parentidlist] [char] (300) NOT NULL ,
	[subforumcount] [int] NOT NULL CONSTRAINT [DF_dnt_forums_subforumcount] DEFAULT (''),
	[name] [nchar] (50) NOT NULL ,
	[status] [int] NOT NULL CONSTRAINT [DF_dnt_forums_status] DEFAULT ('0'),
	[colcount] [smallint] NOT NULL CONSTRAINT [DF_dnt_forums_colcount] DEFAULT ('1'),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_forums_displayorder] DEFAULT ('0'),
	[templateid] [smallint] NOT NULL CONSTRAINT [DF_dnt_forums_templateid] DEFAULT ('0'),
	[topics] [int] NOT NULL CONSTRAINT [DF_dnt_forums_topics] DEFAULT ('0'),
	[curtopics] [int] NOT NULL CONSTRAINT [DF_dnt_forums_curtopics] DEFAULT ('0'),
	[posts] [int] NOT NULL CONSTRAINT [DF_dnt_forums_posts] DEFAULT ('0'),
	[todayposts] [int] NOT NULL CONSTRAINT [DF_dnt_forums_todayposts] DEFAULT ('0'),
	[lasttid] [int] NOT NULL CONSTRAINT [DF_dnt_forums_lasttid] DEFAULT ('0'),
	[lasttitle] [nchar] (60) NOT NULL CONSTRAINT [DF_dnt_forums_lasttitle] DEFAULT (''),
	[lastpost] [datetime] NOT NULL CONSTRAINT [DF_dnt_forums_lastpost] DEFAULT (''),
	[lastposterid] [int] NOT NULL CONSTRAINT [DF_dnt_forums_lastposterid] DEFAULT (''),
	[lastposter] [nchar] (20) NOT NULL CONSTRAINT [DF_dnt_forums_lastposter] DEFAULT (''),
	[allowsmilies] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowsmilies] DEFAULT ('0'),
	[allowrss] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowrss] DEFAULT ('0'),
	[allowhtml] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowhtml] DEFAULT ('0'),
	[allowbbcode] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowbbcode] DEFAULT ('0'),
	[allowimgcode] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowimgcode] DEFAULT ('0'),
	[allowblog] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowblog] DEFAULT ('0'),
	[istrade] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowtrade] DEFAULT ('0'),
	[allowpostspecial] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowpostspecial_1] DEFAULT (0),
	[allowspecialonly] [int] NULL CONSTRAINT [DF_dnt_forums_allowspecialonly] DEFAULT (0),
	[alloweditrules] [int] NOT NULL CONSTRAINT [DF_dnt_forums_alloweditrules] DEFAULT ('0'),
	[allowthumbnail] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowthumbnail] DEFAULT ('0'),
	[allowtag] [int] NOT NULL CONSTRAINT [DF_dnt_forums_allowtag] DEFAULT (0),
	[recyclebin] [int] NOT NULL CONSTRAINT [DF_dnt_forums_recyclebin] DEFAULT ('0'),
	[modnewposts] [int] NOT NULL CONSTRAINT [DF_dnt_forums_modnewposts] DEFAULT ('0'),
	[modnewtopics] [int] NOT NULL CONSTRAINT [DF_dnt_forums_modnewtopics] DEFAULT (0),
	[jammer] [int] NOT NULL CONSTRAINT [DF_dnt_forums_jammer] DEFAULT ('0'),
	[disablewatermark] [int] NOT NULL CONSTRAINT [DF_dnt_forums_disablewatermark] DEFAULT ('0'),
	[inheritedmod] [int] NOT NULL CONSTRAINT [DF_dnt_forums_inheritedmod] DEFAULT ('0'),
	[autoclose] [smallint] NOT NULL CONSTRAINT [DF_dnt_forums_autoclose] DEFAULT ('0')
) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_forumfields') IS NOT NULL
DROP TABLE [dnt_forumfields]
GO

CREATE TABLE [dnt_forumfields] (
	[fid] [int] NOT NULL ,
	[password] [nvarchar] (16) NOT NULL ,
	[icon] [varchar] (255) NULL ,
	[postcredits] [varchar] (255) NULL ,
	[replycredits] [varchar] (255) NULL ,
	[redirect] [varchar] (255) NULL ,
	[attachextensions] [varchar] (255) NULL ,
	[rules] [text] NULL ,
	[topictypes] [text] NULL ,
	[viewperm] [text] NULL ,
	[postperm] [text] NULL ,
	[replyperm] [text] NULL ,
	[getattachperm] [text] NULL ,
	[postattachperm] [text] NULL ,
	[moderators] [ntext]  NULL ,
	[description] [ntext] NULL ,
	[applytopictype] [tinyint] NOT NULL CONSTRAINT [DF_dnt_forumfields_applytopictype] DEFAULT (0),
	[postbytopictype] [tinyint] NOT NULL CONSTRAINT [DF_dnt_forumfields_postbytopictype] DEFAULT (0),
	[viewbytopictype] [tinyint] NOT NULL CONSTRAINT [DF_dnt_forumfields_viewbytopictype] DEFAULT (0),
	[topictypeprefix] [tinyint] NOT NULL CONSTRAINT [DF_dnt_forumfields_topictypeprefix] DEFAULT (0),
	[permuserlist] [ntext] NULL ,
	[seokeywords]  [nvarchar] (500) NULL,
	[seodescription]  [nvarchar] (500) Null,
	[rewritename]  [nvarchar] (20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF OBJECT_ID('dnt_banned') IS NOT NULL
DROP TABLE [dnt_banned]
GO

CREATE TABLE [dnt_banned](

	[id] [smallint]  NOT NULL,
	[ip1] [smallint]  NOT NULL,
	[ip2] [smallint]  NOT NULL,
	[ip3] [smallint]  NOT NULL,
	[ip4] [smallint]  NOT NULL,
	[admin] [nvarchar] (50) NOT NULL,
	[dateline] [datetime]  NOT NULL,
	[expiration] [datetime]  NOT NULL,			
	CONSTRAINT [PK_dnt_banned] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY]
)
GO

IF OBJECT_ID('dnt_navs') IS NOT NULL
DROP TABLE [dnt_navs]
GO

CREATE TABLE [dnt_navs]
(
	[id] [int] IDENTITY(1,1) not null CONSTRAINT [PK_dnt_navs_id] primary key(id),
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

IF OBJECT_ID('dnt_notices') IS NOT NULL
DROP TABLE [dnt_notices]
GO

CREATE TABLE [dnt_notices]
(
	[nid] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_dnt_notices_nid] PRIMARY KEY(nid),
	[uid] [int]					NOT NULL,
	[type] [smallint]			NOT NULL,
	[new] [tinyint]				NOT NULL,
	[posterid] [int]			NOT NULL,
	[poster] [nchar] (20)		NOT NULL,
	[note] [ntext]				NOT NULL ,
	[postdatetime] [datetime]	NOT NULL,
	[fromid] [int]				NOT NULL
)
GO

IF OBJECT_ID('dnt_forumlinks') IS NOT NULL
DROP TABLE [dnt_forumlinks]
GO

CREATE TABLE [dnt_forumlinks] (
	[id] [smallint] IDENTITY (1, 1) NOT NULL ,
	[displayorder] [int] NOT NULL ,
	[name] [nvarchar] (100) NOT NULL ,
	[url] [nvarchar] (100) NOT NULL ,
	[note] [nvarchar] (200) NOT NULL ,
	[logo] [nvarchar] (100) NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dnt_forums] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnt_forums] PRIMARY KEY  CLUSTERED 
	(
		[fid]
	)  ON [PRIMARY] 
GO

CREATE UNIQUE CLUSTERED INDEX [IX_dnt_statvars] ON [dnt_stats]([type], [variable]) WITH IGNORE_DUP_KEY ON [PRIMARY]
GO

CREATE  INDEX [invisible] ON [dnt_online]([userid], [invisible]) ON [PRIMARY]
GO

CREATE  INDEX [forumid] ON [dnt_online]([forumid]) ON [PRIMARY]
GO

CREATE  INDEX [password] ON [dnt_online]([userid], [password]) ON [PRIMARY]
GO

CREATE  INDEX [ip] ON [dnt_online]([userid], [ip]) ON [PRIMARY]
GO

CREATE  INDEX [forum] ON [dnt_online]([userid], [forumid], [invisible]) ON [PRIMARY]
GO

CREATE  INDEX [pwsecques] ON [dnt_users]([username], [password], [secques]) ON [PRIMARY]
GO

CREATE  INDEX [emailsecques] ON [dnt_users]([username], [email], [secques]) ON [PRIMARY]
GO

CREATE  INDEX [password] ON [dnt_users]([username], [password]) ON [PRIMARY]
GO

CREATE  INDEX [username] ON [dnt_users]([username]) ON [PRIMARY]
GO

CREATE INDEX [IX_dnt_posts1_fid] ON [dnt_posts1](fid,tid,posterid) INCLUDE (title,postdatetime,poster)
GO

CREATE INDEX [IX_dnt_posts1_posterid] ON [dnt_posts1] (posterid,tid,pid) INCLUDE (postdatetime)
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

CREATE  INDEX [parentid] ON [dnt_posts1]([parentid]) ON [PRIMARY]
GO

CREATE INDEX [tid] ON [dnt_posts1](tid) ON [primary]
GO

CREATE  INDEX [treelist] ON [dnt_posts1]([tid], [invisible], [parentid]) ON [PRIMARY]
GO

CREATE  UNIQUE  INDEX [showtopic] ON [dnt_posts1]([tid], [invisible], [pid]) ON [PRIMARY]
GO

CREATE  INDEX [getsearchid] ON [dnt_searchcaches]([searchstring], [groupid]) ON [PRIMARY]
GO

CREATE  INDEX [tid] ON [dnt_attachments]([tid]) ON [PRIMARY]
GO

CREATE  INDEX [pid] ON [dnt_attachments]([pid]) ON [PRIMARY]
GO

CREATE  INDEX [uid] ON [dnt_attachments]([uid]) ON [PRIMARY]
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

CREATE  INDEX [uid] ON [dnt_notices]([uid] DESC ) ON [PRIMARY]
GO

CREATE  INDEX [groupid] ON [dnt_users]([groupid],[adminid]) ON [PRIMARY]
GO

CREATE INDEX [uid] ON [dnt_myattachments]([uid], [extname]) ON [PRIMARY]
GO

CREATE INDEX [aid] ON [dnt_myattachments] ([aid]) ON [PRIMARY]
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
GO

CREATE INDEX [msgtoid] ON [dnt_pms] ([msgtoid])	ON [PRIMARY]
CREATE  UNIQUE  CLUSTERED  INDEX [list] ON [dnt_topics]([fid], [displayorder], [lastpostid] DESC ) ON [PRIMARY]
GO

IF OBJECT_ID('dnt_invitation') IS NOT NULL
DROP TABLE [dnt_invitation]
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

IF OBJECT_ID('dnt_trendstat') IS NOT NULL
DROP TABLE [dnt_trendstat]
GO

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