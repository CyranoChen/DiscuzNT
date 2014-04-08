CREATE TABLE [dnt_goods] (
	[goodsid] [int] IDENTITY (1, 1) NOT NULL ,
	[shopid] [int] NOT NULL CONSTRAINT [DF_dnt_goods_shopid] DEFAULT (0),
	[categoryid] [int] NOT NULL CONSTRAINT [DF_dnt_goods_usergoodstypeid] DEFAULT (0),
	[parentcategorylist] [char] (300)  NOT NULL CONSTRAINT [DF_dnt_goods_parentidlist] DEFAULT (''),
	[shopcategorylist] [char] (300)  NOT NULL CONSTRAINT [DF_dnt_goods_shopcategorylist] DEFAULT (','),
	[recommend] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goods_recommend] DEFAULT (0),
	[discount] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goods_discount] DEFAULT (0),
	[selleruid] [int] NOT NULL CONSTRAINT [DF_dnt_goods_selleruid] DEFAULT (0),
	[seller] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_goods_seller] DEFAULT (''),
	[account] [nchar] (50)  NOT NULL ,
	[title] [nchar] (60)  NOT NULL ,
	[magic] [int] NOT NULL CONSTRAINT [DF_dnt_goods_magic] DEFAULT (0),
	[price] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_goods_price] DEFAULT (0),
	[amount] [smallint] NOT NULL CONSTRAINT [DF_dnt_goods_amount] DEFAULT (0),
	[quality] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goods_quality] DEFAULT (1),
	[lid] [int] NOT NULL ,
	[locus] [nchar] (20)  NOT NULL ,
	[transport] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goods_transport] DEFAULT (0),
	[ordinaryfee] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_goods_ordinaryfee] DEFAULT (0),
	[expressfee] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_goods_expressfee] DEFAULT (0),
	[emsfee] [decimal](18, 2) NOT NULL ,
	[itemtype] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goods_itemtype] DEFAULT (0),
	[dateline] [datetime] NOT NULL CONSTRAINT [DF_dnt_goods_dateline] DEFAULT (getdate()),
	[expiration] [datetime] NOT NULL ,
	[lastbuyer] [nchar] (10)  NOT NULL CONSTRAINT [DF_dnt_goods_lastbuyer] DEFAULT (''),
	[lasttrade] [datetime] NOT NULL ,
	[lastupdate] [datetime] NOT NULL CONSTRAINT [DF_dnt_goods_lastupdate] DEFAULT (getdate()),
	[totalitems] [smallint] NOT NULL ,
	[tradesum] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_goods_tradesum] DEFAULT (0),
	[closed] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goods_closed] DEFAULT (0),
	[aid] [int] NOT NULL ,
	[goodspic] [nchar] (100)  NOT NULL CONSTRAINT [DF_dnt_goods_goodspic] DEFAULT (''),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_goods_displayorder] DEFAULT (0),
	[costprice] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_goods_costprice] DEFAULT (0),
	[invoice] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goods_invoice] DEFAULT (0),
	[repair] [smallint] NOT NULL CONSTRAINT [DF_dnt_goods_repair] DEFAULT (0),
	[message] [ntext]  NOT NULL ,
	[otherlink] [nchar] (250)  NOT NULL ,
	[readperm] [int] NOT NULL ,
	[tradetype] [tinyint] NOT NULL ,
	[viewcount] [int] NOT NULL CONSTRAINT [DF_dnt_goods_viewcount] DEFAULT (0),
	[invisible] [int] NOT NULL CONSTRAINT [DF_dnt_goods_invisible] DEFAULT (0),
	[smileyoff] [int] NOT NULL ,
	[bbcodeoff] [int] NOT NULL CONSTRAINT [DF_dnt_goods_bbcodeoff] DEFAULT (0),
	[parseurloff] [int] NOT NULL ,
	[highlight] [varchar] (500)  NOT NULL CONSTRAINT [DF_dnt_goods_highlight] DEFAULT ('')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dnt_goodsattachments] (
	[aid] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsattachments_uid] DEFAULT (0),
	[goodsid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsattachments_goodsid] DEFAULT (0),
	[categoryid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsattachments_categoryid] DEFAULT (0),
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_goodsattachments_postdatetime] DEFAULT (getdate()),
	[readperm] [int] NOT NULL CONSTRAINT [DF_dnt_goodsattachments_readperm] DEFAULT (0),
	[filename] [nchar] (100) NOT NULL CONSTRAINT [DF_dnt_goodsattachments_filename] DEFAULT (''),
	[description] [nchar] (100) NOT NULL CONSTRAINT [DF_dnt_goodsattachments_description] DEFAULT (''),
	[filetype] [nchar] (50) NOT NULL CONSTRAINT [DF_dnt_goodsattachments_filetype] DEFAULT (''),
	[filesize] [int] NOT NULL CONSTRAINT [DF_dnt_goodsattachments_filesize] DEFAULT (0),
	[attachment] [nchar] (100) NOT NULL CONSTRAINT [DF_dnt_goodsattachments_attachment] DEFAULT ('')
) ON [PRIMARY]
GO

CREATE TABLE [dnt_goodscategories] (
	[categoryid] [int] IDENTITY (1, 1) NOT NULL ,
	[parentid] [int] NOT NULL CONSTRAINT [DF_dnt_goodscategories_parentid] DEFAULT (0),
	[layer] [smallint] NOT NULL CONSTRAINT [DF_dnt_goodscategories_layer] DEFAULT (0),
	[parentidlist] [char] (300)  NOT NULL CONSTRAINT [DF_dnt_goodscategories_parentidlist] DEFAULT (''),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_goodscategories_displayorder] DEFAULT (0),
	[categoryname] [nchar] (50)  NOT NULL CONSTRAINT [DF_dnt_goodscategories_categoryname] DEFAULT (''),
	[haschild] [bit] NOT NULL CONSTRAINT [DF_dnt_goodscategories_haschild] DEFAULT (0),
	[fid] [int] NOT NULL CONSTRAINT [DF_dnt_goodscategories_fid] DEFAULT (0),
	[pathlist] [nchar] (3000)  NOT NULL CONSTRAINT [DF_dnt_goodscategories_pathlist] DEFAULT (''),
	[goodscount] [int] NOT NULL CONSTRAINT [DF_dnt_goodscategories_goodscount] DEFAULT (0),
	CONSTRAINT [PK_dnt_goodscategories] PRIMARY KEY  CLUSTERED 
	(
		[categoryid]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_goodscreditrules] (
	[id] [int] NOT NULL ,
	[lowerlimit] [int] NOT NULL CONSTRAINT [DF_Table_1_buyercredit] DEFAULT (0),
	[upperlimit] [int] NOT NULL CONSTRAINT [DF_Table_1_sellercredit] DEFAULT (0),
	[sellericon] [varchar] (20)  NOT NULL CONSTRAINT [DF_dnt_goodscreditrules_sellericon] DEFAULT (''),
	[buyericon] [varchar] (20)  NOT NULL CONSTRAINT [DF_dnt_goodscreditrules_buyericon] DEFAULT ('')
) ON [PRIMARY]
GO

CREATE TABLE [dnt_goodsleavewords] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[goodsid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_goodsid] DEFAULT (0),
	[tradelogid] [int] NOT NULL ,
	[isbuyer] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_isbuyer] DEFAULT (0),
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_uid] DEFAULT (0),
	[username] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_username] DEFAULT (''),
	[message] [nchar] (200)  NOT NULL ,
	[invisible] [int] NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_invisible] DEFAULT (0),
	[ip] [nvarchar] (15)  NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_ip] DEFAULT (''),
	[usesig] [int] NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_usesig] DEFAULT (0),
	[htmlon] [int] NOT NULL ,
	[smileyoff] [int] NOT NULL ,
	[parseurloff] [int] NOT NULL ,
	[bbcodeoff] [int] NOT NULL ,
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_goodsleaveword_postdatetime] DEFAULT (getdate())
) ON [PRIMARY]
GO

CREATE TABLE [dnt_goodsrates] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[goodstradelogid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsrates_goodstradelogid] DEFAULT (0),
	[message] [nchar] (200)  NOT NULL CONSTRAINT [DF_dnt_goodsrates_message] DEFAULT (''),
	[explain] [nchar] (200)  NOT NULL CONSTRAINT [DF_dnt_goodsrates_explain] DEFAULT (''),
	[ip] [nvarchar] (15)  NOT NULL CONSTRAINT [DF_dnt_goodsrates_ip] DEFAULT (''),
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsrates_uid] DEFAULT (0),
	[uidtype] [tinyint] NOT NULL ,
	[username] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_goodsrates_username] DEFAULT (''),
	[ratetouid] [int] NOT NULL CONSTRAINT [DF_dnt_goodsrates_ratetouid] DEFAULT (0),
	[ratetousername] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_goodsrates_ratetoname] DEFAULT (''),
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_goodsrates_postdatetime] DEFAULT (getdate()),
	[goodsid] [int] NOT NULL ,
	[goodstitle] [nchar] (60)  NOT NULL CONSTRAINT [DF_dnt_goodsrates_goodstitle] DEFAULT (''),
	[price] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_goodsrates_price] DEFAULT (0),
	[ratetype] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goodsrates_ratetype] DEFAULT (0)
) ON [PRIMARY]
GO


CREATE TABLE [dnt_goodstags] (
	[tagid] [int] NOT NULL ,
	[goodsid] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dnt_goodstradelogs] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[goodsid] [int] NOT NULL CONSTRAINT [DF_dnt_tradelog_goodsid] DEFAULT (0),
	[orderid] [varchar] (50)  NOT NULL CONSTRAINT [DF_dnt_tradelog_orderid] DEFAULT (''),
	[tradeno] [varchar] (50)  NOT NULL CONSTRAINT [DF_dnt_tradelog_tradeno] DEFAULT (''),
	[subject] [nchar] (60)  NOT NULL CONSTRAINT [DF_dnt_tradelog_subject] DEFAULT (''),
	[price] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_tradelog_price] DEFAULT (0),
	[quality] [tinyint] NOT NULL ,
	[categoryid] [int] NOT NULL CONSTRAINT [DF_dnt_tradelog_categoryid] DEFAULT (0),
	[number] [smallint] NOT NULL CONSTRAINT [DF_dnt_tradelog_number] DEFAULT (0),
	[tax] [decimal](18, 2) NOT NULL ,
	[locus] [varchar] (50)  NOT NULL CONSTRAINT [DF_dnt_tradelog_locus] DEFAULT (''),
	[sellerid] [int] NOT NULL CONSTRAINT [DF_dnt_tradelog_sellerid] DEFAULT (0),
	[seller] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_tradelog_seller] DEFAULT (''),
	[selleraccount] [varchar] (50)  NOT NULL ,
	[buyerid] [int] NOT NULL CONSTRAINT [DF_dnt_tradelog_buyerid] DEFAULT (0),
	[buyer] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_tradelog_buyer] DEFAULT (''),
	[buyercontact] [nchar] (100)  NOT NULL CONSTRAINT [DF_dnt_tradelog_buyercontact] DEFAULT (''),
	[buyercredit] [smallint] NOT NULL CONSTRAINT [DF_dnt_tradelog_buyercredits] DEFAULT (0),
	[buyermsg] [nchar] (100)  NOT NULL CONSTRAINT [DF_dnt_tradelog_buyermsg] DEFAULT (''),
	[status] [tinyint] NOT NULL CONSTRAINT [DF_dnt_tradelog_status] DEFAULT (0),
	[lastupdate] [datetime] NOT NULL CONSTRAINT [DF_dnt_tradelog_lastupdate] DEFAULT (getdate()),
	[offline] [tinyint] NOT NULL CONSTRAINT [DF_dnt_tradelog_offline] DEFAULT (0),
	[buyername] [nchar] (20)  NOT NULL CONSTRAINT [DF_dnt_tradelog_buyername] DEFAULT (''),
	[buyerzip] [varchar] (50)  NOT NULL CONSTRAINT [DF_dnt_tradelog_buyerzip] DEFAULT (''),
	[buyerphone] [varchar] (50)  NOT NULL ,
	[buyermobile] [varchar] (50)  NOT NULL CONSTRAINT [DF_dnt_tradelog_buyermobile] DEFAULT (''),
	[transport] [tinyint] NOT NULL CONSTRAINT [DF_dnt_tradelog_transport] DEFAULT (0),
	[transportfee] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_tradelog_transportfee] DEFAULT (0),
	[transportpay] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goodstradelogs_transportpay] DEFAULT (1),
	[tradesum] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_goodstradelogs_tradesum] DEFAULT (0),
	[baseprice] [decimal](18, 2) NOT NULL CONSTRAINT [DF_dnt_tradelog_baseprice] DEFAULT (0),
	[discount] [tinyint] NOT NULL CONSTRAINT [DF_dnt_tradelog_discount] DEFAULT (0),
	[ratestatus] [tinyint] NOT NULL CONSTRAINT [DF_dnt_tradelog_ratestatus] DEFAULT (0),
	[message] [ntext]  NOT NULL CONSTRAINT [DF_dnt_tradelog_message] DEFAULT ('')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dnt_goodsusercredits] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_goodscredits_uid] DEFAULT (0),
	[oneweek] [int] NOT NULL CONSTRAINT [DF_dnt_goodscredits_oneweek] DEFAULT (0),
	[onemonth] [int] NOT NULL CONSTRAINT [DF_dnt_goodscredits_onemonth] DEFAULT (0),
	[sixmonth] [int] NOT NULL CONSTRAINT [DF_dnt_goodscredits_sixmonth] DEFAULT (0),
	[sixmonthago] [int] NOT NULL CONSTRAINT [DF_dnt_goodscredits_sixmonthago] DEFAULT (0),
	[ratefrom] [tinyint] NOT NULL ,
	[ratetype] [tinyint] NOT NULL CONSTRAINT [DF_dnt_goodscredits_ratetype] DEFAULT (0)
) ON [PRIMARY]
GO

CREATE TABLE [dnt_locations] (
	[lid] [int] IDENTITY (1, 1) NOT NULL ,
	[city] [nvarchar] (50)  NOT NULL CONSTRAINT [DF_dnt_locations_city] DEFAULT (''),
	[state] [nvarchar] (50)  NOT NULL CONSTRAINT [DF_dnt_locations_state] DEFAULT (''),
	[country] [nvarchar] (50)  NOT NULL CONSTRAINT [DF_dnt_locations_country] DEFAULT (''),
	[zipcode] [nvarchar] (20)  NOT NULL CONSTRAINT [DF_dnt_locations_zipcode] DEFAULT ('')
) ON [PRIMARY]

CREATE TABLE [dnt_shopthemes] (
	[themeid] [int] IDENTITY (1, 1) NOT NULL ,
	[directory] [varchar] (100) NOT NULL CONSTRAINT [DF_dnt_shopthemes_directory] DEFAULT (''),
	[name] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_shopthemes_name] DEFAULT (''),
	[author] [nvarchar] (100) NOT NULL CONSTRAINT [DF_dnt_shopthemes_author] DEFAULT (''),
	[createdate] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_shopthemes_createdate] DEFAULT (''),
	[copyright] [nvarchar] (100) NOT NULL CONSTRAINT [DF_dnt_shopthemes_copyright] DEFAULT ('')
) ON [PRIMARY]
GO

CREATE TABLE [dnt_shops] (
	[shopid] [int] IDENTITY (1, 1) NOT NULL ,
	[logo] [nvarchar] (100) NOT NULL ,
	[shopname] [nvarchar] (50) NOT NULL CONSTRAINT [DF_dnt_shops_shopname] DEFAULT (''),
	[categoryid] [int] NOT NULL CONSTRAINT [DF_dnt_shops_categoryid] DEFAULT (0),
	[themeid] [int] NOT NULL CONSTRAINT [DF_dnt_shops_themeid] DEFAULT (0),
	[themepath] [nchar] (50) NOT NULL CONSTRAINT [DF_dnt_shops_themepath] DEFAULT (''),
	[uid] [int] NOT NULL CONSTRAINT [DF_dnt_shops_uid] DEFAULT (0),
	[username] [nchar] (20) NOT NULL CONSTRAINT [DF_dnt_shops_username] DEFAULT (''),
	[introduce] [nvarchar] (500) NOT NULL CONSTRAINT [DF_dnt_shops_introduce] DEFAULT (''),
	[lid] [int] NOT NULL CONSTRAINT [DF_dnt_shops_lid] DEFAULT (0),
	[locus] [nchar] (20) NOT NULL ,
	[bulletin] [nvarchar] (500) NOT NULL CONSTRAINT [DF_dnt_shops_announcement] DEFAULT (''),
	[createdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_shops_createdatetime] DEFAULT (getdate()),
	[invisible] [int] NOT NULL ,
	[viewcount] [int] NOT NULL CONSTRAINT [DF_dnt_shops_viewcount] DEFAULT (0)
) ON [PRIMARY]
GO

CREATE TABLE [dnt_shoplinks] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[shopid] [int] NOT NULL CONSTRAINT [DF_dnt_shoplinks_shopid] DEFAULT (0),
	[displayorder] [int] NOT NULL ,
	[name] [nvarchar] (100) NOT NULL ,
	[linkshopid] [int] NOT NULL CONSTRAINT [DF_dnt_shoplinks_linkshopid] DEFAULT (0)
) ON [PRIMARY]
GO

CREATE TABLE [dnt_shopcategories] (
	[categoryid] [int] IDENTITY (1, 1) NOT NULL ,
	[parentid] [int] NOT NULL CONSTRAINT [DF_dnt_shopcategories_parentcid] DEFAULT (0),
	[parentidlist] [char] (300) NOT NULL CONSTRAINT [DF_dnt_shopcategories_parentidlist] DEFAULT (0),
	[layer] [int] NOT NULL CONSTRAINT [DF_dnt_shopcategories_layer] DEFAULT (0),
	[childcount] [int] NOT NULL CONSTRAINT [DF_dnt_shopcategories_childcount] DEFAULT (0),
	[syscategoryid] [int] NOT NULL ,
	[name] [nchar] (50) NOT NULL CONSTRAINT [DF_dnt_shopcategories_name] DEFAULT (''),
	[categorypic] [nvarchar] (100) NOT NULL CONSTRAINT [DF_dnt_shopcategories_categorypic] DEFAULT (''),
	[shopid] [int] NOT NULL CONSTRAINT [DF_dnt_shopcategories_shopid] DEFAULT (0),
	[displayorder] [int] NOT NULL CONSTRAINT [DF_dnt_shopcategories_displayorder] DEFAULT (0)
) ON [PRIMARY]
