if exists (select * from sysobjects where id = object_id(N'[catchsoftstatics]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [catchsoftstatics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_locations]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_locations]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_paymentlog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_paymentlog]
;


if exists (select * from sysobjects where id = object_id(N'[dnt_admingroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_admingroups]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_adminvisitlog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_adminvisitlog]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_advertisements]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_advertisements]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_announcements]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_announcements]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_attachments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_attachments]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_attachtypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_attachtypes]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_bbcodes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_bbcodes]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_bonuslog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_bonuslog]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_creditslog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_creditslog]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_debatediggs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_debatediggs]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_debates]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_debates]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_failedlogins]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_failedlogins]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_favorites]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_favorites]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_help]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_help]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_medals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_medals]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_medalslog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_medalslog]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_moderatormanagelog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_moderatormanagelog]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_moderators]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_moderators]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_myattachments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_myattachments]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_myposts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_myposts]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_mytopics]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_mytopics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_online]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_online]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_onlinelist]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_onlinelist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_onlinetime]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_onlinetime]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_pms]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_pms]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_polloptions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)drop table [dnt_polloptions]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_polls]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_polls]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_postdebatefields]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_postdebatefields]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_postid]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_postid]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_posts1]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_posts1]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_ratelog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_ratelog]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_scheduledevents]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_scheduledevents]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_searchcaches]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_searchcaches]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_smilies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_smilies]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_statistics]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_statistics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_stats]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_stats]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_statvars]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_statvars]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_tablelist]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_tablelist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_tags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_tags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_templates]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_templates]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_topicidentify]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_topicidentify]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_topictagcaches]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_topictagcaches]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_topics]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_topics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_topictags]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_topictags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_topictypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_topictypes]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_userfields]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_userfields]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_usergroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_usergroups]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_users]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_words]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_words]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_forums]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_forums]
;


if exists (select * from sysobjects where id = object_id(N'[dnt_forumfields]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_forumfields]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_forumlinks]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_forumlinks]
;


if exists (select * from sysobjects where id = object_id(N'[dnt_checkemailandsecques]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_checkemailandsecques]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_checkpasswordandsecques]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_checkpasswordandsecques]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_checkpasswordbyuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_checkpasswordbyuid]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_checkpasswordbyusername]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_checkpasswordbyusername]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createadmingroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createadmingroup]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createattachment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createattachment]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createfavorite]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createfavorite]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createpm]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createpm]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createpost1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createpost1]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createsearchcache]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createsearchcache]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createtopic]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createtopic]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createuser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createuser]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletepost1bypid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletepost1bypid]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deleteps]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deleteps]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletespace]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletespace]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletetopicbytidlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletetopicbytidlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getadmintopiclist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getadmintopiclist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getalbumlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getalbumlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getalltopiccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getalltopiccount]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getfavoritescount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getfavoritescount]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getfavoritescountbytype]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getfavoritescountbytype]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getfavoriteslist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getfavoriteslist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getfavoriteslistbyalbum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getfavoriteslistbyalbum]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getfavoriteslistbyspacepost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getfavoriteslistbyspacepost]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getfirstpost1id]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getfirstpost1id]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getforumnewtopics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getforumnewtopics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getlastpostlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getlastpostlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getmyposts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getmyposts]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getmytopics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getmytopics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getnewtopics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getnewtopics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getphotolist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getphotolist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpmcount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpmcount]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpmlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpmlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpoll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpoll]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpost1count]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpost1count]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpost1tree]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpost1tree]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpostcountbycondition]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpostcountbycondition]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpostlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpostlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getpostlistbycondition]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getpostlistbycondition]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getshortuserinfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getshortuserinfo]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getsinglepost1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getsinglepost1]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getsitemapnewtopics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getsitemapnewtopics]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiccount]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiccountbycondition]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiccountbycondition]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiccountbytype]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiccountbytype]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiclist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiclist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiclistbydate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiclistbydate]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiclistbynumber]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiclistbynumber]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiclistbytype]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiclistbytype]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiclistbytypedate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiclistbytypedate]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettoptopiclist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettoptopiclist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getuserinfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getuserinfo]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getuserlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getuserlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_shrinklog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_shrinklog]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updateadmingroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateadmingroup]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updatepost1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updatepost1]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updatetopic]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updatetopic]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updatetopicviewcount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updatetopicviewcount]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updateuserauthstr]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateuserauthstr]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updateuserforumsetting]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateuserforumsetting]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updateuserpassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateuserpassword]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updateuserpreference]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateuserpreference]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updateuserprofile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updateuserprofile]
;


if exists (select * from sysobjects where id = object_id(N'[dnt_createdebatepostexpand]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createdebatepostexpand]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createphototags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createphototags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createspaceposttags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createspaceposttags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createtags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createtags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_createtopictags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_createtopictags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletephototags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletephototags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletespaceposttags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletespaceposttags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_deletetopictags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_deletetopictags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getdebatepostlist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getdebatepostlist]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getlastexecutescheduledeventdatetime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getlastexecutescheduledeventdatetime]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getmyattachments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getmyattachments]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_getmyattachmentsbytype]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_getmyattachmentsbytype]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_gettopiclistbytag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_gettopiclistbytag]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_neatenrelatetopic]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_neatenrelatetopic]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_revisedebatetopicdiggs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_revisedebatetopicdiggs]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_setlastexecutescheduledeventdatetime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_setlastexecutescheduledeventdatetime]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_updategoodstags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dnt_updategoodstags]
;

if exists (select * from sysobjects where id = object_id(N'[dnt_navs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_navs]
;


if exists (select * from sysobjects where id = object_id(N'[dnt_notices]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_notices]
;

IF OBJECT_ID (N'dnt_split') IS NOT NULL
DROP FUNCTION dnt_split
;

if exists (select * from sysobjects where id = object_id(N'[dnt_banned]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dnt_banned]
;