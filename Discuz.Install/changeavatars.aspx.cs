using System;
using System.Data;

using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using System.IO;
using Discuz.Forum;

namespace Discuz.Install
{
    public class chageavatars : System.Web.UI.Page
    {
        public string preproduct = "Discuz!NT 2.6";
        public string productname = "Discuz!NT 3.0";
        public System.Web.UI.WebControls.Button change;
        public System.Web.UI.WebControls.Label info;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ChangeAvatars_Click(object sender, EventArgs e)
        {
            DataTable avatars = DbHelper.ExecuteDataset(string.Format("SELECT {0}users.uid,avatar FROM {0}users JOIN {0}userfields ON {0}users.uid={0}userfields.uid WHERE avatar LIKE '/%'",
                BaseConfigs.GetTablePrefix)).Tables[0];
            int count = 0;
            foreach (DataRow dr in avatars.Rows)
            {
                string sourceAvatarPath = Utils.GetMapPath(dr["avatar"].ToString());
                string uid = Avatars.FormatUid(dr["uid"].ToString());
                if (File.Exists(sourceAvatarPath))
                {
                    string destDir = string.Format("{0}avatars/upload/{1}/{2}/{3}", BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2));
                    if(!Directory.Exists(Utils.GetMapPath(destDir)))
                        Directory.CreateDirectory(Utils.GetMapPath(destDir));
                    string destAvatarPath = Utils.GetMapPath(string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_{5}.jpg",
                        BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), "large"));
                    File.Copy(sourceAvatarPath, destAvatarPath, true);
                    destAvatarPath = Utils.GetMapPath(string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_{5}.jpg",
                        BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), "medium"));
                    File.Copy(sourceAvatarPath, destAvatarPath, true);
                    destAvatarPath = Utils.GetMapPath(string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_{5}.jpg",
                        BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), "small"));
                    string destPath = string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_{5}.jpg",
                        BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), "small");
                    File.Copy(sourceAvatarPath, destAvatarPath, true);
                    Thumbnail thumb = new Thumbnail();
                    thumb.SetImage(destPath);
                    thumb.SaveThumbnailImage(48, 48);
                    count++;
                }
            }
            info.Text = "<span style='color:blue'>提示:</span>头像转换程序已经成功转换了 <span style='color:red'>" + count + "</span> 个头像.";
            change.Text = "进入首页";
            change.Attributes.Add("onclick", "window.location.href='../index.aspx';return false;");
        }
    }
}
