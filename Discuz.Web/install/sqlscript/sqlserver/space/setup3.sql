INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright) VALUES ('', '默认皮肤', 0, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright) VALUES ('', '部落窝', 0, '部落窝', '2007-08-23', 'Copyright?2005-2007 BlogWorld. All rights reserved');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright) VALUES ('default', '默认皮肤', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('fadgirl', '时尚女孩', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('dream', '梦幻', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('patina', '古色古香', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('romantic', '浪漫传奇', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('window', '窗', 2, '部落窝', '2007-08-23', 'Copyright?2005-2007 BlogWorld. All rights reserved');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('orange', '桔色', 2,  '部落窝', '2007-08-23', 'Copyright?2005-2007 BlogWorld. All rights reserved');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('green', '幽静夜空', 2, '部落窝', '2007-08-23', 'Copyright?2005-2007 BlogWorld. All rights reserved');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('bird', '山水麻雀', 2,  '部落窝', '2007-08-23', 'Copyright?2005-2007 BlogWorld. All rights reserved');
INSERT INTO [dnt_spacethemes] (directory,name,type,author,createdate,copyright)VALUES ('promise', '约定一生', 2,  '部落窝', '2007-08-23', 'Copyright?2005-2007 BlogWorld. All rights reserved');




INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('友情链接', 0, 'builtin_linkmodule.xml', 'Discuz.Space.Modules.LinkModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('个人信息', 0, 'builtin_userinfomodule.xml', 'Discuz.Space.Modules.Forum.UserInfoModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('帖子调用', 0, 'builtin_showtopicmodule.xml', 'Discuz.Space.Modules.Forum.ShowTopicModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('日历', 0, 'builtin_calendarmodule.xml', 'Discuz.Space.Modules.CalendarModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('用户菜单', 0, 'builtin_leftmenumodule.xml', 'Discuz.Space.Modules.LeftMenuModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('最新评论', 0, 'builtin_newcommentmodule.xml', 'Discuz.Space.Modules.NewCommentModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('最新文章', 0, 'builtin_newpostmodule.xml', 'Discuz.Space.Modules.NewPostModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('文章列表', 0, 'builtin_postlistmodule.xml', 'Discuz.Space.Modules.PostListModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('便条纸', 0, 'builtin_notepadmodule.xml', 'Discuz.Space.Modules.Notepad, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('数据统计', 0, 'builtin_statisticmodule.xml', 'Discuz.Space.Modules.StatisticModule, Discuz.Space.Modules');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('我的相册', 0, 'builtin_showalbummodule.xml', 'Discuz.Space.Modules.Album.ShowAlbumModule, Discuz.Space.Modules.Album');
INSERT INTO [dnt_spacemoduledefs] (modulename,cachetime,configfile,controller)  VALUES ('自定义面板', 0, 'builtin_customizepanel.xml', 'Discuz.Space.Modules.CustomizePanel, Discuz.Space.Modules');

UPDATE  [dnt_users] SET [spaceid] = 0 WHERE [spaceid]>0;