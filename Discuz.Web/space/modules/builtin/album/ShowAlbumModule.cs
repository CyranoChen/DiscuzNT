using System.Web;
using System.Text;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Space.Provider;
using Discuz.Space.Entities;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Plugin.Album;
using Discuz.Album.Data;
using Discuz.Album.Config;

namespace Discuz.Space.Modules.Album
{
    /// <summary>
    /// �ҵ����ģ��
    /// </summary>
    public class ShowAlbumModule : ModuleBase
    {
        private string jsFile = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/Modules/builtin/album/showalbummodule.config");
        public ShowAlbumModule()
        { }

        /// <summary>
        /// �Զ���༭���ÿ�����
        /// </summary>
        /// <param name="editbox">�༭��ԭ����</param>
        /// <returns></returns>
        public override string OnEditBoxLoad(string editbox)
        {
            if (GeneralConfigs.GetConfig().Enablealbum != 1)
            {
                this.Editable = false;
                return "";
            }
            this.Editable = true;
            UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
            editbox = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" align=\"center\">";

            //��ʾ�����Id,0Ϊȫ�����
            string value = userprefs.GetValueByName("albumid");
            value = value == string.Empty ? "0" : value;
            int albumid = Utils.StrToInt(value, 0);
            List<AlbumInfo> albumlist = DTOProvider.GetSpaceAlbumList(this.Module.Uid, 0, Utils.StrToInt(AlbumConfigs.GetConfig().MaxAlbumCount, 0), 1);
            string options = string.Empty;

            if (albumid == 0)
                options += "<option value=\"0\" selected>ȫ��" + GeneralConfigs.GetConfig().Albumname + "</option>";
            else
                options += "<option value=\"0\">ȫ��" + GeneralConfigs.GetConfig().Albumname + "</option>";                

            foreach (AlbumInfo album in albumlist)
            {
                if (album.Albumid == albumid)
                {
                    options += string.Format("<option value=\"{0}\" selected>{1}</option>", album.Albumid, album.Title);
                    this.ModulePref.Title = album.Title;
                }
                else
                {
                    options += string.Format("<option value=\"{0}\">{1}</option>", album.Albumid, album.Title);
                }
            }
            
            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<select id=\"m___MODULE_ID___0\" name=\"m___MODULE_ID___up_{1}\">{2}</select></td></tr>", "ѡ��" + GeneralConfigs.GetConfig().Albumname + ": ", "albumid", options, "");

            //��ʾͼƬ��
            value = userprefs.GetValueByName("photocount");
            value = value == "" ? "10" : value;
            int photocount = Utils.StrToInt(value, 10);

            if (photocount > 10 || photocount < 3)
                photocount = 10;

            options = string.Empty;
            for (int i = 3; i <= 10; i++)
            {
                if (photocount == i)
                    options += string.Format("<option value=\"{0}\" selected>{0} ��</option>", i);
                else
                    options += string.Format("<option value=\"{0}\">{0} ��</option>", i);
            }

            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{3}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<select id=\"m___MODULE_ID___0\" name=\"m___MODULE_ID___up_{1}\">{2}</select></td></tr>", "չʾͼƬ����: ", "photocount", options, "");
            editbox += "</table>";
            return base.OnEditBoxLoad(editbox);
        }

        public override string GetModulePost(HttpContext httpContext)
        {
            return base.GetModulePost(httpContext);
        }

        /// <summary>
        /// �Զ���ģ�����ʱ����Ϊ
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public override string OnMouduleLoad(string content)
        {
            if (AlbumPluginProvider.GetInstance() == null || GeneralConfigs.GetConfig().Enablealbum != 1)
                return "�����δ��װ��ģ���ݲ�����";

            UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
            int photocount = Utils.StrToInt(userprefs.GetValueByName("photocount"), 10);
            int albumid = Utils.StrToInt(userprefs.GetValueByName("albumid"), 0);
            List<PhotoInfo> photolist = DTOProvider.GetPhotoListByUserId(this.Module.Uid, albumid, photocount);
            StringBuilder sb = new StringBuilder(StaticFileProvider.GetContent(jsFile));
            StringBuilder sbImgList = new StringBuilder();
            for (int i = 0; i < photolist.Count; i++)
            {
                sbImgList.AppendFormat("data[\"-1_{0}\"] = \"img: {1}; url: {2}; target: _blank; \"\r\n", i+1, BaseConfigs.GetForumPath + Discuz.Album.Globals.GetSquareImage(photolist[i].Filename), BaseConfigs.GetForumPath + "showalbumlist.aspx?uid=" + this.Module.Uid);
            }
            sb.Replace("{$templatepath}", BaseConfigs.GetForumPath + "templates/" + Templates.GetTemplateItem(GeneralConfigs.GetConfig().Templateid).Directory);
            sb.Replace("{$photolist}", sbImgList.ToString());
            content = sb.ToString();
            return base.OnMouduleLoad(content);
        }
    }
}
