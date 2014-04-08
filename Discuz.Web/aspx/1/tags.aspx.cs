using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;

namespace Discuz.Web
{
    /// <summary>
    /// ��ǩ�б�ҳ
    /// </summary>
    public class tags : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ʹ����ָ����ǩ�������б�
        /// </summary>
        public List<TopicInfo> topiclist;
        /// <summary>
        /// ʹ����ָ����ǩ�Ŀռ���־�б�
        /// </summary>
        public List<SpacePostInfo> spacepostlist;
        /// <summary>
        /// ʹ����ָ����ǩ��ͼƬ�б�
        /// </summary>
        public List<PhotoInfo> photolist;
        /// <summary>
        /// �����������TagId
        /// </summary>
        public int tagid = DNTRequest.GetInt("tagid", 0);
        /// <summary>
        /// Tag����ϸ��Ϣ
        /// </summary>
        public TagInfo tag;
        /// <summary>
        /// ҳ��
        /// </summary>
        public int pageid =  DNTRequest.GetInt("page", 1) < 1 ? 1 : DNTRequest.GetInt("page", 1);
        /// <summary>
        /// ������
        /// </summary>
        public int topiccount = 0;
        /// <summary>
        /// ��־��
        /// </summary>
        public int spacepostcount = 0;
        /// <summary>
        /// ͼƬ��
        /// </summary>
        public int photocount = 0;
        /// <summary>
        /// ҳ��
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// ��ǰ�б�����,topic=�����б�,spacepost=��־�б�,photo=ͼƬ�б�
        /// </summary>
        public string listtype = DNTRequest.GetString("t");
        /// <summary>
        /// ��Ʒ�б�
        /// </summary>
        public GoodsinfoCollection goodslist;
        /// <summary>
        /// ��Ʒ��
        /// </summary>
        public int goodscount;
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public TagInfo[] taglist;
        #endregion

        protected override void ShowPage()
        {
            if (config.Enabletag != 1)
            {
                AddErrLine("û������Tag����");
                return;
            }
            if (tagid > 0)
            {
                tag = Tags.GetTagInfo(tagid);
                if (tag == null || tag.Orderid < 0)
                {
                    AddErrLine("ָ���ı�ǩ�����ڻ��ѹر�");
                    return;
                }
                pagetitle = tag.Tagname;
               
                if (Utils.StrIsNullOrEmpty(listtype))   listtype = "topic";

                if (IsErr()) return;

                BindItem();
            }
            else
            {
                pagetitle = "��ǩ";
                taglist = ForumTags.GetCachedHotForumTags(100);
            }
        }

        private void BindItem()
        {
            switch (listtype)
            {
                case "spacepost":
                    SpacePluginBase spb = SpacePluginProvider.GetInstance();
                    if (spb == null)
                    {
                        AddErrLine("δ��װ�ռ���");
                        return;
                    }
                    spacepostcount = spb.GetSpacePostCountWithSameTag(tagid);
                    SetPage(spacepostcount);
                    if (spacepostcount > 0)
                    {
                        spacepostlist = spb.GetSpacePostsWithSameTag(tagid, pageid, config.Tpp);
                        pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "tags.aspx?t=spacepost&tagid=" + tagid, 8);
                    }
                    else
                        AddErrLine("�ñ�ǩ��������־");
                    break;
                case "photo":
                    AlbumPluginBase apb = AlbumPluginProvider.GetInstance();
                    if (apb == null)
                    {
                        AddErrLine("δ��װ�����");
                        return;
                    }
                    photocount = apb.GetPhotoCountWithSameTag(tagid);
                    SetPage(photocount);
                    if (photocount > 0)
                    {
                        photolist = apb.GetPhotosWithSameTag(tagid, pageid, config.Tpp);
                        foreach (PhotoInfo photo in photolist)
                        {
                            if (photo.Filename.IndexOf("http") < 0) //����Զ����Ƭʱ
                                photo.Filename = forumpath + AlbumPluginProvider.GetInstance().GetThumbnailImage(photo.Filename);
                            else
                                photo.Filename = AlbumPluginProvider.GetInstance().GetThumbnailImage(photo.Filename);
                        }
                        pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "tags.aspx?t=photo&tagid=" + tagid, 8);
                    }
                    else
                        AddErrLine("�ñ�ǩ������ͼƬ");
                    break;
                case "mall":
                    if (MallPluginProvider.GetInstance() == null)
                    {
                        AddErrLine("δ��װ�̳ǲ��");
                        return;
                    }
                    goodscount = MallPluginProvider.GetInstance().GetGoodsCountWithSameTag(tagid);
                    SetPage(goodscount);
                    if (goodscount > 0)
                    {
                        goodslist = MallPluginProvider.GetInstance().GetGoodsWithSameTag(tagid, pageid, config.Tpp);
                        pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "tags.aspx?t=mall&tagid=" + tagid, 8);
                    }
                    else
                        AddErrLine("�ñ�ǩ��������Ʒ");
                    break;
                case "topic":
                    topiccount = Topics.GetTopicCountByTagId(tagid);
                    SetPage(topiccount);
                    if (topiccount > 0)
                    {
                        topiclist = Topics.GetTopicListByTagId(tagid, pageid, config.Tpp);
                        pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "tags.aspx?t=topic&tagid=" + tagid, 8);
                    }
                    else
                        AddErrLine("�ñ�ǩ����������");
                    break;
            }
        }


        private void SetPage(int count)
        {
            pagecount = count % config.Tpp == 0 ? count / config.Tpp : count / config.Tpp + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;
            pageid = pageid > pagecount ? pagecount : pageid;
        }

    }
}
