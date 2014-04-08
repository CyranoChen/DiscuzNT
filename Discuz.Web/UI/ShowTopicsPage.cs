using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using System.Text;

using Discuz.Aggregation;
using Discuz.Common.Generic;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;

namespace Discuz.Web.UI
{
    /// <summary>
    /// RSS页面类
    /// </summary>
    public class ShowTopicsPage : System.Web.UI.Page
    {
        int length = DNTRequest.GetQueryInt("length", -1);
        int count = DNTRequest.GetQueryInt("count", 10);
        int cachetime = DNTRequest.GetQueryInt("cachetime", 20);
        public string rootUrl = Utils.GetRootUrl(Discuz.Config.BaseConfigs.GetForumPath);

        string spacerooturl = string.Empty;

        public ShowTopicsPage()
        {
            spacerooturl = rootUrl + "space/";
        }

        protected void OutPutUpdatedSpaces(string template, string alternatingTemplate)
        {
            if (SpacePluginProvider.GetInstance() == null)
            {
                Response.Write("document.write('未安装空间插件');");
                return;
            }

            StringBuilder result = new StringBuilder();
            int i = 0;
            string title = "";
            foreach (DataRow dr in Focuses.GetUpdatedSpaces(count, cachetime).Rows)
            {
                title = dr["spacetitle"].ToString().Trim();
                result.AppendFormat((i % 2 == 0 ? template : alternatingTemplate), "", (length == -1 ? title : Utils.GetUnicodeSubString(title, length, "")), "", "", title, "", spacerooturl + "?uid=" + dr["userid"].ToString());
                i++;
            }
            Response.Write("document.write('" + result.ToString().Replace("'", "\\'") + "');");
        }

        protected void OutPutRecommendedSpaces(string template, string alternatingTemplate)
        {
            if (SpacePluginProvider.GetInstance() == null)
            {
                Response.Write("document.write('未安装空间插件');");
                return;
            }

            StringBuilder result = new StringBuilder();
            int i = 0;
            foreach (SpaceConfigInfoExt space in AggregationFacade.SpaceAggregation.GetSpaceListFromFile("Website"))
            {
                if (i >= count) break;
                result.AppendFormat((i % 2 == 0 ? template : alternatingTemplate), "", (length == -1 ? space.Spacetitle : Utils.GetUnicodeSubString(space.Spacetitle, length, "")), "", "", space.Spacetitle, string.Empty, spacerooturl + "?uid=" + space.Userid.ToString());
                i++;
            }
            Response.Write("document.write('" + result.ToString().Replace("'", "\\'") + "');");
        }

        protected void OutPutNewSpacePosts(string template, string alternatingTemplate)
        {
            if (SpacePluginProvider.GetInstance() == null)
            {
                Response.Write("document.write('未安装空间插件');");
                return;
            }

            StringBuilder result = new StringBuilder();
            int i = 0;
            string title = "";
            foreach (DataRow dr in Focuses.GetNewSpacePosts(count, cachetime).Rows)
            {
                title = dr["title"].ToString().Trim();
                result.AppendFormat((i % 2 == 0 ? template : alternatingTemplate), "", (length == -1 ? title : Utils.GetUnicodeSubString(title, length, "")), "", "", title, "", spacerooturl + "?uid=" + dr["uid"].ToString(), spacerooturl + "viewspacepost.aspx?postid=" + dr["postid"].ToString());
                i++;
            }
            Response.Write("document.write('" + result.ToString().Replace("'", "\\'") + "');");
        }

        protected void OutPutRecommendedSpacePosts(string template, string alternatingTemplate)
        {
            if (SpacePluginProvider.GetInstance() == null)
            {
                Response.Write("document.write('未安装空间插件');");
                return;
            }

            StringBuilder result = new StringBuilder();
            int i = 0;
            string title = "";
            foreach (SpaceShortPostInfo post in AggregationFacade.SpaceAggregation.GetSpacePostList("Website"))
            {
                if (i > count) break;
                title = post.Title;
                result.AppendFormat((i % 2 == 0 ? template : alternatingTemplate), "", (length == -1 ? title : Utils.GetUnicodeSubString(title, length, "")), "", "", title, "", spacerooturl + "?uid=" + post.Uid, spacerooturl + "viewspacepost.aspx?postid=" + post.Postid);
                i++;
            }
            Response.Write("document.write('" + result.ToString().Replace("'", "\\'") + "');");
        }

        protected void OutPutRecommendedAlbum(string template, string alternatingTemplate)
        {
            if (AlbumPluginProvider.GetInstance() == null)
            {
                Response.Write("document.write('未安装相册插件');");
                return;
            }

            StringBuilder result = new StringBuilder();
            int i = 0;
            string title = "";
            foreach (AlbumInfo album in AggregationFacade.AlbumAggregation.GetRecommandAlbumList("Website"))
            {
                if (i > count) break;
                title = album.Title;
                result.AppendFormat((i % 2 == 0 ? template : alternatingTemplate), "", (length == -1 ? title : Utils.GetUnicodeSubString(title, length, "")), "", "", title, album.Logo, spacerooturl + "?uid=" + album.Userid, "", rootUrl + "showalbum.aspx?albumid=" + album.Albumid);
                i++;
            }
            Response.Write("document.write('" + result.ToString().Replace("'", "\\'") + "');");
        }
    }
}
