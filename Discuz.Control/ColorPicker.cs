using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

namespace Discuz.Control
{
    /// <summary>
    /// ColorPicker 颜色拾取器控件。
    /// </summary>
    [DefaultProperty("ScriptPath"), ToolboxData("<{0}:ColorPicker runat=server></{0}:ColorPicker>")]
    public class ColorPicker : Discuz.Control.WebControl, IPostBackDataHandler,
        INamingContainer //为子控件提供了一个新的命名范围,保证子控件的ID唯一性
    {

        /// <summary>
        /// 拾取文本框
        /// </summary>
        protected Discuz.Control.TextBox ColorTextBox = new Discuz.Control.TextBox();

        /// <summary>
        /// 显示图标
        /// </summary>
        protected System.Web.UI.HtmlControls.HtmlImage ImgHtmlImage = new System.Web.UI.HtmlControls.HtmlImage();

        /// <summary>
        /// 构造函数
        /// </summary>
        public ColorPicker() : base()
        {
        }

        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {

            ColorTextBox.Size = 8;
            ColorTextBox.ID = this.ID;
            this.Controls.Add(ColorTextBox);

            ImgHtmlImage.ID = "ColorPreview";
            ImgHtmlImage.Src = ImageUrl;
            ImgHtmlImage.Attributes.Add("onclick", "IsShowColorPanel('" + this.ColorTextBox.ClientID + "','" + this.ImgHtmlImage.ClientID + "'," + this.LeftOffSet + "," + this.TopOffSet + ")");
            ImgHtmlImage.Attributes.Add("class", "img");
            ImgHtmlImage.Attributes.Add("title", "选择颜色");
            this.Controls.Add(ImgHtmlImage);

            base.CreateChildControls();
        }


        /// <summary>
        /// 添加属性方法
        /// </summary>
        /// <param name="key">键值,如class等</param>
        /// <param name="valuestr">要绑定的字符串</param>
        public void AddAttributes(string key, string valuestr)
        {
            ColorTextBox.Attributes.Add(key, valuestr);
        }

      
		#region protected override void OnPreRender(EventArgs e)
		/// <summary>
		/// 重写<see cref="System.Web.UI.Control.OnPreRender"/>方法。
		/// </summary>
		/// <param name="e">包含事件数据的 <see cref="EventArgs"/> 对象。</param>
		protected override void OnPreRender(EventArgs e)
		{
			string scriptStr = string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\">\r\n<script language=\"javascript\" src=\"{1}\"></script>\r\n",
				this.Css_Path,
                this.ScriptPath);


			
#if NET1
			if (!Page.IsClientScriptBlockRegistered("ColorPickerSet"))
		{
			Page.RegisterClientScriptBlock("ColorPickerSet", scriptStr);
		}
#else
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ColorPickerSet"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ColorPickerSet", scriptStr);
            }
           
#endif

			base.OnPreRender(e);
		}


      
		#endregion

        #region Property ImageUrl

        /// <summary>
        /// 图片路径
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
                    return "../images/colorpicker.gif";
                }
            }
            set
            {
                base.ViewState["imageurl"] = value;
                ImgHtmlImage.Src = value;
            }
        }

        #endregion

        #region Property Text

        /// <summary>
        /// 当前选择的颜色值
        /// </summary>
        [Description("当前选择的颜色值"), DefaultValue("")]
        public string Text
        {
            get
            {
                return ColorTextBox.Text;
            }
            set
            {
                ColorTextBox.Text = value.Trim();
            }
        }

        #endregion


        #region Property ReadOnly

        /// <summary>
        /// 是否是只读。
        /// </summary>
        [Description("是否是只读"), DefaultValue(true)]
        public bool ReadOnly
        {
            get
            {
                if (Environment.Version.Major == 1)
                {
                    return ColorTextBox.ReadOnly;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (Environment.Version.Major == 1)
                {
                    ColorTextBox.ReadOnly = value;
                }
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
                return obj == null ? "../js/colorpicker.js" : (string)obj;
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


        #region Property CSSPATH

        /// <summary>
        /// CSS文件路径。
        /// </summary>
        [Description("CSS文件路径。"), Category("Appearance"), DefaultValue("")]
        public string Css_Path
        {
            get
            {
                object obj = ViewState["ColorPickerCssPath"];
                return obj == null ? "../styles/colorpicker.css" : (string)obj;
            }
            set
            {
                ViewState["ColorPickerCssPath"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 向上偏移量
        /// </summary>
        [Description("向上偏移量"), DefaultValue(0)]
        public float TopOffSet
        {
            get
            {
                object obj = ViewState["TopOffSet"];
                return obj == null ? 0 : (float)obj;
            }
            set
            {
                ViewState["TopOffSet"] = value;
            }
        }

        /// <summary>
        /// 向左偏移量
        /// </summary>
        [Description("向左偏移量"), DefaultValue(0)]
        public float LeftOffSet
        {
            get
            {
                object obj = ViewState["LeftOffSet"];
                return obj == null ? 0 : (float)obj;
            }
            set
            {
                ViewState["LeftOffSet"] = value;
            }
        }
   

        #region IPostBackDataHandler 成员

        /// <summary>
        /// 引发PostBack事件
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
            string presentValue = this.ColorTextBox.Text;
            string postedValue = postCollection[postDataKey];

            if (!presentValue.Equals(postedValue))//如果回发数据不等于原有数据
            {
                this.ColorTextBox.Text = postedValue;
                return true;
            }
            return false;

        }
        #endregion

        /// <summary>
        /// 获取颜色面板的HTML
        /// </summary>
        /// <returns></returns>
        public string ColorPickHtmlContent()
        {

			StringBuilder sb = new StringBuilder();
			sb.Append("<span id=\"ColorPicker{0}\" style=\"display:none; position:absolute;z-index:500;\" onmouseout=\"HideColorPanel('{0}');\"  onmouseover=\"ShowColorPanel('{0}','{1}',{2},{3});\">");
			sb.Append("<div  style=\"display:block;cursor:crosshair;z-index:501\" class=\"article\" >");
			sb.Append("<table border=0 cellPadding=0 cellSpacing=10 onmouseover=\"ShowColorPanel('{0}','{1}',{2},{3});\">");
			sb.Append("<tbody>");
			sb.Append("<tr>");

			//输出颜色面板
			sb.Append("<script language=\"javaScript\">");
			sb.Append("WriteColorPanel('{0}','{1}',{2},{3});");
			sb.Append("</script>");

			sb.Append("</tr></tbody></table>");
			sb.Append("<table style=\"font-size:12px;word-break:break-all;width:100%;border:0px\"  cellPadding=0 cellSpacing=10 onmouseover=\"ShowColorPanel('{0}','{1}',{2},{3});\">");
			sb.Append("<tbody>");
			sb.Append("<tr>");
			sb.Append("<td align=middle rowSpan=2>选中色彩");
			sb.Append("<table border=1 cellPadding=0 cellSpacing=0 height=30 id=ShowColor{0} width=40 bgcolor=\"\">");
			sb.Append("<tbody>");
			sb.Append("<tr>");
			sb.Append("<td></td></tr></tbody></table></td>");
			sb.Append("<td rowSpan=2>基色: <SPAN id=RGB{0}></SPAN><br />亮度: <SPAN id=GRAY{0}>120</SPAN><br />代码: <INPUT id=SelColor{0} size=7 value=\"\" border=0></TD>");
			sb.Append("<td><input type=\"button\" onclick=\"javascript:ColorPickerOK('{0}','{1}');\" value=\"确定\"></TD></TR>");
			sb.Append("<tr>");
			sb.Append("<td><input type=\"button\" onclick=\"javascript:document.getElementById('{0}').value='';document.getElementById('{1}').style.background='#FFFFFF';HideColorPanel('{0}');\" value=\"取消\"></TD>");
			sb.Append("</tr></tbody></table>");
			sb.Append("</DIV>");
			sb.Append("<iframe id=\"pickcoloriframe{0}\" style=\"position:absolute;z-index:102;top:-1px;width:250px;scrolling:no;height:237px;\" frameborder=\"0\"></iframe>");
			sb.Append("</span>");
            
			//初始化颜色选择器的背景色
			sb.Append("<script language=javascript>\r\n");
			sb.Append("InitColorPicker('{1}','" + this.Text + "');\r\n");
			sb.Append("</script>\r\n");
            
			return string.Format(sb.ToString(),
                this.ColorTextBox.ClientID,
                this.ImgHtmlImage.ClientID,
				this.LeftOffSet,
				this.TopOffSet);
        }

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

            output.Write(this.ColorPickHtmlContent());
         
        }
    }

}
