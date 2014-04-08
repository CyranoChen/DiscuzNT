using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using System.Collections;
using Discuz.Cache;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// ���˴��б�
    /// </summary>

    public partial class wordgrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region �󶨹��˴��б�
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "���˴��б�";
            DataGrid1.BindData(BanWords.GetBanWordList());

            antipamreplacement.Text = config.Antispamreplacement;
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression;
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void SaveWord_Click(object sender, EventArgs e)
        {
            #region ����������޸�
            int row = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string find = DataGrid1.GetControlValue(row, "find").Trim();
                string replacement = DataGrid1.GetControlValue(row, "replacement").Trim();
                if (find == "" || replacement == "")
                {
                    error = true;
                    continue;
                }
                BanWords.UpdateBanWord(id, find, replacement);
                row++;
            }
            DNTCache.GetCacheService().RemoveObject("/Forum/BanWordList");
            Caches.GetBanWordList();
            if (error)
                base.RegisterStartupScript("PAGE", "alert('ĳЩ��Ϣ��������δ�ܸ��£�');window.location.href='global_wordgrid.aspx';");
            base.RegisterStartupScript("PAGE", "window.location.href='global_wordgrid.aspx';");
            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region ���ݰ���ʾ���ȿ���

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox t = (TextBox)e.Item.Cells[4].Controls[0];
                t.Attributes.Add("maxlength", "254");
                t.Attributes.Add("size", "30");

                t = (TextBox)e.Item.Cells[5].Controls[0];
                t.Attributes.Add("maxlength", "254");
                t.Attributes.Add("size", "30");
            }

            #endregion
        }

        /// <summary>
        /// �������������ַ��б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAntiPamReplacement_Click(object sender, EventArgs e)
        {
            config.Antispamreplacement = antipamreplacement.Text;

            GeneralConfigs.Serialiaze(config, Server.MapPath("../../config/general.config"));
            Caches.ReSetConfig();
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "���������ַ�", "");
            base.RegisterStartupScript("PAGE", "window.location.href='global_wordgrid.aspx';");
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region ɾ�����˴�

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    BanWords.DeleteBanWords(DNTRequest.GetString("id"));
                    Response.Redirect("global_wordgrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('��δѡ���κ�ѡ��');window.location.href='global_wordgrid.aspx';</script>");
                }
            }

            #endregion
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            #region ��ӹ��˴�

            if (find.Text == "")
            {
                base.RegisterStartupScript("", "<script>alert('Ҫ��ӵĹ������ݲ���Ϊ��');window.location.href='global_wordgrid.aspx';</script>");
                return;
            }

            if (replacement.Text == "")
            {
                base.RegisterStartupScript("", "<script>alert('Ҫ��ӵ��滻���ݲ���Ϊ��');window.location.href='global_wordgrid.aspx';</script>");
                return;
            }

            if (BanWords.IsExistBanWord(find.Text))
            {
                base.RegisterStartupScript("", "<script>alert('���ݿ����Ѵ�����ͬ�Ĺ�������');window.location.href='global_wordgrid.aspx';</script>");
                return;
            }


            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�����ַ�����", "�ַ�Ϊ:" + find.Text);

            try
            {
                BanWords.CreateBanWord(username, find.Text, replacement.Text);
                BindData();
                base.RegisterStartupScript("PAGE", "window.location.href='global_wordgrid.aspx';");
                return;
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('�޷��������ݿ�.');window.location.href='global_wordgrid.aspx';</script>");
                return;
            }

            #endregion
        }

        private String[] getwords()
        {
            return badwords.Text.Split('\n');
        }

        private void addbadwords_Click(object sender, EventArgs e)
        {
            if (this.badwords.Text == "")
            {
                BindData();
                return;
            }

            String[] badwords = getwords();
            String[] filterwords;

            #region ����radiobuttonlistѡ������
            if (radfilter.SelectedValue == "0")
            {
                //��յ�ǰ�����,�ٲ���
                string find = "";
                string replacement = "";
                string banWordsIdList = "";
                foreach (DataRow dr in BanWords.GetBanWordList().Rows)
                {
                    banWordsIdList += dr["id"].ToString() + ",";
                }
                if (banWordsIdList != "")
                    BanWords.DeleteBanWords(banWordsIdList.TrimEnd(','));

                for (int i = 0; i < badwords.Length; i++)
                {
                    filterwords = badwords[i].Split('=');

                    find = filterwords[0].ToString().Replace("\r", "").Trim();

                    if (!GetReplacement(badwords, filterwords, ref find, ref replacement))
                    {
                        continue;
                    }

                    BanWords.CreateBanWord(username, find, replacement);
                }

            }

            if (radfilter.SelectedValue == "1")
            {
                //ʹ���µ����ø����Ѿ����ڵĴ���
                string find = "";
                string replacement = "";

                for (int i = 0; i < badwords.Length; i++)
                {
                    filterwords = badwords[i].Split('=');

                    find = filterwords[0].ToString().Replace("\r", "").Trim();

                    if (!GetReplacement(badwords, filterwords, ref find, ref replacement))
                    {
                        continue;
                    }
                    BanWords.UpdateBadWords(find, replacement);
                }
            }

            if (radfilter.SelectedValue == "2")
            {
                //�������Ѿ����ڵĴ���

                string find = "";
                string replacement = "";

                DataTable dt = BanWords.GetBanWordList();

                for (int i = 0; i < badwords.Length; i++)
                {

                    #region
                    //filterwords = badwords[i].Split('=');

                    //find = filterwords[0].ToString().Replace("\r", "").Trim();

                    //#region

                    //if (find == "")
                    //{
                    //    continue;
                    //}

                    //if (filterwords.Length == 2 && filterwords[1].ToString() != "")
                    //{
                    //        replacement = filterwords[1].ToString();
                    //}
                    //else if (filterwords.Length < 2)
                    //{
                    //    replacement = "**";
                    //}
                    //else//filterwords.Length > 2 �����
                    //{
                    //    replacement = filterwords[filterwords.Length - 1];

                    //    filterwords.SetValue("", filterwords.Length - 1);

                    //    find = string.Join("=", filterwords);
                    //    find = find.Remove(find.Length - 2);
                    //}


                    //#endregion
                    #endregion

                    filterwords = badwords[i].Split('=');

                    find = filterwords[0].ToString().Replace("\r", "").Trim();

                    if (!GetReplacement(badwords, filterwords, ref find, ref replacement))
                    {
                        continue;
                    }


                    DataRow[] arrRow = dt.Select("find='" + find + "'");

                    if (arrRow.Length == 0)
                    {
                        BanWords.CreateBanWord(username, find, replacement);
                    }

                }
            }
            #endregion

            BindData();

            this.badwords.Text = "";
        }

        private static bool GetReplacement(String[] badwords, String[] filterwords, ref string find, ref string replacement)
        {
            if (find == "")
            {
                return false;
            }

            if (filterwords.Length == 2)
            {
                for (int m = 0; m < badwords.Length; m++)
                {
                    if (filterwords[1].ToString() != "")
                    {
                        replacement = filterwords[1].ToString();
                    }
                }
            }
            else if (filterwords.Length < 2)
            {
                for (int m = 0; m < badwords.Length; m++)
                {
                    replacement = "**";
                }
            }
            else
            {
                replacement = filterwords[filterwords.Length - 1];

                filterwords.SetValue("", filterwords.Length - 1);

                find = string.Join("=", filterwords);
                find = find.Remove(find.Length - 2);
            }

            if (replacement == string.Empty)
            {
                replacement = "**";
            }
            return true;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.SaveWord.Click += new EventHandler(this.SaveWord_Click);
            this.addbadwords.Click += new EventHandler(addbadwords_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.saveantipamreplacement.Click += new EventHandler(this.SaveAntiPamReplacement_Click);
            DataGrid1.ColumnSpan = 5;
        }

        #endregion

    }
}