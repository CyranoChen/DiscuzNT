namespace Discuz.Control
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using Discuz.Common;

    /// <summary>
    /// 属性页控件
    /// </summary>
    [ParseChildren(true, "Items"), Description("Disuz WebControl TabControl"), ToolboxData("<{0}:TabControl runat=server></{0}:TabControl>"), DefaultEvent("SelectedIndexChanged"), DesignTimeVisible(true), PersistChildren(false), Designer(typeof(TabControlDesigner))]
    public class TabControl : WebControl, IPostBackDataHandler, IPostBackEventHandler, INamingContainer
    {
      
        static TabControl()
        {
            TabControl.TabSelectedIndexChangedEvent = new object();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TabControl()
        {
            this.SelectedTab = new HtmlInputHidden();
            this._SelectedIndex = -1;
            this.SelectedTab.Value = string.Empty;
            this._Width = Unit.Pixel(350);
            this._Height = Unit.Pixel(150);
            this._Items = new TabPageCollection(this);
            this._SelectionMode = SelectionModeEnum.Client;

            this.Height = Unit.Pixel(100);
            this.Width = Unit.Pixel(100);
            this._HeightUnitMode = HeightUnitEnum.percent;
            this._WidthUnitMode = WidthUnitEnum.percent;
            this.LeftOffSetX = 0;

        }

        #region 声明事件处理

        private static readonly object TabSelectedIndexChangedEvent;

        /// <summary>
        /// 声明事件处理
        /// </summary>
        public event EventHandler TabSelectedIndexChanged
        {
            add
            {
                base.Events.AddHandler(TabControl.TabSelectedIndexChangedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(TabControl.TabSelectedIndexChangedEvent, value);
            }
        }

        #endregion

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="parsedObj"></param>
        protected override void AddParsedSubObject(object parsedObj)
        {
            if (parsedObj is TabPage)
            {
                this.Items.Add((TabPage)parsedObj);
            }
        }


        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {
            //this.Controls.Clear();
            this.CreateControlCollection();
            this.SelectedTab.ID = this.UniqueID;
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Controls.Add(this.Items[i]);
            }
            base.ChildControlsCreated = true;
            base.CreateChildControls();
        }

        /// <summary>
        /// 重写OnPreRender
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            int i = this.SelectedIndex;
            if (i != -1)
            {
                this.Items[i].Selected = true;
                this.SelectedTab.Value = this.Items[i].UniqueID;
            }
            else
            {
                this.SelectedTab.Value = string.Empty;
            }

            string output = string.Format("<SCRIPT language=\"javascript\" src=\"{0}\"></SCRIPT>\r\n<LINK href=\"{1}\" type=\"text/css\" rel=\"stylesheet\">\r\n",TabScriptPath,TabCssPath);
#if NET1
			if (!Page.IsClientScriptBlockRegistered("TabWindow"))
			{
				Page.RegisterClientScriptBlock("TabWindow", output);
			}
#else
            if (!Page.ClientScript.IsClientScriptBlockRegistered("TabWindow"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "TabWindow", output);
            }
#endif

            base.OnPreRender(args);
        }

        /// <summary>
        /// 属性页选取发生变化的处理事件
        /// </summary>
        /// <param name="e"></param>
        protected void OnTabSelectedIndexChanged(EventArgs e)
        {
            if (base.Events != null)
            {
                EventHandler handler1 = (EventHandler)base.Events[TabSelectedIndexChangedEvent];
                if (handler1 != null)
                {
                    handler1(this, e);
                }
            }
        }

        /// <summary>
        /// 输出html来在浏览器中显示控件
        /// </summary>
        /// <param name="pOutPut"></param>
        protected override void Render(HtmlTextWriter pOutPut)
        {
            if (LeftOffSetX > 0)
            {
                pOutPut.Write("<div Class=\"tabs\" ID=\"" + this.UniqueID + "_Tab\" style=\"padding-left:" + LeftOffSetX + ";\">");
            }
            else
            {
                pOutPut.Write("<div Class=\"tabs\" ID=\"" + this.UniqueID + "_Tab\" >");
            }

            this.SelectedTab.RenderControl(pOutPut);
            pOutPut.Write("<ul>");
            this.RenderTabButton(pOutPut);
            pOutPut.Write("</ul></div><div id=\"" + this.UniqueID + "tabarea\" class=\"tabarea\">");
            this.RenderTabContent(pOutPut);
            pOutPut.Write("</div>");
        }

        internal void RenderDownLevelContent(HtmlTextWriter output)
        {
            this.Render(output);
        }
 
        private void RenderTabButton(HtmlTextWriter pOutPut)
        {
            if (this.SelectionMode == SelectionModeEnum.Server)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        pOutPut.Write("<li class=\"CurrentTabSelect\" ><a href=\"#\" class=\"current\" onfocus=\"this.blur();\">" + this.Items[i].Caption + "</a></li>");
                    }
                    else
                    {

#if NET1
						pOutPut.Write("<li class=\"TabSelect\" onmouseover=\"tabpage_mouseover(this)\" onmouseout=\"tabpage_mouseout(this)\" onClick=\"tabpage_selectonserver(this,'" + this.Items[i].UniqueID + "');" + this.Page.GetPostBackEventReference(this, "") + "\"><a href=\"#\" onfocus=\"this.blur();\">" + this.Items[i].Caption + "</a></li>");
#else
                        pOutPut.Write("<li class=\"TabSelect\" onmouseover=\"tabpage_mouseover(this)\" onmouseout=\"tabpage_mouseout(this)\" onClick=\"tabpage_selectonserver(this,'" + this.Items[i].UniqueID + "');" + this.Page.ClientScript.GetPostBackEventReference(this, "") + "\"><a href=\"#\" onfocus=\"this.blur();\">" + this.Items[i].Caption + "</a></li>");
#endif
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        pOutPut.Write("<li id=\"" + this.Items[i].UniqueID + "_li\" class=\"CurrentTabSelect\" onclick=\"tabpage_selectonclient(this,'" + this.Items[i].UniqueID + "');\"><a href=\"#\" class=\"current\" onfocus=\"this.blur();\">" + this.Items[i].Caption + "</a></li>");
                    }
                    else
                    {
                        pOutPut.Write("<li id=\"" + this.Items[i].UniqueID + "_li\" class=\"TabSelect\" onmouseover=\"tabpage_mouseover(this)\" onMouseOut=\"tabpage_mouseout(this)\" onclick=\"tabpage_selectonclient(this,'" + this.Items[i].UniqueID + "');\"><a href=\"#\" onfocus=\"this.blur();\">" + this.Items[i].Caption + "</a></li>");
                    }
                }
            }
        }

        private void RenderTabContent(HtmlTextWriter pOutPut)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].RenderControl(pOutPut);
            }
        }

        /// <summary>
        /// 加载提交数据
        /// </summary>
        /// <param name="ControlDataKey"></param>
        /// <param name="PostBackDataCollection"></param>
        /// <returns></returns>
        bool IPostBackDataHandler.LoadPostData(string ControlDataKey, NameValueCollection PostBackDataCollection)
        {
                string postbackdata = PostBackDataCollection[ControlDataKey];
                if ((postbackdata != null) && (postbackdata != this.SelectedTabPageID))
                {
                    this.SelectedTabPageID = postbackdata;
                    return true;
                }
                return false;
         }

        /// <summary>
        /// 引发提交数据变化事件
        /// </summary>
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            this.OnTabSelectedIndexChanged(EventArgs.Empty);
        }

        /// <summary>
        /// 引发提交数据事件
        /// </summary>
        /// <param name="pEventArgument"></param>
        void IPostBackEventHandler.RaisePostBackEvent(string pEventArgument)
        {}


        /// <summary>
        /// 返回属性页集合对象
        /// </summary>
        [MergableProperty(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public TabPageCollection Items
        {
            get
            {
                return this._Items;
            }
        }

        /// <summary>
        /// 获取或设置当前选中的属性页索引
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                if (this.Items.Count <= 0)
                {
                    return (this._SelectedIndex = -1);
                }
                if (this._SelectedIndex == -1)
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        if (this.Items[i].Visible && (this.Items[i].UniqueID == this.SelectedTabPageID))
                        {
                            return (this._SelectedIndex = i);
                        }
                    }
                    return (this._SelectedIndex = 0);
                }
                if (this._SelectedIndex >= this.Items.Count)
                {
                    return (this._SelectedIndex = 0);
                }
                return this._SelectedIndex;
            }
            set
            {
                if ((value < -1) || (value >= this.Items.Count))
                {
                    throw new ArgumentOutOfRangeException("选项页必须小于" + this.Items.Count.ToString());
                }
                this._SelectedIndex = value;
            }
        }

        protected string SelectedTabPageID
        {
            get
            {
                if (this.ViewState["SelectedTabPageID"] != null)
                {
                    return (string)this.ViewState["SelectedTabPageID"];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["SelectedTabPageID"] = value;
            }
        }

           
        /// <summary>
        /// avascript脚本文件所在目录
        /// </summary>
        [Description("Javascript脚本文件所在目录。"), DefaultValue("./")]
        public string TabScriptPath
        {
            get
            {
                object obj = ViewState["TabScriptPath"];
                return obj == null ? "../js/tabstrip.js" : (string)obj;
            }
            set
            {
                ViewState["TabScriptPath"] = value;
            }
        }

        /// <summary>
        /// css文件所在目录
        /// </summary>
        [Description("css文件所在目录。"), DefaultValue("./")]
        public string TabCssPath
        {
            get
            {
                object obj = ViewState["TabCssPath"];
                return obj == null ? "../styles/tab.css" : (string)obj;
            }
            set
            {
                ViewState["TabCssPath"] = value;
            }
        }


        /// <summary>
        /// 顶部属性页标题距左边偏移量
        /// </summary>
        [Description("顶部属性页标题距左边偏移量"), DefaultValue(0)]
        public int LeftOffSetX
        {
            get
            {
                object obj = ViewState["LeftOffSetX"];
                return obj == null ? 0 : Utils.StrToInt(obj.ToString(),0);
            }
            set
            {
                ViewState["LeftOffSetX"] = value;
            }
        }


        /// <summary>
        /// 初始化创建tabpage页
        /// 此方法主要解决datagrid 控件 的DataGrid_ItemCreated方法创建分页时其parent.parent (tabcontrol)为空从而导致无法分页(在.net1下这个方法可以不调用)
        /// 请在page中的InitializeComponent使用
        /// </summary>
        public void InitTabPage()
        {
            CreateChildControls();
        }


        #region 其它设置选项

        /// <summary>
        /// 选取模式
        /// </summary>
        public SelectionModeEnum SelectionMode
        {
            get
            {
                return this._SelectionMode;
            }
            set
            {
                this._SelectionMode = value;
            }
        }

        /// <summary>
        /// 高度单位(元素,百分比等)
        /// </summary>
        public HeightUnitEnum HeightUnitMode
        {
            get
            {
                return this._HeightUnitMode;
            }
            set
            {
                this._HeightUnitMode = value;
            }
        }

        /// <summary>
        /// 宽度单位(元素,百分比等)
        /// </summary>
        public WidthUnitEnum WidthUnitMode
        {
            get
            {
                return this._WidthUnitMode;
            }
            set
            {
                this._WidthUnitMode = value;
            }
        }


        private HeightUnitEnum _HeightUnitMode;
        private WidthUnitEnum _WidthUnitMode;
        private Unit _Height;
        private TabPageCollection _Items;
        private int _SelectedIndex;
        private SelectionModeEnum _SelectionMode;
        private Unit _Width;
        private HtmlInputHidden SelectedTab;

        /// <summary>
        /// 高度单位(枚举)
        /// </summary>
        public enum HeightUnitEnum
        {
            percent,
            px
        }

        /// <summary>
        /// 宽度单位(枚举)
        /// </summary>
        public enum WidthUnitEnum
        {
            percent,
            px
        }

        /// <summary>
        /// 所选模式(枚举)
        /// </summary>
        public enum SelectionModeEnum
        {
            Client,
            Server
        }

        #endregion

    }
}

