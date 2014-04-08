namespace Discuz.Control
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// 属性页类
    /// </summary>
    [ToolboxItem(false), PersistChildren(true), ParseChildren(false)]
    public class TabPage : WebControl, INamingContainer
    {
        private string _ClientID;

        //构造函数
        public TabPage()
        {
            this._Selected = false;
            this._Caption = string.Empty;
            this._ClientID = string.Empty;
            this._ActionLink = string.Empty;
        }

        /// <summary>
        /// 得到属性控件
        /// </summary>
        /// <returns></returns>
        public object GetTabControl()
        {
            return this._tabControl;
        }

        /// <summary>
        /// 输出html来在浏览器中显示控件
        /// </summary>
        /// <param name="pOutPut"></param>
        protected override void Render(HtmlTextWriter pOutPut)
        {
            if ((this._tabControl == null) || (this._tabControl.GetType().ToString() != "Discuz.Control.TabControl"))
            {
                throw new ArgumentException("Disuz.TabPage 必须是 Disuz.TabControl 的子控件");
            }
            if (this.Selected)
            {
                pOutPut.Write(string.Concat(new object[] { "<div id=\"", this.UniqueID, "\" class=\"tab-page\" style=\"display: block;background: #fff;\">" }));
            }
            else
            {
                pOutPut.Write(string.Concat(new object[] { "<div id=\"", this.UniqueID, "\" class=\"tab-page\" style=\"display: none;background: #fff;\">" }));
            }
            
            this.RenderChildren(pOutPut);
            pOutPut.Write("</div>");
        }


        internal void RenderDownLevelContent(HtmlTextWriter pOutPut)
        {
            this.Render(pOutPut);
        }


        private TabControl _tabControl;

        /// <summary>
        /// 设置属性页
        /// </summary>
        /// <param name="pTabControl"></param>
        internal void SetTabControl(TabControl pTabControl)
        {
            this._tabControl = pTabControl;
        }

        private string _ActionLink;

        /// <summary>
        /// 活动链接
        /// </summary>
        [NotifyParentProperty(true), Browsable(true), Description("")]
        private string ActionLink
        {
            get
            {
                return this._ActionLink;
            }
            set
            {
                this._ActionLink = value;
            }
        }

        private string _Caption;
        /// <summary>
        /// 标题
        /// </summary>
        [NotifyParentProperty(true), Description(""), Browsable(true)]
        public string Caption
        {
            get
            {
                return this._Caption;
            }
            set
            {
                this._Caption = value;
            }
        }

        private bool _Selected;
        /// <summary>
        /// 是否选取
        /// </summary>
        internal bool Selected
        {
            get
            {
                return this._Selected;
            }
            set
            {
                this._Selected = value;
            }
        }
    }
}

