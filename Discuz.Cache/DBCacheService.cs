using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Cache.Data
{
    /// <summary>
    /// 该类用于获取NoSqlDb声明的缓存服务
    /// </summary>
    public class DBCacheService
    {
        static ICacheOnlineUser iOnlineUser= null;
        static ICacheUsers iCacheUsers = null;
        static ICacheTopics iCacheTopics = null;
        static ICachePosts iCachePosts = null;
        static ICacheAttachments iCacheAttachments = null;
        static ICacheAttachFiles iCacheAttachFiles = null;
        private static object lockHelper = new object();

        public static ICacheOnlineUser GetOnlineUserService()
        {
            if (iOnlineUser == null)
            {
                lock (lockHelper)
                {
                    if (iOnlineUser == null)
                    {
                        try
                        {
                            if (EntLibConfigs.GetConfig().Cacheonlineuser.Enable)
                            {
                                iOnlineUser = (ICacheOnlineUser)Activator.CreateInstance(Type.GetType(
                                     EntLibConfigs.GetConfig().Cacheonlineuser.CacheType == 2 ?
                                      "Discuz.EntLib.TokyoTyrant.Data.OnlineUsers, Discuz.EntLib.TokyoTyrant" :
                                      "Discuz.EntLib.MongoDB.Data.OnlineUsers, Discuz.EntLib.MongoDB", false, true));
                            }
                        }
                        catch
                        {
                            throw new Exception("请检查" + (EntLibConfigs.GetConfig().Cacheonlineuser.CacheType == 2 ?
                                    "Discuz.EntLib.TokyoTyrant.dll" :
                                    "Discuz.EntLib.MongoDB.dll") + "文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return iOnlineUser;
        }

        public static ICacheUsers GetUsersService()
        {
            if (iCacheUsers == null)
            {
                lock (lockHelper)
                {
                    if (iCacheUsers == null)
                    {
                        try
                        {
                            if (EntLibConfigs.GetConfig().Cacheusers.Enable)
                            {
                                iCacheUsers = (ICacheUsers)Activator.CreateInstance(Type.GetType(
                                     EntLibConfigs.GetConfig().Cacheonlineuser.CacheType == 2 ?
                                      "Discuz.EntLib.TokyoTyrant.Data.Users, Discuz.EntLib.TokyoTyrant" :
                                      "Discuz.EntLib.MongoDB.Data.Users, Discuz.EntLib.MongoDB", false, true));
                            }
                       
                        }
                        catch
                        {
                            throw new Exception("请检查" + (EntLibConfigs.GetConfig().Cacheusers.CacheType == 2 ?
                                     "Discuz.EntLib.TokyoTyrant.dll" :
                                     "Discuz.EntLib.MongoDB.dll") + "文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return iCacheUsers;
        }

        public static ICacheTopics GetTopicsService()
        {
            if (iCacheTopics == null)
            {
                lock (lockHelper)
                {
                    if (iCacheTopics == null)
                    {
                        try
                        {
                            if (EntLibConfigs.GetConfig().Cachetopics.Enable)
                            {
                                iCacheTopics = (ICacheTopics)Activator.CreateInstance(Type.GetType(
                                     EntLibConfigs.GetConfig().Cachetopics.CacheType == 2 ?
                                      "Discuz.EntLib.TokyoTyrant.Data.Topics, Discuz.EntLib.TokyoTyrant" :
                                      "Discuz.EntLib.MongoDB.Data.Topics, Discuz.EntLib.MongoDB", false, true));
                            }
                        }
                        catch
                        {
                            throw new Exception("请检查" + (EntLibConfigs.GetConfig().Cachetopics.CacheType == 2 ?
                                      "Discuz.EntLib.TokyoTyrant.dll" :
                                      "Discuz.EntLib.MongoDB.dll") + "文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return iCacheTopics;
        }

        public static ICachePosts GetPostsService()
        {
            if (iCachePosts == null)
            {
                lock (lockHelper)
                {
                    if (iCachePosts == null)
                    {
                        try
                        {
                            if (EntLibConfigs.GetConfig().Cacheposts.Enable)
                            {
                                iCachePosts = (ICachePosts)Activator.CreateInstance(Type.GetType(
                                      "Discuz.EntLib.MongoDB.Data.Posts, Discuz.EntLib.MongoDB", false, true));
                            }
                        }
                        catch
                        {
                            throw new Exception("请检查 Discuz.EntLib.MongoDB.dll 文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return iCachePosts;
        }

        public static ICacheAttachments GetAttachmentsService()
        {
            if (iCacheAttachments == null)
            {
                lock (lockHelper)
                {
                    if (iCacheAttachments == null)
                    {
                        try
                        {
                            if (EntLibConfigs.GetConfig().Cacheattachments.Enable)
                            {
                                iCacheAttachments = (ICacheAttachments)Activator.CreateInstance(Type.GetType(
                                      "Discuz.EntLib.MongoDB.Data.Attachments, Discuz.EntLib.MongoDB", false, true));
                            }
                        }
                        catch
                        {
                            throw new Exception("请检查 Discuz.EntLib.MongoDB.dll 文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return iCacheAttachments;
        }

        public static ICacheAttachFiles GetAttachFilesService()
        {
            if (iCacheAttachFiles == null)
            {
                lock (lockHelper)
                {
                    if (iCacheAttachFiles == null)
                    {
                        try
                        {
                            if (EntLibConfigs.GetConfig().Cacheattachfiles.Enable)
                            {
                                iCacheAttachFiles = (ICacheAttachFiles)Activator.CreateInstance(Type.GetType(
                                      "Discuz.EntLib.MongoDB.Data.AttachFiles, Discuz.EntLib.MongoDB", false, true));
                            }
                        }
                        catch
                        {
                            throw new Exception("请检查 Discuz.EntLib.MongoDB.dll 文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return iCacheAttachFiles;
        }      
    }   
}
