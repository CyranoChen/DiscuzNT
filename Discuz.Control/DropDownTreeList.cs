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
    /// 下拉树形框控件。
	/// </summary>
	[DefaultProperty("Text"),ToolboxData("<{0}:DropDownTreeList runat=server></{0}:DropDownTreeList>")]
    public class DropDownTreeList : Discuz.Control.WebControl, IPostBackDataHandler, IPostBackEventHandler
	{
        /// <summary>
        /// 下拉列表框控件变量
        /// </summary>
		public System.Web.UI.WebControls.DropDownList TypeID=new System.Web.UI.WebControls.DropDownList();
	

        /// <summary>
        /// 构造函数
        /// </summary>
		public DropDownTreeList(): base()
		{
            this.BorderStyle = BorderStyle.Groove; 
			this.BorderWidth=1; 
		}

        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
        public void BuildTree(DataTable dt,string textName,string valueName)
        {

            string SelectedType = "0";

            TypeID.SelectedValue = SelectedType;

            this.Controls.Add(TypeID);

            //DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];

            TypeID.Items.Clear();
            //加载树
            TypeID.Items.Add(new ListItem("请选择     ", "0"));
            DataRow[] drs = dt.Select(this.ParentID + "=0");

            foreach (DataRow r in drs)
            {
                TypeID.Items.Add(new ListItem(r[textName].ToString(),r[valueName].ToString()));
                string blank = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                BindNode(r[valueName].ToString(), dt, blank,textName,valueName);
            }
            TypeID.DataBind();

        }

		/// <summary>
		/// 创建树结点
		/// </summary>
		/// <param name="sonparentid">当前数据项</param>
		/// <param name="dt">数据表</param>
		/// <param name="blank">空白符</param>
		private void BindNode(string sonparentid ,DataTable dt,string blank,string textName,string valueName)
		{
			DataRow [] drs = dt.Select(this.ParentID+"=" + sonparentid );
			
			foreach( DataRow r in drs )
			{
				string nodevalue = r[valueName].ToString();				
				string text = r[textName].ToString();					
				text = blank + text;
				TypeID.Items.Add(new ListItem(text,nodevalue));
                string blankNode = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;" + blank);
				BindNode(nodevalue,dt,blankNode,textName,valueName);
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

        /// <summary>
        /// 父字段名称
        /// </summary>
		private string m_parentid="parentid";
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
					this.TypeID.Attributes.Add("onChange","document.getElementById('"+value+"').focus();");
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



		#region IPostBackDataHandler 成员

 
        /// <summary>
        /// 引发PostBackChanged事件
        /// </summary>
		public void RaisePostDataChangedEvent()
		{
		}


        /// <summary>
        /// 引发PostBack事件
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
        }

        /// <summary>
        /// 加载提交数据
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{

			string presentValue = this.TypeID.SelectedValue;
			string postedValue = postCollection[postDataKey];

            //如果回发数据不等于原有数据
			if (!presentValue.Equals(postedValue))
			{
				return true;
			}
			return false;

		}
		#endregion

	}
}
