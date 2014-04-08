CREATE TABLE [dnt_albums] (
	[albumid] [int] IDENTITY (1, 1) NOT NULL ,
	[albumcateid] [int] NOT NULL ,
	[userid] [int] NOT NULL CONSTRAINT [DF_dnt_spacealbums_userid] DEFAULT ((-1)),
	[username] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_albums_username] DEFAULT (''),
	[title] [nchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spacealbums_title] DEFAULT (''),
	[description] [nchar] (200)  NOT NULL CONSTRAINT [DF_dnt_spacealbums_description] DEFAULT (''),
	[logo] [nchar] (255)  NOT NULL CONSTRAINT [DF_dnt_spacealbums_logo] DEFAULT (''),
	[password] [nchar] (50)  NOT NULL CONSTRAINT [DF_dnt_spacealbums_password] DEFAULT (''),
	[imgcount] [int] NOT NULL CONSTRAINT [DF_dnt_spacealbums_imgcount] DEFAULT (0),
	[views] [int] NOT NULL CONSTRAINT [DF_dnt_spacealbums_views] DEFAULT (0),
	[type] [int] NOT NULL CONSTRAINT [DF_dnt_spacealbums_type] DEFAULT (0),
	[createdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_spacealbums_createdatetime] DEFAULT (getdate()),
	CONSTRAINT [PK_dnt_spacealbums] PRIMARY KEY  CLUSTERED 
	(
		[albumid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO


CREATE TABLE [dnt_albumcategories] (
	[albumcateid] [int] IDENTITY (1, 1) NOT NULL ,
	[title] [nchar] (50)  NOT NULL ,
	[description] [nchar] (300)  NULL ,
	[albumcount] [int] NOT NULL CONSTRAINT [DF_dnt_albumcategories_albumcount] DEFAULT (0),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_albumcategories_displayorder] DEFAULT (0),
	CONSTRAINT [PK_dnt_albumcategories] PRIMARY KEY  CLUSTERED 
	(
		[albumcateid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_photos] (
	[photoid] [int] IDENTITY (1, 1) NOT NULL ,
	[filename] [char] (255)  NOT NULL ,
	[attachment] [nchar] (255)  NOT NULL ,
	[filesize] [int] NOT NULL ,
	[title] [nchar] (20)  NOT NULL ,
	[description] [nchar] (200)  NOT NULL CONSTRAINT [DF_dnt_spacephoto_description] DEFAULT (''),
	[postdate] [datetime] NOT NULL CONSTRAINT [DF_dnt_spacephoto_postdate] DEFAULT (getdate()),
	[albumid] [int] NOT NULL ,
	[userid] [int] NOT NULL ,
	[username] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_photos_username] DEFAULT (''),
	[views] [int] NOT NULL CONSTRAINT [DF_dnt_photos_viewcount] DEFAULT (0),
	[commentstatus] [tinyint] NOT NULL CONSTRAINT [DF_dnt_photos_commentstatus] DEFAULT (0),
	[tagstatus] [tinyint] NOT NULL CONSTRAINT [DF_dnt_photos_tagstatus] DEFAULT (0),
	[comments] [int] NOT NULL CONSTRAINT [DF_dnt_photos_comments] DEFAULT (0),
	[isattachment] [int] NOT NULL CONSTRAINT [DF_dnt_photos_isattachment] DEFAULT (0),
	[width] [int] NULL ,
	[height] [int] NULL ,
	CONSTRAINT [PK_dnt_spacephoto] PRIMARY KEY  CLUSTERED 
	(
		[photoid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_phototags] (
	[tagid] [int] NOT NULL CONSTRAINT [DF_dnt_phototags_tagid] DEFAULT (0),
	[photoid] [int] NOT NULL CONSTRAINT [DF_Table3_pid] DEFAULT (0)
) ON [PRIMARY]
GO

CREATE TABLE [dnt_photocomments] (
	[commentid] [int] IDENTITY (1, 1) NOT NULL ,
	[photoid] [int] NOT NULL ,
	[username] [nvarchar] (20)  NOT NULL ,
	[userid] [int] NOT NULL ,
	[ip] [varchar] (100)  NOT NULL CONSTRAINT [DF_dnt_photocomments_ip] DEFAULT (''),
	[postdatetime] [smalldatetime] NOT NULL ,
	[content] [nvarchar] (2000)  NOT NULL 
) ON [PRIMARY]
GO

 CREATE  INDEX [albumid] ON [dnt_photos]([albumid]) ON [PRIMARY]
GO

 CREATE  INDEX [photoiduserid] ON [dnt_photos]([photoid], [userid]) ON [PRIMARY]
GO

 CREATE  INDEX [userid] ON [dnt_photos]([userid]) ON [PRIMARY]
GO

 CREATE INDEX [list_albumcateid] ON [dnt_albums]([imgcount], [albumcateid], [albumid]) ON [PRIMARY]
GO

 CREATE INDEX [list_userid] ON [dnt_albums]([type], [imgcount], [userid], [albumid]) ON [PRIMARY]