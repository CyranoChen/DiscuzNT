using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace Discuz.Control
{
    /// <summary>
    /// 控钮控件。
    /// </summary>
    [DefaultEvent("Click"), DefaultProperty("Text"), ToolboxData("<{0}:Button runat=server></{0}:Button>")]
    public class Button : Discuz.Control.WebControl, IPostBackEventHandler
    {
        /// <summary>
        /// 单击事件绑定的对象
        /// </summary>
        protected static readonly object EventClick = new object();
        private string onClientClick = "";

        public string OnClientClick
        {
            get
            {
                return onClientClick;
            }
            set
            {
                onClientClick = value.EndsWith(";") ? value : value + ";";
            }
        }


        static Button()
        {
            EventClick = new object();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Button()
        {
            this.ButtontypeMode = ButtonType.WithImage;
        }

        /// <summary>
        /// 定义事件处理器
        /// </summary>
        public event EventHandler Click
        {
            add
            {
                Events.AddHandler(EventClick, value);
            }
            remove
            {
                Events.RemoveHandler(EventClick, value);
            }
        }

      
        /// <summary>
        /// 跨页面提交链接
        /// </summary>
        public virtual string PostBackUrl
        {
            get
            {
                string text1 = (string)this.ViewState["PostBackUrl"];
                if (text1 != null)
                {
                    return text1;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["PostBackUrl"] = value;
            }
        }




        /// <summary>
        /// 定义Onclick
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClick(EventArgs e)
        {
            EventHandler clickHandler = (EventHandler)Events[EventClick];
            if (clickHandler != null)
            {
                clickHandler(this, e);
            }
        }

        /// <summary>
        /// 引发PostBack事件
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            OnClick(new EventArgs());
        }

        /// <summary>
        /// 引发PostBack事件(实现IPostBackEventHandler接口)
        /// </summary>
        /// <param name="eventArgument"></param>
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }

        /// <summary>
        /// 重写OnPreRender
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        /// <summary>
        /// 加载ViewState
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                base.LoadViewState(savedState);
                string text1 = (string)this.ViewState["Text"];
                if (text1 != null)
                {
                    this.Text = text1;
                }
            }
        }

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="obj"></param>
        protected override void AddParsedSubObject(object obj)
        {
            if (this.HasControls())
            {
                base.AddParsedSubObject(obj);
            }
            else if (obj is LiteralControl)
            {
                this.Text = ((LiteralControl)obj).Text;
            }
            else
            {
                string text1 = this.Text;
                if (text1.Length != 0)
                {
                    this.Text = string.Empty;
                    base.AddParsedSubObject(new LiteralControl(text1));
                }
                base.AddParsedSubObject(obj);
            }
        }




        /// <summary>
        /// 输出html,在浏览器中显示控件
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            if (this.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }

            string buttonEnabled = "";
            if (!this.Enabled)  //设置Button是否可以使用
                buttonEnabled = " disabled=\"true\"";

            if (this.AutoPostBack)
            {
                StringBuilder sb = new System.Text.StringBuilder();
                if(!string.IsNullOrEmpty(this.OnClientClick))
                {
                    sb.Append(this.OnClientClick);
                }
                sb.Append("if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { return false; }}");    //保证验证函数的执行
                //sb.Append("javascript:{if (typeof(Page_ClientValidate)!='function'|| Page_ClientValidate())__doPostBack('"+this.UniqueID+"','')}"); 
                //sb.Append("if(window.confirm('are you sure?')==false) return false;");        //自定义客户端脚本
                sb.Append("this.disabled=true;");
                if (ValidateForm)
                {
                    sb.Append("if(validate(this.form)){");
                    if (ShowPostDiv)
                    {
                        sb.Append("document.getElementById('success').style.display = 'block';HideOverSels('success');");
                    }
                    sb.Append(Page.ClientScript.GetPostBackEventReference(this, "") + ";}else{this.disabled=false;}");    //用__doPostBack来提交，保证按钮的服务器端click事件执行
                }        // disable所有自身
                else
                {
                    sb.Append(Page.ClientScript.GetPostBackEventReference(this, "") + ";");    //用__doPostBack来提交，保证按钮的服务器端click事件执行
                }

                if (ScriptContent != "")
                {
                    sb.Append(ScriptContent);
                }

                if (this.ButtontypeMode == ButtonType.Normal)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + buttonEnabled + " onclick=\"" + sb.ToString() + "\">" + this.Text + "</button></span>");
                }

                if (this.ButtontypeMode == ButtonType.WithImage)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + buttonEnabled + " onclick=\"" + sb.ToString() + "\"><img src=\"" + this.ButtonImgUrl + "\"/>" + this.Text + "</button></span>");
                }

            }
            else
            {
                if (this.ButtontypeMode == ButtonType.Normal)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + buttonEnabled + " onclick=\"" + this.OnClientClick + ScriptContent + "\">" + this.Text + "</button></span>");
                }

                if (this.ButtontypeMode == ButtonType.WithImage)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + buttonEnabled + " onclick=\"" + this.OnClientClick + ScriptContent + "\"><img src=\"" + this.ButtonImgUrl + "\"/>" + this.Text + "</button></span>");
                }
            }


            if (this.HintInfo != "")
            {
                output.WriteEndTag("span");
            }
        }

   
        private bool _autoPostBack = true;

        /// <summary>
        /// AutoPostBack属性
        /// </summary>
        public bool AutoPostBack
        {
            set
            {
                this._autoPostBack = value;
            }
            get
            {
                return this._autoPostBack;
            }
        }

        private bool _showPostDiv = true;

        /// <summary>
        /// 是否显示"正在提交"信息
        /// </summary>
        public bool ShowPostDiv
        {
            set
            {
                this._showPostDiv = value;
            }
            get
            {
                return this._showPostDiv;
            }
        }

        #region 定义是否调用js函数validate(this.form);进行数据校验

        private bool _validateForm = false;

        /// <summary>
        /// 定义是否调用js函数validate(this.form);进行数据校验
        /// </summary>
        public bool ValidateForm
        {
            set
            {
                this._validateForm = value;
            }
            get
            {
                return this._validateForm;
            }
        }
        #endregion





        #region properytyButtontypeMode 按钮样式
        
        /// <summary>
        /// 按钮枚举样式(Normal:普通, WithImage:带图)
        /// </summary>
        public enum ButtonType
        {
            Normal,   //普通
            WithImage,  //带图
        }

        /// <summary>
        /// 按钮样式
        /// </summary>
        public ButtonType ButtontypeMode
        {
            get
            {
                object obj = ViewState["ButtontypeMode"];
                return obj == null ? ButtonType.WithImage : (ButtonType)obj;
            }
            set
            {
                ViewState["ButtontypeMode"] = value;
            }
        }
        #endregion


        #region Property Text 按钮文字

        /// <summary>
        /// 按钮文字
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(" 提 交 ")]
        public string Text
        {
            get
            {
                object obj = ViewState["ButtonText"];
                return obj == null ? " 提 交 " : (string)obj;
            }
            set
            {
                ViewState["ButtonText"] = value;
            }
        }
        #endregion


        #region Property ButtonImgUrl 图版按钮链接

        /// <summary>
        /// 图片按钮链接
        /// </summary>
        [Description("图版按钮链接"), DefaultValue("../images/submit.gif")]
        public string ButtonImgUrl
        {
            get
            {
                object obj = ViewState["ButtonImgUrl"];
                return obj == null ? "../images/submit.gif" : (string)obj;
            }
            set
            {
                ViewState["ButtonImgUrl"] = value;
            }
        }

        #endregion


        #region Property XpBGImgFilePath XP背景图片路径

        /// <summary>
        /// XP背景图片路径
        /// </summary>
        [Description("图版按钮链接"), DefaultValue("../images/")]
        public string XpBGImgFilePath
        {
            get
            {
                object obj = ViewState["XpBGImgFilePath"];
                return obj == null ? "../images/" : (string)obj;
            }
            set
            {
                ViewState["XpBGImgFilePath"] = value;
            }
        }

        #endregion


        #region Property ScriptContent 要加载的客户端脚本内容

        /// <summary>
        /// 要加载的客户端脚本内容
        /// </summary>
        [Description("图版按钮链接"), DefaultValue("../images/")]
        public string ScriptContent
        {
            get
            {
                object obj = ViewState["ScriptContent"];
                return obj == null ? "" : (string)obj;
            }
            set
            {
                ViewState["ScriptContent"] = value;
            }
        }

        #endregion
    }
}