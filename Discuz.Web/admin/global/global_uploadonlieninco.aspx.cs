using System;
using System.Data;
using System.IO;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 上传在线图例. 
    /// </summary>
    
    public partial class uploadonlieninco : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                incolist.DataSource = LoadDataInfo();
                incolist.DataBind();
            }
        }

        public DataTable LoadDataInfo()
        {
            #region 加载数据信息
            image.UpFilePath = Server.MapPath(image.UpFilePath);

            DataTable dt = new DataTable("img");
            dt.Columns.Add("imgfile", Type.GetType("System.String"));

            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../images/groupicons"));
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if ((file != null) && (file.Extension != ""))
                {
                    if ((file.Extension.ToLower() == ".jpg") || (file.Extension.ToLower() == ".gif") || (file.Extension.ToLower() == ".png") || ((file.Extension.ToLower() == ".jpeg")))
                    {
                        DataRow dr = dt.NewRow();
                        dr["imgfile"] = file.Name;
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;

            #endregion
        }

        private void UpdateOnLineIncoCache_Click(object sender, EventArgs e)
        {
            #region 更新在线图例缓存
            if (this.CheckCookie())
            {
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.RemoveObject("/Forum/UI/OnlineIconList");
                image.UpdateFile();
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_onlinelistgrid.aspx';");
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.UpdateOnLineIncoCache.Click += new EventHandler(this.UpdateOnLineIncoCache_Click);
        }

        #endregion
    }
}