using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common;
using Discuz.Web.Admin;
using Discuz.Album.Config;
using Discuz.Album.Data;

namespace Discuz.Album.Admin
{

#if NET1
    public class AlbumConfig : AdminPage
#else
    public partial class AlbumConfig : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.RadioButtonList EnableAlbum;
        protected Discuz.Control.TextBox maxalbumcount;
        protected Discuz.Control.Button SaveCombinationInfo;
        protected System.Web.UI.HtmlControls.HtmlGenericControl ShowAlbumOption;
        protected System.Web.UI.HtmlControls.HtmlGenericControl ShowUserGroup;
		protected System.Web.UI.HtmlControls.HtmlTable groupphotosize;

        #endregion
#endif

        public const string ValidInt = @"^[0-9]*[1-9][0-9]*$";   


        protected void Page_Load(object sender, EventArgs e)
        {
            #region 绑定数据
            if (!IsPostBack)
            {
                EnableAlbum.SelectedValue = config.Enablealbum.ToString();
                EnableAlbum.Items[0].Attributes.Add("onclick", "ShowHiddenOption(true);");
                EnableAlbum.Items[1].Attributes.Add("onclick", "ShowHiddenOption(false);");
                ShowAlbumOption.Attributes.Add("style", config.Enablealbum == 1 ? "display:block" : "display:none");
                ShowUserGroup.Attributes.Add("style", config.Enablealbum == 1 ? "display:block" : "display:none");
                AlbumConfigInfo albumconfiginfo = AlbumConfigs.GetConfig();
                maxalbumcount.Text = albumconfiginfo.MaxAlbumCount;
                BindUserGorupMaxspacephotosize();
            }
            #endregion
        }

        private void BindUserGorupMaxspacephotosize()
        {
            #region 绑定用户组照片空间大小
            DataTable dt = DatabaseProvider.GetInstance().GetUserGroupMaxspacephotosize();
            int i = 1;
            HtmlTableRow tr = new HtmlTableRow();
            foreach (DataRow dr in dt.Rows)
            {
                if (i % 2 == 1)
                {
                    tr = new HtmlTableRow();
                }
                HtmlTableCell td = new HtmlTableCell("td");
                td.Controls.Add(new LiteralControl(dr["grouptitle"].ToString()));
                tr.Cells.Add(td);
                td = new HtmlTableCell("td");
                Discuz.Control.TextBox tb = new Discuz.Control.TextBox();
                tb.ID = "maxspacephotosize" + dr["groupid"].ToString();
                tb.Size = 10;
                tb.MaxLength = 9;
                tb.Text = dr["maxspacephotosize"].ToString();
                tb.RequiredFieldType = "数据校验";
                td.Controls.Add(tb);
                tr.Cells.Add(td);
                tr.Cells.Add(GetTD("maxspacephotosize" + dr["groupid"].ToString()));
                groupphotosize.Rows.Add(tr);
                i++;
            }
            #endregion
        }

        private HtmlTableCell GetTD(string targetId)
        {
            #region 绑定空间值
            LiteralControl select = new LiteralControl();
            select.Text = "<select onchange=\"document.getElementById('" + targetId + "').value=this.value\">\n<option value=\"\">请选择</option>\n";
            select.Text += "<option value=\"51200\">50K</option>\n<option value=\"102400\">100K</option>\n<option value=\"153600\">150K</option>\n";
            select.Text += "<option value=\"204800\">200K</option>\n<option value=\"256000\">250K</option>\n<option value=\"307200\">300K</option>\n";
            select.Text += "<option value=\"358400\">350K</option>\n<option value=\"409600\">400K</option>\n<option value=\"512000\">500K</option>\n";
            select.Text += "<option value=\"614400\">600K</option>\n<option value=\"716800\">700K</option>\n<option value=\"819200\">800K</option>\n";
            select.Text += "<option value=\"921600\">900K</option>\n<option value=\"1024000\">1M</option>\n<option value=\"2048000\">2M</option>\n";
            select.Text += "<option value=\"4096000\">4M</option>\n<option value=\"6144000\">6M</option>\n<option value=\"8192000\">8M</option>\n";
            select.Text += "<option value=\"10240000\">10M</option>\n<option value=\"12288000\">12M</option>\n<option value=\"14336000\">14M</option>\n";
            select.Text += "<option value=\"16384000\">16M</option>\n<option value=\"18432000\">18M</option>\n<option value=\"20480000\">20M</option>\n";
            select.Text += "<option value=\"22528000\">22M</option>\n<option value=\"24576000\">24M</option>\n<option value=\"26624000\">26M</option>\n";
            select.Text += "<option value=\"28672000\">28M</option>\n<option value=\"30720000\">30M</option></select>";
            HtmlTableCell td = new HtmlTableCell("td");
            td.Controls.Add(select);
            return td;
            #endregion
        }

        protected void SaveCombinationInfo_Click(object sender, EventArgs e)
        {
            #region 保存相册配置
            config.Enablealbum = int.Parse(EnableAlbum.SelectedValue);
            GeneralConfigs.Serialiaze(config, Server.MapPath("../../config/general.config"));
            if (Utils.IsNumeric(maxalbumcount.Text.ToString()) == true && Utils.IsInt(maxalbumcount.Text.ToString()) == true)
            {
                if (config.Enablealbum == 1)
                {
                    AlbumConfigInfo albumconfiginfo = new AlbumConfigInfo();
                    albumconfiginfo.MaxAlbumCount = maxalbumcount.Text;
                    AlbumConfigs.SaveConfig(albumconfiginfo);
                    //保存组照片最大空间
                    DataTable dt = DatabaseProvider.GetInstance().GetUserGroupMaxspacephotosize();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!Utils.IsInt(DNTRequest.GetString("maxspacephotosize" + dr["groupid"].ToString()).ToString()))
                        {
                            base.RegisterStartupScript("", "<script>alert('输入错误,相册大小只能是0或者正整数');window.location.href='album_config.aspx';</script>");
                            return;
                        } 
                        int photosize = DNTRequest.GetInt("maxspacephotosize" + dr["groupid"].ToString(), 0);
                        Discuz.Entity.UserGroupInfo __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(Convert.ToInt32(dr["groupid"].ToString()));
                        __usergroupinfo.Maxspacephotosize = photosize;
                        AdminUserGroups.UpdateUserGroupInfo(__usergroupinfo);
                    }
                }
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                Response.Redirect("album_config.aspx");
            }
            else
            {

                base.RegisterStartupScript("", "<script>alert('相册数上限输入错误,请检查');window.location.href='album_config.aspx';</script>");
                return;
            }
            #endregion
        }
    }
}
