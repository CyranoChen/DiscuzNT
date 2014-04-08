using System.Data;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Control
{
	/// <summary>
    /// 树形列表框控件。
	/// </summary>
	[DefaultProperty("Text"),ToolboxData("<{0}:ListBoxTreeList runat=server></{0}:ListBoxTreeList>")]
    public class ListBoxTreeList : Discuz.Control.WebControl
	{
        /// <summary>
        /// 列表框控件变量
        /// </summary>
		public System.Web.UI.WebControls.ListBox TypeID=new System.Web.UI.WebControls.ListBox();
	
        public void BuildTree(DataTable dt, string textName, string valueName)
        {

            string SelectedType = "0";

            TypeID.SelectedValue = SelectedType;

            this.Controls.Add(TypeID);

            TypeID.Items.Clear();
            //加载树
            TypeID.Items.Add(new ListItem("请选择     ", "0"));
            DataRow[] drs = dt.Select(this.ParentID + "=0");

            foreach (DataRow r in drs)
            {
                TypeID.Items.Add(new ListItem(r[textName].ToString(), r[valueName].ToString()));
                string blank = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                BindNode(r[0].ToString(), dt, textName, valueName, blank);
            }
            TypeID.DataBind();
        }

        /// <summary>
        /// 创建树结点
        /// </summary>
        /// <param name="sonparentid">当前数据项</param>
        /// <param name="dt">数据表</param>
        /// <param name="blank">空白符</param>
        private void BindNode(string sonparentid, DataTable dt, string textName, string valueName, string blank)
		{
			DataRow [] drs = dt.Select(this.ParentID+"=" + sonparentid );
			
			foreach( DataRow r in drs )
			{
				string nodevalue = r[valueName].ToString();				
				string text = r[textName].ToString();
                text = blank + text;
				TypeID.Items.Add(new ListItem(text,nodevalue));
                string blankNode = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;" + blank);
                BindNode(nodevalue, dt, textName, valueName, blankNode);
			}
		}

        /// <summary>
        /// 选取项
        /// </summary>
		[Bindable(true),Browsable(true),Category("Appearance"),DefaultValue("")]
		public string SelectedValue
		{
			get
			{
				return this.TypeID.SelectedValue;
			}

			set
			{
				this.TypeID.SelectedValue = value;
			}
		}

		private string m_parentid="parentid";

        /// <summary>
        /// 父字段名称
        /// </summary>
		[Bindable(true),Category("Appearance"),DefaultValue("parentid")]
		public string ParentID
		{
			get
			{
				return m_parentid;
			}

			set
			{
				m_parentid = value;
			}
		}

        /// <summary>
        /// 当某选项被选中后,获取焦点的控件ID(如提交按钮等)
        /// </summary>
		[Bindable(true),Category("Appearance"),DefaultValue("")] 
		public string SetFocusButtonID
		{
			get
			{
				object o = ViewState[this.ClientID+"_SetFocusButtonID"];
				return (o==null)?"":o.ToString(); 
			}
			set
			{
				ViewState[this.ClientID+"_SetFocusButtonID"] = value;
				if(value!="")
				{
					this.Attributes.Add("onChange","document.getElementById('"+value+"').focus();");
				}
			}
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
		}
	}
}
