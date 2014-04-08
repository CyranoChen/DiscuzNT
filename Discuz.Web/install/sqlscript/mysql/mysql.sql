-- --------------------------------------------------
--
-- Discuz!NT SQL file for installation
--
-- --------------------------------------------------

DROP TABLE IF EXISTS `dnt_admingroups`;
CREATE TABLE `dnt_admingroups` (
  `admingid` smallint(6) NOT NULL,
  `alloweditpost` tinyint(3) unsigned NOT NULL default '0',
  `alloweditpoll` tinyint(3) unsigned NOT NULL default '0',
  `allowstickthread` tinyint(3) unsigned NOT NULL default '0',
  `allowmodpost` tinyint(3) unsigned NOT NULL default '0',
  `allowdelpost` tinyint(3) unsigned NOT NULL default '0',
  `allowmassprune` tinyint(3) unsigned NOT NULL default '0',
  `allowrefund` tinyint(3) unsigned NOT NULL default '0',
  `allowcensorword` tinyint(3) unsigned NOT NULL default '0',
  `allowviewip` tinyint(3) unsigned NOT NULL default '0',
  `allowbanip` tinyint(3) unsigned NOT NULL default '0',
  `allowedituser` tinyint(3) unsigned NOT NULL default '0',
  `allowmoduser` tinyint(3) unsigned NOT NULL default '0',
  `allowbanuser` tinyint(3) unsigned NOT NULL default '0',
  `allowpostannounce` tinyint(3) unsigned NOT NULL default '0',
  `allowviewlog` tinyint(3) unsigned NOT NULL default '0',
  `disablepostctrl` tinyint(3) unsigned NOT NULL default '0',
  `allowviewrealname` tinyint(4) NOT NULL default '0'
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

INSERT INTO `dnt_admingroups` (`admingid`, `alloweditpost`, `alloweditpoll`, `allowstickthread`, `allowmodpost`, `allowdelpost`, `allowmassprune`, `allowrefund`, `allowcensorword`, `allowviewip`, `allowbanip`, `allowedituser`, `allowmoduser`, `allowbanuser`, `allowpostannounce`, `allowviewlog`, `disablepostctrl`, `allowviewrealname`) VALUES 
(1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
(2, 1, 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
(3, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1);

DROP TABLE IF EXISTS `dnt_adminvisitlog`;
CREATE TABLE `dnt_adminvisitlog` (
  `visitid` int(11) NOT NULL auto_increment,
  `uid` int(11) default NULL,
  `username` varchar(20) NOT NULL,
  `groupid` int(11) default NULL,
  `grouptitle` varchar(50) NOT NULL,
  `ip` varchar(15) default NULL,
  `postdatetime` datetime default NULL,
  `actions` varchar(100) NOT NULL,
  `others` varchar(200) NOT NULL,
  KEY `visitid` (`visitid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_advertisements`;
CREATE TABLE `dnt_advertisements` (
  `advid` int(11) NOT NULL auto_increment,
  `available` int(11) NOT NULL,
  `type` varchar(50) NOT NULL,
  `displayorder` int(11) NOT NULL,
  `title` varchar(50) NOT NULL,
  `targets` varchar(255) NOT NULL,
  `starttime` date default NULL,
  `endtime` date default NULL,
  `code` longtext,
  `parameters` longtext,
  KEY `advid` (`advid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_albumcategories`;
CREATE TABLE `dnt_albumcategories` (
  `albumcateid` int(11) NOT NULL auto_increment,
  `title` varchar(50) NOT NULL,
  `description` text,
  `albumcount` int(11) NOT NULL default '0',
  `displayorder` int(11) NOT NULL default '0',
  PRIMARY KEY  (`albumcateid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_albumcategories` (`title`, `description`, `albumcount`, `displayorder`) VALUES 
 ('人物', '生活随拍 朋友恋人 偶像明星 帅哥美女 网友自拍', 0, 1),
 ('风景', '自然风景 名胜古迹 建筑雕塑 民俗风情 城市魅影', 0, 2),
 ('动物', '狗 猫 鸟类 鱼类 昆虫', 0, 3),
 ('植物', '花卉 树木', 0, 4),
 ('艺术', '邮票 古玩 绘画 书法 篆刻', 0, 5),
 ('娱乐', '搞笑 游戏 动漫 电影', 0, 6),
 ('体育', '足球 篮球 奥运会 世界杯', 0, 7),
 ('时尚', '汽车 时装 家居 美食 手机', 0, 8),
 ('科技', '天文 地理 海洋 科普 计算机', 0, 9);

DROP TABLE IF EXISTS `dnt_albums`;
CREATE TABLE `dnt_albums` (
  `albumid` int(11) NOT NULL auto_increment,
  `albumcateid` int(11) NOT NULL,
  `userid` int(11) NOT NULL default '-1',
  `username` varchar(20) NOT NULL default '',
  `title` varchar(50) NOT NULL default '',
  `description` varchar(200) NOT NULL default '',
  `logo` varchar(255) NOT NULL default '',
  `password` varchar(50) NOT NULL default '',
  `imgcount` int(11) NOT NULL default '0',
  `views` int(11) NOT NULL default '0',
  `type` int(11) NOT NULL default '0',
  `createdatetime` datetime NOT NULL,
  PRIMARY KEY  (`albumid`),
  KEY `list_userid` (`albumid`,`userid`,`imgcount`,`type`),
  KEY `list_albumcateid` (`imgcount` , `albumcateid` , `albumid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_announcements`;
CREATE TABLE `dnt_announcements` (
  `id` int(11) NOT NULL auto_increment,
  `poster` varchar(20) NOT NULL,
  `posterid` int(11) NOT NULL,
  `title` varchar(250) NOT NULL,
  `displayorder` int(11) NOT NULL,
  `starttime` datetime NOT NULL,
  `endtime` datetime NOT NULL,
  `message` longtext NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_attachments`;
CREATE TABLE `dnt_attachments` (
  `aid` int(11) NOT NULL auto_increment,
  `uid` int(11) NOT NULL default '0',
  `tid` int(11) NOT NULL,
  `pid` int(11) NOT NULL,
  `postdatetime` datetime NOT NULL,
  `readperm` int(11) NOT NULL,
  `filename` varchar(100) NOT NULL,
  `description` varchar(100) NOT NULL,
  `filetype` varchar(50) NOT NULL,
  `filesize` int(11) NOT NULL,
  `attachment` varchar(100) NOT NULL,
  `downloads` int(11) NOT NULL,
  PRIMARY KEY  (`aid`),
  KEY `pid` (`pid`),
  KEY `tid` (`tid`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_attachtypes`;
CREATE TABLE `dnt_attachtypes` (
  `id` smallint(6) NOT NULL auto_increment,
  `extension` text NOT NULL,
  `maxsize` int(11) NOT NULL,
  KEY `id` (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_attachtypes` (`extension`, `maxsize`) VALUES 
('jpg',2048000),
('gif',1024000),
('png',2048000),
('zip',2048000),
('rar',2048000),
('jpeg',2048000);

DROP TABLE IF EXISTS `dnt_bbcodes`;
CREATE TABLE `dnt_bbcodes` (
  `id` int(11) NOT NULL auto_increment,
  `available` int(11) NOT NULL,
  `tag` varchar(100) NOT NULL,
  `icon` varchar(50) default NULL,
  `example` varchar(255) NOT NULL,
  `params` int(11) NOT NULL,
  `nest` int(11) NOT NULL,
  `explanation` longtext,
  `replacement` longtext,
  `paramsdescript` longtext,
  `paramsdefvalue` longtext,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_bbcodes`(`available`,`tag`,`icon`,`example`,`params`,`nest`,`explanation`,`replacement`,`paramsdescript`,`paramsdefvalue`) VALUES 
(1,'flash','flash.gif','[flash]http://localhost/abc.swf[/flash]',1,1,'在页面中插入flash影片','<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0" width="550" height="400"><param name="allowScriptAccess" value="sameDomain"/><param name="movie" value="{1}"/><param name="quality" value="high"/><param name="bgcolor" value="#ffffff"/><embed src="{1}" quality="high" bgcolor="#ffffff" width="550" height="400" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" /></object>','请输入flash地址','http://'),
(1,'wmv','wmv.gif','[wmv=200,200]http://localhost/123.avi[/wmv]',3,1,'在帖子中加入 Windows Media Player 格式的视频内容','<object align=middle classid=CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width={2} height={3}><param name=ShowStatusBar value=-1><param name=Filename value={1}><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src={1}  width={2} height={3}></embed></object>','请输入Windows Media Player视频文件地址,请输入Windows Media Player视频文件的显示宽度,请输入Windows Media Player视频文件的显示高度','http://,200,200'),
(1,'wma','wma.gif','[wma]http://localhost/123.mp3[/wma]',1,1,'在帖子中加入 Windows Media Player 格式的音频内容','<object classid="CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6" width="260" height="64"><param name="autostart" value="0" /><param name="url" value="{1}" /><embed src="{1}" autostart="0" type="video/x-ms-wmv" width="260" height="42"></embed></object>','请输入 Windows Media Player 音频的地址','http://'),
(1,'rm','rm.gif','[rm=200,200]http://localhost/123.rm[/rm]',3,1,'在帖子中插入RealPlayer视频','<object classid="clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA" width="{2}" height="{3}"><param name="src" value="{1}" /><param name="controls" value="imagewindow" /><param name="console" value="{1}" /><embed src="{1}" type="audio/x-pn-realaudio-plugin" controls="IMAGEWINDOW" console="{1}" width="{2}" height="360"></embed></object><br style="height:0" /><object classid="CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA" width="{2}" height="32"><param name="src" value="{1}" /><param name="controls" value="controlpanel" /><param name="console" value="{1}" /><embed src="{1}" type="audio/x-pn-realaudio-plugin" controls="ControlPanel" console="{1}" width="{2}" height="32"></embed></object>','请输入RealPlayer视频的地址,请输入RealPlayer视频的宽度,请输入RealPlayer视频的高度','http://,200,200'),
(1,'ra','ra.gif','[ra]http://localhost/123.ra[/ra]',1,1,'在帖子中插入RealPlayer音频','<object classid="clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA" width="400" height="30"><param name="src" value="{1}" /><param name="controls" value="controlpanel" /><param name="console" value="{1}" /><embed src="{1}" type="audio/x-pn-realaudio-plugin" console="{1}" controls="ControlPanel" width="400" height="30"></embed></object>','请输入RealPlayer音频地址','http://'),
(1,'fly','fly.gif','[fly]示例文字[/fly]',1,1,'在帖子中插入滚动文字','<marquee width="90%" behavior="alternate" scrollamount="3">{1}</marquee>','请输入滚动文字','滚动文字'),
(1,'silverlight','silverlight.gif','[silverlight]http://localhost/123.wmv[/silverlight]',3,1,'在帖子中使用Silverlight播放器', '<script type="text/javascript" src="silverlight/player/showtopiccontainer.js"></script><div id="divPlayer_{RANDOM}"></div><script>Silverlight.InstallAndCreateSilverlight("1.0",document.getElementById("divPlayer_{RANDOM}"),installExperienceHTML,"InstallPromptDiv",function(){new StartPlayer_0("divPlayer_{RANDOM}", parseInt("{2}"), parseInt("{3}"), "{1}", forumpath)})</script>', '请输入音频或视频的地址,请输入音频或视频的宽度,请输入视频的高度(音频无效)', 'http://,400,300');

DROP TABLE IF EXISTS `dnt_creditslog`;
CREATE TABLE `dnt_creditslog` (
  `id` int(11) NOT NULL auto_increment,
  `uid` int(11) NOT NULL,
  `fromto` int(11) NOT NULL,
  `sendcredits` tinyint(3) unsigned NOT NULL,
  `receivecredits` tinyint(3) unsigned NOT NULL,
  `send` double NOT NULL,
  `receive` double NOT NULL,
  `paydate` datetime NOT NULL,
  `operation` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_failedlogins`;
CREATE TABLE `dnt_failedlogins` (
  `ip` varchar(15) NOT NULL,
  `errcount` smallint(6) NOT NULL default '0',
  `lastupdate` datetime NOT NULL,
  PRIMARY KEY  (`ip`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_favorites`;
CREATE TABLE `dnt_favorites` (
  `uid` int(11) NOT NULL,
  `tid` int(11) NOT NULL,
  `typeid` tinyint(3) unsigned NOT NULL default '0'
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_forumfields`;
CREATE TABLE `dnt_forumfields` (
  `fid` smallint(6) NOT NULL,
  `password` varchar(16) NOT NULL,
  `icon` varchar(255) default NULL,
  `postcredits` varchar(255) default NULL,
  `replycredits` varchar(255) default NULL,
  `redirect` varchar(255) default NULL,
  `attachextensions` varchar(255) default NULL,
  `rules` longtext,
  `topictypes` longtext,
  `viewperm` longtext,
  `postperm` longtext,
  `replyperm` longtext,
  `getattachperm` longtext,
  `postattachperm` longtext,
  `moderators` longtext,
  `description` longtext,
  `applytopictype` tinyint(3) unsigned NOT NULL default '0',
  `postbytopictype` tinyint(3) unsigned NOT NULL default '0',
  `viewbytopictype` tinyint(3) unsigned NOT NULL default '0',
  `topictypeprefix` tinyint(3) unsigned NOT NULL default '0',
  `permuserlist` longtext
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

INSERT INTO `dnt_forumfields` (`fid`, `password`, `icon`, `postcredits`, `replycredits`, `redirect`, `attachextensions`, `rules`, `topictypes`, `viewperm`, `postperm`, `replyperm`, `getattachperm`, `postattachperm`, `moderators`, `description`, `applytopictype`, `postbytopictype`, `viewbytopictype`, `topictypeprefix`, `permuserlist`) VALUES 
('1','', '', '', '', '', '', '', '', '', '', '', '', '', '', '',0,0,0,0,''),
('2','', '', '', '', '', '', '', '', '', '', '', '', '', '', '默认版块说明文字',0,0,0,0,'');

DROP TABLE IF EXISTS `dnt_forumlinks`;
CREATE TABLE `dnt_forumlinks` (
  `id` smallint(6) NOT NULL auto_increment,
  `displayorder` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `url` varchar(100) NOT NULL,
  `note` varchar(200) NOT NULL,
  `logo` varchar(100) NOT NULL,
  KEY `id` (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_forumlinks` (`displayorder`, `name`, `url`, `note`, `logo`) VALUES 
 (1, 'Discuz!NT', 'http://nt.discuz.net', '提供最新 Discuz!NT 产品新闻、软件下载与技术交流', 'images/logo.gif');

DROP TABLE IF EXISTS `dnt_forums`;
CREATE TABLE `dnt_forums` (
  `fid` int(11) NOT NULL auto_increment,
  `parentid` smallint(6) NOT NULL,
  `layer` smallint(6) NOT NULL,
  `pathlist` text NOT NULL,
  `parentidlist` text NOT NULL,
  `subforumcount` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `status` int(11) NOT NULL,
  `colcount` smallint(6) NOT NULL,
  `displayorder` int(11) NOT NULL,
  `templateid` smallint(6) NOT NULL,
  `topics` int(11) NOT NULL default '0',
  `curtopics` int(11) NOT NULL default '0',
  `posts` int(11) NOT NULL default '0',
  `todayposts` int(11) NOT NULL default '0',
  `lasttid` int(11) NOT NULL default '0',
  `lasttitle` varchar(60) NOT NULL default '',
  `lastpost` datetime NOT NULL,
  `lastposterid` int(11) NOT NULL default '0',
  `lastposter` varchar(20) NOT NULL default '',
  `allowsmilies` int(11) NOT NULL,
  `allowrss` int(11) NOT NULL,
  `allowhtml` int(11) NOT NULL,
  `allowbbcode` int(11) NOT NULL,
  `allowimgcode` int(11) NOT NULL,
  `allowblog` int(11) NOT NULL,
  `allowtrade` int(11) NOT NULL,
  `alloweditrules` int(11) NOT NULL,
  `allowthumbnail` int(11) NOT NULL,
  `recyclebin` int(11) NOT NULL,
  `modnewposts` int(11) NOT NULL,
  `jammer` int(11) NOT NULL,
  `disablewatermark` int(11) NOT NULL,
  `inheritedmod` int(11) NOT NULL,
  `autoclose` smallint(6) NOT NULL,
  PRIMARY KEY  (`fid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_forums` (`parentid`, `layer`, `pathlist`, `parentidlist`, `subforumcount`, `name`, `status`, `colcount`, `displayorder`, `templateid`, `topics`, `curtopics`, `posts`, `todayposts`, `lasttid`, `lasttitle`, `lastpost`, `lastposterid`, `lastposter`, `allowsmilies`, `allowrss`, `allowhtml`, `allowbbcode`, `allowimgcode`, `allowblog`, `allowtrade`, `alloweditrules`, `allowthumbnail`, `recyclebin`, `modnewposts`, `jammer`, `disablewatermark`, `inheritedmod`, `autoclose`) VALUES 
(0, 0, '<a href="showforum-1.aspx">默认分类</a>', '0', 1, '默认分类', 1, 1, 1, 0, 0, 0, 0, 0, 0, '', '1900-1-1 0:00:00', 0, '', 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
(1, 1, '<a href="showforum-1.aspx">默认分类</a><a href="showforum-2.aspx">默认版块</a>', '1', 0, '默认版块', 1, 1, 2, 0, 0, 0, 0, 0, 0, '', '1900-1-1 0:00:00', 0, '', 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

DROP TABLE IF EXISTS `dnt_help`;
CREATE TABLE `dnt_help` (
  `id` int(11) NOT NULL auto_increment,
  `title` varchar(100) NOT NULL,
  `message` longtext,
  `pid` int(11) NOT NULL,
  `orderby` int(11) NOT NULL default '0',
  KEY `id` (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_help` (`id`, `title`, `message`, `pid`, `orderby`) VALUES 
(1, '用户须知', '', 0, 1),
(2, '论坛常见问题', '', 0, 2),
(3, '个人空间常见问题', '', 0, 3),
(4, '相册常见问题', '', 0, 4),
(5, '我必须要注册吗？', '这取决于管理员如何设置 Discuz!NT 论坛的用户组权限选项，您甚至有可能必须在注册成正式用户后后才能浏览帖子。当然，在通常情况下，您至少应该是正式用户才能发新帖和回复已有帖子。请 <a href=\"register.aspx\" target=\"_blank\">点击这里</a> 免费注册成为我们的新用户！<br /><br />强烈建议您注册，这样会得到很多以游客身份无法实现的功能。', 1, 1),
(6, '我如何登录论坛？', '如果您已经注册成为该论坛的会员，那么您只要通过访问页面右上的<a href=\"login.aspx\" target=\"_blank\">登录</a>，进入登陆界面填写正确的用户名和密码（如果您设有安全提问，请选择正确的安全提问并输入对应的答案），点击“提交”即可完成登陆如果您还未注册请点击这里。<br /><br />如果需要保持登录，请选择相应的 Cookie 时间，在此时间范围内您可以不必输入密码而保持上次的登录状态。', 1, 2),
(7, '忘记我的登录密码，怎么办？', '当您忘记了用户登录的密码，您可以通过注册时填写的电子邮箱重新设置一个新的密码。点击登录页面中的 <a href=\"getpassword.aspx\" target=\"_blank\">取回密码</a>，按照要求填写您的个人信息，系统将自动发送重置密码的邮件到您注册时填写的 Email 信箱中。如果您的 Email 已失效或无法收到信件，请与论坛管理员联系。', 1, 3),
(8, '我如何使用个性化头像', '在<a href=\"usercppreference.aspx\" target=\"_blank\">用户中心</a>中的 个人设置  -> 个性设置，可以使用论坛自带的头像或者自定义的头像。', 1, 4),
(9, '我如何修改登录密码', '在<a href=\"usercpnewpassword.aspx\" target=\"_blank\">用户中心</a>中的 个人设置 -> 更改密码，填写“原密码”，“新密码”，“确认新密码”。点击“提交”，即可修改。', 1, 5),
(10, '我如何使用个性化签名和昵称', '在<a href=\"usercpprofile.aspx\" target=\"_blank\">用户中心</a>中的 个人设置 -> 编辑个人档案，有一个“昵称”和“个人签名”的选项，可以在此设置。', 1, 6),
(11, '我如何发表新主题，以及投票', '在论坛版块中，点“发帖”，点击即可进入功能齐全的发帖界面。<br /><br />注意：需要发布投票时请在发帖界面的下方开启投票选项进行设置即可。如发布普通主题，直接点击“发帖”，当然您也可以使用版块下面的“快速发帖”发表新帖(如果此选项打开)。一般论坛都设置为需要登录后才能发帖。', 2, 1),
(12, '我如何发表回复', '回复有分三种：第一、帖子最下方的快速回复； 第二、在您想回复的楼层点击右下方“回复”； 第三、完整回复页面，点击本页“新帖”旁边的“回复”。', 2, 2),
(13, '我如何编辑自己的帖子', '在帖子的右上角，有编辑，回复，报告等选项，点击编辑，就可以对帖子进行编辑。', 2, 3),
(14, '我如何出售购买主题', '<li>出售主题：当您进入发贴界面后，如果您所在的用户组有发买卖贴的权限，在“售价(金钱)”后面填写主题的价格，这样其他用户在查看这个帖子的时候就需要进入交费的过程才可以查看帖子。</li><li>购买主题：浏览你准备购买的帖子，在帖子的相关信息的下面有[查看付款记录] [购买主题] [返回上一页] 等链接，点击“购买主题”进行购买。</li>', 2, 4),
(15, '我如何上传附件', '<li>发表新主题的时候上传附件，步骤为：写完帖子标题和内容后点上传附件右方的浏览，然后在本地选择要上传附件的具体文件名，最后点击发表话题。</li><li>发表回复的时候上传附件，步骤为：写完回复楼主的内容，然后点上传附件右方的浏览，找到需要上传的附件，点击发表回复。</li>', 2, 5),
(16, '我如何实现发帖时图文混排效果', '<li>发表新主题的时候点击上传附件左侧的“[插入]”链接把附件标记插入到帖子中适当的位置即可。</li>', 2, 6),
(17, '我如何使用Discuz!NT代码', '<table width=\"99%\" cellpadding=\"2\" cellspacing=\"2\"><tr><th width=\"50%\">Discuz!NT代码</th><th width=\"402\">效果</th></tr><tr><td>[b]粗体文字 Abc[/b]</td><td><strong>粗体文字 Abc</strong></td></tr><tr><td>[i]斜体文字 Abc[/i]</td><td><i>斜体文字 Abc</i></td></tr><tr><td>[u]下划线文字 Abc[/u]</td><td><u>下划线文字 Abc</u></td></tr><tr><td>[color=red]红颜色[/color]</td><td><font color=\"red\">红颜色</font></td></tr><tr><td>[size=3]文字大小为 3[/size] </td><td><font size=\"3\">文字大小为 3</font></td></tr><tr><td>[font=仿宋]字体为仿宋[/font] </td><td><font face=\"仿宋\">字体为仿宋</font></td></tr><tr><td>[align=Center]内容居中[/align] </td><td><div align=\"center\">内容居中</div></td></tr><tr><td>[url]http://www.comsenz.com[/url]</td><td><a href=\"http://www.comsenz.com\" target=\"_blank\">http://www.comsenz.com</a>（超级链接）</td></tr><tr><td>[url=http://nt.discuz.net]Discuz!NT 论坛[/url]</td><td><a href=\"http://nt.discuz.net\" target=\"_blank\">Discuz!NT 论坛</a>（超级链接）</td></tr><tr><td>[email]myname@mydomain.com[/email]</td><td><a href=\"mailto:myname@mydomain.com\">myname@mydomain.com</a>（E-mail链接）</td></tr><tr><td>[email=support@discuz.net]Discuz!NT 技术支持[/email]</td><td><a href=\"mailto:support@discuz.net\">Discuz!NT 技术支持（E-mail链接）</a></td></tr><tr><td>[quote]Discuz!NT Board 是由康盛创想（北京）科技有限公司开发的论坛软件[/quote] </td><td><div style=\"font-size: 12px\"><br /><br /><div class=\"quote\"><h5>引用:</h5><blockquote>原帖由 <i>admin</i> 于 2006-12-26 08:45 发表<br />Discuz!NT Board 是由康盛创想（北京）科技有限公司开发的论坛软件</blockquote></div></td></tr> <tr><td>[code]Discuz!NT Board 是由康盛创想（北京）科技有限公司开发的论坛软件[/code] </td><td><div style=\"font-size: 12px\"><br /><br /><div class=\"blockcode\"><h5>代码:</h5><code id=\"code0\">Discuz!NT Board 是由康盛创想（北京）科技有限公司开发的论坛软件</code></div></td></tr><tr><td>[hide]隐藏内容 Abc[/hide]</td><td>效果:只有当浏览者回复本帖时，才显示其中的内容，否则显示为“<b>**** 隐藏信息 跟帖后才能显示 *****</b>”</td></tr><tr><td>[list][*]列表项 #1[*]列表项 #2[*]列表项 #3[/list]</td><td><ul><li>列表项 ＃1</li><li>列表项 ＃2</li><li>列表项 ＃3 </li></ul></td></tr><tr><td>[img]http://nt.discuz.net/templates/default/images/clogo.gif[/img] </td><td>帖子内显示为：<img src=\"http://nt.discuz.net/templates/default/images/clogo.gif\" /></td></tr><tr><td>[img=88,31]http://nt.discuz.net/templates/default/images/clogo.gif[/img] </td><td>帖子内显示为：<img src=\"http://nt.discuz.net/templates/default/images/clogo.gif\" /></td> </tr> <tr><td>[fly]飞行的效果[/fly]</td><td><marquee scrollamount=\"3\" behavior=\"alternate\" width=\"90%\">飞行的效果</marquee></td></tr><tr><td>[flash]Flash网页地址 [/flash] </td><td>帖子内嵌入 Flash 动画</td></tr><tr><td>X[sup]2[/sup]</td><td>X<sup>2</sup></td></tr><tr><td>X[sub]2[/sub]</td><td>X<sub>2</sub></td></tr></table>', 2, 7),
(18, '我如何使用短消息功能', '您登录后，点击导航栏上的短消息按钮，即可进入短消息管理。点击[发送短消息]按钮，在\"发送到\"后输入收信人的用户名，填写完标题和内容，点提交(或按 Ctrl+Enter 发送)即可发出短消息。<br /><br />如果要保存到发件箱，以在提交前勾选\"保存到发件箱中\"前的复选框。<ul><li>点击收件箱可打开您的收件箱查看收到的短消息。</li><li>点击发件箱可查看保存在发件箱里的短消息。 </li></ul>', 2, 8),
(19, '我如何查看论坛会员数据', '点击导航栏上面的会员，然后显示的是此论坛的会员数据。注：需要论坛管理员开启允许你查看会员资料才可看到。', 2, 9),
(20, '我如何使用搜索', '点击导航栏上面的搜索，输入搜索的关键字并选择一个范围，就可以检索到您有权限访问论坛中的相关的帖子。', 2, 10),
(21, '我如何使用“我的”功能', '<li>会员必须首先<a href=\"login.aspx\" target=\"_blank\">登录</a>，没有用户名的请先<a href=\"register.aspx\" target=\"_blank\">注册</a>；</li><li>登录之后在论坛的左上方会出现一个“我的”的超级链接，点击这个链接之后就可进入到有关于您的信息。</li>', 2, 11),
(22, '我如何向管理员举报帖子', '打开一个帖子，在帖子的右上角可以看到：\"举报” | \"树型“ | \"收藏\" | \"编辑\" | \"删除\" |\"评分\" 等等几个按钮，单击“举报”按钮即可完成举报某个帖子的操作。', 2, 12),
(23, '我如何“收藏”帖子', '当你浏览一个帖子时，在它的右上角可以看到：\"举报” | \"树型“ | \"收藏\" | \"编辑\" | \"删除\" |\"评分\"，点击相对应的文字连接即可完成相关的操作。', 2, 13),
(24, '我如何使用RSS订阅', '在论坛的首页和进入版块的页面的右上角就会出现一个rss订阅的小图标<img src=\"templates/default/images/rss.gif\" border=\"0\">，鼠标点击之后将出现本站点的rss地址，你可以将此rss地址放入到你的rss阅读器中进行订阅。', 2, 14),
(25, '我如何清除Cookies', '介绍3种常用浏览器的Cookies清除方法(注：此方法为清除全部的Cookies,请谨慎使用)<ul><li>Internet Explorer: 工具（选项）内的Internet选项→常规选项卡内，IE6直接可以看到删除Cookies的按钮点击即可，IE7为“浏 览历史记录”选项内的删除点击即可清空Cookies。对于Maxthon,腾讯TT等IE核心浏览器一样适用。 </li><li>FireFox:工具→选项→隐私→Cookies→显示Cookie里可以对Cookie进行对应的删除操作。 </li><li>Opera:工具→首选项→高级→Cookies→管理Cookies即可对Cookies进行删除的操作。</li></ul>', 2, 15),
(26, '我如何开通个人空间', '如果您有权限开通“我的个人空间”，当用户登录论坛以后在论坛首页，在搜索框下方有申请个人空间连接点击提交申请，如果管理员已经开启了手动开通则需要等待管理员来审核通过您的申请', 3, 1),
(27, '我如何使用表情代码', '表情是一些用字符表示的表情符号，如果打开表情功能，Discuz!NT 会把一些符号转换成小图像，显示在帖子中，更加美观明了。同时Discuz!NT支持表情分类，分页功能。插入表情时只需使用鼠标点击表情即可。', 2, 16),
(28, '我如何在个人空间发表日志', '如果您已经开通“个人空间”，当用户登录论坛以后在论坛用户中心 -> 个人空间 -> 管理文章内可以进行发表和管理日志的操作。', 3, 2),
(29, '我如何在相册中上传图片', '如果您已经开通“相册功能”，当用户登录论坛以后在论坛用户中心 -> 相册 -> 管理相册内可以进行发表和管理相册的操作。', 4, 1);

DROP TABLE IF EXISTS `dnt_medals`;
CREATE TABLE `dnt_medals` (
  `medalid` smallint(6) NOT NULL,
  `name` varchar(50) NOT NULL,
  `available` int(11) NOT NULL default '0',
  `image` varchar(30) NOT NULL default ''
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

INSERT INTO `dnt_medals` (`medalid`, `name`, `available`, `image`) VALUES 
(1,'Medal No.1',1,'Medal1.gif'),
(2,'Medal No.2',1,'Medal2.gif'),	
(3,'Medal No.3',1,'Medal3.gif'),	
(4,'Medal No.4',1,'Medal4.gif'),	
(5,'Medal No.5',1,'Medal5.gif'),	
(6,'Medal No.6',1,'Medal6.gif'),	
(7,'Medal No.7',1,'Medal7.gif'),	
(8,'Medal No.8',1,'Medal8.gif'),	
(9,'Medal No.9',1,'Medal9.gif'),	
(10,'Medal No.10',1,'Medal10.gif'),	
(11,'Medal No.11',0,''),	
(12,'Medal No.12',0,''),	
(13,'Medal No.13',0,''),	
(14,'Medal No.14',0,''),	
(15,'Medal No.15',0,''),	
(16,'Medal No.16',0,''),	
(17,'Medal No.17',0,''),	
(18,'Medal No.18',0,''),	
(19,'Medal No.19',0,''),	
(20,'Medal No.20',0,''),	
(21,'Medal No.21',0,''),	
(22,'Medal No.22',0,''),	
(23,'Medal No.23',0,''),	
(24,'Medal No.24',0,''),	
(25,'Medal No.25',0,''),	
(26,'Medal No.26',0,''),	
(27,'Medal No.27',0,''),	
(28,'Medal No.28',0,''),	
(29,'Medal No.29',0,''),	
(30,'Medal No.30',0,''),	
(31,'Medal No.31',0,''),	
(32,'Medal No.32',0,''),	
(33,'Medal No.33',0,''),	
(34,'Medal No.34',0,''),	
(35,'Medal No.35',0,''),	
(36,'Medal No.36',0,''),	
(37,'Medal No.37',0,''),	
(38,'Medal No.38',0,''),	
(39,'Medal No.39',0,''),	
(40,'Medal No.40',0,''),	
(41,'Medal No.41',0,''),	
(42,'Medal No.42',0,''),	
(43,'Medal No.43',0,''),	
(44,'Medal No.44',0,''),	
(45,'Medal No.45',0,''),	
(46,'Medal No.46',0,''),	
(47,'Medal No.47',0,''),	
(48,'Medal No.48',0,''),	
(49,'Medal No.49',0,''),	
(50,'Medal No.50',0,''),	
(51,'Medal No.51',0,''),
(52,'Medal No.52',0,''),	
(53,'Medal No.53',0,''),	
(54,'Medal No.54',0,''),	
(55,'Medal No.55',0,''),	
(56,'Medal No.56',0,''),	
(57,'Medal No.57',0,''),	
(58,'Medal No.58',0,''),	
(59,'Medal No.59',0,''),	
(60,'Medal No.60',0,''),	
(61,'Medal No.61',0,''),	
(62,'Medal No.62',0,''),	
(63,'Medal No.63',0,''),	
(64,'Medal No.64',0,''),	
(65,'Medal No.65',0,''),	
(66,'Medal No.66',0,''),	
(67,'Medal No.67',0,''),	
(68,'Medal No.68',0,''),	
(69,'Medal No.69',0,''),	
(70,'Medal No.70',0,''),	
(71,'Medal No.71',0,''),	
(72,'Medal No.72',0,''),	
(73,'Medal No.73',0,''),	
(74,'Medal No.74',0,''),	
(75,'Medal No.75',0,''),	
(76,'Medal No.76',0,''),	
(77,'Medal No.77',0,''),	
(78,'Medal No.78',0,''),	
(79,'Medal No.79',0,''),	
(80,'Medal No.80',0,''),	
(81,'Medal No.81',0,''),	
(82,'Medal No.82',0,''),	
(83,'Medal No.83',0,''),	
(84,'Medal No.84',0,''),	
(85,'Medal No.85',0,''),	
(86,'Medal No.86',0,''),	
(87,'Medal No.87',0,''),	
(88,'Medal No.88',0,''),	
(89,'Medal No.89',0,''),	
(90,'Medal No.90',0,''),	
(91,'Medal No.91',0,''),	
(92,'Medal No.92',0,''),	
(93,'Medal No.93',0,''),	
(94,'Medal No.94',0,''),	
(95,'Medal No.95',0,''),	
(96,'Medal No.96',0,''),	
(97,'Medal No.97',0,''),	
(98,'Medal No.98',0,''),	
(99,'Medal No.99',0,'');

DROP TABLE IF EXISTS `dnt_medalslog`;
CREATE TABLE `dnt_medalslog` (
  `id` int(11) NOT NULL auto_increment,
  `adminname` varchar(50) default NULL,
  `adminid` int(11) default NULL,
  `ip` varchar(15) default NULL,
  `postdatetime` datetime default NULL,
  `username` varchar(50) default NULL,
  `uid` int(11) default NULL,
  `actions` varchar(100) default NULL,
  `medals` int(11) default NULL,
  `reason` varchar(100) default NULL,
  KEY `id` (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_moderatormanagelog`;
CREATE TABLE `dnt_moderatormanagelog` (
  `id` int(11) NOT NULL auto_increment,
  `moderatoruid` int(11) default NULL,
  `moderatorname` varchar(50) default NULL,
  `groupid` int(11) default NULL,
  `grouptitle` varchar(50) default NULL,
  `ip` varchar(15) default NULL,
  `postdatetime` datetime default NULL,
  `fid` int(11) default NULL,
  `fname` varchar(100) default NULL,
  `tid` int(11) default NULL,
  `title` varchar(200) default NULL,
  `actions` varchar(50) default NULL,
  `reason` varchar(200) default NULL,
  KEY `id` (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_moderators`;
CREATE TABLE `dnt_moderators` (
  `uid` int(11) NOT NULL,
  `fid` smallint(6) NOT NULL,
  `displayorder` smallint(6) NOT NULL,
  `inherited` smallint(6) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_myposts`;
CREATE TABLE `dnt_myposts` (
  `uid` int(11) NOT NULL,
  `tid` int(11) NOT NULL,
  `pid` int(11) NOT NULL,
  `dateline` datetime NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_mytopics`;
CREATE TABLE `dnt_mytopics` (
  `uid` int(11) NOT NULL,
  `tid` int(11) NOT NULL,
  `dateline` datetime NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_online`;
CREATE TABLE `dnt_online` (
  `olid` int(11) NOT NULL auto_increment,
  `userid` int(11) NOT NULL default '-1',
  `ip` varchar(15) NOT NULL default '0.0.0.0',
  `username` varchar(20) NOT NULL default '',
  `nickname` varchar(20) NOT NULL default '',
  `password` varchar(32) NOT NULL default '',
  `groupid` smallint(6) NOT NULL default '0',
  `olimg` varchar(80) NOT NULL default '',
  `adminid` smallint(6) NOT NULL default '0',
  `invisible` smallint(6) NOT NULL default '0',
  `action` smallint(6) NOT NULL default '0',
  `lastactivity` smallint(6) NOT NULL default '0',
  `lastposttime` datetime NOT NULL,
  `lastpostpmtime` datetime NOT NULL,
  `lastsearchtime` datetime NOT NULL,
  `lastupdatetime` datetime NOT NULL,
  `forumid` int(11) NOT NULL default '0',
  `forumname` varchar(50) NOT NULL default '',
  `titleid` int(11) NOT NULL default '0',
  `title` varchar(80) NOT NULL default '',
  `verifycode` varchar(10) NOT NULL default '',
  PRIMARY KEY  (`olid`),
  KEY `forum` (`userid`,`forumid`,`invisible`),
  KEY `forumid` (`forumid`),
  KEY `invisible` (`userid`,`invisible`),
  KEY `ip` (`userid`,`ip`),
  KEY `password` (`userid`,`password`)
) ENGINE=MEMORY DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_onlinelist`;
CREATE TABLE `dnt_onlinelist` (
  `groupid` smallint(6) NOT NULL,
  `displayorder` int(11) default NULL,
  `title` varchar(50) NOT NULL,
  `img` varchar(50) default NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

INSERT INTO `dnt_onlinelist` (`groupid`, `displayorder`, `title`, `img`) VALUES 
(0, 999, '用户','member.gif'),
(1, 1, '管理员','admin.gif'),
(2, 2, '超级版主','supermoder.gif'),
(3, 3, '版主','moder.gif'),
(4, 4, '禁止发言',''),
(5, 5, '禁止访问',''),
(6, 6, '禁止 IP',''),
(7, 7, '游客','guest.gif'),
(8, 8, '等待验证会员',''),
(9, 9, '乞丐',''),
(10, 10, '新手上路',''),
(11, 11, '注册会员',''),
(12, 12, '中级会员',''),
(13, 13, '高级会员',''),
(14, 14, '金牌会员',''),
(15, 15, '论坛元老','');

DROP TABLE IF EXISTS `dnt_paymentlog`;
CREATE TABLE `dnt_paymentlog` (
  `id` int(11) NOT NULL auto_increment,
  `uid` int(11) NOT NULL,
  `tid` int(11) NOT NULL,
  `authorid` int(11) NOT NULL,
  `buydate` datetime NOT NULL,
  `amount` smallint(6) NOT NULL,
  `netamount` smallint(6) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_photocomments`;
CREATE TABLE `dnt_photocomments` (
  `commentid` int(11) NOT NULL auto_increment,
  `photoid` int(11) NOT NULL,
  `username` varchar(20) NOT NULL,
  `userid` int(11) NOT NULL,
  `ip` varchar(100) NOT NULL default '',
  `postdatetime` datetime NOT NULL,
  `content` text NOT NULL,
  KEY `commentid` (`commentid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_photos`;
CREATE TABLE `dnt_photos` (
  `photoid` int(11) NOT NULL auto_increment,
  `filename` varchar(255) NOT NULL,
  `attachment` varchar(255) NOT NULL,
  `filesize` int(11) NOT NULL,
  `title` varchar(20) NOT NULL,
  `description` varchar(200) NOT NULL default '',
  `postdate` datetime NOT NULL,
  `albumid` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `username` varchar(20) NOT NULL default '',
  `views` int(11) NOT NULL default '0',
  `commentstatus` tinyint(3) unsigned NOT NULL default '0',
  `tagstatus` tinyint(3) unsigned NOT NULL default '0',
  `comments` int(11) NOT NULL default '0',
  `isattachment` int(11) NOT NULL default '0',
  `width` int(11) default NULL,
  `height` int(11) default NULL,
  PRIMARY KEY  (`photoid`),
  KEY `albumid` (`albumid`),
  KEY `photoiduserid` (`photoid`,`userid`),
  KEY `userid` (`userid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_phototags`;
CREATE TABLE `dnt_phototags` (
  `tagid` int(11) NOT NULL default '0',
  `photoid` int(11) NOT NULL default '0'
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_pms`;
CREATE TABLE `dnt_pms` (
  `pmid` int(11) NOT NULL auto_increment,
  `msgfrom` varchar(50) NOT NULL,
  `msgfromid` int(11) NOT NULL,
  `msgto` varchar(50) NOT NULL,
  `msgtoid` int(11) NOT NULL,
  `folder` smallint(6) NOT NULL,
  `new` int(11) NOT NULL,
  `subject` varchar(60) NOT NULL,
  `postdatetime` datetime NOT NULL,
  `message` longtext NOT NULL,
  PRIMARY KEY  (`pmid`),
  KEY `msgtoid` (`msgtoid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_polls`;
CREATE TABLE `dnt_polls` (
  `tid` int(11) NOT NULL,
  `polltype` tinyint(3) unsigned NOT NULL,
  `itemcount` smallint(6) NOT NULL,
  `itemnamelist` longtext NOT NULL,
  `itemvaluelist` longtext NOT NULL,
  `usernamelist` longtext NOT NULL,
  `enddatetime` date default NULL,
  `userid` int(11) NOT NULL,
  PRIMARY KEY  (`tid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_postid`;
CREATE TABLE `dnt_postid` (
  `pid` int(11) NOT NULL auto_increment,
  `postdatetime` datetime NOT NULL,
  PRIMARY KEY  (`pid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_posts1`;
CREATE TABLE `dnt_posts1` (
  `pid` int(11) NOT NULL default '0',
  `fid` int(11) NOT NULL,
  `tid` int(11) NOT NULL,
  `parentid` int(11) NOT NULL default '0',
  `layer` int(11) NOT NULL default '0',
  `poster` varchar(20) NOT NULL default '',
  `posterid` int(11) NOT NULL default '0',
  `title` varchar(60) NOT NULL,
  `postdatetime` datetime NOT NULL,
  `message` longtext NOT NULL,
  `ip` varchar(15) NOT NULL default '',
  `lastedit` varchar(50) NOT NULL default '',
  `invisible` int(11) NOT NULL default '0',
  `usesig` int(11) NOT NULL default '0',
  `htmlon` int(11) NOT NULL default '0',
  `smileyoff` int(11) NOT NULL default '0',
  `parseurloff` int(11) NOT NULL default '0',
  `bbcodeoff` int(11) NOT NULL default '0',
  `attachment` int(11) NOT NULL default '0',
  `rate` int(11) NOT NULL default '0',
  `ratetimes` int(11) NOT NULL default '0',
  PRIMARY KEY  (`pid`),
  UNIQUE KEY `showtopic` (`tid`,`invisible`,`pid`),
  KEY `parentid` (`parentid`),
  KEY `treelist` (`tid`,`invisible`,`parentid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_ratelog`;
CREATE TABLE `dnt_ratelog` (
  `id` int(11) NOT NULL auto_increment,
  `pid` int(11) NOT NULL,
  `uid` int(11) NOT NULL,
  `username` varchar(20) NOT NULL,
  `extcredits` tinyint(3) unsigned NOT NULL,
  `postdatetime` datetime NOT NULL,
  `score` smallint(6) NOT NULL,
  `reason` varchar(50) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_searchcaches`;
CREATE TABLE `dnt_searchcaches` (
  `searchid` int(11) NOT NULL auto_increment,
  `keywords` varchar(255) NOT NULL,
  `searchstring` varchar(255) NOT NULL,
  `ip` varchar(15) NOT NULL,
  `uid` int(11) NOT NULL,
  `groupid` int(11) NOT NULL,
  `postdatetime` datetime NOT NULL,
  `expiration` datetime NOT NULL,
  `topics` int(11) NOT NULL,
  `tids` longtext NOT NULL,
  PRIMARY KEY  (`searchid`),
  KEY `getsearchid` (`searchstring`,`groupid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_smilies`;
CREATE TABLE `dnt_smilies` (
  `id` int(11) NOT NULL,
  `displayorder` int(11) NOT NULL,
  `type` int(11) NOT NULL,
  `code` varchar(30) NOT NULL,
  `url` varchar(60) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

INSERT INTO `dnt_smilies` (`id`, `displayorder`, `type`, `code`, `url`) VALUES 
(1, 0, 0, '默认表情', 'default'),
(2, 0, 1, ':O', 'default/0.gif'),
(3, 0, 1, ':~', 'default/1.gif'),
(4, 0, 1, ':-|', 'default/10.gif'),
(5, 0, 1, ':@', 'default/11.gif'),
(6, 0, 1, ':Z', 'default/12.gif'),
(7, 0, 1, ':D', 'default/13.gif'),
(8, 0, 1, ':)', 'default/14.gif'),
(9, 0, 1, ':(', 'default/15.gif'),
(10, 0, 1, ':+', 'default/16.gif'),
(11, 0, 1, ':#', 'default/17.gif'),
(12, 0, 1, ':Q', 'default/18.gif'),
(13, 0, 1, ':T', 'default/19.gif'),
(14, 0, 1, ':*', 'default/2.gif'),
(15, 0, 1, ':P', 'default/20.gif'),
(16, 0, 1, ':-D', 'default/21.gif'),
(17, 0, 1, ':d', 'default/22.gif'),
(18, 0, 1, ':o', 'default/23.gif'),
(19, 0, 1, ':g', 'default/24.gif'),
(20, 0, 1, ':|-)', 'default/25.gif'),
(21, 0, 1, ':!', 'default/26.gif'),
(22, 0, 1, ':L', 'default/27.gif'),
(23, 0, 1, ':giggle', 'default/28.gif'),
(24, 0, 1, ':smoke', 'default/29.gif'),
(25, 0, 1, ':|', 'default/3.gif'),
(26, 0, 1, ':f', 'default/30.gif'),
(27, 0, 1, ':-S', 'default/31.gif'),
(28, 0, 1, ':?', 'default/32.gif'),
(29, 0, 1, ':x', 'default/33.gif'),
(30, 0, 1, ':yun', 'default/34.gif'),
(31, 0, 1, ':8', 'default/35.gif'),
(32, 0, 1, ':!', 'default/36.gif'),
(33, 0, 1, ':!!!', 'default/37.gif'),
(34, 0, 1, ':xx', 'default/38.gif'),
(35, 0, 1, ':bye', 'default/39.gif'),
(36, 0, 1, ':8-)', 'default/4.gif'),
(37, 0, 1, ':pig', 'default/40.gif'),
(38, 0, 1, ':cat', 'default/41.gif'),
(39, 0, 1, ':dog', 'default/42.gif'),
(40, 0, 1, ':hug', 'default/43.gif'),
(41, 0, 1, ':$$', 'default/44.gif'),
(42, 0, 1, ':(!)', 'default/45.gif'),
(43, 0, 1, ':cup', 'default/46.gif'),
(44, 0, 1, ':cake', 'default/47.gif'),
(45, 0, 1, ':li', 'default/48.gif'),
(46, 0, 1, ':bome', 'default/49.gif'),
(47, 0, 1, ':<', 'default/5.gif'),
(48, 0, 1, ':kn', 'default/50.gif'),
(49, 0, 1, ':football', 'default/51.gif'),
(50, 0, 1, ':music', 'default/52.gif'),
(51, 0, 1, ':shit', 'default/53.gif'),
(52, 0, 1, ':coffee', 'default/54.gif'),
(53, 0, 1, ':eat', 'default/55.gif'),
(54, 0, 1, ':pill', 'default/56.gif'),
(55, 0, 1, ':rose', 'default/57.gif'),
(56, 0, 1, ':fade', 'default/58.gif'),
(57, 0, 1, ':kiss', 'default/59.gif'),
(58, 0, 1, ':$', 'default/6.gif'),
(59, 0, 1, ':heart:', 'default/60.gif'),
(60, 0, 1, ':break:', 'default/61.gif'),
(61, 0, 1, ':metting:', 'default/62.gif'),
(62, 0, 1, ':gift:', 'default/63.gif'),
(63, 0, 1, ':phone:', 'default/64.gif'),
(64, 0, 1, ':time:', 'default/65.gif'),
(65, 0, 1, ':email:', 'default/66.gif'),
(66, 0, 1, ':TV:', 'default/67.gif'),
(67, 0, 1, ':sun:', 'default/68.gif'),
(68, 0, 1, ':moon:', 'default/69.gif'),
(69, 0, 1, ':X', 'default/7.gif'),
(70, 0, 1, ':strong:', 'default/70.gif'),
(71, 0, 1, ':weak:', 'default/71.gif'),
(72, 0, 1, ':share:', 'default/72.gif'),
(73, 0, 1, ':v:', 'default/73.gif'),
(74, 0, 1, ':duoduo:', 'default/74.gif'),
(75, 0, 1, ':MM:', 'default/75.gif'),
(76, 0, 1, ':hh:', 'default/76.gif'),
(77, 0, 1, ':mm:', 'default/77.gif'),
(78, 0, 1, ':beer:', 'default/78.gif'),
(79, 0, 1, ':pesi:', 'default/79.gif'),
(80, 0, 1, ':Zz', 'default/8.gif'),
(81, 0, 1, ':xigua:', 'default/80.gif'),
(82, 0, 1, ':yu:', 'default/81.gif'),
(83, 0, 1, ':duoyun:', 'default/82.gif'),
(84, 0, 1, ':snowman:', 'default/83.gif'),
(86, 0, 1, ':xingxing:', 'default/84.gif'),
(87, 0, 1, ':male:', 'default/85.gif'),
(88, 0, 1, ':female:', 'default/86.gif'),
(89, 0, 1, ':t(', 'default/9.gif');

DROP TABLE IF EXISTS `dnt_spaceattachments`;
CREATE TABLE `dnt_spaceattachments` (
  `aid` int(11) NOT NULL auto_increment,
  `uid` int(11) NOT NULL default '0',
  `spacepostid` int(11) NOT NULL default '0',
  `postdatetime` datetime NOT NULL,
  `filename` varchar(100) NOT NULL default '',
  `filetype` varchar(50) NOT NULL default '',
  `filesize` int(11) NOT NULL default '0',
  `attachment` varchar(100) NOT NULL default '',
  `downloads` int(11) NOT NULL default '0',
  PRIMARY KEY  (`aid`),
  KEY `spacepostiduserid` (`spacepostid`,`uid`),
  KEY `tid` (`spacepostid`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_spacecategories`;
CREATE TABLE `dnt_spacecategories` (
  `categoryid` int(11) NOT NULL auto_increment,
  `title` varchar(50) NOT NULL default '',
  `uid` int(11) NOT NULL default '0',
  `description` text NOT NULL,
  `typeid` int(11) NOT NULL default '0',
  `categorycount` int(11) NOT NULL default '0',
  `displayorder` int(11) NOT NULL default '0',
  PRIMARY KEY  (`categoryid`),
  UNIQUE KEY `userid` (`uid`,`displayorder`,`categoryid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_spacecomments`;
CREATE TABLE `dnt_spacecomments` (
  `commentid` int(11) NOT NULL auto_increment,
  `postid` int(11) NOT NULL default '0',
  `author` varchar(50) NOT NULL default '',
  `email` varchar(100) NOT NULL default '',
  `url` varchar(255) NOT NULL default '',
  `ip` varchar(100) NOT NULL default '',
  `postdatetime` datetime NOT NULL,
  `content` text NOT NULL,
  `parentid` int(11) NOT NULL,
  `uid` int(11) NOT NULL,
  `posttitle` varchar(60) NOT NULL,
  PRIMARY KEY  (`commentid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_spaceconfigs`;
CREATE TABLE `dnt_spaceconfigs` (
  `spaceid` int(11) NOT NULL auto_increment,
  `userid` int(11) NOT NULL,
  `spacetitle` varchar(100) NOT NULL default '',
  `description` varchar(200) NOT NULL default '',
  `blogdispmode` tinyint(3) unsigned NOT NULL default '0',
  `bpp` int(11) NOT NULL default '16',
  `commentpref` tinyint(3) unsigned NOT NULL default '0',
  `messagepref` tinyint(3) unsigned NOT NULL default '0',
  `rewritename` varchar(100) NOT NULL default '',
  `themeid` int(11) NOT NULL default '0',
  `themepath` varchar(50) NOT NULL default '',
  `postcount` int(11) NOT NULL default '0',
  `commentcount` int(11) NOT NULL default '0',
  `visitedtimes` int(11) NOT NULL default '0',
  `createdatetime` datetime NOT NULL,
  `updatedatetime` datetime NOT NULL,
  `defaulttab` int(11) NOT NULL default '0',
  `status` int(11) NOT NULL default '0',
  PRIMARY KEY  (`spaceid`),
  UNIQUE KEY `userid` (`userid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_spacecustomizepanels`;
CREATE TABLE `dnt_spacecustomizepanels` (
  `moduleid` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `panelcontent` varchar(16) NOT NULL,
  PRIMARY KEY  (`moduleid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_spacelinks`;
CREATE TABLE `dnt_spacelinks` (
  `linkid` int(11) NOT NULL auto_increment,
  `userid` int(11) NOT NULL,
  `linktitle` varchar(50) NOT NULL,
  `linkurl` varchar(255) NOT NULL,
  `description` varchar(200) default NULL,
  PRIMARY KEY  (`linkid`),
  KEY `linkiduserid` (`linkid`,`userid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_spacemoduledefs`;
CREATE TABLE `dnt_spacemoduledefs` (
  `moduledefid` int(11) NOT NULL auto_increment,
  `modulename` varchar(20) NOT NULL,
  `cachetime` int(11) NOT NULL,
  `configfile` varchar(100) NOT NULL,
  `controller` varchar(255) default NULL,
  PRIMARY KEY  (`moduledefid`),
  KEY `configfile` (`configfile`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_spacemoduledefs` (`modulename`, `cachetime`, `configfile`, `controller`) VALUES 
 ('友情链接', 0, 'builtin_linkmodule.xml', 'Discuz.Space.Modules.LinkModule, Discuz.Web'),
 ('个人信息', 0, 'builtin_userinfomodule.xml', 'Discuz.Space.Modules.builtin.forum.UserInfoModule, Discuz.Web'),
 ('帖子调用', 0, 'builtin_showtopicmodule.xml', 'Discuz.Space.Modules.builtin.forum.ShowTopicModule, Discuz.Web'),
 ('日历', 0, 'builtin_calendarmodule.xml', 'Discuz.Space.Modules.CalendarModule, Discuz.Web'),
 ('用户菜单', 0, 'builtin_leftmenumodule.xml', 'Discuz.Space.Modules.LeftMenuModule, Discuz.Web'),
 ('最新评论', 0, 'builtin_newcommentmodule.xml', 'Discuz.Space.Modules.NewCommentModule, Discuz.Web'),
 ('最新文章', 0, 'builtin_newpostmodule.xml', 'Discuz.Space.Modules.NewPostModule, Discuz.Web'),
 ('文章列表', 0, 'builtin_postlistmodule.xml', 'Discuz.Space.Modules.PostListModule, Discuz.Web'),
 ('便条纸', 0, 'builtin_notepadmodule.xml', 'Discuz.Space.Modules.builtin.notepad, Discuz.Web'),
 ('数据统计', 0, 'builtin_statisticmodule.xml', 'Discuz.Space.Modules.StatisticModule, Discuz.Web'),
 ('我的相册', 0, 'builtin_showalbummodule.xml', 'Discuz.Web.space.modules.builtin.album.ShowAlbumModule, Discuz.Web'),
 ('自定义面板', 0, 'builtin_customizepanel.xml', 'Discuz.Space.Modules.builtin.CustomizePanel, Discuz.Web');

DROP TABLE IF EXISTS `dnt_spacemodules`;
CREATE TABLE `dnt_spacemodules` (
  `moduleid` int(11) NOT NULL,
  `tabid` int(11) NOT NULL,
  `uid` int(11) NOT NULL default '0',
  `moduledefid` int(11) NOT NULL,
  `panename` varchar(10) NOT NULL,
  `displayorder` int(11) NOT NULL,
  `userpref` text,
  `val` tinyint(3) unsigned NOT NULL,
  `moduleurl` varchar(255) NOT NULL,
  `moduletype` tinyint(3) unsigned NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_spacepostcategories`;
CREATE TABLE `dnt_spacepostcategories` (
  `id` int(11) NOT NULL auto_increment,
  `postid` int(11) NOT NULL default '0',
  `categoryid` int(11) NOT NULL default '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_spaceposts`;
CREATE TABLE `dnt_spaceposts` (
  `postid` int(11) NOT NULL auto_increment,
  `author` varchar(40) NOT NULL,
  `uid` int(11) NOT NULL,
  `postdatetime` datetime NOT NULL,
  `content` longtext NOT NULL,
  `title` varchar(120) NOT NULL,
  `category` varchar(255) NOT NULL,
  `poststatus` tinyint(3) unsigned NOT NULL,
  `commentstatus` tinyint(3) unsigned NOT NULL,
  `postupdatetime` datetime NOT NULL,
  `commentcount` int(11) NOT NULL,
  `views` int(11) NOT NULL default '0',
  PRIMARY KEY  (`postid`),
  KEY `postidcommentcount` (`postid`,`commentcount`),
  KEY `postiduid` (`postid`,`uid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_spaceposttags`;
CREATE TABLE `dnt_spaceposttags` (
  `tagid` int(11) NOT NULL default '0',
  `spacepostid` int(11) NOT NULL default '0'
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_spacetabs`;
CREATE TABLE `dnt_spacetabs` (
  `tabid` int(11) NOT NULL,
  `uid` int(11) NOT NULL,
  `displayorder` int(11) NOT NULL,
  `tabname` varchar(50) NOT NULL,
  `iconfile` varchar(50) default NULL,
  `template` varchar(50) default NULL,
  KEY `uid` (`uid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_spacethemes`;
CREATE TABLE `dnt_spacethemes` (
  `themeid` int(11) NOT NULL auto_increment,
  `directory` varchar(100) NOT NULL default '',
  `name` varchar(50) NOT NULL default '',
  `type` int(11) NOT NULL default '0',
  `author` varchar(100) NOT NULL default '',
  `createdate` varchar(50) NOT NULL default '',
  `copyright` varchar(100) NOT NULL default '',
  PRIMARY KEY  (`themeid`),
  KEY `type` (`type`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_spacethemes` (`directory`, `name`, `type`, `author`, `createdate`, `copyright`) VALUES 
('', '默认皮肤', 0, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.'),
('', '部落窝', 0, '部落窝', '2007-08-23', 'Copyright©2005-2007 BlogWorld. All rights reserved'),
('default', '默认皮肤', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.'),
('fadgirl', '时尚女孩', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.'),
('dream', '梦幻', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.'),
('patina', '古色古香', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.'),
('romantic', '浪漫传奇', 1, 'Discuz!NT', '2007-08-23', 'Copyright 2007 Comsenz Inc.'),
('window', '窗', 2, '部落窝', '2007-08-23', 'Copyright©2005-2007 BlogWorld. All rights reserved'),
('orange', '桔色', 2,  '部落窝', '2007-08-23', 'Copyright©2005-2007 BlogWorld. All rights reserved'),
('green', '幽静夜空', 2, '部落窝', '2007-08-23', 'Copyright©2005-2007 BlogWorld. All rights reserved'),
('bird', '山水麻雀', 2,  '部落窝', '2007-08-23', 'Copyright©2005-2007 BlogWorld. All rights reserved'),
('promise', '约定一生', 2,  '部落窝', '2007-08-23', 'Copyright©2005-2007 BlogWorld. All rights reserved');

DROP TABLE IF EXISTS `dnt_statistics`;
CREATE TABLE `dnt_statistics` (
  `totaltopic` int(11) NOT NULL,
  `totalpost` int(11) NOT NULL,
  `totalusers` int(11) NOT NULL,
  `lastusername` varchar(20) NOT NULL,
  `lastuserid` int(11) NOT NULL,
  `highestonlineusercount` int(11) default 0,
  `highestonlineusertime` datetime default NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk; 

INSERT INTO `dnt_statistics` (`totaltopic`, `totalpost`, `totalusers`, `lastusername`, `lastuserid`, `highestonlineusercount`, `highestonlineusertime`) VALUES 
(0, 0, 1,'',0,0,now());

DROP TABLE IF EXISTS `dnt_tablelist`;
CREATE TABLE `dnt_tablelist` (
  `id` int(11) NOT NULL auto_increment,
  `createdatetime` datetime NOT NULL,
  `description` varchar(50) NOT NULL default '',
  `mintid` int(11) NOT NULL default '-1',
  `maxtid` int(11) NOT NULL default '-1',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_tablelist` (`createdatetime` ,`description`, `mintid`, `maxtid`) VALUES 
(now(),'dnt_posts1',1,0);

DROP TABLE IF EXISTS `dnt_tags`;
CREATE TABLE `dnt_tags` (
  `tagid` int(11) NOT NULL,
  `tagname` varchar(10) NOT NULL,
  `userid` int(11) NOT NULL default '0',
  `postdatetime` datetime NOT NULL,
  `orderid` int(11) NOT NULL default '0',
  `color` varchar(6) NOT NULL,
  `count` int(11) NOT NULL default '0',
  `fcount` int(11) NOT NULL default '0',
  `pcount` int(11) NOT NULL default '0',
  `scount` int(11) NOT NULL default '0',
  `vcount` int(11) NOT NULL default '0',
  PRIMARY KEY  (`tagid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_templates`;
CREATE TABLE `dnt_templates` (
  `templateid` smallint(6) NOT NULL auto_increment,
  `directory` varchar(100) NOT NULL,
  `name` varchar(50) NOT NULL,
  `author` varchar(100) NOT NULL,
  `createdate` varchar(50) NOT NULL,
  `ver` varchar(100) NOT NULL,
  `fordntver` varchar(100) NOT NULL,
  `copyright` varchar(100) NOT NULL,
  PRIMARY KEY  (`templateid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_templates` (`directory`, `name`, `author`, `createdate`, `ver`, `fordntver`, `copyright`) VALUES 
('default','Default','Discuz!NT','2007-11-06','2.0','2.0','Copyright 2007 Comsenz Inc.'),
('pink','Pink','Discuz!NT','2007-11-06','2.0','2.0','Copyright 2007 Comsenz Inc.'),
('music','Music','Discuz!NT','2007-11-06','2.0','2.0','Copyright 2007 Comsenz Inc.');

DROP TABLE IF EXISTS `dnt_topicidentify`;
CREATE TABLE `dnt_topicidentify` (
  `identifyid` int(11) NOT NULL auto_increment,
  `name` varchar(50) default NULL,
  `filename` varchar(50) NOT NULL,
  PRIMARY KEY  (`identifyid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_topicidentify` (`name`, `filename`) VALUES 
('找抽帖', 'zc.gif'),
('变态帖', 'bt.gif'),
('吵架帖', 'cj.gif'),
('炫耀帖', 'xy.gif'),
('炒作帖', 'cz.gif'),
('委琐帖', 'ws.gif'),
('火星帖', 'hx.gif'),
('精彩帖', 'jc.gif'),
('无聊帖', 'wl.gif'),
('温情帖', 'wq.gif'),
('XX帖', 'xx.gif'),
('跟风帖', 'gf.gif'),
('垃圾帖', 'lj.gif'),
('推荐帖', 'tj.gif'),
('搞笑帖', 'gx.gif'),
('悲情帖', 'bq.gif'),
('牛帖', 'nb.gif');

DROP TABLE IF EXISTS `dnt_topics`;
CREATE TABLE `dnt_topics` (
  `tid` int(11) NOT NULL auto_increment,
  `fid` smallint(6) NOT NULL,
  `iconid` tinyint(3) unsigned NOT NULL default '0',
  `typeid` tinyint(3) unsigned NOT NULL default '0',
  `readperm` int(11) NOT NULL default '0',
  `price` smallint(6) NOT NULL default '0',
  `poster` varchar(20) NOT NULL default '',
  `posterid` int(11) NOT NULL default '0',
  `title` varchar(60) NOT NULL,
  `postdatetime` datetime NOT NULL,
  `lastpost` datetime NOT NULL,
  `lastpostid` int(11) NOT NULL default '0',
  `lastposter` varchar(20) NOT NULL default '',
  `lastposterid` int(11) NOT NULL default '0',
  `views` int(11) NOT NULL default '0',
  `replies` int(11) NOT NULL default '0',
  `displayorder` int(11) NOT NULL default '0',
  `highlight` text NOT NULL,
  `digest` tinyint(3) unsigned NOT NULL default '0',
  `rate` tinyint(3) unsigned NOT NULL default '0',
  `hide` int(11) NOT NULL default '0',
  `poll` int(11) NOT NULL default '0',
  `attachment` int(11) NOT NULL default '0',
  `moderated` tinyint(3) unsigned NOT NULL default '0',
  `closed` int(11) NOT NULL default '0',
  `magic` int(11) NOT NULL default '0',
  `identify` int(11) NOT NULL default '0',
  PRIMARY KEY  (`tid`),
  UNIQUE KEY `list` (`fid`,`displayorder`,`lastpostid`),
  KEY `displayorder` (`displayorder`),
  KEY `displayorder_fid` (`displayorder`,`fid`),
  KEY `fid` (`fid`),
  KEY `fid_displayorder` (`fid`,`displayorder`),
  KEY `list_date` (`fid`,`displayorder`,`postdatetime`,`lastpostid`),
  KEY `list_replies` (`fid`,`displayorder`,`postdatetime`,`replies`),
  KEY `list_tid` (`fid`,`displayorder`,`tid`),
  KEY `list_views` (`fid`,`displayorder`,`postdatetime`,`views`),
  KEY `tid` (`tid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_topictagcaches`;
CREATE TABLE `dnt_topictagcaches` (
  `tid` int(11) NOT NULL default '0',
  `linktid` int(11) NOT NULL default '0',
  `linktitle` varchar(60) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_topictags`;
CREATE TABLE `dnt_topictags` (
  `tagid` int(11) NOT NULL default '0',
  `tid` int(11) NOT NULL default '0'
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_topictypes`;
CREATE TABLE `dnt_topictypes` (
  `typeid` int(11) NOT NULL auto_increment,
  `displayorder` int(11) NOT NULL default '0',
  `name` varchar(30) NOT NULL default '',
  `description` text NOT NULL,
  KEY `typeid` (`typeid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_userfields`;
CREATE TABLE `dnt_userfields` (
  `uid` int(11) NOT NULL,
  `website` varchar(80) NOT NULL default '',
  `icq` varchar(12) NOT NULL default '',
  `qq` varchar(12) NOT NULL default '',
  `yahoo` varchar(40) NOT NULL default '',
  `msn` varchar(40) NOT NULL default '',
  `skype` varchar(40) NOT NULL default '',
  `location` varchar(50) NOT NULL default '',
  `customstatus` varchar(50) NOT NULL default '',
  `avatar` varchar(255) NOT NULL default 'avatars\\common\\0.gif',
  `avatarwidth` int(11) NOT NULL default '60',
  `avatarheight` int(11) NOT NULL default '60',
  `medals` text NOT NULL,
  `bio` text NOT NULL,
  `signature` text NOT NULL,
  `sightml` text NOT NULL,
  `authstr` varchar(20) NOT NULL default '',
  `authtime` datetime NOT NULL,
  `authflag` tinyint(3) unsigned NOT NULL default '0',
  `realname` varchar(10) default '',
  `idcard` varchar(20) default '',
  `mobile` varchar(20) default '',
  `phone` varchar(20) default '',
  PRIMARY KEY  (`uid`)
) ENGINE=MyISAM DEFAULT CHARSET=gbk;

DROP TABLE IF EXISTS `dnt_usergroups`;
CREATE TABLE `dnt_usergroups` (
  `groupid` smallint(6) NOT NULL auto_increment,
  `radminid` int(11) NOT NULL,
  `type` smallint(6) default '0',
  `system` smallint(6) NOT NULL default '0',
  `grouptitle` varchar(50) NOT NULL,
  `creditshigher` int(11) NOT NULL,
  `creditslower` int(11) NOT NULL,
  `stars` int(11) NOT NULL,
  `color` varchar(7) NOT NULL,
  `groupavatar` varchar(60) NOT NULL,
  `readaccess` int(11) NOT NULL,
  `allowvisit` int(11) NOT NULL,
  `allowpost` int(11) NOT NULL,
  `allowreply` int(11) NOT NULL,
  `allowpostpoll` int(11) NOT NULL,
  `allowdirectpost` int(11) NOT NULL,
  `allowgetattach` int(11) NOT NULL,
  `allowpostattach` int(11) NOT NULL,
  `allowvote` int(11) NOT NULL,
  `allowmultigroups` int(11) NOT NULL,
  `allowsearch` int(11) NOT NULL,
  `allowavatar` int(11) NOT NULL,
  `allowcstatus` int(11) NOT NULL,
  `allowuseblog` int(11) NOT NULL,
  `allowinvisible` int(11) NOT NULL,
  `allowtransfer` int(11) NOT NULL,
  `allowsetreadperm` int(11) NOT NULL,
  `allowsetattachperm` int(11) NOT NULL,
  `allowhidecode` int(11) NOT NULL,
  `allowhtml` int(11) NOT NULL,
  `allowcusbbcode` int(11) NOT NULL,
  `allownickname` int(11) NOT NULL,
  `allowsigbbcode` int(11) NOT NULL,
  `allowsigimgcode` int(11) NOT NULL,
  `allowviewpro` int(11) NOT NULL,
  `allowviewstats` int(11) NOT NULL,
  `disableperiodctrl` int(11) NOT NULL,
  `reasonpm` int(11) NOT NULL,
  `maxprice` smallint(6) NOT NULL,
  `maxpmnum` smallint(6) NOT NULL,
  `maxsigsize` smallint(6) NOT NULL,
  `maxattachsize` int(11) NOT NULL,
  `maxsizeperday` int(11) NOT NULL,
  `attachextensions` varchar(100) NOT NULL,
  `raterange` text NOT NULL,
  `allowspace` smallint(6) NOT NULL default '0',
  `maxspaceattachsize` int(11) NOT NULL default '0',
  `maxspacephotosize` int(11) NOT NULL default '0',
  PRIMARY KEY  (`groupid`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

INSERT INTO `dnt_usergroups` (`radminid`, `type`, `system`, `grouptitle`, `creditshigher`, `creditslower`, `stars`, `color`, `groupavatar`, `readaccess`, `allowvisit`, `allowpost`, `allowreply`, `allowpostpoll`, `allowdirectpost`, `allowgetattach`, `allowpostattach`, `allowvote`, `allowmultigroups`, `allowsearch`, `allowavatar`, `allowcstatus`, `allowuseblog`, `allowinvisible`, `allowtransfer`, `allowsetreadperm`, `allowsetattachperm`, `allowhidecode`, `allowhtml`, `allowcusbbcode`, `allownickname`, `allowsigbbcode`, `allowsigimgcode`, `allowviewpro`, `allowviewstats`, `disableperiodctrl`, `reasonpm`, `maxprice`, `maxpmnum`, `maxsigsize`, `maxattachsize`, `maxsizeperday`, `attachextensions`, `raterange`, `allowspace`, `maxspaceattachsize`, `maxspacephotosize`) VALUES 
(1, 0, 1, '管理员', 0, 0, 9, '', '', 255, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 30, 200, 500, 99999999, 99999999, '', '1,True,extcredits1,威望,-50,50,300|2,False,extcredits2,金钱,-50,50,300|3,False,extcredits3,,,,|4,False,extcredits4,,,,|5,False,extcredits5,,,,|6,False,extcredits6,,,,|7,False,extcredits7,,,,|8,False,extcredits8,,,,', 1, 99999999, 99999999),
(2, 0, 1, '超级版主', 0, 0, 8, '', '', 255, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 20, 120, 300, 99999999, 99999999, '', '1,True,extcredits1,威望,-50,50,100|2,False,extcredits2,金钱,-30,30,50|3,False,extcredits3,,,,|4,False,extcredits4,,,,|5,False,extcredits5,,,,|6,False,extcredits6,,,,|7,False,extcredits7,,,,|8,False,extcredits8,,,,', 1, 99999999, 99999999),
(3, 0, 1, '版主', 0, 0, 7, '', '', 200, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 10, 80, 200, 4194304, 33554432, '', '1,True,extcredits1,威望,-30,30,50|2,False,extcredits2,金钱,-10,10,30|3,False,extcredits3,,,,|4,False,extcredits4,,,,|5,False,extcredits5,,,,|6,False,extcredits6,,,,|7,False,extcredits7,,,,|8,False,extcredits8,,,,', 1, 33554432, 33554432),
(0, 0, 1, '禁止发言', 0, 0, 0, '', '', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 0, 0, 0),
(0, 0, 1, '禁止访问', 0, 0, 0, '', '', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 0, 0, 0),
(0, 0, 1, '禁止 IP', 0, 0, 0, '', '', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 0, 0, 0),
(0, 0, 1, '游客', 0, 0, 0, '', '', 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 0, 0, 0),
(0, 0, 1, '等待验证会员', 0, 0, 0, '', '', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, '', '', 0, 0, 0),
(0, 0, 0, '乞丐', -9999999, 0, 0, '', '', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 0, 0, 0),
(0, 0, 0, '新手上路', 0, 50, 1, '', '', 10, 1, 1, 1, 0, 0, 1, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 20, 80, 524288, 1048576, '', '', 1, 1048576, 1048576),
(0, 0, 0, '注册会员', 50, 200, 2, '', '', 20, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 30, 100, 1048576, 2097152, '', '', 1, 2097152, 2097152),
(0, 0, 0, '中级会员', 200, 500, 3, '', '', 30, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 50, 150, 2097152, 4194304, '', '', 1, 4194304, 4194304),
(0, 0, 0, '高级会员', 500, 1000, 4, '', '', 50, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 60, 200, 4194304, 8388608, '', '', 1, 8388608, 8388608),
(0, 0, 0, '金牌会员', 1000, 3000, 6, '', '', 70, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 20, 80, 300, 4194304, 16777216, '', '', 1, 16777216, 16777216),
(0, 0, 0, '论坛元老', 3000, 9999999, 8, '', '', 100, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 100, 500, 4194304, 33554432, '', '', 1, 33554432, 33554432);

DROP TABLE IF EXISTS `dnt_users`;
CREATE TABLE `dnt_users` (
  `uid` int(11) NOT NULL auto_increment,
  `username` varchar(20) NOT NULL default '',
  `nickname` varchar(20) NOT NULL default '',
  `password` varchar(32) NOT NULL default '',
  `secques` varchar(8) NOT NULL default '',
  `spaceid` int(11) NOT NULL default '0',
  `gender` int(11) NOT NULL default '0',
  `adminid` int(11) NOT NULL default '0',
  `groupid` smallint(6) NOT NULL default '0',
  `groupexpiry` int(11) NOT NULL default '0',
  `extgroupids` varchar(60) NOT NULL default '',
  `regip` varchar(15) NOT NULL default '',
  `joindate` datetime NOT NULL,
  `lastip` varchar(15) NOT NULL default '',
  `lastvisit` datetime NOT NULL,
  `lastactivity` datetime NOT NULL,
  `lastpost` datetime NOT NULL,
  `lastpostid` int(11) NOT NULL default '0',
  `lastposttitle` varchar(60) NOT NULL default '',
  `posts` int(11) NOT NULL default '0',
  `digestposts` smallint(6) NOT NULL default '0',
  `oltime` int(11) NOT NULL default '0',
  `pageviews` int(11) NOT NULL default '0',
  `credits` decimal(12,0) NOT NULL default '0',
  `extcredits1` decimal(20,2) NOT NULL default '0.00',
  `extcredits2` decimal(20,2) NOT NULL default '0.00',
  `extcredits3` decimal(20,2) NOT NULL default '0.00',
  `extcredits4` decimal(20,2) NOT NULL default '0.00',
  `extcredits5` decimal(20,2) NOT NULL default '0.00',
  `extcredits6` decimal(20,2) NOT NULL default '0.00',
  `extcredits7` decimal(20,2) NOT NULL default '0.00',
  `extcredits8` decimal(20,2) NOT NULL default '0.00',
  `avatarshowid` int(11) NOT NULL default '0',
  `email` varchar(50) NOT NULL default '',
  `bday` varchar(10) NOT NULL default '',
  `sigstatus` int(11) NOT NULL default '0',
  `tpp` int(11) NOT NULL default '0',
  `ppp` int(11) NOT NULL default '0',
  `templateid` smallint(6) NOT NULL default '0',
  `pmsound` int(11) NOT NULL default '0',
  `showemail` int(11) NOT NULL default '0',
  `newsletter` int(11) NOT NULL default '0',
  `invisible` int(11) NOT NULL default '0',
  `newpm` int(11) NOT NULL default '0',
  `newpmcount` int(11) NOT NULL default '0',
  `accessmasks` int(11) NOT NULL default '0',
  `onlinestate` int(11) NOT NULL default '0',
  PRIMARY KEY  (`uid`),
  KEY `emailsecques` (`username`,`email`,`secques`),
  KEY `password` (`username`,`password`),
  KEY `pwsecques` (`username`,`password`,`secques`),
  KEY `username` (`username`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `dnt_words`;
CREATE TABLE `dnt_words` (
  `id` smallint(6) NOT NULL auto_increment,
  `admin` varchar(20) NOT NULL,
  `find` varchar(255) NOT NULL,
  `replacement` varchar(255) NOT NULL,
  KEY `id` (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=gbk AUTO_INCREMENT=1 ;