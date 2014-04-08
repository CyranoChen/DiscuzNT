using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

namespace Discuz.Control
{
    /// <summary>
    /// 日历控件。
    /// </summary>
    [DefaultProperty("ScriptPath"), ToolboxData("<{0}:Calendar runat=server></{0}:Calendar>")]
    public class Calendar : Discuz.Control.WebControl, IPostBackDataHandler, 
        INamingContainer //为子控件提供了一个新的命名范围,保证子控件的ID唯一性
    {
        protected Discuz.Control.TextBox DateTextBox = new Discuz.Control.TextBox();
        protected System.Web.UI.HtmlControls.HtmlImage ImgHtmlImage = new System.Web.UI.HtmlControls.HtmlImage();
        private bool readOnly = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Calendar(): base()
        {
        }

        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {
            if(ReadOnly)    //设置日期控件只读
            {
                DateTextBox.Attributes.Add("readonly", "readonly");
            }
            DateTextBox.Size = 8;
            DateTextBox.ID = this.ID;
            this.Controls.Add(DateTextBox);

            ImgHtmlImage.Src = ImageUrl;
            ImgHtmlImage.Align = "bottom";
            ImgHtmlImage.Attributes.Add("onclick", "showcalendar(event, $('" + this.ID + "_" + this.ID + "'))");
            ImgHtmlImage.Attributes.Add("class", "calendarimg");
            this.Controls.Add(ImgHtmlImage);

            System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1 = new RegularExpressionValidator();
            RegularExpressionValidator1.ID = RegularExpressionValidator1.ClientID;
            RegularExpressionValidator1.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
            RegularExpressionValidator1.ControlToValidate = DateTextBox.ID;
            RegularExpressionValidator1.ValidationExpression = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
            RegularExpressionValidator1.ErrorMessage = "请输入正确的日期,如:2006-1-1";
            this.Controls.Add(RegularExpressionValidator1);

            base.CreateChildControls();
        }

        /// <summary>
        /// 添加属性方法
        /// </summary>
        /// <param name="key">键值,如class等</param>
        /// <param name="valuestr">要绑定的字符串</param>
        public void AddAttributes(string key, string valuestr)
        {
            DateTextBox.Attributes.Add(key, valuestr);
        }



        #region protected override void OnPreRender(EventArgs e)
        /// <summary>
        /// 重写<see cref="System.Web.UI.Control.OnPreRender"/>方法。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="EventArgs"/> 对象。</param>
        protected override void OnPreRender(EventArgs e)
        {
#if NET1
            if (!Page.IsClientScriptBlockRegistered("CalendarSet"))
			{
                Page.RegisterClientScriptBlock("CalendarSet", string.Format("<SCRIPT language='javascript' src='{0}'></SCRIPT>", ScriptPath));
			}
#else
            if (!Page.ClientScript.IsClientScriptBlockRegistered("CalendarSet"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CalendarSet", string.Format("<SCRIPT language='javascript' src='{0}'></SCRIPT>", ScriptPath));
            }
#endif

            base.OnPreRender(e);
        }

        #endregion


        #region Property ImageUrl

        /// <summary>
        /// 图片的路径
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string ImageUrl
        {
            get
            {
                if (base.ViewState["imageurl"] != null)
                {
                    return (String)base.ViewState["imageurl"];
                }
                else
                {
                    return "../images/btn_calendar.gif";
                }
            }
            set
            {
                base.ViewState["imageurl"] = value;
                ImgHtmlImage.Src = value;
            }
        }

        #endregion


        #region Property Date

        /// <summary>
        /// 当前选择的日期
        /// </summary>
        [Description("当前选择的日期。"), DefaultValue("")]
        public DateTime SelectedDate
        {
            get
            {
                try
                {
                    return DateTime.Parse(DateTextBox.Text);
                }
                catch
                {
                    return Convert.ToDateTime("1900-1-1");
                }
            }
            set
            {
                try
                {
                    DateTextBox.Text = value.ToString("yyyy-MM-dd");
                }
                catch
                {
                    DateTextBox.Text = "";
                }
            }
        }

        #endregion


        #region Property ReadOnly

        /// <summary>
        /// 是否是只读。
        /// </summary>
        [Description("是否是只读。"), DefaultValue(true)]
        public bool ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
            }
        }

        #endregion


        #region Property ScriptPath

        /// <summary>
        /// Javascript脚本文件所在目录。
        /// </summary>
        [Description("Javascript脚本文件所在目录。"), DefaultValue("./")]
        public string ScriptPath
        {
            get
            {
                object obj = ViewState["ScriptPath"];
                return obj == null ? "../js/calendar.js" : (string)obj;
            }
            set
            {
                ViewState["ScriptPath"] = value;
            }
        }

        #endregion


        #region Property BorderStyle

        /// <summary>
        /// 边框的样式。
        /// </summary>
        [Description("边框的样式。"), Category("Appearance"), DefaultValue(""), TypeConverter(typeof(Discuz.Control.BorderStyleConverter))]
        new public string BorderStyle
        {
            get
            {
                object obj = ViewState["BorderStyle"];
                return obj == null ? "solid" : (string)obj;
            }
            set
            {
                ViewState["BorderStyle"] = value;
            }
        }

        #endregion


        #region Property BorderWidth

        /// <summary>
        /// 边框的宽度。
        /// </summary>
        [Description("边框的宽度。"), Category("Appearance"), DefaultValue("")]
        new public string BorderWidth
        {
            get
            {

                object obj = ViewState["BorderWidth"];
                return obj == null ? "1" : (string)obj;
            }
            set
            {
                ViewState["BorderWidth"] = value;
            }
        }

        #endregion


        #region Property BorderColor

        /// <summary>
        /// 边框的颜色。
        /// </summary>
        [Description("边框的颜色。"), Category("Appearance"), DefaultValue("")]
        public override Color BorderColor
        {
            get
            {
                object obj = ViewState["BorderColor"];
                return obj == null ? Color.FromArgb(0x99, 0x99, 0x99) : (Color)obj;
            }
            set
            {
                ViewState["BorderColor"] = value;
            }
        }
        #endregion

        #region IPostBackDataHandler 成员

        /// <summary>
        /// 引发回传事件
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
        }

        /// <summary>
        /// 加载提交信息
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            string presentValue = this.DateTextBox.Text;
            string postedValue = postCollection[postDataKey];

            if (!presentValue.Equals(postedValue))//如果回发数据不等于原有数据
            {
                this.DateTextBox.Text = postedValue;
                return true;
            }
            return false;

        }
        #endregion


        /// <summary> 
        /// 输出html,在浏览器中显示控件
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        protected override void Render(HtmlTextWriter output)
        {

            if (this.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }

            RenderChildren(output);

            if (this.HintInfo != "")
            {
                output.WriteEndTag("span");
            }
        }

    }

    /// <summary>
    /// 边框风格转换器
    /// </summary>
    public class BorderStyleConverter : StringConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BorderStyleConverter()
        {
        }

        /// <summary>
        /// 获取标准值列表支持
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// 获取标准值列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "none", "dotted", "dashed", "solid", "double", "groove", "ridge", "inset", "window-inset", "outset" });
        }
    }
}
