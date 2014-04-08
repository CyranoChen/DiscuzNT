using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Discuz.Config;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public partial class ftpsetting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ftpType = GetParam(DNTRequest.GetString("ftptype"));
                FTPConfigInfoCollection ftpconfiginfocollection = 
                    (FTPConfigInfoCollection)SerializationHelper.Load(typeof(FTPConfigInfoCollection), Server.MapPath("../../config/ftp.config"));
                Allowupload.Items[0].Attributes.Add("onclick", "ShowFtpLayout(true)");
                Allowupload.Items[1].Attributes.Add("onclick", "ShowFtpLayout(false)");
                foreach (FTPConfigInfo fci in ftpconfiginfocollection)
                {
                    if (fci.Name == ftpType)  //绑定论坛FTP设置
                    {
                        Serveraddress.Text = fci.Serveraddress;
                        Serverport.Text = fci.Serverport.ToString();
                        Username.Text = fci.Username;
                        Password.Text = fci.Password;
                        hiddpassword.Value = fci.Password;
                        Mode.SelectedValue = fci.Mode.ToString();
                        Uploadpath.Text = fci.Uploadpath;
                        Timeout.Text = fci.Timeout.ToString();
                        Allowupload.SelectedValue = fci.Allowupload.ToString();
                        Remoteurl.Text = fci.Remoteurl;
                        Reservelocalattach.SelectedValue = fci.Reservelocalattach.ToString();
                        Reserveremoteattach.SelectedValue = fci.Reserveremoteattach.ToString();
                        if (fci.Allowupload == 0)
                        {
                            FtpLayout.Attributes.Add("style", "display:none");
                        }
                        else
                        {
                            FtpLayout.Attributes.Add("style", "display:block");
                        }
                    }
                }
                if (Serveraddress.Text != "")   //如果设置完全则返回
                    return;
                FtpLayout.Attributes.Add("style", "display:none");
                foreach (FTPConfigInfo fci in ftpconfiginfocollection)
                {
                    if (fci.Serveraddress != "")
                    {
                        Serveraddress.Text = fci.Serveraddress;
                        Serverport.Text = fci.Serverport.ToString();
                        Username.Text = fci.Username;
                        Password.Text = fci.Password;
                        Mode.SelectedValue = fci.Mode.ToString();
                        Uploadpath.Text = ftpType.ToLower();
                        Timeout.Text = fci.Timeout.ToString();
                        Remoteurl.Text = fci.Remoteurl.Replace(fci.Name.ToLower(), ftpType.ToLower());
                        Reservelocalattach.SelectedValue = fci.Reservelocalattach.ToString();
                        Reserveremoteattach.SelectedValue = fci.Reserveremoteattach.ToString();
                    }
                }
            }
        }

        protected void SaveFtpInfo_Click(object sender, EventArgs e)
        {
            string ftpType = GetParam(DNTRequest.GetString("ftptype"));
            if (Serveraddress.Text.Trim() == "" || Serverport.Text.Trim() == "" || Username.Text.Trim() == "" || 
                Password.Text.Trim() == "" || Uploadpath.Text.Trim() == "" || Timeout.Text.Trim() == "" || Remoteurl.Text.Trim() == "")
            {
                base.RegisterStartupScript("", "<script>alert('远程附件设置各项不允许为空');window.location.href='global_ftpsetting.aspx?ftptype=" + ftpType + "';</script>");
                return;
            }
            if (Uploadpath.Text.EndsWith("/"))
            {
                base.RegisterStartupScript("", "<script>alert('附件保存路径不允许以“/”结尾');window.location.href='global_ftpsetting.aspx?ftptype=" + ftpType + "';</script>");
                return;
            }
            if (Remoteurl.Text.EndsWith("/"))
            {
                base.RegisterStartupScript("", "<script>alert('远程访问 URL 不允许以“/”结尾');window.location.href='global_ftpsetting.aspx?ftptype=" + ftpType + "';</script>");
                return;
            }
            if (!Utils.IsNumeric(Serverport.Text) || int.Parse(Serverport.Text) < 1)
            {
                base.RegisterStartupScript("", "<script>alert('远程访问端口必须为数字并且大于1');window.location.href='global_ftpsetting.aspx?ftptype=" + ftpType + "';</script>");
                return;
            }
            if (!Utils.IsNumeric(Timeout.Text) || int.Parse(Timeout.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('超时时间必须为数字并且大于1');window.location.href='global_ftpsetting.aspx?ftptype=" + ftpType + "';</script>");
                return;
            }
            FTPConfigInfoCollection ftpConfigInfoCollection = 
                (FTPConfigInfoCollection)SerializationHelper.Load(typeof(FTPConfigInfoCollection), Server.MapPath("../../config/ftp.config"));
            bool isEdit = false;
            foreach (FTPConfigInfo fci in ftpConfigInfoCollection)
            {
                if (fci.Name == ftpType)
                {
                    fci.Serveraddress = Serveraddress.Text;
                    fci.Serverport = int.Parse(Serverport.Text);
                    fci.Username = Username.Text;
                    fci.Password = Password.Text;
                    fci.Mode = int.Parse(Mode.SelectedValue);
                    fci.Uploadpath = Uploadpath.Text;
                    fci.Timeout = int.Parse(Timeout.Text);
                    fci.Allowupload = int.Parse(Allowupload.SelectedValue);
                    fci.Remoteurl = Remoteurl.Text;
                    fci.Reservelocalattach = int.Parse(Reservelocalattach.SelectedValue);
                    fci.Reserveremoteattach = int.Parse(Reserveremoteattach.SelectedValue);
                    isEdit = true;
                    break;
                }
            }
            if (!isEdit)
            {
                FTPConfigInfo fci = new FTPConfigInfo();
                fci.Name = ftpType;
                fci.Serveraddress = Serveraddress.Text;
                fci.Serverport = int.Parse(Serverport.Text);
                fci.Username = Username.Text;
                fci.Password = Password.Text;
                fci.Mode = int.Parse(Mode.SelectedValue);
                fci.Uploadpath = ftpType.ToLower();
                fci.Timeout = int.Parse(Timeout.Text);
                fci.Allowupload = int.Parse(Allowupload.SelectedValue);
                fci.Remoteurl = Remoteurl.Text;
                fci.Reservelocalattach = int.Parse(Reservelocalattach.SelectedValue);
                fci.Reserveremoteattach = int.Parse(Reserveremoteattach.SelectedValue);
                ftpConfigInfoCollection.Add(fci);
            }
            SerializationHelper.Save(ftpConfigInfoCollection, Server.MapPath("../../config/ftp.config"));
            Response.Redirect("global_ftpsetting.aspx?ftptype=" + ftpType);            
        }

        private string GetParam(string param)
        {
            switch (param)
            {
                case "forumattach":
                    return "ForumAttach";
                case "spaceattach":
                    return "SpaceAttach";
                case "albumattach":
                    return "AlbumAttach";
                default:
                    return param;
            }
        }
    }
}
