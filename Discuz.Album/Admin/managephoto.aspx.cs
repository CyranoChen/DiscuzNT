using System;
using System.Data;
using System.Web.UI;
using System.Xml;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Web.Admin;
using Discuz.Album.Data;

namespace Discuz.Album.Admin
{

#if NET1
    public class ManagePhoto : AdminPage
#else
    public partial class ManagePhoto : AdminPage
#endif
    {

#if NET1
        #region ¿Ø¼þÉùÃ÷
        protected Discuz.Control.Button DeleteApply;
        #endregion
#endif

        public DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            int albumid = DNTRequest.GetInt("albumid", 0);
            dt = DbProvider.GetInstance().SpacePhotosList(albumid);
        }

        protected void DeleteApply_Click(object sender, EventArgs e)
        {
            #region É¾³ýÏà²á
            int albumid = DNTRequest.GetInt("albumid", 0);
            int uid = DbProvider.GetInstance().GetUidByAlbumid(albumid);
            if (DNTRequest.GetFormString("photoid") == "")
                return;
            DbProvider.GetInstance().DeleteSpacePhotoByIDList(DNTRequest.GetFormString("photoid"), albumid, uid);
            AlbumInfo _AlbumInfo = DTOProvider.GetAlbumInfo(albumid);
            _AlbumInfo.Imgcount = DbProvider.GetInstance().GetSpacePhotoCountByAlbumId(albumid); ;
            DbProvider.GetInstance().SaveSpaceAlbum(_AlbumInfo);
            Response.Redirect("album_manage.aspx");
            #endregion
        }

        public string GetThumbnail(string filename)
        {
            #region ·µ»ØËõÂÔÍ¼µØÖ·
            return BaseConfigs.GetForumPath + filename.Replace(".", "_thumbnail.");
            #endregion
        }

    }
}
