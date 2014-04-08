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
    public class Manage : AdminPage
#else
    public partial class Manage : AdminPage
#endif
    {

#if NET1
        #region ¿Ø¼þÉùÃ÷
        protected Discuz.Control.Button SearchAlbum;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Web.Admin.ajaxalbumlist AjaxAlbumList1;
        protected Discuz.Control.Button DeleteApply;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void DeleteApply_Click(object sender, EventArgs e)
        {
            #region É¾³ýÏà²á
            string albumid = DNTRequest.GetString("albumid");
            if (albumid.Trim() == "")
            {
                return;
            }
            foreach (string id in albumid.Split(','))
            {
#if NET1
                PhotoInfoCollection _spacephotoinfoarray = BlogProvider.GetSpacePhotosInfo(DbProvider.GetInstance().GetSpacePhotoByAlbumID(int.Parse(id)));
#else
                Discuz.Common.Generic.List<PhotoInfo> _spacephotoinfoarray = DTOProvider.GetSpacePhotosInfo(DbProvider.GetInstance().GetSpacePhotoByAlbumID(int.Parse(id)));
#endif

                string photoidList = "";
                int uid = DbProvider.GetInstance().GetUidByAlbumid(int.Parse(id));
                if (_spacephotoinfoarray != null)
                {
                    foreach (PhotoInfo _s in _spacephotoinfoarray)
                    {
                            photoidList += _s.Photoid + ",";
                    }
                }
                if (photoidList != "")
                {
                    photoidList = photoidList.Substring(0, photoidList.Length - 1);
                    DbProvider.GetInstance().DeleteSpacePhotoByIDList(photoidList,int.Parse(id),uid);
                }
                DbProvider.GetInstance().DeleteSpaceAlbum(int.Parse(id), uid);
            }
            Response.Redirect("album_manage.aspx");
            #endregion
        }

    }
}
