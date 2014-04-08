CREATE TABLE [dnt_spaceattachments] (
	[aid] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_spaceattachments_uid] DEFAULT (0),
	[spacepostid] [int] NOT NULL CONSTRAINT [DF_dnt_spaceattachments_spacepostid] DEFAULT (0),
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_spaceattachments_postdatetime] DEFAULT (getdate()),
	[filename] [nchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spaceattachments_filename] DEFAULT (''),
	[filetype] [nchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spaceattachments_filetype] DEFAULT (''),
	[filesize] [int] NOT NULL CONSTRAINT [DF_dnt_spaceattachments_filesize] DEFAULT (0),
	[attachment] [nchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spaceattachments_attachment] DEFAULT (''),
	[downloads] [int] NOT NULL CONSTRAINT [DF_dnt_spaceattachments_downloads] DEFAULT (0),
	CONSTRAINT [PK_dnt_spaceattachments] PRIMARY KEY  CLUSTERED 
	(
		[aid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacecategories] (
	[categoryid] [int] IDENTITY (1, 1) NOT NULL ,
	[title] [nvarchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spacecategory_title] DEFAULT (''),
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_spacecategory_uid] DEFAULT (0),
	[description] [nvarchar] (1000)  NOT NULL CONSTRAINT [DF_dnt_spacecategorys_description] DEFAULT (''),
	[typeid] [int] NOT NULL CONSTRAINT [DF_dnt_spacecategory_typeid] DEFAULT (0),
	[categorycount] [int] NOT NULL CONSTRAINT [DF_dnt_spacecategory_categorycount] DEFAULT (0),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_spacecategory_displayorder] DEFAULT (0),
	CONSTRAINT [PK_dnt_spacecategory] PRIMARY KEY  CLUSTERED 
	(
		[categoryid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacecomments] (
	[commentid] [int] IDENTITY (1, 1) NOT NULL ,
	[postid] [int] NOT NULL CONSTRAINT [DF_dnt_spacecomments_postid] DEFAULT (0),
	[author] [nvarchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spacecomments_author] DEFAULT (''),
	[email] [nvarchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spacecomments_email] DEFAULT (''),
	[url] [nvarchar] (255)  NOT NULL CONSTRAINT [DF_dnt_spacecomments_url] DEFAULT (''),
	[ip] [varchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spacecomments_ip] DEFAULT (''),
	[postdatetime] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_spacecomments_postdatetime] DEFAULT (getdate()),
	[content] [nvarchar] (2000)  NOT NULL CONSTRAINT [DF_dnt_spacecomments_content] DEFAULT (''),
	[parentid] [int] NOT NULL ,
	[uid] [int] NOT NULL ,
	[posttitle] [nvarchar] (60)  NOT NULL ,
	CONSTRAINT [PK_dnt_spacecomments] PRIMARY KEY  CLUSTERED 
	(
		[commentid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spaceconfigs] (
	[spaceid] [int] IDENTITY (1, 1) NOT NULL ,
	[userid] [int] NOT NULL ,
	[spacetitle] [nchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spaceconfig_spacetitle] DEFAULT (''),
	[description] [nchar] (200)  NOT NULL CONSTRAINT [DF_dnt_spaceconfig_description] DEFAULT (''),
	[blogdispmode] [tinyint] NOT NULL CONSTRAINT [DF_dnt_spaceconfig_blogdispmode] DEFAULT (0),
	[bpp] [int] NOT NULL CONSTRAINT [DF_dnt_spaceconfig_bpp] DEFAULT (16),
	[commentpref] [tinyint] NOT NULL CONSTRAINT [DF_dnt_spaceconfig_commentpref] DEFAULT (0),
	[messagepref] [tinyint] NOT NULL CONSTRAINT [DF_dnt_spaceconfig_messagepref] DEFAULT (0),
	[rewritename] [char] (100)  NOT NULL CONSTRAINT [DF_dnt_spaceconfig_rewritename] DEFAULT (''),
	[themeid] [int] NOT NULL CONSTRAINT [DF_dnt_spaceconfig_theme] DEFAULT (0),
	[themepath] [nchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spaceconfig_template] DEFAULT (''),
	[postcount] [int] NOT NULL CONSTRAINT [DF_dnt_spaceconfigs_postcount] DEFAULT (0),
	[commentcount] [int] NOT NULL CONSTRAINT [DF_dnt_spaceconfigs_commentcount] DEFAULT (0),
	[visitedtimes] [int] NOT NULL CONSTRAINT [DF_dnt_spaceconfigs_visitedtimes] DEFAULT (0),
	[createdatetime] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_spaceconfigs_createdatetime] DEFAULT (getdate()),
	[updatedatetime] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_spaceconfigs_updatedatetime] DEFAULT (getdate()),
	[defaulttab] [int] NOT NULL CONSTRAINT [DF_dnt_spaceconfigs_defaulttab] DEFAULT (0),
	[status] [int] NOT NULL CONSTRAINT [DF_dnt_spaceconfigs_status] DEFAULT (0),
	CONSTRAINT [PK_dnt_spaceconfig] PRIMARY KEY  CLUSTERED 
	(
		[spaceid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacecustomizepanels] (
	[moduleid] [int] NOT NULL ,
	[userid] [int] NOT NULL ,
	[panelcontent] [ntext]  NOT NULL CONSTRAINT [DF_dnt_spacecustomizepanels_panelcontent] DEFAULT (''),
	CONSTRAINT [PK_dnt_spacecustomizepanels] PRIMARY KEY  CLUSTERED 
	(
		[moduleid],
		[userid]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dnt_spacelinks] (
	[linkid] [int] IDENTITY (1, 1) NOT NULL ,
	[userid] [int] NOT NULL ,
	[linktitle] [nvarchar] (50)  NOT NULL ,
	[linkurl] [varchar] (255)  NOT NULL ,
	[description] [nvarchar] (200)  NULL ,
	CONSTRAINT [PK_dnt_spacelink] PRIMARY KEY  CLUSTERED 
	(
		[linkid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacemoduledefs] (
	[moduledefid] [int] IDENTITY (1, 1) NOT NULL ,
	[modulename] [nvarchar] (20)  NOT NULL ,
	[cachetime] [int] NOT NULL ,
	[configfile] [varchar] (100)  NOT NULL ,
	[controller] [varchar] (255)  NULL ,
	CONSTRAINT [PK_dnt_spacemoduledefs] PRIMARY KEY  CLUSTERED 
	(
		[moduledefid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacemodules] (
	[moduleid] [int] NOT NULL ,
	[tabid] [int] NOT NULL ,
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_spacemodules_uid] DEFAULT (0),
	[moduledefid] [int] NOT NULL ,
	[panename] [varchar] (10)  NOT NULL ,
	[displayorder] [int] NOT NULL ,
	[userpref] [nvarchar] (4000)  NULL ,
	[val] [tinyint] NOT NULL ,
	[moduleurl] [varchar] (255)  NOT NULL ,
	[moduletype] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacepostcategories] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[postid] [int] NOT NULL CONSTRAINT [DF_dnt_spacepostcategorys_postid] DEFAULT (0),
	[categoryid] [int] NOT NULL CONSTRAINT [DF_dnt_spacepostcategorys_categoryid] DEFAULT (0),
	CONSTRAINT [PK_dnt_spacepostcategorys] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spaceposts] (
	[postid] [int] IDENTITY (1, 1) NOT NULL ,
	[author] [nvarchar] (40)  NOT NULL ,
	[uid] [int] NOT NULL ,
	[postdatetime] [datetime] NOT NULL ,
	[content] [ntext]  NOT NULL ,
	[title] [nvarchar] (120)  NOT NULL ,
	[category] [varchar] (255)  NOT NULL ,
	[poststatus] [tinyint] NOT NULL ,
	[commentstatus] [tinyint] NOT NULL ,
	[postupdatetime] [datetime] NOT NULL ,
	[commentcount] [int] NOT NULL ,
	[views] [int] NOT NULL CONSTRAINT [DF_dnt_spaceposts_views] DEFAULT (0),
	CONSTRAINT [PK_dnt_spaceposts] PRIMARY KEY  CLUSTERED 
	(
		[postid]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dnt_spaceposttags] (
	[tagid] [int] NOT NULL CONSTRAINT [DF_dnt_spaceposttags_tagid] DEFAULT (0),
	[spacepostid] [int] NOT NULL CONSTRAINT [DF_dnt_spaceposttags_spacepostid] DEFAULT (0)
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacetabs] (
	[tabid] [int] NOT NULL ,
	[uid] [int] NOT NULL ,
	[displayorder] [int] NOT NULL ,
	[tabname] [nvarchar] (50)  NOT NULL ,
	[iconfile] [varchar] (50)  NULL ,
	[template] [varchar] (50)  NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_spacethemes] (
	[themeid] [int] IDENTITY (1, 1) NOT NULL ,
	[directory] [varchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spacetheme_directory] DEFAULT (''),
	[name] [nvarchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spacetheme_name] DEFAULT (''),
	[type] [int] NOT NULL CONSTRAINT [DF_dnt_spacetheme_type] DEFAULT (0),
	[author] [nvarchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spacetheme_author] DEFAULT (''),
	[createdate] [nvarchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spacetheme_createdate] DEFAULT (''),
	[copyright] [nvarchar] (100)  NOT NULL CONSTRAINT [DF_dnt_spacetheme_copyright] DEFAULT (''),
	CONSTRAINT [PK_dnt_spacethemes] PRIMARY KEY  CLUSTERED 
	(
		[themeid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO


 CREATE  INDEX [tid] ON [dnt_spaceattachments]([spacepostid]) ON [PRIMARY]
GO

 CREATE  INDEX [uid] ON [dnt_spaceattachments]([uid]) ON [PRIMARY]
GO

 CREATE  INDEX [spacepostiduserid] ON [dnt_spaceattachments]([spacepostid], [uid]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [userid] ON [dnt_spacecategories]([uid], [displayorder], [categoryid]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [userid] ON [dnt_spaceconfigs]([userid]) ON [PRIMARY]
GO

 CREATE  INDEX [linkiduserid] ON [dnt_spacelinks]([linkid], [userid]) ON [PRIMARY]
GO

 CREATE  INDEX [configfile] ON [dnt_spacemoduledefs]([configfile]) ON [PRIMARY]
GO

 CREATE  INDEX [postiduid] ON [dnt_spaceposts]([postid], [uid]) ON [PRIMARY]
GO

 CREATE  INDEX [postidcommentcount] ON [dnt_spaceposts]([postid], [commentcount]) ON [PRIMARY]
GO

 CREATE  INDEX [uid] ON [dnt_spacetabs]([uid]) ON [PRIMARY]
GO

 CREATE  INDEX [type] ON [dnt_spacethemes]([type]) ON [PRIMARY]