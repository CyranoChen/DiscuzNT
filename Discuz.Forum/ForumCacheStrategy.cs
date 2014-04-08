using System;
using System.Web.Caching;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Forum
{
	/// <summary>
	/// ForumCacheStrategy ����
	/// ���Ƶ���̳���������, ��ʵ�ֵ���ʱ�����ú���������
	/// </summary>
    //public class ForumCacheStrategy : Discuz.Cache.ICacheStrategy
    //{
    //    protected static volatile System.Web.Caching.Cache webCacheforfocus = null;

    //    private int _timeOut = 1200; // Ĭ�ϻ�������Ϊ20����

    //    /// <summary>
    //    /// ���캯��
    //    /// </summary>
    //    static ForumCacheStrategy()
    //    {
    //        webCacheforfocus = Discuz.Cache.DefaultCacheStrategy.GetWebCacheObj;
    //    }		
		
    //    /// <summary>
    //    /// ���õ������ʱ��[��λ:��]
    //    /// </summary>
    //    virtual public int TimeOut
    //    {
    //        set { _timeOut = (value < 1200) ? value : 1200; }
    //        get { return (_timeOut < 1200) ? _timeOut : 1200; }
    //    }
		
    //    /// <summary>
    //    /// ���뵱ǰ���󵽻�����
    //    /// </summary>
    //    /// <param name="objId">key for the object</param>
    //    /// <param name="o">object</param>
    //    public void AddObject(string objId, object o)
    //    {		
    //        if (objId == null || objId.Length == 0 || o == null) 
    //            return;

    //        CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);
    //        webCacheforfocus.Insert(objId, o, null, DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
    //    }

    //    /// <summary>
    //    /// ���뵱ǰ���󵽻�����
    //    /// </summary>
    //    /// <param name="objId">����ļ�ֵ</param>
    //    /// <param name="o">����Ķ���</param>
    //    /// <param name="o">����ʱ��,��λ:��</param>
    //    public virtual void AddObject(string objId, object o, int expire)
    //    {
    //        if (objId == null || objId.Length == 0 || o == null)
    //            return;

    //        CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

    //        //��ʾ��������
    //        if (expire == 0)
    //            webCacheforfocus.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
    //        else
    //            webCacheforfocus.Insert(objId, o, null, DateTime.Now.AddSeconds(expire), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
    //    }

    //    public void AddObjectWith(string objId, object o)
    //    {
    //        if (objId == null || objId.Length == 0 || o == null) 
    //            return ;

    //        webCacheforfocus.Insert(objId, o, null, System.DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration);
    //    }



    //    /// <summary>
    //    /// ���뵱ǰ���󵽻�����,��������ļ���������
    //    /// </summary>
    //    /// <param name="objId">key for the object</param>
    //    /// <param name="o">object</param>
    //    /// <param name="files">���ӵ�·���ļ�</param>
    //    public void AddObjectWithFileChange(string objId, object o, string[] files)
    //    {
    //        if (objId == null || objId.Length == 0 || o == null)
    //            return;
		
    //        CacheDependency dep = new CacheDependency(files, DateTime.Now);
    //        webCacheforfocus.Insert(objId, o, dep, System.DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration);
    //    }
	


    //    /// <summary>
    //    /// ���뵱ǰ���󵽻�����,��ʹ��������
    //    /// </summary>
    //    /// <param name="objId">key for the object</param>
    //    /// <param name="o">object</param>
    //    /// <param name="dependKey">���ӵ�·���ļ�</param>
    //    public void AddObjectWithDepend(string objId, object o, string[] dependKey)
    //    {
    //        if (objId == null || objId.Length == 0 || o == null) 
    //            return;

    //        CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);
    //        webCacheforfocus.Insert(objId, o, dep, System.DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration);
    //    }
	
    //    /// <summary>
    //    /// ɾ���������
    //    /// </summary>
    //    /// <param name="objId">����Ĺؼ���</param>
    //    public void RemoveObject(string objId)
    //    {
    //        if (objId == null || objId.Length == 0)
    //            return;

    //        webCacheforfocus.Remove(objId);
    //    }


    //    /// <summary>
    //    /// ����һ��ָ���Ķ���
    //    /// </summary>
    //    /// <param name="objId">����Ĺؼ���</param>
    //    /// <returns>����</returns>
    //    public object RetrieveObject(string objId)
    //    {
    //        if (objId == null || objId.Length == 0)
    //            return null;

    //        return webCacheforfocus.Get(objId);
    //    }


    //    //�����ص�ί�е�һ��ʵ��
    //    public void onRemove(string key, object val, CacheItemRemovedReason reason)
    //    {
    //        return;
    //    }
    //}


    ///// <summary>
    ///// RssCacheStrategy ����
    ///// Rss���������, ��ʵ�ֵ���ʱ����������
    ///// </summary>
    ///// </summary>
    //public class RssCacheStrategy : ForumCacheStrategy
    //{
    //    private int _timeOut = 600; // Ĭ�ϻ�������Ϊ10����

    //    //���õ������ʱ��[��λ:����] 
    //    override public int TimeOut
    //    {
    //        set { _timeOut = (value > 0 && value < 9999) ? value : 600; }
    //        get { return (_timeOut > 0 && _timeOut < 9999) ? _timeOut : 600; }
    //    }
    //}
    
    ///// <summary>
    ///// SitemapCacheStrategy ����
    ///// Sitemap���������, ��ʵ�ֵ���ʱ����������
    ///// </summary>
    ///// </summary>
    //public class SitemapCacheStrategy : ForumCacheStrategy
    //{
    //    private int _timeOut = 720; // Ĭ�ϻ�������Ϊ12����

    //    //���õ������ʱ��[��λ:��] 
    //    override public int TimeOut
    //    {
    //        set { _timeOut = (value > 0 && value < 9999) ? value : 720; }
    //        get { return (_timeOut > 0 && _timeOut < 9999) ? _timeOut : 720; }
    //    }
    //}
}