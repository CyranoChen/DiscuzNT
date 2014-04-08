if exists (select * from sysobjects where id = object_id(N'[dnt_goods]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goods]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodsattachments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodsattachments]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodscategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodscategories]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodscreditrules]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodscreditrules]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodsleavewords]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodsleavewords]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodsrates]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodsrates]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodstags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodstags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodstradelogs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodstradelogs]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_goodsusercredits]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_goodsusercredits]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_locations]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_locations]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_shopcategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_shopcategories]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_shoplinks]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_shoplinks]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_shops]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_shops]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_shopthemes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_shopthemes]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getgoodscount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getgoodscount]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getgoodscountByCid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getgoodscountByCid]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getgoodslist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getgoodslist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getgoodslistByCid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getgoodslistByCid]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_creategoodstags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_creategoodstags]
;

