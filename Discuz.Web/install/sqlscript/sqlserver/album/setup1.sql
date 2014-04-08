if exists (select * from sysobjects where id = object_id(N'[dnt_albums]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_albums]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_albumcategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_albumcategories]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_photos]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_photos]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_phototags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_phototags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_photocomments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_photocomments]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getalbumlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getalbumlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getphotolist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getphotolist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createphototags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createphototags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletephototags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletephototags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getphotolistbytag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getphotolistbytag]
;
