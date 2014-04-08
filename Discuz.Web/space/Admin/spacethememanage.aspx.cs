using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Data;
using System.Data;
using Discuz.Web.Admin;
using Discuz.Space.Data;


namespace Discuz.Space.Admin
{

#if NET1
    public class SpaceThemeManage : AdminPage
#else
    public partial class SpaceThemeManage : AdminPage
#endif
    {

#if NET1
        #region �ؼ�����
        protected Discuz.Control.DataGrid themegrid;
        protected Discuz.Control.Button SubmitButton;
        protected Discuz.Control.TextBox themeName;
		protected Discuz.Control.Button SaveTheme;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ThemeGridBind();
            }
        }

        private void ThemeGridBind()
        {
            #region �󶨸��˿ռ������б�
            themegrid.TableHeaderName = "���˿ռ��������";
            themegrid.DataKeyField = "themeid";
            themegrid.BindData(DbProvider.GetInstance().GetSpaceThemes());
            #endregion
        }

        public void SubmitButton_Click(object sender, EventArgs e)
        {
            #region �ύ�¸��˿ռ�����
            if (themeName.Text == "")
            {
                base.RegisterStartupScript( "alert", "<script type='text/javascript'>alert('����������Ʋ���Ϊ��');window.location.href='space_spacethememanage.aspx';</script>");
				return;
			}
            if (DbProvider.GetInstance().IsThemeExist(themeName.Text))
			{
                base.RegisterStartupScript( "alert", "<script type='text/javascript'>alert('������������Ѿ�����');window.location.href='space_spacethememanage.aspx';</script>");
				return;
			}
            DbProvider.GetInstance().AddSpaceTheme(string.Empty, themeName.Text, 0, string.Empty, Utils.GetDate(),string.Empty);
            base.RegisterStartupScript( "", "<script>window.location.href='space_spacethememanage.aspx';</script>");
            #endregion
        }

        private void SaveTheme_Click(object sender, EventArgs e)
        {
            #region ������˿ռ�������޸�
            if (this.CheckCookie())
            {
                int rowid = -1;
                bool error = false;
                foreach (object o in themegrid.GetKeyIDArray())
                {
                    string id = o.ToString();
                    string name = themegrid.GetControlValue(rowid, "name");
                    if (name == "")
                    {
                        error = true;
                        continue;
                    }
                    DbProvider.GetInstance().UpdateThemeName(Convert.ToInt32(id), name);
                    rowid++;
                }

                if(error)
                    base.RegisterStartupScript("", "<script>alert('ĳЩ��¼ȡֵ�Ƿ���δ�ܱ����£�');window.location.href='space_spacethememanage.aspx';</script>");
                else
                    base.RegisterStartupScript("", "<script>window.location.href='space_spacethememanage.aspx';</script>");
            }
            #endregion
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            #region ɾ�����˿ռ�����
            if (this.CheckCookie())
            {
                string idlist = DNTRequest.GetString("id");
                foreach (string id in idlist.Split(','))
                {
                    DbProvider.GetInstance().DeleteTheme(Convert.ToInt32(id));
                }
                base.RegisterStartupScript("", "<script>window.location.href='space_spacethememanage.aspx';</script>");
            }
            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
            this.Load += new EventHandler(this.Page_Load);
            this.SaveTheme.Click += new EventHandler(this.SaveTheme_Click);
            themegrid.ColumnSpan = 5;
            themegrid.AllowPaging = false;
        }

        #endregion

    
    }
}