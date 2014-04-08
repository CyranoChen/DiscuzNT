using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����б�
    /// </summary>
    public partial class advsgrid : AdminPage
    {
        public int type = DNTRequest.GetInt("type", -1);
        public string[] advtypes = new string[] { "ͷ��������", "β��������", "ҳ�����ֹ��", "���ڹ��", "�������", "�������", "Silverlightý����", "����ͨ�����", "�������", "���ٷ������Ϸ����", "���ٱ༭���������", "�ۺ���ҳͷ�����", "�ۺ���ҳ�����·����", "�ۺ���ҳ���������Ϸ����", "�ۺ���ҳ�Ƽ�����Ϸ����", "�ۺ���ҳ�Ƽ�����·����", "�ۺ���ҳ�Ƽ�����·����", "�ۺ���ҳ�ײ����", "ҳ�ں�����" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitAdvertisementAvailable();
                BindData(type);
            }
        }

        private void InitAdvertisementAvailable()
        {
            DataRow[] drs = Advertisements.GetAdvertisements(type).Select("endtime < '" + DateTime.Now.ToString() + "'");
            if (drs.Length == 0)
                return;
            string aidList = "";
            foreach (DataRow dr in drs)
            {
                aidList += dr["advid"] + ",";
            }
            if (aidList != "")
                Advertisements.UpdateAdvertisementAvailable(aidList.TrimEnd(','), 0);
        }

        public void BindData(int type)
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.BindData(Advertisements.GetAdvertisements(type));
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DelAds_Click(object sender, EventArgs e)
        {
            #region ɾ��ָ���Ĺ��
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("advid") != "")
                {
                    Advertisements.DeleteAdvertisementList(DNTRequest.GetString("advid"));
                    DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_advsgrid.aspx';");
                }
                else
                    base.RegisterStartupScript("", "<script>alert('��δѡ���κ�ѡ��');window.location.href='global_advsgrid.aspx';</script>");
            }
            #endregion
        }

        public string BoolStr(string closed)
        {
            #region ����Ƿ���ЧͼƬ,����ǰ̨��
            if (closed == "1")
                return "<div align=center><img src=../images/OK.gif /></div>";
            else
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            #endregion
        }

        public string ParameterType(string parameters)
        {
            return parameters.Split('|')[0];
        }

        public string TargetsType(string targets)
        {
            #region �����Ͷ�ŷ�Χ�ı�ʶ��ת��Ϊ����
            string result = ""; //���Ͷ�ŷ�Χ�ı�ʶ��
            if (targets.IndexOf("ȫ��") >= 0) return "ȫ��";
            else
            {
                if (targets.IndexOf("��ҳ") >= 0)
                {
                    result = "��ҳ,";
                    targets = targets.Replace("��ҳ,", "");
                }
            }

            if (targets.Trim() != "��ҳ")
                foreach (ForumInfo info in Forums.GetForumList(targets))
                    result += info.Name + ",";

            return result.Length > 0 ? result.Substring(0, result.Length - 1) : "";
            #endregion
        }


        private void SetUnAvailable_Click(object sender, EventArgs e)
        {
            UpdateAdvertisementAvailable(0);
        }

        private void SetAvailable_Click(object sender, EventArgs e)
        {
            UpdateAdvertisementAvailable(1);
        }

        private void UpdateAdvertisementAvailable(int available)
        {
            #region ���ù���Ϊ��Ч״̬
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("advid") != "")
                {
                    Advertisements.UpdateAdvertisementAvailable(DNTRequest.GetString("advid"), available);
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_advsgrid.aspx';");
                }
                else
                    base.RegisterStartupScript("", "<script>alert('��δѡ���κ�ѡ��');window.location.href='global_advsgrid.aspx';</script>");
            }
            #endregion
        }

        public string GetAdType(string adtype)
        {
            return advtypes[TypeConverter.StrToInt(adtype)];
        }


        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SetAvailable.Click += new EventHandler(this.SetAvailable_Click);
            this.SetUnAvailable.Click += new EventHandler(this.SetUnAvailable_Click);
            this.DelAds.Click += new EventHandler(this.DelAds_Click);

            #region ������
            DataGrid1.TableHeaderName = "����б�";
            DataGrid1.DataKeyField = "advid";
            DataGrid1.ColumnSpan = 12;
            #endregion
        }

        #endregion
    }
}