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
                BindData("");	//绑定主题分类
            }
        }

        /// <summary>
        /// 绑定主题
        /// </summary>
        public void BindData(string searthKeyWord)
        {
            #region 绑定主题
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "主题分类";
            DataGrid1.BindData(TopicTypes.GetTopicTypes(searthKeyWord));
            #endregion
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        private int GetDisplayOrder(string topicTypeName, DataTable topicTypes)
        {
            #region 返回主题的序号
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
            #region 返回主题是否在其内
            foreach (string type in topicTypes.Split('|'))
            {
                if (type.IndexOf("," + topicName.Trim() + ",") != -1)
                    return type;
            }
            return "";
            #endregion
        }


        /// <summary>
        /// 增加新主题分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddNewRec_Click(object sender, EventArgs e)
        {
            #region 增加新主题分类
            //检查输入是否合法
            if (!CheckValue(typename.Text, displayorder.Text, description.Text)) return;

            //检查是否有同名分类存在
            if(TopicTypes.IsExistTopicType(typename.Text))
            {
                base.RegisterStartupScript( "", "<script>alert('数据库中已存在相同的主题分类名称');window.location.href='forum_topictypesgrid.aspx';</script>");
                return;
            }

            //增加分类到dnt_topictypes,并写日志
            TopicTypes.CreateTopicTypes(typename.Text, int.Parse(displayorder.Text), description.Text);
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加主题分类", "添加主题分类,名称为:" + typename.Text);

            //更新分类缓存
            DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypes");
            base.RegisterStartupScript("", "<script>window.location.href='forum_topictypesgrid.aspx';</script>");
            return;
            #endregion
        }

        /// <summary>
        /// 删除主题分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void delButton_Click(object sender, EventArgs e)
        {
            #region 删除主题分类
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    //取得要删除的ID列表，以“，”分隔
                    string idlist = DNTRequest.GetString("id");

                    //调用更新版块的方法
                    DeleteForumTypes(idlist);

                    //从主题分类表(dnt_topictypes)中删除相应的分类并写日志
                    TopicTypes.DeleteTopicTypes(idlist);
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除主题分类", "删除主题分类,ID为:" + DNTRequest.GetString("id").Replace("0 ", ""));

                    //更新主题分类缓存
                    DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypes");
                    Response.Redirect("forum_topictypesgrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_attachtypesgrid.aspx';</script>");
                }
            }
            #endregion
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <param name="typename">主题分类名称</param>
        /// <param name="displayorder">排序次序</param>
        /// <param name="description">描述</param>
        /// <returns>合法返回treu，否则返回false</returns>
        private bool CheckValue(string typename, string displayorder, string description)
        {
            #region 检查参数是否合法
            if (typename == "" || typename.Length > 100 )
            {
                base.RegisterStartupScript("", "<script>alert('主题分类名称不能为空');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }

            if ((displayorder == "") || (Convert.ToInt32(displayorder) < 0))
            {
                base.RegisterStartupScript("", "<script>alert('显示顺序不能为空 ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }

            if (description.Length > 500)
            {
                base.RegisterStartupScript("", "<script>alert('描述不能长于500个符');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            if( typename.IndexOf("|") > 0 )
            {
                base.RegisterStartupScript("", "<script>alert('不能含有非法字符 | ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            return true;
            #endregion
        }

        /// <summary>
        /// 前台绑定“关联论坛”的方法
        /// </summary>
        /// <param name="id">主题分类的ID</param>
        /// <returns>返回关联论坛的链接字符串</returns>
        public string LinkForum(string id)
        {
            #region 返回主题分类绑定的论坛名称
            return Forums.GetForumLinkOfAssociatedTopicType(Convert.ToInt32(id));
            #endregion
        }

        /// <summary>
        /// 删除版块中的主题分类
        /// </summary>
        /// <param name="idlist">要删除主题分类的ID列表</param>
        private void DeleteForumTypes(string idlist)
        {
            #region 删除所选的主题分类

            TopicTypes.DeleteForumTopicTypes(idlist);

            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 设置编辑框宽度
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
            #region 保存主题分类编辑
            //下四行取编辑行的更新值
            int rowid = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                string id = o.ToString();
                string name = DataGrid1.GetControlValue(rowid, "name");
                string displayorder = DataGrid1.GetControlValue(rowid, "displayorder");
                string description = DataGrid1.GetControlValue(rowid, "description");


                //判断主题分类表中是否有与要更新的重名

                if (!CheckValue(name, displayorder, description) || TopicTypes.IsExistTopicType(name, int.Parse(id)))
                {
                    error = true;
                    continue;
                }

                //取得主题分类的缓存
                Discuz.Common.Generic.SortedList<int, string> topictypearray = new Discuz.Common.Generic.SortedList<int, string>();
                topictypearray = Caches.GetTopicTypeArray();

                DataTable dt = Forums.GetExistTopicTypeOfForum();
                DataTable topicTypes = TopicTypes.GetTopicTypes();
                foreach (DataRow dr in dt.Rows)
                {
                    //用新名更新dnt_forumfields表的topictypes字段
                    string topictypes = dr["topictypes"].ToString();
                    if (topictypes.Trim() == "")    //如果主题列表为空则不处理
                        continue;
                    string oldTopicType = GetTopicTypeString(topictypes, topictypearray[Int32.Parse(id)].ToString().Trim()); //获取修改名字前的旧主题列表
                    if (oldTopicType == "")    //如果主题列表中不包含当前要修改的主题，则不处理
                        continue;
                    string newTopicType = oldTopicType.Replace("," + topictypearray[Int32.Parse(id)].ToString().Trim() + ",", "," + name + ",");
                    topictypes = topictypes.Replace(oldTopicType + "|", ""); //将旧的主题列表从论坛主题列表中删除
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

                //更新主题分类表(dnt_topictypes)
                TopicTypes.UpdateTopicTypes(name, int.Parse(displayorder), description, int.Parse(id));
                rowid++;
            }

            //更新缓存
            DNTCache cache = DNTCache.GetCacheService();
            cache.RemoveObject("/Forum/TopicTypes");
            if(error)
                base.RegisterStartupScript("", "<script>alert('数据库中已存在相同的主题分类名称或为空，该记录不能被更新！');window.location.href='forum_topictypesgrid.aspx';</script>");
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

        #region Web 窗体设计器生成的代码
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
