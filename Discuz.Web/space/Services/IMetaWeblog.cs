using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

namespace Discuz.Space.Services
{ 

    #region Structs 

    public struct BlogInfo
    {
        public string blogid;
        public string url;
        public string blogName;
    } 

    public struct Category
    {
        public string categoryId;
        public string categoryName;
    } 

    [Serializable]
    public struct CategoryInfo
    {
        public string description;
        public string htmlUrl;
        public string rssUrl;
        public string title;
        public string categoryid;
    } 

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        public int length;
        public string type;
        public string url;
    } 

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public DateTime dateCreated;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string description;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string title; 

        public string[] categories;
        public Enclosure enclosure;
        public string link;
        public string permalink;
        [XmlRpcMember(
             Description = "Not required when posting. Depending on server may "
             + "be either string or integer. "
             + "Use Convert.ToInt32(postid) to treat as integer or "
             + "Convert.ToString(postid) to treat as string")]
        public object postid;
        public Source source;
        public string userid; 

        //public string mt_excerpt;
    } 

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Source
    {
        public string name;
        public string url;
    } 

    public struct UserInfo
    {
        public string userid;
        public string firstname;
        public string lastname;
        public string nickname;
        public string email;
        public string url;
    } 

    public struct MediaObjectUrl
        {
            public string url;
        } 

    public struct MediaObject
        {
            public string name;
            public string type;
            public byte[] bits;
        }
    #endregion
    public interface IMetaWeblog
    {
        #region MetaWeblog API 

        [XmlRpcMethod("metaWeblog.newPost",
             Description = "Makes a new post to a designated blog using the "
             + "MetaWeblog API. Returns postid as a string.")]
        string newPost(
            string blogid,
            string username,
            string password,
            Post post,
            bool publish); 

        [XmlRpcMethod("metaWeblog.editPost", Description = "Updates and existing post to a designated blog "
             + "using the MetaWeblog API. Returns true if completed.")]
        bool editPost(
            string postid,
            string username,
            string password,
            Post post,
            bool publish); 

        [XmlRpcMethod("metaWeblog.getPost",
             Description = "Retrieves an existing post using the MetaWeblog "
             + "API. Returns the MetaWeblog struct.")]
        Post getPost(
            string postid,
            string username,
            string password); 

        [XmlRpcMethod("metaWeblog.getCategories",
             Description = "Retrieves a list of valid categories for a post "
             + "using the MetaWeblog API. Returns the MetaWeblog categories "
             + "struct collection.")]
        CategoryInfo[] getCategories(
            string blogid,
            string username,
            string password); 

        [XmlRpcMethod("metaWeblog.getRecentPosts",
             Description = "Retrieves a list of the most recent existing post "
             + "using the MetaWeblog API. Returns the MetaWeblog struct collection.")]
        Post[] getRecentPosts(
            string blogid,
            string username,
            string password,
            int numberOfPosts); 

        [XmlRpcMethod("metaWeblog.newMediaObject", Description = "Add a media object to a post using the metaWeblog API. Returns media url as a string.")]
        MediaObjectUrl newMediaObject(string blogid, string username, string password, MediaObject mediaObject); 

        #endregion 

        #region BloggerAPI 

        [XmlRpcMethod("blogger.deletePost",
             Description = "Deletes a post.")]
        [return: XmlRpcReturnValue(Description = "Always returns true.")]
        bool deletePost(
            string appKey,
            string postid,
            string username,
            string password,
            [XmlRpcParameter(
                 Description = "Where applicable, this specifies whether the blog "
                 + "should be republished after the post has been deleted.")]
        bool publish); 

        [XmlRpcMethod("blogger.getUsersBlogs",
             Description = "Returns information on all the blogs a given user "
             + "is a member.")]
        BlogInfo[] getUsersBlogs(
            string appKey,
            string username,
            string password); 

        [XmlRpcMethod("blogger.getUserInfo",
             Description = "Returns information about the given user.")]
        UserInfo getUserInfo(
            string appKey,
            string username,
            string password); 

        #endregion 

    } 

} 

