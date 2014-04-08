using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Discuz.Entity;

namespace Discuz.Web.Services.API
{
    #region auth
    [XmlRoot("auth_createToken_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TokenInfo
    {
        [XmlElement("session_key")]
        public string Token;
    }

    [XmlRoot ("auth_getSession_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
	public class SessionInfo
	{
		[XmlElement("session_key")]
		public string SessionKey;

		[XmlElement("uid")]
		public long UId;
        
        [XmlElement("user_name")]
        public string UserName;

        [XmlElement("expires")]
		public long Expires;

        //[XmlIgnore ()]
        //public bool IsInfinite
        //{
        //    get { return Expires == 0; }
        //}	
		
		public SessionInfo ()
		{}

		// use this if you want to create a session based on infinite session
		// credentials
		public SessionInfo (string session_key, long uid)
		{
			this.SessionKey = session_key;
			this.UId = uid;
			this.Expires = 0;
		}
	}

    [XmlRoot("auth_register_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class RegisterResponse
    {
        [XmlText]
        public int Uid;

        //[XmlAttribute("list")]
        //public bool List;
    }

    [XmlRoot("auth_encodePassword_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class EncodePasswordResponse
    {
        [XmlText]
        public string Password;
    }
    #endregion

    [XmlRoot("users_setInfo_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class SetInfoResponse
    {
        [XmlText]
        public int Successfull;
    }

    [XmlRoot("users_setExtCredits_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class SetExtCreditsResponse
    {
        [XmlText]
        public int Successfull;
    }

    [XmlRoot("users_getID_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class GetIDResponse
    {
        [XmlText]
        public int UId;
    }

    [XmlRoot("users_changePassword_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class ChangePasswordResponse
    {
        [XmlText]
        public int Successfull;
    }

    /*
    [XmlRoot("photos_getAlbums_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
	public class AlbumsResponse
	{
		[XmlElement ("album")]
		public Album[] album_array;

		[XmlIgnore ()]
		public Album[] Albums
		{
			get { return album_array ?? new Album[0]; }
		}

		[XmlAttribute ("list")]
		public bool List;
	}

	[XmlRoot ("photos_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
	public class PhotosResponse
	{
		[XmlElement ("photo")]
		public Photo[] photo_array;

		[XmlIgnore ()]
		public Photo[] Photos
		{
			get { return photo_array ?? new Photo[0]; }
		}

		[XmlAttribute ("list")]
		public bool List;
	}

	[XmlRoot ("photos_getTags_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
	public class PhotoTagsResponse
	{
		[XmlElement ("photo_tag")]
		public Tag[] tag_array;

		public Tag[] Tags
		{
			get { return tag_array ?? new Tag[0]; }
		}

		[XmlAttribute ("list")]
		public bool List;
	}
	[XmlRoot ("groups_get_response", Namespace = "http://nt.discuz.net/api/")]
	public class GroupsResponse
	{
		[XmlElement ("group")]
		public Group[] group_array;

		public Group[] Groups
		{
			get { return group_array ?? new Group[0]; }
		}

		[XmlAttribute ("list")]
		public bool List;
	}

	[XmlRoot ("groups_getMembers_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
	public class GroupMembersResponse
	{
		[XmlElement ("members")]
		public PeopleList Members;

		[XmlElement ("admins")]
		public PeopleList Admins;

		[XmlElement ("officers")]
		public PeopleList Officers;

		[XmlElement ("not_replied")]
		public PeopleList NotReplied;
	}
    */


    [XmlRoot("users_getInfo_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class UserInfoResponse
    {
        [XmlElement("user")]
        [JsonPropertyAttribute("user")]
        public User[] user_array;

        [JsonIgnore]
        public User[] Users
        {
#if NET1
			get { return user_array  == null ? new User[0] : user_array; }
#else
            get { return user_array ?? new User[0]; }
#endif
        }

        [XmlAttribute("list")]
        [JsonIgnore]
        public bool List;
    }

    [XmlRoot("users_getLoggedInUser_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class LoggedInUserResponse
    {
        [XmlText]
        public int Uid;

        //[XmlAttribute("list")]
        //public bool List;
    }



    public class ArgResponse
    {
        [XmlElement("arg")]
        public Arg[] Args;
 
        [XmlAttribute("list")]
        public bool List;
    }

    /*
    [XmlRoot ("events_get_response", Namespace="http://nt.discuz.net/api/", IsNullable=false)]
    public class EventsResponse 
    {
        [XmlElement ("event")]
        public Event[] event_array;

        public Event[] Events
        {
            get { return event_array ?? new Event[0]; }
        }
	
        [XmlAttribute ("list")]
        public bool List;
    }

    [XmlRoot ("events_getMembers_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class EventMembersResponse
    {
        [XmlElement ("attending")]
        public PeopleList Attending;

        [XmlElement ("unsure")]
        public PeopleList Unsure;

        [XmlElement ("declined")]
        public PeopleList Declined;

        [XmlElement ("not_replied")]
        public PeopleList NotReplied;
    }

    [XmlRoot ("friends_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class FriendsResponse
    {
        [XmlElement ("uid")]
        public int[] uids;

        [XmlIgnore ()]
        public int[] UIds
        {
            get { return uids ?? new int[0]; }
        }
    }

    [XmlRoot ("friends_areFriends_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class AreFriendsResponse
    { 
        [XmlElement ("friend_info")]
        public FriendInfo[] friend_infos;
    }
     * */


    #region Topics
    [XmlRoot("topics_create_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicCreateResponse
    {
        [JsonPropertyAttribute("topic_id")]
        [XmlElement("topic_id")]
        public int TopicId;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public string Url;

        [JsonPropertyAttribute("need_audit")]
        [XmlElement("need_audit")]
        public bool NeedAudit;

        //[XmlAttribute("list")]
        //public bool List;
    }

    [XmlRoot("topics_reply_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicReplyResponse
    {
        [JsonPropertyAttribute("post_id")]
        [XmlElement("post_id")]
        public int PostId;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public string Url;

        [JsonPropertyAttribute("need_audit")]
        [XmlElement("need_audit")]
        public bool NeedAudit;

    }

    [XmlRoot("topics_getRecentReplies_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicGetRencentRepliesResponse
    {
        [XmlElement("count")]
        [JsonProperty("count")]
        public int Count;

        [XmlElement("post")]
        [JsonProperty("posts")]
        public Post[] Posts;

        [JsonIgnore]
        [XmlAttribute("list")]
        public bool List;

    }

    [XmlRoot("topics_getList_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicGetListResponse
    {
        [XmlElement("count")]
        [JsonProperty("count")]
        public int Count;

        [XmlElement("topic")]
        [JsonProperty("topics")]
        public ForumTopic[] Topics;

        [JsonIgnore]
        [XmlAttribute("list")]
        public bool List;
    }

    [XmlRoot("topics_delete_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicDeleteResponse
    {
        [XmlText]
        public int Successfull;
    }

    [XmlRoot("topics_edit_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicEditResponse
    {
        [XmlText]
        public int Successfull;
    }

    [XmlRoot("topics_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicGetResponse
    {
        [JsonPropertyAttribute("topic_id")]
        [XmlElement("topic_id")]
        public int TopicId;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public string Url;

        [JsonPropertyAttribute("title")]
        [XmlElement("title", IsNullable = false)]
        public string Title;

        [JsonPropertyAttribute("fid")]
        [XmlElement("fid", IsNullable = false)]
        public int Fid;

        [JsonPropertyAttribute("icon_id")]
        [XmlElement("icon_id", IsNullable = false)]
        public int Iconid;

        [JsonPropertyAttribute("tags")]
        [XmlElement("tags", IsNullable = true)]
        public string Tags;

        [JsonPropertyAttribute("author")]
        [XmlElement("author")]
        public string Author = string.Empty;

        [JsonPropertyAttribute("author_id")]
        [XmlElement("author_id")] 
        public int AuthorId;

        [JsonPropertyAttribute("reply_count")]
        [XmlElement("reply_count")]
        public int ReplyCount;

        [JsonPropertyAttribute("view_count")]
        [XmlElement("view_count")]
        public int ViewCount;

        [JsonPropertyAttribute("last_post_time")]
        [XmlElement("last_post_time")]
        public string LastPostTime = string.Empty;

        [JsonPropertyAttribute("last_poster_id")]
        [XmlElement("last_poster_id")]
        public int LastPosterId;

        [XmlElement("post")]
        [JsonProperty("posts")]
        public Post[] Posts;

        [XmlElement("attachment")]
        [JsonProperty("attachments")]
        public Attachment[] Attachments;

        [JsonIgnore]
        [XmlAttribute("list")]
        public bool List;

        [JsonPropertyAttribute("type_id")]
        [XmlElement("type_id")]
        public int TypeId;

        [JsonPropertyAttribute("type_name")]
        [XmlElement("type_name")]
        public string TypeName;
        //[JsonPropertyAttribute("message")]
        //[XmlElement("message", IsNullable = false)]
        //public string Message;
    }

    [XmlRoot("topics_deleteReplies_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicDeleteRepliesResponse
    {
        [XmlText]
        public string Result;
    }


    #endregion

    #region Notifications
    [XmlRoot("notification_send_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class NotificationSendResponse
    {
        [XmlText]
        public string Result;
    }

    [XmlRoot("notification_sendemail_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class NotificationSendEmailResponse
    {
        [XmlText]
        public string Recipients;
    }

    [XmlRoot("notification_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class NotificationGetResponse
    {
        [JsonPropertyAttribute("message")]
        [XmlElement("message", IsNullable = true)]
        public Notification Message;

        [JsonPropertyAttribute("notification")]
        [XmlElement("notification", IsNullable = true)]
        public Notification Notification;
    }
    #endregion

    #region Messages
    [XmlRoot("messages_send_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class MessageSendResponse
    {
        [XmlText]
        public string Result;
    }

    [XmlRoot("messages_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class MessageGetResponse
    {
        [XmlElement("count")]
        [JsonProperty("count")]
        public int Count;

        [XmlElement("pm")]
        [JsonProperty("pms")]
        public Message[] Messages;

        [JsonIgnore]
        [XmlAttribute("list")]
        public bool List;
    }
    #endregion

    #region Forums
    [XmlRoot("forums_create_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class ForumCreateResponse
    {
        [JsonPropertyAttribute("fid")]
        [XmlElement("fid")]
        public int Fid;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public string Url;
    }

    [XmlRoot("forums_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class ForumGetResponse
    {
        [JsonPropertyAttribute("fid")]
        [XmlElement("fid")]
        public int Fid;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public string Url;

        [JsonPropertyAttribute("topics")]
        [XmlElement("topics")]
        public int Topics;	//主题数
        
        [JsonPropertyAttribute("current_topics")]
        [XmlElement("current_topics")]
        public int CurTopics;	//主题数不包括子版

        [JsonPropertyAttribute("posts")]
        [XmlElement("posts")]
        public int Posts;	//帖子数

        [JsonPropertyAttribute("today_posts")]
        [XmlElement("today_posts")]
        public int TodayPosts;	//今日发帖

        [JsonPropertyAttribute("last_post")]
        [XmlElement("last_post")]
        public string LastPost;	//最后发表日期

        [JsonPropertyAttribute("last_poster")]
        [XmlElement("last_poster")]
        public string LastPoster; //最后发表的用户名

        [JsonPropertyAttribute("last_poster_id")]
        [XmlElement("last_poster_id")]
        public int LastPosterId; //最后发表的用户id

        [JsonPropertyAttribute("last_tid")]
        [XmlElement("last_tid")]
        public int LastTid; //最后发表帖子的主题id

        [JsonPropertyAttribute("last_title")]
        [XmlElement("last_title")]
        public string LastTitle; //最后发表的帖子标题

        [JsonPropertyAttribute("description")]
        [XmlElement("description")]
        public string Description;	//论坛描述

        [JsonPropertyAttribute("icon")]
        [XmlElement("icon")]
        public string Icon;	//论坛图标,显示于首页论坛列表等

        [JsonPropertyAttribute("moderators")]
        [XmlElement("moderators")]
        public string Moderators;	//版主列表(仅供显示使用,不记录实际权限)

        [JsonPropertyAttribute("rules")]
        [XmlElement("rules")]
        public string Rules;	//本版规则

        [JsonPropertyAttribute("parent_id")]
        [XmlElement("parent_id")]
        public int ParentId;	//本论坛的上级论坛或分本论坛的上级论坛或分类的fid

        [JsonPropertyAttribute("path_list")]
        [XmlElement("path_list")]
        public string PathList; //论坛级别所处路径的html链接代码

        [JsonPropertyAttribute("parent_id_list")]
        [XmlElement("parent_id_list")]
        public string ParentIdList; //论坛级别所处路径id列表

        [JsonPropertyAttribute("sub_forum_count")]
        [XmlElement("sub_forum_count")]
        public int SubForumCount; //论坛包括的子论坛个数

        [JsonPropertyAttribute("name")]
        [XmlElement("name")]
        public string Name;	//论坛名称

        [JsonPropertyAttribute("status")]
        [XmlElement("status")]
        public int Status;	//是否显示




    }

    [XmlRoot("forums_getIndexList_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class ForumGetIndexListResponse
    {
        [XmlElement("forum")]
        [JsonProperty("forums")]
        public IndexForum[] Forums;

        [JsonIgnore]
        [XmlAttribute("list")]
        public bool List;
    }
    #endregion
}
