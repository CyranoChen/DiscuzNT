using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Cache;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class topictypesgrid : AdminPage
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData("");	//���������
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        public void BindData(string searthKeyWord)
        {
            #region ������
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "�������";
            DataGrid1.BindData(TopicTypes.GetTopicTypes(searthKeyWord));
            #endregion
        }

        /// <summary>
        /// ��ҳ
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        private int GetDisplayOrder(string topicTypeName, DataTable topicTypes)
        {
            #region ������������
            foreach (DataRow dr in topicTypes.Rows)
            {
                if (dr["name"].ToString().Trim() == topicTypeName.Trim())
                {
                    return int.Parse(dr["displayorder"].ToString());
                }
            }
            return -1;
            #endregion
        }

        private string GetTopicTypeString(string topicTypes, string topicName)
        {
            #region ���������Ƿ�������
            foreach (string type in topicTypes.Split('|'))
            {
                if (type.IndexOf("," + topicName.Trim() + ",") != -1)
                    return type;
            }
            return "";
            #endregion
        }


        /// <summary>
        /// �������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddNewRec_Click(object sender, EventArgs e)
        {
            #region �������������
            //��������Ƿ�Ϸ�
            if (!CheckValue(typename.Text, displayorder.Text, description.Text)) return;

            //����Ƿ���ͬ���������
            if(TopicTypes.IsExistTopicType(typename.Text))
            {
                base.RegisterStartupScript( "", "<script>alert('���ݿ����Ѵ�����ͬ�������������');window.location.href='forum_topictypesgrid.aspx';</script>");
                return;
            }

            //���ӷ��ൽdnt_topictypes,��д��־
            TopicTypes.CreateTopicTypes(typename.Text, int.Parse(displayorder.Text), description.Text);
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "����������", "����������,����Ϊ:" + typename.Text);

            //���·��໺��
            DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypes");
            base.RegisterStartupScript("", "<script>window.location.href='forum_topictypesgrid.aspx';</script>");
            return;
            #endregion
        }

        /// <summary>
        /// ɾ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void delButton_Click(object sender, EventArgs e)
        {
            #region ɾ���������
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    //ȡ��Ҫɾ����ID�б��ԡ������ָ�
                    string idlist = DNTRequest.GetString("id");

                    //���ø��°��ķ���
                    DeleteForumTypes(idlist);

                    //����������(dnt_topictypes)��ɾ����Ӧ�ķ��ಢд��־
                    TopicTypes.DeleteTopicTypes(idlist);
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ɾ���������", "ɾ���������,IDΪ:" + DNTRequest.GetString("id").Replace("0 ", ""));

                    //����������໺��
                    DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypes");
                    Response.Redirect("forum_topictypesgrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('��δѡ���κ�ѡ��');window.location.href='forum_attachtypesgrid.aspx';</script>");
                }
            }
            #endregion
        }

        /// <summary>
        /// �������Ƿ�Ϸ�
        /// </summary>
        /// <param name="typename">�����������</param>
        /// <param name="displayorder">�������</param>
        /// <param name="description">����</param>
        /// <returns>�Ϸ�����treu�����򷵻�false</returns>
        private bool CheckValue(string typename, string displayorder, string description)
        {
            #region �������Ƿ�Ϸ�
            if (typename == "" || typename.Length > 100 )
            {
                base.RegisterStartupScript("", "<script>alert('����������Ʋ���Ϊ��');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }

            if ((displayorder == "") || (Convert.ToInt32(displayorder) < 0))
            {
                base.RegisterStartupScript("", "<script>alert('��ʾ˳����Ϊ�� ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }

            if (description.Length > 500)
            {
                base.RegisterStartupScript("", "<script>alert('�������ܳ���500����');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            if( typename.IndexOf("|") > 0 )
            {
                base.RegisterStartupScript("", "<script>alert('���ܺ��зǷ��ַ� | ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            return true;
            #endregion
        }

        /// <summary>
        /// ǰ̨�󶨡�������̳���ķ���
        /// </summary>
        /// <param name="id">��������ID</param>
        /// <returns>���ع�����̳�������ַ���</returns>
        public string LinkForum(string id)
        {
            #region �����������󶨵���̳����
            return Forums.GetForumLinkOfAssociatedTopicType(Convert.ToInt32(id));
            #endregion
        }

        /// <summary>
        /// ɾ������е��������
        /// </summary>
        /// <param name="idlist">Ҫɾ����������ID�б�</param>
        private void DeleteForumTypes(string idlist)
        {
            #region ɾ����ѡ���������

            TopicTypes.DeleteForumTopicTypes(idlist);

            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region ���ñ༭����
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).Width = 150;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).MaxLength = 30;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Width = 30;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).Width = 250;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).MaxLength = 500;
            }
            #endregion
        }

        private void SaveTopicType_Click(object sender, EventArgs e)
        {
            #region �����������༭
            //������ȡ�༭�еĸ���ֵ
            int rowid = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                string id = o.ToString();
                string name = DataGrid1.GetControlValue(rowid, "name");
                string displayorder = DataGrid1.GetControlValue(rowid, "displayorder");
                string description = DataGrid1.GetControlValue(rowid, "description");


                //�ж������������Ƿ�����Ҫ���µ�����

                if (!CheckValue(name, displayorder, description) || TopicTypes.IsExistTopicType(name, int.Parse(id)))
                {
                    error = true;
                    continue;
                }

                //ȡ���������Ļ���
                Discuz.Common.Generic.SortedList<int, string> topictypearray = new Discuz.Common.Generic.SortedList<int, string>();
                topictypearray = Caches.GetTopicTypeArray();

                DataTable dt = Forums.GetExistTopicTypeOfForum();
                DataTable topicTypes = TopicTypes.GetTopicTypes();
                foreach (DataRow dr in dt.Rows)
                {
                    //����������dnt_forumfields���topictypes�ֶ�
                    string topictypes = dr["topictypes"].ToString();
                    if (topictypes.Trim() == "")    //��������б�Ϊ���򲻴���
                        continue;
                    string oldTopicType = GetTopicTypeString(topictypes, topictypearray[Int32.Parse(id)].ToString().Trim()); //��ȡ�޸�����ǰ�ľ������б�
                    if (oldTopicType == "")    //��������б��в�������ǰҪ�޸ĵ����⣬�򲻴���
                        continue;
                    string newTopicType = oldTopicType.Replace("," + topictypearray[Int32.Parse(id)].ToString().Trim() + ",", "," + name + ",");
                    topictypes = topictypes.Replace(oldTopicType + "|", ""); //���ɵ������б����̳�����б���ɾ��
                    ArrayList topictypesal = new ArrayList();
                    foreach (string topictype in topictypes.Split('|'))
                    {
                        if (topictype != "")
                            topictypesal.Add(topictype);
                    }
                    bool isInsert = false;
                    for (int i = 0; i < topictypesal.Count; i++)
                    {
                        int curDisplayOrder = GetDisplayOrder(topictypesal[i].ToString().Split(',')[1], topicTypes);
                        if (curDisplayOrder > int.Parse(displayorder))
                        {
                            topictypesal.Insert(i, newTopicType);
                            isInsert = true;
                            break;
                        }
                    }
                    if (!isInsert)
                    {
                        topictypesal.Add(newTopicType);
                    }
                    topictypes = "";
                    foreach (object t in topictypesal)
                    {
                        topictypes += t.ToString() + "|";
                    }
                    TopicTypes.UpdateForumTopicType(topictypes, int.Parse(dr["fid"].ToString()));
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypesOption" + dr["fid"].ToString());
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypesLink" + dr["fid"].ToString());
                }

                //������������(dnt_topictypes)
                TopicTypes.UpdateTopicTypes(name, int.Parse(displayorder), description, int.Parse(id));
                rowid++;
            }

            //���»���
            DNTCache cache = DNTCache.GetCacheService();
            cache.RemoveObject("/Forum/TopicTypes");
            if(error)
                base.RegisterStartupScript("", "<script>alert('���ݿ����Ѵ�����ͬ������������ƻ�Ϊ�գ��ü�¼���ܱ����£�');window.location.href='forum_topictypesgrid.aspx';</script>");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='forum_topictypesgrid.aspx';");
            return;
            #endregion
        }

        private void Search_Click(object sender, EventArgs e)
        {
            BindData(topictypename.Text);
            searchtable.Visible = false;
            ResetSearchTable.Visible = true;
        }

        private void ResetSearchTable_Click(object sender, EventArgs e)
        {
            Response.Redirect("forum_topictypesgrid.aspx");
        }

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.delButton.Click += new EventHandler(this.delButton_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.SaveTopicType.Click += new EventHandler(this.SaveTopicType_Click);
            this.Search.Click += new EventHandler(this.Search_Click);
            this.ResetSearchTable.Click += new EventHandler(this.ResetSearchTable_Click);
            DataGrid1.ColumnSpan = 5;
        }
        #endregion
    }
}
