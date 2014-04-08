using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;

using Discuz.Common;
using Discuz.Data;
using Discuz.Common.Generic;

namespace Discuz.Control
{
    /// <summary>
    /// DataGrid 列表控件
    /// </summary>
    [DefaultProperty("Text"), ToolboxData("<{0}:DataGrid runat=server></{0}:DataGrid>")]
    public class DataGrid : System.Web.UI.WebControls.DataGrid, INamingContainer
    {
        /// <summary>
        /// 跳转按钮
        /// </summary>
        public Discuz.Control.Button GoToPagerButton = new Discuz.Control.Button();


        /// <summary>
        /// 跳转文本框
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputText GoToPagerInputText = new HtmlInputText();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataGrid()
            : base()
        {
            //this.GridLines=GridLines.Horizontal;
            //this.BorderWidth=0;
            //this.CellPadding=3;
            this.CssClass = "datalist";
            //this.ShowFooter=true;
            this.ShowHeader = true;
            this.AutoGenerateColumns = false;
            //this.PagerStyle.CssClass="datagridPager";
            //this.FooterStyle.CssClass="datagridFooter";
            this.SelectedItemStyle.CssClass = "datagridSelectedItem";
            this.ItemStyle.CssClass = "";// "datagridItem";
            this.HeaderStyle.CssClass = "category";
            this.PageSize = 25;
            this.PagerStyle.Mode = PagerMode.NumericPages;
            this.AllowCustomPaging = false;
            this.AllowPaging = true;
            this.AllowSorting = true;
            this.DataKeyField = "ID";

            GoToPagerInputText.Attributes.Add("onfocus", "this.className='colorfocus';");
            GoToPagerInputText.Attributes.Add("onblur", "this.className='colorblur';");
            GoToPagerInputText.Attributes.Add("CssClass", "colorblur");

            this.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid_ItemCreated);
            this.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.SortGrid);
            GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
        public DataGrid(string sqlstring)
            : this()
        {
            BindData(sqlstring);
        }


        /// <summary>
        /// 加载默认列(编辑列和删除列)
        /// </summary>
        public void LoadDefaultColumn()
        {
            this.LoadEditColumn();
            this.LoadDeleteColumn();
        }


        /// <summary>
        /// 跳转按钮链接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GoToPagerButton_Click(object sender, System.EventArgs e)
        {
            if (this.GoToPagerInputText.Value != "")
            {

                if (!this.AllowCustomPaging)
                {


                    if (!Regex.IsMatch(this.GoToPagerInputText.Value, "^\\d+?\\d*$"))
                    {
                        LoadCurrentPageIndex(0);
                        return;
                    }
                    else
                    {
                        int gotoPager = Utils.StrToInt(this.GoToPagerInputText.Value.Trim(), 1);

                        if (gotoPager < 0)
                        {
                            LoadCurrentPageIndex(0);
                            return;
                        }

                        if (gotoPager > this.PageCount)
                        {
                            LoadCurrentPageIndex(this.PageCount - 1);
                        }
                        else
                        {
                            LoadCurrentPageIndex(gotoPager - 1);
                        }
                    }
                }
                else
                {
                    //设置当前DATAGRID的CurrentPageIndex属性
                    SetCurrentPageIndexByGoToPager();
                }
            }
        }



        /// <summary>
        /// 设置当前DATAGRID的CurrentPageIndex属性
        /// </summary>
        private void SetCurrentPageIndexByGoToPager()
        {
            if (this.GoToPagerInputText.Value != "")
            {

                if (!Regex.IsMatch(this.GoToPagerInputText.Value, "^\\d+?\\d*$"))
                {
                    this.CurrentPageIndex = 0;
                    return;
                }
                else
                {
                    int gotoPager = Utils.StrToInt(this.GoToPagerInputText.Value.Trim(), 1);

                    if (gotoPager < 0)
                    {
                        CurrentPageIndex = 0;
                        return;
                    }

                    if (gotoPager > PageCount)
                    {
                        CurrentPageIndex = this.PageCount - 1;
                    }
                    else
                    {
                        this.CurrentPageIndex = gotoPager - 1;
                    }
                }
            }
        }

        /// <summary>
        /// 加载编辑列
        /// </summary>
        public void LoadEditColumn()
        {
            EditCommandColumn ecc = new EditCommandColumn();//更新按钮列 
            ecc.SortExpression = "desc";
            ecc.ButtonType = ButtonColumnType.LinkButton;//链接按钮 
            ecc.EditText = "编辑";
            ecc.UpdateText = "更新";
            ecc.CancelText = "取消";
            ecc.ItemStyle.Width = 70;
            ecc.ItemStyle.BorderWidth = 1;
            ecc.ItemStyle.BorderStyle = BorderStyle.Solid;
            ecc.ItemStyle.BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);
            this.Columns.AddAt(0, ecc);//增加按钮列 
        }

        /// <summary>
        /// 加载删除列
        /// </summary>
        public void LoadDeleteColumn()
        {
            ButtonColumn bc = new ButtonColumn();
            bc.SortExpression = "desc";
            bc.CommandName = "Delete";
            bc.Text = "删除";
            //bc.FooterText="添加";
            bc.ItemStyle.Width = 70;
            this.Columns.AddAt(1, bc);//增加按钮列 
        }

  
        /// <summary>
        /// 绑定数据
        /// </summary>
        public void BindData()
        {
            if ((this.SourceDataTable != null))
            {
                BindData(this.SourceDataTable);
            }
            else
            {
                ;
            }
        }


        /// <summary>
        /// 绑定数据对象
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
        public void BindData(string sqlstring)
        {
            this.SqlText = sqlstring;
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            BindData(dt);
        }

        public void BindData(DataTable dt)
        {
            this.SourceDataTable = dt;
            this.VirtualItemCount = dt.Rows.Count;
            this.DataSource = dt;
            this.DataBind();
        }

        public void BindData<T>(List<T> list)
        {
            this.VirtualItemCount = list.Count;
            this.DataSource = list;
            this.DataBind();
        }

        /// <summary>
        /// 排序列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SortGrid(Object sender, DataGridSortCommandEventArgs e)
        {

            SortTable(e.SortExpression, (DataTable)null);

            foreach (System.Web.UI.WebControls.DataGridColumn dc in this.Columns)
            {
                if (dc.SortExpression == e.SortExpression)
                {
                    if (dc.HeaderText.IndexOf("<img src=") >= 0)
                    {
                        if (this.DataGridSortType == "ASC")
                        {
                            dc.HeaderText = dc.HeaderText.Replace("<img src=" + this.ImagePath + "asc.gif height=13>", "<img src=" + this.ImagePath + "desc.gif height=13>");
                        }
                        else
                        {
                            dc.HeaderText = dc.HeaderText.Replace("<img src=" + this.ImagePath + "desc.gif height=13>", "<img src=" + this.ImagePath + "asc.gif height=13>");
                        }
                    }
                    else
                    {
                        if (this.DataGridSortType == "ASC")
                        {
                            dc.HeaderText = dc.HeaderText + "<img src=" + this.ImagePath + "desc.gif height=13>";
                        }
                        else
                        {
                            dc.HeaderText = dc.HeaderText + "<img src=" + this.ImagePath + "asc.gif height=13>";
                        }
                    }
                    //dc.. ..ItemStyle="sortColumn";
                    //dc.ItemStyle.BackColor = Color.AliceBlue;
                }
                else
                {
                    dc.HeaderText = dc.HeaderText.Replace("<img", "~").Split('~')[0];
                    //dc.ItemStyle.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// 排序数据表
        /// </summary>
        /// <param name="SortExpression">排序表达式</param>
        /// <param name="dt">待排序的数据表</param>
        public void SortTable(string SortExpression, DataTable dt)
        {
            DataView dv = new DataView();
            if (dt != null && dt.Rows.Count > 0)
            {
                dv = new DataView(dt);
            }
            else
            {
                if (this.SourceDataTable != null)
                {
                    dv = new DataView(SourceDataTable);
                }
                else
                {
                    return;
                }
            }
            dv.Sort = SortExpression.Replace("<img", "~").Split('~')[0] + " " + this.DataGridSortType;
            this.DataSource = dv;
            this.DataBind();
        }

        /// <summary>
        /// 排序数据表
        /// </summary>
        /// <param name="SortExpression">排序表达式</param>
        /// <param name="sqlstring">查询字符串</param>
        public void SortTable(string SortExpression, string sqlstring)
        {
            DataView dv = new DataView();
            if (sqlstring != null && sqlstring != "")
            {
                dv = new DataView(DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0]);
            }
            else
            {
                return;
            }

            dv.Sort = SortExpression.Replace("<img", "~").Split('~')[0] + " " + this.DataGridSortType;
            this.DataSource = dv;
            this.DataBind();
        }

        public void SortTable<T>(string sortExpression,List<T> list)
        {
            this.DataSource = list;
            this.Sort = sortExpression + " " + this.DataGridSortType;
            this.DataBind();
        }

        #region Property DataGridSortType

        /// <summary>
        /// 边框的宽度。
        /// </summary>
        [Description("表头的名称。"), Category("Appearance"), DefaultValue("ASC")]
        public string DataGridSortType
        {
            get
            {

                object obj = ViewState["DataGridSortType"];
                string ascordesc = obj == null ? "ASC" : (string)obj;
                if (ascordesc == "ASC")
                {
                    ViewState["DataGridSortType"] = "DESC";
                    return "DESC";
                }
                else
                {
                    ViewState["DataGridSortType"] = "ASC";
                    return "ASC";
                }

            }
            set
            {
                ViewState["DataGridSortType"] = value;
            }
        }

        #endregion


        private string sort;

        /// <summary>
        /// 排序字段
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Sort
        {
            get
            {
                return sort;
            }
            set
            {
                sort = value;
                SortTable(sort, (DataTable)null);
            }
        }

        /// <summary>
        /// 通过SQL字符串删除表记录并重新绑定
        /// </summary>
        /// <param name="sqlstring"></param>
        public void DeleteByString(string sqlstring)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
            this.EditItemIndex = -1;
            BindData();
        }

        /// <summary>
        /// 编辑指定INDEX的记录
        /// </summary>
        /// <param name="itemindex"></param>
        public void EditByItemIndex(int itemindex)
        {
            this.EditItemIndex = itemindex;
            BindData();
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        public void Cancel()
        {
            this.EditItemIndex = -1;
            BindData();
        }

        private void PageIndex(int pageindex)
        {
            BindData();
        }

        /// <summary>
        /// 跳转指定分页
        /// </summary>
        /// <param name="value"></param>
        public void LoadCurrentPageIndex(int value)
        {
            this.CurrentPageIndex = (value < 0) ? 0 : value;
            BindData();
        }

        /// <summary>
        /// 数据列表项绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "this.className='mouseoverstyle'");
                e.Item.Attributes.Add("onmouseout", "this.className='mouseoutstyle'");
                e.Item.Style["cursor"] = "hand";
                //e.Item.Cells[3].Attributes.Add("onclick", "alert('你点击的ID是: " + e.Item.Cells[0].Text + "!');");
            }

            if (!this.SaveDSViewState)
            {
                this.Controls[0].EnableViewState = false;
            }

            if (this.IsFixConlumnControls)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //int count=0;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        if ((!e.Item.Cells[i].HasControls()))
                        {
                            if (GetBoundColumnFieldReadOnly()[i].ToString().ToLower() == "false") //判断是否存在只读属性
                            {
                                System.Web.UI.WebControls.TextBox t = new System.Web.UI.WebControls.TextBox();
                                t.ID = GetBoundColumnField()[i].ToString();
                                t.Attributes.Add("onmouseover", "if(this.className != 'FormFocus') this.className='FormBase'");
                                t.Attributes.Add("onmouseout", "if(this.className != 'FormFocus') this.className='formnoborder'");
                                t.Attributes.Add("onfocus", "this.className='FormFocus';");
                                t.Attributes.Add("onblur", "this.className='formnoborder';");
                                t.Attributes.Add("class", "formnoborder");
                                t.Text = e.Item.Cells[i].Text.Trim().Replace("&nbsp;", "");

                                //设置宽度
                                if (this.Columns[i].ItemStyle.Width.Value > 0)
                                {
                                    t.Width = (int)this.Columns[i].ItemStyle.Width.Value;
                                }
                                //else
                                //{
                                //    t.Width = 100;
                                //}
                                e.Item.Cells[i].Controls.Add(t);
                            }

                        }
                        else
                        {
                            foreach (System.Web.UI.Control c in e.Item.Cells[i].Controls)
                            {
                                //加载discuz下拉控件
                                if (c is Discuz.Control.DropDownList)
                                {
                                    Discuz.Control.DropDownList dropDownList = (Discuz.Control.DropDownList)c;

                                    try
                                    {
                                        dropDownList.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, dropDownList.DataValueField));
                                    }
                                    catch
                                    { ;}
                                }

                                //加载普通下拉控件
                                if (c is System.Web.UI.WebControls.DropDownList)
                                {
                                    System.Web.UI.WebControls.DropDownList dropDownList = (System.Web.UI.WebControls.DropDownList)c;
                                    try
                                    {
                                        dropDownList.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, dropDownList.DataValueField));
                                    }
                                    catch
                                    { ;}
                                }

                            }
                        }

                        // count++;
                    }
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].BorderWidth = 1;
                        e.Item.Cells[i].BorderStyle = BorderStyle.Solid;
                        e.Item.Cells[i].BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);
                        if (e.Item.Cells[i].HasControls())
                        {
                            for (int j = 0; j < e.Item.Cells[i].Controls.Count; j++)
                            {
                                System.Web.UI.WebControls.TextBox t = e.Item.Cells[i].Controls[j] as System.Web.UI.WebControls.TextBox;
                                if (t != null)
                                {
                                    t.Attributes.Add("onfocus", "this.className='FormFocus';");
                                    t.Attributes.Add("onblur", "this.className='FormBase';");
                                    t.Attributes.Add("class", "FormBase");
                                }
                            }
                        }
                    }
                }
            }
        }

        //protected void RowChanged(object sender, System.EventArgs e)
        //{
        //    DataGridItem dgi = (DataGridItem)(((Control)sender).NamingContainer);
        //    Label bookidlabel = (Label)dgi.Cells[0].Controls[1];
        //    int bookid = int.Parse(bookidlabel.Text);
        //    if (!bookidlist.Contains(bookid))
        //    {
        //        bookidlist.Add(bookid);
        //    }
        //}

        /// <summary>
        /// 装载选项按钮
        /// </summary>
        /// <param name="keyid"></param>
        /// <returns></returns>
        public string LoadSelectedCheckBox(string keyid)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("<INPUT id=\"keyid\"  type=\"checkbox\" value=\"" + keyid + "\"	name=\"keyid\" checked  style=\"display:none\">");
            return sb.ToString();
        }


        /// <summary>
        /// 得到当前页面的keyid字段的数组
        /// </summary>
        /// <returns></returns>
        public System.Collections.ArrayList GetKeyIDArray()
        {
            System.Collections.ArrayList al = new ArrayList();
            if (DNTRequest.GetString("keyid") != "")
            {
                foreach (string id in DNTRequest.GetString("keyid").Split(','))
                {
                    al.Add(id);
                }
            }
            return al;
        }


        /// <summary>
        /// 得到指定行的控件字段的值
        /// </summary>
        /// <param name="controlnumber">控件数</param>
        /// <param name="fieldname">字段名</param>
        /// <returns></returns>
        public string GetControlValue(int controlnumber, string fieldname)
        {
            return DNTRequest.GetFormString(this.ClientID.Replace("_", ":") + ":_ctl" + (controlnumber + 3) + ":" + fieldname);
        }


        /// <summary>
        /// 得到指定行的CheckBox控件字段的值
        /// </summary>
        /// <param name="controlnumber">控件数</param>
        /// <param name="fieldname">字段名</param>
        /// <returns></returns>
        public bool GetCheckBoxValue(int controlnumber, string fieldname)
        {
            string selectcontrolvalue = GetControlValue(controlnumber, fieldname);
            if (selectcontrolvalue == "on")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到相应的字段列表
        /// </summary>
        /// <returns></returns>
        private ArrayList GetBoundColumnField()
        {
            System.Collections.ArrayList __arraylist = new ArrayList();
            foreach (DataGridColumn o in this.Columns)
            {
                System.Web.UI.WebControls.BoundColumn __boundcolumn = o as System.Web.UI.WebControls.BoundColumn;
                if (__boundcolumn != null)
                {
                    __arraylist.Add(__boundcolumn.DataField);
                }
                else
                {
                    System.Web.UI.WebControls.TemplateColumn __templatecolumn = o as System.Web.UI.WebControls.TemplateColumn;
                    __arraylist.Add(__templatecolumn.HeaderText);
                }
            }
            return __arraylist;
        }


        /// <summary>
        /// 得到相应的字段只读属性
        /// </summary>
        /// <returns></returns>
        private ArrayList GetBoundColumnFieldReadOnly()
        {
            System.Collections.ArrayList __arraylist = new ArrayList();
            foreach (DataGridColumn o in this.Columns)
            {
                System.Web.UI.WebControls.BoundColumn __boundcolumn = o as System.Web.UI.WebControls.BoundColumn;
                if (__boundcolumn != null)
                {
                    __arraylist.Add(__boundcolumn.ReadOnly);
                }
                else
                {
                    __arraylist.Add(true);
                }
            }
            return __arraylist;
        }

        /// <summary>
        /// 数据列表项创建事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataGrid_ItemCreated(Object sender, DataGridItemEventArgs e)
        {
            ListItemType elemType = e.Item.ItemType;

            if (elemType == ListItemType.Pager)
            {
                TableCell cell1 = (TableCell)e.Item.Controls[0];
                //cell1.MergeStyle(DataGrid1.HeaderStyle); 
                //cell1.BackColor = Color.Navy; 
                //cell1.BorderWidth=0;
                //cell1.ColumnSpan = this.ColumnSpan; 
                cell1.HorizontalAlign = HorizontalAlign.Left;
                cell1.VerticalAlign = VerticalAlign.Bottom;
                cell1.CssClass = "datagridPager";


                LiteralControl splittable = new LiteralControl("splittable");
                splittable.Text = "</td></tr></table><table class=\"datagridpage\"><tr><td height=\"2\"></td></tr><tr><td>";
                cell1.Controls.AddAt(0, splittable);


                LiteralControl PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = " ";
                if (this.PageCount <= 1)
                {
                    try
                    {
                        cell1.Controls.RemoveAt(1); //当页数为1时, 则不显示页码
                    }
                    catch { ; }
                }
                else
                {
                    PageNumber.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                PageNumber.Text += "<font color=black>共 " + this.PageCount + " 页, 当前第 " + (this.CurrentPageIndex + 1) + " 页";

                if (this.VirtualItemCount > 0)
                {
                    PageNumber.Text += ", 共 " + this.VirtualItemCount + " 条记录";
                }


                PageNumber.Text += "    &nbsp;&nbsp;" + ((this.PageCount > 1) ? "跳转到:" : "");
                cell1.Controls.Add(PageNumber);


                //当大于1时显示跳转按钮
                if (this.PageCount > 1)
                {
                    //加载跳转文件框
                    GoToPagerInputText.ID = "GoToPagerInputText";
                    GoToPagerInputText.Attributes.Add("runat", "server");
                    GoToPagerInputText.Attributes.Add("onkeydown", "if(event.keyCode==13) { var gotoPageID=this.name.replace('InputText','Button'); return(document.getElementById(gotoPageID)).focus();}");
                    GoToPagerInputText.Size = 6;
                    GoToPagerInputText.Value = (this.CurrentPageIndex == 0) ? "1" : (this.CurrentPageIndex + 1).ToString();
                    cell1.Controls.Add(GoToPagerInputText);

                    PageNumber = new LiteralControl("PageNumber");
                    PageNumber.Text = "页&nbsp;&nbsp;";
                    cell1.Controls.Add(PageNumber);

                    //加载跳转按钮	
                    GoToPagerButton.ID = "GoToPagerButton";
                    GoToPagerButton.Text = " Go ";
                    cell1.Controls.Add(GoToPagerButton);
                }


                e.Item.Controls.Add(cell1);

                TableCell pager = (TableCell)e.Item.Controls[0];


                for (int i = 1; i < pager.Controls.Count; i += 2)
                {
                    Object o = pager.Controls[i];

                    if (o is LinkButton)
                    {
                        LinkButton h = (LinkButton)o;

                        if (h.Text == "..." && i == 1)//pager.Controls[i].ID == "_ctl0")
                        {
                            h.Text = "上一页";
                            continue;
                        }
                        if (i > 1 && h.Text == "...")
                        {
                            h.Text = "下一页";
                            continue;
                        }

                        //h.Attributes.Add("href", "javascript:__doPostBack('TabControl1$tabPage1$DataGrid1$_ctl29$_ctl"+(Utils.StrToInt(h.Text)-1)+"','');");
                        //h.Attributes.Add("onclick", "javascript:__doPostBack('TabControl1$tabPage1$DataGrid1$_ctl29$_ctl1','');");
                        //h.Attributes.Add("onclick", "javascript:document.getElementById('Layer5').innerHTML ='<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在加载数据...<BR /></td></tr></table>';document.getElementById('success').style.display ='block';");
                    }
                    if (o is Label)
                    {
                        Label l = (Label)o;
                        if (l.Text == "..." && i == 1)//l.ID == "_ctl0") 
                        {
                            l.Text = "上一页";
                        }

                        if (i > 1 && l.Text == "...")
                        {
                            l.Text = "下一页";
                        }
                    }
                }

            }
            else
            {
                if ((elemType == ListItemType.AlternatingItem) || (elemType == ListItemType.Item) || (elemType == ListItemType.Header))
                {
                    foreach (System.Web.UI.Control control in e.Item.Controls)
                    {
                        TableCell cell = (TableCell)control;
                        cell.BorderWidth = 1;
                        cell.BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);
                        //下面的代码暂时不可用
                        //System.Web.UI.WebControls.Style s=new System.Web.UI.WebControls.Style();
                        //s.CssClass="datagridItemtd";
                        //cell.ApplyStyle(s);
                    }
                }

                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (elemType == ListItemType.Header)
                    {
                        e.Item.Cells[i].Wrap = false;
                    }
                    else
                    {
                        if (i >= 2)
                        {
                            e.Item.Cells[i].Wrap = true;
                        }
                        else
                        {
                            e.Item.Cells[i].Wrap = false;
                        }
                    }
                }
            }

        }


        #region Property SaveDSViewState
        /// <summary>
        /// 是否保存数据的ViewState值
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public bool SaveDSViewState
        {
            get
            {
                object obj = ViewState["SaveDSViewState"];
                if (obj == null) return false;
                else
                {
                    if (obj.ToString().ToLower() == "true")
                        return true;
                    else
                        return false;
                }
            }
            set
            {
                ViewState["SaveDSViewState"] = value;
            }
        }
        #endregion


        public DataTable SourceDataTable
        {
            get
            {
                return (DataTable)ViewState["SourceDataTable"];
            }
            set
            {
                ViewState["SourceDataTable"] = value;
            }
        }

        public string SqlText
        {
            get
            {
                return (string)ViewState["SqlText"];
            }
            set
            {
                ViewState["SqlText"] = value;
            }
        }

        #region Property ColumnSpan
        /// <summary>
        /// ColumnSpan数,用于在分页底部设置表格的colspan属性
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public int ColumnSpan
        {
            get
            {
                object obj = ViewState["ColumnSpan"];
                return obj == null ? 1 : (int)obj;
            }

            set
            {
                ViewState["ColumnSpan"] = value;
            }
        }
        #endregion


        #region Property TableHeaderName

        /// <summary>
        /// 表名称。
        /// </summary>
        [Description("表名称。"), Category("Appearance"), DefaultValue("")]
        public string TableHeaderName
        {
            get
            {

                object obj = ViewState["TableHeaderName"];
                return obj == null ? "1" : (string)obj;
            }
            set
            {
                ViewState["TableHeaderName"] = value;
            }
        }

        #endregion


        #region Property ImagePath
        /// <summary>
        /// 图片所在的文件路径。
        /// </summary>
        [Description("表头的名称。"), Category("Appearance"), DefaultValue("")]
        public string ImagePath
        {
            get
            {

                object obj = ViewState["ImagePath"];
                return obj == null ? "../images/" : (string)obj;
            }
            set
            {
                ViewState["ImagePath"] = value;
            }
        }

        #endregion


        #region Property IsFixConlumnControls
        /// <summary>
        /// 是否安插列控件。
        /// </summary>
        [Description("是否在列表页中载入列控件"), Category("Appearance"), DefaultValue(false)]
        public bool IsFixConlumnControls
        {
            get
            {

                object o = ViewState["IsFixConlumnControls"];
                if ((o != null) && (o.ToString().ToLower() == "true"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsFixConlumnControls"] = value;
            }
        }
        #endregion


        /// <summary> 
        /// 输出html来在浏览器中显示控件
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        protected override void Render(HtmlTextWriter output)
        {

            output.WriteLine("<table class=\"ntcplist\" >\r\n");
            output.WriteLine("<tr class=\"head\">\r\n");
            output.WriteLine("<td>" + this.TableHeaderName + "</td>\r\n");
            output.WriteLine("</tr>\r\n");
            output.WriteLine("<tr>\r\n");
            output.WriteLine("<td>\r\n");

            base.Render(output);

            output.WriteLine("</td></tr></TABLE>");
        }

        /// <summary>
        /// 重写数据源属性
        /// </summary>
        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;

                //当不是手工定制分页时(因为手工定制分页时会在前台指定VirtualItemCount数值)
                if (!this.AllowCustomPaging)
                {
                    if (value is DataTable)
                    {
                        this.VirtualItemCount = (value as DataTable).Rows.Count;
                    }

                    if (value is DataSet)
                    {
                        DataSet ds = value as DataSet;
                        if (ds.Tables.Count > 0)
                        {
                            this.VirtualItemCount = ds.Tables[0].Rows.Count;
                        }
                    }

                    //当为数组类型时
                    if (value.GetType().Name.ToString().IndexOf("[]") > 0)
                    {
                        Array array = value as Array;
                        if (array.Length > 0)
                        {
                            this.VirtualItemCount = array.Length;
                        }
                    }
                }
            }
        }

    }
}
