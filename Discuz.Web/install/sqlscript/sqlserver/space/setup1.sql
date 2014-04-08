if exists (select * from sysobjects where id = object_id(N'[dnt_spaceattachments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spaceattachments]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacecategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacecategories]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacecomments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacecomments]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spaceconfigs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spaceconfigs]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacecustomizepanels]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacecustomizepanels]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacelinks]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacelinks]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacemoduledefs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacemoduledefs]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacemodules]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacemodules]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacepostcategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacepostcategories]
;


if exists (select * from sysobjects where id = object_id(N'[dnt_spaceposts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spaceposts]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spaceposttags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spaceposttags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacetabs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacetabs]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_spacethemes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_spacethemes]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletespace]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletespace]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createspaceposttags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createspaceposttags]
;


if exists (select * from sysobjects where id = object_id(N'[dnt_deletespaceposttags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletespaceposttags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getspacepostlistbytag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getspacepostlistbytag]
;
