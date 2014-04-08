using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Data
{
    public class DbFields
    {
        public const string ADMIN_GROUPS = "[admingid],[alloweditpost],[alloweditpoll],[allowstickthread],[allowmodpost],[allowdelpost],[allowmassprune],[allowrefund],[allowcensorword],[allowviewip],[allowbanip],[allowedituser],[allowmoduser],[allowbanuser],[allowpostannounce],[allowviewlog],[disablepostctrl],[allowviewrealname]";

        public const string ADMIN_VISIT_LOG = "[visitid],[uid],[username],[groupid],[grouptitle],[ip],[postdatetime],[actions],[others]";

        public const string ADVERTISEMENTS = "[advid],[available],[type],[displayorder],[title],[targets],[starttime],[endtime],[code],[parameters]";

        public const string ANNOUNCEMENTS = "[id],[poster],[posterid],[title],[displayorder],[starttime],[endtime],[message]";

        public const string ATTACHMENTS = "[aid],[uid],[tid],[pid],[postdatetime],[readperm],[filename],[description],[filetype],[filesize],[attachment],[downloads],[attachprice],[width],[height]";

        public const string ATTACH_PAYMENT_LOG = "[id],[uid],[username],[aid],[authorid],[postdatetime],[amount],[netamount]";

        public const string ATTACH_TYPES = "[id],[extension],[maxsize]";

        public const string BANNED = "[id],[ip1],[ip2],[ip3],[ip4],[admin],[dateline],[expiration]";

        public const string BBCODES = "[id],[available],[tag],[icon],[example],[params],[nest],[explanation],[replacement],[paramsdescript],[paramsdefvalue]";

        public const string BONUS_LOG = "[tid],[authorid],[answerid],[answername],[pid],[dateline],[bonus],[extid],[isbest]";

        public const string CREDITS_LOG = "[id],[uid],[fromto],[sendcredits],[receivecredits],[send],[receive],[paydate],[operation]";

        public const string CREDITS_LOG_JOIN = "[cl].[id],[cl].[uid],[cl].[fromto],[cl].[sendcredits],[cl].[receivecredits],[cl].[send],[cl].[receive],[cl].[paydate],[cl].[operation]";

        public const string DEBATE_DIGGS = "[tid],[pid],[digger],[diggerid],[diggerip],[diggdatetime]";

        public const string DEBATES = "[tid],[positiveopinion],[negativeopinion],[terminaltime],[positivediggs],[negativediggs]";

        public const string DEBATES_JOIN = "[d].[tid],[d].[positiveopinion],[d].[negativeopinion],[d].[terminaltime],[d].[positivediggs],[d].[negativediggs]";

        public const string FAILED_LOGINS = "[ip],[errcount],[lastupdate]";

        public const string FAVORITES = "[uid],[tid],[typeid]";

        public const string FORUM_FIELDS = "[fid],[password],[icon],[postcredits],[replycredits],[redirect],[attachextensions],[rules],[topictypes],[viewperm],[postperm],[replyperm],[getattachperm],[postattachperm],[moderators],[description],[applytopictype],[postbytopictype],[viewbytopictype],[topictypeprefix],[permuserlist],[seokeywords],[seodescription],[rewritename]";

        public const string FORUM_LINKS = "[id],[displayorder],[name],[url],[note],[logo]";

        public const string FORUMS = "[fid],[parentid],[layer],[pathlist],[parentidlist],[subforumcount],[name],[status],[colcount],[displayorder],[templateid],[topics],[curtopics],[posts],[todayposts],[lasttid],[lasttitle],[lastpost],[lastposterid],[lastposter],[allowsmilies],[allowrss],[allowhtml],[allowbbcode],[allowimgcode],[allowblog],[istrade],[allowpostspecial],[allowspecialonly],[alloweditrules],[allowthumbnail],[allowtag],[recyclebin],[modnewposts],[jammer],[disablewatermark],[inheritedmod],[autoclose]";

        public const string FORUMS_JOIN_FIELDS = "[f].[fid],[f].[parentid],[f].[layer],[f].[pathlist],[f].[parentidlist],[f].[subforumcount],[f].[name],[f].[status],[f].[colcount],[f].[displayorder],[f].[templateid],[f].[topics],[f].[curtopics],[f].[posts],[f].[todayposts],[f].[lasttid],[f].[lasttitle],[f].[lastpost],[f].[lastposterid],[f].[lastposter],[f].[allowsmilies],[f].[allowrss],[f].[allowhtml],[f].[allowbbcode],[f].[allowimgcode],[f].[allowblog],[f].[istrade],[f].[allowpostspecial],[f].[allowspecialonly],[f].[alloweditrules],[f].[allowthumbnail],[f].[allowtag],[f].[recyclebin],[f].[modnewposts],[f].[jammer],[f].[disablewatermark],[f].[inheritedmod],[f].[autoclose],[ff].[password],[ff].[icon],[ff].[postcredits],[ff].[replycredits],[ff].[redirect],[ff].[attachextensions],[ff].[rules],[ff].[topictypes],[ff].[viewperm],[ff].[postperm],[ff].[replyperm],[ff].[getattachperm],[ff].[postattachperm],[ff].[moderators],[ff].[description],[ff].[applytopictype],[ff].[postbytopictype],[ff].[viewbytopictype],[ff].[topictypeprefix],[ff].[permuserlist],[ff].[seokeywords],[ff].[seodescription],[ff].[rewritename]";

        public const string HELP = "[id],[title],[message],[pid],[orderby]";

        public const string LOCATIONS = "[lid],[city],[state],[country],[zipcode]";

        public const string MEDALS = "[medalid],[name],[available],[image]";

        public const string MEDALS_LOG = "[id],[adminname],[adminid],[ip],[postdatetime],[username],[uid],[actions],[medals],[reason]";

        public const string MODERATOR_MANAGE_LOG = "[id],[moderatoruid],[moderatorname],[groupid],[grouptitle],[ip],[postdatetime],[fid],[fname],[tid],[title],[actions],[reason]";

        public const string MODERATORS = "[uid],[fid],[displayorder],[inherited]";

        public const string MY_ATTACHMENTS = "[aid],[uid],[attachment],[description],[postdatetime],[downloads],[filename],[pid],[tid],[extname]";

        public const string MY_POSTS = "[uid],[tid],[pid],[dateline]";

        public const string MY_TOPICS = "[uid],[tid],[dateline]";

        public const string NAVS = "[id],[parentid],[name],[title],[url],[target],[type],[available],[displayorder],[highlight],[level]";

        public const string NOTICES = "[nid],[uid],[type],[new],[posterid],[poster],[note],[postdatetime]";

        public const string ONLINE = "[olid],[userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title],[verifycode],[newpms],[newnotices]";

        public const string ONLINE_LIST = "[groupid],[displayorder],[title],[img]";

        public const string ONLINE_TIME = "[uid],[thismonth],[total],[lastupdate]";

        public const string PAYMENT_LOG = "[id],[uid],[tid],[authorid],[buydate],[amount],[netamount]";

        public const string PAYMENT_LOG_JOIN = "[pl].[id],[pl].[uid],[pl].[tid],[pl].[authorid],[pl].[buydate],[pl].[amount],[pl].[netamount]";

        public const string PMS = "[pmid],[msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message]";

        public const string POLL_OPTIONS = "[polloptionid],[tid],[pollid],[votes],[displayorder],[polloption],[voternames]";

        public const string POLLS = "[pollid],[tid],[displayorder],[multiple],[visible],[maxchoices],[expiration],[uid],[voternames]";

        public const string POST_DEBATE_FIELDS = "[tid],[pid],[opinion],[diggs]";

        public const string POSTID = "[pid],[postdatetime]";

        public const string POSTS = "[pid],[fid],[tid],[parentid],[layer],[poster],[posterid],[title],[postdatetime],[message],[ip],[lastedit],[invisible],[usesig],[htmlon],[smileyoff],[parseurloff],[bbcodeoff],[attachment],[rate],[ratetimes]";

        public const string POSTS_JOIN = "[p].[pid],[p].[fid],[p].[tid],[p].[parentid],[p].[layer],[p].[poster],[p].[posterid],[t].[title],[p].[postdatetime],[p].[message],[p].[ip],[p].[lastedit],[p].[invisible],[p].[usesig],[p].[htmlon],[p].[smileyoff],[p].[parseurloff],[p].[bbcodeoff],[p].[attachment],[p].[rate],[p].[ratetimes]";

        public const string RATE_LOG = "[id],[pid],[uid],[username],[extcredits],[postdatetime],[score],[reason]";

        public const string RATE_LOG_JOIN = "[r].[id],[r].[pid],[r].[uid],[r].[username],[r].[extcredits],[r].[postdatetime],[r].[score],[r].[reason]";

        public const string SCHEDULED_EVENTS = "[scheduleID],[key],[lastexecuted],[servername]";

        public const string SEARCH_CACHES = "[searchid],[keywords],[searchstring],[ip],[uid],[groupid],[postdatetime],[expiration],[topics],[tids]";

        public const string SMILIES = "[id],[displayorder],[type],[code],[url]";

        public const string STATISTICS = "[totaltopic],[totalpost],[totalusers],[lastusername],[lastuserid],[highestonlineusercount],[highestonlineusertime],[yesterdayposts],[highestposts],[highestpostsdate]";

        public const string STATS = "[type],[variable],[count]";

        public const string STAT_VARS = "[type],[variable],[value]";

        public const string TABLE_LIST = "[id],[createdatetime],[description],[mintid],[maxtid]";

        public const string TAGS = "[tagid],[tagname],[userid],[postdatetime],[orderid],[color],[count],[fcount],[pcount],[scount],[vcount],[gcount]";

        public const string TEMPLATES = "[templateid],[directory],[name],[author],[createdate],[ver],[fordntver],[copyright],[templateurl]";

        public const string TOPIC_IDENTIFY = "[identifyid],[name],[filename]";

        public const string TOPICS = "[tid],[fid],[iconid],[typeid],[readperm],[price],[poster],[posterid],[title],[attention],[postdatetime],[lastpost],[lastpostid],[lastposter],[lastposterid],[views],[replies],[displayorder],[highlight],[digest],[rate],[hide],[attachment],[moderated],[closed],[magic],[identify],[special]";

        public const string TOPICS_JOIN = "[t].[tid],[t].[fid],[t].[iconid],[t].[typeid],[t].[readperm],[t].[price],[t].[poster],[t].[posterid],[t].[title],[t].[attention],[t].[postdatetime],[t].[lastpost],[t].[lastpostid],[t].[lastposter],[t].[lastposterid],[t].[views],[t].[replies],[t].[displayorder],[t].[highlight],[t].[digest],[t].[rate],[t].[hide],[t].[attachment],[t].[moderated],[t].[closed],[t].[magic],[t].[identify],[t].[special]";

        public const string TOPIC_TAG_CACHES = "[tid],[linktid],[linktitle]";

        public const string TOPIC_TAGS = "[tagid],[tid]";

        public const string TOPIC_TYPES = "[typeid],[displayorder],[name],[description]";

        public const string USER_FIELDS = "[uid],[website],[icq],[qq],[yahoo],[msn],[skype],[location],[customstatus],[avatar],[avatarwidth],[avatarheight],[medals],[bio],[signature],[sightml],[authstr],[authtime],[authflag],[realname],[idcard],[mobile],[phone],[ignorepm]";

        public const string USER_GROUPS = "[groupid],[radminid],[type],[system],[grouptitle],[creditshigher],[creditslower],[stars],[color],[groupavatar],[readaccess],[allowvisit],[allowpost],[allowreply],[allowpostpoll],[allowdirectpost],[allowgetattach],[allowpostattach],[allowvote],[allowmultigroups],[allowsearch],[allowavatar],[allowcstatus],[allowuseblog],[allowinvisible],[allowtransfer],[allowsetreadperm],[allowsetattachperm],[allowhidecode],[allowhtml],[allowhtmltitle],[allowcusbbcode],[allownickname],[allowsigbbcode],[allowsigimgcode],[allowviewpro],[allowviewstats],[disableperiodctrl],[reasonpm],[maxprice],[maxpmnum],[maxsigsize],[maxattachsize],[maxsizeperday],[attachextensions],[raterange],[allowspace],[maxspaceattachsize],[maxspacephotosize],[allowdebate],[allowbonus],[minbonusprice],[maxbonusprice],[allowtrade],[allowdiggs],[modnewtopics],[modnewposts],[ignoreseccode]";

        public const string USERS = "[uid],[username],[nickname],[password],[secques],[spaceid],[gender],[adminid],[groupid],[groupexpiry],[extgroupids],[regip],[joindate],[lastip],[lastvisit],[lastactivity],[lastpost],[lastpostid],[lastposttitle],[posts],[digestposts],[oltime],[pageviews],[credits],[extcredits1],[extcredits2],[extcredits3],[extcredits4],[extcredits5],[extcredits6],[extcredits7],[extcredits8],[avatarshowid],[email],[bday],[sigstatus],[tpp],[ppp],[templateid],[pmsound],[showemail],[invisible],[newpm],[newpmcount],[accessmasks],[onlinestate],[newsletter],[salt]";

        public const string USERS_JOIN_FIELDS = "[u].[uid],[u].[username],[u].[nickname],[u].[password],[u].[secques],[u].[spaceid],[u].[gender],[u].[adminid],[u].[groupid],[u].[groupexpiry],[u].[extgroupids],[u].[regip],[u].[joindate],[u].[lastip],[u].[lastvisit],[u].[lastactivity],[u].[lastpost],[u].[lastpostid],[u].[lastposttitle],[u].[posts],[u].[digestposts],[u].[oltime],[u].[pageviews],[u].[credits],[u].[extcredits1],[u].[extcredits2],[u].[extcredits3],[u].[extcredits4],[u].[extcredits5],[u].[extcredits6],[u].[extcredits7],[u].[extcredits8],[u].[avatarshowid],[u].[email],[u].[bday],[u].[sigstatus],[u].[tpp],[u].[ppp],[u].[templateid],[u].[pmsound],[u].[showemail],[u].[invisible],[u].[newpm],[u].[newpmcount],[u].[accessmasks],[u].[onlinestate],[u].[newsletter], [uf].[website],[uf].[icq],[uf].[qq],[uf].[yahoo],[uf].[msn],[uf].[skype],[uf].[location],[uf].[customstatus],[uf].[avatar],[uf].[avatarwidth],[uf].[avatarheight],[uf].[medals],[uf].[bio],[uf].[signature],[uf].[sightml],[uf].[authstr],[uf].[authtime],[uf].[authflag],[uf].[realname],[uf].[idcard],[uf].[mobile],[uf].[phone],[uf].[ignorepm],[u].[salt]";

        public const string WORDS = "[id],[admin],[find],[replacement]";

    }
}
