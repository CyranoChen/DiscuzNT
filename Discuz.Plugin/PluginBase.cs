using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Entity;
using System.Data;

namespace Discuz.Plugin
{
    public abstract class PluginBase : IFeed, IPost, IUser, ISearch
    {
        #region IUser Members

        public void Create(UserInfo user)
        {
            OnUserCreated(user);
        }

        public void Ban(int userid)
        {
            OnUserBanned(userid);
        }

        public void UnBan(int userid)
        {
            OnUserUnBanned(userid);
        }

        public void Delete(int userid)
        {
            OnUserDeleted(userid);
        }

        public void LogIn(UserInfo user)
        {
            OnUserLoggedIn(user);
        }

        public void LogOut(UserInfo user)
        {
            OnUserLoggedOut(user);
        }

        #endregion

        #region IPost Members

        public void CreateTopic(TopicInfo topic, PostInfo post, AttachmentInfo[] attachs)
        {
            OnTopicCreated(topic, post, attachs);
        }

        public void CreatePost(Discuz.Entity.PostInfo post)
        {
            OnPostCreated(post);
        }

        public void Edit(Discuz.Entity.PostInfo post)
        {
            OnPostEdited(post);
        }

        public void Ban(Discuz.Entity.PostInfo post)
        {
            OnPostBanned(post);
        }

        public void UnBan(Discuz.Entity.PostInfo post)
        {
            OnPostUnBanned(post);
        }

        public void Delete(Discuz.Entity.PostInfo post)
        {
            OnPostDeleted(post);
        }

        public string CreateAttachment(AttachmentInfo[] attachs, int usergroupid, int userid, string username)
        {
            return OnAttachCreated(attachs, usergroupid, userid, username);
        }


        #endregion

        #region IFeed Members

        public string GetFeed(int ttl, int uid)
        {
            return GetFeedXML(ttl, uid);
        }

        public string GetFeed(int ttl)
        {
            return GetFeedXML(ttl);
        }

        #endregion

        #region ISearch Members

        public DataTable GetResult(int pagesize, string idstr)
        {
            return GetSearchResult(pagesize, idstr);
        }

        #endregion

        protected virtual void OnUserCreated(UserInfo user)
        { }

        protected virtual void OnUserBanned(int userid)
        { }

        protected virtual void OnUserUnBanned(int userid)
        { }

        protected virtual void OnUserDeleted(int userid)
        { }

        protected virtual void OnUserLoggedIn(UserInfo user)
        { }

        protected virtual void OnUserLoggedOut(UserInfo user)
        { }

        protected virtual void OnTopicCreated(TopicInfo topic, PostInfo post, AttachmentInfo[] attachs)
        { }

        protected virtual void OnPostCreated(PostInfo post)
        { }

        protected virtual void OnPostEdited(PostInfo post)
        { }

        protected virtual void OnPostBanned(PostInfo post)
        { }

        protected virtual void OnPostUnBanned(PostInfo post)
        { }

        protected virtual void OnPostDeleted(PostInfo post)
        { }

        protected virtual string GetFeedXML(int ttl, int uid)
        {
            return "";
        }

        protected virtual string GetFeedXML(int ttl)
        {
            return "";
        }

        protected virtual DataTable GetSearchResult(int pagesize, string idstr)
        {
            return new DataTable();
        }

        protected virtual string OnAttachCreated(AttachmentInfo[] attachs, int usergroupid, int userid, string username)
        {
            return "";
        }

    }
}
